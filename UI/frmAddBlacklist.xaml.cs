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
using System.Data;
using ParkingModel;
using System.ComponentModel;
using ParkingInterface;



namespace UI
{
    //public class MyViewModel : INotifyPropertyChanged
    //{
    //    string text;
    //    public string Text { get { return text; } set { text = value; this.OnPropertyChanged(Text); }}

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected virtual void OnPropertyChanged(string propertyName)
    //    {
    //        if (this.PropertyChanged != null)
    //            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}

    /// <summary>
    /// frmAddBlacklist.xaml 的交互逻辑
    /// </summary>
    public partial class frmAddBlacklist : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        public frmAddBlacklist()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //txtCPH.DataContext = new MyViewModel();
                cmbHeader.Text = Model.LocalProvince;
                dgvCar.ItemsSource = gsd.SelectBlacklist();
                ShowRights();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + "AddBlacklist_Load");
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCPH.Text.Trim() == "")
                {
                    MessageBox.Show("车牌号码不能为空!", "提示");
                    return;
                }

                if (txtReason.Text.Trim() == "")
                {
                    MessageBox.Show("添加黑名单原因不能为空!", "提示");
                    return;
                }
                if (null == dtpStart.SelectedDate.Value || null == dtpEnd.SelectedDate.Value)
                {
                    MessageBox.Show("限制日期不能为空!", "提示");
                    return;
                }

                Blacklist blacklist = new Blacklist();
                blacklist.CPH = cmbHeader.Text + txtCPH.Text;
                blacklist.StartTime = Convert.ToDateTime(dtpStart.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                blacklist.EndTime = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                blacklist.Reason = txtReason.Text;
                blacklist.AddDelete = 0;
                blacklist.DownloadSignal = "000000000000000";
                int ret = gsd.AddMYBlacklist(blacklist);
                if (ret > 0)
                {
                    gsd.AddLog("黑名单车辆", Model.sUserName + ":添加黑名单车辆：" + blacklist.CPH);
                    dgvCar.ItemsSource = gsd.SelectBlacklist();
                    MessageBox.Show("添加成功!", "提示");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("frmAddBlacklist", ex.Message + "btnAdd_Click");
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCPH.Text.Trim() == "")
                {
                    MessageBox.Show("车牌号码不能为空!", "提示");
                    return;
                }

                if (txtCPH.Text.Trim().Length != 6)
                {
                    MessageBox.Show("不是正确的车牌号码!", "提示");
                    return;
                }
                dgvCar.ItemsSource = gsd.SelectBlacklist(cmbHeader.Text + txtCPH.Text.ToUpper());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "btnSelect_Click");
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvCar.Items.Count > 0)// datagridView
                {
                    if (dgvCar.SelectedIndex > -1)
                    {
                        Blacklist bl = dgvCar.SelectedItem as Blacklist;
                        int ID = bl.ID;
                        string strCPH = bl.CPH;
                        string strDownloadSignal = bl.DownloadSignal;
                        bool ret = false;
                        if (strDownloadSignal == "000000000000000")
                        {
                            ret = gsd.DeleteMYBlacklist(ID);
                        }
                        else
                        {
                            ret = gsd.UpdateMYBlacklist(ID);
                        }
                        if (ret)
                        {
                            dgvCar.ItemsSource = gsd.SelectBlacklist();
                            MessageBox.Show("删除成功!", "提示");
                            gsd.AddLog("黑名单车辆", Model.sUserName + ":删除黑名单车辆：" + strCPH);
                        }

                        dgvCar.ItemsSource = gsd.SelectBlacklist();// 是不是多一次过程
                    }
                    else
                    {
                        MessageBox.Show("请选择需要删除的黑名单!", "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "btnDelete_Click");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
        void ShowRights()
        {
            GetUiAllRightButton((this.Content as Grid).Children);
            long pid = gsd.GetIDByName("在线监控", "btnBlacklist");
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
                    //v.Visibility = lstRs[0].CanView == true ? Visibility.Visible : Visibility.Hidden;
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

        private void txtCPH_TextChanged(object sender, TextChangedEventArgs e)
        {
            int pos = 0;
            pos = txtCPH.SelectionStart;
            txtCPH.Text = txtCPH.Text.ToUpper();
            txtCPH.Select(pos, 0);
        }

        private void btnDel_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ret = false;
                for (int i = 0; i < dgvCar.Items.Count; i++)
                {
                    Blacklist bl = dgvCar.Items[i] as Blacklist;
                    int ID = bl.ID;
                    string strCPH = bl.CPH;
                    string strDownloadSignal = bl.DownloadSignal;
                  
                    if (strDownloadSignal == "000000000000000")
                    {
                        ret = gsd.DeleteMYBlacklist(ID);
                    }
                    else
                    {
                        ret = gsd.UpdateMYBlacklist(ID);
                    }
                    if (ret)
                    {
                        gsd.AddLog("黑名单车辆", Model.sUserName + ":删除黑名单车辆：" + strCPH);
                    }
                }
                if(ret)
                {
                    dgvCar.ItemsSource = gsd.SelectBlacklist();
                    MessageBox.Show("全部删除成功!", "提示");
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "btnDel_Copy_Click");
            }
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
