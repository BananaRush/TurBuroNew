using System;
using System.Text.RegularExpressions;


namespace ModelData.Utilits
{
    public class HtmlClear
    {
        public static string ClearHtml(string text)
        {
            try
            {
                return Regex.Replace(text, "<[^>]+>", string.Empty)
                .Replace("&nbsp;", " ")
                .Replace("&ensp;", " ")
                .Replace("&emsp;", " ")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&laquo;", "")
                .Replace("&raquo;", "")
                .Replace("&ndash;", "")
                .Replace("&mdash;", "-");
            }
            catch(Exception e)
            {
                return text;
            }

        }
    }
}
