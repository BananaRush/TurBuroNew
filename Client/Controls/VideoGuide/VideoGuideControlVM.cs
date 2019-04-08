using Client.Utilits;
using ModelData;
using ModelData.Utilits;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.Controls.VideoGuide
{
    class VideoGuideControlVM : Bandel
    {
        private RelayCommand _mediaElementLoad;
        private bool _isVisibility;
        private MediaElement _mediaElement = null;
        private string _videoPath = string.Empty;

        public VideoGuideControlVM()
        {
#if DEBUG
            IsVisibility = true;
#endif
        }

        public async void GetData()
        {
            string fileName = await WebApi.Video.GetVideoGuide();
            VideoPath = Path.Combine(AppContext.BaseDirectory, Config.GetFileDirectory(), Path.GetFileName(fileName));
            if (File.Exists(VideoPath) && _mediaElement != null)
                _mediaElement.Play();
            else
                IsVisibility = false;
        }

        public bool IsVisibility
        {
            get => _isVisibility;
            set
            {
                SetProperty(ref _isVisibility, value);
                if (value)
                    _mediaElement?.Play();
                else
                    _mediaElement?.Stop();
            } 
        }

        public string VideoPath
        {
            get => _videoPath;
            set => SetProperty(ref _videoPath, value);
        }

        public RelayCommand MediaElementLoad
        {
            get
            {
                return _mediaElementLoad ?? (_mediaElementLoad = new RelayCommand(obj =>
                {
                    if (obj is MediaElement element)
                    {
                        _mediaElement = element;
                        GetData();
                    }
                }));
            }
        }
    }
}
