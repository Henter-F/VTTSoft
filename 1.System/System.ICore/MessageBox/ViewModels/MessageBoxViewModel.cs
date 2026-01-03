using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace System.ICore.MessageBox.ViewModels
{
    public class MessageBoxViewModel : BaseViewModel
    {
        #region 私有变量
        private bool _showOk = false;

        private bool _showCancel = false;

        private bool _showYes = false;

        private bool _showNo = false;

        private string _iconString = null;

        private Action _onCloseAction = null;

        private MessageBoxButton _messageBoxButton = MessageBoxButton.OK;

        private MessageBoxImage _messageBoxImage = MessageBoxImage.Information;
        #endregion

        #region 属性
        private string m_Caption;
        public string Caption
        {
            get { return m_Caption; }
            set
            {
                if (m_Caption != value)
                {
                    m_Caption = value;
                    OnPropertyChanged("Caption");
                }
            }
        }

        private string m_Message;
        public string Message
        {
            get { return m_Message; }
            set
            {
                if (m_Message != value)
                {
                    m_Message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private Style m_ImageStyle;
        public Style ImageStyle
        {
            get { return m_ImageStyle; }
            set
            {
                if (m_ImageStyle != value)
                {
                    m_ImageStyle = value;
                    OnPropertyChanged("ImageStyle");
                }
            }
        }

        #endregion

        #region 公共变量
        public MessageBoxResult MessageBoxResult = MessageBoxResult.Cancel;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageBoxButton">指定显示按钮</param>
        /// <param name="messageBoxImage">指定显示图标</param>
        /// <param name="closeAction">点击按钮时触发的方法</param>
        public MessageBoxViewModel(MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, Action onClose)
        {
            _messageBoxButton = messageBoxButton;
            _messageBoxImage = messageBoxImage;
            _onCloseAction = onClose;
            SetDisplayButton();
            SetDisplayIcon();

            HitOK = new SimpleCommand(a => { return _showOk; }, OnHitOK);
            HitCancel = new SimpleCommand(a => { return _showCancel; }, OnHitCancel);
            HitYes = new SimpleCommand(a => { return _showYes; }, OnHitYes);
            HitNo = new SimpleCommand(a => { return _showNo; }, OnHitNo);
        }
        public ICommand HitOK { get; private set; }
        private void OnHitOK(object obj)
        {
            MessageBoxResult = MessageBoxResult.OK;
            _onCloseAction?.Invoke();
        }
        public ICommand HitCancel { get; private set; }
        private void OnHitCancel(object obj)
        {
            MessageBoxResult = MessageBoxResult.Cancel;
            _onCloseAction?.Invoke();
        }
        public ICommand HitYes { get; private set; }
        private void OnHitYes(object obj)
        {
            MessageBoxResult = MessageBoxResult.Yes;
            _onCloseAction?.Invoke();
        }
        public ICommand HitNo { get; private set; }
        private void OnHitNo(object obj)
        {
            MessageBoxResult = MessageBoxResult.No;
            _onCloseAction?.Invoke();
        }
        private void SetDisplayButton()
        {
            switch (_messageBoxButton)
            {
                case MessageBoxButton.OK:
                    _showOk = true;
                    break;
                case MessageBoxButton.OKCancel:
                    _showOk = true;
                    _showCancel = true;
                    break;
                case MessageBoxButton.YesNo:
                    _showYes = true;
                    _showNo = true;
                    break;
                case MessageBoxButton.YesNoCancel:
                    _showYes = true;
                    _showNo = true;
                    _showCancel = true;
                    break;
            }
        }
        private void SetDisplayIcon()
        {
            switch (_messageBoxImage)
            {
                case MessageBoxImage.Information:
                    _iconString = nameof(MessageBoxImage.Information);
                    break;
                case MessageBoxImage.Warning:
                    _iconString = nameof(MessageBoxImage.Warning);
                    break;
                case MessageBoxImage.Question:
                    _iconString = nameof(MessageBoxImage.Question);
                    break;
                case MessageBoxImage.Error:
                    _iconString = nameof(MessageBoxImage.Error);
                    break;
            }

            if (!string.IsNullOrEmpty(_iconString))
            {
                ImageStyle = GetResource<Style>();
            }
        }
        private T GetResource<T>()
        {
            if (Application.Current.TryFindResource(_iconString) is T resource)
            {
                return resource;
            }
            return default;
        }
    }
}
