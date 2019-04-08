using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using VremenaGoda.Controls;

namespace VremenaGoda.Converters
{
    class KeyBoardLanguageConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 1 && values[0] is CultureInfo lang && values[1] is string key)
            {
                if (values[1] == null || ((string)values[1]).ToLower() == "pic") return null;
                return Equals(lang, CultureInfo.GetCultureInfo("ru-RU")) ? (Char.IsUpper(key[0])? _allWord[key.ToLower()].ToUpper():( (key=="?"&& CoolKeyBoard.Shift)? _allWord[key].ToUpper(): _allWord[key])) : (Char.IsUpper(key[0])? key.ToUpper(): key);
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        private readonly Dictionary<string, string> _allWord = new Dictionary<string, string>()
        {
            {"q", "й"},
            {"w", "ц"},
            {"e", "у"},
            {"r", "к"},
            {"t", "е"},
            {"y", "н"},
            {"u", "г"},
            {"i", "ш"},
            {"o", "щ"},
            {"p", "з"},
            {"a", "ф"},
            {"s", "ы"},
            {"d", "в"},
            {"f", "а"},
            {"g", "п"},
            {"h", "р"},
            {"j", "о"},
            {"k", "л"},
            {"l", "д"},
            {"z", "я"},
            {"x", "ч"},
            {"c", "с"},
            {"v", "м"},
            {"b", "и"},
            {"n", "т"},
            {"m", "ь"},
            {"?", "ю"},
            {"@", "@"}
        };
    }
}
