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
using System.Data;

namespace UI.Report
{
    /// <summary>
    /// ReportCarCharge.xaml 的交互逻辑
    /// </summary>
    public partial class ReportCarCharge : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        bool bSearch = false;
        string strInPic, strOutPic = "";
        DateTime dtStartHF = new DateTime();
        DateTime dtEndHF = new DateTime();
        string strGateInName, strGateOutName = "";
        DataSet ds;
        List<CarOut> lstCO = new List<CarOut>();
        ReportCar rc;
        List<string> lstValue = new List<string>();
        public ReportCarCharge()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Text", Type.GetType("System.String"));
                dt.Columns.Add("Value", Type.GetType("System.Int32"));
                dt.Rows.Add(new object[] { "现金", 0 });
                dt.Rows.Add(new object[] { "支付宝", 1 });
                dt.Rows.Add(new object[] { "微信", 2 });
                cboPayType.ItemsSource = dt.DefaultView;

                picIn.Image = Properties.Resources.Car2;
                picOut.Image = Properties.Resources.Car2;

                dtStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");


                DataTable dtOP = new DataTable();
                dtOP.Columns.Add("Key", Type.GetType("System.String"));
                dtOP.Columns.Add("Value", Type.GetType("System.String"));
                dtOP.Rows.Add(new object[] { "=", "等于" });
                dtOP.Rows.Add(new object[] { "like", "包含" });
                dtOP.Rows.Add(new object[] { "<>", "不等于" });
                dtOP.Rows.Add(new object[] { ">=", "不小于" });
                dtOP.Rows.Add(new object[] { "<=", "不大于" });
                //BinModel.lstOP.Clear();
                //BinModel.lstOP.Add("等于");
                //BinModel.lstOP.Add("包含");
                //BinModel.lstOP.Add("不等于");
                //BinModel.lstOP.Add("不小于");
                //BinModel.lstOP.Add("不大于");
                
                dgvQuery.ItemsSource = gsd.GetSaveQuery("GetCarOut", "0");
                dgOP.ItemsSource = dtOP.DefaultView;

                cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarOut");
                cmbQuery.DisplayMemberPath = "SchName";
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
            }
            //ShowRights();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<SelectModel> lstSM = new List<SelectModel>();
                SelectModel sm = new SelectModel();
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "OutTime", Operator = ">=", FieldValue = dtStartTime.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"), Combinator = "and" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "OutTime", Operator = "<=", FieldValue = dtEndTime.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"), Combinator = "and" });
                for (int i = 0; i < dgvQuery.Items.Count; i++)
                {
                    var vr = dgvQuery.Items[i] as Query;
                    if (vr.Selected == 1)
                    {
                        string tmpValue = "";
                        string fieldName = vr.RealField.ToString();
                        if (fieldName == "CardType")
                        {
                            tmpValue = CR.GetCardType(vr.SearchValue, 0);
                        }
                        else
                        {
                            tmpValue = vr.SearchValue ?? "";
                        }

                        //if (vr.OP == "等于")
                        //{
                        //    vr.OP = "=";
                        //}
                        //else if (vr.OP == "不等于")
                        //{
                        //    vr.OP = "<>";
                        //}
                        //else if (vr.OP == "不小于")
                        //{
                        //    vr.OP = ">=";
                        //}
                        //else if (vr.OP == "不大于")
                        //{
                        //    vr.OP = "<=";
                        //}
                        //else
                        //{
                        //    vr.OP = "like";
                        //}

                        if (vr.OP == "like")
                        {
                            sm.Conditions.Add(new SelectModel.conditions { FieldName = vr.RealField, Operator = vr.OP, FieldValue = "%" + tmpValue + "%", Combinator = "and" });
                        }
                        else
                            sm.Conditions.Add(new SelectModel.conditions { FieldName = vr.RealField, Operator = vr.OP, FieldValue = tmpValue, Combinator = "and" });
                    }
                }
                lstSM.Add(sm);
                bSearch = false;

                lstCO = gsd.GetWhereCarCharge(lstSM);
                if (lstCO.Count > 0 && lstCO != null)
                {
                    ds = TypeCovert.ToDataSet<CarOut>(lstCO);
                    dgvCar.ItemsSource = ds.Tables[0].DefaultView;
                }
                //lblTotalCount.Content = lstCO.Count.ToString();

                rc = gsd.GetCarChargeReport(lstSM);
                if (rc != null)
                {
                    lblMthCount.Content = rc.MthCount;
                    lblMtpCount.Content = rc.MtpCount;
                    lblOthCount.Content = rc.OptCount;
                    lblStrCount.Content = rc.StrCount;
                    lblTmpCount.Content = rc.TmpCount;
                    lblFreCount.Content = rc.FreCount;
                    lblDisCount.Content = rc.TotalDiscount;
                    lblCharge.Content = rc.TotalSFJE;
                    lblYSE.Content = rc.TotalYSJE;
                    lblWeChat.Content = rc.TotalWeChat;
                    lblAlipay.Content = rc.TotalAliPay;
                    lblMoney.Content = rc.TotalCash;
                    lblOthCount.Content = rc.PerCount;
                    lblTotalCount.Content = rc.TotalCount;
                }

                bSearch = true;
                tbShow.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnSelect_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnSelect_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbQuery.Text))
                {
                    List<QueryScheme> lstQS = new List<QueryScheme>();
                    for (int i = 0; i < dgvQuery.Items.Count; i++)
                    {
                        var vr = dgvQuery.Items[i] as Query;
                        if (vr.Selected == 1)
                        {
                            QueryScheme qs = new QueryScheme();
                            qs.startime = dtStartTime.SelectedDate.Value.ToString();
                            qs.endtime = dtEndTime.SelectedDate.Value.ToString();
                            qs.FieldName = vr.RealField;
                            qs.Operators = vr.OP;
                            qs.Selectvalues = vr.SearchValue;
                            qs.QueryTable = "GetCarOut";
                            qs.SchName = cmbQuery.Text;
                            lstQS.Add(qs);
                        }
                    }

                    List<QueryScheme> lstQS0 = gsd.InParkSelectSchemeInfo(cmbQuery.Text);
                    if (lstQS0.Count > 0)
                    {
                        if (MessageBox.Show("是否覆盖此方案", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                        {
                            bool ret = gsd.InParkInsertSchem(lstQS);
                            if (ret)
                            {
                                MessageBox.Show("保存成功", "提示");
                                cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarOut");
                                cmbQuery.DisplayMemberPath = "SchName";
                            }
                            else
                            {
                                MessageBox.Show("保存失败", "提示");
                            }
                        }
                    }
                    else
                    {
                        bool ret = gsd.InParkInsertSchem(lstQS);
                        if (ret)
                        {
                            MessageBox.Show("保存成功", "提示");
                            cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarOut");
                            cmbQuery.DisplayMemberPath = "SchName";
                        }
                        else
                        {
                            MessageBox.Show("保存失败", "提示");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnSave_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nSelect_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定删除此方案吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    bool ret = gsd.InParkDeleteScheme(cmbQuery.Text);
                    if (ret)
                    {
                        cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarOut");
                        cmbQuery.DisplayMemberPath = "SchName";
                    }
                    else
                    {
                        MessageBox.Show("删除失败", "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + "btnDelete_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnDelete_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbQuery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbQuery.Items.Count > 0)
                {
                    if (cmbQuery.SelectedIndex > -1)
                    {
                        string SchName = (cmbQuery.SelectedItem as QueryScheme).SchName;

                        for (int i = 0; i < dgvQuery.Items.Count; i++)
                        {
                            var var = dgvQuery.Items[i] as Query;
                            var.OP = "=";
                            var.SearchValue = "";
                            var.Selected = 0;
                        }

                        if (!string.IsNullOrEmpty(SchName))
                        {
                            List<QueryScheme> lstQS = gsd.InParkSelectSchemeInfo(SchName);
                            if (lstQS.Count > 0)
                            {
                                dtStartTime.Text = lstQS[0].startime;
                                dtEndTime.Text = lstQS[0].endtime;
                                for (int i = 0; i < dgvQuery.Items.Count; i++)
                                {
                                    var var = dgvQuery.Items[i] as Query;
                                    for (int j = 0; j < lstQS.Count; j++)
                                    {
                                        if (lstQS[j].FieldName == var.RealField)
                                        {
                                            var.OP = lstQS[j].Operators;
                                            //BindValue(lstQS[j].FieldName);
                                            var.SearchValue = lstQS[j].Selectvalues;
                                            var.Selected = 1;
                                        }
                                    }
                                }
                            }

                            dgvQuery.Items.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + "cmbQuery_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\ncmbQuery_SelectionChanged", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void dgvQuery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //当切换条目时发生 并当checkbox为选中状态才发生
                if (dgvQuery.Items.Count > 0)
                {
                    if (dgvQuery.SelectedIndex > -1)
                    {
                        var vr = dgvQuery.SelectedItem as Query;
                        BindValue(vr.RealField);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvQuery_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\ndgvQuery_SelectionChanged", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        void BindValue(string field)
        {
            string sDate = DateTime.Now.AddMonths(-1).ToShortDateString();
            List<Dictionary<string, object>> dic = gsd.GetQueryValue("tCarOut",field, sDate);

            lstValue.Clear();
            for (int i = 0; i < dic.Count; i++)
            {
                if(dic[i].Keys.Contains(field))
                {
                    if (field == "CardType")
                    {
                        lstValue.Add(CR.GetCardType(dic[i][field].ToString(), 1));
                    }
                    else
                    {
                        lstValue.Add(dic[i][field].ToString());
                    }
                }   
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportExcel exit = new ExportExcel(dgvCar);
            exit.ShowDialog();
        }

        private void dgvCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0 && bSearch)
                {
                    if (dgvCar.SelectedIndex > -1)
                    {
                        var selectItem = dgvCar.SelectedItem as DataRowView;
                        string inPic = selectItem["InPic"].ToString();
                        strInPic = inPic;
                        string outPic = selectItem["OutPic"].ToString();
                        strOutPic = outPic;
                        string zjPic = selectItem["ZJPic"].ToString();
                        dtStartHF = Convert.ToDateTime(selectItem["InTime"]);
                        dtEndHF = Convert.ToDateTime(selectItem["OutTime"]);
                        strGateInName = selectItem["InGateName"].ToString();
                        strGateOutName = selectItem["OutGateName"].ToString();

                        loadPic(inPic, picIn);
                        loadPic(outPic, picOut);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvCar_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\ndgvCar_SelectionChanged", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintReport.CarChargeReport report = new PrintReport.CarChargeReport(lstCO, rc);
            report.ShowDialog();
        }

        private void cboValue_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox curComboBox = sender as ComboBox;
            curComboBox.ItemsSource = lstValue;
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

        public List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }

        //private void cboValue_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    ComboBox curComboBox = sender as ComboBox;
        //    List<TextBox> textBox = GetChildObjects<TextBox>(curComboBox, typeof(TextBox));
        //    if (textBox.Count > 0)
        //    {
        //        int pos = 0;
        //        pos = textBox[0].SelectionStart;
        //        textBox[0].Text = curComboBox.Text.ToUpper();
        //        textBox[0].Select(pos, 0);
                
        //    }
        //}
    }
}
