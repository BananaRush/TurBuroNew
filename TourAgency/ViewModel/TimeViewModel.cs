using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TourAgency.Model;

namespace TourAgency.ViewModel
{
    class TimeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<TimeModel> timeCities { get; set; }
        private DispatcherTimer timer = new DispatcherTimer();
        private TimeModel _SpbTime;
        public TimeModel SpbTime
        {
            get { return _SpbTime; }
            set
            {
                _SpbTime = value;
            }
        }

        public TimeViewModel()
        {
            timeCities = new ObservableCollection<TimeModel>();
            InitTimeZones();
            GetTime();
            timer.Interval = new TimeSpan(0, 0, 61 - DateTime.Now.Second);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Interval = new TimeSpan(0, 1, 0);
            GetTime();
        }

        private void InitTimeZones()
        {
            SpbTime = new TimeModel("Петербург", 0);
            timeCities.Add(new TimeModel("Лондон", -4));
            timeCities.Add(new TimeModel("Шанхай", 5));
            timeCities.Add(new TimeModel("Москва", 0));
            timeCities.Add(new TimeModel("Нью-Йорк", -8));
        }

        private void GetTime()
        {
            DateTime now = DateTime.Now;
            SpbTime.TimeString = now.ToString("HH:mm");
            foreach (var item in timeCities)
            {
                item.TimeString = $"{now.AddHours(item.HourDifference):HH:mm}";
            }
        }
    }
}
