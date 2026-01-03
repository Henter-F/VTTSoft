using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ICore.Enum;
using System.ICore.Interface;
using System.ICore.Log;
using System.ICore.MenuItemSet;
using System.ICore.MessageBox.Helper;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIShell.OSGi;

namespace System.MainView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        public DockingManager DockManagers { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DockManagers = DockManager;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               
                UpdataMenuItem();

                if (File.Exists("布局.xml"))
                {
                    XmlLayoutSerializer serializer = new XmlLayoutSerializer(DockManagers);
                    using (var stream = new StreamReader("布局.xml"))
                    {
                        serializer.Deserialize(stream);
                    }
                }
                var ViewModel = BundleRuntime.Instance?.GetFirstOrDefaultService<IMainViewModel>();
                ViewModel.Close += () =>
                {
                    try
                    {
                        MessageBoxResult result = MessageBoxHelper.ShowAsk("确定要退出程序吗?", "退出程序");
                        if (result == MessageBoxResult.Yes)
                        {
                            Window[] childArray = Application.Current.Windows.Cast<Window>().ToArray();
                            for (int i = childArray.Length; i-- > 0;)//关闭所有子窗体
                            {
                                Window item = childArray[i];
                                if (item.Title != this.Title) item.Close();
                            }
                            var ctl = BundleRuntime.Instance?.GetFirstOrDefaultService<IProjectPanelViewModel>();
                            ctl?.ExecuteStopWorkCommand();
                            ctl?.CenterArea.StartDispose();
                            Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUitl.LogError(ex.ToString());
                    }
                };

                var ModelView = BundleRuntime.Instance?.GetFirstOrDefaultService<ILoginViewModel>();
                if (ModelView != null)
                {
                    ModelView.UserLevelChangeEnvent -= UserLevelChangeEnventCammend;
                    ModelView.UserLevelChangeEnvent += UserLevelChangeEnventCammend;
                    ModelView.LogIn("Operator", "123");
                }
                var Model = BundleRuntime.Instance?.GetFirstOrDefaultService<ILogViewModel>();
                Model?.UpdateLogView();
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        private void MainView_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                foreach (var document in DockManagers.Layout.Descendents().OfType<LayoutDocument>())
                {

                    if (document.Title.Trim().Contains("视图窗口"))
                    {
                        document.IsActive = true;

                    }
                }
                var ctl1 = BundleRuntime.Instance?.GetFirstOrDefaultService<IProjectPanelViewModel>();
                ctl1?.CenterArea.StartConstructor();

                Task.Run(() =>
                {
                    while (ctl1?.CenterArea.WFConstructor.Status == WorkFlowStatus.Active)
                    {
                        Thread.Sleep(5);
                    }
                    Thread.Sleep(200);
                    ClearMemory();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ctl1?.ExecuteStartWorkCommand();
                    });

                });
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal; //设置窗口还原
            }
            else
            {
                this.WindowState = WindowState.Maximized; //设置窗口最大化
            }
        }

        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //设置窗口最小化
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// 更新工具栏
        /// </summary>
        public void UpdataMenuItem()
        {
            try
            {
                foreach (var item in MenuItemSeting.MenuItemUI)
                {
                    if (item.Key == MenuStyle.系统)
                    {
                        foreach (var v in item.Value)
                        {
                            SystemMenuItem.Items.Add(v);
                        }
                    }
                    else if (item.Key == MenuStyle.设置)
                    {
                        foreach (var v in item.Value)
                        {
                            SetParaMenuItem.Items.Add(v);
                        }
                    }
                    else if (item.Key == MenuStyle.设备)
                    {
                        foreach (var v in item.Value)
                        {
                            if (v.Tag == null)
                            {
                                DeviceMenuItem.Items.Add(v);
                            }
                            else
                            {
                                foreach (var waq in DeviceMenuItem.Items)
                                {
                                    if (waq is MenuItem)
                                    {
                                        MenuItem menu = (MenuItem)waq;
                                        if (menu.Header.ToString() == v.Tag.ToString())
                                        {
                                            menu.Items.Add(v);
                                            menu.Visibility = Visibility.Visible;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (item.Key == MenuStyle.视图)
                    {
                        foreach (var v in item.Value)
                        {
                            VisionMenuItem.Items.Add(v);
                        }
                    }
                    else if (item.Key == MenuStyle.工具)
                    {
                        foreach (var v in item.Value)
                        {
                            ToolMenuItem.Items.Add(v);
                        }
                    }
                    else if (item.Key == MenuStyle.调试)
                    {
                        foreach (var v in item.Value)
                        {
                            DebugMenuItem.Items.Add(v);
                        }
                    }
                    else if (item.Key == MenuStyle.帮助)
                    {
                        foreach (var v in item.Value)
                        {
                            HelpMenuItem.Items.Add(v);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }
        public void UserLevelChangeEnventCammend(string UserName, PrivilegeLevelType LevelType)
        {
            try
            {
                var ViewModel = BundleRuntime.Instance?.GetFirstOrDefaultService<IMainViewModel>();
                if (ViewModel != null)
                {
                    ViewModel.CurUserName = UserName;
                    ViewModel.CurLevelType = LevelType;
                    ViewModel.CurUserNameAndLevel = string.Format("{0}({1})", UserName, LevelType.ToString());
                }
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }

        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
    }
}
