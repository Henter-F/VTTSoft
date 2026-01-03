using System;
using System.Collections.Generic;
using System.ICore.ConfigSave;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.MainView.Config
{
    [Serializable]
    public class MainConfig : ConfigDat
    {

        /// <summary>
        /// 标题栏
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public double ScreenWidth { get; set; } = 1500;

        /// <summary>
        /// 窗口高度
        /// </summary>
        public double ScreenHeight { get; set; } = 900;
    }
}
