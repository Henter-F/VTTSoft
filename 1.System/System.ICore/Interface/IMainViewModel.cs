using System;
using System.Collections.Generic;
using System.ICore.ConfigSave;
using System.ICore.Enum;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.Interface
{
    public interface IMainViewModel
    {
        event Action Close;

        ConfigDat Config { get; }

        string CurUseFormulaName { get; set; }

        string CurUserName { get; set; }

        string CurUserNameAndLevel { get; set; }

        PrivilegeLevelType CurLevelType { get; set; }

        IMainWindow MainView { get; }

        bool Init();




        void ExecuteMainClose();
    }
}
