using Client.Utilits;
using Client.Utilits.Language;
using ModelData.Utilits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VisitAPI.Model;

namespace Client.Controls.Events
{
    class EventsControlVM : Bandel
    {
        private const int LoadElementCout = 10;
        private RelayCommand _getEventList;
        private RelayCommand _loadComponent;
        private RelayCommand _getFullEvent;
        private ObservableCollection<EventsModel> _eventsList;
        private UserControl _control;
        private FullEventsPage _fullEventsPage;
        private int total_results { get; set; }
        public string URL = "http://www.visit-petersburg.ru";
        private volatile bool IsDownloadImg;

        public EventsControlVM()
        {
            GetData();
        }

        public async void GetData()
        {
            TitleTop = ClientLang.Lang.LangNews;
            EventsExtendedModel elm = await VisitAPI.WebAPI.Events.GetExtended();
            if (elm != null)
            {
                total_results = elm.total_results;
                EventsList = new ObservableCollection<EventsModel>(elm.results);
                DownloadImg();
            }
        }

        public RelayCommand GetEventList
        {
            get
            {
                return _getEventList ?? (_getEventList = new RelayCommand(async () => 
                {
                    if(_control.Visibility != Visibility.Hidden)
                    {
                        if(_fullEventsPage == null)
                        {
                            _fullEventsPage = new FullEventsPage();
                        }

                        _control.Visibility = Visibility.Collapsed;
                        _fullEventsPage.DataContext = this;
                        _control.DataContext = null;
                        App.NavService.Navigate(_fullEventsPage);
                    }

                    try
                    {
                        if ((EventsList.Count + LoadElementCout) < total_results)
                        {
                            int page = (EventsList.Count / LoadElementCout) + 1;
                            int downloadIndex = EventsList.Count;

                            EventsExtendedModel list = await VisitAPI.WebAPI.Events.GetExtended(page);

                            foreach (var item in list.results)
                                EventsList.Add(item);

                            DownloadImg(downloadIndex);
                        }
                    }
                    catch { }
                }));
            }
        }

        public RelayCommand LoadComponent
        {
            get
            {
                return _loadComponent ?? (_loadComponent = new RelayCommand(obj => 
                {
                    if(obj is UserControl control)
                    {
                        _control = control;
                    }
                }));
            }
        }

        public RelayCommand GetFullEvent
        {
            get
            {
                return _getFullEvent ?? (_getFullEvent = new RelayCommand(async obj => 
                {
                    if (obj is int index)
                    {
                        EventsInfoModel eventsInfo = await VisitAPI.WebAPI.Events.GetEvent(index);
                        if(eventsInfo != null)
                        {
                            string txt = HtmlEventTemplate.Get(eventsInfo.Title, SaveUrlImage.GetByteImg(eventsInfo.Image), eventsInfo.Intro + eventsInfo.Text);
                            App.NavService.Navigate(new View.InformationPage()
                            {
                                DataContext = new ViewModel.InformationPageVM("Новости", txt)
                            });
                        }
                    }
                }));
            }
        }

        public override RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(() =>
                {
                    if(_control.Visibility != Visibility.Visible)
                    {
                        _fullEventsPage.DataContext = null;
                        _control.DataContext = this;
                        App.GoBack();
                        _control.Visibility = Visibility.Visible;

                        for (int i = EventsList.Count -1; 10 < EventsList.Count; i--)
                            EventsList.RemoveAt(i);
                    }
                }));
            }
        }

        public ObservableCollection<EventsModel> EventsList
        {
            get => _eventsList ?? new ObservableCollection<EventsModel>();
            set => SetProperty(ref _eventsList, value);
        }

        private void DownloadImg(int position = 0)
        {
            if (IsDownloadImg == true)
                return;

            Task outer = Task.Factory.StartNew(() =>
            {
                IsDownloadImg = true;
                for (int i = position; i < EventsList.Count; i++)
                    EventsList[i].Image = SaveUrlImage.Save(URL + EventsList[i].Image);
                IsDownloadImg = false;
            });
        }
    }
}
