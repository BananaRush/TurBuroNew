using StorageAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TourAgency.Model.ModelWebApi;
using TourAgency.Pages;
using TourAgency.ViewModel;

namespace TourAgency.Utilities
{
   public class Session: BaseViewModel
   {
       private static Session _session;
       public  event EventHandler SwitchLanguage;
       public static Session CurrentSession => _session ?? (_session = new Session());

       public Session()
       {
           DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal, Application.Current.Dispatcher)
           {
               Interval = TimeSpan.FromSeconds(3)
           };
           timer.Tick += TimerOnTick;
           timer.IsEnabled = true;
           timer.Start();
        }

       [DllImport("User32.Dll")]
       public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

       [DllImport("user32.dll")]
       public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);


        private void TimerOnTick(object sender, EventArgs eventArgs)
       {
           if (UserInactivity.GetSeconds() < Interval) return;
           {
               Session.CurrentSession.User = null;
               {
                   if (!((App.Current.MainWindow as MainWindow).Frame.Content is FirstPage))
                   {
                       (App.Current.MainWindow as MainWindow).Frame.Navigate(new FirstPage());
                       TimerWorked.Invoke(this, null);
                   }
                   VisibilityAccount = Visibility.Collapsed;
                   KeyBoardVisibility = Visibility.Collapsed;
                   //SendMessage(FindWindow(null, "LicenseConfim"), 0x0010, IntPtr.Zero, IntPtr.Zero);
               }
           }
       }

       private bool _isCameraUsed;

       public bool IsCameraUsed
       {
           get => _isCameraUsed;
           set
           {
               _isCameraUsed = value;
                if(!value)
                    CameraTranslation.StartNow();
                else
                    CameraTranslation.StopNow();
               OnPropertyChanged();
           }
       }

       private List<Test> _tests;

       public List<Test> Tests
       {
           get => _tests ?? (_tests = new List<Test>());
           set
           {
               _tests = value;
               OnPropertyChanged();
           }
       }

       private Test _test;

       public Test Test
       {
           get => _test ?? (_test = new Test());
           set
           {
               value.Responses.RemoveAll(f => string.IsNullOrEmpty(f.Text));
               _test = value;
               OnPropertyChanged();
           }

       }


       private string _liveString;

       public string LiveString
       {
           get => _liveString;
           set
           {
               _liveString = value;
               OnPropertyChanged();
           }
       }

       private string _actualNews;

       public string ActualNews
       {
           get => _actualNews;
           set
           {
               _actualNews = value;
               OnPropertyChanged();
           }
       }

       public string NameTerminal => File.ReadAllText("Content/NameTerminal.txt", Encoding.GetEncoding(1251));
       private Visibility _visibilityAccount = Visibility.Collapsed;

       public Visibility VisibilityAccount
       {
           get => _visibilityAccount;
           set
           {
               _visibilityAccount = value;
               OnPropertyChanged();
           }
       }


       private int Interval = 3*60;

       private User _user;
       public User User
       {
           get => _user;    
           set
           {
               _user = value;
               OnPropertyChanged();
           }
       }

       public event EventHandler TimerWorked;

       private List<Tuple<string, string, Tuple<NewsContentType, string>>> _addedButton;
       public List<Tuple<string, string, Tuple<NewsContentType, string>>> AddedButton
       {
           get => _addedButton ?? (_addedButton = new List<Tuple<string, string, Tuple<NewsContentType, string>>>());
           set
           {
               _addedButton = value;
               OnPropertyChanged();
           }
       }

       private Visibility _keyboardVisibility = Visibility.Collapsed;

       public Visibility KeyBoardVisibility
       {
           get => _keyboardVisibility;
           set
           {
               _keyboardVisibility = value;
               OnPropertyChanged();
           }
       }

       public CultureInfo _choosedLanguage = CultureInfo.CurrentCulture;
       public CultureInfo ChoosedLanguage
       {
           get => _choosedLanguage;
           set
           {
               _choosedLanguage = value;
               OnPropertyChanged();
               SwitchLanguage?.Invoke(null, null);
           }
       }

       public event EventHandler SplashHidden;
       public Dictionary<string, Tuple<string,string>> DictionaryLanguage = new Dictionary<string, Tuple<string, string>>();

       private Visibility _visibilitySplash;

       public Visibility VisibilitySplash
       {
           get => _visibilitySplash;
           set
           {
               _visibilitySplash = value;
               OnPropertyChanged();

                if (value==Visibility.Collapsed)
                    SplashHidden.Invoke(null, null);
           }
       }

       private bool _isLoading;
       public bool IsLoading
       {
           get => _isLoading;
           set
           {
               _isLoading = value;
               OnPropertyChanged();
           }
       }
       public const string FtpUser = "fresa-bm_touragency";
       public const string FtpPass= "i8rn/brK";
       public const string FtpServ = "ftp.fresa-bm.nichost.ru";

   }
}
