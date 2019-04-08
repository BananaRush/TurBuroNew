using StorageAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using TourAgency.Model.ModelWebApi;
using TourAgency.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для LanguageControl.xaml
    /// </summary>
    public partial class LanguageControl : UserControl
    {
        public LanguageControl()
        {
            InitializeComponent();
        }
          private  Random rnd = new Random();

        private async void RussianLanguage_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rad)
            {
                Session.CurrentSession.IsLoading = true;
                await Task.Delay(300);
                switch (rad.Content)
                {
                    case "Русский":
                        Session.CurrentSession.ChoosedLanguage = CultureInfo.GetCultureInfo("ru");
                        break;
                    case "English":
                        Session.CurrentSession.ChoosedLanguage = CultureInfo.GetCultureInfo("en");
                        var a = Session.CurrentSession.Tests.Where(f=>f.Lang == Lang.En);
                        if(a.Any())
                       Session.CurrentSession.Test= a.ToList()[rnd.Next(0, a.Count() - 1)];
                        break;
                    case "中國":
                        Session.CurrentSession.ChoosedLanguage = CultureInfo.GetCultureInfo("ch");
                        var b = Session.CurrentSession.Tests.Where(f => f.Lang == Lang.En);
                        if(b.Any()) 
                        Session.CurrentSession.Test = b.ToList()[rnd.Next(0, b.Count() - 1)];
                        break;
                }
            }
        }

        private void RussianLanguage_OnTouchDown(object sender, TouchEventArgs e)
        {
            
            ((RadioButton) sender).IsChecked = true;
        }
    }
}
