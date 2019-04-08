using Client.Utilits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls.Still
{
    public class StillControlVM : Bandel
    {
        private bool isVisibility = true;

        public StillControlVM()
        {

        }

        public bool IsVisibility
        {
            get => isVisibility;
            set => SetProperty(ref isVisibility, value);
        }
    }
}
