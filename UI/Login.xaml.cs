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
using System.IO;
using System.Configuration;
using ParkingCommunication;

namespace UI
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : SFMControls.WindowBase
    {

        GetServiceData gsd = new GetServiceData();
        bool IsClear = false; // 退出时用到
        string curStationID = "";

        public Login()
        {
            InitializeComponent();  //555555
        }

        void updConfig(List<Operators>lstOper,List<StationSet>lstSt)
        {
            // 界面上的操作
            if (lstOper.Count > 0)
            {
                cbbUserName.ItemsSource = lstOper;
                cbbUserName.DisplayMemberPath = "UserName";
                cbbUserName.SelectedValuePath = "UserNO";
                if (cbbUserName.Items.Count > 0)
                    cbbUserName.SelectedIndex = 0;              
            }

            if (lstSt.Count > 0)
            {
                cbbStationId.ItemsSource = lstSt;
                cbbStationId.DisplayMemberPath = "StationName";
                cbbStationId.SelectedValuePath = "StationId";
                if (cbbStationId.Items.Count > 0)
                {
                    if (curStationID != null && curStationID != "")
                    {
                        for (int i = 0; i < lstSt.Count; i++)
                        {
                            if (Convert.ToInt16(curStationID) == lstSt[i].StationId)
                            {
                                cbbStationId.SelectedIndex = i;
                                break;
                            }
                            if (i == lstSt.Count - 1)
                            {
                                cbbStationId.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        cbbStationId.SelectedIndex = 0;
                    }    
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ImageBrush berriesBrush = new ImageBrush();
                //berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Login.jpg"), UriKind.Absolute));
                //this.Background = berriesBrush;

                string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "ParkingInterface.dll");
                if (File.Exists(path))// 可以替代将ServiceIP放到SharedPrefs中;
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                    Model.serverIP = config.AppSettings.Settings["ServiceIP"].Value;
                    Model.serverPort = config.AppSettings.Settings["ServicePort"].Value;

                    // 判断数字
                    curStationID = config.AppSettings.Settings.AllKeys.Contains("StationID") ? 
                        (CR.IsNumberic(config.AppSettings.Settings["StationID"].Value) ? config.AppSettings.Settings["StationID"].Value : "") 
                        : "";

                    if (!CR.IsIP(Model.serverIP) || !CR.IsNumberic(Model.serverPort))
                    {
                        MessageBox.Show("服务IP或端口格式不正确,请重新配置");
                    }
                    else
                    {
                        bool LoadDataSucceed = false;
                        List<ParkingModel.Operators> lstOptr = null;
                        List<ParkingModel.StationSet> lstStation = null;

                        // 这里是异步执行吗?
                        Task t = new Task(() =>
                        {
                            Request req;

                            try
                            {
                                req = new Request();
                                lstOptr = req.GetData<List<ParkingModel.Operators>>("GetOperatorsWithoutLogin");

                                lstStation = req.GetData<List<ParkingModel.StationSet>>("GetStationSetWithoutLogin", null, null, "StationId");
                                LoadDataSucceed = true;//
                            }
                            catch (System.Net.WebException wex)
                            {
                                MessageBox.Show(wex.Message, "提示");
                                return;
                            }
                        });
                        t.Start();
                        Task.WaitAny(new Task[] { t }, 3000);//等待3s，然后判断是加载数据成功，会出现相应的延时;

                        if (!LoadDataSucceed)
                        {
                            frmServerSet frm = new frmServerSet(new Action<List<Operators>, List<StationSet>>(updConfig)); // 显示服务器参数设置
                            frm.Owner = this;
                            frm.ShowDialog();
                        }
                        else
                        {
                            cbbStationId.ItemsSource = lstStation;
                            cbbStationId.DisplayMemberPath = "StationName";
                            cbbStationId.SelectedValuePath = "StationId";

                            cbbUserName.ItemsSource = lstOptr;
                            cbbUserName.DisplayMemberPath = "UserName";
                            cbbUserName.SelectedValuePath = "UserNO";

                            cbbUserName.SelectedIndex = 0;
                            cbbStationId.SelectedIndex = 0;

                            if (curStationID != null && curStationID != "")
                            {
                                for (int i = 0; i < lstStation.Count; i++)
                                {
                                    if (lstStation[i].StationId == Convert.ToInt16(curStationID))
                                    {
                                        cbbStationId.SelectedIndex = i;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("配置文件丢失，请联系管理员");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nWindow_Loaded", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //狗识别
        //void DogRecognition()
        //{
        //    cpi.tcc2 cpiX = new cpi.tcc2();
        //    if (Model.bZhuCe)//2015-08-05
        //    {

        //        string strddd = @"C:\WINDOWS\system32\cad.dat";
        //        if (!System.IO.File.Exists(strddd))
        //        {
        //            MessageBox.Show("数据丢失 DH ！", "提示");
        //            System.Windows.Forms.Application.ExitThread();
        //            return;
        //        }

        //        //string path1 = System.IO.Path.GetFullPath("cad.dsn");
        //        string strXX = "";
        //        for (int i = 0; i < 21; i++)
        //        {
        //            StringBuilder strRst = new StringBuilder(255);
        //            int aa = CR.GetPrivateProfileString("ODBC", "STR" + i.ToString(), "", strRst, 255, strddd);
        //            strXX += strRst.ToString();
        //        }
        //        short[] iT = new short[5];

        //        string strTmp0 = cpiX.strSetupDH(ref strXX, ref iT[0], ref iT[1], ref iT[2], ref iT[3], ref iT[4]);
        //        Model.iXieYi = iT[1];
        //        ParkingCommunication.Config.HeadIndex = iT[1];
        //        Model.iType = iT[0];

        //        Model.strTmpInTime = strTmp0;

        //        //2015-10-16
        //        string[] sVerArray = Model.strRevision.Split('.');
        //        if (sVerArray.Length >= 5)
        //        {
        //            sVerArray[4] = Model.iType.ToString();
        //            sVerArray[5] = Model.iXieYi.ToString();
        //            Model.strRevision = string.Join(".", sVerArray);
        //        }
        //        else
        //        {
        //            Model.strRevision = "ZNYKT-T-YT.1.1.002." + Model.iType + "." + Model.iXieYi + ".151015";
        //        }

        //        if (CR.IsTime(strTmp0))
        //        {
        //            DateTime dtStart = Convert.ToDateTime("2014-06-27 00:00:00");
        //            DateTime dtEnd = Convert.ToDateTime(strTmp0);
        //            if (DateTime.Now > dtEnd)
        //            {
        //                MessageBox.Show("系统数据溢出 DH ！", "提示");
        //                System.Windows.Forms.Application.ExitThread();
        //                return;
        //            }
        //        }
        //    }

        //    if (Model.iAutoPlateEn == 1)
        //    {
        //        //加密狗操作
        //        bool bStart = false;
        //        short iTmp = 0;
        //        string strTmp = "";

        //        //cpi.tcc2 cpiX = new cpi.tcc2();
        //        Model.lType = cpiX.CommStr(Convert.ToInt16(Model.iXieYi));
        //        //sModel.lType = 187300;
        //        if (Model.iAutoPattern == 0)
        //        {
        //            bStart = cpiX.MinAutoStr(Convert.ToInt16(Model.iRsbYsb), 2, iTmp, strTmp);//iTmp 2，4双路
        //        }
        //        else if (Model.iAutoPattern == 1)
        //        {
        //            bStart = cpiX.MinAutoStr(Convert.ToInt16(Model.iRsbYsb), Convert.ToInt16(Model.iAutoPlateType), iTmp, strTmp);
        //        }
        //        else if (Model.iAutoPattern == 2)
        //        {
        //            bStart = cpiX.MinAutoStr(1, Convert.ToInt16(Model.iAutoPlateType), ref iTmp, strTmp);
        //            if ((799 < iTmp && iTmp < 900) || (1800 < iTmp && iTmp < 2001) || iTmp > 3800)  //(1800 < iTmp && iTmp < 2001)用于四路狗//更改为3800后为四路
        //            {
        //                Model.bDStart = true;
        //            }
        //            else
        //            {
        //                Model.bDStart = false;
        //            }
        //        }
        //        Model.bStart = bStart;
        //    }
        //    else
        //    {
        //        bool bStart = false;
        //        short iTmp = 0;
        //        Model.lType = cpiX.CommStr(Convert.ToInt16(Model.iXieYi));
        //        bStart = cpiX.Txt2Str(Convert.ToInt16(Model.iDogType), ref iTmp, ref iTmp, Model.bDogTypeEn, Convert.ToInt16(Model.iType), Convert.ToInt16(Model.iSoftType));
        //        Model.bStart = bStart;
        //    }
        //}


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string data;
                ParkingInterface.Request req;
                ParkingModel.Operators optr = null;
                ParkingModel.StationSet staSet = null;

                optr = cbbUserName.SelectedItem as ParkingModel.Operators;
                staSet = cbbStationId.SelectedItem as ParkingModel.StationSet;

                if (null == optr)
                {
                    MessageBox.Show("请选择操作员");
                    return;
                }
                if (null == staSet)
                {
                    MessageBox.Show("请选择工作站");
                    return;
                }

                req = new ParkingInterface.Request();

                // 获取Token
                data = req.GetToken(optr.UserNO, CR.UserMd5(txtPwd.Password.Trim()));
                

                if (null != data && data.Trim().Length > 0)  //d41d8cd98f00b204e9800998ecf8427e
                {
                    // 保存数据到Model中，
                    Model.token = data;
                    Model.stationID = staSet.StationId;
                    Model.iParkingNo = staSet.CarparkNO; // 车场编号
                    Model.sUserPwd = CR.UserMd5(txtPwd.Password.Trim());

                    string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "ParkingInterface.dll");
                    if (File.Exists(path))
                    {
                        if (curStationID != null && curStationID != "")
                        {
                            if (Convert.ToInt16(curStationID) == staSet.StationId)// 比较站点的情况，相同不作切换操作
                            {

                            }
                            else
                            {
                                if (MessageBox.Show("是否切换工作站，请谨慎操作?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);// 打开配置文件
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    dic["StationID"] = staSet.StationId;
                                    bool ret = ConfigFile.UpdateAppConfig(config, dic);// ConfigFile管理类  config dic 存储stationId数据
                                    if (ret)
                                        curStationID = dic["StationID"].ToString();
                                }
                                else
                                {
                                    List<StationSet> lstSS = cbbStationId.ItemsSource as List<StationSet>;
                                    for (int i = 0; i < lstSS.Count; i++)
                                    {
                                        if (Convert.ToInt16(curStationID) == lstSS[i].StationId)
                                        {
                                            cbbStationId.SelectedIndex = i;
                                            if (cbbStationId.SelectedIndex > -1)
                                            {
                                                staSet = cbbStationId.SelectedItem as ParkingModel.StationSet;
                                                Model.stationID = staSet.StationId;
                                                Model.iParkingNo = staSet.CarparkNO;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic["StationID"] = staSet.StationId;
                            bool ret = ConfigFile.UpdateAppConfig(config, dic);
                            if (ret)
                                curStationID = dic["StationID"].ToString();
                        }
                    }

                    #region 准备工作
                    //设置本地日期格式
                    CR.SetDateTimeFormat();
                    //数据定义
                    Model.DataInit();
                    ParkingCommunication.Config.HeadIndex = Model.iXieYi; // ParkingCommunication配置什么参数
                    gsd.DataSourceToPubVar();

                    //2016-11-25  新增计费型摄像机
                    //Model.bIsKZB = !Model.bAppEnable;

                    Model.bOut485 = !Model.bIsKZB;
                    Model.bVideoCamera = !Model.bIsKZB;
                    //Model.iAutoUpdateJiHao = Convert.ToInt32(!Model.bIsKZB);

                    //DogRecognition();
                    CR.SetSysTime(gsd.GetSysTime()); // 设置系统时间，请求服务器数据
                    #endregion


                    //string passWord = CR.UserMd5(txtPwd.Password.Trim().ToString());
                    
                    if (CR.GetAppConfig("UserCode") == Model.sUserCard)//用户卡号和登录时间的登记
                    {
                        Model.dLoginTime = Convert.ToDateTime(CR.GetAppConfig("LoginDate"));
                    }
                    else
                    {
                        CR.UpdateAppConfig("UserCode", Model.sUserCard);
                        CR.UpdateAppConfig("LoginDate", DateTime.Now.ToString());
                        Model.dLoginTime = DateTime.Now;
                    }

                    List<Operators> lstOperators = gsd.GetOperators(optr.UserNO);

                    if (lstOperators != null && lstOperators.Count > 0)
                    {
                        Model.sUserName = lstOperators[0].UserName; //找到第一个操作员，下标为0;
                        Model.sUserCard = lstOperators[0].CardNO;
                        Model.sGroupNo = lstOperators[0].UserLevel;// 权限组
                        Model.lstRights = gsd.GetRights(Model.sGroupNo);// 请求权限
                    }

                    CR.LoadRights(true);    //czh 2016-10-12 这里有什么区别?

                    //如果基表里没有权限则重新生成
                    if (Model.lstRights == null || Model.lstRights.Count == 0)
                    {
                        //List<RightsItem> lstRI = new List<RightsItem>();
                        //lstRI.Add(new RightsItem() { FormName = "系统管理", ItemName = "", Category = "车场", PID = 0, Description = "系统管理" });
                        //lstRI.Add(new RightsItem() { FormName = "人事管理", ItemName = "", Category = "车场", PID = 0, Description = "人事管理" });
                        //lstRI.Add(new RightsItem() { FormName = "车牌管理", ItemName = "", Category = "车场", PID = 0, Description = "车牌管理" });
                        //lstRI.Add(new RightsItem() { FormName = "车场管理", ItemName = "", Category = "车场", PID = 0, Description = "车场管理" });
                        //lstRI.Add(new RightsItem() { FormName = "报表查询", ItemName = "", Category = "车场", PID = 0, Description = "报表查询" });
                        //gsd.SetRightsItem(lstRI);
                        MessageBox.Show("无进入在线监控权限", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        List<Rights> lstRs = gsd.GetRightsByName("在线监控", "CmdView");// 获取权限;从权限组中获取
                        if (lstRs.Count > 0)
                        {
                            if (lstRs[0].CanOperate)
                            {
                                this.Hide();

                                IsClear = false;
                                //if (CR.GetAppConfig("IsStartGuide") == "1")
                                //{
                                    ParkingDownCardTest parkdown = new ParkingDownCardTest();
                                    parkdown.ShowDialog();

                                    ParkingReadRecordTest parkrecord = new ParkingReadRecordTest();
                                    parkrecord.ShowDialog();

                                    //ParkingDownCard parkdown = new ParkingDownCard();
                                    //parkdown.ShowDialog();

                                    //ParkingReadRecord parkrecord = new ParkingReadRecord();
                                    //parkrecord.ShowDialog();

                                    ParkingMonitoring parkmonitor = new ParkingMonitoring();// 中间有一堆的数据；
                                    parkmonitor.initFields();
                                    parkmonitor.initControl();

                                    if (Model.iEnableVideo == 1 || Model.iEnableNetVideo == 1)
                                    {
                                        parkmonitor.Myinitcaptrure();// 具体的图像的显示
                                    }
                                    parkmonitor.Show();

                                    gsd.AddLog("分布式停车场管理系统", "登陆");
                                //}
                                //else
                                //{
                                //    Model.isStartGuide = true;
                                //    ParkingSetting park = new ParkingSetting();
                                //    if (park.ShowDialog() == true)
                                //    {
                                //        CR.UpdateAppConfig("IsStartGuide", "1");

                                //        ParkingDownCard parkdown = new ParkingDownCard();
                                //        parkdown.ShowDialog();

                                //        ParkingReadRecord parkrecord = new ParkingReadRecord();
                                //        parkrecord.ShowDialog();

                                //        ParkingMonitoring parkmonitor = new ParkingMonitoring();
                                //        parkmonitor.initFields();
                                //        parkmonitor.initControl();

                                //        if (Model.iEnableVideo == 1 || Model.iEnableNetVideo == 1)
                                //        {
                                //            parkmonitor.Myinitcaptrure();
                                //        }
                                //        parkmonitor.Show();

                                //        gsd.AddLog("分布式停车场管理系统", "登陆");
                                //    }
                                //    else
                                //    {
                                //        this.Show();
                                //    }
                                //}
                            }
                            else
                            {
                                MessageBox.Show("无进入在线监控权限", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                        else
                        {
                            //long ID = gsd.GetIDByName("车场管理", "");
                            //List<RightsItem> lstRI = new List<RightsItem>();
                            //lstRI.Add(new RightsItem() { FormName = "在线监控", ItemName = "CmdView", Category = "车场", Description = "在线监控", PID = ID });
                            //gsd.SetRightsItem(lstRI);
                            MessageBox.Show("无进入在线监控权限", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("用户名或者密码输入错误，请重新登陆!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnLogin_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnLogin_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }


        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            // 进入到服务器设置页面中
            frmServerSet frm = new frmServerSet(new Action<List<Operators>, List<StationSet>>(updConfig));
            frm.Owner = this;
            frm.ShowDialog();
        }

  

        private void btnParkSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string data;
                ParkingInterface.Request req;
                ParkingModel.Operators optr = null;
                ParkingModel.StationSet staSet = null;

                optr = cbbUserName.SelectedItem as ParkingModel.Operators;
                staSet = cbbStationId.SelectedItem as ParkingModel.StationSet;

                if (null == optr)
                {
                    MessageBox.Show("请选择操作员");
                    return;
                }
                if (null == staSet)
                {
                    MessageBox.Show("请选择工作站");
                    return;
                }

                req = new ParkingInterface.Request();

                data = req.GetToken(optr.UserNO, CR.UserMd5(txtPwd.Password.Trim()));

                if (null != data && data.Trim().Length > 0)
                {
                    Model.token = data;
                    Model.stationID = staSet.StationId;
                    Model.iParkingNo = staSet.CarparkNO;


                    string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "ParkingInterface.dll");
                    if (File.Exists(path))
                    {
                        if (curStationID != null && curStationID != "")
                        {
                            if (Convert.ToInt16(curStationID) == staSet.StationId)
                            {

                            }
                            else
                            {
                                if (MessageBox.Show("是否切换工作站，请谨慎操作?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    dic["StationID"] = staSet.StationId;
                                    bool ret = ConfigFile.UpdateAppConfig(config, dic);
                                    if (ret)
                                        curStationID = dic["StationID"].ToString();
                                }
                                else
                                {
                                    List<StationSet> lstSS = cbbStationId.ItemsSource as List<StationSet>;
                                    for (int i = 0; i < lstSS.Count; i++)
                                    {
                                        if (Convert.ToInt16(curStationID) == lstSS[i].StationId)
                                        {
                                            cbbStationId.SelectedIndex = i;
                                            if (cbbStationId.SelectedIndex > -1)
                                            {
                                                staSet = cbbStationId.SelectedItem as ParkingModel.StationSet;
                                                Model.stationID = staSet.StationId;
                                                Model.iParkingNo = staSet.CarparkNO;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic["StationID"] = staSet.StationId;
                            bool ret = ConfigFile.UpdateAppConfig(config, dic);
                            if (ret)
                                curStationID = dic["StationID"].ToString();
                        }
                    }

                    //Model.bAppEnable = true;
                    //Model.bIsKZB = !Model.bAppEnable;

                    ////016-11-25  新增计费型摄像机
                    Model.bOut485 = !Model.bIsKZB;
                    Model.bVideoCamera = !Model.bIsKZB;
                    //Model.iAutoUpdateJiHao = Convert.ToInt32(!Model.bIsKZB);

                    if (Model.bAppEnable == true)
                    {
                        Model.iXieYi = 0;
                        ParkingCommunication.Config.HeadIndex = 0;
                        Model.DataInitRevision();
                    }

                    List<Operators> lstOperators = gsd.GetOperators(optr.UserNO);

                    if (lstOperators != null && lstOperators.Count > 0)
                    {
                        Model.sUserName = lstOperators[0].UserName;
                        Model.sUserCard = lstOperators[0].CardNO;
                        Model.sGroupNo = lstOperators[0].UserLevel;
                        Model.lstRights = gsd.GetRights(Model.sGroupNo);
                    }

                    if (Model.lstRights == null || Model.lstRights.Count == 0)
                    {
                        MessageBox.Show("无进入车场设置权限", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        //this.Hide();
                        List<Rights> lstRs = gsd.GetRightsByName("车场设置", "");
                        if (lstRs.Count > 0)
                        {
                            if (lstRs[0].CanOperate)
                            {
                                this.Hide();

                                IsClear = false;
                                //Model.isStartGuide = false;
                                //ParkingSetting parkset = new ParkingSetting();
                                //parkset.ShowDialog();

                                ParkingSet parkset = new ParkingSet();
                                if (parkset.ShowDialog() == true)
                                {
                                    IsClear = true;
                                }
                                this.Show();
                            }
                            else
                            {
                                MessageBox.Show("无进入车场设置权限", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                        else
                        {
                            //long ID = gsd.GetIDByName("车场管理", "");
                            //List<RightsItem> lstRI = new List<RightsItem>();
                            //lstRI.Add(new RightsItem() { FormName = "车场设置", ItemName = "", Category = "车场", Description = "车场设置", PID = ID });
                            //gsd.SetRightsItem(lstRI);
                            MessageBox.Show("无进入车场设置权限", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("用户名或者密码输入错误，请重新登陆!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnParkSet_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnParkSet_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void WindowBase_Closed(object sender, EventArgs e)
        {
            if (IsClear)
            {
                System.Environment.Exit(0);
            }   
        }
    }
}