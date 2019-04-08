using CefSharp.Wpf;
using System.Windows;
using System.Windows.Input;
using CefSharp;

namespace VSHIM.Control.WebControl
{
    class ChromiumBrowser : ChromiumWebBrowser
    {
        bool IsMouseDown = false;
        Point? LastSavedPoint = null;
        IBrowserHost host;

        public ChromiumBrowser() : base()
        {
            this.TouchMove += ChromiumWebBrowsers_TouchMove;
            this.TouchDown += ChromiumWebBrowsers_TouchDown;
            this.TouchLeave += ChromiumWebBrowsers_TouchLeave;
            this.TouchUp += ChromiumWebBrowsers_TouchUp;
            this.FrameLoadEnd += ChromiumBrowser_FrameLoadEnd;

            //CefSharpSettings.LegacyJavascriptBindingEnabled = true;
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
                IWebBrowser host2 = this; // **your Browser's  Instance** 
                IBrowser bworser = host2.GetBrowser();
                host = bworser.GetHost(); // Get host from Browser
            }
            IsMouseDown = true;
            // this.Cursor = System.Windows.Input.Cursors.None;
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

                LastSavedPoint = p; //Store the last Point for reference
            }
        }


        public static readonly DependencyProperty SetTextProperty =
               DependencyProperty.Register("LoadHtml", typeof(string), typeof(ChromiumBrowser), new
                  PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged)));

        public string LoadHtml
        {
            get { return (string)GetValue(SetTextProperty); }
            set { SetValue(SetTextProperty, value); }
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
                    /* spoiler */
                    div.spoiler {
	                    padding: 5px;
	                    line-height: 1.6;
                    }

                    div.spoiler div.spoiler-title {
	                    color: #000000;
	                    font-size: 12px;
	                    font-weight: bold;
	                    padding: 4px 7px;
	                    border: 1px solid #bbbbbb;
	                    border-bottom-color: #999999;
	                    -moz-border-radius: 3px;
	                    -webkit-border-radius: 3px;
	                    border-radius: 3px;
	                    -moz-box-shadow: 0 1px 0 rgba(255,255,255,.5), 0 0 2px rgba(255,255,255,.15) inset, 0 1px 0 rgba(255,255,255,.15) inset;
	                    -webkit-box-shadow: 0 1px 0 rgba(255,255,255,.5), 0 0 2px rgba(255,255,255,.15) inset, 0 1px 0 rgba(255,255,255,.15) inset;
	                    box-shadow: 0 1px 0 rgba(255,255,255,.5), 0 0 2px rgba(255,255,255,.15) inset, 0 1px 0 rgba(255,255,255,.15) inset;
	                    background: #cfd1cf;
	                    background-image: -webkit-gradient(linear, left top, left bottom, from(#f5f5f5), to(#cfd1cf));
	                    background-image: -moz-linear-gradient(top, #f5f5f5, #e5e5e5);
	                    background-image: -webkit-linear-gradient(top, #f5f5f5, #e5e5e5);
	                    background-image: -o-linear-gradient(top, #f5f5f5, #e5e5e5);
	                    background-image: -ms-linear-gradient(top, #f5f5f5, #e5e5e5);
	                    background-image: linear-gradient(top, #f5f5f5, #e5e5e5);
	                    filter: progid:DXImageTransform.Microsoft.gradient(gradientType=0, startColorstr='#f5f5f5', endColorstr='#cfd1cf');
	                    cursor: pointer;
	                    -moz-user-select: none;
	                    -webkit-user-select: none;
	                    -ms-user-select: none;
                    }

                    div.spoiler div.spoiler-title div.spoiler-toggle {
	                    display: inline-block;
	                    width: 11px;
	                    height: 11px;
	                    line-height: 14px;
	                    margin-left: 4px;
	                    margin-right: 6px;
	                    cursor: pointer;
	                    -webkit-user-modify: read-only;
                    }

                    div.spoiler div.spoiler-title div.hide-icon {
	                    background: url('../images/minus.png') no-repeat scroll left center transparent;
                    }

                    div.spoiler div.spoiler-title div.show-icon {
	                    background: url('../images/plus.png') no-repeat scroll left center transparent;
                    }

                    div.spoiler div.spoiler-content {
	                    font-size: 13px;
	                    border: 1px solid #bbbbbb;
	                    border-top: 0px;
	                    -moz-border-radius: 3px;
	                    -webkit-border-radius: 3px;
	                    border-radius: 3px;
	                    background: none repeat scroll 0 0 #F5F5F5;
	                    padding: 4px 10px;
                    }
                                                html::-webkit-scrollbar 
                                                { 
                                                    width: 0; 
                                                }
                                                </style>
                                                <script src=""//ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js""></script>
                                                <script type=""text/javascript"">

                    jQuery(function($) {
                        $('div.spoiler-title').click(function() {
                            $(this)
                                .children()
                                .first()
                                .toggleClass('show-icon')
                                .toggleClass('hide-icon');
                            $(this)
                                .parent().children().last().toggle();
                        });
                    });
                             function GetNavigation(elem) 
                              {
                                   boundAsync.getNavigation(elem.name);
                              }
                            </script>
                            <body>"
                            +
                            val
                            +
                            @"</body>
                            </html>";


            ChromiumBrowser ee = d as ChromiumBrowser;
            ee.LoadHtml(value, "http://www.example.com");
        }

        private static double PercentageToToZoomLevel(int percent)
        {
            return ((percent - 100)) / 25.0;
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e) { }
    }
}
