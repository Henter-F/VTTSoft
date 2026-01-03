using System;
using System.Collections.Generic;
using System.ICore.ConfigSave;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace System.ICore.Interface
{
    public interface IProjectPanelViewModel
    {
        string Title { get; }

        ImageSource Source { get; }

        ConfigDat Config { get; }

        UserControl ProjectPanelView { get; }

        List<string> WorkListName { get; }


        IProjectCenterArea CenterArea { get; set; }

        List<IProjectSubArea> SubAreas { get; set; }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <returns></returns>
        bool Init();

        /// <summary>
        /// 开启工作流
        /// </summary>
        void ExecuteStartWorkCommand();

        /// <summary>
        /// 停止工作流
        /// </summary>
        void ExecuteStopWorkCommand();

        /// <summary>
        /// 打开解决方案
        /// </summary>
        void ExecuteOpenProjectCommand();

        /// <summary>
        /// 打开构造器
        /// </summary>
        void ExecuteOpenConstructorCommand();

        /// <summary>
        /// 打开释放器
        /// </summary>
        void ExecuteOpenReleaserCommand();
    }
}
