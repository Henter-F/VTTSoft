using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Common.Login.Config;
using System.Common.Login.Control;
using System.ICore.ConfigSave;
using System.ICore.Enum;
using System.ICore.Interface;
using System.ICore.Log;
using System.ICore.MessageBox.Helper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.Common.Login.Model
{
    public class LoginViewModel : ObservableObject, ILoginViewModel
    {
        /// <summary>
        /// 退出事件
        /// </summary>
        public event Action Close;
        /// <summary>
        ///  参数配置
        /// </summary>
        public ConfigDat Config { get { return config; } set { if (value is LoginConfig config1) config = config1; } }
        private LoginConfig config = new LoginConfig();


        /// <summary>
        /// 用户集合
        /// </summary>
        public ObservableCollection<string> UserNameItem
        {
            get
            {
                return new ObservableCollection<string>(config.UserItem.ToList().Select(p => p.UserName).ToList());
            }
        }

        /// <summary>
        /// 空闲时间
        /// </summary>
        public int FreeMinute { get => config.FreeMinute; }

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
        /// 当前登录用户
        /// </summary>
        public string CurUserName
        {
            get => _CurUserName; set
            {
                SetProperty(ref _CurUserName, value);
            }
        }
        private string _CurUserName = string.Empty;

        /// <summary>
        /// 当前登录用户权限
        /// </summary>
        public PrivilegeLevelType CurUserLevel
        {
            get => _CurUserLevel; set
            {
                SetProperty(ref _CurUserLevel, value);
            }
        }
        private PrivilegeLevelType _CurUserLevel = PrivilegeLevelType.操作员;

        /// <summary>
        /// 登录权限发生改变
        /// </summary>
        public Action<string, PrivilegeLevelType> UserLevelChangeEnvent { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        public RelayCommand LogOnCommand { get; set; }

        public UserManagementViewModel Model { get; set; }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public Window View
        {
            get
            {
                if (_view == null || _view.IsClosed)
                {
                    _view = new LoginView();
                    _view.DataContext = this;
                    InputUserName = string.Empty;
                    InputPassWord = string.Empty;
                }
                return _view;
            }
        }
        private LoginView _view = null;

        public LoginViewModel()
        {
            try
            {
                Init();
                LogOnCommand = new RelayCommand(ExecuteLogOnCommand);
                Model = new UserManagementViewModel(this);
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// 日志参数初始化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                config = new LoginConfig
                {
                    Folder = "System",
                    Name = "用户配置",
                };
                config = config.Read<LoginConfig>();
                User user = config.UserItem.FirstOrDefault(p => p.UserName == "SuperAdministrator");
                if (user == null) config.UserItem.Add(new User() { UserName = "SuperAdministrator", PassWord = "121121", LevelType = PrivilegeLevelType.超级管理员, PhoneNumber = "13712027528" });
                User user1 = config.UserItem.FirstOrDefault(p => p.UserName == "Operator");
                if (user1 == null) config.UserItem.Add(new User() { UserName = "Operator", PassWord = "123", LevelType = PrivilegeLevelType.操作员, PhoneNumber = "123456789" });

            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
                return false;
            }
            return true;
        }

        public void ExecuteLogOnCommand()
        {
            try
            {
                bool rst = LogIn(InputUserName, InputPassWord);
                if (rst) { UIMessageTip.ShowOk("登录成功!!!"); Close?.Invoke(); }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        public bool LogIn(string UserName, string PassWord)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PassWord))
                {
                    UIMessageTip.ShowWarning("用户名或者密码为空,请检查!!!", 2000);
                    return false;
                }
                User user = config.UserItem.FirstOrDefault(p => p.UserName == UserName.Trim());
                if (user == null)
                {
                    UIMessageTip.ShowWarning("无当前用户,请检查!!!", 2000);
                    return false;
                }
                else
                {
                    if (user.PassWord == PassWord.Trim())
                    {
                        CurUserName = user.UserName;
                        CurUserLevel = user.LevelType;
                        UserLevelChangeEnvent?.Invoke(CurUserName, CurUserLevel);
                        //UIMessageTip.ShowOk("登录成功!!!");
                        return true;
                    }
                    else
                    {
                        UIMessageTip.ShowWarning("密码输入错误,请检查!!!", 2000);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <param name="UserLevel"></param>
        /// <param name="PhoneNumber"></param>
        public void AddUser(string UserName, string PassWord, PrivilegeLevelType UserLevel, string PhoneNumber)
        {
            try
            {
                if (config.UserItem.FirstOrDefault(p => p.UserName == UserName.Trim()) != null)
                {
                    UIMessageTip.ShowWarning("当前用户已经存在,无法重复添加!!!", 3000);
                    return;
                }
                else
                {
                    User user = new User()
                    {
                        UserName = UserName,
                        PassWord = PassWord,
                        LevelType = UserLevel,
                        PhoneNumber = PhoneNumber
                    };
                    config.UserItem.Add(user);
                }

            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="UserName"></param>
        public void RemoveUser(string UserName)
        {
            try
            {
                User user = config.UserItem.FirstOrDefault(p => p.UserName == UserName.Trim());
                if (user != null)
                {
                    MessageBoxResult result = MessageBoxHelper.ShowAsk($"是否删除用户{UserName}", "删除");
                    if (result == MessageBoxResult.Yes)
                    {
                        config.UserItem.Remove(user);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }


        /// <summary>
        /// 保存参数
        /// </summary>
        public void SaveLoginConfigCommand()
        {
            try
            {
                if (Config?.Save() == true)
                {
                    UIMessageTip.ShowOk("参数保存成功");
                }
                else
                {
                    UIMessageTip.ShowError("参数保存失败");
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }
    }
}
