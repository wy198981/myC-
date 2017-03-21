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
    /// ParkingTempCPH.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingTempCPH : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        List<string> frmCPHList = new List<string>();
        private ParkingMonitoring.UpdateCPHDataHandler CPHDataHandler;
        private System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        int m_hLPRClient = 0;
        int m_nSerialHandle = 0;
        int modulus = 0;
        ParkingCommunication.VoiceSend voicesend = new ParkingCommunication.VoiceSend(1007, 1005);

        private string sTitle = "";   //2016-11-17

        public ParkingTempCPH()
        {
            InitializeComponent();
        }

        public ParkingTempCPH(List<string> _frmCPHList, ParkingMonitoring.UpdateCPHDataHandler _CPHDataHandler, string _sTitle = "")
        {
            InitializeComponent();
            sTitle = _sTitle;
            frmCPHList = _frmCPHList;
            CPHDataHandler = _CPHDataHandler;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                if (sTitle != "")
                {
                    this.Title = sTitle;
                }

                modulus = Convert.ToInt32(frmCPHList[0]);
                m_hLPRClient = ParkingMonitoring.m_hLPRClient[modulus];
                m_nSerialHandle = ParkingMonitoring.m_nSerialHandle[modulus];


                cboHeader0.Text = Model.LocalProvince;
                cboHeader1.Text = Model.LocalProvince;
                if (frmCPHList.Count > 4)
                {
                    if (frmCPHList[2].Length == 7)
                    {
                        cboHeader0.Text = frmCPHList[2].Substring(0, 1);
                        txtCPH0.Text = frmCPHList[2].Substring(1, 6);
                    }
                    if (frmCPHList[3].Length == 7)
                    {
                        cboHeader1.Text = frmCPHList[3].Substring(0, 1);
                        txtCPH1.Text = frmCPHList[3].Substring(1, 6);
                    }
                    cboInName.SelectedValue = Model.Channels[Convert.ToInt32(frmCPHList[0])].sInOutName;
                    cboInName.Text = Model.Channels[Convert.ToInt32(frmCPHList[0])].sInOutName;
                }
                if (Model.iAutoMinutes == 1)
                {
                    dTimer.Tick += new EventHandler(dTimer_Tick);
                    dTimer.Interval = new TimeSpan(0, 0, Model.iAutoSetMinutes);
                    dTimer.Start();

                    //timer1.Interval = Model.iAutoSetMinutes * 60000;
                    //timer1.Enabled = true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nParKingTempCPH_Load");
            }
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            dTimer.Stop();
            this.Close();
        }

        string sInputCPH = "", tmpCardType = "", tmpCardNO = "";
        DateTime dtStop;
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (optCPH0.IsChecked == true)
            {
                sInputCPH = cboHeader0.Text + txtCPH0.Text;
            }
            else
            {
                sInputCPH = cboHeader1.Text + txtCPH1.Text;
            }
            if (sInputCPH.Length > 6)
            {
                if (sInputCPH.Length == 7 || (sInputCPH.Substring(0, 2) == "WJ" && sInputCPH.Length == 8))
                {
                    if (Model.Channels[Convert.ToInt32(frmCPHList[0])].iOpenType == 7)
                    {
                        List<CardIssue> lstCI = gsd.SelectFaXing(sInputCPH);
                        if (lstCI.Count > 0)
                        {
                            dtStop = lstCI[0].CarValidEndDate;

                            tmpCardType = lstCI[0].CarCardType;

                            tmpCardNO = lstCI[0].CardNO;
                        }
                    }
                }
                if (sInputCPH.Length < 7)
                {
                    sInputCPH = "";
                }


                CarIn ci = new CarIn();
                ci.CardNO = tmpCardNO == "" ? frmCPHList[1] : tmpCardNO;
                ci.CPH = sInputCPH;
                ci.CardType = tmpCardType;
                ci.InTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                ci.OutTime = DateTime.Now;
                ci.InGateName = cboInName.Text;
                ci.InOperator = Model.sUserName;
                ci.InOperatorCard = Model.sUserCard;
                ci.OutOperatorCard = "";
                ci.OutOperator = "";
                ci.SFJE = 0;
                //ci.SFTime = DateTime.Now;
                //ci.OvertimeSFTime = DateTime.Now;
                ci.InPic = frmCPHList[5];
                ci.BigSmall = Model.Channels[modulus].iBigSmall;
                ci.InUser = "";
                ci.SFOperatorCard = "";
                ci.StationID = Model.stationID;
                ci.CarparkNO = Model.iParkingNo;
                ci.CardType = frmCPHList[6];
                gsd.AddAdmission(ci, 20);


                //int Count = gsd.UpdateComerecord(tmpCardNO, tmpCardType, sInputCPH, frmCPHList[5], frmCPHList[1], Convert.ToDateTime(frmCPHList[4]));

                CPHDataHandler(sInputCPH, Convert.ToInt32(frmCPHList[0]), ci.InTime);

                voicesend.SendOpen(modulus);

                //CR.SendVoice.SendOpen(axznykt_1, Model.PubVal.Channels[modulus].iCtrlID, Model.PubVal.Channels[modulus].sIP, 0x0C, 5, m_hLPRClient, Model.PubVal.Channels[modulus].iXieYi);//开闸

                

                if (frmCPHList[6].Length > 3 && frmCPHList[6].Substring(0, 3) == "Mth")
                {
                    if (Model.bOut485)
                    {
                        System.Threading.Thread.Sleep(50);
                    }

                    //voicesend.VoiceDisplay(ParkingCommunication.VoiceType.InGateVoice, modulus, frmCPHList[6], sInputCPH, Convert.ToInt32(frmCPHList[8]), frmCPHList[9], Convert.ToInt32(frmCPHList[7]));
                    //string sLoad = CR.CR.LedSound_New(Model.PubVal.byteLSXY[Model.PubVal.iLSIndex, 2], "FFFF", "FFFF", "FFFF", sInputCPH);
                    //CR.SendVoice.LoadLsNoX2010znykt(axznykt_1, Model.PubVal.Channels[modulus].iCtrlID, Model.PubVal.Channels[modulus].sIP, 0x3D, Model.PubVal.byteLSXY[Model.PubVal.iLSIndex, 2], sLoad, m_nSerialHandle, Model.PubVal.Channels[modulus].iXieYi);
                }
                else
                {
                    if (Model.bOut485)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                    //string sLoad = CR.CR.LedSound_New(Model.PubVal.byteLSXY[Model.PubVal.iLSIndex, 0], "FFFF", "FFFF", "FFFF", sInputCPH);
                    //CR.SendVoice.LoadLsNoX2010znykt(axznykt_1, Model.PubVal.Channels[modulus].iCtrlID, Model.PubVal.Channels[modulus].sIP, 0x3D, Model.PubVal.byteLSXY[Model.PubVal.iLSIndex, 0], sLoad, m_nSerialHandle, Model.PubVal.Channels[modulus].iXieYi);
                }

                voicesend.VoiceDisplay(ParkingCommunication.VoiceType.InGateVoice, modulus, frmCPHList[6], sInputCPH, Convert.ToInt32(frmCPHList[8]), frmCPHList[9], Convert.ToInt32(frmCPHList[7]));

                if (frmCPHList[5] != "")
                {
                    string path = gsd.UpLoadPic(frmCPHList[5]);
                    gsd.UpdateCarIn(frmCPHList[1], path);
                }
                    

                this.Close();
            }
            else
            {
                MessageBox.Show("输入的车牌号不对，请校验！", "提示");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            string strsLoad = "D2";
            voicesend.LoadLsNoX2010znykt(modulus, strsLoad);
            //int ret = gsd.DeleteComerecord(frmCPHList[1], Convert.ToDateTime(frmCPHList[4]));
            this.Close();
        }
    }
}
