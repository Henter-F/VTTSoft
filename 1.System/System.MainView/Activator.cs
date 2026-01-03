using System;
using System.Collections.Generic;
using System.ICore.Interface;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIShell.OSGi;
using UIShell.OSGi.Core.Service;

namespace System.MainView
{
    public class Activator : IBundleActivator
    {
        public void Start(IBundleContext context)
        {
            //实例化、注册、显示主界面
            context.AddService<IMainViewModel>(new MainViewModel());
           // IBundleManager bundleManager = BundleRuntime.Instance?.GetFirstOrDefaultService<IBundleManager>();
        }

        public void Stop(IBundleContext context)
        {
           
        }
    }
}
