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
using ParkingModel;
using System.Data;
using System.Threading;
using System.Text.RegularExpressions;
using ParkingInterface;

namespace UI
{
    /// <summary>
    /// ParkingDownCard.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingDownCard : SFMControls.WindowBase
    {
        private System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        private Hid usbHid = new Hid();
        private int State = 0;
        GetServiceData gsd = new GetServiceData();

        //private delegate void outputDelegate(string txt1, string txt2);
        private Thread fThread;

        
        public ParkingDownCard()
        {
            InitializeComponent();
        }

       
        private void dTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (State == 0)
                {
                    fThread = new Thread(new ThreadStart(GetDownLoad));//开辟一个新的线程
                    fThread.Start();
                }
                else if (State == 2)
                {

                }
                else
                {
                    dTimer.Stop();
                    this.Close();
                    Model.Quit_Flag = true;
                    Model.ReadRecord = 1;
                }

            }
            catch (Exception ex)
            {
                gsd.AddLog("进入在线监控前卡号下载:timer1_Tick", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\ntimer1_Tick", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                dTimer.Tick += new EventHandler(dTimer_Tick);
                dTimer.Interval = new TimeSpan(0, 0, 1);
                dTimer.Start();
                CR.BinDic(gsd.GetCardType());
            }
            catch(Exception ex)
            {
                gsd.AddLog("进入在线监控前卡号下载:ParKingDownCard1_Load", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nParKingDownCard1_Load", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GetDownLoad()
        {
            try
            {
                State = 2;

                //DownCPHCard();

                //有效卡号下载
                if (Model.iSFLed == 0)
                {
                    DownCPHCard();
                    //车牌号退卡
                    FindDeleCPHCard();
                    //黑名单下载
                    DownBlacklistCard();
                    DownDBlacklistCard();


                    //DownCard();
                    //恢复待解挂的卡号
                    //FindAllResumeCards();
                    ////挂失待挂失的卡号
                    //FindAllLostCards();
                    ////退卡
                    //FindDeleCard();
                }
                ////车牌号退卡
                //FindDeleCPHCard();
                ////黑名单下载
                //DownBlacklistCard();
                //DownDBlacklistCard();
                State = 1;
            }
            catch (Exception ex)
            {
                gsd.AddLog("GetDownLoad", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// 有效车牌号下载
        /// </summary>
        private void DownCPHCard()
        {
            List<CardIssue> Downds = gsd.GetFaXingCPHDownLoad(Model.stationID);

            if (Downds == null)
            {
                return;
            }

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                //lblCount.Content = Downds.Tables[0].Rows.Count.ToString();
                                                lblShow.Content = "有效车牌号下载";
                                            }));

            int k = 0;

           

            foreach (var dr in Downds)
            {
                k++;
                //2016-12-12 add
                if (Model.stationID <= 0)
                {
                    return;
                }

                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = dr.CPHDownloadSignal;  //2015-11-24
                string sumBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;
                //progressBar1.Value += progressBar1.Step;

                //根据ID修改记录
                //string CarNO = dr["CardNO"].ToString();

                int ID = dr.ID;
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
                if (Model.iDetailLog == 1)
                {
                    CR.WriteToTxtFile(dr.CPH + " 下载车牌");
                }


                SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                string QstrJiHao = "";
                string strCJiHao = "";
                if (dr.CPH != "" && dr.CPH != "66666666" && dr.CPH != "88888888" && dr.CPH.Length > 6)
                {
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {

                        if (strJiHaoSum.Substring(Model.Channels[i].iCtrlID - 1, 1) == "0")
                        {
                            strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[i].iCtrlID);
                            strYDownLoadJihao = strCJiHao;
                            string a = "";
                            if (Model.Channels[i].iXieYi == 1 || Model.Channels[i].iXieYi == 3)
                            {
                                DateTime dt = DateTime.Now;
                                while (dt.AddSeconds(Model.iDelayed) > DateTime.Now)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[i].iXieYi);
                                    if (Model.iDetailLog == 1)
                                    {
                                        CR.WriteToTxtFile(dr.CPH + " 产生下载字符：" + CR.GetDownLoadToCPH(dr));
                                    }
                                    a = sendbll.DownloadCard(Model.Channels[i].sIP, Model.Channels[i].iCtrlID, CR.GetDownLoadToCPH(dr), Model.Channels[i].iXieYi);
                                    if (a != "2")
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    lblShow.Content = "有效车牌号下载";
                                        //});

                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "有效车牌号下载";
                                            }));
                                        break;
                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "连接中断...";
                                            }));

                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    lblShow.Content = "连接中断...";
                                        //});
                                    }
                                }
                            }

                            if (a == "0")
                            {
                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[i].iCtrlID);
                                strSDownLoadJihao = QstrJiHao;
                            }
                            else
                            {
                                //this.Invoke((EventHandler)delegate
                                //{
                                //    if (Model.iDetailLog)
                                //    {
                                //        CR.WriteToTxtFile(dr["CPH"].ToString() + " 下位机返回：" + a);
                                //    }
                                //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    return;
                                //    //this.Close();
                                //});

                                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                if (Model.iDetailLog==1)
                                                {
                                                    CR.WriteToTxtFile(dr.CPH + " 下位机返回：" + a);
                                                }
                                                MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                return;
                                            }));
                            }

                        }
                        else
                        {
                            strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[i].iCtrlID);
                            strYDownLoadJihao = strCJiHao;
                            string a = "";
                            if (Model.Channels[i].iXieYi == 1 || Model.Channels[i].iXieYi == 3)
                            {
                                DateTime dt = DateTime.Now;
                                while (dt.AddSeconds(Model.iDelayed) > DateTime.Now)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[i].iXieYi);
                                    if (Model.iDetailLog == 1)
                                    {
                                        CR.WriteToTxtFile(dr.CPH + " 删除下载字符：" + CR.GetDownLoadToCPH(dr));
                                    }
                                    a = sendbll.DownLossloadCard(Model.Channels[i].sIP, Model.Channels[i].iCtrlID, CR.GetDownLoadToCPH(dr), Model.Channels[i].iXieYi);
                                    if (a != "2")
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    lblShow.Text = "有效车牌号下载";
                                        //});


                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "有效车牌号下载";
                                            }));

                                        break;
                                    }
                                    else
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    lblShow.Text = "连接中断...";
                                        //});

                                        this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "连接中断..";
                                            }));
                                    }
                                }
                            }

                            if (a == "0")
                            {
                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[i].iCtrlID);
                                strSDownLoadJihao = QstrJiHao;
                            }
                            else
                            {
                                //this.Invoke((EventHandler)delegate
                                //{
                                //    if (Model.iDetailLog)
                                //    {
                                //        CR.WriteToTxtFile(dr["CPH"].ToString() + " 下位机返回：" + a);
                                //    }
                                //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    return;
                                //});

                                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               if (Model.iDetailLog == 1)
                                               {
                                                   CR.WriteToTxtFile(dr.CPH + " 下位机返回：" + a);
                                               }
                                               MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                               return;
                                           }));
                            }

                        }
                    }
                }
                if (QstrJiHao == strCJiHao)
                {
                    //bll.UpdateCPHDownLoad(CarNO, sumBiao);

                    gsd.UpdateCPHDownLoad(ID, sumBiao);
                }

                //this.Invoke((EventHandler)delegate
                //{
                //    pgbShow.Value = Convert.ToInt32(k * 100 / Downds.Tables[0].Rows.Count);
                //});
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               pgbShow.Value = Convert.ToInt32(k * 100 / Downds.Count);
                                           }));

            }
        }


        /// <summary>
        /// 有效卡号下载
        /// </summary>
        private void DownCard()
        {
            List<CardIssue> Downds = gsd.GetFaXingDownLoad(Model.iICCardDownLoad, Model.stationID);
            //this.Invoke((EventHandler)delegate
            //{
            //    lblCount.Content = Downds.Tables[0].Rows.Count.ToString();
            //    l.Text = "卡号下载";
            //});
            if (Downds == null)
            {
                return;
            }


            this.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
               new Action(() =>
               {
                   lblCount.Content = Downds.Count.ToString();
                   lblShow.Content = "有效车牌号下载";
               }));

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
                //string CarNO = dr["CardNO"].ToString();

                int ID = dr.ID;
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
                for (int i = 0; i < Model.iChannelCount; i++)
                {
                    strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[i].iCtrlID);
                    strYDownLoadJihao = strCJiHao;

                    if (strJiHaoSum.Substring(Model.Channels[i].iCtrlID - 1, 1) == "0")
                    {
                        string a = "";
                        if (Model.Channels[i].iXieYi == 1 || Model.Channels[i].iXieYi == 3)
                        {
                            DateTime dt = DateTime.Now;
                            while (dt.AddSeconds(Model.iDelayed) > DateTime.Now)
                            {
                                sendbll.SetUsbType(ref usbHid, Model.Channels[i].iXieYi);
                                a = sendbll.DownloadCard(Model.Channels[i].sIP, Model.Channels[i].iCtrlID, CR.GetDownLoad(dr), Model.Channels[i].iXieYi);
                                if (a != "2")
                                {
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    label1.Text = "卡号下载";
                                    //});

                                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "卡号下载";
                                            }));
                    
                                    break;
                                }
                                else
                                {
                                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "连接中断...";
                                            }));

                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    label1.Text = "连接中断...";
                                    //});
                                }
                            }
                        }
                        
                        if (a == "0")
                        {
                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[i].iCtrlID);
                            strSDownLoadJihao = QstrJiHao;
                            //bll.UpdateDownLoad(CarNO, sumBiao);
                        }
                        else
                        {
                            //this.Invoke((EventHandler)delegate
                            //{
                            //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    return;
                            //    //this.Close();
                            //});

                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                return;
                                            }));
                        }
                        // bll.UpdateDownLoad(CarNO, sumBiao);
                    }
                    else
                    {
                        string a = "";
                        if (Model.Channels[i].iXieYi == 1 || Model.Channels[i].iXieYi == 3)
                        {
                            DateTime dt = DateTime.Now;
                            while (dt.AddSeconds(Model.iDelayed) > DateTime.Now)
                            {
                                sendbll.SetUsbType(ref usbHid, Model.Channels[i].iXieYi);
                                a = sendbll.DownLossloadCard(Model.Channels[i].sIP, Model.Channels[i].iCtrlID, CR.GetDownLoad(dr), Model.Channels[i].iXieYi);
                                if (a != "2")
                                {
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    label1.Text = "卡号下载";
                                    //});
                                    //break;

                                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "卡号下载";
                                            }));
                                }
                                else
                                {
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    label1.Text = "连接中断...";
                                    //});

                                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                lblShow.Content = "连接中断...";
                                            }));
                                }
                            }
                        }
                        
                        if (a == "0")
                        {
                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[i].iCtrlID);
                            strSDownLoadJihao = QstrJiHao;
                        }
                        else
                        {
                            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                          new Action(() =>
                                          {
                                              MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                              return;
                                          }));
                        }

                    }
                }
       
                if (QstrJiHao == strCJiHao)
                {
                    gsd.UpdateDownLoad(ID, sumBiao);
                }
               
                //this.Invoke((EventHandler)delegate
                //{
                //    progressBar1.Value = Convert.ToInt32(k * 100 / Downds.Tables[0].Rows.Count);
                //});

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                          new Action(() =>
                                          {
                                              pgbShow.Value = Convert.ToInt32(k * 100 / Downds.Count);
                                          }));
            }
        }


        /// <summary>
        /// 恢复待解挂的卡号
        /// </summary>
        private void FindAllResumeCards()
        {

            List<CheDaoSet> dtParking = gsd.GetParking();

            string strPingk = "";
            for (int a = 0; a < 128; a++)
            {
                strPingk += "1";
            }

            foreach (var dr in dtParking)
            {
                string str1 = strPingk.Substring(0, dr.CtrlNumber - 1); //ab
                string str2 = strPingk.Substring(dr.CtrlNumber); //d
                strPingk = str1 + "0" + str2; //abmd
            }


            SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
            List<CardIssue> ds = gsd.GetJLost("3");

            if (ds == null || ds.Count == 0)
            {
                return;
            }
            //this.Invoke((EventHandler)delegate
            //{
            //    lblCount.Text = ds.Tables[0].Rows.Count.ToString();
            //    label1.Text = "恢复待解挂的卡号下载";
            //});

            this.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
               new Action(() =>
               {
                   lblCount.Content = ds.Count.ToString();
                   lblShow.Content = "恢复待解挂的卡号下载";
               }));
            int Count = ds.Count;
            //lblCount.Text = Count.ToString();
            int k = 0;
            foreach (var dr in ds)
            {
                k++;
                //progressBar1.Value += progressBar1.Step;
                string CarNO = dr.CardNO;

                int ID = dr.ID;
                //根据ID号修改记录

                List<CardIssue> Fds = gsd.GetFaXing(CarNO);
                var Fdr = Fds[0];

                string strJiHao = Fdr.CarValidMachine;
                string strLossJiHao = Fdr.CarFailID;
                string[] sArray = Regex.Split(strLossJiHao, ",", RegexOptions.IgnoreCase);
                string strsum = "";
                foreach (char a in strJiHao.ToCharArray())
                {
                    strsum += ConvertToBin(a);
                }
                //发行有效机号和车道比对
                for (int i = 0; i < 128; i++)
                {
                    string strB1 = strsum.Substring(i, 1);
                    string strB2 = strPingk.Substring(i, 1);
                    if (strB1 != strB2)
                    {
                        string str11 = strPingk.Substring(0, i); //ab
                        string str12 = strPingk.Substring(i + 1); //d
                        strPingk = str11 + "1" + str12; //abmd
                    }
                }

                //新加下载标记
                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = Fdr.DownloadSignal;
                string sumBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;
                //新加解挂成功机号
                string strLossMachine = Fdr.CardLossMachine;
                string strMachine = "";
                foreach (char a in strLossMachine.ToCharArray())
                {
                    strMachine += ConvertToBin(a);
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

                string strFileJiHao = "";
                string QstrJiHao = "";
                string strCJiHao = "";
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    if (sArray[0] != "")
                    {
                        foreach (var x in sArray)
                        {
                            if (x != "" && Convert.ToInt32(x) == Model.Channels[j].iCtrlID)
                            {
                                strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[j].iCtrlID);
                                strYDownLoadJihao = strCJiHao;
                                //string count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr));
                                //if (count == "0")
                                //{
                                //    QstrJiHao = GetJihao(Model.Channels[j].iCtrlID);
                                //}

                                if (CarNO.Length >= 8)
                                {
                                    string count = "";
                                    if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                    {
                                        sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                        count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                    }
                                  
                                    if (count == "0")
                                    {
                                        QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                        strSDownLoadJihao = QstrJiHao;
                                        strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                    }
                                    else
                                    {
                                        strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                    }
                                }
                                else
                                {
                                    if (Model.iICCardDownLoad == 0)
                                    {
                                        string count = "";
                                        if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                        {
                                            sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                            count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                        }
                                       
                                        if (count == "0")
                                        {
                                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                            strSDownLoadJihao = QstrJiHao;
                                            strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                        }
                                        else
                                        {
                                            //this.Invoke((EventHandler)delegate
                                            //{
                                            //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                            //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            //    return;
                                            //});

                                            this.Dispatcher.Invoke(
                                                   System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                   new Action(() =>
                                                   {
                                                       strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                       MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                       return;
                                                   }));
                                        }
                                    }
                                    else //IC卡下载有效
                                    {
                                        if (CR.GetCardType(dr.CarCardType, 0).Substring(0, 3) == "Mth" && CR.GetCardType(dr.CarCardType, 0).Substring(0, 3) == "Fre")
                                        {
                                            string count = "";
                                            if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                            {
                                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                            }
                                            
                                            if (count == "0")
                                            {
                                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                strSDownLoadJihao = QstrJiHao;
                                                strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                            }
                                            else
                                            {
                                                //this.Invoke((EventHandler)delegate
                                                //{
                                                //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //    return;
                                                //});
                                                this.Dispatcher.Invoke(
                                                   System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                   new Action(() =>
                                                   {
                                                       strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                       MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                       return;
                                                   }));
                                            }
                                        }
                                        else
                                        {
                                            string count = "";
                                            if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                            {
                                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                            }
                                           
                                            if (count == "0")
                                            {
                                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                strSDownLoadJihao = QstrJiHao;
                                                strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                            }
                                            else
                                            {
                                                //this.Invoke((EventHandler)delegate
                                                //{
                                                //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //    return;
                                                //});

                                                this.Dispatcher.Invoke(
                                                   System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                   new Action(() =>
                                                   {
                                                       strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                       MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                       return;
                                                   }));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < strsum.Length; i++)
                        {
                            if (strsum.Substring(i, 1) == "0")
                            {
                                if ((int)i + 1 == Model.Channels[j].iCtrlID)
                                {
                                    strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[j].iCtrlID);
                                    strYDownLoadJihao = strCJiHao;
                                    

                                    if (CarNO.Length >= 8)
                                    {
                                        string count = "";
                                        if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                        {
                                            sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                            count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                        }
                                        
                                        if (count == "0")
                                        {
                                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                            strSDownLoadJihao = QstrJiHao;
                                            strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                        }
                                    }
                                    else
                                    {
                                        if (Model.iICCardDownLoad == 0)
                                        {
                                            string count = "";
                                            if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                            {
                                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                            }
                                           
                                            if (count == "0")
                                            {
                                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                strSDownLoadJihao = QstrJiHao;
                                                strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                            }
                                            else
                                            {
                                                //this.Invoke((EventHandler)delegate
                                                //{
                                                //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //    return;
                                                //});

                                                this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                            }
                                        }
                                        else //IC卡下载有效
                                        {
                                            if (dr.CardType.Substring(0, 3) == "Mth" && dr.CardType.Substring(0, 3) == "Fre")
                                            {
                                                string count = "";
                                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                                {
                                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                    count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                                }
                                                
                                                if (count == "0")
                                                {
                                                    QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                    strSDownLoadJihao = QstrJiHao;
                                                    strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                                }
                                                else
                                                {
                                                    //MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    //this.Close();

                                                  this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      this.Close();
                                                  }));
                                                }
                                            }
                                            else
                                            {
                                                string count = "";
                                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                                {
                                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                    count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                                }

                                                if (count == "0")
                                                {
                                                    QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                    strSDownLoadJihao = QstrJiHao;
                                                    strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                                }
                                                else
                                                {
                                                    //MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    //this.Close();

                                                    this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      this.Close();
                                                  }));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //this.Invoke((EventHandler)delegate
                    //{
                    //    progressBar1.Value = Convert.ToInt32(k * 100 / Count);
                    //});

                    this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      pgbShow.Value = Convert.ToInt32(k * 100 / Count);
                                                      
                                                  }));

                }
                string strJihao = "";
                for (int j = 0; j < QstrJiHao.Length / 4; j++)
                {
                    strJihao += string.Format("{0:X}", Convert.ToInt32(QstrJiHao.Substring(j * 4, 4), 2));
                }
                string strTemp1 = strJihao;
                if (QstrJiHao == strCJiHao)
                {

                    if (strPingk == strMachine)//设置的车道机号和挂失下载成功机号
                    {
                        //bll.GetDownLoad(sumBiao, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", CarNO);
                        gsd.GetDownLoad(sumBiao, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", ID);


                        //!!!
                        //string State = bll.CardLost(CarNO, "", "", 0, 0, 0);
                    }
                    else
                    {
                        string strJihaoMachine = "";
                        for (int j = 0; j < strMachine.Length / 4; j++)
                        {
                            strJihaoMachine += string.Format("{0:X}", Convert.ToInt32(strMachine.Substring(j * 4, 4), 2));
                        }
                        //bll.GetDownLoad(sumBiao, strJihaoMachine, CarNO);

                        gsd.GetDownLoad(sumBiao, strJihaoMachine, ID);

                        //!!!
                        //string State = bll.CardLost(CarNO, "", "", 0, 0, 1);
                    }
                }
                else
                {
                    if (strFileJiHao.Length > 0)
                    {
                        strFileJiHao = strFileJiHao.Substring(0, strFileJiHao.Length - 1);
                    }
                    string strJihaoMachine = "";
                    for (int j = 0; j < strMachine.Length / 4; j++)
                    {
                        strJihaoMachine += string.Format("{0:X}", Convert.ToInt32(strMachine.Substring(j * 4, 4), 2));
                    }

                    gsd.GetDownLoad(strbiaozhi, strJihaoMachine, ID);

                    //bll.GetDownLoad(strbiaozhi, strJihaoMachine, CarNO);

                    //!!!
                    //string State = bll.CardLost(CarNO, strFileJiHao, "", 0, 0, 1);
                   
                }
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    for (int i = 0; i < strsum.Length; i++)
                    {
                        if (strsum.Substring(i, 1) == "0")
                        {
                            if ((int)i + 1 == Model.Channels[j].iCtrlID)
                            {
                                string count = "";
                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                    count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, "FE0000000000000000000000000000000", Model.Channels[j].iXieYi);
                                }
                              
                                if (count == "0")
                                {
                                }
                                else
                                {
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //    return;
                                    //});

                                    this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                }
                            }
                        }
                    }
                }


            }
        }


        /// <summary>
        /// 挂失待挂失的卡号
        /// </summary>
        private void FindAllLostCards()
        {
            List<CheDaoSet> dtParking = gsd.GetParking();

            string strPingk = "";
            for (int a = 0; a < 128; a++)
            {
                strPingk += "1";
            }

            foreach (var dr in dtParking)
            {
                string str1 = strPingk.Substring(0, dr.CtrlNumber - 1); //ab
                string str2 = strPingk.Substring(dr.CtrlNumber); //d
                strPingk = str1 + "0" + str2; //abmd
            }


            SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);

            //GetJLost --- GetLost 相同
            List<CardIssue> ds = gsd.GetJLost("1");
            
            //this.Invoke((EventHandler)delegate
            //{
            //    lblCount.Text = ds.Tables[0].Rows.Count.ToString();
            //    label1.Text = "挂失待挂失的卡号下载";
            //});

            if (ds == null || ds.Count == 0)
            {
                return;
            }

            this.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
               new Action(() =>
               {
                   lblCount.Content = ds.Count.ToString();
                   lblShow.Content = "挂失待挂失的卡号下载";
               }));

            int Count = ds.Count;

            int k = 0;
            foreach (var dr in ds)
            {
                k++;
                //progressBar1.Value += progressBar1.Step;
                string CarNO = dr.CardNO;
                int ID = dr.ID;

                List<CardIssue> Fds = gsd.GetFaXing(CarNO);
                var Fdr = Fds[0];

                string strJiHao = Fdr.CarValidMachine;
                string strLossJiHao = Fdr.CarFailID;
                string[] sArray = Regex.Split(strLossJiHao, ",", RegexOptions.IgnoreCase);
                string strsum = "";
                foreach (char a in strJiHao.ToCharArray())
                {
                    strsum += ConvertToBin(a);
                }
                //发行有效机号和车道比对
                for (int i = 0; i < 128; i++)
                {
                    string strB1 = strsum.Substring(i, 1);
                    string strB2 = strPingk.Substring(i, 1);
                    if (strB1 != strB2)
                    {
                        string str11 = strPingk.Substring(0, i); //ab
                        string str12 = strPingk.Substring(i + 1); //d
                        strPingk = str11 + "1" + str12; //abmd
                    }
                }


                //新加下载标记
                int biaozhi = Model.stationID;//需要替换的标志位
                string strbiaozhi = Fdr.DownloadSignal;
                string sumBiao = "";
                string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                string str2 = strbiaozhi.Substring(biaozhi);
                sumBiao = str1 + "1" + str2;
                //新加解挂成功机号
                string strLossMachine = Fdr.CardLossMachine;
                string strMachine = "";
                foreach (char a in strLossMachine.ToCharArray())
                {
                    strMachine += ConvertToBin(a);
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
                string strFileJiHao = "";
                string QstrJiHao = "";
                string strCJiHao = "";
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    if (sArray[0] != "")
                    {
                        foreach (var x in sArray)
                        {
                            if (x != "" && Convert.ToInt32(x) == Model.Channels[j].iCtrlID)
                            {
                                strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[j].iCtrlID);
                                strYDownLoadJihao = strCJiHao;
                                

                                if (CarNO.Length >= 8)
                                {
                                    string count = "";
                                    if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                    {
                                        sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                        count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                    }
                                   
                                    if (count == "0")
                                    {
                                        QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                        strSDownLoadJihao = QstrJiHao;
                                        strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                    }
                                    else
                                    {
                                        strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                    }
                                }
                                else
                                {
                                    if (Model.iICCardDownLoad == 0)
                                    {
                                        string count = "";
                                        if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                        {
                                            sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                            count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                        }
                                        
                                        if (count == "0")
                                        {
                                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                            strSDownLoadJihao = QstrJiHao;
                                            strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                        }
                                        else
                                        {
                                            //this.Invoke((EventHandler)delegate
                                            //{
                                            //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                            //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            //    return;
                                            //});


                                            this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                        }
                                    }
                                    else //IC卡下载有效
                                    {
                                        if (CR.GetCardType(dr.CarCardType, 0).Substring(0, 3) == "Mth" && CR.GetCardType(dr.CarCardType, 0).Substring(0, 3) == "Fre")
                                        {
                                            string count = "";
                                            if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                            {
                                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                            }
                                           
                                            if (count == "0")
                                            {
                                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                strSDownLoadJihao = QstrJiHao;
                                                strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                            }
                                            else
                                            {
                                                this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                            }
                                        }
                                        else
                                        {
                                            string count = "";
                                            if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                            {
                                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                            }
                                           
                                            if (count == "0")
                                            {
                                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                strSDownLoadJihao = QstrJiHao;
                                                strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                            }
                                            else
                                            {
                                                //this.Invoke((EventHandler)delegate
                                                //{
                                                //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //    return;
                                                //});

                                                this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < strsum.Length; i++)
                        {
                            if (strsum.Substring(i, 1) == "0")
                            {
                                if ((int)i + 1 == Model.Channels[j].iCtrlID)
                                {
                                    strCJiHao = GetJihao(strYDownLoadJihao, Model.Channels[j].iCtrlID);
                                    strYDownLoadJihao = strCJiHao;
                                  
                                    if (CarNO.Length >= 8)
                                    {
                                        string count = "";
                                        if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                        {
                                            sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                            count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                        }
                                      
                                        if (count == "0")
                                        {
                                            QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                            strSDownLoadJihao = QstrJiHao;
                                            strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                        }
                                        else
                                        {
                                            //this.Invoke((EventHandler)delegate
                                            //{
                                            //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                            //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            //    return;
                                            //});

                                            this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                        }
                                    }
                                    else
                                    {
                                        if (Model.iICCardDownLoad == 0)
                                        {
                                            string count = "";
                                            if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                            {
                                                sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                            }
                                           
                                            if (count == "0")
                                            {
                                                QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                strSDownLoadJihao = QstrJiHao;
                                                strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                            }
                                            else
                                            {
                                                //this.Invoke((EventHandler)delegate
                                                //{
                                                //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //    return;
                                                //});

                                                this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                            }
                                        }
                                        else
                                        {
                                            if (dr.CardType.ToString().Substring(0, 3) == "Mth" && dr.CardType.ToString().Substring(0, 3) == "Fre")
                                            {
                                                string count = "";
                                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                                {
                                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                    count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                                }
                                                
                                                if (count == "0")
                                                {
                                                    QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                    strSDownLoadJihao = QstrJiHao;
                                                    strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                                }
                                                else
                                                {
                                                    //this.Invoke((EventHandler)delegate
                                                    //{
                                                    //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                    //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    //    return;
                                                    //});

                                                    this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                                }
                                            }
                                            else
                                            {
                                                string count = "";
                                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                                {
                                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                                    count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                                }
                                              
                                                if (count == "0")
                                                {
                                                    QstrJiHao = GetJihao(strSDownLoadJihao, Model.Channels[j].iCtrlID);
                                                    strSDownLoadJihao = QstrJiHao;
                                                    strMachine = GetMach(strMachine, Model.Channels[j].iCtrlID);
                                                }
                                                else
                                                {
                                                    //this.Invoke((EventHandler)delegate
                                                    //{
                                                    //    strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                    //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    //    return;
                                                    //});

                                                    this.Dispatcher.Invoke(
                                                            System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                            new Action(() =>
                                                            {
                                                                strFileJiHao += Model.Channels[j].iCtrlID + ",";
                                                                MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                                return;
                                                            }));

                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                    //this.Invoke((EventHandler)delegate
                    //{
                    //    progressBar1.Value = Convert.ToInt32(k * 100 / Count);
                    //});

                    this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      pgbShow.Value = Convert.ToInt32(k * 100 / Count);
                                                  }));
                }
                string strJihao = "";
                for (int j = 0; j < QstrJiHao.Length / 4; j++)
                {
                    strJihao += string.Format("{0:X}", Convert.ToInt32(QstrJiHao.Substring(j * 4, 4), 2));
                }
                string strTemp1 = strJihao;
                if (QstrJiHao == strCJiHao)
                {
                    if (strPingk == strMachine)//设置的车道机号和挂失下载成功机号
                    {
                        //bll.GetDownLoad(sumBiao, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", CarNO);

                        gsd.GetDownLoad(sumBiao, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", ID);

                        //!!!
                        //string State = bll.CardLost(CarNO, "", "", 0, 2, 0);
                    }
                    else
                    {
                        string strJihaoMachine = "";
                        for (int j = 0; j < strMachine.Length / 4; j++)
                        {
                            strJihaoMachine += string.Format("{0:X}", Convert.ToInt32(strMachine.Substring(j * 4, 4), 2));
                        }
                        //bll.GetDownLoad(sumBiao, strJihaoMachine, CarNO);

                        gsd.GetDownLoad(sumBiao, strJihaoMachine, ID);

                        //!!!
                        //string State = bll.CardLost(CarNO, "", "", 0, 2, 1);
                    }

                }
                else
                {
                    string strJihaoMachine = "";
                    for (int j = 0; j < strMachine.Length / 4; j++)
                    {
                        strJihaoMachine += string.Format("{0:X}", Convert.ToInt32(strMachine.Substring(j * 4, 4), 2));
                    }
                    //bll.GetDownLoad(strbiaozhi, strJihaoMachine, CarNO);

                    gsd.GetDownLoad(sumBiao, strJihaoMachine, ID);

                    //!!!
                    //string State = bll.CardLost(CarNO, strFileJiHao, "", 0, 2, 1);
                    
                }
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    for (int i = 0; i < strsum.Length; i++)
                    {
                        if (strsum.Substring(i, 1) == "0")
                        {
                            if ((int)i + 1 == Model.Channels[j].iCtrlID)
                            {
                                string count = "";
                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                    count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, "FE0000000000000000000000000000000", Model.Channels[j].iXieYi);
                                }
                                
                            }
                        }
                    }
                }

            }

        }


        /// <summary>
        /// 退卡
        /// </summary>
        private void FindDeleCard()
        {
            SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
            List<CardIssue> ds = gsd.GetOutCard(Model.stationID);
            //this.Invoke((EventHandler)delegate
            //{
            //    lblCount.Text = ds.Tables[0].Rows.Count.ToString();
            //    label1.Text = "退卡处理";
            //});
            if (ds == null)
            {
                return;
            }

           this.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
               new Action(() =>
               {
                   lblCount.Content = ds.Count;
                   lblShow.Content = "退卡处理";
               }));

            int Count = ds.Count;
            int k = 0;
            foreach (var dr in ds)
            {
                k++;
                //progressBar1.Value += progressBar1.Step;
                string CarNO = dr.CardNO;
                int ID = dr.ID;

                List<CardIssue> Fds = gsd.GetFaXing(CarNO);
                CardIssue Fdr = Fds[0];

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
                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                    count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dr), Model.Channels[j].iXieYi);
                                }
                                
                                if (count == "0")
                                {
                                    Rststr = Rststr + "0";
                                }
                                else
                                {
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //    return;
                                    //});

                                    this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
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
                    //bll.UpdateTKDownLoad(CarNO, sumBiao);
                    gsd.UpdateTKDownLoad(ID, sumBiao);

                    //!!!
                    //bll.UpdateIDLost(CarNO, 0);
                }

                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    for (int i = 0; i < strsum.Length; i++)
                    {
                        if (strsum.Substring(i, 1) == "0")
                        {
                            if ((int)i + 1 == Model.Channels[j].iCtrlID)
                            {

                                string count = "";
                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                    count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, "FE0000000000000000000000000000000", Model.Channels[j].iXieYi);
                                }
                               

                            }
                        }
                    }
                }

                //this.Invoke((EventHandler)delegate
                //{
                //    progressBar1.Value = Convert.ToInt32(k * 100 / Count);
                //});


                this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      pgbShow.Value = Convert.ToInt32(k * 100 / Count);

                                                  }));
            }
        }


        /// <summary>
        /// 车牌号退卡
        /// </summary>
        private void FindDeleCPHCard()
        {
            SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
            List<CardIssue> ds = gsd.GetOutCPHCard(Model.stationID);
            //this.Invoke((EventHandler)delegate
            //{
            //    lblCount.Text = ds.Tables[0].Rows.Count.ToString();
            //    label1.Text = "车牌号退卡处理";
            //});

            if (ds == null)
            {
                return;
            }

            this.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
               new Action(() =>
               {
                   lblCount.Content = ds.Count;
                   lblShow.Content = "车牌号退卡处理";
               }));


            int Count = ds.Count;
            int k = 0;
            foreach (var dr in ds)
            {
                k++;
                //progressBar1.Value += progressBar1.Step;
                string CarNO = dr.CardNO;
                int ID = dr.ID;

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
                if (Model.iDetailLog==1)
                {
                    CR.WriteToTxtFile(dr.CPH +"车牌退卡下载-------------");
                }
                //替换成车道
                // Dictionary<int, string> dic = CR.GetIP();
                string Rststr = "";
                if (dr.CPH != "" && dr.CPH != "66666666" && dr.CPH != "88888888")
                {
                    for (int j = 0; j < Model.iChannelCount; j++)
                    {
                        for (int i = 0; i < strsum.Length; i++)
                        {
                            if (strsum.Substring(i, 1) == "0")
                            {
                                if ((int)i + 1 == Model.Channels[j].iCtrlID)
                                {
                                    string count = "";
                                    if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                    {
                                        sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                        if (Model.iDetailLog==1)
                                        {
                                            CR.WriteToTxtFile(dr.CPH + " 注销车牌下载字符：" + CR.GetDownLoadToCPH(dr));
                                        }
                                        count = sendbll.DownLossloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoadToCPH(dr), Model.Channels[j].iXieYi);
                                    }

                                    if (count == "0")
                                    {
                                        Rststr += "0";
                                    }
                                    else
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    if (Model.iDetailLog)
                                        //    {
                                        //        CR.WriteToTxtFile(dr["CPH"].ToString() + " 注销车牌下位机返回：" + count);
                                        //    }
                                        //    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //    return;
                                        //});

                                        this.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                  new Action(() =>
                                                  {
                                                      if (Model.iDetailLog==1)
                                                      {
                                                          CR.WriteToTxtFile(dr.CPH + " 注销车牌下位机返回：" + count);
                                                      }
                                                      MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                                      return;
                                                  }));
                                    }
                                }

                            }
                        }
                    }
                }
                string StrCount = "";
                for (int y = 0; y < Model.iChannelCount; y++)
                {
                    StrCount += "0";
                }
                if (StrCount == Rststr)
                {
                    gsd.UpdateCPHDownLoad(ID, sumBiao);
                }

                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    for (int i = 0; i < strsum.Length; i++)
                    {
                        if (strsum.Substring(i, 1) == "0")
                        {
                            if ((int)i + 1 == Model.Channels[j].iCtrlID)
                            {

                                string count = "";
                                if (Model.Channels[j].iXieYi == 1 || Model.Channels[j].iXieYi == 3)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[j].iXieYi);
                                    count = sendbll.DownloadCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, "FE0000000000000000000000000000000", Model.Channels[j].iXieYi);
                                }
                               
                            }
                        }
                    }
                }

                //this.Invoke((EventHandler)delegate
                //{
                //    progressBar1.Value = Convert.ToInt32(k * 100 / Count);
                //});


                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                       new Action(() =>
                                       {
                                             pgbShow.Value = Convert.ToInt32(k * 100 / Count);
                                        }));
            }
        }


        /// <summary>
        /// 黑名单下载
        /// </summary>
        private void DownDBlacklistCard()
        {
            List<Blacklist> Downds = gsd.GetBlacklistDCPHDownLoad(Model.stationID);

            //this.Invoke((EventHandler)delegate
            //{
            //    lblCount.Text = Downds.Tables[0].Rows.Count.ToString();
            //    label1.Text = "黑名单下载";
            //});

            if (Downds == null)
            {
                return;
            }

            this.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
               new Action(() =>
               {
                   lblCount.Content = Downds.Count.ToString();
                   lblShow.Content = "黑名单下载";
               }));

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
                //progressBar1.Value += progressBar1.Step;
                sumTBiao = str1 + "0" + str2;
                int iAddDelete = dr.AddDelete;

                //替换成车道
                //Dictionary<int, string> dic = CR.GetIP();
                //dic[1] = "192.168.1.99";
                //dic[3] = "192.168.1.98";

                SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                string QstrJiHao = "";
                string strCJiHao = "";
                if (dr.CPH != "" && dr.CPH != "66666666" && dr.CPH != "88888888")
                {
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        QstrJiHao = QstrJiHao + "1";

                        string a = "";
                        if (Model.Channels[i].iXieYi == 1 || Model.Channels[i].iXieYi == 3)
                        {
                            DateTime dt = DateTime.Now;
                            while (dt.AddSeconds(Model.iDelayed) > DateTime.Now)
                            {
                                sendbll.SetUsbType(ref usbHid, Model.Channels[i].iXieYi);
                                a = sendbll.DownLossloadCard(Model.Channels[i].sIP, Model.Channels[i].iCtrlID, CR.GetDownBlacklistCPH(dr), Model.Channels[i].iXieYi);
                                if (a != "2")
                                {
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    label1.Text = "卡号下载";
                                    //});

                                    this.Dispatcher.Invoke(
                                        System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                        new Action(() =>
                                        {
                                            lblShow.Content = "卡号下载";
                                         }));
                                    break;
                                }
                                else
                                {
                                    //this.Invoke((EventHandler)delegate
                                    //{
                                    //    label1.Text = "连接中断...";
                                    //});

                                    this.Dispatcher.Invoke(
                                        System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                        new Action(() =>
                                        {
                                            lblShow.Content = "连接中断...";
                                        }));
                                    break;
                                }
                            }
                        }
                       
                        if (a == "0")
                        {
                            strCJiHao = strCJiHao + "1";
                        }

                    }
                }
                if (QstrJiHao == strCJiHao)
                {
                    //bll.UpdateBlacklistDownLoad(dr["CPH"].ToString(), sumTBiao);

                    gsd.UpdateBlacklistDownLoad(dr.ID, sumTBiao);

                    if (iAddDelete == 1 && sumTBiao == "000000000000000")
                    {
                        //BLL.ParkingSetBLL pbll = new BLL.ParkingSetBLL();
                        //pbll.DeleteMYBlacklist(Convert.ToInt32(dr["ID"].ToString()));
                        gsd.DeleteMYBlacklist(dr.ID);
                    }
                }

                //this.Invoke((EventHandler)delegate
                //{
                //    progressBar1.Value = Convert.ToInt32(k * 100 / Downds.Tables[0].Rows.Count);
                //});
                this.Dispatcher.Invoke(
                                        System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                        new Action(() =>
                                        {
                                            pgbShow.Value = Convert.ToInt32(k * 100 / Downds.Count);
                                        }));
                break;
            }


        }


        /// <summary>
        /// 黑名单下载
        /// </summary>
        private void DownBlacklistCard()
        {
            List<Blacklist> Downds = gsd.GetBlacklistCPHDownLoad(Model.stationID);

            if (Downds == null)
            {
                return;
            }

            this.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
               new Action(() =>
               {
                   lblCount.Content = Downds.Count.ToString();
                   lblShow.Content = "黑名单下载";
               }));

            //this.Invoke((EventHandler)delegate
            //{
            //    lblCount.Text = Downds.Tables[0].Rows.Count.ToString();
            //    label1.Text = "黑名单下载";
            //});

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
                //progressBar1.Value += progressBar1.Step;
                sumTBiao = str1 + "0" + str2;
                int iAddDelete = dr.AddDelete;

                //替换成车道
                //Dictionary<int, string> dic = CR.GetIP();
                //dic[1] = "192.168.1.99";
                //dic[3] = "192.168.1.98";

                SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                string QstrJiHao = "";
                string strCJiHao = "";
                if (dr.CPH != "" && dr.CPH != "66666666" && dr.CPH != "88888888")
                {
                    for (int i = 0; i < Model.iChannelCount; i++)
                    {
                        QstrJiHao = QstrJiHao + "1";
                        if (iAddDelete == 0)
                        {

                            string a = "";
                            if (Model.Channels[i].iXieYi == 1 || Model.Channels[i].iXieYi == 3)
                            {
                                DateTime dt = DateTime.Now;
                                while (dt.AddSeconds(Model.iDelayed) > DateTime.Now)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[i].iXieYi);
                                    a = sendbll.DownloadCard(Model.Channels[i].sIP, Model.Channels[i].iCtrlID, CR.GetDownBlacklistCPH(dr), Model.Channels[i].iXieYi);
                                    if (a != "2")
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    label1.Text = "卡号下载";
                                        //});

                                        this.Dispatcher.Invoke(
                                                System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                new Action(() =>
                                                {
                                                    lblShow.Content = "卡号下载";
                                                }));
                                        break;
                                    }
                                    else
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    label1.Text = "连接中断...";
                                        //});

                                        this.Dispatcher.Invoke(
                                               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                               new Action(() =>
                                               {
                                                   lblShow.Content = "连接中断...";
                                               }));
                                    }
                                }
                            }
                            
                            if (a == "0")
                            {
                                strCJiHao = strCJiHao + "1";
                                //bll.UpdateDownLoad(CarNO, sumBiao);
                            }
                            else
                            {
                                //this.Invoke((EventHandler)delegate
                                //{
                                //    //MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    return;
                                //    //this.Close();
                                //});

                                this.Dispatcher.Invoke(
                                               System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                               new Action(() =>
                                               {
                                                   return;
                                               }));
                            }
                            

                        }
                        else
                        {
                            string a = "";
                            if (Model.Channels[i].iXieYi == 1 || Model.Channels[i].iXieYi == 3)
                            {
                                DateTime dt = DateTime.Now;
                                while (dt.AddSeconds(Model.iDelayed) > DateTime.Now)
                                {
                                    sendbll.SetUsbType(ref usbHid, Model.Channels[i].iXieYi);
                                    a = sendbll.DownLossloadCard(Model.Channels[i].sIP, Model.Channels[i].iCtrlID, CR.GetDownBlacklistCPH(dr), Model.Channels[i].iXieYi);
                                    if (a != "2")
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    label1.Text = "卡号下载";
                                        //});

                                        this.Dispatcher.Invoke(
                                                System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                new Action(() =>
                                                {
                                                    lblShow.Content = "卡号下载";
                                                }));
                                        break;
                                    }
                                    else
                                    {
                                        //this.Invoke((EventHandler)delegate
                                        //{
                                        //    label1.Text = "连接中断...";
                                        //});

                                        this.Dispatcher.Invoke(
                                                System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                                new Action(() =>
                                                {
                                                    lblShow.Content = "连接中断...";
                                                }));
                                    }
                                }
                            }
                            
                            if (a == "0")
                            {
                                strCJiHao = strCJiHao + "1";
                            }
                        }

                    }
                }
                if (QstrJiHao == strCJiHao)
                {
                    //bll.UpdateBlacklistDownLoad(dr["CPH"].ToString(), sumBiao);
                    gsd.UpdateBlacklistDownLoad(dr.ID, sumBiao);

                    if (iAddDelete == 1 && sumTBiao == "000000000000000")
                    {
                        //BLL.ParkingSetBLL pbll = new BLL.ParkingSetBLL();
                        //pbll.DeleteMYBlacklist(Convert.ToInt32(dr["ID"].ToString()));

                        gsd.DeleteMYBlacklist(dr.ID);
                    }
                }

                //this.Invoke((EventHandler)delegate
                //{
                //    progressBar1.Value = Convert.ToInt32(k * 100 / Downds.Tables[0].Rows.Count);
                //});

                this.Dispatcher.Invoke(
                                       System.Windows.Threading.DispatcherPriority.Normal, //WPF中跨线程访问控件的方法
                                       new Action(() =>
                                       {
                                           pgbShow.Value = Convert.ToInt32(k * 100 / Downds.Count);
                                       }));
            }
        }


        private string GetJihao(string sum, int JiHao)
        {
            string str1 = sum.Substring(0, Convert.ToInt32(JiHao) - 1); //ab
            string str2 = sum.Substring(Convert.ToInt32(JiHao)); //d
            sum = str1 + "1" + str2; //abmd
            return sum;
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


        private string GetMach(string sum, int JiHao)
        {
            string str1 = sum.Substring(0, Convert.ToInt32(JiHao) - 1); //ab
            string str2 = sum.Substring(Convert.ToInt32(JiHao)); //d
            sum = str1 + "0" + str2; //abmd
            return sum;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            fThread.Abort();
            CommSend.ComClose();
            if (usbHid != null)
            {
                usbHid.CloseDevice();
                usbHid = null;
            }

          
        }
    }
}
