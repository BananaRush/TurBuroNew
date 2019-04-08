using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using TourAgency.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для VideoControl.xaml
    /// </summary>
    public partial class VideoControl : UserControl
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        public VideoControl()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }



        public Uri VideoSource
        {
            get => (Uri)GetValue(VideoSourceProperty);
            set => SetValue(VideoSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for VideoSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VideoSourceProperty =
            DependencyProperty.Register("VideoSource", typeof(Uri), typeof(UserControl), new PropertyMetadata(null));



        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mePlayer.Position.TotalSeconds;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //mePlayer.Close();
            //mePlayer.Position = TimeSpan.MinValue;
            mePlayer.Play();
            //Play.Visibility = Visibility.Collapsed;
            //Stop.Visibility = Visibility.Visible;
            //Stop.IsEnabled = false;
            Storyboard reduceOpacity = this.FindResource("ReduceOpacity") as Storyboard;
            reduceOpacity.Begin();
        }



        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void VideoElement_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (MainGrid.Opacity == 1)
            {
                Storyboard reduceOpacity = this.FindResource("ReduceOpacity") as Storyboard;
                reduceOpacity.Begin();
            }
            else
            {
                Storyboard IncreaceOpacity = this.FindResource("IncreaceOpacity") as Storyboard;
                IncreaceOpacity.Begin();
                //Stop.IsEnabled = true;
            }

        }

        private void Stop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mePlayer.Pause();
            //Play.Visibility = Visibility.Visible;
            //Stop.Visibility = Visibility.Collapsed;
        }

        public event EventHandler EndPlay;
        private void mePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
            IsPlaying = false;
            Storyboard IncreaceOpacity = this.FindResource("IncreaceOpacity") as Storyboard;
            IncreaceOpacity.Begin();
            //Play.Visibility = Visibility.Visible;
            //Stop.Visibility = Visibility.Collapsed;
            mePlayer.Position = TimeSpan.FromSeconds(0);
            EndPlay.Invoke(null,null);
        }


        public void Pause()
        {
            mePlayer.Pause();
            IsPlaying = false;
            //Stop.Visibility = Visibility.Collapsed;


        }

        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
            "IsPlaying", typeof(bool), typeof(VideoControl), new PropertyMetadata(default(bool)));

        public bool IsPlaying
        {
            get => (bool) GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        private void MePlayer_OnTouchDown(object sender, TouchEventArgs e)
        {
            IsPlaying = !IsPlaying;
            if(!IsPlaying)
                mePlayer.Pause();
            else
            {
                mePlayer.Play();
            }
            //if (MainGrid.Opacity == 1)
            //{
            //    Storyboard reduceOpacity = this.FindResource("ReduceOpacity") as Storyboard;
            //    reduceOpacity.Begin();
            //}
            //else
            //{
            //    Storyboard IncreaceOpacity = this.FindResource("IncreaceOpacity") as Storyboard;
            //    IncreaceOpacity.Begin();
            //    //Stop.IsEnabled = true;
            //}
        }

        private void Stop_OnTouchDown(object sender, TouchEventArgs e)
        {
            mePlayer.Pause();
            //Play.Visibility = Visibility.Visible;
            //Stop.Visibility = Visibility.Collapsed;
        }

        private void MePlayer_OnLoaded(object sender, RoutedEventArgs e)
        {
            //mePlayer.Play();
            //mePlayer.Stop();
        }

        public void MePlayer_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Session.CurrentSession.VisibilitySplash == Visibility.Visible) return;

                IsPlaying = !IsPlaying;
            if (!IsPlaying)
                mePlayer.Pause();
            else
            {
                mePlayer.Play();
            }
        }
    }
}
