using ParkingInterface;
using ParkingModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UI
{
    /// <summary>
    /// ParkingTempGob_big.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingTempGob_big : SFMControls.WindowBase
    {
        int modulus = 0;
        bool CanClose = false;
        bool NeedConfirmCPH;
        CarIn inRecord;
        CarOut outRecord;
        CaclMoneyResult _Money;
        Request req = new Request();
        Action<string> SFDataHandler;
        Action<string> PhotoDataHandler;
        Action<string> CPHHandler;
        Action<string, decimal> CardTypeSFJEHandler;
        Action<ParkingModel.CarOut> FinishedHandler;
        Label lblOnlinePayLayer = new Label();
        ParkingCommunication.VoiceSend vSender;
        DispatcherTimer tmrCloseForm;

        CaclMoneyResult Money
        {
            get { return _Money; }
            set
            {
                _Money = value;
                UpdateFormTitle();
            }
        }

        private void UpdateFormTitle()
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() =>
                {
                    UpdateFormTitle();
                });
                return;
            }

            this.Title = "临时车出场收费";
            if (null != _Money && null != _Money.DiscountSet)
            {
                this.Title += string.Format(" - {0} 优惠 {1}{2}", (_Money.DiscountSet.DeptId > 0 ? _Money.DiscountSet.DeptName : _Money.DiscountSet.Address),
                    _Money.DiscountSet.Favorable, _Money.DiscountSet.Manner);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idxOfChannel">设备在Model.Channels中的索引</param>
        /// <param name="_NeedConfirmCPH">是否需要人工确认车牌</param>
        /// <param name="_outRecord">如果需要人工确认车牌，此对象中传出场信息即可，否则要传出场与入场信息(包括ID)</param>
        /// <param name="_SFDataHandler">点击“开闸放行”按钮后触发</param>
        /// <param name="_PhotoDataHandler">点击“证件抓拍”按钮时触发</param>
        /// <param name="_CardTypeSFJEHandler">选择卡类型后触发</param>
        /// <param name="_CPHHandler">确认车牌后触发</param>
        /// <param name="_FinishedHandler">窗口关闭后触发</param>
        public ParkingTempGob_big(int idxOfChannel, bool _NeedConfirmCPH, ParkingModel.CarOut _outRecord, CaclMoneyResult _Money, Action<string> _SFDataHandler = null, Action<string> _PhotoDataHandler = null,
            Action<string, decimal> _CardTypeSFJEHandler = null, Action<string> _CPHHandler = null, Action<ParkingModel.CarOut> _FinishedHandler = null)
        {
            InitializeComponent();

            if (null == _outRecord)
            {
                throw new ArgumentNullException("_outRecord");
            }

            modulus = idxOfChannel;
            NeedConfirmCPH = _NeedConfirmCPH;
            outRecord = _outRecord;
            CPHHandler = _CPHHandler;
            SFDataHandler = _SFDataHandler;
            FinishedHandler = _FinishedHandler;
            PhotoDataHandler = _PhotoDataHandler;
            CardTypeSFJEHandler = _CardTypeSFJEHandler;
            Money = _Money;

            dgvFX.IsReadOnly = true;
            dgvFX.CanUserAddRows = false;
            dgvFX.CanUserDeleteRows = false;
            dgvFX.AutoGenerateColumns = false;
            dgvFX.VerticalGridLinesBrush = System.Windows.Media.Brushes.WhiteSmoke;
            dgvFX.HorizontalGridLinesBrush = System.Windows.Media.Brushes.WhiteSmoke;

            dgvCarIn.IsReadOnly = true;
            dgvCarIn.CanUserAddRows = false;
            dgvCarIn.CanUserDeleteRows = false;
            dgvCarIn.AutoGenerateColumns = false;
            dgvCarIn.VerticalGridLinesBrush = System.Windows.Media.Brushes.WhiteSmoke;
            dgvCarIn.HorizontalGridLinesBrush = System.Windows.Media.Brushes.WhiteSmoke;

            lblOnlinePayLayer = new Label();
            lblOnlinePayLayer.Visibility = System.Windows.Visibility.Collapsed;
            grdMain.Children.Add(lblOnlinePayLayer);

            //大小、位置与显示内容设置
            if (NeedConfirmCPH)
            {
                gridPhoto.Visibility = System.Windows.Visibility.Visible;
                gridCPH.Visibility = System.Windows.Visibility.Visible;

                gridCharge.Visibility = System.Windows.Visibility.Collapsed;

                this.Width = 810;
                this.Height = 720;
                this.Left = SystemParameters.WorkArea.Width - 810;
                this.Top = SystemParameters.WorkArea.Height - 720;
            }
            else
            {
                gridPhoto.Visibility = System.Windows.Visibility.Collapsed;
                gridCPH.Visibility = System.Windows.Visibility.Collapsed;

                gridCharge.Visibility = System.Windows.Visibility.Visible;

                this.Width = 580;
                this.Height = 420;
                this.Left = SystemParameters.WorkArea.Width - 580;
                this.Top = SystemParameters.WorkArea.Height - 420;
            }

            vSender = new ParkingCommunication.VoiceSend(1007, 1005);
        }

        #region 加载/初始化
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            CR.SetWindowLong(hwnd, CR.GWL_STYLE, CR.GetWindowLong(hwnd, CR.GWL_STYLE) & ~CR.WS_SYSMENU);

            rtbPrint.FontSize = 12;

            cmbCardType.SelectionChanged -= cmbCardType_SelectionChanged;

            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                InitDataSource();
                InitControlStatus();

                if (NeedConfirmCPH)
                {
                    txtCPH0.Text = outRecord.CPH;
                    dgvCarIn.ItemsSource = req.GetCarInByCarPlateNumberLike(txtCPH0.Text, 4);
                    dgvFX.ItemsSource = req.GetCardIssueByCarPlateNumberLike(txtCPH0.Text, 4);

                    SetImageSource(imgIn, Properties.Resources.Car2);
                    SetImageSource(imgOut, Model.sImageSavePath + outRecord.OutPic); 
                    //SetImageSource(imgOut, Properties.Resources.Car2);
                }
                else
                {
                    ShowDataToControl();
                    lblSFJE.Content = (lblSFJE.Content == "" ? "0.0" : lblSFJE.Content);

                    if ((CR.GetCardType(cmbCardType.Text, 0).Substring(0, 3) == "Tmp" || CR.GetCardType(cmbCardType.Text, 0).Substring(0, 3) == "Mtp") &&
                        Model.Channels[modulus].iBigSmall == 0 && Model.iBillPrint == 1 && Model.iBillPrintAuto > 0 && Convert.ToDecimal(lblSFJE.Content) > 0)
                    {
                        btnPrint_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmbCardType.SelectionChanged += cmbCardType_SelectionChanged;

            //没有启用收费窗口可更改卡类或者启用了且选择了播报语音则在打开收费窗口时报价
            if (Model.iSetTempCardType <= 0 || Model.iModiTempType_VoiceSF > 0)
            {
                ReportCharge();
            }

            //如果启用了自动关闭收费窗口功能则启动定时器
            if (Model.iAutoMinutes > 0)
            {
                tmrCloseForm = new DispatcherTimer();
                tmrCloseForm.Interval = new TimeSpan(0, 0, 1);
                tmrCloseForm.Tick += tmrCloseForm_Tick;
                tmrCloseForm.Start();
            }
        }

        void tmrCloseForm_Tick(object sender, EventArgs e)
        {
            int CountDownd;
            string FormTitle;

            this.Tag = this.Tag ?? this.Title;
            FormTitle = (this.Tag ?? this.Title) as string;
            int.TryParse((tmrCloseForm.Tag ?? Model.iAutoSetMinutes).ToString(), out CountDownd);

            CountDownd--;
            FormTitle += string.Format("({0})", CountDownd);
            if (CountDownd < 0)
            {
                tmrCloseForm.IsEnabled = false;
                CanClose = true;
                this.Close();
                return;
            }

            this.Title = FormTitle;
            tmrCloseForm.Tag = CountDownd;
        }

        private void InitDataSource()
        {
            List<FreeReason> lstFree;
            List<ParkDiscountJHSet> lstDeviceDiscount;

            lstFree = req.GetData<List<FreeReason>>("GetFreeReason", null, null, "ItemID");
            lstFree.Insert(0, new FreeReason());

            lstDeviceDiscount = req.GetData<List<ParkDiscountJHSet>>("GetParkDiscountJHSet", null, null, "Address");
            lstDeviceDiscount.Insert(0, new ParkDiscountJHSet());

            cmbFree.ItemsSource = lstFree;
            cmbFree.DisplayMemberPath = "ItemDetail";
            cmbFree.SelectedValuePath = "ItemDetail";

            cmbJHDZ.ItemsSource = lstDeviceDiscount;
            cmbJHDZ.DisplayMemberPath = "Address";
            cmbJHDZ.SelectedValuePath = "Address";
        }

        private void InitCardTypeSource()
        {
            List<CardTypeDef> lstCardType;
            List<QueryConditionGroup> lstCondition;

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("Enabled", "=", 1);
            lstCondition[0].Add("Identifying", "like",
                (null == outRecord || null == outRecord.CardType || outRecord.CardType.Trim().Length <= 0 ? "" :
                outRecord.CardType.Trim().Substring(0, outRecord.CardType.Trim().Length > 3 ? 3 : outRecord.CardType.Trim().Length) + "%"));

            lstCardType = req.GetData<List<CardTypeDef>>("GetCardTypeDef", null, lstCondition, "Identifying");

            cmbCardType.ItemsSource = lstCardType;
            cmbCardType.DisplayMemberPath = (Model.Language == "Chinese" ? "CardType" : "Remarks");
            cmbCardType.SelectedValuePath = "Identifying";
        }

        /// <summary>
        /// 初始化控件状态
        /// </summary>
        private void InitControlStatus()
        {
            btnPrint.Visibility = Model.iBillPrint == 1 ? Visibility.Visible : System.Windows.Visibility.Collapsed;

            txtSFJE.IsReadOnly = !(Model.iSetTempMoney == 1 ? true : false);
            //txtSFJE.DecimalPlaces

            button3.Visibility = Model.iSFCancel == 1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            btnPrint.Visibility = Model.iBillPrint > 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            btnFavorable.Visibility = Model.bMonthCardCharge ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

            btnFree.IsEnabled = Model.iTempFree > 0;
            cmbFree.IsEnabled = Model.iTempFree > 0;
            btnPapers.IsEnabled = Model.iIDCapture > 0;
            cmbCardType.IsEnabled = Model.iSetTempCardType > 0;

            if (Model.iOnlinePayEnabled == 1)
            {
                radWX.IsEnabled = true;
                radZFB.IsEnabled = true;
                txtAuth_code.IsEnabled = true;
            }
            else
            {
                radWX.IsEnabled = false;
                radZFB.IsEnabled = false;
                txtAuth_code.IsEnabled = false;
            }
        }

        private void ShowDataToControl()
        {
            CardTypeDef cardType;
            DateTime InTime, OutTime;
            TimeSpan tsStay;

            if (null != outRecord)
            {
                InitCardTypeSource();

                InTime = outRecord.InTime != null ? outRecord.InTime : (outRecord.OutTime != null ? outRecord.OutTime : DateTime.Now);
                OutTime = outRecord.OutTime != null ? outRecord.OutTime : InTime;
                tsStay = Convert.ToDateTime(OutTime.ToString("yyyy-MM-dd HH:mm")) - Convert.ToDateTime(InTime.ToString("yyyy-MM-dd HH:mm"));

                txtCPH0.Text = outRecord.CPH;
                lblCPH.Content = outRecord.CPH;
                lblInDateTime.Content = InTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblOutDateTime.Content = OutTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblTime.Content = (tsStay.Days > 0 ? tsStay.Days + "天" : "") + (tsStay.Hours > 0 ? tsStay.Hours + "小时" : "") + tsStay.Minutes + "分";
                lblSFJE.Content = outRecord.YSJE.ToString();
                txtSFJE.Value = outRecord.SFJE;
                cmbCardType.SelectedValue = outRecord.CardType;

                cardType = cmbCardType.SelectedItem as CardTypeDef;
                lblCardType.Content = (null != cardType ? cardType.CardType : "");

                if (NeedConfirmCPH)
                {
                    SetImageSource(imgIn, Model.sImageSavePath + outRecord.InPic);
                    SetImageSource(imgOut, Model.sImageSavePath + outRecord.OutPic);
                }
            }
        }
        #endregion

        #region 显示图片
        private void SetImageSource(System.Windows.Controls.Image image, string fileName)
        {
            if (null == fileName || !System.IO.File.Exists(fileName))
            {
                SetImageSource(image, Properties.Resources.Car2);
            }
            else
            {
                SetImageSource(image, System.Drawing.Image.FromFile(fileName));
            }
        }

        private void SetImageSource(System.Windows.Controls.Image image, System.Drawing.Image sourceImage)
        {
            int imageWidth = 0, imageHeight = 0;
            InitializeImageSize(sourceImage, image, out imageWidth, out imageHeight);

            Bitmap sourceBmp = new Bitmap(sourceImage, imageWidth, imageHeight);
            IntPtr hBitmap = sourceBmp.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            WriteableBitmap writeableBmp = new WriteableBitmap(bitmapSource);
            sourceImage.Dispose();
            sourceBmp.Dispose();
            image.Source = writeableBmp;
        }

        /// <summary>
        /// Initialize ImageSize.
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="image"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        private static void InitializeImageSize(System.Drawing.Image sourceImage, System.Windows.Controls.Image image,
            out int imageWidth, out int imageHeight)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            float aspect = (float)width / (float)height;
            if (image.Height != double.NaN)
            {
                imageHeight = Convert.ToInt32(image.Height);
                imageWidth = Convert.ToInt32(aspect * imageHeight);
            }
            else if (image.Width != double.NaN)
            {
                imageWidth = Convert.ToInt32(image.Width);
                imageHeight = Convert.ToInt32(image.Width / aspect);
            }
            else
            {
                imageHeight = 100;
                imageWidth = Convert.ToInt32(aspect * imageHeight);
            }
        }
        #endregion

        private void btnCancelCharge_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("请确认是否取消收费？\t\r若点击【是】，该记录将重写回入场！", "提示", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            //bll.RstInGateRetrography(lblCardNO.Text, Tmodel.CPH, Convert.ToDateTime(lblInDateTime.Text), modulus);
            if (req.MoveCarOutToCarIn(null, new { ID = outRecord.ID }) <= 0)
            {
                MessageBox.Show("取消收费失败");
                return;
            }

            //log.Add(this.Text, lblCardNO.Text + " " + Tmodel.CPH + " 取消收费：" + txtSFJE.Text + "元");
            req.AddLog(this.Title, outRecord.CardNO + " " + outRecord.CPH + " 取消收费：" + txtSFJE.Text + "元");

            CanClose = true;
            this.Close();
        }

        bool bOverTime = false;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DateTime InTime, OutTime;
            TimeSpan stime;
            CardIssue issue;
            List<CarIn> lstIn;
            List<CardIssue> lstIssue;
            List<QueryConditionGroup> lstCondition;

            if (txtCPH0.Text.Length < 7)
            {
                MessageBox.Show("请选择正确车牌或手动修改");
                return;
            }
            if (txtCPH0.Text == "")
            {
                return;
            }

            //2015-11-26
            lblCPH.Content = txtCPH0.Text;

            //Tmodel.CPH = txtCPH0.Text;
            outRecord.CPH = txtCPH0.Text;
            bool IsFoue = false;
            if (Model.Channels[modulus].iOpenType == 7)
            {
                //查询场内记录
                //DataTable dt = bll.SelectComeGet(txtCPH0.Text);
                //if (dt.Rows.Count > 0)

                lstCondition = new List<QueryConditionGroup>();
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[0].Add("CPH", "=", txtCPH0.Text);

                lstIn = req.GetData<List<CarIn>>("GetCarIn", null, lstCondition, "InTime desc");
                inRecord = (null != lstIn && lstIn.Count > 0) ? lstIn[0] : null;

                if (null != inRecord)
                {
                    //lblCardNO.Text = dt.Rows[0]["CardNO"].ToString();
                    outRecord.InID = inRecord.ID;
                    outRecord.CardNO = inRecord.CardNO;
                    outRecord.CPH = txtCPH0.Text;

                    lblInDateTime.Content = inRecord.InTime.ToString();
                    lblOutDateTime.Content = outRecord.OutTime.ToString();
                    cmbCardType.Text = CR.GetCardType(inRecord.CardType, 1);
                    outRecord.InTime = inRecord.InTime;
                    outRecord.CardType = inRecord.CardType ?? outRecord.CardType ?? "TmpA";
                    //lblCardType.Text = cmbCardType.Text;
                    InTime = outRecord.InTime != null ? outRecord.InTime : (outRecord.OutTime != null ? outRecord.OutTime : DateTime.Now);
                    OutTime = outRecord.OutTime != null ? outRecord.OutTime : InTime;
                    stime = Convert.ToDateTime(OutTime.ToString("yyyy-MM-dd HH:mm")) - Convert.ToDateTime(InTime.ToString("yyyy-MM-dd HH:mm"));
                    //lblTime.Content = stime.Days + "天" + stime.Hours + "小时" + stime.Minutes + "分";
                    outRecord.StayTime = (stime.Days > 0 ? stime.Days + "天" : "") + (stime.Hours > 0 ? stime.Hours + "小时" : "") + (stime.Minutes > 0 ? stime.Minutes + "分" : "");

                    if ((inRecord.CardType ?? "").StartsWith("Mth", StringComparison.OrdinalIgnoreCase))
                    {
                        //dt = bll.SelectFxCPH_Like(txtCPH0.Text);  //查询发行表    //2015-11-27
                        //if (dt.Rows.Count > 0)

                        lstCondition = new List<QueryConditionGroup>();
                        lstCondition.Add(new QueryConditionGroup());
                        lstCondition[0].Add("CPH", "like", string.Format("%{0}%", txtCPH0.Text));
                        lstCondition.Add(new QueryConditionGroup());
                        lstCondition[1].Add("CardState", "=", 0, "or");
                        lstCondition[1].Add("CardState", "=", 2, "or");

                        lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CPH");
                        issue = (null != lstIssue && lstIssue.Count > 0) ? lstIssue[0] : null;
                        if (null != issue)
                        {
                            //Tmodel.inDateTime = dt.Rows[0]["InTime"].ToString();
                            DateTime dtStart = Convert.ToDateTime(issue.CarValidStartDate.ToShortDateString() + " 00:00:00");
                            DateTime dtEnd = Convert.ToDateTime(issue.CarValidEndDate.AddDays(1).ToShortDateString() + " 00:00:00");
                            //if (CR.CR.DateDiff(CR.CR.DateInterval.Minute, dtStart, DateTime.Now) < 0 || CR.CR.DateDiff(CR.CR.DateInterval.Minute, DateTime.Now, dtEnd) < 0 && dt.Rows[0]["CarCardType"].ToString().Substring(0, 3) == "Mth")//ID卡月卡过期处理
                            if ((DateTime.Now - dtStart).TotalMinutes < 0 || (dtEnd - DateTime.Now).TotalMinutes < 0 && issue.CarCardType.StartsWith("Mth", StringComparison.OrdinalIgnoreCase))//ID卡月卡过期处理
                            {
                                cmbCardType.Text = CR.GetCardType("Mtp" + issue.CarCardType.Substring(3), 1);
                            }
                            else
                            {
                                //2016-12-16
                                if (issue.CarCardType.StartsWith("Mth", StringComparison.OrdinalIgnoreCase) && Model.sMonthOutChargeType.IndexOf(issue.CarCardType) >= 0)
                                {
                                    cmbCardType.Text = CR.GetCardType("Mtp" + Model.sMonthOutChargeType.Substring(Model.sMonthOutChargeType.IndexOf(issue.CarCardType) + 5, 4).Substring(3), 1);
                                }
                                else if (lstIn[0].SFOperatorCard == "123456")
                                {
                                    cmbCardType.Text = CR.GetCardType("Mtp" + issue.CarCardType.Substring(3), 1);
                                }
                                else
                                {
                                    cmbCardType.Text = CR.GetCardType(issue.CarCardType, 1);
                                    lblSFJE.Content = "0.0";
                                    txtSFJE.Value = 0;
                                }
                               
                            }
                        }
                    }

                    if ((inRecord.CardType ?? "").StartsWith("Tmp", StringComparison.OrdinalIgnoreCase) ||
                        (inRecord.CardType ?? "").StartsWith("Mtp", StringComparison.OrdinalIgnoreCase))   //2015-11-27
                    {
                        if (Model.iCentralCharge == 1)
                        {
                            //DataTable dtCharge = bll.GetCentralCharge(lblCardNO.Text);
                            //if (dt.Rows.Count > 0)

                            lstCondition = new List<QueryConditionGroup>();
                            lstCondition.Add(new QueryConditionGroup());
                            lstCondition[0].Add("CPH", "=", inRecord.CPH);

                            lstIn = req.GetData<List<CarIn>>("GetCarIn", null, lstCondition, "InTime desc");

                            if (null != lstIn && lstIn.Count > 0)
                            {
                                //string strSFGate = dt.Rows[0]["SFGate"].ToString();
                                //if (strSFGate == "中央收费")
                                //{
                                //    bOverTime = true;
                                //    //                                 if (CR.PubVal.bOverTimeSF)
                                //    //                                 {
                                //    this.Text = "临时车出场收费 -- 超时收费";
                                //    //}
                                //}

                                if (null != lstIn[0].SFGate && lstIn[0].SFGate.Contains("中央收费"))
                                {
                                    bOverTime = true;
                                    Title = "临时车出场收费 -- 超时收费";
                                }
                            }
                            else
                            {
                                bOverTime = false;
                            }
                        }

                        //2016-05-09
                        //2016-05-12 zsd add Model.iType ==80 && Model.iXieYi == 28  武汉畅盈通 总是要先报语音
                        if (Model.iSetTempCardType == 0 || (Model.iSetTempCardType == 1 && Model.iModiTempType_VoiceSF == 1)) //不能改卡类
                        {
                            Model.TbBaoJia = true;
                        }
                        else
                        {
                            Model.TbBaoJia = false;
                        }

                        //2016-06-17
                        if (Model.iAutoCPHDZ == 1)  //车牌打折，查询打折方式与地址
                        {
                            if (lblCPH.Content.ToString().Length >= 7)
                            {
                                //DataTable dtDZs = bll.GetAutoCPHDZ("CPH like '%" + lblCPH.Text.Substring(1, 6) + "%' and Enable=1").Tables[0];
                                //if (dtDZs.Rows.Count > 0)

                                lstCondition = new List<QueryConditionGroup>();
                                lstCondition.Add(new QueryConditionGroup());
                                lstCondition[0].Add("CPH", "like", "%" + lblCPH.Content.ToString().Substring(1, 6) + "%");
                                lstCondition[0].Add("Status", "=", 0);

                                List<ParkCPHDiscountSet> lstDiscount = req.GetData<List<ParkCPHDiscountSet>>("GetParkCPHDiscountSet", null, lstCondition, "OptTime desc");

                                if (null != lstDiscount && lstDiscount.Count > 0)
                                {
                                    //Tmodel.iYHMode = 1;  //车牌打折
                                    //Tmodel.sYHAdr = dtDZs.Rows[0]["Address"].ToString();
                                    //Tmodel.sYHType = dtDZs.Rows[0]["Manner"].ToString();
                                    //Tmodel.sYHValue = Convert.ToDecimal(dtDZs.Rows[0]["Favorable"].ToString());

                                    outRecord.YHAddress = "1";  //车牌打折
                                    outRecord.YHAddress = lstDiscount[0].Address;
                                    outRecord.YHType = lstDiscount[0].Manner;
                                    outRecord.YHJE = lstDiscount[0].Favorable ?? 0;
                                }
                            }
                        }

                        cmbCardType_SelectionChanged(null, null);
                        IsFoue = true;
                    }
                }
                else
                {
                    //dt = bll.SelectFxCPH_Like(txtCPH0.Text);  //查询发行表    //2015-11-27
                    //if (dt.Rows.Count > 0)

                    lstCondition = new List<QueryConditionGroup>();
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[0].Add("CPH", "like", string.Format("%{0}%", txtCPH0.Text));
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[1].Add("CardState", "=", 0, "or");
                    lstCondition[1].Add("CardState", "=", 2, "or");

                    lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CPH");
                    issue = (null != lstIssue && lstIssue.Count > 0) ? lstIssue[0] : null;

                    if (null != issue)
                    {
                        //lblCardNO.Text = dt.Rows[0]["CardNO"].ToString();
                        outRecord.CardNO = issue.CardNO;
                        outRecord.CPH = txtCPH0.Text;

                        lblInDateTime.Content = outRecord.InTime.ToString();  // dt.Rows[0]["InTime"].ToString();
                        lblOutDateTime.Content = outRecord.OutTime.ToString();
                        //Tmodel.inDateTime = dt.Rows[0]["InTime"].ToString();
                        DateTime dtStart = Convert.ToDateTime(issue.CarValidStartDate.ToShortDateString() + " 00:00:00");
                        DateTime dtEnd = Convert.ToDateTime(issue.CarValidEndDate.AddDays(1).ToShortDateString() + " 00:00:00");
                        if ((DateTime.Now - dtStart).TotalMinutes < 0 || (dtEnd - DateTime.Now).TotalMinutes < 0 && issue.CarCardType.StartsWith("Mth", StringComparison.OrdinalIgnoreCase))//ID卡月卡过期处理
                        {
                            cmbCardType.Text = CR.GetCardType("Mtp" + issue.CarCardType.Substring(3), 1);
                        }
                        else
                        {
                            cmbCardType.Text = CR.GetCardType(issue.CarCardType, 1);
                            lblSFJE.Content = "0.0";
                            txtSFJE.Value = 0;
                        }

                        outRecord.CardType = issue.CarCardType;
                        cmbCardType.IsEnabled = false;   //固定卡不能选卡类
                        //lblCardType.Text = cmbCardType.Text;
                        InTime = outRecord.InTime != null ? outRecord.InTime : (outRecord.OutTime != null ? outRecord.OutTime : DateTime.Now);
                        OutTime = outRecord.OutTime != null ? outRecord.OutTime : InTime;
                        stime = Convert.ToDateTime(OutTime.ToString("yyyy-MM-dd HH:mm")) - Convert.ToDateTime(InTime.ToString("yyyy-MM-dd HH:mm"));
                        lblTime.Content = stime.Days + "天" + stime.Hours + "小时" + stime.Minutes + "分";
                    }
                }

                JJcUpdateStore();
            }
            else if (Model.Channels[modulus].iOpenType == 8)
            {
                if (Model.iOutAutoOpenModel == 1 && CR.GetCardType(outRecord.CardType, 0).Substring(0, 3) == "Tmp")
                {
                }
                else
                {
                    if (Model.iIdSfCancel == 0)
                    {
                        JJcUpdateStore();
                    }
                }
            }
            if (CR.GetCardType(outRecord.CardType, 0).Substring(0, 3) == "Tmp" || CR.GetCardType(outRecord.CardType, 0).Substring(0, 3) == "Mtp")
            {
                if (IsFoue == false && (Model.iSetTempCardType == 0 || (Model.iSetTempCardType == 1 && Model.iModiTempType_VoiceSF == 1)))  //2016-05-07 zsd add Model.bSetTempCardType == false
                {
                    ReportCharge();
                }
            }
            else
            {
                //CPHDataHandler(txtCPH0.Text, Convert.ToInt32(modulus));
            }

            btnCutOff.Focus();

            gridCharge.Visibility = System.Windows.Visibility.Visible;
            gridCPH.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void JJcUpdateStore()
        {
            long ID;
            CarOut co;

            co = outRecord.Copy();
            co.ID = Model.Channels[modulus].iInOut;
            co.CPH = txtCPH0.Text;
            ID = req.SetCarOut(co);

            if (ID > 0)
            {
                outRecord = req.GetDataByID<CarOut>("GetCarOutByID", ID);
            }

            if (null != CPHHandler)
            {
                CPHHandler(co.CPH);
            }

            ShowDataToControl();
        }

        /// <summary>
        /// 剩余车位显示屏显示车牌号
        /// </summary>
        private void SurplusCPH()
        {
            //唐工说不实现，只在监控界面上显示
            //DataTable dtS;
            //CardTypeDef cardType;
            //Dictionary<string, object> dicCondition;

            //try
            //{
            //    //GCR.SedBll senbll = new GCR.SedBll(Model.Channels[modulus].sIP, 1007, 1005);
            //    //DataTable dtS = bll.GetSurplusCar(CR.CR.GetComputerName(), Model.Channels[modulus].iCtrlID);

            //    dicCondition = new Dictionary<string, object>();
            //    dicCondition.Add("CtrID", Model.Channels[modulus].iCtrlID);
            //    dicCondition.Add("StationID", Model.stationID);

            //    dtS = req.GetData<DataTable>("GetLedSetting", dicCondition, null, null);
            //    cardType = cmbCardType.SelectedItem as CardTypeDef;

            //    foreach (DataRow dr in dtS.Rows)
            //    {
            //        string showWay = dr["ShowWay"].ToString();
            //        string SendSum = "";
            //        string StrSum = "";

            //        //string strCardType = CR.CR.GetCardType(lblCardType.Text, 0).Substring(0, 3);
            //        string strCardType = (null == cardType ? outRecord.CardType : cardType.Identifying).Substring(0, 3);
            //        if (showWay.Contains("6"))//收费金额
            //        {
            //            if ((strCardType == "Tmp" || strCardType == "Str") && Model.Channels[modulus].iInOut == 1)
            //            {
            //                string moeny = "此次收费" + Convert.ToDecimal(txtSFJE.Text).ToString() + "元";
            //                StrSum += StrSum != "" ? " " + moeny : moeny;
            //            }
            //        }
            //        if (showWay.Contains("5") && (showWay.Contains("6") == false || (strCardType != "Tmp" && strCardType != "Str")))//是否发送车牌后缀
            //        {
            //            StrSum += StrSum != "" ? " " + dr["CPHEndStr"].ToString() : dr["CPHEndStr"].ToString();
            //        }
            //        if (StrSum != "")
            //        {
            //            string Jstrs = dr["Move"].ToString() + dr["Speed"].ToString() + dr["StopTime"].ToString() + dr["Color"].ToString() + dr["SumTime"].ToString() + CR.GetStrTo16(StrSum);//移动方式： 速度,单幅停留时间,颜色,总显示时间
            //            int sum = 0;
            //            byte[] array = CR.GetByteArray(Jstrs);
            //            foreach (byte by in array)
            //            {
            //                sum += by;
            //            }
            //            sum = sum % 256;
            //            //Thread.Sleep(300);
            //            SendSum = "CC" + Convert.ToInt32(dr["SurplusID"].ToString()).ToString("X2") + "BB5154" + sum.ToString("X2") + Jstrs + "FF";
            //            //string[] stryy = new string[2] { "003131313233B5DA2031C7F8BFD5B3B5CEBB203637200D", "003231313233B5DA2032C7F8BFD5B3B5CEBB203839200D" };
            //            //foreach (string sss in stryy)
            //            //{

            //            CR.SendVoice.SurplusCtrlLedShow(axznykt_1, Model.Channels[modulus].iCtrlID, Convert.ToInt16(dr["SurplusID"].ToString()),
            //                Model.Channels[modulus].sIP, SendSum, sum, Jstrs, m_nSerialHandle, Model.Channels[modulus].iXieYi);

            //            vSender.VoiceDisplay(ParkingCommunication.VoiceType.OutGateVoice,modulus,(null == cardType ? outRecord.CardType : cardType.Identifying),)
            //            // }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    req.AddLog(this.Title + ":SurplusCPH", ex.Message + "\r\n" + ex.StackTrace);
            //    MessageBox.Show(ex.Message + "\r\n" + ":SurplusCPH", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void ReportCharge()
        {
            string inTime;
            string outTime;
            string strTime;
            TimeSpan stime;
            DateTime tmNow;
            CardTypeDef cardType;

            try
            {
                if (Model.iCtrlShowPlate == 1 || Model.iCtrlShowStayTime == 1)
                {
                    //string strPlateTalk = outRecord.CPH;
                    //if (strPlateTalk.Length != 7 || strPlateTalk == "0000000" || strPlateTalk == "6666666" || strPlateTalk == "8888888" || strPlateTalk == "京000000")
                    //{
                    //    if (strPlateTalk.Length == 8 && strPlateTalk.Substring(0, 2) == "WJ")
                    //    {
                    //    }
                    //    else
                    //    {
                    //        strPlateTalk = "";
                    //    }
                    //}

                    //if (Model.bCtrlShowPlate == false)
                    //{
                    //    strPlateTalk = "FFFF";
                    //}

                    //string sLoad = CR.CR.LedSound_New(Model.byteLSXY[Model.iLSIndex, 1], MyTempMoney, "FFFF", strTime, strPlateTalk);
                    //CR.SendVoice.LoadLsNoX2010znykt(axznykt_1, Model.Channels[modulus].iCtrlID, Model.Channels[modulus].sIP, 0x3D, Model.byteLSXY[Model.iLSIndex, 1],
                    //  sLoad, m_nSerialHandle, Model.Channels[modulus].iXieYi);

                    cardType = cmbCardType.SelectedItem as CardTypeDef;
                    tmNow = DateTime.Now;

                    outTime = (outRecord.OutTime != null ? outRecord.OutTime : (outRecord.InTime != null ? outRecord.InTime : tmNow)).ToString("yyyy-MM-dd HH:mm");
                    inTime = (outRecord.InTime != null ? outRecord.InTime : tmNow).ToString("yyyy-MM-dd HH:mm");

                    //outTime = (outRecord.OutTime ?? outRecord.InTime ?? tmNow).ToString("yyyy-MM-dd HH:mm");
                    //inTime = (outRecord.InTime ?? outRecord.OutTime ?? tmNow).ToString("yyyy-MM-dd HH:mm");
                    stime = Convert.ToDateTime(outTime) - Convert.ToDateTime(inTime);
                    strTime = stime.Days.ToString("X4") + stime.Hours.ToString("X2") + stime.Minutes.ToString("00");
                    if (Model.iCtrlShowStayTime == 0)
                    {
                        strTime = "FFFF";
                    }

                    cardType = cmbCardType.SelectedItem as CardTypeDef;
                    vSender.VoiceDisplay(ParkingCommunication.VoiceType.OutGateVoice, modulus, (null != cardType ? cardType.Identifying : outRecord.CardType),
                    outRecord.CPH, 0, "", 0, Convert.ToDecimal(lblSFJE.Content), txtSFJE.Value, strTime);
                }
                else
                {
                    string MyTempMoney = "";

                    if (Model.iXsd == 0)
                    {
                        if (Model.iChargeType == 3)
                        {
                            if (Model.iXsdNum == 1)
                            {
                                MyTempMoney = (Convert.ToInt32(txtSFJE.Value * 10)).ToString("X4");
                            }
                            else
                            {
                                if (txtSFJE.Value > 655)
                                {
                                    string strQIAN = CR.MoneyToChinese(txtSFJE.Text.ToString());
                                    string strsLoad = "73" + CR.GetChineseMoney(strQIAN) + "74";

                                    vSender.LoadLsNoX2010znykt(modulus, strsLoad);

                                    SurplusCPH();
                                    return;
                                }
                                MyTempMoney = (Convert.ToInt32(txtSFJE.Value * 100)).ToString("X4");
                            }
                        }
                        else
                        {
                            MyTempMoney = Convert.ToInt32(txtSFJE.Value).ToString("X4");
                        }
                    }
                    else
                    {
                        MyTempMoney = (Convert.ToInt32(txtSFJE.Value * 10)).ToString("X4");
                    }

                    if (Model.bSfDec)
                    {
                        //string strRst = "";
                        //// string strRst = sendbll.ShowLed(Model.Channels[modulus].iCtrlID, MyTempMoney, Model.Channels[modulus].iXieYi);//开闸
                        //CR.SendVoice.LedShow2010znykt(axznykt_1, Model.Channels[modulus].iCtrlID, Model.Channels[modulus].sIP, MyTempMoney, m_nSerialHandle, Model.Channels[modulus].iXieYi);

                        vSender.LedShow2010znykt(modulus, MyTempMoney);
                    }
                    else
                    {
                        //CR.SendVoice.LedShow2010znykt(axznykt_1, Model.Channels[modulus].iCtrlID, Model.Channels[modulus].sIP, MyTempMoney, m_nSerialHandle, Model.Channels[modulus].iXieYi);

                        vSender.LedShow2010znykt(modulus, MyTempMoney);
                    }
                }

                SurplusCPH();
            }
            catch (Exception ex)
            {
                req.AddLog(this.Title + ":ReportCharge", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + ":Baojia", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 加载时由于要绑定卡类的数据源，会触发计算收费金额的动作，因此增加此变量，避免加载时触发计算收费的动作。
        /// </summary>
        private void CaclSFJE(bool ReportChargeInSound = true)
        {
            decimal SFJE;
            DateTime InTime, OutTime;
            FreeReason free = null;
            CardTypeDef CardType;
            ParkDiscountJHSet discountSet = null;

            SFJE = 0;

            try
            {
                //免费。不需要更改金额，点免费开闸按钮时处理
                //if (cmbFree.SelectedIndex > 0)
                //{
                //    free = cmbFree.SelectedItem as FreeReason;
                //    if (null != free)
                //    {
                //        SFJE = 0;
                //    }
                //}

                if (null == free)
                {
                    CardType = cmbCardType.SelectedItem as CardTypeDef;
                    InTime = outRecord.InTime != null ? outRecord.InTime : (outRecord.OutTime != null ? outRecord.OutTime : DateTime.Now);
                    OutTime = outRecord.OutTime != null ? outRecord.OutTime : InTime;
                    discountSet = cmbJHDZ.SelectedItem as ParkDiscountJHSet;

                    Money = req.CaclMoney((bOverTime && Model.iCentralCharge == 1) ? "TmpJ" : (null == CardType ? "" : CardType.Identifying), InTime, OutTime,
                        outRecord.CPH, null != discountSet ? (long?)discountSet.ID : null);

                    SFJE = (null == Money ? 0 : Money.SFJE);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                SFJE = SFJE < 0 ? 0 : SFJE;

                //计算多次进出最高限额
                if (Model.iZGXE > 0 && SFJE > 0)
                {
                    CardType = cmbCardType.SelectedItem as CardTypeDef;
                    InTime = outRecord.InTime != null ? outRecord.InTime : (outRecord.OutTime != null ? outRecord.OutTime : DateTime.Now);
                    OutTime = outRecord.OutTime != null ? outRecord.OutTime : InTime;
                    SFJE = gsd.GetDayMoneyLimit(outRecord.CardNO, (null == CardType ? "" : CardType.Identifying), (lblCPH.Content ?? "").ToString(),
                        InTime, OutTime, Model.iXsd, SFJE, Model.iZGXEType, modulus);
                }

                txtSFJE.Value = SFJE;
                lblSFJE.Content = null == Money ? SFJE : Money.YSJE;

                //Model.bPayTest = true;
                //if (Model.bPayTest)  //测试模式收费金额都为1分
                //{
                //    txtSFJE.Value = 0.01M;
                //}
            }

            if (ReportChargeInSound)
            {
                ReportCharge();
            }
        }

        private void txtCPH0_KeyDown(object sender, KeyEventArgs e)
        {
            //if (KeyInterop.VirtualKeyFromKey(e.Key) == '\'' ||
            //    KeyInterop.VirtualKeyFromKey(e.Key) == '"'
            //    // || (byte)e.Key == '‘' ||
            //    // (byte)e.Key == '’' ||
            //    //(byte)e.Key == '“' || (byte)e.Key == '”'
            //    )
            //{
            //    e.Handled = true;
            //}
            //if (e.Key == Key.Enter)
            //{
            //    button1_Click(null, null);
            //}
        }

        private void txtCPH0_KeyUp(object sender, KeyEventArgs e)
        {
            int idx;
            List<CarIn> lstIn;
            List<CardIssue> lstIssue;
            List<QueryConditionGroup> lstCondition;

            //由于KeyEventArgs中的Key与键盘上的键无法对应上，因此用以下代码过滤单引号和双引号
            idx = txtCPH0.SelectionStart;
            for (int i = txtCPH0.Text.Length - 1; i >= 0; i--)
            {
                if (txtCPH0.Text[i] == '\'' || txtCPH0.Text[i] == '\"')
                {
                    idx--;
                    txtCPH0.Text = txtCPH0.Text.Remove(i, 1);
                }
            }
            txtCPH0.Select(idx < 0 ? 0 : idx, 0);

            if (txtCPH0.Text.Trim().Length >= 3)
            {
                //模糊查询发行表
                //DataTable dt1 = bll.SelectFxCPH_Like(txtCPHList[index].Text.Trim());   //cboJC0.Text +
                //dgvFX.DataSource = dt1;

                lstCondition = new List<QueryConditionGroup>();
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[0].Add("CPH", "like", string.Format("%{0}%", txtCPH0.Text));
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[1].Add("CardState", "=", 0, "or");
                lstCondition[1].Add("CardState", "=", 2, "or");

                lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CPH");
                dgvFX.ItemsSource = lstIssue;

                //模糊查询场内表
                //DataTable dt2 = bll.SelectComeCPH_Like(txtCPHList[index].Text.Trim());
                //dgvCarIn.DataSource = dt2;

                lstCondition = new List<QueryConditionGroup>();
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[0].Add("CPH", "like", string.Format("%{0}%", txtCPH0.Text));
                lstCondition[0].Add("bigsmall", "=", 0, "or");

                lstIn = req.GetData<List<CarIn>>("GetCarIn", null, lstCondition, "InTime desc");
                dgvCarIn.ItemsSource = lstIn;
            }
        }

        private void dgvFX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardIssue ci;

            ci = dgvFX.SelectedItem as CardIssue;
            if (null == ci)
            {
                return;
            }

            txtCPH0.Text = ci.CPH;
        }

        private void dgvCarIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarIn inRecord;

            inRecord = dgvCarIn.SelectedItem as CarIn;
            if (null == inRecord)
            {
                return;
            }

            txtCPH0.Text = inRecord.CPH;

            //显示入场图片
            if (null != inRecord.InPic)
            {
                try
                {
                    SetImageSource(imgIn, Model.sImageSavePath + inRecord.InPic);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("收费窗口：dgvCarIn_SelectionChanged：显示入场图片 异常：" + ex.Message);
                }
            }
        }

        private void cmbJHDZ_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParkDiscountJHSet discountSet = null;

            if (cmbJHDZ.SelectedIndex > 0)
            {
                discountSet = cmbJHDZ.SelectedItem as ParkDiscountJHSet;
            }
            if (null != discountSet)
            {
                outRecord.YHAddress = discountSet.Address;
                outRecord.YHType = discountSet.Manner;
                outRecord.YHJE = discountSet.Favorable;
            }
            else
            {
                outRecord.YHAddress = null;
                outRecord.YHType = null;
                outRecord.YHJE = 0;
            }

            CaclSFJE();
        }

        private void cmbCardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardTypeDef CardType;

            CardType = cmbCardType.SelectedItem as CardTypeDef;

            if (null == CardType || CardType.CardType == "无效卡" || !Model.bTemp8)
            {
                if (null != CardTypeSFJEHandler)
                {
                    CardTypeSFJEHandler((null == CardType ? "" : CardType.CardType), txtSFJE.Value);
                }
                return;
            }

            CaclSFJE();

            if (null != CardTypeSFJEHandler)
            {
                CardTypeSFJEHandler(CardType.CardType, txtSFJE.Value);
            }
        }

        private void btnVoice_Click(object sender, RoutedEventArgs e)
        {
            ReportCharge();
        }

        private void btnCutOff_Click(object sender, RoutedEventArgs e)
        {
            string strRst = "0";
            CardTypeDef ctDef;
            List<QueryConditionGroup> lstCondition;
            Dictionary<string, object> dicTmp;
            Dictionary<string, object> dicFieldValue;

            try
            {
                GetServiceData gsd = new GetServiceData();

                //2016-09-29
                if (outRecord.OutPic != "")
                {
                    string path = "";
                    if (System.IO.File.Exists(Model.sImageSavePath + outRecord.OutPic))
                    {
                        path = gsd.UpLoadPic(Model.sImageSavePath + outRecord.OutPic);
                    }
                    gsd.UpdateCarOut(outRecord.CardNO, path);
                }




                //  bSaveRecord = false;
                //2016-07-05 扫描枪屏蔽
                //if (Model.bSMPayment && !bEnter)
                //{
                //    bEnter = true;
                //    txtAuth_code.Focus();
                //    return;
                //}
                if ((DateTime.Now - tmLastKeyUp).TotalMilliseconds < 100)    //扫描枪的输入速度才会这么快
                {
                    return;
                }

                //bLoad = false;
                if (Model.iDetailLog == 1)
                {
                    req.AddLog(this.Title, "临时卡开闸发行！收费：" + txtSFJE.Text + "元");
                    //写日志 暂时未处理
                }
                //OpenKH = "10";

                //ComeFree(2, CR.CR.GetCardType(lblCardType.Text, 0));  //需要修改radio
                ctDef = cmbCardType.SelectedItem as CardTypeDef;

                dicFieldValue = new Dictionary<string, object>();

                dicFieldValue.Add("OutGateName", Model.Channels[modulus].sInOutName);
                dicFieldValue.Add("OutOperator", Model.sUserName);
                dicFieldValue.Add("OutOperatorCard", Model.sUserCard);
                dicFieldValue.Add("Balance", 0);
                dicFieldValue.Add("SFTime", Request.GetServerTime());
                dicFieldValue.Add("OvertimeSFTime", Request.GetServerTime());
                dicFieldValue.Add("BigSmall", Model.Channels[modulus].iBigSmall);

                dicFieldValue.Add("PayType", iPayType);
                dicFieldValue.Add("SFJE", txtSFJE.Value);
                dicFieldValue.Add("CardType", null != ctDef ? ctDef.Identifying : outRecord.CardType);
                dicFieldValue.Add("FreeReason", null == Money || null == Money.DiscountSet ? null : string.Format("{0} 优惠 {1}{2}",
                    (Money.DiscountSet.DeptId > 0 ? Money.DiscountSet.DeptName : Money.DiscountSet.Address), Money.DiscountSet.Favorable, Money.DiscountSet.Manner));

                lstCondition = new List<QueryConditionGroup>();
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[0].Add("ID", "=", outRecord.ID);

                if (req.UpdateField("UpdateCarOutFields", dicFieldValue, lstCondition) <= 0)
                {
                    MessageBox.Show("提交数据到服务器失败");
                    return;
                }

                //如果是车牌优惠则更新优惠记录的状态
                if (null != Money && null != Money.DiscountSet && Money.DiscountSet.DeptId >= 0)
                {
                    //生效的优惠状态改为1(已使用)
                    dicTmp = new Dictionary<string, object>();
                    dicTmp.Add("Status", 1);
                    lstCondition = new List<QueryConditionGroup>();
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[0].Add("Status", "=", 0);
                    lstCondition[0].Add("ID", "=", Money.DiscountSet.ID);
                    req.UpdateField("UpdateParkCPHDiscountSetFields", dicTmp, lstCondition);

                    //同车牌的其它车牌优惠记录的状态改为2(已失效)
                    dicTmp = new Dictionary<string, object>();
                    dicTmp.Add("Status", 2);
                    lstCondition = new List<QueryConditionGroup>();
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[0].Add("Status", "=", 0);
                    lstCondition[0].Add("CPH", "=", Money.DiscountSet.CPH);
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[1].Add("ValidEndTime", "is null", 0, "or");
                    lstCondition[1].Add("ValidEndTime", ">=", outRecord.OutTime);
                    req.UpdateField("UpdateParkCPHDiscountSetFields", dicTmp, lstCondition);
                }

                for (int i = 0; i <= 4; i++)  //2015-10-13
                {
                    strRst = vSender.SendOpen(modulus);
                    if (strRst == "0")   //开闸成功
                    {
                        if (Model.bOut485)//不加延时摄像机播报不了这个语音 控制板正常
                        {
                            System.Threading.Thread.Sleep(30);
                        }
                        vSender.VoiceDisplay(ParkingCommunication.VoiceType.TempOutOpen, modulus);
                        break;
                    }
                    ParkingInterface.CR.DelaySec(300);
                }

                if (strRst != "2")
                {
                    if (null != SFDataHandler)   //2015-10-28
                    {
                        SFDataHandler(txtSFJE.Value.ToString("0.00"));
                    }

                    CanClose = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                req.AddLog(this.Title + ":btnCutOff_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + ":btnCutOff_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        DateTime tmLastKeyUp;
        private void btnCutOff_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                return;
            }
            tmLastKeyUp = DateTime.Now;
        }

        /// <summary>
        /// 支付方式，0现金，1微信，2支付宝
        /// </summary>
        int iPayType = 0;
        private void txtAuth_code_KeyDown(object sender, KeyEventArgs e)
        {
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

            if (txtSFJE.Value <= 0)
            {
                MessageBox.Show("请输入大于0的收费金额！");
                txtAuth_code.Text = "";
                return;
            }
            int iFlag = 0;
            string strContent = lblCPH.Content + "--停车时间：" + lblTime.Content + "--交费：" + txtSFJE.Text + "元";

            iFlag = (radWX.IsChecked ?? false) ? 0 : 1;
            ShowOnlinePayLayer((radWX.IsChecked ?? false) ? "微信支付中，请稍候..." : "支付宝支付中，请稍候...");
            //bool brst = CR.SMPay(iFlag, txtSFJE.Text.Trim(), txtAuth_code.Text, strContent, lblCPH.Content);
            result = req.OnlinePay(txtAuth_code.Text, iFlag, txtSFJE.Value, strContent, lblCPH.Content.ToString());
            lblOnlinePayLayer.Visibility = System.Windows.Visibility.Collapsed;

            if (null != result && result.rcode == "200")
            {
                iPayType = (radWX.IsChecked ?? false) ? 1 : 2;

                btnCutOff_Click(null, null);
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

        private void ShowOnlinePayLayer(string alertstr)
        {
            Thickness margin;

            margin = new Thickness();
            margin.Bottom = 0;
            margin.Left = 0;
            margin.Right = 0;
            margin.Top = 0;

            lblOnlinePayLayer.Content = alertstr;
            lblOnlinePayLayer.Margin = margin;
            lblOnlinePayLayer.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            lblOnlinePayLayer.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            lblOnlinePayLayer.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            lblOnlinePayLayer.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            lblOnlinePayLayer.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            lblOnlinePayLayer.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(240, 0xB0, 0xC4, 0xDE));
            lblOnlinePayLayer.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnPapers_Click(object sender, RoutedEventArgs e)
        {
            string strRst = "0";
            CardTypeDef ctDef;
            ParkDiscountJHSet discountSet = null;
            List<QueryConditionGroup> lstCondition;
            Dictionary<string, object> dicFieldValue;

            try
            {
                if (MessageBox.Show("请确认是否免费放行？\t\r若点击【是】，该车辆将被免费放行！", "提示", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }

                cmbFree.Text = "";
                if (null != PhotoDataHandler)
                {
                    PhotoDataHandler(Model.sIDCaptureCard);
                }

                //显示抓拍窗口
                ImageCapture frm = new ImageCapture();
                frm.ShowInTaskbar = false;
                frm.Owner = this;
                if (!(frm.ShowDialog() ?? false))
                {
                    return;
                }

                //ComeFree(1, "Tfr");
                ctDef = cmbCardType.SelectedItem as CardTypeDef;
                if (cmbJHDZ.SelectedIndex > 0)
                {
                    discountSet = cmbJHDZ.SelectedItem as ParkDiscountJHSet;
                }

                dicFieldValue = new Dictionary<string, object>();

                dicFieldValue.Add("OutGateName", Model.Channels[modulus].sInOutName);
                dicFieldValue.Add("OutOperator", Model.sUserName);
                dicFieldValue.Add("OutOperatorCard", Model.sUserCard);
                dicFieldValue.Add("Balance", 0);
                dicFieldValue.Add("SFTime", Request.GetServerTime());
                dicFieldValue.Add("OvertimeSFTime", Request.GetServerTime());
                dicFieldValue.Add("BigSmall", Model.Channels[modulus].iBigSmall);

                dicFieldValue.Add("SFJE", 0);
                dicFieldValue.Add("ZJPic", "");
                dicFieldValue.Add("CardType", "Tfr");
                dicFieldValue.Add("FreeReason", null);

                lstCondition = new List<QueryConditionGroup>();
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[0].Add("ID", "=", outRecord.ID);

                if (req.UpdateField("UpdateCarOutFields", dicFieldValue, lstCondition) <= 0)
                {
                    MessageBox.Show("提交数据到服务器失败");
                    return;
                }

                for (int i = 0; i <= 4; i++)  //2015-10-13
                {
                    strRst = vSender.SendOpen(modulus);
                    if (strRst == "0")   //开闸成功
                    {
                        if (Model.bOut485)//不加延时摄像机播报不了这个语音 控制板正常
                        {
                            System.Threading.Thread.Sleep(30);
                        }
                        vSender.VoiceDisplay(ParkingCommunication.VoiceType.TempOutOpen, modulus);
                        break;
                    }
                    ParkingInterface.CR.DelaySec(300);
                }

                CanClose = true;
                this.Close();
            }
            catch (Exception ex)
            {
                req.AddLog(this.Title + ":btnPapers_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + ":btnPapers_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFree_Click(object sender, RoutedEventArgs e)
        {
            string strRst = "0";
            CardTypeDef ctDef;
            FreeReason freeReason = null;
            List<QueryConditionGroup> lstCondition;
            Dictionary<string, object> dicFieldValue;

            try
            {
                if (MessageBox.Show("请确认是否免费放行？\t\r若点击【是】，该车辆将被免费放行！", "提示", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }

                //ComeFree(1, "Tfr");
                ctDef = cmbCardType.SelectedItem as CardTypeDef;
                if (cmbFree.SelectedIndex > 0)
                {
                    freeReason = cmbFree.SelectedItem as FreeReason;
                }

                dicFieldValue = new Dictionary<string, object>();

                dicFieldValue.Add("OutGateName", Model.Channels[modulus].sInOutName);
                dicFieldValue.Add("OutOperator", Model.sUserName);
                dicFieldValue.Add("OutOperatorCard", Model.sUserCard);
                dicFieldValue.Add("Balance", 0);
                dicFieldValue.Add("SFTime", Request.GetServerTime());
                dicFieldValue.Add("OvertimeSFTime", Request.GetServerTime());
                dicFieldValue.Add("BigSmall", Model.Channels[modulus].iBigSmall);

                dicFieldValue.Add("SFJE", 0);
                dicFieldValue.Add("ZJPic", "");
                dicFieldValue.Add("CardType", "Tfr");
                dicFieldValue.Add("FreeReason", null != freeReason ? string.Format("{0} 免费", freeReason.ItemDetail) : null);

                lstCondition = new List<QueryConditionGroup>();
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[0].Add("ID", "=", outRecord.ID);

                if (req.UpdateField("UpdateCarOutFields", dicFieldValue, lstCondition) <= 0)
                {
                    MessageBox.Show("提交数据到服务器失败");
                    return;
                }

                for (int i = 0; i <= 4; i++)  //2015-10-13
                {
                    strRst = vSender.SendOpen(modulus);
                    if (strRst == "0")   //开闸成功
                    {
                        if (Model.bOut485)//不加延时摄像机播报不了这个语音 控制板正常
                        {
                            System.Threading.Thread.Sleep(30);
                        }
                        vSender.VoiceDisplay(ParkingCommunication.VoiceType.TempOutOpen, modulus);
                        break;
                    }
                    ParkingInterface.CR.DelaySec(300);
                }

                CanClose = true;
                this.Close();
            }
            catch (Exception ex)
            {
                req.AddLog(this.Title + ":btnFree_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + ":btnFree_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGiveUp_Click(object sender, RoutedEventArgs e)
        {
            string strRst = "0";
            CarOut co;
            CardTypeDef ctDef;
            ParkDiscountJHSet discountSet = null;
            List<QueryConditionGroup> lstCondition;
            Dictionary<string, object> dicFieldValue;

            try
            {
                if (Model.iIdSfCancel == 0)
                {
                    if (MessageBox.Show("请确认是否放弃开闸？\t\r若点击【是】，道闸将不会打开，但收费记录仍将保存，放弃开闸不能作为未收费的依据！！", "提示", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                //ComeFree(2, CR.CR.GetCardType(lblCardType.Text, 0));   //2015-11-30 放弃开闸跟开闸放行处理方式一样，只是不开闸
                //bll.GiveUpFreeReason(CR.CR.GetCardType(cmbCardType.Text, 0), "放弃开闸", lblCardNO.Text, Model.iParkingNo, Convert.ToDateTime(lblInDateTime.Text), ParkingMonitoring.File);

                ctDef = cmbCardType.SelectedItem as CardTypeDef;
                if (cmbJHDZ.SelectedIndex > 0)
                {
                    discountSet = cmbJHDZ.SelectedItem as ParkDiscountJHSet;
                }

                dicFieldValue = new Dictionary<string, object>();

                dicFieldValue.Add("OutGateName", Model.Channels[modulus].sInOutName);
                dicFieldValue.Add("OutOperator", Model.sUserName);
                dicFieldValue.Add("OutOperatorCard", Model.sUserCard);
                dicFieldValue.Add("Balance", 0);
                dicFieldValue.Add("SFTime", Request.GetServerTime());
                dicFieldValue.Add("OvertimeSFTime", Request.GetServerTime());
                dicFieldValue.Add("BigSmall", Model.Channels[modulus].iBigSmall);

                dicFieldValue.Add("SFJE", txtSFJE.Value);
                dicFieldValue.Add("CardType", null != ctDef ? ctDef.Identifying : outRecord.CardType);
                dicFieldValue.Add("FreeReason", null != discountSet ? string.Format("{0} 优惠 {1}{2}", discountSet.Address, discountSet.Favorable, discountSet.Manner) : null);

                lstCondition = new List<QueryConditionGroup>();
                lstCondition.Add(new QueryConditionGroup());
                lstCondition[0].Add("ID", "=", outRecord.ID);

                if (req.UpdateField("UpdateCarOutFields", dicFieldValue, lstCondition) <= 0)
                {
                    MessageBox.Show("提交数据到服务器失败");
                    return;
                }

                CanClose = true;
                this.Close();
            }
            catch (Exception ex)
            {
                req.AddLog(this.Title + ":btnGiveUp_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + ":btnGiveUp_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !CanClose;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (null != FinishedHandler)
            {
                FinishedHandler(outRecord);
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

        GetServiceData gsd = new GetServiceData();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<BillPrintSet> lstBPS = gsd.GetPrint();
                FlowDocument doc = new FlowDocument();
                Paragraph ph = new Paragraph();
                ph.Inlines.Add(new Run(("        临时车收费票据")));
                if (lstBPS.Count > 0 && lstBPS != null)
                {
                    if (lstBPS[0].Title != "")
                    {
                        ph.Inlines.Add(new Run("\r\n" + "        " + lstBPS[0].Title));
                    }
                    if (lstBPS[0].FTitle != "")
                    {
                        ph.Inlines.Add(new Run("\r\n" + "        " + lstBPS[0].FTitle));
                    }
                }
                ph.Inlines.Add(new Run("\r\n-----------------------------------"));
                doc.Blocks.Add(ph);
                doc.Blocks.Add(new Paragraph(new Run("车牌号码:" + lblCPH.Content ?? "")));
                //doc.Blocks.Add(new Paragraph(new Run("卡片号码:" + lblCardNo.Content ?? ""))); 
                doc.Blocks.Add(new Paragraph(new Run("车辆编号:" + outRecord.CardNO ?? "")));

                doc.Blocks.Add(new Paragraph(new Run("车辆类型:" + lblCardType.Content ?? "")));              //卡片种类

                doc.Blocks.Add(new Paragraph(new Run("入场时间:" + lblInDateTime.Content ?? "")));
                doc.Blocks.Add(new Paragraph(new Run("出场时间:" + lblOutDateTime.Content ?? "")));

                if (null != Money && null != Money.DiscountSet)
                {
                    //doc.Blocks.Add(new Paragraph(new Run("")));
                    doc.Blocks.Add(new Paragraph(new Run(string.Format("应收金额：{0}", lblSFJE.Content))));
                }

                //doc.Blocks.Add(new Paragraph(new Run("\n")));
                doc.Blocks.Add(new Paragraph(new Run(string.Format("收费金额：{0}", txtSFJE.Value.ToString("0.0")))));
                doc.Blocks.Add(new Paragraph(new Run("收费时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));       //收费时间
                doc.Blocks.Add(new Paragraph(new Run("操作员姓名:" + Model.sUserName)));         //操作员

                //doc.Blocks.Add(new Paragraph(new Run("日期:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));

                if (null != Money && null != Money.DiscountSet)
                {
                    //doc.Blocks.Add(new Paragraph(new Run("\n")));
                    doc.Blocks.Add(new Paragraph(new Run(string.Format("打折方式：{0}{1}", Money.DiscountSet.Favorable, Money.DiscountSet.Manner))));
                    //doc.Blocks.Add(new Paragraph(new Run("\n")));
                    doc.Blocks.Add(new Paragraph(new Run(string.Format("打折地点：{0}", Money.DiscountSet.DeptId > 0 ? Money.DiscountSet.DeptName : Money.DiscountSet.Address))));
                }

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
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnPrint_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + ":btnPrint_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}