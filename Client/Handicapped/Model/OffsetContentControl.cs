using ModelDate;
using ModelDate.Controllers;
using ModelDate.Model.SignalR;
using ModelDate.Model.Table;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using VSHIM.Control.Handicapped.Signal;
using VSHIM.Control.Handicapped.Utilits;
using VSHIM.Control.Handicapped.View;
using VSHIM.Utilits;
using VSHIM.Utilits.DateManager;
using Xceed.Wpf.Toolkit;

namespace VSHIM.Control.Handicapped.Model
{
    class OffsetContentControl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<PerfectColor> PColorInversion = null;
        public UserStatus OperatorName = null;

        private RelayCommand setModeNormal;
        private RelayCommand navBack;
        private RelayCommand navNext;
        private RelayCommand playSound;
        private RelayCommand offsetContent;
        private RelayCommand colorInversion;
        private RelayCommand magnifier;
        private RelayCommand loadControl;
        private RelayCommand textZoom;
        private RelayCommand setTextZoom;
        private RelayCommand voice;
        private RelayCommand setVoice;
        private RelayCommand offsetAdd;
        private RelayCommand offsetSub;
        private RelayCommand closeZoomTextPopup;
        private RelayCommand setColorInversion;
        private RelayCommand popUpCloseInvert;
        private RelayCommand popUpCloseTextZoom;
        private RelayCommand popUpCloseVoice;
        private RelayCommand getChat;
        private RelayCommand getVideoChat;
        private RelayCommand getHelp;

        private bool isOffsetBtn = true;
        private bool isInfersion;
        private int panelClickNormal = 0;
        private int zoomMagnifier = 1;
        private bool isMagnifier;
        private bool istextZoom;
        private bool? isRadioButton = null;
        private bool isSelectBtnInvert = false;
        private bool isSelectBtnTextZoom = false;
        private bool isVoice;
        private bool isSelectVoiceSpeaker;
        private bool isSelectVoiceSound;
        private bool isModulEnabel;
        private bool isVideoConf;
        private bool isHelp;
        private bool isChat;
        private bool isOperator;

        private int handHeight;
        private int handWidth;
        private int navHeight;
        private int navWidth;

        private HandicappedSettings handicappedSettings;
        private Magnifier RefMagnifier = null;
        private ColientSignal ColientSignal = null;
        private ChatWindow _chatWindow = null;
        private CallHelpWindow _callHelpWindow = null;
        private ChatVideoWindow _chatVideoWindow = null;
        private static DispatcherTimer UpdateSettingTimer = new DispatcherTimer();
        private ListMod<UserStatus> listOperator = new ListMod<UserStatus>();
        private readonly VideoTranslation videoTranslation = null;
        private AudioMng mng = null;
        private BufferedWaveProvider bufferStream;
        WaveOut output = new WaveOut();

        public OffsetContentControl()
        {
            bufferStream = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            output.Init(bufferStream);
            output.Play();

            videoTranslation = new VideoTranslation();
            videoTranslation.NewFrame += VideoTranslation_NewFrame;
            // Событие загрузки данных
            TimoutSettingUpdate.UpdateSetting += TimoutSettingUpdate_UpdateSetting;
            GetData();
            listOperator.Changed += ListOperator_Changed;
            // Тут подключаемся к серверу видеосвязи
            ColientSignal = new ColientSignal();
            ColientSignal.Init();
            ColientSignal.OnConnect += ColientSignal_OnConnect;
            ColientSignal.ListOperator += ColientSignal_ListOperator;
            ColientSignal.Message += ColientSignal_Message;
            ColientSignal.StartVideoPrivate += ColientSignal_StartVideoPrivate;
            ColientSignal.ConnectOperator += ColientSignal_ConnectOperator;
            ColientSignal.UserDisconnected += ColientSignal_UserDisconnected;
            ColientSignal.CanselVideoPrivate += ColientSignal_CanselVideoPrivate;
            ColientSignal.VideoFrame += ColientSignal_VideoFrame;
            ColientSignal.CallVideoTookOperator += ColientSignal_CallVideoTookOperator;
            ColientSignal.RecoverAudio += ColientSignal_AudioRecover;
            ColientSignal.Connect();

            mng = new AudioMng();
            mng.input.DataAvailable += Input_DataAvailable;
        }

        private async void GetData()
        {
            HandicappedSettings handSettings = await WebApi.GetHandicappedSettings();

            if (handSettings != null)
            {
                handicappedSettings = handSettings;
                ListColorInversion = new ObservableCollection<PerfectColor>(handicappedSettings.HandicappedColors);
                IsModulEnabel = handicappedSettings.IsEnable;
            }
            else
            {
                handicappedSettings = new HandicappedSettings();
            }

            if (handicappedSettings.IdleMode)
            {
                int Minutes = System.Convert.ToInt32(handicappedSettings.IdleInterval);
                TouchTimoutManager.HandicappedTimeOut += TouchTimoutManager_HandicappedTimeOut;
                TouchTimoutManager.HandicappedOnStart(new TimeSpan(0, 0, Minutes, 0, 0));
            }

            BtnSizeUpdate(handSettings);
        }

        private void BtnSizeUpdate(HandicappedSettings Model)
        {
            try
            {
                HandHeight = Model.HundHeight;
                HandWidth = Model.HundWidth;
                NavHeight = Model.NavHeight;
                NavWidth = Model.NavWidth;
                IsModulEnabel = Model.IsEnable;
            }
            catch { }

        }

        #region Timout

        // Загрузка инвалидных настроек
        private async void TimoutSettingUpdate_UpdateSetting()
        {
            HandicappedSettings handicappedSettings = await WebApi.GetHandicappedSettings();

            if (handicappedSettings != null)
            {
                if (this.handicappedSettings.IdleMode != handicappedSettings.IdleMode)
                {
                    if (handicappedSettings.IdleMode)
                    {
                        int Minutes = System.Convert.ToInt32(handicappedSettings.IdleInterval);
                        TouchTimoutManager.HandicappedTimeOut += TouchTimoutManager_HandicappedTimeOut;
                        TouchTimoutManager.HandicappedOnStart(new TimeSpan(0, 0, Minutes, 0, 0));
                    }
                    else
                    {
                        TouchTimoutManager.HandicappedStop();
                    }
                }

                ListColorInversion = new ObservableCollection<PerfectColor>(handicappedSettings.HandicappedColors);
                this.handicappedSettings = handicappedSettings;
                BtnSizeUpdate(this.handicappedSettings);
            }
        }

        // Выключем инвалидный модуль
        private void TouchTimoutManager_HandicappedTimeOut()
        {
            if (IsTextZoom)
            {
                TextZoom.Execute(null);
            }

            if (IsMagnifier)
            {
                Magnifier.Execute(null);
            }

            if (IsInfersion)
            {
                ColorInversion.Execute(null);
            }

            if (PanelClickNormal > 0)
            {
                OffsetContent.Execute(null);
            }
        }

        #endregion

        #region Command

        public RelayCommand GetHelp
        {
            get
            {
                return getHelp ?? (getHelp = new RelayCommand(() => 
                {
                    IsHelp = !IsHelp;
                    if (IsHelp)
                    {
                        if (_callHelpWindow == null)
                        {
                            _callHelpWindow = new CallHelpWindow();
                            _callHelpWindow.CllHelp += _callHelpWindow_CllHelp;
                            _callHelpWindow.CanselHelp += _callHelpWindow_CanselHelp;
                        }

                        _callHelpWindow.Show();

                    }
                    else
                    {
                        _callHelpWindow.Hide();
                    }
                }));
            }
        }

        public RelayCommand GetVideoChat
        {
            get
            {
                return getVideoChat ?? (getVideoChat = new RelayCommand(() => 
                {
                    IsVideoConf = !IsVideoConf;
                    if(isVideoConf)
                    {
                        if(_chatVideoWindow == null)
                        {
                            _chatVideoWindow = new ChatVideoWindow();
                        }

                        _chatVideoWindow.Show();
                        _chatVideoWindow.Border_Loaded(this, null);
                        // Посылаем сигнал на вызов
                        ColientSignal.CallVideoChat();
                    }
                    else
                    {
                        bufferStream.ClearBuffer();
                        _chatVideoWindow.Hide();
                        // Отменяем сигнал на вызов
                        listOperator.Where(r => r.VideoPrivate == false)
                        .ToList()
                        .ForEach(x => 
                        {
                            ColientSignal.CanselVideoChat(x.ConnectionId);
                        });
 
                    }
                }));
            }
        }

        public RelayCommand GetChat
        {
            get
            {
                return getChat ?? (getChat = new RelayCommand(() => 
                {
                    IsChat = !IsChat;
                    if(IsChat)
                    {
                        if (_chatWindow == null)
                        {
                            _chatWindow = new ChatWindow();
                            _chatWindow.Msg += _chatWindow_Msg; 
                        }

                        _chatWindow.Clear();
                        _chatWindow.Show();
                        ColientSignal.SendOnChat(true);

                    }
                    else
                    {
                        _chatWindow.Hide();
                        ColientSignal.SendOnChat(false);
                    }
                }));
            }
        }

        public RelayCommand LoadControl
        {
            get
            {
                return loadControl ??
                    (loadControl = new RelayCommand(obj =>
                    {
                        Magnifier elm = obj as Magnifier;
                        if(elm != null)
                        {
                            RefMagnifier = elm;
                        }
                    }));
            }
        }

        public RelayCommand SetModeNormal
        {
            get
            {
                return setModeNormal ??
                    (setModeNormal = new RelayCommand(obj =>
                    {
                        if (PanelClickNormal != 0)
                        {
                            PanelClickNormal = 0;
                            IsOffsetBtn = true;
                        }
                    }));
            }
        }

        public RelayCommand NavBack
        {
            get
            {
                return navBack ??
                    (navBack = new RelayCommand(obj =>
                    {
                        KeyNavigator.NavBack();
                    }));
            }
        }

        public RelayCommand NavNext
        {
            get
            {
                return navNext ??
                    (navNext = new RelayCommand(obj =>
                    {
                        KeyNavigator.NavNext();
                    }));
            }
        }

        public RelayCommand PlaySound
        {
            get
            {
                return playSound ??
                    (playSound = new RelayCommand(obj =>
                    {
                        KeyNavigator.Enter();
                    }));
            }
        }

        public RelayCommand OffsetContent
        {
            get
            {
                return offsetContent ??
                    (offsetContent = new RelayCommand(obj =>
                    {
                        if(PanelClickNormal > 0)
                        {
                            PanelClickNormal = 0;
                            IsOffsetBtn = false;
                        }
                        else
                        {
                            PanelClickNormal = Utilits.HMatch.GetOffsetContent(handicappedSettings.OffsetTop);
                            IsOffsetBtn = true;
                        }
                       
                    }));
            }
        }

        public RelayCommand ColorInversion
        {
            get
            {
                return colorInversion ??
                    (colorInversion = new RelayCommand(obj =>
                    {
                        if (IsInfersion == false)
                        {
                            IsInfersion = true;
                          
                        }
                        else
                        {
                            InversionColor.OffInversion();
                            IsInfersion = false;
                            IsSelectBtnInvert = false;
                        }

                    }));
            }
        }

        public RelayCommand SetColorInversion
        {
            get
            {
                return setColorInversion ??
                    (setColorInversion = new RelayCommand(obj =>
                    {
                        if(obj is ModelDate.Model.ColorInversion e)
                        {
                            IsSelectBtnInvert = true;
                            InversionColor.OnInversion(e);
                        }
                    }));
            }
        }

        public RelayCommand Magnifier
        {
            get
            {
                return magnifier ?? (magnifier = new RelayCommand(obj =>
                {
                    if (IsMagnifier != true)
                    {
                        IsMagnifier = true;
                        RefMagnifier.Visibility = System.Windows.Visibility.Visible;
                        RefMagnifier.ZoomFactor = Utilits.HMatch.GetZoomMagnifier(handicappedSettings.MagnificationFactor);
                    }
                    else
                    {
                        RefMagnifier.Visibility = System.Windows.Visibility.Hidden;
                        IsMagnifier = false;
                    }
                }));
            }
        }

        public RelayCommand TextZoom
        {
            get
            {
                return textZoom ?? (textZoom = new RelayCommand(obj =>
                {
                    if (IsTextZoom == false)
                    {
                        IsTextZoom = true;
                    }
                    else
                    {
                        
                        VSHIM.Control.Handicapped.TextZoom.OffZoom();
                        IsSelectBtnTextZoom = false;
                        IsTextZoom = false;
                    }
                }));
            }
        }

        public RelayCommand SetTextZoom
        {
            get
            {
                return setTextZoom ?? (setTextZoom = new RelayCommand(obj =>
                {
                    if (obj != null)
                    {
                        int ZoomLvl = System.Convert.ToInt32(obj);

                        if (ZoomLvl != 0)
                        {
                            VSHIM.Control.Handicapped.TextZoom.OnZoom(ZoomLvl);
                            IsSelectBtnTextZoom = true;
                        }
                    }
                }));
            }
        }

        public RelayCommand Voice
        {
            get
            {
                return voice ?? (voice = new RelayCommand(obj =>
                {
                    if(IsVoice == false)
                    {
                        IsVoice = true;
                    }
                    else
                    {
                        IsHandButton.IsDoubleTouch = false;
                        DictationText.SpeekCansel();
                        IsVoice = false;
                        IsSelectVoiceSound = false;
                        IsSelectVoiceSpeaker = false;
                    }
                }));
            }
        }

        public RelayCommand SetVoice
        {
            get
            {
                return setVoice ??
                    (setVoice = new RelayCommand(obj =>
                    {
                        if(obj != null && obj is string str)
                        {
                            if(str == "Speaker")
                            {
                                IsSelectVoiceSound = true;
                                IsVoice = true;
                            }
                            else
                            {
                                IsSelectVoiceSpeaker = true;
                                IsVoice = true;
                            }
                        }

                        OnPropertyChanged("IsVoice");

                        if (IsSelectVoiceSound || IsSelectVoiceSpeaker)
                        {
                            IsHandButton.IsDoubleTouch = true;
                        }
                    }));
            }
        }

        public RelayCommand OffsetAdd
        {
            get
            {
                return offsetAdd ?? (offsetAdd = new RelayCommand(obj =>
                {
                    PanelClickNormal += 40;
                }));
            }
        }

        public RelayCommand OffsetSub
        {
            get
            {
                return offsetSub ?? (offsetSub = new RelayCommand(obj =>
                {
                    PanelClickNormal -= 40;
                }));
            }
        }

        public RelayCommand CloseZoomTextPopup
        {
            get
            {
                return closeZoomTextPopup ?? (closeZoomTextPopup = new RelayCommand(obj =>
                {
                    IsRadioButton = false;
                }));
            }
        }

        public RelayCommand PopUpCloseInvert
        {
            get
            {
                return popUpCloseInvert ?? (popUpCloseInvert = new RelayCommand(obj =>
                {
                    if(!IsSelectBtnInvert)
                    {
                        IsInfersion = false;
                    }

                    OnPropertyChanged("IsSelectBtnInvert");
                }));
            }
        }

        public RelayCommand PopUpCloseTextZoom
        {
            get
            {
                return popUpCloseTextZoom ?? (popUpCloseTextZoom = new RelayCommand(obj =>
                {
                    if (!IsSelectBtnTextZoom)
                    {
                        IsTextZoom = false;
                    }

                    OnPropertyChanged("IsSelectBtnTextZoom");
                }));
            }
        }

        public RelayCommand PopUpCloseVoice
        {
            get
            {
                return popUpCloseVoice ?? (popUpCloseVoice = new RelayCommand(obj =>
                {
                    if (IsSelectVoiceSpeaker == false && IsSelectVoiceSound == false)
                    {
                        IsVoice = false;
                    }
                }));
            }
        }

        #endregion

        #region Data 

        public bool IsOperator
        {
            get => isOperator;
            set
            {
                isOperator = value;
                OnPropertyChanged("IsOperator");
            }
        }

        public bool IsChat
        {
            get => isChat;
            set
            {
                if(isChat != value)
                {
                    isChat = value;
                    OnPropertyChanged("IsChat");
                }
            }
        }

        public bool IsHelp
        {
            get => isHelp;
            set
            {
                if(isHelp != value)
                {
                    isHelp = value;
                    OnPropertyChanged("IsHelp");
                }
            }
        }

        public bool IsVideoConf
        {
            get => isVideoConf;
            set
            {
                if(isVideoConf != value)
                {
                    isVideoConf = value;
                    OnPropertyChanged("IsVideoConf");
                }
            }
        }

        public int PanelClickNormal
        {
            get
            {
                return panelClickNormal < 0 ? 0 : panelClickNormal;
            }
            set
            {
                if (panelClickNormal != value)
                {
                    panelClickNormal = value;
                    OnPropertyChanged("PanelClickNormal");
                }
            }
        }

        public bool IsOffsetBtn
        {
            get
            {
                return isOffsetBtn;
            }
            set
            {
                if (isOffsetBtn != value)
                {
                    isOffsetBtn = value;
                    OnPropertyChanged("IsOffsetBtn");
                }
            }
        }

        public bool IsInfersion
        {
            get
            {
                return isInfersion;
            }
            set
            {
                if (isInfersion != value)
                {
                    isInfersion = value;
                    OnPropertyChanged("IsInfersion");
                }
            }
        }

        public int ZoomMagnifier
        {
            get
            {
                return zoomMagnifier;
            }

            private set
            {
                if(zoomMagnifier != value)
                {
                    zoomMagnifier = value;
                    OnPropertyChanged("ZoomMagnifier");
                }
            }
        }

        public bool IsMagnifier
        {
            get
            {
                return isMagnifier;
            }
            set
            {
                if(isMagnifier != value)
                {
                    isMagnifier = value;
                    OnPropertyChanged("IsMagnifier");
                }
            }
        }

        public bool IsTextZoom
        {
            get
            {
                return istextZoom;
            }
            set
            {
                if(istextZoom != value)
                {
                    istextZoom = value;
                    OnPropertyChanged("IsTextZoom");
                }
            }
        }

        public bool? IsRadioButton
        {
            get
            {
                return isRadioButton;
            }
            set
            {
                if (isRadioButton != null)
                {
                    isRadioButton = value;
                    OnPropertyChanged("IsRadioButton");
                }
            }
        }

        public bool IsSelectBtnInvert
        {
            get
            {
                return isSelectBtnInvert;
            }
            set
            {
                if(isSelectBtnInvert != value)
                {
                    isSelectBtnInvert = value;
                    OnPropertyChanged("IsSelectBtnInvert");
                }
            }
        }

        public bool IsSelectBtnTextZoom
        {
            get
            {
                return isSelectBtnTextZoom;
            }
            set
            {
                if(isSelectBtnTextZoom != value)
                {
                    isSelectBtnTextZoom = value;
                    OnPropertyChanged("IsSelectBtnTextZoom");
                }
            }
        }

        public bool IsVoice
        {
            get
            {
                return isVoice;
            }
            set
            {
                if(isVoice != value)
                {
                    isVoice = value;
                    OnPropertyChanged("IsVoice");
                }
            }
        }

        public bool IsSelectVoiceSpeaker
        {
            get
            {
                return isSelectVoiceSpeaker;
            }
            set
            {
                if(isSelectVoiceSpeaker != value)
                {
                    isSelectVoiceSpeaker = value;
                    OnPropertyChanged("IsSelectVoiceSpeaker");
                }
            }
        }

        public bool IsSelectVoiceSound
        {
            get
            {
                return isSelectVoiceSound;
            }
            set
            {
                if(isSelectVoiceSound != value)
                {
                    isSelectVoiceSound = value;
                    OnPropertyChanged("IsSelectVoiceSound");
                }
            }
        }

        public int HandHeight
        {
            get
            {
                return handHeight;
            }
            set
            {
                if(handHeight != value)
                {
                    handHeight = value;
                    OnPropertyChanged("HandHeight");
                }
            }
        }

        public int HandWidth
        {
            get
            {
                return handWidth;
            }
            set
            {
                if(handWidth != value)
                {
                    handWidth = value;
                    OnPropertyChanged("HandWidth");
                }
            }
        }

        public int NavHeight
        {
            get
            {
                return navHeight;
            }
            set
            {
                if(navHeight != value)
                {
                    navHeight = value;
                    OnPropertyChanged("NavHeight");
                }
            }
        }

        public int NavWidth
        {
            get
            {
                return navWidth;
            }
            set
            {
                if(navWidth != value)
                {
                    navWidth = value;
                    OnPropertyChanged("NavWidth");
                }
            }
        }

        public ObservableCollection<PerfectColor> ListColorInversion
        {
            get
            {
                return PColorInversion;
            }
            set
            {
                if (PColorInversion != value)
                {
                    PColorInversion = value;
                    OnPropertyChanged("ListColorInversion");
                }
            }

        }

        public bool IsModulEnabel
        {
            get
            {
                return isModulEnabel;
            }
            set
            {
                if(isModulEnabel != value)
                {
                    isModulEnabel = value;
                    OnPropertyChanged("IsModulEnabel");

                    if(value == false)
                    {
                        TouchTimoutManager_HandicappedTimeOut();
                    }
                }
            }
        }

        #endregion

        #region Event

        // Если подключился
        private void ColientSignal_OnConnect(bool obj)
        {
            ColientSignal.GetUserList();
        }

        private void ColientSignal_ListOperator(List<User> obj)
        {
            listOperator.Clear();
            listOperator.AddRange(obj.Select(p=> new UserStatus {
                 ConnectionId = p.ConnectionId,
                 IsChatOpen = p.IsChatOpen,
                 IsWebCam = p.IsWebCam,
                 Name = p.Name,
                 RoomName = p.RoomName,
                 Type = p.Type,
                 VideoTranslation = false,
                 VideoPrivate = false
            }));
        }

        private void ColientSignal_Message(string msg, string id)
        {
            UserStatus elm = listOperator.Where(r => r.ConnectionId == id).FirstOrDefault();
            if(elm != null)
            {
                _chatWindow.Message(msg, elm.Name);
            }
        }

        private void _chatWindow_Msg(string msg)
        {
            ColientSignal.Send(msg);
        }

        private void _callHelpWindow_CllHelp()
        {
            ColientSignal.CallHelp();
        }

        private void _callHelpWindow_CanselHelp()
        {
            ColientSignal.CanselHelp();
        }

        private void ColientSignal_StartVideoPrivate(string id)
        {
            listOperator.Where(w => w.ConnectionId == id)
                .ToList()
                .ForEach(x => 
                {
                    x.VideoTranslation = true;
                    x.VideoPrivate = true;
                });

            // Уведомляем коллекцию, что произошли изменения
            listOperator.Changeds();
        }

        private void ColientSignal_ConnectOperator(User obj)
        {
            listOperator.Add(new UserStatus()
            {
                 ConnectionId = obj.ConnectionId,
                 IsChatOpen = obj.IsChatOpen,
                 IsWebCam = obj.IsWebCam,
                 Name = obj.Name,
                 RoomName = obj.RoomName,
                 Type = obj.Type,
                 VideoTranslation = false,
                 VideoPrivate = false
            });
        }

        // Отслеживание коллекции
        private void ListOperator_Changed(object sender, EventArgs e)
        {
            if (listOperator.Where(r => r.VideoTranslation).Any())
            {
                try
                {
                    if (!mng.IsStart)
                        mng.Start();

                    videoTranslation.Start();
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    videoTranslation.Stop();
                    if (mng.IsStart)
                        mng.Stop();
                }
                catch
                {

                }
            }   

            if(!listOperator.Any())
            {
                IsOperator = false;
            }
            else
            {
                IsOperator = true;
            }
        }

        // Фрейм от текущей камеры
        private void VideoTranslation_NewFrame(byte[] img)
        {
            listOperator.Where(r => r.VideoTranslation)
                .ToList()
                .ForEach(x => ColientSignal.Frame(x.ConnectionId, img));
        }

        private void ColientSignal_UserDisconnected(string id)
        {
            listOperator.Remove(listOperator.Where(r=>r.ConnectionId == id).FirstOrDefault());
        }

        // Оператор сбросил
        private async void ColientSignal_CanselVideoPrivate(string id)
        {
            if(OperatorName != null && OperatorName.ConnectionId == id)
            {
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    if(isVideoConf)
                        GetVideoChat.Execute(null);
                }));
            }

            listOperator.Where(r => r.ConnectionId == id)
                .ToList()
                .ForEach(x => 
                {
                    x.VideoTranslation = false;
                    x.VideoPrivate = false;
                });

            listOperator.Changeds();
        }

        private void Input_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (e.Buffer != null)
            {
                listOperator.Where(r => r.VideoTranslation)
                .ToList()
                .ForEach(x => ColientSignal.Audio(x.ConnectionId, e.Buffer));
            }
        }

        // Фрейм от клиента
        private void ColientSignal_VideoFrame(byte[] buff)
        {
            if (_chatVideoWindow != null)
            {
                _chatVideoWindow.VideoFrame(buff);
            }
        }

        // Оператор ответил
        private void ColientSignal_CallVideoTookOperator(string Id)
        {
            OperatorName = listOperator.FirstOrDefault(r => r.ConnectionId == Id);

            listOperator.Where(r => r.ConnectionId == Id)
                .ToList()
                .ForEach(x => x.VideoTranslation = true);

            listOperator.Changeds();

            if(OperatorName != null && !string.IsNullOrEmpty(OperatorName.Name))
                _chatVideoWindow.SetOperatorName(OperatorName.Name);
        }

        // Получаем ауиди поток
        private void ColientSignal_AudioRecover(byte[] buff)
        {
            try
            {
                bufferStream.AddSamples(buff, 0, buff.Length);
            }
            catch
            {

            }
        }
        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
