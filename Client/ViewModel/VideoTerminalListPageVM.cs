using Client.Utilits;
using ModelData.Models.Database;
using ModelData.Utilits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    class VideoTerminalListPageVM : Bandel
    {
        private ObservableCollection<TerminalsModel>  _terminalList = null;
        public VideoTerminalListPageVM()
        {
            App.VideoTranslationManager.Signal.TerminalList += Signal_TerminalList;
            // Запрашивам список киосков
            App.VideoTranslationManager.Signal.SnGetTerminalList();
            TerminalList = new ObservableCollection<TerminalsModel>();
        }

        private void Signal_TerminalList(List<TerminalsModel> obj)
        {
            
        }

        public ObservableCollection<TerminalsModel> TerminalList
        {
            get => _terminalList;
            set => SetProperty(ref _terminalList, value);
        }
    }
}
