using Client.Utilits;
using ModelData.Model.Database;
using System;
using System.Collections.Generic;

namespace Client.ViewModel
{
    class ImageViewerWindowsVM : Bandel
    {
        private RelayCommand _imgBack;
        private RelayCommand _imgNext;
        private RelayCommand _closed;
        private Tuple<PassageImage, ImageList> _data = null;
        private ImageList _privewer = null;
        private string _image = string.Empty;

        public bool IsBack
        {
            get
            {
                int index = _data.Item1.imageLists.FindIndex(r => r.Id == _privewer.Id);
                if (index > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsNext
        {
            get
            {
                int index = _data.Item1.imageLists.FindIndex(r => r.Id == _privewer.Id);
                if (index < _data.Item1.imageLists.Count - 1)
                {
                    return true;
                }
                return false;
            }
        }

        public ImageViewerWindowsVM()
        {
            _data = Explorer.DataImageTuple;
            if (IsChackData())
            {
                _privewer = _data.Item2;
                Image = _privewer.ImgUrl;
                Update();
            }
        }

        public RelayCommand ImgBack
        {
            get
            {
                return _imgBack ?? (_imgBack = new RelayCommand(() => 
                {
                    if(!IsChackData())
                        return;

                    int index = _data.Item1.imageLists.FindIndex(r=>r.Id == _privewer.Id);
                    if(index > 0)
                    {
                        --index;
                        _privewer = _data.Item1.imageLists[index];
                        Image = _privewer.ImgUrl;
                    }
                    Update();
                }));
            }
        }

        public RelayCommand ImgNext
        {
            get
            {
                return _imgNext ?? (_imgNext = new RelayCommand(() => 
                {
                    if (!IsChackData())
                        return;

                    int index = _data.Item1.imageLists.FindIndex(r => r.Id == _privewer.Id);
                    if (index < _data.Item1.imageLists.Count - 1)
                    {
                        ++index;
                        _privewer = _data.Item1.imageLists[index];
                        Image = _privewer.ImgUrl;
                    }
                    Update();
                }));
            }
        }

        public RelayCommand Closed
        {
            get
            {
                return _closed ?? (_closed = new RelayCommand(obj => 
                {
                    if(obj is System.Windows.Window win)
                    {
                        win.Close();
                    }
                }));
            }
        }

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private void Update()
        {
            OnPropertyChanged(nameof(IsBack));
            OnPropertyChanged(nameof(IsNext));
        }
        private bool IsChackData()
        {
            return _data != null && _data.Item1 != null && _data.Item2 != null;
        }
    }
}
