using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для SwitchButton.xaml
    /// </summary>
    public partial class SwitchButton : UserControl
    {


        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(SwitchButton), new PropertyMetadata(false));

     

        public SwitchButton()
        {
            InitializeComponent();
        }
        private Brush LightColor = (Brush) new BrushConverter().ConvertFrom("#f9f9f9");
        private Brush OrangeColor = (Brush) new BrushConverter().ConvertFrom("#ecb244");


        private void MainGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsChecked == true)
            {
                IsChecked = false;
                RectangleGrid.HorizontalAlignment = HorizontalAlignment.Left;
                MainGrid.Background = LightColor;
            }
            else
            {
                IsChecked = true;
                RectangleGrid.HorizontalAlignment = HorizontalAlignment.Right;
                MainGrid.Background = OrangeColor;
            }
        }
    }
}
