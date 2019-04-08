using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilits.Language
{
    [Serializable]
    public class LangWord : IClientWord
    {
        [NonSerialized]
        private static LangWord instance = new LangWord();

        private LangWord() { }

        public static LangWord Lang
        {
            get
            {
                return instance;
            }
            set
            {
                if (value != null)
                {
                    instance = value;
                }
            }
        }

        public string LangBack { get; set; }
        public string LangLanguage { get; set; }
        public string LangСurrencyСourses { get; set; }
        public string LangNews { get; set; }
        public string LangAllNews { get; set; }
        public string LangLoadMore { get; set; }
        public string LangAccessiblEnvironment { get; set; }
        public string LangObject { get; set; }
        public string LangObject1 { get; set; }
        public string LangTime { get; set; }
        public string LangWeather { get; set; }
        public string LangColorInversion { get; set; }
        public string LangMonochrome { get; set; }
        public string LangDownScreen { get; set; }
        public string LangLoupe { get; set; }
        public string LangObject2 { get; set; }
    }
}
