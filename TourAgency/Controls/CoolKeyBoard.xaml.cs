using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using TourAgency.Commands;
using UserControl = System.Windows.Controls.UserControl;

namespace VremenaGoda.Controls
{
    /// <summary>
    /// Логика взаимодействия для CoolKeyBoard.xaml
    /// </summary>
    public partial class CoolKeyBoard : UserControl
    {
        public CoolKeyBoard()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty IsShiftProperty = DependencyProperty.Register(
            "IsShift", typeof(bool), typeof(CoolKeyBoard), new PropertyMetadata(default(bool)));

        public bool IsShift
        {
            get => (bool) GetValue(IsShiftProperty);
            set
            {
                Shift = value;
                SetValue(IsShiftProperty, value);
            }
        }

        public static readonly DependencyProperty ChoosedCultureProperty = DependencyProperty.Register(
            "ChoosedCulture", typeof(CultureInfo), typeof(CoolKeyBoard), new PropertyMetadata(CultureInfo.GetCultureInfo("ru-Ru")));

        public CultureInfo ChoosedCulture
        {
            get => (CultureInfo) GetValue(ChoosedCultureProperty);
            set => SetValue(ChoosedCultureProperty, value);
        }

        private ICommand _switchLanguageCommand;
        private ICommand _sendKeysCommand;
        private ICommand _deleteCommand;

        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(a =>
        {
            Send(Keys.Back, false);
        }));

        public ICommand SwitchLanguageCommand => _switchLanguageCommand ?? (_switchLanguageCommand = new Command(a =>
        {
          ChoosedCulture=  ChoosedCulture == CultureInfo.GetCultureInfo("en-EN")
                ?  CultureInfo.GetCultureInfo("ru-Ru")
                :  CultureInfo.GetCultureInfo("en-En");
        }));

        public ICommand SendKeysCommand => _sendKeysCommand ?? (_sendKeysCommand = new Command(a =>
        {
            if(a is null)
                return;
            if (a is string key)
            {
               key= key.ToLower();
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(ChoosedCulture);
                switch (key)
                {
                    case "!":
                        Send(Keys.D1, true);
                        return;
                    case "?":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("ru-RU"));
                        Send(Keys.D7, true);
                        return;
                    case ".":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("en-EN"));
                        Send(Keys.OemPeriod, false);
                        return;
                    case ",":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("en-EN"));
                        Send(Keys.Oemcomma, false);
                       return;
                    case "поиск":
                        Send(Keys.Enter, false);
                        return;
                    case "search":
                        Send(Keys.Enter, false);
                        return;
                    case "х":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("ru-RU"));
                        Send(Keys.OemOpenBrackets, false);
                        return;
                    case "ж":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("ru-RU"));
                        Send(Keys.OemSemicolon, false);
                        return;
                    case "э":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("ru-RU"));
                        Send(Keys.OemQuotes, false);
                        return;
                    case "б":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("ru-RU"));
                        Send(Keys.Oemcomma, false);
                        return;
                    case "ю":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("ru-RU"));
                        Send(Keys.OemPeriod, false);
                        return;
                    case "@":
                        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("en-EN"));
                        Send(Keys.D2, true);
                        return;
                    case " ":
                        Send(Keys.Space, false);
                        return;
                }
                if (key.ToLower() == "ввод" || key.ToLower() == "enter")
                {
                    Send(Keys.Enter, false);
                    return;
                }
               var Key= Enum.Parse(typeof(Keys), AllWord.Keys.Contains(key)?AllWord[key].ToUpper():key.ToUpper());
                Send((Keys)Key, false);
            }
        }));


        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private static void PressKey(Keys key, bool up)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            if (up)
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            else
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }

        void Send(Keys bukva, bool shift)
        {
            if (shift && !IsShift)
            {
                PressKey(Keys.LShiftKey, false);
                PressKey(bukva, false);
                PressKey(bukva, true);
                PressKey(Keys.LShiftKey, true);
            }
            else
            {
               if(shift)
                PressKey(Keys.LShiftKey, false);
               else if(IsShift && bukva != Keys.D2 && bukva != Keys.D7 && bukva != Keys.D1 && bukva != Keys.Oemcomma && bukva!= Keys.OemPeriod)
                   PressKey(Keys.LShiftKey, false);

                PressKey(bukva, false);
                PressKey(bukva, true);
                if(shift)
                    PressKey(Keys.LShiftKey, true);
                else if (IsShift && bukva != Keys.D2 && bukva != Keys.D7 && bukva != Keys.D1 && bukva != Keys.Oemcomma && bukva != Keys.OemPeriod)
                PressKey(Keys.LShiftKey, true);
            }
        }

        public static bool Shift;

        private readonly Dictionary<string, string> AllWord = new Dictionary<string, string>()
        {
            {"й","q"},
            {"ц","w" },
            {"у","e" },
            {"к","r" },
            {"е","t" },
            {"н","y" },
            {"г","u" },
            {"ш","i" },
            {"щ","o" },
            {"з","p" },
            {"ф","a" },
            {"ы","s" },
            {"в","d" },
            {"а","f" },
            {"п","g" },
            {"р","h" },
            {"о","j" },
            {"л","k" },
            {"д","l" },
            {"я","z" },
            {"ч","x" },
            {"с","c" },
            {"м","v" },
            {"и","b" },
            {"т","n" },
            {"ь","m" },
            {"ю","?" },
            {"!","!" },
       

        };

        private void UIElement_OnTouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if((string) ((System.Windows.Controls.Button)sender).Tag == "pic")
                    SwitchLanguageCommand.Execute(null);
                else
               SendKeysCommand.Execute(((System.Windows.Controls.Button)sender).Content);
            }
            catch { }
        }

        private void DeleteFromTouch(object sender, TouchEventArgs e)
        {
            DeleteCommand.Execute(null);
        }

        private void ShiftTouch(object sender, TouchEventArgs e)
        {
            IsShift = !IsShift;

            var allButtons = FindVisualChildren<System.Windows.Controls.Button>(this);
            allButtons.ToList().ForEach(f =>
            {
                var str = f.Tag?.ToString();
                if (new[] { "х", "э", "б", "ж" }.Contains(f.Content?.ToString().ToLower()))
                {
                    f.Content = IsShift ? f.Content?.ToString().ToUpper() : f.Content?.ToString().ToLower();
                    return;
                }

                if (!string.IsNullOrEmpty(str) && str.ToLower() != "pic")
                {
                    f.Tag = IsShift ? str.ToUpper() : str.ToLower();
                }
            });
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            IsShift = !IsShift;

            var allButtons = FindVisualChildren<System.Windows.Controls.Button>(this);
            allButtons.ToList().ForEach(f =>
            {
                var str = f.Tag?.ToString();
                if (new[] { "х", "э", "б", "ж" }.Contains(f.Content?.ToString().ToLower()))
                {
                    f.Content = IsShift ? f.Content?.ToString().ToUpper() : f.Content?.ToString().ToLower();
                    return;
                }

                if (!string.IsNullOrEmpty(str) && str.ToLower() != "pic")
                {
                    f.Tag = IsShift ? str.ToUpper() : str.ToLower();
                }
            });
        }
    }
}
