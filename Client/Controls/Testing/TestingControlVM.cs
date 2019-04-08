using Client.Utilits;
using ModelData.Models.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelData.WebApi;

namespace Client.Controls.Testing
{
    public class TestingControlVM : Bandel
    {
        private RelayCommand _testing;
        private RelayCommand _selectOption;
        private ObservableCollection<SurveyValueModel> _listTesting = null;
        List<SurveyModel> surveyModels = null;
        private int _testIndex = 0;
        private string _questionText = string.Empty;
        private SurveyValueModel _selectOptionElm = null;
        private bool _isVisibilityOption = true; 

        public TestingControlVM()
        {
            App.SetLang += App_SetLang;
            App_SetLang();
        }

        private void App_SetLang()
        {
            GetData();
        }

        public async void GetData()
        {
            surveyModels = await SurveyPage.Get(App.DataCom);

            if(surveyModels != null && surveyModels.Any())
            {
                _testIndex = -1;
                SetQuestion();
            }
        }

        private void SetQuestion()
        {
            _selectOptionElm = null;
            if (_testIndex >= surveyModels.Count - 1)
            {
                _testIndex = -1;
            }
            ++_testIndex;
            ListTesting.Clear();
            ListTesting = new ObservableCollection<SurveyValueModel>(surveyModels[_testIndex].ListOption);
            QuestionText = surveyModels[_testIndex].Title;
        }

        public bool IsVisibilityOption
        {
            get => _isVisibilityOption;
            set => SetProperty(ref _isVisibilityOption, value);
        }

        public string QuestionText
        {
            get => _questionText;
            set => SetProperty(ref _questionText, value);
        }

        public ObservableCollection<SurveyValueModel> ListTesting
        {
            get => _listTesting ?? new ObservableCollection<SurveyValueModel>();
            set => SetProperty(ref _listTesting, value);
        }

        public RelayCommand SelectOption
        {
            get
            {
                return _selectOption ?? (_selectOption = new RelayCommand(obj => 
                {
                    if (obj is SurveyValueModel elm)
                    {
                        _selectOptionElm = elm;
                    }
                }));
            }
        }

        public RelayCommand Testing
        {
            get
            {
                return _testing ?? (_testing = new RelayCommand(async() => 
                {
                    if(_selectOptionElm != null)
                    {
                        IsVisibilityOption = false;
                        System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch();
                        swatch.Start();
                        await SurveyPage.OptionCout(_selectOptionElm.Id);
                        swatch.Stop();
                        if(swatch.ElapsedMilliseconds < TimoutUp.UP_NEXT_TESTING_MILLESEC)
                        {
                            int Delay = Convert.ToInt32(TimoutUp.UP_NEXT_TESTING_MILLESEC - swatch.ElapsedMilliseconds);
                            await Task.Delay(Delay);
                        }
                            
                        IsVisibilityOption = true;
                        SetQuestion();
                    }
                }));
            }
        }
    }
}
