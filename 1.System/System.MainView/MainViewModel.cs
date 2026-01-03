using AvalonDock.Layout.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ICore.ConfigSave;
using System.ICore.Enum;
using System.ICore.Interface;
using System.ICore.Log;
using System.IO;
using System.Linq;
using System.MainView.Config;
using System.MainView.Model;
using System.MainView.View;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UIShell.OSGi;

namespace System.MainView
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        private MainSetView View = null;

        private Point mousePosition;

        public int tickcount = 0;

        private ILoginViewModel viewModel = null;

        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        // 获取内存使用率
        ManagementClass memoryClass = new ManagementClass("Win32_OperatingSystem");




        /// <summary>
        ///  参数配置
        /// </summary>
        public ConfigDat Config { get { return config; } set { if (value is MainConfig config1) config = config1; } }
        private MainConfig config = new MainConfig();



        /// <summary>
        /// 当前配方名称
        /// </summary>
        public string CurUseFormulaName
        {
            get { return _CurUseFormulaName; }
            set
            {
                SetProperty(ref _CurUseFormulaName, value);
            }
        }
        private string _CurUseFormulaName = string.Empty;



        /// <summary>
        /// 当前用户
        /// </summary>
        public string CurUserName
        {
            get { return _CurUserName; }
            set
            {
                SetProperty(ref _CurUserName, value);
            }
        }
        private string _CurUserName = string.Empty;




        /// <summary>
        /// 当前用户权限等级
        /// </summary>
        public PrivilegeLevelType CurLevelType
        {
            get { return _CurLevelType; }
            set
            {
                SetProperty(ref _CurLevelType, value);
            }
        }
        private PrivilegeLevelType _CurLevelType = PrivilegeLevelType.操作员;

        /// <summary>
        /// 当前用户名和等级
        /// </summary>
        public string CurUserNameAndLevel
        {
            get { return _CurUserNameAndLevel; }
            set
            {
                SetProperty(ref _CurUserNameAndLevel, value);
            }
        }
        private string _CurUserNameAndLevel = string.Empty;

        /// <summary>
        /// 当前时间
        /// </summary>
        public string CurTime
        {
            get { return _CurTime; }
            set
            {
                SetProperty(ref _CurTime, value);
            }
        }
        private string _CurTime = string.Empty;

        /// <summary>
        /// cpu使用率
        /// </summary>
        public string CPU
        {
            get { return _CPU; }
            set
            {
                SetProperty(ref _CPU, value);
            }
        }
        private string _CPU = string.Empty;

        /// <summary>
        /// 内存使用率
        /// </summary>
        public string Memory
        {
            get { return _Memory; }
            set
            {
                SetProperty(ref _Memory, value);
            }
        }
        private string _Memory = string.Empty;



        /// <summary>
        /// 主界面显示
        /// </summary>
        public IMainWindow MainView
        {
            get
            {
                if (CurMainView == null || !CurMainView.IsInitialized)
                {
                    try
                    {
                        CurMainView = new MainWindow();
                        CurMainView.DataContext = this;

                    }
                    catch (Exception ex)
                    {
                        LogUitl.LogError(ex.ToString());
                    }
                }
                return CurMainView;
            }
        }
        private MainWindow CurMainView = null;


        /// <summary>
        /// 关闭窗口
        /// </summary>
        public RelayCommand MainClose { get; set; }

        /// <summary>
        /// 打开配方设置界面
        /// </summary>
        public RelayCommand OpenFormulaView { get; set; }

        /// <summary>
        /// 保存界面布局
        /// </summary>
        public RelayCommand SaveDockManagerXml { get; set; }

        /// <summary>
        /// 打开登录界面
        /// </summary>
        public RelayCommand OpenLoginView { get; set; }

        public RelayCommand OpenMainSetView { get; set; }

        /// <summary>
        /// 退出事件
        /// </summary>
        public event Action Close;


        public MainSetModel Model { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public MainViewModel()
        {
            try
            {
                if (!Init()) { LogUitl.LogError("初始化主界面参数失败!!!"); }
                MainClose = new RelayCommand(ExecuteMainClose);
                OpenFormulaView = new RelayCommand(ExecuteOpenFormulaView);
                SaveDockManagerXml = new RelayCommand(ExecuteSaveDockManagerXml);
                OpenLoginView = new RelayCommand(ExecuteOpenLoginView);
                OpenMainSetView = new RelayCommand(ExecuteOpenMainSetView);
                mousePosition = GetMousePoint();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(500);
                timer.Tick += new EventHandler(Fixedtimer_Tick);
                timer.IsEnabled = true;
                UIMessageTip.WarningStyle.TextFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                UIMessageTip.OkStyle.TextFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                UIMessageTip.ErrorStyle.TextFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                UIMessageTip.DefaultStyle.TextFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                Model = new MainSetModel(config);

            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        public void ExecuteMainClose()
        {
            Close?.Invoke();
        }

        public bool Init()
        {
            try
            {
                config = new MainConfig
                {
                    Folder = "System",
                    Name = "主界面配置",
                };
                config = config.Read<MainConfig>();
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
                return false;
            }
            return true;
        }

        //判断鼠标是否移动
        private bool HaveUsedTo()
        {
            Point point = GetMousePoint();
            if (point == mousePosition)
            {
                return false;
            }
            mousePosition = point;
            return true;
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct MPoint
        {
            public int X;
            public int Y;

            public MPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetCursorPos(out MPoint mpt);
        /// 获取当前屏幕鼠标位置   
        public Point GetMousePoint()
        {
            MPoint mpt = new MPoint();
            GetCursorPos(out mpt);
            Point p = new Point(mpt.X, mpt.Y);
            return p;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fixedtimer_Tick(object sender, EventArgs e)
        {
            try
            {
                CurTime = DateTime.Now.ToString();
                // 获取CPU使用率
                CPU = Math.Round(cpuCounter.NextValue(), 2) + "%";
                ManagementObjectCollection memoryCollection = memoryClass.GetInstances();
                foreach (ManagementObject obj in memoryCollection)
                {
                    ulong totalVirtualMemory = (ulong)obj["TotalVisibleMemorySize"];
                    ulong freeVirtualMemory = (ulong)obj["FreePhysicalMemory"];
                    ulong usedVirtualMemory = totalVirtualMemory - freeVirtualMemory;
                    double memoryUsage = (double)usedVirtualMemory / (double)totalVirtualMemory * 100;
                    Memory = Math.Round(memoryUsage, 2) + "%";
                }

                if (viewModel == null) viewModel = BundleRuntime.Instance?.GetFirstOrDefaultService<ILoginViewModel>();

                if (!HaveUsedTo() && viewModel != null)
                {
                    tickcount++;   //检测到鼠标没移动，checkCount + 1  
                    if (tickcount == viewModel.FreeMinute * 120)
                    {
                        tickcount = 0;
                        viewModel.LogIn("Operator", "123");
                    }
                }
                else
                {
                    tickcount = 0;    //检测到鼠标移动，重新计数
                }


            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }




        /// <summary>
        /// 打开配方设置界面
        /// </summary>
        public void ExecuteOpenFormulaView()
        {
            try
            {
                if (CurLevelType > PrivilegeLevelType.操作员)
                {
                    var ModelView = BundleRuntime.Instance?.GetFirstOrDefaultService<IFormula>();
                    if (ModelView != null) ModelView.View.Show();
                }
                else
                {
                    UIMessageTip.ShowWarning("当前用户权限等级不够,无法打开配方界面!!!", 2000);
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }


        public void ExecuteOpenLoginView()
        {
            try
            {
                var ModelView = BundleRuntime.Instance?.GetFirstOrDefaultService<ILoginViewModel>();
                if (ModelView != null) ModelView.View.Show();
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }


        public void ExecuteOpenMainSetView()
        {
            try
            {
                if (View == null || View.IsClosed)
                {
                    View = new MainSetView();
                    View.DataContext = Model;
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
        public void ExecuteSaveDockManagerXml()
        {
            try
            {
                var ModelView = BundleRuntime.Instance?.GetFirstOrDefaultService<ILoginViewModel>();
                if (ModelView.CurUserLevel < PrivilegeLevelType.管理员) { UIMessageTip.ShowWarning("当前用户权限等级不够,不允许保存布局"); return; }
                XmlLayoutSerializer serializer = new XmlLayoutSerializer(MainView.DockManagers);
                using (var stream = new StreamWriter("布局.xml"))
                {
                    serializer.Serialize(stream);
                }
                UIMessageTip.ShowOk("保存布局成功!!!");
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }
    }
}
