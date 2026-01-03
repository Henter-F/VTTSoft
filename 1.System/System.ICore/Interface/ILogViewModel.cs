using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ICore.ConfigSave;
using System.ICore.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace System.ICore.Interface
{
    public interface ILogViewModel
    {
        string Title { get; }

        ImageSource Source { get; set; }

        ConfigDat Config { get; }

        UserControl LogView { get; }

        ObservableCollection<PageModel> PageModels { get; set; }

        int SaveLogIndate { get; }

        bool Init();

        void UpdateLogView();

        void ExecuteOpenLogSetting();
    }
}
