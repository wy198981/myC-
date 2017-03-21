using ParkingCommunication;
using ParkingInterface;
using ParkingModel;
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

namespace UI
{
    /// <summary>
    /// SelectDown.xaml 的交互逻辑
    /// </summary>
    public partial class SelectDown : SFMControls.WindowBase
    {
        public SelectDown()
        {
            InitializeComponent();
        }

        Request req = new Request();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<CardIssue> lstIssue;

            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue");

            dataGridView1.IsReadOnly = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ItemsSource = lstIssue;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CardIssue ci;

            try
            {
                ci = dataGridView1.SelectedItem as CardIssue;
                if (null != ci)
                {
                    //DataGridViewRow dataGridViewRow = this.dataGridView1.CurrentRow;
                    //DataRow dataRow = (dataGridViewRow.DataBoundItem as DataRowView).Row;

                    string CardNO = ci.CardNO;
                    string DateStart = ci.CarValidStartDate.ToString("yyyy-MM-dd");
                    string DateEnd = ci.CarValidEndDate.ToString("yyyy-MM-dd");
                    string Jihao = ci.DownloadSignal;
                    string CardType = ci.CarCardType;
                    Get(CardNO, DateStart, DateEnd, Jihao, CardType, ci);
                }
            }
            catch (Exception ex)
            {
                req.AddLog(this.Title + ":btnSelect_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnSelect_Click", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Get(string CardNO, string DateStart, string DateEnd, string Jihao, string CardType, CardIssue ci)
        {
            string CarNO = CardNO;
            string StatType = CR.GetLoadHead(CardType);
            if (ckbOffLine.IsChecked ?? false)
            {
                //2016-04-26
                if (CardType.Substring(0, 3) == "Str")
                {
                    StatType = "E6";
                }
                else
                {
                    StatType = "E0";//判断为储值卡 该值为E6 其他时为E0
                }
                //StatType = "E0";
                CarNO = CR.GetCPHtoCardNO(ci.CPH);
            }
            string CarNumber = CarNO;
            string strDateTime = Convert.ToDateTime(DateStart).ToShortDateString().Replace("-", "").Substring(2, 6);
            string strDateTime1 = Convert.ToDateTime(DateEnd).ToShortDateString().Replace("-", "").Substring(2, 6);
            if (CarNO.Length < 8)
            {
                StatType = "C0";
                if (Model.iICCardDownLoad == 0)
                {
                    strDateTime = "000000";
                    strDateTime1 = "000000";
                }
                CarNumber = Convert.ToString(Convert.ToInt32(CarNO), 16);
            }


            CarNumber = CarNumber.PadLeft(14, '0');

            string strJiHao = (Jihao ?? "").PadRight(127, '0');

            string strJiHaoSum = strJiHao;
            //foreach (char a in strJiHao.ToCharArray())
            //{
            //    strJiHaoSum += ConvertToBin(a);
            //}

            //替换成车道
            //Dictionary<int, string> dic = CR.GetIP() ;

            SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
            for (int j = 0; j < Model.iChannelCount; j++)
            {
                if (strJiHaoSum.Substring(Model.Channels[j].iCtrlID - 1, 1) == "0")
                {
                    //CR.GetDownLoad(dataRow);
                    // string a = sendbll.CheckDldCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, StatType + CarNumber + strDateTime + strDateTime1 + "0000", Model.Channels[j].iXieYi);
                    //string a = sendbll.CheckDldCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, CR.GetDownLoad(dataRow), Model.Channels[j].iXieYi);
                    //SetTextMessage1(k, CarNO, a, Model.Channels[j].sIP, 0);


                    string a = "";
                    string strsend = "";
                    if (ckbOffLine.IsChecked ?? false)
                    {

                        strsend = CR.GetDownLoadToCPH(ci);
                        if (strsend.Length > 2)
                        {
                            strsend = StatType + strsend.Substring(2);
                        }
                    }
                    else
                    {
                        strsend = CR.GetDownLoad(ci);
                    }
                    if (Model.Channels[j].iXieYi == 1)
                    {
                        a = sendbll.CheckDldCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, strsend, Model.Channels[j].iXieYi);
                    }

                    //SetTextMessage1(k, CarNO, a, Model.Channels[j].sIP, 0);
                    //else
                    //{
                    //    short Ji = Convert.ToInt16(Model.Channels[j].iCtrlID);

                    //    a = axznykt_1.ReadDinnerTime2010znykt_(ref Ji, ref strsend);
                    //    if (a.Length > 1)
                    //    {
                    //        if (CarNO == a.Substring(8, 8))
                    //        {
                    //            a = "0";
                    //        }

                    //    }

                    //}
                    if (a == "0")
                    {
                        if (Model.Channels[j].sIP == "")
                        {
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].iCtrlID + "号机为:" + StatType + CarNumber);
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].iCtrlID + "号机的开始时间为:" + strDateTime);
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].iCtrlID + "号机的结束时间为:" + strDateTime1);
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].iCtrlID + "号机下载成功");
                        }
                        else
                        {
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].sIP + "为:" + StatType + CarNumber);
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].sIP + "的开始时间为:" + strDateTime);
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].sIP + "的结束时间为:" + strDateTime1);
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].sIP + "下载成功");
                        }

                        listBox1.Items.Add("---------------------------------------");
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                    else
                    {
                        if (Model.Channels[j].sIP == "")
                        {
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].iCtrlID + "号机上没有下载");
                        }
                        else
                        {
                            listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].sIP + "上没有下载");
                        }
                        listBox1.Items.Add("---------------------------------------");
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                }
                else
                {
                    //string a = sendbll.CheckDldCard(Model.Channels[j].sIP, Model.Channels[j].iCtrlID, StatType + CarNumber + strDateTime1 + strDateTime + "0000");
                    //SetTextMessage1(k, CarNO, a, Model.Channels[j].sIP, 1);
                    if (Model.Channels[j].sIP == "")
                    {
                        listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].iCtrlID + "号机上未授权");
                        listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].iCtrlID + "号机上没有下载");
                    }
                    else
                    {
                        listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].sIP + "上未授权");
                        listBox1.Items.Add("【" + CarNO + "】在" + Model.Channels[j].sIP + "上没有下载");
                    }
                    listBox1.Items.Add("---------------------------------------");
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
            }

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            dataGridView1_MouseDoubleClick(null, null);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtCardNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<CardIssue> lstIssue;
            List<QueryConditionGroup> lstCondition;

            if (txtCardNo.Text.Length < 3)
            {
                return;
            }

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CardNO", "like", string.Format("%{0}%", txtCardNo.Text));

            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition);

            dataGridView1.IsReadOnly = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ItemsSource = lstIssue;
        }

        private void txtCPH_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<CardIssue> lstIssue;
            List<QueryConditionGroup> lstCondition;

            if (txtCPH.Text.Length < 3)
            {
                return;
            }

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CPH", "like", string.Format("%{0}%", txtCPH.Text));

            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition);

            dataGridView1.IsReadOnly = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ItemsSource = lstIssue;
        }
    }
}