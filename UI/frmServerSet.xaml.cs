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
using ParkingInterface;
using System.Configuration;
using System.IO;
using ParkingModel;

namespace UI
{
    /// <summary>
    /// frmServerSet.xaml 的交互逻辑
    /// </summary>
    public partial class frmServerSet : SFMControls.WindowBase
    {
        Action<List<Operators>,List<StationSet>> updConfig;
        List<Operators> lstOptr;
        List<StationSet> lstStation;
        Request req;

        public frmServerSet(Action<List<Operators>, List<StationSet>> _updConfig)
        {
            InitializeComponent();
            updConfig = _updConfig;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush berriesBrush = new ImageBrush();
            berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Login.jpg"), UriKind.Absolute));

            this.Background = berriesBrush;

            txtIP.Text = Model.serverIP;
            txtPort.Text = Model.serverPort;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtIP.Text == "" || txtPort.Text == "")
            {
                MessageBox.Show("IP或端口不能为空");
                return;
            }

            if (!CR.IsIP(txtIP.Text.Trim()) || !CR.IsNumberic(txtPort.Text.Trim()))
            {
                MessageBox.Show("服务IP或端口格式不正确");
            }
            else
            {
                //if (Model.serverPort == txtPort.Text.Trim() && Model.serverIP == txtIP.Text.Trim())
                //{
                //    MessageBox.Show("参数相同，不能重复保存");
                //    return;
                //}

                string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "ParkingInterface.dll");
                if (!File.Exists(path))
                {
                    MessageBox.Show("配置文件丢失，请联系管理员");
                    return;
                }


                Model.serverIP = txtIP.Text;
                Model.serverPort = txtPort.Text;
                req = new Request();

                bool LoadDataSucceed = false;

                Task t = new Task(() =>
                    {
                        lstOptr = req.GetData<List<ParkingModel.Operators>>("GetOperatorsWithoutLogin");
                        lstStation = req.GetData<List<ParkingModel.StationSet>>("GetStationSetWithoutLogin", null, null, "StationId");
                        LoadDataSucceed = true;
                    });
                t.Start();
                Task.WaitAny(new Task[] { t }, 3000);


                if (!LoadDataSucceed)
                {
                    MessageBox.Show("服务连接失败，请重新设置或者检查服务是否正常启动!", "提示",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }


                Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["ServiceIP"] = txtIP.Text.Trim();
                dic["ServicePort"] = txtPort.Text.Trim();
                bool ret = ConfigFile.UpdateAppConfig(config, dic);
                if (ret)
                {
                    if (updConfig != null)
                    {
                        updConfig(lstOptr, lstStation);
                    }
                    MessageBox.Show("服务连接成功", "提示");
                    this.Close();
                }
            }
        }

        private void txtIP_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.OemPeriod || e.Key == Key.Decimal))
            {
                e.Handled = true;
            }
        }

        private void txtPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }


    }
}
