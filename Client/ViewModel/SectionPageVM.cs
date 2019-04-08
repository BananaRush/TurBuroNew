using Client.Interface;
using Client.Utilits;
using Client.View;
using ModelData;
using ModelData.Model.Database;
using ModelData.Models.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client.ViewModel
{
    class SectionPageVM : Bandel, IPageShell
    {
        private ObservableCollection<Section> PSections { get; set; }
        private RelayCommand _goNext;
        private List<Section> sections = null;
        private Section _item = null;

        public SectionPageVM()
        {
            GetData();
        }

        public async void GetData()
        {
            if(Explorer.ButtonNav != null)
            {
                sections = await WebApi.SectionPage.Get(Explorer.ButtonNav.Id);
                if(sections != null)
                {
                    //Set Parents
                    ModelData.Utilits.Utilits.SetParnets(sections, null);
                    Sections = new ObservableCollection<Section>(sections);
                }
            }
        } 

        public ObservableCollection<Section> Sections
        {
            get => PSections ?? new ObservableCollection<Section>();
            set
            {
                if(PSections != value)
                {
                    PSections = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TitleTop { get; set; }

        public RelayCommand GoNext
        {
            get
            {
                return _goNext ?? (_goNext = new RelayCommand(async obj => 
                {
                    _item = obj as Section;

                    if(_item != null)
                    {
                        if(_item.Children.Count == 0)
                        {
                            // Тут загружаем данные
                            List<Information> information = await WebApi.InfoPage.GetIdSection(_item.Id);
                            if(information != null)
                            {
                                if(information.Count > 1)
                                {
                                    // Загружаем много данных
                                    Explorer.InformationList = information;
                                    App.NavService.Navigate(new InformationListPage());
                                }
                                else
                                {
                                    // Единичный экземпляр
                                    Explorer.Information = information.FirstOrDefault();
                                    App.NavService.Navigate(new InformationPage());
                                }
                            }
                        }
                        else
                        {
                            PSections.Clear();
                            foreach (var ims in _item.Children)
                                Sections.Add(ims);
                        }
                    }
                }));
            }
        }

        public override RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(obj => 
                {
                    if(PSections != null && PSections.FirstOrDefault() != null && PSections.FirstOrDefault().Parent != null)
                    {
                        Section elm = PSections.FirstOrDefault().Parent;
                        if (elm.Parent == null)
                        {
                            PSections.Clear();
                            foreach (var ims in sections)
                                Sections.Add(ims);
                          
                        }
                        else
                        {
                            if (elm.Parent.Children.Count != 0)
                            {
                                PSections.Clear();
                                foreach (var ims in elm.Parent.Children)
                                    Sections.Add(ims);
                            }
                        }
                    }
                    else
                    {
                        App.GoBack();
                    }
                }));
            }
        }
    }
}
