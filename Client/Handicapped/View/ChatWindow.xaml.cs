using ModelDate.Model.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using System.Windows.Shapes;

namespace VSHIM.Control.Handicapped.View
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<MsgChatModel> _msgChat = null;
        public event Action<string> Msg = delegate { };

        public ChatWindow()
        {
            InitializeComponent();
            MsgChat = new ObservableCollection<MsgChatModel>();
            DataContext = this;
        }

        public ObservableCollection<MsgChatModel> MsgChat
        {
            get => _msgChat;
            set
            {
                if(_msgChat != value)
                {
                    _msgChat = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Clear()
        {
            MsgChat.Clear();
        }

        public void Message(string msg, string Name)
        {
            Dispatcher.Invoke(() => {
                Name = $"От: {Name}";
                MsgChat.Add(new MsgChatModel()
                {
                    Msg = msg,
                    MsgType = MsgType.Received,
                    Name = Name 
                });

                view_scrol.ScrollToEnd();
            });
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string text = GetString(msg_text);

            if (string.IsNullOrEmpty(text) || text.Trim().Length <= 0)
                return;

            Msg(GetString(msg_text));
            MsgChat.Add(new MsgChatModel()
            {
                 Msg = GetString(msg_text),
                 MsgType = MsgType.Shipped
            });

            msg_text.Document.Blocks.Clear();
            view_scrol.ScrollToEnd();
        }

        string GetString(RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text.Replace("\r\n", "");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
