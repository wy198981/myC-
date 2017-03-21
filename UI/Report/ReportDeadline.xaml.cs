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
using ParkingInterface;
using ParkingModel;

namespace UI.Report
{
    /// <summary>
    /// ReportDeadline.xaml 的交互逻辑
    /// </summary>
    public partial class ReportDeadline : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        public ReportDeadline()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ShowRights();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<SelectModel> lstSM = new List<SelectModel>();
                SelectModel sm = new SelectModel();
                if (rbtSurplusDays.IsChecked == true)
                {
                    if (CR.IsInteger(txtSurplusDays.Text))
                    {
                        sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarValidEndDate", Operator = ">=", FieldValue = DateTime.Now.ToShortDateString(), Combinator = "and" });
                        sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarValidEndDate", Operator = "<=", FieldValue = DateTime.Now.AddDays(Convert.ToInt32(txtSurplusDays.Text)).ToShortDateString(), Combinator = "and" });
                    }
                    else
                    {
                        MessageBox.Show("请输入正确的数字", "提示");
                        txtSurplusDays.Focus();
                        return;
                    }
                   
                }
                else if (rbtTime.IsChecked == true)
                {
                    sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarValidEndDate", Operator = ">=", FieldValue = dtStartTime.SelectedDate.Value, Combinator = "and" });
                    sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarValidEndDate", Operator = "<=", FieldValue = dtEndTime.SelectedDate.Value, Combinator = "and" });
                }
                lstSM.Add(sm);
                List<CardIssue> lstCI = gsd.GetWhereDeadline(lstSM);
                BindControl(lstCI);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnSelect_Click", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\n" + "btnSelect_Click", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        void BindControl(List<CardIssue> lstCI)
        {
            lblCount.Content = lstCI.Count;
            dgvDeadline.ItemsSource = TypeCovert.ToDataSet<CardIssue>(lstCI).Tables[0].DefaultView;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtCarNo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (rbtCarNo.IsChecked == true)
                {
                    List<SelectModel> lstSM = new List<SelectModel>();
                    SelectModel sm = new SelectModel();
                    sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "like", FieldValue = "%" + txtCarNo.Text + "%", Combinator = "and" });
                    lstSM.Add(sm);
                    List<CardIssue> lstCI = gsd.GetWhereDeadline(lstSM);
                    BindControl(lstCI);
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtCPH_KeyPress", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\ntxtCPH_KeyPress", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void txtUserNo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (rbtUserNo.IsChecked == true)
                {
                    List<SelectModel> lstSM = new List<SelectModel>();
                    SelectModel sm = new SelectModel();
                    sm.Conditions.Add(new SelectModel.conditions { FieldName = "UserNO", Operator = "like", FieldValue = "%" + txtUserNo.Text + "%", Combinator = "and" });
                    lstSM.Add(sm);
                    List<CardIssue> lstCI = gsd.GetWhereDeadline(lstSM);
                    BindControl(lstCI);
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch(Exception ex)
            {
                gsd.AddLog(this.Title + ":txtCPH_KeyPress", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\ntxtCPH_KeyPress", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void txtSurplusDays_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;

            //屏蔽非法按键
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                //if (txt.Text.Contains(".") && e.Key == Key.Decimal)
                //{
                //    e.Handled = true;
                //    return;
                //}
                e.Handled = false;
            }
           
            //else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            //{
            //    if (txt.Text.Contains(".") && e.Key == Key.OemPeriod)
            //    {
            //        e.Handled = true;
            //        return;
            //    }
            //    e.Handled = false;
            //}
            else
            {
                e.Handled = true;
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportExcel export = new ExportExcel(dgvDeadline);
            export.ShowDialog();
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
    }
}
