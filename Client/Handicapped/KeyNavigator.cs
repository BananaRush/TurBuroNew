using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using VSHIM.Control.Handicapped.View;

namespace VSHIM.Control.Handicapped
{
    static class KeyNavigator
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private static Grid MainGrid = null;
        public delegate void SelectObj(object obj, ref bool IsNavigator);
        public static event SelectObj DSelectObj = delegate { };
        private static object FocusElement = null;
        private static TraversalRequest Back = new TraversalRequest(FocusNavigationDirection.Previous);
        private static TraversalRequest Next = new TraversalRequest(FocusNavigationDirection.Next);
        private static bool IsNavigator = false;
        private static int ElementHash = 0;

        static KeyNavigator()
        {

        }

        public static void SetKeyNavigator(Grid grid)
        {
            MainGrid = grid;
            // Регестрируем событие фокуса элемента
            MainGrid.AddHandler(Keyboard.GotKeyboardFocusEvent, new RoutedEventHandler(SelectObject));
        }

        private static void SelectObject(object sender, RoutedEventArgs e)
        {
            FocusElement = Keyboard.FocusedElement;
            ElementHash = FocusElement.GetHashCode();
            DSelectObj(FocusElement, ref IsNavigator);
        }

        public static void NavBack()
        {
            UIElement uIElement = FocusElement as UIElement;

            if(uIElement != null)
            {
                IsNavigator = true;
                uIElement.MoveFocus(Back);
            }
        }

        public static void NavNext()
        {
            IsNavigator = true;
            PressKey(Keys.Tab);
        }

        public static void Enter()
        {
            if(ElementHash != 0 && ElementHash == FocusElement.GetHashCode())
            {
                IsNavigator = true;
                DSelectObj(FocusElement, ref IsNavigator);

                if(FocusElement is System.Windows.Controls.Primitives.ButtonBase)
                {
                    ElementHash = 0;
                }
            }
            else
            {
                System.Windows.Controls.Primitives.ButtonBase buttonBase = FocusElement as System.Windows.Controls.Primitives.ButtonBase;

                if(buttonBase == null)
                {
                    return;
                }

                if (buttonBase.Command != null)
                {
                    object obj = buttonBase.CommandParameter;
                    buttonBase.Command.Execute(obj);
                }
            }

        }

        private static void PressKey(Keys key)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
        }

        private static void PlaySound()
        {
            DSelectObj(FocusElement, ref IsNavigator);
        }
    }
}
