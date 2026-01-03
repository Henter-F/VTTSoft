using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ICore.ConfigSave;
using System.ICore.Enum;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Common.Login.Config
{
    [Serializable]
    public class LoginConfig : ConfigDat
    {
        /// <summary>
        /// 用户的集合
        /// </summary>
        public ObservableCollection<User> UserItem { get; set; } = new ObservableCollection<User>();

        /// <summary>
        /// 多长时间没有操作权限退出到操作员权限
        /// </summary>
        public int FreeMinute { get; set; } = 10;
    }

    [Serializable]
    public class User
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 权限等级
        /// </summary>
        public PrivilegeLevelType LevelType { get; set; } = PrivilegeLevelType.操作员;

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
