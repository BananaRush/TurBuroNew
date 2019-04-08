using Client.Interface;
using Client.Utilits.Language;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Utilits
{
    public class Bandel : INotifyPropertyChanged, IPageShell, IClientWord
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected RelayCommand _goBackCommand;

        public Bandel()
        {
            //App.SetLang += App_SetLang;
        }

        //private void App_SetLang()
        //{
        //    NewOnPropertyChanged(LangBack);
        //    NewOnPropertyChanged(LangLanguage);
        //    NewOnPropertyChanged(LangСurrencyСourses);
        //    NewOnPropertyChanged(LangNews);
        //    NewOnPropertyChanged(LangLoadMore);
        //    NewOnPropertyChanged(LangAccessiblEnvironment);
        //    NewOnPropertyChanged(LangObject);
        //    NewOnPropertyChanged(LangObject1);
        //    NewOnPropertyChanged(LangTime);
        //    NewOnPropertyChanged(LangWeather);
        //    NewOnPropertyChanged(LangColorInversion);
        //    NewOnPropertyChanged(LangMonochrome);
        //    NewOnPropertyChanged(LangLoupe);
        //    NewOnPropertyChanged(LangObject2);
        //}

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void NewOnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual string TitleTop { get; set; }

        public virtual RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(() =>
                {
                    App.GoBack();
                }));
            }
        }

        public string LangBack
        {
            get => ClientLang.Lang.LangBack;
            set { }
        }

        public string LangLanguage
        {
            get => ClientLang.Lang.LangLanguage;
            set { }
        }

        public string LangСurrencyСourses
        {
            get => ClientLang.Lang.LangСurrencyСourses;
            set { }
        }

        public string LangNews
        {
            get => ClientLang.Lang.LangNews;
            set { }
        }

        public string LangAllNews
        {
            get => ClientLang.Lang.LangAllNews;
            set { }
        }

        public string LangLoadMore
        {
            get => ClientLang.Lang.LangLoadMore;
            set { }
        }

        public string LangAccessiblEnvironment
        {
            get => ClientLang.Lang.LangAccessiblEnvironment;
            set { }
        }

        public string LangObject
        {
            get => ClientLang.Lang.LangObject;
            set { }
        }

        public string LangObject1
        {
            get => ClientLang.Lang.LangObject1;
            set { }
        }

        public string LangTime
        {
            get => ClientLang.Lang.LangTime;
            set { }
        }

        public string LangWeather
        {
            get => ClientLang.Lang.LangWeather;
            set { }
        }

        public string LangColorInversion
        {
            get => ClientLang.Lang.LangColorInversion;
            set { }
        }

        public string LangMonochrome
        {
            get => ClientLang.Lang.LangMonochrome;
            set { }
        }

        public string LangDownScreen
        {
            get => ClientLang.Lang.LangDownScreen;
            set { }
        }

        public string LangLoupe
        {
            get => ClientLang.Lang.LangLoupe;
            set { }
        }

        public string LangObject2
        {
            get => ClientLang.Lang.LangObject2;
            set { }
        }
    }
}
