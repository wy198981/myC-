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

namespace UI
{
    /// <summary>
    /// frmPersonnelSelect.xaml 的交互逻辑
    /// </summary>
    public partial class frmPersonnelSelect : SFMControls.WindowBase
    {
        private ParkingPlateRegister.SelectPersonnelDataHandler CPHDJfxPersonnelDataHandler;
        GetServiceData gsd = new GetServiceData();

        public frmPersonnelSelect()
        {
            InitializeComponent();
        }

        public frmPersonnelSelect(ParkingPlateRegister.SelectPersonnelDataHandler _CPHDJfxPersonnelDataHandler)
        {
            InitializeComponent();
            CPHDJfxPersonnelDataHandler = _CPHDJfxPersonnelDataHandler;//委托赋值
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserNO.IsEnabled = false;
            txtUserName.IsEnabled = false;
            txtDeptName.IsEnabled = false;
            txtHomeAddress.IsEnabled = false;
            txtIDCard.IsEnabled = false;
            txtMobNumber.IsEnabled = false;
            txtCPH.IsEnabled = false;
        }

        private void txtUserNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemQuotes || !((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9)))
            {
                e.Handled = true;
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtDeptName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtHomeAddress_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtIDCard_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ckbUserNO_Click(object sender, RoutedEventArgs e)
        {
            if (ckbUserNO.IsChecked == true)
            {
                txtUserNO.IsEnabled = true;
            }
            else
            {
                txtUserNO.IsEnabled = false;
                txtUserNO.Text = "";
            }
        }

        private void ckbUserName_Click(object sender, RoutedEventArgs e)
        {
            if (ckbUserName.IsChecked == true)
            {
                txtUserName.IsEnabled = true;
            }
            else
            {
                txtUserName.IsEnabled = false;
                txtUserName.Text = "";
            }
        }

        private void ckbDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (ckbDepartment.IsChecked == true)
            {
                txtDeptName.IsEnabled = true;
            }
            else
            {
                txtDeptName.IsEnabled = false;
                txtDeptName.Text = "";
            }
        }

        private void ckbHomeAddress_Click(object sender, RoutedEventArgs e)
        {
            if (ckbHomeAddress.IsChecked == true)
            {
                txtHomeAddress.IsEnabled = true;
            }
            else
            {
                txtHomeAddress.IsEnabled = false;
                txtHomeAddress.Text = "";
            }
        }

        private void ckbIDCard_Click(object sender, RoutedEventArgs e)
        {
            if (ckbIDCard.IsChecked == true)
            {
                txtIDCard.IsEnabled = true;
            }
            else
            {
                txtIDCard.IsEnabled = false;
                txtIDCard.Text = "";
            }
        }

        private void ckbMobNumber_Click(object sender, RoutedEventArgs e)
        {
            if (ckbMobNumber.IsChecked == true)
            {
                txtMobNumber.IsEnabled = true;
            }
            else
            {
                txtMobNumber.IsEnabled = false;
                txtMobNumber.Text = "";
            }
        }

        private void ckbCPH_Click(object sender, RoutedEventArgs e)
        {
            if (ckbCPH.IsChecked == true)
            {
                txtCPH.IsEnabled = true;
            }
            else
            {
                txtCPH.IsEnabled = false;
                txtCPH.Text = "";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<SelectModel> lstSM = new List<SelectModel>();
                SelectModel sm = new SelectModel();
                if (txtUserNO.Text.ToString().Trim() != "")
                {
                    sm.Conditions.Add(new SelectModel.conditions() { FieldName = "UserNO", Operator = "like", FieldValue = "%" + txtUserNO.Text.ToString().Trim() + "%", Combinator = "and" });
                }
                if (txtUserName.Text.ToString().Trim() != "")
                {
                    sm.Conditions.Add(new SelectModel.conditions() { FieldName = "UserName", Operator = "like", FieldValue = "%" + txtUserName.Text.ToString().Trim() + "%", Combinator = "and" });
                }
                if (txtDeptName.Text.ToString().Trim() != "")
                {
                    sm.Conditions.Add(new SelectModel.conditions() { FieldName = "DeptName", Operator = "like", FieldValue = "%" + txtDeptName.Text.ToString().Trim() + "%", Combinator = "and" });
                }
                if (txtHomeAddress.Text.ToString().Trim() != "")
                {
                    sm.Conditions.Add(new SelectModel.conditions() { FieldName = "HomeAddress", Operator = "like", FieldValue = "%" + txtHomeAddress.Text.ToString().Trim() + "%", Combinator = "and" });
                }
                if (txtIDCard.Text.ToString().Trim() != "")
                {
                    sm.Conditions.Add(new SelectModel.conditions() { FieldName = "IDCard", Operator = "like", FieldValue = "%" + txtIDCard.Text.ToString().Trim() + "%", Combinator = "and" });
                }
                if (txtMobNumber.Text.ToString().Trim() != "")
                {
                    sm.Conditions.Add(new SelectModel.conditions() { FieldName = "MobNumber", Operator = "like", FieldValue = "%" + txtMobNumber.Text.ToString().Trim() + "%", Combinator = "and" });
                }
                if (txtCPH.Text.ToString().Trim() != "")
                {
                    sm.Conditions.Add(new SelectModel.conditions() { FieldName = "CPH", Operator = "like", FieldValue = "%" + txtCPH.Text.ToString().Trim() + "%", Combinator = "and" });
                }
                lstSM.Add(sm);
                string where = JsonJoin.ModelToJson(lstSM);
                if (CPHDJfxPersonnelDataHandler != null)
                {
                    CPHDJfxPersonnelDataHandler(where);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnSave_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnSave_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
