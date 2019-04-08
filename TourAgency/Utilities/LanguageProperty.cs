using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Controls;
using TourAgency.Converters;

namespace TourAgency.Utilities
{
    class LanguageProperty : DependencyObject
    {
        private static DependencyPropertyDescriptor dp ;
        public static IMultiValueConverter GetTextPropertyConverter(DependencyObject obj) { return (IMultiValueConverter)obj.GetValue(TextPropertyConverterProperty); }
        public static void SetTextPropertyConverter(DependencyObject obj, IMultiValueConverter value) { obj.SetValue(TextPropertyConverterProperty, value); }
        public static readonly DependencyProperty TextPropertyConverterProperty = DependencyProperty.RegisterAttached("TextPropertyConverter", typeof(IMultiValueConverter), typeof(LanguageConverter), new PropertyMetadata
        {
            PropertyChangedCallback = async (obj, e) =>
            {
                var box = (TextBlock)obj;
                if (!string.IsNullOrEmpty(box?.Text))
                {
                    try
                    {

                        if (!Session.CurrentSession.DictionaryLanguage.Keys.Contains(box.Text))
                            Session.CurrentSession.DictionaryLanguage.Add(box.Text,
                                new Tuple<string, string>(await API.MakeTranslate("ru-en", box.Text),
                                    await API.MakeTranslate("ru-zh", box.Text)));

                    }
                    catch
                    {
                        //
                    }
                }
                dp = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));
                dp.AddValueChanged(box, async (object a, EventArgs b) =>
                {
                    var boxtxt = (TextBlock)a;
                    if (!string.IsNullOrEmpty(boxtxt?.Text))
                    {
                        try
                        {

                        if (!Session.CurrentSession.DictionaryLanguage.Keys.Contains(boxtxt.Text))
                            Session.CurrentSession.DictionaryLanguage.Add(boxtxt.Text,
                                new Tuple<string, string>(await API.MakeTranslate("ru-en", boxtxt.Text),
                                    await API.MakeTranslate("ru-zh", boxtxt.Text)));
                        }
                        catch
                        {
                         //
                        }
                    }
                });
                await box.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    var binding = BindingOperations.GetBinding(box, TextBlock.TextProperty);
                    MultiBinding newBinding = null;
                    if (binding != null)
                     newBinding = new MultiBinding()
                    {
                        Converter = GetTextPropertyConverter(box),
                        Mode = binding.Mode,
                        StringFormat = binding.StringFormat,
                    };
                    var myBinding = new Binding()
                    {
                        Source = Session.CurrentSession,
                        Path = new PropertyPath("ChoosedLanguage")
                    };

                    if (newBinding == null)
                        newBinding = new MultiBinding()
                        {
                            Converter = GetTextPropertyConverter(box),
                            ConverterParameter = box.Text
                        };
                    newBinding.Bindings.Add(myBinding);
                    myBinding.IsAsync = true;
                    
                    if (binding != null)
                    {
                        newBinding.Bindings.Add(binding);
                    }

                    BindingOperations.SetBinding(box, TextBlock.TextProperty, newBinding);
                }));
            }
        });
    }
}
