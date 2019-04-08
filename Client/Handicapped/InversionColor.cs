using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static VSHIM.Control.Handicapped.NativeMethods;

namespace VSHIM.Control.Handicapped
{
	
	public enum ColorInversion : int
    {
        [HandColor("Инверсия цвета", "#ff9c00")]
        Inverted = 1,

        [HandColor("Оттенки серого", "#a2a2a2")]
        GreyScale,

        [HandColor("Оттенок черного", "#303030")]
        GreyInvert,

        [HandColor("Протанопия", "#8f8222")]
        Protanopia,

        [HandColor("Протаномалия", "#b75315")]
        Protanomaly,

        [HandColor("Дейтеранопия", "#a17b00")]
        Deuteranopia,

        [HandColor("Дайтреномалия", "#c25101")]
        Deuteranomaly,

        [HandColor("Тританопия", "#ff1901")]
        Tritanopia,

        [HandColor("Тританомалия", "#fe1200")]
        Tritanomaly,

        [HandColor("Ахроматопсия", "#525252")]
        Achromatopsia,

        [HandColor("Ахроматомалия", "#9F282A")]
        Achromatomaly
    }

	

    public static class InversionColor
    {
        // Конструктор
        static InversionColor()
        {
          
        }

        public static void OffInversion()
        {
            MagUninitialize();
        }

        public static void OnInversion(ModelDate.Model.ColorInversion colorInversion)
        {
            OffInversion();

            MAGCOLOREFFECT magEffectInvert = GetColor(colorInversion);
            if(magEffectInvert.transform != null && magEffectInvert.transform.Count() != 0)
            {
                MagInitialize();
                SetMagnificationDesktopColorEffect(ref magEffectInvert);
            }
        }

        private static MAGCOLOREFFECT GetColor(ModelDate.Model.ColorInversion colorInversion)
        {
            if(colorInversion == ModelDate.Model.ColorInversion.Inverted)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.GreyScale)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.GreyInvert)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Protanopia)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Protanomaly)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Deuteranopia)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Deuteranomaly)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Tritanopia)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Tritanomaly)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Achromatopsia)
            {
                return new MAGCOLOREFFECT
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

            if (colorInversion == ModelDate.Model.ColorInversion.Achromatomaly)
            {
                return new MAGCOLOREFFECT
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

            return new MAGCOLOREFFECT();
        }
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
}
