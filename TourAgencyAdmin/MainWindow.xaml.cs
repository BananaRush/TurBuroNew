using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using Newtonsoft.Json;
using TourAgencyAdmin.Annotations;
using TourAgencyAdmin.Utilities;
using TourAgencyAdmin.Utilities.ModelWebApi;
using Slider = TourAgencyAdmin.Utilities.ModelWebApi.Slider;

namespace TourAgencyAdmin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            GetAllCategories();
            GetAllSliders();
            GetAllTest();
        }

        #region fields
        private ObservableCollection<NewsModel> _allcategories;
        private ObservableCollection<Test> _allTests;
        private ICommand _addButtonCommand;
        private ICommand _deleteButtonCommand;
        private ObservableCollection<CustomSlider> _allSliders;
        private ICommand _addSlideCommand;
        private ICommand _removeSlideCommand;
        private ICommand _cancelCommand;
        private ICommand _updateCommand;
        private ICommand _addOprosCommand;
        private ICommand _removeOprosCommand;
        private string _liveString;
        #endregion

        public string LiveString
        {
            get => _liveString;
            set
            {
                _liveString = value;
                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new Command(a =>
        {
            ChoosedNews = null;
            ChoosedSlider = null;
            GetAllCategories();
            GetAllSliders();
            GetAllTest();
        }));

        public ICommand UpdateCommand => _updateCommand ?? (_updateCommand = new Command(async a =>
        {
            if (AllSliders.Any(f => f.Content == null) || AllCategories.Any(f=> f.Content==null || f.IconUri== null|| f.Text== null))
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
          await WebApi.Slider.Get(1000, 0).ContinueWith(r =>
            {
                if (r.Result == null)
                {
                    MessageBox.Show("Возникла ошибка при обновлении!");
                    return;
                }

                AllSliders?.ToList().ForEach(async f =>
                {
                        Slider slide = new Slider() { ContentType = f.ContentType, Content = f.Content, Id= f.Id};
                        slide.Caption = JsonConvert.SerializeObject(f.model);
                        if (!r.Result.Select(t=>t.Id).Contains(slide.Id))
                        {
                           f.Id= await WebApi.Slider.Post(slide);
                        }
                        else
                        {
                            await WebApi.Slider.Put(f.Id, slide);
                        }
                    });

                r.Result.ToList().ForEach(async t =>
                {
                    if (!AllSliders.Select(g=> g.Id).Contains(t.Id))
                        await WebApi.Slider.Delete(t.Id);
                });
            });
            await WebApi.News.Get(1000, 0).ContinueWith( r =>
            {
                if (r.Result == null)
                {
                    MessageBox.Show("Возникла ошибка при обновлении!");
                    return;
                }

                AllCategories?.ToList().ForEach(async f =>
                {
                    NewsModel slide = f;
                    if (!r.Result.Select(t=> t.Id).Contains(slide.Id))
                    {
                       f.Id= await WebApi.News.Post(slide);
                    }
                    else
                    {
                        await WebApi.News.Put(f.Id, slide);
                    }
                });

                r.Result.ToList().ForEach(async t =>
                {
                    if (!AllCategories.Select(g=> g.Id).Contains(t.Id))
                        await WebApi.News.Delete(t.Id);
                });
            });


            await WebApi.News.SetLive(LiveString);

            await WebApi.Test.Get(1000, 0).ContinueWith(r =>
            {
                if (r.Result == null)
                {
                    MessageBox.Show("Возникла ошибка при обновлении!");
                    return;
                }

                
                AllTests?.ToList().ForEach(async f =>
                {
                    Test test = f;
                    if (!r.Result.Select(t => t.Id).Contains(test.Id))
                    {
                        f.Id = await WebApi.Test.Post(test);
                    }
                    else
                    {
                        await WebApi.Test.Put(f.Id, test);
                    }
                });

                r.Result.ToList().ForEach(async t =>
                {
                    if (!AllTests.Select(g => g.Id).Contains(t.Id))
                        await WebApi.Test.Delete(t.Id);
                });
            });

            await UploadNewContent();

        }));

        public ICommand AddSlideCommand => _addSlideCommand ?? (_addSlideCommand = new Command(a =>
        {
            AllSliders.Add(new CustomSlider(){model = new ModelSliderContent()});
        }));

        public ICommand RemoveSlideCommand => _removeSlideCommand ?? (_removeSlideCommand = new Command(a =>
        {
            AllSliders.Remove(ChoosedSlider);
        }));

        public ICommand DeleteButtonCommand => _deleteButtonCommand ?? (_deleteButtonCommand = new Command(a =>
        {
            AllCategories.Remove(ChoosedNews);
        }));

        public ICommand AddButtonCommand => _addButtonCommand ?? (_addButtonCommand = new Command(a =>
        {
            AllCategories.Add(new NewsModel(){Text = "Новая кнопка"});
        }));

        public ICommand AddOprosCommand => _addOprosCommand ?? (_addOprosCommand = new Command(a =>
        {
           AllTests.Add(new Test(){Title = "1"});
            foreach (Test allTest in AllTests)
            {
                if(allTest.Responses==null) allTest.Responses = new List<Response>();
                while (allTest.Responses.Count != 3)
                {
                    allTest.Responses.Add(new Response());
                }
            }
        }));

        public ICommand RemoveOprosCommand => _removeOprosCommand ?? (_removeOprosCommand = new Command(a =>
        {
            AllTests.Remove(ChoosedTest);
        }));

        public ObservableCollection<Test> AllTests
        {
            get => _allTests?? (_allTests = new ObservableCollection<Test>());
            set
            {
                _allTests = value;
                OnPropertyChanged();
            }

        }

        public ObservableCollection<CustomSlider> AllSliders
        {
            get => _allSliders ?? (_allSliders = new ObservableCollection<CustomSlider>());
            set
            {
                _allSliders = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NewsModel> AllCategories
        {
            get => _allcategories ?? (_allcategories = new ObservableCollection<NewsModel>());
            set
            {
                _allcategories = value;
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty ChoosedSliderProperty = DependencyProperty.Register(
            "ChoosedSlider", typeof(CustomSlider), typeof(MainWindow), new PropertyMetadata(default(CustomSlider)));

        public CustomSlider ChoosedSlider
        {
            get => (CustomSlider) GetValue(ChoosedSliderProperty);
            set => SetValue(ChoosedSliderProperty, value);
        }


        public static readonly DependencyProperty ChoosedNewsProperty = DependencyProperty.Register(
            "ChoosedNews", typeof(NewsModel), typeof(MainWindow), new PropertyMetadata(default(NewsModel)));

        public NewsModel ChoosedNews
        {
            get => (NewsModel) GetValue(ChoosedNewsProperty);
            set => SetValue(ChoosedNewsProperty, value);
        }

        public static readonly DependencyProperty ChoosedTestProperty = DependencyProperty.Register(
            "ChoosedTest", typeof(Test), typeof(MainWindow), new PropertyMetadata(default(Test)));

        public Test ChoosedTest
        {
            get => (Test) GetValue(ChoosedTestProperty);
            set => SetValue(ChoosedTestProperty, value);
        }

        private async Task GetAllCategories()
        {
            AllCategories= new ObservableCollection<NewsModel>(await WebApi.News.Get(1000, 0));
        }

        private async Task GetAllSliders()
        {
            try
            {

            AllSliders = new ObservableCollection<CustomSlider>((await WebApi.Slider.Get(1000,0)).Select(f=> new CustomSlider(){Caption = f.Caption, Content = f.Content, ContentType = f.ContentType, Id = f.Id, model = JsonConvert.DeserializeObject<ModelSliderContent>(f.Caption)}));

            }
            catch 
            {
                AllSliders = new ObservableCollection<CustomSlider>();
            }

            LiveString = await WebApi.News.GetLive();

        }

        private async Task GetAllTest()
        {
            AllTests = new ObservableCollection<Test>((await WebApi.Test.Get(100,0)));
            foreach (Test allTest in AllTests)
            {
                    while (allTest.Responses.Count!=3)
                    {
                        allTest.Responses.Add(new Response());
                    }
            }
        }

        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            ChoosedNews = AllCategories[int.Parse(((RadioButton) sender).Tag.ToString())];
        }

        private void ChoosePicture(object sender, MouseButtonEventArgs e)
        {
            try
            {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image|*.png;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                if (!Directory.Exists("Content"))
                    Directory.CreateDirectory("Content");
                File.Copy(dialog.FileName,  $"Content/new_{Path.GetFileName(dialog.FileName)}", true);
                ChoosedNews.IconUri = $"{Path.GetFileName(dialog.FileName)}";
                UpdateNotINPCType(nameof(ChoosedNews));
            }

            }
            catch 
            {
                MessageBox.Show("Ошибка при перемещении файла, возможно он занят...");
            }
        }

        private void UpdateNotINPCType(string name)
        {
            var memory = this.GetType().GetProperty(name).GetValue(this);
            this.GetType().GetProperty(name).SetValue(this, null);
            this.GetType().GetProperty(name).SetValue(this, memory);
        }

        private void SlideChecked(object sender, RoutedEventArgs e)
        {
            ChoosedSlider = AllSliders[int.Parse(((RadioButton)sender).Tag.ToString())];
        }

        private void UploadMedia(object sender, MouseButtonEventArgs e)
        {
            try
            {

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image or Video|*.mp4;*.wmv;*.png;*.jpg";
                if (dialog.ShowDialog() == true)
                {
                    if (!Directory.Exists("Content"))
                        Directory.CreateDirectory("Content");
                    File.Copy(dialog.FileName, $"Content/new_{Path.GetFileName(dialog.FileName)}", true);
                    ChoosedSlider.Content = $"{Path.GetFileName(dialog.FileName)}";
                    UpdateNotINPCType(nameof(ChoosedSlider));
                }

            }
            catch
            {
                MessageBox.Show("Ошибка при перемещении файла, возможно он занят...");
            }
        }

        private async Task UploadNewContent()
        {
            await Task.Run(() =>
            {
                var allNewFile = Directory.GetFiles("Content").Where(f => Path.GetFileName(f).StartsWith("new_"));
                using (WebClient wc =
                    new WebClient() {Credentials = new NetworkCredential("fresa-bm_touragency", "i8rn/brK")})
                {
                    foreach (string s in allNewFile)
                    {
                        try
                        {

                        wc.UploadFile($"ftp://ftp.fresa-bm.nichost.ru/{Path.GetFileName(s.Replace("new_", String.Empty))}", s);
                        File.Move(s, s.Replace("new_", String.Empty));

                        }
                        catch
                        {
                           //
                        }
                    }
                }
            });
        }

        private void TestChoosed(object sender, RoutedEventArgs e)
        {
            ChoosedTest = AllTests[int.Parse(((RadioButton)sender).Tag.ToString())];
        }
    }

    public class CustomSlider : Slider
    {

        public ModelSliderContent model { get; set; }
    }
}
