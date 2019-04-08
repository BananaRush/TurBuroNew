using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls.FeatureModule
{
    public enum ColorInversion : int
    {
        Non = -1,

        [Display(Name = "Инверсия цвета")]
        Inverted = 1,

        [Display(Name = "Оттенки серого")]
        GreyScale,

        [Display(Name = "Оттенок черного")]
        GreyInvert,

        [Display(Name = "Протанопия")]
        Protanopia,

        [Display(Name = "Протаномалия")]
        Protanomaly,

        [Display(Name = "Дейтеранопия")]
        Deuteranopia,

        [Display(Name = "Дайтреномалия")]
        Deuteranomaly,

        [Display(Name = "Тританопия")]
        Tritanopia,

        [Display(Name = "Тританомалия")]
        Tritanomaly,

        [Display(Name = "Ахроматопсия")]
        Achromatopsia,

        [Display(Name = "Ахроматомалия")]
        Achromatomaly
    }

    public static class NativeMethods
    {
        const string Magnification = "Magnification.dll";

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagInitialize();

        [DllImport(Magnification, ExactSpelling = true, SetLastError = true)]
        public static extern bool MagUninitialize();

        public struct MAGCOLOREFFECT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public float[] transform;
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool SetMagnificationDesktopColorEffect(ref MAGCOLOREFFECT pEffect);
    }

    public static class InversionColor
    {
        private static ColorInversion _colorInversion = ColorInversion.Non;
        public static bool IsInversionColor { get; set; }
        // Конструктор
        static InversionColor()
        {

        }

        public static void OffInversion()
        {
            NativeMethods.MagUninitialize();
            IsInversionColor = false;
        }

        public static void OnInversion(ColorInversion colorInversion)
        {
            OffInversion();
            if(_colorInversion != colorInversion)
            {
                NativeMethods.MAGCOLOREFFECT magEffectInvert = GetColor(colorInversion);
                if (magEffectInvert.transform != null && magEffectInvert.transform.Count() != 0)
                {
                    NativeMethods.MagInitialize();
                    NativeMethods.SetMagnificationDesktopColorEffect(ref magEffectInvert);
                }
                _colorInversion = colorInversion;
                IsInversionColor = true;
            }
            else
            {
                _colorInversion = ColorInversion.Non;
            }
        }

        private static NativeMethods.MAGCOLOREFFECT GetColor(ColorInversion colorInversion)
        {
            if (colorInversion == ColorInversion.Inverted)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        -1f, 0.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, -1f, 0.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, -1f, 0.0f, 0.0f,
                        0.0f, 0.0f, 0.0f, 1f, 0.0f,
                        1f, 1f, 1f, 0.0f, 1f,
                    }
                };
            }

            if (colorInversion == ColorInversion.GreyScale)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                      0.3f,  0.3f,  0.3f,  0.0f,  0.0f,
                      0.6f,  0.6f,  0.6f,  0.0f,  0.0f,
                      0.1f,  0.1f,  0.1f,  0.0f,  0.0f,
                      0.0f,  0.0f,  0.0f,  1.0f,  0.0f,
                      0.0f,  0.0f,  0.0f,  0.0f,  1.0f
                    }
                };
            }

            if (colorInversion == ColorInversion.GreyInvert)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        -0.3f, -0.3f, -0.3f, 0.0f, 0.0f,
                        -0.59f, -0.59f, -0.59f, 0.0f, 0.0f,
                        -0.11f,-0.11f,-0.11f,0.0f,0.0f,
                        0.0f, 0.0f, 0.0f, 1f, 0.0f,
                        1f, 1f, 1f, 0.0f, 1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Protanopia)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.567f,0.433f,0f,0f,0f,
                        0.558f,0.442f,0f,0f,0f,
                        0f,0.242f,0.758f,0f,0f,
                        0f,0f,0f,1f,0f,
                        0f,0f,0f,0f,1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Protanomaly)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.817f,0.183f,0f,0f,0f,
                        0.333f,0.667f,0f,0f,0f,
                        0f,0.125f,0.875f,0f,0f,
                        0f,0f,0f,1f,0f,
                        0f,0f,0f,0f,1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Deuteranopia)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.625f,0.375f,0f,0f,0f,
                        0.7f,0.3f,0f,0f,0f,
                        0f,0.3f,0.7f,0f,0f,
                        0f,0f,0f,1f,0f,
                        0f,0f,0f,0f,1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Deuteranomaly)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.8f,0.2f,0f,0f,0f,
                        0.258f,0.742f,0f,0f,0f,
                        0f,0.142f,0.858f,0f,0f,
                        0f,0f,0f,1f,0f,
                        0f,0f,0f,0f,1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Tritanopia)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.95f,0.05f,0f,0f,0f,
                        0f,0.433f,0.567f,0f,0f,
                        0f,0.475f,0.525f,0f,0f,
                        0f,0f,0f,1f,0f,
                        0f,0f,0f,0f,1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Tritanomaly)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.967f,0.033f,0f,0f,0f,
                        0f,0.733f,0.267f,0f,0f,
                        0f,0.183f,0.817f,0f,0f,
                        0f,0f,0f,1f,0f,
                        0f,0f,0f,0f,1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Achromatopsia)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.299f,0.587f,0.114f,0f,0f,
                        0.299f,0.587f,0.114f,0f,0f,
                        0.299f,0.587f,0.114f,0f,0f,
                        0f,0f,0f,1f,0f,
                        0.0f, 0.0f, 0.0f, 0.0f, 1f
                    }
                };
            }

            if (colorInversion == ColorInversion.Achromatomaly)
            {
                return new NativeMethods.MAGCOLOREFFECT
                {
                    transform = new[]
                    {
                        0.618f, 0.320f, 0.062f, 0f, 0f,
                        0.163f, 0.775f, 0.062f, 0f, 0f,
                        0.163f, 0.320f, 0.516f, 0f, 0f,
                        0f,0f,0f,1f,0f,
                        0.0f, 0.0f, 0.0f, 0.0f, 1f
                    }
                };
            }

            return new NativeMethods.MAGCOLOREFFECT();
        }
    }
}
