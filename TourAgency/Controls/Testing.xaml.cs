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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourAgency.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для Testing.xaml
    /// </summary>
    public partial class Testing : UserControl
    {
        public Testing()
        {
            InitializeComponent();
        }

        private void AgainButton_Click(object sender, RoutedEventArgs e)
        {
            StartTest.Visibility = Visibility.Visible;
            Question.Visibility = Visibility.Visible;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartTest.Visibility = Visibility.Collapsed;
        }

        private async void TestClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var textblock = FindVisualChildren<TextBlock>(button);
            var a= textblock.FirstOrDefault();
            if(a==null) return;
            var test= Session.CurrentSession.Tests.FirstOrDefault(f => f.Question == Session.CurrentSession.Test.Question);
            if(test == null) return;
            await WebApi.Test.MakeVoit(test.Question, Session.CurrentSession.Tests.IndexOf(test));
            Question.Visibility = Visibility.Collapsed;
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
    }

}
