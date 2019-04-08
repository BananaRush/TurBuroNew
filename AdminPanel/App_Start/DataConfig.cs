using ModelData.Interface;
using ModelData.Models.Database;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.App_Start
{
    public static class DataConfig
    {
        public static LanguageModel Language { get; set; }
        public static List<TerminalsModel> Terminals { get; set; }

        public static readonly Func<NewsModel, bool> SortData = (model) =>
        {
            bool mod = model.Language == Language?.CodeLang;

            return mod;
        };

        public static readonly Func<UIElementModel, bool> SortDataS = (model) =>
        {
            bool mod = model.Language == Language?.CodeLang;

            return mod;
        };
    }
}