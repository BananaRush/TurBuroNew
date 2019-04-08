using CefSharp.Wpf;
using Client.Interface;
using Client.Utilits;
using ModelData.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static NavigationService NavService = null;
        public static event Action SetLang = delegate { };
        public static VideoTranslationManager VideoTranslationManager = null;

        public static DataComModel DataCom = new DataComModel()
        {
            Language = "RU"
        };

        public static void LangAction()
        {
            SetLang();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        static public void GoBack()
        {
            if (NavService.CanGoBack)
            {
                NavService.GoBack();
            }
        }
    }
}
