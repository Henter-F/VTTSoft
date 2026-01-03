using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace System.ICore.MessageBox.Views
{
    /// <summary>
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox : Window
    {
        private Timer MyTimer { get; set; }
        private int ShowTime = 0;
        public MessageBox(int showingTime = 0)
        {
            ShowTime = showingTime;
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            // 自动关闭
            if (ShowTime > 0)
            {
                MyTimer = new Timer(ShowTime);
                MyTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                MyTimer.Start();
            }
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MyTimer.Stop();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                Close();
            }));
        }
    }
}
