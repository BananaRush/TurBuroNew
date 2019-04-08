using System;
using System.Net;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class GeneralHub : Hub
    {
        public void SendMessage(string adminId, string message, DateTime date, bool isAdmin)
        {
            Clients.All.SendMessage(adminId, message, date, isAdmin);
        }

        public void ConnectToAdmin(string adminId)
        {
            Clients.All.OnConnect(adminId);
        }

        public void Disconnect(string adminId)
        {
            string address = $"http://195.133.1.197/api/video/CallEnd?name={adminId}";
            HttpWebRequest httpClient = (HttpWebRequest)WebRequest.Create(address);
            httpClient.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)httpClient.GetResponse();
            Clients.All.OnDisconnect(adminId);
        }
    }

    public class VideoConHub : Hub
    {
        public void ConnectToAdmin(string adminName, string clientName, string roomId)
        {
            Clients.All.OnClientConnect(adminName, clientName, roomId);
        }

        public void Disconnect(string adminName, string clientName, string roomId)
        {
            Clients.All.OnDisconnect(adminName, clientName, roomId);
        }

        public void Answer(string adminName, string clientName, string roomId)
        {
            Clients.All.OnAdminAnswer(adminName, clientName, roomId);
        }
    }

    public class ClientCamHub : Hub
    {
        public void Connect(string clientName, string roomId)
        {
            Clients.All.OnConnect(clientName, roomId);
        }

        public void Disconnect(string clientName)
        {
            Clients.All.OnDisconnect(clientName);
        }
    }

    public class EmotionShareHub : Hub
    {
        public void SendedPhoto(string[] nameTerminals, string photoPath)
        {
            Clients.All.GettedPhoto(nameTerminals, photoPath);
        }
    }
}