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
using SFMControls;
using ParkingCommunication.CameraSDK.ZNYKT5;

namespace UI.Video
{
    /// <summary>
    /// InAndOut.xaml 的交互逻辑
    /// </summary>
    public partial class InAndOut : SFMControls.WindowBase
    {
        private int m_hLPRClient = 0;

        private VzClientSDK.VZ_OutputConfigInfo m_oOutputInfo = new VzClientSDK.VZ_OutputConfigInfo();

        public InAndOut()
        {
            InitializeComponent();
        }

        public InAndOut(int hLPRClient)
        {
            InitializeComponent();
            m_hLPRClient = hLPRClient;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            cmbBound.Items.Add("2400");
            cmbBound.Items.Add("4800");
            cmbBound.Items.Add("9600");
            cmbBound.Items.Add("19200");
            cmbBound.Items.Add("38400");
            cmbBound.Items.Add("57600");
            cmbBound.Items.Add("115200");

            cmbParity.Items.Add("无校验");
            cmbParity.Items.Add("奇校验");
            cmbParity.Items.Add("偶校验");

            cmbStop.Items.Add("1");
            cmbStop.Items.Add("2");

            cmbDataBit.Items.Add("8");


            cmbBound1.Items.Add("2400");
            cmbBound1.Items.Add("4800");
            cmbBound1.Items.Add("9600");
            cmbBound1.Items.Add("19200");
            cmbBound1.Items.Add("38400");
            cmbBound1.Items.Add("57600");
            cmbBound1.Items.Add("115200");

            cmbParity1.Items.Add("无校验");
            cmbParity1.Items.Add("奇校验");
            cmbParity1.Items.Add("偶校验");

            cmbStop1.Items.Add("1");
            cmbStop1.Items.Add("2");

            cmbDataBit1.Items.Add("8");

            initOutput();
            initDelay();
            initWL();
            initMatch();
            initSerialParam(0);
            initSerialParam(1);
        }

        private void initOutput()
        {
            VzClientSDK.VzLPRClient_GetOutputConfig(m_hLPRClient, ref m_oOutputInfo);

            //识别通过
            chk1.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].switchout1);
            chk2.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].switchout2);
            chk3.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].switchout3);
            chk4.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].switchout4);
            chk5.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].levelout1);
            chk6.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].levelout2);
            chk7.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].rs485out1);
            chk8.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[0].rs485out2);

            //识别不通过
            chk9.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].switchout1);
            chk10.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].switchout2);
            chk11.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].switchout3);
            chk12.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].switchout4);
            chk13.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].levelout1);
            chk14.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].levelout2);
            chk15.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].rs485out1);
            chk16.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[1].rs485out2);

            //无车牌
            chk17.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].switchout1);
            chk18.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].switchout2);
            chk19.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].switchout3);
            chk20.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].switchout4);
            chk21.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].levelout1);
            chk22.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].levelout2);
            chk23.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].rs485out1);
            chk24.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[2].rs485out2);

            //黑名单
            chk25.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].switchout1);
            chk26.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].switchout2);
            chk27.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].switchout3);
            chk28.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].switchout4);
            chk29.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].levelout1);
            chk30.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].levelout2);
            chk31.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].rs485out1);
            chk32.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[3].rs485out2);

            //开关量/电平输入 1
            chk33.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[4].switchout1);
            chk34.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[4].switchout2);
            chk35.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[4].switchout3);
            chk36.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[4].switchout4);
            chk37.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[4].levelout1);
            chk38.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[4].levelout2);

            //开关量/电平输入 2
            chk39.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[5].switchout1);
            chk40.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[5].switchout2);
            chk41.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[5].switchout3);
            chk42.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[5].switchout4);
            chk43.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[5].levelout1);
            chk44.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[5].levelout2);

            //开关量/电平输入 3
            chk45.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[6].switchout1);
            chk46.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[6].switchout2);
            chk47.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[6].switchout3);
            chk48.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[6].switchout4);
            chk49.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[6].levelout1);
            chk50.IsChecked = Convert.ToBoolean(m_oOutputInfo.oConfigInfo[6].levelout2);
        }

        private void initDelay()
        {
            int nDelay = 0;
            VzClientSDK.VzLPRClient_GetTriggerDelay(m_hLPRClient, ref nDelay);
            txtDelay.Text = Convert.ToString(nDelay);
        }

        private void initWL()
        {
            int nType = 0;
            VzClientSDK.VzLPRClient_GetWLCheckMethod(m_hLPRClient, ref nType);
            if (nType == 0)
                rbtOffineAuto.IsChecked = true;
            else if (nType == 1)
                rbtStart.IsChecked = true;
            else if (nType == 2)
                rbtStop.IsChecked = true;
        }

        private void initMatch()
        {
            int nType = 0, nLen = 0;
            bool bIgnore = false;
            VzClientSDK.VzLPRClient_GetWLFuzzy(m_hLPRClient, ref nType, ref nLen, ref bIgnore);
            if (nType == 0)
                rbtAllMatch.IsChecked = true;
            else if (nType == 1)
                rbtPartMatch.IsChecked = true;
            else if (nType == 2)
                rbtFuccyMatch.IsChecked = true;

            if (nLen == 1)
                rbtLen1.IsChecked = true;
            else if (nLen == 2)
                rbtLen2.IsChecked = true;
            else if (nLen == 3)
                rbtLen3.IsChecked = true;

            chkIgnore.IsChecked = bIgnore;
        }

        private void initSerialParam(int nPort)
        {
            if (!(nPort >= 0))
                return;

            //cmbBound.Items.IndexOf
            //cmbSerialPort.SelectedIndex = cmbSerialPort.Items.IndexOf(Convert.ToString(nPort + 1));
            VzClientSDK.VZ_SERIAL_PARAMETER oSerialParam = new VzClientSDK.VZ_SERIAL_PARAMETER();
            VzClientSDK.VzLPRClient_GetSerialParameter(m_hLPRClient, nPort, ref oSerialParam);

            if (nPort == 0)
            {
                cmbBound.SelectedIndex = cmbBound.Items.IndexOf(Convert.ToString(oSerialParam.uBaudRate));
                if (oSerialParam.uParity == 0)
                    cmbParity.SelectedIndex = cmbParity.Items.IndexOf("无校验");
                else if (oSerialParam.uParity == 1)
                    cmbParity.SelectedIndex = cmbParity.Items.IndexOf("奇校验");
                else if (oSerialParam.uParity == 2)
                    cmbParity.SelectedIndex = cmbParity.Items.IndexOf("偶校验");
                cmbStop.SelectedIndex = cmbStop.Items.IndexOf(Convert.ToString(oSerialParam.uStopBit));
                cmbDataBit.SelectedIndex = cmbDataBit.Items.IndexOf(Convert.ToString(oSerialParam.uDataBits));
            }
            else
            {
                cmbBound1.SelectedIndex = cmbBound1.Items.IndexOf(Convert.ToString(oSerialParam.uBaudRate));
                if (oSerialParam.uParity == 0)
                    cmbParity1.SelectedIndex = cmbParity1.Items.IndexOf("无校验");
                else if (oSerialParam.uParity == 1)
                    cmbParity1.SelectedIndex = cmbParity1.Items.IndexOf("奇校验");
                else if (oSerialParam.uParity == 2)
                    cmbParity1.SelectedIndex = cmbParity1.Items.IndexOf("偶校验");
                cmbStop1.SelectedIndex = cmbStop1.Items.IndexOf(Convert.ToString(oSerialParam.uStopBit));
                cmbDataBit1.SelectedIndex = cmbDataBit1.Items.IndexOf(Convert.ToString(oSerialParam.uDataBits));
            }
        }

        private void btnOutSet_Click(object sender, RoutedEventArgs e)
        {
            //识别通过
            m_oOutputInfo.oConfigInfo[0].switchout1 = Convert.ToInt32(chk1.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[0].switchout2 = Convert.ToInt32(chk2.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[0].switchout3 = Convert.ToInt32(chk3.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[0].switchout4 = Convert.ToInt32(chk4.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[0].levelout1 = Convert.ToInt32(chk5.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[0].levelout2 = Convert.ToInt32(chk6.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[0].rs485out1 = Convert.ToInt32(chk7.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[0].rs485out2 = Convert.ToInt32(chk8.IsChecked ?? false);

            //识别不通过
            m_oOutputInfo.oConfigInfo[1].switchout1 = Convert.ToInt32(chk9.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[1].switchout2 = Convert.ToInt32(chk10.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[1].switchout3 = Convert.ToInt32(chk11.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[1].switchout4 = Convert.ToInt32(chk12.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[1].levelout1 = Convert.ToInt32(chk13.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[1].levelout2 = Convert.ToInt32(chk14.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[1].rs485out1 = Convert.ToInt32(chk15.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[1].rs485out2 = Convert.ToInt32(chk16.IsChecked ?? false);

            //无车牌
            m_oOutputInfo.oConfigInfo[2].switchout1 = Convert.ToInt32(chk17.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[2].switchout2 = Convert.ToInt32(chk18.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[2].switchout3 = Convert.ToInt32(chk19.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[2].switchout4 = Convert.ToInt32(chk20.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[2].levelout1 = Convert.ToInt32(chk21.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[2].levelout2 = Convert.ToInt32(chk22.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[2].rs485out1 = Convert.ToInt32(chk23.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[2].rs485out2 = Convert.ToInt32(chk24.IsChecked ?? false);

            //黑名单
            m_oOutputInfo.oConfigInfo[3].switchout1 = Convert.ToInt32(chk25.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[3].switchout2 = Convert.ToInt32(chk26.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[3].switchout3 = Convert.ToInt32(chk27.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[3].switchout4 = Convert.ToInt32(chk28.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[3].levelout1 = Convert.ToInt32(chk29.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[3].levelout2 = Convert.ToInt32(chk30.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[3].rs485out1 = Convert.ToInt32(chk31.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[3].rs485out2 = Convert.ToInt32(chk32.IsChecked ?? false);

            //开关量/电平输入 1
            m_oOutputInfo.oConfigInfo[4].switchout1 = Convert.ToInt32(chk33.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[4].switchout2 = Convert.ToInt32(chk34.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[4].switchout3 = Convert.ToInt32(chk35.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[4].switchout4 = Convert.ToInt32(chk36.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[4].levelout1 = Convert.ToInt32(chk37.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[4].levelout2 = Convert.ToInt32(chk38.IsChecked ?? false);

            //开关量/电平输入 2
            m_oOutputInfo.oConfigInfo[5].switchout1 = Convert.ToInt32(chk39.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[5].switchout2 = Convert.ToInt32(chk40.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[5].switchout3 = Convert.ToInt32(chk41.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[5].switchout4 = Convert.ToInt32(chk42.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[5].levelout1 = Convert.ToInt32(chk43.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[5].levelout2 = Convert.ToInt32(chk44.IsChecked ?? false);

            //开关量/电平输入 3
            m_oOutputInfo.oConfigInfo[6].switchout1 = Convert.ToInt32(chk45.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[6].switchout2 = Convert.ToInt32(chk46.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[6].switchout3 = Convert.ToInt32(chk47.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[6].switchout4 = Convert.ToInt32(chk48.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[6].levelout1 = Convert.ToInt32(chk49.IsChecked ?? false);
            m_oOutputInfo.oConfigInfo[6].levelout2 = Convert.ToInt32(chk50.IsChecked ?? false);


            int nRet = VzClientSDK.VzLPRClient_SetOutputConfig(m_hLPRClient, ref m_oOutputInfo);
            if (nRet != 0)
            {
                MessageBox.Show("设置输出配置失败!");
            }
            else
            {
                MessageBox.Show("设置输出配置成功!");
            }
        }

        private void btnSerialPortSet_Click(object sender, RoutedEventArgs e)
        {
            int nSerialPort = cmbSerialPort.SelectedIndex;
            VzClientSDK.VZ_SERIAL_PARAMETER oSerialParam = new VzClientSDK.VZ_SERIAL_PARAMETER();
            oSerialParam.uBaudRate = Convert.ToUInt32(cmbBound.Text);
            int nParity = cmbParity.SelectedIndex;
            oSerialParam.uParity = Convert.ToUInt32(nParity);
            oSerialParam.uDataBits = Convert.ToUInt32(cmbDataBit.Text);
            oSerialParam.uStopBit = Convert.ToUInt32(cmbStop.Text);
            int nRet = VzClientSDK.VzLPRClient_SetSerialParameter(m_hLPRClient, 0, ref oSerialParam);

            VzClientSDK.VZ_SERIAL_PARAMETER oSerialParam1 = new VzClientSDK.VZ_SERIAL_PARAMETER();
            oSerialParam1.uBaudRate = Convert.ToUInt32(cmbBound1.Text);
            int nParity1 = cmbParity1.SelectedIndex;
            oSerialParam1.uParity = Convert.ToUInt32(nParity1);
            oSerialParam1.uDataBits = Convert.ToUInt32(cmbDataBit1.Text);
            oSerialParam1.uStopBit = Convert.ToUInt32(cmbStop1.Text);
            int nRet1 = VzClientSDK.VzLPRClient_SetSerialParameter(m_hLPRClient, 1, ref oSerialParam1);

            if (nRet1 != 0)
                MessageBox.Show("设置串口参数失败！");
            else
                MessageBox.Show("设置串口参数成功！");
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            int nType = 0;
            if (rbtAllMatch.IsChecked.Value)
                nType = 0;
            else if (rbtPartMatch.IsChecked.Value)
                nType = 1;
            else if (rbtFuccyMatch.IsChecked.Value)
                nType = 2;

            int nLen = 0;
            if (rbtLen1.IsChecked.Value)
                nLen = 1;
            else if (rbtLen2.IsChecked.Value)
                nLen = 2;
            else if (rbtLen3.IsChecked.Value)
                nLen = 3;

            bool bIgnore = chkIgnore.IsChecked.Value;
            int nRet = VzClientSDK.VzLPRClient_SetWLFuzzy(m_hLPRClient, nType, nLen, bIgnore);
            if (nRet != 0)
                MessageBox.Show("设置模糊查询方式失败！");
            else
                MessageBox.Show("设置模糊查询方式成功！");
        }

        private void btnSetWriteList_Click(object sender, RoutedEventArgs e)
        {
            int nType = 0;
            if (rbtOffineAuto.IsChecked.Value)
                nType = 0;
            else if (rbtStart.IsChecked.Value)
                nType = 1;
            else if (rbtStop.IsChecked.Value)
                nType = 2;

            int nRet = VzClientSDK.VzLPRClient_SetWLCheckMethod(m_hLPRClient, nType);
            if (nRet != 0)
                MessageBox.Show("设置白名单启用条件失败！");
            else
                MessageBox.Show("设置白名单启用条件成功！");
        }

        private void btnDelay_Click(object sender, RoutedEventArgs e)
        {
            int nDelay = Convert.ToInt32(txtDelay.Text.Trim());
            int nRet = VzClientSDK.VzLPRClient_SetTriggerDelay(m_hLPRClient, nDelay);
            if (nRet != 0)
                MessageBox.Show("设置触发延迟时间失败！");
            else
                MessageBox.Show("设置触发延迟时间成功！");
        }

        private void rbtFuccyMatch_Click(object sender, RoutedEventArgs e)
        {
            if (rbtFuccyMatch.IsChecked == true)
            {
                bdLenSelect.IsEnabled = true;
            }
            else
            {
                bdLenSelect.IsEnabled = false;
            }
        }


    }
}
