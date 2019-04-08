using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using TourAgency.Annotations;
using TourAgency.Commands;
using TourAgency.Controls;
using TourAgency.Model;
using TourAgency.Model.ModelWebApi;
using TourAgency.Pages;
using TourAgency.Utilities;
using Path = System.IO.Path;

namespace TourAgency
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        DispatcherTimer TimerUserClick = null;

        public MainWindow()
        {
            InitializeComponent();
            Map.TimerUodate += Exploeer_TimerUodate;
            Session.CurrentSession.User = null;
            this.Loaded += MainWindow_Loaded;
            if (!Directory.Exists("Content"))
                Directory.CreateDirectory("Content");


            //this.Cursor = Cursors.None;
            //Process process = Process.Start(new ProcessStartInfo
            //{
            //    FileName = "taskkill",
            //    Arguments = "/F /IM explorer.exe",
            //    CreateNoWindow = true,
            //    UseShellExecute = false,
            //    WindowStyle = ProcessWindowStyle.Hidden
            //});

            //process?.WaitForExit();
            Closing += (e, a) =>
            {
                Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
                Application.Current.Shutdown();
            };

            Session.CurrentSession.TimerWorked += async (sender, args) =>
            {
                Splash.Opacity = 1;
                Splash.Visibility = Visibility.Visible;
                Session.CurrentSession.VisibilitySplash = Visibility.Visible;
                ExtendedPopup = false;
                await Task.Delay(500);
                InvaligToggled = false;
                AccessiblePopup.IsOpen = false;
                await Task.Delay(500);
                DisableAccesibility();
            };

            StartServer();
            StartMagnifier();
            EmotionShareComunication.StartWork();
            CameraTranslation.Start();

            #region Таймер обновления кнопок
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(10); 
            timer.Tick += async (a, b) => await TestViewModelForMainVindow();
            timer.Start();
            #endregion

            TimerUserClick = new DispatcherTimer();
            TimerUserClick.Interval = TimeSpan.FromMinutes(1);
            TimerUserClick.Tick += TimerUserClick_Tick;
            TimerUserClick.Start();
        }

        private void Exploeer_TimerUodate()
        {
            ResetUserTimer();
        }

        private void TimerUserClick_Tick(object sender, EventArgs e)
        {
            Session.CurrentSession.IsLoading = true;

            // Отключаем инвалидный модуль
            NativeMethods.MagUninitialize();
            if (IsInvalidExpanded)
            {
                InvalidButtonCommand.Execute(null);
            }

            InvaligToggled = false;
            AccessiblePopup.IsOpen = false;
            ExtendedPopup = false;
            InvaligToggled = false;
            AccessiblePopup.IsOpen = false;

            if (AreButtonsOpen)
            {
                AreButtonsOpen = !AreButtonsOpen;
            }

            Session.CurrentSession.VisibilityAccount = Visibility.Hidden;
            IsInvalidExpanded = false;
            isCheckedMag = false;
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 2);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized", 1);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 100);
            Magnifier.IsChecked = false;
            this.Frame.Content = new FirstPage();
            Session.CurrentSession.IsLoading = false;
        }

        private void ResetUserTimer()
        {
            TimerUserClick.Stop();
            TimerUserClick.Start();
        }

        private RelayCommand _resetTimerMainWindowClick;
        public RelayCommand ResetTimerMainWindowClick
        {
            get
            {
                return _resetTimerMainWindowClick ?? (_resetTimerMainWindowClick = new RelayCommand(() => 
                {
                    ResetUserTimer();
                }));
            }
        }


        private void StartMagnifier()
        {
            Process.Start(@"C:\WINDOWS\system32\Magnify.exe");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 2);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized", 1);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Invert", 0);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 100);

        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //showButtonsStoryboard = this.FindResource("ExtendButtons") as Storyboard;
            //hideButtonsStoryboard = this.FindResource("HideButtons") as Storyboard;
            LowerOpacity = this.FindResource("ReduceOpacity") as Storyboard;
            RiseOpacity = this.FindResource("IncreaceOpacity") as Storyboard;
            this.Frame.Content = new FirstPage();
            await TestViewModelForMainVindow();
        }

        private ObservableCollection<Button> _menuButton = null;

        public ObservableCollection<Button> MenuButton
        {
            get => _menuButton;
            set
            {
                if(_menuButton != value)
                {
                    _menuButton = value;
                    OnPropertyChanged();
                }
            }
        }

        private Random rnd = new Random();

        public async Task TestViewModelForMainVindow()
        {
            try
            {
                Session.CurrentSession.IsLoading = true;

                List<Button> btn = new List<Button>();
                List<NewsModel> list = await WebApi.MenuButton.Get();

                for(int i = 0; i < list.Count; i++)
                {
                    btn.Add(new Button()
                    {
                        Height = 23,
                        Width = 120,
                        Content = list[i].Text,
                        CommandParameter = list[i],
                        Command = NavigateCommand
                    });
                }

                MenuButton = new ObservableCollection<Button>(btn);
                Session.CurrentSession.IsLoading = false;
            }
            catch { }
        }

        private ICommand _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new Command(obj =>
        {
            if(obj is NewsModel elm)
            {
                if(elm.ContentType == NewsContentType.Uri)
                {
                    Frame.Navigate(new Map(elm));
                }

                if(elm.ContentType == NewsContentType.Page)
                {
                    Frame.Navigate(new InfoPage(elm));
                }

                if (elm.ContentType == NewsContentType.Section)
                {
                    Frame.Navigate(new SectionPage(elm));
                }
            }
        }));

        //private ICommand _navigateCommand;
        //public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new Command(a =>
        //{

        //    if (a is Tuple<NewsContentType, string> nav)
        //    {
        //        if (nav.Item2 == "кабинет")
        //        {
        //            if (Session.CurrentSession.User == null)
        //            {
        //                //Frame.Navigate(new AccountWindow());
        //                account.Visibility = Visibility.Visible;
        //                Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;
        //                InvaligToggled = false;
        //                AccessiblePopup.IsOpen = false;

        //            }
        //            else
        //            {
        //                Frame.Navigate(new PersonalAccountsPage());
        //                Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
        //            }
        //            return;
        //        }
        //        switch (nav.Item1)
        //        {
        //            case NewsContentType.Page:
        //                Frame.Navigate(new SingleNewsControl(JsonConvert.DeserializeObject<SingleNewsModel>(nav.Item2)));
        //                break;
        //            case NewsContentType.Pdf:
        //                Frame.Navigate(new PdfViewer(nav.Item2));
        //                break;
        //            case NewsContentType.Uri:
        //                _lastBrowserPage?.ChromiumWebBrowser.Dispose();
        //                //_lastBrowserPage = new Map(nav.Item2, TopButtonCollection.FirstOrDefault(f => f.Item3 == nav)?.Item1);
        //                Frame.Navigate(_lastBrowserPage);
        //                break;
        //        }
        //    }
        //}));

        private ICommand _mapCommand;
        private ICommand _chatCommand;
        private ICommand _videoCommand;
        private ICommand _invalidButtonCommand;
        private ICommand _SPbCommand;
        private ICommand _maskCommand;
        private ICommand _appCommand;
        private ICommand _moreCommand;
        private ICommand _videoGuideCommand;
        private ICommand _inversionCommand;
        private ICommand _emotionShareCommand;

        public ICommand EmotionShareCommand => _emotionShareCommand ?? (_emotionShareCommand =
            new SimpleKeyboard.Command(
                a =>
                {
                    Frame.Navigate(new FotoPage(true));
                }));
        
        private bool IsInvalidExpanded { get; set; }
        private bool IsOnInversion { get; set; }

        public ICommand InversionCommand => _inversionCommand ?? (_inversionCommand = new Command(a =>
        {
            //OffAllAccesible();
            MonohromeButton.IsChecked = false;
            IsOnInversion = !IsOnInversion;

            var magEffectInvert = new NativeMethods.MAGCOLOREFFECT
            {
                transform = new[] {
                    -1.0f,  0.0f,  0.0f, 0.0f, 0.0f,
                    0.0f, -1.0f,  0.0f, 0.0f, 0.0f,
                    0.0f,  0.0f, -1.0f, 0.0f, 0.0f,
                    0.0f,  0.0f,  0.0f, 1.0f, 0.0f,
                    1.0f,  1.0f,  1.0f, 0.0f, 1.0f
                }
            };

            if (IsOnInversion)
            {
              
                NativeMethods.MagInitialize();
                NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);
            }
            else
            {
                magEffectInvert =
                new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[] {
                        1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f, 0.0f,
                        0.0f, 0.0f, 0.0f, 0.0f, 1.0f
                    }
                };

                NativeMethods.MagInitialize();
                NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);
            }
        }));

        public ICommand AppCommand => _appCommand ?? (_appCommand = new SimpleKeyboard.Command(a =>
        {
            Frame.Navigate(new Map("http://www.visit-petersburg.ru/ru/application/", "приложения"));

        }));

        public ICommand MaskCommand => _maskCommand ?? (_maskCommand = new Command(a =>
        {
            Frame.Navigate(new FotoPage());
        }));

        public ICommand InvalidButtonCommand => _invalidButtonCommand ?? (_invalidButtonCommand = new Command(a =>
        {
            ExtendedPopup = false;
            InvaligToggled = false;
            AccessiblePopup.IsOpen = false;

            //hideButtonsStoryboard.Begin();
            if (AreButtonsOpen)
                AreButtonsOpen = !AreButtonsOpen;
            IsInvalidExpanded = !IsInvalidExpanded;
            DoubleAnimation horzAnim = new DoubleAnimation
            {
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
            };

            horzAnim.From = IsInvalidExpanded ? 0 : ScrollViewerChild.Height / 2;
            horzAnim.To = IsInvalidExpanded ? ScrollViewerChild.Height / 2 : 0;
            Storyboard.SetTargetProperty(horzAnim, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(horzAnim, InvalidBlock);
            Storyboard s = new Storyboard();
            s.Children.Add(horzAnim);
            s.Begin();

            s.Completed += (sender, args) =>
            {
                InvalidBlockText.Visibility = IsInvalidExpanded ? Visibility.Visible : Visibility.Collapsed;
            };

        }));

        public ICommand MapCommand => _mapCommand ?? (_mapCommand = new Command(a =>
        {
            Frame.Navigate(new Map($"file:///{Path.GetFullPath("HTML/YandexMapHTML.html")}?lang=ru_RU", "карта"));
        }));

        public ICommand VideoCommand => _videoCommand ?? (_videoCommand = new Command(a =>
        {
            if (!(Frame.Content is VideoConnectionPage))
            {
                var video = new VideoConnectionPage();
                Frame.Navigate(video);
            }

        }));

        public ICommand ChatCommand => _chatCommand ?? (_chatCommand = new Command(a =>
        {
            if (!(Frame.Content is Chat))
                Frame.Navigate(new Chat());
            Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;
        }));

        public ICommand SPbCommand => _SPbCommand ?? (_SPbCommand = new Command(a =>
        {
            if (!(Frame.Content is FirstPage))
                Frame.Navigate(new FirstPage());
        }));


        public ICommand MoreCommand => _moreCommand ?? (_moreCommand = new SimpleKeyboard.Command(a =>
        {
            ToggleFlag();
            //if (AreButtonsOpen)
            //{
            //    showButtonsStoryboard.Begin();
            //}
            //else
            //{
            //    hideButtonsStoryboard.Begin();
            //}
        }));

        private Map _lastBrowserPage;
        private Boolean AreButtonsOpen;
        private void ToggleFlag()
        {
            ExtendedPopup = !ExtendedPopup;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private RelayCommand _button_Click;

        public RelayCommand Button_Click
        {
            get
            {
                return _button_Click ?? (_button_Click = new RelayCommand(() => {
                    ToggleFlag();
                }));
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    ToggleFlag();
        //    //if (AreButtonsOpen)
        //    //{
        //    //    showButtonsStoryboard.Begin();
        //    //}
        //    //else
        //    //{
        //    //    hideButtonsStoryboard.Begin();
        //    //}
        //}

        private void Frame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
            ExtendedPopup = false;
           // hideButtonsStoryboard.Begin();
        }

        private Process proc;

        private void StartServer()
        {
            if (!File.Exists("serverstart.bat"))
            {
                StreamWriter sw = new StreamWriter("serverstart.bat");
                sw.Write("chcp 886" +
                         Environment.NewLine +
                         "cd localserver" +
                         Environment.NewLine +
                         "node app.js");
                sw.Close();
            }
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("serverstart.bat");
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            proc = new System.Diagnostics.Process();
            //procStartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
            proc.StartInfo = procStartInfo;
            proc.Start();

            if (File.Exists("consoleserverdata.txt"))
                File.Delete("consoleserverdata.txt");
            using (StreamReader reader = proc.StandardOutput)
            {
                FileStream file;
                byte[] buffer = new byte[8 * 1024];
                int len;
                bool isstarted = false;
                while (!isstarted)
                {
                    try
                    {
                        file = File.Create("consoleserverdata.txt");
                        len = reader.BaseStream.Read(buffer, 0, buffer.Length);
                        file.Write(buffer, 0, len);
                        file.Close();
                        isstarted = String.Concat(File.ReadAllLines("consoleserverdata.txt"))
                            .Contains("server is started");

                        File.Delete("consoleserverdata.txt");
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        Storyboard LowerOpacity;
        Storyboard RiseOpacity;

        public ICommand VideoGuideCommand => _videoGuideCommand ?? (_videoGuideCommand = new SimpleKeyboard.Command(a =>
        {
            string st = "Video/";
            string namePage = "";
            string nameVideo = "";
            if (Frame.Content is Map)
            {
                Map map = Frame.Content as Map;
                namePage = map.TitleTop;
            }
            else
            {
                namePage = Frame.Content.ToString();
            }
            nameVideo = getVideoAdress(namePage);
            if (string.IsNullOrWhiteSpace(nameVideo))
                return;
            string Currentlanguage = Getlanguage(Session.CurrentSession.ChoosedLanguage.IetfLanguageTag);
            if (Currentlanguage == null)
                return;
            st = st + Currentlanguage + "/" + nameVideo;
            media.Source = new Uri(st, UriKind.Relative);
            RiseOpacity.Begin();
            media.Play();
            popupMedia.IsOpen = true;
        }));


        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
             LowerOpacity.Begin();
            popupMedia.IsOpen = false;

            media.Source = null;
            media.Close();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            ResetUserTimer();
            if (Frame.Content.ToString() != "TourAgency.Pages.FirstPage")
            {
                popupMedia.IsOpen = false;
                media.Stop();
                LowerOpacity.Begin();
                media.Source = null;
                media.Close();
            }

            if (Frame.Content is AccountWindow)
                Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;

        }

        public string getVideoAdress(string stPage)
        {
            switch (stPage)
            {
                case "TourAgency.Pages.FirstPage":
                    return "01.m4v";
                case "Достопримечательности":
                    return "02.m4v";
                case "Информация":
                    return "03.m4v";
                case "События":
                    return "04.m4v";
                case "Рестораны":
                    return "05.m4v";
                case "Шопинг":
                    return "06.m4v";
                case "Гостиницы":
                    return "07.m4v";
                //case "Информация":
                //    return "08.m4v";
                case "Транспорт":
                    return "09.m4v";
                case "Помощь туристу":
                    return "10.m4v";
                case "Карта":
                    return "11.m4v";
         
                default:
                    return null;
            }
        }

        private void DisableAccesibility()
        {
            OffAllAccesible();
            Process.GetProcesses().FirstOrDefault(f => f.ProcessName.Contains("Magnify"))?.Kill();
            MonohromeButton.IsChecked = false;
            IsOnInversion = false;
            var magEffectInvert =
                new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[] {
                        1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f, 0.0f,
                        0.0f, 0.0f, 0.0f, 0.0f, 1.0f
                    }
                };
            NativeMethods.MagInitialize();
            NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);
            InvertionButton.IsChecked = false;
            MonohromeButton.IsChecked = false;
            float redScale = 0.2126f, greenScale = 0.7152f, blueScale = 0.0722f;
            magEffectInvert = new NativeMethods.MAGCOLOREFFECT
            {
                transform = new[] {
                    redScale,   redScale,   redScale,   0.0f,  0.0f,
                    greenScale, greenScale, greenScale, 0.0f,  0.0f,
                    blueScale,  blueScale,  blueScale,  0.0f,  0.0f,
                    0.0f,       0.0f,       0.0f,       1.0f,  0.0f,
                    0.0f,       0.0f,       0.0f,       0.0f,  1.0f
                }
            };

            NativeMethods.MagInitialize();
            NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);


            ExtendedPopup = false;
            InvaligToggled = false;
            AccessiblePopup.IsOpen = false;

            //hideButtonsStoryboard.Begin();
            if (AreButtonsOpen)
                AreButtonsOpen = !AreButtonsOpen;
            IsInvalidExpanded = false;
            DoubleAnimation horzAnim = new DoubleAnimation
            {
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
            };
            horzAnim.From = IsInvalidExpanded ? 0 : ScrollViewerChild.Height / 2;
            horzAnim.To = IsInvalidExpanded ? ScrollViewerChild.Height / 2 : 0;
            Storyboard.SetTargetProperty(horzAnim, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(horzAnim, InvalidBlock);
            Storyboard s = new Storyboard();
            s.Children.Add(horzAnim);
            s.Begin();
            s.Completed += (sender, args) =>
            {
                InvalidBlockText.Visibility = IsInvalidExpanded ? Visibility.Visible : Visibility.Collapsed;
            };



            isCheckedMag = false;
            if (isCheckedMag)
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 3);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized",
                    1);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 200);
            }
            else
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 2);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized",
                    1);
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 100);
            }
            AccessiblePopup.IsOpen = false;
            Magnifier.IsChecked = false;

        }

        private string Getlanguage(string currentL)
        {
            switch (currentL)
            {
                case "en":
                    return "English";
                case "ch":
                    return "Chinese";
                case "ru":
                    return "Russian";

            }
            return null;
        }

        private void Splash_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Splash.Visibility = Visibility.Collapsed;
            Session.CurrentSession.VisibilitySplash = Visibility.Collapsed;
            NeironStats.Start();
        }

        private void TextOnTouch(object sender, TouchEventArgs e)
        {
            if(!(Frame.Content is FirstPage))
            Frame.Navigate(new FirstPage());
        }

        private RelayCommand _searchTouch;
        public RelayCommand SearchTouch
        {
            get
            {
                return _searchTouch ?? (_searchTouch = new RelayCommand(() => 
                {
                    Frame.Navigate(new Map("http://www.visit-petersburg.ru/search/?searchid=2225809&text=&web=0", "ПОИСК"));
                }));
            }
        }


        //private void SearchTouch(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(new Map("http://www.visit-petersburg.ru/search/?searchid=2225809&text=&web=0", "ПОИСК"));
        //}

        private void EmotionShareTouch(object sender, RoutedEventArgs e)
        {
            EmotionShareCommand.Execute(null);
        }

        private RelayCommand _inverSionTouch;
        public RelayCommand InverSionTouch
        {
            get
            {
                return _inverSionTouch ?? (_inverSionTouch = new RelayCommand(() => {
                    InversionCommand.Execute(null);
                }));
            }
        }


        //private void InverSionTouch(object sender, RoutedEventArgs e)
        //{
        //   InversionCommand.Execute(null);
        //}

        private void OffAllAccesible()
        {
            NativeMethods.MagUninitialize();
            isCheckedMag = false;
            AccessiblePopup.IsOpen = false;
        }

        private RelayCommand _monohromTouch;
        public RelayCommand MonohromTouch
        {
            get
            {
                return _monohromTouch ?? (_monohromTouch = new RelayCommand(obj => 
                {
                    if (obj != null && obj is ToggleButton sender)
                    {
                        //OffAllAccesible();
                        IsOnInversion = false;
                        float redScale = 0.2126f, greenScale = 0.7152f, blueScale = 0.0722f;
                        var magEffectInvert = new NativeMethods.MAGCOLOREFFECT
                        {
                            transform = new[] 
                            {
                                redScale,   redScale,   redScale,   0.0f,  0.0f,
                                greenScale, greenScale, greenScale, 0.0f,  0.0f,
                                blueScale,  blueScale,  blueScale,  0.0f,  0.0f,
                                0.0f,       0.0f,       0.0f,       1.0f,  0.0f,
                                0.0f,       0.0f,       0.0f,       0.0f,  1.0f
                            }
                        };

                        if (sender.IsChecked == true)
                        {
                            NativeMethods.MagInitialize();
                            NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);
                        }
                        else
                        {
                            magEffectInvert = new NativeMethods.MAGCOLOREFFECT
                            {
                                transform = new[] 
                                {
                                    1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
                                    0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
                                    0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
                                    0.0f, 0.0f, 0.0f, 1.0f, 0.0f,
                                    0.0f, 0.0f, 0.0f, 0.0f, 1.0f
                                }
                            };
                            NativeMethods.MagInitialize();
                            NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);
                        }
                    }
                }));
            }
        }


        //private void MonohromTouch(object sender, RoutedEventArgs e)
        //{
        //    //OffAllAccesible();
        //    IsOnInversion = false;
        //    float redScale = 0.2126f, greenScale = 0.7152f, blueScale = 0.0722f;
        //    var magEffectInvert = new NativeMethods.MAGCOLOREFFECT
        //    {
        //        transform = new[] {
        //            redScale,   redScale,   redScale,   0.0f,  0.0f,
        //            greenScale, greenScale, greenScale, 0.0f,  0.0f,
        //            blueScale,  blueScale,  blueScale,  0.0f,  0.0f,
        //            0.0f,       0.0f,       0.0f,       1.0f,  0.0f,
        //            0.0f,       0.0f,       0.0f,       0.0f,  1.0f
        //        }
        //    };

        //    if (((ToggleButton) sender).IsChecked == true)
        //    {
        //        NativeMethods.MagInitialize();
        //        NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);
        //    }
        //    else
        //    {
        //        magEffectInvert =
        //            new NativeMethods.MAGCOLOREFFECT
        //            {
        //                transform = new[] {
        //                    1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        //                    0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
        //                    0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
        //                    0.0f, 0.0f, 0.0f, 1.0f, 0.0f,
        //                    0.0f, 0.0f, 0.0f, 0.0f, 1.0f
        //                }
        //            };
        //        NativeMethods.MagInitialize();
        //        NativeMethods.MagSetFullscreenColorEffect(ref magEffectInvert);
        //    }

        //}

        private RelayCommand _downScreenTouch;
        public RelayCommand DownScreenTouch
        {
            get
            {
                return _downScreenTouch ?? (_downScreenTouch = new RelayCommand(() => 
                {
                    InvalidButtonCommand.Execute(null);
                }));
            }
        }


        //private void DownScreenTouch(object sender, RoutedEventArgs e)
        //{
        //   InvalidButtonCommand.Execute(null);
        //}

        public static readonly DependencyProperty InvaligToggledProperty = DependencyProperty.Register(
            "InvaligToggled", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public bool InvaligToggled
        {
            get => (bool) GetValue(InvaligToggledProperty);
            set => SetValue(InvaligToggledProperty, value);
        }

        public static readonly DependencyProperty ExtendedPopupProperty = DependencyProperty.Register(
            "ExtendedPopup", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public bool ExtendedPopup
        {
            get => (bool) GetValue(ExtendedPopupProperty);
            set => SetValue(ExtendedPopupProperty, value);
        }


        private RelayCommand _invalidButtonTouch;
        public RelayCommand InvalidButtonTouch
        {
            get
            {
                return _invalidButtonTouch ?? (_invalidButtonTouch = new RelayCommand(() => {
                    InvaligToggled = !InvaligToggled;
                }));
            }
        }

        private bool isCheckedMag = false;

        private RelayCommand _mAgnifierTouch;
        public RelayCommand MAgnifierTouch
        {
            get
            {
                return _mAgnifierTouch ?? (_mAgnifierTouch = new RelayCommand(async () => {
                    isCheckedMag = !isCheckedMag;
                    if (isCheckedMag)
                    {
                        Process.Start(@"C:\WINDOWS\system32\Magnify.exe");
                        await Task.Delay(500);
                        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 3);
                        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized",
                            1);
                        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 200);
                    }
                    else
                    {
                        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 2);
                        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized",
                            1);
                        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 100);
                        Process.GetProcesses().FirstOrDefault(f => f.ProcessName.Contains("Magnify"))?.Kill();

                    }
                    AccessiblePopup.IsOpen = false;
                }));
            }
        }

        //private async void MAgnifierTouch(object sender, RoutedEventArgs e)
        //{
        //    isCheckedMag = !isCheckedMag;
        //    if (isCheckedMag)
        //    {
        //        Process.Start(@"C:\WINDOWS\system32\Magnify.exe");
        //        await Task.Delay(500);
        //        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 3);
        //        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized",
        //            1);
        //        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 200);
        //    }
        //    else
        //    {
        //        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnificationMode", 2);
        //        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "MagnifierUIWindowMinimized",
        //            1);
        //        Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "Magnification", 100);
        //        Process.GetProcesses().FirstOrDefault(f => f.ProcessName.Contains("Magnify"))?.Kill();

        //    }
        //    AccessiblePopup.IsOpen = false;
        //}

        private void CamersTouch(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new VideoConnectionPage(true));
        }

        private async void AccessiblePopup_OnOpened(object sender, EventArgs e)
        {
            ExtendedPopup = false;
            await Task.Delay(500);
        }

        private async void ExtendButtonsGrid_OnOpened(object sender, EventArgs e)
        {
            AccessiblePopup.IsOpen = false;
            await Task.Delay(500);
        }
    }

    static class NativeMethods
    {
        const string Magnification = "Magnification.dll";

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagInitialize();

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagUninitialize();

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagSetFullscreenColorEffect(ref MAGCOLOREFFECT pEffect);

        public struct MAGCOLOREFFECT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public float[] transform;
        }
    }
}
