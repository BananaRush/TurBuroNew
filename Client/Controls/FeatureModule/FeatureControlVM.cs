using Client.Utilits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls.FeatureModule
{
    class FeatureControlVM : Bandel
    {
        private RelayCommand _colorInversion;
        private RelayCommand _topOffset;
        private RelayCommand _magnifier;
        public FeatureControlVM()
        {

        }
        public RelayCommand Magnifier
        {
            get
            {
                return _magnifier ?? (_magnifier = new RelayCommand(() => 
                {
                    FeatureModuleManager.OnEventIsMagnifier();
                }));
            }
        }
        public RelayCommand TopOffset
        {
            get
            {
                return _topOffset ?? (_topOffset = new RelayCommand(() => 
                {
                    FeatureModuleManager.OnEventOffssetTop();
                }));
            }
        }

        public RelayCommand ColorInversion
        {
            get
            {
                return _colorInversion ?? (_colorInversion = new RelayCommand(obj =>
                {
                    int type = Convert.ToInt32(obj);
                    FeatureModuleManager.ColorInversion(type);
                }));
            }
        }
    }
}
