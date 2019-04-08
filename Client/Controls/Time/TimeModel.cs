using Client.Utilits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls.Time
{
    public class TimeModel : Bandel
    {
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public TimeModel(string name, int diff)
        {
            this.CityName = name.ToUpper();
            this.HourDifference = diff;
        }
    }
}
