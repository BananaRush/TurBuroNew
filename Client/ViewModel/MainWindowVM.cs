using Client.Controls;
using Client.Controls.FeatureModule;
using Client.Interface;
using Client.Model;
using Client.Utilits;
using Client.Utilits.Language;
using Client.View;
using ModelData;
using ModelData.Models.Database;
using ModelData.Utilits;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Client.ViewModel
{
    class MainWindowVM : Bandel, IMainFrame
    {
        private RelayCommand _navigateCommand;
        private RelayCommand _offssetTopCMD;
        private RelayCommand _goHome;
        private ObservableCollection<FrameworkElement> _uIElementsList { get; set; }
        private List<UIElm> _uIElms = new List<UIElm>();
        private int _offssetTop = 0;
        private Visibility _visibilityMagnifier = Visibility.Hidden;

        // Таймер загрузки данных
        private DispatcherTimer _updateTimer = new DispatcherTimer();

        public MainWindowVM()
        {
            App.NavService = IsMainFrame.NavigationService;
            IsMainFrame.Navigated += IsMainFrame_Navigated;
            IsMainFrame.Navigate(new _NonPage());
            UIElementsList = new ObservableCollection<FrameworkElement>();
            _updateTimer.Tick += new EventHandler(UpdateTimerTick);
            _updateTimer.Interval = new TimeSpan(0, 0, TimoutUp.UP_MAIN_MINUTE, 0);
            _updateTimer.Start();
            UpdateTimerTick(_updateTimer, null);
            FeatureModuleManager.IsMagnifier += FeatureModuleManager_IsMagnifier;
            FeatureModuleManager.OffssetTop += FeatureModuleManager_OffssetTop;

            // Запускам лоадер файлов
            FileLoader fileLoader = new FileLoader();
            fileLoader.Start();

            App.SetLang += App_SetLang;
            App_SetLang();

            App.VideoTranslationManager = new VideoTranslationManager();
            App.VideoTranslationManager.Init();
        }

        private void App_SetLang()
        {
            ClientLang.SetLanguage(App.DataCom.Language);
            GoHome.Execute(null);
            UpdateTimerTick(_updateTimer, null);
        }

        public async void GetData()
        {
            // Получаем список кнопок
            List<NewsModel> buttonNavList = await WebApi.MenuButton.Get(App.DataCom);
            // Получаем список элементов
            List<UIElementModel> list = await WebApi.UIElement.Get(App.DataCom);

            if (list == null)
                return;

            ViewManager.Update(list, UIElementsList, buttonNavList);

            foreach (var item in list)
            {
                // Навигационные кнопки
                if(item.ElementType == UIElementType.ButtonNav)
                {
                    UIElementsList.Add(CreateButtons.Create(item, 
                        buttonNavList.Where(r => r.Id == item.ButtonNavId).FirstOrDefault()?.Text, 
                        buttonNavList.Where(r => r.Id == item.ButtonNavId).FirstOrDefault(), 
                        NavigateCommand));
                }

                // Навигационный фрейм
                if(item.ElementType == UIElementType.NavigationFrame)
                {
                    IsMainFrame.VerticalAlignment = VerticalAlignment.Top;
                    IsMainFrame.HorizontalAlignment = HorizontalAlignment.Left;
                    IsMainFrame.VerticalContentAlignment = VerticalAlignment.Stretch;
                    IsMainFrame.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    IsMainFrame.Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0);
                    IsMainFrame.Height = Convert.ToDouble(item.Height.Replace(".", ","));
                    IsMainFrame.Width = Convert.ToDouble(item.Width.Replace(".", ","));
                    IsMainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

                    if(!UIElementsList.Any(r=>r is Frame))
                    {
                        UIElementsList.Add(IsMainFrame);
                    }
                }

                // Слайдер
                if(item.ElementType == UIElementType.Slider)
                {
                    UIElementsList.Add(new CarouselControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0),
                    });
                }

                // Язык
                if(item.ElementType == UIElementType.Language)
                {
                    UIElementsList.Add(new LanguageControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0)
                    });
                }

                // Погода
                if(item.ElementType == UIElementType.Weather)
                {
                    UIElementsList.Add(new WeatherControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0),
                    });

                    Panel.SetZIndex(UIElementsList.Last(), 99);
                }

                // Время
                if(item.ElementType == UIElementType.Time)
                {
                    UIElementsList.Add(new TimeControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0)
                    });

                    Panel.SetZIndex(UIElementsList.Last(), 100);
                }

                // Валюты
                if(item.ElementType == UIElementType.Currency)
                {
                    UIElementsList.Add(new CurrencyControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0)
                    });
                }

                // Новости
                if(item.ElementType == UIElementType.News)
                {
                    UIElementsList.Add(new EventsControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0)
                    });
                }

                // Опросы
                if(item.ElementType == UIElementType.Survey)
                {
                    UIElementsList.Add(new TestingControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0)
                    });
                }

                // Логотип
                if(item.ElementType == UIElementType.Logotype)
                {
                    try
                    {
                        UIElementsList.Add(new Image()
                        {
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0),
                            Height = Convert.ToDouble(item.Height.Replace(".", ",")),
                            Width = Convert.ToDouble(item.Width.Replace(".", ",")),
                            Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(SaveUrlImage.Save(Path.Combine(Config.GetHost(), item.ImageName)))),
                        });

                        Panel.SetZIndex(UIElementsList.Last(), 101);
                    }
                    catch { }

                }

                // Кнопка на главную
                if(item.ElementType == UIElementType.BackButton)
                {
                    Button button = new Button();
                    button.Style = Application.Current.Resources["ButtonNan"] as Style;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0);
                    button.Height = Convert.ToDouble(item.Height.Replace(".", ","));
                    button.Width = Convert.ToDouble(item.Width.Replace(".", ","));
                    button.Command = GoHome;

                    if(button.Content != null)
                    {
                        button.Content = new Image()
                        {
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(SaveUrlImage.Save(Path.Combine(Config.GetHost(), item.ImageName)))),
                            Stretch = System.Windows.Media.Stretch.Fill
                        };
                    }

                    UIElementsList.Add(button);
                }

                // Млдуль спец. возможностей
                if(item.ElementType == UIElementType.FeatureModule)
                {
                    UIElementsList.Add(new FeatureControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0),
                        Height = Convert.ToDouble(item.Height.Replace(".", ",")),
                        Width = Convert.ToDouble(item.Width.Replace(".", ",")),
                    });

                    Panel.SetZIndex(UIElementsList.Last(), 105);
                }

                if(item.ElementType == UIElementType.VideoGuide)
                {
                    UIElementsList.Add(new VideoGuideControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0),
                        Height = Convert.ToDouble(item.Height.Replace(".", ",")),
                        Width = Convert.ToDouble(item.Width.Replace(".", ",")),
                    });

                    Panel.SetZIndex(UIElementsList.Last(), 106);
                }

                if(item.ElementType == UIElementType.Still)
                {
                    UIElementsList.Add(new StillControl()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(Convert.ToDouble(item.Left.Replace(".", ",")), Convert.ToDouble(item.Top.Replace(".", ",")), 0, 0),
                        Height = Convert.ToDouble(item.Height.Replace(".", ",")),
                        Width = Convert.ToDouble(item.Width.Replace(".", ",")),
                    });

                    Panel.SetZIndex(UIElementsList.Last(), 107);
                }

                // Отвечает за навигацию элементов
                // Не трогать Блядь!!!
                UIElementsList.LastOrDefault().Tag = new TagElement(item.Id, item.GetHashCode(), item.IsNavVisibility, item);
            }
        }

        public RelayCommand NavigateCommand
        {
            get
            {
                return _navigateCommand ?? (_navigateCommand = new RelayCommand(obj =>
                {
                    if(obj != null && obj is NewsModel elm)
                    {
                        Explorer.ButtonNav = elm;

                        if (elm.ContentType == StorageAPI.Models.NewsContentType.Uri)
                        {
                            App.NavService.Navigate(new WebBrowserPage());
                        }

                        if(elm.ContentType == StorageAPI.Models.NewsContentType.Section)
                        {
                            App.NavService.Navigate(new SectionPage());
                        }

                        if(elm.ContentType == StorageAPI.Models.NewsContentType.Image)
                        {
                            App.NavService.Navigate(new PassagePage());
                        }

                        if(elm.ContentType == StorageAPI.Models.NewsContentType.Page)
                        {
                            App.NavService.Navigate(new InformationPage());
                        }

                        if (elm.ContentType == StorageAPI.Models.NewsContentType.NewLents)
                        {
                            App.NavService.Navigate(new InformationListPage());
                        }
                    }
                }));
            }
        }

        public RelayCommand GoHome
        {
            get
            {
                return _goHome ?? (_goHome = new RelayCommand(() => 
                {
                    App.NavService.Navigate(new PhotoPage());
                    
                    if (!(IsMainFrame.Content is _NonPage))
                        App.NavService.Navigate(new _NonPage());
                }));
            }
        }

        public ObservableCollection<FrameworkElement> UIElementsList
        {
            get => _uIElementsList;
            set
            {
                if (_uIElementsList != value)
                {
                    _uIElementsList = value;
                    OnPropertyChanged();
                }
            }
        }

        public Frame IsMainFrame { get; } = new Frame();

        #region FeatureModule
        public RelayCommand OffssetTopCMD
        {
            get
            {
                return _offssetTopCMD ?? (_offssetTopCMD = new RelayCommand(() => {
                    FeatureModuleManager_OffssetTop();
                }));
            }
        }

        private void FeatureModuleManager_IsMagnifier()
        {
            if (VisibilityMagnifier == Visibility.Hidden)
                VisibilityMagnifier = Visibility.Visible;
            else
                VisibilityMagnifier = Visibility.Hidden;
        }

        private void FeatureModuleManager_OffssetTop()
        {
            if (OffssetTop > 0)
                OffssetTop = 0;
            else
                OffssetTop = 1600;
        }

        public int OffssetTop
        {
            get => _offssetTop;
            set => SetProperty(ref _offssetTop, value);
        }

        public Visibility VisibilityMagnifier
        {
            get => _visibilityMagnifier;
            set => SetProperty(ref _visibilityMagnifier, value);
        }

        #endregion
        // Скрывем элементы при навигации
        private void IsMainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Frame frame = sender as Frame;
            if (e.Content is _NonPage)
            {
                // Показываем всё
                UIElementsList.ToList().ForEach(x => x.Visibility = Visibility.Visible);
                frame.Visibility = Visibility.Hidden;
            }
            else
            {
                UIElementsList.Where(delegate(FrameworkElement elm) 
                {
                    if (elm.Tag is TagElement tag)
                        return tag.IsNavVisibility;
                    return false;
                }).ToList().ForEach(x => x.Visibility = Visibility.Hidden);
                frame.Visibility = Visibility.Visible;
            }
        }

        // Обновляем загружаем данные
        private void UpdateTimerTick(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
