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
    /// BaseConfig_From.xaml 的交互逻辑
    /// </summary>
    public partial class BaseConfig_From : SFMControls.WindowBase
    {
        private int m_hLPRClient = 0;
        public BaseConfig_From()
        {
            InitializeComponent();
        }

        public BaseConfig_From(int hLPRClient)
        {
            InitializeComponent();
            m_hLPRClient = hLPRClient;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            getPlateRecType();
            getTrigType();
            getRealTimeResult();
        }

        //获取识别类型
        private void getPlateRecType()
        {
            int uBitsRecType = 0;
            VzClientSDK.VzLPRClient_GetPlateRecType(m_hLPRClient, ref uBitsRecType);
            chkBlue.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_BLUE);
            chkYellow.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_YELLOW);
            chkBlack.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_BLACK);
            chkCoach.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_COACH);
            chkTablets.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_POLICE);
            chkArm.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_AMPOL);
            chkTag.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_ARMY);
            chkHK.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_GANGAO);
            chkEC.IsChecked = Convert.ToBoolean((int)uBitsRecType & VzClientSDK.VZ_LPRC_REC_EMBASSY);
        }

        //获取车牌识别类型
        private void getTrigType()
        {
            int uBitsTrigType = 0;
            VzClientSDK.VzLPRClient_GetPlateTrigType(m_hLPRClient, ref uBitsTrigType);
            chkStableTri.IsChecked = Convert.ToBoolean((int)uBitsTrigType & VzClientSDK.VZ_LPRC_TRIG_ENABLE_STABLE);
            chkVirtualTri.IsChecked = Convert.ToBoolean((int)uBitsTrigType & VzClientSDK.VZ_LPRC_TRIG_ENABLE_VLOOP);
            chkIO1.IsChecked = Convert.ToBoolean((int)uBitsTrigType & VzClientSDK.VZ_LPRC_TRIG_ENABLE_IO_IN1);
            chkIO2.IsChecked = Convert.ToBoolean((int)uBitsTrigType & VzClientSDK.VZ_LPRC_TRIG_ENABLE_IO_IN2);
            chkIO3.IsChecked = Convert.ToBoolean((int)uBitsTrigType & VzClientSDK.VZ_LPRC_TRIG_ENABLE_IO_IN3);
        }

        //获取实时显示
        private void getRealTimeResult()
        {
            VzClientSDK.VZ_LPRC_DRAWMODE drawMode = new VzClientSDK.VZ_LPRC_DRAWMODE();
            int nRet = VzClientSDK.VzLPRClient_GetDrawMode(m_hLPRClient, ref drawMode);
            chkVirtualAndReco.IsChecked = Convert.ToBoolean(drawMode.byDspAddRule);
            chkResult.IsChecked = Convert.ToBoolean(drawMode.byDspAddTarget);
            chkPlatePos.IsChecked = Convert.ToBoolean(drawMode.byDspAddTrajectory);
        }

        //设置识别类型
        private bool setPlateRecType()
        {
            Int32 uBitsRecType = 0;
            uBitsRecType |= chkBlue.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_BLUE : 0;
            uBitsRecType |= chkYellow.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_YELLOW : 0;
            uBitsRecType |= chkBlack.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_BLACK : 0;
            uBitsRecType |= chkCoach.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_COACH : 0;
            uBitsRecType |= chkTablets.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_POLICE : 0;
            uBitsRecType |= chkArm.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_AMPOL : 0;
            uBitsRecType |= chkTag.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_ARMY : 0;
            uBitsRecType |= chkHK.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_GANGAO : 0;
            uBitsRecType |= chkEC.IsChecked.Value ? VzClientSDK.VZ_LPRC_REC_EMBASSY : 0;
            int nRet = VzClientSDK.VzLPRClient_SetPlateRecType(m_hLPRClient, (UInt32)uBitsRecType);
            bool bFuncRet = true;
            if (nRet != 0)
            {
                MessageBox.Show("设置识别类型失败！");
                bFuncRet = false;
            }
            return bFuncRet;
        }

        //设置车牌识别类型
        private bool setTrigType()
        {
            Int32 uBitsTrigType = 0;
            uBitsTrigType |= chkStableTri.IsChecked.Value ? VzClientSDK.VZ_LPRC_TRIG_ENABLE_STABLE : 0;
            uBitsTrigType |= chkVirtualTri.IsChecked.Value ? VzClientSDK.VZ_LPRC_TRIG_ENABLE_VLOOP : 0;
            uBitsTrigType |= chkIO1.IsChecked.Value ? VzClientSDK.VZ_LPRC_TRIG_ENABLE_IO_IN1 : 0;
            uBitsTrigType |= chkIO2.IsChecked.Value ? VzClientSDK.VZ_LPRC_TRIG_ENABLE_IO_IN2 : 0;
            uBitsTrigType |= chkIO3.IsChecked.Value ? VzClientSDK.VZ_LPRC_TRIG_ENABLE_IO_IN3 : 0;
            int nRet = VzClientSDK.VzLPRClient_SetPlateTrigType(m_hLPRClient, Convert.ToUInt32(uBitsTrigType));
            bool bFuncRet = true;
            if (nRet != 0)
            {
                MessageBox.Show("设置输出结果失败！");
                bFuncRet = false;
            }
            return bFuncRet;
        }

        //设置实时显示
        private bool setRealTimeResult()
        {
            VzClientSDK.VZ_LPRC_DRAWMODE drawMode = new VzClientSDK.VZ_LPRC_DRAWMODE();
            drawMode.byDspAddRule = Convert.ToByte(chkVirtualAndReco.IsChecked.Value);
            drawMode.byDspAddTarget = Convert.ToByte(chkResult.IsChecked.Value);
            drawMode.byDspAddTrajectory = Convert.ToByte(chkPlatePos.IsChecked.Value);
            int nRet = VzClientSDK.VzLPRClient_SetDrawMode(m_hLPRClient, ref drawMode);
            bool bFuncRet = true;
            if (nRet != 0)
            {
                MessageBox.Show("设置实时显示失败！");
                bFuncRet = false;
            }
            return bFuncRet;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            bool bRecRet = setPlateRecType();
            bool bTrigRet = setTrigType();
            bool bRealRet = setRealTimeResult();
            if (bRecRet && bTrigRet && bRealRet)
                MessageBox.Show("设置基本配置成功！");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
