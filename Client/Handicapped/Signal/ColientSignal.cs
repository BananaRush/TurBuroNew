using Microsoft.AspNet.SignalR.Client;
using ModelDate;
using ModelDate.Model.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHIM.Control.Handicapped.Signal
{
    class ColientSignal : BaseHubClient
    {
        public event Action<string, string> Message = delegate { };
        public event Action<List<User>> ListOperator = delegate { };
        public event Action<bool> OnConnect = delegate { };
        public event Action<string> StartVideoPrivate = delegate { };
        public event Action<string> StopVideoPrivate = delegate { };
        public event Action<User> ConnectOperator = delegate { };
        public event Action<string> UserDisconnected = delegate { };
        public event Action<string> CanselVideoPrivate = delegate { };
        public event Action<byte[]> VideoFrame = delegate { };
        public event Action<string> CallVideoTookOperator = delegate { };
        public event Action<byte[]> RecoverAudio = delegate { };
        
        public new void Init()
        {
            HubConnectionUrl = "http://localhost:52279/";
            HubProxyName = "ChatHub";
            HubTraceLevel = TraceLevels.All;
            HubTraceWriter = Console.Out;
            base.Init();
            // Получает сообщение от оператора
            _myHubProxy.On<string, string>("messageClient", (val, val1) => Message(val, val1));
            // Получает список операторов
            _myHubProxy.On<List<User>>("listClient", (val) => ListOperator(val));
            // Если клиент подключился
            _myHubProxy.On<bool>("isConnect", (val) => OnConnect(val));
            // Оператор запрашивает видео приодит Id оператора
            _myHubProxy.On<string>("startVideoPrivate", (val) => StartVideoPrivate(val));
            // Stop 
            _myHubProxy.On<string>("stopVideoPrivate", (val) => StopVideoPrivate(val));
            // Operator connect
            _myHubProxy.On<User>("clientConnect", (val) => ConnectOperator(val));
            // Dissconect operator
            _myHubProxy.On<string>("userDisconnected", (val) => UserDisconnected(val));
            // Отмена видео
            _myHubProxy.On<string>("canselVideoPrivate", (val) => CanselVideoPrivate(val));
            // Получаем видео фрейм
            _myHubProxy.On<byte[]>("videoFrame", (val) => VideoFrame(val));
            // Админ ответил на видео звонок
            _myHubProxy.On<string>("callVideoTookOperator", (val) => CallVideoTookOperator(val));
            // Прием аудио
            _myHubProxy.On<byte[]>("recoverAudio", (val) => RecoverAudio(val));
            
            StartHubInternal();
        }

        public override void StartHub()
        {
            _hubConnection.Dispose();
            Init();
        }

        public void Connect()
        {
            _myHubProxy.Invoke("Connect", new User()
            {
                Type = ClientType.Client,
                Name = Config.GetTerminalName()
            });
        }

        public void SendOnChat(bool flag)
        {
            _myHubProxy.Invoke("ChatStatus", flag);
        }

        public void Send(string msg)
        {
            _myHubProxy.Invoke("ClientMessage", msg);
        }

        public void CallHelp()
        {
            _myHubProxy.Invoke("CallHelp");
        }

        public void CanselHelp()
        {
            _myHubProxy.Invoke("CanselHelp");
        }

        public void Frame(string Id, byte[] buff)
        {
            _myHubProxy.Invoke("FrameVideo", Id, buff);
        }

        public void Audio(string Id, byte[] buff)
        {
            _myHubProxy.Invoke("SendAudio", Id, buff);
        }

        public void GetUserList()
        {
            _myHubProxy.Invoke("GetUserList");
        }

        public void CallVideoChat()
        {
            _myHubProxy.Invoke("CallVideoChat");
        }

        public void CanselVideoChat(string IdOperator)
        {
            _myHubProxy.Invoke("CanselVideoChat", IdOperator);
        }
    }
}
