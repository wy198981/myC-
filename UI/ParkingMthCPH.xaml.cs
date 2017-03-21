using ParkingInterface;
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

namespace UI
{
    /// <summary>
    /// ParkingMthCPH.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingMthCPH : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        List<string> frmCPHList = new List<string>();
        int m_hLPRClient = 0;
        int m_nSerialHandle = 0;
        int modulus = 0;
        ParkingCommunication.VoiceSend voicesend = new ParkingCommunication.VoiceSend(1007, 1005);

        public ParkingMthCPH()
        {
            InitializeComponent();
        }

        public ParkingMthCPH(List<string> _listStr)
        {
            InitializeComponent();
            frmCPHList = _listStr;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                modulus = Convert.ToInt32(frmCPHList[0]);
                m_hLPRClient = ParkingMonitoring.m_hLPRClient[modulus];
                m_nSerialHandle = ParkingMonitoring.m_nSerialHandle[modulus];
                lblCPH.Content = frmCPHList[3];
                lblGateName.Content = ParkingModel.Model.Channels[Convert.ToInt32(frmCPHList[0])].sInOutName;
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "Window_Loaded", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                voicesend.SendOpen(modulus);
                if (ParkingModel.Model.bOut485)
                {
                    System.Threading.Thread.Sleep(50);
                }

                TimeSpan stime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) - Convert.ToDateTime(frmCPHList[4]);
                string strTime = stime.Days.ToString("X4") + stime.Hours.ToString("X2") + stime.Minutes.ToString("00");

                voicesend.VoiceDisplay(ParkingCommunication.VoiceType.OutGateVoice, modulus, frmCPHList[6], frmCPHList[3], Convert.ToInt32(frmCPHList[7]), "FFFF", Convert.ToInt32(frmCPHList[8]), 0, 0, strTime);
                if ("" != frmCPHList[5])
                {
                    string path = gsd.UpLoadPic(frmCPHList[5]);
                    gsd.UpdateCarOut(frmCPHList[1], path);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnOK_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnOK_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strsLoad = "D4";
                voicesend.LoadLsNoX2010znykt(modulus, strsLoad);
                long Count = gsd.RstInGateRetrography(new { ID = frmCPHList[frmCPHList.Count - 1] });
                this.Close();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnCancel_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnCancel_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
