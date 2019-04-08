using Client.Utilits;
using Client.View;
using ModelData;
using ModelData.Model.Database;
using ModelData.Utilits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    class PassageImageCast : PassageImage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private const int COUNT_LOAD = 9;
        private int _index = 0;
        ObservableCollection<ImageList> _imageLists = null;
        public bool IsLoadImage { get; set; }
        public ObservableCollection<ImageList> ImageLists
        {
            get => _imageLists ?? new ObservableCollection<ImageList>();
            set
            {
                if (_imageLists != value)
                {
                    _imageLists = value;
                    OnPropertyChanged(nameof(ImageLists));
                }
            }
        }

        public PassageImageCast(PassageImage model)
        {
            Id = model.Id;
            Title = model.Title;
            imageLists = model.imageLists;
            ImageLists = new ObservableCollection<ImageList>();
            AddImages();
        }
 
        public void AddImages()
        {
            IsLoadImage = false;
            if (imageLists != null && imageLists.Any())
            {
                if (imageLists.Count > (COUNT_LOAD + _index))
                {
                    for (int i = _index; i <= (COUNT_LOAD + _index); i++)
                    {
                        ImageLists.Add(imageLists[i]);
                    }
                    _index += COUNT_LOAD + 1;
                }
                else
                {
                    if(_index != 0)
                    {
                        for (int i = _index; i < imageLists.Count; i++)
                        {
                            ImageLists.Add(imageLists[i]);
                        }
                    }
                    else
                    {
                        ImageLists = new ObservableCollection<ImageList>(imageLists);
                    }
                }

                IsLoadImage = imageLists.Count > ImageLists.Count;
            }

            OnPropertyChanged(nameof(IsLoadImage));
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class PassagePageVM : Bandel
    {
        private ObservableCollection<PassageImageCast> _passageList = null;
        private RelayCommand _getImage;
        private RelayCommand _addImage;
        public PassagePageVM()
        {
            GetData();
        }

        public async void GetData()
        {
            if(Explorer.ButtonNav != null)
            {
                List<PassageImage> list1 = await WebApi.PassagePage.Get(Explorer.ButtonNav.Id);
                List<PassageImageCast> list = new List<PassageImageCast>();
                
                if (list1 != null)
                {
                    for (int i = 0; i < list1.Count; i++)
                        list.Add(new PassageImageCast(list1[i]));

                    foreach (var item in list)
                    {
                        foreach(var ite in item.imageLists)
                        {
                            ite.ImgUrl = Config.HostImage + ite.ImgUrl;
                        }
                    }

                    PassageList = new ObservableCollection<PassageImageCast>(list);
                }
            }
        }

        public RelayCommand AddImage
        {
            get
            {
                return _addImage ?? (_addImage = new RelayCommand(obj => 
                {
                    if(obj is int index)
                    {
                        PassageImageCast elm = PassageList.FirstOrDefault(r => r.Id == index);
                        elm?.AddImages();
                    }
                }));
            }
        }
        public RelayCommand GetImage
        {
            get
            {
                return _getImage ?? (_getImage = new RelayCommand(obj => 
                {
                    if(obj is ImageList elm)
                    {

                        PassageImage passageImage = null;

                        foreach(var item in PassageList)
                        {
                            if (item.imageLists.Where(r => r.Id == elm.Id).Any())
                            {
                                passageImage = item;
                                break;
                            }
                        }

                        Explorer.DataImageTuple = new Tuple<PassageImage, ImageList>(passageImage, elm);
                        ImageViewerWindows imageViewerWindows = new ImageViewerWindows();
                        imageViewerWindows.ShowDialog();
                    }
                }));
            }
        }

        public ObservableCollection<PassageImageCast> PassageList
        {
            get => _passageList ?? new ObservableCollection<PassageImageCast>();
            set => SetProperty(ref _passageList, value);
        }
    }
}
