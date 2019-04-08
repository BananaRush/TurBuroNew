using Microsoft.AspNet.SignalR;
using ModelData.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Controllers.SignalR
{
    public class VideoConHub : Hub
    {
        private static Dictionary<string, TerminalsModel> Terminal = new Dictionary<string, TerminalsModel>();

        public void Connect(string terminalId)
        {
            var id = Context.ConnectionId;

            Clients.Caller.isConnect(true);
        }
    }
}