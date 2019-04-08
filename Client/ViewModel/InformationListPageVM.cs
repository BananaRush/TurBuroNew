using Client.Interface;
using Client.Utilits;
using Client.View;
using ModelData.Model.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    class InformationListPageVM : Bandel, IPageShell
    {
        private ObservableCollection<Information> _list = null;
        private RelayCommand _getInfo;

        public InformationListPageVM()
        {
            GetData();
        }

        public async void GetData()
        {
            if(Explorer.InformationList != null)
            {
                List = new ObservableCollection<Information>(Explorer.InformationList);
            }
            else if(Explorer.ButtonNav != null)
            {
                List<Information> list = await ModelData.WebApi.InfoPage.GetLents(Explorer.ButtonNav.Id);
                if(list != null)
                {
                    List = new ObservableCollection<Information>(list);
                }
            }
        }

        public RelayCommand GetInfo
        {
            get
            {
                return _getInfo ?? (_getInfo = new RelayCommand(obj => 
                {
                    if(obj is Information elm)
                    {
                        Explorer.Information = elm;
                        App.NavService.Navigate(new InformationPage());
                    }
                }));
            }
        }

        public ObservableCollection<Information> List
        {
            get => _list ?? new ObservableCollection<Information>();
            set => SetProperty(ref _list, value);
        }
    }
}
