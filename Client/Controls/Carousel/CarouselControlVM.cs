using Client.Utilits;
using ModelData;
using ModelData.Utilits;
using StorageAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using model = StorageAPI.Models.Database;

namespace Client.Controls.Carousel
{
    public class ItemSlider : IEnumerator
    {
        private List<model.Slider>  _list = null;
        int position = 0;
        public event Action<model.Slider, int> SetElement = delegate { };
        Timer _nextTimer = null;

        public ItemSlider(List<model.Slider> list)
        {
            _list = list;
            _nextTimer = new Timer();
            _nextTimer.Elapsed += _nextTimer_Elapsed;
        }

        public List<model.Slider> GetList
        {
            get => _list;
            set
            {
                _list = value;
                position = 0;
            } 
        }

        private void _nextTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                MoveNext();
            });
        }

        public bool MoveNext()
        {
            _nextTimer.Stop();
            position++;
            if(position >= _list.Count)
            {
                position = 0;
            }

            Timer();

            return true;
        }

        public void MoveBack()
        {
            _nextTimer.Stop();
            position--;
            if (position <= -1)
            {
                position = _list.Count -1;
            }

            Timer();
        }

        public void Reset()
        {
            position = 0;
        }

        public void GetElement()
        {
            if(_list != null)
            {
                Timer();
            }
        }

        private void Timer()
        {
            SetElement(Current, position);

            //if(Current.ContentType == SliderContentType.Image)
            //{
                if (Current.Timeout != 0)
                {
                    _nextTimer.Interval = TimeSpan.FromSeconds(Current.Timeout).TotalMilliseconds;
                    _nextTimer.Start();
                }
            //}
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public model.Slider Current
        {
            get
            {
                try
                {
                    return _list[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public int Count()
        {
            return _list.Count;
        }
    }

    public class CarouselControlVM : Bandel
    {
        private RelayCommand _back;
        private RelayCommand _next;
        private RelayCommand _getElement;
        private object _viewer = null;
        private ObservableCollection<bool> _checkList = null;
        private MediaElement _mediaElement = null;
        private ItemSlider _itemSlider = null;  
        private DispatcherTimer _updateTimer = new DispatcherTimer();

        public CarouselControlVM()
        {
            _mediaElement = new MediaElement();
            _mediaElement.VerticalAlignment = VerticalAlignment.Stretch;
            _mediaElement.HorizontalAlignment = HorizontalAlignment.Stretch;
            _mediaElement.LoadedBehavior = MediaState.Manual;
            _mediaElement.UnloadedBehavior = MediaState.Manual;
            _mediaElement.MediaEnded += MediaElement_MediaEnded;
            _updateTimer.Tick += new EventHandler(UpdateTimerTick);
            _updateTimer.Interval = new TimeSpan(0, 0, TimoutUp.UP_SLIDER_MINUTE, 0);
            _updateTimer.Start();
            UpdateTimerTick(_updateTimer, null);
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => GetData());
        }

        public async void GetData()
        {
            List<model.Slider> list = await WebApi.Carousel.GetSliders();
            list = list.Where(r=> File.Exists(Path.Combine(Config.GetFileDirectory(), Path.GetFileName(r.Content)))).OrderBy(r => r.Number).ToList();
            if (list != null && list.Any())
            {
                if(_itemSlider == null)
                {
                    _itemSlider = new ItemSlider(list);
                    List<bool> list1 = Enumerable.Range(0, _itemSlider.Count()).Select(x => false).ToList();
                    CheckList = new ObservableCollection<bool>(list1);
                    _itemSlider.SetElement += PeopleEnum_SetElement;
                    _itemSlider.GetElement();
                }
                else
                {
                    if(list.Count != _itemSlider.GetList.Count)
                    {
                        // ЗАВЕРШАЕМСЯ
                        __setListSlider();
                    }
                    else
                    {
                        for(int i = 0; i < list.Count; i++)
                        {
                            if(!list[i].Expance(_itemSlider.GetList[i]))
                            {
                                __setListSlider();
                                break;
                            }
                        }
                    }
                }

                void __setListSlider(){
                    _itemSlider.GetList = list;
                    List<bool> list1 = Enumerable.Range(0, _itemSlider.Count()).Select(x => false).ToList();
                    CheckList = new ObservableCollection<bool>(list1);
                    _itemSlider.GetElement();
                }
            }
        }

        private void PeopleEnum_SetElement(model.Slider obj, int index)
        {
            //Viewer = new Label() { Content = obj.Caption };
            _mediaElement.Stop();
            if (obj.ContentType == SliderContentType.Image)
            {
                Image image = new Image();
                image.VerticalAlignment = VerticalAlignment.Stretch;
                image.HorizontalAlignment = HorizontalAlignment.Stretch;
                image.Stretch = System.Windows.Media.Stretch.Fill;
                image.Source = new BitmapImage(new Uri(obj.Content));
                Viewer = image;
            }

            if(obj.ContentType == SliderContentType.Video)
            {
                _mediaElement.Source = new Uri(obj.Content);
                Viewer = _mediaElement;
                _mediaElement.Play();
            }

            // Установка чексов
            for (int i = 0; i < CheckList.Count; i++)
                CheckList[i] = false;
            CheckList[index] = true;
        }

        // Если видос кончился переключаемся
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Next.Execute(null);
        }

        public RelayCommand Back
        {
            get
            {
                return _back ?? (_back = new RelayCommand(() => 
                {
                    _itemSlider?.MoveBack();
                }));
            }
        }

        public RelayCommand Next
        {
            get
            {
                return _next ?? (_next = new RelayCommand(() => 
                {
                    _itemSlider?.MoveNext();
                }));
            }
        }

        public RelayCommand GetElement
        {
            get
            {
                return _getElement ?? (_getElement = new RelayCommand(() => { }));
            }
        }

        public object Viewer
        {
            get => _viewer;
            set => SetProperty(ref _viewer, value);
        }

        public ObservableCollection<bool> CheckList
        {
            get => _checkList ?? new ObservableCollection<bool>();
            set => SetProperty(ref _checkList, value);
        }
    }
}
