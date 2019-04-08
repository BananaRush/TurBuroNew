using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Utilits
{
    public static class HtmlEventTemplate
    {
        public static string Get(string title, string img, string info)
        {
            string template = "<style>html { padding: 18px; } h3{ font-size:30px; } p{ font-size:30px; }</style><h1 style='text-align:center; font-size:42px;'>@title@</h1><img style='display: block; margin: 0 auto;max-height: 40%;' src='@img@'/><div>@info@</div>";

            return template
                .Replace("@title@", title)
                .Replace("@img@", img)
                .Replace("@info@", info);
        }
    }
}
