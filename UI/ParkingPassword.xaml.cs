using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ParkingModel;
using ParkingInterface;

namespace UI
{
    /// <summary>
    /// ParkingPassword.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingPassword : SFMControls.WindowBase
    {
        Request req = new Request();
        bool isClose = false;
        public ParkingPassword()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string UserNo = Model.sUserCard;
                string data = req.GetToken(UserNo, CR.UserMd5(txtPwd.Password.Trim()));
                if (null != data && data.Trim().Length > 0)
                {
                    isClose = true;
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("输入密码错误！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入密码错误！", "提示");
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush berriesBrush = new ImageBrush();
            berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Login.jpg"), UriKind.Absolute));

            this.Background = berriesBrush;
        } 
    }
}
