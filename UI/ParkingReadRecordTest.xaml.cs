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
using System.Threading;
using System.Data;
using ParkingCommunication;
using ParkingModel;
using ParkingInterface;

namespace UI
{
    /// <summary>
    /// ParkingReadRecordTest.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingReadRecordTest : SFMControls.WindowBase
    {
        bool Clear = false;
        GetServiceData gsd = new GetServiceData();
        private int State = 0;
        private System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        private Thread fThread;
        //private int threadState = 0;
        SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005); // 这里会和控制板交互来发送数据;
        private int SumCount = 0;

        private Hid usbHid = new Hid();

        public ParkingReadRecordTest()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                CR.BinDic(gsd.GetCardType());
                State = 0;
                dTimer.Tick += new EventHandler(dTimer_Tick);
                dTimer.Interval = new TimeSpan(0, 0, 1);
                dTimer.Start();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "Window_Loaded", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (State == 0)
                {
                    fThread = new Thread(new ThreadStart(GetRead));//开辟一个新的线程
                    fThread.IsBackground = true;
                    fThread.Start();
                }
                else if (State == 2)
                {

                }
                else
                {
                    dTimer.Stop();
                    Clear = true;
                    this.Close();
                    if (Model.iParking == 0)
                    {
                        Model.Monitoring = 1;
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("读取记录", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dTimer_Tick", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// 提取记录
        /// </summary>
        /// <param name="JiHao"></param>
        private void GetRead()
        {
            DateTime XstartDate = DateTime.Now;

            State = 2;
            List<CheDaoSet> ds = gsd.GetCtrName(Model.stationID);
            int read = 0;
            foreach (var dr in ds)// 根据本机的xml文件的数据和请求服务器的车道数据对比
            {
                read++;
                if (SqlExceptionXml.SqlExceptionXml.GetCardNOs().Count > 0)// 使用xml来保存了数据，保存机号和车辆编号
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    try
                    {
                        dic = SqlExceptionXml.SqlExceptionXml.GetCardNOs();
                    }
                    catch (Exception ex)
                    {
                        gsd.AddLog(this.Title + ":SqlExceptionXml.SqlExceptionXml", ex.Message + "\r\n" + ex.StackTrace);
                    }

                    foreach (var key in dic.Keys)
                    {
                        if (dr.CtrlNumber.ToString() == key.ToString()) // 获取的机号和 SqlExceptionXml存储的机号相等，
                        {
                            BinModel(dic[key].ToString(), dr.InOut.ToString(), dr.InOutName, dr, "连接异常");// ### (这里的处理有疑问)
                            SqlExceptionXml.SqlExceptionXml.DeleteCardNOs(dic[key].ToString());
                        }
                    }

                }
                //SetTextMessage(dr["InOutName"].ToString(), dr["IP"].ToString(), dr["CtrlNumber"].ToString());
                //lblName.Text = dr["InOutName"].ToString();
                //lblIP.Text = dr["IP"].ToString();
                //lblJiHao.Text = dr["CtrlNumber"].ToString();

                //跨线程访问控件的方法
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblLaneName.Content = dr.InOutName;
                                                lblLaneIP.Content = dr.IP;
                                                lblLaneNumber.Content = dr.CtrlNumber;
                                                lblRecordCount.Content = SumCount.ToString();
                                            }));

                int XieYi = dr.XieYi;
                string Count = "0";

                if (XieYi == 1 || XieYi == 3)// 协议判断
                {
                    DateTime dt = DateTime.Now;
                    while (dt.AddSeconds(Model.iDelayed) >= DateTime.Now)
                    {
                        sendbll.SetUsbType(ref usbHid, XieYi);  //2015-09-18 
                        Count = sendbll.ReadRecordCount(dr.IP, dr.CtrlNumber, XieYi);// 192.168.2.186/21/1 和控制板进行通信 ##还是有疑问
                        if (Count != "-2")
                        {
                            //this.Invoke((EventHandler)delegate
                            //{
                            //    label1.Text = "正在加载...";
                            //});

                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               tbkLoding.Text = "正在加载...";
                                           }));
                            break;
                        }
                        else
                        {
                            //this.Invoke((EventHandler)delegate
                            //{
                            //   label1.Text = "链接中断...";
                            //});
                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               tbkLoding.Text = "链接中断...";
                                           }));
                        }
                    }
                }

                if (Count == "-2")
                {
                    //SetTextColose();

                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               System.Windows.Forms.MessageBox.Show("与控制机通讯不通!\n请检查通讯链接后重新提取脱机记录！", "通讯不通", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                           }));
                }

                SumCount = Convert.ToInt32(Count);

                for (int i = 0; i < Convert.ToInt32(Count); i++)
                {
                    string Data = "";

                    if (XieYi == 1 || XieYi == 3)
                    {
                        DateTime dt = DateTime.Now;
                        while (dt.AddSeconds(Model.iDelayed) >= DateTime.Now)
                        {
                            sendbll.SetUsbType(ref usbHid, XieYi);
                            Data = sendbll.ReadRecord(dr.IP, dr.CtrlNumber, XieYi);
                            if (Data != "2")
                            {
                                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               tbkLoding.Text = "正在加载...";
                                           }));
                                break;
                            }
                            else
                            {
                                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               tbkLoding.Text = "链接中断...";
                                           }));
                            }
                        }
                    }

                    if (Model.ReadRepeat[read - 1] == Data)
                    {
                    }
                    else
                    {
                        Model.ReadRepeat[read - 1] = Data;
                        if (Data != "2")
                        {
                            BinModel(Data, dr.InOut.ToString(), dr.InOutName, dr, "脱机");

                            //SetTextCount(i);

                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               pgbShow.setPrecent(Convert.ToInt32((i + 1) * 100 / Convert.ToInt32(Count)));
                                               //pgbShow.Value = Convert.ToInt32((i + 1) * 100 / Convert.ToInt32(Count));
                                           }));
                        }
                        else
                        {
                            //SetTextColose();
                            //MessageBox.Show("与控制机通讯不通", Language.LanguageXML.GetName("CR/Prompt"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //this.Close();

                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               System.Windows.Forms.MessageBox.Show("与控制机通讯不通!\n请检查通讯链接后重新提取脱机记录！", "通讯不通", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                           }));
                            break;
                        }
                    }
                }

            }
            DateTime endDate = DateTime.Now;
            TimeSpan ss = endDate - XstartDate;
            //SetTextTest(ss);

            State = 1;

        }


        private void BinModel(string Data, string flag, string strName, CheDaoSet dr, string str)
        {
            try
            {
                if (Data.Length > 8)// 车牌编号?
                {
                    //卡片类型
                    string CardType = "";
                    //卡号
                    string CardNO = "";

                    //车牌号
                    string sumCPH = Data.Substring(46, 8);// ??? 46开始截取?
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
                            if (Data.Substring(0, 2) == "E8")
                            {
                                List<CarIn> Sdt = gsd.SelectComeCPH(CPH, 6, "Tmp", "Tmp");
                                if (Sdt.Count > 0)
                                {
                                    CardNO = Sdt[0].CardNO;
                                    CPH = Sdt[0].CPH;
                                }
                                else
                                {
                                    CardNO = Data.Substring(8, 8);
                                }
                            }
                            else
                            {
                                CardNO = gsd.GetCardNO(CPH);
                                if (CardNO == "")
                                {
                                    CardNO = Data.Substring(8, 8);
                                }
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

                                    int ID = Dzds[0].ID;
                                    //bll.UpdateICFaXingDate(CardNO);
                                    gsd.UpdateICFaXingDate(ID);
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
                        //2016-10-10 新增
                        if (CardType == "")
                        {
                            CardType = "TmpA";
                        }
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


                    //}
                    //出场时间
                    //string OutTime = "";    
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

                    WriteTemp(CardNO, CardType, dtInTime, dtOutTime, SFJE, SFJE, Data, strName + str, dr.BigSmall, CPH);

                    bool bOffLineOut = true;
                    if (gsd.GetOffLineOut(CPH, dtInTime) > 0)
                    {
                        bOffLineOut = false;
                    }

                    if (flag == "0" && CardType.Substring(0, 3) != "Opt")
                    {


                        //Model.AdmissionModel model = new Model.AdmissionModel();
                        //model.CardNO = CardNO;
                        //model.CPH = CPH;
                        //model.CardType = CardType;
                        //model.InTime = dtInTime;
                        //model.OutTime = dtOutTime;
                        //model.InGateName = strName + str;
                        //model.InOperator = Model.sUserName;
                        //model.InOperatorCard = Model.sUserCard;
                        //model.OutOperatorCard = "";
                        //model.OutOperator = "";
                        //model.SFJE = SFJE;
                        //model.SFTime = DateTime.Now;
                        //model.OvertimeSFTime = DateTime.Now;
                        //model.InOut = Convert.ToInt32(dr["InOut"].ToString());
                        //model.BigSmall = Convert.ToInt32(dr["BigSmall"].ToString());


                        CarIn ci = new CarIn();
                        ci.CardNO = CardNO;
                        ci.CPH = CPH;
                        ci.CardType = CardType;
                        ci.InTime = dtInTime;
                        ci.OutTime = dtOutTime;
                        ci.InGateName = strName + str;
                        ci.InOperator = Model.sUserName;
                        ci.InOperatorCard = Model.sUserCard;
                        ci.OutOperatorCard = "";
                        ci.OutOperator = "";
                        ci.SFJE = SFJE;
                        ci.SFTime = DateTime.Now;
                        ci.OvertimeSFTime = DateTime.Now;
                        ci.StationID = Model.stationID;
                        ci.CarparkNO = Model.iParkingNo;
                        //ci.InOut = Convert.ToInt32(dr["InOut"].ToString());
                        ci.BigSmall = dr.BigSmall;

                        gsd.DeleteInOutCPH(CPH, dr.BigSmall, ci.InTime);

                        if (ci.CPH.Length > 6 && !bOffLineOut)
                        {
                            gsd.UpdateIn(ci);
                        }
                        else
                        {
                            //!!!
                            gsd.AddAdmission(ci, 10);
                        }

                    }
                    else
                    {
                        if (CardType.Contains("Str"))
                        {
                            //BLL.IssueCardBLL Ibll = new BLL.IssueCardBLL();
                            //Ibll.UpdateICYU(CardNO, Balance);

                            gsd.UpdateICYU(CardNO, Balance);

                        }

                        //Model.OutNameModel model = new Model.OutNameModel();
                        //model.CardNO = CardNO;
                        //model.CPH = CPH;
                        //model.CardType = CardType;
                        //model.InTime = dtInTime;
                        //model.OutTime = dtOutTime;
                        //model.OutGateName = strName + str;
                        //model.OutOperator = Model.sUserName;
                        //model.OutOperatorCard = Model.sUserCard;

                        CarOut co = new CarOut();
                        co.CardNO = CardNO;
                        co.CPH = CPH;
                        co.CardType = CardType;
                        co.InTime = dtInTime;
                        co.OutTime = dtOutTime;
                        co.OutGateName = strName + str;
                        co.OutOperator = Model.sUserName;
                        co.OutOperatorCard = Model.sUserCard;
                        co.CarparkNO = Model.iParkingNo;
                        co.StationID = Model.stationID;

                        if (co.CardType.Substring(0, 3) == "Tmp" || co.CardType.Substring(0, 3) == "Str" || (co.CardType.Substring(0, 3) == "Mth" && Model.bMonthFdSf == false && Model.iYKOverTimeCharge == 1))
                        {
                            if (co.CardType.Substring(0, 3) == "Tmp")
                            {
                                if (Model.iXsd == 0)
                                {
                                    if (Model.iChargeType == 3)
                                    {
                                        if (Model.iXsdNum == 1)
                                        {
                                            co.SFJE = SFJE / 10;
                                        }
                                        else
                                        {
                                            co.SFJE = SFJE / 100;
                                        }
                                    }
                                    else
                                    {
                                        co.SFJE = SFJE;
                                    }
                                }
                                else
                                {
                                    co.SFJE = SFJE / 10;
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
                                            co.SFJE = SFJE / 10;
                                        }
                                        else
                                        {
                                            co.SFJE = SFJE / 100;
                                        }
                                    }
                                    else
                                    {
                                        co.SFJE = SFJE;
                                    }
                                }
                                else
                                {
                                    co.SFJE = SFJE / 10;
                                }
                            }
                        }
                        else
                        {
                            co.SFJE = 0;
                        }

                        co.YSJE = co.SFJE;
                        // model.SFJE = SFJE;
                        co.Balance = Balance;
                        co.SFTime = dtOutTime;
                        co.OvertimeSFTime = DateTime.Now;
                        //!!!
                        //co.InOut = Convert.ToInt32(dr["InOut"].ToString());
                        co.BigSmall = dr.BigSmall;


                        if (co.CPH.Length > 6 && gsd.UpdateInOut(co) > 0)
                        {

                        }
                        else
                        {
                            if (co.CardType.Substring(0, 3) == "Tmp")
                            {
                                gsd.DeleteInOutCPH(CPH, dr.BigSmall, (co.InTime != null ? co.InTime : co.OutTime != null ? co.OutTime : DateTime.Now));
                            }
                            else
                            {
                                List<CarIn> MyRsX = gsd.GetMyRsX(co.CardNO, "", Model.iParkingNo, dr.BigSmall);
                                if (MyRsX.Count > 0)
                                {
                                    co.InTime = MyRsX[0].InTime;
                                }
                            }
                            //!!!

                            //增加相同车配在同一分钟的记录不保存到出场表 2016-11-17
                            if (co.CPH != "" && co.CPH != "66666666" && co.CPH != "00000000" && co.CPH != "88888888" && gsd.GetOutCPHMinutesCount(co.CPH, dr.BigSmall, co.OutTime) > 0)
                            {
                                gsd.AddLog("脱机记录" + "：提取到脱机记录：", co.CPH + "同一车牌同一分钟连续入出场不保存记录");
                            }
                            else
                                gsd.AddOutName(co, 10);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                //log.Add("读取脱机记录" + ":BinModel", ex.Message + "\r\n" + ex.StackTrace + Data);

                gsd.AddLog("读取脱机记录" + ":BinModel", ex.Message + "\r\n" + ex.StackTrace + Data);
                //label6.Text = ex.Message;
            }
        }


        /// <summary>
        /// 保存历史记录
        /// </summary>
        /// <param name="strTmp"></param>
        private void WriteTemp(string strCardNO, string strCardType, DateTime dtInTime, DateTime dtOutTime, decimal dSFJE, decimal dYSJE, string strTmp, string strGateName, int iBigSmall, string strCPH)
        {
            try
            {
                string str1 = "";
                str1 = strTmp + " " + Model.strRevision.Substring(13); //20111214

                //Model.RecordMemoryModel model = new Model.RecordMemoryModel();
                //model.InOperatorCard = Model.sUserCard;
                //model.InOperator = Model.sUserName;
                //model.OutOperator = "";
                //model.OutOperatorCard = "";
                //model.InPic = "";
                //model.InUser = "";
                //model.OutPic = "";
                //model.OutUser = "";
                //model.ZJPic = "";
                //model.SFTime = DateTime.Now;
                //model.CardType = strCardType;
                //model.CardNO = strCardNO;
                //model.InTime = dtInTime;
                //model.OutTime = dtOutTime;
                //model.OvertimeSFTime = DateTime.Now;
                //model.SFJE = dSFJE;
                //model.YSJE = dYSJE;
                //model.InGateName = strGateName;
                //model.OutGateName = "";
                //model.FreeReason = str1;
                //model.BigSmall = iBigSmall;
                //model.CarparkNO = Model.iParkingNo;
                //model.CPH = strCPH;

                RawRecord rr = new RawRecord();
                rr.InOperatorCard = Model.sUserCard;
                rr.InOperator = Model.sUserName;
                rr.OutOperator = "";
                rr.OutOperatorCard = "";
                rr.InPic = "";
                rr.InUser = "";
                rr.OutPic = "";
                rr.OutUser = "";
                rr.ZJPic = "";
                rr.SFTime = DateTime.Now;
                rr.CardType = strCardType;
                rr.CardNO = strCardNO;
                rr.InTime = dtInTime;
                rr.OutTime = dtOutTime;
                rr.OvertimeSFTime = DateTime.Now;
                rr.SFJE = dSFJE;
                rr.YSJE = dYSJE;
                rr.InGateName = strGateName;
                rr.OutGateName = "";
                rr.FreeReason = str1;
                rr.BigSmall = iBigSmall;
                rr.CPH = strCPH;
                rr.CarparkNO = Model.iParkingNo;
                rr.StationID = Model.stationID;
                gsd.AddRecordMemory(rr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                //log.Add("在线监控" + ":WriteTemp", ex.Message + "\r\n" + ex.StackTrace);
                gsd.AddLog("在线监控" + ":WriteTemp", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void ForceOffLine()
        {
            List<CheDaoSet> ds = gsd.GetCtrName(Model.stationID);
            foreach (var dr in ds)
            {
                int XieYi = dr.XieYi;
                string Count = "0";

                if (XieYi == 1)
                {
                    sendbll.SetUsbType(ref usbHid, XieYi);  //2015-09-18
                    Count = sendbll.ForceOffLine(dr.IP, dr.CtrlNumber, XieYi);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //2016-04-11 强制脱机指令
            ForceOffLine();

            if (usbHid.deviceOpened == true)
            {
                usbHid.CloseDevice();
            }
            if (usbHid != null)   //2015-08-20
            {
                usbHid = null;
            }
            if (!Clear)
                System.Environment.Exit(0);
        }
    }
}
