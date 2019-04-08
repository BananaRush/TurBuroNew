using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CefSharp;
using CefSharp.Wpf;
using Emgu.CV;
using Emgu.CV.Structure;
using SimpleKeyboard;
using TourAgency.Annotations;
using TourAgency.Utilities;
using MessageBox = TourAgency.Controls.MessageBox;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для FotoPage.xaml
    /// </summary>
    public partial class FotoPage : Page, INotifyPropertyChanged
    {
        public FotoPage(bool isEmotion)
        {
            InitializeComponent();
            NeironStats.StatisticStop();
            Session.CurrentSession.IsCameraUsed = true;
            IsEmotion = true;
            AllKiosk.Remove(Session.CurrentSession.NameTerminal);
            capture = new Capture();
            IsDisabled = true;
            capture.Start();
            capture.ImageGrabbed+= CaptureOnImageGrabbed;
            Unloaded += UnloadedCapture;
        }

        private void UnloadedCapture(object o, RoutedEventArgs routedEventArgs)
        {
            StopCapture();
            capture.Dispose();
            Session.CurrentSession.IsCameraUsed = false;
        }

        private Capture capture;
        private void CaptureOnImageGrabbed(object o, EventArgs eventArgs)
        {
            using (Mat mat = new Mat())
            {
                capture.Retrieve(mat);
                Image<Bgr, byte> img = mat.ToImage<Bgr, byte>();
                Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        Source = ToBitmapSource(img);

                    }
                    catch (Exception e)
                    {
                      
                    }
                });
            }

        }

        private void StopCapture()
        {
            capture.ImageGrabbed -= CaptureOnImageGrabbed;
            capture.Stop();
        }

        public FotoPage()
        {
            InitializeComponent();
            NeironStats.StatisticStop();
            CefSettings settings = new CefSettings();
            ModelMasks = new List<ModelMask>()
            {
                new ModelMask()
                {
                    index = 1,
                    NameImg = $"{path}terminator_crop.jpg"
                },
                new ModelMask()
                {
                    index = 10,
                    NameImg = $"{path}cage.jpg"
                },
                new ModelMask()
                {
                    index = 3,
                    NameImg = $"{path}fragrance-George-Clooney-main_crop.jpg"
                },
                new ModelMask()
                {
                    index = 4,
                    NameImg = $"{path}Justin-Bieber2_crop.jpg"
                },
                new ModelMask()
                {
                    index = 12,
                    NameImg = $"{path}obama4_crop.jpg"
                }
            };
            Unloaded+= OnUnloaded;
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream", "1");
            settings.CefCommandLineArgs.Add("enable-speech-input", "1");
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capture", "1");
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);
            _chromiumWeb = new ChromiumWebBrowser();
            _chromiumWeb.FrameLoadEnd += (sender, args) => Dispatcher.Invoke(()=> IsDisabled = true);
            _chromiumWeb.Address = "http://localhost:9966/examples/facesubstitution.html";
            AddChromium();
        }

        private void OnUnloaded(object o, RoutedEventArgs routedEventArgs)
        {
            _chromiumWeb.Dispose();
        }


        public static readonly DependencyProperty TimerTextProperty = DependencyProperty.Register(
            "TimerText", typeof(string), typeof(FotoPage), new PropertyMetadata(default(string)));

        public string TimerText
        {
            get => (string) GetValue(TimerTextProperty);
            set => SetValue(TimerTextProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(FotoPage), new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get => (ImageSource) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }


        public static readonly DependencyProperty IsEmotionProperty = DependencyProperty.Register(
            "IsEmotion", typeof(bool), typeof(FotoPage), new PropertyMetadata(default(bool)));

        public bool IsEmotion
        {
            get => (bool) GetValue(IsEmotionProperty);
            set => SetValue(IsEmotionProperty, value);
        }

        private  string path = Path.GetFullPath("Content/");

        public static readonly DependencyProperty ModelMasksProperty = DependencyProperty.Register(
            "ModelMasks", typeof(IEnumerable<ModelMask>), typeof(FotoPage), new PropertyMetadata(default(IEnumerable<ModelMask>)));

        public IEnumerable<ModelMask> ModelMasks
        {
            get => (IEnumerable<ModelMask>) GetValue(ModelMasksProperty);
            set => SetValue(ModelMasksProperty, value);
        }

        public static readonly DependencyProperty IsDisabledProperty = DependencyProperty.Register(
            "IsDisabled", typeof(bool), typeof(FotoPage), new PropertyMetadata(default(bool)));

        public bool IsDisabled
        {
            get => (bool) GetValue(IsDisabledProperty);
            set => SetValue(IsDisabledProperty, value);
        }

        public static readonly DependencyProperty PhotoResultProperty = DependencyProperty.Register(
            "PhotoResult", typeof(string), typeof(FotoPage), new PropertyMetadata(default(string)));

        public string PhotoResult
        {
            get => (string) GetValue(PhotoResultProperty);
            set => SetValue(PhotoResultProperty, value);
        }

        private ObservableCollection<string> _allKiosk;

        public ObservableCollection<string> AllKiosk
        {
            get => _allKiosk ?? (_allKiosk = new ObservableCollection<string>()
            {
                "Порт 1",
                "Порт 2",
                "Порт 3",
                "Порт 4",
                "Пулково 1",
                "Пулково 2",
                "Музей"
            });
            set
            {
                _allKiosk = value;
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty ResulReadyProperty = DependencyProperty.Register(
            "ResulReady", typeof(bool), typeof(FotoPage), new PropertyMetadata(default(bool), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if((dependencyObject as FotoPage).IsEmotion)
                return;
            Session.CurrentSession.KeyBoardVisibility=  (bool)dependencyPropertyChangedEventArgs.NewValue? Visibility.Visible: Visibility.Collapsed;
        }

        public bool ResulReady
        {
            get => (bool) GetValue(ResulReadyProperty);
            set => SetValue(ResulReadyProperty, value);
        }

        private ICommand _makePhotoCommand;
        private ICommand _switchMaskCommand;
        private ICommand _sendEmailCommand;
        private ICommand _sendPhotoCommand;

        public ICommand SendPhotoCommand => _sendPhotoCommand ?? (_sendEmailCommand = new Command(async a =>
        {
            var allChecked = FindVisualChildren<CheckBox>(ItemsControl).Where(f => f.IsChecked == true);
            if (!allChecked.Any())
            {
                new MessageBox("Выберите терминалы, на которые будет отправлена фотография!").ShowDialog();
                return;
            }
            Session.CurrentSession.IsLoading = true;
          var res=  await EmotionShareComunication.Send(allChecked.Select(f => f.Content.ToString()).ToArray(), 
              Path.GetFileName(PhotoResult));
            Session.CurrentSession.IsLoading = false;
            if (res)
            {
                new MessageBox("Фото успешно отправлено!").ShowDialog();
                (App.Current.MainWindow as MainWindow).Frame.Navigate(new FirstPage());
            }
            else
            {
                new MessageBox("Что то пошло не так :(\nПопробуйте позже").ShowDialog();
            }

        }));

        public ICommand SendEmailCommand => _sendEmailCommand ?? (_sendEmailCommand = new Command( a =>
        {
            if (string.IsNullOrEmpty(TextBox.Text))
            {
                new MessageBox("Введите электронную почту!").ShowDialog();
                return;
            }
            try
            {
                Session.CurrentSession.IsLoading = true;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("visit.peterburg@mail.ru");
                mail.To.Add(new MailAddress(TextBox.Text));
                mail.Subject = "Фотолаборатория";
                var attach = new Attachment($"Content/makedPhoto{Directory.GetFiles("Content").Length - 1}.png");
                mail.Attachments.Add(attach);
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.mail.ru";
                client.Port = 25;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("visit.peterburg@mail.ru", "privedmedved70");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.SendAsync(mail, null);
                client.SendCompleted += (sender, args) =>
                {
                    mail.Dispose();

                    attach.Dispose();
                    TextBox.Text = string.Empty;
                    new MessageBox("Фотография отправлена!").ShowDialog();
                };
            }
            catch (Exception e)
            {
                Session.CurrentSession.IsLoading = false;
                ResulReady = false;
                new MessageBox("Что то пошло не так :(\nПопробуйте позже").ShowDialog();
            }
            Session.CurrentSession.IsLoading = false;
            ResulReady = false;
        }));

        public ICommand SwitchMaskCommand => _switchMaskCommand ?? (_switchMaskCommand = new Command(a =>
        {
            if (a is ModelMask mask)
            {
                _chromiumWeb.ExecuteScriptAsync($"window.selectMaskByIndex({mask.index})");
            }
        }));

        public ICommand MakePhotoCommand => _makePhotoCommand ?? (_makePhotoCommand = new Command(a =>
        {
            if (IsEmotion)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Source));
                string namePhoto = Directory.GetCurrentDirectory() + "/Content/EmotionShare/" +
                                   $"{DateTime.Now:dd_MM_yyy_HH_mm_ss}.png";
                using (FileStream stream = new FileStream(namePhoto, FileMode.Create))
                    encoder.Save(stream);
                    PhotoResult = namePhoto;
                    ResulReady = true;
                    return;
            }
            RenderTargetBitmap renderTargetBitmap =
                new RenderTargetBitmap((int)_chromiumWeb.ActualWidth, (int)_chromiumWeb.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(_chromiumWeb);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            
            using (Stream fileStream = File.Create($"Content/makedPhoto{Directory.GetFiles("Content").Length}.png"))
            {
                pngImage.Save(fileStream);
            }
            ResulReady = true;
            PhotoResult = Path.GetFullPath($"Content/makedPhoto{Directory.GetFiles("Content").Length-1}.png");
        }));


        private void AddChromium()
        {
            Grid.Children.Add(_chromiumWeb);
        }

        private string _name;
        private ChromiumWebBrowser _chromiumWeb;
       
        public class ModelMask
        {
            public int index { get; set;}
            public string NameImg { get; set; }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ResulReady = false;
        }

        private void TextBox_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;
        }

        private void TextBox_OnFocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
        }

        private void BackCommand(object sender, TouchEventArgs e)
        {
            if(IsEmotion)
            StopCapture();
            (App.Current.MainWindow as MainWindow).Frame.GoBack();
        }

        private void SendTouch(object sender, RoutedEventArgs e)
        {
            if(IsEmotion)
                SendPhotoCommand.Execute(null);
            else
            SendEmailCommand.Execute(null);
        }


        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap();
                long imageSize = image.Size.Height * image.Size.Width * 4;
                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(ptr);
                return bs;
            }
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MakePhotoTouch(object sender, RoutedEventArgs e)
        {
            MakePhotoCommand.Execute(null);
        }
    }
}
