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
using System.Runtime.InteropServices;

namespace UI.Video
{
    /// <summary>
    /// OSDSET_Form.xaml 的交互逻辑
    /// </summary>
    public partial class OSDSET_Form : SFMControls.WindowBase
    {
        private int m_hLPRClient = 0;
        VzClientSDK.VZ_LPRC_OSD_Param osdParam = new VzClientSDK.VZ_LPRC_OSD_Param();

        public OSDSET_Form()
        {
            InitializeComponent();
        }

        public OSDSET_Form(int hLPRClient)
        {
            InitializeComponent();
            m_hLPRClient = hLPRClient;
        }


        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            int size = Marshal.SizeOf(osdParam);
            IntPtr intptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(osdParam, intptr, true);
            VzClientSDK.VzLPRClient_GetOsdParam(m_hLPRClient, intptr);
            osdParam = (VzClientSDK.VZ_LPRC_OSD_Param)Marshal.PtrToStructure(intptr, typeof(VzClientSDK.VZ_LPRC_OSD_Param));

            chkDateWay.IsChecked = (int)osdParam.dstampenable == 0 ? false : true;
            chkTimeWay.IsChecked = (int)osdParam.tstampenable == 0 ? false : true;
            chkWordWay.IsChecked = (int)osdParam.nTextEnable == 0 ? false : true;

            txtDatePosX.Text = osdParam.datePosX.ToString();
            txtDatePosY.Text = osdParam.datePosY.ToString();
            txtTimePosX.Text = osdParam.timePosX.ToString();
            txtTimePosY.Text = osdParam.timePosY.ToString();
            txtWordPosX.Text = osdParam.nTextPositionX.ToString();
            txtWordPosY.Text = osdParam.nTextPositionY.ToString();
            txtWordWay.Text = osdParam.overlaytext;

            int nDataFormat = osdParam.dateFormat;
            if (nDataFormat >= 0 && nDataFormat < 3)
            {
                cmbDateWay.SelectedIndex = nDataFormat;
            }
            else
            {
                cmbDateWay.SelectedIndex = 0;
            }
            int nTimeFormat = osdParam.timeFormat;
            if (nTimeFormat == 0 || nTimeFormat == 1)
            {
                cmbTimeWay.SelectedIndex = nTimeFormat;
            }
            else
            {
                cmbTimeWay.SelectedIndex = 0;
            }
            Marshal.FreeHGlobal(intptr);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            osdParam.dstampenable = chkDateWay.IsChecked.Value ? (byte)1 : (byte)0;
            osdParam.tstampenable = chkTimeWay.IsChecked.Value ? (byte)1 : (byte)0;
            osdParam.nTextEnable = chkWordWay.IsChecked.Value ? (byte)1 : (byte)0;

            osdParam.datePosX = Int32.Parse(txtDatePosX.Text);
            osdParam.datePosY = Int32.Parse(txtDatePosY.Text);
            osdParam.timePosX = Int32.Parse(txtTimePosX.Text);
            osdParam.timePosY = Int32.Parse(txtTimePosY.Text);
            osdParam.nTextPositionX = Int32.Parse(txtWordPosX.Text);
            osdParam.nTextPositionY = Int32.Parse(txtWordPosY.Text);

            osdParam.overlaytext = txtWordWay.Text.ToString();

            osdParam.dateFormat = cmbDateWay.SelectedIndex;
            osdParam.timeFormat = cmbTimeWay.SelectedIndex;

            int size = Marshal.SizeOf(osdParam);
            IntPtr intptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(osdParam, intptr, true);
            int temp = VzClientSDK.VzLPRClient_SetOsdParam(m_hLPRClient, intptr);
            Marshal.FreeHGlobal(intptr);

            if (chkIsUpdateTime.IsChecked.Value)
            {
                DateTime dt = Convert.ToDateTime(Convert.ToDateTime(dtDate.Text).ToString("yyyy-MM-dd ") + Convert.ToDateTime(dtTime.Text).ToString("HH:mm:ss"));
                VzClientSDK.VZ_DATE_TIME_INFO TimeInfo = new VzClientSDK.VZ_DATE_TIME_INFO();
                TimeInfo.uYear = (uint)dt.Year;
                TimeInfo.uMonth = (uint)dt.Month;
                TimeInfo.uMDay = (uint)dt.Day;
                TimeInfo.uHour = (uint)dt.Hour;
                TimeInfo.uMin = (uint)dt.Minute;
                TimeInfo.uSec = (uint)dt.Second;

                int timesize = Marshal.SizeOf(TimeInfo);
                IntPtr timeptr = Marshal.AllocHGlobal(timesize);
                Marshal.StructureToPtr(TimeInfo, timeptr, true);
                int timetemp = VzClientSDK.VzLPRClient_SetDateTime(m_hLPRClient, timeptr);
                if (temp == 0 && timetemp == 0)
                {
                    MessageBox.Show("修改成功!");
                }
                else
                {
                    MessageBox.Show("修改失败!");
                }
                Marshal.FreeHGlobal(timeptr);
            }
            else if (temp == 0)
            {
                MessageBox.Show("修改成功!");
            }
            else
            {
                MessageBox.Show("修改失败!");
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
