using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHIM.Control.WebControl
{
    static class UrlParse
    {
        public static bool UrlParsr(string OldUrl, string NewUrl)
        {
            Uri oldUri = new Uri(OldUrl);
            Uri newUri = new Uri(NewUrl);
            OldUrl = oldUri.Host;
            NewUrl = newUri.Host;

            if (OldUrl == NewUrl)
                return true;

            return false;
        }
    }
}
