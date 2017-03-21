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
using System.Data;
using System.Printing;

namespace UI
{
    /// <summary>
    /// frmPostpone.xaml 的交互逻辑
    /// </summary>
    public partial class frmPostpone : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        CardIssue model = new CardIssue();
        private ParkingPlateRegister.PostponeHandler cpponeHandler;

        public frmPostpone()
        {
            InitializeComponent();
        }

        public frmPostpone(CardIssue ci, ParkingPlateRegister.PostponeHandler _PostponeHandler)
        {
            InitializeComponent();
            cpponeHandler = _PostponeHandler;
            model = ci;
        }

        DateTime endTime = new DateTime();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblInfo.Content = "";
                CR.BinDic(gsd.GetCardType());
                lblCardNo.Content = model.CardNO;
                lblUserName.Content = model.UserName;
                lblCPH.Content = model.CPH;
                lblUserNo.Content = model.UserNO;
                lblCardType.Content = model.CardType;
                dtpStartTime.Text = model.CarValidStartDate.ToShortDateString();
                dtpEndTime.Text = model.CarValidEndDate.ToShortDateString();
                string cardType = CR.GetCardType(model.CardType, 0);
                if (cardType.Contains("Str"))
                {
                    lblY.Visibility = Visibility.Visible;  //lbl10
                    lblBalance.Visibility = Visibility.Visible;
                    lblBalance.Content = model.Balance;
                    this.Title = "车牌充值";
                    btnPostpone.Content = "充   值";
                }
                else
                {
                    lblY.Visibility = Visibility.Hidden;
                    lblBalance.Visibility = Visibility.Hidden;
                    this.Title = "车牌延期";
                    btnPostpone.Content = "延   期";
                }

                endTime = model.CarValidEndDate;
                if (cardType.Contains("Mth") || cardType.Contains("Fre"))
                {
                    if (Model.iMonthRule == 1)
                    {
                        dtpStartTime.Text = DateTime.Now.ToShortDateString();
                        List<ChargeRules> lstCR = gsd.GetMonthRuleDefine(cardType);
                        cmbRule.ItemsSource = lstCR;
                        cmbRule.DisplayMemberPath = "FreeMinute";
                        cmbRule.SelectedValuePath = "JE";
                        if (lstCR.Count > 0)
                        {
                            lblCharge.Visibility = Visibility.Visible;
                            cmbRule.Visibility = Visibility.Visible;
                            cmbRule.SelectedIndex = 0;
                            dtpStartTime.IsEnabled = false;
                            dtpEndTime.IsEnabled = false;
                        }
                        else
                        {
                            lblCharge.Visibility = Visibility.Hidden;
                            cmbRule.Visibility = Visibility.Hidden;
                            dtpStartTime.IsEnabled = true;
                            dtpEndTime.IsEnabled = true;
                        }
                    }
                    else
                    {
                        lblCharge.Visibility = Visibility.Hidden;
                        cmbRule.Visibility = Visibility.Hidden;
                        dtpStartTime.IsEnabled = true;
                        dtpEndTime.IsEnabled = true;
                    }
                }
                else
                {
                    lblCharge.Visibility = Visibility.Hidden;
                    cmbRule.Visibility = Visibility.Hidden;
                    dtpStartTime.IsEnabled = true;
                    dtpEndTime.IsEnabled = true;
                }

                DateTime dtValidBegin, dtValidEnd, dtIn, dtNow;

                if (cardType.StartsWith("Mth"))
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic["CardNo"] = lblCardNo.Content;
                    dic["CPH"] = lblCPH.Content;
                    dic["CardType"] = cardType;
                    List<CarIn> lstCI = gsd.GetMyCarComeRecord(dic);
                    if (lstCI.Count > 0)
                    {
                        if (lstCI[0].InTime != null)
                        {
                            dtIn = lstCI[0].InTime;
                            List<CardIssue> lstCardIssue = gsd.GetMyfaxingssue(lblCardNo.Content.ToString());
                            if (lstCardIssue.Count > 0)
                            {
                                if (lstCardIssue[0].CarValidStartDate != DateTime.MinValue && lstCardIssue[0].CarValidStartDate != DateTime.MaxValue)
                                {
                                    dtNow = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                                    dtValidBegin = lstCardIssue[0].CarValidStartDate;
                                    dtValidEnd = lstCardIssue[0].CarValidEndDate;
                                    if (dtIn > dtValidEnd)
                                    {
                                        dtpStartTime.SelectedDate = dtIn;
                                        dtpEndTime.SelectedDate = dtIn.AddMonths(1);
                                        dtpStartTime.IsEnabled = false;
                                    }
                                    else if (dtIn <= dtValidEnd && dtValidEnd < dtNow)
                                    {
                                        dtpStartTime.SelectedDate = dtValidEnd;
                                        dtpEndTime.SelectedDate = dtIn.AddMonths(1);
                                        dtpStartTime.IsEnabled = false;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":frmPostpone_Load", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnPostpone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CZJinE = txtMoney.Text.Trim();
                string CarType = CR.GetCardType(model.CardType, 0);
                if (CarType.Contains("Str"))
                {
                    decimal YuE = Convert.ToDecimal(lblBalance.Content.ToString()) + Convert.ToDecimal(txtMoney.Text.Trim());
                    if (Convert.ToDecimal(txtMoney.Text.Trim()) != 0)
                    {
                        if (gsd.UpdateICYU(model.CardNO, YuE) > 0)
                        {
                            Money ICmodel = new Money();
                            ICmodel.CardNO = model.CardNO;
                            ICmodel.OptDate = DateTime.Now;
                            ICmodel.SFJE = Convert.ToDecimal(CZJinE);
                            ICmodel.OperatorNO = Model.sUserCard;
                            ICmodel.OptType = "F";

                            //2015-11-02
                            ICmodel.NewStartDate = Convert.ToDateTime(dtpStartTime.Text);
                            ICmodel.NewEndDate = Convert.ToDateTime(dtpEndTime.Text);
                            ICmodel.LastEndDate = Convert.ToDateTime(endTime);

                            if (gsd.Add(ICmodel) > 0)
                            {
                                //if (PostponeHandler == null)
                                {
                                    cpponeHandler();
                                }
                                //else
                                {
                                    //PostponeHandler("");
                                }

                                //gsd.AddLog(this.Title, string.Format(Language.LanguageXML.GetName("ICissue/i121"), strList[0], CZJinE));
                                MessageBox.Show("充值成功", "提示");

                                //2015-12-26
                                btnPostpone.IsEnabled = false;
                                if (Model.iBillPrint == 1)
                                {
                                    btnPrint.IsEnabled = true;
                                }
                                else
                                {
                                    this.Close();
                                }
                            }
                        }
                    }
                }
                else
                {

                    if (Model.iMonthRule == 1)
                    {
                        if (cmbRule.Visibility == Visibility.Visible)
                        {
                            if (cmbRule.Text != "")
                            {
                                if (lblInfo.Content.ToString() == "交纳金额不是收费规则的整数倍")
                                {
                                    MessageBox.Show(lblInfo.Content.ToString(), "提示");
                                    txtMoney.Focus();
                                    return;
                                }
                            }
                        }
                    }


                    if (Convert.ToDateTime(dtpEndTime.Text) < Convert.ToDateTime(dtpStartTime.Text))
                    {
                        MessageBox.Show("有效止日不能小于有效起日", "提示");
                        return;
                    }

                    TimeSpan ts = new TimeSpan(DateTime.Parse(dtpEndTime.SelectedDate.Value.ToString("yyyy-MM-dd")).Ticks - DateTime.Parse(dtpStartTime.SelectedDate.Value.ToString("yyyy-MM-dd")).Ticks);
                    if (ts.TotalDays < 1)
                    {
                        if (System.Windows.Forms.MessageBox.Show("延期日期只有一天，是否继续？", "询问", System.Windows.Forms.MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                        {
                            return;
                        }
                    }

                    if (gsd.UpdateICFaXingDate(model.CardNO, Convert.ToDateTime(dtpStartTime.Text), Convert.ToDateTime(dtpEndTime.Text)) > 0)
                    {
                        Money ICmodel = new Money();
                        ICmodel.CardNO = model.CardNO;
                        ICmodel.OptDate = DateTime.Now;
                        ICmodel.SFJE = Convert.ToDecimal(CZJinE);
                        ICmodel.OperatorNO = Model.sUserCard;
                        ICmodel.OptType = "F";

                        //2015-11-02
                        ICmodel.NewStartDate = Convert.ToDateTime(dtpStartTime.Text);
                        ICmodel.NewEndDate = Convert.ToDateTime(dtpEndTime.Text);
                        ICmodel.LastEndDate = Convert.ToDateTime(endTime);

                        if (gsd.Add(ICmodel) > 0)
                        {
                            //log.Add(this.Text, string.Format(Language.LanguageXML.GetName("ICissue/i123"), strList[0], CZJinE));
                            //MessageBox.Show(Language.LanguageXML.GetName("ICissue/i124"), Language.LanguageXML.GetName("CR/Prompt"));

                            //if (PostponeHandler == null)
                            {
                                cpponeHandler();
                            }
                            //else
                            {
                                // PostponeHandler("");
                            }

                            MessageBox.Show("延期成功", "提示");

                            //2015-12-26
                            btnPostpone.IsEnabled = false;
                            if (Model.iBillPrint == 1)
                            {
                                btnPrint.IsEnabled = true;
                            }
                            else
                            {
                                this.Close();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnPostpone_Click", ex.Message + "\r\n" + ex.StackTrace);
                //MessageBox.Show(ex.Message + "\r\nbtnPostpone_Click", Language.LanguageXML.GetName("CR/D6"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtMoney.Text.Length == 0)
                {
                    txtMoney.Text = "0";
                }

                if (Model.iMonthRule == 1)
                {
                    if (cmbRule.Visibility == Visibility.Visible)
                    {
                        if (cmbRule.Text != "")
                        {
                            Decimal M1 = Convert.ToDecimal(txtMoney.Text);
                            Decimal M2 = Convert.ToDecimal(cmbRule.SelectedValue.ToString());
                            int t = Convert.ToInt32(Math.Floor(Convert.ToDecimal(M1 / M2)));

                            if (t > 0)
                            {
                                TimeSpan ts = Convert.ToDateTime(endTime) - DateTime.Now;
                                DateTime dt = Convert.ToDateTime(dtpStartTime.Text);
                                dtpEndTime.Text = dt.AddMonths(Convert.ToInt32(t) * Convert.ToInt32(cmbRule.Text)).AddDays(ts.Days).ToString();
                                decimal iYu = M1 % M2;
                                if (iYu > 0)
                                {
                                    lblInfo.Content = "交纳金额不是收费规则的整数倍";
                                }
                                else
                                {
                                    lblInfo.Content = "";
                                }
                            }
                            else
                            {
                                dtpEndTime.Text = model.CarValidEndDate.ToShortDateString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtBalance_MouseLeave", ex.Message + "\r\n" + ex.StackTrace);
                //MessageBox.Show(ex.Message + "\r\ntxtBalance_MouseLeave", Language.LanguageXML.GetName("CR/D6"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMoney_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key == Key.Decimal) || (e.Key >= Key.NumPad0 && e.Key < Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void txtMoney_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (Model.iMonthRule == 1)
                {
                    if (cmbRule.Visibility == Visibility.Visible)
                    {
                        Decimal M1 = Convert.ToDecimal(txtMoney.Text);

                        Decimal M2 = Convert.ToDecimal(cmbRule.SelectedValue.ToString());
                        int t = Convert.ToInt32(Math.Floor(Convert.ToDecimal(M1 / M2)));

                        if (t > 0)
                        {
                            TimeSpan ts = Convert.ToDateTime(endTime) - DateTime.Now;
                            DateTime dt = Convert.ToDateTime(dtpStartTime.Text);
                            dtpEndTime.Text = dt.AddMonths(Convert.ToInt32(t) * Convert.ToInt32(cmbRule.Text)).AddDays(ts.Days).ToString();
                        }
                        else
                        {
                            dtpEndTime.Text = model.CarValidEndDate.ToShortDateString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtMoney_MouseLeave", ex.Message + "\r\n" + ex.StackTrace);
                //MessageBox.Show(ex.Message + "\r\ntxtMoney_MouseLeave", Language.LanguageXML.GetName("CR/D6"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<BillPrintSet> lstBPS = gsd.GetPrint();
                FlowDocument doc = new FlowDocument();
                Paragraph ph = new Paragraph();

                ph.Inlines.Add(new Run("                  充值收费票据"));
                if (lstBPS.Count > 0 && lstBPS != null)
                {
                    if (lstBPS[0].Title != "")
                    {
                        ph.Inlines.Add(new Run("\r\n" + "                  " + lstBPS[0].Title));
                    }
                    if (lstBPS[0].FTitle != "")
                    {
                        ph.Inlines.Add(new Run("\r\n" + "                  " + lstBPS[0].FTitle));
                    }
                }
                ph.Inlines.Add(new Run("\r\n-----------------------------------"));
                doc.Blocks.Add(ph);
                doc.Blocks.Add(new Paragraph(new Run("车辆编号:" + lblCardNo.Content ?? "")));                //卡片号码
                doc.Blocks.Add(new Paragraph(new Run("车辆类型:" + lblCardType.Content ?? "")));              //卡片种类
                doc.Blocks.Add(new Paragraph(new Run("人员姓名:" + lblUserName.Content ?? "")));              //人员姓名
                doc.Blocks.Add(new Paragraph(new Run("车牌号码:" + lblCPH.Content ?? "")));                   //车牌号码
                if(CR.GetCardType(lblCardType.Content.ToString(),0).Substring(0,3)=="Mth")
                {
                    doc.Blocks.Add(new Paragraph(new Run("到期日期:" + dtpEndTime.SelectedDate.Value)));      //到期日期
                    doc.Blocks.Add(new Paragraph(new Run("收费金额:" + txtMoney.Text + "元")));               //收费金额
                }
                else if (CR.GetCardType(lblCardType.Content.ToString(), 0).Substring(0, 3) == "Str")
                {
                    doc.Blocks.Add(new Paragraph(new Run("充值金额:" + txtMoney.Text + "元")));         //充值金额
                }

                doc.Blocks.Add(new Paragraph(new Run("收费时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));       //收费时间
                doc.Blocks.Add(new Paragraph(new Run("操作员姓名:" + Model.sUserName)));         //操作员
                                                                                                  
                doc.Blocks.Add(new Paragraph(new Run("日期:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));

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
                gsd.AddLog("车辆延期:btnPrint_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnPrint_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRule.SelectedIndex > -1)
            {
                ChargeRules cr = cmbRule.SelectedItem as ChargeRules;
                txtMoney.Text = cmbRule.SelectedValue.ToString();
                TimeSpan ts = Convert.ToDateTime(endTime) - DateTime.Now;
                DateTime dt = Convert.ToDateTime(dtpStartTime.Text);
                dtpEndTime.Text = dt.AddMonths(Convert.ToInt32(cr.FreeMinute)).AddDays(ts.Days).ToString();
            }
        }


    }
}
