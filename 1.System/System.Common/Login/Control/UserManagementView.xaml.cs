using System;
using System.Collections.Generic;
using System.ICore.Enum;
using System.ICore.Log;
using System.Linq;
using System.Text;
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

namespace System.Common.Login.Control
{
    /// <summary>
    /// UserManagementView.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagementView : Window
    {
        public bool IsClosed { get; set; } = false;
        public UserManagementView()
        {
            InitializeComponent();

            cmb_LevelStyle.ItemsSource = Enum.GetNames(typeof(PrivilegeLevelType));
            pwd.TextDecorations = new TextDecorationCollection(new TextDecoration[] {
                new TextDecoration() {
                     Location= TextDecorationLocation.Strikethrough,
                      Pen= new Pen(Brushes.Black, 10f) {
                          DashCap =  PenLineCap.Round,
                           StartLineCap= PenLineCap.Round,
                            EndLineCap= PenLineCap.Round,
                             DashStyle= new DashStyle(new double[] {0.0,1.2 }, 0.6f)
                      }
                }

            });
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

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsClosed = true;
                this.Close();
            }
            catch (Exception ex)
            {
                LogUitl.LogError(ex.ToString());
            }
        }
    }
}
