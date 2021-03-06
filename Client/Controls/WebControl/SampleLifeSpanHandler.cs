﻿using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHIM.Control.WebControl
{
    public class SampleLifeSpanHandler : ILifeSpanHandler
    {
        public void OnBeforeClose(IWebBrowser browser)
        {

        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
          
        }

        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
     
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            browserControl.Load(targetUrl);
            return true;
        }
    }
}
