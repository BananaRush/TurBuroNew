using ModelData.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Utilit
{
    static public class CSSButtonStyle
    {
        static public string GetStyle(UIElementModel model)
        {
            string css = "";

            css += $"font-size:{model.FontSize}px;";
            if (!string.IsNullOrEmpty(model.FontFamily))
            {
                css += $"font-family:{model.FontFamily};";
            }

            if (!string.IsNullOrEmpty(model.Background))
            {
                css += $"background-color:{model.Background};";
            }

            if (!string.IsNullOrEmpty(model.ColorText))
            {
                css += $"color:{model.ColorText};";
            }

            return css;
        }
    }
}