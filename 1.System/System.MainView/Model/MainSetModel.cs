using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ICore.ConfigSave;
using System.ICore.Log;
using System.Linq;
using System.MainView.Config;
using System.Text;
using System.Threading.Tasks;

namespace System.MainView.Model
{
    public class MainSetModel : ObservableObject
    {

        /// <summary>
        ///  参数配置
        /// </summary>
        public ConfigDat Config { get { return config; } set { if (value is MainConfig config1) config = config1; } }
        private MainConfig config = new MainConfig();


        public RelayCommand SaveConfigCommand { get; set; }


        public MainSetModel(MainConfig _config)
        {
            Config = _config;
            SaveConfigCommand = new RelayCommand(SaveMainConfigCommand);
        }


        /// <summary>
        /// 保存参数
        /// </summary>
        public void SaveMainConfigCommand()
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
