using ModelData.Models.Database;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.Utilits
{
    class CreateButtons
    {
        public CreateButtons()
        {

        }

        public static FrameworkElement Create(UIElementModel model, string text, NewsModel param, ICommand cmd)
        {
            Border mainBlock = new Border();
            Button button = new Button();
            button.Style = SetStyleNon();
            button.VerticalAlignment = VerticalAlignment.Stretch;
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid grid = new Grid();
            button.Content = grid;
            button.Command = cmd;
            button.CommandParameter = param;
            // Устанавливаем текст
            SetBtnTextBlock(grid, model, text);
            // Устанавливам размер
            SetBtnSize(model, mainBlock);
            // Устанавливаем позицию
            SetBtnPosition(model, mainBlock);
            // Устанавливаем цвет
            SetBtnBackgraund(model, mainBlock);

            mainBlock.Child = button;
            return mainBlock;
        }

        public static void Update(Border mainBlock, UIElementModel model, NewsModel param)
        {
            Button button = null;
            Grid grid = null;
            TextBlock textBlock = null;

            if(mainBlock.Child is Button btn)
            {
                button = btn;
                if(button.Content is Grid grids)
                {
                    grid = grids;

                    for(int i = 0; i < grids.Children.Count; i++)
                    {
                        if(grids.Children[i] is TextBlock text)
                        {
                            textBlock = text;

                            button.CommandParameter = param;
                            // Устанавливаем текст
                            textBlock.Text = param.Text;
                            // Устанавливаем шрифт
                            SetTextBlockFontFamely(model, textBlock);
                            // Устанавливаем размер шрифта
                            SetTextBlockFontSize(model, textBlock);
                            // Цвет текста
                            SetTextBlockForeground(model, textBlock);
                            // Задает позицию отображения текста
                            SetTextBlockPosition(model, textBlock);
                            // Устанавливам размер
                            SetBtnSize(model, mainBlock);
                            // Устанавливаем позицию
                            SetBtnPosition(model, mainBlock);
                            // Устанавливаем цвет
                            SetBtnBackgraund(model, mainBlock);

                            break;
                        }
                    }
                }
            }
        }

        private static Style SetStyleNon()
        {
            return Application.Current.Resources["ButtonNan"] as Style;
        }

        private static void SetBtnTextBlock(Grid BtnGrid, UIElementModel model, string txt)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = txt;
            // Устанавливаем шрифт
            SetTextBlockFontFamely(model, textBlock);
            // Устанавливаем размер шрифта
            SetTextBlockFontSize(model, textBlock);
            // Цвет текста
            SetTextBlockForeground(model, textBlock);
            // Задает позицию отображения текста
            SetTextBlockPosition(model, textBlock);
            BtnGrid.Children.Add(textBlock);
        }

        private static void SetTextBlockFontFamely(UIElementModel elm, TextBlock textBlock)
        {
            try
            {
                textBlock.FontFamily = new FontFamily(elm.FontFamily);
            }
            catch
            {
                textBlock.FontFamily = new FontFamily("Arial");
            }
        }

        private static void SetTextBlockFontSize(UIElementModel elm, TextBlock textBlock)
        {
            double FontSize = 0;

            try
            {
                FontSize = elm.FontSize;
            }
            catch
            {
                FontSize = 60;
            }

            textBlock.FontSize = FontSize;
        }

        private static void SetTextBlockForeground(UIElementModel elm, TextBlock textBlock)
        {
            try
            {
                var bc = new BrushConverter();
                textBlock.Foreground = (Brush)bc.ConvertFrom(elm.ColorText); 
            }
            catch
            {
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        private static void SetTextBlockPosition(UIElementModel elm, TextBlock textBlock)
        {
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            //if (elm.IsConstructorIncluded)
            //{
            //    double Top = 0;
            //    double Left = 0;

            //    textBlock.VerticalAlignment = VerticalAlignment.Top;
            //    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            //    try
            //    {
            //        Top = elm.TextTop;
            //        Left = elm.TextLeft;
            //    }
            //    catch { }

            //    textBlock.Margin = new Thickness(Left, Top, 0, 0);
            //}
            //else
            //{
            //    textBlock.VerticalAlignment = VerticalAlignment.Center;
            //    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            //}
        }

        private static void SetBtnSize(UIElementModel elm, Border btn)
        {
            double Height = 0;
            double Width = 0;

            try
            {
                Height = Convert.ToDouble(elm.Height.Replace(".", ","));
                Width = Convert.ToDouble(elm.Width.Replace(".", ","));
                btn.Height = Height;
                btn.Width = Width;
            }
            catch
            {
                btn.Height = 25;
                btn.Width = 25;
            }
        }

        private static void SetBtnPosition(UIElementModel elm, Border btn)
        {
            double Top = 0;
            double Left = 0;
          
            try
            {
                Top = Convert.ToDouble(elm.Top.Replace(".", ","));
                Left = Convert.ToDouble(elm.Left.Replace(".", ","));
            }
            catch { }

            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.Margin = new Thickness(Left, Top, 0, 0);
        }

        private static void SetBtnBackgraund(UIElementModel elm, Border btn)
        {
            try
            {
                var bc = new BrushConverter();
                btn.Background = (Brush)bc.ConvertFrom(elm.Background);
            }
            catch
            {
                btn.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
    }
}
