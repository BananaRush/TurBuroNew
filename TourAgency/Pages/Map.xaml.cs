using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using CefSharp;
using TourAgency.Commands;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using TourAgency.Utilities;
using System.Web.UI.HtmlControls;
using StorageAPI.Models.Database;
using ModelData.Model.Database;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для Map.xaml
    /// </summary>
    public partial class Map : Page
    {
        public static event Action TimerUodate = delegate { };
        public static NewsModel _model = null;

        public Map(string url, string title)
        {
            InitializeComponent();
            this.Loaded += Map_Loaded;
            this.Unloaded += Map_Unloaded;
            TitleTop = title;
            Url = url;
            SetBrowserCompatibilityMode();
            Session.CurrentSession.SwitchLanguage += CurrentSessionOnSwitchLanguage; 
            ChromiumWebBrowser.Navigating+= ChromiumWebBrowserOnNavigating;
            ChromiumWebBrowser.Navigated+= ChromiumWebBrowserOnNavigated;
            ChromiumWebBrowser.Navigate(new Uri((Url.Contains("petersburg") || Url.Contains("welcome2018") ||Url.Contains("portspb.ru")) && !Equals(Session.CurrentSession.ChoosedLanguage, CultureInfo.GetCultureInfo("ru"))? Url.Replace("/ru/", "/en/").Replace("2018.com", "2018.com/en/").Replace("portspb.ru", "portspb.ru/en") : Url));
            ChromiumWebBrowser.LoadCompleted+= ChromiumWebBrowserOnLoadCompleted;
        }

        public Map(NewsModel model)
        {
            _model = model;
            InitializeComponent();
            this.Loaded += Map_Loaded;
            this.Unloaded += Map_Unloaded;
            SetBrowserCompatibilityMode();
            Session.CurrentSession.SwitchLanguage += CurrentSessionOnSwitchLanguage;
            ChromiumWebBrowser.Navigating += ChromiumWebBrowserOnNavigating;
            ChromiumWebBrowser.Navigated += ChromiumWebBrowserOnNavigated;
            //ChromiumWebBrowser.Navigate(new Uri((Url.Contains("petersburg") || Url.Contains("welcome2018") || Url.Contains("portspb.ru")) && !Equals(Session.CurrentSession.ChoosedLanguage, CultureInfo.GetCultureInfo("ru")) ? Url.Replace("/ru/", "/en/").Replace("2018.com", "2018.com/en/").Replace("portspb.ru", "portspb.ru/en") : Url));
            ChromiumWebBrowser.LoadCompleted += ChromiumWebBrowserOnLoadCompleted;
        }

        private void ChromiumWebBrowserOnNavigating(object sender, NavigatingCancelEventArgs navigatingCancelEventArgs)
        {
            try
            {
                TimerUodate();
                HideJsScriptErrors(ChromiumWebBrowser);

                if (navigatingCancelEventArgs.Uri?.Host != new Uri(Url).Host && !navigatingCancelEventArgs.Uri.Host.Contains("yandex.ru"))
                navigatingCancelEventArgs.Cancel = true;

            }
            catch (Exception e)
            {
             
            }
        }

        static readonly Guid SID_SWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
        void OnWebBrowserNewWindow(string URL, int Flags, string TargetFrameName, ref object PostData, string Headers, ref bool Processed)
        {
            Processed = true;
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
        internal interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.IUnknown)]
            object QueryService(ref Guid guidService, ref Guid riid);
        }

        private void SetBrowserCompatibilityMode()
        {
            // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            // FeatureControl settings are per-process
            var fileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            if (String.Compare(fileName, "devenv.exe", true) == 0) // make sure we're not running inside Visual Studio
                return;

            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
                UInt32 mode = 10000; // 10000; 
                key.SetValue(fileName, mode, RegistryValueKind.DWord);
            }

            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BLOCK_LMZ_SCRIPT",
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                // enable <scripts> in local machine zone
                UInt32 mode = 0;
                key.SetValue(fileName, mode, RegistryValueKind.DWord);
            }

            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_NINPUT_LEGACYMODE",
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                // disable Legacy Input Model
                UInt32 mode = 0;
                key.SetValue(fileName, mode, RegistryValueKind.DWord);
            }

        }

        private void CurrentSessionOnSwitchLanguage(object o, EventArgs eventArgs)
        {
            if (Url.Contains("YandexMap"))
            {
                ChromiumWebBrowser.Navigate(
                    !Equals(Session.CurrentSession.ChoosedLanguage, CultureInfo.GetCultureInfo("ru"))
                        ? new Uri(Url.Replace("ru_RU", "en_US"))
                        : new Uri(Url.Replace("en_US", "ru_RU")));
                ChromiumWebBrowser.InvokeScript("eval", "document.body.style.zoom = 1.5;");
            }
            else
              ChromiumWebBrowser.Navigate(new Uri((Url.Contains("petersburg") || Url.Contains("2018.com")|| Url.Contains("portspb.ru")) && !Equals(Session.CurrentSession.ChoosedLanguage, CultureInfo.GetCultureInfo("ru")) ? Url.Replace("/ru/", "/en/").Replace("/ru", "/en").Replace("2018.com", "2018.com/en/").Replace("portspb.ru", "portspb.ru/en") : Url).AbsoluteUri);

        
        }

        private void ChromiumWebBrowserOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
            try
            {
                ChromiumWebBrowser.InvokeScript("eval", Script);
                ChromiumWebBrowser.InvokeScript("eval", @"$(document).bind('DOMNodeInserted', function(event) {
                
                    $('.b-serp-item__title-link').removeAttr('target');
            });");
                if (Url.Contains("/search/"))
                    Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;
            }
            catch (Exception e)
            {

            }
        }

        private void ChromiumWebBrowserOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            HideJsScriptErrors(ChromiumWebBrowser);
            #region  NewWindowDisable

            IServiceProvider serviceProvider = (IServiceProvider)ChromiumWebBrowser.Document;
            Guid serviceGuid = SID_SWebBrowserApp;
            Guid iid = typeof(SHDocVw.IWebBrowser2).GUID;
            SHDocVw.IWebBrowser2 myWebBrowser2 = (SHDocVw.IWebBrowser2)serviceProvider.QueryService(ref serviceGuid, ref iid);
            SHDocVw.DWebBrowserEvents_Event wbEvents = (SHDocVw.DWebBrowserEvents_Event)myWebBrowser2;
            wbEvents.NewWindow += new SHDocVw.DWebBrowserEvents_NewWindowEventHandler(OnWebBrowserNewWindow);
            #endregion

        }

        private void HideJsScriptErrors(WebBrowser wb)
        {
            try
            {

                FieldInfo fld = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fld == null)
                    return;
                object obj = fld.GetValue(wb);
                if (obj == null)
                    return;

                obj.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, obj, new object[] { true });
            }
            catch { }
        }
     
        private void Map_Unloaded(object sender, RoutedEventArgs e)
        {
            Session.CurrentSession.SwitchLanguage -= CurrentSessionOnSwitchLanguage;
            Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
            this.Unloaded -= Map_Unloaded;
          
        }
        
        public Map()
        {
            InitializeComponent();
            this.Loaded += Map_Loaded;
            this.Unloaded += Map_Unloaded;
        }

        CefSettings settings = new CefSettings();
        bool IsMouseDown = false;
        Point? LastSavedPoint = null;
        IBrowserHost host;
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "TitleTop", typeof(string), typeof(Map), new PropertyMetadata(default(string)));

        public string TitleTop
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
            "Url", typeof(string), typeof(Map), new PropertyMetadata("../HTML/Map/YandexMapHTML.html"));

        public string Url
        {
            get => (string) GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        private async void Map_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Map_Loaded;
            if(_model != null)
            {
                UrlInfo urlInfo = await WebApi.WebBrowser.Get(_model.Id);
                try
                {
                    Url = urlInfo.Url;
                    TitleTop = urlInfo.Name;
                    ChromiumWebBrowser.Navigate(new Uri(Url));
                }
                catch(Exception r) {

                }
            }
        }

        private ICommand _GoBackCommand;
        public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new Command(a =>
        {
           if( ChromiumWebBrowser.CanGoBack)
            ChromiumWebBrowser.GoBack();
           else
            (App.Current.MainWindow as MainWindow).Frame.Navigate(new FirstPage());
        }));



//        private const string ScriptYandex = @"var a = document.getElementsByClassName('b-serp-item__title-link');
//for(var i=0; i<a.lenght;i++ ){
//a[i].setAttribute('onmousedown', '');
//}
//"

        private const string Script = @"var els = [0, 0, 0, 0, 0, 0, 0, 0,0], curEls = 0;
function deleteElements() {
    if (els.indexOf(0) != -1) {
        var a1 = document.getElementsByClassName('jivo-iframe-container-bottom'),
            a2 = document.getElementById('widgetEsirGovSpbRu'),
            a3 = document.getElementById('menu'),
            a4 = document.getElementsByClassName('bottom_footer'),
            a5 = document.getElementsByClassName('content'),
            a6 = document.getElementById('st-container'),
            a7 = document.getElementsByTagName('body'),
            a8 = document.getElementsByClassName('header_btns_block');
            a9= document.getElementsByClassName('btn_round btn_round_black');
        if (!els[0] && a1.length != 0) {
            a1[0].parentNode.removeChild(a1[0]);
            els[0] = 1;
        }
        if (!els[1] && a2) {
            a2.parentNode.removeChild(a2);
            els[1] = 1;
        }
        if (!els[2] && a3) {
            a3.parentNode.removeChild(a3);
            els[2] = 1;
        }
        if (!els[3] && a4.length != 0) {
            a4[0].parentNode.removeChild(a4[0]);
            els[3] = 1;
        }
        if (!els[4] && a5.length != 0) {
            a5[0].style.paddingLeft = 0;
            els[4] = 1;
        }
        if (!els[5] && a6) {
            a6.style.marginTop = 0;
            a6.style.paddingLeft = 0;
            a6.children[0].style.padding=0;
            a6.children[0].style.margin=0;
            els[5] = 1;
        }
        if (!els[6] && a7.length != 0) {
            a7[0].style.marginTop = 0;
            a7[0].style.paddingLeft = 0;
            els[6] = 1;
        }
        if (!els[7] && a8.length != 0) {
            a8[0].parentNode.removeChild(a8[0]);
            els[7] = 1;
        }
if(!els[8] && a9.length!=0){
a9[0].parentNode.removeChild(a9[0]);
            els[8] = 1;
}
        setTimeout(function () { deleteElements(); }, 1000);
    }
}

deleteElements();

" ;

        public void Dispose()
        {
            settings?.Dispose();
            host?.Dispose();
            ChromiumWebBrowser.Source = null;
            ChromiumWebBrowser?.Dispose();
        }

        private void BackTouch(object sender, TouchEventArgs e)
        {
            GoBackCommand.Execute(null);
        }

        private void ChromiumWebBrowser_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.MessageBox.Show("sad");
        }
    }
}
