using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSHIM.Control.WebControl;

namespace Client.Controls.WebControl
{
    public class WebBrowserUrl : ChromiumWebBrowser
    {
        public WebBrowserUrl() : base()
        {
            this.RequestHandler = new BrowserRequestHandler();
        }
    }
}
