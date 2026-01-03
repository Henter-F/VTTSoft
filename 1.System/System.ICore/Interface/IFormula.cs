using System;
using System.Collections.Generic;
using System.ICore.ConfigSave;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.ICore.Interface
{
    public interface IFormula
    {
        ConfigDat Config { get; }

        string CurUseFormula { get; set; }

        Window View { get; }

        bool Init();
    }
}
