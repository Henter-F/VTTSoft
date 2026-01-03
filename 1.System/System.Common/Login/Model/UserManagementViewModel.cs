using System;
using System.Collections.Generic;
using System.Common.Login.Config;
using System.Common.Login.Control;
using System.ICore.ConfigSave;
using System.ICore.Enum;
using System.ICore.Interface;
using System.ICore.Log;
using System.ICore.MenuItemSet;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UIShell.OSGi;
using Sunny.UI;

namespace System.Common.Login.Model
{
    public class UserManagementViewModel : ObservableObject
    {

        private ILoginViewModel Model = null;

        private UserManagementView View = null;

        /// <summary>
        ///  参数配置
        /// </summary>
        public ConfigDat Config { get { return config; } set { if (value is LoginConfig config1) config = config1; } }
        private LoginConfig config = new LoginConfig();

        /// <summary>
        /// 输入的用户
        /// </summary>
        public string InputUserName
        {
            get => _InputUserName; set
            {
                SetProperty(ref _InputUserName, value);
            }
        }
        private string _InputUserName = string.Empty;


        /// <summary>
        /// 输入的密码
        /// </summary>
        public string InputPassWord
        {
            get => _IputPassWord; set
            {
                SetProperty(ref _IputPassWord, value);
            }
        }
        private string _IputPassWord = string.Empty;

        /// <summary>
        /// 输入用户权限
        /// </summary>
        public int InputLevel
        {
            get => _InputLevel; set
            {
                SetProperty(ref _InputLevel, value);
            }
        }
        private int _InputLevel = (int)PrivilegeLevelType.操作员;

        /// <summary>
        /// 输入电话号码
        /// </summary>
        public string InputPhoneNumber
        {
            get => _InputPhoneNumber; set
            {
                SetProperty(ref _InputPhoneNumber, value);
            }
        }
        private string _InputPhoneNumber = string.Empty;

        /// <summary>
        /// 选择的用户
        /// </summary>
        public User SelectUser
        {
            get => _SelectUser; set
            {
                SetProperty(ref _SelectUser, value);
            }
        }
        private User _SelectUser = null;

        /// <summary>
        /// 添加用户
        /// </summary>
        public RelayCommand AddUserCommand { get; set; }

        /// <summary>
        /// 移除用户
        /// </summary>
        public RelayCommand RemoveUserCommand { get; set; }

        /// <summary>
        /// 保存参数
        /// </summary>
        public RelayCommand SaveConfigCommand { get; set; }


        public UserManagementViewModel(ILoginViewModel _model)
        {
            try
            {
                Model = _model;
                if (Model == null || Model?.Config == null) { UIMessageTip.ShowError("获取登录参数失败!!!"); return; }
                Config = Model?.Config;
                AddUserCommand = new RelayCommand(() =>
                {
                    Model?.AddUser(InputUserName, InputPassWord, (PrivilegeLevelType)InputLevel, InputPhoneNumber);
                });
                RemoveUserCommand = new RelayCommand(() =>
                {
                    if (SelectUser == null) return;
                    Model?.RemoveUser(SelectUser.UserName);
                });
                SaveConfigCommand = new RelayCommand(() =>
                {
                    Model?.SaveLoginConfigCommand();
                });
                UpdataMenuItem();
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }



        public void UpdataMenuItem()
        {
            try
            {
                MenuItem menuItem1 = new MenuItem();
                menuItem1.Header = "用户管理";
                menuItem1.Height = 30;
                Image image2 = new Image();
                image2.Source = new BitmapImage(new Uri("pack://application:,,,/System.ICore;Component/Image/用户管理.png"));
                menuItem1.VerticalContentAlignment = VerticalAlignment.Center;
                menuItem1.HorizontalContentAlignment = HorizontalAlignment.Center;
                menuItem1.Icon = image2;
                menuItem1.Click += MenuItem2_Click;
                MenuItemSeting.AddMenuItem(MenuStyle.设置, menuItem1);
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }


        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (View == null || View.IsClosed)
                {
                    var ModelView = BundleRuntime.Instance?.GetFirstOrDefaultService<ILoginViewModel>();
                    if (ModelView.CurUserLevel < PrivilegeLevelType.管理员) { UIMessageTip.ShowWarning("当前用户权限等级不够,无法打开!!!", 2000); return; }
                    View = new UserManagementView();
                    View.DataContext = this;
                    InputUserName = string.Empty;
                    InputPassWord = string.Empty;
                    View.Show();
                }
                else
                {
                    View.Activate();
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

    }
}
