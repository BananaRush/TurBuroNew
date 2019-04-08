using ModelDate.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VSHIM.Control.Handicapped
{
    class Handicapped
    {
        private Grid MainGrid = null;
        public Handicapped(Grid grid)
        {
            MainGrid = grid;
        }

        public void Inizializate()
        {
            // Устоновить смещение экрана
            Window win = MainGrid.Parent as Window;
            if(win != null)
            {
                /* Тут запрашиваем данные */
                OffsetContent.SetModul(win, MainGrid);
                // Устоновить кнопки навигации
                KeyNavigator.SetKeyNavigator(MainGrid);
                // Регистрируем событие выделения обьекта
                KeyNavigator.DSelectObj += KeyNavigator_DSelectObj;
                TextZoom.Inizializate(MainGrid);

            }
        }

        private void KeyNavigator_DSelectObj(object obj, ref bool IsNavigator)
        {
            if (IsNavigator || IsHandButton.IsDoubleTouch)
            {
                DictationText.SpeekCansel();
                DictationText.Play(obj);
                IsNavigator = false;
            }
        }
    }
}
