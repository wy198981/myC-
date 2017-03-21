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
    /// ParkingUpdateCPH.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingUpdateCPH : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        string strCPH = "";
        string strOutPic = "";
        string UpdateCardNO = "";
        public ParkingUpdateCPH()
        {
            InitializeComponent();
        }

        public ParkingUpdateCPH(string _strCPH, string _strOutPic, string _UpdateCardNO)
        {
            InitializeComponent();
            strCPH = _strCPH;
            strOutPic = _strOutPic;
            UpdateCardNO = _UpdateCardNO;

            foreach (UIElement element in grdHeader.Children)
            {
                if (element is RadioButton)
                {
                    RadioButton current = ((RadioButton)element);
                    current.Click += current_Click;
                }
            }
        }

        private void current_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string cph = txtCPH.Text + txtUpdateCPH.Text;
                //if (cph.Length == 7)
                //{
                    txtCPH.Text = ((RadioButton)sender).Content.ToString();
                    //txtUpdateCPH.Text = ((RadioButton)sender).Content.ToString() + txtUpdateCPH.Text.Substring(1, 6);
                //}
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":current_Click", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cph = txtCPH.Text + txtUpdateCPH.Text;

                if (cph == "")
                {
                    System.Windows.Forms.MessageBox.Show("请输入车牌!", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                string strCPH = "京津冀晋蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新港澳台警使武领学民航";
                if (cph != "" && cph.Length != 7)
                {
                    if (cph.Length == 8 && cph.Substring(0, 2) == "WJ")
                    {

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return;
                    }
                }
                else if (cph.Length == 7 && cph.Substring(0, 2) == "WJ" && strCPH.Contains(cph.Substring(2, 1)))
                {
                    System.Windows.Forms.MessageBox.Show("武警车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }


                int ret = gsd.GetInCPH(cph);

                if (ret > 0)
                {
                    MessageBox.Show("该车辆为入场车辆", "提示");
                    return;
                }

                if (cph.Trim() != "")
                {
                    string tmpCardType = "";
                    string tmpCardNO = "";
                    List<CardIssue> lstCI = gsd.SelectFaXing(cph);
                    if (lstCI.Count > 0)
                    {
                        tmpCardType = lstCI[0].CarCardType;
                        tmpCardNO = lstCI[0].CardNO;
                    }
                    gsd.UpdateComeCPH(tmpCardNO, tmpCardType, UpdateCardNO, cph);
                    gsd.AddLog("车牌校验", strCPH + "修改车牌为：" + cph);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnOK_Click", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(@"Resources\Main0.jpg", UriKind.Relative));

                this.Background = berriesBrush;

                picNetVideo0.Image = Properties.Resources.Car2;

                if (strCPH.Length == 8)
                {
                    txtUpdateCPH.Text = strCPH.Substring(2);
                    txtCPH.Text = strCPH.Substring(0, 2);
                }
                else if (strCPH.Length == 7)
                {
                    txtUpdateCPH.Text = strCPH.Substring(1);
                    txtCPH.Text = strCPH.Substring(0, 1);
                }
                
                //txtUpdateCPH.Text = strCPH;
                if (strOutPic != "")
                {
                    if (System.IO.File.Exists(Model.sImageSavePath + strOutPic))
                    {
                        picNetVideo0.Image.Dispose();
                        System.Drawing.Image fileImage = System.Drawing.Image.FromFile(Model.sImageSavePath + strOutPic);
                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(fileImage, picNetVideo0.Width, picNetVideo0.Height);
                        picNetVideo0.Image = bm;
                        fileImage.Dispose();
                    }
                    else
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem((ot) =>
                        {
                            bool ret = gsd.DownLoadPic(strOutPic, ot.ToString());
                            if (ret)
                            {
                                picNetVideo0.Image.Dispose();
                                System.Drawing.Image fileImage = System.Drawing.Image.FromFile(ot.ToString());
                                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(fileImage, picNetVideo0.Width, picNetVideo0.Height);
                                picNetVideo0.Image = bm;
                                fileImage.Dispose();
                            }
                        }, Model.sImageSavePath + strOutPic);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":ParkingUpdateCPH_Load", ex.Message + "\r\n" + ex.StackTrace);
            }
        }
    }
}
