using System;
using System.Collections.Generic;
using System.ICore.ConfigSave;
using System.ICore.Enum;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.ICore.Interface
{
    public interface ILoginViewModel
    {
        event Action Close;

        ConfigDat Config { get; }

        int FreeMinute { get; }

        string CurUserName { get; set; }

        PrivilegeLevelType CurUserLevel { get; set; }

        Action<string, PrivilegeLevelType> UserLevelChangeEnvent { get; set; }

        Window View { get; }

        bool Init();

        bool LogIn(string UserName, string PassWord);

        void AddUser(string UserName, string PassWord, PrivilegeLevelType UserLevel, string PhoneNumber);

        void RemoveUser(string UserName);

        void SaveLoginConfigCommand();
    }
}
