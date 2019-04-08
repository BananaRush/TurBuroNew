using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace VSHIM.Control.Handicapped
{
    static class IsHandButton
    {
        private static bool isDoubleTouch;

        public static int HashHandButton { get; set; }

        public static bool IsDoubleTouch
        {
            get
            {
                return isDoubleTouch;
            }
            set
            {
                if(isDoubleTouch != value)
                {
                    isDoubleTouch = value;

                    if(value == false)
                    {
                        HashHandButton = 0;
                    }
                }
            }
        }
    }

    public class HandButton : Button
    {
        public static readonly DependencyProperty IsDoubleTouchProperty =
            DependencyProperty.Register("IsDoubleTouch", typeof(bool), typeof(HandButton));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(HandButton));
        
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register("Colors", typeof(Brush), typeof(HandButton));
        
        public bool IsDoubleTouch
        {
            get
            {
                return (bool)GetValue(IsDoubleTouchProperty);
            }
            set
            {
                SetValue(IsDoubleTouchProperty, value);
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public Brush Colors
        {
            get
            {
                return (Brush)GetValue(ColorsProperty);
            }
            set
            {
                SetValue(ColorsProperty, value);
            }
        }

        protected override void OnClick()
        {
            int Hash = GetHashCode();

            if ((IsDoubleTouch || IsHandButton.IsDoubleTouch) && IsHandButton.HashHandButton != Hash)
            {
                IsHandButton.HashHandButton = Hash;
            }
            else
            {
                if(Command != null)
                {
                    object obj = CommandParameter;
                    Command.Execute(obj);
                    IsHandButton.HashHandButton = 0;
                }
            }
        }
    }
}
