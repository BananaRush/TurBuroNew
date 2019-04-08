using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CefSharp;
using CefSharp.Internals;
using CefSharp.Wpf;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace TourAgency.Utilities
{
   static class CameraTranslation
   {
       public static void Start()
       {
           connection = new HubConnection("http://195.133.1.197:228");
           hubProxy = connection.CreateHubProxy("ClientCamHub");
           CefSettings settings = new CefSettings();
           settings.CefCommandLineArgs.Add("enable-media-stream", "1");
           settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream", "1");
           settings.CefCommandLineArgs.Add("enable-speech-input", "1");
           settings.CefCommandLineArgs.Add("enable-usermedia-screen-capture", "1");
           if (!Cef.IsInitialized)
               Cef.Initialize(settings);
           chrom = new ChromiumWebBrowser(){Visibility =  Visibility.Visible};
           Panel.SetZIndex(chrom, -100);
           chrom.Address = "http://127.0.0.1";
            (App.Current.MainWindow as MainWindow).Grid.Children.Add(chrom);
            chrom.FrameLoadEnd += async (a, b) =>
           {
               if(!b.Frame.IsMain) return;
               await chrom.GetMainFrame().EvaluateScriptAsync("function FindElementsForHide(){" +
                                                            "    var a = document.getElementById('sharing-div');" +
                                                            "    if(a){" +
                                                            "        a.style.display = 'none';" +
                                                            "        document.getElementById('icons').style.display = 'none';" +
                                                            "    }else{setTimeout(FindElementsForHide, 100);}" +
                                                            "}" +
                                                            "document.getElementById('confirm-join-button').click();" +
                                                            "FindElementsForHide();");
           };
            connection.Start().ContinueWith(r =>
           {
               if(r.IsFaulted) return;
               hubProxy.On("OnConnect", (string clientName, string roomId) =>
               {
                   if(clientName != Session.CurrentSession.NameTerminal) return;
                   IsNeeded = true;
                   CameraTranslation.roomId = roomId;
                   if (!Session.CurrentSession.IsCameraUsed)
                      App.Current.Dispatcher.Invoke(()=> chrom.Load($"https://appr.tc/r/{roomId}"));
               });

               hubProxy.On("OnDisconnect", (string clientName) =>
               {
                   if(clientName != Session.CurrentSession.NameTerminal) return;
                   IsNeeded = false;
                   roomId = null;
                   App.Current.Dispatcher.Invoke(()=> chrom.Load("http://127.0.0.1"));
                   
               });
           });
       }
     private static ChromiumWebBrowser chrom ;

       public static void StopNow()
       {
           chrom.Load("http://127.0.0.1");
       }

        public static void StartNow()
        {
            if(IsNeeded)
           chrom.Load($"https://appr.tc/r/{roomId}");
            else
            {
                chrom.Load("http://127.0.0.1");
            }

        }

       


        private static HubConnection connection;
       private static IHubProxy hubProxy;
       private static bool IsNeeded = false;
       private static string roomId;
   }
}
