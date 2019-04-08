using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using TourAgency.Model.ModelWebApi;
using TourAgency.Utilities;

namespace TourAgency
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private App()
        {
            //try
            //{
            //    // Подгружает с WebAPI
            //    if (File.Exists("Content/AddedButton.txt"))
            //    {
            //        foreach (var line in File.ReadAllLines("Content/AddedButton.txt", Encoding.GetEncoding(1251)))
            //        {
            //            var button =
            //                JsonConvert.DeserializeObject<Model.ModelWebApi.NewsModel>(
            //                   line);

            //            Session.CurrentSession.AddedButton.Add(new Tuple<string, string,
            //                Tuple<NewsContentType, string>>(button.Text, button.IconUri,
            //                new Tuple<NewsContentType, string>(button.ContentType, button.Content)));
            //        }
            //    }
            //}
            //catch (Exception e) { }
            AppDomain.CurrentDomain.UnhandledException+= CurrentDomainOnUnhandledException;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            MessageBox.Show(unhandledExceptionEventArgs.ExceptionObject.ToString());
        }
    }
}
