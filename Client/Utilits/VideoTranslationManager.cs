using Client.Utilits.SignalR;
using ModelData.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Utilits
{
    public class VideoTranslationManager
    {
        public readonly SignalRVideoManager Signal;

        public VideoTranslationManager()
        {
            Signal = new SignalRVideoManager();
        }

        public void Init()
        {
            Signal.OnConnect += Signal_ConnectionEvent;
            Signal.StartHub();
        }

        #region EventHub

        private void Signal_ConnectionEvent(bool obj)
        {
           // MessageBox.Show("sss");
        }

        #endregion
    }
}
