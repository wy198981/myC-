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
using System.Collections.ObjectModel;
using System.Collections;
using System.Data;

namespace UI.Report
{
    
    /// <summary>
    /// ReportInPark.xaml 的交互逻辑
    /// </summary>
    public partial class ReportInPark : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        bool bSearch = true;
        string strGateInName = "";
        string strInPic = "";
        DateTime dtStartHF = new DateTime();
        ReportCar rc;
        List<CarIn> lstCI = new List<CarIn>();
        List<SelectModel> lstSM = new List<SelectModel>();
        List<string> lstValue = new List<string>();


        public ReportInPark()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                picIn.Image = Properties.Resources.Car2;
                BinModel.lstOP.Clear();
                BinModel.lstOP.Add("等于");
                BinModel.lstOP.Add("包含");
                BinModel.lstOP.Add("不等于");
                BinModel.lstOP.Add("不小于");
                BinModel.lstOP.Add("不大于");

                DataTable dtOP = new DataTable();
                dtOP.Columns.Add("Key", Type.GetType("System.String"));
                dtOP.Columns.Add("Value", Type.GetType("System.String"));
                dtOP.Rows.Add(new object[] { "=", "等于" });
                dtOP.Rows.Add(new object[] { "like", "包含" });
                dtOP.Rows.Add(new object[] { "<>", "不等于" });
                dtOP.Rows.Add(new object[] { ">=", "不小于" });
                dtOP.Rows.Add(new object[] { "<=", "不大于" });

                dgvQuery.ItemsSource = new ObservableCollection<Query>(gsd.GetSaveQuery("GetCarIn", "0"));

                dgOP.ItemsSource = dtOP.DefaultView;
                cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarIn");
                cmbQuery.DisplayMemberPath = "SchName";


                //ShowRights();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":ReportInPark_Load", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nReportInPark_Load", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string fieldName = "", op = "", fieldValue = "", tmpValue = "";

            lstSM.Clear();
            
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "InTime", Operator = ">=", FieldValue = dtStartTime.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"), Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "InTime", Operator = "<=", FieldValue = dtEndTime.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"), Combinator = "and" });
            if (dgvQuery.Items.Count > 0)
            {
                for (int i = 0; i < dgvQuery.Items.Count; i++)
                {
                    var vr = dgvQuery.Items[i] as Query;
                    if (vr.Selected == 1)
                    {
                        fieldName = vr.RealField.ToString();

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
                        //    op = "=";
                        //}
                        //else if(vr.OP=="不等于")
                        //{
                        //    op = "<>";
                        //}
                        //else if (vr.OP == "不小于")
                        //{
                        //    op = ">=";
                        //}
                        //else if (vr.OP == "不大于")
                        //{
                        //    op = "<=";
                        //}
                        //else
                        //{
                        //    op = "like";
                        //}

                        if (vr.DataType == "i") //数据类型是否为整形
                        {
                            fieldValue = vr.SearchValue == "" ? "0" : vr.SearchValue;
                        }
                        else
                        {
                            if (vr.OP == "like")
                            {
                                fieldValue = "%" + tmpValue + "%";
                            }
                            else
                            {
                                fieldValue = tmpValue;
                            }
                        }

                        sm.Conditions.Add(new SelectModel.conditions { FieldName = fieldName, Operator = vr.OP, FieldValue = fieldValue, Combinator = "and" });
                    }
                }
            }

            try
            {
                lstSM.Add(sm);
                bSearch = false;
                lstCI = gsd.GetWhereInCarCharge(lstSM);
 
                dgvCar.ItemsSource = TypeCovert.ToDataSet<CarIn>(lstCI).Tables[0].DefaultView;
                bSearch = true;
                tbSelect.SelectedIndex = 1;

                rc = gsd.GetWhereInStatistics(lstSM);
                if (rc != null)
                {
                    lblCarCount.Content = rc.TotalCount;
                    lblFreCount.Content = rc.FreCount;
                    lblMtpCount.Content = rc.MthCount;
                    lblStrCount.Content = rc.StrCount;
                    lblTmpCount.Content = rc.TmpCount;

                }
                
                //ParkingInfo pi = pi = gsd.GetParkingInfo(DateTime.Now.ToString("yyyyMMdd000000"), Model.iParkingNo.ToString());
                
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnSelect_Click", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\nbtnSelect_Click", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
          
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
                MessageBox.Show(ex.Message + "\r\ncmbQuery_SelectionChanged", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbQuery.Text))
                {
                    List<QueryScheme> lstQS = new List<QueryScheme>();
                    if (dgvQuery.Items.Count > 0)
                    {
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
                                
                                qs.QueryTable = "GetCarIn";
                                qs.SchName = cmbQuery.Text;
                                lstQS.Add(qs);
                            }
                        }
                    }

                    List<QueryScheme> lstQS0 = gsd.InParkSelectSchemeInfo(cmbQuery.Text);
                    if (lstQS0.Count > 0)
                    {
                        if (System.Windows.Forms.MessageBox.Show("是否覆盖此方案", "提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            bool ret = gsd.InParkInsertSchem(lstQS);
                            if (ret)
                            {
                                MessageBox.Show("保存成功", "提示");
                                cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarIn");
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
                            cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarIn");
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
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\nbtSelect_Click", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbQuery.Text))
                {
                    if (MessageBox.Show("确定删除此方案吗?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        bool ret = gsd.InParkDeleteScheme(cmbQuery.Text);
                        if (ret)
                        {
                            cmbQuery.ItemsSource = gsd.InParkSelectSchemeName("GetCarIn");
                            cmbQuery.DisplayMemberPath = "SchName";
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnDelete_Click", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\nbtnDelete_Click", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                MessageBox.Show(ex.Message + "\r\ndgvQuery_SelectionChanged", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void BindValue(string field)
        {
            string sDate = DateTime.Now.AddMonths(-1).ToShortDateString();
            List<Dictionary<string, object>> dic = gsd.GetQueryValue("tCarIn", field, sDate);
        
            lstValue.Clear();

            for (int i = 0; i < dic.Count; i++)
            {
                if (field == "CardType")
                {
                    lstValue.Add(CR.GetCardType(dic[i][field].ToString(), 1));
                }
                else
                {
                    if (dic[i].Keys.Contains(field))
                    {
                        lstValue.Add(dic[i][field].ToString());
                    }
                }
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //System.Environment.Exit(0);
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
                        dtStartHF = Convert.ToDateTime(selectItem["InTime"]);
                        strGateInName = selectItem["InGateName"].ToString();
                        if (inPic != "")
                        {
                            if (System.IO.File.Exists(Model.sImageSavePath + inPic))
                            {
                                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(Model.sImageSavePath + strInPic), picIn.Width, picIn.Height);
                                picIn.Image = bm;
                            }
                            else
                            {
                                System.Threading.ThreadPool.QueueUserWorkItem((ot) =>
                                {
                                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                    new Action(() =>
                                    {
                                    bool ret = gsd.DownLoadPic(inPic, ot.ToString());

                                    if (ret)
                                    {
                                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(ot.ToString()), picIn.Width, picIn.Height);
                                        picIn.Image = bm;
                                    }
                                    else
                                    {
                                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, picIn.Width, picIn.Height);
                                        picIn.Image = bm;


                                    }
                                    }));
                                }, Model.sImageSavePath + inPic);
                            }
                        }
                        else
                        {
                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Properties.Resources.Car2, picIn.Width, picIn.Height);
                            picIn.Image = bm;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvCar_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\ndgvCar_SelectionChanged", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintReport.InParkReport report = new PrintReport.InParkReport(lstCI, rc);
            report.ShowDialog();
        }

        private void btnDelete0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0)
                {
                    if (dgvCar.SelectedIndex > -1)
                    {
                        var vr = dgvCar.SelectedItem as DataRowView;
                        int ret = gsd.DeleteMYCARCOMERECORD(vr["CardNO"].ToString());
                        if (ret > 0)
                        {
                            lstCI = gsd.GetWhereInCarCharge(lstSM);
                            dgvCar.ItemsSource = TypeCovert.ToDataSet<CarIn>(lstCI).Tables[0].DefaultView;
                      
                            rc = gsd.GetWhereInStatistics(lstSM);
                            if (rc != null)
                            {
                                lblCarCount.Content = rc.TotalCount;
                                lblFreCount.Content = rc.FreCount;
                                lblMtpCount.Content = rc.MthCount;
                                lblStrCount.Content = rc.StrCount;
                                lblTmpCount.Content = rc.TmpCount;
                            }
                     
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnDelete0_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnDelete0_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void cboValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox curComboBox = sender as ComboBox;
            List<TextBox> textBox = GetChildObjects<TextBox>(curComboBox, typeof(TextBox));
            if (textBox.Count > 0)
            {
                int pos = 0;
                pos = textBox[0].SelectionStart;
                textBox[0].Text = curComboBox.Text.ToUpper();
                textBox[0].Select(pos, 0);
            }            
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Model.sUserCard == "888888")
            {
                btnDelete0.Visibility = Visibility.Visible;
            }
        }

       

    }

  
}
