using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using VSHIM.Control.WebControl;
using CefSharp;
using VSHIM.Control.Handicapped.Utilits;

namespace VSHIM.Control.Handicapped
{
    static class TextZoom
    {
        private static Frame frame = null;
        private static bool IsZoom = false;
        private static ChromiumBrowser browser = null;
        private static int ZoomLvlValue = 0;
        private static Dictionary<int, double> ElementFontSize = new Dictionary<int, double>();

        static TextZoom()
        {

        }

        public static void Inizializate(Grid grid)
        {
            foreach(var item in grid.Children)
            {
                if(item is Frame)
                {
                    frame = (Frame)item;
                    break;
                }
                else
                {
                    if (item is Grid)
                    {
                        Inizializate((Grid)item);
                    }
                }
            }
        }

        private static void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Page elm = e.Content as Page;
            if(elm != null)
            {
                elm.Loaded += Elm_Loaded;
            }
        }

        private static void Elm_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (ItemsControl item in FindVisualChildren<ItemsControl>((Page)sender))
            {
                item.LayoutUpdated += Item_LayoutUpdated;
            }

            FindWebBrowser((Page)sender);

            SetTextZoom(true);
        }

        private static void FindWebBrowser(Page sender)
        {
            foreach (ChromiumBrowser item in FindVisualChildren<ChromiumBrowser>(sender))
            {
                item.FrameLoadEnd += Item_FrameLoadEnd;
            }
        }

        private static void FindWebBrowser(Frame sender)
        {
            foreach (ChromiumBrowser item in FindVisualChildren<ChromiumBrowser>(sender))
            {
                browser = item;
            }
        }

        private static void Item_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            browser = ((ChromiumBrowser)sender);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                SetTextZoom(true);
            }));
        }

        private static void Item_LayoutUpdated(object sender, EventArgs e)
        {
            SetTextZoom(true);
        }

        private static void SetTextZoom(bool IsZoom)
        {
            if (TextZoom.IsZoom == false) return;

            double FontSize = 0;
            int index = 0;

            foreach (var item in FindVisualChildren<TextBlock>(frame))
            {
                if(IsZoom)
                {
                    if(item.FontSize > 0)
                    {

                        index = item.GetHashCode();
                        if(!ElementFontSize.ContainsKey(index))
                        {
                            ElementFontSize.Add(index, item.FontSize);
                            FontSize = HMatch.ValuePercentage(item.FontSize, ZoomLvlValue);
                        }
                        else
                        {
                            double font = ElementFontSize[index];
                            FontSize = HMatch.ValuePercentage(font, ZoomLvlValue);
                        }     

                        if(FontSize > 64)
                        {
                            item.FontSize = 64;
                        }
                        else
                        {
                            item.FontSize = FontSize;
                        }
                    }
                }
                else
                {
                    index = item.GetHashCode();
                    if (ElementFontSize.ContainsKey(index))
                    {
                        FontSize = ElementFontSize[index];
                        item.FontSize = FontSize;
                    }
                }
            }

            if (IsZoom)
            {
                if (browser != null)
                {
                    index = browser.GetHashCode();
                    if (!ElementFontSize.ContainsKey(index))
                    {
                        ElementFontSize.Add(index, WebBrowserConfig.ZoomLvl);
                        FontSize = HMatch.ValuePercentageWebBrowser(WebBrowserConfig.ZoomLvl, ZoomLvlValue);
                    }
                    else
                    {
                        double font = ElementFontSize[index];
                        FontSize = HMatch.ValuePercentageWebBrowser(font, ZoomLvlValue);
                    }

                    FontSize = Math.Round(FontSize, 1);

                    browser.SetZoomLevel(FontSize);
                    browser.ZoomLevel = FontSize;
                }
            }
            else
            {
                if (browser != null)
                {
                    browser.SetZoomLevel(WebBrowserConfig.ZoomLvl);
                    browser.ZoomLevel = WebBrowserConfig.ZoomLvl;
                }
            }
        }

        public static void OnZoom(int ZoomLvl)
        {
            ZoomLvlValue = ZoomLvl;
            IsZoom = true;
            FindWebBrowser(frame);
            SetTextZoom(true);

            frame.Navigated += Frame_Navigated;
        }

        public static void OffZoom()
        {
            Page elm = frame.Content as Page;
            if (elm != null)
            {
                elm.Loaded += delegate { };
            }

            frame.Navigated += delegate { };

            ZoomLvlValue = 0;
            SetTextZoom(false);
            IsZoom = false;
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
