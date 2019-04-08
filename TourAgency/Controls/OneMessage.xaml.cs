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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для OneMessage.xaml
    /// </summary>
    public partial class OneMessage : UserControl
    {
        public OneMessage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Указывает, является ли отправителем данного сообщения пользователь. true, если да
        /// </summary>
        public bool IsMine
        {
            get { return (bool)GetValue(IsMineProperty); }
            set { SetValue(IsMineProperty, value); }
        }
        public String MessageText
        {
            get { return (String)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }
        public DateTime MessageDate
        {
            get { return (DateTime)GetValue(MessageDateProperty); }
            set { SetValue(MessageDateProperty, value); }
        }

        public static readonly DependencyProperty IsMineProperty =
            DependencyProperty.Register("IsMine", typeof(bool), typeof(OneMessage), new PropertyMetadata(false));
        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register("MessageText", typeof(String), typeof(OneMessage), new PropertyMetadata(String.Empty));
        public static readonly DependencyProperty MessageDateProperty =
            DependencyProperty.Register("MessageDate", typeof(DateTime), typeof(OneMessage), new PropertyMetadata(new DateTime()));
    }
}
