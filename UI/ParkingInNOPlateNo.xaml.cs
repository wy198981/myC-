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
using ParkingCommunication;
using ParkingInterface;

namespace UI
{
    /// <summary>
    /// ParkingInNOPlateNo.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingInNOPlateNo : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();

        //主板开闸
        VoiceSend cmd = new VoiceSend(1007, 1005);
        ParkingMonitoring.InNoCPHHandler InNoCPHHandler;
        string strPic = "";
        int imodulus = 0;
        int m_hLPRClient = 0;
        int m_nSerialHandle = 0;

        public ParkingInNOPlateNo()
        {
            InitializeComponent();
        }

        public ParkingInNOPlateNo(string _strPic, int _imodulus, ParkingMonitoring.InNoCPHHandler _InNoCPHHandler)
        {
            InitializeComponent();
            strPic = _strPic;
            imodulus = _imodulus;
            InNoCPHHandler = _InNoCPHHandler;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                picInPic.Image = Properties.Resources.Car2;
                m_hLPRClient = ParkingMonitoring.m_hLPRClient[imodulus];
                m_nSerialHandle = ParkingMonitoring.m_nSerialHandle[imodulus];
                cmbGateName.Items.Add(Model.Channels[imodulus].sInOutName);
                cmbGateName.Text = Model.Channels[imodulus].sInOutName;
                cmbCPH.Text = Model.LocalProvince;

                if (System.IO.File.Exists(strPic))
                {
                    picInPic.Image = System.Drawing.Image.FromFile(strPic);
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("无牌车入场" + ":ParkingInNOCPH_Load", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CPH = cmbCPH.Text + txtCPH.Text.Trim();
                if (txtCPH.Text == "")
                {
                    cmbCPH.Text = "";
                }

                if (CPH != "")
                {
                    if (!CR.CheckUpCPH(CPH))
                    {
                        MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + CPH + "】会引起车牌数据显示错误", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                JjcgetWriteStore(imodulus);

                //string strRetun = CR.SendVoice.SendOpen(axznykt_1, Model.PubVal.Channels[imodulus].iCtrlID, Model.PubVal.Channels[imodulus].sIP, 0x0C, 5, m_hLPRClient, Model.PubVal.Channels[imodulus].iXieYi);//开闸
                
                //开闸发语音
                cmd.SendOpen(imodulus);

                if (Model.bOut485)
                {
                    System.Threading.Thread.Sleep(50);
                }

                cmd.VoiceDisplay(VoiceType.Welcome, imodulus);

                //CR.SendVoice.VoiceLoad(axznykt_1, Model.PubVal.Channels[imodulus].iCtrlID, Model.PubVal.Channels[imodulus].sIP, "42", m_nSerialHandle, Model.PubVal.Channels[imodulus].iXieYi);

                InNoCPHHandler();
                this.Close();
            }
            catch (Exception ex)
            {
                gsd.AddLog("无牌车入场" + ":btnADD_Click", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 写入场记录
        /// </summary>
        private void JjcgetWriteStore(int modulus)
        {
            try
            {
                CarIn model = new CarIn();
                model.CardNO = CR.GetAutoCPHCardNO(Model.Channels[modulus].iCtrlID);
                model.CPH = cmbCPH.Text + txtCPH.Text;
                model.CardType = "TmpA";
                model.InTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                model.OutTime = DateTime.Now;
                model.InGateName = cmbGateName.Text;
                model.InOperator = Model.sUserName;
                model.InOperatorCard = Model.sUserCard;
                model.OutOperatorCard = "";
                model.OutOperator = "";
                model.SFJE = 0;
                //model.SFTime = DateTime.Now;
                //model.OvertimeSFTime = DateTime.Now;
                model.InPic = strPic;
                //model.InOut = Model.Channels[modulus].iInOut;
                model.BigSmall = Model.Channels[modulus].iBigSmall;
                model.InUser = cmbColor.Text;
                model.OutUser = cmbColor.Text;
                model.SFGate = cmbCarType.Text;
                model.CarparkNO = Model.iParkingNo;
                model.StationID = Model.stationID;

                if (strPic != "")
                {
                    model.InPic = gsd.UpLoadPic(strPic);
                }

                model.SFOperatorCard = "无牌车";
                gsd.AddAdmission(model,20);
            }
            catch (Exception ex)
            {
                gsd.AddLog("无牌车入场" + ":JjcgetWriteStore", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void txtCPH_TextChanged(object sender, TextChangedEventArgs e)
        {
            int pos = 0;
            pos = txtCPH.SelectionStart;
            txtCPH.Text = txtCPH.Text.ToUpper();
            txtCPH.Select(pos, 0);
        }

        private void txtCPH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.I || e.Key == Key.O)
            {
                e.Handled = true;
            }
        }
    }
}
