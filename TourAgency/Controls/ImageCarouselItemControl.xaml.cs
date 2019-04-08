using System;
using System.Collections.Generic;
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

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для ImageCarouselItemControl.xaml
    /// </summary>
    public partial class ImageCarouselItemControl : UserControl
    {
        public ImageCarouselItemControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
            "Url", typeof(string), typeof(ImageCarouselItemControl), new PropertyMetadata(default(string)));

        public string Url
        {
            get => (string) GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public string PhotoSource
        {
            get { return (string)GetValue(PhotoSourceProperty); }
            set { SetValue(PhotoSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PhotoSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotoSourceProperty =
            DependencyProperty.Register("PhotoSource", typeof(string), typeof(UserControl), new PropertyMetadata(""));



        public string Heading
        {
            get { return (string)GetValue(HeadingProperty); }
            set { SetValue(HeadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Heading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadingProperty =
            DependencyProperty.Register("Heading", typeof(string), typeof(UserControl), new PropertyMetadata(""));



        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(UserControl), new PropertyMetadata(""));



        public string PostDate
        {
            get { return (string)GetValue(PostDateProperty); }
            set { SetValue(PostDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PostDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PostDateProperty =
            DependencyProperty.Register("PostDate", typeof(string), typeof(UserControl), new PropertyMetadata(""));



        public object ObjectToNavigate
        {
            get { return (object)GetValue(ObjectToNavigateProperty); }
            set { SetValue(ObjectToNavigateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ObjectToNavigate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ObjectToNavigateProperty =
            DependencyProperty.Register("ObjectToNavigate", typeof(object), typeof(UserControl), new PropertyMetadata(0));



    }
}
