using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Threading;
using Microsoft.AspNet.SignalR.Client;
using TourAgency.Annotations;
using TourAgency.Commands;
using TourAgency.Utilities;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    public partial class Chat : Page, INotifyPropertyChanged
    {
   
        public Chat()
        {
            InitializeComponent();
            _hubConnection = new HubConnection("http://195.133.1.197:228");
            _hubProxy = _hubConnection.CreateHubProxy("GeneralHub");
            IsEnableds = false;
            StartSignalR();
        }


        private string _adminId;

        public async Task<bool> GetFreeAdmin()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    string address = "http://195.133.1.197/api/video/GetFirstBisy";
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await httpClient.GetAsync(address);
                    HttpContent content = response.Content;
                    string result = await content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result) || result == "null")
                    {
                        return false;
                    }
                    _adminId = result.Replace("\"", "");
                    httpClient.Dispose();
                    return true;
                }
                catch (Exception e)
                {
                    new Controls.MessageBox("Нет связи с сервером :(").ShowDialog();
                    return false;
                }
            });
        }

        private async Task StartSignalR()
        {
            try
            {
                if (!await GetFreeAdmin())
                {
                   new Controls.MessageBox("Нет свободных консультантов, попробуйте позже!").ShowDialog();
                    Dispatcher.Invoke(() =>
                    {
                        if(NavigationService?.CanGoBack == true)
                        NavigationService?.GoBack();
                    });
                    return;
                }
                IsEnableds = true;
                await _hubConnection.Start().ContinueWith(async r =>
                {
                    if (r.IsFaulted) return;
                   
                    _hubProxy.On("SendMessage", (string adminId, string message, DateTime date, bool isAdmin) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (adminId != _adminId) return;
                            Dispatcher.CurrentDispatcher.Invoke(() =>
                                Messages.Add(new Tuple<string, DateTime, bool>(message, date, isAdmin)));
                            ScrollViewer.ScrollToEnd();

                        });
                    });
                    

                    _hubProxy.On("OnDisconnect", (string adminId) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (adminId != _adminId) return;
                            InputText = string.Empty;
                            IsEnableds = false;
                            Messages.Clear();
                        });

                    });

                    await _hubProxy.Invoke("ConnectToAdmin", _adminId);

                });


            }
            catch (Exception e)
            {

            }
        }

        private bool _isEnabled;

        public bool IsEnableds
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public void ChatEnd(string adminId)
        {
            Task.Run(async () =>
            {
                string address = $"http://195.133.1.197/api/video/CallEnd?name={adminId}";
                HttpClient httpClient = new HttpClient();
                await httpClient.GetAsync(address);
                httpClient.Dispose();
                await _hubProxy.Invoke("Disconnect", _adminId);
            });
        }

        private HubConnection _hubConnection;
        private IHubProxy _hubProxy;
        private ObservableCollection<Tuple<String, DateTime, Boolean>> _Messages;
        public ObservableCollection<Tuple<String, DateTime, Boolean>> Messages
        {
            get => _Messages ?? (_Messages = new ObservableCollection<Tuple<string, DateTime, bool>>());
            set
            {
                _Messages = value;
                OnPropertyChanged();
            }
        }

        private String _InputText;
        public String InputText
        {
            get { return _InputText; }
            set
            {
                _InputText = value;
                OnPropertyChanged();
            }
        }
        private Command _sendCommand;

        public Command SendCommand => _sendCommand ?? (_sendCommand = new Command(async delegate
        {
            try
            {

            if (string.IsNullOrEmpty(InputText)) return;
            await _hubProxy.Invoke("SendMessage", new object[] { _adminId, InputText, DateTime.Now, false });
            InputText = string.Empty;
                ScrollViewer.ScrollToEnd();
            }
            catch (Exception e)
            {
              
            }

        }));

        private Command _backCommand;

        public Command BackCommand => _backCommand ?? (_backCommand = new Command(delegate
        {
            IsEnableds = false;
            Messages?.Clear();
            ChatEnd(_adminId);
            Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
            (App.Current.MainWindow as MainWindow).Frame.GoBack();
        }));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackTouch(object sender, TouchEventArgs e)
        {
            BackCommand.Execute(null);
        }

        private void SendTouch(object sender, TouchEventArgs e)
        {
            SendCommand.Execute(null);
        }
    }
}

