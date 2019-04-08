using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAgency.Annotations;

namespace TourAgencyAdmin.Utilities
{
  public class ModelSliderContent: INotifyPropertyChanged
  {
         public string BoldText { get; set; }
        public string SmallText { get; set; }
        public string AddedText { get; set; }
        public string Url { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
