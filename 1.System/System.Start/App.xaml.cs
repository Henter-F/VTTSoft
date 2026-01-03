using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ICore.Control;
using System.ICore.Interface;
using System.ICore.Log;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UIShell.OSGi;

namespace System.Start
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private object _bundleRuntime;
        private WindowCircularProgressBar progressBar = null;

        public App()
        {
            StartBundleRuntime();
        }


        private void StartBundleRuntime() // Start OSGi Core.
        {
            var strname = Process.GetCurrentProcess().ProcessName;
            Process[] processcollection = Process.GetProcessesByName(strname);
            // 如果该程序进程数量大于，则说明该程序已经运行，则弹出提示信息并提出本次操作，否则就创建该程序
            if (processcollection.Length > 1)
            {
                MessageBox.Show("程序已经运行中，无法重复打开相同两个程序!");
                Environment.Exit(0);
            }
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;


            if (progressBar == null)
            {
                progressBar = new WindowCircularProgressBar();
                progressBar.Show();
            }

            // 创建BundleRuntime
            var bundleRuntime = new BundleRuntime(new string[] { "System", "BasicTool", "StationTool", "VisionTool", "MotionTool" });
            // 不启动多版本支持
            bundleRuntime.EnableAssemblyMultipleVersions = false;
            // 监听插件状态变化，更新进度条
            bundleRuntime.Framework.EventManager.AddBundleEventListener(BundleStateChangedHandler, true);
            // 监听框架状态变化
            bundleRuntime.Framework.EventManager.AddFrameworkEventListener(FrameworkStateChangedHandler);
            // 将Application实例添加到全局服务，与插件进行共享
            bundleRuntime.AddService<Application>(this);

            // 启动插件框架
            bundleRuntime.Start();

            // 移除事件监听
            bundleRuntime.Framework.EventManager.RemoveBundleEventListener(BundleStateChangedHandler, true);
            bundleRuntime.Framework.EventManager.RemoveFrameworkEventListener(FrameworkStateChangedHandler);

            //Startup += App_Startup;
            Exit += App_Exit;
            _bundleRuntime = bundleRuntime;
            App_Startup(null, null);
        }

        private void FrameworkStateChangedHandler(object sender, FrameworkEventArgs e)
        {

        }

        private void BundleStateChangedHandler(object sender, BundleStateChangedEventArgs e)
        {
            if (progressBar != null && e.CurrentState == BundleState.Starting)
            {
                progressBar.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    string Meassage = string.Format("加载{0}模块中...", e.Bundle.Name);
                    progressBar.UpdataShowMeassgeProgress(Meassage, 0);
                }));
            }
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            Application app = Application.Current;
            var bundleRuntime = _bundleRuntime as BundleRuntime;
            app.ShutdownMode = ShutdownMode.OnLastWindowClose;
            var ViewModel = bundleRuntime.GetFirstOrDefaultService<IMainViewModel>();
            if (ViewModel != null)
            {
                app.MainWindow = ViewModel.MainView as Window;
                app.MainWindow.Show();
            }
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException1;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException1;
            AppDomain.CurrentDomain.FirstChanceException -= CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            if (progressBar != null)
            {
                progressBar.Close();
                progressBar = null;
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            if (_bundleRuntime != null)
            {
                var bundleRuntime = _bundleRuntime as BundleRuntime;
                bundleRuntime.Stop();
                _bundleRuntime = null;
            }
        }


        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            //LogUitl.LogError(e.Exception);
        }

        private static void CurrentDomain_UnhandledException1(object sender, UnhandledExceptionEventArgs e)
        {
            LogUitl.LogError(e.ExceptionObject.ToString());
        }

    }
}
