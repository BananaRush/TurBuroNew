using Client.Utilits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls.Language
{
    class LanguageControlVM : Bandel
    {
        private RelayCommand _setLang;

        public LanguageControlVM()
        {

        }

        public RelayCommand SetLang
        {
            get
            {
                return _setLang ?? (_setLang = new RelayCommand(obj => 
                {
                    if(obj is string lang)
                    {
                        if(App.DataCom.Language != lang.ToUpper())
                        {
                            App.DataCom.Language = lang.ToUpper();
                            // Уведомляем что смена произошла
                            App.LangAction();
                        }
                    }
                }));
            }
        }
    }
}
