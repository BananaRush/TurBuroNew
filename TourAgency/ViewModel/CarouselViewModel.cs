using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TourAgency.Model;
using TourAgency.Model.ModelWebApi;
using TourAgency.Utilities;
using TourAgencyAdmin.Utilities;

namespace TourAgency.ViewModel
{
    class CarouselViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<CarouselItemModel> Items { get; set; }

        public CarouselViewModel()
        {
            Items = new ObservableCollection<CarouselItemModel>();
            MakeModelSlider();
            //Items.Add(new CarouselItemModel("«Реал» в финале", "Мадридский «Реал» вышел в финал Клубного Чемпионата Мира",
            //                                "../Images/CarouselImages/madrid.jpg", new DateTime(2017, 12, 14)));
            //Items.Add(new CarouselItemModel("Сборная Россия не вышла из группы", "Уступив сборной Мексики (3:1), сборная России покидает Кубок Конфедераций", "../Images/carousel-dummy.jpg",
            //                                new DateTime(2017, 06, 30)));
            //Items.Add(new CarouselItemModel($"file:///{Directory.GetCurrentDirectory()}/Content/Videos/3promo_en_no_voice0001.mp4", " "));
            //Items.Add(new CarouselItemModel($"file:///{Directory.GetCurrentDirectory()}/Content/Videos/2.mp4", " "));
        }

        private async void  MakeModelSlider()
        {
            try
            {
                var allSlide = await WebApi.Slider.Get(1000, 0);
                if (allSlide == null)
                    return;
                foreach (var slide in allSlide)
                {
                    if (slide.ContentType == SliderContentType.Image)
                    {
                        var caption = JsonConvert.DeserializeObject<ModelSliderContent>(slide.Caption);
                        if (!File.Exists($"Content/{slide.Content}"))
                            await DownloadFile(slide.Content);
                        Items.Add(new CarouselItemModel(caption.BoldText, caption.SmallText,
                            Path.GetFullPath($"Content/{slide.Content}"), caption.AddedText, caption.Url));
                    }
                    if (slide.ContentType == SliderContentType.Video)
                    {
                        if (!File.Exists($"Content/{slide.Content}"))
                            await DownloadFile(slide.Content);
                        Items.Insert(0, new CarouselItemModel(Path.GetFullPath($"Content/{slide.Content}"), " "));

                    }
                }
            }
            catch 
            {
                
            }
        }

        private async Task DownloadFile(string file)
        {
            await Task.Run(() =>
            {
                try
                {
                    Session.CurrentSession.IsLoading = true;
                using (WebClient wc =
                    new WebClient() {Credentials = new NetworkCredential(Session.FtpUser, Session.FtpPass)})
                {
                    wc.DownloadFile(new Uri($"ftp://{Session.FtpServ}/{file}"), $"Content/{file}");
                }
                    Session.CurrentSession.IsLoading = false;
                }
                catch (Exception e)
                {
                    Session.CurrentSession.IsLoading = false;
                }
            });
        }
    }
}
