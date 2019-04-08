using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.AspNet.SignalR.Client;
using TourAgency.Commands;
using TourAgency.Controls;

namespace TourAgency.ViewModel
{
    class TestViewModelForChat : BaseViewModel
    {
        public TestViewModelForChat()
        {
            _hubConnection = new HubConnection("http://91.146.4.71:230");
            _hubConnection.StateChanged+= HubConnectionOnStateChanged;
            _hubProxy = _hubConnection.CreateHubProxy("GeneralHub");
            StartSignalR();
        }

        private void HubConnectionOnStateChanged(StateChange stateChange)
        {
           if( stateChange.NewState!= ConnectionState.Connected)return; 
            _hubProxy.On("SendMessage", (string adminId, string message, DateTime date, bool isAdmin) =>
            {
                if (adminId != _adminId) return;
               Dispatcher.CurrentDispatcher.Invoke(() => Messages.Add(new Tuple<string, DateTime, bool>(message, date, isAdmin)));
            });

            _hubProxy.On("OnDisconnect", (string adminId) =>
            {
                if (adminId != _adminId) return;
                InputText = string.Empty;
                IsEnabled = false;
                ChatEnd(adminId);
                Messages.Clear();

            });
        
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
                    return false;
                }
            });
        }

        private async Task StartSignalR()
        {
            try
            {

            if (!await GetFreeAdmin())
                return;

            IsEnabled = true;

                await _hubConnection.Start().ContinueWith(async r =>
                {
                    if (r.IsFaulted) return;
                    await _hubProxy.Invoke("ConnectToAdmin", _adminId);

                });


            }
            catch (Exception e)
            {
                
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (Equals(value, _isEnabled)) return;
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
            get => _Messages?? (_Messages = new ObservableCollection<Tuple<string, DateTime, bool>>());
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
            if (string.IsNullOrEmpty(InputText)) return;
              await _hubProxy.Invoke("SendMessage", new object[] { _adminId, InputText, DateTime.Now, false });
              InputText = string.Empty;
              
          }));

        private Command _backCommand;

        public Command BackCommand => _backCommand ?? (_backCommand = new Command(delegate
        {
            IsEnabled = false;
            Messages?.Clear();
            ChatEnd(_adminId);
            (App.Current.MainWindow as MainWindow).Frame.GoBack();
        }));

    }
}
