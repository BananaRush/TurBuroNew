using CefSharp.Wpf;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using System;

namespace VSHIM.Control.WebControl
{
    class ChromiumBrowser : ChromiumWebBrowser
    {
        bool IsMouseDown = false;
        public Point? LastSavedPoint = null;
        IBrowserHost host;
        private static string valtext { get; set; }
        public event Action OnClickButton = delegate { };
        private static bool IsInizialization = false;

        public static readonly DependencyProperty SetTextProperty =
                DependencyProperty.Register("LoadHtml", typeof(string), typeof(ChromiumBrowser), new
                PropertyMetadata(string.Empty,
                new PropertyChangedCallback(OnSetTextChanged)));

        public ChromiumBrowser() : base()
        {
            this.TouchMove += ChromiumWebBrowsers_TouchMove;
            this.TouchDown += ChromiumWebBrowsers_TouchDown;
            this.TouchLeave += ChromiumWebBrowsers_TouchLeave;
            this.TouchUp += ChromiumWebBrowsers_TouchUp;
            this.FrameLoadEnd += ChromiumBrowser_FrameLoadEnd;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            this.IsBrowserInitializedChanged += ChromiumBrowser_IsBrowserInitializedChanged;
            IsInizialization = false;
            this.RequestHandler = new BrowserRequestHandler();
        }

        private void ChromiumBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            ((ChromiumBrowser)sender).SetZoomLevel(WebBrowserConfig.ZoomLvl);
            
        }

        public void SetZoom(double zoom)
        {
            this.SetZoomLevel(zoom);
        }

        private void ChromiumWebBrowsers_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            IsMouseDown = false;
            LastSavedPoint = null;
        }

        private void ChromiumWebBrowsers_TouchLeave(object sender, System.Windows.Input.TouchEventArgs e)
        {
            IsMouseDown = false;
            LastSavedPoint = null;
        }

        private void ChromiumWebBrowsers_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (host == null)
            {
                IWebBrowser host2 = this;
                IBrowser bworser = host2.GetBrowser();
                host = bworser.GetHost();
            }
            IsMouseDown = true;
        }

        private void ChromiumWebBrowsers_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (host != null && IsMouseDown)
            {
                TouchPoint t = e.GetTouchPoint(this);
                Point p = t.Position;

                if (!LastSavedPoint.HasValue)
                    LastSavedPoint = p;

                int x = (int)p.X;
                int y = (int)p.Y;
                int oldX = (int)LastSavedPoint.Value.X;
                int oldy = (int)LastSavedPoint.Value.Y;

                host.SendMouseWheelEvent(x, y, 0, y - oldy, CefEventFlags.MiddleMouseButton);

                LastSavedPoint = p; 
            }
        }

        public string LoadHtml
        {
            get { return (string)GetValue(SetTextProperty); }
            set { SetCurrentValue(SetTextProperty, value); }
        }

        private static void OnSetTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string val = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
            val = val.Replace("&nbsp;", " ");
            string value = @"<!DOCTYPE html>
                            <style>
                            html, body 
                            {
                                -moz-user-select: none;
                                word-break: break-word;
		                        word-wrap: break-word;
		                        overflow-wrap: break-word;
                                flex-flow: wrap; 
                                overflow-x: hidden;
                                max-widht:100%;
                                padding:0px;
                                margin: 0px;
                                -ms-user-select: none;
                                -o-user-select: none;
                                -webkit-user-select: none;
                                user-select: none;
                            }

                            html::-webkit-scrollbar 
                            { 
                                width: 0; 
                            }
                            </style>
                            <script>
                             function GetNavigation(elm) 
                             {
                                boundAsync.getNavigation(elm);
                             }
                            </script>
                            <body>"
                            + 
                            val 
                            + 
                            @"</body>
                            </html>";
            valtext = value;

            if (!IsInizialization)
                return;
            try
            {
                ChromiumBrowser ee = d as ChromiumBrowser;
                ee.LoadHtml(valtext, "http://www.example.com");
            }
            catch { }
        }

        private void ChromiumBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ChromiumBrowser ee = sender as ChromiumBrowser;
                ee.LoadHtml(valtext, "http://www.example.com");
                IsInizialization = true;
            }
            catch { }
        }

        private static double PercentageToToZoomLevel(int percent)
        {
            return ((percent - 100)) / 25.0;
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e) { }
    }
}
