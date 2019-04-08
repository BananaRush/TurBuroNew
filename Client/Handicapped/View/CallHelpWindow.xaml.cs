using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VSHIM.Utilits;

namespace VSHIM.Control.Handicapped.View
{
    /// <summary>
    /// Логика взаимодействия для CallHelpWindow.xaml
    /// </summary>
    public partial class CallHelpWindow : Window
    {
        private bool _ishelp = false;
        private string str1 = "Оператор ожидает вызова для помощи";
        private string str2 = "Оператор выдвинулся к вам на помощь";
        private RelayCommand _call;
        private bool IsFlag = false;
        public event Action CllHelp = delegate { };
        public event Action CanselHelp = delegate { };

        public CallHelpWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public RelayCommand Call
        {
            get
            {
                return _call ?? (_call = new RelayCommand(async sender => 
                {
                    if(!IsFlag)
                    {
                        _ishelp = !_ishelp;
                        
                        if (sender is HandButton button)
                        {
                            if (_ishelp)
                            {
                                CllHelp();
                                text_help.Text = str2;
                                button.Content = "Отменить вызов оператора";
                            }
                            else
                            {
                                CanselHelp();
                                button.Content = "Вызвать оператора";
                                text_help.Text = str1;
                            }
                        }

                        IsFlag = true;
                        await Task.Delay(3000);
                        IsFlag = false;
                    }
                }));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            text_help.Text = str1;
        }
    }
}
