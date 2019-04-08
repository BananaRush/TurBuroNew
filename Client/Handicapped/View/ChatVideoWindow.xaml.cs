using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VSHIM.Control.Handicapped.View
{
    /// <summary>
    /// Логика взаимодействия для ChatVideoWindow.xaml
    /// </summary>
    public partial class ChatVideoWindow : Window
    {
        public VideoTranslation videoTranslation = null;

        public ChatVideoWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void VideoTranslation_NewFrame1(byte[] img)
        {
            //Conver(img);
        }

        private void VideoTranslation_NewFrame(byte[] img)
        {
            //Conver(img);
        }

        private void Conver(byte[] img)
        {
            using (MemoryStream memory = new MemoryStream(img))
            {
                memory.Position = 0;
                imgs.Dispatcher.Invoke(new Action(delegate ()
                {
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.None;
                    bitmapimage.EndInit();

                    MainImg.ImageSource = bitmapimage;
                }));
            }
        }

        public void VideoFrame(byte[] buff)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                Conver(buff);
            }));
        }

        public void SetOperatorName(string name)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                this.OperatorName.Text = name;
            }));
        }

        public void Border_Loaded(object sender, RoutedEventArgs e)
        {
            MainImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Control/Handicapped/Image/777.png"));
        }
    }
}
