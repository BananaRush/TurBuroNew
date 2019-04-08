using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VisitAPI.Model
{
    public class EventsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _image = string.Empty;

        public int Id { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Image
        {
            get => _image;
            set
            {
                if(_image != value)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Important { get; set; }
        public string Created { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
