using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.Model
{
    class CarouselItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Dictionary<int, string> monthsByKey = new Dictionary<int, string>()
        {
            { 1, "января" }, { 2, "февраля" }, { 3, "марта" }, { 4, "апреля" },
            { 5, "мая" }, { 6, "июня" }, { 7, "июля" }, { 8, "августа" },
            { 9, "сентября" }, { 10, "октября" }, { 11, "ноября" }, { 12, "декабря" }
        };

        private string _url;

        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        private string _Heading;
        public string Heading
        {
            get { return _Heading; }
            set
            {
                _Heading = value;
                OnPropertyChanged("Heading");
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _PhotoString;
        public string PhotoString
        {
            get { return _PhotoString; }
            set
            {
                _PhotoString = value;
                OnPropertyChanged("PhotoString");
            }
        }

        private string _PostDate;
        public string PostDate
        {
            get { return _PostDate; }
            set
            {
                _PostDate = value;
                OnPropertyChanged("PostDate");
            }
        }

        private object _NavigateObject;
        public object NavigateObject
        {
            get { return _NavigateObject; }
            set
            {
                _NavigateObject = value;
                OnPropertyChanged("NavigateObject");
            }
        }

        private string _PdfUri;
        public string PdfUri
        {
            get { return _PdfUri; }
            set
            {
                _PdfUri = value;
                OnPropertyChanged("PdfUri");
            }
        }

        private Uri _VideoUri;
        public Uri VideoUri
        {
            get { return _VideoUri; }
            set
            {
                _VideoUri = value;
                OnPropertyChanged("VideoUri");
            }
        }
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
            }
        }

        public CarouselItemModel(string heading, string description, string photo, string postdate, string url)
        {
            Type = "Image";
            this.Heading = heading;
            this.Description = description;
            this.PhotoString = photo;
            this.PostDate = postdate;
            this.Url = url;
        }

        public CarouselItemModel(string pdfUri)
        {
            Type = "Pdf";
            this.PdfUri = pdfUri;
        }

        public CarouselItemModel(String videoUri, string heading)
        {
            Type = "Video";
            this.VideoUri = new Uri(videoUri);
            this.Heading = heading;
        }
    }
}
