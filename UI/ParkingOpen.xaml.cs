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

namespace UI
{
    /// <summary>
    /// ParkingOpen.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingOpen : SFMControls.WindowBase
    {
        private ParkingMonitoring.ParkingMonitoringWHandler BinDataHandler;//定义需要调用的委托
        private int modulus = 0;
        GetServiceData gsd = new GetServiceData();
        int m_hLPRClient = 0;
        int m_nSerialHandle = 0;

        ParkingCommunication.VoiceSend voicesend = new ParkingCommunication.VoiceSend(1007, 1005);

        public ParkingOpen()
        {
            InitializeComponent();
        }

        public ParkingOpen(ParkingMonitoring.ParkingMonitoringWHandler _BinDataHandler, int Count)
        {
            BinDataHandler = _BinDataHandler;
            modulus = Count;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(@"Resources\Main0.jpg", UriKind.Relative));

                this.Background = berriesBrush;

                CR.BinDic(gsd.GetCardType());
                List<CardTypeDef>lstCTD = gsd.GetGetCardTypeToTrue();
                cmbCardType.Items.Add("人工开闸");
                for (int i = 0; i < lstCTD.Count;i++ )
                {
                    cmbCardType.Items.Add(lstCTD[i].CardType);
                }
                cmbCardType.SelectedIndex = 0;
                if (Model.iSoftOpenNoPlate == 1)
                {

                    //CR.SendVoice.SendOpen(axznykt_1, Model.Channels[modulus].iCtrlID, Model.Channels[modulus].sIP, 0x0C, 5, m_hLPRClient, Model.Channels[modulus].iXieYi);//开闸

                    voicesend.SendOpen(modulus);
                    //发语音  2015-10-10   
                    if (Model.Channels[modulus].iInOut == 0)
                    {
                        if (Model.bOut485)
                        {
                            System.Threading.Thread.Sleep(50);
                        }
                        voicesend.VoiceDisplay(ParkingCommunication.VoiceType.Welcome, modulus);

                        //CR.SendVoice.VoiceLoad(axznykt_1, Model.Channels[modulus].iCtrlID, Model.Channels[modulus].sIP, "42", m_nSerialHandle, Model.Channels[modulus].iXieYi);
                    }
                    else
                    {
                        if (Model.bOut485)
                        {
                            System.Threading.Thread.Sleep(50);
                        }
                       // CR.SendVoice.LedShow2010znykt(axznykt_1, Model.Channels[modulus].iCtrlID, Model.Channels[modulus].sIP, "0000", m_nSerialHandle, Model.Channels[modulus].iXieYi);

                        voicesend.VoiceDisplay(ParkingCommunication.VoiceType.TempOutOpen, modulus);
                    }

                    gsd.AddYiChang(GetModel());
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":ParkingOpen_Load", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\nParkingOpen_Load", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private CarIn GetModel()
        {
            try
            {
                CarIn model = new CarIn();
                model.CardNO = txtCardNO.Text;
                model.CardType = CR.GetCardType(cmbCardType.Text, 0);
                model.InTime = DateTime.Now;
                model.CPH = cmbCarNumber.Text;
                model.OutTime = DateTime.Now;
                model.SFJE = 0;
                if (Model.Channels[modulus].iInOut == 0)
                {
                    model.InGateName = Model.Channels[modulus].sInOutName;
                    model.OutGateName = cmbKZ.Text;
                }
                else
                {
                    model.InGateName = cmbKZ.Text;
                    model.OutGateName = Model.Channels[modulus].sInOutName;
                }
            
                model.InOperatorCard = "";
                model.InOperator = "";
                model.OutOperatorCard = Model.sUserCard;
                model.OutOperator = Model.sUserName;

                if (Model.Channels[modulus].iInOut == 0)
                {
                    model.InPic = ParkingMonitoring.File;
                    model.OutPic = "";
                }
                else
                {
                    model.InPic = "";
                    model.OutPic = ParkingMonitoring.File;

                }
                model.InUser = Model.sUserName;
                model.YSJE = 0;
                model.Balance = 0;
                model.BigSmall = Model.Channels[modulus].iBigSmall;
                ParkingMonitoring.File = "";
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbKZ.Text == "")
                {
                    this.Title = "输入开闸事由后方能开闸";
                    return;
                }

                if (txtCardNO.Text == "")
                {
                    MessageBox.Show("卡片号码不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (!CR.CheckUpCPH(cmbCarNumber.Text))
                {
                    MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cmbCarNumber.Text + "】会引起车牌数据显示错误", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string strRetun = voicesend.SendOpen(modulus); 
                //strRetun = CR.SendVoice.SendOpen(axznykt_1, Model.PubVal.Channels[modulus].iCtrlID, Model.PubVal.Channels[modulus].sIP, 0x0C, 5, m_hLPRClient, Model.PubVal.Channels[modulus].iXieYi);//开闸

                if (strRetun != "0")
                {
                    System.Windows.Forms.MessageBox.Show("与控制机通讯不通", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    this.Close();
                }

                BinDataHandler(modulus);
              
                if (Model.Channels[modulus].iInOut == 0)
                {
                    if (Model.bOut485)
                    {
                        System.Threading.Thread.Sleep(50);
                    }

                    voicesend.VoiceDisplay(ParkingCommunication.VoiceType.Welcome, modulus);
                    //CR.SendVoice.VoiceLoad(axznykt_1, Model.PubVal.Channels[modulus].iCtrlID, Model.PubVal.Channels[modulus].sIP, "42", m_nSerialHandle, Model.PubVal.Channels[modulus].iXieYi);
                }
                else
                {
                    if (Model.bOut485)
                    {
                        System.Threading.Thread.Sleep(50);
                    }

                    voicesend.VoiceDisplay(ParkingCommunication.VoiceType.TempOutOpen, modulus);
                    //CR.SendVoice.ShowLed55(axznykt_1, Model.PubVal.Channels[modulus].iCtrlID, Model.PubVal.Channels[modulus].sIP, m_nSerialHandle, Model.PubVal.Channels[modulus].iXieYi);
                }

                gsd.AddYiChang(GetModel());
                this.Close();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnOK_Click", ex.Message + "\r\n" + ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\nbtnOK_Click", "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void cmbCarNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<TextBox> textBox = GetChildObjects<TextBox>(cmbCarNumber, typeof(TextBox));
            if (textBox.Count > 0)
            {
                int pos = 0;
                pos = textBox[0].SelectionStart;
                textBox[0].Text = cmbCarNumber.Text.ToUpper();
                textBox[0].Select(pos, 0);
            }
        }

        private void cmbCarNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.I || e.Key == Key.O)
            {
                e.Handled = true;
            }
        }


    }
}
