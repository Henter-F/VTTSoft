using System;
using System.Collections.Generic;
using System.ICore.Log;
using System.Linq;
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

namespace System.ICore.Control
{
    /// <summary>
    /// WindowCircularProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class WindowCircularProgressBar : Window
    {
        public WindowCircularProgressBar()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _loading.Visibility = Visibility.Visible;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _loading.Visibility = Visibility.Collapsed;
        }
        public void UpdataShowMeassgeProgress(string Meassage, double ProgressValue)
        {
            try
            {
                Tb_ShowMeassge.Text = Meassage;
                Thread.Sleep(150);
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }
    }
}
