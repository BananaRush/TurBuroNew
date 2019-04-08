using Microsoft.AspNet.SignalR.Client;
using ModelData.Models.Database;
using ModelData.Utilits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilits.SignalR
{
    public class SignalRVideoManager : BaseHubClient
    {
        /// <summary>
        /// Событие возвращает значение True если удалось подключиться
        /// </summary>
        public event Action<bool> OnConnect = delegate { };

        /// <summary>
        /// Возвращяет список для доступных для просмотра терминалов
        /// </summary>
        public event Action<List<TerminalsModel>> TerminalList = delegate { };

        public new void Init()
        {
            string Name = string.Empty;
            string Url = Config.GetHost();

            HubConnectionUrl = Url;
            HubProxyName = "VideoConHub";
            HubTraceLevel = TraceLevels.All;
            HubTraceWriter = Console.Out;
            base.Init();
            RegistrHub();
            // Регистрируем обработчики
            StartHubInternal();
            SnConnect(Config.GetIndificator());
        }

        // Запустить
        public override void StartHub()
        {
            Init();
        }

        private void RegistrHub()
        {
            _myHubProxy.On<bool>("isConnect", (val) => OnConnect(val));
        }

        // Сигналы
        public void SnConnect(string terminalId)
        {
            try
            {
                _myHubProxy.Invoke("Connect", terminalId);
            }
            catch
            {

            }
        }

        public void SnGetTerminalList()
        {
            try
            {
                _myHubProxy.Invoke("GetTerminalList");
            }
            catch
            {

            }
        }
    }
}
