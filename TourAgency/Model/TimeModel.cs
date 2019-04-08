using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.Model
{
    class TimeModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _CityName;
        public string CityName
        {
            get
            {
                return _CityName;
            }
            set
            {
                _CityName = value;
                OnPropertyChanged("CityName");
            }
        }

        private int _HourDifference;
        public int HourDifference
        {
            get
            {
                return _HourDifference;
            }
            set
            {
                _HourDifference = value;
                OnPropertyChanged("HourDifference");
            }
        }

        private string _TimeString;
        public string TimeString
        {
            get
            {
                return _TimeString;
            }
            set
            {
                _TimeString = value;
                OnPropertyChanged("TimeString");
            }
        }

        public TimeModel(string name, int diff)
        {
            this.CityName = name.ToUpper();
            this.HourDifference = diff;
        }
    }
}
