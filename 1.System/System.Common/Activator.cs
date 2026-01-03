using System;
using System.Collections.Generic;
using System.Common.Login.Model;
using System.ICore.Enum;
using System.ICore.Interface;
using System.ICore.MenuItemSet;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using UIShell.OSGi;
using Sunny.UI;
using System.ICore.Log;

namespace System.Common
{
    public class Activator : IBundleActivator
    {
       // private StationDebugCtl View = null;
        public void Start(IBundleContext context)
        {
            context.AddService<ILoginViewModel>(new LoginViewModel());
        }

        public void Stop(IBundleContext context)
        {
           
        }

        /// <summary>
        /// 主界面设置
        /// </summary>
        public void UpdataMenuItem()
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "工位调试";
            menuItem.Height = 30;
            Image image1 = new Image();
            image1.Source = new BitmapImage(new Uri("pack://application:,,,/System.ICore;Component/Image/调试.png"));
            menuItem.VerticalContentAlignment = VerticalAlignment.Center;
            menuItem.HorizontalContentAlignment = HorizontalAlignment.Center;
            menuItem.Icon = image1;
            menuItem.Click += MenuItem1_Click;
            MenuItemSeting.AddMenuItem(MenuStyle.调试, menuItem);


            MenuItem menuItem1 = new MenuItem();
            menuItem1.Header = "全局变量";
            menuItem1.Height = 30;
            Image image2 = new Image();
            image2.Source = new BitmapImage(new Uri("pack://application:,,,/System.ICore;Component/Image/全局变量.png"));
            menuItem1.VerticalContentAlignment = VerticalAlignment.Center;
            menuItem1.HorizontalContentAlignment = HorizontalAlignment.Center;
            menuItem1.Icon = image2;
            menuItem1.Click += MenuItem2_Click;
            MenuItemSeting.AddMenuItem(MenuStyle.工具, menuItem1);





        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (View == null || View.IsLoaded == false)
            //    {
            //        var ModelView = BundleRuntime.Instance?.GetFirstOrDefaultService<ILoginViewModel>();
            //        if (ModelView.CurUserLevel > PrivilegeLevelType.操作员)
            //        {
            //            View = new StationDebugCtl();
            //            View.Show();
            //        }
            //        else
            //        {

            //            UIMessageTip.ShowWarning("当前用户权限等级不够,无法打开工位调试界面!!!");
            //        }
            //    }
            //    else
            //    {
            //        View.Activate();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogUitl.LogError(ex.ToString());
            //}
        }


        public void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (VariableView == null || VariableView.IsClosed)
                //{
                //    VariableView = new GlobalVariableView();
                //    VariableView.Show();
                //}
                //else
                //{
                //    VariableView.Activate();
                //}
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }
    }
}
