using Client.Model;
using ModelData.Models.Database;
using ModelData.Utilits;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.Utilits
{
    class ViewManager
    {
        public static void Update(List<UIElementModel> data, ObservableCollection<FrameworkElement> element, List<NewsModel> buttonNavList)
        {
            if (!element.Any())
                return;

            if(data == null || !data.Any())
            {
                element.Clear();
                return;
            }

            // Удаляем элементы которых нет
            for(int i = 0; i < element.Count; i++)
            {
                bool delflag = true;
                for(int j = 0; j < data.Count; j++)
                {
                    if(element[i].Tag is TagElement tag)
                    {
                        if(tag.Id == data[j].Id)
                        {
                            delflag = false;
                            break;
                        }
                    }
                }

                if (delflag)
                {
                    element.RemoveAt(i);
                    i = -1;
                }
            }

            // Проверяем на равенство и обновляем элементы
            for(int i = 0; i < data.Count; i++)
            {
                bool delflag = false;
                for (int j = 0; j < element.Count; j++)
                {
                    if(element[j].Tag is TagElement tag)
                    {
                        if(tag.Id == data[i].Id)
                        {
                            delflag = true;

                            if (!tag.UIElementModel.NewEquals(data[i]))
                            {
                                // Тут обновляемся
                                tag.UIElementModel = data[i];
                                tag.IsNavVisibility = data[i].IsNavVisibility;
                                element[j].Tag = tag;
                                Update(data[i], element[j], buttonNavList);
                            }
                            break;
                        }
                    }
                }
                if (delflag)
                {
                    data.RemoveAt(i);
                    i = -1;
                }
            }

            // какие добавить
            // какие обновить

        }

        private static void Update(UIElementModel data, FrameworkElement element, List<NewsModel> buttonNavList)
        {
            if (data.ElementType == UIElementType.ButtonNav)
            {
                if(element is Border border)
                {
                    CreateButtons.Update(border, data, buttonNavList.Where(r => r.Id == data.ButtonNavId).FirstOrDefault());
                }
            }
            // Навигационный фрейм
            if (data.ElementType == UIElementType.NavigationFrame)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
                element.Height = Convert.ToDouble(data.Height.Replace(".", ","));
                element.Width = Convert.ToDouble(data.Width.Replace(".", ","));
            }

            // Слайдер
            if (data.ElementType == UIElementType.Slider)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
            }

            // Язык
            if (data.ElementType == UIElementType.Language)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
            }

            // Погода
            if (data.ElementType == UIElementType.Weather)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
            }

            // Время
            if (data.ElementType == UIElementType.Time)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
            }

            // Валюты
            if (data.ElementType == UIElementType.Currency)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
            }

            // Новости
            if (data.ElementType == UIElementType.News)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
            }

            // Опросы
            if (data.ElementType == UIElementType.Survey)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
            }

            // Логотип
            if (data.ElementType == UIElementType.Logotype)
            {
                if(element is Image image)
                {
                    image.VerticalAlignment = VerticalAlignment.Top;
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
                    image.Height = Convert.ToDouble(data.Height.Replace(".", ","));
                    image.Width = Convert.ToDouble(data.Width.Replace(".", ","));
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(SaveUrlImage.Save(Path.Combine(Config.GetHost(), data.ImageName))));
                }
            }

            // Кнопка на главную
            if (data.ElementType == UIElementType.BackButton)
            {
                if(element is Button button)
                {
                    button.Style = Application.Current.Resources["ButtonNan"] as Style;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
                    button.Height = Convert.ToDouble(data.Height.Replace(".", ","));
                    button.Width = Convert.ToDouble(data.Width.Replace(".", ","));

                    if(button.Content is Image image)
                    {
                        image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(SaveUrlImage.Save(Path.Combine(Config.GetHost(), data.ImageName))));
                    }
                }
            }

            // Млдуль спец. возможностей
            if (data.ElementType == UIElementType.FeatureModule)
            {
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Margin = new Thickness(Convert.ToDouble(data.Left.Replace(".", ",")), Convert.ToDouble(data.Top.Replace(".", ",")), 0, 0);
                element.Height = Convert.ToDouble(data.Height.Replace(".", ","));
                element.Width = Convert.ToDouble(data.Width.Replace(".", ","));
            }
        }
    }
}

public static class HashHelper
{
    public static bool NewEquals(this UIElementModel obj, UIElementModel uI)
    {
        System.Diagnostics.Debug.Print("Добавь свойство на проверку");
        if (EqualityComparer<UIElementModel>.Default.Equals(obj, uI)) return false;

        if(obj.Background == uI.Background &&
            obj.Top == uI.Top &&
            obj.Left == uI.Left &&
            obj.Width == uI.Width &&
            obj.Height == uI.Height &&
            obj.IsNavVisibility == uI.IsNavVisibility &&
            obj.FileImg == uI.FileImg &&
            obj.ImageName == uI.ImageName &&
            obj.ColorText == uI.ColorText &&
            obj.Background == uI.Background &&
            obj.FontFamily == uI.FontFamily &&
            obj.FontSize == uI.FontSize)
            return true;

        return false;
    }
}
