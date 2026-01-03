using System;
using System.Collections.Generic;
using System.ICore.MessageBox.ViewModels;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.ICore.MessageBox.Helper
{
    public class MessageBoxHelper
    {


        private static Views.MessageBox CreateMessageBox(Window owner, string messageBoxText, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, MessageBoxResult defaultResult, int showingTime = 0)
        {
            Views.MessageBox messageBox = new Views.MessageBox(showingTime);
            if (messageBox.WindowStartupLocation == WindowStartupLocation.CenterOwner)
            {
                var ownerWindow = owner ?? Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                messageBox.Owner = ownerWindow;
                messageBox.Topmost = ownerWindow is null;
            }
            MessageBoxViewModel messageBoxHelper = new MessageBoxViewModel(messageBoxButton, messageBoxImage, () => { messageBox.Close(); })
            {
                Message = messageBoxText,
                Caption = caption ?? "系统消息",
                MessageBoxResult = defaultResult
            };
            messageBox.DataContext = messageBoxHelper;

            return messageBox;
        }

        public static MessageBoxResult ShowInfo(string messageBoxText, string caption = null, int showingTime = 0)
        {
            return Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, showingTime);
        }

        public static MessageBoxResult ShowAsk(string messageBoxText, string caption = null)
        {
            return Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        }

        public static MessageBoxResult ShowAskWithCancel(string messageBoxText, string caption = null)
        {
            return Show(messageBoxText, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
        }

        public static MessageBoxResult ShowError(string messageBoxText, string caption = "错误")
        {
            return Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        public static MessageBoxResult ShowWarning(string messageBoxText, string caption = "警告")
        {
            return Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption = null, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, int showingTime = 0)
        {
            Views.MessageBox messageBox = null;
            MessageBoxViewModel messageBoxHelper = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, messageBoxButton, messageBoxImage, defaultResult, showingTime);
                messageBoxHelper = messageBox.DataContext as MessageBoxViewModel;
                switch (messageBoxImage)
                {
                    case MessageBoxImage.Information:
                        SystemSounds.Asterisk.Play();
                        break;
                    case MessageBoxImage.Warning:
                        SystemSounds.Exclamation.Play();
                        break;
                    case MessageBoxImage.Question:
                        SystemSounds.Question.Play();
                        break;
                    case MessageBoxImage.Error:
                        SystemSounds.Hand.Play();
                        break;
                    default:
                        SystemSounds.Asterisk.Play();
                        break;
                }
                messageBox.ShowDialog();
            });
            return messageBoxHelper.MessageBoxResult;
        }

    }   
}
