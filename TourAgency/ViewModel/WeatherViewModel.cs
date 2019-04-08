using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourAgency.Model;

namespace TourAgency.ViewModel
{
    class WeatherViewModel : INotifyPropertyChanged
    {
        private const string APPID = "f9ef2fb557ae6dfe75dde04566e5ff9f";
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Dictionary<int, string> otherCities = new Dictionary<int, string>()
        {
            { 524901, "МСК" }, { 1486209, "ЕКБ" }
        };

        private WeatherRoot weatherRoot = new WeatherRoot();
        public WeatherModel spbWeather { get; set; }
        public ObservableCollection<WeatherModel> otherCitiesWeather { get; set; }

        public WeatherViewModel()
        {
            spbWeather = new WeatherModel();
            otherCitiesWeather = new ObservableCollection<WeatherModel>();
            try
            {
                spbWeather = GetWeather(498817, "СПБ");
                foreach (var item in otherCities)
                {
                    otherCitiesWeather.Add(GetWeather(item.Key, item.Value));
                }
            }
            catch (WebException ex)
            {
                
            }
            catch (Exception ex)
            {
               
            }
        }

        private WeatherModel GetWeather(int cityID, string shortname)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://api.openweathermap.org/data/2.5/forecast?id=" + cityID +"&APPID=" + APPID);
                weatherRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherRoot>(json);
            }

            WeatherModel weather = new WeatherModel();
            weather.DayTemperature = (int)Math.Round(weatherRoot.list[0].main.temp - 273.15);
            weather.NightTemperature = (int)Math.Round(weatherRoot.list[3].main.temp - 273.15);
            weather.DayIcon = "../Images/WeatherIcons/" + (cityID == 498817 ? "White/" : "") + GetWeatherIcon(weatherRoot.list[0].weather[0].id, weatherRoot.list[0].dt) + ".png";
            weather.NightIcon = "../Images/WeatherIcons/" + (cityID == 498817 ? "White/" : "") + GetWeatherIcon(weatherRoot.list[3].weather[0].id, weatherRoot.list[3].dt) + ".png";
            weather.CityShortName = shortname;
            return weather;
        }

        private string GetWeatherIcon(int id, int dt)
        {
            if((id>=200 && id<=202) || (id>=230 && id<=232))
            {
                return "thunder-rain";
            }
            else if(id>=210 && id<=221)
            {
                return "thunder";
            }
            else if(id>=300 && id<=531)
            {
                return "rain";
            }
            else if(id>=600 && id<=622)
            {
                return "snow";
            }
            else if(id>=701 && id<=781)
            {
                return "mist";
            }
            else if(id==800)
            {
                DateTime temp = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                temp = temp.AddSeconds(dt);
                if(temp.Hour>=7 && temp.Hour <= 20)
                {
                    return "sunny-day";
                }
                else
                {
                    return "clear-night";
                }
            }
            else if(id==801)
            {
                return "few-clouds";
            }
            else if(id==802)
            {
                return "clouds";
            }
            else if(id>=803 && id<=804)
            {
                return "broken-clouds";
            }
            else
            {
                return null;
            }
        }
    }
}
