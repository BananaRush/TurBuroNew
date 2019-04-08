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
using System.Windows.Threading;
using TourAgency.Model;
using TourAgency.Pages;
using TourAgency.Utilities;
using TourAgencyAdmin.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для CarouselControl.xaml
    /// </summary>
    public partial class CarouselControl : UserControl, INotifyPropertyChanged
    {
        public CarouselControl()
        {
            InitializeComponent();
            selectedItem = 0;
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick+= TimerOnTick;

            Session.CurrentSession.TimerWorked+= CurrentSessionOnTimerWorked;

            Session.CurrentSession.SplashHidden +=  (sender, args) =>
            {
                if(!timer.IsEnabled)
                    FindVisualChildren<VideoControl>(ItemsC).FirstOrDefault(f => f.VideoSource == (ItemsC.Items[selectedItem] as CarouselItemModel).VideoUri)?.MePlayer_OnMouseDown(null, null);

            };
            Loaded+= OnLoaded;
        }

        private void CurrentSessionOnTimerWorked(object o, EventArgs eventArgs)
        {
            if (!timer.IsEnabled)
                FindVisualChildren<VideoControl>(ItemsC).FirstOrDefault(f => f.VideoSource == (ItemsC.Items[selectedItem] as CarouselItemModel).VideoUri)?.MePlayer_OnMouseDown(null, null);
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            
            RadioButtonsVisibility();
            try
            {

                while (ItemsC.Items.Count ==0)
                {
                    await Task.Delay(100);
                }
            if ((ItemsC.Items[selectedItem] as CarouselItemModel).Type == "Video")
            {
                FindVisualChildren<VideoControl>(ItemsC).FirstOrDefault(f => f.VideoSource == (ItemsC.Items[selectedItem] as CarouselItemModel).VideoUri)?.MePlayer_OnMouseDown(null, null);
            }
            else
            {
                timer.Start();
            }
            }
            catch (Exception e)
            {
                //
            }
        }

        private async void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (selectedItem == ItemsC.Items.Count-1)
            {

                for (int i = 0; i < ItemsC.Items.Count; i++)
                {
                    ScrollLeft_Click(null, null);
                    await Task.Delay(300);
                }
                
                return;
            }

                ScrollRight_Click(null, null);
        }

        DispatcherTimer timer = new DispatcherTimer();

        private void RadioButtonsVisibility()
        {
            var num = ItemsC.Items.Count;
            CarouselItem0.Visibility = Visibility.Visible;
            CarouselItem1.Visibility = Visibility.Visible;
            CarouselItem2.Visibility = Visibility.Visible;
            CarouselItem3.Visibility = Visibility.Visible;
            CarouselItem4.Visibility = Visibility.Visible;
            switch(num)
            {
                case 4:
                    CarouselItem4.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    CarouselItem4.Visibility = Visibility.Collapsed;
                    CarouselItem3.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    CarouselItem2.Visibility = Visibility.Collapsed;
                    CarouselItem3.Visibility = Visibility.Collapsed;
                    CarouselItem4.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    CarouselItem1.Visibility = Visibility.Collapsed;
                    CarouselItem2.Visibility = Visibility.Collapsed;
                    CarouselItem3.Visibility = Visibility.Collapsed;
                    CarouselItem4.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        public double CarouselPosition
        {
            get { return (double)GetValue(CarouselPositionProperty); }
            set
            {
                SetValue(CarouselPositionProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CarouselPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CarouselPositionProperty =
            DependencyProperty.Register("CarouselPosition", typeof(double), typeof(UserControl), new PropertyMetadata(0.0, new PropertyChangedCallback(OnCarouselPositionChanged)));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void OnCarouselPositionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CarouselControl control = sender as CarouselControl;
            control.sviewer.ScrollToHorizontalOffset(control.sviewer.HorizontalOffset + (double)(e.NewValue)-control.oldvalue);
            control.oldvalue = (double)(e.NewValue);
        }

        private double oldvalue = 0.0;
        private int _selectedItem;
        public int selectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedItem"));
            }
        }

        private void ScrollLeft_Click(object sender, RoutedEventArgs e)
        {
            ScrollLeft.IsEnabled = false;
            ScrollRight.IsEnabled = false;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            doubleAnimation.EasingFunction = new CubicEase();
            oldvalue = CarouselPosition;
            doubleAnimation.From = CarouselPosition;
            doubleAnimation.To = -1780 + CarouselPosition;
            doubleAnimation.Completed += (s, ea) =>
            {
                ScrollLeft.IsEnabled = true;
                ScrollRight.IsEnabled = true;
            };
            BeginAnimation(CarouselPositionProperty, doubleAnimation);
            var offset = selectedItem - 1;
            offset = Math.Max(0, offset);
            offset = Math.Min(ItemsC.Items.Count - 1, offset);
            selectedItem = offset;
            FindVisualChildren<VideoControl>(ItemsC)?.ToList().ForEach(f =>
            {
                f.Pause();
            });
            if ((ItemsC.Items[selectedItem] as CarouselItemModel).Type == "Video")
            {
                FindVisualChildren<VideoControl>(ItemsC).FirstOrDefault(f=> f.VideoSource == (ItemsC.Items[selectedItem] as CarouselItemModel).VideoUri)?.MePlayer_OnMouseDown(null, null);
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        private void ScrollRight_Click(object sender, RoutedEventArgs e)
        {
            ScrollLeft.IsEnabled = false;
            ScrollRight.IsEnabled = false;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            doubleAnimation.EasingFunction = new CubicEase();
            doubleAnimation.From = CarouselPosition;
            doubleAnimation.To = 1780 + CarouselPosition;
            doubleAnimation.Completed += (s, ea) =>
            {
                ScrollLeft.IsEnabled = true;
                ScrollRight.IsEnabled = true;
            };
            BeginAnimation(CarouselPositionProperty, doubleAnimation);
            var offset = selectedItem + 1;
            offset = Math.Max(0, offset);
            offset = Math.Min(ItemsC.Items.Count - 1, offset);
            selectedItem = offset;
            FindVisualChildren<VideoControl>(ItemsC)?.ToList().ForEach(f =>
            {
                f.Pause();
            });
            if ((ItemsC.Items[selectedItem] as CarouselItemModel).Type == "Video")
            {
                FindVisualChildren<VideoControl>(ItemsC).FirstOrDefault(f => f.VideoSource == (ItemsC.Items[selectedItem] as CarouselItemModel).VideoUri)?.MePlayer_OnMouseDown(null, null);
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        private Point initialPoint;

        private void sviewer_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            //initialPoint = e.Position;
            initialPoint = e.ManipulationOrigin;
        }

        private void sviewer_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            try
            {
                if (e.IsInertial)
                {
                    Point currentPoint = e.ManipulationOrigin;
                    double diff = currentPoint.X - initialPoint.X;
                    if ( diff < -100)
                    {
                        ScrollRight_Click(null, null);
                    }
                    if (diff > 100)
                    {
                       ScrollLeft_Click(null, null);
                    }
                    e.Complete();
                }

            }
            catch
            {
                //
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

        private void VideoControl_OnEndPlay(object sender, EventArgs e)
        {
            TimerOnTick(null, null);
        }

        private void TouchBaner(object sender, TouchEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Frame.Navigate(new Map(((ImageCarouselItemControl)sender).Url, ((ImageCarouselItemControl)sender).Heading));
        }
    }
}
