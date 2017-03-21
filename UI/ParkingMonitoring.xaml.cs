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
using ParkingCommunication;
using System.Threading;
using ParkingModel;
using System.Data;
using System.Windows.Threading;
using ParkingInterface;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using System.Windows.Media.Animation;


namespace UI
{
    /// <summary>
    /// ParkingMonitoring.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingMonitoring : SFMControls.WindowBase
    {
        #region Fields
        /// <summary>
        /// 访问服务端对象
        /// </summary>
        GetServiceData gsd = new GetServiceData();

        //private bool isCarIn = false;

        /// <summary>
        /// 
        /// </summary>
        private Hid usbHid = new Hid();

        private bool myChecking;

        /// <summary>
        /// 车道数索引
        /// </summary>
        private int modulus = 100;

        /// <summary>
        /// 卡片类型指令
        /// </summary>
        private string cardTypeCmd;

        /// <summary>
        /// 卡片号码
        /// </summary>
        private string cardNO;


        /// <summary>
        /// 入出场标识(0表示入场，1表示出场, Flag)
        /// </summary>
        private int InOut;

        /// <summary>
        /// FlagTwo
        /// </summary>
        private bool isTwo;

        /// <summary>
        /// FlagOne
        /// </summary>
        private bool isOne;

        /// <summary>
        /// ResumeNO
        /// </summary>
        private string tempCardNO;

        /// <summary>
        /// 入口图像
        /// </summary>
        private string[] inPic = new string[11];

        /// <summary>
        /// 车辆数据是否写入了出场库(记录数 recordCount)
        /// </summary>
        private int recordCount;

        /// <summary>
        /// 根据不同的值分别读取不同的数据   如： 1：临时卡车牌 2：进免费车车牌 3：出进免费车车牌
        /// </summary>
        private int carSetup;

        private int lostFlag;

        /// <summary>
        /// 卡片号码
        /// </summary>
        private string[] cardNoNo = new string[10];

        /// <summary>
        /// 入场时间
        /// </summary>
        private DateTime[] inTime = new DateTime[10];

        /// <summary>
        /// 出场时间
        /// </summary>
        private DateTime[] outTime = new DateTime[10];

        /// <summary>
        /// 收费金额（Shoufeijiner）
        /// </summary>
        private double[] charge = new double[10];

        /// <summary>
        /// 储值余额(ChuzhiYuer)
        /// </summary>
        private double[] storeBalance = new double[10];

        /// <summary>
        /// 车牌号码
        /// </summary>
        private string[] carNoNo = new string[10];

        /// <summary>
        /// 车辆颜色
        /// </summary>
        private string[] carColor = new string[10];

        /// <summary>
        /// 卡片种类
        /// </summary>
        private string[] cardType = new string[10];

        /// <summary>
        /// 车辆类型（CarXinHao）
        /// </summary>
        private string[] carType = new string[10];

        /// <summary>
        /// picFileName
        /// </summary>
        private string picFileName = "";

        /// <summary>
        /// 图像文件名
        /// </summary>
        private string filesJpg;

   
        ///// <summary>
        ///// 删除上一张图片
        ///// </summary>
        //private string delLastInImage = "";

        private string delLastOutImage = "";
   

        /// <summary>
        /// picFileNameTemp
        /// </summary>
        private string[] temppicFileName = new string[2];

        /// <summary>
        /// 入出口名称
        /// </summary>
        private string[] inOutName = new string[10];

        private int[] myIDICFlag = new int[10];

        /// <summary>
        /// 入出场图片（inOutPic）
        /// </summary>
        private string[] inOutPic = new string[10];

        private string[] inOutPicR = new string[10];

        /// <summary>
        /// 临时卡收费（LskMoney）
        /// </summary>
        private string tempMoney;

        /// <summary>
        /// 月卡剩余天数(YkDay)
        /// </summary>
        private long monthSurplusDay = 0;

        /// <summary>
        /// 月卡剩余天数（CYkDay)
        /// </summary>
        private string monthSurplusDayCmd = "";

        /// <summary>
        /// 加载时间
        /// </summary>
        private DateTime loadTime;

        /// <summary>
        /// 车牌号码(指令 strPlateTalk)
        /// </summary>
        private string carNoCmd = "";

        /// <summary>
        /// bInOutCtrl
        /// </summary>
        private bool inOutCtrl;

        /// <summary>
        /// bStopIn
        /// </summary>
        private bool[] forbidIn = new bool[11];

        /// <summary>
        /// bErr
        /// </summary>
        private bool bErr;

        /// <summary>
        /// bMonthOut
        /// </summary>
        private bool monthOut;

        /// <summary>
        /// iPaiCheOut
        /// </summary>
        private byte carNoOut;

        /// <summary>
        /// lOutCarCount
        /// </summary>
        private int outCarCount;

        private bool bCzyKZ;

        /// <summary>
        /// bOutTalkTemp
        /// </summary>
        private bool bOutTalkTemp;

        private string strCardCW = "";

        /// <summary>
        /// 提示延时
        /// </summary>
        private int iDisplayDelay;

        private string[] strCardTmp = new string[11];

        private int[] iCardSec = new int[11];

        private int iCardMemrySecLimit;

        private bool bNoSound;

        private bool[] bDateTimeErr = new bool[2];

        private string sTmp;

        /// <summary>
        /// 控制机显示屏延时显示剩余车位
        /// </summary>
        private int iCtrlLedDelay;

        private bool bAutoWritePlateIng;

        private string strAutoWritePlateIng;

        private bool[] bStopInInit = new bool[11];

        private string sInGate;

        private string sInUser;

        private string strErrType;

        private string strTmpType;

        private string strJdLed;

        private bool bOpenWindows;

        private int iModifyCarPos;

        private DateTime dLastChuKaTime;

        private bool bPaperRecord;

        /// <summary>
        /// 是否为纸票记录
        /// </summary>
        private bool[] bPaperBill = new bool[11];

        /// <summary>
        /// 纸票记录字符串
        /// </summary>
        private string sPaperScan;

        /// <summary>
        /// 扫描纸票的索引
        /// </summary>
        private int iPaperIndex;

        private int iIndex;

        /// <summary>
        /// 纸票卡号
        /// </summary>
        private string sPaperCardNO;

        /// <summary>
        /// 取条码延时
        /// </summary>
        private int[] iPaperDelay = new int[11];

        /// <summary>
        /// 图片是否抓拍
        /// </summary>
        private bool[] bReadPicAuto = new bool[11];

        /// <summary>
        /// 图片路径
        /// </summary>
        private string[] strReadPicFile = new string[11]; //定义为11个字符串

        /// <summary>
        /// 图片路径
        /// </summary>
        private string[] strReadPicFileJpg = new string[11];

        /// <summary>
        /// 压递感识别车牌的索引
        /// </summary>
        private int iDzIndex;

        /// <summary>
        /// 识别车牌号
        /// </summary>
        private string[] autoCarNo = new string[11];

        /// <summary>
        /// 读取识别车牌号
        /// </summary>
        private string readAutoCarNo;

        /// <summary>
        /// 区别读卡或者识别（false读卡记录 true识别记录）
        /// </summary>
        private bool bReadAuto = false;

        /// <summary>
        /// 车牌X秒后过期(iCphDelayArray)
        /// </summary>
        private int[] iCarNoDelay = new int[11];

        /// <summary>
        /// 卡号X秒后过期(iCardDelayArray)
        /// </summary>
        private int[] iCardDelay = new int[11];

        private bool bCarNoConfirm = false;

        /// <summary>
        /// 压地感识别车牌记录
        /// </summary>
        private bool[] bDzBill = new bool[11];

        /// <summary>
        /// myCarNo
        /// </summary>
        private string[] myCarNo = new string[11];

        /// <summary>
        /// 压地感识别车牌
        /// </summary>
        private string sDzScan;

        private static string strNoCarNo = "";

        /// <summary>
        /// 卡号进入等待
        /// </summary>
        private bool[] bCardStop = new bool[11];

        /// <summary>
        /// 进行二次解析
        /// </summary>
        private bool[] bScondCard = new bool[11];

        private string[] strCardNos = new string[11];

        private int bScondmodus = 0;

        long[] lOutID = new long[11];

        public static string File = "";

        /// <summary>
        /// 道闸图片控件注册(picListDZ)
        /// </summary>
        List<System.Windows.Forms.PictureBox> lstPicDZ = new List<System.Windows.Forms.PictureBox>();

        /// <summary>
        /// 低杆图片空间注册(picListDG)
        /// </summary>
        List<System.Windows.Forms.PictureBox> lstPicDG = new List<System.Windows.Forms.PictureBox>();

        /// <summary>
        /// 卡机控件注册(picListCard)
        /// </summary>
        List<System.Windows.Forms.PictureBox> lstPicCard = new List<System.Windows.Forms.PictureBox>();

        /// <summary>
        /// 车卡号码注册(Lbl_NoList)
        /// </summary>
        List<Label> lstLblCardNo = new List<Label>();

        /// <summary>
        /// 车卡类型注册(Lbl_TypeList)
        /// </summary>
        List<Label> lstLblCardType = new List<Label>();

        /// <summary>
        /// 入出时间注册(Lbl_TimeList)
        /// </summary>
        List<Label> lstLblInOutTime = new List<Label>();

        /// <summary>
        /// 车牌号注册
        /// </summary>
        List<TextBlock> lstTxbCarNo = new List<TextBlock>();

        /// <summary>
        /// 收费金额注册
        /// </summary>
        List<Label> lstLblCharge = new List<Label>();

        /// <summary>
        /// 车道名称
        /// </summary>
        List<Label> lstLblLaneName = new List<Label>();

        /// <summary>
        /// 开闸
        /// </summary>
        List<Button> lstBtnCmdOpen = new List<Button>();

        /// <summary>
        /// 关闸
        /// </summary>
        List<Button> lstBtnCmdClose = new List<Button>();

        /// <summary>
        /// 网络摄像头
        /// </summary>
        List<System.Windows.Forms.PictureBox> lstPicVideo = new List<System.Windows.Forms.PictureBox>();

        List<Button> lstBtnManual = new List<Button>();

        List<System.Windows.Forms.PictureBox> lstPicSmallCarNo = new List<System.Windows.Forms.PictureBox>();

        List<Image> lstImgSmallCarNo = new List<Image>();

        List<System.Windows.Forms.Integration.WindowsFormsHost> lstWfhSmallCarNo = new List<System.Windows.Forms.Integration.WindowsFormsHost>();

        Size[] videoRawSize = new Size[4];

        private Thread fThread;

        private Thread fThreadtimer3;

        //private Thread fThreadbank;

        /// <summary>
        /// 退出窗口
        /// </summary>
        private bool bExit = false;

        /// <summary>
        /// 读卡线程退出成功
        /// </summary>
        private bool bThreadReadExitOK = false;

        /// <summary>
        /// 写卡线程退出成功
        /// </summary>
        private bool bThreadReadTimer3ExitOK = false;

        /// <summary>
        /// 摄像头类型
        /// </summary>
        private string[] strVideoType = new string[11];

        /// <summary>
        /// 月卡过期
        /// </summary>
        private bool bMonthPastdue = false;

        private string localImageIn = "";
        private string localImageOut = "";

        /// <summary>
        /// 启用主线程
        /// </summary>
        private bool isStart = true;

        /// <summary>
        /// 发送车场满位是否延时
        /// </summary>
        private bool bLoadCar = false;

        /// <summary>
        /// 优惠模式:1-车牌打折 2-机号打折 3-免费车辆(iYHMode) 
        /// </summary>
        private int iCouponMode = 0;

        /// <summary>
        /// 优惠机号 iYHJH 
        /// </summary>
        private int iCouponNo = 0;

        /// <summary>
        /// 根据机号查询出优惠位置 sYHAdr
        /// </summary>
        private string sCouponAddr = "";

        /// <summary>
        /// 优惠方式 (分钟，元) sYHType
        /// </summary>
        private string sCouponMode = "";

        /// <summary>
        /// 优惠值（分钟，元）sYHValue
        /// </summary>
        private decimal sCouponValue = 0;

        private string[] strCarNoColor = new string[11];

        private bool[] bOffLine = new bool[11];

        private bool[] bLoadFullCW = new bool[11];

        //private int readkeyfailcount = 0;

        private bool[] bIsMoth = new bool[11];

        /// <summary>
        /// 应收金额
        /// </summary>
        private decimal decYSJE = 0;

        List<CheDaoSet> lstCDS = new List<CheDaoSet>();

        VoiceSend sender0;

        //用于记录窗体上显示的控件的值
        MonitorModel monitor = new MonitorModel();
        Summary summary0 = new Summary();

        DateTime dLastDelImageDate = new DateTime();  //最后一次自动删除图片的时间

        Size rawSize = new Size();

        DispatcherTimer dTimer = new DispatcherTimer();
        System.Timers.Timer timer = new System.Timers.Timer();


        SedBll Readsendbll = new SedBll("", 1007, 1005);

        private string strRemberCPH = "";
        private DateTime timeCPH;
        private int iRemberInOut = 0;

        CaclMoneyResult cacl = new CaclMoneyResult();

        //记录初始显示状态
        Visibility[] rawVI = new Visibility[4];
        #endregion


        #region Delegate
        public delegate void ReadCardHandler();

        public delegate void NoCPHHandler(string CPH);

        private void NoCPH(string CPH)
        {
            strNoCarNo = CPH;
        }

        public delegate void InNoCPHHandler();

        private void RefreshInOut()
        {
            GetBinInOut();
            loadCar();
        }

        public delegate void UpdateCPHDataHandler(string CPH, int Count, DateTime dtIn);

        private void UpdateCPH(string CPH, int Count,DateTime dtIn)
        {
            if (Count == 1000)
            {
                if (System.IO.File.Exists(CPH))
                {
                    System.Drawing.Image fileImage = System.Drawing.Image.FromFile(filesJpg);
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(fileImage, fileImage.Width, fileImage.Height);

                    // UpdatePicImage(lstPicVideo[2], bm);
                    lstPicVideo[2].Image = bm;
                    localImageIn = inPic[0];
                }
                else
                {
                    //UpdatePicImage(lstPicVideo[2], Properties.Resources.Car2);
                    lstPicVideo[2].Image = Properties.Resources.Car2;
                    localImageIn = "";
                }
            }
            else
            {
                lstTxbCarNo[Count].Text = CPH;
                lblInTime.Content = dtIn.ToString("yyyy-MM-dd HH:mm:ss");

            }
            GetBinInOut();
            loadCar();
        }

        private delegate void SFDataHandler(string SFJE);

        private void UpdateSFJE(string SFJE)
        {
            this.Focus();
            lblCharge.Content = SFJE;
            txbCharge.Text = SFJE;
        }

        private void BinData(string CardType, decimal SFJE)
        {
            if (CR.IsChineseCharacters(CardType))
            {
                lblCardType.Content = CardType;
            }
            else
                lblCardType.Content = CR.GetCardType(CardType, 1);
            lblCharge.Content = SFJE;
        }

        private delegate void ParkingMonitoringPhotoHandler(string Count);

        private void TempGob_big_Photo(string count)
        {
            string Filebmps = "", PathStr = "";
            DateTime MyCapDateTime;

            if (Model.sImageSavePath.Substring(Model.sImageSavePath.Length - 1) != @"\")
            {
                Model.sImageSavePath = Model.sImageSavePath + @"\";
            }
            MyCapDateTime = DateTime.Now;
            PathStr = Model.sImageSavePath + MyCapDateTime.ToString("yyyyMMdd");
            if (System.IO.Directory.Exists(PathStr) == false)
            {
                System.IO.Directory.CreateDirectory(PathStr);
            }
            Filebmps = PathStr + @"\" + monitor.CardNo + MyCapDateTime.ToString("yyyyMMddHHmmss") + "证件" + ".bmp";
            File = PathStr + @"\" + monitor.CardNo + MyCapDateTime.ToString("yyyyMMddHHmmss") + "证件" + ".jpg";
        }

        public delegate void ParkingMonitoringWHandler(int Count);


        public delegate void UpdateStatuesHandler(string info);

        private void UpdateStatus(string info)
        {
            txbUserName.Text = Model.sUserName;
            txbUserNo.Text = Model.sUserCard;
            txbWorkTime.Text = Model.dLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
            txbOperatorInfo.Text = info;
        }

        #endregion


        #region Dispatcher
        private delegate void UpdateUiTextDelegate(Control control, string text);
        private void UpdateUiText(Control control, string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(UpdateUiText), control, text);
                return;
            }
            if (control.GetType().ToString() == "System.Windows.Controls.Label")
            {
                (control as Label).Content = text;
            }
            else if (control.GetType().ToString() == "System.Windows.Controls.TextBox")
            {
                (control as TextBox).Text = text;
            }
            else if (control.GetType().ToString() == "System.Windows.Controls.GroupBox")
            {
                (control as GroupBox).Header = text;
            }
        }


        private delegate void UpdateTxbTextDelegate(TextBlock tb, string text);
        private void UpdateTxbText(TextBlock tb, string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Send, new UpdateTxbTextDelegate(UpdateTxbText), tb, text);
                return;
            }
            tb.Text = text;
        }


        private delegate void UpdatePicImageDelege(System.Windows.Forms.PictureBox pic, System.Drawing.Bitmap image);

        private void UpdatePicImage(System.Windows.Forms.PictureBox pic, System.Drawing.Bitmap image)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Send, new UpdatePicImageDelege(UpdatePicImage), pic, image);
                return;
            }
            pic.Image = image;
        }


        private delegate void UpdateUiVisibilityDelege(Control control, Visibility vi);

        private void UpdateUiVisibility(Control control, Visibility vi)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiVisibilityDelege(UpdateUiVisibility), control, vi);
                return;
            }
            control.Visibility = vi;
        }
        #endregion


        #region Constructions
        public ParkingMonitoring()
        {
            InitializeComponent();
        }
        #endregion


        #region Methods
        #region Init
        /// <summary>
        /// 初始化成员变量
        /// </summary>
        public void initFields()
        {
            rawSize.Width = this.Width;
            rawSize.Height = this.Height;

            for (int i = 0; i < 11; i++)
            {
                strReadPicFile[i] = "";
                strReadPicFileJpg[i] = "";
                strCardTmp[i] = "";
                strCarNoColor[i] = "";
                autoCarNo[i] = "";
                bOffLine[i] = false;
                bLoadFullCW[i] = true;
                forbidIn[i] = false;
                bStopInInit[i] = false;
                //nCamId[i] = 11;
                bIsMoth[i] = false;
                //m_hLPRClient[i] = 0;
            }


            lstBtnManual.Add(btnManual0);// 对应于手动触发按钮
            lstBtnManual.Add(btnManual1);
            lstBtnManual.Add(btnManual2);
            lstBtnManual.Add(btnManual3);

            lstLblLaneName.Add(lblChannel0);// 对应图像显示通道信息
            lstLblLaneName.Add(lblChannel1);
            lstLblLaneName.Add(lblChannel2);
            lstLblLaneName.Add(lblChannel3);

            lstBtnCmdOpen.Add(btnCmdOpen0);// 开闸
            lstBtnCmdOpen.Add(btnCmdOpen1);
            lstBtnCmdOpen.Add(btnCmdOpen2);
            lstBtnCmdOpen.Add(btnCmdOpen3);

            lstBtnCmdClose.Add(btnCmdClose0);// 关闸
            lstBtnCmdClose.Add(btnCmdClose1);
            lstBtnCmdClose.Add(btnCmdClose2);
            lstBtnCmdClose.Add(btnCmdClose3);

            lstTxbCarNo.Add(txbCarNo0);
            lstTxbCarNo.Add(txbCarNo1);
            lstTxbCarNo.Add(txbCarNo2);
            lstTxbCarNo.Add(txbCarNo3);

            lstPicSmallCarNo.Add(picCarNoSmall0);
            lstPicSmallCarNo.Add(picCarNoSmall1);
            lstPicSmallCarNo.Add(picCarNoSmall2);
            lstPicSmallCarNo.Add(picCarNoSmall3);

            lstWfhSmallCarNo.Add(wfhCarNoSmall0);
            lstWfhSmallCarNo.Add(wfhCarNoSmall1);
            lstWfhSmallCarNo.Add(wfhCarNoSmall2);
            lstWfhSmallCarNo.Add(wfhCarNoSmall3);
            //lstImgSmallCarNo.Add()

            lstPicVideo.Add(picNetVideo0);
            lstPicVideo.Add(picNetVideo1);
            lstPicVideo.Add(picNetVideo2);
            lstPicVideo.Add(picNetVideo3);



            //2016-06-16 判断是否换班
            if (CR.GetAppConfig("UserCode") == Model.sUserCard)
            {
                Model.dLoginTime = Convert.ToDateTime(CR.GetAppConfig("LoginDate"));
            }
            else
            {
                CR.UpdateAppConfig("UserCode", Model.sUserCard);
                CR.UpdateAppConfig("LoginDate", DateTime.Now.ToString());
                Model.dLoginTime = DateTime.Now;
            }

            CR.BinDic(gsd.GetCardType()); // 更新卡片种类,并保存数据

            ComeGoFlagSumCar();

            lstCDS = gsd.GetReadName();

            //自动加载控制机 时间
            Model.iLoadTimeType = 0;

            loadTime = DateTime.Now.AddDays(-1);


            videoRawSize[0] = new Size(wfhVideo0.Width, wfhVideo0.Height);
            videoRawSize[1] = new Size(wfhVideo1.Width, wfhVideo1.Height);
            videoRawSize[2] = new Size(wfhVideo2.Width, wfhVideo2.Height);
            videoRawSize[3] = new Size(wfhVideo3.Width, wfhVideo3.Height);
        }

        private void ComeGoFlagSumCar()
        {
            for (int i = 0; i < Model.iChannelCount; i++)
            {
                if (Model.Channels[i].iInOut == 0) // 0为入，1为出
                {
                    isOne = true;
                }
                else if (Model.Channels[i].iInOut == 1 || Model.Channels[i].iInOut == 3)
                {
                    isTwo = true;
                    if (Model.Channels[i].iInOut == 3)
                    {
                        //MyShoukaji = 1;
                    }
                }
                else if (Model.Channels[i].iInOut == 4)
                {
                    isOne = true;
                    isTwo = true;
                }
            }
        }

        /// <summary>
        /// 初始化用户控件
        /// </summary>
        public void initControl()
        {
            ////// 设置全屏
            //this.WindowState = System.Windows.WindowState.Normal;
            //this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            //this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
            ////this.Topmost = true;

            //this.Left = 0.0;
            //this.Top = 0.0;
            //this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            //this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            sender0 = new VoiceSend(m_hLPRClient, m_nSerialHandle, 1007, 1005);
            sender0.LoadParameter();// 记载数据

            ImageBrush berriesBrush = new ImageBrush();
            berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Login.jpg"), UriKind.Absolute));

            this.Background = berriesBrush;

            //绑定实时画面的图片
            picNetVideo0.Image = Properties.Resources.Car2;
            picNetVideo1.Image = Properties.Resources.Car2;
            picNetVideo2.Image = Properties.Resources.Car2;
            picNetVideo3.Image = Properties.Resources.Car2;

            if (Model.iAutoPlateEn == 1)
            {
                btnPreCPH.Content = "车辆入场";
                btnUnPreCPH.Content = "车辆出场";
            }

            txbWorkTime.Text = Model.dLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
            txbUserName.Text = Model.sUserName;
            txbUserNo.Text = Model.sUserCard;
            txbSystemTime.Text = Convert.ToString(DateTime.Now.ToLocalTime());

            for (int i = 0; i < lstLblCardType.Count; i++)
            {
                lstLblCardType[i].Content = "";
            }

            for (int i = 0; i < lstTxbCarNo.Count; i++)
            {
                lstTxbCarNo[i].Text = "";
            }

            for (int i = 0; i < lstLblLaneName.Count; i++)
            {
                lstLblLaneName[i].Content = "";
            }

            //for (int i = 0; i < lstBtnManual.Count; i++)
            //{
            //    lstBtnManual[i].IsEnabled = false;
            //}

            if (Model.iVideo4 == 1)
            {
                wfh3.Visibility = Visibility.Visible;
                wfh4.Visibility = Visibility.Visible;
                ptr3.Image = Properties.Resources.Car2;
                ptr4.Image = Properties.Resources.Car2;
            }
            else
            {
                wfh3.Visibility = Visibility.Collapsed;
                wfh4.Visibility = Visibility.Collapsed;
            }

          
            lblPersonNo.Content = "";
            lblPersonName.Content = "";
            lblCardNo.Content = "";
            lblCardType.Content = "";
            lblInTime.Content = "";
            lblOutTime.Content = "";
            lblCharge.Content = "";
            lblBalance.Content = "";


            int LaneCount = Model.iChannelCount;
            if (Model.iChannelCount > 4)
            {
                LaneCount = 4;
            }
            switch (LaneCount)
            {
                case 1:
                    lstLblLaneName[0].Content = Model.Channels[0].sInOutName;
                    lstBtnCmdOpen[0].IsEnabled = true;
                    lstBtnCmdClose[0].IsEnabled = true;
                    lstBtnManual[0].IsEnabled = true;

                    lstBtnCmdOpen[1].IsEnabled = false;
                    lstBtnCmdClose[1].IsEnabled = false;
                    lstBtnManual[1].IsEnabled = false;

                    lstBtnCmdOpen[2].IsEnabled = false;
                    lstBtnCmdClose[2].IsEnabled = false;
                    lstBtnManual[2].IsEnabled = false;

                    lstBtnCmdOpen[3].IsEnabled = false;
                    lstBtnCmdClose[3].IsEnabled = false;
                    lstBtnManual[3].IsEnabled = false;
                    break;
                case 2:
                    lstLblLaneName[0].Content = Model.Channels[0].sInOutName;
                    lstLblLaneName[1].Content = Model.Channels[1].sInOutName;
                    //add
                    lstLblLaneName[2].Content = "入口图片显示";
                    lstLblLaneName[3].Content = "出口图片显示";

                    lstBtnCmdOpen[0].IsEnabled = true;
                    lstBtnCmdClose[0].IsEnabled = true;
                    lstBtnManual[0].IsEnabled = true;

                    lstBtnCmdOpen[1].IsEnabled = true;
                    lstBtnCmdClose[1].IsEnabled = true;
                    lstBtnManual[1].IsEnabled = true;

                    lstBtnCmdOpen[2].IsEnabled = false;
                    lstBtnCmdClose[2].IsEnabled = false;
                    lstBtnManual[2].IsEnabled = false;

                    lstBtnCmdOpen[3].IsEnabled = false;
                    lstBtnCmdClose[3].IsEnabled = false;
                    lstBtnManual[3].IsEnabled = false;
                    break;
                case 3:
                    lstLblLaneName[0].Content = Model.Channels[0].sInOutName;
                    lstLblLaneName[1].Content = Model.Channels[1].sInOutName;
                    lstLblLaneName[2].Content = Model.Channels[2].sInOutName;
                    lstBtnCmdOpen[0].IsEnabled = true;
                    lstBtnCmdClose[0].IsEnabled = true;
                    lstBtnManual[0].IsEnabled = true;

                    lstBtnCmdOpen[1].IsEnabled = true;
                    lstBtnCmdClose[1].IsEnabled = true;
                    lstBtnManual[1].IsEnabled = true;

                    lstBtnCmdOpen[2].IsEnabled = true;
                    lstBtnCmdClose[2].IsEnabled = true;
                    lstBtnManual[2].IsEnabled = true;

                    lstBtnCmdOpen[3].IsEnabled = false;
                    lstBtnCmdClose[3].IsEnabled = false;
                    lstBtnManual[3].IsEnabled = false;
                    break;
                case 4:
                    lstLblLaneName[0].Content = Model.Channels[0].sInOutName;
                    lstLblLaneName[1].Content = Model.Channels[1].sInOutName;
                    lstLblLaneName[2].Content = Model.Channels[2].sInOutName;
                    lstLblLaneName[3].Content = Model.Channels[3].sInOutName;
                    lstBtnCmdOpen[0].IsEnabled = true;
                    lstBtnCmdClose[0].IsEnabled = true;
                    lstBtnManual[0].IsEnabled = true;

                    lstBtnCmdOpen[1].IsEnabled = true;
                    lstBtnCmdClose[1].IsEnabled = true;
                    lstBtnManual[1].IsEnabled = true;

                    lstBtnCmdOpen[2].IsEnabled = true;
                    lstBtnCmdClose[2].IsEnabled = true;
                    lstBtnManual[2].IsEnabled = true;

                    lstBtnCmdOpen[3].IsEnabled = true;
                    lstBtnCmdClose[3].IsEnabled = true;
                    lstBtnManual[3].IsEnabled = true;
                    break;
            }

            if (Model.iShowGateState == 0)
            {
                lstBtnCmdOpen[0].IsEnabled = false;
                lstBtnCmdClose[0].IsEnabled = false;

                lstBtnCmdOpen[1].IsEnabled = false;
                lstBtnCmdClose[1].IsEnabled = false;

                lstBtnCmdOpen[2].IsEnabled = false;
                lstBtnCmdClose[2].IsEnabled = false;

                lstBtnCmdOpen[3].IsEnabled = false;
                lstBtnCmdClose[3].IsEnabled = false;
            }

            RefreshInOut();

            ShowRights();
        }

        //2016-05-11
        /// <summary>
        /// 自动更改屏机号
        /// </summary>
        private void AutoModiPingJH()
        {
            for (int i = 0; i < Model.iChannelCount; i++)
            {
                string IP = Model.Channels[i].sIP;

                IPAddress address;

                SedBll sendbll = new SedBll(IP, 1007, 1005);
                string rtnStr = "";
                if (Model.Channels[i].iXieYi == 1)//TCP发送
                {
                    if (IPAddress.TryParse(IP, out address))
                    {
                        rtnStr = sendbll.DisplayCmdX1(Convert.ToByte(Model.Channels[i].iCtrlID), 0x42, CR.GetByteArray(Convert.ToInt32(1).ToString("X2") + Convert.ToInt32(1).ToString("X2")), 1);
                    }
                    else
                    {
                        //MessageBox.Show("输入的IP不正确！", Language.LanguageXML.GetName("CR/Prompt"));
                    }
                }
                //if (rtnStr == "0")
                //{
                //    //log.Add(this.Text, "设置机号成功 原机号" + cmbNo.Text + "  新机号 " + cmbNewNo.Text);
                //    //lblString.Text = "设置机号成功！";
                //}
                //else
                //{
                //    //MessageBox.Show("与控制机通讯不通", Language.LanguageXML.GetName("CR/Prompt"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    // this.Close();
                //}
            }
        }

        #endregion


        #region 权限分配
        List<SFMControls.ButtonSfm> lstRightButton = new List<SFMControls.ButtonSfm>();

        void ShowRights()
        {
            GetUiAllRightButton(Canvas1.Children);
            if (lstRightButton.Count == 0)
            {
                return;
            }
            else
            {
                List<RightsItem> lstRI = new List<RightsItem>();

                long pid = gsd.GetIDByName("在线监控", "CmdView");

                foreach (var v in lstRightButton)
                {
                    if (!v.IsEnabled)
                    {
                        continue;
                    }

                    List<Rights> lstRs = gsd.GetRightsByName(v.FormName, v.ItemName);
                    if (lstRs.Count > 0)
                    {
                        //v.Visibility = lstRs[0].CanView == true ? Visibility.Visible : Visibility.Hidden;
                        v.IsEnabled = lstRs[0].CanOperate;
                    }
                    else
                    {
                        v.IsEnabled = false;

                        if (v.ItemName == "CmdView")
                        {
                            long ID = 0;
                            if (v.FormName == "场内车辆查询" || v.FormName == "车辆收费查询")
                            {
                                ID = gsd.GetIDByName("报表查询", "");
                            }
                            else if (v.FormName == "车牌登记" || v.FormName == "固定车期限查询")
                            {
                                ID = gsd.GetIDByName("车场管理", "");
                            }
                            lstRI.Add(new RightsItem() { FormName = v.FormName, ItemName = v.ItemName, Category = "车场", Description = v.Content.ToString(), PID = ID });
                        }
                        else
                        {
                            lstRI.Add(new RightsItem() { FormName = v.FormName, ItemName = v.ItemName, Category = "车场", Description = v.Content.ToString(), PID = pid });
                        }
                    }
                }
                if (lstRI.Count > 0)
                    gsd.SetRightsItem(lstRI);
            }
        }

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
        #endregion


        #region Threading
        #region TimerGet
        public void TimerGet()
        {
            while (true)
            {
                if (bExit == true)
                {
                    bThreadReadExitOK = true;
                    fThread.Abort();
                    return;
                };
                while (isStart)
                {
                    if (bExit == true)
                    {
                        bThreadReadExitOK = true;
                        fThread.Abort();
                        return;
                    };
                    Thread.Sleep(300);
                    ReadCard();
                }
            }
        }

        int iCountRead = 0;
        Quene.LinkQueue LQueue = new Quene.LinkQueue();
        /// <summary>
        /// 读卡记录
        /// </summary>
        public void ReadCard()
        {
            try
            {
                cardNO = "";
                iCountRead++;
                Quene.ModelNode model = LQueue.Dequeue();

                if (Model.iDetailLog == 1)// 保存详细日志
                {
                    CR.WriteToTxtFileRead(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "循环次数：" + iCountRead);
                }
                if (bPaperBill[iPaperIndex] == true) // 是否为纸票记录
                {
                    cardNO = sPaperScan;
                    modulus = iPaperIndex;
                    iIndex = iPaperIndex;
                    InOut = 1;
                }
                else if (bScondCard[bScondmodus] == true)// 进行二次解析
                {
                    if (strCardNos[bScondmodus] != "")
                    {
                        cardNO = strCardNos[bScondmodus];
                    }
                    modulus = bScondmodus;
                    iIndex = bScondmodus;
                    InOut = Model.Channels[bScondmodus].iInOut;
                    strCardNos[bScondmodus] = "";
                    cardNoNo[bScondmodus] = "";
                }
                else if (bDzBill[iDzIndex] == true)//压递感识别车牌
                {
                    cardNO = sDzScan;
                    modulus = iDzIndex;
                    iIndex = iDzIndex;
                    InOut = Model.Channels[iDzIndex].iInOut;
                }
                else if (model != null)
                {
                    if (Model.iDetailLog == 1)
                    {
                        CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "接收到队列信息----车牌号码：" + model.strCPH);
                    }
                    bool bAutoCPH = false;

                    //处理车牌识别车牌产生虚拟记录
                    bAutoCPH = DZChePaiShiBieQ(model.iDzIndex, model.strFile, model.strFileJpg, model.strCPH); // ###########3主逻辑

                    //this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    //        new Action(() =>
                    //        {
                    //            //处理车牌识别车牌产生虚礼记录
                    //            bAutoCPH = DZChePaiShiBieQ(model.iDzIndex, model.strFile, model.strFileJpg, model.strCPH);
                    //        }));
                    if (bAutoCPH)
                    {
                        strReadPicFile[model.iDzIndex] = model.strFile;
                        strReadPicFileJpg[model.iDzIndex] = model.strFileJpg;

                        cardNO = sDzScan;
                        modulus = model.iDzIndex;
                        iIndex = model.iDzIndex;
                        InOut = Model.Channels[iIndex].iInOut;
                        sDzScan = "";
                        bReadAuto = true;
                    }
                    else
                    {

                    }
                }
                else
                {
                    //带控制板并且主板必须为TCP 才和控制机有心跳包+69325
                    if (Model.bIsKZB && Model.strKZJ == "1")
                    {
                        if (ReadCardsRecord() == false)
                        {
                            //this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            //new Action(() =>
                            //{
                            //btnExit.Enabled = true;   //后面在处理
                            //}));
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                GetReadCard();
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":ReadCard", ex.Message + "\r\n" + ex.StackTrace);
            }
        }


        private int rowsCount = 0;
        /// <summary>
        /// 读卡的记录
        /// </summary>
        /// <returns></returns>
        private bool ReadCardsRecord()
        {
            try
            {
                string strTmp = "";

                bool Record = false;

                if (lstCDS.Count == 0)
                {
                    return false;
                }

                for (int y = 0; y < lstCDS.Count; y++)
                {
                    if (y != rowsCount)
                    {

                    }
                    else
                    {
                        CheDaoSet cds = lstCDS[y];
                        if (y + 1 == lstCDS.Count)
                        {
                            rowsCount = 0;
                        }
                        else
                        {
                            rowsCount = y + 1;
                        }
                        for (int i = 0; i < Model.iChannelCount; i++)
                        {
                            if (cds.CtrlNumber == Model.Channels[i].iCtrlID && rowsCount != y)
                            {
                                modulus = i;
                                break;
                            }
                            else
                            {
                                if (Model.iChannelCount == 1 && cds.CtrlNumber == Model.Channels[i].iCtrlID)
                                {
                                    modulus = 0;
                                }
                            }
                        }

                        Model.i2in1Out = 0;
                        Model.b2in1 = false;
                        carNoOut = 0;
                        if (myChecking = true && cds.CheckPortID > 0)//检测口正在检测不发读记录指令
                        {
                            cardNO = "0";
                        }

                        //TempIn
                        if (cds.OnLine == 0)
                        {
                            return false;
                        }

                        if (cds.XieYi == 1)
                        {
                            cardNO = Readsendbll.ReadRecord(cds.IP, cds.CtrlNumber, cds.XieYi);
                        }

                        if (Model.Quit_Flag == false)
                        {
                            return false;
                        }

                        if (cardNO == "2")
                        {
                            //this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            //new Action(() =>
                            //{
                            if (cds.XieYi == 1)
                            {
                                //txbOperatorInfo.Text = "控制机【" + dr["IP"].ToString() + "】忙";

                                UpdateTxbText(txbOperatorInfo, "控制机【" + cds.IP + "】忙");
                            }
                            else
                            {
                                //txbOperatorInfo.Text = "控制机【" + dr["CtrlNumber"].ToString() + "】忙";

                                UpdateTxbText(txbOperatorInfo, "控制机【" + cds.CtrlNumber + "】忙");
                            }
                            //}));
                        }

                        if (Model.iShowGateState == 1)//道闸状态
                        {
                            if (cardNO.Length == 8)
                            {
                                //this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                // new Action(() =>
                                //{
                                //    GateState(cardNO, modulus);
                                //}));
                            }
                        }
                        //入口出卡机状态   为处理
                        if (cds.HasOutCard == 1)
                        {
                            if (cardNO.Length == 8)
                            {
                                //this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                //new Action(() =>
                                //{
                                //    CkjState(cardNO, modulus);
                                //}));

                            }
                        }

                        if (cardNO.Length > 20)
                        {

                            if (cardNO.Substring(0, 1) == "E")
                            {
                                BinModel(cardNO, modulus);
                                return false;
                            }

                            bReadAuto = false;
                            if (tempCardNO == cardNO && cardNO.Substring(8, 8) != "00000000")
                            {
                                if (cardNO.Substring(0, 1) == "D")
                                {
                                    if (Model.iIdReReadHandle == 0)
                                    {
                                        if (cardNO.Length > 64)
                                        {
                                            WriteTemp1(cardNO, "10002", cds.InOutName.ToString());
                                        }
                                        cardNO = "1";
                                        return false;
                                    }
                                    else
                                    {
                                        tempCardNO = cardNO;
                                    }

                                }
                                else//IC重复记录处理
                                {
                                    cardNO = "1";
                                    return false;
                                }
                            }
                            else
                            {
                                tempCardNO = cardNO;
                            }
                            //一拖二做出口用  暂时为处理
                            if (cds.InOut == 0)
                            {
                                InOut = 0;
                            }
                            else if (cds.InOut == 1)
                            {
                                InOut = 1;
                                strTmp = cardNO.Substring(54, 2);
                                if (strTmp == "00")
                                {
                                    Model.i2in1Out = 0;
                                }
                                else
                                {
                                    if (cds.CtrlNumber != cds.OpenID)
                                    {
                                        Model.i2in1Out = 0;
                                    }
                                    else if (strTmp == "FF")
                                    {
                                        Model.i2in1Out = 1;
                                    }

                                }
                            }
                            else if (cds.InOut == 3)
                            {
                                InOut = 3;
                            }
                            else if (cds.InOut == 4)
                            {
                                InOut = 4;
                                Model.b2in1 = true;
                                strTmp = cardNO.Substring(54, 2);
                                if (strTmp == "00")
                                {
                                    InOut = 0;
                                    Model.i2in1Out = 0;
                                }
                                else
                                {
                                    InOut = 1;
                                    Model.i2in1Out = 1;
                                }
                            }
                            else
                            {
                                InOut = 2;
                            }


                            bOutTalkTemp = false;

                            if (Model.b2in1)
                            {
                                iIndex = modulus * 2 + Model.i2in1Out;
                            }
                            else
                            {
                                //如果控制机号与开闸机号不一样，则为临时卡计费器,把Modulus设为开闸机的Modulus
                                if (cds.CtrlNumber != cds.OpenID)
                                {
                                    for (int i = 0; i < Model.iChannelCount; i++)
                                    {
                                        if (cds.OpenID == Model.Channels[i].iOpenID && i != modulus)
                                        {
                                            if (cds.InOut != 4)
                                            {
                                                modulus = i;
                                                Model.TbBaoJia = true;
                                                bOutTalkTemp = true;
                                                break;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    Model.TbBaoJia = false;
                                }
                                iIndex = modulus;
                            }

                            if (iIndex > 3)
                            {
                                if (InOut == 0)
                                {
                                    iIndex = 2;
                                }
                                else
                                {
                                    iIndex = 3;
                                }
                            }
                            if (cardNO.Substring(16, 2) == "3F")
                            {
                                if (ComeGoLimit(cds.InOut, cardNO.Substring(8, 8)) == true)
                                {
                                    //this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                    //new Action(() =>
                                    //{
                                    //    txbOperatorInfo.Text = "读卡有" + Model.iInOutLimitSeconds.ToString() + "秒限制！";
                                    //}));

                                    UpdateTxbText(txbOperatorInfo, "读卡有" + Model.iInOutLimitSeconds.ToString() + "秒限制！");
                                    break;
                                }
                            }

                            Record = true;
                            break;
                        }
                        if (Model.Quit_Flag == false)
                        {
                            return false;
                        }

                    }
                }
                return Record;
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":ReadCardsRecord", ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }
        }

        DateTime ZdtStart;
        private void GetReadCard()
        {
            try
            {
                string OpenBit = "";

                if (cardNO.Length > 20)//读卡成功
                {
                    //读卡成功后，才保存原始记录
                    if (Model.bCardLog && Model.Channels[modulus].iInOut == 0)
                    {
                        //保存原始记录
                        WriteTemp(cardNO);
                    }
                    DateTime startTime = DateTime.Now;

                    //ID卡出卡机出卡软件不用发开闸指令
                    OpenBit = "1";

                    if (Model.Quit_Flag == false)
                    {
                        return;
                    }
                    //btnExit.Enabled = false;  //没有写线程

                    bPaperBill[modulus] = false;
                    bDzBill[iDzIndex] = false;
                    bScondCard[bScondmodus] = false;

                    cardTypeCmd = "";
                    tempMoney = "";

                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                        {
                                            ClearAll();
                                        }));

                    DateTime startTime1 = DateTime.Now;
                    if (Model.iDetailLog == 1)
                    {
                        CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "开始处理数据---车道：" + modulus.ToString());
                    }

                    //分解记录 判断车牌的权限
                    if (FillOutData() == false)
                    {

                        return;
                    }

                    if (Model.iDetailLog == 1)
                    {
                        CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "处理数据结束---车道：" + modulus.ToString());
                    }
                    //控制机显示屏，显示车牌号
                    if (Model.iType == 43 && Model.iXieYi == 17)  //广州南泽经常要此功能
                    {
                        //2015-11-12
                        if (carNoNo[modulus] != "" && carNoNo[modulus] != null)
                        {
                            CtrlLedShowCPH(carNoNo[modulus], modulus);
                        }
                        else if (myCarNo[modulus] != "" && myCarNo[modulus] != null)
                        {
                            CtrlLedShowCPH(myCarNo[modulus], modulus);
                        }
                    }

                    DateTime endTime = DateTime.Now;
                    TimeSpan ss = endTime - startTime1;
                    string straaa = ss.Days + "天" + ss.Hours + "小时" + ss.Minutes + "分" + ss.Seconds + "秒" + ss.Milliseconds + "毫秒";

                    //label34.Text = "绑定界面数据时间" + straaa;

                    if (Model.bPaiChe)
                    {
                        //MessageBox.Show("B");
                        if (carNoOut == 1)
                        {
                            InOut = 1;
                        }
                        else if (carNoOut == 2)
                        {
                            InOut = 0;
                        }
                    }

                    // 记录卡片类型     (2016-09-08)         
                    string tempCardTypeCmd = CR.GetCardType(monitor.CardType, 0).Substring(0, 3);

                    if (InOut == 0 && bErr == false)//入场
                    {
                        DateTime endTime1 = DateTime.Now;
                        TimeSpan ssqqq = endTime1 - ZdtStart;
                        //label37.Text = "入口开闸前：" + ssqqq.Days + "天" + ssqqq.Hours + "小时" + ssqqq.Minutes + "分" + ssqqq.Seconds + "秒" + ssqqq.Milliseconds + "毫秒";

                        DateTime startTimeOpen = DateTime.Now;

                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口处理开闸发送语音---车道：" + modulus.ToString());
                        }

                        Flag0hereopen(OpenBit);

                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口处理开闸发送语音结束---车道：" + modulus.ToString());
                        }

                        DateTime endTime2 = DateTime.Now;
                        TimeSpan ssqqq2 = endTime2 - ZdtStart;
                        //label45.Text = "入口开闸后：" + ssqqq2.Days + "天" + ssqqq2.Hours + "小时" + ssqqq2.Minutes + "分" + ssqqq2.Seconds + "秒" + ssqqq2.Milliseconds + "毫秒";

                        if (Model.iInAutoOpenModel == 2 && cardType[modulus].Substring(0, 3) == "Tmp")
                        {

                        }
                        else
                        {
                            SurplusCPH();//剩余车位显示屏
                        }

                        DateTime endTimeOpen = DateTime.Now;
                        TimeSpan ssOpen = endTimeOpen - startTimeOpen;
                        string straaaOpen = ssOpen.Days + "天" + ssOpen.Hours + "小时" + ssOpen.Minutes + "分" + ssOpen.Seconds + "秒" + ssOpen.Milliseconds + "毫秒";

                        if ((Model.iEnableVideo == 1 || Model.iEnableNetVideo == 1) && Model.iImageSave == 1)
                        {

                            if (bReadPicAuto[modulus])//识别车牌
                            {
                                filesJpg = strReadPicFileJpg[modulus];
                                inOutPic[modulus] = filesJpg;
                                picFileName = strReadPicFile[modulus];
                            }
                            else//没有识别车牌
                            {
                                if (strReadPicFile[modulus] != "")
                                {
                                    filesJpg = strReadPicFileJpg[modulus];
                                    inOutPic[modulus] = filesJpg;
                                    picFileName = strReadPicFile[modulus];
                                }
                                else
                                {
                                    ImageSavePath(modulus, 0);//生成图片路劲
                                    Mycaptureconvert(modulus, 0);//图像抓拍
                                }
                            }
                            if (filesJpg != "")
                            {
                                DateTime startTimeJpG = DateTime.Now;

                                if (Model.iCarPosLed == 0)//是否加水印 
                                {
                                    if (!System.IO.File.Exists(filesJpg))
                                    {
                                        CR.AddShuiYin(picFileName, filesJpg, Model.Channels[modulus].sInOutName, cardNoNo[modulus], modulus, carNoNo[modulus]);
                                    }
                                }

                                if (Model.iDetailLog == 1)
                                {
                                    CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口显示图片---车道：" + modulus.ToString());
                                }

                                //LoadImageGoCome();

                                if (System.IO.File.Exists(filesJpg))//显示图片到界面
                                {
                                    // NetWorkVideo2.Image.Dispose();
                                    if (Model.iVideo4 == 1)
                                    {
                                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(filesJpg), 400, 300);
                                        lstPicVideo[3].Image = bm;
                                        localImageIn = inOutPic[modulus];
                                    }
                                    else
                                    {
                                        System.Drawing.Image fileImage = System.Drawing.Image.FromFile(filesJpg);
                                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(fileImage, fileImage.Width, fileImage.Height);
                                        lstPicVideo[2].Image = bm;
                                        //NetWorkVideo2.Image = bm;
                                        localImageIn = filesJpg;

                                    }

                                    filesJpg = "";
                                    picFileName = "";
                                }
                                if (Model.iDetailLog == 1)
                                {
                                    CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口显示图片结束---车道：" + modulus.ToString());
                                }
                                DateTime endTimeJpg = DateTime.Now;
                                TimeSpan ssJpg = endTimeJpg - startTimeJpG;
                                string straaaJpg = ssJpg.Days + "天" + ssJpg.Hours + "小时" + ssJpg.Minutes + "分" + ssJpg.Seconds + "秒" + ssJpg.Milliseconds + "毫秒";
                                //label34.Texit = straaaJpg;
                            }
                            else
                            {
                                inOutPic[modulus] = "";
                            }

                        }
                        else
                        {
                            inOutPic[modulus] = "";
                            inOutPic[modulus] = "";
                        }

                        if (Model.iIDOneInOneOut == 0)//多进多出删除临时车入场记录 并保存到出场记录
                        {
                            if (cardType[modulus].Substring(0, 3) == "Tmp" && myCarNo[modulus] != null && myCarNo[modulus] != "" && myCarNo[modulus] != "66666666" && myCarNo[modulus] != "00000000" && myCarNo[modulus] != "88888888")
                            {
                                int iRst = gsd.DeleteInOutCPH(myCarNo[modulus], Model.Channels[modulus].iBigSmall, monitor.InTime);
                                if (iRst > 0)
                                {
                                    ShowLoadAlert("临时车重复入场");
                                }
                            }
                        }




                        if (Model.iAutoPlateEn == 0)
                        {
                            JjcgetWriteStore();
                        }
                        else
                        {
                            if (Model.iDetailLog == 1)
                            {
                                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口准备数据保存---车道：" + modulus.ToString());
                            }
                            if (Model.Channels[modulus].iOpenType == 7)
                            {
                                if (tempCardTypeCmd != "Tmp" && tempCardTypeCmd != "Mtp" && monitor.CardType != "ID卡记录")
                                {
                                    if (Model.iInMothOpenModel == 1 && cardType[modulus].Substring(0, 3) != "Fre" && cardType[modulus].Substring(0, 3) != "Str")
                                    {
                                        List<string> frmCPHList = new List<string>();
                                        frmCPHList.Add(modulus.ToString() == null ? "" : modulus.ToString());
                                        frmCPHList.Add(cardNoNo[modulus] == null ? "" : cardNoNo[modulus]);
                                        frmCPHList.Add(myCarNo[modulus] == null ? "" : myCarNo[modulus]);
                                        frmCPHList.Add(carNoNo[modulus] == null ? "" : "");
                                        frmCPHList.Add(monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        frmCPHList.Add(inOutPic[modulus] == null ? "" : inOutPic[modulus]);
                                        frmCPHList.Add(cardType[modulus] == null ? "" : cardType[modulus]);
                                        frmCPHList.Add(summary0.SurplusCarCount.ToString());
                                        frmCPHList.Add(monthSurplusDay.ToString());

                                        //add

                                        frmCPHList.Add(strCardCW);
                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                        {
                                            ParkingTempCPH parkFrm = new ParkingTempCPH(frmCPHList, new UpdateCPHDataHandler(UpdateCPH));
                                            parkFrm.Show();
                                        }));

                                        sender0.LoadLsNoX2010znykt(modulus, "ABD3");
                                        //SendVioce("ABD3", modulus);
                                    }
                                    else
                                        JjcgetWriteStore();

                                }
                                else if (Model.iInAutoOpenModel == 0 && (tempCardTypeCmd == "Tmp" || tempCardTypeCmd == "Mtp"))
                                {
                                    JjcgetWriteStore();
                                }
                                else if (Model.iInAutoOpenModel == 1 && (tempCardTypeCmd == "Tmp" || tempCardTypeCmd == "Mtp"))
                                {
                                    if (bReadAuto)
                                    {
                                        if (myCarNo[modulus] == "")
                                        {
                                            myCarNo[modulus] = carNoNo[modulus];
                                            // dal.AddOptLog("入场弹出确认车牌", "入场弹出确认车牌.车牌号为空！");
                                        }

                                        List<string> frmCPHList = new List<string>();
                                        frmCPHList.Add(modulus.ToString() == null ? "" : modulus.ToString());
                                        frmCPHList.Add(cardNoNo[modulus] == null ? "" : cardNoNo[modulus]);
                                        frmCPHList.Add(myCarNo[modulus] == null ? "" : myCarNo[modulus]);
                                        frmCPHList.Add(carNoNo[modulus] == null ? "" : "");
                                        frmCPHList.Add(monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        frmCPHList.Add(inOutPic[modulus] == null ? "" : inOutPic[modulus]);
                                        frmCPHList.Add(cardType[modulus] == null ? "" : cardType[modulus]);
                                        frmCPHList.Add(summary0.SurplusCarCount.ToString());
                                        frmCPHList.Add(monthSurplusDay.ToString());

                                        //add

                                        frmCPHList.Add(strCardCW);
                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                        {
                                            ParkingTempCPH parkFrm = new ParkingTempCPH(frmCPHList, new UpdateCPHDataHandler(UpdateCPH));
                                            parkFrm.Show();
                                        }));
                                        sender0.LoadLsNoX2010znykt(modulus, "ADD3");
                                        //SendVioce("ADD3", modulus);
                                    }
                                    else
                                        JjcgetWriteStore();
                                }
                                else
                                {
                                    if (!bReadAuto)
                                    {
                                        JjcgetWriteStore();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                            if (Model.iDetailLog == 1)
                            {
                                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口保存数据结束---车道：" + modulus.ToString());
                            }
                        }
                    }
                    else if (InOut == 1 && bErr == false)//出场
                    {
                        //iYHMode = 0;
                        //iYHJH = 0;
                        //sYHAdr = "";
                        //sYHType = "";
                        //sYHValue = 0;

                        iCouponMode = 0;  //车牌打折
                        sCouponAddr = "";
                        sCouponMode = "";
                        sCouponValue = 0;
                        iCouponNo = 0;

                        if (iCouponMode == 0)  //2016-06-17 没有其它优惠模式  打折优先级：车牌打折<机号打折<免费车辆
                        {
                            if (Model.iAutoCPHDZ == 1)  //车牌打折，查询打折方式与地址
                            {
                                if (monitor.CarNo.Length >= 7)
                                // if (lblCarNo.Content.ToString().Length >= 7)
                                {
                                    //!!!
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    dic["CPH"] = "%" + monitor.CarNo.Substring(1, 6);
                                    //dic["CPH"] = "%" + lblCarNo.Content.ToString().Substring(1, 6);
                                    dic["Status"] = 0;
                                    List<ParkCPHDiscountSet> dtDZs = gsd.GetAutoCPHDZ(dic);
                                    if (dtDZs.Count > 0)
                                    {
                                        iCouponMode = 1;  //车牌打折
                                        sCouponAddr = dtDZs[0].Address;
                                        sCouponMode = dtDZs[0].Manner;
                                        sCouponValue = dtDZs[0].Favorable ?? 0;
                                    }
                                }
                            }
                        }

                        if (bErr == false)
                        {
                            DateTime endTime1 = DateTime.Now;
                            TimeSpan ssqqq = endTime1 - ZdtStart;
                            //label37.Text = "出口开闸前：" + ssqqq.Days + "天" + ssqqq.Hours + "小时" + ssqqq.Minutes + "分" + ssqqq.Seconds + "秒" + ssqqq.Milliseconds + "毫秒";
                            if (Model.iDetailLog == 1)
                            {
                                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口处理开闸---车道：" + modulus.ToString());
                            }

                            Flag1hereopen();

                            if (Model.iDetailLog == 1)
                            {
                                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口处理开闸完成---车道：" + modulus.ToString());
                            }

                            DateTime endTime2 = DateTime.Now;
                            TimeSpan ssqqq2 = endTime2 - ZdtStart;
                            //label45.Text = "出口开闸后：" + ssqqq2.Days + "天" + ssqqq2.Hours + "小时" + ssqqq2.Minutes + "分" + ssqqq2.Seconds + "秒" + ssqqq2.Milliseconds + "毫秒";
                            if (Model.iOutAutoOpenModel == 2 && cardType[modulus].Substring(0, 3) == "Tmp")
                            {

                            }
                            else
                            {
                                SurplusCPH();
                            }
                        }
                        bool bZPhoto = false;
                        //出口首先处理图片路劲
                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口处理图片路径---车道：" + modulus.ToString());
                        }
                        if ((Model.iEnableVideo == 1 || Model.iEnableNetVideo == 1) && Model.iImageSave == 1)
                        {
                            if (bReadPicAuto[modulus])
                            {
                                bZPhoto = false;
                                filesJpg = strReadPicFileJpg[modulus];
                                //inPic[modulus] = filesJpg;
                                inOutPic[modulus] = filesJpg;
                                picFileName = strReadPicFile[modulus];
                            }
                            else
                            {
                                filesJpg = "";
                                picFileName = "";
                                if (strReadPicFile[modulus] != "")
                                {
                                    bZPhoto = false;
                                    filesJpg = strReadPicFileJpg[modulus];
                                    //inPic[modulus] = filesJpg;
                                    inOutPic[modulus] = filesJpg;
                                    picFileName = strReadPicFile[modulus];
                                }
                                else
                                {
                                    ImageSavePath(modulus, 0);//生成图片路劲
                                    bZPhoto = true;
                                    inOutPic[modulus] = filesJpg;
                                }

                            }
                        }
                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口处理图片路径完成---车道：" + modulus.ToString());
                        }
                        if (Model.iAutoPlateEn == 0)
                        {
                            LoadCarGoOut();  //2015-10-21
                            if (recordCount == 0)
                            {

                                if ((tempCardTypeCmd == "Tmp" || tempCardTypeCmd == "Mtp" && Convert.ToDecimal(monitor.Charge.ToString() == "" ? "0" : monitor.Charge.ToString()) > 0) && (Model.Channels[modulus].iOpenType == 0 || Model.Channels[modulus].iOpenType == 1))
                                //if ((CR.GetCardType(lblCardType.Content.ToString(), 0).Substring(0, 3) == "Tmp" || CR.GetCardType(lblCardType.Content.ToString(), 0).Substring(0, 3) == "Mtp" && Convert.ToDecimal(lblCharge.Content.ToString() == "" ? "0" : lblCharge.Content.ToString()) > 0) && (Model.Channels[modulus].iOpenType == 0 || Model.Channels[modulus].iOpenType == 1))
                                {
                                    TempCarGo();   //弹出收费窗口未处理
                                }
                                else
                                {
                                    JJcUpdateStore();
                                }
                            }
                            Model.FilesJpgTemp[modulus] = "";
                        }
                        else
                        {
                            if (Model.iDetailLog == 1)
                            {
                                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口处理记录或者弹出窗口---车道：" + modulus.ToString());
                            }
                            if ((tempCardTypeCmd == "Tmp" || tempCardTypeCmd == "Mtp") && (Model.Channels[modulus].iOpenType == 0 || Model.Channels[modulus].iOpenType == 1 || Model.Channels[modulus].iOpenType == 3 || Model.Channels[modulus].iOpenType == 7 || Model.Channels[modulus].iOpenType == 8))
                            {
                                if (Model.iOutAutoOpenModel == 0)
                                {
                                    JJcUpdateStore();
                                }
                                else if (Model.iOutAutoOpenModel == 1)
                                {
                                    if (Model.iAutoKZ == 1 && Model.iAutoCZJL && monitor.Charge == 0)
                                    {
                                        JJcUpdateStore();
                                    }
                                    else
                                    {
                                        if (myCarNo[modulus] == "")
                                        {
                                            myCarNo[modulus] = carNoNo[modulus];
                                        }
                                        DateTime endTime1 = DateTime.Now;
                                        TimeSpan ssqqq = endTime1 - ZdtStart;
                                        //label52.Text = "出口弹出收费前：" + ssqqq.Days + "天" + ssqqq.Hours + "小时" + ssqqq.Minutes + "分" + ssqqq.Seconds + "秒" + ssqqq.Milliseconds + "毫秒";

                                        TempCarGo_AUTO(modulus);

                                        DateTime endTime2 = DateTime.Now;
                                        TimeSpan ssqqq2 = endTime2 - ZdtStart;
                                        //label53.Text = "出口弹出收费后：" + ssqqq2.Days + "天" + ssqqq2.Hours + "小时" + ssqqq2.Minutes + "分" + ssqqq2.Seconds + "秒" + ssqqq2.Milliseconds + "毫秒";
                                    }
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                if (monitor.CardType.ToString() != "ID卡记录")
                                //if (lblCardType.Content.ToString() != "ID卡记录")
                                {
                                    if (Model.iOutMothOpenModel == 1 && cardType[modulus].Substring(0, 3) == "Mth")
                                    {
                                        if (myCarNo[modulus] == "")
                                        {
                                            myCarNo[modulus] = carNoNo[modulus];
                                        }
                                        List<string> frmCPHList = new List<string>();
                                        frmCPHList.Add(modulus.ToString() == null ? "" : modulus.ToString());
                                        frmCPHList.Add(cardNoNo[modulus] == null ? "" : cardNoNo[modulus]);
                                        frmCPHList.Add(myCarNo[modulus] == null ? "" : myCarNo[modulus]);
                                        frmCPHList.Add(carNoNo[modulus] == null ? "" : carNoNo[modulus]);
                                        frmCPHList.Add(monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        frmCPHList.Add(inOutPic[modulus] == null ? "" : inOutPic[modulus]);
                                        frmCPHList.Add(cardType[modulus] == null ? "" : cardType[modulus]);
                                        //add
                                        frmCPHList.Add(monthSurplusDay.ToString());
                                        frmCPHList.Add(summary0.SurplusCarCount.ToString());
                                        frmCPHList.Add(lOutID[modulus] == 0 ? "0" : lOutID[modulus].ToString());
                                        //frmCPHList.Add(strCardCW);

                                        JJcUpdateStore();

                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                        {
                                            ParkingMthCPH frmMth = new ParkingMthCPH(frmCPHList);
                                            frmMth.Show();
                                        }));

                                        sender0.LoadLsNoX2010znykt(modulus, "ABD3");
                                        //SendVioce("ABD3", modulus);//发送组合语音
                                    }
                                    else
                                    {
                                        JJcUpdateStore();
                                    }
                                }
                                else
                                {
                                    return;
                                }

                            }

                            if (Model.iDetailLog == 1)
                            {
                                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口处理记录或者弹出窗口完成---车道：" + modulus.ToString());
                            }
                        }
                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口显示图片---车道：" + modulus.ToString());
                        }
                        //图片处理为处理完流程以后
                        if ((Model.iEnableVideo == 1 || Model.iEnableNetVideo == 1) && Model.iImageSave == 1)
                        {
                            if (bZPhoto)
                            {
                                Mycaptureconvert(modulus, 0);//图像抓拍
                            }
                            if (filesJpg != "")
                            {
                                if (Model.iCarPosLed == 0)
                                {
                                    if (!System.IO.File.Exists(filesJpg))
                                    {
                                        if (Model.Channels[modulus].iOpenType == 8)//读卡加识别
                                        {
                                            CR.AddShuiYin(picFileName, filesJpg, Model.Channels[modulus].sInOutName, cardNoNo[modulus], modulus, carNoNo[modulus], myCarNo[modulus]);
                                        }
                                        else
                                        {
                                            CR.AddShuiYin(picFileName, filesJpg, Model.Channels[modulus].sInOutName, cardNoNo[modulus], modulus, carNoNo[modulus]);
                                        }
                                    }
                                }
                                else
                                {
                                    //Thread.Sleep(50);
                                }

                                filesJpg = "";
                                picFileName = "";
                            }
                            else
                            {
                                inOutPic[modulus] = "";
                            }

                            if (bCzyKZ)
                            {
                            }
                            else
                            {
                                LoadImageGoCome();
                            }
                        }
                        else
                        {
                            inOutPic[modulus] = "";
                            inOutPicR[modulus] = "";
                        }
                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "出口显示图片完成---车道：" + modulus.ToString());
                        }
                    }
                    //储值卡更新余额
                    if (tempCardTypeCmd == "Str" && Model.Channels[modulus].iBigSmall == 0)//储值卡修改发行表余额
                    {
                        if (cardTypeCmd == "3F")
                        {
                            //gsd.UpdateBalance(lblCardNo.Content.ToString(), Convert.ToDecimal(lblBalance.Content.ToString()));
                            gsd.UpdateBalance(monitor.CardNo, monitor.Balance);
                        }
                        else
                        {
                            if (Convert.ToInt32(cardNO.Substring(38, 4), 16) < 10000)
                            {
                                if (Model.iXsd == 0)
                                {
                                    if (Model.iChargeType == 3)
                                    {
                                        if (Model.iXsdNum == 1)
                                        {
                                            //gsd.UpdateBalance(lblCardNo.Content.ToString(), Convert.ToDecimal(Convert.ToDecimal(cardNO.Substring(38, 4)) / 10));
                                            gsd.UpdateBalance(monitor.CardNo, Convert.ToDecimal(Convert.ToDecimal(cardNO.Substring(38, 4)) / 10));
                                        }
                                        else
                                        {
                                            //gsd.UpdateBalance(lblCardNo.Content.ToString(), Convert.ToDecimal(Convert.ToDecimal(cardNO.Substring(38, 4)) / 100));
                                            gsd.UpdateBalance(monitor.CardNo, Convert.ToDecimal(Convert.ToDecimal(cardNO.Substring(38, 4)) / 100));
                                        }
                                    }
                                    else
                                    {
                                        //gsd.UpdateBalance(lblCardNo.Content.ToString(), Convert.ToInt32(cardNO.Substring(38, 4)));
                                        gsd.UpdateBalance(monitor.CardNo, Convert.ToInt32(cardNO.Substring(38, 4)));
                                    }
                                }
                                else
                                {
                                    gsd.UpdateBalance(monitor.CardNo, Convert.ToDecimal(Convert.ToDecimal(cardNO.Substring(38, 4)) / 10));
                                    //gsd.UpdateBalance(lblCardNo.Content.ToString(), Convert.ToDecimal(Convert.ToDecimal(cardNO.Substring(38, 4)) / 10));
                                }
                            }
                        }
                    }
                }
                DateTime endTimeAll = DateTime.Now;
                TimeSpan ssqqqAll = endTimeAll - ZdtStart;

                UpdateTxbText(txbOperatorInfo, ssqqqAll.Days + "天" + ssqqqAll.Hours + "小时" + ssqqqAll.Minutes + "分" + ssqqqAll.Seconds + "秒" + ssqqqAll.Milliseconds + "毫秒");
                //txbOperatorInfo.Text = ssqqqAll.Days + "天" + ssqqqAll.Hours + "小时" + ssqqqAll.Minutes + "分" + ssqqqAll.Seconds + "秒" + ssqqqAll.Milliseconds + "毫秒";

                bOffLine[modulus] = false;
                autoCarNo[modulus] = "";
                myCarNo[modulus] = "";
                bLoadCar = true;
                strReadPicFileJpg[modulus] = "";
                strReadPicFile[modulus] = "";
                carNoNo[modulus] = "";
                loadCar();


                //2016-09-08   从内存更新控件
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    lblPersonNo.Content = monitor.PersonNo;
                    lblPersonName.Content = monitor.PersonName;
                    lblDeptName.Content = monitor.DeptName;
                    lblCardNo.Content = monitor.CardNo;
                    lblCardType.Content = monitor.CardType;
                    lblInTime.Content = monitor.InTime == DateTime.MinValue ? "" : monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss");
                    lblOutTime.Content = monitor.OutTime == DateTime.MinValue ? "" : monitor.OutTime.ToString("yyyy-MM-dd HH:mm:ss");
                    lblCharge.Content = monitor.Charge.ToString("0.00");

                    if (CR.GetCardType(monitor.CardType, 0).Substring(0, 3) == "Mth" || CR.GetCardType(monitor.CardType, 0).Substring(0, 3) == "Fre")
                    {
                        lblUnit.Visibility = Visibility.Hidden;
                        lblBalance.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        lblBalance.Visibility = Visibility.Visible;
                        lblUnit.Visibility = Visibility.Visible;
                        lblValid.Content = "剩余金额:";
                        lblBalance.Content = monitor.Balance.ToString("0.00");
                    }


                    txbCharge.Text = monitor.Charge.ToString("0.00");
                    //txbSurplusCarCount.Text = summary.SurplusCarCount.ToString();

                    //lblMthCount.Content = monitor.MthCount;
                    //lblTmpCount.Content = monitor.TmpCount;
                    //lblStrCount.Content = monitor.StrCount;
                    //lblFreCount.Content = monitor.FreCount;
                    //lblFreMoney.Content = monitor.FreMoney;
                    //lblMoneyCount.Content = monitor.MoneyCount;
                    //lblOpenCount.Content = monitor.OpenCount;
                    //lblOutCount.Content = monitor.OpenCount;
                }));
                //btnExit.Enabled = true;
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":GetReadCard", ex.Message + "\r\n" + ex.StackTrace);
                txbOperatorInfo.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        private bool FillOutData()
        {
            try
            {
                //isCarIn = false;
                bool FillOut = false;
                string ssInDateTime = "";
                string ssOutDateTime = "";
                List<CardIssue> lstMonth = new List<CardIssue>();
                List<CarIn> dtInCome = new List<CarIn>();
                monthSurplusDayCmd = "";
                //lblBalance.Visibility = Visibility.Visible;
                bMonthPastdue = false;

                if (Model.Quit_Flag == false)
                {
                    return false;
                }
                bErr = false;
                strErrType = "";
                monthOut = false;
                FillOut = true;
                bDateTimeErr[0] = false;
                bDateTimeErr[1] = false;
                bPaperRecord = false;
                cardTypeCmd = cardNO.Substring(16, 2);
                if (cardNO.Substring(0, 1) == "C") //处理IC卡卡号
                {
                    if (cardNO.Substring(8, 4) == "0000")
                    {
                        // ClassCardsOn(cardNO.Substring(12, 4));
                        cardNoNo[modulus] = Convert.ToInt32(cardNO.Substring(12, 4), 16).ToString("00000");
                        myIDICFlag[modulus] = 15;
                    }
                    else
                    {
                        cardNoNo[modulus] = cardNO.Substring(8, 8);
                        bPaperRecord = true;
                    }
                }
                else if (cardNO.Substring(0, 1) == "E") //处理脱机卡号
                {
                    carNoNo[modulus] = cardNO.Substring(8, 8);
                }
                else
                {
                    cardNoNo[modulus] = cardNO.Substring(8, 8); //处理ID卡卡号
                    myIDICFlag[modulus] = 16;
                    if (cardNO.Substring(0, 1) != "D")
                    {
                        if (cardTypeCmd != "AA")
                        {
                            //txbOperatorInfo.Text = "非法记录1:" + cardNoNo[modulus];
                            UpdateTxbText(txbOperatorInfo, "非法记录1:" + cardNoNo[modulus]);
                            return false;
                        }
                    }
                }

                if (cardTypeCmd == "EE" || cardTypeCmd == "FF")
                {
                    cardNoNo[modulus] = Convert.ToInt32(cardNO.Substring(12, 4), 16).ToString("00000");
                }

                monitor.CardNo = cardNoNo[modulus];
                //lblCardNo.Content = cardNoNo[modulus];
                cardType[modulus] = CR.GetCardType(cardTypeCmd);

                bool bMonth = false;
                bool bInCome = false;

                if (bIsMoth[modulus]) //识别车是否为月卡，或者免费卡，或者储值卡
                {
                    lstMonth = gsd.GetReadFaxing(cardNoNo[modulus]);
                    if (lstMonth.Count > 0)
                    {
                        bMonth = true;
                        cardType[modulus] = lstMonth[0].CarCardType;

                        monitor.PersonNo = lstMonth[0].UserNO;
                        monitor.PersonName = lstMonth[0].UserName;
                        monitor.DeptName = lstMonth[0].DeptName;

                        UpdateUiText(lblPersonName, lstMonth[0].UserName);
                        UpdateUiText(lblPersonNo, lstMonth[0].UserNO);
                        UpdateUiText(lblDeptName, lstMonth[0].DeptName);
                        //lblPersonNo.Content = lstMonth[0].UserNO;
                        //lblPersonName.Content = lstMonth[0].UserName;
                        //lblDeptName.Content = lstMonth[0].DeptName;
                    }
                    else
                    {
                        if (Model.bCarYellowTmp && iAutoColor[modulus] == 2)
                        {
                            cardType[modulus] = Model.strCarYellowTmpType;
                        }
                        else
                            cardType[modulus] = "TmpA";
                    }
                }
                else
                {
                    if (cardType[modulus] != "Person")
                    {
                        if (Model.bCarYellowTmp && iAutoColor[modulus] == 2)
                        {
                            cardType[modulus] = Model.strCarYellowTmpType;
                        }
                        else
                            cardType[modulus] = "TmpA";
                    }
                }

                if (Model.iIDOneInOneOut == 1 || InOut > 0)
                {
                    dtInCome = gsd.GetMyRsX(cardNoNo[modulus], cardType[modulus], Model.iParkingNo, Model.Channels[modulus].iBigSmall);


                    //if (Model.iIDOneInOneOut == 1 && dtInCome.Count == 0)
                    //{
                    //    Request req = new Request();
                    //    List<CarIn> LstCarIn = req.GetCarInByCarPlateNumberLike(myCarNo[modulus], 4);
                    //    if (LstCarIn.Count > 0)
                    //        isCarIn = true;
                    //}
                   
                    if (dtInCome.Count > 0)
                    {
                        cardType[modulus] = dtInCome[0].CardType;
                        inPic[0] = dtInCome[0].InPic;
                        inPic[1] = dtInCome[0].InUser;
                        bInCome = true;
                    }
                    else
                    {
                        inPic[0] = "";
                        inPic[1] = "";
                    }
                }
                else
                {
                    inPic[0] = "";
                    inPic[1] = "";
                }

                monitor.CardType = CR.GetCardType(cardType[modulus], 1);
                monitor.CarNo = myCarNo[modulus];
                //lblCardType.Content = CR.GetCardType(cardType[modulus], 1);
                //lblCarNo.Content = myCarNo[modulus];
                UpdateUiText(lblCardType, monitor.CardType);
                UpdateUiText(lblCardNo, monitor.CardNo);

                UpdateTxbText(lstTxbCarNo[iIndex], myCarNo[modulus]);
                //lstTxbCarNo[iIndex].Text = lblCarNo.Content.ToString();

                carNoNo[modulus] = monitor.CarNo;
                //carNoNo[modulus] = lblCarNo.Content.ToString();
                //lstLblCardNo[modulus].Content = lblCarNo.Content.ToString();
                //lstLblCardType[modulus].Content = lblCardType.Content;

                if (bMonth)
                {
                    StringBuilder strBJiHao = new StringBuilder();
                    foreach (char c in lstMonth[0].CarValidMachine)
                    {
                        //strBJiHao.Append(CR.ConvertToBin(c));
                        strBJiHao.Append(c);
                    }
                    if (strBJiHao.ToString().Substring(Model.Channels[modulus].iCtrlID - 1, 1) == "1")
                    {
                        UpdateTxbText(txbOperatorInfo, Model.Channels[modulus].iCtrlID.ToString() + "号机上无权限!");
                        ShowLoadAlert("在 " + Model.Channels[modulus].iCtrlID.ToString() + " 号机上无权限！");
                        //txbOperatorInfo.Text = "在" + Model.Channels[modulus].iCtrlID.ToString() + "号机上无权限!";
                        sender0.VoiceDisplay(VoiceType.CarInvalid, Model.Channels[modulus].iCtrlID);
                        return false;
                    }

                    DateTime dtStart = Convert.ToDateTime(lstMonth[0].CarValidStartDate.ToString("yyyy-MM-dd 00:00:00"));
                    DateTime dtEnd = Convert.ToDateTime(lstMonth[0].CarValidEndDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));

                    if (CR.DateDiff(CR.DateInterval.Minute, dtStart, DateTime.Now) < 0 || CR.DateDiff(CR.DateInterval.Minute, DateTime.Now, dtEnd) < 0 && cardType[modulus].Substring(0, 3) == "Mth")
                    {
                        if (Model.iYKOverTimeCharge == 0)
                        {
                            UpdateTxbText(txbOperatorInfo, "已过期，请到管理处延期!");

                            ShowLoadAlert("已过期，请到管理处延期!");
                            //txbOperatorInfo.Text = "已过期，请到管理处延期!";
                            sender0.VoiceDisplay(VoiceType.BeOverdue, modulus);
                            if (bReadPicAuto[modulus])
                            {
                                UpdateTxbText(lstTxbCarNo[iIndex], myCarNo[modulus]);
                                //lstTxbCarNo[modulus].Text = myCarNo[modulus];
                                CR.AddShuiYin(strReadPicFile[modulus], strReadPicFileJpg[modulus], Model.Channels[modulus].sInOutName, cardNoNo[modulus], modulus, myCarNo[modulus], "过期车牌");
                                bReadPicAuto[modulus] = false;

                                if (Model.Channels[modulus].iInOut == 0)
                                {
                                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(strReadPicFileJpg[modulus]), lstPicVideo[2].Width, lstPicVideo[2].Height);

                                    UpdatePicImage(lstPicVideo[2], bm);
                                    //lstPicVideo[2].Image = bm;
                                    localImageIn = strReadPicFileJpg[modulus];
                                }
                                else
                                {
                                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(strReadPicFileJpg[modulus]), lstPicVideo[3].Width, lstPicVideo[3].Height);
                                    UpdatePicImage(lstPicVideo[3], bm);
                                    //lstPicVideo[3].Image = bm;
                                    localImageOut = strReadPicFileJpg[modulus];
                                }
                            }
                            return false;
                        }

                        else if (Model.iYKOverTimeCharge == 2)
                        {
                            //ID卡月卡过期处理
                            if (CR.DateDiff(CR.DateInterval.Minute, dtStart, DateTime.Now) < 0 || CR.DateDiff(CR.DateInterval.Minute, DateTime.Now, dtEnd.AddDays(Model.iMothOverDay)) < 0 & cardType[modulus].Substring(0, 3) == "Mth")
                            {
                                UpdateTxbText(txbOperatorInfo, "已过期，请到管理处延期!");
                                ShowLoadAlert("已过期，请到管理处延期!");
                                //txbSystemTime.Text = "已过期，请到管理处延期";
                                sender0.VoiceDisplay(VoiceType.BeOverdue, modulus);
                                gsd.AddLog("在线监控:FillOutData", "已过期，请联系管理处" + cardNO);
                                if (bReadPicAuto[modulus])
                                {
                                    UpdateTxbText(lstTxbCarNo[modulus], myCarNo[modulus]);
                                    //lstTxbCarNo[modulus].Text = myCarNo[modulus];
                                    CR.AddShuiYin(strReadPicFile[modulus], strReadPicFileJpg[modulus], Model.Channels[modulus].sInOutName, cardNoNo[modulus], modulus, myCarNo[modulus], "过期车牌");
                                    bReadPicAuto[modulus] = false;
                                    if (Model.Channels[modulus].iInOut == 0)
                                    {
                                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(strReadPicFileJpg[modulus]), lstPicVideo[2].Width, lstPicVideo[2].Height);
                                        UpdatePicImage(lstPicVideo[2], bm);
                                        //lstPicVideo[2].Image = bm;
                                        localImageIn = strReadPicFileJpg[modulus];
                                    }
                                    else
                                    {
                                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(strReadPicFileJpg[modulus]), lstPicVideo[3].Width, lstPicVideo[3].Height);
                                        UpdatePicImage(lstPicVideo[3], bm);
                                        //lstPicVideo[3].Image = bm;
                                        localImageOut = strReadPicFileJpg[modulus];
                                    }
                                    return false;
                                }
                            }
                            else
                            {
                                long lDay = CR.DateDiff(CR.DateInterval.Day, dtEnd, DateTime.Now) + 1;
                                ShowLoadAlert("已过期" + lDay.ToString() + "天，请提醒用户延期");
                                bMonthPastdue = true;
                                sender0.VoiceDisplay(VoiceType.BeOverdue, modulus);
                            }
                        }
                        else
                        {
                            if (Model.Channels[modulus].iInOut == 0)
                            {
                                ShowLoadAlert("已过期,临时车,请通行");
                            }
                            sender0.LoadLsNoX2010znykt(modulus, "6CADAC");
                            bMonthPastdue = true;
                        }
                    }
                    //ID免费卡过期处理
                    else if (CR.DateDiff(CR.DateInterval.Minute, dtStart, DateTime.Now) < 0 || CR.DateDiff(CR.DateInterval.Minute, DateTime.Now, dtEnd) < 0 && cardType[modulus].Substring(0, 3) == "Fre")
                    {
                        UpdateTxbText(txbOperatorInfo, "已过期，请到管理处延期!");
                        ShowLoadAlert("已过期,请到管理处延期！");
                        //txbOperatorInfo.Text = "已过期,请到管理处延期！";
                        //ShowLoadAlert("已过期,请到管理处延期！");
                        sender0.VoiceDisplay(VoiceType.BeOverdue, modulus);
                        return false;
                    }
                    else
                    {
                        bMonthPastdue = false;
                        if (cardType[modulus] == "Cnt" && Model.bPaiChe)
                        {
                            //次卡过期处理
                            if (dtStart > DateTime.Now || dtEnd < DateTime.Now && Model.bLENOPaiChe == false)
                            {
                                UpdateTxbText(txbOperatorInfo, "已过期，请到管理处延期!");
                                // txbOperatorInfo.Text = "已过期,请到管理处延期！";
                                ShowLoadAlert("已过期,请到管理处延期！");
                                sender0.VoiceDisplay(VoiceType.BeOverdue, modulus);
                                return false;
                            }
                            else
                            {

                            }
                        }
                        else if (cardType[modulus] == "Opt")
                        {
                            bCzyKZ = false;
                            if (Model.Channels[modulus].iInOut == 0)
                            {
                                bCzyKZ = true;
                                InOut = 1;
                            }
                            else if (Model.Channels[modulus].iInOut == 1)
                            {
                                bCzyKZ = true;
                            }
                            else if (Model.Channels[modulus].iInOut == 4)
                            {
                                if (Model.i2in1Out == 0)
                                {
                                    bCzyKZ = true;
                                    InOut = 1;
                                }
                                else
                                {
                                    bCzyKZ = true;
                                }
                            }
                        }
                        //---2016-09-05
                        monthSurplusDay = CR.DateDiff(CR.DateInterval.Day, DateTime.Now, dtEnd);
                        if (monthSurplusDay > 9999)
                        {
                            monthSurplusDay = 9999;
                        }
                        if (cardType[modulus].Substring(0, 3) == "Mth" || cardType[modulus].Substring(0, 3) == "Fre")
                        {
                            UpdateUiVisibility(lblBalance, Visibility.Hidden);
                            UpdateUiVisibility(lblUnit, Visibility.Hidden);
                            UpdateUiText(lblValid, "有效期至:  " + dtEnd.ToShortDateString() + "  可用" + monthSurplusDay + "天");

                            //lblValid.Content = "有效期至:  " + dtEnd.ToShortDateString() + "  可用" + monthSurplusDay + "天";
                            //lblBalance.Visibility = Visibility.Hidden;
                            //lblUnit.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            //UpdateUiVisibility(lblBalance, Visibility.Visible);
                            //UpdateUiVisibility(lblUnit, Visibility.Visible);

                            //lblBalance.Visibility = Visibility.Visible;
                            //lblUnit.Visibility = Visibility.Visible;
                        }
                        monthSurplusDayCmd = monthSurplusDay.ToString("0000");
                    }
                    if (Model.iIdReReadHandle == 1)
                    {
                        bNoSound = IdCardMemory(cardNoNo[modulus]);
                    }

                }
                inOutName[modulus] = Model.Channels[modulus].sInOutName; //出入口名称

                //入场判断是否一进一出  并且处理入场时间  是否控制满位
                if (Model.Channels[modulus].iInOut == 0)
                {
                    //临时车禁止驶入小车场
                    if ((cardTypeCmd == "3F" || cardTypeCmd == "8F") && Model.iTempCanNotInSmall == 1 && InOut == 0 && Model.Channels[modulus].iBigSmall == 1 && cardType[modulus].Substring(0, 3) == "Tmp")
                    {
                        UpdateTxbText(txbOperatorInfo, "临时车禁止驶入小车场!");
                        ShowLoadAlert("临时车禁止驶入小车场！");
                        //txbOperatorInfo.Text = "临时车禁止驶入小车场!";
                        //!!!
                        sender0.LoadLsNoX2010znykt(modulus, "ADD2");
                        gsd.AddLog("在线监控:FillOutData", "临时车禁止驶入小车场" + cardNO);
                        return false;
                    }

                    if (Model.iIDOneInOneOut == 1)
                    {
                        inOutCtrl = false;
                        if (cardType[modulus] == "Opt")
                        {
                            inOutCtrl = false;
                        }
                        else if (cardType[modulus].Substring(0, 3) == "Tmp")
                        {
                            inOutCtrl = true;
                        }
                        if (Model.sID1In1OutCardType.Contains(cardType[modulus]))
                        {
                            inOutCtrl = true;
                        }
                        if (inOutCtrl)
                        {
                            if (bInCome)
                            {
                                UpdateTxbText(txbOperatorInfo, "此车已入场[" + Model.Channels[modulus].sInOutName + "]");

                                //txbOperatorInfo.Text = "此车已入场[" + Model.Channels[modulus].sInOutName + "]";
                                sender0.VoiceDisplay(VoiceType.AlreadyEntered, modulus);
                                return false;
                            }
                        }
                    }

                    //满位 禁止读卡
                    if (Model.iFullLight > 0)
                    {
                        if (summary0.SurplusCarCount < 1)
                        //if (Convert.ToInt32(txbSurplusCarCount.Text) < 1)
                        {
                            //免费车不计入车位个数
                            if (Model.iFreeCardNoInPlace == 1 && cardType[modulus].Substring(0, 3) == "Fre")
                            {

                            }
                            else
                            {
                                if (Model.iFullLight == 5)
                                {
                                    UpdateTxbText(txbOperatorInfo, "车位已满!");

                                    if (Model.bFullComfirmOpen)
                                    {
                                        List<string> frmCPHList = new List<string>();
                                        frmCPHList.Add(modulus.ToString() == null ? "" : modulus.ToString());
                                        frmCPHList.Add(cardNoNo[modulus] == null ? "" : cardNoNo[modulus]);
                                        frmCPHList.Add(myCarNo[modulus] == null ? "" : myCarNo[modulus]);
                                        frmCPHList.Add(carNoNo[modulus] == null ? "" : carNoNo[modulus]);
                                        frmCPHList.Add(monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        frmCPHList.Add(inOutPic[modulus] == null ? "" : inOutPic[modulus]);
                                        frmCPHList.Add(cardType[modulus] == null ? "" : cardType[modulus]);
                                        frmCPHList.Add(summary0.SurplusCarCount.ToString());
                                        frmCPHList.Add(monthSurplusDay.ToString());

                                        //add

                                        frmCPHList.Add(strCardCW);
                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                       {
                                           ParkingTempCPH parkFrm = new ParkingTempCPH(frmCPHList, new UpdateCPHDataHandler(UpdateCPH), "车场满位，确认开闸");
                                           parkFrm.Show();
                                       }));
                                    }


                                    ShowLoadAlert("车位已满！");
                                    //txbOperatorInfo.Text = "车位已满!";
                                    sender0.VoiceDisplay(VoiceType.ParkingFull, modulus);
                                    return false;
                                }
                                else if (cardType[modulus].Substring(0, 3) == "Str" && Model.iFullLight == 3)
                                {
                                    UpdateTxbText(txbOperatorInfo, "储值车位已满!");

                                    if (Model.bFullComfirmOpen)
                                    {
                                        List<string> frmCPHList = new List<string>();
                                        frmCPHList.Add(modulus.ToString() == null ? "" : modulus.ToString());
                                        frmCPHList.Add(cardNoNo[modulus] == null ? "" : cardNoNo[modulus]);
                                        frmCPHList.Add(myCarNo[modulus] == null ? "" : myCarNo[modulus]);
                                        frmCPHList.Add(carNoNo[modulus] == null ? "" : carNoNo[modulus]);
                                        frmCPHList.Add(monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        frmCPHList.Add(inOutPic[modulus] == null ? "" : inOutPic[modulus]);
                                        frmCPHList.Add(cardType[modulus] == null ? "" : cardType[modulus]);
                                        frmCPHList.Add(summary0.SurplusCarCount.ToString());
                                        frmCPHList.Add(monthSurplusDay.ToString());

                                        //add

                                        frmCPHList.Add(strCardCW);
                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                       {
                                           ParkingTempCPH parkFrm = new ParkingTempCPH(frmCPHList, new UpdateCPHDataHandler(UpdateCPH), "车场满位，确认开闸");
                                           parkFrm.Show();
                                       }));
                                    }

                                    ShowLoadAlert("储值车位已满！");
                                    //txbOperatorInfo.Text = "储值车位已满!";
                                    sender0.VoiceDisplay(VoiceType.ParkingFull, modulus);
                                    return false;
                                }
                                else if (cardType[modulus].Substring(0, 3) == "Tmp" && Model.iFullLight == 2)
                                {
                                    UpdateTxbText(txbOperatorInfo, "临时车位已满!");
                                    if (Model.bFullComfirmOpen)
                                    {
                                        List<string> frmCPHList = new List<string>();
                                        frmCPHList.Add(modulus.ToString() == null ? "" : modulus.ToString());
                                        frmCPHList.Add(cardNoNo[modulus] == null ? "" : cardNoNo[modulus]);
                                        frmCPHList.Add(myCarNo[modulus] == null ? "" : myCarNo[modulus]);
                                        frmCPHList.Add(carNoNo[modulus] == null ? "" : carNoNo[modulus]);
                                        frmCPHList.Add(monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        frmCPHList.Add(inOutPic[modulus] == null ? "" : inOutPic[modulus]);
                                        frmCPHList.Add(cardType[modulus] == null ? "" : cardType[modulus]);
                                        frmCPHList.Add(summary0.SurplusCarCount.ToString());
                                        frmCPHList.Add(monthSurplusDay.ToString());

                                        //add

                                        frmCPHList.Add(strCardCW);
                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                       {
                                           ParkingTempCPH parkFrm = new ParkingTempCPH(frmCPHList, new UpdateCPHDataHandler(UpdateCPH), "车场满位，确认开闸");
                                           parkFrm.Show();
                                       }));
                                    }
                                    ShowLoadAlert("临时车位已满！");
                                    //txbOperatorInfo.Text = "临时车位已满!";
                                    sender0.VoiceDisplay(VoiceType.ParkingFull, modulus);
                                    return false;
                                }
                                else if (cardType[modulus].Substring(0, 3) == "Mth" && Model.iFullLight == 1)
                                {
                                    UpdateTxbText(txbOperatorInfo, "月租车位已满!");

                                    if (Model.bFullComfirmOpen)
                                    {
                                        List<string> frmCPHList = new List<string>();
                                        frmCPHList.Add(modulus.ToString() == null ? "" : modulus.ToString());
                                        frmCPHList.Add(cardNoNo[modulus] == null ? "" : cardNoNo[modulus]);
                                        frmCPHList.Add(myCarNo[modulus] == null ? "" : myCarNo[modulus]);
                                        frmCPHList.Add(carNoNo[modulus] == null ? "" : carNoNo[modulus]);
                                        frmCPHList.Add(monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        frmCPHList.Add(inOutPic[modulus] == null ? "" : inOutPic[modulus]);
                                        frmCPHList.Add(cardType[modulus] == null ? "" : cardType[modulus]);
                                        frmCPHList.Add(summary0.SurplusCarCount.ToString());
                                        frmCPHList.Add(monthSurplusDay.ToString());

                                        //add

                                        frmCPHList.Add(strCardCW);
                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                        new Action(() =>
                                        {
                                            ParkingTempCPH parkFrm = new ParkingTempCPH(frmCPHList, new UpdateCPHDataHandler(UpdateCPH), "车场满位，确认开闸");
                                            parkFrm.Show();
                                        }));
                                    }

                                    ShowLoadAlert("月租车位已满！");
                                    //txbOperatorInfo.Text = "月租车位已满!";
                                    sender0.VoiceDisplay(VoiceType.ParkingFull, modulus);
                                    return false;
                                }
                            }
                        }
                    }

                    monitor.InTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //lblInTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //Lbl_TimeList[modulus].Text = lblinDateTime.Text;

                    if (cardType[modulus].Substring(0, 3) == "Str")
                    {
                        if (bMonth)
                        {
                            monitor.Balance = lstMonth[0].Balance;
                            //lblBalance.Content = Convert.ToDecimal(lstMonth[0].Balance).ToString("00.00");
                        }
                        else
                        {
                            monitor.Balance = 0;
                            //lblBalance.Content = "0.0";
                        }

                        if (monitor.Balance == 0)
                        //if (Convert.ToDecimal(lblBalance.Content) == 0)
                        {
                            UpdateTxbText(txbOperatorInfo, "余额不足，请先充值!");
                            ShowLoadAlert("余额不足，请先充值！");
                            //txbOperatorInfo.Text = "余额不足，请先充值!";
                            sender0.VoiceDisplay(VoiceType.BalanceInsufficient, modulus);
                            return false;
                        }
                    }

                    //判断月卡多车位多车是否禁止入场
                    if (cardType[modulus].Substring(0, 3) == "Mth" && Model.iMorePaingCar == 1 && Model.iMorePaingType == 0)
                    {
                        if (gsd.GetComeCount(monitor.PersonNo, carNoNo[modulus]) > gsd.GetPersonCount(monitor.PersonNo))
                        //if (gsd.GetComeCount(lblPersonNo.Content.ToString(), carNoNo[modulus]) > gsd.GetPersonCount(lblPersonNo.Content.ToString()))
                        {
                            UpdateTxbText(txbOperatorInfo, "此车禁止入场，入场车辆数已经超过车位个数");
                            ShowLoadAlert("此车禁止入场，入场车辆数已经超过车位个数。");
                            //txbOperatorInfo.Text = "此车禁止入场，入场车辆数已经超过车位个数";
                            sender0.LoadLsNoX2010znykt(modulus, "9ED2");
                            return false;
                        }

                    }
                    //判断通车位禁止入场
                    if ((cardType[modulus].Substring(0, 3) == "Mth" || cardType[modulus].Substring(0, 3) == "Fre") && Model.iForbidSamePosition == 1 && lstMonth[0].CarPlace != "")
                    {
                        int ret = gsd.GetReadIn(lstMonth[0].CarPlace, cardNoNo[modulus]);
                        if (ret > 0)
                        {
                            UpdateTxbText(txbOperatorInfo, "此车禁止入场，已有相同车位的车已入场，车位[" + lstMonth[0].CarPlace.ToString() + "]");
                            ShowLoadAlert("此车禁止入场，已有相同车位的车已入场，车位[" + lstMonth[0].CarPlace.ToString() + "]");
                            //txbOperatorInfo.Text = "此车禁止入场，已有相同车位的车已入场，车位[" + lstMonth[0].CarPlace.ToString() + "]";
                            sender0.LoadLsNoX2010znykt(modulus, "9ED2");
                            return false;
                        }
                    }
                }
                //处理出口
                else if (Model.Channels[modulus].iInOut == 1)
                {
                    //出场控制一进一出
                    if (Model.iIDOneInOneOut == 1 && !bOffLine[modulus])
                    {
                        inOutCtrl = false;
                        if (cardType[modulus] == "Opt")
                        {
                            inOutCtrl = false;
                        }
                        else if (cardType[modulus].Substring(0, 3) == "Tmp" || cardType[modulus].Substring(0, 3) == "Mtp")
                        {
                            inOutCtrl = true;
                        }

                        if (Model.sID1In1OutCardType.Contains(cardType[modulus]))
                        {
                            inOutCtrl = true;
                        }

                        if (inOutCtrl)
                        {
                            //查询入场表是否含有数据
                            if (bInCome)
                            {
                                Model.iAutoCZJL = true;
                                monitor.InTime = dtInCome[0].InTime;
                                // lblInTime.Content = dtInCome.Rows[0]["InTime"].ToString();
                                //UpdateUiText(lblInTime, dtInCome.Rows[0]["InTime"].ToString());
                                //lstLblInOutTime[modulus].Content = lblInTime.Content;
                            }
                            else
                            {
                                Model.iAutoCZJL = false;

                                UpdateTxbText(txbOperatorInfo, "此车已出场[" + Model.Channels[modulus].sInOutName + "]");
                                //txbOperatorInfo.Text = "此车已出场[" + Model.Channels[modulus].sInOutName + "]";
                                ShowLoadAlert("此车已出场【" + Model.Channels[modulus].sInOutName + "】");
                                sender0.VoiceDisplay(VoiceType.AlreadyAppeared, modulus);
                                return false;
                                
                            }
                        }
                        else
                        {
                            if (bInCome)
                            {
                                Model.iAutoCZJL = true;
                                monitor.InTime = dtInCome[0].InTime;
                                //UpdateUiText(lblInTime, dtInCome.Rows[0]["InTime"].ToString());
                                //lblInTime.Content = dtInCome.Rows[0]["InTime"].ToString();
                                //lstLblInOutTime[modulus].Content = lblInTime.Content;
                            }
                            else
                            {
                                Model.iAutoCZJL = false;
                                monitor.InTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                //UpdateUiText(lblInTime, dtInCome.Rows[0]["InTime"].ToString());
                                //lblInTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                //lstLblInOutTime[modulus].Content = lblInTime.Content;
                            }
                        }

                        monitor.OutTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //lblOutTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //lstLblInOutTime[modulus].Content = lblOutTime.Content;
                    }
                    //出场控制一进一出
                    else if (Model.iIDOneInOneOut == 0 && !bOffLine[modulus])
                    {
                        if (bInCome)
                        {
                            Model.iAutoCZJL = true;
                            monitor.InTime = dtInCome[0].InTime;
                            //lblInTime.Content = dtInCome.Rows[0]["InTime"].ToString();   
                        }
                        else
                        {
                            Model.iAutoCZJL = false;
                            monitor.InTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            //lblInTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        //lstLblInOutTime[modulus].Content = lblInTime.Content;

                        monitor.OutTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //lblOutTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //lstLblInOutTime[modulus].Content = lblOutTime.Content;
                    }
                    //脱机提取分解出入场时间
                    else if (bOffLine[modulus])
                    {
                        string inYear = DateTime.Now.Year.ToString().Substring(0, 3) + cardNO.Substring(18, 1);
                        if (Convert.ToInt32(inYear) > DateTime.Now.Year)
                        {
                            inYear = (Convert.ToInt32(inYear) - 10).ToString();
                        }
                        string inMouth = Convert.ToInt32(cardNO.Substring(19, 1), 16).ToString();
                        ssInDateTime = inYear + "-" + inMouth + "-" + cardNO.Substring(20, 2) + " " + cardNO.Substring(22, 2) + ":" + cardNO.Substring(24, 2) + ":" + cardNO.Substring(26, 2);
                        if (CR.IsTime(ssInDateTime))
                        {
                            monitor.InTime = Convert.ToDateTime(ssInDateTime);
                            //lblInTime.Content = Convert.ToDateTime(ssInDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                            //lstLblInOutTime[modulus].Content = lblInTime.Content;
                        }
                        else
                        {
                            monitor.InTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            //lblInTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //lstLblInOutTime[modulus].Content = lblInTime.Content;
                            bDateTimeErr[0] = true;
                            Model.iLoadTimeType = 0;
                            loadTime = DateTime.Now.AddDays(-1);
                        }

                        string OutYear = DateTime.Now.Year.ToString().Substring(0, 3) + cardNO.Substring(28, 1);
                        if (Convert.ToInt32(OutYear) > DateTime.Now.Year)
                        {
                            OutYear = (Convert.ToInt32(OutYear) - 10).ToString();
                        }
                        string OutMouth = Convert.ToInt32(cardNO.Substring(29, 1), 16).ToString();
                        ssOutDateTime = OutYear + "-" + OutMouth + "-" + cardNO.Substring(30, 2) + " " + cardNO.Substring(32, 2) + ":" + cardNO.Substring(34, 2) + ":" + cardNO.Substring(36, 2);

                        if (CR.IsTime(ssOutDateTime))
                        {
                            monitor.OutTime = DateTime.Parse(Convert.ToDateTime(ssOutDateTime).ToString("yyyy-MM-dd HH:mm:00"));
                            //lblOutTime.Content = Convert.ToDateTime(ssOutDateTime).ToString("yyyy-MM-dd HH:mm:00");
                            if (Math.Abs(CR.DateDiff(CR.DateInterval.Minute, DateTime.Now, Convert.ToDateTime(ssOutDateTime))) > 2)
                            {
                                Model.iLoadTimeType = 0;
                                loadTime = DateTime.Now.AddDays(-1);
                                if (Convert.ToDateTime(ssOutDateTime) < Convert.ToDateTime("2011-01-01"))
                                {
                                    monitor.OutTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //lblOutTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    bDateTimeErr[0] = true;
                                }
                                if (Math.Abs(CR.DateDiff(CR.DateInterval.Minute, DateTime.Now, Convert.ToDateTime(ssOutDateTime))) > 2)
                                {
                                    monitor.OutTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //lblOutTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    bDateTimeErr[0] = true;
                                }
                                //lstLblInOutTime[iIndex].Content = lblOutTime.Content;
                            }
                            else
                            {
                                monitor.OutTime = Convert.ToDateTime(ssOutDateTime);
                                //lblOutTime.Content = Convert.ToDateTime(ssOutDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        else
                        {
                            monitor.OutTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            //lblOutTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            bDateTimeErr[0] = true;
                            Model.iLoadTimeType = 0;
                            loadTime = DateTime.Now.AddDays(-1);
                        }
                        //lstLblInOutTime[iIndex].Content = lblOutTime.Content;
                    }

                    outTime[modulus] = monitor.OutTime;
                    //outTime[modulus] = Convert.ToDateTime(lblOutTime.Content);

                }
                inTime[modulus] = monitor.InTime;
                //inTime[modulus] = Convert.ToDateTime(lblInTime.Content);

                //处理车位号 方便播报语音
                if (bMonth)
                {
                  
                    strCardCW = lstMonth[0].CarPlace ?? "";
                    if (strCardCW == "")
                    {
                        strCardCW = "FFFF";
                    }
                    if (strCardCW.Length != 4)
                    {
                        string CarCW = strCardCW;
                        for (int i = 0; i < 4 - strCardCW.Length; i++)
                        {
                            CarCW = "0" + CarCW;
                        }
                        strCardCW = CarCW.Substring(CarCW.Length - 4, 4);
                    }
                }
                else
                {
                    strCardCW = "FFFF";
                }

                if (monitor.CardNo == "00000000")
                //if (lblCardNo.Content.ToString() == "00000000")
                {
                    // MyCarNo = "人工开闸";
                    InOut = 1;
                }

                if (cardTypeCmd == "DD")
                {
                    InOut = 1;
                }

                if (InOut > 0 && Model.Channels[modulus].iBigSmall == 0)
                {
                    //处理储值卡收费
                    if (cardType[modulus].Substring(0, 3) == "Str")
                    {
                        if (bMonth)
                        {
                            monitor.Balance = lstMonth[0].Balance;
                            //lblBalance.Content = Convert.ToDecimal(lstMonth[0].Balance.ToString("00.00"));
                        }
                        else
                        {
                            monitor.Balance = Convert.ToDecimal(0.0);
                            //lblBalance.Content = "0.0";
                        }
            
                        cacl = gsd.GetMONEY(cardType[modulus], monitor.InTime, monitor.OutTime, monitor.CarNo);
                        monitor.Charge = cacl.SFJE;
                        monitor.AmountReceivable = cacl.YSJE;

                        //lblCharge.Content = gsd.GetMoney(cardType[modulus], Convert.ToDateTime(lblInTime.Content), Convert.ToDateTime(lblOutTime.Content)).ToString("0.00");

                        if (CR.IsNumberic(monitor.Charge.ToString()) == true)
                        //if (CR.IsNumberic(lblCharge.Content.ToString()) == true)
                        {
                            decYSJE = monitor.Charge;
                            //decYSJE = Convert.ToDecimal(lblCharge.Content);
                        }
                        else
                        {
                            decYSJE = 0;
                        }
                        //每天最高收费限额
                        if (Model.iZGXE == 1 && (Model.iZGType == 1 || Model.iZGType == 2))
                        {
                            //!!!
                            monitor.AmountReceivable = gsd.GetDayMoneyLimit("", cardType[modulus], carNoNo[modulus], monitor.InTime, monitor.OutTime, Model.iXsd, monitor.Charge, Model.iZGXEType, modulus);
                            if (monitor.AmountReceivable < monitor.Charge)
                            {
                                monitor.Charge = monitor.AmountReceivable;
                            }
                        }

                        if (monitor.Charge > 0)
                        //if (Convert.ToDecimal(lblCharge.Content) > 0)
                        {
                            if (monitor.Balance < monitor.Charge)
                            //if (Convert.ToDecimal(lblBalance.Content) < Convert.ToDecimal(lblCharge.Content))
                            {
                                UpdateTxbText(txbOperatorInfo, "余额不足，请先充值!");
                                ShowLoadAlert("余额不足，请先充值！");
                                //txbOperatorInfo.Text = "余额不足，请先充值!";
                                sender0.VoiceDisplay(VoiceType.BalanceInsufficient, modulus);
                                return false;
                            }
                            else
                            {
                                monitor.Balance = Convert.ToDecimal((monitor.Balance - monitor.Charge).ToString("0.00"));
                                //lblBalance.Content = (Convert.ToDecimal(lblBalance.Content) - Convert.ToDecimal(lblCharge.Content)).ToString("0.00");
                            }
                        }
                    }
                    else
                    {
                        if (bMonthPastdue)
                        {
                            cardType[modulus] = "Mtp" + CR.GetCardType(monitor.CardType, 0).Substring(3, 1);
                            //cardType[modulus] = "Mtp" + CR.GetCardType(lblCardType.Content.ToString(), 0).Substring(3, 1);
                            //lblCardType.Content = CR.GetCardType(cardType[modulus], 1);
                            monitor.CardType = CR.GetCardType(cardType[modulus], 1);
                        }

                        //2016-08-26 月卡出场按临时卡类收费
                        bool bMonthOutChargeType = false;
                        if (cardType[modulus].Substring(0, 3) == "Mth" && Model.sMonthOutChargeType.IndexOf(cardType[modulus]) >= 0)
                        {
                            bMonthOutChargeType = true;
                        }


                        //判断多车位多车
                        bool bMothCard = false;
                        if (Model.iMorePaingCar == 1 && Model.iMorePaingType == 1 && cardType[modulus].Substring(0, 3) == "Mth" && gsd.GetInMonth(cardNoNo[modulus]) > 0)
                        {
                            bMothCard = true;
                        }
                        else
                        {
                            bMothCard = false;
                        }

                        if ((cardTypeCmd == "3F" || cardTypeCmd == "8F" || bPaperRecord == true) && (cardType[modulus].Substring(0, 3) == "Mtp" || cardType[modulus].Substring(0, 3) == "Tmp" || bMothCard || bMonthOutChargeType) && !bOffLine[modulus])
                        {
                            bool bOverTime = false;
                            //启用中央收费功能
                            if (Model.iCentralCharge == 1)
                            {
                                List<CarIn> lstCI = gsd.GetCentralCharge(carNoNo[modulus]);
                                if (lstCI.Count > 0)
                                {
                                    string strSFGate = lstCI[0].SFGate;
                                    if (strSFGate == "中央收费")
                                    {
                                        bOverTime = true;
                                    }
                                }
                                else
                                {
                                    bOverTime = false;
                                }
                            }
                            else
                            {

                            }

                            string tmpCardType = "";
                            DateTime dtIn, dtOut;

                            dtIn = monitor.InTime;
                            dtOut = monitor.OutTime;
                            //dtIn = Convert.ToDateTime(lblInTime.Content);
                            //dtOut = Convert.ToDateTime(lblOutTime.Content);

                            if (bMonthOutChargeType)   //月卡出场按临时卡类收费
                            {
                                if (Model.sMonthOutChargeType.IndexOf(cardType[modulus]) >= 0)  //2016-08-26
                                {
                                    tmpCardType = Model.sMonthOutChargeType.Substring(Model.sMonthOutChargeType.IndexOf(cardType[modulus]) + 5, 4);  //MthA:TmpA
                                }
                                else
                                {
                                    tmpCardType = "Tmp" + cardType[modulus].Substring(3);
                                }
                            }
                            else if (bMothCard)
                            {
                                tmpCardType = "Tmp" + cardType[modulus].Substring(3);
                            }
                            else if (cardType[modulus].Substring(0, 3) == "Mtp" && Model.iChargeType != 1)
                            {
                                tmpCardType = "Tmp" + cardType[modulus].Substring(3);

                                GetMonthCardChargeTime(monitor.CardNo, ref dtIn, ref dtOut);
                                //GetMonthCardChargeTime(lblCardNo.Content.ToString(), ref dtIn, ref dtOut);
                            }
                            else
                            {
                                tmpCardType = cardType[modulus];
                                dtIn = monitor.InTime;
                                //dtIn = Convert.ToDateTime(lblInTime.Content);
                            }


                            if (bOverTime && Model.iCentralCharge == 1)
                            {
                                tmpCardType = "TmpJ";
                            }


                           
                            cacl = gsd.GetMONEY(tmpCardType, dtIn, dtOut, monitor.CarNo);
                            monitor.Charge = cacl.SFJE;
                            monitor.AmountReceivable = cacl.YSJE;
                            //lblCharge.Content = gsd.GetMONEY(tmpCardType, dtIn, dtOut).ToString();

                            //if (Model.iXsd == 1)
                            //{
                            //    if (monitor.Charge > 0)
                            //    {
                            //        monitor.Charge = monitor.Charge / 10;
                            //    }
                            //}


                            if (CR.IsNumberic(monitor.Charge.ToString()) == true)
                            //if (CR.IsNumberic(lblCharge.Content.ToString()) == true)
                            {
                                decYSJE = Convert.ToDecimal(monitor.Charge);
                                //decYSJE = Convert.ToDecimal(lblCharge.Content);
                            }
                            else
                            {
                                decYSJE = 0;
                            }

                            if ((bMothCard || bMonthOutChargeType) && monitor.Charge > 0)
                            //if (bMothCard && Convert.ToDecimal(lblCharge.Content) > 0)
                            {
                                cardType[modulus] = "Mtp" + cardType[modulus].Substring(3);
                                monitor.CardType = CR.GetCardType(cardType[modulus], 1);

                                //lblCardType.Content = CR.GetCardType(cardType[modulus], 1);
                            }

                            if (Model.iCentralCharge == 1)
                            {
                                if (Model.iOutCharge == 0 && monitor.Charge > 0)
                                //if (!Model.bOutSF && Convert.ToDecimal(lblCharge.Content) > 0)
                                {
                                    string strsLoad = "7A";
                                    if (bOverTime)
                                    {
                                        strsLoad = "7B";
                                        UpdateTxbText(txbOperatorInfo, "已超时请回收费处补费");
                                        ShowLoadAlert("已超时请回收费处补费");
                                        //txbOperatorInfo.Text = "已超时请回收费处补费";

                                    }
                                    else
                                    {
                                        strsLoad = "7A";
                                        UpdateTxbText(txbOperatorInfo, "未交费请到收费处交费");
                                        ShowLoadAlert("未交费请到收费处交费");
                                        //txbOperatorInfo.Text = "未交费请到收费处交费";
                                    }
                                    int iLenY = (strsLoad.Length / 2);
                                    strsLoad += CR.YHXY(strsLoad).ToString("X2");
                                    sender0.LoadLsNoX2010znykt(modulus, strsLoad);
                                    return false;
                                }
                                else
                                {
                                    if (bOverTime)
                                    {
                                        Model.bOverTimeSF = true;
                                    }
                                    else
                                    {
                                        Model.bOverTimeSF = false;
                                    }
                                }
                            }

                            if (Model.iZGXE == 1 && (Model.iZGType == 0 || Model.iZGType == 2))
                            {
                                //最高收费
                                //lblbuckleMoney.Text = CR.GetDayMoneyLimit("", CardType[modulus], CarNoNo[modulus], Convert.ToDateTime(lblinDateTime.Text), Convert.ToDateTime(lbloutDateTime.Text), Model.iXsd, Convert.ToDecimal(lblbuckleMoney.Text), Model.iZGXEType, modulus).ToString();
                                monitor.AmountReceivable = gsd.GetDayMoneyLimit("", cardType[modulus], carNoNo[modulus], monitor.InTime, monitor.OutTime, Model.iXsd, monitor.AmountReceivable, Model.iZGXEType, modulus);
                                if (monitor.AmountReceivable < monitor.Charge)
                                {
                                    monitor.Charge = monitor.AmountReceivable;
                                }
                            }
                        }
                        else
                        {
                            if (Model.iXsd == 0)
                            {
                                if (Model.iChargeType == 3)
                                {
                                    monitor.Charge = Convert.ToDecimal((Convert.ToInt32(cardNO.Substring(42, 4), 16) / 100));
                                    if (Model.iXsdNum == 1)
                                    {
                                        //lblCharge.Content = Convert.ToDecimal(Convert.ToInt32(cardNO.Substring(42, 4), 16) / 10).ToString();
                                        if (monitor.Charge.ToString() == "999.9")
                                        {
                                            UpdateTxbText(txbOperatorInfo, "超消费额999.9元");
                                            //txbOperatorInfo.Text = "超消费额999.9元";
                                        }
                                    }
                                    else
                                    {
                                        //lblCharge.Content = Convert.ToDecimal((Convert.ToInt32(cardNO.Substring(42, 4), 16) / 100)).ToString();
                                        //if (lblCharge.Content.ToString() == "665.35")
                                        if (monitor.Charge.ToString() == "665.35")
                                        {
                                            UpdateTxbText(txbOperatorInfo, "超消费额665.35元!");
                                            //txbOperatorInfo.Text = "超消费额665.35元!";
                                        }
                                    }
                                }
                                else
                                {
                                    if (Model.bSfDec)
                                    {
                                        monitor.Charge = Convert.ToDecimal(cardNO.Substring(42, 4));
                                        //lblCharge.Content = Convert.ToDecimal(cardNO.Substring(42, 4)).ToString("0.00");
                                    }
                                    else
                                    {
                                        monitor.Charge = Convert.ToInt32(cardNO.Substring(42, 4), 16);
                                        //lblCharge.Content = Convert.ToInt32(cardNO.Substring(42, 4), 16).ToString();
                                    }
                                }
                            }
                            else
                            {
                                decimal tmp = Convert.ToDecimal(Convert.ToInt32(cardNO.Substring(42, 4), 16));
                                monitor.Charge = tmp / 10;
                                //lblCharge.Content = (tmp / 10).ToString("0.00");
                            }

                            decYSJE = monitor.Charge.ToString() == "" ? 0 : monitor.Charge;
                            //decYSJE = Convert.ToDecimal(lblCharge.Content.ToString() == "" ? "0" : lblCharge.Content);
                            if (Model.iZGXE == 1 && (Model.iZGType == 1 || Model.iZGType == 2))
                            {
                                //最高收费
                                //lblbuckleMoney.Text = CR.GetDayMoneyLimit("", CardType[modulus], CarNoNo[modulus], Convert.ToDateTime(lblinDateTime.Text), Convert.ToDateTime(lbloutDateTime.Text), Model.iXsd, Convert.ToDecimal(lblbuckleMoney.Text), Model.iZGXEType, modulus).ToString();
                                monitor.AmountReceivable = gsd.GetDayMoneyLimit("", cardType[modulus], carNoNo[modulus], monitor.InTime, monitor.OutTime, Model.iXsd, monitor.AmountReceivable, Model.iZGXEType, modulus);
                                if (monitor.AmountReceivable < monitor.Charge)
                                {
                                    monitor.Charge = monitor.AmountReceivable;
                                }
                            }
                        }
                    }
                }
                else
                {
                    monitor.Charge = 0;
                    if (cardType[modulus].Substring(0, 3) == "Str")//处理储值卡收费
                    {
                        if (bMonth)
                        {
                            monitor.Balance = lstMonth[0].Balance;
                        }
                        else
                        {
                            monitor.Balance = Convert.ToDecimal(0.0);
                        }
                    }
                    //lblCharge.Content = "0.00";
                }

                charge[modulus] = Convert.ToDouble(monitor.Charge.ToString() == "" ? 0 : monitor.Charge);
                storeBalance[modulus] = Convert.ToDouble(monitor.Balance.ToString() == "" ? 0 : monitor.Balance);

                //charge[modulus] = Convert.ToDouble(lblCharge.Content.ToString() == "" ? "0" : lblCharge.Content);
                //storeBalance[modulus] = Convert.ToDouble(lblBalance.Content.ToString() == "" ? "0" : lblBalance.Content.ToString());

                if (Model.Channels[modulus].iInOut == 1)
                {
                    WriteTemp(monitor.CardNo, monitor.CardType, monitor.InTime, monitor.OutTime, Convert.ToDecimal(monitor.Charge.ToString() == "" ? 0 : monitor.Charge), decYSJE, cardNO);

                    //WriteTemp(lblCardNo.Content.ToString(), cardType[modulus], Convert.ToDateTime(lblInTime.Content.ToString()), Convert.ToDateTime(lblOutTime.Content.ToString()), Convert.ToDecimal(lblCharge.Content == "" ? "0" : lblCharge.Content), decYSJE, cardNO);
                    //txbCharge.Text = Convert.ToDecimal(lblCharge.Content).ToString("0.00");
                }

                if (monitor.CardNo.Length > 6)
                //if (lblCarNo.Content.ToString().Length > 6)
                {
                    //-----------------------------------------------------
                    //if (lblCarNo.Text == "66666666" || lblCarNo.Text == "00000000" || lblCarNo.Text == "88888888")
                    //{
                    //    textBox1.Visible = false;
                    //}
                    //else
                    //{
                    //    textBox1.Visible = true;
                    //    textBox1.Text = lblCarNo.Text;
                    //}
                }
                else
                {
                    //textBox1.Visible = false;
                }

                if (Model.bPcTalkPlate)
                {
                    carNoCmd = monitor.CarNo;
                    //carNoCmd = lblCarNo.Content.ToString();
                    if (carNoCmd.Length != 7 || carNoCmd == "00000000" || carNoCmd == "6666666" || carNoCmd == "8888888" || carNoCmd == "京000000")
                    {

                    }
                    else
                    {

                    }
                }
                return FillOut;
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":FillOutData" + cardNO, ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// 入场开闸发语音
        /// </summary>
        /// <param name="OpenBit"></param>
        private void Flag0hereopen(string OpenBit)
        {
            try
            {
                string tempCardTypeCmd = CR.GetCardType(monitor.CardType, 0);
                if (tempCardTypeCmd.Substring(0, 3) == "Mth" && Model.iInMothOpenModel == 1)
                {
                    return;
                }

                if (cardTypeCmd == "3F" && OpenBit == "1" && Model.iInAutoOpenModel == 0)
                {
                    sender0.SendOpen(modulus);
                }
                else if ((tempCardTypeCmd.Substring(0, 3) == "Mth" || tempCardTypeCmd.Substring(0, 3) == "Str" || tempCardTypeCmd.Substring(0, 3) == "Fre") && Model.iInAutoOpenModel > 0)
                {
                    sender0.SendOpen(modulus);
                }
                else if ((tempCardTypeCmd.Substring(0, 3) == "Tmp" || tempCardTypeCmd.Substring(0, 3) == "Mtp") && (bReadAuto && Model.iInAutoOpenModel == 0))
                {
                    sender0.SendOpen(modulus);
                }
                else
                {
                    return;
                }


                if (Model.iDetailLog == 1)
                {
                    CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口开闸完成----车道:" + modulus.ToString());
                }


                #region  多功能语音发送
                if (Model.iRemainPosLedShowPlate == 1)
                {
                    List<VoiceMultiSet> lstVMS = gsd.GetMyVoiceCount(cardNoNo[modulus]);
                    List<VoiceSelfDefine> lstVSD = gsd.SelectCardNOVoice(cardNoNo[modulus], Model.Channels[modulus].iCtrlID);
                    List<CardIssue> lstCI = gsd.SelectPersonBirthDate(cardNoNo[modulus]);
                    if (lstVMS.Count > 0 || lstVSD.Count > 0 || (lstCI.Count > 0 && Model.iRemainPosLedShowInfo == 1))
                    {
                        byte[] arryVoice;
                        if (lstVMS.Count > 0)
                        {
                            arryVoice = CR.ChinatoAscII(lstVMS[0].InVoice);
                        }
                        else if (lstVSD.Count > 0)
                        {
                            arryVoice = CR.ChinatoAscII(lstVSD[0].Voice);
                        }
                        else
                        {
                            arryVoice = CR.ChinatoAscII("生日快乐");
                        }
                        string str = string.Empty;
                        int jihao = Model.Channels[modulus].iCtrlID;
                        string strIP = Model.Channels[modulus].sIP;
                        int xieyi = Convert.ToInt32(Model.Channels[modulus].iXieYi);
                        if (xieyi == 1)
                        {
                            ParkingCommunication.SedBll sedbll = new SedBll(strIP, 1007, 1005);
                            string strReturn = sedbll.CtrlLedShow(Convert.ToByte(jihao), 0x3D, 0x9F, str, xieyi);
                            return;
                        }
                    }
                }
                #endregion

                if (bMonthPastdue) { return; }
                iCtrlLedDelay = 1;

                if (tempCardTypeCmd.Substring(0, 3) == "Mth" || tempCardTypeCmd.Substring(0, 3) == "Str")
                {
                    iCtrlLedDelay = 4;
                }


                sender0.VoiceDisplay(VoiceType.InGateVoice, modulus, tempCardTypeCmd, monitor.CarNo, Convert.ToInt32(monthSurplusDay), strCardCW, summary0.SurplusCarCount, monitor.Balance);

                if (Model.iDetailLog == 1)
                {
                    CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + myCarNo[modulus] + "入口发送语音完成---车道：" + modulus.ToString());
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控:Flag0hereopen", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 出场开闸发语音
        /// </summary>
        private void Flag1hereopen()
        {
            try
            {
                if (cardTypeCmd == "3F")
                {
                    string tempCardTypeCmd = CR.GetCardType(monitor.CardType, 0);
                    if (tempCardTypeCmd.Substring(0, 3) == "Mth" && Model.iOutMothOpenModel == 1)
                    {
                        return;
                    }

                    if ((tempCardTypeCmd.Substring(0, 3) == "Tmp" || tempCardTypeCmd.Substring(0, 3) == "Mtp") && Model.iOutAutoOpenModel > 0)
                    {
                        if (Model.iAutoKZ == 1 && monitor.Charge == 0 && Model.iOutAutoOpenModel == 1)
                        {
                            if (Model.iAutoCZJL)
                            {
                                sender0.SendOpen(modulus);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (bReadAuto || tempCardTypeCmd.Substring(0, 3) != "Tmp")
                        {
                            sender0.SendOpen(modulus);
                        }
                        else
                        {
                            return;
                        }
                    }

                    #region  多功能语音发送
                    if (Model.iRemainPosLedShowPlate == 1)
                    {
                        List<VoiceMultiSet> lstVMS = gsd.GetMyVoiceCount(cardNoNo[modulus]);
                        List<VoiceSelfDefine> lstVSD = gsd.SelectCardNOVoice(cardNoNo[modulus], Model.Channels[modulus].iCtrlID);
                        List<CardIssue> lstCI = gsd.SelectPersonBirthDate(cardNoNo[modulus]);
                        if (lstVMS.Count > 0 || lstVSD.Count > 0 || (lstCI.Count > 0 && Model.iRemainPosLedShowInfo == 1))
                        {
                            byte[] arryVoice;
                            if (lstVMS.Count > 0)
                            {
                                arryVoice = CR.ChinatoAscII(lstVMS[0].InVoice);
                            }
                            else if (lstVSD.Count > 0)
                            {
                                arryVoice = CR.ChinatoAscII(lstVSD[0].Voice);
                            }
                            else
                            {
                                arryVoice = CR.ChinatoAscII("生日快乐");
                            }
                            string str = string.Empty;
                            int jihao = Model.Channels[modulus].iCtrlID;
                            string strIP = Model.Channels[modulus].sIP;
                            int xieyi = Convert.ToInt32(Model.Channels[modulus].iXieYi);
                            if (xieyi == 1)
                            {
                                ParkingCommunication.SedBll sedbll = new SedBll(strIP, 1007, 1005);
                                string strReturn = sedbll.CtrlLedShow(Convert.ToByte(jihao), 0x3D, 0x9F, str, xieyi);
                                return;
                            }
                        }
                    }
                    #endregion


                    if (tempCardTypeCmd.Substring(0, 3) == "Tmp" || tempCardTypeCmd.Substring(0, 3) == "Mtp")
                    {
                        if (Model.iAutoKZ == 1 && monitor.Charge == 0 && Model.iOutAutoOpenModel == 1)
                        {
                            sender0.VoiceDisplay(VoiceType.TempOutOpen, modulus);
                            return;
                        }
                    }


                    if (tempCardTypeCmd.Substring(0, 3) != "Cnt")
                    {
                        TimeSpan ts = monitor.OutTime - monitor.InTime;
                        string stopTime = ts.Days.ToString("X4") + ts.Hours.ToString("X2") + ts.Minutes.ToString("X2");
                        sender0.VoiceDisplay(VoiceType.OutGateVoice, modulus, tempCardTypeCmd, monitor.CarNo, Convert.ToInt32(monthSurplusDay), strCardCW, Convert.ToInt32(summary0.SurplusCarCount), monitor.Balance, monitor.Charge, stopTime);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":Flag1hereopen", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void GetMonthCardChargeTime(string scardNo, ref DateTime dtInTime, ref DateTime dtOutTime)
        {
            //将控件上的值全部用缓存去处理
            DateTime cardEndDate = monitor.InTime;
            List<CardIssue> lstCI = gsd.GetMonthCardEndDate(scardNo);
            if (lstCI.Count > 0)
            {
                DateTime validStartTime = Convert.ToDateTime(lstCI[0].CarValidStartDate.ToString("yyyy-MM-dd 00:00:00"));
                DateTime validEndTime = Convert.ToDateTime(lstCI[0].CarValidEndDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
                //入场时间小于发行开始时间
                if (CR.DateDiff(CR.DateInterval.Minute, validStartTime, monitor.InTime) < 0)
                {
                    dtInTime = monitor.InTime;
                    if (CR.DateDiff(CR.DateInterval.Minute, validStartTime, monitor.OutTime) < 0)
                    {
                        dtOutTime = monitor.OutTime;
                    }
                    //出场在有效期内
                    else if (CR.DateDiff(CR.DateInterval.Minute, validStartTime, monitor.OutTime) >= 0 && CR.DateDiff(CR.DateInterval.Minute, validEndTime, monitor.OutTime) <= 0)
                    {
                        dtOutTime = validStartTime;
                    }
                    //出场大于发行结束日期
                    else
                    {
                        TimeSpan ts = validStartTime - monitor.InTime;
                        long tmpDate = (long)ts.TotalMinutes;
                        ts = monitor.OutTime - validEndTime;
                        tmpDate += (long)ts.TotalMinutes;
                        dtOutTime = monitor.InTime.AddMinutes(tmpDate);
                    }
                }

                else if (CR.DateDiff(CR.DateInterval.Minute, validStartTime, monitor.InTime) >= 0 && CR.DateDiff(CR.DateInterval.Minute, validEndTime, monitor.InTime) <= 0)
                {
                    dtInTime = validEndTime;
                    dtOutTime = monitor.OutTime;
                }

                else
                {
                    dtInTime = monitor.InTime;
                    dtOutTime = monitor.OutTime;
                }
            }
            return;
        }

        //2015-11-12 控制机显示屏，显示车牌号(及时显示)
        private void CtrlLedShowCPH(string sCPH, int iModulus)
        {
            string strLoad = sCPH;

            byte[] array = System.Text.Encoding.Default.GetBytes(strLoad);
            string str = string.Empty;
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    str += array[i].ToString("X2");
                }
            }

            byte byteCmdX;
            int iMacNO = Model.Channels[iModulus].iCtrlID;
            SedBll VsendBll = new SedBll(Model.Channels[iModulus].sIP, 1007, 1005);
            if (iMacNO > 127)
            {
                iMacNO = iMacNO - 127;
                byteCmdX = 0x45;
            }
            else
            {
                byteCmdX = 0x3D;
            }
            if (Model.Channels[iModulus].iXieYi == 1)
            {
                string rtnStr = VsendBll.CtrlLedShow(Convert.ToByte(iMacNO), byteCmdX, 0x67, str, Model.Channels[iModulus].iXieYi);
            }
        }

        private bool ComeGoLimit(int MyComeGo, string MyCardNO)
        {
            List<CarIn> InDs = gsd.GetCarcomerecord(MyCardNO);
            List<CarOut> OutDs = gsd.GetCargooutrecord(MyCardNO);

            if (MyComeGo == 0)
            {
                if (OutDs.Count > 0)
                {
                    string ss = OutDs[0].OutTime.ToString("yyyy-MM-dd HH:mm:ss");
                    TimeSpan ts = DateTime.Now - OutDs[0].OutTime;//秒
                    int seconds = ts.Seconds + ts.Minutes * 60 + ts.Hours * 60 * 60 + ts.Days * 60 * 60 * 24;
                    if (seconds <= Model.iInOutLimitSeconds && Model.iInOutLimitSeconds > 4)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (InDs.Count > 0)
                {
                    TimeSpan ints = DateTime.Now - InDs[0].InTime;
                    int inseconds = ints.Seconds + ints.Minutes * 60 + ints.Hours * 60 * 60 + ints.Days * 60 * 60 * 24;

                    if (inseconds <= Model.iInOutLimitSeconds && Model.iInOutLimitSeconds > 4)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Model.iIdReReadHandle == 1 && Convert.ToBoolean(Model.iIDOneInOneOut))
                    {
                        return false;
                    }
                    else
                    {
                        if (OutDs.Count > 0)
                        {
                            TimeSpan ts = DateTime.Now - OutDs[0].OutTime;//秒;//秒
                            int seconds = ts.Seconds + ts.Minutes * 60 + ts.Hours * 60 * 60 + ts.Days * 60 * 60 * 24;
                            if (seconds <= Model.iInOutLimitSeconds && Model.iInOutLimitSeconds > 4)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        void CphCompare()
        {
            string tempCardTypeCmd = CR.GetCardType(monitor.CardType, 0).Substring(0, 3);
            if (tempCardTypeCmd == "Mth")
            {
                if (Model.Channels[modulus].iInOut == 0)
                {
                    if (myCarNo[modulus] != carNoNo[modulus] && Model.iInAutoOpenModel == 1)
                    {
                        bCarNoConfirm = true;
                        return;
                    }
                    else
                    {
                        bCarNoConfirm = false;
                        return;
                    }
                }
                else
                {
                    if (myCarNo[modulus] != carNoNo[modulus] && Model.iOutAutoOpenModel == 1)
                    {
                        bCarNoConfirm = true;
                        return;
                    }
                    else
                    {
                        bCarNoConfirm = false;
                        return;
                    }
                }

            }
            if (tempCardTypeCmd == "Tmp" || tempCardTypeCmd == "Mtp")
            {
                List<CarIn> carin = gsd.SelectCome(cardNoNo[modulus], Model.iParkingNo, Model.Channels[modulus].iBigSmall);
                if (carin.Count > 0)
                {
                    if (carin[0].CPH == "66666666" || carin[0].CPH == "00000000" || carin[0].CPH == "")
                    {
                    }
                    else
                    {
                        carNoNo[modulus] = carin[0].CPH;
                    }
                    //CarNoNo[modulus] = ;
                    //2016-12-14 不比较第一位
                    if ((myCarNo[modulus].Substring(1) != carNoNo[modulus].Substring(1) || myCarNo[modulus] == "") && Model.iOutAutoOpenModel < 2)
                    {
                        bCarNoConfirm = true;
                    }
                    else
                    {
                        UpdateTxbText(lstTxbCarNo[modulus], carNoNo[modulus]);
                        //lstTxbCarNo[modulus].Text = carNoNo[modulus];
                        //lblCarNo.Content = carNoNo[modulus];

                        monitor.CarNo = carNoNo[modulus];
                        bCarNoConfirm = false;
                    }
                }
                else
                {
                    if (Model.Channels[modulus].iOpenType == 8)
                    {
                        bCarNoConfirm = true;
                    }
                    else
                    {
                        if (bOffLine[modulus])
                        {
                            bCarNoConfirm = false;
                        }
                        else
                        {
                            bCarNoConfirm = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 远距离ID卡重复读卡优化处理
        /// </summary>
        /// <param name="cardTmp"></param>
        /// <returns></returns>
        private bool IdCardMemory(string cardTmp)
        {
            bool cardMemory = false;
            for (int i = 0; i < 10; i++)
            {
                if (strCardTmp[i] == cardTmp)
                {
                    cardMemory = true;
                    break;
                }
            }
            if (cardMemory == false)
            {
                int i;
                for (i = 0; i < 10; i++)
                {
                    if (iCardSec[i] == 0)
                    {
                        strCardTmp[i] = cardTmp;
                        break;
                    }
                }
                if (i > 10)
                {
                    strCardTmp[0] = cardTmp;
                }
            }
            return cardMemory;
        }

        private void BinModel(string Data, int iIntindex)
        {
            try
            {
                if (Data.Length > 8)
                {
                    //卡片类型
                    string CardType = "";
                    //卡号
                    string CardNO = "";

                    //车牌号
                    string sumCPH = Data.Substring(46, 8);
                    string CPH = "";
                    if (sumCPH != "38888888")
                    {
                        CPH = CR.GetHexToCPH(sumCPH.Substring(0, 2)) + sumCPH.Substring(2, 6);
                    }

                    if (Data.Substring(0, 1) == "C")
                    {
                        CardType = CR.GetCardType(Data.Substring(16, 2));
                        if (Data.Substring(8, 4) == "0000")
                        {
                            CardNO = Data.Substring(12, 4);
                            CardNO = Convert.ToInt32(CardNO, 16).ToString("00000");
                        }
                        else
                        {
                            CardNO = Data.Substring(8, 8);
                        }
                    }
                    else if (Data.Substring(0, 1) == "E")
                    {
                        if (CPH == "")
                        {
                            CardNO = Data.Substring(8, 8);
                        }
                        else
                        {
                            CardNO = gsd.GetCardNO(CPH);
                            if (Model.Channels[iIntindex].iInOut == 1)
                            {
                                List<CarIn> Sdt = gsd.SelectComeCPH(CPH, 6, "Tmp", "Tmp");
                                if (Sdt.Count > 0)
                                {
                                    CardNO = Sdt[0].CardNO;
                                    CPH = Sdt[0].CPH;
                                }
                            }
                            if (CardNO == "")
                            {
                                CardNO = Data.Substring(8, 8);
                            }
                        }
                        CardType = gsd.GetCardType(CardNO);
                        if (CardType == "")
                        {
                            if (Data.Substring(0, 2) == "E8")
                            {
                                //判断下位机临时车是否为上位机发行为月卡 2016-03-31
                                List<CardIssue> Dzds = gsd.SelectFxCPH(CPH, 6, "Mth", "Fre", "Str");
                                if (Dzds.Count > 0)
                                {
                                    CardNO = Dzds[0].CardNO;
                                    CardType = Dzds[0].CarCardType;
                                    CPH = Dzds[0].CPH;
                                    gsd.UpdateICFaXingDate(CardNO);
                                }
                                else
                                {
                                    CardType = "TmpA";
                                }
                            }
                            else
                            {
                                CardType = "MthA";
                            }
                        }
                    }
                    else
                    {
                        CardNO = Data.Substring(8, 8);
                        CardType = gsd.GetCardType(CardNO);
                    }

                    //入场时间
                    DateTime dtInTime = DateTime.Now;

                    string inYear = DateTime.Now.Year.ToString().Substring(0, 3) + Data.Substring(18, 1);
                    if (Convert.ToInt32(inYear) > DateTime.Now.Year)
                    {
                        inYear = (Convert.ToInt32(inYear) - 10).ToString();
                    }
                    string inMouth = Convert.ToInt32(Data.Substring(19, 1), 16).ToString();
                    string strIntime = inYear + "-" + inMouth + "-" + Data.Substring(20, 2) + " " + Data.Substring(22, 2) + ":" + Data.Substring(24, 2) + ":" + Data.Substring(26, 2);

                    string OutYear = DateTime.Now.Year.ToString().Substring(0, 3) + Data.Substring(28, 1);
                    if (Convert.ToInt32(OutYear) > DateTime.Now.Year)
                    {
                        OutYear = (Convert.ToInt32(OutYear) - 10).ToString();
                    }
                    string OutMouth = Convert.ToInt32(Data.Substring(29, 1), 16).ToString();
                    string strOuttime = OutYear + "-" + OutMouth + "-" + Data.Substring(30, 2) + " " + Data.Substring(32, 2) + ":" + Data.Substring(34, 2) + ":" + Data.Substring(36, 2);
                    DateTime dtOutTime = DateTime.Now;

                    if (CR.IsTime(strOuttime))
                    {
                        dtOutTime = Convert.ToDateTime(strOuttime);
                    }
                    else
                    {
                        dtOutTime = DateTime.Now;
                    }

                    if (CR.IsTime(strIntime))
                    {
                        dtInTime = Convert.ToDateTime(strIntime);
                    }
                    else
                    {
                        dtInTime = dtOutTime;
                    }

                    //余额
                    int Balance = 0;
                    if (CardType.Substring(0, 3) == "Str" && Data.Substring(0, 1) == "C")
                    {
                        Balance = Convert.ToInt32(Data.Substring(38, 4));
                    }
                    //收费金额
                    decimal SFJE = Convert.ToInt32(Data.Substring(42, 4), 16);
                    if (CardType == "")
                    {
                        CardType = "TmpA";
                    }

                    //上位机修正为月卡后把收费金额修改为0 2016-03-31
                    if (CardType.Substring(0, 3) == "Mth" || CardType.Substring(0, 3) == "Fre")
                    {
                        SFJE = 0;
                    }

                    bool bOffLineOut = true;
                    if (gsd.GetOffLineOut(CPH, dtInTime) > 0)
                    {
                        bOffLineOut = false;
                        //flag = "1";
                    }
                    //MessageBox.Show("出入口："+flag);
                    if (Model.Channels[iIntindex].iInOut == 0 && CardType.Substring(0, 3) != "Opt")
                    {
                        gsd.DeleteInOutCPH(CPH, Model.Channels[iIntindex].iBigSmall);

                        CarIn model = new CarIn();
                        model.CardNO = CardNO;
                        model.CPH = CPH;
                        model.CardType = CardType;
                        model.InTime = dtInTime;
                        model.OutTime = dtOutTime;
                        model.InGateName = Model.Channels[iIntindex].sInOutName + "脱机";
                        model.InOperator = Model.sUserName;
                        model.InOperatorCard = Model.sUserCard;
                        model.OutOperatorCard = "";
                        model.OutOperator = "";
                        model.SFJE = SFJE;
                        model.SFTime = DateTime.Now;
                        model.OvertimeSFTime = DateTime.Now;
                        //数据表中不存在该字段
                        //model.InOut = Model.Channels[iIntindex].iInOut;
                        model.BigSmall = Model.Channels[iIntindex].iBigSmall;


                        if (model.CPH.Length > 6 && !bOffLineOut)
                        {
                            gsd.UpdateIn(model);
                        }
                        else
                        {
                            gsd.AddAdmission(model, 10);
                        }
                    }
                    else
                    {
                        if (CardType.Contains("Str"))
                        {
                            gsd.UpdateICYU(CardNO, Balance);
                        }
                        CarOut model = new CarOut();
                        model.CardNO = CardNO;
                        model.CPH = CPH;
                        model.CardType = CardType;
                        model.InTime = dtInTime;
                        model.OutTime = dtOutTime;
                        model.OutGateName = Model.Channels[iIntindex].sInOutName + "脱机";
                        model.OutOperator = Model.sUserName;
                        model.OutOperatorCard = Model.sUserCard;

                        if (model.CardType.Substring(0, 3) == "Tmp" || model.CardType.Substring(0, 3) == "Str" || (model.CardType.Substring(0, 3) == "Mth" && Model.bMonthFdSf == false && Model.iYKOverTimeCharge == 1))
                        {
                            if (model.CardType.Substring(0, 3) == "Tmp")
                            {
                                if (Model.iXsd == 0)
                                {
                                    if (Model.iChargeType == 3)
                                    {
                                        if (Model.iXsdNum == 1)
                                        {
                                            model.SFJE = SFJE / 10;
                                        }
                                        else
                                        {
                                            model.SFJE = SFJE / 100;
                                        }
                                    }
                                    else
                                    {
                                        model.SFJE = SFJE;
                                    }
                                }
                                else
                                {
                                    model.SFJE = SFJE / 10;
                                }
                            }
                            else
                            {
                                if (Model.iXsd == 0)
                                {
                                    if (Model.iChargeType == 3)
                                    {
                                        if (Model.iXsdNum == 1)
                                        {
                                            model.SFJE = SFJE / 10;
                                        }
                                        else
                                        {
                                            model.SFJE = SFJE / 100;
                                        }
                                    }
                                    else
                                    {
                                        model.SFJE = SFJE;
                                    }
                                }
                                else
                                {
                                    model.SFJE = SFJE / 10;
                                }
                            }
                        }
                        else
                        {
                            model.SFJE = 0;
                        }

                        model.Balance = Balance;
                        model.SFTime = dtOutTime;
                        model.OvertimeSFTime = DateTime.Now;
                        //model.InOut = Model.Channels[iIntindex].iInOut;
                        model.BigSmall = Model.Channels[iIntindex].iBigSmall;

                        if (model.CPH.Length > 6 && gsd.UpdateInOut(model) > 0)
                        {

                        }
                        else
                        {
                            gsd.DeleteInOutCPH(CPH, Model.Channels[iIntindex].iBigSmall, model.InTime);
                            //!!!
                            gsd.AddOutName(model, 10);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                gsd.AddLog("读取记录" + ":ParkingReadRecord", ex.Message + "\r\n" + ex.StackTrace + Data);
            }
        }

        private void GateState(string mStr, int Iint)
        {

        }

        private void CkjState(string mStr, int Iint)
        {

        }

        /// <summary>
        /// 写入场记录
        /// </summary>
        private void JjcgetWriteStore()
        {
            try
            {
                CarIn ci = new CarIn();
                ci.CardNO = cardNoNo[modulus];
                ci.CPH = carNoNo[modulus];
                ci.CardType = cardType[modulus];
                ci.InTime = Convert.ToDateTime(inTime[modulus].ToString("yyyy-MM-dd HH:mm:ss"));
                sTmp = "";
                if (bDateTimeErr[0])
                {
                    sTmp = "inErr";
                }
                ci.OutTime = DateTime.Now;
                ci.InGateName = inOutName[modulus] + sTmp;
                ci.InOperator = Model.sUserName;
                ci.InOperatorCard = Model.sUserCard;
                ci.OutOperatorCard = "";
                ci.OutOperator = "";
                ci.SFJE = 0;
                //ci.SFTime = DateTime.Now;
                //ci.OvertimeSFTime = DateTime.Now;

                ci.Balance = monitor.Balance;

                if (System.IO.File.Exists(inOutPic[modulus]))
                {
                    ci.InPic = inOutPic[modulus];
                }
                else
                {
                    ci.InPic = "";
                }


                ci.BigSmall = Model.Channels[modulus].iBigSmall;
                ci.InUser = strCarNoColor[modulus];
                ci.CarparkNO = Model.iParkingNo;
                ci.StationID = Model.stationID;

                //2016-09-29
                if ((cardType[modulus].Substring(0, 3) == "Tmp" && Model.iInAutoOpenModel == 1) || (cardType[modulus].Substring(0, 3) == "Mth" && Model.iInMothOpenModel == 1))
                {

                }
                else
                {
                    if (inOutPic[modulus] != "")
                    {
                        ci.InPic = gsd.UpLoadPic(inOutPic[modulus]);
                    }
                }
                //2016-09-29

                if (Model.iMorePaingCar == 1)
                {
                    if (ci.CardType.Substring(0, 3) == "Mth" && gsd.GetComeCount(monitor.PersonNo, carNoNo[modulus]) > gsd.GetPersonCount(monitor.PersonNo))
                    {
                        ci.SFOperatorCard = "123456";
                        string strsLoad = CR.GetChineseCPH(carNoNo[modulus]) + "9EADAC";
                        sender0.LoadLsNoX2010znykt(modulus, strsLoad);

                        ShowLoadAlert("车位占用，临时车，请通行");
                    }
                    else
                    {
                        ci.SFOperatorCard = "";
                    }
                }
                else
                {
                    ci.SFOperatorCard = "";
                }

                int ret = gsd.AddAdmission(ci, 20);

                if (Model.iTempDown == 0)
                {
                    if (cardType[modulus].Substring(0, 3) == "Tmp" && ci.CPH != "" && ci.CPH != "66666666" && ci.CPH != "00000000" && ci.CPH != "88888888")
                    {
                        gsd.AddTemp(ci.CPH, ci.InTime);
                    }
                }
                //2016-09-08

                GetBinInOut();


            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":JjcgetWriteStore", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 写出场记录
        /// </summary>
        private void JJcUpdateStore()
        {
            CarOut co = new CarOut();
            string outsTmp = "";
            if (bDateTimeErr[1])
            {
                outsTmp = outsTmp + "outErr";
            }
            co.CardNO = cardNoNo[modulus];
            co.CPH = carNoNo[modulus];
            co.CardType = cardType[modulus];
            co.InTime = monitor.InTime;
            co.OutTime = Convert.ToDateTime(monitor.OutTime == DateTime.MinValue ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : monitor.OutTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //co.InTime = Convert.ToDateTime(lblInTime.Content.ToString());
            //co.OutTime = Convert.ToDateTime((lblOutTime.Content.ToString() == "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(lblOutTime.Content).ToString("yyyy-MM-dd HH:mm:ss")));
            co.OutGateName = inOutName[modulus] + outsTmp;
            co.OutOperator = Model.sUserName;
            co.OutOperatorCard = Model.sUserCard;
            co.SFJE = monitor.Charge;
            //co.SFJE = Convert.ToDecimal(lblCharge.Content);
            co.Balance = Convert.ToDecimal(storeBalance[modulus]);
            co.SFTime = co.OutTime;
            co.OvertimeSFTime = DateTime.Now;
            if (System.IO.File.Exists(inOutPic[modulus]))
            {
                co.OutPic = inOutPic[modulus];
            }
            else
            {
                co.OutPic = "";
            }
            co.BigSmall = Model.Channels[modulus].iBigSmall;
            co.OutUser = strCarNoColor[modulus];
            co.YSJE = monitor.AmountReceivable;
            co.FreeReason = "";
            co.CarparkNO = Model.iParkingNo;
            co.StationID = Model.stationID;

            //2016-09-29
            if ((cardType[modulus].Substring(0, 3) == "Tmp" && Model.iOutAutoOpenModel == 1 && co.SFJE > 0) || (cardType[modulus].Substring(0, 3) == "Mth" && Model.iOutMothOpenModel == 1))
            {

            }
            else
            {
                if (inOutPic[modulus] != "")
                {
                    co.OutPic = gsd.UpLoadPic(inOutPic[modulus]);
                }
            }


            lOutID[modulus] = gsd.AddOutName(co, 20);

            if (cardType[modulus].Substring(0, 3) == "Tmp" && co.CPH != "" && co.CPH != "66666666" && co.CPH != "00000000" && co.CPH != "88888888")
            {
                gsd.UpdateTempInOut(co.CPH, 1);
            }

            GetBinInOut();
        }

        void ClearAll()
        {
            monitor = new MonitorModel();

            lblPersonNo.Content = "";
            lblPersonName.Content = "";
            lblDeptName.Content = "";
            lblCardNo.Content = "";
            lblCardType.Content = "";
            lblInTime.Content = "";
            lblOutTime.Content = "";
            lblCharge.Content = "";
            lblBalance.Content = "";
            lblValid.Content = "剩余金额:";
        }

        #region 原始记录
        /// <summary>
        /// 保存下位机记录
        /// </summary>
        /// <param name="strTmp"></param>
        /// <param name="strNo"></param>
        /// <param name="InName"></param>
        private void WriteTemp1(string strTmp, string strNo, string InName)
        {
            try
            {
                string str1 = "";
                string str2 = "";

                string ICIDNO = "";

                if (strTmp.Substring(0, 1) == "C")
                {
                    ICIDNO = Convert.ToInt32(strTmp.Substring(12, 4), 16).ToString("0000");
                }
                else
                {
                    ICIDNO = strTmp.Substring(8, 8);
                }

                if (strTmp.Length > 65)
                {
                    strTmp = strTmp.Substring(0, 46) + "0" + strTmp.Substring(strTmp.Length - 19, 19);
                }
                if (strTmp.Substring(16, 2) == "DD")
                {
                    strTmp = strTmp.Substring(0, 47) + "8888838" + strTmp.Substring(strTmp.Length - 12, 12);
                }

                str1 = strTmp + " " + Model.strRevision.Substring(0, 6); //20111214

                if (bOutTalkTemp)
                {
                    str2 = "计费器";
                }
                else
                {
                    str2 = "";
                }

                RawRecord model = new RawRecord();
                model.InOperatorCard = Model.sUserCard;
                model.OutOperatorCard = strNo;
                model.InOperator = Model.sUserName;
                model.CardType = "REC";
                model.CardNO = ICIDNO;
                model.InTime = DateTime.Now;
                model.OutTime = DateTime.Now;
                model.InGateName = InName;
                model.OutGateName = str2;
                model.FreeReason = str1;
                model.OutOperator = "";
                model.InPic = "";
                model.InUser = "";
                model.OutPic = "";
                model.OutUser = "";
                model.ZJPic = "";
                model.SFTime = DateTime.Now;
                model.OvertimeSFTime = DateTime.Now;
                gsd.AddRecordMemory(model);
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":WriteTemp1", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void WriteTemp(string strTmp)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                string[] strTmpX = new string[4];
                string ssInDateTime = "";
                string ssOutDateTime = "";
                long lMoney;

                string ICIDNO = "";

                if (strTmp.Substring(0, 1) == "C")
                {
                    ICIDNO = Convert.ToInt32(strTmp.Substring(12, 4), 16).ToString("0000");
                }
                else
                {
                    ICIDNO = strTmp.Substring(8, 8);
                }

                if (strTmp.Substring(16, 2) == "DD")
                {
                    strTmp = strTmp.Substring(0, 47) + "8888838" + strTmp.Substring(strTmp.Length - 12, 12);
                }
                strTmpX[0] = "2011-11-30 22:00:00";  //20111214
                strTmpX[1] = "2011-11-30 22:00:00";//  '20111214
                strTmpX[2] = DateTime.Now.ToString();
                strTmpX[3] = "REC";
                lMoney = 0;

                if (Model.iDetailLog == 1)
                {
                    lMoney = Convert.ToInt32(strTmp.Substring(42, 4), 16);

                    string inYear = DateTime.Now.Year.ToString().Substring(0, 3) + strTmp.Substring(18, 1);
                    if (Convert.ToInt32(inYear) > DateTime.Now.Year)
                    {
                        inYear = (Convert.ToInt32(inYear) - 10).ToString();
                    }
                    string inMouth = Convert.ToInt32(strTmp.Substring(19, 1), 16).ToString();
                    ssInDateTime = inYear + "-" + inMouth + "-" + strTmp.Substring(20, 2) + " " + strTmp.Substring(22, 2) + ":" + strTmp.Substring(24, 2) + ":" + strTmp.Substring(26, 2);
                    if (CR.IsTime(ssInDateTime))
                    {
                        strTmpX[0] = ssInDateTime;
                    }

                    string OutYear = DateTime.Now.Year.ToString().Substring(0, 3) + strTmp.Substring(28, 1);
                    if (Convert.ToInt32(OutYear) > DateTime.Now.Year)
                    {
                        OutYear = (Convert.ToInt32(OutYear) - 10).ToString();
                    }
                    string OutMouth = Convert.ToInt32(strTmp.Substring(29, 1), 16).ToString();
                    ssOutDateTime = OutYear + "-" + OutMouth + "-" + strTmp.Substring(30, 2) + " " + strTmp.Substring(32, 2) + ":" + strTmp.Substring(34, 2) + ":" + strTmp.Substring(36, 2);
                    if (CR.IsTime(ssOutDateTime))
                    {
                        strTmpX[1] = ssOutDateTime;
                    }
                }
                else
                {
                    strTmpX[0] = DateTime.Now.ToString();
                    strTmpX[1] = DateTime.Now.ToString();
                }
                str1 = strTmp + " " + Model.strRevision.Substring(13); //20111214

                if (bOutTalkTemp)
                {
                    str2 = "计费器";
                }
                else
                {
                    str2 = "";
                }
                RawRecord model = new RawRecord();
                model.InOperatorCard = Model.sUserCard;
                model.InOperator = Model.sUserName;
                model.OutOperator = "";
                model.OutOperatorCard = "";
                model.InPic = "";
                model.InUser = "";
                model.OutPic = "";
                model.OutUser = "";
                model.ZJPic = "";
                model.SFTime = DateTime.Now;
                model.CardType = "REC";
                model.CardNO = ICIDNO;
                model.InTime = Convert.ToDateTime(strTmpX[0]);
                model.OutTime = Convert.ToDateTime(strTmpX[1]);
                model.OvertimeSFTime = Convert.ToDateTime(strTmpX[2]);
                model.SFJE = lMoney;
                model.InGateName = Model.Channels[modulus].sInOutName;
                model.OutGateName = str2;
                model.FreeReason = str1;
                model.CPH = myCarNo[modulus] == null ? "" : myCarNo[modulus];

                gsd.AddRecordMemory(model);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                gsd.AddLog("在线监控" + ":WriteTemp", ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 保存历史记录
        /// </summary>
        /// <param name="strTmp"></param>
        private void WriteTemp(string strCardNO, string strCardType, DateTime dtInTime, DateTime dtOutTime, decimal dSFJE, decimal dYSJE, string strTmp)
        {
            try
            {
                string str1 = "";
                str1 = strTmp + " " + Model.strRevision.Substring(13); //20111214

                RawRecord model = new RawRecord();
                model.InOperatorCard = Model.sUserCard;
                model.InOperator = Model.sUserName;
                model.OutOperator = "";
                model.OutOperatorCard = "";
                model.InPic = "";
                model.InUser = "";
                model.OutPic = "";
                model.OutUser = "";
                model.ZJPic = "";
                model.SFTime = DateTime.Now;
                model.CardType = strCardType;
                model.CardNO = strCardNO;
                model.InTime = dtInTime;
                model.OutTime = dtOutTime;
                model.OvertimeSFTime = DateTime.Now;
                model.SFJE = dSFJE;
                model.YSJE = dYSJE;
                model.InGateName = Model.Channels[modulus].sInOutName;
                model.OutGateName = "";
                model.FreeReason = str1;
                model.BigSmall = Model.Channels[modulus].iBigSmall;
                model.CarparkNO = Model.iParkingNo;
                model.CPH = myCarNo[modulus] == null ? "" : myCarNo[modulus];

                gsd.AddRecordMemory(model);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                gsd.AddLog("在线监控" + ":WriteTemp", ex.Message + ex.StackTrace);
            }
        }
        #endregion


        DispatcherTimer dTimerVisible;

        private void ShowLoadAlert(string alertstr)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                    {
                        lblInfo.Visibility = Visibility.Visible;

                        //Canvas parent = lblInfo.Parent as Canvas;
                        //var maxZ = parent.Children.OfType<UIElement>().Where(x => x != lblInfo).Select(x => Canvas.GetZIndex(x)).Max();
                        //Canvas.SetZIndex(lblInfo, maxZ + 1);

                        txtInfo.Text = alertstr;

                        Storyboard myStoryboard = new Storyboard();
                        DoubleAnimation OpacityDoubleAnimation = new DoubleAnimation();
                        OpacityDoubleAnimation.From = 0;
                        OpacityDoubleAnimation.To = 1;
                        OpacityDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));
                        Storyboard.SetTargetName(OpacityDoubleAnimation, lblInfo.Name);
                        Storyboard.SetTargetProperty(OpacityDoubleAnimation, new PropertyPath(DataGrid.OpacityProperty));
                        lblInfo.RenderTransform = new TranslateTransform();
                        DependencyProperty[] propertyChain = new DependencyProperty[]
                        {
                             DataGrid.RenderTransformProperty,
                             TranslateTransform.XProperty
                        };

                        DoubleAnimation InDoubleAnimation = new DoubleAnimation();
                        InDoubleAnimation.From = -lblInfo.Width;
                        InDoubleAnimation.To = 0;
                        InDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));
                        Storyboard.SetTargetName(InDoubleAnimation, lblInfo.Name);
                        Storyboard.SetTargetProperty(InDoubleAnimation, new PropertyPath("(0).(1)", propertyChain));
                        myStoryboard.Children.Add(OpacityDoubleAnimation);
                        myStoryboard.Children.Add(InDoubleAnimation);

                        InDoubleAnimation.Completed += (c, com) =>
                        {
                            dTimerVisible = new DispatcherTimer();
                            dTimerVisible.Tick += new EventHandler(dTimerVisible_Tick);
                            dTimerVisible.Interval = new TimeSpan(0, 0, Model.iPromptDelayed);
                            dTimerVisible.Start();
                        };

                        myStoryboard.Begin(lblInfo);
                    }));
        }

        void dTimerVisible_Tick(object sender, EventArgs e)
        {
            dTimerVisible.Stop();
            Storyboard myStoryboard = new Storyboard();
            DoubleAnimation OpacityDoubleAnimation = new DoubleAnimation();
            OpacityDoubleAnimation.From = 0;
            OpacityDoubleAnimation.To = 1;
            OpacityDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            Storyboard.SetTargetName(OpacityDoubleAnimation, lblInfo.Name);
            Storyboard.SetTargetProperty(OpacityDoubleAnimation, new PropertyPath(DataGrid.OpacityProperty));
            lblInfo.RenderTransform = new TranslateTransform();
            DependencyProperty[] propertyChain = new DependencyProperty[]
                        {
                             DataGrid.RenderTransformProperty,
                             TranslateTransform.XProperty
                        };
            DoubleAnimation InDoubleAnimation = new DoubleAnimation();
            InDoubleAnimation.From = 0;
            InDoubleAnimation.To = -lblInfo.Width;
            InDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            Storyboard.SetTargetName(InDoubleAnimation, lblInfo.Name);
            Storyboard.SetTargetProperty(InDoubleAnimation, new PropertyPath("(0).(1)", propertyChain));
            myStoryboard.Children.Add(OpacityDoubleAnimation);
            myStoryboard.Children.Add(InDoubleAnimation);

            InDoubleAnimation.Completed += (c, com) =>
            {
                lblInfo.Visibility = Visibility.Collapsed;
            };

       
            myStoryboard.Begin(lblInfo);
        }

        #endregion

        #region Timer3Get
        public void Timer3Get()
        {
            while (true)
            {
                if (bExit == true)
                {
                    bThreadReadTimer3ExitOK = true;
                    //ThreadRead.Join();
                    fThreadtimer3.Abort();

                    return;
                };
                Thread.Sleep(800);

                RunTimer3();

                //if (!Dispatcher.CheckAccess())
                //{
                //    Dispatcher.Invoke(DispatcherPriority.Send, new ReadCardHandler(RunTimer3));
                //} 

            }
        }

        int iCartCount = 0;
        int DecCount = 0;
        int iDownloadTmp = 0;
        int timeCount = 0;

        private void RunTimer3()
        {
            try
            {
                iCartCount++;
                if (iCartCount > 60)
                {
                    GetBinInOut();
                    iCartCount = 0;
                }
                DecCount++;
                if (Model.Quit_Flag == false)
                {
                    return;
                }
                if (Model.OverCharge_Flag == true)
                {
                    return;
                }

                if (DecCount > 120)
                {
                    if (Model.bZhuCe)
                    {
                        if (CR.IsTime(Model.strTmpInTime))
                        {
                            DateTime dtStart = Convert.ToDateTime("2014-06-27 00:00:00");
                            DateTime dtEnd = Convert.ToDateTime(Model.strTmpInTime);
                            if (DateTime.Now > dtEnd)
                            {
                                fThread.Abort();
                                fThreadtimer3.Abort();
                                MessageBox.Show("系统数据溢出 DH ！", "提示");
                                System.Windows.Forms.Application.ExitThread();
                            }
                        }
                    }
                    DecCount = 0;
                    //DogDetection();
                }

                lostFlag++;
                iDownloadTmp++;
                timeCount++;
                if (iDownloadTmp > 20)
                {
                    iDownloadTmp = 0;
                    if (Model.strKZJ == "1" && Model.bIsKZB && Model.iTempDown == 0)
                    {
                        TempDownLoad();//下载脱机临时车
                    }
                }

                if (lostFlag > 20)
                {
                    loadCar();
                    UpdateTxbText(txbOperatorInfo, "");
                    lostFlag = 0;
                }

                //延时时间省略

                if (timeCount == Model.iLoadTimeInterval * 60 || Model.iLoadTimeType == 0)
                {
                    if (Model.strKZJ == "1" && Model.bIsKZB)
                    {
                        //脱机车牌下载
                        DownCPHCard();//下载车牌号

                        FindDeleCPHCard();//下载退卡车牌号

                        DownBlacklistCard();//下载黑名单车牌号

                        DownDBlacklistCard();//下载删除黑名单车牌号
                    }

                    if (CR.DateDiff(CR.DateInterval.Minute, loadTime, DateTime.Now) > Model.iLoadTimeInterval)
                    {
                        CR.SetSysTime(gsd.GetSysTime());
                        AutoAddDate();
                        loadTime = DateTime.Now;
                    }
                    Model.iLoadTimeType = 1;
                    timeCount = 0;
                }
            }
            catch (Exception ex)
            {
                UpdateTxbText(txbOperatorInfo, ex.Message + "RunTimer3");

                gsd.AddLog("在线监控" + ":RunTimer3", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        #region 脱机车牌下载
        /// <summary>
        /// 黑名单下载
        /// </summary>
        private void DownDBlacklistCard()
        {
            List<Blacklist> Downds = gsd.GetBlacklistDCPHDownLoad(Model.stationID);
            int k = 0;


            foreach (var dr in Downds)
            {
                k++;
                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = dr.DownloadSignal;
                string sumBiao = "";
                string sumTBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;

                sumTBiao = str1 + "0" + str2;
                //progressBar1.Value += progressBar1.Step;

                int iAddDelete = dr.AddDelete;
                //替换成车道
                //Dictionary<int, string> dic = CR.GetIP();
                //dic[1] = "192.168.1.99";
                //dic[3] = "192.168.1.98";

                SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                string QstrJiHao = "";
                string strCJiHao = "";
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    QstrJiHao = QstrJiHao + "1";

                    if (iAddDelete == 0)
                    {
                        string a = "";
                        if (Model.Channels[j].iXieYi == 1)
                        {
                            //sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                            a = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownBlacklistCPH(dr), Model.Channels[j].iXieYi);
                        }
                        else
                        {
                            a = "0";
                            //                             short Ji = Convert.ToInt16(Model.Channels[j].iCtrlID);
                            //                             string strsend = CR.GetDownBlacklistCPH(dr);
                            //                             a = axznykt_1.LoadDinnerTime2010znykt_(ref Ji, ref strsend);
                        }

                        if (a == "0")
                        {
                            strCJiHao = strCJiHao + "1";
                            //Ibll.UpdateDownLoad(CarNO, sumBiao);
                        }
                    }
                    else
                    {
                        string a = "";
                        if (Model.Channels[j].iXieYi == 1)
                        {
                            // sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                            a = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownBlacklistCPH(dr), Model.Channels[j].iXieYi);
                        }

                    }
                    //Ibll.UpdateDownLoad(CarNO, sumBiao);

                }
                if (QstrJiHao == strCJiHao)
                {
                    //gsd.UpdateBlacklistDownLoad(dr["CPH"].ToString(), sumTBiao);
                    gsd.UpdateBlacklistDownLoad(dr.ID, sumTBiao);
                    if (iAddDelete == 1 && sumTBiao == "000000000000000")
                    {
                        gsd.DeleteMYBlacklist(dr.ID);
                    }
                }
                //else
                //{
                //    Ibll.UpdateICLost(CarNO, 1, strTemp1, 2);
                //}
            }
        }

        /// <summary>
        /// 黑名单下载
        /// </summary>
        private void DownBlacklistCard()
        {
            List<Blacklist> Downds = gsd.GetBlacklistCPHDownLoad(Model.stationID);
            int k = 0;


            foreach (var dr in Downds)
            {
                k++;
                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = dr.DownloadSignal;
                string sumBiao = "";
                string sumTBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;

                sumTBiao = str1 + "0" + str2;
                //progressBar1.Value += progressBar1.Step;

                int iAddDelete = dr.AddDelete;
                //替换成车道
                //Dictionary<int, string> dic = CR.GetIP();
                //dic[1] = "192.168.1.99";
                //dic[3] = "192.168.1.98";

                SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                string QstrJiHao = "";
                string strCJiHao = "";
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    QstrJiHao = QstrJiHao + "1";

                    if (iAddDelete == 0)
                    {
                        string a = "";
                        if (Model.Channels[j].iXieYi == 1)
                        {
                            // sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                            a = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownBlacklistCPH(dr), Model.Channels[j].iXieYi);
                        }
                        else
                        {
                            a = "0";
                            //                             short Ji = Convert.ToInt16(Model.Channels[j].iCtrlID);
                            //                             string strsend = CR.GetDownBlacklistCPH(dr);
                            //                             a = axznykt_1.LoadDinnerTime2010znykt_(ref Ji, ref strsend);
                        }

                        if (a == "0")
                        {
                            strCJiHao = strCJiHao + "1";
                            //Ibll.UpdateDownLoad(CarNO, sumBiao);
                        }
                    }
                    else
                    {
                        string a = "";
                        if (Model.Channels[j].iXieYi == 1)
                        {
                            //sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                            a = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownBlacklistCPH(dr), Model.Channels[j].iXieYi);
                        }
                        else
                        {
                            a = "0";
                            //                             short Ji = Convert.ToInt16(Model.Channels[j].iCtrlID);
                            //                             string strsend = CR.GetDownBlacklistCPH(dr);
                            //                             a = axznykt_1.DeleteDinnerTime2010znykt_(ref Ji, ref strsend);
                        }

                        if (a == "0")
                        {
                            strCJiHao = strCJiHao + "1";
                        }
                    }
                    //Ibll.UpdateDownLoad(CarNO, sumBiao);

                }
                if (QstrJiHao == strCJiHao)
                {
                    //gsd.UpdateBlacklistDownLoad(dr["CPH"].ToString(), sumBiao);

                    gsd.UpdateBlacklistDownLoad(dr.ID, sumTBiao);
                    if (iAddDelete == 1 && sumTBiao == "000000000000000")
                    {
                        gsd.DeleteMYBlacklist(dr.ID);
                    }
                }
                //else
                //{
                //    Ibll.UpdateICLost(CarNO, 1, strTemp1, 2);
                //}
            }
        }
        /// <summary>
        /// 有效卡号下载
        /// </summary>
        private void DownCPHCard()
        {
            List<CardIssue> Downds = gsd.GetFaXingCPHDownLoad(Model.stationID);
            int k = 0;


            foreach (var dr in Downds)
            {
                k++;
                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = dr.CPHDownloadSignal;   //2015-11-24
                string sumBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;
                //progressBar1.Value += progressBar1.Step;
                string CarNO = dr.CardNO;
                string strJiHao = dr.CarValidMachine;

                string strJiHaoSum = "";
                foreach (char a in strJiHao.ToCharArray())
                {
                    strJiHaoSum += ConvertToBin(a);
                }

                string strYDownLoadJihao = "";//下载机号
                for (int i = 0; i < 128; i++)
                {
                    strYDownLoadJihao += "0";
                }
                string strSDownLoadJihao = "";//下载成功
                for (int i = 0; i < 128; i++)
                {
                    strSDownLoadJihao += "0";
                }
                //替换成车道
                //Dictionary<int, string> dic = CR.GetIP();
                //dic[1] = "192.168.1.99";
                //dic[3] = "192.168.1.98";

                SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                string QstrJiHao = "";
                string strCJiHao = "";
                if (dr.CPH != "" && dr.CPH != "66666666" && dr.CPH != "88888888" && dr.CPH.Length > 6)
                {
                    for (int j = 0; j < Model.iChannelCount; j++)
                    {
                        if (strJiHaoSum.Substring(Model.Channels[j].iCtrlID - 1, 1) == "0")//判断这张卡片是否发行这个车道
                        {
                            strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[j].iCtrlID);
                            strYDownLoadJihao = strCJiHao;
                            string a = "";
                            if (Model.Channels[j].iXieYi == 1)
                            {
                                a = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoadToCPH(dr), Model.Channels[j].iXieYi);
                            }
                            else
                            {
                                a = "0";
                                //                                 short Ji = Convert.ToInt16(Model.Channels[j].iCtrlID);
                                //                                 string strsend = CR.GetDownLoadToCPH(dr);
                                //                                 a = axznykt_1.LoadDinnerTime2010znykt_(ref Ji, ref strsend);
                            }

                            if (a == "0")
                            {
                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                strSDownLoadJihao = QstrJiHao;
                                //Ibll.UpdateDownLoad(CarNO, sumBiao);
                            }
                            //Ibll.UpdateDownLoad(CarNO, sumBiao);
                        }
                        else
                        {
                            string a = "";
                            if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                            {
                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                a = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoadToCPH(dr), Model.Channels[j].iXieYi);
                            }
                            else
                            {
                                a = "0";
                                //                                 short Ji = Convert.ToInt16(Model.Channels[j].iCtrlID);
                                //                                 string strsend = CR.GetDownLoadToCPH(dr);
                                //                                 a = axznykt_1.DeleteDinnerTime2010znykt_(ref Ji, ref strsend);
                            }

                            if (a == "0")
                            {
                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                strSDownLoadJihao = QstrJiHao;
                            }


                        }
                    }
                    //string strJihao = "";
                    //for (int j = 0; j < QstrJiHao.Length / 4; j++)
                    //{
                    //    strJihao += string.Format("{0:X}", Convert.ToInt32(QstrJiHao.Substring(j * 4, 4), 2));
                    //}
                    //string strTemp1 = strJihao;
                    if (QstrJiHao == strCJiHao)
                    {
                        // gsd.UpdateCPHDownLoad(CarNO, sumBiao);

                        gsd.UpdateBlacklistDownLoad(dr.ID, sumBiao);
                    }
                }
                else
                {
                    //gsd.UpdateCPHDownLoad(CarNO, sumBiao);

                    gsd.UpdateBlacklistDownLoad(dr.ID, sumBiao);
                }
                //else
                //{
                //    Ibll.UpdateICLost(CarNO, 1, strTemp1, 2);
                //}
            }
        }

        /// <summary>
        /// 有效卡号下载
        /// </summary>
        private void DownCard()
        {
            List<CardIssue> Downds = gsd.GetFaXingDownLoad(Model.iICCardDownLoad, Model.stationID);

            int k = 0;


            foreach (var dr in Downds)
            {
                k++;
                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = dr.DownloadSignal;
                string sumBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;
                //progressBar1.Value += progressBar1.Step;
                string CarNO = dr.CardNO;
                string strJiHao = dr.CarValidMachine;

                string strJiHaoSum = "";
                foreach (char a in strJiHao.ToCharArray())
                {
                    strJiHaoSum += ConvertToBin(a);
                }

                string strYDownLoadJihao = "";//下载机号
                for (int i = 0; i < 128; i++)
                {
                    strYDownLoadJihao += "0";
                }
                string strSDownLoadJihao = "";//下载成功
                for (int i = 0; i < 128; i++)
                {
                    strSDownLoadJihao += "0";
                }


                SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                string QstrJiHao = "";
                string strCJiHao = "";
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[j].iCtrlID);
                    strYDownLoadJihao = strCJiHao;
                    if (strJiHaoSum.Substring(Model.Channels[j].iCtrlID - 1, 1) == "0")//判断这张卡片是否发行这个车道
                    {
                        string a = "";
                        if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                        {
                            sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                            a = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                        }


                        if (a == "0")
                        {
                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                            strSDownLoadJihao = QstrJiHao;
                            //Ibll.UpdateDownLoad(CarNO, sumBiao);
                        }
                        //Ibll.UpdateDownLoad(CarNO, sumBiao);
                    }
                    else
                    {
                        string a = "";
                        if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                        {
                            sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                            a = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                        }


                        if (a == "0")
                        {
                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                            strSDownLoadJihao = QstrJiHao;
                        }


                    }
                }
                //string strJihao = "";
                //for (int j = 0; j < QstrJiHao.Length / 4; j++)
                //{
                //    strJihao += string.Format("{0:X}", Convert.ToInt32(QstrJiHao.Substring(j * 4, 4), 2));
                //}
                //string strTemp1 = strJihao;
                if (QstrJiHao == strCJiHao)
                {
                    //gsd.UpdateDownLoad(CarNO, sumBiao);
                    gsd.UpdateBlacklistDownLoad(dr.ID, sumBiao);
                }
                //else
                //{
                //    Ibll.UpdateICLost(CarNO, 1, strTemp1, 2);
                //}
            }
        }

        void TempDownLoad()
        {
            try
            {

                gsd.GetDeleteTemp();//统计容量

                //删除下位机卡号
                List<AutoTempDownLoad> DowndsOut = gsd.GetTempDownLoad(Model.stationID, 1, 1);

                foreach (var dr in DowndsOut)
                {
                    int biaozhi = Model.stationID;//需要替换的标志位
                    string strbiaozhi = dr.DownloadSignal;
                    string strCPH = dr.CPH;

                    //                 if (strbiaozhi == "000000000000000")
                    //                 {
                    //                     Ibll.DeleteTemp(strCPH);
                    //                 }

                    string sumBiao = "";
                    string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                    string str2 = strbiaozhi.Substring(biaozhi);
                    sumBiao = str1 + "0" + str2;

                    string strSum = "";
                    string strDownSum = "";
                    SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                    for (int j = 0; j < Model.iChannelCount; j++)
                    {
                        if (Model.Channels[j].iInOut == 1)//判断这张卡片是否发行这个车道
                        {
                            strSum += "1";
                            string a = "";
                            if (Model.Channels[j].iXieYi == 1)
                            {
                                a = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoadToTempCPH(dr), Model.Channels[j].iXieYi);
                            }


                            if (a == "0")
                            {
                                strDownSum += "1";
                                //Ibll.UpdateDownLoad(CarNO, sumBiao);
                            }
                        }
                    }
                    if (strSum == strDownSum)
                    {
                        if (sumBiao == "000000000000000")
                        {
                            gsd.DeleteTemp(strCPH);
                        }
                        else
                        {
                            gsd.UpdateTempDownLoad(strCPH, sumBiao);
                        }
                    }

                }
                //加载临时卡 卡号
                List<AutoTempDownLoad> DowndsIn = gsd.GetTempDownLoad(Model.stationID, 0, 0);

                foreach (var dr in DowndsIn)
                {
                    int biaozhi = Model.stationID;//需要替换的标志位
                    string strbiaozhi = dr.DownloadSignal;
                    string sumBiao = "";
                    string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                    string str2 = strbiaozhi.Substring(biaozhi);
                    sumBiao = str1 + "1" + str2;
                    string strCPH = dr.CPH;


                    string strSum = "";
                    string strDownSum = "";
                    SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                    for (int j = 0; j < Model.iChannelCount; j++)
                    {
                        if (Model.Channels[j].iInOut == 1)//判断这张卡片是否发行这个车道
                        {
                            strSum += "1";
                            string a = "";
                            if (Model.Channels[j].iXieYi == 1)
                            {
                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                a = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoadToTempCPH(dr), Model.Channels[j].iXieYi);
                            }
                            else
                            {
                                a = "0";
                            }

                            if (a == "0")
                            {
                                strDownSum += "1";
                                //Ibll.UpdateDownLoad(CarNO, sumBiao);
                            }
                            //Ibll.UpdateDownLoad(CarNO, sumBiao);
                        }
                    }
                    if (strSum == strDownSum)
                    {
                        gsd.UpdateTempDownLoad(strCPH, sumBiao);
                    }
                }
            }
            catch (System.Exception ex)
            {
                UpdateTxbText(txbOperatorInfo, ex.Message + "(TempDownLoad)");
                gsd.AddLog("在线监控" + ":TempDownLoad", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private string ConvertToBin(char c)
        {
            switch (c)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'A':
                case 'a':
                    return "1010";
                case 'B':
                case 'b':
                    return "1011";
                case 'C':
                case 'c':
                    return "1100";
                case 'D':
                case 'd':
                    return "1101";
                case 'E':
                case 'e':
                    return "1110";
                case 'F':
                case 'f':
                    return "1111";
                default:
                    throw new Exception();
            }
        }

        private string GetJihao(string sum, int JiHao)
        {
            string str1 = sum.Substring(0, Convert.ToInt32(JiHao) - 1); //ab
            string str2 = sum.Substring(Convert.ToInt32(JiHao)); //d
            sum = str1 + "1" + str2; //abmd
            return sum;
        }

        /// <summary>
        /// 退卡
        /// </summary>
        private void FindDeleCPHCard()
        {
            SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
            List<CardIssue> ds = gsd.GetOutCPHCard(Model.stationID);
            //label1.Text = "挂失待挂失的卡号下载";
            int Count = ds.Count;
            int k = 0;
            foreach (var dr in ds)
            {
                k++;
                //progressBar1.Value += progressBar1.Step;
                string CarNO = dr.CardNO;


                List<CardIssue> Fds = gsd.GetFaXing(CarNO);
                var Fdr = Fds[0];

                string strJiHao = Fdr.CarValidMachine;

                string strsum = "";
                foreach (char a in strJiHao.ToCharArray())
                {
                    strsum += ConvertToBin(a);
                }
                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = dr.DownloadSignal;
                string sumBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;
                //替换成车道
                // Dictionary<int, string> dic = CR.GetIP();
                if (dr.CPH != "" && dr.CPH != "66666666" && dr.CPH != "88888888")
                {
                    string Rststr = "";
                    for (int j = 0; j < Model.iChannelCount; j++)
                    {
                        for (int i = 0; i < strsum.Length; i++)
                        {
                            if (strsum.Substring(i, 1) == "0")
                            {
                                if ((int)i + 1 == Model.Channels[j].iCtrlID)
                                {
                                    string count = "";
                                    if (Model.Channels[j].iXieYi == 1)
                                    {
                                        //sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                        count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoadToCPH(dr), Model.Channels[j].iXieYi);
                                    }
                                    else
                                    {
                                        count = "0";
                                        //                                         short Ji = Convert.ToInt16(Model.Channels[j].iCtrlID);
                                        //                                         string strsend = CR.GetDownLoadToCPH(dr);
                                        //                                         count = axznykt_1.DeleteDinnerTime2010znykt_(ref Ji, ref strsend);
                                    }
                                    if (count == "0")
                                    {
                                        Rststr = Rststr + "0";
                                    }
                                }

                            }
                        }
                    }
                    string StrCount = "";
                    for (int y = 0; y < Model.iChannelCount; y++)
                    {
                        StrCount = StrCount + "0";
                    }
                    if (StrCount == Rststr)
                    {
                        //gsd.UpdateCPHDownLoad(CarNO, sumBiao);

                        gsd.UpdateBlacklistDownLoad(dr.ID, sumBiao);
                        //Ibll.UpdateIDLost(CarNO, 0);
                    }
                }
                else
                {
                    //gsd.UpdateCPHDownLoad(CarNO, sumBiao);

                    gsd.UpdateBlacklistDownLoad(dr.ID, sumBiao);
                }
            }
        }

        #endregion


        #region 校准主板时间
        /// <summary>
        /// 时间加载
        /// </summary>
        private void AutoAddDate()
        {
            for (int y = 0; y < Model.iChannelCount; y++)
            {
                //加载ZNYKTY5摄像机时间
                if (bVZTime)
                {
                    if (m_hLPRClient[y] > 0)
                    {
                        DateTime dtNow = DateTime.Now;
                        ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_DATE_TIME_INFO timrParam = new ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_DATE_TIME_INFO();
                        timrParam.uYear = (uint)dtNow.Year;
                        timrParam.uMonth = (uint)dtNow.Month;
                        timrParam.uMDay = (uint)dtNow.Day;
                        timrParam.uHour = (uint)dtNow.Hour;
                        timrParam.uMin = (uint)dtNow.Minute;
                        timrParam.uSec = (uint)dtNow.Second;

                        int size = Marshal.SizeOf(timrParam);
                        IntPtr intptr = Marshal.AllocHGlobal(size);
                        Marshal.StructureToPtr(timrParam, intptr, true);
                        int temp = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_SetDateTime(m_hLPRClient[y], intptr);
                        Marshal.FreeHGlobal(intptr);
                    }
                }
                sender0.LoadTime(Model.Channels[y].iCtrlID, Model.Channels[y].sIP, m_nSerialHandle[y], Model.Channels[y].iXieYi);
            }
        }
        #endregion


        #region 狗检测
        //void DogDetection()
        //{
        //    bool bStart = false;
        //    short iTmp = 0;
        //    string strTmp = "";

        //    cpi.tcc2 cpiX = new cpi.tcc2();

        //    if (Model.iAutoPlateEn == 1)
        //    {
        //        if (Model.iAutoPattern == 0)
        //        {
        //            bStart = cpiX.MinAutoStr(Convert.ToInt16(Model.iRsbYsb), 2, iTmp, strTmp);
        //        }
        //        else if (Model.iAutoPattern == 1)
        //        {
        //            bStart = cpiX.MinAutoStr(Convert.ToInt16(Model.iRsbYsb), Convert.ToInt16(Model.iAutoPlateType), iTmp, strTmp);
        //        }
        //        else if (Model.iAutoPattern == 2)
        //        {
        //            bStart = cpiX.MinAutoStr(1, Convert.ToInt16(Model.iAutoPlateType), iTmp, strTmp);

        //            //2016-06-15 th 修改新加密狗程序
        //            if (!bStart)
        //            {
        //                string strX = "";
        //                cpiX.InitData();

        //                short i0 = 0;
        //                string strPassWord = "3Y8Er0Qc72dF9YT4";
        //                short i1 = 2;
        //                string strRst = cpiX.OpenSfirm(ref i0, ref strPassWord, ref i1, ref iTmp, ref strX);
        //                if (strRst == "ok")
        //                {
        //                    string[] strListTmp = new string[10];
        //                    strRst = cpiX.ReadSfirm(0, "dPr0v3OEALs5j7KC", ref strListTmp[0], ref strListTmp[1], ref strListTmp[2], ref strListTmp[3], ref strListTmp[4], ref strListTmp[5], ref strListTmp[6], ref strListTmp[7], ref strListTmp[8]);
        //                    if (strRst == "ok")
        //                    {
        //                        bStart = true;
        //                    }
        //                }
        //            }

        //        }
        //        if (!bStart)
        //        {
        //            readkeyfailcount++;
        //            if (readkeyfailcount > 10)    //2015-10-16 连续10次检测狗失败，才退出
        //            {
        //                fThread.Abort();
        //                fThreadtimer3.Abort();
        //                MessageBox.Show("加密狗错误！", "提示");
        //                System.Windows.Forms.Application.ExitThread();
        //            }
        //        }
        //        else
        //        {
        //            readkeyfailcount = 0;
        //        }
        //    }
        //    else
        //    {
        //        bStart = cpiX.Txt2Str(Convert.ToInt16(Model.iDogType), ref iTmp, ref iTmp, Model.bDogTypeEn, Convert.ToInt16(Model.iType), Convert.ToInt16(Model.iSoftType));
        //        if (!bStart)
        //        {
        //            readkeyfailcount++;
        //            if (readkeyfailcount > 10)
        //            {
        //                fThread.Abort();
        //                fThreadtimer3.Abort();
        //                MessageBox.Show("加密狗错误！", "提示");
        //                //Application.ExitThread();
        //            }
        //        }
        //        else
        //        {
        //            readkeyfailcount = 0;
        //        }
        //    }
        //}
        #endregion


        #region 统计车位信息
        bool bRStopIn = true;
        private void loadCar()
        {
            try
            {
                ParkingInfo pi = new ParkingInfo();
                pi = gsd.GetParkingInfo(DateTime.Now.ToString("yyyyMMdd000000"), Model.iParkingNo.ToString());
                //lblMthCount.Content = pi.MonthCarCountInPark;
                //lblTmpCount.Content = pi.TempCarCountInPark;
                //lblFreCount.Content = pi.FreeCarCountInPark;
                //lblStrCount.Content = pi.PrepaidCarCountInPark;

                summary0.MthCount = pi.MonthCarCountInPark;
                summary0.TmpCount = pi.TempCarCountInPark;
                summary0.FreCount = pi.FreeCarCountInPark;
                summary0.StrCount = pi.PrepaidCarCountInPark;

                outCarCount = 0;
                if (Model.bPaiChe)
                {
                    summary0.OutCount = outCarCount;

                    UpdateUiText(lblOutCount, summary0.OutCount.ToString());
                    //lblOutCount.Content = outCarCount;
                }
                if (Model.iFreeCardNoInPlace == 1)
                {
                    summary0.SurplusCarCount = (Model.iParkTotalSpaces - pi.TotalCarCountInPark + pi.FreeCarCountInPark);
                    //免费车不计入车位数 （iModifyCarPos没用到）
                    //summary.SurplusCarCount = (Model.iParkTotalSpaces - pi.TotalCarCountInPark + pi.FreeCarCountInPark).ToString();
                }
                else
                {
                    summary0.SurplusCarCount = Model.iParkTotalSpaces - pi.TotalCarCountInPark;
                    //summary.SurplusCarCount = (Model.iParkTotalSpaces - pi.TotalCarCountInPark).ToString();
                }

                if (Model.bTempCarPlace)
                {
                    summary0.SurplusCarCount = Model.iTempCarPlaceNum - pi.TempCarCountInPark;
                    summary0.OutCount = Model.iMonthCarPlaceNum - pi.MonthCarCountInPark;
                    //summary.SurplusCarCount = (Model.iTempCarPlaceNum - pi.TempCarCountInPark).ToString();
                    //lblOutCount.Content = (Model.iMonthCarPlaceNum - pi.MonthCarCountInPark).ToString();

                    UpdateUiText(grpSurplusCarCount, "临时车位");
                    UpdateUiVisibility(lblOutCount, Visibility.Visible);
                    UpdateUiVisibility(lblOut, Visibility.Visible);
                    UpdateUiText(lblOut, "固定车位:");
                    UpdateUiText(lblOutCount, summary0.OutCount.ToString());
                    //grpSurplusCarCount.Header = "临时车位";
                    //lblOutCount.Visibility = Visibility.Visible;
                    //lblOut.Content = "固定车位:";
                }
                else if (Model.bMonthCarPlace)
                {
                    summary0.SurplusCarCount = Model.iMonthCarPlaceNum - pi.MonthCarCountInPark;
                    summary0.OutCount = Model.iMonthCarPlaceNum - pi.MonthCarCountInPark;

                    //summary.SurplusCarCount = (Model.iMonthCarPlaceNum - pi.MonthCarCountInPark).ToString();
                    //lblOutCount.Content = (Model.iTempCarPlaceNum - pi.TempCarCountInPark).ToString();

                    UpdateUiText(grpSurplusCarCount, "固定车位");
                    UpdateUiVisibility(lblOutCount, Visibility.Visible);
                    UpdateUiVisibility(lblOut, Visibility.Visible);
                    UpdateUiText(lblOut, "临时车位:");
                    UpdateUiText(lblOutCount, summary0.OutCount.ToString());
                    //grpSurplusCarCount.Header = "固定车位：";
                    //lblOutCount.Visibility = Visibility.Visible;
                    //lblOut.Content = "临时车位：";
                }
                else if (Model.bMoneyCarPlace)
                {
                    summary0.SurplusCarCount = Model.iMoneyCarPlaceNum - pi.PrepaidCarCountInPark;
                    summary0.OutCount = Model.iMonthCarPlaceNum - pi.MonthCarCountInPark;

                    //summary.SurplusCarCount = (Model.iMoneyCarPlaceNum - pi.PrepaidCarCountInPark).ToString();
                    //lblOutCount.Content = (Model.iMonthCarPlaceNum - pi.MonthCarCountInPark).ToString();

                    UpdateUiText(grpSurplusCarCount, "储值车位");
                    UpdateUiVisibility(lblOutCount, Visibility.Visible);
                    UpdateUiVisibility(lblOut, Visibility.Visible);
                    UpdateUiText(lblOut, "固定车位:");
                    UpdateUiText(lblOutCount, summary0.OutCount.ToString());
                    //grpSurplusCarCount.Header = "储值车位：";
                    //lblOutCount.Visibility = Visibility.Visible;
                    //lblOut.Content = "固定车位：";
                }

                //UpdateUiVisibility(lblOut, Visibility.Visible);
                //lblOut.Visibility = Visibility.Visible;


                if (Model.iFullLight > 0)
                {
                    for (int x = 0; x < Model.iChannelCount; x++)
                    {
                        if (Model.Channels[x].iInOut == 0)
                        {
                            if (summary0.SurplusCarCount > 0)
                            {
                                if (forbidIn[x] || bStopInInit[x] || bLoadFullCW[x])
                                {
                                    sender0.VoiceDisplay(VoiceType.RelieveMonthlyParkingFull, x);//释放满位信号

                                    sender0.VoiceDisplay(VoiceType.RelieveTemporaryParkingFull, x);//释放满位信号

                                    sender0.VoiceDisplay(VoiceType.RelieveStoredValueParkingFull, x);//释放满位信号

                                   // sender0.VoiceDisplay(VoiceType.RelieveMonthlyParkingFull, x);//释放满位信号

                                    sender0.VoiceDisplay(VoiceType.RelieveParkingFull, x);//释放满位信号

                                    forbidIn[x] = false;
                                    bStopInInit[x] = false;
                                    bLoadFullCW[x] = false;
                                }

                            }
                            else
                            {
                                if (forbidIn[x] == false || bLoadFullCW[x])
                                {
                                    if (Model.iFullLight == 1)
                                    {
                                        sender0.VoiceDisplay( VoiceType.MonthlyParkingFull,x);
                                    }
                                    else if (Model.iFullLight == 2)
                                    {
                                        sender0.VoiceDisplay( VoiceType.TemporaryParkingFull,x);
                                    }
                                    else if (Model.iFullLight == 3)
                                    {
                                        sender0.VoiceDisplay( VoiceType.StoredValueParkingFull,x);
                                    }
                                    else if (Model.iFullLight == 5)
                                    {
                                        sender0.VoiceDisplay( VoiceType.ParkingFull,x);
                                    }
                                
                                    
                                    forbidIn[x] = true;
                                    bLoadFullCW[x] = false;
                                }

                            }
                        }

                    }

                }
                else
                {
                    if (bRStopIn)
                    {

                        for (int x = 0; x < Model.iChannelCount; x++)
                        {
                            if (Model.Channels[x].iInOut == 0)
                            {
                                sender0.VoiceDisplay(VoiceType.Relieve, x);
                            }
                        }
                        bRStopIn = false;
                    }
                }


                if (Model.iSumMoneyHide == 1)
                {
                    UpdateUiVisibility(lblMoneyCount, Visibility.Hidden);
                    UpdateUiVisibility(lblSumMoney, Visibility.Hidden);
                    UpdateUiVisibility(lblUnitCount, Visibility.Hidden);
                    //lblMoneyCount.Visibility = Visibility.Hidden;
                    //lblSumMoney.Visibility = Visibility.Hidden;
                }
                else
                {
                    UpdateUiVisibility(lblMoneyCount, Visibility.Visible);
                    UpdateUiVisibility(lblSumMoney, Visibility.Visible);
                    UpdateUiVisibility(lblUnitCount, Visibility.Visible);
                    summary0.MoneyCount = pi.TotalCharge;

                    //lblMoneyCount.Visibility = Visibility.Visible;
                    //lblSumMoney.Visibility = Visibility.Visible;
                    //monitor.MoneyCount = pi.TotalCharge;
                }

                if (lostFlag != 0)
                {
                    //剩余车位显示屏   后面在处理
                    if (Model.iCtrlShowRemainPos == 1 || Model.iCtrlShowInfo == 1)
                    {
                        //iDispCount67 = iDispCount67 + 1;
                        iCtrlLedDelay = iCtrlLedDelay - 1;
                        //iDispCount67 = 0;
                        iCtrlLedDelay = -1;
                        string strLoad = "";
                        string strShowInfo = "";
                        if (Model.iCtrlShowInfo == 1)
                        {
                            strShowInfo = CR.GetAppConfig("fbxx");
                            if (strShowInfo == null)
                            {
                                strShowInfo = "欢迎光临";
                            }
                            //strShowInfo = "欢迎光临";
                        }
                        if (Model.iCtrlShowRemainPos == 1)
                        {
                            if (strLoad != "")
                            {
                                strLoad += "  ";
                            }
                            if (summary0.SurplusCarCount < 1)
                            {
                                strLoad += "车位已满 谢谢！";
                            }
                            else
                            {
                                strLoad += "剩余车位:" + summary0.SurplusCarCount.ToString();
                                //strLoad += "剩余车位:" + 995;
                            }

                        }
                        string strShowInfoZ = string.Empty;
                        byte[] array = null;
                        if (Model.iCtrlShowInfo == 1)
                        {
                            array = System.Text.Encoding.Default.GetBytes(strShowInfo);
                            //strShowInfoZ = string.Empty;
                            if (array != null)
                            {
                                for (int i = 0; i < array.Length; i++)
                                {
                                    strShowInfoZ += array[i].ToString("X2");
                                }
                            }
                        }
                        byte[] array1 = System.Text.Encoding.Default.GetBytes(strLoad);
                        string str = string.Empty;
                        if (array1 != null)
                        {
                            for (int i = 0; i < array1.Length; i++)
                            {
                                str += array1[i].ToString("X2");
                            }
                        }
                        if (Model.iCtrlShowInfo == 1)
                        {
                            str = "01" + strShowInfoZ + "02" + str;
                        }

                        for (int x = 0; x < Model.iChannelCount; x++)
                        {
                            if (Model.Channels[x].iOnLine == 0)
                            {
                                continue;
                            }


                            if (Model.Channels[x].iInOut == 0)
                            {
                                byte byteCmdX;
                                int iMacNO = Model.Channels[x].iCtrlID;
                                SedBll VsendBll = new SedBll(Model.Channels[x].sIP, 1007, 1005);
                                if (iMacNO > 127)
                                {
                                    iMacNO = iMacNO - 127;
                                    byteCmdX = 0x45;
                                }
                                else
                                {
                                    byteCmdX = 0x3D;
                                }
                                if (Model.Channels[x].iXieYi == 1)
                                {
                                    string rtnStr = VsendBll.CtrlLedShow(Convert.ToByte(iMacNO), byteCmdX, 0x67, str, Model.Channels[x].iXieYi);
                                }


                            }
                            else if (Model.iCtrlShowInfo == 1 && Model.Channels[x].iInOut == 1)
                            {
                                byte byteCmdX;
                                int iMacNO = Model.Channels[x].iCtrlID;
                                SedBll VsendBll = new SedBll(Model.Channels[x].sIP, 1007, 1005);
                                if (iMacNO > 127)
                                {
                                    iMacNO = iMacNO - 127;
                                    byteCmdX = 0x45;
                                }
                                else
                                {
                                    byteCmdX = 0x3D;
                                }
                                if (Model.Channels[x].iXieYi == 1)
                                {
                                    string rtnStr = VsendBll.CtrlLedShow(Convert.ToByte(iMacNO), byteCmdX, 0x67, strShowInfoZ, Model.Channels[x].iXieYi);
                                }

                            }
                        }
                    }
                    //月卡3 临时卡4 免费卡5 储值卡6 总车辆数7 收费总数8 开闸数9 免费金额10

                    summary0.OpenCount = pi.ManualOpenCarCount;
                    summary0.FreMoney = pi.TotalDiscount;

                    //lblOpenCount.Content = pi.ManualOpenCarCount;
                    //lblFreMoney.Content = pi.TotalDiscount;

                    for (int x = 0; x < Model.iChannelCount; x++)
                    {
                        List<LedSetting> dtS = gsd.GetSurplusCar(Model.Channels[x].iCtrlID);
                        //string CPH = lblCarNo.Content.ToString();

                        if (dtS.Count == 0 || dtS == null)
                        {
                            break;
                        }

                        string CPH = monitor.CarNo;

                        foreach (var dr in dtS)
                        {
                            string showWay = dr.ShowWay;
                            string SendSum = "";
                            string StrSum = "";
                            bool bMW = false;//2016-09-08 th
                            if (showWay.Contains("3"))//是否含有空车位
                            {
                                if ((summary0.SurplusCarCount.ToString() == "" ? 0 : summary0.SurplusCarCount) < 1)
                                {
                                    if (dr.Pattern == "2")
                                    {
                                        StrSum = "0000";
                                        bMW = false;
                                    }
                                    else
                                    {
                                        StrSum = "车位已满 谢谢";
                                        bMW = true;
                                    }

                                }
                                else
                                {
                                    bMW = false;
                                    if (dr.CPHEndStr == "")
                                    {
                                        if (dr.Pattern == "2")
                                        {
                                            StrSum = (Convert.ToInt32(summary0.SurplusCarCount)).ToString("0000");
                                            // StrSum = (Convert.ToInt32(lblRemainCar.Text) - 1).ToString("0000");
                                        }
                                        else if (dr.Pattern == "8")
                                        {
                                            StrSum = "剩余车位:" + (Convert.ToInt32(summary0.SurplusCarCount)).ToString("000");
                                        }
                                        else
                                        {
                                            StrSum = "空车位:" + (Convert.ToInt32(summary0.SurplusCarCount)).ToString("000");
                                        }
                                        //StrSum = "空车位:" + (Convert.ToInt32(lblRemainCar.Text)).ToString("000");
                                    }
                                    else
                                    {
                                        StrSum = dr.CPHEndStr + (Convert.ToInt32(summary0.SurplusCarCount)).ToString("000");
                                    }
                                }

                            }
                            if (StrSum != "")
                            {
                                string Jstrs = "";

                                if (bMW)
                                {
                                    Jstrs = "01" + dr.Speed + "00" + dr.Color + dr.SumTime + CR.GetStrTo16(StrSum);//移动方式： 速度,单幅停留时间,颜色,总显示时间
                                }
                                else
                                {
                                    Jstrs = dr.Move + dr.Speed + dr.StopTime + dr.Color + dr.SumTime + CR.GetStrTo16(StrSum);//移动方式： 速度,单幅停留时间,颜色,总显示时间
                                }
                                
                                int sum = 0;

                                byte[] array = CR.GetByteArray(Jstrs);
                                foreach (byte by in array)
                                {
                                    sum += by;
                                }
                                sum = sum % 256;
                                //Thread.Sleep(300);

                                SendSum = "CC" + Convert.ToInt32(dr.SurplusID).ToString("X2") + "BB5154" + sum.ToString("X2") + Jstrs + "FF";
                                SedBll senbll = new SedBll(Model.Channels[x].sIP, 1007, 1005);

                                if (Model.Channels[x].iXieYi == 1)
                                {
                                    string strRetun = senbll.SurplusCtrlLedShow(Convert.ToByte(Model.Channels[x].iCtrlID), SendSum, Model.Channels[x].iXieYi);
                                }
                            }
                        }
                    }
                }


                //实时刷新

                //UpdateTxbText(txbSurplusCarCount, summary.SurplusCarCount.ToString());
                //UpdateUiText(lblMthCount, monitor.MthCount.ToString());
                //UpdateUiText(lblTmpCount, monitor.TmpCount.ToString());
                //UpdateUiText(lblStrCount, monitor.StrCount.ToString());
                //UpdateUiText(lblFreCount, monitor.FreCount.ToString());
                //UpdateUiText(lblFreMoney, monitor.FreMoney.ToString());
                //UpdateUiText(lblOpenCount, monitor.OpenCount.ToString());

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                    {
                        lblMthCount.Content = summary0.MthCount;
                        lblTmpCount.Content = summary0.TmpCount;
                        lblStrCount.Content = summary0.StrCount;
                        lblFreCount.Content = summary0.FreCount;
                        lblFreMoney.Content = summary0.FreMoney.ToString("0.00");
                        lblMoneyCount.Content = summary0.MoneyCount.ToString("0.00");
                        lblOpenCount.Content = summary0.OpenCount;
                        lblOutCount.Content = summary0.OpenCount;
                        txbSurplusCarCount.Text = summary0.SurplusCarCount.ToString();
                    }));
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":loadCar", ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        #endregion

        #endregion
        #endregion


        #region 手动输入车牌
        private void DZNOShiBie(int idzIint)
        {
            try
            {
                picFileName = "";

                int KXD1 = Model.iKXD;

                txbOperatorInfo.Foreground = new SolidColorBrush(Colors.Black);
                txbOperatorInfo.Text = "";

                bReadPicAuto[idzIint] = false;

                strReadPicFile[idzIint] = "";
                strReadPicFileJpg[idzIint] = "";
                ParkingPlateNoInput carNoInput = new ParkingPlateNoInput(new NoCPHHandler(NoCPH), Model.Channels[idzIint].iInOut);
                carNoInput.Owner = this;
                carNoInput.ShowDialog();

                gsd.AddLog("手动输入", "未识别车牌，手工输入" + strNoCarNo + " 车道名称：" + Model.Channels[idzIint].sInOutName);

                ImageSavePath(idzIint, 0);//生成图片路劲
                Mycaptureconvert(idzIint, 0);//图像抓拍
                Model.iAutoColor = 0;

                Quene.ModelNode model = new Quene.ModelNode();
                model.sDzScan = "";
                model.iDzIndex = idzIint;
                model.strFile = picFileName;
                model.strFileJpg = filesJpg;
                model.strCPH = strNoCarNo;
                LQueue.Enqueue(model);// 放到

                strNoCarNo = "";
                picFileName = "";
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控", "DZNOShiBie" + ex.Message);
                txbOperatorInfo.Text = ex.Message;
            }
        }

        /// <summary>
        /// 临时卡收费弹出窗口
        /// </summary>
        private void TempCarGo()
        {
            CaclMoneyResult d = null;
            try
            {
                if (Model.iChargeType == 3 && Model.iXsdNum == 2 && charge[modulus] > 655)
                {
                    d = gsd.GetMONEY(cardType[modulus], monitor.InTime, monitor.OutTime);
                    decimal d1 = d.SFJE;

                    //2016-10-28 客户端计算收费
                    //if (Model.iXsd == 1)
                    //{
                    //    if (d1 > 0)
                    //    {
                    //        d1 = d1 / 10;
                    //    }
                    //}

                    monitor.Charge = d1;
                    monitor.AmountReceivable = d.YSJE;
                    charge[modulus] = Convert.ToDouble(d1);

                    SedBll sendbll = new SedBll(Model.Channels[modulus].sIP, 1007, 1005);
                    string strQIAN = CR.MoneyToChinese(monitor.Charge.ToString());
                    string strsLoad = "73" + CR.GetChineseMoney(strQIAN) + "74";
                    // strsLoad = "01D2";
                    int iLenY = (strsLoad.Length / 2);

                    strsLoad += CR.YHXY(strsLoad).ToString("X2");

                    strsLoad = iLenY.ToString("X2") + strsLoad;
                    // GCR.SedBll sendbll = new GCR.SedBll(Model.Channels[Tmodel.modulus].sIP, 1007, 1005);
                    // string strRst = sendbll.ShowLed(Model.Channels[Tmodel.modulus].iCtrlID, MyTempMoney, Model.Channels[Tmodel.modulus].iXieYi);//开闸
                    string strRst = "";
                    if (Model.Channels[modulus].iXieYi == 1)
                    {
                        strRst = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.Channels[modulus].iCtrlID), 0x3D, 0x72, strsLoad, Model.Channels[modulus].iXieYi);
                        //strRst = sendbll.ShowLed(Model.Channels[Tmodel.modulus].iCtrlID, MyTempMoney, Model.Channels[Tmodel.modulus].iXieYi);
                    }

                    string strY = "收费类型:" + Model.iChargeType + " 卡片类型:" + cardType[modulus] + " 入场时间:" + monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss") + " 出场时间：" + monitor.OutTime.ToString("yyyy-MM-dd HH:mm:ss");
                    gsd.UpdateCORDMEMORY(Convert.ToDecimal(monitor.Charge.ToString() == "" ? "0" : monitor.Charge.ToString()), cardNoNo[modulus], Model.Channels[modulus].sInOutName, 0, strY, monitor.OutTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                //485暂时不处理  客户显示频
                if (Model.iSFLed == 1)
                {
                    if (Model.bDispLY)
                    {
                        Model.TbBaoJia = true;
                    }
                    switch (Model.iSFLed)
                    {
                        case 0:
                            Model.TbBaoJia = true;
                            break;
                        case 1:

                            break;
                    }
                }


                //2016-06-16 中央收费,出口收费，则读取真正的入场时间（入场记录的出场时间）
                string sRealInTime = "";
                if (Model.iCentralCharge == 1 && Model.iOutCharge == 1)
                {
                    List<CarIn> ds = gsd.GetMyRsX(monitor.CardNo, "", Model.iParkingNo, Model.Channels[modulus].iBigSmall);
                    if (ds.Count > 0)
                    {
                        sRealInTime = ds[0].OutTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }


                CphCompare();
                if (Model.iIdSfCancel == 1 && monitor.CardNo.Length == 8)
                {

                }
                else
                {
                    JJcUpdateStore();//写出场库
                }

                CarOut co = new CarOut();
                co.CardNO = monitor.CardNo;
                co.CardType = cardType[modulus];
                co.InTime = monitor.InTime;
                co.OutTime = monitor.OutTime;
                co.CPH = monitor.CarNo;
                co.YSJE = monitor.AmountReceivable;
                co.SFJE = monitor.Charge;

                if (inOutPic[modulus].Contains(Model.sImageSavePath))
                {
                    co.OutPic = inOutPic[modulus].Substring(Model.sImageSavePath.Length);
                }


                co.YHJE = sCouponValue;
                co.YHAddress = sCouponAddr;
                co.YHType = sCouponMode;
                co.ID = lOutID[modulus];
                co.StationID = Model.stationID;
                co.CarparkNO = Model.iParkingNo;

                co.CPColor = iAutoColor[modulus];

                //CarOut model = new CarOut();
                //model.CardNo = lblcard.Text;
                //model.CardType = lblcardModel.Text;
                //model.inDateTime = lblinDateTime.Text;
                //model.outDateTime = lbloutDateTime.Text;
                //model.modulus = modulus;
                //model.Money = lblbuckleMoney.Text;
                //model.CPH = lblCarNo.Text;
                //model.OutPic = MycomeGopicture[modulus];
                //model.iYHMode = iYHMode;   //2016-06-17
                //model.iYHJH = iYHJH;
                //model.sYHAdr = sYHAdr;
                //model.sYHType = sYHType;
                //model.sYHValue = sYHValue;
                //model.inDateTimeReal = sRealInTime;   //2016-06-16

                if (bReadAuto == false && Model.Channels[modulus].iOpenType != 8)
                {
                    bCarNoConfirm = false;
                }
                else if (bReadAuto == false && Model.Channels[modulus].iOpenType == 8 && cardType[modulus].Substring(0, 3) == "Tmp")
                {
                    bCarNoConfirm = false;
                }
                List<string> tempList = new List<string>();
                tempList.Add(bCarNoConfirm.ToString());
                if (myCarNo[modulus] == "")
                {
                    gsd.AddLog("出场弹出确认车牌", "出场弹出确认车牌.车牌号为空！");
                }

                if (CR.GetCardType(monitor.CardType, 0).Substring(0, 3) == "Tmp")
                {
                    tempList.Add(myCarNo[modulus]);
                    tempList.Add(carNoNo[modulus]);

                }
                else
                {
                    tempList.Add(myCarNo[modulus]);
                    if (carNoNo[modulus] == "")
                    {
                        tempList.Add("88888888");
                    }
                    else
                    {
                        tempList.Add(carNoNo[modulus]);
                    }
                }

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            ParkingTempGob_big TempGob = new ParkingTempGob_big(modulus, bCarNoConfirm, co, cacl, new Action<string>(UpdateSFJE), new Action<string>(TempGob_big_Photo), new Action<string, decimal>(BinData));
                            TempGob.Owner = this;
                            TempGob.Show();
                        }));
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":TempCarGo", ex.Message + "\r\n" + ex.StackTrace);
                txbOperatorInfo.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }

        private void Open(int iIntIndex)
        {
            try
            {
                DateTime startTime1 = DateTime.Now;
                string strRetun = sender0.SendOpen(iIntIndex);
                //CR.SendVoice.SendOpen(axznykt_1, Model.Channels[iIntIndex].iCtrlID, Model.Channels[iIntIndex].sIP, 0x0C, 5, m_hLPRClient[iIntIndex], Model.Channels[iIntIndex].iXieYi);//开闸

                DateTime endTime = DateTime.Now;
                TimeSpan ss = endTime - startTime1;
                string straaa = ss.Days + "天" + ss.Hours + "小时" + ss.Minutes + "分" + ss.Seconds + "秒" + ss.Milliseconds + "毫秒";


                DateTime startTime2 = DateTime.Now;
                EnterPat_Photo(iIntIndex);

                DateTime endTime2 = DateTime.Now;
                TimeSpan ss2 = endTime2 - startTime2;
                string straaa2 = ss2.Days + "天" + ss2.Hours + "小时" + ss2.Minutes + "分" + ss2.Seconds + "秒" + ss2.Milliseconds + "毫秒";

                DateTime startTime3 = DateTime.Now;
                //发语音  2015-10-10   
                if (Model.Channels[iIntIndex].iInOut == 0)
                {
                    if (Model.bOut485)
                    {
                        Thread.Sleep(50);
                    }
                    //CR.SendVoice.VoiceLoad(axznykt_1, Model.Channels[iIntIndex].iCtrlID, Model.Channels[iIntIndex].sIP, "42", m_nSerialHandle[iIntIndex], Model.Channels[iIntIndex].iXieYi);
                 
                    sender0.VoiceDisplay(VoiceType.Welcome, iIntIndex);
                }
                else
                {
                    if (Model.bOut485)
                    {
                        Thread.Sleep(50);
                    }
            
                    sender0.VoiceDisplay(VoiceType.TempOutOpen, iIntIndex);
                    //CR.SendVoice.ShowLed55(axznykt_1, Model.Channels[iIntIndex].iCtrlID, Model.Channels[iIntIndex].sIP, m_nSerialHandle[iIntIndex], Model.Channels[iIntIndex].iXieYi);
                }
                DateTime endTime3 = DateTime.Now;
                TimeSpan ss3 = endTime3 - endTime3;
                string straaa3 = ss3.Days + "天" + ss3.Hours + "小时" + ss3.Minutes + "分" + ss3.Seconds + "秒" + ss3.Milliseconds + "毫秒";

                DateTime startTime4 = DateTime.Now;
                CarIn model = new CarIn();
                model.CardNO = "111111";
                model.CardType = "Person";
                model.InTime = DateTime.Now;
                model.CPH = "";
                model.OutTime = DateTime.Now;
                model.SFJE = 0;
                if (Model.Channels[iIntIndex].iInOut == 0)
                {
                    model.InGateName = Model.Channels[iIntIndex].sInOutName;
                    model.OutGateName = "";
                }
                else
                {
                    model.InGateName = "";
                    model.OutGateName = Model.Channels[iIntIndex].sInOutName;
                }

                model.InOperatorCard = "";
                model.InOperator = "";
                model.OutOperatorCard = Model.sUserCard;
                model.OutOperator = Model.sUserName;

                if (Model.Channels[iIntIndex].iInOut == 0)
                {
                    model.InPic = File;
                    model.OutPic = "";
                }
                else
                {
                    model.InPic = "";
                    model.OutPic = File;
                }
                model.InUser = Model.sUserName;
                model.YSJE = 0;
                model.Balance = 0;
                model.BigSmall = Model.Channels[iIntIndex].iBigSmall;
                File = "";
                gsd.AddYiChang(model);

                DateTime endTime4 = DateTime.Now;
                TimeSpan ss4 = endTime4 - startTime4;
                string straaa4 = ss4.Days + "天" + ss4.Hours + "小时" + ss4.Minutes + "分" + ss4.Seconds + "秒" + ss4.Milliseconds + "毫秒";
                //log.Add("Open处理保存时长", straaa4);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Open", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nOpen", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 手动开闸时抓拍图片（委托调用）
        /// </summary>
        /// <param name="Count"></param>
        private void EnterPat_Photo(int Count)
        {
            try
            {
                string Filebmps = "";
                File = "";

                if (Model.sImageSavePath.Length == 0)
                {
                    Model.sImageSavePath = @"C:\";
                }
                if (Model.sImageSavePath.Substring(Model.sImageSavePath.Length - 1) != @"\")
                {
                    Model.sImageSavePath = Model.sImageSavePath + @"\";
                }
                DateTime MyCapDateTime;
                MyCapDateTime = DateTime.Now;
                string PathStr = "";
                PathStr = Model.sImageSavePath + MyCapDateTime.ToString("yyyyMMdd");
                if (System.IO.Directory.Exists(PathStr) == false)
                {
                    System.IO.Directory.CreateDirectory(PathStr);
                }
                int runint = new System.Random().Next(1000);

                Filebmps = PathStr + @"\" + MyCapDateTime.ToString("yyyyMMddHHmmss") + runint.ToString() + ".bmp";
                File = PathStr + @"\" + MyCapDateTime.ToString("yyyyMMddHHmmss") + runint.ToString() + ".jpg";


                //-----------------------------------------注释

                if (Model.iEnableNetVideo == 1)
                {
                    if (Model.iEnableNetVideoType == 0)
                    {
                        for (int chanl = 0; chanl < 4; chanl++)
                        {
                            if (lstLblLaneName[chanl].Content.ToString() == Model.Channels[Count].sInOutName)
                            {
                                Thread.Sleep(100);
                                int han = (int)lstPicVideo[chanl].Handle;
                                //if (VideoSate[chanl] == 0)
                                {
                                    if (strVideoType[chanl] == "ZNYKTY2")
                                    {
                                        //CR.DHClient.DHCapturePicture(pRealPlayHandle[chanl], Filebmps);
                                    }
                                    else if (strVideoType[chanl] == "ZNYKTY3")
                                    {
                                        bool tr1 = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_SetCapturePictureMode(1);
                                        bool tr = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle[chanl], Filebmps);
                                    }
                                    else if (strVideoType[chanl] == "ZNYKTYY4")
                                    {
                                        //bool tr1 = ParkingCommunication.CameraSDK.ZNYKT4.CHCNetSDK.NET_DVR_SetCapturePictureMode(1);
                                        //bool tr = CR.CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle[chanl], Filebmps);
                                    }
                                    else if (strVideoType[chanl] == "ZNYKTY5" || strVideoType[chanl] == "ZNYKTY15")
                                    {
                                        ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_GetSnapShootToJpeg2(m_hLPRPlay[chanl], Filebmps, 100);
                                    }
                                    else if (strVideoType[chanl] == "ZNYKTY10")
                                    {
                                        //CR.DHClient.DHCapturePicture(pRealPlayHandle[chanl], Filebmps);
                                    }
                                    else if (strVideoType[chanl] == "ZNYKTY8")
                                    {
                                        //v_Talk[chanl].CaptureSaveImage(0, Filebmps);
                                    }
                                    else if (strVideoType[chanl] == "ZNYKTY14" || strVideoType[chanl] == "ZNYKTY11")
                                    {
                                        ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_SaveImageToJpeg(nCamId[chanl], Filebmps);
                                    }
                                    else if (strVideoType[chanl] == "ZNYKTY13")
                                    {
                                        IntPtr iJpg = IntPtr.Zero;
                                        int iJpgLen = 0;
                                        ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_CaptureOnePicture(ushort.Parse(chanl.ToString()), out iJpg, out iJpgLen);
                                        DZHSSaveImage(iJpg, Filebmps, iJpgLen);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                    }

                }


                CR.AddShuiYin(Filebmps, File, Model.Channels[Count].sInOutName, "手动开闸");

                //2015-06-19
                if (File.Length > 1)
                {
                    if (System.IO.File.Exists(File))
                    {
                        if (Model.iVideo4 == 1)
                        {
                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(File), 400, 300);
                            if (Count == 0)
                            {
                                ptr4.Image = bm;
                            }
                            else if (Count == 1)
                            {
                                ptr3.Image = bm;
                            }
                        }
                        else
                        {
                            if (Count == 0)
                            {
                                //改
                                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(File), lstPicVideo[3].Width, lstPicVideo[3].Height);
                                lstPicVideo[2].Image = bm;
                            }
                            else if (Count == 1)
                            {
                                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(File), lstPicVideo[3].Width, lstPicVideo[3].Height);
                                lstPicVideo[3].Image = bm;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        #endregion


        #region 自动识别车牌


        /// <summary>
        /// 处理车牌号码
        /// </summary>
        /// <param name="idzIint">车道索引</param>
        /// <param name="strFile">图片名</param>
        /// <param name="strFileJpg">路径</param>
        /// <param name="str1">车牌号</param>
        /// <returns></returns>
        private bool DZChePaiShiBieQ(int idzIint, string strFile, string strFileJpg, string str1)
        {
            try
            {

                ZdtStart = DateTime.Now;


                #region----相同车牌不同车道在多少时间内不处理
                int iCountY = 0;
                if (strRemberCPH.Length == 0)
                {
                    strRemberCPH = str1;
                    timeCPH = DateTime.Now;
                    iRemberInOut = Model.Channels[idzIint].iInOut;
                }
                else
                {
                    if (str1 != "" && str1.Length > 6 && strRemberCPH.Length > 6)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (str1.Substring(i, 1) == strRemberCPH.Substring(i, 1))//判断两个车牌相同位数
                            {
                                iCountY++;
                            }
                        }
                    }
                }
                if (iCountY > 3 && iRemberInOut != Model.Channels[idzIint].iInOut)
                {
                    if (DateTime.Now < timeCPH.AddSeconds(Convert.ToDouble(Model.iSameCphDelay)))
                    {
                        gsd.AddLog("在线监控" + ":DZChePaiShiBieQ", "车牌号:" + str1 + "在" + Model.iSameCphDelay.ToString() + "之内不处理4位以上相同车牌");
                        return false;
                    }
                    else
                    {
                        strRemberCPH = str1;
                        timeCPH = DateTime.Now;
                        iRemberInOut = Model.Channels[idzIint].iInOut;
                    }
                }
                else
                {
                    strRemberCPH = str1;
                    timeCPH = DateTime.Now;
                    iRemberInOut = Model.Channels[idzIint].iInOut;
                }
                #endregion


                picFileName = "";

                if (strFileJpg != "")
                {
                    bReadPicAuto[idzIint] = true;
                    strReadPicFile[idzIint] = strFile;
                    strReadPicFileJpg[idzIint] = strFileJpg;
                }

                if (Model.iDetailLog == 1)
                {
                    CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + str1 + "接收车牌---车道：" + idzIint.ToString());
                }

                #region ---入口无牌车处理
                if (str1.Contains("_无_") || str1 == "无车牌")
                {
                    str1 = "";

                    if (Model.Channels[idzIint].iInOut == 0 && Model.iNoCPHAutoKZ == 1)//入口无牌车是否自动开闸放行
                    {
                        CarIn model = new CarIn();
                        model.CardNO = CR.GetAutoCPHCardNO(Model.Channels[idzIint].iCtrlID);
                        model.CPH = "";
                        model.CardType = "TmpA";
                        model.InTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00"));


                        model.OutTime = DateTime.Now;
                        model.InGateName = Model.Channels[idzIint].sInOutName;
                        model.InOperator = Model.sUserName;
                        model.InOperatorCard = Model.sUserCard;
                        model.OutOperatorCard = "";
                        model.OutOperator = "";
                        model.SFJE = 0;
                        model.SFTime = DateTime.Now;
                        model.OvertimeSFTime = DateTime.Now;

                        //model.InOut = Model.Channels[idzIint].iInOut;
                        model.BigSmall = Model.Channels[idzIint].iBigSmall;
                        model.InUser = "";
                        model.OutUser = "";

                        if (Model.iCarPosLed == 0)
                        {
                            if (!System.IO.File.Exists(filesJpg))
                            {
                                CR.AddShuiYin(strReadPicFile[idzIint], strReadPicFileJpg[idzIint], Model.Channels[idzIint].sInOutName, cardNoNo[idzIint], modulus, lstTxbCarNo[idzIint].Text);
                            }
                        }
                        else
                        {

                        }

                        model.InPic = strReadPicFileJpg[idzIint];

                        try
                        {
                            model.SFOperatorCard = "无牌车";
                            gsd.AddAdmission(model, 20);
                        }
                        catch (System.Exception ex)
                        {
                            gsd.AddLog("在线监控", "无牌车添加记录 AddAdmission" + ex.Message);
                        }

                        sender0.VoiceDisplay(VoiceType.Welcome, idzIint);
                        sender0.SendOpen(idzIint);

                        UpdateTxbText(txbOperatorInfo, "无牌车自动入场");
                        //txbOperatorInfo.Text = "无牌车自动入场";
                        return false;
                    }
                }
                #endregion

                if (str1 != "")
                {
                    str1 = str1.Replace("0", "0");
                    #region ----特殊车牌查询
                    if (Model.bSpecilCPH)
                    {
                        //2016-06-22 全字母车牌不处理
                        if (Model.bCphAllEn)
                        {
                            if (CR.IsCphAllEn(str1) == true)
                            {
                                gsd.AddLog("在线监控", "非法车牌：车牌【" + str1 + "】不处理");
                                return false;
                            }
                        }

                        //2016-06-22 车牌结果字符完全一样不处理
                        if (Model.bCphAllSame)
                        {
                            if (CR.IsCphAllSame(str1) == true)
                            {
                                gsd.AddLog("在线监控", "非法车牌：车牌【" + str1 + "】不处理");
                                return false;
                            }
                        }

                        string tmpCPH = gsd.SearchSpecialCPH(str1);
                        if (tmpCPH != "")
                        {
                            gsd.AddLog("在线监控", "特殊车牌处理：" + str1 + " 处理为 " + tmpCPH);
                            str1 = tmpCPH;
                        }
                    }
                    #endregion

                    List<Blacklist> bl = gsd.SelectBlacklist(str1);
                    if (bl.Count > 0)
                    {
                        //this.Invoke((EventHandler)delegate
                        //{
                        //LblDisplay.Text = "黑名单车辆";
                        ShowLoadAlert(str1 + ":" + bl[0].Reason);
                        // });

                        string strsLoad = "";
                        if (Model.Channels[idzIint].iInOut == 0)
                        {
                            strsLoad = CR.GetChineseCPH(str1) + "D2AF";
                        }
                        else
                        {
                            strsLoad = CR.GetChineseCPH(str1) + "D4AF";
                        }
                        //SendVioce(strsLoad, idzIint);//组合语音发送

                        sender0.LoadLsNoX2010znykt(idzIint, strsLoad);
                        return false;
                    }


                    //string[] listMCPH = new string[] { "粤B485PL", "粤BN45W9", "粤BH60P7", "粤B5YC90" };
                    //foreach (string strM in listMCPH)
                    //{
                    //    if (strM == str1.Substring(0, 7))
                    //    {
                    //        sender0.SendOpen(idzIint);
                    //        //CR.SendVoice.SendOpen(axznykt_1, Model.Channels[idzIint].iCtrlID, Model.Channels[idzIint].sIP, 0x0C, 5, m_hLPRClient[idzIint], Model.Channels[idzIint].iXieYi);//开闸
                    //        return false;
                    //    }
                    //}
                    List<CardIssue> Dzds;

                    //2016-01-06 先对比6位
                    Dzds = gsd.SelectFxCPH(str1, 6, "Mth", "Fre", "Str");     //发行表改为3种卡类一起查
                    if (Dzds.Count == 0 && Model.iAutoPlateDBJD < 2)
                    {
                        Dzds = gsd.SelectFxCPH(str1, 5, "Mth", "Fre", "Str");

                        if (Dzds.Count == 0 && Model.iAutoPlateDBJD < 1)
                        {
                            Dzds = gsd.SelectFxCPH(str1, 4, "Mth", "Fre", "Str");
                        }
                    }

                    if (Dzds.Count > 0)
                    {
                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + str1 + "接收车牌车牌为月租车---车道：" + idzIint.ToString());
                        }
                        bIsMoth[idzIint] = true;
                        string DzCardNo = Dzds[0].CardNO;
                        if (DzCardNo.Length == 5)//兼容以前的IC卡数据 卡号产生是不同的
                        {
                            string strCardNo = Convert.ToInt32(DzCardNo).ToString("X8");
                            sDzScan = "C0000000" + strCardNo + "3F3524174100352417410000000000388888880000DFFF013F";
                        }
                        else
                        {
                            sDzScan = "D1000000" + DzCardNo + "3F3524174100352417410000000000388888880000DFFF013F";
                        }
                        autoCarNo[idzIint] = "";
                        readAutoCarNo = Dzds[0].CPH;
                        myCarNo[idzIint] = readAutoCarNo;
                        bReadAuto = true;
                        Dzds.Clear();
                    }
                    else
                    {
                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "车牌号码：" + str1 + "接收车牌车牌为临时车---车道：" + idzIint.ToString());
                        }
                        Dzds.Clear();
                        if (((Model.iAutoColor == 3 || str1.Substring(6, 1) == "警" || str1.Substring(0, 2) == "WJ") && Model.iAutoColorSet == 1) || (str1.Substring(1, 1) == "0" && Model.iAuto0Set == 1))
                        {
                            sender0.SendOpen(idzIint);

                            //2016-06-13 增加军警车语音播报 th 
                            carNoCmd = str1;
                            if (carNoCmd.Length != 7 || carNoCmd == "0000000" || carNoCmd == "6666666" || carNoCmd == "8888888" || carNoCmd == "京000000")
                            {
                                if (carNoCmd.Length == 8 && carNoCmd.Substring(0, 2) == "WJ")
                                {
                                }
                                else
                                {
                                    carNoCmd = "";
                                }
                            }
                            //CYkDay = "FFFF";
                            strCardCW = "FFFF";
                            monthSurplusDayCmd = "FFFF";

                            if (Model.Channels[idzIint].iInOut == 0)
                            {
                                sender0.VoiceDisplay(VoiceType.InGateVoice, idzIint, "MthA", carNoCmd, 0, "", summary0.SurplusCarCount);
                                //string sLoad = CR.LedSound_New(Model.byteLSXY[Model.iLSIndex, 2], CYkDay, strCardCW, "FFFF", strPlateTalk);
                                //CR.SendVoice.LoadLsNoX2010znykt(axznykt_1, Model.Channels[idzIint].iCtrlID, Model.Channels[idzIint].sIP, 0x3D, Model.byteLSXY[Model.iLSIndex, 2], sLoad, m_nSerialHandle[idzIint], Model.Channels[idzIint].iXieYi);
                            }
                            else
                            {
                                sender0.VoiceDisplay(VoiceType.OutGateVoice, idzIint, "MthA", carNoCmd, 0, "", summary0.SurplusCarCount);
                                //string sLoad = CR.LedSound_New(Model.byteLSXY[Model.iLSIndex, 3], CYkDay, strCardCW, "FFFF", strPlateTalk);
                                //CR.SendVoice.LoadLsNoX2010znykt(axznykt_1, Model.Channels[idzIint].iCtrlID, Model.Channels[idzIint].sIP, 0x3D, Model.byteLSXY[Model.iLSIndex, 3], sLoad, m_nSerialHandle[idzIint], Model.Channels[idzIint].iXieYi);
                            }

                            string strFreFile = "";
                            if (Model.iCarPosLed == 1)
                            {
                                strFreFile = strFileJpg;
                            }
                            else
                            {
                                CR.AddShuiYin(strReadPicFile[idzIint], strFileJpg, Model.Channels[idzIint].sInOutName, CR.GetAutoCPHCardNO(Model.Channels[idzIint].iCtrlID), idzIint, str1);
                                strFreFile = strFileJpg;
                            }

                            gsd.AddYiChang(GetModel(idzIint, str1, strFreFile));
                            strReadPicFile[idzIint] = "";
                            picFileName = "";
                            filesJpg = "";
                            return false;
                        }
                        else
                        {
                            bIsMoth[idzIint] = false;

                            string diqu1 = "京津冀晋蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新港澳台警使武领学";
                            if (str1.Substring(0, 1) == "W" && diqu1.Contains(str1.Substring(2, 1)))
                            {
                                str1 = str1.Substring(0, 8);
                            }
                            else
                            {
                                str1 = str1.Substring(0, 7);
                            }

                            if (Model.iOneKeyShortCut == 0)
                            {
                                if (str1.Length == 7)
                                {
                                    str1 = "临" + str1.Substring(1);
                                }
                                else if (str1.Length == 8 && str1.Substring(0, 2) == "WJ")
                                {
                                    str1 = "临" + str1.Substring(2);
                                }
                            }

                            if (Model.iInAutoOpenModel == 2 || Model.iOutAutoOpenModel == 2)
                            {
                                string strsLoad = "";
                                // strsLoad = "01D2";
                                if (Model.Channels[idzIint].iInOut == 0)
                                {
                                    if (Model.bAutoTemp)
                                    {
                                        strsLoad = "ADB6";
                                    }
                                    else
                                    {
                                        strsLoad = "ADD2";
                                    }
                                }
                                else
                                {
                                    strsLoad = "ADD4";
                                }

                                sender0.LoadLsNoX2010znykt(idzIint, strsLoad);
                                //SendVioce(strsLoad, idzIint);//组合语音发送
                            }


                            string DzCardNo = CR.GetAutoCPHCardNO(Model.Channels[idzIint].iCtrlID);

                            string strReadCPH = "";
                            if (Model.Channels[idzIint].iXieYi == 1 && Model.Channels[idzIint].iInOut == 1)
                            {
                                SedBll sendRead = new SedBll(Model.Channels[idzIint].sIP, 1007, 1005);
                                strReadCPH = sendRead.ReadRecordOffLine(Model.Channels[idzIint].sIP, Model.Channels[idzIint].iCtrlID, Model.Channels[idzIint].iXieYi);
                            }
                            //strReadCPH = "E88005DE041821893F582611220058261123000000000237B46269AA41FFFF827F";
                            //2016-01-06 先按6位找，再依次按选择的精度找
                            List<CarIn> Sdt;
                            Sdt = gsd.SelectComeCPH(str1, 6, "Tmp", "Tmp");
                            if (Sdt.Count == 0 && Model.iAutoPlateDBJD < 2)
                            {
                                Sdt = gsd.SelectComeCPH(str1, 5, "Tmp", "Tmp");
                                if (Sdt.Count == 0 && Model.iAutoPlateDBJD < 1)
                                {
                                    Sdt = gsd.SelectComeCPH(str1, 4, "Tmp", "Tmp");
                                }
                            }
                            if (Sdt.Count > 0)
                            {
                                Model.iAutoCZJL = true;

                                if (strReadCPH.Length > 8 && strReadCPH.Substring(48, 6) == str1.Substring(1, 6))
                                {
                                    string carNOStr = strReadCPH.Substring(46, 8);
                                    int iTmp = Convert.ToInt32(carNOStr.Substring(0, 2), 16);
                                    if (iTmp < 58)
                                    {
                                        carNOStr = CR.GetHexToCPH(carNOStr.Substring(0, 2)) + carNOStr.Substring(2, 6);
                                    }
                                    else
                                    {
                                        carNOStr = Model.strAreaDefault + carNOStr.Substring(2, 6);
                                    }
                                    DateTime dtInTime = Sdt[0].InTime;

                                    string strIntime = dtInTime.Year.ToString().Substring(3, 1) + dtInTime.Month.ToString("X") + dtInTime.Day.ToString("00") + dtInTime.ToString("HHmmss");

                                    bOffLine[idzIint] = true;
                                    str1 = carNOStr;
                                    sDzScan = "D1" + strReadCPH.Substring(2);
                                    string InDzCardNo = Sdt[0].CardNO;
                                    sDzScan = sDzScan.Substring(0, 8) + InDzCardNo + "3F" + strIntime + sDzScan.Substring(28);
                                }
                                else
                                {
                                    string InDzCardNo = Sdt[0].CardNO;
                                    sDzScan = "D1000000" + InDzCardNo + "3F3524174100352417410000000000388888880000DFFF013F";
                                    str1 = Sdt[0].CPH;
                                    bOffLine[idzIint] = false;
                                }

                            }
                            else
                            {
                                if (strReadCPH.Length > 8 && strReadCPH.Substring(48, 6) == str1.Substring(1, 6))
                                {
                                    Model.iAutoCZJL = true;
                                    string carNOStr = strReadCPH.Substring(46, 8);
                                    int iTmp = Convert.ToInt32(carNOStr.Substring(0, 2), 16);
                                    if (iTmp < 58)
                                    {
                                        carNOStr = CR.GetHexToCPH(carNOStr.Substring(0, 2)) + carNOStr.Substring(2, 6);
                                    }
                                    else
                                    {
                                        carNOStr = Model.strAreaDefault + carNOStr.Substring(2, 6);
                                    }
                                    bOffLine[idzIint] = true;
                                    str1 = carNOStr;
                                    sDzScan = "D1" + strReadCPH.Substring(2);
                                }
                                else
                                {
                                    Model.iAutoCZJL = false;
                                    bOffLine[idzIint] = false;
                                    sDzScan = "D1000000" + DzCardNo + "3F3524174100352417410000000000388888880000DFFF013F";
                                }
                            }

                            Sdt.Clear();

                            myCarNo[idzIint] = str1;
                            autoCarNo[idzIint] = myCarNo[idzIint];
                            bReadAuto = true;
                        }
                    }
                    iDzIndex = idzIint;
                    bool bRst = false;
                    if (Model.Channels[idzIint].iOpenType == 7)
                    {
                        bDzBill[idzIint] = true;
                        bRst = true;

                    }
                    else
                    {
                        //this.Invoke((EventHandler)delegate
                        //{
                        //    txtCPHList[idzIint].Text = MyChepaiHao[idzIint];
                        //    bDzBill[idzIint] = false;
                        //    bRst = false;
                        //});
                    }
                    if (Model.Channels[idzIint].iOpenType == 8 && bCardStop[idzIint] && strCardNos[idzIint] != "")
                    {
                        cardNoNo[idzIint] = "";
                        bScondmodus = idzIint;
                        bScondCard[idzIint] = true;
                        bRst = true;
                    }
                    DateTime endTime1 = DateTime.Now;
                    TimeSpan ssqqq = endTime1 - ZdtStart;
                    //label7.Text = "车牌处理时间" + ssqqq.Days + "天" + ssqqq.Hours + "小时" + ssqqq.Minutes + "分" + ssqqq.Seconds + "秒" + ssqqq.Milliseconds + "毫秒";
                    return bRst;
                }
                else
                {
                    //this.Invoke((EventHandler)delegate
                    //{
                    //    LblDisplay.Text = "无车牌";
                    //    MyChepaiHao[idzIint] = "";
                    //});
                    return false;
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控", "DZChePaiShiBie" + ex.Message);
                txbOperatorInfo.Text = ex.Message;
                return false;
            }
        }

        private void TempCarGo_AUTO(int iIndex)
        {
            CaclMoneyResult d = null;

            try
            {
                //string strTmp = "";
                if (Model.iChargeType == 3 && Model.iXsdNum == 2 && charge[modulus] > 655)
                {
                    d = gsd.GetMONEY(cardType[modulus], monitor.InTime, monitor.OutTime);
                    decimal d1 = d.SFJE;
                    monitor.Charge = Convert.ToDecimal(d1.ToString("000.00"));
                    monitor.AmountReceivable = d.YSJE;

                    //lblbuckleMoney.Text = d1.ToString("000.00");
                    //Shoufeijiner[modulus] = Convert.ToDouble(d1);

                    SedBll sendbll = new SedBll(Model.Channels[modulus].sIP, 1007, 1005);
                    string strQIAN = CR.MoneyToChinese(monitor.Charge.ToString());
                    string strsLoad = "73" + CR.GetChineseMoney(strQIAN) + "74";
                    int iLenY = (strsLoad.Length / 2);

                    strsLoad += CR.YHXY(strsLoad).ToString("X2");

                    strsLoad = iLenY.ToString("X2") + strsLoad;

                    string strRst = "";
                    if (Model.Channels[modulus].iXieYi == 1)
                    {
                        strRst = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.Channels[modulus].iCtrlID), 0x3D, 0x72, strsLoad, Model.Channels[modulus].iXieYi);
                    }

                    string strY = "收费类型:" + Model.iChargeType + " 卡片类型:" + cardType[modulus] + " 入场时间:" + monitor.InTime.ToString("yyyy-MM-dd HH:mm:ss") + " 出场时间：" + monitor.OutTime.ToString("yyyy-MM-dd HH:mm:ss");
                    gsd.UpdateCORDMEMORY(Convert.ToDecimal(monitor.Charge.ToString() == "" ? "0" : monitor.Charge.ToString("0.0")), cardNoNo[modulus], Model.Channels[modulus].sInOutName, 0, strY, monitor.OutTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                //485暂时不处理  客户显示频
                if (Model.iSFLed == 1)
                {
                    if (Model.bDispLY)
                    {
                        Model.TbBaoJia = true;
                    }
                    switch (Model.iSFLed)
                    {
                        case 0:
                            Model.TbBaoJia = true;
                            break;
                        case 1:

                            break;
                    }
                }

                //2016-06-16 中央收费,出口收费，则读取真正的入场时间（入场记录的出场时间）
                string sRealInTime = "";
                if (Model.iCentralCharge == 1 && Model.iOutCharge == 1)
                {
                    List<CarIn> ds = gsd.GetMyRsX(monitor.CardNo.ToString(), "", Model.iParkingNo, Model.Channels[modulus].iBigSmall);
                    if (ds.Count > 0)
                    {
                        sRealInTime = ds[0].OutTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                CphCompare();
                if (Model.iIdSfCancel == 1 && monitor.CardNo.ToString().Length == 8)
                {
                }
                else
                {
                    if (bCarNoConfirm == false)
                    {
                        JJcUpdateStore();//写出场库
                    }

                }



                //TempModel.TempModel model = new TempModel.TempModel();
                //model.CardNo = lblcard.Text;
                //model.CardType = lblcardModel.Text;
                //model.inDateTime = lblinDateTime.Text;
                //model.outDateTime = lbloutDateTime.Text;
                //model.modulus = modulus;
                //model.Money = lblbuckleMoney.Text;
                //model.CPH = lblCarNo.Text;
                //model.OutPic = MycomeGopicture[modulus];
                //model.iYHMode = iYHMode;   //2016-06-17
                //model.iYHJH = iYHJH;
                //model.sYHAdr = sYHAdr;
                //model.sYHType = sYHType;
                //model.sYHValue = sYHValue;
                //model.inDateTimeReal = sRealInTime;    //2016-05-27
                //DateTime startTime1 = DateTime.Now;

                //ParkingTempGob_big TempGob = new ParkingTempGob_big(model, new ParkingMonitoringHandler(BinData), new ParkingMonitoringPhotoHandler(TempGob_big_Photo), new SFDataHandler(updateSFJE), tempList, new UpdateCPHDataHandler(updateCPH));



                CarOut co = new CarOut();
                co.CardNO = monitor.CardNo;
                co.CardType = cardType[modulus];
                co.InTime = monitor.InTime;
                co.OutTime = monitor.OutTime;
                co.CPH = monitor.CarNo;
                co.YSJE = monitor.AmountReceivable;
                co.SFJE = monitor.Charge;
                //co.OutPic = inOutPic[modulus];

                if (inOutPic[modulus].Contains(Model.sImageSavePath))
                {
                    co.OutPic = inOutPic[modulus].Substring(Model.sImageSavePath.Length);
                }


                co.YHJE = sCouponValue;
                co.YHAddress = sCouponAddr;
                co.YHType = sCouponMode;
                co.ID = lOutID[modulus];
                co.StationID = Model.stationID;
                co.CarparkNO = Model.iParkingNo;
                co.CPColor = iAutoColor[modulus];

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            ParkingTempGob_big TempGob = new ParkingTempGob_big(modulus, bCarNoConfirm, co, cacl, new Action<string>(UpdateSFJE), new Action<string>(TempGob_big_Photo), new Action<string, decimal>(BinData));
                            TempGob.Owner = this;
                            TempGob.Show();
                        }));

                DateTime endTime1 = DateTime.Now;
                //TimeSpan ssqqq = endTime1 - startTime1;
                //label52.Text = "弹出收费框：" + ssqqq.Days + "天" + ssqqq.Hours + "小时" + ssqqq.Minutes + "分" + ssqqq.Seconds + "秒" + ssqqq.Milliseconds + "毫秒";
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控", "TempCarGo_AUTO" + ex.Message);
                txbOperatorInfo.Text = ex.Message;
            }
        }

        private CarIn GetModel(int Iint, string CPH, string strFileFre)
        {
            CarIn ci = new CarIn();
            ci.CardNO = CR.GetAutoCPHCardNO(Model.Channels[Iint].iCtrlID);
            ci.CardType = "FreA";
            ci.InTime = DateTime.Now;
            ci.CPH = CPH;
            ci.OutTime = DateTime.Now;
            ci.SFJE = 0;
            if (Model.Channels[Iint].iInOut == 0)
            {
                ci.InGateName = Model.Channels[Iint].sInOutName;
                ci.OutGateName = "";
                ci.InOperatorCard = Model.sUserCard;
                ci.InOperator = Model.sUserName;
                ci.OutOperatorCard = "";
                ci.OutOperator = "";
                ci.InPic = strFileFre;
                ci.OutPic = "";
            }
            else
            {
                ci.InGateName = "";
                ci.OutGateName = Model.Channels[Iint].sInOutName;
                ci.InOperatorCard = "";
                ci.InOperator = "";
                ci.OutOperatorCard = Model.sUserCard;
                ci.OutOperator = Model.sUserName;
                ci.InPic = "";
                ci.OutPic = strFileFre;
            }
            ci.InUser = Model.sUserName;
            ci.YSJE = 0;
            ci.Balance = 0;
            ci.BigSmall = Model.Channels[Iint].iBigSmall;
            ci.FreeReason = "军警车，政府车免费放行";
            return ci;
        }
        #endregion


        #region 相机管理
        #region ZNYKT3 海康 ()

        public Int32[] m_lUserID = new Int32[11];
        public ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_DEVICEINFO_V30 m_struDeviceInfo;
        private Int32[] m_iPreviewType = new Int32[11];
        private Int32[] m_lRealHandle = new Int32[11];
        private IntPtr m_ptrRealHandle;
        private IntPtr m_ptrRealHandle1;
        private ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.REALDATACALLBACK m_fRealData = null;
        ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.MSGCallBack m_falarmData;
        private Int32 m_lFortifyHandle = -1;

        Dictionary<string, int> dicListIp = new Dictionary<string, int>();

        private void ProcessCommAlarm_ITSPlate(ref ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            try
            {
                ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_ITS_PLATE_RESULT struAlarmInfoV30 = new ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_ITS_PLATE_RESULT();
                uint dwSize = (uint)Marshal.SizeOf(struAlarmInfoV30);

                struAlarmInfoV30 = (ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_ITS_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_ITS_PLATE_RESULT));

                string strCPH = struAlarmInfoV30.struPlateInfo.sLicense;

                byte bChanel = struAlarmInfoV30.byDriveChan;

                int iInt = 0;
                foreach (var dic in dicListIp)
                {
                    if (dic.Key == pAlarmer.sDeviceIP)
                    {
                        iInt = dic.Value;
                        break;
                    }
                }

                ImageSaveHSPath(iInt, 0, strCPH);

                string strYFile = strHSPathStr;
                string strYFileJpg = strHSPathStrJPG;
                picFileName = "";
                filesJpg = "";
                if (strCPH == "无车牌")
                {
                    if (struAlarmInfoV30.struPicInfo[0].dwDataLen != 0)
                    {
                        System.IO.FileStream fs;
                        if (Model.iCarPosLed == 1)
                        {
                            fs = new System.IO.FileStream(strYFileJpg, System.IO.FileMode.Create);
                        }
                        else
                        {
                            fs = new System.IO.FileStream(strYFile, System.IO.FileMode.Create);
                        }
                        int iLen = (int)struAlarmInfoV30.struPicInfo[0].dwDataLen;
                        byte[] by = new byte[iLen];
                        Marshal.Copy(struAlarmInfoV30.struPicInfo[0].pBuffer, by, 0, iLen);
                        fs.Write(by, 0, iLen);
                        fs.Close();
                    }
                    return;
                }
                int iCount = 0;
                for (int i = 0; i < struAlarmInfoV30.dwPicNum - 1; i++)
                {
                    if (struAlarmInfoV30.struPicInfo[i].dwDataLen != 0)
                    {
                        if (iCount == 0)
                        {
                            iCount++;
                            if (Model.iCarPosLed == 1)
                            {
                                DZHSSaveImage(struAlarmInfoV30.struPicInfo[i].pBuffer, strYFileJpg, (int)struAlarmInfoV30.struPicInfo[i].dwDataLen);
                            }
                            else
                            {
                                DZHSSaveImage(struAlarmInfoV30.struPicInfo[i].pBuffer, strYFile, (int)struAlarmInfoV30.struPicInfo[i].dwDataLen);
                            }

                        }
                    }
                }


                //this.Invoke((EventHandler)delegate
                //{
                //    if (strCPH.Length > 6)
                //    {
                //        if (Model.bAutoPlateEn)
                //        {
                //            Quene.ModelNode model = new Quene.ModelNode();
                //            model.sDzScan = "";
                //            model.iDzIndex = iInt;
                //            model.strFile = strYFile;
                //            model.strFileJpg = strYFileJpg;
                //            model.strCPH = strCPH.Substring(1, strCPH.Length - 1);
                //            LQueue.Enqueue(model);


                //            //                             Thread.Sleep(Model.iAutoYTime);
                //            //                             DZHSChePaiShiBie(iInt, strYFile, strYFileJpg, strCPH.Substring(1, strCPH.Length - 1));
                //        }
                //    }

                //});
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控:ProcessCommAlarm_ITSPlate", ex.Message + "\r\n" + ex.StackTrace);
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txbOperatorInfo.Text = ex.Message;
                            }));
            }
        }

        public void MsgCallback(int lCommand, ref ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            switch (lCommand)
            {
                //                 case CR.CHCNetSDK.COMM_ALARM:
                //                    // ProcessCommAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                //                     break;
                //                 case CR.CHCNetSDK.COMM_ALARM_V30:
                //                     //ProcessCommAlarm_V30(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                //                     break;
                //                 case CR.CHCNetSDK.COMM_UPLOAD_PLATE_RESULT:
                //                    // ProcessCommAlarm_Plate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                //                     break;
                case ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.COMM_ITS_PLATE_RESULT:
                    ProcessCommAlarm_ITSPlate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                //         case CHCNetSDK.COMM_ALARM_RULE:
                //             this.BeginInvoke(AlarmInfo, "COMM_ALARM_RULE");
                //             break;
                //         case CHCNetSDK.COMM_TRADEINFO:
                //             this.BeginInvoke(AlarmInfo, "COMM_TRADEINFO");
                //             break;
                //         case CHCNetSDK.COMM_IPCCFG:
                //             this.BeginInvoke(AlarmInfo, "COMM_IPCCFG");
                //             break;
                //         case CHCNetSDK.COMM_IPCCFG_V31:
                //             this.BeginInvoke(AlarmInfo, "COMM_IPCCFG_V31");
                //             break;
                default:
                    break;
            }
        }

        private void Preview(string DVRIPAddress, Int16 DVRPortNumber, string DVRUserName, string DVRPassword, System.Windows.Forms.PictureBox px, int Count, string strType)
        {
            m_iPreviewType[Count] = 0;
            m_lRealHandle[Count] = -1;
            m_lUserID[Count] = -1;
            m_lUserID[Count] = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref m_struDeviceInfo);
            if (m_lUserID[Count] == -1)
            {
                MessageBox.Show("无法连接到设备", "提示");
                return;
            }
            else
            {

            }

            ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo = new ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_CLIENTINFO();

            lpClientInfo.lChannel = 1;
            lpClientInfo.lLinkMode = 0x0000;
            lpClientInfo.sMultiCastIP = "";
            lpClientInfo.hPlayWnd = px.Handle;

            if (m_iPreviewType[Count] == 0)
            {
                m_fRealData = null;
                IntPtr pUser = new IntPtr();
                m_lRealHandle[Count] = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_RealPlay_V30(m_lUserID[Count], ref lpClientInfo, null, pUser, 1);
            }

            if (strType == "ZNYKTYY4")
            {
                if (Count == 0)
                {
                    m_falarmData = new ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.MSGCallBack(MsgCallback);
                }

                ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_SETUPALARM_PARAM m_struSetupParam = new ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
                m_struSetupParam.dwSize = (uint)Marshal.SizeOf(m_struSetupParam);
                m_struSetupParam.byLevel = 1;
                m_struSetupParam.byAlarmInfoType = 1;

                m_lFortifyHandle = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_SetupAlarmChan_V41(m_lUserID[Count], ref m_struSetupParam);
                if (m_lFortifyHandle != -1)
                {

                }
                else
                {
                    //uint iLastErr = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_GetLastError();
                }
            }
            if (m_lRealHandle[Count] == -1)
            {
                //uint nError = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_GetLastError();
                return;
            }
        }
        #endregion


        #region ZNYKT5 zs
        public static Int32[] m_hLPRClient = new Int32[11];
        public static Int32[] m_nSerialHandle = new Int32[11];
        public Int32[] m_hLPRPlay = new Int32[11];
        private uint VZ_LPRC_USER_DATA_MAX_LEN = 128;

        private ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZDEV_SERIAL_RECV_DATA_CALLBACK serialRECV = null;
        ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZLPRC_PLATE_INFO_CALLBACK m_PlateResultCB;

        private string GetDEC(string strKey, int iFlag)
        {
            string strRst = "";
            if (strKey.Length > 10)
            {
                strRst = strKey.Substring(strKey.Length - 4, 4) + strKey.Substring(0, strKey.Length - 4);

                strRst = strRst.Substring(8) + strRst.Substring(0, 8);

                strRst = strRst.Substring(6) + strRst.Substring(0, 6);

                strRst = strRst.Substring(3) + strRst.Substring(0, 3);//更改摄像机加密协议

                int iLen = strRst.Length;
                if (iFlag == 0)
                {
                    strRst = CR.UserMd5(strRst);
                }
                else
                {
                    strRst = CR.UserMd5new(strRst);
                }

            }
            return strRst;
        }

        int IProc = 0;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x00AA:
                    IProc++;

                    break;
                case 0x00AB:
                    ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_LPR_MSG_PLATE_INFO plateInfo;
                    plateInfo = (ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_LPR_MSG_PLATE_INFO)Marshal.PtrToStructure(wParam, typeof(ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_LPR_MSG_PLATE_INFO));



                    if (Model.iCPHPhoto == 1 && plateInfo.CPHFilesJpg != "")
                    {
                        //2016-12-17
                        //if (delLastInImage != plateInfo.CPHFilesJpg && delLastInImage != "" && Model.Channels[plateInfo.startIndex].iInOut == 0)
                        //{
                        //    if (System.IO.File.Exists(delLastInImage))
                        //    {
                        //        System.IO.File.Delete(delLastInImage);
                        //        delLastInImage = "";
                        //    }
                        //}

                        //if (delLastOutImage != plateInfo.CPHFilesJpg && delLastOutImage != "" && Model.Channels[plateInfo.startIndex].iInOut == 1)
                        //{
                        //    if (System.IO.File.Exists(delLastOutImage))
                        //    {
                        //        System.IO.File.Delete(delLastOutImage);
                        //        delLastOutImage = "";
                        //    }
                        //}

                        //if (Model.Channels[plateInfo.startIndex].iInOut == 0)
                        //    delLastInImage = plateInfo.CPHFilesJpg;
                        //else
                        //    delLastOutImage = plateInfo.CPHFilesJpg;

                        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(plateInfo.CPHFilesJpg), lstPicSmallCarNo[plateInfo.startIndex].Width, lstPicSmallCarNo[plateInfo.startIndex].Height);
                        lstPicSmallCarNo[plateInfo.startIndex].Image = bmp;

                        CR.DeleteImg(plateInfo.CPHFilesJpg);
                        //lstImgSmallCarNo[plateInfo.startIndex].Source = new BitmapImage();
                    }
                    Quene.ModelNode model = new Quene.ModelNode();
                    model.sDzScan = "";
                    model.iDzIndex = plateInfo.startIndex;
                    model.strFile = plateInfo.Files;
                    model.strFileJpg = plateInfo.FilesJpg;
                    model.strCPH = plateInfo.plate;
                    LQueue.Enqueue(model);
                    break;
            }
            return IntPtr.Zero;
        }

        int[] iAutoColor = new int[11];

        DateTime dtInOnPlate = DateTime.Now.AddMinutes(-1);
        DateTime dtoutOnPlate = DateTime.Now.AddMinutes(-1);

        public IntPtr hwndMain;
        private int OnPlateResult(int handle, IntPtr pUserData,
                                                  IntPtr pResult, uint uNumPlates,
                                                  ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_LPRC_RESULT_TYPE eResultType,
                                                  IntPtr pImgFull,
                                                  IntPtr pImgPlateClip)
        {
            ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.TH_PlateResult result = (ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.TH_PlateResult)Marshal.PtrToStructure(pResult, typeof(ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.TH_PlateResult));

            if (eResultType != ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_LPRC_RESULT_TYPE.VZ_LPRC_RESULT_REALTIME)
            {
                string strLicense = new string(result.license).Trim('\0');

                Model.iAutoColor = result.nColor;

                for (int iVz = 0; iVz < m_hLPRClient.Length; iVz++)
                {
                    if (m_hLPRClient[iVz] == handle)
                    {
                        if (Model.iPersonVideo == 1 && iVz > 1)
                        {
                            iVz = iVz - 2;
                        }
                        if (Model.iPersonVideo == 1)
                        {
                            if (Model.Channels[iVz].iInOut == 0)
                            {
                                if (DateTime.Now < dtInOnPlate.AddSeconds(Convert.ToDouble(Model.iCarPosLedJH)))
                                {
                                    //CR.DeleteImg(strFile);
                                    return 0;
                                }
                                else
                                {
                                    dtInOnPlate = DateTime.Now;
                                }
                            }
                            else
                            {
                                if (DateTime.Now < dtoutOnPlate.AddSeconds(Convert.ToDouble(Model.iCarPosLedJH)))
                                {
                                    return 0;
                                }
                                else
                                {
                                    dtoutOnPlate = DateTime.Now;
                                }
                            }
                        }

                        ImageSaveHSPath(iVz, 0, "");
                        string strYFile = strHSPathStr;
                        string strYFileJpg = strHSPathStrJPG;
                        string strCPHFileJpg = "";
                        if (pImgFull.ToInt32() != 0)
                        {
                            if (Model.iCPHPhoto == 1)
                            {
                                if (pImgPlateClip.ToInt32() != 0)
                                {
                                    ImageSaveHSPath(iVz, 0, strLicense);
                                    strCPHFileJpg = strHSPathStrJPG;
                                    int ret = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ImageSaveToJpeg(pImgPlateClip, strCPHFileJpg, 100);
                                    if (ret == 0)
                                    {

                                    }
                                    else
                                    {
                                        strCPHFileJpg = "";
                                    }

                                }
                            }

                            if (Model.iCarPosLed == 1)
                            {
                                int ret = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ImageSaveToJpeg(pImgFull, strYFileJpg, 100);
                                if (ret < 0)
                                {
                                    gsd.AddLog("在线监控" + ":OnPlateResult", strLicense + "图片保存失败");
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    LblDisplay.Text = "图片保存失败！";
                                    //});
                                }

                            }
                            else
                            {
                                int ret = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ImageSaveToJpeg(pImgFull, strYFile, 100);
                                if (ret < 0)
                                {
                                    gsd.AddLog("在线监控" + ":OnPlateResult", strLicense + "图片保存失败");
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    LblDisplay.Text = "图片保存失败！";
                                    //});
                                }
                            }
                            //保存图片
                        }
                        if (Model.iAutoPlateEn == 1)
                        {
                            ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_LPR_MSG_PLATE_INFO plateInfo = new ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_LPR_MSG_PLATE_INFO();
                            iAutoColor[iVz] = result.nColor;
                            plateInfo.plate = strLicense;
                            plateInfo.Files = strYFile;
                            plateInfo.FilesJpg = strYFileJpg;
                            plateInfo.startIndex = iVz;
                            plateInfo.CPHFilesJpg = strCPHFileJpg;
                            int size = Marshal.SizeOf(plateInfo);
                            IntPtr intptr = Marshal.AllocHGlobal(size);
                            Marshal.StructureToPtr(plateInfo, intptr, true);

                            CR.PostMessage(hwndMain, 0x00AB, (int)intptr, 0);
                        }
                        break;
                    }
                }
            }
            return 0;
        }

        private void VzPreview(string DVRIPAddress, Int16 DVRPortNumber, string DVRUserName, string DVRPassword, System.Windows.Forms.PictureBox px, int Count, string strType)
        {
            if (m_hLPRClient[Count] == 0)
            {
                m_hLPRClient[Count] = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_Open(DVRIPAddress, (ushort)DVRPortNumber, DVRUserName, DVRPassword);
                if (m_hLPRClient[Count] == 0)
                {
                    MessageBox.Show("摄像机【" + DVRIPAddress + "】连接失败！");
                }
                else
                {
                    m_nSerialHandle[Count] = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_SerialStart(m_hLPRClient[Count], 1, serialRECV, IntPtr.Zero);
                    if (strType == "ZNYKTY15")
                    {
                        byte[] strUserData = new byte[VZ_LPRC_USER_DATA_MAX_LEN + 1];
                        GCHandle hObject = GCHandle.Alloc(strUserData, GCHandleType.Pinned);
                        IntPtr pObject = hObject.AddrOfPinnedObject();

                        int nSizeData = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ReadUserData(m_hLPRClient[Count], pObject, VZ_LPRC_USER_DATA_MAX_LEN);
                        if (nSizeData <= 0)
                        {
                            m_hLPRClient[Count] = 0;
                            ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_Close(m_hLPRClient[Count]);
                            MessageBox.Show("请检查相机型号是否正确-2");
                            return;
                        }
                        else
                        {
                            string strKey = System.Text.Encoding.Default.GetString(strUserData).Substring(0, nSizeData);

                            ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_DEV_SERIAL_NUM struSN = new ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZ_DEV_SERIAL_NUM();

                            int rt1 = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_GetSerialNumber(m_hLPRClient[Count], ref struSN);

                            string strKeyY = struSN.uHi.ToString("X8") + "-" + struSN.uLo.ToString("X8");

                            string strKey2 = GetDEC(strKeyY, 1);

                            if (strKey == strKey2)
                            {
                            }
                            else
                            {
                                m_hLPRClient[Count] = 0;
                                ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_Close(m_hLPRClient[Count]);
                                MessageBox.Show("请检查相机型号是否正确-1");
                                return;
                            }
                        }
                    }

                    //实时开始播放视频数据
                    m_hLPRPlay[Count] = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_StartRealPlay(m_hLPRClient[Count], px.Handle);

                    int iEnableImage = 1;// 指定识别结果的回调是否需要包含截图信息：1为需要，0为不需要
                    if (Count == 0)
                    {
                        m_PlateResultCB = new ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZLPRC_PLATE_INFO_CALLBACK(OnPlateResult);
                    }
                    int iRst = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_SetPlateInfoCallBack(m_hLPRClient[Count], m_PlateResultCB, IntPtr.Zero, iEnableImage);


                    if (Model.bOut485)
                    {

                        string SendSum = "";
                        string Jstrs = "";
                        int sum = 0;
                        SendSum += "A500BB4144";
                        Jstrs = "060102960064" + Model.Channels[Count].iCtrlID.ToString("X2") + "01";
                        byte[] array = CR.GetByteArray(Jstrs);
                        foreach (byte by in array)
                        {
                            sum += by;
                        }

                        sum = sum % 256;

                        short iNo = 0;
                        short cmd = 0xA5;
                        string strS = SendSum + sum.ToString("X2") + Jstrs + "FF";

                        byte[] bVZSend = CR.GetArray(strS);
                        GCHandle hObject1 = GCHandle.Alloc(bVZSend, GCHandleType.Pinned);
                        IntPtr pObject1 = hObject1.AddrOfPinnedObject();

                        int ret = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle[Count], pObject1, bVZSend.Length);

                    }
                }
            }
            else
            {

            }
        }
        #endregion


        #region ZNYKT10 浙江大华
        /// <summary>
        /// 设备IP
        /// </summary>
        private string[] strLogionIP = new string[10];

        /// <summary>
        /// 设备用户登录ＩＤ
        /// </summary>
        private int[] pLoginID = new int[10];

        /// <summary>
        /// 设备用户登录ＩＤ
        /// </summary>
        private int[] pCallBack = new int[10];

        /// <summary>
        /// 程序消息提示Title
        /// </summary>
        private const string pMsgTitle = "大华网络程序";

        /// <summary>
        /// 最后操作信息显示格式
        /// </summary>
        private const string pErrInfoFormatStyle = "代码:errcode;\n描述:errmSG.";

        /// <summary>
        /// 当前回放的文件信息
        /// </summary>
        ParkingCommunication.CameraSDK.ZNYKT10.NET_RECORDFILE_INFO fileInfo;

        /// <summary>
        /// 播放方式
        /// </summary>
        private int playBy = 0;

        /// <summary>
        /// 实时播放句柄保存
        /// </summary>
        private int[] pRealPlayHandle = new int[11];

        /// <summary>
        /// 回放句柄保存
        /// </summary>
        private int[] pPlayBackHandle;

        /// <summary>
        /// 回放通道号
        /// </summary>
        private int pPlayBackChannelID;

        private ParkingCommunication.CameraSDK.ZNYKT10.fDisConnect disConnect;

        private ParkingCommunication.CameraSDK.ZNYKT10.NET_DEVICEINFO deviceInfo;

        private ParkingCommunication.CameraSDK.ZNYKT10.fHaveReConnect onlineMsg;     //设备重新在线消息

        private ParkingCommunication.CameraSDK.ZNYKT10.fAnalyzerDataCallBack fAnalyze;

        private void DHPreview(string DVRIPAddress, string DVRPortNumber, string DVRUserName, string DVRPassword, System.Windows.Forms.PictureBox px, int Count, string strType)
        {
            //设备用户信息获得
            deviceInfo = new ParkingCommunication.CameraSDK.ZNYKT10.NET_DEVICEINFO();
            int error = 0;
            pLoginID[Count] = ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHLogin(DVRIPAddress, ushort.Parse(DVRPortNumber), DVRUserName, DVRPassword, out deviceInfo, out error);

            if (strType == "ZNYKTYY10")
            {
                if (Count == 0)
                {
                    fAnalyze = new ParkingCommunication.CameraSDK.ZNYKT10.fAnalyzerDataCallBack(RealLoadPicCallback);// 设置回调
                }
                pCallBack[Count] = ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHRealLoadPictureEx(pLoginID[Count], 0, ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_ALL, true, fAnalyze, 0, IntPtr.Zero);


                if (pCallBack[Count] != 0)
                {
                    strLogionIP[Count] = DVRIPAddress;

                    Console.WriteLine("消息订阅成功");
                    Console.WriteLine("Wait Event...");
                }
                else
                {
                    Console.WriteLine("订阅失败，需要交由其他线程，重新订阅");
                }
            }

            if (pLoginID[Count] != 0)
            {
                pPlayBackHandle = new int[deviceInfo.byChanNum];
                //pRealPlayHandle = new int[deviceInfo.byChanNum];
                for (int i = 0; i < deviceInfo.byChanNum; i++)
                {
                    pRealPlayHandle[Count] = ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHRealPlay(pLoginID[Count], i, px.Handle);
                }
            }
        }


        private string PrintTrafficCarInfo(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TRAFFICCAR_INFO stTrafficCar)
        {
            return ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHByteArrayToString(stTrafficCar.szPlateNumber).Trim('\0');
        }

        bool bShiBie = false;
        public int RealLoadPicCallback(Int32 lAnalyzerHandle, UInt32 dwAlarmType, IntPtr pAlarmInfo, IntPtr pBuffer, UInt32 dwBufSize, UInt32 dwUser, Int32 nSequence, IntPtr reserved)
        {
            try
            {
                int iInt = 0;
                for (int icallBack = 0; icallBack < Model.iChannelCount; icallBack++)
                {
                    if (lAnalyzerHandle == pCallBack[icallBack])
                    {
                        iInt = icallBack;
                        break;
                    }
                }

                string str1 = "";
                if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFIC_RUNREDLIGHT)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_RUNREDLIGHT_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_RUNREDLIGHT_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_RUNREDLIGHT_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_RUNREDLIGHT_INFO));
                    str1 = PrintTrafficCarInfo(Info.stTrafficCar);
                    //ShowEventFileInfo(Info.stuFileInfo);
                }
                else if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFICJUNCTION)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICJUNCTION_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICJUNCTION_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICJUNCTION_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICJUNCTION_INFO));
                    str1 = PrintTrafficCarInfo(Info.stTrafficCar);
                    // str1 = Byte2String(Info.stTrafficCar.szPlateNumber);
                    // ShowEventFileInfo(Info.stuFileInfo);
                }
                else if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFIC_TURNLEFT)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNLEFT_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNLEFT_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNLEFT_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNLEFT_INFO));
                    str1 = PrintTrafficCarInfo(Info.stTrafficCar);
                    //ShowEventFileInfo(Info.stuFileInfo);
                }
                else if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFIC_TURNRIGHT)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNRIGHT_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNRIGHT_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNRIGHT_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_TURNRIGHT_INFO));
                    str1 = PrintTrafficCarInfo(Info.stTrafficCar);
                    // ShowEventFileInfo(Info.stuFileInfo);
                }
                else if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFIC_OVERSPEED)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_OVERSPEED_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_OVERSPEED_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_OVERSPEED_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_OVERSPEED_INFO));
                    str1 = PrintTrafficCarInfo(Info.stTrafficCar);
                    //ShowEventFileInfo(Info.stuFileInfo);
                }
                else if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFIC_UNDERSPEED)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_UNDERSPEED_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_UNDERSPEED_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_UNDERSPEED_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_UNDERSPEED_INFO));
                    str1 = PrintTrafficCarInfo(Info.stTrafficCar);
                    //ShowEventFileInfo(Info.stuFileInfo);
                }
                else if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFICGATE)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICGATE_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICGATE_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICGATE_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFICGATE_INFO));
                    Console.WriteLine("Speed = {0}", Info.nSpeed);
                    Console.WriteLine("Name = {0}", ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHByteArrayToString(Info.szName));
                    // ShowEventFileInfo(Info.stuFileInfo);
                }
                else if (dwAlarmType == ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_TRAFFIC_MANUALSNAP)
                {
                    ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_MANUALSNAP_INFO Info = new ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_MANUALSNAP_INFO();
                    Info = (ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_MANUALSNAP_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT10.DEV_EVENT_TRAFFIC_MANUALSNAP_INFO));
                    str1 = PrintTrafficCarInfo(Info.stTrafficCar);
                    // ShowEventFileInfo(Info.stuFileInfo);
                }
                else
                {
                    Console.WriteLine("Get Event = {0:x}", dwAlarmType);
                }

                ImageSaveHSPath(iInt, 0, str1);

                string strYFile = strHSPathStr;
                string strYFileJpg = strHSPathStrJPG;
                picFileName = "";
                filesJpg = "";
                if (Model.iCarPosLed == 1)
                {
                    DZHSSaveImage(pBuffer, strYFileJpg, (int)dwBufSize);
                }
                else
                {
                    DZHSSaveImage(pBuffer, strYFile, (int)dwBufSize);
                }


                //this.Invoke((EventHandler)delegate
                //{
                if (str1.Length > 6)
                {
                    DateTime endTime = DateTime.Now;

                    if (Model.iAutoPlateEn == 1)
                    {
                        iAutoColor[iInt] = Model.iAutoColor;
                        Quene.ModelNode model = new Quene.ModelNode();
                        model.sDzScan = "";
                        model.iDzIndex = iInt;
                        model.strFile = strYFile;
                        model.strFileJpg = strYFileJpg;
                        model.strCPH = str1;
                        LQueue.Enqueue(model);
                    }
                }

                //});

                bShiBie = false;

                return 1;
            }
            catch (Exception ex)
            {
                string sss = ex.Message;
                return 0;
            }
        }



        /// <summary>
        /// 设备断开连接处理
        /// </summary>
        /// <param name="lLoginID"></param>
        /// <param name="pchDVRIP"></param>
        /// <param name="nDVRPort"></param>
        /// <param name="dwUser"></param>
        private void DisConnectEvent(int lLoginID, StringBuilder pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            //设备断开连接处理            
            for (int i = 0; i < strLogionIP.Length; i++)
            {
                if (strLogionIP[i] == pchDVRIP.ToString())
                {
                    bool bret = ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHStopLoadPic(pCallBack[i]);
                    if (!bret)
                    {
                        //listBoxMsg.BeginInvoke(new System.EventHandler(UpdateListBoxMsg), pchDVRIP.ToString() + "设备离线,通道" + i.ToString() + "停止订阅事件失败!");
                    }
                }

            }
        }

        private void OnlineEvent(int lLoginID, StringBuilder pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            //自动重连成功事件后，再发起订阅设备事件消息
            for (int i = 0; i < strLogionIP.Length; i++)
            {
                if (strLogionIP[i] == pchDVRIP.ToString())
                {
                    pCallBack[i] = ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHRealLoadPicture(pLoginID[i], 0, ParkingCommunication.CameraSDK.ZNYKT10.EventIvs.EVENT_IVS_ALL, fAnalyze, 0);
                    if (pCallBack[i] == 0)
                    {
                        //listBoxMsg.BeginInvoke(new System.EventHandler(UpdateListBoxMsg), pchDVRIP.ToString() + "-通道" + i.ToString() + "重新订阅事件失败!!");
                    }
                }
            }

            //listBoxMsg.BeginInvoke(new System.EventHandler(UpdateListBoxMsg), pchDVRIP.ToString() + "设备重新在线消息");
        }


        #endregion


        #region ZNYKT11 翰森一体机
        bool m_bSDKInited = false;

        IntPtr[] m_hDev = new IntPtr[11];
        IntPtr[] m_hDecoder = new IntPtr[11];
        ParkingCommunication.CameraSDK.ZNYKT11.dv.DV_CaptureCallback m_pCapCb = null;
        ParkingCommunication.CameraSDK.ZNYKT11.dv.DV_DataCallback m_pEncDataCb = null;
        ParkingCommunication.CameraSDK.ZNYKT11.dv.DV_DecoderDataCallback m_pDecDataCb = null;


        IntPtr[] m_hDevY = new IntPtr[11];
        IntPtr[] m_hDecoderY = new IntPtr[11];
        ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_CaptureCallback m_pCapCbY = null;
        ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_DataCallback m_pEncDataCbY = null;

        public void HSNetY(string DVRIPAddress, Int16 DVRPortNumber, string DVRUserName, string DVRPassword, System.Windows.Forms.PictureBox px, int Count)
        {
            int errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK;
            IntPtr hDev = IntPtr.Zero;
            ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_DeviceCnnInfo devInfo = new ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_DeviceCnnInfo();

            // 打开设备
            devInfo.szIP = DVRIPAddress;

            errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_OpenDevice(ref devInfo, ref hDev);
            if (errCode == ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK)
            {
                // 启动码流
                errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_StartStream(hDev, 0, px.Handle);

                errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_SetCaptureCallback(hDev, m_pCapCbY, IntPtr.Zero);

                if (errCode == ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK)
                {
                    m_hDevY[Count] = hDev;
                }
            }
        }


        void CaptureCallbackY(IntPtr hDevice, ref ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_CaptureInfo pCaptureInfo, IntPtr pUserData)
        {
            int iCoutHS = 0;
            for (int i = 0; i < Model.iChannelCount; i++)
            {
                if (m_hDevY[i] == hDevice)
                {
                    iCoutHS = i;
                    break;
                }
            }

            ImageSaveHSPath(iCoutHS, 0, "");
            string strYFile = strHSPathStr;
            string strYFileJpg = strHSPathStrJPG;
            string strLicense = pCaptureInfo.szPlateText;
            if (Model.iCarPosLed == 1)
            {
                int errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK;
                errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_SaveDataToFile(strYFileJpg, pCaptureInfo.pData[0], pCaptureInfo.nLength[0], 0);
                if (errCode != ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK)
                {

                    gsd.AddLog("在线监控" + ":CaptureCallbackY", strLicense + "图片保存失败");
                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                    {
                        txbOperatorInfo.Text = "图片保存失败！";
                    }));
                }

            }
            else
            {
                int errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK;
                errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_SaveDataToFile(strYFile, pCaptureInfo.pData[0], pCaptureInfo.nLength[0], 0);
                if (errCode != ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK)
                {
                    gsd.AddLog("在线监控" + ":CaptureCallbackY", strLicense + "图片保存失败");
                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txbOperatorInfo.Text = "图片保存失败！";
                            }));
                }
            }

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                if (Model.iAutoPlateEn == 1)
                                {
                                    //MessageBox.Show(CR.PubVal.iAutoColor.ToString());
                                    //!!!
                                    DZChePaiShiBieQ(iCoutHS, strYFile, strYFileJpg, strLicense);
                                }
                            }));
        }
        #endregion


        #region ZNYKT13 亿维
        public struct VIDEO_WND
        {
            public string m_DevName, m_DevUrl, m_DevUser, m_DevPasswd;
            public int m_nPort;
            public IntPtr m_hOpenVideo, m_hLogon, m_hOpenImg, m_hTalk, m_hPlayTalk, m_hRecord;
            public ushort m_VideoWidth, m_VideoHeight;
            public int m_CurrPosY;
            public object m_mutexRecord;
            public Label DisplayWnd;
            public bool m_bSlave, m_bListen, m_bCheckLine, m_bCarRect;
            public byte[] m_bOut;
            public AutoResetEvent LoginEvent;
            public Task m_ReLoginThread;
            public AutoResetEvent ChannelEvent;
            public Task m_ReOpenChThread;
            public AutoResetEvent PictureEvent;
            public Task m_ReOpenPicThread;
        }

        const int WM_USER = 0x0400;
        const int WM_YW7000NET_COMMAND = WM_USER + 100;
        VIDEO_WND[] VideoInfo = new VIDEO_WND[11];
        int m_nSelIndex = 0;
        ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.ChannelStreamCallback m_ChannelStreamCallback = null;
        ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.PictureCallback m_CapturePictureCallback = null;

        private int ChannelStreamCallback(IntPtr hOpenChannel,
                                   IntPtr pStreamData,
                                   uint dwClientID,
                                   IntPtr pContext,
                                   ParkingCommunication.CameraSDK.ZNYKT13.ENCODE_VIDEO_TYPE encodeVideoType,
                                   ref ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.HHAV_INFO pAVInfo)
        {
            ushort wDisplayWnd = (ushort)pContext;
            uint dwFrameSize = 0;
            ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.HV_FRAME_HEAD FrameHead = (ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.HV_FRAME_HEAD)Marshal.PtrToStructure(pStreamData, typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.HV_FRAME_HEAD));
            byte[] bExtHead = new byte[Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.HV_FRAME_HEAD)) + Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.EXT_FRAME_HEAD))];
            Marshal.Copy(pStreamData, bExtHead, 0, bExtHead.Length);
            IntPtr ptrExtHead = Marshal.AllocHGlobal((int)Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.EXT_FRAME_HEAD)));
            Marshal.Copy(bExtHead, Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.HV_FRAME_HEAD)), ptrExtHead, (int)Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.EXT_FRAME_HEAD)));
            ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.EXT_FRAME_HEAD ExtFrameHead = (ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.EXT_FRAME_HEAD)Marshal.PtrToStructure(ptrExtHead, typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.EXT_FRAME_HEAD));
            Marshal.FreeHGlobal(ptrExtHead);
            int ret = 0;
            dwFrameSize = (uint)(Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000AVDefine.HV_FRAME_HEAD)) + FrameHead.nByteNum);
            if (FrameHead.streamFlag != (byte)ParkingCommunication.CameraSDK.ZNYKT13.eFrameType.MY_FRAME_TYPE_A)
            {
                VideoInfo[wDisplayWnd].m_VideoWidth = ExtFrameHead.szFrameInfo.szFrameVideo.nVideoWidth;
                VideoInfo[wDisplayWnd].m_VideoHeight = ExtFrameHead.szFrameInfo.szFrameVideo.nVideoHeight;
            }
            ret = ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_PutDecStreamDataEx(
                wDisplayWnd,
                pStreamData,
                dwFrameSize,
                encodeVideoType,
                ref pAVInfo);
            lock (VideoInfo[wDisplayWnd].m_mutexRecord)
            {
                if (VideoInfo[wDisplayWnd].m_hRecord != IntPtr.Zero)
                    ParkingCommunication.CameraSDK.ZNYKT13.HHReadWriter.HHFile_InputFrame(VideoInfo[wDisplayWnd].m_hRecord, pStreamData, (int)dwFrameSize, 0);
            }
            return ret;
        }

        private int CapturePictureCallback(IntPtr hPictureChn, IntPtr pPicData, int nPicLen, uint dwClientID, IntPtr pContext)
        {
            bool bHasPlateNo = false;
            ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.HH_PICTURE_INFO sPicInfo;
            int nOffset = 0;
            ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_ReadPictureInfo(hPictureChn, out sPicInfo);
            bool bWriteTestFile = false;

            int iVz = 0;
            for (int iYW = 0; iYW < VideoInfo.Length; iYW++)
            {
                if (VideoInfo[iYW].m_DevUrl == sPicInfo.szServerIP)
                {
                    iVz = iYW;
                    break;
                }
            }


            DateTime dtNow = DateTime.Now;
            ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.VEHICLE_INFO_S stImageInfo = (ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.VEHICLE_INFO_S)Marshal.PtrToStructure(pPicData + nPicLen - Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.VEHICLE_INFO_S)), typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.VEHICLE_INFO_S));

            string strYFile = strHSPathStr;
            string strYFileJpg = strHSPathStrJPG;
            if (pPicData != IntPtr.Zero && nPicLen - nOffset != 0)
            {
                ImageSaveHSPath(iVz, 0, "");
                strYFile = strHSPathStr;
                strYFileJpg = strHSPathStrJPG;
                if (Model.iCarPosLed == 1)
                {
                    DZHSSaveImage(pPicData, strYFileJpg, (int)nPicLen - nOffset);
                }
                else
                {
                    DZHSSaveImage(pPicData, strYFile, (int)nPicLen - nOffset);
                }
            }

            if (Model.iAutoPlateEn == 1)
            {
                Quene.ModelNode model = new Quene.ModelNode();
                model.sDzScan = "";
                model.iDzIndex = iVz;
                model.strFile = strYFile;
                model.strFileJpg = strYFileJpg;
                model.strCPH = stImageInfo.szPlateNum;
                LQueue.Enqueue(model);
            }
            return 1;
        }

        private void DZHSSaveImage(IntPtr ipBuffer, string strFile, int dwDataLen)
        {
            try
            {
                System.IO.FileStream fs;
                using (fs = new System.IO.FileStream(strFile, System.IO.FileMode.Create))
                {
                    int iLen = dwDataLen;
                    byte[] by = new byte[iLen];
                    Marshal.Copy(ipBuffer, by, 0, iLen);
                    fs.Write(by, 0, iLen);
                    fs.Close();
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txbOperatorInfo.Text = ex.Message;
                            }));
                gsd.AddLog("在线监控:DZHSSaveImage", ex.Message + "\r\n" + ex.StackTrace);
            }
        }


        public void SZYW(string DVRIPAddress, Int16 DVRPortNumber, string DVRUserName, string DVRPassword, System.Windows.Forms.PictureBox px, int Count)
        {

            ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_InitPlayer2(ushort.Parse(Count.ToString()), px.Handle, true);
            ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_OpenStream(ushort.Parse(Count.ToString()));

            ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE errCode = ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_LogonServer(DVRIPAddress, ushort.Parse(DVRPortNumber.ToString()), "", DVRUserName, DVRPassword,
                (uint)Count,
                out VideoInfo[Count].m_hLogon,
                IntPtr.Zero);

            if (errCode == ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE.HHERR_SUCCESS)
            {


                ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.HH_SERVER_INFO SERVER_INFO = new ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.HH_SERVER_INFO();
                ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_ReadServerInfo(VideoInfo[m_nSelIndex].m_hLogon, ref SERVER_INFO);

                string strKey = CR.UserMd5(SERVER_INFO.nDeviceID.ToString());

                ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.USERDATA_CONFIG stTrafficSnap = new ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.USERDATA_CONFIG();
                uint config_len = (uint)Marshal.SizeOf(typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.USERDATA_CONFIG));
                uint dwAppend = 0;

                IntPtr ptrTrafficSnap = Marshal.AllocHGlobal((int)config_len);

                errCode = ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_GetServerConfig(VideoInfo[m_nSelIndex].m_hLogon, ParkingCommunication.CameraSDK.ZNYKT13.HHCMD_NET.HHCMD_GET_USERDATA, ptrTrafficSnap, ref config_len, ref dwAppend);

                stTrafficSnap = (ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.USERDATA_CONFIG)Marshal.PtrToStructure(ptrTrafficSnap, typeof(ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.USERDATA_CONFIG));

                string strKey1 = System.Text.Encoding.Default.GetString(stTrafficSnap.userData).Substring(0, stTrafficSnap.nDataLen);
                if (strKey == strKey1)
                {

                }
                else
                {
                    MessageBox.Show("连接相机【" + DVRIPAddress + "】失败JM!", "提示");
                    return;
                }
                Marshal.FreeHGlobal(ptrTrafficSnap);


                m_hLPRClient[Count] = (int)VideoInfo[Count].m_hLogon;//赋值摄像机开闸参数值
                m_nSerialHandle[Count] = (int)VideoInfo[Count].m_hLogon;//赋值摄像机开闸参数值

                VideoInfo[Count].m_DevName = "";
                VideoInfo[Count].m_DevUrl = DVRIPAddress;
                VideoInfo[Count].m_nPort = int.Parse(DVRPortNumber.ToString());
                VideoInfo[Count].m_DevUser = DVRUserName;
                VideoInfo[Count].m_DevPasswd = DVRPassword;

                Thread.Sleep(200);

                ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.HHOPEN_CHANNEL_INFO openInfo = new ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.HHOPEN_CHANNEL_INFO();
                openInfo.dwClientID = (uint)Count;
                openInfo.nOpenChannel = 0;
                if (false)
                    openInfo.nOpenChannel |= (uint)1 << 8;
                openInfo.protocolType = ParkingCommunication.CameraSDK.ZNYKT13.NET_PROTOCOL_TYPE.NET_PROTOCOL_TCP;
                openInfo.funcStreamCallback = m_ChannelStreamCallback;
                openInfo.pCallbackContext = (IntPtr)Count;

                ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE errCode1 = ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_OpenChannel(
                    DVRIPAddress,
                    ushort.Parse(DVRPortNumber.ToString()),
                    "",
                    DVRUserName,
                    DVRPassword,
                    ref openInfo,
                    out VideoInfo[Count].m_hOpenVideo,
                    IntPtr.Zero);
                if (errCode1 != ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE.HHERR_SUCCESS)
                {
                    MessageBox.Show("打开通道失败【" + DVRIPAddress + "】失败!", "提示");
                }
                else
                {
                    Thread.Sleep(200);

                    ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.HHOPEN_PICTURE_INFO sOpenPicInfo;
                    sOpenPicInfo.dwClientID = (uint)Count;
                    sOpenPicInfo.nOpenChannel = 0;
                    sOpenPicInfo.protocolType = ParkingCommunication.CameraSDK.ZNYKT13.NET_PROTOCOL_TYPE.NET_PROTOCOL_TCP;
                    sOpenPicInfo.funcPictureCallback = m_CapturePictureCallback;
                    sOpenPicInfo.pCallbackContext = (IntPtr)Count;

                    ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE errCode2 = ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_OpenPicture(
                       DVRIPAddress,
                       ushort.Parse(DVRPortNumber.ToString()),
                       "",
                       DVRUserName,
                       DVRPassword,
                       ref sOpenPicInfo,
                       out VideoInfo[Count].m_hOpenImg,
                       IntPtr.Zero);
                    if (errCode2 != ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE.HHERR_SUCCESS)
                    {
                        MessageBox.Show("打开图片通道失败【" + DVRIPAddress + "】失败!", "提示");
                    }
                    else
                    {

                    }
                }
                if (Model.bOut485 && Model.iAutoUpdateJiHao == 1)
                {
                    string SendSum = "";
                    string Jstrs = "";
                    int sum = 0;
                    SendSum += "A500BB4144";
                    Jstrs = "060102960064" + Model.Channels[Count].iCtrlID.ToString("X2") + "01";
                    byte[] array = CR.GetByteArray(Jstrs);
                    foreach (byte by in array)
                    {
                        sum += by;
                    }

                    sum = sum % 256;

                    short iNo = 0;
                    short cmd = 0xA5;
                    string strS = SendSum + sum.ToString("X2") + Jstrs + "FF";
                    //修改显示屏机号
                    byte[] bVZSend = CR.GetArray(strS);
                    ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.Sen485(VideoInfo[Count].m_hLogon, bVZSend);
                }
            }
            else
            {
                MessageBox.Show("连接相机【" + DVRIPAddress + "】失败!", "提示");
            }
        }
        #endregion


        #region ZNYKT14 芊熠
        int[] nCamId = new int[11];
        int iFGetImageCB = 0;

        ParkingCommunication.CameraSDK.ZNYKT14.MyClass.FGetImageCB fCb = null;

        private int FGetImageCB(int tHandle, uint uiImageId, IntPtr ptImageInfo, IntPtr ptPicInfo)
        {
            iFGetImageCB++;
            if (Model.iDetailLog == 1)
            {
                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****次数：" + iFGetImageCB.ToString() + "---");
            }
            if (ptImageInfo == IntPtr.Zero
                || ptPicInfo == IntPtr.Zero)
            {
                return 0;
            }

            ParkingCommunication.CameraSDK.ZNYKT14.MyClass.T_ImageUserInfo tImageInfo = (ParkingCommunication.CameraSDK.ZNYKT14.MyClass.T_ImageUserInfo)Marshal.PtrToStructure(ptImageInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT14.MyClass.T_ImageUserInfo));
            ParkingCommunication.CameraSDK.ZNYKT14.MyClass.T_PicInfo tPicInfo = (ParkingCommunication.CameraSDK.ZNYKT14.MyClass.T_PicInfo)Marshal.PtrToStructure(ptPicInfo, typeof(ParkingCommunication.CameraSDK.ZNYKT14.MyClass.T_PicInfo));
            for (int iVz = 0; iVz < nCamId.Length; iVz++)
            {
                if (nCamId[iVz] == tHandle)
                {
                    string strLprResult = "";
                    if (Model.iDetailLog == 1)
                    {
                        CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****次数：" + iFGetImageCB.ToString() + "车辆图像：" + tImageInfo.ucViolateCode.ToString());
                    }
                    //车辆图像
                    if (tImageInfo.ucViolateCode == 0)
                    {
                        string szLprResult = System.Text.Encoding.Default.GetString(tImageInfo.szLprResult).Replace("\0", "");
                        if (Model.iDetailLog == 1)
                        {
                            CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****次数：" + iFGetImageCB.ToString() + "车牌号码：" + szLprResult);
                        }

                        strLprResult = szLprResult;
                        switch (tImageInfo.ucVehicleSize)//车型
                        {
                            case 0:
                                {
                                    //labelVehicSize.Text = "未知车型";
                                    break;
                                }

                            case 1:
                                {
                                    // labelVehicSize.Text = "大型车";
                                    break;
                                }
                            case 2:
                                {
                                    //labelVehicSize.Text = "中型车";
                                    break;
                                }
                            case 3:
                                {
                                    //labelVehicSize.Text = "小型车";
                                    break;
                                }
                            case 4:
                                {
                                    //labelVehicSize.Text = "摩托车";
                                    break;
                                }
                            case 5:
                                {
                                    //labelVehicSize.Text = "行人";
                                    break;
                                }
                            default:
                                {
                                    // labelVehicSize.Text = "未知车型";
                                    break;
                                }
                        }
                        Model.iAutoColor = 0;
                        switch (tImageInfo.ucPlateColor)//车牌颜色
                        {
                            case 0:
                                Model.iAutoColor = 1;
                                iAutoColor[iVz] = 1;
                                break;
                            case 1:
                                Model.iAutoColor = 2;
                                iAutoColor[iVz] = 2;
                                break;
                            case 2:
                                Model.iAutoColor = 3;
                                iAutoColor[iVz] = 3;
                                break;
                            case 3:
                                //加
                                Model.iAutoColor = 4;
                                break;
                            case 4:
                                //加
                                Model.iAutoColor = 5;
                                break;
                            default:
                                iAutoColor[iVz] = 0;
                                Model.iAutoColor = 0;
                                break;
                        }

                        if (Model.iPersonVideo == 1 && iVz > 1)
                        {
                            iVz = iVz - 2;
                        }
                        if (Model.iPersonVideo == 1)
                        {
                            if (Model.Channels[iVz].iInOut == 0)
                            {
                                if (DateTime.Now < dtInOnPlate.AddSeconds(Convert.ToDouble(Model.iCarPosLedJH)))
                                {
                                    gsd.AddLog("在线监控" + ":FGetImageCB", "车牌号:" + strLprResult + "一个车道多个相机不处理车牌");
                                    return 0;
                                }
                                else
                                {
                                    dtInOnPlate = DateTime.Now;
                                }
                            }
                            else
                            {
                                if (DateTime.Now < dtoutOnPlate.AddSeconds(Convert.ToDouble(Model.iCarPosLedJH)))
                                {
                                    gsd.AddLog("在线监控" + ":FGetImageCB", "车牌号:" + strLprResult + "一个车道多个相机不处理车牌");
                                    return 0;
                                }
                                else
                                {
                                    dtoutOnPlate = DateTime.Now;
                                }
                            }
                        }

                        string strYFile = strHSPathStr;
                        string strYFileJpg = strHSPathStrJPG;
                        if (tPicInfo.ptPanoramaPicBuff != IntPtr.Zero && tPicInfo.uiPanoramaPicLen != 0)
                        {
                            ImageSaveHSPath(iVz, 0, "");
                            strYFile = strHSPathStr;
                            strYFileJpg = strHSPathStrJPG;
                            if (Model.iCarPosLed == 1)
                            {
                                DZHSSaveImage(tPicInfo.ptPanoramaPicBuff, strYFileJpg, (int)tPicInfo.uiPanoramaPicLen);
                            }
                            else
                            {
                                DZHSSaveImage(tPicInfo.ptPanoramaPicBuff, strYFile, (int)tPicInfo.uiPanoramaPicLen);
                            }
                        }

                        if (Model.iAutoPlateEn == 1)
                        {
                            Quene.ModelNode model = new Quene.ModelNode();
                            model.sDzScan = "";
                            model.iDzIndex = iVz;
                            model.strFile = strYFile;
                            model.strFileJpg = strYFileJpg;
                            model.strCPH = strLprResult;
                            LQueue.Enqueue(model);
                            if (Model.iDetailLog == 1)
                            {
                                CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****次数：" + iFGetImageCB.ToString() + "车牌号码：" + szLprResult + "--成功加入队列");
                            }
                            break;
                        }
                    }
                }
            }
            return 0;
        }

        public void SZQY(string DVRIPAddress, Int16 DVRPortNumber, string DVRUserName, string DVRPassword, System.Windows.Forms.PictureBox px, int Count)
        {
            nCamId[Count] = ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_AddCamera(DVRIPAddress);

            m_hLPRClient[Count] = nCamId[Count];//赋值摄像机开闸参数值
            m_nSerialHandle[Count] = nCamId[Count];//赋值摄像机开闸参数值
            int iRet = ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_ConnCamera(nCamId[Count], 30000, 10);
            if (iRet != 0)
            {
                ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_StopVideo(nCamId[Count]);
                MessageBox.Show("连接相机【" + DVRIPAddress + "】失败!", "提示");
                return;
            }
            iRet = ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_StartVideo(nCamId[Count], 0, px.Handle);
            if (iRet != 0)
            {
                ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_StopVideo(nCamId[Count]);
                ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_DisConnCamera(nCamId[Count]);
                MessageBox.Show("打开【" + DVRIPAddress + "】视频失败!", "提示");
                return;
            }
            if (Count == 0)
            {
                fCb = new ParkingCommunication.CameraSDK.ZNYKT14.MyClass.FGetImageCB(FGetImageCB);
                ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_RegImageRecv(fCb);
            }

            if (Model.bOut485 && Model.iAutoUpdateJiHao == 1)
            {
                string SendSum = "";
                string Jstrs = "";
                int sum = 0;
                SendSum += "A500BB4144";
                Jstrs = "060102960064" + Model.Channels[Count].iCtrlID.ToString("X2") + "01";
                byte[] array = CR.GetByteArray(Jstrs);
                foreach (byte by in array)
                {
                    sum += by;
                }

                sum = sum % 256;

                short iNo = 0;
                short cmd = 0xA5;
                string strS = SendSum + sum.ToString("X2") + Jstrs + "FF";
                //修改显示屏机号
                byte[] bVZSend = CR.GetArray(strS);
                int iRst = ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_TransRS485Data(nCamId[Count], Model.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
            }
        }
        #endregion


        /// <summary>
        /// 初始化摄像机
        /// </summary>
        /// <param name="strVideoTypeInit"></param>
        private void VoiceInit(string strVideoTypeInit)
        {
            if (strVideoTypeInit == "ZNYKTY2" || strVideoTypeInit == "ZNYKTY10")
            {
                disConnect = new ParkingCommunication.CameraSDK.ZNYKT10.fDisConnect(DisConnectEvent);
                ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHInit(disConnect, IntPtr.Zero);
                ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHSetEncoding(ParkingCommunication.CameraSDK.ZNYKT10.LANGUAGE_ENCODING.gb2312);//字符编码格式设置，默认为gb2312字符编码，如果为其他字符编码请设置 

                onlineMsg = new ParkingCommunication.CameraSDK.ZNYKT10.fHaveReConnect(OnlineEvent);
                ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHSetAutoReconnect(onlineMsg, IntPtr.Zero);
            }

            else if (strVideoTypeInit == "ZNYKTY3" || strVideoTypeInit == "ZNYKTY4")
            {
                bool m_bInitSDK = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_Init();
                bool bSetLogToFile = ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", 100);
            }
            else if (strVideoTypeInit == "ZNYKTY5" || strVideoTypeInit == "ZNYKTY15")
            {
                ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_Setup();
            }
            //else if (strVideoTypeInit == "ZNYKTY11")
            //{
            //    m_pCapCbY = new ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_CaptureCallback(CaptureCallbackY);
            //    int errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK;
            //    errCode = ParkingCommunication.CameraSDK.ZNYKT11.dvY.DV_InitSDK();
            //    if (errCode == ParkingCommunication.CameraSDK.ZNYKT11.dvY.DVERR_OK)
            //    {
            //        m_bSDKInited = true;
            //    }
            //}
            else if (strVideoTypeInit == "ZNYKTY8")
            {

            }
            else if (strVideoTypeInit == "ZNYKTY14" || strVideoTypeInit == "ZNYKTY11")
            {
                //深圳芊熠 
                ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_Init();

                int ptLen = 255;
                StringBuilder strVersion = new StringBuilder(ptLen);
                ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_GetSdkVersion(strVersion, ref ptLen);
            }
            else if (strVideoTypeInit == "ZNYKTY13")
            {
                //深圳亿维
                m_ChannelStreamCallback = new ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.ChannelStreamCallback(ChannelStreamCallback);
                m_CapturePictureCallback = new ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.PictureCallback(CapturePictureCallback);

                ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE errCodeYw = ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_Startup(hwndMain, WM_YW7000NET_COMMAND, 0, false, false, string.Empty);
                if (errCodeYw != ParkingCommunication.CameraSDK.ZNYKT13.HHERR_CODE.HHERR_SUCCESS)
                {
                }

                ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_InitSDK(hwndMain);
                ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_SetDecoderQulity(false);
            }
        }


        bool bVZTime = false;
        /// <summary>
        /// 视频处理
        /// </summary>
        public void Myinitcaptrure() // #wy 设置回调
        {
            bool[] bVideo = new bool[4];
            Model.g_IsHaveMiniCard = false;
            try
            {
                if (Model.iEnableNetVideo == 1) // 
                {
                    int VzCount = 0;
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        List<NetCameraSet> lstNCS = gsd.SelectVideo(Model.Channels[i].sIDAddress);
                        List<NetCameraSet> lstNCS1 = new List<NetCameraSet>();
                        if (Model.iPersonVideo == 1)
                        {
                            lstNCS1 = gsd.SelectVideo(Convert.ToInt32(Model.Channels[i].sPersonVideo));
                        }

                        for (int chanl = 0; chanl < 4; chanl++)
                        {
                            if (lstLblLaneName[chanl].Content.ToString() == Model.Channels[i].sInOutName && Model.Channels[i].iOnLine == 1)
                            {
                                if (Model.Channels[i].iCtrlID == Model.Channels[i].iOpenID)
                                {
                                    if (lstNCS.Count > 0)
                                    {
                                        string videotype = lstNCS[0].VideoType;
                                        strVideoType[chanl] = lstNCS[0].VideoType;
                                        if (VzCount == 0)
                                        {
                                            VoiceInit(strVideoType[chanl]);
                                            VzCount++;
                                        }
                                        if (Model.iPersonVideo == 1)
                                        {
                                            if (lstNCS1.Count > 0)
                                            {
                                                strVideoType[chanl + 2] = lstNCS1[0].VideoType;
                                                if (VzCount == 0)
                                                {
                                                    VoiceInit(strVideoType[chanl + 2]);
                                                    VzCount++;
                                                }
                                            }
                                        }

                                        Model.strVideoType = videotype;
                                        switch (videotype)
                                        {
                                            case "ZNYKTY2":
                                                if (VzCount == 0)
                                                {

                                                }
                                                if (Model.iPersonVideo == 1)
                                                {

                                                }
                                                break;
                                            case "ZNYKTY3":
                                                if (VzCount == 0)
                                                {
                                                    Preview(lstNCS[0].VideoIP, Convert.ToInt16(lstNCS[0].VideoPort), lstNCS[0].VideoUserName, lstNCS[0].VideoPassWord, lstPicVideo[chanl], i, lstNCS[0].VideoType);
                                                }
                                                if (Model.iPersonVideo == 1)
                                                {
                                                    if (lstNCS1.Count > 0)
                                                    {
                                                        if (lstNCS[0].VideoIP.Length > 10)
                                                        {
                                                            Preview(lstNCS1[0].VideoIP, Convert.ToInt16(lstNCS1[0].VideoPort), lstNCS1[0].VideoUserName, lstNCS1[0].VideoPassWord, lstPicVideo[chanl + 2], i + 2, lstNCS1[0].VideoType);
                                                        }
                                                        else
                                                        {
                                                            strVideoType[chanl + 2] = "";
                                                        }
                                                    }
                                                }
                                                break;
                                            case "ZNYKTY4":
                                                if (VzCount == 0)
                                                {

                                                }
                                                if (Model.iPersonVideo == 1)
                                                {

                                                }
                                                break;
                                            case "ZNYKTY5":
                                            case "ZNYKTY15":
                                                //if (VzCount == 0)
                                                //{
                                                bVZTime = true;
                                                dicListIp[lstNCS[0].VideoIP] = chanl;
                                                lstBtnManual[i].IsEnabled = true;
                                                if (Model.iCPHPhoto == 1)
                                                {
                                                    lstWfhSmallCarNo[i].Visibility = Visibility.Visible;
                                                    //lstPicSmallCarNo[i].Visible = true;
                                                }
                                                Model.strVideoType = "ZNYKTY5";
                                                VzPreview(lstNCS[0].VideoIP, Convert.ToInt16(lstNCS[0].VideoPort), lstNCS[0].VideoUserName, lstNCS[0].VideoPassWord, lstPicVideo[chanl], i, lstNCS[0].VideoType);
                                                //}
                                                if (Model.iPersonVideo == 1)
                                                {
                                                    if (lstNCS1.Count > 0)
                                                    {
                                                        dicListIp[lstNCS[0].VideoIP] = chanl;
                                                        lstBtnManual[i + 2].Visibility = Visibility.Visible;
                                                        VzPreview(lstNCS1[0].VideoIP, Convert.ToInt16(lstNCS1[0].VideoPort), lstNCS1[0].VideoUserName, lstNCS1[0].VideoPassWord, lstPicVideo[chanl + 2], i + 2, lstNCS1[0].VideoType);
                                                    }
                                                }
                                                break;
                                            case "ZNYKTY8":
                                                if (VzCount == 0)
                                                {

                                                }
                                                if (Model.iPersonVideo == 1)
                                                {

                                                }
                                                break;
                                            case "ZNYKTY10": // 进行设置回调callback
                                                DHPreview(lstNCS[0].VideoIP, lstNCS[0].VideoPort.ToString(), lstNCS[0].VideoUserName, lstNCS[0].VideoPassWord, lstPicVideo[chanl], i, lstNCS[0].VideoType);
                                                break;
                                            //case "ZNYKTY11":
                                            //    //bZNYKT6 = true;
                                            //    HSNetY(lstNCS[0].VideoIP, Convert.ToInt16(lstNCS[0].VideoPort), lstNCS[0].VideoUserName, lstNCS[0].VideoPassWord, lstPicVideo[chanl], i);
                                            //    break;
                                            case "ZNYKTY13":
                                                //if (VzCount == 0)
                                                {
                                                    Model.strVideoType = "ZNYKTY13";
                                                    SZYW(lstNCS[0].VideoIP, Convert.ToInt16(lstNCS[0].VideoPort), lstNCS[0].VideoUserName, lstNCS[0].VideoPassWord, lstPicVideo[chanl], i);
                                                }
                                                if (Model.iPersonVideo == 1)
                                                {

                                                }
                                                break;
                                            case "ZNYKTY11":
                                            case "ZNYKTY14":

                                                //if (VzCount == 0)
                                                {
                                                    SZQY(lstNCS[0].VideoIP, Convert.ToInt16(lstNCS[0].VideoPort), lstNCS[0].VideoUserName, lstNCS[0].VideoPassWord, lstPicVideo[chanl], i);
                                                }
                                                Model.strVideoType = "ZNYKTY14";
                                                if (Model.iPersonVideo == 1)
                                                {
                                                    if (lstNCS1.Count > 0)
                                                        SZQY(lstNCS1[0].VideoIP, Convert.ToInt16(lstNCS1[0].VideoPort), lstNCS1[0].VideoUserName, lstNCS1[0].VideoPassWord, lstPicVideo[chanl + 2], i + 2);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                gsd.AddLog("在线监控" + ":Myinitcaptrure", ex.Message + "\r\n" + ex.StackTrace);
            }
        }


        string strHSPathStr = "";
        string strHSPathStrJPG = "";

        /// <summary>
        /// 抓拍图像处理
        /// </summary>
        /// <param name="modulus"></param>
        /// <param name="MycheRen"></param>
        private void Mycaptureconvert(int modulus, int MycheRen)
        {
            try
            {
                if (Model.iEnableNetVideo == 1)
                {
                    for (int chanl = 0; chanl < 4; chanl++)
                    {
                        if (lstLblLaneName[chanl].Content.ToString() == Model.Channels[modulus].sInOutName)
                        {
                            string videoType = strVideoType[chanl + 2];
                            if (MycheRen == 1)
                            {
                                switch (videoType)
                                {
                                    case "ZNYKTY2":
                                        ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHCapturePicture(pRealPlayHandle[modulus], filesJpg);
                                        break;
                                    case "ZNYKTY3":
                                        ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_SetCapturePictureMode(1);
                                        ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle[chanl + 2], picFileName);
                                        break;
                                    case "ZNYKTY5":
                                    case "ZNYKTY15":

                                        ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_GetSnapShootToJpeg2(m_hLPRPlay[chanl + 2], filesJpg, 100);
                                        break;
                                    case "ZNYKTY13":
                                        //bool tr1 = ParkingCommunication.CameraSDK.ZNYKT13.CHCNetSDK.NET_DVR_SetCapturePictureMode(1);
                                        //bool tr = CR.CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle[chanl + 2], Files);
                                        break;

                                    case "ZNYKTY11":
                                    case "ZNYKTY14":
                                        if (Model.iCarPosLed == 1)
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_SaveImageToJpeg(nCamId[chanl + 2], filesJpg);
                                        }
                                        else
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_SaveImageToJpeg(nCamId[chanl + 2], picFileName);
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                videoType = strVideoType[chanl];
                                switch (videoType)
                                {
                                    case "ZNYKTY2":
                                        ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHCapturePicture(pRealPlayHandle[modulus], picFileName);
                                        break;
                                    case "ZNYKTY3":
                                        ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_SetCapturePictureMode(1);

                                        if (Model.iCarPosLed == 1)
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle[chanl], filesJpg);
                                        }
                                        else
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT3.CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle[chanl], picFileName);
                                        }

                                        break;
                                    case "ZNYKTY5":
                                    case "ZNYKTY15":
                                        if (Model.iCarPosLed == 1)
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_GetSnapShootToJpeg2(m_hLPRPlay[chanl], filesJpg, 10);
                                        }
                                        else
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_GetSnapShootToJpeg2(m_hLPRPlay[chanl], picFileName, 100);
                                        }

                                        break;

                                    case "ZNYKTY10":
                                        if (Model.iCarPosLed == 1)
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHCapturePicture(pRealPlayHandle[modulus], filesJpg);
                                        }
                                        else
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHCapturePicture(pRealPlayHandle[modulus], picFileName);
                                        }
                                        break;
                                    case "ZNYKTY13":

                                        IntPtr iJpg = IntPtr.Zero;
                                        int iJpgLen = 0;
                                        ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_CaptureOnePicture(ushort.Parse(modulus.ToString()), out iJpg, out iJpgLen);

                                        if (Model.iCarPosLed == 1)
                                        {
                                            DZHSSaveImage(iJpg, filesJpg, iJpgLen);

                                        }
                                        else
                                        {
                                            DZHSSaveImage(iJpg, picFileName, iJpgLen);

                                        }
                                        break;

                                    case "ZNYKTY11":
                                    case "ZNYKTY14":
                                        if (Model.iCarPosLed == 1)
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_SaveImageToJpeg(nCamId[modulus], filesJpg);
                                        }
                                        else
                                        {
                                            ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_SaveImageToJpeg(nCamId[modulus], picFileName);
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":Mycaptureconvert", ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        #endregion


        #region 图片
        /// <summary>
        /// 自动删除过期图片  2015-11-21  半个小时内删除
        /// </summary>
        private void AutoDelImage()
        {
            string dirFullName = "";  //完整路径的目录名
            string dirName = "";      //仅目录名
            try
            {
                while (true)
                {
                    Thread.Sleep(60000);  //每一分钟检测一次

                    if (DateTime.Now.Date != dLastDelImageDate.Date)   //今天没有执行过
                    {
                        //自动删除的时间点
                        DateTime dAutoDelTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Model.iImageAutoDelTime, 0, 0);

                        TimeSpan span = DateTime.Now.Subtract(dAutoDelTime);   //与当前时间相差多少分钟
                        //Console.WriteLine(span.TotalMinutes);

                        if (span.TotalMinutes > 0 && span.TotalMinutes <= 30)  //相差30分钟以内才执行
                        {
                            if (Model.sImageSavePath.Substring(Model.sImageSavePath.Length - 1) != @"\")
                            {
                                Model.sImageSavePath = Model.sImageSavePath + @"\";
                            }
                            string path = System.IO.Path.Combine(Model.sImageSavePath, @"CaptureImage\" + Model.stationID);
                            System.IO.DirectoryInfo parentdi = new System.IO.DirectoryInfo(path);
                            foreach (System.IO.DirectoryInfo dir in parentdi.GetDirectories("*"))  //访问所有子目录
                            {
                                Console.WriteLine(dir.FullName);
                                dirFullName = dir.FullName;  //完整路径的目录名
                                dirName = dir.Name;          //仅目录名
                                string sDirDate;                    //字符串型的目录日期
                                DateTime dDirDate;                  //日期型的目录日期  
                                if (dirName.Length == 8)
                                {
                                    //把目录名 转换成 日期
                                    sDirDate = dirName.Substring(0, 4) + "-" + dirName.Substring(4, 2) + "-" + dirName.Substring(6, 2);

                                    //判断是否为正确的日期类型，如果不是则表示不是图片目录
                                    if (DateTime.TryParse(sDirDate, out dDirDate))      //是日期类型
                                    {
                                        //判断是否为指定天数以前的，是则删除
                                        span = DateTime.Now.Subtract(dDirDate);
                                        if (span.TotalDays > Model.iImageSaveDays)
                                        {
                                            System.IO.Directory.Delete(dirFullName, true);

                                            gsd.AddLog("自动删除" + Model.iImageSaveDays + "天以前的图片", dirFullName + " 目录删除成功");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(dirName + ": 不是正确的日期类型，不是图片目录");
                                    }
                                }
                            }

                            //原图目录 与图片目录不一样，则也删除
                            //if (Model.sImageSavePath != Model.sImageSavePathBD)
                            //{
                            //    parentdi = new System.IO.DirectoryInfo(Model.sImageSavePathBD);
                            //    foreach (System.IO.DirectoryInfo dir in parentdi.GetDirectories("*"))  //访问所有子目录
                            //    {
                            //        //Console.WriteLine(dir.FullName);
                            //        dirFullName = dir.FullName;  //完整路径的目录名
                            //        dirName = dir.Name;          //仅目录名
                            //        string sDirDate;                    //字符串型的目录日期
                            //        DateTime dDirDate;                  //日期型的目录日期  

                            //        if (dirName.Length == 8)
                            //        {
                            //            //把目录名 转换成 日期
                            //            sDirDate = dirName.Substring(0, 4) + "-" + dirName.Substring(4, 2) + "-" + dirName.Substring(6, 2);

                            //            //判断是否为正确的日期类型，如果不是则表示不是图片目录
                            //            if (DateTime.TryParse(sDirDate, out dDirDate))      //是日期类型
                            //            {
                            //                //判断是否为指定天数以前的，是则删除
                            //                span = DateTime.Now.Subtract(dDirDate);
                            //                if (span.TotalDays > Model.iImageSaveDays)
                            //                {
                            //                    System.IO.Directory.Delete(dirFullName, true);
                            //                    Console.WriteLine(dirFullName + " 删除成功");
                            //                    gsd.AddLog("自动删除" + Model.iImageSaveDays + "天以前的图片", dirFullName + " 目录删除成功");
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Console.WriteLine(dirName + ": 不是正确的日期类型，不是图片目录");
                            //            }
                            //        }
                            //    }
                            //}

                            dLastDelImageDate = DateTime.Now.Date;  //保存执行时间
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                gsd.AddLog("自动删除" + Model.iImageSaveDays + "天以前的图片", "出错：" + dirFullName + " " + ex.Message);
                return;
            }
        }

        /// <summary>
        /// 图象存盘的路径
        /// </summary>
        /// <param name="modulus"></param>
        /// <param name="MycheRen"></param>
        private void ImageSavePath(int modulus, int MycheRen)
        {
            try
            {
                DateTime MyCapDateTime;
                string PathStr = "";
                //string PathStrJPG = "";

                filesJpg = "";
                picFileName = "";
                if (MycheRen == 0)
                {
                    inOutPic[modulus] = filesJpg;
                }
                else
                {
                    inOutPicR[modulus] = filesJpg;
                }

                if (Model.sImageSavePath.Substring(Model.sImageSavePath.Length - 1) != @"\")
                {
                    Model.sImageSavePath = Model.sImageSavePath + @"\";
                }
                //if (Model.sImageSavePathBD.Substring(Model.sImageSavePathBD.Length - 1) != @"\")
                //{
                //    Model.sImageSavePathBD = Model.sImageSavePathBD + @"\";
                //}


                MyCapDateTime = DateTime.Now;
                PathStr = Model.sImageSavePath + @"CaptureImage\" + Model.stationID.ToString() + @"\" + MyCapDateTime.ToString("yyyyMMdd");
                if (System.IO.Directory.Exists(PathStr) == false)
                {
                    System.IO.Directory.CreateDirectory(PathStr);
                }
                //PathStrJPG = Model.sImageSavePathBD + @"CaptureImage\" + Model.stationID + @"\" + MyCapDateTime.ToString("yyyyMMdd");
                //if (System.IO.Directory.Exists(PathStrJPG) == false)
                //{
                //    System.IO.Directory.CreateDirectory(PathStrJPG);
                //}
                string strUID = Guid.NewGuid().ToString();
                if (InOut == 0)
                {
                    //picFileName = PathStrJPG + @"\" + strUID + MyCapDateTime.ToString("yyyyMMddHHmmss") + (MycheRen == 0 ? "c" : "r") + "in.bmp";
                    filesJpg = PathStr + @"\" + strUID + MyCapDateTime.ToString("yyyyMMddHHmmss") + (MycheRen == 0 ? "c" : "r") + "in.jpg";
                    if (MycheRen == 0)
                    {
                        inOutPic[modulus] = filesJpg;
                    }
                    else
                    {
                        inOutPicR[modulus] = filesJpg;
                    }
                }
                else
                {
                    //picFileName = PathStrJPG + @"\" + strUID + MyCapDateTime.ToString("yyyyMMddHHmmss") + (MycheRen == 0 ? "c" : "r") + "go.bmp";
                    filesJpg = PathStr + @"\" + strUID + MyCapDateTime.ToString("yyyyMMddHHmmss") + (MycheRen == 0 ? "c" : "r") + "go.jpg";
                    if (MycheRen == 0)
                    {
                        inOutPic[modulus] = filesJpg;
                    }
                    else
                    {
                        inOutPicR[modulus] = filesJpg;
                    }
                }
            }
            catch (Exception ex)
            {

                gsd.AddLog("在线监控" + ":ImageSavePath", ex.Message + "\r\n" + ex.StackTrace);

                //this.Invoke((EventHandler)delegate
                //{
                //    txbOperatorInfo.Text = ex.Message;
                //});
            }
        }

        /// <summary>
        /// 图象存盘的路径
        /// </summary>
        /// <param name="modulus"></param>
        /// <param name="MycheRen"></param>
        private void ImageSaveHSPath(int modulus, int MycheRen, string strCPH)
        {
            try
            {
                DateTime MyCapDateTime;
                string PathStr = "";
                //string PathStrJPG = "";

                strHSPathStrJPG = "";
                strHSPathStr = "";

                if (Model.sImageSavePath.Substring(Model.sImageSavePath.Length - 1) != @"\")
                {
                    Model.sImageSavePath = Model.sImageSavePath + @"\";
                }
                //if (Model.sImageSavePathBD.Substring(Model.sImageSavePathBD.Length - 1) != @"\")
                //{
                //    Model.sImageSavePathBD = Model.sImageSavePathBD + @"\";
                //}

                MyCapDateTime = DateTime.Now;
                PathStr = Model.sImageSavePath + @"CaptureImage\" + Model.stationID + @"\" + MyCapDateTime.ToString("yyyyMMdd");
                if (System.IO.Directory.Exists(PathStr) == false)
                {
                    System.IO.Directory.CreateDirectory(PathStr);
                }
                //PathStrJPG = Model.sImageSavePathBD + @"CaptureImage\" + Model.stationID + @"\" + MyCapDateTime.ToString("yyyyMMdd");
                //if (System.IO.Directory.Exists(PathStrJPG) == false)
                //{
                //    System.IO.Directory.CreateDirectory(PathStrJPG);
                //}

                string strUID = Guid.NewGuid().ToString();

                //strHSPathStr = PathStrJPG + @"\" + strUID + MyCapDateTime.ToString("yyyyMMddHHmmss") + (MycheRen == 0 ? "c" : "r") + strCPH + ".bmp";
                strHSPathStrJPG = PathStr + @"\" + strUID + MyCapDateTime.ToString("yyyyMMddHHmmss") + (MycheRen == 0 ? "c" : "r") + strCPH + ".jpg";
                //if (strCPH.Length == 0)
                //{
                //    if (MycheRen == 0)
                //    {
                //        inOutPic[modulus] = strHSPathStrJPG;
                //    }
                //    else
                //    {
                //        inOutPicR[modulus] = strHSPathStrJPG;
                //    }
                //}

            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":ImageSaveHSPath", ex.Message + "\r\n" + ex.StackTrace);
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txbOperatorInfo.Text = ex.Message;
                            }));
            }
        }

        private void LoadImageGoCome()
        {
            try
            {
                if (cardType[modulus].Substring(0, 1) != "手" && Model.iImageSave == 1)
                {
                    if (Model.iVideo4 == 1)
                    {
                        //4路视频暂时不处理
                    }
                    if (picFileName.Length == 0)//没有路劲时
                    {

                    }
                    else
                    {
                    }
                    if (inPic[0] == "")
                    { }
                    else
                    {
                        if (inPic[0] != null)
                        {
                            if (System.IO.File.Exists(inPic[0]))
                            {
                                if (Model.iVideo4 == 1)
                                {
                                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(inPic[0]), 400, 300);
                                    ptr4.Image = bm;
                                    //pictureBox4.Image = bm;
                                    localImageIn = inOutPic[modulus];
                                }
                                else
                                {
                                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(inPic[0]), lstPicVideo[2].Width, lstPicVideo[2].Height);
                                    lstPicVideo[2].Image = bm;
                                    localImageIn = inPic[0];

                                }
                            }
                        }

                    }
                    if (inOutPic[modulus] == "")
                    { }
                    else
                    {
                        if (inOutPic[modulus] != null)
                        {
                            if (System.IO.File.Exists(inOutPic[modulus]))
                            {

                                if (Model.iVideo4 == 1)
                                {
                                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(inOutPic[modulus]), 400, 300);
                                    ptr3.Image = bm;
                                    localImageOut = inOutPic[modulus];
                                }
                                else
                                {
                                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(inOutPic[modulus]), lstPicVideo[3].Width, lstPicVideo[3].Height);
                                    lstPicVideo[3].Image = bm;
                                    localImageOut = inOutPic[modulus];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":LoadImageGoCome", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void LoadCarGoOut()
        {
            try
            {
                recordCount = 0;
                sInGate = "";
                sInUser = "";

                //if (lblOutTime.Content.ToString().Length == 0)
                //{
                //    lblOutTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //}

                if (monitor.OutTime.ToString().Length == 0)
                {
                    monitor.OutTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (Model.bYXCFJL)
                {
                    return;
                }
                List<CarOut> Outds = gsd.GetOutCardNo2(Convert.ToDateTime(monitor.InTime), Convert.ToDateTime(monitor.OutTime), monitor.CardNo, Model.iParkingNo, Model.Channels[modulus].iBigSmall, charge[modulus].ToString());
                if (Outds.Count > 0)
                {
                    recordCount = 1;
                    sInGate = Outds[0].InGateName;
                    sInUser = Outds[0].InOperator;
                }
                Outds.Clear();


                if (monitor.CardType == "操作卡")
                //if (lblCardType.Content.ToString() == "操作卡")
                {
                    recordCount = 0;
                }
                //loadCar();
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":LoadCarGoOut", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 加载图片到控件
        /// </summary>
        /// <param name="path">图片绝对路径</param>
        /// <param name="pic">控件名</param>
        /// <param name="inout">进出标识</param>
        public void loadPic(string path, System.Windows.Forms.PictureBox pic, int inout)
        {
            if (path != "")
            {
                if (System.IO.File.Exists(path))
                {
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(path), pic.Width, pic.Height);
                    pic.Image = bm;

                    if (inout == 0)
                    {
                        localImageIn = path;
                    }
                    else
                    {
                        localImageOut = path;
                    }
                }
                else
                {
                    if (path.Contains(Model.sImageSavePath))
                    {
                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                        pic.Image = bm;

                        if (inout == 0)
                        {
                            localImageIn = "";
                            txbOperatorInfo.Text = "入口图片路径不存在!";
                        }
                        else
                        {
                            localImageOut = "";
                            txbOperatorInfo.Text = "出口图片路径不存在!";
                        }
                    }
                    else
                    {
                        if (path.Substring(0, 12) == "CaptureImage")
                        {
                            if (System.IO.File.Exists(Model.sImageSavePath + path))
                            {
                                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(Model.sImageSavePath + path), pic.Width, pic.Height);
                                pic.Image = bm;

                                if (inout == 0)
                                {
                                    localImageIn = Model.sImageSavePath + path;
                                }
                                else
                                {
                                    localImageOut = Model.sImageSavePath + path;
                                }
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

                                            if (inout == 0)
                                            {
                                                localImageIn = ot.ToString();
                                            }
                                            else
                                            {
                                                localImageOut = ot.ToString();
                                            }
                                        }
                                        else
                                        {
                                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                                            pic.Image = bm;

                                            if (inout == 0)
                                            {
                                                localImageIn = "";
                                                txbOperatorInfo.Text = "入口图片路径不存在!";
                                            }
                                            else
                                            {
                                                localImageOut = "";
                                                txbOperatorInfo.Text = "出口图片路径不存在!";
                                            }
                                        }
                                    }));
                                }, Model.sImageSavePath + path);
                            }
                        }
                        else
                        {
                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                            pic.Image = bm;

                            if (inout == 0)
                            {
                                localImageIn = "";
                                txbOperatorInfo.Text = "入口图片路径不存在!";
                            }
                            else
                            {
                                localImageOut = "";
                                txbOperatorInfo.Text = "出口图片路径不存在!";
                            }
                        }
                    }
                }
            }
            else
            {
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, pic.Width, pic.Height);
                pic.Image = bm;

                if (inout == 0)
                {
                    localImageIn = "";
                    txbOperatorInfo.Text = "入口图片路径不存在!";
                }
                else
                {
                    localImageOut = "";
                    txbOperatorInfo.Text = "出口图片路径不存在!";
                }
            }
        }
        #endregion


        #region 显示屏
        private void SurplusCPH()
        {
            try
            {
                SedBll senbll = new SedBll(Model.Channels[modulus].sIP, 1007, 1005);
                List<LedSetting> dtS = gsd.GetSurplusCar(Model.Channels[modulus].iCtrlID);

                string CPH = monitor.CarNo;
                string strCardType = CR.GetCardType(monitor.CardType, 0).Substring(0, 3);
                //string CPH = lblCarNo.Content.ToString();
                //string strCardType = CR.GetCardType(lblCardType.Content.ToString(), 0).Substring(0, 3);

                foreach (var dr in dtS)
                {
                    string showWay = dr.ShowWay;
                    string SendSum = "";
                    string StrSum = "";
                    bool bMW = false;//2016-09-08 th
                    if (showWay.Contains("3"))//默认显示空车位
                    {

                        if (showWay.Contains("4"))//是否显示车牌
                        {
                            if (CPH.Length == 7 && CPH != "0000000" && CPH != "6666666" && carNoCmd != "京000000" && CPH != "8888888" && CPH != "")
                            {
                                StrSum = CPH;
                            }
                        }
                        else  //不显示车牌
                        {
                            int iCoutRemainCar = 0;
                            if (Model.iFreeCardNoInPlace == 1 && strCardType.Substring(0, 3) == "Fre")
                            {
                                iCoutRemainCar = Convert.ToInt32(txbSurplusCarCount.Text);
                            }
                            else
                            {
                                if (Model.Channels[modulus].iInOut == 0) //入场
                                {
                                    iCoutRemainCar = summary0.SurplusCarCount - 1;
                                }
                                else //出场
                                {
                                    iCoutRemainCar = summary0.SurplusCarCount + 1;
                                }
                            }

                            if (iCoutRemainCar < 1)
                            {
                                if (dr.Pattern == "2")
                                {
                                    StrSum = "0000";
                                    bMW = false;
                                }
                                else
                                {
                                    StrSum = "车位已满 谢谢！";
                                    bMW = true;
                                }

                            }
                            else
                            {
                                bMW = false;
                                if (dr.CPHEndStr == "")
                                {
                                    if (dr.Pattern == "2")
                                    {
                                        StrSum = iCoutRemainCar.ToString("0000");

                                    }
                                    else if (dr.Pattern == "8")
                                    {
                                        StrSum = "剩余车位:" + iCoutRemainCar.ToString("000");
                                    }
                                    else
                                    {
                                        StrSum = "空车位:" + iCoutRemainCar.ToString("000");
                                    }
                                }
                                else
                                {
                                    StrSum = dr.CPHEndStr + iCoutRemainCar.ToString("000");
                                }

                            }


                        }

                    }
                    else   //默认显示时间或广告
                    {
                        if (showWay.Contains("4"))//是否显示车牌
                        {
                            if (CPH.Length == 7 && CPH != "0000000" && CPH != "6666666" && carNoCmd != "京000000" && CPH != "8888888" && CPH != "")
                            {
                                StrSum = CPH;
                            }
                        }
                    }

                    if (showWay.Contains("6"))//收费金额
                    {
                        if ((strCardType == "Tmp" || strCardType == "Str") && Model.Channels[modulus].iInOut == 1)
                        {
                            string money = "此次收费" + monitor.Charge.ToString() == "" ? "0" : monitor.Charge.ToString() + "元";
                            //string moeny = "此次收费" + Convert.ToDecimal(lblCharge.Content.ToString() == "" ? "0" : lblCharge.Content.ToString()).ToString() + "元";
                            StrSum += StrSum != "" ? " " + money : money;
                            if (strCardType == "Str")
                            {
                                StrSum += "余额" + monitor.Balance.ToString() + "元";
                                //StrSum += " 余额" + Convert.ToDecimal(lblBalance.Content.ToString()).ToString() + "元";
                            }
                        }
                    }
                    if (showWay.Contains("5") && (showWay.Contains("6") == false || (strCardType != "Tmp" && strCardType != "Str")))//是否发送车牌后缀
                    {
                        StrSum += StrSum != "" ? " " + dr.CPHEndStr : dr.CPHEndStr;
                    }
                    if (StrSum != "")
                    {
                        string Jstrs = "";

                        if (bMW)
                        {
                            Jstrs = "01" + dr.Speed + "00" + dr.Color + dr.SumTime + CR.GetStrTo16(StrSum);//移动方式： 速度,单幅停留时间,颜色,总显示时间
                        }
                        else
                        {
                            Jstrs = dr.Move + dr.Speed + dr.StopTime + dr.Color + dr.SumTime + CR.GetStrTo16(StrSum);//移动方式： 速度,单幅停留时间,颜色,总显示时间
                        }

                        int sum = 0;
                        byte[] array = CR.GetByteArray(Jstrs);
                        foreach (byte by in array)
                        {
                            sum += by;
                        }
                        sum = sum % 256;


                        SendSum = "CC" + Convert.ToInt32(dr.SurplusID).ToString("X2") + "BB5154" + sum.ToString("X2") + Jstrs + "FF";

                        if (Model.Channels[modulus].iXieYi == 1)
                        {
                            //!!!
                            SedBll senbll0 = new SedBll(Model.Channels[modulus].sIP, 1007, 1005);
                            //senbll0.SurplusCtrlLedShow(axznykt_1, Model.Channels[modulus].iCtrlID, Convert.ToInt16(dr["SurplusID"].ToString()), Model.Channels[modulus].sIP, SendSum, sum, Jstrs, m_nSerialHandle[modulus], Model.Channels[modulus].iXieYi);
                            senbll0.SurplusCtrlLedShow(Convert.ToByte(Model.Channels[modulus].iCtrlID), SendSum, Model.Channels[modulus].iXieYi);
                        }
                    }
                }
                lostFlag = 0;
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":SurplusCPH", ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        #endregion


        #region 入出场记录刷新
        private Dictionary<string, object> TextString(string S0, string S1, string S2, TextBox t0, TextBox t1, TextBox t2)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (t0.Text != "")
            {
                dic[S0] = "%" + t0.Text + "%";
            }
            if (t1.Text != "")
            {
                dic[S1] = "%" + t1.Text + "%";
            }
            if (t2.Text != "")
            {
                dic[S2] = "%" + t2.Text + "%";
            }
            return dic;
        }

        /// <summary>
        /// 绑定记录
        /// </summary>
        private void GetBinInOut()  //2015-07-07
        {
            try
            {
                List<CarOut> dtBin;
                List<CarIn> dtBinOut;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            dic = TextString("CPH", "OutGateName", "OutOperator", txtSelectOutCarNo, txtSelectOutName, txtSelectOutOperator);
                        }));

                List<CarOut> dtSo = gsd.GetOutGate(dic);

                dtBin = dgvCharge.ItemsSource as List<CarOut>;
                //if (dtBin != null)
                //{
                //    dtBin.Clear();
                //    if (dtSo.Count > 0)
                //    {
                //        foreach (var dr in dtSo)
                //        {
                //            dtBin.Add(dr);
                //        }
                //    }
                //    dtSo.Clear();
                //}
                //else
                //{
                //dgvCharge.ItemsSource = dtSo;

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        dgvCharge.ItemsSource = dtSo;
                    }));
                //dtSo.Clear();
                //}




                dic = new Dictionary<string, object>();

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                       new Action(() =>
                       {
                           dic = TextString("CPH", "InGateName", "InOperator", txtSelectCarNo, txtSelectInName, txtSelectInOperator);
                       }));

                List<CarIn> dtSoOut = gsd.GetInGate(dic);
                //DataTable dtSoOut = bll.GetInGate(str);
                dtBinOut = dgvCar.ItemsSource as List<CarIn>;
                //if (dtBinOut != null)
                //{
                //    dtBinOut.Clear();
                //    if (dtSoOut.Count > 0)
                //    {
                //        foreach (var dr in dtSoOut)
                //        {
                //            dtBinOut.Add(dr);
                //        }

                //    }
                //    dtSoOut.Clear();
                //}
                //else
                //{
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        dgvCar.ItemsSource = dtSoOut;
                    }));
                //dtSoOut.Clear();
                //}
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控" + ":GetBinInOut", ex.Message + "----" + ex.StackTrace);
                //log.Add("在线监控" + ":GetBinInOut", ex.Message + "----" + ex.StackTrace);
            }
        }
        #endregion
        #endregion


        #region Events
        #region Windows
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Model.bStart == false)
            //{
            //    MessageBox.Show("请插入加密狗", "提示");
            //    bThreadReadExitOK = true;
            //    bThreadReadTimer3ExitOK = true;
            //    this.Close();
            //    return;
            //}
            for (int i = 0; i < rawVI.Length; i++)
                rawVI[i] = lstWfhSmallCarNo[i].Visibility;

            //// 设置全屏
            this.WindowState = System.Windows.WindowState.Normal;
            this.ResizeMode = ResizeMode.NoResize;


            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            hwndMain = new WindowInteropHelper(this).Handle;
            (PresentationSource.FromVisual(this) as HwndSource).AddHook(new HwndSourceHook(this.WndProc));

            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 1);
            dTimer.Start();

            //dTimer1.Tick += new EventHandler(dTimer1_Tick);
            //dTimer1.Interval = new TimeSpan(0, 0, 30);
            //dTimer1.Start();

            int ret = (new Request()).KeppAlive(Model.token);
            if (ret / 1000 > 0)
            {
                int time = ret / 1000;
                if (time <= Model.NetDelayey)
                {
                    MessageBox.Show("心跳频率小于网络延时，请联系管理员", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                    timer.Interval = ret - Model.NetDelayey * 1000;
                    timer.Start();
                }
            }


            if (Model.iImageAutoDel == 1)
            {
                Thread threadAutoDelImage = new Thread(new ThreadStart(AutoDelImage));//开辟一个新的线程
                threadAutoDelImage.IsBackground = true;
                threadAutoDelImage.Name = "threadAutoDelImage";
                threadAutoDelImage.Start();
            }


            if (Model.bAppEnable && Model.iAutoUpdateJiHao == 1)
            {
                AutoModiPingJH();
            }

            //开启主线程
            fThread = new Thread(new ThreadStart(TimerGet));//开辟一个新的线程(对于车辆进场来进行相应的处理)
            fThread.IsBackground = true;
            fThread.Start();

            // TimerGet();//开启主线程
            fThreadtimer3 = new Thread(new ThreadStart(Timer3Get));//开辟一个新的线程 定时的刷新车场信息
            fThreadtimer3.IsBackground = true;
            fThreadtimer3.Start();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (Model.iExitOnlineByPwd > 0)
                {
                    ParkingPassword pp = new ParkingPassword();
                    pp.Owner = this;
                    if (pp.ShowDialog() != true)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                bExit = true;

                this.Cursor = Cursors.Wait;//等待
                //while (bThreadReadExitOK == false || bThreadReadTimer3ExitOK == false)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    Thread.Sleep(100);
                //}

                if (null != fThread && null != fThreadtimer3)
                {
                    fThread.Abort();
                    fThreadtimer3.Abort();
                }

                Model.Quit_Flag = false;

                if (usbHid != null && usbHid.deviceOpened == true)  //2015-08-20
                {
                    usbHid.CloseDevice();
                }

                gsd.AddLog("在线监控", "退出");
                if (lstLblLaneName.Count == 0)
                {
                    return;
                }
                for (int Count = 0; Count < Model.iChannelCount; Count++)
                {
                    for (int chanl = 0; chanl < 4; chanl++)
                    {
                        if (lstLblLaneName[chanl].Content.ToString() == Model.Channels[Count].sInOutName)
                        {
                            List<NetCameraSet> lstNCS = gsd.SelectVideo(Model.Channels[Count].sIDAddress);
                            if (lstNCS.Count > 0)
                            {
                                if (lstNCS[0].VideoType == "ZNYKTY2")
                                {
                                    // CR.DHClient.DHStopRealPlay(pRealPlayHandle[Count]);
                                }
                                else if (lstNCS[0].VideoType == "ZNYKTY3")
                                {
                                    //ParkingCommunication.CameraSDK.ZNYKT3.NET_DVR_StopRealPlay(m_lRealHandle[Count]);
                                }
                                else if (lstNCS[0].VideoType == "ZNYKTY4")
                                {
                                    //CR.CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle[Count]);
                                }

                                else if (lstNCS[0].VideoType == "ZNYKTY5" || lstNCS[0].VideoType == "ZNYKTY15")
                                {
                                    m_hLPRClient[Count] = 0;
                                    ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_StopRealPlay(m_hLPRPlay[Count]);
                                }
                                else if (lstNCS[0].VideoType == "ZNYKTY10")
                                {
                                    ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHStopLoadPic(pCallBack[Count]);
                                    ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHStopRealPlay(pRealPlayHandle[Count]);
                                }

                                else if (lstNCS[0].VideoType == "ZNYKTYY8")
                                {
                                    //v_Talk[Count].CloseDevice();
                                }
                                else if (lstNCS[0].VideoType == "ZNYKTY14" || lstNCS[0].VideoType == "ZNYKTY11")
                                {
                                    m_hLPRClient[Count] = 0;
                                    ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_StopVideo(nCamId[Count]);
                                    ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_DisConnCamera(nCamId[Count]);
                                }
                                else if (lstNCS[0].VideoType == "ZNYKTY13")
                                {
                                    //关闭车牌回调
                                    if (VideoInfo[Count].m_hOpenImg != IntPtr.Zero)
                                    {
                                        ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_ClosePicture(VideoInfo[Count].m_hOpenImg);
                                    }
                                    //关闭视频链接
                                    if (VideoInfo[Count].m_hOpenVideo != IntPtr.Zero)
                                    {
                                        ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_CloseChannel(VideoInfo[Count].m_hOpenVideo);
                                    }

                                    ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_ReleasePlayer(ushort.Parse(Count.ToString()));

                                    if (VideoInfo[Count].m_hLogon != IntPtr.Zero)
                                    {
                                        ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_LogoffServer(VideoInfo[Count].m_hLogon);
                                        VideoInfo[m_nSelIndex].m_hLogon = IntPtr.Zero;
                                    }
                                }
                            }
                        }

                    }

                }

                ParkingCommunication.CameraSDK.ZNYKT14.MyClass.Net_UNinit();//释放ZNYKTY14 相机SDK

                //CR.CHCNetSDK.NET_DVR_Cleanup();//释放海康普通相机和海康系列一体机 相机SDK

                ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_Cleanup();//释放ZNYKTY5和ZNYKTY15一体机SDK

                if (Model.bStartZNYKT13)//释放ZNYKTY13一体机SDK
                {
                    ParkingCommunication.CameraSDK.ZNYKT13.YW7000NetClient.YW7000NET_Cleanup();
                    ParkingCommunication.CameraSDK.ZNYKT13.YW7000PlayerSDK.YW7000PLAYER_ReleaseSDK();
                }

                ParkingCommunication.CameraSDK.ZNYKT10.DHClient.DHCleanup();//释放大华普通相机和大华系列一体机 相机SDK

                this.Cursor = Cursors.Arrow;//正常状态
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;//正常状态

                gsd.AddLog("在线监控" + ":ParkingMonitoring_FormClosing", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nParkingMonitoring_FormClosing", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //2016-12-17
            //if (Model.iCPHPhoto == 1)
            //{
            //    if (System.IO.File.Exists(delLastInImage))
            //    {
            //        System.IO.File.Delete(delLastInImage);
            //        delLastInImage = "";
            //    }

            //    if (System.IO.File.Exists(delLastOutImage))
            //    {
            //        System.IO.File.Delete(delLastOutImage);
            //        delLastInImage = "";
            //    }
            //}
            //(new Request()).LogOut(Model.token);

            //Application.Current.Shutdown();
            System.Environment.Exit(0);
        }
        #endregion


        #region Controls
        #region Timer
        private void dTimer_Tick(object sender, EventArgs e)
        {
            txbSystemTime.Text = Convert.ToString(DateTime.Now.ToLocalTime());
        }

        BackgroundWorker bw;
        int reconnectionCount = 0;
        private void dTimer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int ret = (new Request()).KeppAlive(Model.token);
            }
            catch
            {

            }
        }



        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                try
                {
                    string data = (new Request()).GetToken(Model.sUserCard, Model.sUserPwd);
                    if (null != data && data.Trim().Length > 0)
                    {
                        reconnectionCount = 0;
                        Model.token = data;
                        UpdateTxbText(txbOperatorInfo, "重连服务成功!");
                        timer.Start();
                        bw.Dispose();
                        break;
                    }
                }
                catch
                {
                    Thread.Sleep(5000);
                    reconnectionCount++;
                    UpdateTxbText(txbOperatorInfo, "服务已断开或网络异常，正在拼命重连,次数：" + reconnectionCount.ToString());
                }

            }

            //工作方法
        }



        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //保持连接
                int ret = (new Request()).KeppAlive(Model.token);
            }
            catch
            {
                timer.Close();
                bw = new System.ComponentModel.BackgroundWorker();
                bw.WorkerSupportsCancellation = true; //可以取消
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
                //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                bw.RunWorkerAsync();
            }
        }
        #endregion

        private void btnPreCPH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnPreCPH.Content.ToString() == "车辆入场")
                {
                    //2015-10-30
                    if (cmbCarIn.Items.Count > 0)
                        cmbCarIn.Items.Clear();
                    cmbCarIn.Text = "";
                    int iSCount = 0;
                    int iSumCount = 0;
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        if (Model.Channels[i].iInOut == 0)
                        {
                            iSumCount++;
                            iSCount = i;
                            cmbCarIn.Items.Add(Model.Channels[i].sInOutName);
                        }
                    }
                    if (iSumCount > 1)
                    {
                        cmbCarIn.Visibility = Visibility.Visible;
                    }
                    else if (iSumCount == 1)
                    {
                        DZNOShiBie(iSCount);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (Model.bSetTempCardPlate == true)
                    {
                        if (isOne == true)
                        {
                            carSetup = 1;
                            //!!!
                            //ParkingWriteTempCar TempCar = new ParkingWriteTempCar();
                            //TempCar.ShowDialog(); ;
                            //TempCar.Dispose();
                        }
                        else
                        {
                            txbOperatorInfo.Text = "只有入口可预置临时车牌/卡类！";
                        }
                    }
                    else
                    {
                        txbOperatorInfo.Text = "没有权限预制临时卡车牌";
                    }
                }
            }
            catch (Exception ex)
            {
                txbOperatorInfo.Text = ex.Message + "(" + btnPreCPH.Content.ToString() + ")";
                gsd.AddLog("在线监控" + ":btnPreCPH_Click", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnUnPreCPH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnUnPreCPH.Content.ToString() == "车辆出场")
                {
                    //2015-10-30
                    cmbCarOut.Items.Clear();
                    cmbCarOut.Text = "";
                    int iSCount = 0;
                    int iSumCount = 0;
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        if (Model.Channels[i].iInOut == 1 && Model.Channels[i].iCtrlID == Model.Channels[i].iOpenID)
                        {
                            iSumCount++;
                            iSCount = i;
                            cmbCarOut.Items.Add(Model.Channels[i].sInOutName);
                        }
                    }

                    if (iSumCount > 1)
                    {
                        cmbCarOut.Visibility = Visibility.Visible;
                    }
                    else if (iSumCount == 1)
                    {
                        DZNOShiBie(iSCount);
                    }
                    else
                    {

                    }
                }

                else
                {
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        if (Model.Channels[i].iInOut == 0)
                        {
                            SedBll send = new SedBll(Model.Channels[i].sIP, 1007, 1005);
                            string Count = "";
                            if (Model.Channels[i].iXieYi == 1)
                            {
                                Count = send.PrecutcardType(Model.Channels[i].iCtrlID, "0038383838383838", Model.Channels[i].iXieYi);
                            }

                            if (Count == "0")
                            {
                                txbOperatorInfo.Text = "取消预制成功！";
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                txbOperatorInfo.Text = ex.Message + "(" + btnUnPreCPH.Content.ToString() + ")";
                gsd.AddLog("在线监控" + ":btnUnPreCPH_Click", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnNoInCPH_Click(object sender, RoutedEventArgs e)
        {
            cmbNoCarIn.Items.Clear();
            cmbNoCarIn.Text = "";
            int iSCount = 0;
            int iSumCount = 0;
            for (int i = 0; i < Model.iChannelCount; i++)
            {
                if (Model.Channels[i].iInOut == 0)
                {
                    iSumCount++;
                    iSCount = i;
                    cmbNoCarIn.Items.Add(Model.Channels[i].sInOutName);
                }
            }

            if (iSumCount > 1)
            {
                cmbNoCarIn.Visibility = Visibility.Visible;
            }
            else if (iSumCount == 1)
            {
                ImageSavePath(iSCount, 0);
                Mycaptureconvert(iSCount, 0);//图像抓拍

                if (Model.iCarPosLed == 0)
                {
                    //!!! 与原软件不符
                    CR.AddShuiYin(picFileName, filesJpg, Model.Channels[iSCount].sInOutName, CR.GetAutoCPHCardNO(Model.Channels[iSCount].iCtrlID), iSCount, "无牌车");
                }
                ParkingInNOPlateNo frm = new ParkingInNOPlateNo(filesJpg, iSCount, new InNoCPHHandler(RefreshInOut));
                frm.ShowDialog();
                picFileName = "";
                filesJpg = "";

            }
            else
            {

            }
        }

        private void btnNoOutCPH_Click(object sender, RoutedEventArgs e)
        {
            cmbNoCarOut.Items.Clear();
            cmbNoCarOut.Text = "";
            int iSCount = 0;
            int iSumCount = 0;
            for (int i = 0; i < Model.iChannelCount; i++)
            {
                if (Model.Channels[i].iInOut == 1)
                {
                    iSumCount++;
                    iSCount = i;
                    cmbNoCarOut.Items.Add(Model.Channels[i].sInOutName);
                }
            }

            if (iSumCount > 1)
            {
                cmbNoCarOut.Visibility = Visibility.Visible;
            }
            else if (iSumCount == 1)
            {
                ImageSavePath(iSCount, 0);
                Mycaptureconvert(iSCount, 0);//图像抓拍

                if (Model.iCarPosLed == 0)
                {
                    CR.AddShuiYin(picFileName, filesJpg, Model.Channels[iSCount].sInOutName, CR.GetAutoCPHCardNO(Model.Channels[iSCount].iCtrlID), iSCount, "无牌车");
                }
                ParkingOutNOPlateNo frm = new ParkingOutNOPlateNo(filesJpg, iSCount, new InNoCPHHandler(RefreshInOut), picFileName);
                frm.ShowDialog();
                picFileName = "";
                filesJpg = "";
            }
            else
            {

            }
        }

        private void cmbCarOut_MouseLeave(object sender, MouseEventArgs e)
        {
            cmbCarOut.Visibility = Visibility.Hidden;
        }

        private void cmbNoCarIn_MouseLeave(object sender, MouseEventArgs e)
        {
            cmbNoCarIn.Visibility = Visibility.Hidden;
        }

        private void cmbNoCarOut_MouseLeave(object sender, MouseEventArgs e)
        {
            cmbNoCarOut.Visibility = Visibility.Hidden;
        }

        private void btnRegisterCPH_Click(object sender, RoutedEventArgs e)
        {
            ParkingPlateRegister ppr = new ParkingPlateRegister();
            ppr.Owner = this;
            ppr.ShowDialog();
        }

        private void btnBlacklist_Click(object sender, RoutedEventArgs e)
        {
            frmAddBlacklist fab = new frmAddBlacklist();
            fab.Owner = this;
            fab.ShowDialog();
        }

        private void btnDeadline_Click(object sender, RoutedEventArgs e)
        {
            Report.ReportDeadline rDeadline = new Report.ReportDeadline();
            rDeadline.Owner = this;
            rDeadline.ShowDialog();
        }

        private void btmInPark_Click(object sender, RoutedEventArgs e)
        {
            Report.ReportInPark rInPark = new Report.ReportInPark();
            rInPark.Owner = this;
            rInPark.ShowDialog();
        }

        private void btnCarCharge_Click(object sender, RoutedEventArgs e)
        {
            Report.ReportCarCharge rCharge = new Report.ReportCarCharge();
            rCharge.Owner = this;
            rCharge.ShowDialog();
        }

        private void btnChangeShifts_Click(object sender, RoutedEventArgs e)
        {
            ParkingChangeShifts pcs = new ParkingChangeShifts(Model.sUserCard, new UpdateStatuesHandler(UpdateStatus));
            pcs.Owner = this;
            pcs.ShowDialog();
        }

        private void cmbCarIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCarIn.Items.Count > 0)
                {
                    if (cmbCarIn.SelectedIndex > -1)
                    {
                        for (int i = 0; i < Model.iChannelCount; i++)
                        {
                            if (Model.Channels[i].sInOutName == cmbCarIn.SelectedItem.ToString())
                            {
                                cmbCarIn.Visibility = Visibility.Hidden;
                                DZNOShiBie(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控", "cmbCarIn_SelectionChanged" + ex.Message);
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbCarOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCarOut.Items.Count > 0)
            {
                if (cmbCarOut.SelectedIndex > -1)
                {
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        if (Model.Channels[i].sInOutName == cmbCarOut.SelectedItem.ToString())
                        {
                            cmbCarOut.Visibility = Visibility.Hidden;
                            DZNOShiBie(i);
                        }
                    }
                }
            }
        }

        private void cmbNoCarIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNoCarIn.Items.Count > 0)
            {
                if (cmbNoCarIn.SelectedIndex > -1)
                {
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        if (Model.Channels[i].sInOutName == cmbNoCarIn.SelectedItem.ToString())
                        {

                            ImageSavePath(i, 0);
                            Mycaptureconvert(i, 0);//图像抓拍

                            if (Model.iCarPosLed == 0)
                            {
                                CR.AddShuiYin(picFileName, filesJpg, Model.Channels[i].sInOutName, CR.GetAutoCPHCardNO(Model.Channels[i].iCtrlID), i, "无牌车");
                            }
                            ParkingInNOPlateNo frm = new ParkingInNOPlateNo(filesJpg, i, new InNoCPHHandler(RefreshInOut));
                            frm.Show();
                            picFileName = "";
                            filesJpg = "";
                            cmbNoCarIn.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }

        private void cmbNoCarOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNoCarOut.Items.Count > 0)
            {
                if (cmbNoCarOut.SelectedIndex > -1)
                {
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        if (Model.Channels[i].sInOutName == cmbNoCarOut.SelectedItem.ToString())
                        {

                            ImageSavePath(i, 0);
                            Mycaptureconvert(i, 0);//图像抓拍

                            if (Model.iCarPosLed == 0)
                            {
                                CR.AddShuiYin(picFileName, filesJpg, Model.Channels[i].sInOutName, CR.GetAutoCPHCardNO(Model.Channels[i].iCtrlID), i, "无牌车");
                            }

                            ParkingOutNOPlateNo frm = new ParkingOutNOPlateNo(filesJpg, i, new InNoCPHHandler(RefreshInOut), picFileName);

                            //ParkingOutNOPlateNo frm = new ParkingOutNOPlateNo(filesJpg, i, new InNoCPHHandler(RefreshInOut));
                            frm.Show();
                            picFileName = "";
                            filesJpg = "";
                            cmbNoCarOut.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }

        private void cmbCarIn_MouseLeave(object sender, MouseEventArgs e)
        {
            cmbCarIn.Visibility = Visibility.Hidden;
        }

        private void btnCarOutRefresh_Click(object sender, RoutedEventArgs e)
        {
            List<CarOut> dtBin;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic = TextString("CPH", "OutGateName", "OutOperator", txtSelectOutCarNo, txtSelectOutName, txtSelectOutOperator);

            List<CarOut> dtSo = gsd.GetOutGate(dic);

            dtBin = dgvCharge.ItemsSource as List<CarOut>;
            //if (dtBin != null)
            //{
            //    dtBin.Clear();
            //    if (dtSo != null && dtSo.Count > 0)
            //    {
            //        foreach (var dr in dtSo)
            //        {
            //            dtBin.Add(dr);
            //        }
            //    }
            //    dtSo.Clear();
            //}
            //else
            //{
            //    if (dtSo != null)
            dgvCharge.ItemsSource = dtSo;
            //}
        }

        private void btnCarInRefresh_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic = TextString("CPH", "InGateName", "InOperator", txtSelectCarNo, txtSelectInName, txtSelectInOperator);
            List<CarIn> dtSoOut = gsd.GetInGate(dic);
            List<CarIn> dtBinOut = dgvCar.ItemsSource as List<CarIn>;
            //if (dtBinOut != null)
            //{
            //    dtBinOut.Clear();
            //    if (dtSoOut != null && dtSoOut.Count > 0)
            //    {
            //        foreach (var dr in dtSoOut)
            //        {
            //            dtBinOut.Add(dr);
            //        }

            //    }

            //    //dtSoOut.Clear();
            //}
            //else
            //{
            //    if (dtSoOut != null)
            dgvCar.ItemsSource = dtSoOut;
            //}
        }

        private void picNetVideo0_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (wfhVideo0.Width == videoRawSize[0].Width && wfhVideo0.Height == videoRawSize[0].Height)
            {
                wfhVideo0.Margin = new Thickness(-10, -59, 0, 0);
                wfhVideo0.Width = rawSize.Width;
                wfhVideo0.Height = rawSize.Height;
                wfhVideo1.Visibility = Visibility.Collapsed;
                wfhVideo2.Visibility = Visibility.Collapsed;
                wfhVideo3.Visibility = Visibility.Collapsed;

                for (int i = 0; i < lstWfhSmallCarNo.Count; i++)
                {
                    lstWfhSmallCarNo[i].Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                wfhVideo0.Width = videoRawSize[0].Width;
                wfhVideo0.Height = videoRawSize[0].Height;
                wfhVideo0.Margin = new Thickness(0);
                wfhVideo1.Visibility = Visibility.Visible;
                wfhVideo2.Visibility = Visibility.Visible;
                wfhVideo3.Visibility = Visibility.Visible;

                for (int i = 0; i < lstWfhSmallCarNo.Count; i++)
                {
                    lstWfhSmallCarNo[i].Visibility = rawVI[i];
                }
            }
        }

        private void picNetVideo1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (wfhVideo1.Width == videoRawSize[1].Width && wfhVideo1.Height == videoRawSize[1].Height)
            {
                wfhVideo1.Margin = new Thickness(-438, -59, 0, 0);
                wfhVideo1.Width = rawSize.Width;
                wfhVideo1.Height = rawSize.Height;
                wfhVideo0.Visibility = Visibility.Collapsed;
                wfhVideo2.Visibility = Visibility.Collapsed;
                wfhVideo3.Visibility = Visibility.Collapsed;
                for (int i = 0; i < lstWfhSmallCarNo.Count; i++)
                {
                    lstWfhSmallCarNo[i].Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                wfhVideo1.Width = videoRawSize[1].Width;
                wfhVideo1.Height = videoRawSize[1].Height;
                wfhVideo1.Margin = new Thickness(0);
                wfhVideo0.Visibility = Visibility.Visible;
                wfhVideo2.Visibility = Visibility.Visible;
                wfhVideo3.Visibility = Visibility.Visible;
                for (int i = 0; i < lstWfhSmallCarNo.Count; i++)
                {
                    lstWfhSmallCarNo[i].Visibility = rawVI[i];
                }
            }
        }

        private void picNetVideo2_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (wfhVideo2.Width == videoRawSize[2].Width && wfhVideo2.Height == videoRawSize[2].Height)
            {
                wfhVideo2.Margin = new Thickness(-10, -465, 0, 0);
                wfhVideo2.Width = rawSize.Width;
                wfhVideo2.Height = rawSize.Height;
                wfhVideo1.Visibility = Visibility.Collapsed;
                wfhVideo0.Visibility = Visibility.Collapsed;
                wfhVideo3.Visibility = Visibility.Collapsed;

                if (Model.iVideo4 == 1)
                {
                    wfh3.Visibility = Visibility.Collapsed;
                    wfh4.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                wfhVideo2.Width = videoRawSize[2].Width;
                wfhVideo2.Height = videoRawSize[2].Height;
                wfhVideo2.Margin = new Thickness(0);
                wfhVideo1.Visibility = Visibility.Visible;
                wfhVideo0.Visibility = Visibility.Visible;
                wfhVideo3.Visibility = Visibility.Visible;

                if (Model.iVideo4 == 1)
                {
                    wfh3.Visibility = Visibility.Visible;
                    wfh4.Visibility = Visibility.Visible;
                }
            }
        }

        private void picNetVideo3_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (wfhVideo3.Width == videoRawSize[3].Width && wfhVideo3.Height == videoRawSize[3].Height)
            {
                wfhVideo3.Margin = new Thickness(-438, -465, 0, 0);
                wfhVideo3.Width = rawSize.Width;
                wfhVideo3.Height = rawSize.Height;
                wfhVideo1.Visibility = Visibility.Collapsed;
                wfhVideo2.Visibility = Visibility.Collapsed;
                wfhVideo0.Visibility = Visibility.Collapsed;

                if (Model.iVideo4 == 1)
                {
                    wfh3.Visibility = Visibility.Collapsed;
                    wfh4.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                wfhVideo3.Width = videoRawSize[3].Width;
                wfhVideo3.Height = videoRawSize[3].Height;
                wfhVideo3.Margin = new Thickness(0);
                wfhVideo1.Visibility = Visibility.Visible;
                wfhVideo2.Visibility = Visibility.Visible;
                wfhVideo0.Visibility = Visibility.Visible;

                if (Model.iVideo4 == 1)
                {
                    wfh3.Visibility = Visibility.Visible;
                    wfh4.Visibility = Visibility.Visible;
                }
            }
        }

        private void dgvCar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgvCar.Items.Count > 0)
            {
                if (dgvCar.SelectedIndex > -1)
                {
                    var dr = dgvCar.SelectedItem as CarIn;
                    string strCPH = dr.CPH;
                    string strOutPic = dr.InPic;
                    string UpdateCardNO = dr.CardNO;
                    ParkingUpdateCPH frm = new ParkingUpdateCPH(strCPH, strOutPic, UpdateCardNO);
                    frm.ShowDialog();
                }
            }
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

                        string strInPic = dr.InPic ?? "";

                        loadPic(strInPic, lstPicVideo[2], 0);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控", "dgvCar_MouseClick" + ex.Message);
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvCharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgvCharge.Items.Count > 0)
                {
                    if (dgvCharge.SelectedIndex > -1)
                    {
                        var dr = dgvCharge.SelectedItem as CarOut;
                        string strInPic = dr.InPic ?? "";
                        string strOutPic = dr.OutPic ?? "";
                        //加载入场图片
                        loadPic(strInPic, lstPicVideo[2], 0);
                        //加载出场图片
                        loadPic(strOutPic, lstPicVideo[3], 1);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("在线监控", "dgvCharge_MouseLeftButtonDown" + ex.Message);
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCmdOpen0_Click(object sender, RoutedEventArgs e)
        {
            if (Model.iSoftOpenNoPlate == 1)
            {
                Open(0);
            }
            else
            {
                ParkingOpen Come = new ParkingOpen(new ParkingMonitoringWHandler(EnterPat_Photo), 0);
                Come.ShowDialog();
            }
        }

        private void btnCmdOpen1_Click(object sender, RoutedEventArgs e)
        {
            if (Model.iSoftOpenNoPlate == 1)
            {
                Open(1);
            }
            else
            {
                ParkingOpen Come = new ParkingOpen(new ParkingMonitoringWHandler(EnterPat_Photo), 1);
                Come.ShowDialog();
            }
        }

        private void btnCmdOpen2_Click(object sender, RoutedEventArgs e)
        {
            if (Model.iSoftOpenNoPlate == 1)
            {
                Open(2);
            }
            else
            {
                ParkingOpen Come = new ParkingOpen(new ParkingMonitoringWHandler(EnterPat_Photo), 2);
                Come.ShowDialog();
            }
        }

        private void btnCmdOpen3_Click(object sender, RoutedEventArgs e)
        {
            if (Model.iSoftOpenNoPlate == 1)
            {
                Open(3);
            }
            else
            {
                ParkingOpen Come = new ParkingOpen(new ParkingMonitoringWHandler(EnterPat_Photo), 3);
                Come.ShowDialog();
            }
        }

        private void btnCmdClose0_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定关闸吗？", "关闸 ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                string strRetun = "";
                strRetun = sender0.SendOpen(0);
            }
        }

        private void btnCmdClose1_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定关闸吗？", "关闸 ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                string strRetun = "";
                strRetun = sender0.SendOpen(1);
            }
        }

        private void btnCmdClose2_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定关闸吗？", "关闸 ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                string strRetun = "";
                strRetun = sender0.SendOpen(2);
            }
        }

        private void btnCmdClose3_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定关闸吗？", "关闸 ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                string strRetun = "";
                strRetun = sender0.SendOpen(3);
            }
        }

        private void btnManual0_Click(object sender, RoutedEventArgs e)
        {
            if (m_hLPRClient[0] > 0)
            {
                ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ForceTrigger(m_hLPRClient[0]);
            }
        }

        private void btnManual1_Click(object sender, RoutedEventArgs e)
        {
            if (m_hLPRClient[1] > 0)
            {
                ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ForceTrigger(m_hLPRClient[1]);
            }
        }

        private void btnManual2_Click(object sender, RoutedEventArgs e)
        {
            if (m_hLPRClient[2] > 0)
            {
                ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ForceTrigger(m_hLPRClient[2]);
            }
        }

        private void btnManual3_Click(object sender, RoutedEventArgs e)
        {
            if (m_hLPRClient[3] > 0)
            {
                ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_ForceTrigger(m_hLPRClient[3]);
            }
        }

        private void ptr4_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            wfh4.Width = wfhVideo2.Width;
            wfh4.Height = wfhVideo2.Height;
        }

        private void ptr3_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            wfh3.Width = wfhVideo2.Width;
            wfh3.Height = wfhVideo2.Height;

        }

        private void ptr4_MouseLeave(object sender, EventArgs e)
        {
            wfh4.Width = 80;
            wfh4.Height = 68;
        }

        private void ptr3_MouseLeave(object sender, EventArgs e)
        {
            wfh3.Width = 80;
            wfh3.Height = 68;
        }

        private void txtSelectCarNo_KeyUp(object sender, KeyEventArgs e)
        {
            Dictionary<string, object> dic = TextString("CPH", "InGateName", "InOperator", txtSelectCarNo, txtSelectInName, txtSelectInOperator);
            List<CarIn> lstCi = gsd.GetInGate(dic);
            if (lstCi != null && lstCi.Count > 0)
                dgvCar.ItemsSource = lstCi;
        }

        private void txtSelectInOperator_KeyUp(object sender, KeyEventArgs e)
        {
            Dictionary<string, object> dic = TextString("CPH", "InGateName", "InOperator", txtSelectCarNo, txtSelectInName, txtSelectInOperator);
            List<CarIn> lstCi = gsd.GetInGate(dic);
            if (lstCi != null && lstCi.Count > 0)
                dgvCar.ItemsSource = lstCi;
        }

        private void txtSelectInName_KeyUp(object sender, KeyEventArgs e)
        {
            Dictionary<string, object> dic = TextString("CPH", "InGateName", "InOperator", txtSelectCarNo, txtSelectInName, txtSelectInOperator);
            List<CarIn> lstCi = gsd.GetInGate(dic);
            if (lstCi != null && lstCi.Count > 0)
                dgvCar.ItemsSource = lstCi;
        }

        private void txtSelectOutCarNo_KeyUp(object sender, KeyEventArgs e)
        {
            Dictionary<string, object> dic = TextString("CPH", "OutGateName", "OutOperator", txtSelectOutCarNo, txtSelectOutName, txtSelectOutOperator);
            List<CarOut> lstCo = gsd.GetOutGate(dic);
            if (lstCo != null && lstCo.Count > 0)
                dgvCharge.ItemsSource = lstCo;
        }

        private void txtSelectOutOperator_KeyUp(object sender, KeyEventArgs e)
        {
            Dictionary<string, object> dic = TextString("CPH", "OutGateName", "OutOperator", txtSelectOutCarNo, txtSelectOutName, txtSelectOutOperator);
            List<CarOut> lstCo = gsd.GetOutGate(dic);
            if (lstCo != null && lstCo.Count > 0)
                dgvCharge.ItemsSource = lstCo;
        }

        private void txtSelectOutName_KeyUp(object sender, KeyEventArgs e)
        {
            Dictionary<string, object> dic = TextString("CPH", "OutGateName", "OutOperator", txtSelectOutCarNo, txtSelectOutName, txtSelectOutOperator);
            List<CarOut> lstCo = gsd.GetOutGate(dic);
            if (lstCo != null && lstCo.Count > 0)
                dgvCharge.ItemsSource = lstCo;
        }

        int DGFlag0 = 1;
        int DGFlag1 = 1;
        int DGFlag2 = 1;
        int DGFlag3 = 1;

        private void btnCmdOpen0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //同时按下Ctrl和右键,则开启测试模式
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.RightButton == MouseButtonState.Pressed)
            {
                try
                {
                    if (Model.iCheDui == 1)
                    {
                        //short iJHs = 2;
                        string DZopen = "AAAA";
                        string DZclose = "5555";
                        //short cmd = 0x8B;

                        SedBll sendbll = new SedBll(Model.Channels[0].sIP, 1007, 1005);
                        if (DGFlag0 == 1)
                        {
                            sendbll.HeightCutoff(Model.Channels[0].iCtrlID, DZopen, Model.Channels[0].iXieYi);
                            DGFlag0 = 2;
                            //picDZ0.Image = ImgDZ.Images[1];
                            //ImgDG0.Tag = 2;
                        }
                        else
                        {
                            sendbll.HeightCutoff(Model.Channels[0].iCtrlID, DZclose, Model.Channels[0].iXieYi);//关闸
                            DGFlag0 = 1;
                            //ImgDG0.Tag = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    gsd.AddLog("在线监控" + ":btnCmdOpen0_MouseDown", ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }


        private void btnCmdOpen1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //同时按下Ctrl和右键,则开启测试模式
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.RightButton == MouseButtonState.Pressed)
            {
                try
                {
                    if (Model.iCheDui == 1)
                    {
                        //short iJHs = 2;
                        string DZopen = "AAAA";
                        string DZclose = "5555";
                        //short cmd = 0x8B;

                        SedBll sendbll = new SedBll(Model.Channels[1].sIP, 1007, 1005);
                        if (DGFlag1 == 1)
                        {
                            sendbll.HeightCutoff(Model.Channels[1].iCtrlID, DZopen, Model.Channels[1].iXieYi);
                            DGFlag1 = 2;
                            //picDZ0.Image = ImgDZ.Images[1];
                            //ImgDG0.Tag = 2;
                        }
                        else
                        {
                            sendbll.HeightCutoff(Model.Channels[1].iCtrlID, DZclose, Model.Channels[1].iXieYi);//关闸
                            DGFlag1 = 1;
                            //ImgDG0.Tag = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    gsd.AddLog("在线监控" + ":btnCmdOpen0_MouseDown", ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }

        private void btnCmdOpen2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //同时按下Ctrl和右键,则开启测试模式
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.RightButton == MouseButtonState.Pressed)
            {
                try
                {
                    if (Model.iCheDui == 1)
                    {
                        //short iJHs = 2;
                        string DZopen = "AAAA";
                        string DZclose = "5555";
                        //short cmd = 0x8B;

                        SedBll sendbll = new SedBll(Model.Channels[2].sIP, 1007, 1005);
                        if (DGFlag2 == 1)
                        {
                            sendbll.HeightCutoff(Model.Channels[2].iCtrlID, DZopen, Model.Channels[2].iXieYi);
                            DGFlag2 = 2;
                            //picDZ0.Image = ImgDZ.Images[1];
                            //ImgDG0.Tag = 2;
                        }
                        else
                        {
                            sendbll.HeightCutoff(Model.Channels[2].iCtrlID, DZclose, Model.Channels[2].iXieYi);//关闸
                            DGFlag2 = 1;
                            //ImgDG0.Tag = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    gsd.AddLog("在线监控" + ":btnCmdOpen0_MouseDown", ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }

        private void btnCmdOpen3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //同时按下Ctrl和右键,则开启测试模式
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.RightButton == MouseButtonState.Pressed)
            {
                try
                {
                    if (Model.iCheDui == 1)
                    {
                        //short iJHs = 2;
                        string DZopen = "AAAA";
                        string DZclose = "5555";
                        //short cmd = 0x8B;

                        SedBll sendbll = new SedBll(Model.Channels[3].sIP, 1007, 1005);
                        if (DGFlag3 == 1)
                        {
                            sendbll.HeightCutoff(Model.Channels[3].iCtrlID, DZopen, Model.Channels[3].iXieYi);
                            DGFlag3 = 2;
                            //picDZ0.Image = ImgDZ.Images[1];
                            //ImgDG0.Tag = 2;
                        }
                        else
                        {
                            sendbll.HeightCutoff(Model.Channels[3].iCtrlID, DZclose, Model.Channels[3].iXieYi);//关闸
                            DGFlag3 = 1;
                            //ImgDG0.Tag = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    gsd.AddLog("在线监控" + ":btnCmdOpen0_MouseDown", ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }
        #endregion

        #endregion


    }
}