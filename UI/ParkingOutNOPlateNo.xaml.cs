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
using System.Data;
using ParkingInterface;
using ParkingCommunication;
using System.Printing;

namespace UI
{
    /// <summary>
    /// ParkingOutNOPlateNo.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingOutNOPlateNo : SFMControls.WindowBase
    {
        VoiceSend vs = new VoiceSend(1007, 1005);

        GetServiceData gsd = new GetServiceData();
        string strPic = "";
        int imodulus = 0;

        int iCouponJH = 0;
        string sCouponAddr = "";
        string sCouponMode = "";
        decimal sCouponValue = 0;
        bool bDZ = false;
        bool bLoad = false;
        string inPic = "";
        bool bLoadMouse = false;
        int iPayType = 0;
        int m_hLPRClient = 0;
        int m_nSerialHandle = 0;

        ParkingMonitoring.InNoCPHHandler InNoCPHHandler;

        public ParkingOutNOPlateNo()
        {
            InitializeComponent();
        }

        public ParkingOutNOPlateNo(string _strPic, int _imodulus, ParkingMonitoring.InNoCPHHandler _InNoCPHHandler, string _inPic)
        {
            InitializeComponent();
            strPic = _strPic;
            imodulus = _imodulus;
            InNoCPHHandler = _InNoCPHHandler;
            inPic = _inPic;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                picCarIn.Image = Properties.Resources.Car2;
                picCarOut.Image = Properties.Resources.Car2;

                bLoad = false;

                cmbCarType.Items.Add("丰田");
                cmbCarType.Items.Add("广本");
                cmbCarType.Items.Add("奇瑞");
                cmbCarType.Items.Add("奔驰");
                cmbCarType.Items.Add("宝马");
                cmbCarType.Items.Add("奥迪");
                cmbCarType.Items.Add("长安");
                cmbCarType.Items.Add("富康");
                cmbCarType.Items.Add("尼桑");
                cmbCarType.Items.Add("红旗");
                cmbCarType.Items.Add("捷达");
                cmbCarType.Items.Add("卡迪拉客");
                cmbCarType.Items.Add("劳斯莱斯");
                cmbCarType.Items.Add("凌肯");
                cmbCarType.Items.Add("三菱");
                cmbCarType.Items.Add("蓝鸟");
                cmbCarType.Items.Add("富豪");
                cmbCarType.Items.Add("吉普");
                cmbCarType.Items.Add("桑塔纳");
                cmbCarType.Items.Add("其它");
                //cmbCardType.SelectedIndex = 0;
               
                cmbColor.Items.Add("银色");
                cmbColor.Items.Add("黑色");
                cmbColor.Items.Add("白色");
                cmbColor.Items.Add("蓝色");
                cmbColor.Items.Add("红色");
                cmbColor.Items.Add("棕色");
                //cmbColor.SelectedIndex = 0;

                List<CardTypeDef> lstCTD = gsd.GetTemp();
                cmbCardType.ItemsSource = lstCTD;
                cmbCardType.DisplayMemberPath = "CardType";
                cmbCardType.SelectedValuePath = "Identifying";
                if (lstCTD.Count > 0)
                    cmbCardType.SelectedIndex = 0;

                cmbJHDZ.ItemsSource = gsd.GetJiHaoDZ();
                cmbJHDZ.DisplayMemberPath = "Address";
                cmbJHDZ.SelectedValuePath = "Address";
                cmbJHDZ.Text = "";

                cmbFree.ItemsSource = gsd.GetFreeReasonDefine();
                cmbFree.DisplayMemberPath = "ItemDetail";
                cmbFree.SelectedValuePath = "ItemID";
                cmbFree.Text = "";


                if (Model.iTempFree == 1)
                {
                    btnFree.IsEnabled = true;
                    cmbFree.IsEnabled = true;
                }
                else
                {
                    btnFree.IsEnabled = false;
                    cmbFree.IsEnabled = false;
                }

                cmbGateName.Items.Add(Model.Channels[imodulus].sInOutName);
                cmbGateName.Text = Model.Channels[imodulus].sInOutName;

                if (System.IO.File.Exists(strPic))
                {
                    picCarOut.Image = System.Drawing.Image.FromFile(strPic);
                }

                bLoadMouse = false;

                //是否可更改卡类
                if (Model.iSetTempCardType == 0)
                {
                    cmbCardType.IsEnabled = false;
                }

                m_hLPRClient = ParkingMonitoring.m_hLPRClient[imodulus];
                m_nSerialHandle = ParkingMonitoring.m_nSerialHandle[imodulus];

                if (Model.iOnlinePayEnabled == 1)
                {
                    grpScan.IsEnabled = true;
                }
                else
                {
                    grpScan.IsEnabled = false;
                }

                bLoad = true;

                //ShowRights();
            }
            catch (System.Exception ex)
            {
                gsd.AddLog("无牌车出场" + ":ParkingOutNoCPH_Load", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "ParkingOutNoCPH_Load", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (cmbColor.Text != "")
                {
                    dic["InUser"] = cmbColor.Text.ToString();
                }
                if (cmbCarType.Text != "")
                {
                    dic["SFGate"] = cmbCarType.Text.ToString();
                }
                if (txtCPH.Text != "")
                {
                    dic["CPH"] = txtCPH.Text.ToString();
                }
                dgvCar.ItemsSource = gsd.GetInNoCPH(dic, Convert.ToDateTime(dtpStart.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(dtpStart.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59")));
                bLoadMouse = false;
                btnOpen.IsEnabled = false;
                btnFree.IsEnabled = false;
                btnPrint.IsEnabled = false;
            }
            catch (Exception ex)
            {
                gsd.AddLog("无牌车出场" + ":btnSelect_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnSelect_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0 && dgvCar.SelectedIndex > -1)
                {
                    var selectItem = dgvCar.SelectedItem as CarIn;
                    //object obj = selectItem["CPH"];
                    string inPic = selectItem.InPic;
                    string srtInPic = inPic;
                    DateTime dtStartHF = selectItem.InTime;
                    string strGateInName = selectItem.InGateName;
                    string strCardType = ParkingInterface.CR.GetCardType(cmbCardType.Text, 0);
                    string strCardNO = selectItem.CardNO;

                    if (Model.iCentralCharge == 1)
                    {
                        List<CarIn> dt = gsd.GetNoCentralCharge(strCardNO);
                        if (dt.Count > 0)
                        {
                            string strSFGate = dt[0].SFGate;
                            if (strSFGate == "中央收费")
                            {
                                strCardType = "TmpJ";
                                cmbCarType.IsEnabled = false;

                            }
                        }
                        else
                        {
                            cmbCarType.IsEnabled = true;
                        }
                    }
                    else
                    {
                        cmbCarType.IsEnabled = true;
                    }

                    if (strCardType.Substring(0, 3) == "Tmp")
                    {
                        CaclMoneyResult d1 = gsd.GetMONEY(strCardType, dtStartHF, DateTime.Now, selectItem.CPH);
                        lblMoney.Content = d1.SFJE;

                        bLoadMouse = true;
                        btnOpen.IsEnabled = true;
                        btnPrint.IsEnabled = true;

                        //是否允许免费
                        if (Model.iTempFree == 1)
                        {
                            btnFree.IsEnabled = true;
                            cmbFree.IsEnabled = true;
                        }
                    }
                    else
                    {
                        lblMoney.Content = "0.00";
                    }
                    lblSFJE.Content = lblMoney.Content;


                    if (cmbJHDZ.Text.Trim() != "")
                    {
                        //int iJiHao = Convert.ToInt32(cmbJHDZ.Text);
                        //iCouponJH = iJiHao;

                        string addr = cmbJHDZ.SelectedValue.ToString();
                        List<ParkDiscountJHSet> jiHaoDT = gsd.GetJiHaoDZ(addr);
                        if (jiHaoDT.Count > 0)
                        {
                            sCouponAddr = jiHaoDT[0].Address;
                            sCouponMode = jiHaoDT[0].Manner;
                            sCouponValue = jiHaoDT[0].Favorable;
                        }
                        if (sCouponAddr != "")
                        {
                            if (sCouponMode == "小时")
                            {
                                if (Convert.ToInt32(sCouponValue) == 9999)
                                {
                                    lblMoney.Content = "0.0";
                                }
                                else
                                {
                                    if (CR.DateDiff(CR.DateInterval.Minute, Convert.ToDateTime(lblInDateTime.Content), DateTime.Now) <= sCouponValue * 60)
                                    {
                                        lblMoney.Content = "0.0";
                                    }
                                    else
                                    {
                                        DateTime dtInTimes = Convert.ToDateTime(lblInDateTime.Content).AddHours(Convert.ToInt32(sCouponValue));
                                        CaclMoneyResult dOut = gsd.GetMONEY(CR.GetCardType(cmbCardType.Text, 0), dtInTimes, DateTime.Now, txtCPH.Text ?? "");
                                        lblMoney.Content = Convert.ToDouble(dOut.SFJE).ToString("0.0");
                                    }
                                }
                            }
                            else if (sCouponMode == "元")
                            {
                                if (Convert.ToDecimal(lblSFJE.Content) < sCouponValue)
                                {
                                    //lblSFJE.Text = "0.0";
                                    lblMoney.Content = "0.0";
                                }
                                else
                                {
                                    //带小数点收费还未处理
                                    if (Model.iXsd == 1)
                                    {
                                        decimal dMoney = (Convert.ToDecimal(lblSFJE.Content) * 10 - sCouponValue) / 10;
                                        lblMoney.Content = dMoney.ToString();
                                        //lblMoney.Content = Convert.ToDouble(dMoney).ToString("0.0");
                                    }
                                    else
                                    {
                                        decimal dMoney = Convert.ToDecimal(lblSFJE.Content) - sCouponValue;
                                        //lblSFJE.Text = dMoney.ToString();
                                        lblMoney.Content = Convert.ToDouble(dMoney).ToString("0.0");
                                    }

                                }
                            }
                            bDZ = true;
                        }
                    }

                    //出场临时车播报收费语音
                    vs.VoiceDisplay(VoiceType.OutGateVoice, imodulus, "TmpA", "", 0, "", 0, 0, Convert.ToDecimal(lblMoney.Content.ToString()));

                    //string MyTempMoney = "";
                    //if (Model.iXsd == 0)
                    //{
                    //    if (Model.iChargeType == 3)
                    //    {
                    //        if (Model.iXsdNum == 1)
                    //        {
                    //            MyTempMoney = (Convert.ToInt32(Convert.ToDecimal(lblMoney.Content.ToString()) * 10)).ToString("X4");
                    //        }
                    //        else
                    //        {
                    //            MyTempMoney = (Convert.ToInt32(Convert.ToDecimal(lblMoney.Content.ToString()) * 100)).ToString("X4");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        MyTempMoney = Convert.ToInt32(Convert.ToDecimal(lblMoney.Content.ToString())).ToString("X4");
                    //    }
                    //}
                    //else
                    //{
                    //    MyTempMoney = (Convert.ToInt32(Convert.ToDecimal(lblMoney.Content.ToString()) * 10)).ToString("X4");
                    //}

                    //CR.SendVoice.LedShow2010znykt(axznykt_1, Model.Channels[imodulus].iCtrlID, Model.Channels[imodulus].sIP, MyTempMoney, m_nSerialHandle, Model.Channels[imodulus].iXieYi);


                    if (Model.iOnlinePayEnabled == 1 && Model.bPayTest)//2016-07-05 测试模式  应收金额都为0.01
                    {
                        lblSFJE.Content = "0.01";
                        lblMoney.Content = "0.01";
                    }

                }

            }
            catch (Exception ex)
            {
                gsd.AddLog("无牌车出场" + ":btnSF_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnSF_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0 && bLoadMouse)
                {
                    var selectItem = dgvCar.SelectedItem as CarIn;
                    //object obj = selectItem["CPH"];
                    string inPic = selectItem.InPic;
                    string srtInPic = inPic;
                    DateTime dtStartHF = selectItem.InTime;
                    string strGateInName = selectItem.InGateName;
                    string strCardType = ParkingInterface.CR.GetCardType(cmbCardType.Text, 0);
                    string strCardNO = selectItem.CardNO;
                    string strCPH = selectItem.CPH;

                    lblInDateTime.Content = dtStartHF.ToString("yyyy-MM-dd HH:mm:ss");   //2016-06-12

                    CarOut model = new CarOut();
                    model.CardNO = strCardNO;
                    model.CPH = strCPH;
                    model.CardType = strCardType;
                    model.InTime = dtStartHF;
                    model.OutTime = DateTime.Now;
                    model.OutGateName = cmbGateName.Text + "无牌车";
                    model.OutOperator = Model.sUserName;
                    model.OutOperatorCard = Model.sUserCard;
                    model.SFJE = Convert.ToDecimal(lblMoney.Content.ToString() == "" ? "0" : lblMoney.Content.ToString());
                    model.YSJE = Convert.ToDecimal(lblSFJE.Content.ToString() == "" ? "0" : lblSFJE.Content.ToString());
                    model.Balance = 0;
                    model.SFTime = DateTime.Now;
                    model.OvertimeSFTime = DateTime.Now;
                    model.OutPic = strPic;
                    //model.InOut = Model.Channels[imodulus].iInOut;
                    model.BigSmall = Model.Channels[imodulus].iBigSmall;
                    model.OutUser = "无牌车出场";
                    model.CarparkNO = Model.iParkingNo;

                    gsd.AddOutName(model,20);

                    string sOutTime = model.OutTime.ToString("yyyy-MM-dd HH:mm:ss");

                    string strPayType = "";
                    if (iPayType > 0)
                    {
                        if (iPayType == 1)
                        {
                            strPayType = "微信";
                        }
                        else if (iPayType == 2)
                        {
                            strPayType = "支付宝";
                        }
                    }
                    string strDZFree = "";
                    if (sCouponAddr != "")
                    {
                        strDZFree = "免费：" + sCouponAddr;
                    }
                    
                    gsd.UpdateSFJE(model.CardNO, CR.GetCardType(cmbCardType.Text, 0), Convert.ToDateTime(lblInDateTime.Content), Convert.ToDecimal(lblMoney.Content), Convert.ToDateTime(sOutTime), model.OutGateName, 2, "", Convert.ToDecimal(lblSFJE.Content), strDZFree, cmbFree.Text, strPayType);


                    iPayType = 0;

                    string strRst = vs.SendOpen(imodulus);

                    // string strRst = CR.SendVoice.SendOpen(axznykt_1, Model.Channels[imodulus].iCtrlID, Model.Channels[imodulus].sIP, 0x0C, 5, m_hLPRClient, Model.Channels[imodulus].iXieYi);//开闸
                    if (strRst == "0")   //开闸成功
                    {
                        vs.VoiceDisplay(VoiceType.TempOutOpen, imodulus);
                        //string sLoad = CR.LedSound_New(Model.byteLSXY[Model.iLSIndex, 3], "FFFF", "FFFF", "FFFF", "");
                        //CR.SendVoice.ShowLed55(axznykt_1, Model.Channels[imodulus].iCtrlID, Model.Channels[imodulus].sIP, m_nSerialHandle, Model.Channels[imodulus].iXieYi);
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnOpen_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnOpen_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFree_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbFree.Text == "")
                {
                    MessageBox.Show("请选择免费原因！", "提示");
                    return;
                }

                if (System.Windows.Forms.MessageBox.Show("请确认是否免费？\t\r若点击【是】，该车辆将被免费！", "提示", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                if (dgvCar.Items.Count > 0)
                {
                    if (dgvCar.SelectedIndex > -1)
                    {

                        var selectItem = dgvCar.SelectedItem as  CarIn;
                        //object obj = selectItem["CPH"];
                        string inPic = selectItem.InPic;
                        string srtInPic = inPic;
                        DateTime dtStartHF = selectItem.InTime;
                        string strGateInName = selectItem.InGateName;
                        string strCardType = ParkingInterface.CR.GetCardType(cmbCardType.Text, 0);
                        string strCardNO = selectItem.CardNO;
                        string strCPH = selectItem.CPH;

                        CarOut model = new CarOut();
                        model.CardNO = strCardNO;
                        model.CPH = strCPH;
                        model.CardType = strCardType;
                        model.InTime = dtStartHF;
                        model.OutTime = DateTime.Now;
                        model.OutGateName = cmbGateName.Text + "无牌车";
                        model.OutOperator = Model.sUserName;
                        model.OutOperatorCard = Model.sUserCard;
                        model.SFJE = 0;
                        model.YSJE = Convert.ToDecimal(lblSFJE.Content.ToString() == "" ? "0" : lblSFJE.Content.ToString());
                        model.Balance = 0;
                        model.SFTime = DateTime.Now;
                        model.OvertimeSFTime = DateTime.Now;
                        model.OutPic = strPic;
                        //model.InOut = Model.Channels[imodulus].iInOut;
                        model.BigSmall = Model.Channels[imodulus].iBigSmall;
                        model.OutUser = "无牌车出场";

                        //bll.AddOutName(model, 20);
                        gsd.AddOutName(model, 20);

                        string sOutTime = model.OutTime.ToString("yyyy-MM-dd HH:mm:ss");

                        //更新应收金额
                        gsd.UpdateYSJE(Convert.ToDecimal(lblSFJE.Content), model.CardNO, Model.iParkingNo, Convert.ToDateTime(lblInDateTime.Content), Convert.ToDateTime(sOutTime));

                        gsd.UpdateFreeReason("Tfr", cmbFree.Text, model.CardNO, Model.iParkingNo, Convert.ToDateTime(lblInDateTime.Content), inPic, Convert.ToDateTime(sOutTime));


                        bLoadMouse = false;

                        string strRst = vs.SendOpen(imodulus);

                        //string strRst = CR.SendVoice.SendOpen(axznykt_1, Model.Channels[imodulus].iCtrlID, Model.Channels[imodulus].sIP, 0x0C, 5, m_hLPRClient, Model.Channels[imodulus].iXieYi);//开闸
                        if (strRst == "0")   //开闸成功
                        {
                            vs.VoiceDisplay(VoiceType.TempOutOpen, imodulus);
                            //string sLoad = CR.CR.LedSound_New(Model.byteLSXY[Model.iLSIndex, 3], "FFFF", "FFFF", "FFFF", "");
                            //CR.SendVoice.ShowLed55(axznykt_1, Model.Channels[imodulus].iCtrlID, Model.Channels[imodulus].sIP, m_nSerialHandle, Model.Channels[imodulus].iXieYi);
                        }

                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnOK_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnOK_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgvCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0)
                {
                    if (dgvCar.SelectedIndex > -1)
                    {
                        var dr = dgvCar.SelectedItem as CarIn;
                        string inPic = dr.InPic;
                        string strInPic = inPic;
                        DateTime dtSartHF = dr.InTime;

                        lblInDateTime.Content = dr.InTime;

                        string strGateInName = dr.InGateName;
                        string strCardType = CR.GetCardType(dr.CardType, 0);
                        loadPic(inPic, picCarIn);

                        //if (inPic != "")
                        //{
                        //    if (System.IO.File.Exists(inPic))
                        //    {
                        //        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(inPic), picCarIn.Width, picCarIn.Height);
                        //        picCarIn.Image = bm;

                        //    }
                        //    else
                        //    {
                        //        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, picCarIn.Width, picCarIn.Height);
                        //        picCarIn.Image = bm;
                        //    }
                        //}
                        //else
                        //{
                        //    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, picCarIn.Width, picCarIn.Height);
                        //    picCarIn.Image = bm;
                        //}
                        btnSF.IsEnabled = true;
                        btnOpen.IsEnabled = false;
                        btnFree.IsEnabled = false;
                        btnPrint.IsEnabled = false;

                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvCar_MouseClick", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dgvCar_MouseClick", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// 加载图片到控件
        /// </summary>
        /// <param name="path">图片绝对路径</param>
        /// <param name="pic">控件名</param>
        /// <param name="inout">进出标识</param>
        public void loadPic(string path, System.Windows.Forms.PictureBox pic)
        {
            if (path != "")
            {
                if (System.IO.File.Exists(path))
                {
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(path), pic.Width, pic.Height);
                    pic.Image = bm;
                }
                else
                {
                    if (path.Contains(Model.sImageSavePath))
                    {
                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                        pic.Image = bm;
                    }
                    else
                    {
                        if (path.Substring(0, 12) == "CaptureImage")
                        {
                            if (System.IO.File.Exists(Model.sImageSavePath + path))
                            {
                                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(Model.sImageSavePath + path), pic.Width, pic.Height);
                                pic.Image = bm;
                            }
                            else
                            {
                                System.Threading.ThreadPool.QueueUserWorkItem((ot) =>
                                {
                                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                    new Action(() =>
                                    {
                                        bool ret = gsd.DownLoadPic(path, ot.ToString());

                                        if (ret)
                                        {
                                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(ot.ToString()), pic.Width, pic.Height);
                                            pic.Image = bm;
                                        }
                                        else
                                        {
                                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                                            pic.Image = bm;

                                        }
                                    }));
                                }, Model.sImageSavePath + path);
                            }
                        }
                        else
                        {
                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                            pic.Image = bm;
                        }
                    }
                }
            }
            else
            {
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                pic.Image = bm;
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InNoCPHHandler();
        }


        void ShowRights()
        {
            GetUiAllRightButton((this.Content as Grid).Children);
            long pid = gsd.GetIDByName(this.Title, "CmdView");
            List<RightsItem> lstRI = new List<RightsItem>();
            if (lstRightButton == null || lstRightButton.Count == 0)
            {
                return;
            }
            foreach (var v in lstRightButton)
            {
                List<Rights> lstRs = gsd.GetRightsByName(v.FormName, v.ItemName);
                if (lstRs.Count > 0)
                {
                    v.Visibility = lstRs[0].CanView == true ? Visibility.Visible : Visibility.Hidden;
                    v.IsEnabled = lstRs[0].CanOperate;
                }
                else
                {
                    lstRI.Add(new RightsItem() { FormName = v.FormName, ItemName = v.ItemName, Category = "车场", Description = v.Content.ToString(), PID = pid });
                }
            }
            if (lstRI.Count > 0)
            {
                gsd.SetRightsItem(lstRI);
            }
        }

        List<SFMControls.ButtonSfm> lstRightButton = new List<SFMControls.ButtonSfm>();
        private void GetUiAllRightButton(UIElementCollection uiControls)
        {
            foreach (UIElement element in uiControls)
            {
                if (element is SFMControls.ButtonSfm)
                {
                    SFMControls.ButtonSfm current = element as SFMControls.ButtonSfm;
                    if (current.ItemName != "" && current.FormName != "" && current.ItemName != null && current.FormName != "")
                    {
                        lstRightButton.Add(current);
                    }
                }
                else if (element is Grid)
                {
                    GetUiAllRightButton((element as Grid).Children);
                }
                else if (element is Expander)
                {
                    if ((element as Expander).Content is StackPanel)
                    {
                        StackPanel sa = (element as Expander).Content as StackPanel;
                        GetUiAllRightButton(sa.Children);
                    }
                    else if ((element as Expander).Content is Grid)
                    {
                        Grid sa = (element as Expander).Content as Grid;
                        GetUiAllRightButton(sa.Children);
                    }
                }
                else if (element is StackPanel)
                {
                    GetUiAllRightButton((element as StackPanel).Children);
                }
                else if (element is ScrollViewer)
                {
                    StackPanel sp = (element as ScrollViewer).Content as StackPanel;
                    GetUiAllRightButton(sp.Children);
                }
                else if (element is TabControl)
                {
                    foreach (UIElement Pageelment in (element as TabControl).Items)
                    {
                        TabItem tabtemp = (TabItem)Pageelment;

                        Grid gd = tabtemp.Content as Grid;
                        GetUiAllRightButton(gd.Children);
                    }
                }
                else if (element is GroupBox)
                {
                    GroupBox tabtemp = (GroupBox)element;
                    Grid gd = tabtemp.Content as Grid;
                    if (gd != null)
                        GetUiAllRightButton(gd.Children);
                }
            }
        }

        private void cmbJHDZ_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                iCouponJH = 0;
                sCouponAddr = "";
                sCouponMode = "";
                sCouponValue = 0;
                if (bLoad == true)
                {
                    string address = cmbJHDZ.SelectedValue.ToString();
                    //iCouponJH = iJiHao;
                    List<ParkDiscountJHSet> jiHaoDT = gsd.GetJiHaoDZ(address);
                    if (jiHaoDT.Count > 0)
                    {
                        sCouponAddr = jiHaoDT[0].Address;
                        sCouponMode = jiHaoDT[0].Manner;
                        sCouponValue = jiHaoDT[0].Favorable;
                    }
                    if (sCouponAddr != "")
                    {
                        if (sCouponMode == "小时")
                        {
                            if (Convert.ToInt32(sCouponValue) == 9999)
                            {
                                lblMoney.Content = "0.0";
                 
                            }
                            else
                            {
                                if (CR.DateDiff(CR.DateInterval.Minute, Convert.ToDateTime(lblInDateTime.Content ?? DateTime.Now), DateTime.Now) <= sCouponValue * 60)
                                {
                                    lblMoney.Content = "0.0";
                                }
                                else
                                {
                                    DateTime dtInTime = Convert.ToDateTime(lblInDateTime.Content).AddHours(Convert.ToInt32(sCouponValue));
                                    CaclMoneyResult dOut = gsd.GetMONEY(CR.GetCardType(cmbCardType.Text, 0), dtInTime, DateTime.Now, txtCPH.Text ?? "");
                                    lblMoney.Content = Convert.ToDouble(dOut.SFJE).ToString("0.0");

                                }
                            }
                        }
                        else if (sCouponMode == "元")
                        {
                            if (Convert.ToDecimal(lblSFJE.Content.ToString() == "" ? 0 : lblSFJE.Content) < sCouponValue)
                            {
                                lblMoney.Content = "0.0";
                            }
                            else
                            {
                                //带小数点收费还未处理
                                if (Model.iXsd == 1)
                                {
                                    decimal dMoney = (Convert.ToDecimal(lblSFJE.Content) * 10 - sCouponValue) / 10;
                                    lblMoney.Content = dMoney.ToString();
                                }
                                else
                                {
                                    decimal dMoney = Convert.ToDecimal(lblSFJE.Content) - sCouponValue;
                                    lblMoney.Content = Convert.ToDouble(dMoney).ToString("0.0");
                                }

                            }
                        }
                        bDZ = true;

                      
                        if (Convert.ToDecimal(lblMoney.Content) > 0)
                            vs.VoiceDisplay(VoiceType.OutGateVoice, imodulus, "TmpA", "", 0, "", 0, Convert.ToDecimal(lblMoney.Content));
                        

                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":cmbJHDZ_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "cmbJHDZ_SelectionChanged", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void txtAuth_code_KeyDown(object sender, KeyEventArgs e)
        {
            Request req = new Request();
            Response result;

            if (e.Key != Key.Enter)//如果输入的是回车键  
            {
                return;
            }
            if (string.IsNullOrEmpty(txtAuth_code.Text))
            {
                MessageBox.Show("请输入授权码！");
                return;
            }

            if (Convert.ToDecimal(lblMoney.Content) <= 0)
            {
                MessageBox.Show("请输入大于0的收费金额！");
                txtAuth_code.Text = "";
                return;
            }
            int iFlag = 0;
            string strContent = "无牌车---交费：" + lblMoney.Content + "元" ;

            iFlag = (radWX.IsChecked ?? false) ? 0 : 1;
            //ShowOnlinePayLayer((radWX.IsChecked ?? false) ? "微信支付中，请稍候..." : "支付宝支付中，请稍候...");
            //bool brst = CR.SMPay(iFlag, txtSFJE.Text.Trim(), txtAuth_code.Text, strContent, lblCPH.Content);
            result = req.OnlinePay(txtAuth_code.Text, iFlag, Convert.ToDecimal(lblMoney.Content), strContent, "");
            //lblOnlinePayLayer.Visibility = System.Windows.Visibility.Collapsed;

            if (null != result && result.rcode == "200")
            {
                iPayType = (radWX.IsChecked ?? false) ? 1 : 2;

                btnOpen_Click(null, null);
            }
            else
            {
                if (null != result)
                {
                    Request res = new Request();
                    res.AddLog("在线监控", "在线支付失败：" + result.msg);
                    System.Diagnostics.Trace.WriteLine("在线支付失败：" + result.msg);
                }

                MessageBox.Show("支付失败！！");
                txtAuth_code.Text = "";
            }

            e.Handled = true;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0 && dgvCar.SelectedIndex > -1)
                {
                    var vr = dgvCar.SelectedItem as CarIn;

                    List<BillPrintSet> lstBPS = gsd.GetPrint();
                    FlowDocument doc = new FlowDocument();
                    Paragraph ph = new Paragraph();

                    ph.Inlines.Add(new Run("              临时车收费票据"));
                    if (lstBPS.Count > 0 && lstBPS != null)
                    {
                        if (lstBPS[0].Title != "")
                        {
                            ph.Inlines.Add(new Run("\r\n" + "              " + lstBPS[0].Title));
                        }
                        if (lstBPS[0].FTitle != "")
                        {
                            ph.Inlines.Add(new Run("\r\n" + "              " + lstBPS[0].FTitle));
                        }
                    }
                    ph.Inlines.Add(new Run("\r\n-----------------------------------"));
                    doc.Blocks.Add(ph);
                    doc.Blocks.Add(new Paragraph(new Run("车牌号码: 无牌车")));
                    doc.Blocks.Add(new Paragraph(new Run("车辆编号:" + vr.CardNO)));
                    doc.Blocks.Add(new Paragraph(new Run("车辆类型:" + cmbCardType.Text)));
                    doc.Blocks.Add(new Paragraph(new Run("入场时间:" + vr.InTime.ToString("yyyy-MM-dd HH:mm:ss"))));
                    doc.Blocks.Add(new Paragraph(new Run("出场时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                    doc.Blocks.Add(new Paragraph(new Run("应收金额:" + lblSFJE.Content ?? "0.0" + "元")));
                    doc.Blocks.Add(new Paragraph(new Run("收费金额:" + lblMoney.Content ?? "0.0" + "元")));

                    doc.Blocks.Add(new Paragraph(new Run("操作员姓名:" + Model.sUserName)));

                    doc.Blocks.Add(new Paragraph(new Run("打折方式:" + cmbJHDZ.Text)));

                    Paragraph ph1 = new Paragraph();
                    ph1.Inlines.Add(new Run("-----------------------------------"));
                    if (lstBPS.Count > 0 && lstBPS != null)
                    {
                        if (lstBPS[0].Footer != "")
                        {
                            ph1.Inlines.Add(new Run("\r\n" + lstBPS[0].Footer));
                        }
                    }

                    doc.Blocks.Add(ph1);
                    rtbPrint.Document = doc;
                    var printDialog = new PrintDialog();
                    printDialog.PrintQueue = GetPrinter();
                    printDialog.PrintDocument(((IDocumentPaginatorSource)rtbPrint.Document).DocumentPaginator, "A Flow Document");
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnPrint_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + ":btnPrint_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public PrintQueue GetPrinter(string printerName = null)
        {
            try
            {
                PrintQueue selectedPrinter = null;
                if (!string.IsNullOrEmpty(printerName))
                {
                    var printers = new LocalPrintServer().GetPrintQueues();
                    selectedPrinter = printers.FirstOrDefault(p => p.Name == printerName);
                }
                else
                {
                    selectedPrinter = LocalPrintServer.GetDefaultPrintQueue();
                }
                return selectedPrinter;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private void cmbCardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0)
                {
                    if (dgvCar.SelectedIndex > -1)
                    {

                        var vr = dgvCar.SelectedItem as CarIn;
                        string strTmp = "";
                        string CardType = CR.GetCardType(cmbCardType.Text, 0);
                        if (CardType != "无效卡" && bLoad == true)
                        {
                            string strCPH = vr.CPH;
                            DateTime dtInTime = vr.InTime;
                            string strPic = vr.InPic;
                            string strCardType = CardType;

                            txtCPH.Text = strCPH;

                            if (strCardType.Substring(0, 3) == "Tmp")
                            {
                                CaclMoneyResult d1 = gsd.GetMONEY(strCardType, dtInTime, DateTime.Now);
                                lblMoney.Content = "0.00";

                                bLoadMouse = true;
                                btnOpen.IsEnabled = false;
                                btnPrint.IsEnabled = false;
                                //是否允许免费
                                if (Model.iTempFree == 1)
                                {
                                    btnFree.IsEnabled = true;
                                    cmbFree.IsEnabled = true;
                                }
                                //btnPrint.Enabled = true;
                            }
                            else
                            {
                                lblMoney.Content = "0.00";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":cmbCardType_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + ":cmbCardType_SelectionChanged", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
    }
}
