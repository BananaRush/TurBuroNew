using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using TourAgency.Pages;
using TourAgency.Properties;

namespace TourAgency.Utilities
{
   public static class EmotionShareComunication
    {
        public static async void StartWork()
        {
            try
            {

            hubConnection = new HubConnection("http://195.133.1.197:228");
            hubProxy = hubConnection.CreateHubProxy("EmotionShareHub");
                if(hubProxy == null) return;
             await hubConnection.Start().ContinueWith(r =>
                {
                    if(r.IsFaulted) return;
                    hubProxy.On("GettedPhoto", (string[] nameTerminals, string photoPath) =>
                    {
                        try
                        {

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            if (!nameTerminals.Contains(Session.CurrentSession.NameTerminal)) return;
                            var res = new LicenseConfim(true, photoPath).ShowDialog();
                            if (res == true)
                                (App.Current.MainWindow as MainWindow).Frame.Navigate(new FotoPage(true));
                        });

                        }
                        catch 
                        {
                          //
                        }
                    });
                });
            

            }
            catch
            {
             // 
            }
        }

        public static async Task<bool> Send(string[] nameTerminals,string photo)
        {
            try
            {

            using (WebClient wc = new WebClient(){Credentials = new NetworkCredential(Session.FtpUser, Session.FtpPass)})
            {
                wc.UploadFile($"ftp://{Session.FtpServ}/EmotionShare/{photo}", Path.GetFullPath($"Content/EmotionShare/{photo}"));
            }
            
            if (hubProxy == null) return false;
          return await hubProxy.Invoke("SendedPhoto", nameTerminals, photo).ContinueWith(r =>
            {
                if(r.IsFaulted) return false;
                return true;
            });

            }
            catch
            {
                return false;
            }
        }
        private static HubConnection hubConnection;
        private static IHubProxy hubProxy;
    }
}
