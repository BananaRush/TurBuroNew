using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;
using Microsoft.AspNet.SignalR.Client;
using TourAgency.Annotations;
using TourAgency.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для VideoConnectionControl.xaml
    /// </summary>
    public partial class VideoConnectionControl : UserControl, INotifyPropertyChanged, IDisposable
    {
        public VideoConnectionControl()
        {
            InitializeComponent();
            NeironStats.StatisticStop();
            Session.CurrentSession.IsCameraUsed = true;

            Random r = new Random();
            _myName = "id" + r.Next(100000, 999999) + "" + r.Next(100000, 999999);

            CefSettings settings = new CefSettings();
            Unloaded+= OnUnloaded;
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream", "1");
            settings.CefCommandLineArgs.Add("enable-speech-input", "1");
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capture", "1");
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);
            _chromiumWeb = new ChromiumWebBrowser();

            _chromiumWeb.FrameLoadEnd += async (sender, args) =>
            {
                if (!args.Frame.IsMain) return;
                Dispatcher.Invoke(() => _chromiumWeb.Visibility = Visibility.Hidden);
                await GetRoomId();
            };
            //_chromiumWeb.RegisterAsyncJsObject("DesktopObject", this);
            //Address = "file:///" + System.IO.Path.GetFullPath("/VideoConnection.html");
            //_chromiumWeb.Address = $"file:///{Directory.GetCurrentDirectory()}/HTML/index.html";
            _chromiumWeb.Address = "https://appr.tc/";//$"http://91.146.4.71:9966/demos/demo_audio_video_simple.html";
            _hubConnection = new HubConnection("http://195.133.1.197:228");
            _hubProxy = _hubConnection.CreateHubProxy("VideoConHub");
            AddChromium();
        }

        private void OnUnloaded(object o, RoutedEventArgs routedEventArgs)
        {
            Session.CurrentSession.IsCameraUsed = false;
        }

        private void AddChromium()
        {
            AddChild(_chromiumWeb);
            _chromiumWeb.Visibility = Visibility.Hidden;
        }

        private string _myName;
        private string _adminName;
        private string _roomId;
        private ChromiumWebBrowser _chromiumWeb;
        private HubConnection _hubConnection;
        private IHubProxy _hubProxy;

        private async Task GetRoomId()
        {
            await _chromiumWeb.GetMainFrame().EvaluateScriptAsync("document.getElementById('room-id-input').value").ContinueWith(async d =>
            {
                if (d.IsFaulted) return;
                _roomId = d.Result.Result.ToString();
                await _chromiumWeb.GetMainFrame().EvaluateScriptAsync("function FindElementsForHide(){" +
                                                                      "    var a = document.getElementById('sharing-div');" +
                                                                      "    if(a){" +
                                                                      "        a.style.display = 'none';" +
                                                                      "        document.getElementById('icons').style.display = 'none';" +
                                                                      "    }else{setTimeout(FindElementsForHide, 100);}" +
                                                                      "}" +
                                                                      "document.getElementById('join-button').click();" +
                                                                      "FindElementsForHide();");
                await Task.Delay(1000);
                Dispatcher.Invoke(() => _chromiumWeb.Visibility = Visibility.Visible);
                await _hubConnection.Start().ContinueWith(t =>
                {
                    if (t.IsFaulted) return;

                    _hubProxy.On("OnDisconnect", (string adminName, string clientName, string roomId) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (clientName != _myName || roomId != _roomId) return;
                            if (_chromiumWeb.Address != "https://appr.tc/")
                                _chromiumWeb.Address = "https://appr.tc/";
                            MakeAdminNotBusy();
                        });
                    });

                    _hubProxy.On("OnAdminAnswer", (string adminName, string clientName, string roomId) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (clientName != _myName || roomId != _roomId) return;
                            CallStarted();
                        });
                    });

                });

                VideoConnectionLoadedEvent();
            });
        }

        private void MakeAdminNotBusy()
        {
            if (string.IsNullOrEmpty(_adminName)) return;
            Task.Run(async () =>
            {
                string address = $"http://195.133.1.197/api/video/CallEnd?name={_adminName}";
                HttpClient httpClient = new HttpClient();
                await httpClient.GetAsync(address);
                VideoConnectionOnCallEndedEvent();
                httpClient.Dispose();
                _adminName = String.Empty;
            });
        }

        public void MakeCall()
        {
            Task.Run(async () =>
            {
                string address = "http://195.133.1.197/api/video/GetFirstBisy";
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpClient.GetAsync(address);
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(result) || result == "null")
                {
                    VideoConnectionOnErrorEvent("No operators online");
                    return;
                }
                _adminName = result.Replace("\"", "");
                await _hubProxy.Invoke("ConnectToAdmin", _adminName, _myName, _roomId);
                httpClient.Dispose();
            });
        }

        public void documentloaded()
        {
            _chromiumWeb.GetMainFrame().EvaluateScriptAsync("window.a();");
        }

        public void HangUp()
        {
            try
            {

                _chromiumWeb.GetMainFrame()?.EvaluateScriptAsync("document.getElementById('hangup').click();");
                if (_chromiumWeb.Address != "https://appr.tc/")
                    _chromiumWeb.Address = "https://appr.tc/";
                _hubProxy.Invoke("Disconnect", _adminName, _myName, _roomId);
                MakeAdminNotBusy();

            }
            catch (Exception e)
            {

            }
        }

        public void onerror(string message)
        {
            VideoConnectionOnErrorEvent(message);
        }

        public void CallStarted()
        {
            Task.Run(async () =>
            {
                string address = $"http://195.133.1.197/api/video/MakeConnect?name={_adminName}";
                HttpClient httpClient = new HttpClient();
                await httpClient.GetAsync(address);
                VideoConnectionOnCallStartedEvent();
                httpClient.Dispose();
                //await _chromiumWeb.GetMainFrame().EvaluateScriptAsync("gotStream();");
            });
        }



        public event VideoConnectionOnErrorEventHandler VideoConnectionOnError;

        protected virtual void VideoConnectionOnErrorEvent(object message)
        {
            VideoConnectionOnError?.Invoke(message);
        }

        public event VideoConnectionOnCallStartedEventHandler VideoConnectionOnCallStarted;

        protected virtual void VideoConnectionOnCallStartedEvent()
        {
            VideoConnectionOnCallStarted?.Invoke(this);
        }

        public event VideoConnectionOnCallEndedEventHandler VideoConnectionOnCallEnded;

        protected virtual void VideoConnectionOnCallEndedEvent()
        {
            VideoConnectionOnCallEnded?.Invoke(this);
        }

        public event VideoConnectionLoadedEventHandler VideoConnectionLoaded;

        protected virtual void VideoConnectionLoadedEvent()
        {
            VideoConnectionLoaded?.Invoke(this);
        }

        private string _address;

        public string Address
        {
            get => _address;
            set
            {
                if (Equals(value, _address)) return;
                _address = value;
                _chromiumWeb.Address = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _chromiumWeb?.Dispose();
        }
    }

    public delegate void VideoConnectionOnCallStartedEventHandler(object sender);
    public delegate void VideoConnectionOnCallEndedEventHandler(object sender);
    public delegate void VideoConnectionOnErrorEventHandler(object message);
    public delegate void VideoConnectionLoadedEventHandler(object sender);
}
