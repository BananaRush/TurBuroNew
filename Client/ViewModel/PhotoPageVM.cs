using Client.Utilits;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.ViewModel
{
    class PhotoPageVM : Bandel
    {
        private RelayCommand _photo;
        private RelayCommand _imageFrameLoad;
        private RelayCommand _sendEmail;
        private readonly VideoTranslation videoTranslation = null;
        private Image _imageFrame;
        public readonly int TIMER_SECUNDS = 5;
        private string _timerTick = string.Empty;
        private string _imagePath = string.Empty;
        private bool _checkEmailForm;
        private bool _visibilityBtnEmail;
        private string _email = string.Empty;
        public PhotoPageVM()
        {
            videoTranslation = new VideoTranslation();
            videoTranslation.NewFrame += VideoTranslation_NewFrame;
            videoTranslation.Start();
        }

        private void VideoTranslation_NewFrame(byte[] img)
        {
            using (MemoryStream memory = new MemoryStream(img))
            {
                memory.Position = 0;
                if (_imageFrame == null)
                    return;
                _imageFrame.Dispatcher.Invoke(new Action(delegate ()
                {
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                    _imageFrame.Source = bitmapimage;
                }));
            }
        }

        public RelayCommand Photo
        {
            get
            {
                return _photo ?? (_photo = new RelayCommand(() =>
                {
                    if(VisibilityBtnEmail == true)
                    {
                        VisibilityBtnEmail = false;
                        CheckEmailForm = false;
                        videoTranslation.Start();
                        return;
                    }

                    TimeSpan ts = new TimeSpan(0, 0, TIMER_SECUNDS);
                    Task.Factory.StartNew(() => {
                        while (ts.Seconds != 0)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(delegate ()
                            {
                                TimerTick = ts.Seconds.ToString();
                            }));

                            Thread.Sleep(1000);
                            ts = ts.Subtract(new TimeSpan(0, 0, 1));
                        }
                        videoTranslation.Stop();

                        _imageFrame.Dispatcher.Invoke(new Action(delegate ()
                        {
                            TimerTick = string.Empty;
                            VisibilityBtnEmail = SavePhoto();
                        }));
                    });
                }));
            }
        }

        public RelayCommand ImageFrameLoad
        {
            get
            {
                return _imageFrameLoad ?? (_imageFrameLoad = new RelayCommand(obj => 
                {
                    if(obj is Image img)
                    {
                        _imageFrame = img;
                    }
                }));
            }
        }

        public RelayCommand SendEmail
        {
            get
            {
                return _sendEmail ?? (_sendEmail = new RelayCommand(async() => 
                {
                    if (!File.Exists(_imagePath))
                        return;

                    if(!EmailManager.IsValidEmail(Email))
                    {
                        // Email не действителен
                        return;
                    }

                    if(await EmailManager.SendFile(Email, _imagePath))
                    {
                        _imagePath = string.Empty;
                    }
                }));
            }
        }
        private bool SavePhoto()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "Photo");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (_imageFrame == null)
                    return false;

                path = Path.Combine(path, Guid.NewGuid().ToString() + ".png");
                RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)_imageFrame.ActualWidth, (int)_imageFrame.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(_imageFrame);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(stream);
                }

                _imagePath = path;

            }
            catch { return false; }
            return true;
        }

        public string TimerTick
        {
            get => _timerTick;
            set => SetProperty(ref _timerTick, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public bool CheckEmailForm
        {
            get => _checkEmailForm;
            set => SetProperty(ref _checkEmailForm, value);
        }

        public bool VisibilityBtnEmail
        {
            get => _visibilityBtnEmail;
            set => SetProperty(ref _visibilityBtnEmail, value);
        }
    }
}
