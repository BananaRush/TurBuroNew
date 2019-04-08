using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency
{
    public class MainSettings
    {
        public enum Language
        {
            Russian,
            English,
            Chinese
        }
        
        private static MainSettings _current;

        public static MainSettings Current
        {
            get => _current ?? (_current = new MainSettings());
            set => _current = value;
        }

        public Language CurrentLang = Language.Russian;

    }
}
