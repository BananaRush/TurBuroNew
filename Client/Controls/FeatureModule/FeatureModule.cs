using Client.Controls.FeatureModule;
using Client.Utilits;
using System;
using Xceed.Wpf.Toolkit;

namespace Client.Controls
{
    public static class FeatureModuleManager
    {
        public static event Action IsMagnifier = delegate { };
        public static event Action OffssetTop = delegate { };
        public static void ColorInversion(int type)
        {
            if (type == 1)
                InversionColor.OnInversion(FeatureModule.ColorInversion.Inverted);

            if (type == 2)
                InversionColor.OnInversion(FeatureModule.ColorInversion.GreyScale);
        }

        public static void OnEventIsMagnifier()
        {
            IsMagnifier();
        }

        public static void OnEventOffssetTop()
        {
            OffssetTop();
        }
    }
}
