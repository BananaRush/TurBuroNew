using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VSHIM.Control.Handicapped.View;

namespace VSHIM.Control.Handicapped
{
    static class OffsetContent
    {

        public static void SetModul(Window window, Grid mainGrid)
        {
            View.OffsetContentControl offsetContentControl = new View.OffsetContentControl();
            offsetContentControl.MainContent.Content = mainGrid;
            window.Content = offsetContentControl;
        }
    }
}
