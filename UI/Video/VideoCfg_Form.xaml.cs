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
    /// VideoCfg_Form.xaml 的交互逻辑
    /// </summary>
    public partial class VideoCfg_Form : SFMControls.WindowBase
    {
        private int m_hLPRClient = 0;
        public VideoCfg_Form()
        {
            InitializeComponent();
        }

        public VideoCfg_Form(int hLPRClient)
        {
            InitializeComponent();
            m_hLPRClient = hLPRClient;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            LoadVideoCfg();
            LoadVideoSource();
        }

        private void LoadVideoCfg()
        {
            string strRateval = "512";

            if (m_hLPRClient > 0)
            {
                int nSizeVal = 0, nRateval = 0, nEncodeType = 0, modeval = 0, bitval = 0, ratelist = 0, levelval = 0;

                int ret = VzClientSDK.VzLPRClient_GetVideoFrameSizeIndex(m_hLPRClient, ref nSizeVal);
                if (ret == 0)
                {
                    if (nSizeVal == 0)
                    {
                        cmbFrameSize.SelectedIndex = 1;
                    }
                    else if (nSizeVal == 1)
                    {
                        cmbFrameSize.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbFrameSize.SelectedIndex = nSizeVal;
                    }
                }

                ret = VzClientSDK.VzLPRClient_GetVideoFrameRate(m_hLPRClient, ref nRateval);
                if (ret == 0 && nRateval > 0)
                {
                    cmbFrameRate.SelectedIndex = nRateval - 1;
                }

                ret = VzClientSDK.VzLPRClient_GetVideoEncodeType(m_hLPRClient, ref nEncodeType);
                if (ret == 0)
                {
                    if (nEncodeType == 0)
                    {
                        cmbEncodeType.SelectedIndex = 0;
                        cmbCompressMode.IsEnabled = true;
                    }
                    else
                    {
                        cmbEncodeType.SelectedIndex = 1;
                        cmbCompressMode.IsEnabled = false;
                    }
                }

                ret = VzClientSDK.VzLPRClient_GetVideoCompressMode(m_hLPRClient, ref modeval);
                if (ret == 0)
                {
                    cmbCompressMode.SelectedIndex = modeval;

                    if (modeval == 0)
                    {
                        txtRateval.IsEnabled = true;
                    }
                    else
                    {
                        txtRateval.IsEnabled = false;
                    }
                }

                ret = VzClientSDK.VzLPRClient_GetVideoCBR(m_hLPRClient, ref bitval, ref ratelist);
                if (ret == 0)
                {
                    txtRateval.Text = bitval.ToString();
                }

                ret = VzClientSDK.VzLPRClient_GetVideoVBR(m_hLPRClient, ref levelval);
                if (ret == 0)
                {
                    cmbImgQuality.SelectedIndex = levelval;
                }
            }

            txtRateval.Text = strRateval;
        }

        private void LoadVideoSource()
        {
            int brt = 0, cst = 0, sat = 0, hue = 0;
            int ret = VzClientSDK.VzLPRClient_GetVideoPara(m_hLPRClient, ref brt, ref cst, ref sat, ref hue);
            if (ret == 0)
            {
                sdBright.Value = brt;
                sdContrast.Value = cst;
                sdSaturation.Value = sat;
                sdDefinition.Value = hue;
                lblLD.Content = brt.ToString();
                lblDBD.Content = cst.ToString();
                lblBHD.Content = sat.ToString();
                lblQXD.Content = hue.ToString();
            }

            int frequency = 0;
            ret = VzClientSDK.VzLPRClient_GetFrequency(m_hLPRClient, ref frequency);
            if (ret == 0)
            {
                cmbVideoStandard.SelectedIndex = frequency;
            }

            int shutter = 0;
            ret = VzClientSDK.VzLPRClient_GetShutter(m_hLPRClient, ref shutter);
            if (ret == 0)
            {
                if (shutter == 2)
                {
                    cmbExposureTime.SelectedIndex = 0;
                }
                else if (shutter == 3)
                {
                    cmbExposureTime.SelectedIndex = 1;
                }
                else if (shutter == 4)
                {
                    cmbExposureTime.SelectedIndex = 2;
                }
            }

            int flip = 0;
            ret = VzClientSDK.VzLPRClient_GetFlip(m_hLPRClient, ref flip);
            if (ret == 0)
            {
                cmbImgPos.SelectedIndex = flip;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string sRateVal = txtRateval.Text.ToString();
            int nRate = Int32.Parse(sRateVal);

            if (nRate <= 0 || nRate > 4096)
            {
                MessageBox.Show("码流范围为0-4096，请重新输入！");
                return;
            }

            int nSizeVal = cmbFrameSize.SelectedIndex;
            if (nSizeVal == 0)
            {
                nSizeVal = 1;
            }
            else if (nSizeVal == 1)
            {
                nSizeVal = 0;
            }


            int ret = VzClientSDK.VzLPRClient_SetVideoFrameSizeIndex(m_hLPRClient, nSizeVal);
            if (ret != 0)
            {
                MessageBox.Show("设置分辨率失败，请重试！");
                return;
            }

            int nRateval = cmbFrameRate.SelectedIndex + 1;
            ret = VzClientSDK.VzLPRClient_SetVideoFrameRate(m_hLPRClient, nRateval);
            if (ret != 0)
            {
                MessageBox.Show("设置帧率失败，请重试！");
                return;
            }

            int nEncodeType = (cmbEncodeType.SelectedIndex == 0) ? 0 : 2;
            ret = VzClientSDK.VzLPRClient_SetVideoEncodeType(m_hLPRClient, nEncodeType);
            if (ret != 0)
            {
                MessageBox.Show("设置编码方式失败，请重试！");
                return;
            }

            if (cmbCompressMode.IsEnabled)
            {
                int modeval = cmbCompressMode.SelectedIndex;
                ret = VzClientSDK.VzLPRClient_SetVideoCompressMode(m_hLPRClient, modeval);
                if (ret != 0)
                {
                    MessageBox.Show("设置码流控制失败，请重试！");
                    return;
                }
            }

            int level = cmbImgQuality.SelectedIndex;
            ret = VzClientSDK.VzLPRClient_SetVideoVBR(m_hLPRClient, level);
            if (ret != 0)
            {
                MessageBox.Show("设置图像质量失败，请重试！");
                return;
            }

            if (txtRateval.IsEnabled)
            {
                ret = VzClientSDK.VzLPRClient_SetVideoCBR(m_hLPRClient, nRate);
                if (ret != 0)
                {
                    MessageBox.Show("设置码流上限失败，请重试！");
                    return;
                }
            }

            MessageBox.Show("设置成功！");
        }

        private void btnRecovery_Click(object sender, RoutedEventArgs e)
        {
            int brt = 50;
            sdBright.Value = brt;

            int cst = 40;
            sdContrast.Value = cst;

            int sat = 30;
            sdSaturation.Value = sat;

            int hue = 50;
            sdDefinition.Value = hue;


            VzClientSDK.VzLPRClient_SetVideoPara(m_hLPRClient, brt, cst, sat, hue);


            VzClientSDK.VzLPRClient_SetFrequency(m_hLPRClient, 1);
            cmbVideoStandard.SelectedIndex = 1;

            VzClientSDK.VzLPRClient_SetShutter(m_hLPRClient, 3);
            cmbExposureTime.SelectedIndex = 1;

            VzClientSDK.VzLPRClient_SetFlip(m_hLPRClient, 0);
            cmbImgPos.SelectedIndex = 0;
        }

        private void sdBright_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int brt = Convert.ToInt32(sdBright.Value);
            int cst = Convert.ToInt32(sdContrast.Value);
            int sat = Convert.ToInt32(sdSaturation.Value);
            int hue = Convert.ToInt32(sdDefinition.Value);
            lblLD.Content = brt.ToString();
            lblDBD.Content = cst.ToString();
            lblBHD.Content = sat.ToString();
            lblQXD.Content = hue.ToString();
            VzClientSDK.VzLPRClient_SetVideoPara(m_hLPRClient, brt, cst, sat, hue);
        }

        private void sdContrast_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int brt = Convert.ToInt32(sdBright.Value);
            int cst = Convert.ToInt32(sdContrast.Value);
            int sat = Convert.ToInt32(sdSaturation.Value);
            int hue = Convert.ToInt32(sdDefinition.Value);
            lblLD.Content = brt.ToString();
            lblDBD.Content = cst.ToString();
            lblBHD.Content = sat.ToString();
            lblQXD.Content = hue.ToString();
            VzClientSDK.VzLPRClient_SetVideoPara(m_hLPRClient, brt, cst, sat, hue);
        }

        private void sdSaturation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int brt = Convert.ToInt32(sdBright.Value);
            int cst = Convert.ToInt32(sdContrast.Value);
            int sat = Convert.ToInt32(sdSaturation.Value);
            int hue = Convert.ToInt32(sdDefinition.Value);
            lblLD.Content = brt.ToString();
            lblDBD.Content = cst.ToString();
            lblBHD.Content = sat.ToString();
            lblQXD.Content = hue.ToString();
            VzClientSDK.VzLPRClient_SetVideoPara(m_hLPRClient, brt, cst, sat, hue);
        }

        private void sdDefinition_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int brt = Convert.ToInt32(sdBright.Value);
            int cst = Convert.ToInt32(sdContrast.Value);
            int sat = Convert.ToInt32(sdSaturation.Value);
            int hue = Convert.ToInt32(sdDefinition.Value);
            lblLD.Content = brt.ToString();
            lblDBD.Content = cst.ToString();
            lblBHD.Content = sat.ToString();
            lblQXD.Content = hue.ToString();
            VzClientSDK.VzLPRClient_SetVideoPara(m_hLPRClient, brt, cst, sat, hue);
        }

        private void cmbVideoStandard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int frequency = cmbVideoStandard.SelectedIndex;
            int ret = VzClientSDK.VzLPRClient_SetFrequency(m_hLPRClient, frequency);
        }

        private void cmbExposureTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int shutter = 0;
            int curSel = cmbExposureTime.SelectedIndex;
            if (curSel == 0)
            {
                shutter = 2;
            }
            else if (curSel == 1)
            {
                shutter = 3;
            }
            else if (curSel == 2)
            {
                shutter = 4;
            }

            int ret = VzClientSDK.VzLPRClient_SetShutter(m_hLPRClient, shutter);
        }

        private void cmbImgPos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int flip = cmbImgPos.SelectedIndex;
            int ret = VzClientSDK.VzLPRClient_SetFlip(m_hLPRClient, flip);
        }


    }
}
