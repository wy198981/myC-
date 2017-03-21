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
using ParkingInterface;

namespace UI
{
    /// <summary>
    /// ParkingPlateNoInput.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingPlateNoInput : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        private ParkingMonitoring.NoCPHHandler NoCPHHandler;
        /// <summary>
        /// 入出场标识(0表示入场，1表示出场)
        /// </summary>
        private int inOutFlag = 0;

        public ParkingPlateNoInput()
        {
            InitializeComponent();
        }

        public ParkingPlateNoInput(ParkingMonitoring.NoCPHHandler _nocphhandler,int _inOutFlag)
        {
            InitializeComponent();
            NoCPHHandler = _nocphhandler;
            inOutFlag = _inOutFlag;

            foreach (UIElement element in grdHeader.Children)
            {
                if (element is RadioButton)
                {
                    RadioButton current = ((RadioButton)element);
                    if (current.Content.ToString() == Model.LocalProvince)
                    {
                        current.IsChecked = true;
                    }
                    current.Click += current_Click;
                }
            }
        }


        int iCount = 0;
        private void current_Click(object sender, RoutedEventArgs e)
        {
            if (iCount >= 0)
            {
                cmbCarNumber.Text = ((RadioButton)sender).Content.ToString();
                if (rbtWJ.IsChecked == true)
                {
                    if (txtCarNumber.Text.Length > 0)
                    {
                        string strCPH = "京津冀晋蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新港澳台警使武领学民航";
                        if (strCPH.Contains(txtCarNumber.Text.Substring(0, 1)))
                        {
                            txtCarNumber.Text = ((RadioButton)sender).Content.ToString() + txtCarNumber.Text.Substring(1);
                        }
                        else
                        {
                            txtCarNumber.Text = ((RadioButton)sender).Content.ToString() + txtCarNumber.Text;
                        }
                    }
                    else
                    {
                        txtCarNumber.Text = ((RadioButton)sender).Content.ToString() + txtCarNumber.Text;
                    }
                }
                else
                {
                    lblCarNumber.Content = ((RadioButton)sender).Content;
                    txtCarNumber.Focus();
                }
            }
            else
            {
                txtCarNumber.Focus();
            }
            iCount++;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CPH = "";
                if (rbtPT.IsChecked == true || rbtWJ.IsChecked == true)
                {
                    CPH = lblCarNumber.Content.ToString() + txtCarNumber.Text;
                }
                else if (rbtJinC.IsChecked == true)
                {
                    CPH = lblCarNumber.Content.ToString() + txtCarNumber.Text + lblJCNumber.Content.ToString();
                }
                else if (rbtJC.IsChecked == true)
                {
                    CPH = txtCarNumber.Text;
                }



                CPH = CPH.ToUpper();


                if (ParkingInterface.CR.CheckUpCPH(CPH))
                {
                    NoCPHHandler(CPH);
                    this.Close();
                }

                //string strCPH = "京津冀晋蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新港澳台警使武领学民航";
                //if (CPH.Length == 7)
                //{
                //    if (CPH.Length == 7 && CPH.Substring(0, 2) == "WJ" && strCPH.Contains(CPH.Substring(2, 1)))
                //    {
                //        System.Windows.Forms.MessageBox.Show("武警车牌号不规范!请重新输入！\n\n【" + CPH + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                //        return;
                //    }
                //    NoCPHHandler(CPH);
                //    this.Close();
                //}

                //else if (CPH.Length == 8 && CPH.Substring(0, 2) == "WJ")
                //{
                //    NoCPHHandler(CPH);
                //    this.Close();
                //}
                else
                {
                    MessageBox.Show("请输入正确的车牌号", "提示");
                    return;
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnOK_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnOK_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NoCPHHandler("");
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                cmbCarNumber.Text = Model.LocalProvince;
                lblCarNumber.Content = Model.LocalProvince;
                txtCarNumber.Focus();//2015-10-13
                dgvCarIn.AutoGenerateColumns = false;   //2015-12-05
                lblJCNumber.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nWindow_Loaded", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtCarNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O || e.Key == Key.I)
            {
                e.Handled = true;
            }

            //string strCPH = "京津冀晋蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新港澳台警使武领学民航0123456789";
            //if (strCPH.Contains(Convert.ToChar(e.Key)))
            //{

            //}
            //else
            //{
            //    e.Handled = true;
            //}

            

            //if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.A && e.Key <= Key.Z)))
            //{
            //    e.Handled = true;
            //}
        }

        private void txtCarNumber_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (inOutFlag == 0)
                {
                    if (txtCarNumber.Text.Trim().Length >= 3)
                    {
                        //模糊查询发行表
                        List<CardIssue> dt1 = gsd.SelectFxCPH_Like(txtCarNumber.Text.Trim());
                        if (dt1 != null)
                            dgvCarIn.ItemsSource = dt1;
                        if (dgvCarIn.Items.Count > 0)
                        {
                            dgvCarIn.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            dgvCarIn.Visibility = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        dgvCarIn.Visibility = Visibility.Hidden;
                    }
                }
                else   //出场查询入场表
                {
                    if (txtCarNumber.Text.Trim().Length >= 3)
                    {
                        //模糊查询场内表
                        List<CarIn> dt2 = gsd.SelectComeCPH_Like(txtCarNumber.Text.Trim());
                        if (dt2 != null)
                            dgvCarIn.ItemsSource = dt2;
                        if (dgvCarIn.Items.Count > 0)
                        {
                            dgvCarIn.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            dgvCarIn.Visibility = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        dgvCarIn.Visibility = Visibility.Hidden;
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtCarNumber_KeyUp", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\ntxtCarNumber_KeyUp", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvCarIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgvCarIn.Items.Count > 0)
                {
                    if (dgvCarIn.SelectedIndex > -1)
                    {
                        dgvCarIn.Visibility = Visibility.Hidden;
                        object obj;
                        if (inOutFlag == 0)
                        {
                            var selectItem = dgvCarIn.SelectedItem as CardIssue;
                            obj = selectItem.CPH;
                        }
                        else
                        {
                            var selectItem = dgvCarIn.SelectedItem as CarIn;
                            obj = selectItem.CPH;
                        }
                        cmbCarNumber.Text = obj.ToString().Substring(0, 1);
                        txtCarNumber.Text = obj.ToString().Substring(1);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvCarIn_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\ndgvCarIn_SelectionChanged", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbtPT_Click(object sender, RoutedEventArgs e)
        {
            if (rbtPT.IsChecked == true)
            {
                lblJCNumber.Visibility = Visibility.Hidden;
                lblCarNumber.Visibility = Visibility.Visible;
                lblCarNumber.Content = Model.LocalProvince;
                txtCarNumber.MaxLength = 6;
                txtCarNumber.Text = "";
                txtCarNumber.Focus();
            }
        }

        private void rbtJinC_Click(object sender, RoutedEventArgs e)
        {
            if (rbtJinC.IsChecked == true)
            {
                lblJCNumber.Visibility = Visibility.Visible;
                lblCarNumber.Visibility = Visibility.Visible;
                lblCarNumber.Content = Model.LocalProvince;
                txtCarNumber.MaxLength = 5;
                txtCarNumber.Text = "";
                txtCarNumber.Focus();
            }
        }

        private void rbtWJ_Click(object sender, RoutedEventArgs e)
        {
            if (rbtWJ.IsChecked == true)
            {
                lblJCNumber.Visibility = Visibility.Collapsed;
                lblCarNumber.Visibility = Visibility.Visible;
                lblCarNumber.Content = "WJ";
                txtCarNumber.MaxLength = 6;
                txtCarNumber.Text = "";
                txtCarNumber.Focus();
            }
        }

        private void rbtJC_Click(object sender, RoutedEventArgs e)
        {
            if (rbtJC.IsChecked == true)
            {
                lblJCNumber.Visibility = Visibility.Hidden;
                lblCarNumber.Visibility = Visibility.Hidden;
                txtCarNumber.MaxLength = 7;
                txtCarNumber.Text = "";
                txtCarNumber.Focus();
            }
        }

        private void txtCarNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            int pos = 0;
            pos = txtCarNumber.SelectionStart;
            txtCarNumber.Text = txtCarNumber.Text.ToUpper();
            txtCarNumber.Select(pos, 0);
        }

       

     
    }
}
