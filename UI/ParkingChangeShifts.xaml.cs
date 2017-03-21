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
using System.Printing;

namespace UI
{
    /// <summary>
    /// ParkingChangeShifts.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingChangeShifts : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        string cardNO;
        ParkingMonitoring.UpdateStatuesHandler updateStatues;
        DateTime dtJiaoTime;
        Handover lstHandover = new Handover();

        public ParkingChangeShifts(string _cardNO,ParkingMonitoring.UpdateStatuesHandler _updateStatues)
        {
            InitializeComponent();
            cardNO = _cardNO;
            dtJiaoTime = Model.dLoginTime;
            updateStatues = _updateStatues;
        }


        
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbUserNO.Text.Trim() != Model.sUserCard)
                {
                    if (Model.sUserCard == "888888" || Model.sUserName == "管理员" || lblUserNo.Content.ToString().Trim() == "888888")
                    {
                        MessageBox.Show("【管理员】不能换班登录！", "提示");
                        return;
                    }
                    string UserNo = lblUserNo.Content.ToString();
                    string PassWord = CR.UserMd5(txtPassWord.Password.Trim().ToString());
                    if (UserNo == "")
                    {
                        MessageBox.Show("操作编号不能为空！", "提示");
                        return;
                    }
                    else
                    {
                        string data = (new Request()).GetToken(UserNo, PassWord);
                        if (data != null && data.Length > 0)
                        {
                            if (Model.bOnlyLocation)
                            {
                                AutoPrint();
                            }

                            List<Operators> lstOP = gsd.GetOperators(UserNo);
                            if (lstOP.Count > 0)
                            {
                                DateTime dtTime = Model.dLoginTime;
                                int ret = gsd.SetHandover(Model.sUserCard, lstOP[0].CardNO, Model.dLoginTime);

                                if (ret > 0)
                                {
                                    Model.sUserName = lstOP[0].UserName;
                                    Model.sUserCard = lstOP[0].CardNO;
                                    //Model.sUserPwd = lstOP[0].Pwd;
                                    if (CR.GetAppConfig("UserCode") == Model.sUserCard)
                                    {
                                        Model.dLoginTime = Convert.ToDateTime(CR.GetAppConfig("LoginDate"));
                                    }
                                    else
                                    {
                                        CR.UpdateAppConfig("UserCode", Model.sUserCard);
                                        CR.UpdateAppConfig("LoginDate", DateTime.Now.ToString());
                                        Model.dLoginTime = DateTime.Now;
                                    }
                                    updateStatues("增加一条新换班纪录");
                                }
                                else
                                {
                                    updateStatues("新换班纪录添加失败");
                                }
                            }
                            else
                            {
                                MessageBox.Show("用户编号输入不正确！", "提示");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("用户编号或者密码输入不正确！", "提示");
                            return;
                        }
                        
                    }
                }
                else
                {
                    updateStatues("相同操作编号不用换班");
                    //LblDisplay.Text = "相同操作卡号不用换班！";
                }

                txtPassWord.Password = "";
                lblUserName.Content = "";

                this.Close();
            }
            catch (Exception ex)
            {
                //LblDisplay.Text = ex.Message + "(" + btnLogin.Text + ")";
                gsd.AddLog("在线监控" + ":btnLogin_Click", ex.Message + "\r\n" + ex.StackTrace);
            }
        }


        void AutoPrint()
        {
            try
            {
                List<BillPrintSet> lstBPS = gsd.GetPrint();
                FlowDocument doc = new FlowDocument();
                Paragraph ph = new Paragraph();
         
                lstHandover = gsd.GetHuanBan(cardNO, lblUserNo.Content.ToString(), dtJiaoTime, DateTime.Now);

                //ph.Inlines.Add(new Run("                  停车卡初次发行收费票据"));
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
                doc.Blocks.Add(new Paragraph(new Run("操作员编号:" + Model.sUserCard)));
                doc.Blocks.Add(new Paragraph(new Run("姓    名:" + Model.sUserName)));
                doc.Blocks.Add(new Paragraph(new Run("接班时间:" + dtJiaoTime.ToString("yyyy-MM-dd HH:mm:ss"))));
                doc.Blocks.Add(new Paragraph(new Run("交班时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                doc.Blocks.Add(new Paragraph(new Run("场内月租车辆:" + lstHandover.InMthNum)));
                doc.Blocks.Add(new Paragraph(new Run("场内临时车辆:" + lstHandover.InTmpNum)));
                doc.Blocks.Add(new Paragraph(new Run("月租车出场数:" + lstHandover.OutMthNum)));
                doc.Blocks.Add(new Paragraph(new Run("临时车出场数:" + lstHandover.InMthNum)));
                doc.Blocks.Add(new Paragraph(new Run("免费车出场数:" + lstHandover.FreCarNum)));
                doc.Blocks.Add(new Paragraph(new Run("临免车出场数:" + lstHandover.OutTfrNum)));
                doc.Blocks.Add(new Paragraph(new Run("手动开闸次数:" + lstHandover.OutPerOpen)));
                doc.Blocks.Add(new Paragraph(new Run("临时车辆收费:" + lstHandover.TmpPaid)));

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
                gsd.AddLog("车牌登记:btnPrint_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnPrint_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGiveUp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (lblUserNo.Content.ToString() == "")
            {
                MessageBox.Show("请选择操作员编号", "提示");
                return;
            }

            lstHandover = gsd.GetHuanBan(cardNO, lblUserNo.Content.ToString(), dtJiaoTime, DateTime.Now);
            //lstHandover.TakeOverOperatorName = cmbUserNO.Text;
            //lstHandover.ThisTakeOverTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //lstHandover.Remarks = "";
            //lstHandover.TakeOverOperatorNO = lblUserNo.Content.ToString();
            //lstHandover.TakeOverOperatorCard = lblUserNo.Content.ToString();
            List<Handover> lstHan = new List<Handover>();
            lstHan.Add(lstHandover);
            PrintReport.WorkJiaoJieReport report = new PrintReport.WorkJiaoJieReport(lstHan);

            report.Owner = this;
            report.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush berriesBrush = new ImageBrush();
            berriesBrush.ImageSource = new BitmapImage(new Uri(@"Resources\Main0.jpg", UriKind.Relative));

            this.Background = berriesBrush;

            cmbUserNO.ItemsSource = gsd.GetUser(cardNO);
            cmbUserNO.DisplayMemberPath = "UserName";
            cmbUserNO.SelectedValuePath = "UserNO";
            if (cmbUserNO.Items.Count > 0)
            {
                cmbUserNO.SelectedIndex = 0;
                lblUserNo.Content = cmbUserNO.SelectedValue.ToString();
            }
            else
            {
                lblUserNo.Content = "";
            }
        
            txtPassWord.Password = "";
        }

        private void cmbUserNO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUserNO.Items.Count > 0)
            {
                lblUserNo.Content = cmbUserNO.SelectedValue.ToString();
            }
        }
    }
}
