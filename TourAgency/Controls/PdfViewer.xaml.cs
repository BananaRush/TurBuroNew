using MoonPdfLib.MuPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using TourAgency.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : UserControl
    {
        public PdfViewer()
        {
            InitializeComponent();
            this.Loaded += PdfViewer_Loaded;
        }
        public PdfViewer(String uri)
        {
            Uri = uri;
            InitializeComponent();
            this.Loaded += PdfViewer_Loaded;
        }

        public String Uri
        {
            get { return (String)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(String), typeof(UserControl), new PropertyMetadata(String.Empty));


        private void PdfViewer_Loaded(object sender, RoutedEventArgs e)
        {
            OpenPdf();
        }

        private void OpenPdf()
        {
            try
            {
                if (Uri == null)
                    return;
                if (!File.Exists($"Content/{Uri}"))
                    new WebClient() {Credentials = new NetworkCredential(Session.FtpUser, Session.FtpPass)}
                        .DownloadFile(new Uri($"ftp://{Session.FtpServ}/{Uri}"), $"Content/{Uri}");
                byte[] bytes = File.ReadAllBytes($"Content/{Uri}");
                var source = new MemorySource(bytes);
                PdfPanel.Open(source);
                PdfPanel.Zoom(1.5);
            }
            catch
            {
               
            }
        }
    }
}
