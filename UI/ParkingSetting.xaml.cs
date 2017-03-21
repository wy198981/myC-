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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParkingModel;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data;
using ParkingCommunication;
using ParkingInterface;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using ParkingCommunication.CameraSDK.ZNYKT13;
using ParkingCommunication.CameraSDK.ZNYKT5;
using ParkingCommunication.CameraSDK.ZNYKT14;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Management;
using Microsoft.Windows.Themes;

namespace UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingSetting : SFMControls.WindowBase
    {
        #region Constructors
        public ParkingSetting()
        {
            InitializeComponent();
        }
        #endregion


        #region Common
        #region Fields
        System.Timers.Timer timer = new System.Timers.Timer();

        /// <summary>
        /// 获取服务数据
        /// </summary>
        GetServiceData gsd = new GetServiceData();

        private ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VZDEV_SERIAL_RECV_DATA_CALLBACK serialRECV = null;

        //用于存放没有权限记录的集合
        List<SFMControls.ButtonSfm> lstRightButton = new List<SFMControls.ButtonSfm>();

        int step = 1;
        #endregion

        #region Methods
        void ShowControl()
        {
            grpCamera.Visibility = Visibility.Hidden;
            grpFreeReason.Visibility = Visibility.Hidden;
            grpMonthChargeRule.Visibility = Visibility.Hidden;
            //grpParkIng.Visibility = Visibility.Hidden;
            grpSerialPort.Visibility = Visibility.Hidden;
            gpID1In1Out.Visibility = Visibility.Hidden;

            if (chkModiTempType.IsChecked == true)
            {
                chkVoiceSF.IsEnabled = true;
            }
            else
            {
                chkVoiceSF.IsEnabled = false;
            }

            if (ckbBillP.IsChecked == true)
            {
                chkBillPrintAuto.IsEnabled = true;
            }
            else
            {
                chkBillPrintAuto.IsEnabled = false;
            }

            if (ckbMore.IsChecked == true)
            {
                spMoreCar.Visibility = Visibility.Visible;
            }
            else
            {
                spMoreCar.Visibility = Visibility.Collapsed;
            }

            if (chkPersonVideo.IsChecked == true)
            {
                dgLane.Columns[6].Visibility = Visibility.Visible;

            }
            else
            {
                dgLane.Columns[6].Visibility = Visibility.Collapsed;
            }

            ckbUpdateJiHao.Visibility = Model.bAppEnable ? Visibility.Visible : Visibility.Collapsed;

            if (Model.bAppEnable)
            {
                ckbChanelTJ.Visibility = Visibility.Visible;
            }
            else
            {
                ckbChanelTJ.Visibility = Visibility.Collapsed;
            }

            if (chkScanPay.IsChecked == true)
            {
                gpbScan.Visibility = Visibility.Visible;
            }
            else
            {
                gpbScan.Visibility = Visibility.Collapsed;
            }

            BinVideoData();
        }

        void UpdateControlFromPubVar()
        {
            #region 保存图像视频设置 (视频卡操作去掉)
            chkEnableNetVideo.IsChecked = Model.iEnableNetVideo == 1 ? true : false;
            if (chkEnableNetVideo.IsChecked == true)
            {
                BinVideoData();
                grpCamera.IsEnabled = true;
            }
            chkVideo4.IsChecked = Convert.ToBoolean(Model.iVideo4);
            chkPersonVideo.IsChecked = Model.iPersonVideo == 1 ? true : false;
            chkZJZP.IsChecked = Model.iIDCapture == 1 ? true : false;
            txtImgSavePath.Text = Model.sImageSavePath;
            chkImageAutoDel.IsChecked = Convert.ToBoolean(Model.iImageAutoDel);
            cboImageSaveDays.Text = Model.iImageSaveDays.ToString();
            cboImageAutoDelTime.Text = Model.iImageAutoDelTime.ToString();

            chkJF.IsChecked = Model.bAppEnable;
            #endregion

            #region 保存收费设置变量
            cboChargeSTD.SelectedIndex = Model.iChargeType;
            cboXsNum.SelectedIndex = Model.iXsdNum - 1;
            chkDiscount.IsChecked = Model.iDiscount == 1 ? true : false;
            chkXsd.IsChecked = Model.iXsd == 1 ? true : false;
            chkFreeCar.IsChecked = Model.iFreeCar == 1 ? true : false;
            chkSetTempMoney.IsChecked = Model.iSetTempMoney == 1 ? true : false;
            if (Model.iYKOverTimeCharge == 1)
            {
                chkYkGqSf.IsChecked = true;
            }
            else if (Model.iYKOverTimeCharge == 2)
            {
                chkYkDay.IsChecked = true;
            }
            else
            {
                chkYkJZRC.IsChecked = true;
            }
            chkTopSF.IsChecked = (Model.iZGXE == 1) ? true : false;
            chkVoiceSF.IsChecked = Model.iModiTempType_VoiceSF == 1 ? true : false;
            chkIDOutCancel.IsChecked = Convert.ToBoolean(Model.iSFCancel);
            chkMonthChargeRule.IsChecked = Convert.ToBoolean(Model.iMonthRule);
            cmbZGType.SelectedIndex = Model.iZGType;
            cmbStopTimeType.SelectedIndex = Model.iZGXEType;
            cmbMothDay.Text = Model.iMothOverDay.ToString();
            chkModiTempType.IsChecked = Convert.ToBoolean(Model.iSetTempCardType);
            chkTempFree.IsChecked = Convert.ToBoolean(Model.iTempFree);
            chkScanPay.IsChecked = Convert.ToBoolean(Model.iOnlinePayEnabled);
            //Model.bSetTempCardType
            #endregion

            #region 保存在线监控设置
            cboLoadTimeInterval.Text = Model.iLoadTimeInterval.ToString();
            chkDisplayTime.IsChecked = Convert.ToBoolean(Model.iDisplayTime);
            //软件控制道闸开关
            chkShowGateState.IsChecked = Model.iShowGateState == 1 ? true : false;
            if (Model.iExitOnlineByPwd == 0)
            {
                chkExitOnlineByPwd.IsChecked = false;
                cboExitPassType.IsEnabled = false;
                cboExitPassType.SelectedIndex = 0;
            }
            else
            {
                chkExitOnlineByPwd.IsChecked = true;
                cboExitPassType.IsEnabled = true;
                cboExitPassType.SelectedIndex = Model.iExitOnlineByPwd - 1;
            }

            chkSoftOpenNoPlate.IsChecked = Convert.ToBoolean(Model.iSoftOpenNoPlate);
            chkCheDui.IsChecked = Convert.ToBoolean(Model.iCheDui);
            chkExHandle.IsChecked = Convert.ToBoolean(Model.iExceptionHandle);
            chkShowBoxCardNum.IsChecked = Convert.ToBoolean(Model.iShowBoxCardNum);

            chkbAutoPrePlate.IsChecked = Convert.ToBoolean(Model.iAutoPrePlate);
            chkForbidSamePos.IsChecked = Convert.ToBoolean(Model.iForbidSamePosition);
            chkCheckPortFirst.IsChecked = Convert.ToBoolean(Model.iCheckPortFirst);

            if (Model.iFullLight == 0)
            {
                chkFullNoIn.IsChecked = false;
                grpParkIng.Visibility = Visibility.Hidden;
            }
            else
            {
                chkFullNoIn.IsChecked = true;
                grpParkIng.Visibility = Visibility.Visible;
            }


            if (Model.iFullLight == 0)
            {
                optFullAllIn.IsChecked = true;
            }
            else if (Model.iFullLight == 1)
            {
                optFullMonth.IsChecked = true;
            }
            else if (Model.iFullLight == 2)
            {
                optFullTemp.IsChecked = true;
            }
            else if (Model.iFullLight == 3)
            {
                optFullStore.IsChecked = true;
            }
            else if (Model.iFullLight == 5)
            {
                optFullAllNoIn.IsChecked = true;
            }

            cboVideoShift.Text = Model.iVideoShiftTime.ToString();

            //SetListBoxSelection(lvwID1In1Out, Model.sID1In1OutCardType);
            #endregion

            #region 保存ID卡功能设置
            chkIDSoftOpen.IsChecked = Model.iIDSoftOpen == 1 ? true : false;
            cboIDInOutLimitSeconds.Text = Model.iInOutLimitSeconds.ToString();
            chkRealTimeDownLoad.IsChecked = Model.iRealTimeDownLoad == 1 ? true : false;
            chkIDSfCancel.IsChecked = Convert.ToBoolean(Model.iIdSfCancel);
            chkICCardDownLoad.IsChecked = Model.iICCardDownLoad == 1 ? true : false;
            chkIDReReadHandle.IsChecked = Convert.ToBoolean(Model.iIdReReadHandle);
            chkIDPlateDownLoad.IsChecked = Convert.ToBoolean(Model.iIdPlateDownLoad);
            chkIDOneInOneOut.IsChecked = Model.iIDOneInOneOut == 1 ? true : false; //ID控制一进一出有问题
            chkIDComfirmOpen.IsChecked = Model.iIDComfirmOpen == 1 ? true : false;
            //SetListBoxSelection(lvwID1In1Out, Model.sID1In1OutCardType);
            //Model.sID1In1OutCardType=
            //Model.sIDComfirmOpenCardType=
            #endregion

            #region 保存语音显示功能
            chkCtrlShowPlate.IsChecked = Convert.ToBoolean(Model.iCtrlShowPlate);
            chkCtrlShowStayTime.IsChecked = Convert.ToBoolean(Model.iCtrlShowStayTime);
            chkCtrlShowCW.IsChecked = Convert.ToBoolean(Model.iCtrlShowCW);
            chkCtrlShowInfo.IsChecked = Convert.ToBoolean(Model.iCtrlShowInfo);
            chkCtrlShowRemainPos.IsChecked = Convert.ToBoolean(Model.iCtrlShowRemainPos);
            cboCtrlVoiceLedVer.SelectedIndex = Model.iCtrlVoiceLedVersion;
            cboCtrlVoiceMode.SelectedIndex = Model.iCtrlVoiceMode;
            cboIDNoticeDay.Text = Model.iIDNoticeDay.ToString();
            #endregion

            #region 外接附加设备设置(包含多功能语音模块)
            ckbBillP.IsChecked = Model.iBillPrint == 1 ? true : false;
            chkBillPrintAuto.IsChecked = Convert.ToBoolean(Model.iBillPrintAuto);
            cboPrintFontSize.Text = Model.iPrintFontSize.ToString();
            //Model.iCarPosLed = carShow.IsChecked ??  false ? 1 : 0; //出入场图片不加水印
            Combo5cw.SelectedIndex = Model.iCarPosCom - 1;
            //Model.iCarPosLedJH = Convert.ToInt32(Combo5cwjh.Text);   //combo5cwjh 车牌识别
            CboLedLen.Text = Model.iCarPosLedLen.ToString();
            //Model.iSFLed = ClientS.IsChecked ??  false ? 1 : 0;   //脱机车牌（车牌识别）
            Combo5sf.SelectedIndex = Model.iSFLedCom - 1;
            CboSfLed.Text = Model.iSFLedType.ToString();
            chkFeeLed.IsChecked = Convert.ToBoolean(Model.iRemainPosLedShowInfo);
            chkShowPlate.IsChecked = Convert.ToBoolean(Model.iRemainPosLedShowPlate);
            chkReloginPrint.IsChecked = Convert.ToBoolean(Model.iReLoginPrint);
            chkBarcode.IsChecked = Convert.ToBoolean(Model.iBarCodePrint);
            //Model.IsCPHAuto = ckbIsCPHAuto.IsChecked ??  false ? 1 : 0;  在线识别月卡不开闸
            #endregion

            #region 其它设置
            chkSetPwd.IsChecked = Convert.ToBoolean(Model.iCtrlSetHasPwd);
            chkQueryName.IsChecked = Convert.ToBoolean(Model.iQueryName);
            //cboStationNo.Text = Model.iWorkstationNo.ToString();  //工作站编号
            //cboParkingNo.Text = Model.iParkingNo.ToString();
            cboArea.Text = Model.strAreaDefault;
            chkFreeCardNoInPlace.IsChecked = Convert.ToBoolean(Model.iFreeCardNoInPlace);
            chkDetailLog.IsChecked = Convert.ToBoolean(Model.iDetailLog);
            chkSumMoneyHide.IsChecked = Convert.ToBoolean(Model.iSumMoneyHide);
            txtCar.Text = Model.iParkTotalSpaces.ToString();
            txtTempCarPlaceNum.Text = Model.iTempCarPlaceNum.ToString();
            TxtMonthCarPlaceNum.Text = Model.iMonthCarPlaceNum.ToString();
            txtMoneyCarPlaceNum.Text = Model.iMoneyCarPlaceNum.ToString();
            cboPromptDelayed.Text = Model.iPromptDelayed.ToString();

            txtWXAppID.Text = Model.strWXAppID;
            txtWXKEY.Text = Model.strWXKEY;
            txtWXMCHID.Text = Model.strWXMCHID;
            txtZFBAppID.Text = Model.strZFBAppID;
            txtZFBPID.Text = Model.strZFBPID;

            if (Model.iOnlyShowThisRemainPos == 1)
            {
                Model.bTempCarPlace = true;
                Model.bMonthCarPlace = false;
                Model.bMoneyCarPlace = false;
                rbtTempCarPlace.IsChecked = true;
            }
            else if (Model.iOnlyShowThisRemainPos == 2)
            {
                Model.bTempCarPlace = false;
                Model.bMonthCarPlace = true;
                Model.bMoneyCarPlace = false;
                rbtMonthCarPlace.IsChecked = true;
            }
            else if (Model.iOnlyShowThisRemainPos == 3)
            {
                Model.bTempCarPlace = false;
                Model.bMonthCarPlace = false;
                Model.bMoneyCarPlace = true;
                rbtMoneyCarPlace.IsChecked = true;
            }
            else
            {
                rbtSumCarPlace.IsChecked = true;
            }

            //2016-08-19
            chkOneKeyShortCut.IsChecked = Convert.ToBoolean(Model.iOneKeyShortCut);
            ckbTempDown.IsChecked = Convert.ToBoolean(Model.iTempDown);
            ckbMinutes.IsChecked = Convert.ToBoolean(Model.iAutoMinutes);
            cboLocalProvince.Text = Model.LocalProvince;
            ckbUpdateJiHao.IsChecked = Convert.ToBoolean(Model.iAutoUpdateJiHao);
            ckbClientS.IsChecked = Model.iSFLed == 1 ? true : false;
            cmbMinutes.Text = Model.iAutoSetMinutes.ToString();
            #endregion

            #region 车牌参数设置
            chkAutoPlateEn.IsChecked = Convert.ToBoolean(Model.iAutoPlateEn);
            if (Model.iAutoPlateDBJD == 0)
            {
                db0.IsChecked = true;
            }
            else if (Model.iAutoPlateDBJD == 1)
            {
                db1.IsChecked = true;
            }
            else
            {
                db2.IsChecked = true;
            }
            //临时车入场
            if (Model.iInAutoOpenModel == 0)
            {
                optInAutoOpenModel0.IsChecked = true;
            }
            else if (Model.iInAutoOpenModel == 1)
            {
                optInAutoOpenModel1.IsChecked = true;
            }
            else
            {
                optInAutoOpenModel2.IsChecked = true;
            }
            //临时车出场
            if (Model.iOutAutoOpenModel == 0)
            {
                optOutAutoOpenModel0.IsChecked = true;
            }
            else if (Model.iOutAutoOpenModel == 1)
            {
                optOutAutoOpenModel1.IsChecked = true;
            }
            else
            {
                optOutAutoOpenModel2.IsChecked = true;
            }
            //月卡入场
            optInAutoOpenMoth0.IsChecked = Model.iInMothOpenModel == 0 ? true : false;
            optInAutoOpenMoth1.IsChecked = optInAutoOpenMoth0.IsChecked == true ? false : true;
            //月卡出场
            optOutAutoOpenMoth0.IsChecked = Model.iOutMothOpenModel == 0 ? true : false;
            optOutAutoOpenMoth1.IsChecked = optOutAutoOpenMoth0.IsChecked == true ? false : true;

            ckbCPHPhoto.IsChecked = Convert.ToBoolean(Model.iCPHPhoto);
            ckbDeleteImg.IsChecked = Model.iAutoDeleteImg == 1 ? true : false;
            cboSameCphDelay.Text = Model.iSameCphDelay; //--------------i
            chkCarShow.IsChecked = Model.iCarPosLed == 1 ? true : false;
            ckb0AutoKZ.IsChecked = Convert.ToBoolean(Model.iAutoKZ);
            ckbFree.IsChecked = Model.iAutoColorSet == 1 ? true : false;
            ckbFree0.IsChecked = Model.iAuto0Set == 1 ? true : false;
            ckbNOCPH.IsChecked = Convert.ToBoolean(Model.iNoCPHAutoKZ);
            ckbTempCanNotInSmall.IsChecked = Convert.ToBoolean(Model.iTempCanNotInSmall);
            chkshibiedazhe.IsChecked = Convert.ToBoolean(Model.iAutoCPHDZ);
            chkCenterCharge.IsChecked = Convert.ToBoolean(Model.iCentralCharge);
            chkOutCharge.IsChecked = Convert.ToBoolean(Model.iOutSF);
            ckbMore.IsChecked = Convert.ToBoolean(Model.iMorePaingCar);

            rbtNoIn.IsChecked = Model.iMorePaingType == 0 ? true : false;
            rbtByTemp.IsChecked = rbtNoIn.IsChecked == true ? false : true;
            combo5cwjh.Text = Model.iCarPosLedJH.ToString();

            cboCphDelay.Text = Model.iCphDelay;
            cboSameCphDelay.Text = Model.iSameCphDelay;
            #endregion
        }

        void UpdateControlFromObject()
        {
            CR.BinDic(gsd.GetCardType());

            cboMonthRuleType.ItemsSource = gsd.GetMonth();

            cboMonthRuleType.SelectedValuePath = "Identifying";
            cboMonthRuleType.DisplayMemberPath = "CardType";
            if (cboMonthRuleType.Items.Count > 0)
                cboMonthRuleType.SelectedIndex = 0;



        }

        //全局变量到数据源
        private void PubVarToDataSource()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            #region 保存图像视频设置 (视频卡操作去掉)
            dic["iEnableNetVideo"] = Model.iEnableNetVideo.ToString();
            dic["bVideo4"] = Model.iVideo4.ToString();
            dic["iPersonVideo"] = Model.iPersonVideo.ToString();
            dic["iIDCapture"] = Model.iIDCapture.ToString();
            dic["sImageSavePath"] = Model.sImageSavePath;
            dic["bImageAutoDel"] = Model.iImageAutoDel.ToString();
            dic["iImageSaveDays"] = Model.iImageSaveDays.ToString();
            dic["iImageAutoDelTime"] = Model.iImageAutoDelTime.ToString();

            dic["bAppEnable"] = (Model.bAppEnable ? 1 : 0).ToString();
            #endregion

            #region 保存收费设置变量
            dic["iChargeType"] = Model.iChargeType.ToString();
            dic["iXsdNum"] = Model.iXsdNum.ToString();
            dic["iDiscount"] = Model.iDiscount.ToString();
            dic["iXsd"] = Model.iXsd.ToString();
            dic["iFreeCar"] = Model.iFreeCar.ToString();
            dic["bSetTempMoney"] = Model.iSetTempMoney.ToString();
            dic["iYKOverTimeCharge"] = Model.iYKOverTimeCharge.ToString();
            dic["iZGXE"] = Model.iZGXE.ToString();
            dic["bModiTempType_VoiceSF"] = Model.iModiTempType_VoiceSF.ToString();
            dic["bSFCancel"] = Model.iSFCancel.ToString();
            dic["bMonthRule"] = Model.iMonthRule.ToString();
            dic["iZGType"] = Model.iZGType.ToString();
            dic["iZGXEType"] = Model.iZGXEType.ToString();
            dic["iMothOverDay"] = Model.iMothOverDay.ToString();
            dic["bSetTempCardType"] = Model.iSetTempCardType.ToString();
            dic["iTempFree"] = Model.iTempFree.ToString();
            
            //Model.bSetTempCardType
            #endregion

            #region 保存在线监控设置
            dic["iLoadTimeInterval"] = Model.iLoadTimeInterval.ToString();
            dic["bDisplayTime"] = Model.iDisplayTime.ToString();
            //软件控制道闸开关
            dic["iShowGateState"] = Model.iShowGateState.ToString();
            dic["iExitOnlineByPwd"] = Model.iExitOnlineByPwd.ToString();
            dic["bSoftOpenNoPlate"] = Model.iSoftOpenNoPlate.ToString();
            dic["bCheDui"] = Model.iCheDui.ToString();
            dic["bExceptionHandle"] = Model.iExceptionHandle.ToString();
            dic["bShowBoxCardNum"] = Model.iShowBoxCardNum.ToString();
            //Model.bOneKeyShortCut = chkOneKeyShortCut.IsChecked ??  false;
            dic["bAutoPrePlate"] = Model.iAutoPrePlate.ToString();
            dic["bForbidSamePosition"] = Model.iForbidSamePosition.ToString();
            dic["bCheckPortFirst"] = Model.iCheckPortFirst.ToString();
            dic["iFullLight"] = Model.iFullLight.ToString();
            dic["iVideoShiftTime"] = Model.iVideoShiftTime.ToString();

            #endregion

            #region 保存ID卡功能设置
            dic["bIDSoftOpen"] = Model.iIDSoftOpen.ToString();
            dic["iInOutLimitSeconds"] = Model.iInOutLimitSeconds.ToString();
            dic["iRealTimeDownLoad"] = Model.iRealTimeDownLoad.ToString();
            dic["bIdSfCancel"] = Model.iIdSfCancel.ToString();
            dic["iICCardDownLoad"] = Model.iICCardDownLoad.ToString();
            dic["bIdReReadHandle"] = Model.iIdReReadHandle.ToString();
            dic["bIdPlateDownLoad"] = Model.iIdPlateDownLoad.ToString();
            dic["iIDOneInOneOut"] = Model.iIDOneInOneOut.ToString(); //ID控制一进一出有问题
            dic["iIDComfirmOpen"] = Model.iIDComfirmOpen.ToString();
            dic["sID1In1OutCardType"] = Model.sID1In1OutCardType;
            //Model.sID1In1OutCardType=
            //Model.sIDComfirmOpenCardType=
            #endregion

            #region 保存语音显示功能
            dic["bCtrlShowPlate"] = Model.iCtrlShowPlate.ToString();
            dic["bCtrlShowStayTime"] = Model.iCtrlShowStayTime.ToString();
            dic["bCtrlShowCW"] = Model.iCtrlShowCW.ToString();
            dic["bCtrlShowInfo"] = Model.iCtrlShowInfo.ToString();
            dic["bCtrlShowRemainPos"] = Model.iCtrlShowRemainPos.ToString();
            dic["iCtrlVoiceLedVersion"] = Model.iCtrlVoiceLedVersion.ToString();
            dic["iCtrlVoiceMode"] = Model.iCtrlVoiceMode.ToString();
            dic["iIDNoticeDay"] = Model.iIDNoticeDay.ToString();
            #endregion

            #region 外接附加设备设置(包含多功能语音模块)
            dic["iBillPrint"] = Model.iBillPrint.ToString();
            dic["bBillPrintAuto"] = Model.iBillPrintAuto.ToString();
            dic["iPrintFontSize"] = Model.iPrintFontSize.ToString();
            //Model.iCarPosLed = carShow.IsChecked ??  false ? 1 : 0; //出入场图片不加水印
            dic["iCarPosCom"] = Model.iCarPosCom.ToString();
            //Model.iCarPosLedJH = Convert.ToInt32(Combo5cwjh.Text);   //combo5cwjh 车牌识别
            dic["iCarPosLedLen"] = Model.iCarPosLedLen.ToString();
            //Model.iSFLed = ClientS.IsChecked ??  false ? 1 : 0;   //脱机车牌（车牌识别）
            dic["iSFLedCom"] = Model.iSFLedCom.ToString();
            dic["iSFLedType"] = Model.iSFLedType.ToString();
            dic["bRemainPosLedShowInfo"] = Model.iRemainPosLedShowInfo.ToString();
            dic["bRemainPosLedShowPlate"] = Model.iRemainPosLedShowPlate.ToString();
            dic["bReLoginPrint"] = Model.iReLoginPrint.ToString();
            dic["bBarCodePrint"] = Model.iBarCodePrint.ToString();
            //Model.IsCPHAuto = ckbIsCPHAuto.IsChecked ??  false ? 1 : 0;  在线识别月卡不开闸
            #endregion

            #region 其它设置
            dic["bCtrlSetHasPwd"] = Model.iCtrlSetHasPwd.ToString();
            dic["bQueryName"] = Model.iQueryName.ToString();
            //dic["iWorkstationNo"] = Model.iWorkstationNo.ToString();  //工作站编号
            //dic["iParkingNo"] = Model.iParkingNo.ToString();
            dic["strAreaDefault"] = Model.strAreaDefault.ToString();
            dic["bFreeCardNoInPlace"] = Model.iFreeCardNoInPlace.ToString();
            dic["bDetailLog"] = Model.iDetailLog.ToString();
            dic["bSumMoneyHide"] = Model.iSumMoneyHide.ToString();
            dic["iParkTotalSpaces"] = Model.iParkTotalSpaces.ToString();
            dic["iTempCarPlaceNum"] = Model.iTempCarPlaceNum.ToString();
            dic["iMonthCarPlaceNum"] = Model.iMonthCarPlaceNum.ToString();
            dic["iMoneyCarPlaceNum"] = Model.iMoneyCarPlaceNum.ToString();
            dic["iOnlyShowThisRemainPos"] = Model.iOnlyShowThisRemainPos.ToString();

            dic["bOneKeyShortCut"] = Model.iOneKeyShortCut.ToString();
            dic["bTempDown"] = Model.iTempDown.ToString();
            dic["bAutoMinutes"] = Model.iAutoMinutes.ToString();
            dic["LocalProvince"] = Model.LocalProvince.ToString();
            dic["bAutoUpdateJiHao"] = Model.iAutoUpdateJiHao.ToString();
            dic["iSFLed"] = Model.iSFLed.ToString();
            dic["iAutoSetMinutes"] = Model.iAutoSetMinutes.ToString();
            dic["iPromptDelayed"] = Model.iPromptDelayed.ToString();

            dic["bOnlinePayEnabled"] = Model.iOnlinePayEnabled.ToString();

            dic["strWXAppID"] = Model.strWXAppID;
            dic["strWXMCHID"] = Model.strWXMCHID;
            dic["strWXKEY"] = Model.strWXKEY;
            dic["strZFBAppID"] = Model.strZFBAppID;
            dic["strZFBPID"] = Model.strZFBPID;

            #endregion

            #region 车牌参数设置
            dic["bAutoPlateEn"] = Model.iAutoPlateEn.ToString();
            dic["iAutoPlateDBJD"] = Model.iAutoPlateDBJD.ToString();
            dic["iInAutoOpenModel"] = Model.iInAutoOpenModel.ToString();
            dic["iOutAutoOpenModel"] = Model.iOutAutoOpenModel.ToString();
            dic["iInMothOpenModel"] = Model.iInMothOpenModel.ToString();
            dic["iOutMothOpenModel"] = Model.iOutMothOpenModel.ToString();
            dic["bCPHPhoto"] = Model.iCPHPhoto.ToString();
            dic["iAutoDeleteImg"] = Model.iAutoDeleteImg.ToString();
            dic["iSameCphDelay"] = Model.iSameCphDelay; //--------------i
            dic["iCarPosLed"] = Model.iCarPosLed.ToString();
            dic["iAutoKZ"] = Model.iAutoKZ.ToString();
            dic["iAutoColorSet"] = Model.iAutoColorSet.ToString();
            dic["iAuto0Set"] = Model.iAuto0Set.ToString();
            dic["bNoCPHAutoKZ"] = Model.iNoCPHAutoKZ.ToString();
            dic["bTempCanNotInSmall"] = Model.iTempCanNotInSmall.ToString();
            dic["bAutoCPHDZ"] = Model.iAutoCPHDZ.ToString();
            dic["bCentralCharge"] = Model.iCentralCharge.ToString();
            dic["bOutSF"] = Model.iOutSF.ToString();
            dic["bMorePaingCar"] = Model.iMorePaingCar.ToString();
            dic["bMorePaingType"] = Model.iMorePaingType.ToString();
            dic["iCarPosLedJH"] = Model.iCarPosLedJH.ToString();
            dic["iCphDelay"] = Model.iCphDelay.ToString();
            dic["iSameCphDelay"] = Model.iSameCphDelay.ToString();
            gsd.SaveParkSysSet(dic, Model.stationID);
            #endregion
        }

        #region 权限分配
        void GetUiAllRightButton(UIElementCollection uiControls)
        {
            foreach (UIElement element in uiControls)
            {
                if (element is SFMControls.ButtonSfm)
                {
                    SFMControls.ButtonSfm current = element as SFMControls.ButtonSfm;
                    if (current.ItemName != "" && current.FormName != "")
                    {
                        lstRightButton.Add(current);
                    }
                }
                else if (element is Grid)
                {
                    GetUiAllRightButton((element as Grid).Children);
                }
                //else if (element is Expander)
                //{
                //    if ((element as Expander).Content is StackPanel)
                //    {
                //        StackPanel sa = (element as Expander).Content as StackPanel;
                //        GetUiAllRightButton(sa.Children);
                //    }
                //    else if ((element as Expander).Content is Grid)
                //    {
                //        Grid sa = (element as Expander).Content as Grid;
                //        GetUiAllRightButton(sa.Children);
                //    }
                //}
                else if (element is StackPanel)
                {
                    GetUiAllRightButton((element as StackPanel).Children);
                }
                //else if (element is ScrollViewer)
                //{
                //    StackPanel sp = (element as ScrollViewer).Content as StackPanel;
                //    GetUiAllRightButton(sp.Children);
                //}
                else if (element is TabControl)
                {
                    foreach (UIElement Pageelment in (element as TabControl).Items)
                    {
                        TabItem tabtemp = (TabItem)Pageelment;

                        Grid gd = tabtemp.Content as Grid;
                        GetUiAllRightButton(gd.Children);
                    }
                }
                //else if (element is GroupBox)
                //{
                //    GroupBox tabtemp = (GroupBox)element;
                //    Grid gd = tabtemp.Content as Grid;
                //    GetUiAllRightButton(gd.Children);
                //}
            }
        }

        void ShowRights()
        {
            //先判断是否有车道查看权限
            List<Rights> lstCheDaoView = gsd.GetRightsByName("车道设置", "CmdView");
            if (lstCheDaoView.Count > 0)
            {
                tbCheDaoSet.IsEnabled = lstCheDaoView[0].CanOperate;
                if (lstCheDaoView[0].CanOperate)
                {
                    foreach (UIElement element in spCheDao.Children)
                    {
                        if (element is SFMControls.ButtonSfm)
                        {
                            SFMControls.ButtonSfm current = element as SFMControls.ButtonSfm;
                            if (current.ItemName != "" && current.FormName != "")
                            {
                                lstCheDaoView = gsd.GetRightsByName(current.FormName, current.ItemName);
                                if (lstCheDaoView.Count > 0)
                                {
                                    current.IsEnabled = lstCheDaoView[0].CanOperate;
                                }
                                else
                                {
                                    current.IsEnabled = false;
                                }
                            }
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                tbCheDaoSet.IsEnabled = false;
            }


            List<Rights> lstCmdCPHDown = gsd.GetRightsByName("脱机车牌下载", "cmdOfflineCPHDownload");
            if (lstCmdCPHDown.Count > 0)
            {
                tbOfflineCPHDownload.Visibility = lstCmdCPHDown[0].CanOperate == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                tbOfflineCPHDownload.Visibility = Visibility.Collapsed;
                long plateManageID = gsd.GetIDByName("车牌管理", "");
                List<RightsItem> lstRI0 = new List<RightsItem>();
                lstRI0.Add(new RightsItem() { FormName = "脱机车牌下载", ItemName = "cmdOfflineCPHDownload", Description = "脱机车牌下载", PID = plateManageID, Category = "车场" });
                gsd.SetRightsItem(lstRI0);
            }



            List<Rights> lstCmdCtrlSearch = gsd.GetRightsByName(cmdCtrlSearch.Header.ToString(), cmdCtrlSearch.Name);

            List<RightsItem> lstRI = new List<RightsItem>();

            if (lstCmdCtrlSearch.Count > 0)
            {
                cmdCtrlSearch.IsEnabled = lstCmdCtrlSearch[0].CanOperate;
            }
            else
            {
                long sysManageID = gsd.GetIDByName("系统管理", "");
                cmdCtrlSearch.IsEnabled = false;
                lstRI.Add(new RightsItem() { FormName = cmdCtrlSearch.Header.ToString(), ItemName = cmdCtrlSearch.Name, Category = "车场", Description = cmdCtrlSearch.Header.ToString(), PID = sysManageID });
            }

            //
            long parkID = gsd.GetIDByName("车场管理", "");
            List<Rights> lstCmdCtrlSet = gsd.GetRightsByName(CmdView.Header.ToString(), CmdView.Name);
            if (lstCmdCtrlSet.Count > 0)
            {
                CmdView.IsEnabled = lstCmdCtrlSet[0].CanOperate;
            }
            else
            {
                CmdView.IsEnabled = false;
                lstRI.Add(new RightsItem() { FormName = CmdView.Header.ToString(), ItemName = CmdView.Name, Category = "车场", Description = CmdView.Header.ToString(), PID = parkID });
            }

            //
            List<Rights> lstCmdOfflineManage = gsd.GetRightsByName(cmdOfflineManage.Header.ToString(), cmdOfflineManage.Name);
            if (lstCmdOfflineManage.Count > 0)
            {
                cmdOfflineManage.IsEnabled = lstCmdOfflineManage[0].CanOperate;
            }
            else
            {
                cmdOfflineManage.IsEnabled = false;
                lstRI.Add(new RightsItem() { FormName = cmdOfflineManage.Header.ToString(), ItemName = cmdOfflineManage.Name, Category = "车场", Description = cmdOfflineManage.Header.ToString(), PID = parkID });
            }

            //
            List<Rights> lstCmdUdiskOperator = gsd.GetRightsByName(cmdUdiskOperator.Header.ToString(), cmdUdiskOperator.Name);
            if (lstCmdUdiskOperator.Count > 0)
            {
                cmdUdiskOperator.IsEnabled = lstCmdUdiskOperator[0].CanOperate;
            }
            else
            {
                cmdUdiskOperator.IsEnabled = false;
                lstRI.Add(new RightsItem() { FormName = cmdUdiskOperator.Header.ToString(), ItemName = cmdUdiskOperator.Name, Category = "车场", Description = cmdUdiskOperator.Header.ToString(), PID = parkID });
            }





            //免费原因设置
            List<Rights> lstFreeView = gsd.GetRightsByName(btnSelectFreeReason.FormName, btnSelectFreeReason.ItemName);
            if (lstFreeView.Count > 0)
            {
                btnSelectFreeReason.IsEnabled = lstFreeView[0].CanOperate;
            }
            else
            {
                btnSelectFreeReason.IsEnabled = false;
            }

            //网络摄像机设置
            List<Rights> lstVideoView = gsd.GetRightsByName(cmdSelectNetCameraSet.FormName, cmdSelectNetCameraSet.ItemName);
            if (lstVideoView.Count > 0)
            {
                cmdSelectNetCameraSet.IsEnabled = lstVideoView[0].CanOperate;
            }
            else
            {
                cmdSelectNetCameraSet.IsEnabled = false;
            }

            //月卡收费设置
            List<Rights> lstMonthView = gsd.GetRightsByName(btnSelectMonthRule.FormName, btnSelectMonthRule.ItemName);
            if (lstMonthView.Count > 0)
            {
                btnSelectMonthRule.IsEnabled = lstMonthView[0].CanOperate;
            }
            else
            {
                btnSelectMonthRule.IsEnabled = false;
            }

            if (lstRI.Count > 0)
            {
                gsd.SetRightsItem(lstRI);
            }
        }

        void ShowRight0(GroupBox gb)
        {
            gb.Visibility = Visibility.Visible;
            List<SFMControls.ButtonSfm> lstBtn = new List<SFMControls.ButtonSfm>();
            List<RightsItem> lstRI = new List<RightsItem>();
            long ID = gsd.GetIDByName(gb.Header.ToString(), "CmdView");
            foreach (UIElement element in (gb.Content as Grid).Children)
            {
                if (element is SFMControls.ButtonSfm)
                {
                    SFMControls.ButtonSfm current = element as SFMControls.ButtonSfm;
                    if (current.ItemName != "" && current.FormName != "")
                    {
                        lstBtn.Add(current);
                    }
                }
            }

            if (lstBtn.Count > 0)
            {
                foreach (var v in lstRightButton)
                {
                    List<Rights> lstRs = gsd.GetRightsByName(v.FormName, v.ItemName);
                    if (lstRs == null || lstRs.Count == 0)
                    {
                        v.IsEnabled = false;
                        lstRI.Add(new RightsItem() { FormName = v.FormName, ItemName = v.ItemName, Category = "车场", Description = v.Content.ToString(), PID = ID });
                    }
                    else
                    {
                        v.IsEnabled = lstRs[0].CanOperate;
                    }
                }
            }

            if (lstRI.Count > 0)
            {
                gsd.SetRightsItem(lstRI);
            }
        }

        #endregion
        #endregion

        #region Events
        #region Windows
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ImageBrush berriesBrush = new ImageBrush();
                //berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));
                //this.Background = berriesBrush;

                for (int i = 0; i < 128; i++)
                    combo5cwjh.Items.Add(i.ToString());

                BinModel.lstOpenType.Clear();
                BinModel.lstOpenType.Add("临时卡确定开闸");
                BinModel.lstOpenType.Add("全部确定开闸");
                BinModel.lstOpenType.Add("全部自动开闸");
                BinModel.lstOpenType.Add("临免确定开闸");
                BinModel.lstOpenType.Add("确定开闸并吞卡");
                BinModel.lstOpenType.Add("自动开闸并吞卡");
                BinModel.lstOpenType.Add("自动吞卡确定开闸");
                BinModel.lstOpenType.Add("读卡或识别开闸");
                BinModel.lstOpenType.Add("读卡加识别开闸");

                BinModel.lstInOut.Clear();
                BinModel.lstInOut.Add("入口车道");
                BinModel.lstInOut.Add("出口车道");
                BinModel.lstInOut.Add("中央收费");
                BinModel.lstInOut.Add("出口吞卡");

                BinModel.lstCtrlNumber.Clear();
                for (int i = 1; i < 31; i++)
                {
                    BinModel.lstCtrlNumber.Add(i);
                }

                BinModel.lstBigSmall.Clear();
                BinModel.lstBigSmall.Add("大车场");
                BinModel.lstBigSmall.Add("小车场");

                BinModel.lstXieYi.Clear();
                BinModel.lstXieYi.Add("485");
                BinModel.lstXieYi.Add("TCP");

                BinModel.lstVideoType.Clear();
                if (Model.bAppEnable)
                {
                    BinModel.lstVideoType.Add("ZNYKT15");
                    BinModel.lstVideoType.Add("ZNYKT11");
                }
                else
                {
                    //BinModel.lstVideoType.Add("ZNYKTY4");
                    BinModel.lstVideoType.Add("ZNYKT5");
                    BinModel.lstVideoType.Add("ZNYKT10");
                    BinModel.lstVideoType.Add("ZNYKT11");
                    BinModel.lstVideoType.Add("ZNYKT14");
                    BinModel.lstVideoType.Add("ZNYKT15");
                }

                lvwID1In1Out.ItemsSource = gsd.GetIDCardType();

                dgLane.PreviewMouseLeftButtonUp += dgLane_PreviewMouseLeftButtonUp;

                BindCheDao();
                BindChargeCheDao();
                gsd.DataSourceToPubVar();
                UpdateControlFromPubVar();
                UpdateControlFromObject();

                //2016-11-28
                Model.bOut485 = !Model.bIsKZB;
                Model.bVideoCamera = !Model.bIsKZB;
                Model.iAutoUpdateJiHao = Convert.ToInt32(!Model.bIsKZB);


                //czh 2016-10-25
                InitSearchDevice();
                InitDeviceSetting();
                InitOfflineSetIP();
                InitUDisk();

                int ret = (new Request()).KeppAlive(Model.token);
                if (ret / 1000 > 0)
                {
                    int time = ret / 1000;
                    if (time <= Model.NetDelayey)
                    {
                        MessageBox.Show("心跳频率小于网络延时，请联系管理员", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.Close();
                    }
                    else
                    {
                        timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                        timer.Interval = ret - Model.NetDelayey * 1000;
                        timer.Start();
                    }
                }

                //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                //timer.Interval = 30000;
                //timer.Start();

                ShowControl();

                //if (Model.isStartGuide)
                //{
                //    var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                //    CR.SetWindowLong(hwnd, CR.GWL_STYLE, CR.GetWindowLong(hwnd, CR.GWL_STYLE) & ~CR.WS_SYSMENU);
                //    btnOK.Visibility = Visibility.Hidden;
                //    btnCancel.Visibility = Visibility.Visible;
                //    tbPark.SelectedIndex = 1;
                //    btnBack.IsEnabled = false;
                //    chkEnableNetVideo.IsEnabled = true;
                //    grpCamera.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    btnBack.Visibility = Visibility.Hidden;
                //    btnNext.Visibility = Visibility.Hidden;
                //}

                ShowRights();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "Window_Loaded", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //GetUiAllRightButton(Canvas1.Children);
            //showRights();

            InitOfflineCPHSet();
        }




        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //(new Request()).LogOut(Model.token);
            //System.Environment.Exit(0);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //模拟的做一些耗时的操作
            int ret = (new Request()).KeppAlive(Model.token);
        }
        #endregion
        #endregion

        #endregion


        #region 向导设置
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            step--;
            if (step == 1)
            {
                btnCancel.Visibility = Visibility.Visible;
                tbPark.SelectedIndex = 1;
                btnBack.IsEnabled = false;
            }
            if (step == 2)
            {
                btnBack.IsEnabled = true;
                tbPark.SelectedIndex = 8;
            }
            else if (step == 3)
            {
                tbPark.SelectedIndex = 0;
            }
            else if (step == 4)
            {
                //btnOK_Click(null, null);
                tbPark.SelectedIndex = 12;
                btnNext.Content = "下一步";
            }
            else if (step == 5)
            {
                tbPark.SelectedIndex = 11;
                tcControlSet.SelectedIndex = 1;
                btnNext.Content = "完成";

            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            step++;
            if (step == 2)
            {
                btnCancel.Visibility = Visibility.Hidden;
                btnBack.IsEnabled = true;
                tbPark.SelectedIndex = 8;
            }
            else if (step == 3)
            {
                tbPark.SelectedIndex = 0;
            }
            else if (step == 4)
            {
                btnOK_Click(null, null);
                tbPark.SelectedIndex = 12;
            }
            else if (step == 5)
            {
                tbPark.SelectedIndex = 11;
                tcControlSet.SelectedIndex = 1;
                btnNext.Content = "完成";
            }
            else if (step == 6)
            {
                this.DialogResult = true;
                this.Close();
            }
        }
        #endregion


        #region 车道设置
        #region Fields
        /// <summary>
        /// 数据源(车道设置)
        /// </summary>
        ObservableCollection<CheDaoSet> cds = new ObservableCollection<CheDaoSet>();

        List<CheDaoSet> lstCDS = new List<CheDaoSet>();

        /// <summary>
        /// 记录编辑前后的列值
        /// </summary>
        object preValue, aftValue = null;
        #endregion

        #region Methods
        void BindCheDao()
        {
            lstCDS = gsd.GetChannelSet(Model.stationID);
            cds = new ObservableCollection<CheDaoSet>(lstCDS);
            dgLane.DataContext = cds;
            dgLane.SelectedIndex = dgLane.Items.Count - 1;

            dcboPersonID.ItemsSource = gsd.SelectVideoAllIP();
            dcboPersonID.DisplayMemberPath = "VideoIP";
            dcboPersonID.SelectedValuePath = "ID";

        }

        T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                    child = GetVisualChild<T>(v);
                if (child != null)
                    break;
            }
            return child;
        }
        #endregion

        #region Events
        private void btnAddCheDao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dgLane.SelectedIndex = -1;
                cds.Add(new CheDaoSet
                {
                    StationID = Model.stationID,
                    InOut = 1,
                    CtrlNumber = cds.Count > 0 ? (cds[cds.Count - 1].CtrlNumber + 1) : 1,
                    OpenID = cds.Count > 0 ? (cds[cds.Count - 1].CtrlNumber + 1) : 1,
                    InOutName = "出口车道" + (cds.Count > 0 ? (cds[cds.Count - 1].CtrlNumber + 1) : 1).ToString(),
                    CameraIP = "",
                    OpenType = 7,
                    BigSmall = 0,
                    CheckPortID = 0,
                    OnLine = 1,
                    TempOut = 0,
                    HasOutCard = 0,
                    XieYi = 1,
                    IP = "192.168.168.168"
                });

                dgLane.SelectedIndex = dgLane.Items.Count - 1;
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnAddCheDao_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnAddCheDao_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteCheDao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgLane.Items.Count > 0)
                {
                    int index = dgLane.SelectedIndex;
                    long ID = 0;
                    if (index == -1)
                    {
                        if (cds.Count > 0)
                            ID = cds[cds.Count - 1].ID;
                    }
                    else
                    {
                        ID = (cds[index] as CheDaoSet).ID;
                    }
                    if (ID != 0)
                    {
                        int count = gsd.DeleteChannelSet(ID);
                        if (count > 0)
                        {
                            BindCheDao();
                        }
                        else
                        {
                            if (cds.Count > 0)
                            {
                                cds.RemoveAt(cds.Count - 1);
                            }
                        }
                    }
                    else
                    {
                        if (cds.Count > 0)
                        {
                            if (index == -1)
                            {
                                cds.RemoveAt(cds.Count - 1);
                            }
                            else
                            {
                                cds.RemoveAt(index);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnDeleteCheDao_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnDeleteCheDao_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgLane_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = sender as DataGrid;
                if (e.AddedItems != null && e.AddedItems.Count > 0)
                {
                    // find row for the first selected item     
                    DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]);
                    if (row != null)
                    {
                        DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);
                        // find grid cell object for the cell with index 0       
                        DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(0) as DataGridCell;
                        if (cell != null)
                        {
                            //Console.WriteLine(((TextBlock)cell.Content).Text);
                            MessageBox.Show(((TextBlock)cell.Content).Text, "提示");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgLane_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dgLane_SelectionChanged", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgLane_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                string typeValue = e.Column.GetType().ToString();

                if (typeValue == "System.Windows.Controls.DataGridComboBoxColumn")
                {
                    preValue = (e.Column.GetCellContent(e.Row) as ComboBox).Text;
                }
                else if (typeValue == "System.Windows.Controls.DataGridTextColumn")
                {
                    preValue = (e.Column.GetCellContent(e.Row) as TextBlock).Text;
                }
                else if (typeValue == "System.Windows.Controls.DataGridCheckBoxColumn")
                {
                    preValue = (e.Column.GetCellContent(e.Row) as CheckBox).IsChecked;
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgLane_BeginningEdit", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dgLane_BeginningEdit", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgLane_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                string typeValue = e.Column.GetType().ToString();

                if (typeValue == "System.Windows.Controls.DataGridComboBoxColumn")
                {
                    aftValue = (e.EditingElement as ComboBox).Text;
                }
                else if (typeValue == "System.Windows.Controls.DataGridTextColumn")
                {
                    aftValue = (e.EditingElement as TextBox).Text;
                }
                else if (typeValue == "System.Windows.Controls.DataGridCheckBoxColumn")
                {
                    aftValue = (e.EditingElement as CheckBox).IsChecked;
                }

                if (e.Column.Header.ToString() == "车像抓拍")
                {
                    cds[dgLane.SelectedIndex].CameraIP = aftValue.ToString();;

                    if (Model.bAppEnable)
                    {
                        cds[dgLane.SelectedIndex].IP = aftValue.ToString();
                    }
                }

                if (preValue != aftValue)
                {
                    CheDaoSet cds0 = dgLane.SelectedItem as CheDaoSet;
                    string headValue = e.Column.Header.ToString();
                    if (headValue == "控制机号")
                    {
                        cds0.CtrlNumber = Convert.ToInt32(aftValue.ToString());
                        cds[dgLane.SelectedIndex].InOutName = BinModel.lstInOut[cds0.InOut] + aftValue.ToString();
                    }
                    else if (headValue == "车道类型")
                    {
                        cds0.InOut = BinModel.lstInOut.IndexOf(aftValue.ToString());
                        cds[dgLane.SelectedIndex].InOutName = aftValue.ToString() + cds0.CtrlNumber;
                    }
                    else if (headValue == "通讯协议")
                    {
                        if (aftValue.ToString() == "485")
                        {
                            dgLane.Columns[dgLane.Columns.Count - 1].Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            dgLane.Columns[dgLane.Columns.Count - 1].Visibility = Visibility.Visible;
                        }
                    }
                   
                    else if (headValue == "人像抓拍")
                    {
                        cds[dgLane.SelectedIndex].PersonVideo = BinModel.lstPersonVideo.IndexOf(aftValue.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgLane_CellEditEnding", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dgLane_CellEditEnding", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

     

        void dgLane_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (null == dg || null == dg.CurrentItem || null == e.OriginalSource ||
              e.OriginalSource is ScrollViewer || e.OriginalSource is DataGridColumnHeader || e.OriginalSource is CheckBox || e.OriginalSource is BulletChrome)
            {
                return;
            }

            FrameworkElement item = dg.CurrentColumn.GetCellContent(dg.CurrentItem);
            string type = item.GetType().ToString();

            if (type == "System.Windows.Controls.CheckBox")
            {
                CheckBox cb = item as CheckBox;
                cb.IsChecked = !cb.IsChecked;
            }
            else
            {
                dgLane.BeginEdit();
            }
            
        }

        #endregion
        #endregion


        #region 参数设置
        #region Fields
        //!!!
        string strData = "";

        private Hid usbHid = new Hid();

        //构造一个对称算法
        private SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
        #endregion

        #region Methods
        void BindChargeCheDao()
        {
            dgvChargeCheDao.DataContext = cds;
            dgvChargeCheDao.PreviewMouseLeftButtonUp += DataGrid_PreviewMouseLeftButtonUp;
        }

        /// <summary>
        /// 打印机发送
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="byteJH"></param>
        /// <param name="iType"></param>
        /// <param name="sBaud"></param>
        /// <param name="sValue1"></param>
        /// <param name="sValue2"></param>
        /// <param name="sValue3"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public bool BarPrintBaudSet(string IP, byte byteJH, int iType, string sBaud, string sValue1, string sValue2, string sValue3, int Flag)
        {
            int iBaud = 0;
            string strBin;
            byte[] MyStr = new byte[82];
            switch (sBaud)
            {
                case "4800":
                    iBaud = 5;
                    break;
                case "9600":
                    iBaud = 4;
                    break;
                case "19200":
                    iBaud = 3;
                    break;
                case "38400":
                    iBaud = 2;
                    break;
                case "57600":
                    iBaud = 1;
                    break;
                case "115200":
                    iBaud = 0;
                    break;
            }
            strBin = Convert.ToInt32(Convert.ToString(iType, 2)).ToString("0000");
            strBin += Convert.ToInt32(Convert.ToString(iBaud, 2)).ToString("0000");

            MyStr[0] = Config.byteHead[Config.HeadIndex];
            MyStr[1] = byteJH;
            MyStr[2] = byteJH;
            MyStr[3] = 0x8E;
            MyStr[4] = 0x8E;
            MyStr[5] = Convert.ToByte(Convert.ToInt32(strBin, 2));

            for (int i = 6; i < 81; i++)
            {
                MyStr[i] = 0x20;
            }
            MyStr[6] = 0xAA;
            byte[] array1;
            array1 = System.Text.Encoding.Default.GetBytes(sValue1);
            for (int i = 0; i < array1.Length; i++)
            {
                MyStr[7 + i] = array1[i];
            }
            MyStr[56] = 0xAA;
            byte[] array2;
            array2 = System.Text.Encoding.Default.GetBytes(sValue3);
            for (int i = 0; i < array2.Length; i++)
            {
                MyStr[57 + i] = array2[i];
            }
            string str = string.Empty;
            if (MyStr != null)
            {
                for (int i = 5; i < MyStr.Length - 1; i++)
                {
                    str += MyStr[i].ToString("X2");
                }
            }
            MyStr[81] = CR.YHXY(str);
            SedBll sendbll = new SedBll(IP, 1007, 1005);
            string strRst = sendbll.BarCodePrint(MyStr, Flag);
            if (strRst == "0")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 加载控制板参数指令
        /// </summary>
        /// <returns></returns>
        private string GetStrLoad()
        {
            try
            {


                string SStr1 = "";
                string SStr2 = "";
                string SStr3 = "";
                string SStr4 = "";

                if (rbIN.IsChecked == true)
                {
                    SStr1 += "1";
                }
                else
                {
                    SStr1 += "0";
                }
                for (int i = 0; i < 7; i++)  //chkb 1-7  中6 保留
                {
                    if (i == 5)
                    {
                        if (ckb6.IsChecked == true)
                        {
                            SStr1 = "0" + SStr1;
                        }
                        else
                        {
                            SStr1 = "1" + SStr1;
                        }
                    }
                    else
                        SStr1 = "1" + SStr1;
                }

                if (ckb8.IsChecked == true)
                {
                    SStr2 = "0" + SStr2;
                }
                else
                {
                    SStr2 = "1" + SStr2;
                }


                for (int i = 0; i < 7; i++)
                {
                    if (i == 2)
                    {
                        if (ckb11.IsChecked == true)
                        {
                            SStr2 = "0" + SStr2;
                        }
                        else
                        {
                            SStr2 = "1" + SStr2;
                        }
                    }
                    else
                        SStr2 = "1" + SStr2; //chkb 9-15
                }

                //SStr3(包含 17 20)
                for (int i = 0; i < 8; i++)
                {
                    if (i == 1)
                    {
                        if (ckb17.IsChecked == true)
                        {
                            SStr3 = "0" + SStr3;
                        }
                        else
                        {
                            SStr3 = "1" + SStr3;
                        }
                    }
                    else if (i == 4)
                    {
                        if (ckb20.IsChecked == true)
                        {
                            SStr3 = "0" + SStr3;
                        }
                        else
                        {
                            SStr3 = "1" + SStr3;
                        }
                    }
                    else
                    {
                        SStr3 = "1" + SStr3;
                    }
                }

                //SStr3(包含27)
                for (int i = 0; i < 8; i++)
                {
                    if (i == 3)
                    {
                        if (ckb27.IsChecked == true)
                        {
                            SStr4 = "0" + SStr4;
                        }
                        else
                        {
                            SStr4 = "1" + SStr4;
                        }
                    }
                    else
                    {
                        SStr4 = "1" + SStr4;
                    }
                }

                //for (int i = 0; i < 16; i++)
                //{
                //    if (i == 1)
                //    {
                //        if (ckb17.IsChecked == true)
                //        {
                //            SStr3 = "0" + SStr3;
                //        }
                //        else
                //        {
                //            SStr3 = "1" + SStr3;
                //        }
                //    }
                //    else if (i == 4)
                //    {
                //        if (ckb20.IsChecked == true)
                //        {
                //            SStr3 = "0" + SStr3;
                //        }
                //        else
                //        {
                //            SStr3 = "1" + SStr3;
                //        }
                //    }
                //    else if (i == 11)
                //    {
                //        if (ckb27.IsChecked == true)
                //        {
                //            SStr3 = "0" + SStr3;
                //        }
                //        else
                //        {
                //            SStr3 = "1" + SStr3;
                //        }
                //    }
                //    else
                //    {
                //        SStr3 = "1" + SStr3;  //chkb 16-31 17 20 27
                //    }
                //}

                //chk24--------31

                string strSum1 = string.Format("{0:X}", Convert.ToInt32(SStr1.Substring(0, 4), 2));
                strSum1 += string.Format("{0:X}", Convert.ToInt32(SStr1.Substring(4, 4), 2));
                string strSum2 = string.Format("{0:X}", Convert.ToInt32(SStr2.Substring(0, 4), 2));
                strSum2 += string.Format("{0:X}", Convert.ToInt32(SStr2.Substring(4, 4), 2));
                string strSum3 = string.Format("{0:X}", Convert.ToInt32(SStr3.Substring(0, 4), 2));
                strSum3 += string.Format("{0:X}", Convert.ToInt32(SStr3.Substring(4, 4), 2));
                string strSum4 = string.Format("{0:X}", Convert.ToInt32(SStr4.Substring(0, 4), 2));
                strSum4 += string.Format("{0:X}", Convert.ToInt32(SStr4.Substring(4, 4), 2));
                return strSum1 + strSum2 + strSum3 + strSum4;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void GetStrRead(string str, string strData)
        {
            try
            {
                if (strData.Length > 1)
                {
                    string str1 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(17, 1), 16), 2)).ToString("0000");
                    string str2 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(16, 1), 16), 2)).ToString("0000");
                    string str3 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(13, 1), 16), 2)).ToString("0000");

                    if (str3.Substring(3, 1) == "1")
                    {
                        chb0AutoKZ.IsChecked = false;
                    }
                    else
                    {
                        chb0AutoKZ.IsChecked = true;
                    }

                    if (str1.Substring(3, 1) == "1")
                    {
                        ckbQAutoKZ.IsChecked = false;
                    }
                    else
                    {
                        ckbQAutoKZ.IsChecked = true;
                    }

                    if (str1.Substring(2, 1) == "1")
                    {
                        ckbQTmpAutoKZ.IsChecked = false;  //启用临时车脱机功能
                    }
                    else
                    {
                        ckbQTmpAutoKZ.IsChecked = true;
                    }

                    if (str1.Substring(1, 1) == "1")
                    {
                        ckbTmpChinese.IsChecked = false;
                    }
                    else
                    {
                        ckbTmpChinese.IsChecked = true;
                    }

                    if (str1.Substring(0, 1) == "1")
                    {
                        ckbMthNo.IsChecked = false;
                    }
                    else
                    {
                        ckbMthNo.IsChecked = true;
                    }

                    if (str2.Substring(0, 1) == "1")
                    {
                        ckbChanelTJ.IsChecked = false;
                    }
                    else
                    {
                        ckbChanelTJ.IsChecked = true;
                    }

                }
                if (str.Length == 8)
                {
                    string strSum = "";
                    foreach (char c in str)
                    {
                        strSum += ConvertToBin(c);
                    }
                    string str1 = strSum.Substring(0, 8);
                    string str2 = strSum.Substring(8, 8);
                    string str3 = strSum.Substring(16, 8);
                    string str4 = strSum.Substring(24, 8);

                    if (str1.Substring(7, 1) == "1")
                    {
                        rbIN.IsChecked = true;
                        rbOUT.IsChecked = false;
                    }
                    else
                    {
                        rbIN.IsChecked = false;
                        rbOUT.IsChecked = true;
                    }

                    if (str2.Substring(7, 1) == "1")
                    {
                        ckb8.IsChecked = false;
                    }
                    else
                    {
                        ckb8.IsChecked = true;
                    }

                    //ckb11 ckb6 ckb27 ckb20 ckb17

                    if (str2.Substring(4, 1) == "1")
                    {
                        ckb11.IsChecked = false;
                    }
                    else
                    {
                        ckb11.IsChecked = true;
                    }

                    if (str1.Substring(1, 1) == "1")
                    {
                        ckb6.IsChecked = false;
                    }
                    else
                    {
                        ckb6.IsChecked = true;
                    }

                    if (str4.Substring(4, 1) == "1")
                    {
                        ckb27.IsChecked = false;
                    }
                    else
                    {
                        ckb27.IsChecked = true;
                    }

                    if (str3.Substring(3, 1) == "1")
                    {
                        ckb20.IsChecked = false;
                    }
                    else
                    {
                        ckb20.IsChecked = true;
                    }

                    if (str3.Substring(6, 1) == "1")
                    {
                        ckb17.IsChecked = false;
                    }
                    else
                    {
                        ckb17.IsChecked = true;
                    }
                    lblString.Content = "读取成功！";
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private string ConvertToBin(char c)
        {
            switch (c)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'A':
                case 'a':
                    return "1010";
                case 'B':
                case 'b':
                    return "1011";
                case 'C':
                case 'c':
                    return "1100";
                case 'D':
                case 'd':
                    return "1101";
                case 'E':
                case 'e':
                    return "1110";
                case 'F':
                case 'f':
                    return "1111";
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// 字符串的加密
        /// </summary>
        /// <param name="Value">要加密的字符串</param>
        /// <param name="sKey">密钥，必须32位</param>
        /// <param name="sIV">向量，必须是12个字符</param>
        /// <returns>加密后的字符串</returns>
        public string EncryptString(string Value, string sKey, string sIV)
        {
            try
            {
                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;
                mCSP.Key = Convert.FromBase64String(sKey);
                mCSP.IV = Convert.FromBase64String(sIV);
                //指定加密的运算模式
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                //获取或设置加密算法的填充模式
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);//创建加密对象
                byt = Encoding.UTF8.GetBytes(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "出现异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ("Error in Encrypting " + ex.Message);
            }
        }


        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">加密后的字符串</param>
        /// <param name="sKey">密钥，必须32位</param>
        /// <param name="sIV">向量，必须是12个字符</param>
        /// <returns>解密后的字符串</returns>
        public string DecryptString(string Value, string sKey, string sIV)
        {
            try
            {
                ICryptoTransform ct;//加密转换运算
                MemoryStream ms;//内存流
                CryptoStream cs;//数据流连接到数据加密转换的流
                byte[] byt;
                //将3DES的密钥转换成byte
                mCSP.Key = Convert.FromBase64String(sKey);
                //将3DES的向量转换成byte
                mCSP.IV = Convert.FromBase64String(sIV);
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);//创建对称解密对象
                byt = Convert.FromBase64String(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "出现异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ("Error in Decrypting " + ex.Message);
            }
        }

        string GetListBoxSelected(ListView LV)
        {
            string strSelected = string.Empty;
            for (int i = 0; i < lvwID1In1Out.Items.Count; i++)
            {
                ListViewItem lv = (ListViewItem)LV.ItemContainerGenerator.ContainerFromItem(LV.Items[i]);
                if (lv == null)
                {
                    break;
                }
                if (lv.IsSelected == true)
                {
                    var vr = LV.Items[i] as CardTypeDef;
                    if (strSelected == string.Empty)
                    {
                        strSelected = vr.Identifying;
                    }
                    else
                    {
                        strSelected = strSelected + "/" + vr.Identifying;
                    }
                }
            }
            return strSelected;
        }

        void SetListBoxSelection(ListView LV, string strSelected)
        {
            if (strSelected == "")
                return;

            string[] sCardTypeArray = strSelected.Split(new Char[] { '/' });
            for (int i = 0; i < sCardTypeArray.Length; i++)
            {
                for (int j = 0; j < LV.Items.Count; j++)
                {
                    ListViewItem lv = (ListViewItem)LV.ItemContainerGenerator.ContainerFromItem(LV.Items[j]);
                    if (lv == null)
                    {
                        return;
                    }
                    else
                    {
                        var vr = LV.Items[j] as CardTypeDef;
                        if (vr.Identifying == sCardTypeArray[i])
                        {
                            lv.IsSelected = true;
                        }
                    }
                }

            }
        }

        #endregion

        #region Events
        //控件保存为全局变量
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region 保存车道设置
                List<CheDaoSet> lstAdd;
                List<CheDaoSet> lstUpdate;

                lstAdd = new List<CheDaoSet>();
                lstUpdate = new List<CheDaoSet>();

                for (int i = 0; i < cds.Count; i++)
                {
                    cds[i].OpenID = cds[i].CtrlNumber;
                    cds[i].StationID = Model.stationID;

                    if (cds[i].ID > 0)
                    {
                        lstUpdate.Add(cds[i]);
                    }
                    else
                    {
                        lstAdd.Add(cds[i]);
                    }
                }



                lstCDS = new List<CheDaoSet>(cds.ToList());
                bool ret = gsd.SaveChannelSet(lstCDS);
                if (ret)
                {
                    for (int i = 0; i < lstCDS.Count; i++)
                    {
                        Model.Channels[i].iInOut = lstCDS[i].InOut;
                        Model.Channels[i].sInOutName = lstCDS[i].InOutName;
                        Model.Channels[i].iCtrlID = lstCDS[i].CtrlNumber;
                        Model.Channels[i].iOpenID = lstCDS[i].OpenID;
                        Model.Channels[i].iOpenType = lstCDS[i].OpenType;
                        //Model.PubVal.Channels[i].sIDAddress = lstCDS[i].;
                        Model.Channels[i].sPersonVideo = lstCDS[i].PersonVideo.ToString();
                        Model.Channels[i].iBigSmall = lstCDS[i].BigSmall;
                        Model.Channels[i].iCheckPortID = lstCDS[i].CheckPortID;
                        Model.Channels[i].iOnLine = lstCDS[i].OnLine;
                        Model.Channels[i].iTempOut = lstCDS[i].TempOut;
                        Model.Channels[i].iOutCard = lstCDS[i].HasOutCard;
                        Model.Channels[i].sIDAddress = lstCDS[i].CameraIP;
                        //Model.PubVal.Channels[i].sIDSignal = ;
                        Model.Channels[i].sSubJH = lstCDS[i].SubJH;
                        Model.Channels[i].iXieYi = lstCDS[i].XieYi;
                        Model.Channels[i].sIP = lstCDS[i].IP;
                    }
                }
                #endregion


                #region 保存图像视频设置 (视频卡操作去掉)
                Model.iEnableNetVideo = chkEnableNetVideo.IsChecked == true ? 1 : 0;
                Model.iVideo4 = chkVideo4.IsChecked == true ? 1 : 0;
                Model.iPersonVideo = chkPersonVideo.IsChecked == true ? 1 : 0;
                Model.iIDCapture = chkZJZP.IsChecked == true ? 1 : 0;
                Model.sImageSavePath = txtImgSavePath.Text.Trim();
                Model.iImageAutoDel = chkImageAutoDel.IsChecked == true ? 1 : 0;
                Model.iImageSaveDays = Convert.ToInt32(cboImageSaveDays.Text);
                Model.iImageAutoDelTime = Convert.ToInt32(cboImageAutoDelTime.Text);

                Model.bAppEnable = chkJF.IsChecked.Value;
                #endregion

                #region 保存收费设置变量
                Model.iChargeType = cboChargeSTD.SelectedIndex;
                Model.iXsdNum = cboXsNum.SelectedIndex + 1;
                Model.iDiscount = chkDiscount.IsChecked == true ? 1 : 0;
                Model.iXsd = chkXsd.IsChecked == true ? 1 : 0;
                Model.iFreeCar = chkFreeCar.IsChecked == true ? 1 : 0;
                Model.iSetTempMoney = chkSetTempMoney.IsChecked == true ? 1 : 0;
                Model.iYKOverTimeCharge = (chkYkGqSf.IsChecked == true) ? 1 : (chkYkDay.IsChecked == true ? 2 : 0);
                Model.iZGXE = (chkTopSF.IsChecked == true) ? 1 : 0;
                Model.iModiTempType_VoiceSF = chkVoiceSF.IsChecked == true ? 1 : 0;
                Model.iSFCancel = chkIDOutCancel.IsChecked == true ? 1 : 0;
                Model.iMonthRule = chkMonthChargeRule.IsChecked == true ? 1 : 0;
                Model.iZGType = cmbZGType.SelectedIndex;
                Model.iZGXEType = cmbStopTimeType.SelectedIndex;
                Model.iMothOverDay = Convert.ToInt32(cmbMothDay.Text);
                Model.iSetTempCardType = chkModiTempType.IsChecked == true ? 1 : 0;
                Model.iTempFree = chkTempFree.IsChecked == true ? 1 : 0;
                //Model.iOnlinePayEnabled = chkScanPay.IsChecked == true ? 1 : 0;
          
                #endregion

                #region 保存在线监控设置
                Model.iLoadTimeInterval = Convert.ToInt32(cboLoadTimeInterval.Text);
                Model.iDisplayTime = chkDisplayTime.IsChecked == true ? 1 : 0;
                //软件控制道闸开关
                Model.iShowGateState = chkShowGateState.IsChecked == true ? 1 : 0;
                Model.iExitOnlineByPwd = chkExitOnlineByPwd.IsChecked == true ? (cboExitPassType.SelectedIndex + 1) : 0;
                Model.iSoftOpenNoPlate = chkSoftOpenNoPlate.IsChecked == true ? 1 : 0;
                Model.iCheDui = chkCheDui.IsChecked == true ? 1 : 0;
                Model.iExceptionHandle = chkExHandle.IsChecked == true ? 1 : 0;
                Model.iShowBoxCardNum = chkShowBoxCardNum.IsChecked == true ? 1 : 0;
                //Model.bOneKeyShortCut = chkOneKeyShortCut.IsChecked ??  false;
                Model.iAutoPrePlate = chkbAutoPrePlate.IsChecked == true ? 1 : 0;
                Model.iForbidSamePosition = chkForbidSamePos.IsChecked == true ? 1 : 0;
                Model.iCheckPortFirst = chkCheckPortFirst.IsChecked == true ? 1 : 0;
                //Model.iFullLight = optFullAllNoIn.IsChecked == true ? 0 : (optFullMonth.IsChecked == true ? 1 : (optFullTemp.IsChecked == true ? 2 : (optFullStore.IsChecked == true ? 3 : (optFullAllIn.IsChecked == true ? 4 : 5))));
                Model.iVideoShiftTime = Convert.ToInt32(cboVideoShift.Text);

                if (optFullAllNoIn.IsChecked == true)
                {
                    Model.iFullLight = 5;
                }
                else if (optFullMonth.IsChecked == true)
                {
                    Model.iFullLight = 1;
                }
                else if (optFullTemp.IsChecked == true)
                {
                    Model.iFullLight = 2;
                }
                else if (optFullStore.IsChecked == true)
                {
                    Model.iFullLight = 3;
                }
                else
                {
                    Model.iFullLight = 0;
                }

                if (chkFullNoIn.IsChecked == false)
                {
                    Model.iFullLight = 0;
                }

                Model.sID1In1OutCardType = GetListBoxSelected(lvwID1In1Out);

                #endregion

                #region 保存ID卡功能设置
                Model.iIDSoftOpen = chkIDSoftOpen.IsChecked == true ? 1 : 0;
                Model.iInOutLimitSeconds = Convert.ToInt32(cboIDInOutLimitSeconds.Text);
                Model.iRealTimeDownLoad = chkRealTimeDownLoad.IsChecked == true ? 1 : 0;
                Model.iIdSfCancel = chkIDSfCancel.IsChecked == true ? 1 : 0;
                Model.iICCardDownLoad = chkICCardDownLoad.IsChecked == true ? 1 : 0;
                Model.iIdReReadHandle = chkIDReReadHandle.IsChecked == true ? 1 : 0;
                Model.iIdPlateDownLoad = chkIDPlateDownLoad.IsChecked == true ? 1 : 0;
                Model.iIDOneInOneOut = chkIDOneInOneOut.IsChecked == true ? 1 : 0; //ID控制一进一出有问题
                Model.iIDComfirmOpen = chkIDComfirmOpen.IsChecked == true ? 1 : 0;
                //Model.sID1In1OutCardType=
                //Model.sIDComfirmOpenCardType=
                #endregion

                #region 保存语音显示功能
                Model.iCtrlShowPlate = chkCtrlShowPlate.IsChecked == true ? 1 : 0;
                Model.iCtrlShowStayTime = chkCtrlShowStayTime.IsChecked == true ? 1 : 0;
                Model.iCtrlShowCW = chkCtrlShowCW.IsChecked == true ? 1 : 0;
                Model.iCtrlShowInfo = chkCtrlShowInfo.IsChecked == true ? 1 : 0;
                Model.iCtrlShowRemainPos = chkCtrlShowRemainPos.IsChecked == true ? 1 : 0;
                Model.iCtrlVoiceLedVersion = cboCtrlVoiceLedVer.SelectedIndex;
                Model.iCtrlVoiceMode = cboCtrlVoiceMode.SelectedIndex;
                Model.iIDNoticeDay = Convert.ToInt32(cboIDNoticeDay.Text);
                #endregion

                #region 外接附加设备设置(包含多功能语音模块)
                Model.iBillPrint = ckbBillP.IsChecked == true ? 1 : 0;
                Model.iBillPrintAuto = chkBillPrintAuto.IsChecked == true ? 1 : 0;
                Model.iPrintFontSize = Convert.ToInt32(cboPrintFontSize.Text);
                //Model.iCarPosLed = carShow.IsChecked ??  false ? 1 : 0; //出入场图片不加水印
                Model.iCarPosCom = Convert.ToInt32(Combo5cw.SelectedIndex + 1);
                //Model.iCarPosLedJH = Convert.ToInt32(Combo5cwjh.Text);   //combo5cwjh 车牌识别
                Model.iCarPosLedLen = Convert.ToInt32(CboLedLen.Text);
                //Model.iSFLed = ClientS.IsChecked ??  false ? 1 : 0;   //脱机车牌（车牌识别）
                Model.iSFLedCom = Convert.ToInt32(Combo5sf.SelectedIndex + 1);
                Model.iSFLedType = Convert.ToInt32(CboSfLed.Text);
                Model.iRemainPosLedShowInfo = chkFeeLed.IsChecked == true ? 1 : 0;
                Model.iRemainPosLedShowPlate = chkShowPlate.IsChecked == true ? 1 : 0;
                Model.iReLoginPrint = chkReloginPrint.IsChecked == true ? 1 : 0;
                Model.iBarCodePrint = chkBarcode.IsChecked == true ? 1 : 0;
                //Model.IsCPHAuto = ckbIsCPHAuto.IsChecked ??  false ? 1 : 0;  在线识别月卡不开闸
                #endregion


                #region 其它设置
                Model.iCtrlSetHasPwd = chkSetPwd.IsChecked == true ? 1 : 0;
                Model.iQueryName = chkQueryName.IsChecked == true ? 1 : 0;
                //Model.iWorkstationNo = Convert.ToInt32(cboStationNo.Text);  //工作站编号
                //Model.iParkingNo = Convert.ToInt32(cboParkingNo.Text);
                Model.strAreaDefault = cboArea.Text;
                Model.iFreeCardNoInPlace = chkFreeCardNoInPlace.IsChecked == true ? 1 : 0;
                Model.iDetailLog = chkDetailLog.IsChecked == true ? 1 : 0;
                Model.iSumMoneyHide = chkSumMoneyHide.IsChecked == true ? 1 : 0;
                Model.iParkTotalSpaces = Convert.ToInt32(txtCar.Text.Trim());
                Model.iTempCarPlaceNum = Convert.ToInt32(txtTempCarPlaceNum.Text);
                Model.iMonthCarPlaceNum = Convert.ToInt32(TxtMonthCarPlaceNum.Text);
                Model.iMoneyCarPlaceNum = Convert.ToInt32(txtMoneyCarPlaceNum.Text);
                Model.iPromptDelayed = Convert.ToInt32(cboPromptDelayed.Text);

                Model.iOnlinePayEnabled = chkScanPay.IsChecked == true ? 1 : 0;

                //扫码设置
                Model.strWXAppID = txtWXAppID.Text;
                Model.strWXMCHID = txtWXMCHID.Text;
                Model.strWXKEY = txtWXKEY.Text;
                Model.strZFBAppID = txtZFBAppID.Text;
                Model.strZFBPID = txtZFBPID.Text;

                if (rbtTempCarPlace.IsChecked == true)
                {
                    Model.iOnlyShowThisRemainPos = 1;
                    Model.bTempCarPlace = true;
                    Model.bMonthCarPlace = false;
                    Model.bMoneyCarPlace = false;
                }
                else if (rbtMonthCarPlace.IsChecked == true)
                {
                    Model.iOnlyShowThisRemainPos = 2;
                    Model.bTempCarPlace = false;
                    Model.bMonthCarPlace = true;
                    Model.bMoneyCarPlace = false;
                }
                else if (rbtMoneyCarPlace.IsChecked == true)
                {
                    Model.iOnlyShowThisRemainPos = 3;
                    Model.bTempCarPlace = false;
                    Model.bMonthCarPlace = false;
                    Model.bMoneyCarPlace = true;
                }
                else  //2015-09-16
                {
                    Model.iOnlyShowThisRemainPos = 0;
                    Model.bTempCarPlace = false;
                    Model.bMonthCarPlace = false;
                    Model.bMoneyCarPlace = false;
                }

                Model.iOneKeyShortCut = chkOneKeyShortCut.IsChecked == true ? 1 : 0;
                Model.iTempDown = ckbTempDown.IsChecked == true ? 1 : 0;
                Model.iAutoMinutes = ckbMinutes.IsChecked == true ? 1 : 0;
                Model.LocalProvince = cboLocalProvince.Text;
                Model.iAutoUpdateJiHao = ckbUpdateJiHao.IsChecked == true ? 1 : 0;
                Model.iSFLed = ckbClientS.IsChecked == true ? 1 : 0;
                Model.iAutoSetMinutes = Convert.ToInt32(cmbMinutes.Text);
                #endregion

                #region 车牌参数设置
                Model.iAutoPlateEn = chkAutoPlateEn.IsChecked == true ? 1 : 0;
                Model.iAutoPlateDBJD = db0.IsChecked == true ? 0 : (db1.IsChecked == true ? 1 : 2);
                Model.iInAutoOpenModel = optInAutoOpenModel0.IsChecked == true ? 0 : (optInAutoOpenModel1.IsChecked == true ? 1 : 2);
                Model.iOutAutoOpenModel = optOutAutoOpenModel0.IsChecked == true ? 0 : (optOutAutoOpenModel1.IsChecked == true ? 1 : 2);
                Model.iInMothOpenModel = optInAutoOpenMoth0.IsChecked == true ? 0 : 1;
                Model.iOutMothOpenModel = optOutAutoOpenMoth0.IsChecked == true ? 0 : 1;
                Model.iCPHPhoto = ckbCPHPhoto.IsChecked == true ? 1 : 0;
                Model.iAutoDeleteImg = ckbDeleteImg.IsChecked == true ? 1 : 0;
                Model.iSameCphDelay = cboSameCphDelay.Text; //--------------i
                Model.iCarPosLed = chkCarShow.IsChecked == true ? 1 : 0;
                Model.iAutoKZ = ckb0AutoKZ.IsChecked == true ? 1 : 0;
                Model.iAutoColorSet = ckbFree.IsChecked == true ? 1 : 0;
                Model.iAuto0Set = ckbFree0.IsChecked == true ? 1 : 0;
                Model.iNoCPHAutoKZ = ckbNOCPH.IsChecked == true ? 1 : 0;
                Model.iTempCanNotInSmall = ckbTempCanNotInSmall.IsChecked == true ? 1 : 0;
                Model.iAutoCPHDZ = chkshibiedazhe.IsChecked == true ? 1 : 0;
                Model.iCentralCharge = chkCenterCharge.IsChecked == true ? 1 : 0;
                Model.iOutSF = chkOutCharge.IsChecked == true ? 1 : 0;
                Model.iMorePaingCar = ckbMore.IsChecked == true ? 1 : 0;
                Model.iMorePaingType = rbtNoIn.IsChecked == true ? 0 : 1;
                Model.iCarPosLedJH = Convert.ToInt32(combo5cwjh.Text == "" ? "0" : combo5cwjh.Text);

                Model.iCphDelay = cboCphDelay.Text;
                Model.iSameCphDelay = cboSameCphDelay.Text;
                #endregion

                PubVarToDataSource();



                string[] strTmp = new string[2];

                string strRetun = "";
                for (int y = 0; y < Model.iChannelCount; y++)
                {

                    #region ---带控制板加载控制板参数
                    if (Model.bIsKZB)
                    {

                        SedBll sendbll = new SedBll(Model.Channels[y].sIP, 1007, 1005);
                        //固定卡有效期剩余xx天提示
                        byte[] BSend = CR.GetByteArray(Convert.ToInt32(cboIDNoticeDay.Text).ToString("00") + Convert.ToInt32(cboIDNoticeDay.Text).ToString("00"));

                        if (Model.Channels[y].iXieYi == 1 || Model.Channels[y].iXieYi == 3)
                        {
                            sendbll.SetUsbType(ref usbHid, Model.Channels[y].iXieYi);
                            strRetun = sendbll.DisplayCmdX1(Convert.ToByte(Model.Channels[y].iCtrlID), 0x84, BSend, Model.Channels[y].iXieYi);

                        }

                        System.Threading.Thread.Sleep(100);
                        if (strRetun == "2")
                        {
                            MessageBox.Show("控制机【" + Model.Channels[y].iCtrlID.ToString() + "】通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            continue;
                        }

                        //设置语音播报方式
                        switch (cboCtrlVoiceMode.SelectedIndex)
                        {
                            case 0:// '欢迎光临/一路顺风:
                                strTmp[0] = "55";
                                strTmp[1] = "55";
                                break;
                            case 1:// '您好/一路平安
                                strTmp[0] = "AA";
                                strTmp[1] = "AA";
                                break;
                            case 2:// '欢迎光临/一路平安
                                strTmp[0] = "55";
                                strTmp[1] = "AA";
                                break;
                            case 3:// '您好/一路顺风
                                strTmp[0] = "AA";
                                strTmp[1] = "55";
                                break;
                        }
                        if (Model.Channels[y].iXieYi == 1 || Model.Channels[y].iXieYi == 3)
                        {
                            sendbll.SetUsbType(ref usbHid, Model.Channels[y].iXieYi);
                            strRetun = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.Channels[y].iCtrlID), 0x3D, 0x60, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], Model.Channels[y].iXieYi);
                        }

                        System.Threading.Thread.Sleep(100);
                        if (strRetun == "2")
                        {
                            MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        //设置语音播报版本
                        switch (cboCtrlVoiceLedVer.SelectedIndex)
                        {
                            case 0:// '3.0语音/显示屏
                                strTmp[0] = "AA";
                                break;
                            case 1:// 4.1语音/显示屏
                                strTmp[0] = "55";
                                break;
                        }

                        if (Model.iCtrlShowPlate == 1)
                        {
                            strTmp[0] = "55";
                        }
                        else
                        {
                            strTmp[0] = "AA";
                        }
                        if (Model.iCtrlShowStayTime == 1)
                        {
                            strTmp[1] = "55";
                        }
                        else
                        {
                            strTmp[1] = "AA";
                        }
                        if (Model.Channels[y].iXieYi == 1 || Model.Channels[y].iXieYi == 3)
                        {
                            sendbll.SetUsbType(ref usbHid, Model.Channels[y].iXieYi);
                            strRetun = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.Channels[y].iCtrlID), 0x3D, 0x71, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], Model.Channels[y].iXieYi);
                        }

                        System.Threading.Thread.Sleep(100);
                        if (strRetun == "2")
                        {
                            MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        //判断带小数点
                        if (chkXsd.IsChecked == true)
                        {
                            strTmp[0] = "AA";
                        }
                        else
                        {
                            if (cboChargeSTD.SelectedIndex == 3)
                            {
                                if (Model.iXsdNum == 1)
                                {
                                    strTmp[0] = "AA";
                                }
                                else
                                {
                                    strTmp[0] = "BB";
                                }
                            }
                            else
                            {
                                strTmp[0] = "55";
                            }
                        }
                        if (Model.Channels[y].iXieYi == 1 || Model.Channels[y].iXieYi == 3)
                        {
                            sendbll.SetUsbType(ref usbHid, Model.Channels[y].iXieYi);
                            strRetun = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.Channels[y].iCtrlID), 0x3D, 0x6F, strTmp[0] + strTmp[0] + "00" + "00", Model.Channels[y].iXieYi);
                        }

                        if (strRetun == "2")
                        {
                            MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        //加载过期多少天后禁止入场 2016-06-23 th
                        if (chkYkDay.IsChecked == true)
                        {
                            if (Model.Channels[y].iXieYi == 1)
                            {
                                sendbll.SendCommand(Model.Channels[y].iCtrlID, 0x85, Model.iMothOverDay, Model.Channels[y].iXieYi);//开闸
                            }

                        }
                        else
                        {
                            if (Model.Channels[y].iXieYi == 1)
                            {
                                sendbll.SendCommand(Model.Channels[y].iCtrlID, 0x85, 0, Model.Channels[y].iXieYi);//开闸
                            }
                        }


                        //脱机车牌识别专用 车牌识别在线下位机月卡不开闸
                        string strData = sendbll.LoadOffLineSet(Convert.ToByte(Model.Channels[y].iCtrlID), Model.Channels[y].iXieYi, Model.Channels[y].sIP);

                        if (Model.iMorePaingCar == 1 || Model.iIDOneInOneOut == 1)
                        {
                            if (strData.Length > 1)
                            {
                                string str1 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(17, 1), 16), 2)).ToString("0000");

                                str1 = "0" + str1.Substring(1);


                                string strSend = Convert.ToString(Convert.ToInt32(str1, 2), 16).ToUpper();

                                strSend = strData.Substring(0, 17) + strSend + strData.Substring(18);

                                string strRsts = sendbll.LoadDinnerPriceSet(Convert.ToByte(Model.Channels[y].iCtrlID), strSend, Model.Channels[y].iXieYi, Model.Channels[y].sIP);
                                if (strRsts == "2")
                                {
                                    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (strData.Length > 1)
                            {
                                string str1 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(17, 1), 16), 2)).ToString("0000");

                                str1 = "1" + str1.Substring(1);


                                string strSend = Convert.ToString(Convert.ToInt32(str1, 2), 16).ToUpper();

                                strSend = strData.Substring(0, 17) + strSend + strData.Substring(18);

                                string strRsts = sendbll.LoadDinnerPriceSet(Convert.ToByte(Model.Channels[y].iCtrlID), strSend, Model.Channels[y].iXieYi, Model.Channels[y].sIP);

                                if (strRsts == "2")
                                {
                                    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }
                    #endregion


                    #region -----摄像机加载显示屏
                    else
                    {
                        List<NetCameraSet> lstNCS = gsd.SelectVideo(Model.Channels[y].sIDAddress);
                        if (lstNCS.Count > 0)
                        {
                            string strVideoType = lstNCS[0].VideoType.ToString();
                            if (strVideoType == "ZNYKT5" || strVideoType == "ZNYKT14" || strVideoType == "ZNYKT13")
                            {

                                int m_nSerialHandle = 0;
                                int m_hLPRClient = 0;
                                if (strVideoType == "ZNYKT5")
                                {
                                    m_hLPRClient = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_Open(lstNCS[0].VideoIP, ushort.Parse(lstNCS[0].VideoPort), lstNCS[0].VideoUserName, lstNCS[0].VideoPassWord);
                                    if (m_hLPRClient == 0)
                                    {
                                        MessageBox.Show("与摄像机链接【" + lstNCS[0].VideoIP + "】失败！");
                                        break;
                                    }

                                    m_nSerialHandle = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_SerialStart(m_hLPRClient, 1, serialRECV, IntPtr.Zero);
                                }
                                else if (strVideoType == "ZNYKT14")
                                {
                                    //int nCamId = MyClass.Net_AddCamera(Videodt.Rows[0]["VideoIP"].ToString());
                                    //m_nSerialHandle = nCamId;
                                    //int iRet = MyClass.Net_ConnCamera(nCamId, 30000, 10);
                                    //if (iRet != 0)
                                    //{
                                    //    MessageBox.Show("与摄像机链接【" + Videodt.Rows[0]["VideoIP"].ToString() + "】失败！");
                                    //    break;
                                    //}
                                }
                                else if (strVideoType == "ZNYKT13")
                                {
                                    //HHERR_CODE errCode = YW7000NetClient.YW7000NET_LogonServer(Videodt.Rows[0]["VideoIP"].ToString(), ushort.Parse(Videodt.Rows[0]["VideoPort"].ToString()), "", Videodt.Rows[0]["VideoUserName"].ToString(), Videodt.Rows[0]["VideoPassWord"].ToString(), (uint)0, out VideoInfo[0].m_hLogon, IntPtr.Zero);
                                    //if (errCode != HHERR_CODE.HHERR_SUCCESS)
                                    //{
                                    //    MessageBox.Show("连接相机【" + txtIP.Text + "】失败!", "提示");
                                    //    break;
                                    //}
                                }


                                if (Model.iCtrlShowPlate == 1)
                                {
                                    strTmp[0] = "55";
                                }
                                else
                                {
                                    strTmp[0] = "AA";
                                }
                                if (Model.iCtrlShowStayTime == 1)
                                {
                                    strTmp[1] = "55";
                                }
                                else
                                {
                                    strTmp[1] = "AA";
                                }


                                byte[] bVZSend = ParkingCommunication.AllCommand.LoadLsNoX2010znykt(0, 0x3D, 0x71, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], 0); ;


                                if (strVideoType == "ZNYKT5")
                                {
                                    int ret0 = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_SerialSendbyte(m_nSerialHandle, bVZSend, bVZSend.Length);
                                }
                                else if (strVideoType == "ZNYKT14")
                                {
                                    //int iRst = MyClass.Net_TransRS485Data(m_nSerialHandle, CR.PubVal.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
                                }
                                else if (strVideoType == "ZNYKTY13")
                                {
                                    //YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend);
                                }

                                System.Threading.Thread.Sleep(100);


                                //设置语音播报方式
                                switch (cboCtrlVoiceMode.SelectedIndex)
                                {
                                    case 0:// '欢迎光临/一路顺风:
                                        strTmp[0] = "55";
                                        strTmp[1] = "55";
                                        break;
                                    case 1:// '您好/一路平安
                                        strTmp[0] = "AA";
                                        strTmp[1] = "AA";
                                        break;
                                    case 2:// '欢迎光临/一路平安
                                        strTmp[0] = "55";
                                        strTmp[1] = "AA";
                                        break;
                                    case 3:// '您好/一路顺风
                                        strTmp[0] = "AA";
                                        strTmp[1] = "55";
                                        break;
                                }

                                byte[] bVZSend2 = ParkingCommunication.AllCommand.LoadLsNoX2010znykt(0, 0x3D, 0x60, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], 0); ;


                                if (strVideoType == "ZNYKT5")
                                {
                                    int ret0 = ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_SerialSendbyte(m_nSerialHandle, bVZSend2, bVZSend2.Length);
                                }
                                else if (strVideoType == "ZNYKT14")
                                {
                                    //int iRst = MyClass.Net_TransRS485Data(m_nSerialHandle, CR.PubVal.i485TT, bVZSend2, Convert.ToByte(bVZSend2.Length));
                                }
                                else if (strVideoType == "ZNYKT13")
                                {
                                    //YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend2);
                                }
                                System.Threading.Thread.Sleep(100);
                                //判断带小数点
                                if (Model.iXsd == 1)
                                {
                                    strTmp[0] = "AA";
                                }
                                else
                                {
                                    if (Model.iChargeType == 3)
                                    {
                                        if (Model.iXsdNum == 1)
                                        {
                                            strTmp[0] = "AA";
                                        }
                                        else
                                        {
                                            strTmp[0] = "BB";
                                        }
                                    }
                                    else
                                    {
                                        strTmp[0] = "55";
                                    }
                                }

                                byte[] bVZSend1 = ParkingCommunication.AllCommand.LoadLsNoX2010znykt(0, 0x3D, 0x6F, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], 0); ;



                                if (strVideoType == "ZNYKT5")
                                {
                                    //int ret = VzClientSDK.VzLPRClient_SerialSendbyte(m_nSerialHandle, bVZSend1, bVZSend1.Length);
                                }
                                else if (strVideoType == "ZNYKT14")
                                {
                                    //int iRst = MyClass.Net_TransRS485Data(m_nSerialHandle, CR.PubVal.i485TT, bVZSend1, Convert.ToByte(bVZSend1.Length));
                                }
                                else if (strVideoType == "ZNYKT13")
                                {
                                    //YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend1);
                                }

                                if (strVideoType == "ZNYKT5")
                                {
                                    if (m_hLPRClient > 0)
                                    {
                                        ParkingCommunication.CameraSDK.ZNYKT5.VzClientSDK.VzLPRClient_Close(m_hLPRClient);
                                        m_hLPRClient = 0;
                                    }
                                }
                                else if (strVideoType == "ZNYKT14")
                                {
                                    //MyClass.Net_DisConnCamera(m_nSerialHandle);
                                    //MyClass.Net_DelCamera(m_nSerialHandle);
                                }
                                else if (strVideoType == "ZNYKT13")
                                {
                                    //if (VideoInfo[0].m_hLogon != IntPtr.Zero)
                                    //{
                                    //    YW7000NetClient.YW7000NET_LogoffServer(VideoInfo[0].m_hLogon);
                                    //    VideoInfo[m_nSelIndex].m_hLogon = IntPtr.Zero;
                                    //}
                                }
                            }
                        }
                    }
                    #endregion

                    //2016-06-21 zsd 启用计费相机,则加载控制板参数
                    if (Model.bAppEnable && Model.Channels[y].iXieYi == 1)
                    {
                        string IP = Model.Channels[y].sIP;     //获取IP

                        IPAddress address;
                        if (IPAddress.TryParse(IP, out address))
                        {
                            SedBll sendbll = new SedBll(Model.Channels[y].sIP, 1007, 1005);
                            //先读取参数，再更改
                            string rtnStr = sendbll.ReadSystem(Convert.ToByte(Model.Channels[y].iCtrlID), 1);
                            GetStrRead(rtnStr, "");

                            if (Model.Channels[y].iInOut == 0)
                            {
                                rbIN.IsChecked = true;   //入口
                            }
                            else
                            {
                                rbOUT.IsChecked = true;  //出口
                                //ckb8.IsChecked ??  false = true;   //IC临时卡确定开闸 
                            }

                            if (Model.Channels[y].iBigSmall == 0)   //大车场
                            {
                                ckb6.IsChecked = false;
                            }
                            else  //小车场
                            {
                                ckb6.IsChecked = true;
                            }

                            rtnStr = sendbll.DisplayCmdX2(Convert.ToByte(Model.Channels[y].iCtrlID), 0x43, CR.GetByteArray(GetStrLoad()), 1);

                            if (rtnStr == "0")
                            {
                                gsd.AddLog(this.Title, "相机参数自动加载成功:" + Model.Channels[y].sIP);
                            }
                            else
                            {
                                MessageBox.Show("与相机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                //this.Close();
                            }
                        }
                        else
                        {
                            //MessageBox.Show("输入的IP不正确！", Language.LanguageXML.GetName("CR/Prompt"));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnOK_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnOK_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            gsd.AddLog(this.Title, "保存设置成功");
            MessageBox.Show("设置成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
            //if (!Model.isStartGuide)
            //    
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void cboChargeSTD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboChargeSTD.SelectedIndex == 3) //北京收费
            {
                cboXsNum.Visibility = Visibility.Visible;
                chkXsd.IsChecked = false;
                chkXsd.IsEnabled = false;
                btnHolidaySet.Visibility = Visibility.Collapsed;
            }
            else if (cboChargeSTD.SelectedIndex == 1)
            {
                chkXsd.IsEnabled = true;
                cboXsNum.Visibility = Visibility.Collapsed;
                btnHolidaySet.Visibility = Visibility.Visible;
            }
            else
            {
                cboXsNum.Visibility = Visibility.Collapsed;
                chkXsd.IsEnabled = true;
                btnHolidaySet.Visibility = Visibility.Collapsed;
            }
        }

        private void chkExitOnlineByPwd_Click(object sender, RoutedEventArgs e)
        {
            cboExitPassType.IsEnabled = chkExitOnlineByPwd.IsChecked == true ? true : false;
        }

        private void cboLoadTimeInterval_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
               || (e.Key >= Key.D0 && e.Key <= Key.D9)
               || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void chkFullNoIn_Click(object sender, RoutedEventArgs e)
        {
            if (chkFullNoIn.IsChecked == false)
            {
                Model.iFullLight = 0;
                grpParkIng.Visibility = Visibility.Collapsed;
            }
            else
            {
                grpParkIng.Visibility = Visibility.Visible;
            }
        }

        private void cboIDInOutLimitSeconds_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
              || (e.Key >= Key.D0 && e.Key <= Key.D9)
              || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void cboIDNoticeDay_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
             || (e.Key >= Key.D0 && e.Key <= Key.D9)
             || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void cboPrintFontSize_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            || (e.Key >= Key.D0 && e.Key <= Key.D9)
            || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void chkModiTempType_Click(object sender, RoutedEventArgs e)
        {
            chkVoiceSF.IsEnabled = chkModiTempType.IsChecked == true ? true : false;
            if (chkModiTempType.IsChecked == false)
            {
                chkVoiceSF.IsChecked = false;
            }
        }

        private void btnHolidaySet_Click(object sender, RoutedEventArgs e)
        {
            ParkingHoliday ph = new ParkingHoliday();
            ph.Owner = this;
            ph.ShowDialog();
        }

        private void btnPrintTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ckbBillP_Click(object sender, RoutedEventArgs e)
        {
            if (ckbBillP.IsChecked == true)
            {
                chkBillPrintAuto.IsEnabled = true;
            }
            else
            {
                chkBillPrintAuto.IsEnabled = false;
                chkBillPrintAuto.IsChecked = false;
            }
        }

        private void ckbMore_Click(object sender, RoutedEventArgs e)
        {
            if (ckbMore.IsChecked == true)
            {
                spMoreCar.Visibility = Visibility.Visible;
            }
            else
            {
                spMoreCar.Visibility = Visibility.Hidden;
            }
        }

        private void btnIDOneInOneOut_Click(object sender, RoutedEventArgs e)
        {
            if (gpID1In1Out.Visibility == Visibility.Visible)
            {
                gpID1In1Out.Visibility = Visibility.Hidden;
            }
            else
            {
                gpID1In1Out.Visibility = Visibility.Visible;
                SetListBoxSelection(lvwID1In1Out, Model.sID1In1OutCardType);
            }
        }

        private void chkIDOneInOneOut_Click(object sender, RoutedEventArgs e)
        {
            //btnIDOneInOneOut.IsEnabled = chkIDOneInOneOut.IsChecked.Value;
        }

        private void chkScanPay_Click(object sender, RoutedEventArgs e)
        {
            gpbScan.Visibility = chkScanPay.IsChecked == true ? Visibility.Visible : Visibility.Hidden;
        }

        #region 控制板参数设置
        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Model.bIsKZB)
                {
                    //2015-11-18 判断是否勾选出口车道
                    bool bHaveSelect = false;   //是否有勾选的车道
                    for (int i = 0; i < dgvChargeCheDao.Items.Count; i++)
                    {
                        FrameworkElement item = dgvChargeCheDao.Columns[0].GetCellContent(dgvChargeCheDao.Items[i]);
                        DataGridTemplateColumn temp = (dgvChargeCheDao.Columns[0] as DataGridTemplateColumn);
                        CheckBox cb = temp.CellTemplate.FindName("chkSelect", item) as CheckBox;
                        if (cb.IsChecked == true)
                        {
                            bHaveSelect = true;  //有勾选
                        }
                    }
                    if (bHaveSelect == false)  //没勾选
                    {
                        MessageBox.Show("请先勾选车道！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    return;
                }


                if (dgvChargeCheDao.Items.Count > 0)
                {
                    for (int i = 0; i < dgvChargeCheDao.Items.Count; i++)
                    {
                        FrameworkElement item = dgvChargeCheDao.Columns[0].GetCellContent(dgvChargeCheDao.Items[i]);
                        DataGridTemplateColumn temp = (dgvChargeCheDao.Columns[0] as DataGridTemplateColumn);
                        CheckBox cb = temp.CellTemplate.FindName("chkSelect", item) as CheckBox;


                        if (cb.IsChecked == true)
                        {
                            var vr = dgvChargeCheDao.Items[i] as CheDaoSet;
                            string rtnStr = "";
                            string strData = "";
                            int JiHao = vr.CtrlNumber;
                            string IP = vr.IP;

                            int XieYi = vr.XieYi;
                            SedBll sendbll = new SedBll(IP, 1007, 1005);

                            if (XieYi == 1)
                            {
                                rtnStr = sendbll.ReadSystem(Convert.ToByte(JiHao), 1);
                                strData = sendbll.LoadOffLineSet(Convert.ToByte(JiHao), 1, IP);

                                //2016-05-14 TH读取摄像机版本号
                                if (Model.bAppEnable)
                                {
                                    string strDataVersion = sendbll.LoadVersionSet(Convert.ToByte(JiHao), 1, IP);
                                    if (strDataVersion.Length > 8)
                                    {
                                        //lblVersion.Text = "摄像机版本号：" + Convert.ToInt32(strDataVersion.Substring(0, 2), 16) + "." + Convert.ToInt32(strDataVersion.Substring(2, 2), 16) + "." + Convert.ToInt32(strDataVersion.Substring(4, 2), 16) + "." + Convert.ToInt32(strDataVersion.Substring(6, 2), 16) + "." + Convert.ToInt32(strDataVersion.Substring(8, 2), 16) + "." + Convert.ToInt32(strDataVersion.Substring(10, 2), 16);
                                        //lblVersion0.Text = "摄像机版本时间：" + GetAscii(strDataVersion.Substring(12, 16));
                                        //lblVersion1.Text = "摄像机版本名称：" + GetAscii(strDataVersion.Substring(28, 32));
                                        //groupBox29.Visible = true;
                                        if (Convert.ToInt32(strDataVersion.Substring(10, 2), 16) == 1)
                                        {
                                            //lblVersion2.Text = "摄像机版本:支持脱机收费";
                                        }
                                        else
                                        {
                                            //lblVersion2.Text = "摄像机版本:不支持脱机收费";
                                        }
                                    }
                                }
                                else
                                {
                                    //lblVersion.Text = "";
                                    //lblVersion0.Text = "";
                                    //lblVersion1.Text = "";
                                    //lblVersion2.Text = "";
                                }
                            }
                            else if (XieYi == 3)//USB通讯
                            {
                                sendbll.SetUsbType(ref usbHid, 3);
                                rtnStr = sendbll.ReadSystem(Convert.ToByte(JiHao), 3);
                            }

                            if (rtnStr == "2")
                            {
                                MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            GetStrRead(rtnStr, strData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnRead_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnRead_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void btnReadIO_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            //筛选
            fileDialog.Filter = "Data(*.data)|*.data|All files (*.*)|*.*";
            if (fileDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                //显示选择文件
                //MessageBox.Show(fileDialog.FileName);
                string strFIle = fileDialog.FileName;
                StreamReader sr = new StreamReader(strFIle, Encoding.Default);
                String line;
                line = sr.ReadLine();

                string sKey = "ZNYKTh9hESZDDGeCmFPGuxzaiB7NLQTH";
                // string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM5";
                //向量，必须是12个字符
                string sIV = "andyhua8910=";

                sr.Dispose();

                //解密
                string desValue = DecryptString(line, sKey, sIV);



                if (desValue.Length > 14)
                {
                    string strSet = desValue.Substring(0, 4);
                    DateTime dtEnd = Convert.ToDateTime(desValue.Substring(4, 10) + " 23:59:59");
                    string strCount = desValue.Substring(14, desValue.Length - 14);
                    int iCount = Convert.ToInt32(strCount == "" ? "0" : strCount);

                    if (dtEnd > DateTime.Now && iCount > 0)
                    {

                        if (Model.bIsKZB)
                        {
                            //2015-11-18 判断是否勾选出口车道
                            bool bHaveSelect = false;   //是否有勾选的车道
                            for (int i = 0; i < dgvChargeCheDao.Items.Count; i++)
                            {


                                var item = dgvChargeCheDao.Items[i];

                                DataGridTemplateColumn templeColumn = dgvChargeCheDao.Columns[0] as DataGridTemplateColumn;
                                FrameworkElement fwElement = dgvChargeCheDao.Columns[0].GetCellContent(item);
                                CheckBox cBox = templeColumn.CellTemplate.FindName("chkSelect", fwElement) as CheckBox;

                                if (cBox.IsChecked == true)
                                {
                                    bHaveSelect = true;  //有勾选
                                }
                            }
                            if (bHaveSelect == false)  //没勾选
                            {
                                MessageBox.Show("请先勾选车道！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }


                        if (dgvChargeCheDao.Items.Count > 0)
                        {
                            for (int i = 0; i < dgvChargeCheDao.Items.Count; i++)
                            {
                                var item = dgvChargeCheDao.Items[i];

                                DataGridTemplateColumn templeColumn = dgvChargeCheDao.Columns[0] as DataGridTemplateColumn;
                                FrameworkElement fwElement = dgvChargeCheDao.Columns[0].GetCellContent(item);
                                CheckBox cBox = templeColumn.CellTemplate.FindName("chkSelect", fwElement) as CheckBox;

                                if (cBox.IsChecked == true)
                                {
                                    string rtnStr = "";

                                    var v = item as CheDaoSet;

                                    int JiHao = Convert.ToInt32(v.CtrlNumber);
                                    string IP = v.IP;

                                    int XieYi = Convert.ToInt32(v.XieYi);
                                    SedBll sendbll = new SedBll(IP, 1007, 1005);

                                    string strRsts = "";
                                    if (strData.Length > 1)
                                    {
                                        string str1 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(17, 1), 16), 2)).ToString("0000");

                                        string strSend = Convert.ToString(Convert.ToInt32(strSet, 2), 16).ToUpper();

                                        strSend = strData.Substring(0, 12) + strSend + strData.Substring(13);

                                        strRsts = sendbll.LoadDinnerPriceSet(Convert.ToByte(JiHao), strSend, 1, IP);

                                    }


                                    progressBar2.Maximum = 100;
                                    for (int y = 0; y < 100; y++)
                                    {
                                        progressBar2.Value = y;
                                        Thread.Sleep(10);
                                    }


                                    if (strRsts == "0")
                                    {
                                        MessageBox.Show("加载成功", "提示");
                                    }
                                    else
                                    {
                                        MessageBox.Show("加载失败", "提示");
                                    }

                                }
                            }
                        }

                        string strSum = strSet + dtEnd.ToShortDateString() + (iCount - 1);


                        //加密
                        string oldValue = strSum;
                        //加密后结果
                        //密钥,必须32位


                        //print
                        string newValue = EncryptString(oldValue, sKey, sIV);

                        //string strPath = System.Windows.Forms.Application.StartupPath + @"\Camera.data";

                        FileStream fs = new FileStream(strFIle, FileMode.Create);
                        //获得字节数组
                        byte[] data = System.Text.Encoding.Default.GetBytes(newValue);
                        //开始写入
                        fs.Write(data, 0, data.Length);
                        //清空缓冲区、关闭流
                        fs.Flush();
                        fs.Close();


                    }
                    else
                    {
                        MessageBox.Show("请联系管理员 DT");
                    }
                }
            }
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in dgvChargeCheDao.Items)
                {
                    if (item != dgvChargeCheDao.SelectedItem)
                    {
                        DataGridTemplateColumn templeColumn = dgvChargeCheDao.Columns[0] as DataGridTemplateColumn;
                        FrameworkElement fwElement = dgvChargeCheDao.Columns[0].GetCellContent(item);
                        CheckBox cBox = templeColumn.CellTemplate.FindName("chkSelect", fwElement) as CheckBox;

                        if (cBox != null)
                        {
                            cBox.IsChecked = false;
                        }
                    }
                }

                var vr = dgvChargeCheDao.SelectedItem as CheDaoSet;
                string rtnStr = "";
                string strData = "";
                int JiHao = vr.CtrlNumber;
                string IP = vr.IP;
                int XieYi = vr.XieYi;
                SedBll sendbll = new SedBll(IP, 1007, 1005);
                if (XieYi == 1) //
                {
                    rtnStr = sendbll.ReadSystem(Convert.ToByte(JiHao), 1);
                    strData = sendbll.LoadOffLineSet(Convert.ToByte(JiHao), 1, IP);

                    //2016-05-14 TH读取摄像机版本号
                    if (Model.bAppEnable)
                    {
                        string strDataVersion = sendbll.LoadVersionSet(Convert.ToByte(JiHao), 1, IP);
                    }
                    else
                    {

                    }
                }
                else if (XieYi == 3)//USB通讯
                {
                    sendbll.SetUsbType(ref usbHid, 3);
                    rtnStr = sendbll.ReadSystem(Convert.ToByte(JiHao), 3);
                }
                else
                {
                    //short Ji = Convert.ToInt16(JiHao);
                    //short cmd = 0x44;
                    //short Count1 = Convert.ToInt16(4);
                    //string ComSend = GetStrLoad();
                    //rtnStr = axZNYKT_1.ReadByte2010ZNYKT_(ref Ji, ref cmd, Count1);
                    //rtnStr = sendbll.ReadSystem(Convert.ToByte(cmbNo.Text, 16), 0);
                }
                if (rtnStr == "2")
                {
                    MessageBox.Show("与控制机通讯不通", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                GetStrRead(rtnStr, strData);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":CheckBox_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nCheckBox_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvChargeCheDao.SelectedIndex == -1)
                {
                    return;
                }
                if (dgvChargeCheDao.Items.Count > 0)
                {
                    string rtnStr = "";
                    string strData = "";
                    var vr = dgvChargeCheDao.SelectedItem as CheDaoSet;
                    int JiHao = vr.CtrlNumber;
                    string IP = vr.IP;
                    int XieYi = vr.XieYi;
                    SedBll sendbll = new SedBll(IP, 1007, 1005);
                    if (XieYi == 1)//TCP通讯
                    {
                        rtnStr = sendbll.DisplayCmdX2(Convert.ToByte(JiHao), 0x43, CR.GetByteArray(GetStrLoad()), 1);

                        strData = sendbll.LoadOffLineSet(Convert.ToByte(JiHao), 1, IP);
                        if (strData.Length > 1)
                        {
                            string str1 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(17, 1), 16), 2)).ToString("0000");

                            string str2 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(16, 1), 16), 2)).ToString("0000");

                            string str3 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(13, 1), 16), 2)).ToString("0000");

                            if (chb0AutoKZ.IsChecked == true)
                            {
                                str3 = str3.Substring(0, 3) + "0";
                            }
                            else
                            {
                                str3 = str3.Substring(0, 3) + "1";
                            }



                            string strSet = "";
                            if (ckbMthNo.IsChecked == true)
                            {
                                strSet = "0";
                            }
                            else
                            {
                                strSet = "1";
                            }
                            if (ckbTmpChinese.IsChecked == true)
                            {
                                strSet = strSet + "0";
                            }
                            else
                            {
                                strSet = strSet + "1";
                            }

                            if (ckbQTmpAutoKZ.IsChecked == true)
                            {
                                strSet = strSet + "0";
                            }
                            else
                            {
                                strSet = strSet + "1";
                            }


                            if (ckbQAutoKZ.IsChecked == true)
                            {
                                strSet = strSet + "0";
                            }
                            else
                            {
                                strSet = strSet + "1";
                            }

                            if (ckbChanelTJ.IsChecked == true)
                            {
                                str2 = "0" + str2.Substring(1);
                            }
                            else
                            {
                                str2 = "1" + str2.Substring(1);
                            }


                            str1 = strSet;


                            str1 = strSet;
                            string strSend = Convert.ToString(Convert.ToInt32(str1, 2), 16).ToUpper();

                            string strSend1 = Convert.ToString(Convert.ToInt32(str2, 2), 16).ToUpper();

                            string strSend2 = Convert.ToString(Convert.ToInt32(str3, 2), 16).ToUpper();

                            //strSend = strData.Substring(0, 16) + strSend1 + strSend + strData.Substring(18);

                            strSend = strData.Substring(0, 13) + strSend2 + strData.Substring(14, 3) + strSend + strData.Substring(18);

                            string strRsts = sendbll.LoadDinnerPriceSet(Convert.ToByte(JiHao), strSend, 1, IP);
                        }
                    }
                    else if (XieYi == 3)//USB通讯
                    {
                        sendbll.SetUsbType(ref usbHid, 3);
                        rtnStr = sendbll.DisplayCmdX2(Convert.ToByte(JiHao), 0x43, CR.GetByteArray(GetStrLoad()), 3);
                    }

                    if (rtnStr == "0")
                    {
                        gsd.AddLog(this.Title, "控制板参数加载成功 " + JiHao.ToString() + " 号机");
                        lblString.Content = "控制板参数加载成功！";
                    }
                    else
                    {
                        MessageBox.Show("与控制机通讯不通", "");
                    }

                }

            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnLoad_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnLoad_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnFormart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvChargeCheDao.SelectedIndex == -1)
                {
                    return;
                }
                if (dgvChargeCheDao.Items.Count > 0)
                {
                    string rtnStr = "";
                    //string strData = "";
                    var vr = dgvChargeCheDao.SelectedItem as CheDaoSet;
                    int JiHao = vr.CtrlNumber;
                    string IP = vr.IP;
                    int XieYi = vr.XieYi;
                    SedBll sendbll = new SedBll(IP, 1007, 1005);

                    if (MessageBox.Show("重要提示：“控制板格式化”将把控制板中的所有数据清空，请慎重操作！！！\n\r提示:部分主板不支持格式化功能！\n\r请确认是否继续格式化【" + JiHao + "】号机 ？", "删除确认 ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        if (MessageBox.Show("重要提示：“控制板格式化”将把控制板中的所有数据清空，请慎重操作！！！\n\r提示:部分主板不支持格式化功能！\n\r请确认是否继续格式化【" + JiHao + "】号机 ？", "删除确认 ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            if (MessageBox.Show("重要提示：“控制板格式化”将把控制板中的所有数据清空，请慎重操作！！！\n\r提示:部分主板不支持格式化功能！\n\r请确认是否继续格式化【" + JiHao + "】号机 ？", "删除确认 ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                if (XieYi == 1)//TCP通讯
                                {
                                    rtnStr = sendbll.DisplayCmdX1(Convert.ToByte(JiHao.ToString(), 16), 0xA0, CR.GetByteArray("AAAAAAAAAAAAAAAAAAAABBBBBBBBBBBBBBBBBBBB"), 1);
                                }
                                else if (XieYi == 3)
                                {
                                    sendbll.SetUsbType(ref usbHid, 3);
                                    rtnStr = sendbll.DisplayCmdX1(Convert.ToByte(JiHao.ToString(), 16), 0xA0, CR.GetByteArray("AAAAAAAAAAAAAAAAAAAABBBBBBBBBBBBBBBBBBBB"), 3);
                                }
                                if (rtnStr == "0")
                                {
                                    gsd.AddLog(this.Title, "控制板格式化成功 " + JiHao + " 号机");
                                    lblString.Content = "格式化成功！";
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnFormart_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnFormart_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvChargeCheDao_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string typeValue = e.Column.GetType().ToString();
            if (typeValue == "System.Windows.Controls.DataGridCheckBoxColumn")
            {
                aftValue = (e.EditingElement as CheckBox).IsChecked;
            }
        }

        private void btnReadKZJType_Click(object sender, RoutedEventArgs e)
        {
            if (Model.bIsKZB)
            {
                //2015-11-18 判断是否勾选出口车道
                bool bHaveSelect = false;   //是否有勾选的车道
                for (int i = 0; i < dgvChargeCheDao.Items.Count; i++)
                {
                    var item = dgvChargeCheDao.Items[i];

                    DataGridTemplateColumn templeColumn = dgvChargeCheDao.Columns[0] as DataGridTemplateColumn;
                    FrameworkElement fwElement = dgvChargeCheDao.Columns[0].GetCellContent(item);
                    CheckBox cBox = templeColumn.CellTemplate.FindName("chkSelect", fwElement) as CheckBox;

                    if (cBox.IsChecked == true)
                    {
                        bHaveSelect = true;  //有勾选
                    }
                }
                if (bHaveSelect == false)  //没勾选
                {
                    MessageBox.Show("请先勾选车道！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                return;
            }
            if (dgvChargeCheDao.Items.Count > 0)
            {
                for (int i = 0; i < dgvChargeCheDao.Items.Count; i++)
                {
                    var item = dgvChargeCheDao.Items[i];

                    DataGridTemplateColumn templeColumn = dgvChargeCheDao.Columns[0] as DataGridTemplateColumn;
                    FrameworkElement fwElement = dgvChargeCheDao.Columns[0].GetCellContent(item);
                    CheckBox cBox = templeColumn.CellTemplate.FindName("chkSelect", fwElement) as CheckBox;

                    if (cBox.IsChecked == true)
                    {
                        var v = item as CheDaoSet;

                        string rtnStr = "";
                        int JiHao = Convert.ToInt32(v.CtrlNumber);
                        string IP = v.IP;

                        int XieYi = v.XieYi;
                        SedBll sendbll = new SedBll(IP, 1007, 1005);
                        string strData = sendbll.LoadOffLineSet(Convert.ToByte(JiHao), 1, IP);

                        if (strData.Length > 1)
                        {
                            string str1 = Convert.ToInt32(Convert.ToString(Convert.ToInt32(strData.Substring(12, 1), 16), 2)).ToString("0000");

                            if (str1 == "1100")
                            {
                                lblKZJType.Content = "控制机类型为：0";//CCDold
                            }
                            else if (str1 == "1101")
                            {
                                lblKZJType.Content = "控制机类型为：1";//CCDnew
                            }
                            else if (str1 == "1110")
                            {
                                lblKZJType.Content = "控制机类型为：2";//CMOSold
                            }
                            else if (str1 == "1111")
                            {
                                lblKZJType.Content = "控制机类型为：3";//CMOSnew
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region 语音显示屏设置
        private void btnLoadBarPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int j = 0; j < Model.iChannelCount; j++)
                {
                    if (Model.Channels[j].iInOut == 0)
                    {
                        bool bRst = BarPrintBaudSet(Model.Channels[j].sIP, Convert.ToByte(Model.Channels[j].iCtrlID), Convert.ToInt32(cboPrintType.Text), cboBarPrintBaud.Text, txtBarPrintLoad0.Text, "", txtBarPrintLoad1.Text, Model.Channels[j].iXieYi);

                        if (bRst == false)
                        {
                            MessageBox.Show("与控制机通讯不通", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            //this.Close();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("加载成功", "", MessageBoxButton.OK);
                            //this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnLoadBarPrint_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnLoadBarPrint_Click", "打印机测试", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        #endregion
        #endregion
        #endregion


        #region 网络摄像机设置
        #region Fields
        string preValue0 = "", aftValue0 = "";
        /// <summary>
        /// 定义绑定的数据源(相机管理)
        /// </summary>
        List<NetCameraSet> lstNCS = new List<NetCameraSet>();
        ObservableCollection<NetCameraSet> ncs = new ObservableCollection<NetCameraSet>();
        #endregion

        #region Methods
        void BinVideoData()
        {
            lstNCS = gsd.SelectVideoAllIP();

            dcboVideoType.ItemsSource = BinModel.lstVideoType;

            if (chkVideo4.IsChecked == true)
            {
                dcboPersonID.ItemsSource = gsd.SelectVideoAllIP();
                dcboPersonID.DisplayMemberPath = "VideoIP";
                dcboPersonID.SelectedValuePath = "ID";
            }


            if (chkEnableNetVideo.IsChecked == true)
            {
                dcboCameraIP.ItemsSource = gsd.SelectVideoAllIP();
                dcboCameraIP.DisplayMemberPath = "VideoIP";
                dcboCameraIP.SelectedValuePath = "VideoIP";
            }
            else
            {
                dcboCameraIP.ItemsSource = null;
            }
            

            ncs = new ObservableCollection<NetCameraSet>(lstNCS);
            dgvVideo.SelectedIndex = dgvVideo.Items.Count - 1;
            dgvVideo.DataContext = ncs;



            //BinModel.lstCameraIP.Clear();
            //if (chkEnableNetVideo.IsChecked == true)
            //{
            //    for (int i = 0; i < lstNCS.Count; i++)
            //    {
            //        BinModel.lstCameraIP.Add(lstNCS[i].VideoIP);
            //    }
            //}
            //dgLane.DataContext = null;
            //dgLane.DataContext = cds;
        }

        bool DelCamera()
        {
            if (dgvVideo.Items.Count > 0)
            {
                if (dgvVideo.SelectedIndex > -1)
                {
                    NetCameraSet ncs0 = dgvVideo.SelectedItem as NetCameraSet;
                    if (ncs0.VideoIP != "")
                    {
                        int ret = gsd.DeleteVideo(ncs0.VideoIP);
                        if (ret > 0)
                        {
                            BinVideoData();
                            return true;
                        }
                        else
                        {
                            if (ncs.Count > 0)
                            {
                                ncs.Remove(ncs0);
                            }
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    dgvVideo.SelectedIndex = dgvVideo.Items.Count - 1;
                    return DelCamera();
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Events
        private void cmdSelectNetCameraSet_Click(object sender, RoutedEventArgs e)
        {
            ShowRight0(grpCamera);

            try
            {
                ncs = new ObservableCollection<NetCameraSet>(lstNCS);
                dgvVideo.SelectedIndex = dgvVideo.Items.Count - 1;
                dgvVideo.DataContext = ncs;

                //BinVideoData();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":cmdSelectNetCameraSet_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "cmdSelectNetCameraSet_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkEnableNetVideo_Click(object sender, RoutedEventArgs e)
        {
            BinVideoData();
        }

        private void btnAddCamera_Click(object sender, RoutedEventArgs e)
        {
            if (Model.bAppEnable)
            {
                ncs.Add(new NetCameraSet
                {
                    VideoType = "ZNYKT15",
                    VideoIP = "192.168.168.188",
                    VideoPort = "80",
                    VideoPassWord = "admin",
                    VideoUserName = "admin"
                });
            }
            else
            {
                ncs.Add(new NetCameraSet
                {
                    VideoType = "ZNYKT5",
                    VideoIP = "192.168.168.188",
                    VideoPort = "80",
                    VideoPassWord = "admin",
                    VideoUserName = "admin"
                });
            }
            dgvVideo.SelectedIndex = dgvVideo.Items.Count - 1;
        }

        private void btnDelCamera_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DelCamera();
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnDelCamera_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnDelCamera_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveCamera_Click(object sender, RoutedEventArgs e)
        {
            lstNCS = new List<NetCameraSet>(ncs.ToList());
            bool ret = gsd.SaveNetVideo(lstNCS);
            if (ret)
            {
                BinVideoData();
                grpCamera.Visibility = Visibility.Hidden;
            }
        }

        private void chkPersonVideo_Click(object sender, RoutedEventArgs e)
        {
            if (chkVideo4.IsChecked == true)
            {
                if (chkPersonVideo.IsChecked == true)
                {
                    dgLane.Columns[6].Visibility = Visibility.Visible;
                    BinVideoData();
                }
                else
                {
                    dgLane.Columns[6].Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                chkPersonVideo.IsChecked = false;
                dgLane.Columns[6].Visibility = Visibility.Collapsed;
            }
        }

        private void btnImgSavePath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browseDialog = new System.Windows.Forms.FolderBrowserDialog();

            browseDialog.RootFolder = Environment.SpecialFolder.Desktop;
            browseDialog.SelectedPath = "E:";
            browseDialog.ShowNewFolderButton = true;
            browseDialog.Description = "请选择图像保存目录";

            if (browseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtImgSavePath.Text = browseDialog.SelectedPath;
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                || (e.Key >= Key.D0 && e.Key <= Key.D9)
                || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void cboImageSaveDays_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                || (e.Key >= Key.D0 && e.Key <= Key.D9)
                || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        private void dgvVideo_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                string typeValue = e.Column.GetType().ToString();

                if (typeValue == "System.Windows.Controls.DataGridComboBoxColumn")
                {
                    preValue0 = (e.Column.GetCellContent(e.Row) as ComboBox).Text;
                }

            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvVideo_BeginningEdit", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dgvVideo_BeginningEdit", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvVideo_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                string typeValue = e.Column.GetType().ToString();

                if (typeValue == "System.Windows.Controls.DataGridComboBoxColumn")
                {
                    aftValue0 = (e.EditingElement as ComboBox).Text;
                }
                if (preValue0 != aftValue0)
                {
                    ncs[dgvVideo.SelectedIndex].VideoType = aftValue0;
                    if (aftValue0 == "ZNYKT5" || aftValue0 == "ZNYKT15")
                    {
                        ncs[dgvVideo.SelectedIndex].VideoPort = "80";
                        ncs[dgvVideo.SelectedIndex].VideoPassWord = "admin";
                    }
                    else if (aftValue0 == "ZNYKT10")
                    {
                        ncs[dgvVideo.SelectedIndex].VideoPort = "37777";
                        ncs[dgvVideo.SelectedIndex].VideoPassWord = "admin";
                    }
                    else if (aftValue0 == "ZNYKT11")
                    {
                        ncs[dgvVideo.SelectedIndex].VideoPort = "8000";
                        ncs[dgvVideo.SelectedIndex].VideoPassWord = "admin";
                    }
                    else if (aftValue0 == "ZNYKT4")
                    {
                        ncs[dgvVideo.SelectedIndex].VideoPort = "8000";
                        ncs[dgvVideo.SelectedIndex].VideoPassWord = "12345";
                    }
                    else if (aftValue0 == "ZNYKT14")
                    {
                        ncs[dgvVideo.SelectedIndex].VideoPort = "8000";
                        ncs[dgvVideo.SelectedIndex].VideoPassWord = "admin";
                    }
                    //dgvVideo.ItemsSource = null;
                    //dgvVideo.ItemsSource = ncs;
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvVideo_CellEditEnding", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dgvVideo_CellEditEnding", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        #endregion


        #region 月卡收费自定义
        #region Fields
        /// <summary>
        /// 动态绑定(月卡收费规则自定义)
        /// </summary>
        ObservableCollection<ChargeRules> cr = new ObservableCollection<ChargeRules>();
        List<ChargeRules> lstCR = new List<ChargeRules>();
        #endregion

        #region Methods
        void BindMonthRuleData()
        {
            lstCR = gsd.GetMonthRuleDefine();
            for (int i = 0; i < lstCR.Count; i++)
            {
                lstCR[i].CardType = CR.GetCardType(lstCR[i].CardType, 1);
            }
            cr = new ObservableCollection<ChargeRules>(lstCR);
            dgvMonthChargeRule.DataContext = cr;
            dgvMonthChargeRule.SelectedIndex = dgvMonthChargeRule.Items.Count - 1;
        }
        #endregion

        #region Events
        private void btnSelectMonthRule_Click(object sender, RoutedEventArgs e)
        {
            ShowRight0(grpMonthChargeRule);
            BindMonthRuleData();
        }
        private void btnAddMonthRule_Click(object sender, RoutedEventArgs e)
        {
            if (cboMonthRuleType.Text == "")
            {
                MessageBox.Show("请输入卡类", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            for (int i = 0; i < cr.Count; i++)
            {
                if (cr[i].CardType == cboMonthRuleType.Text)
                {
                    return;
                }
            }

            cr.Add(new ChargeRules
            {
                CardType = cboMonthRuleType.Text,
                FreeMinute = 1,
                JE = 200,
                ParkID = 0,
                TopSF = 30
            });
        }

        private void btnDelMonthRule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvMonthChargeRule.Items.Count > 0)
                {
                    int index = dgvMonthChargeRule.SelectedIndex;
                    if (index == -1)
                    {
                        dgvMonthChargeRule.SelectedIndex = dgvMonthChargeRule.Items.Count - 1;
                    }
                    cr.Remove(dgvMonthChargeRule.SelectedItem as ChargeRules);
                    lstCR = new List<ChargeRules>(cr.ToList());
                    bool ret = gsd.SaveMonthRuleDefine(lstCR);

                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnDelMonthRule_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnDelMonthRule_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveMonthRule_Click(object sender, RoutedEventArgs e)
        {
            lstCR = new List<ChargeRules>(cr.ToList());
            gsd.SaveMonthRuleDefine(lstCR);
            grpMonthChargeRule.Visibility = Visibility.Hidden;
        }
        #endregion
        #endregion


        #region 免费原因设置
        #region Fields
        /// <summary>
        /// 绑定数据源(免费原因自定义)
        /// </summary>
        ObservableCollection<FreeReason> fr = new ObservableCollection<FreeReason>();
        List<FreeReason> lstFR = new List<FreeReason>();
        #endregion

        #region Methods
        void BindFreeReasonData()
        {
            lstFR = gsd.GetFreeReasonDefine();
            fr = new ObservableCollection<FreeReason>(lstFR);
            dgvFreeReason.DataContext = fr;
            dgvFreeReason.SelectedIndex = dgvFreeReason.Items.Count - 1;
        }
        #endregion

        #region Events
        private void btnSelectFreeReason_Click(object sender, RoutedEventArgs e)
        {
            ShowRight0(grpFreeReason);
            BindFreeReasonData();
        }

        private void btnAddFreeReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < fr.Count; i++)
                {
                    if (txtFreeReason.Text.Trim() == fr[i].ItemDetail)
                    {
                        return;
                    }
                }
                if (txtFreeReason.Text != "")
                {
                    fr.Add(new FreeReason
                    {
                        ItemID = fr.Count,
                        ItemDetail = txtFreeReason.Text
                    });
                }
                else
                {
                    MessageBox.Show("免费原因不能为空", "提示");
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnAddFreeReason_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message, "提示");
            }
        }

        private void btnDelFreeReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvFreeReason.Items.Count > 0)
                {
                    int index = dgvFreeReason.SelectedIndex;

                    if (index != -1)
                    {
                        fr.Remove(dgvFreeReason.SelectedItem as FreeReason);
                        lstFR = new List<FreeReason>(fr.ToList());
                        bool ret = gsd.SaveFreeReasonDefine(lstFR);
                    }
                    else
                    {
                        if (dgvFreeReason.Items.Count > 0)
                        {
                            dgvFreeReason.SelectedIndex = dgvFreeReason.Items.Count - 1;
                            fr.Remove(dgvFreeReason.SelectedItem as FreeReason);
                            lstFR = new List<FreeReason>(fr.ToList());
                            bool ret = gsd.SaveFreeReasonDefine(lstFR);
                            if (ret)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSaveFreeReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstFR = new List<FreeReason>(fr.ToList());
                gsd.SaveFreeReasonDefine(lstFR);
                grpFreeReason.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #endregion


        #region 脱机车牌下载
        const uint WM_USER = 0x0400;
        const uint WM_YW7000NET_COMMAND = WM_USER + 100;
        private void InitOfflineCPHSet()
        {
            //dgvOutName.DataContext = cds;
            //dgvOutName.IsReadOnly = true;
            dgvOutName.AutoGenerateColumns = false;
            dgvOutName.ItemsSource = cds;
            dgvOutName.PreviewMouseLeftButtonUp += DataGrid_PreviewMouseLeftButtonUp;

            dtYiWei = new DataTable();
            dtYiWei.Columns.Add("CPH", System.Type.GetType("System.String"));
            dtYiWei.Columns.Add("StartTime", System.Type.GetType("System.String"));
            dtYiWei.Columns.Add("EndTime", System.Type.GetType("System.String"));
            dtYiWei.Columns.Add("IsQiY", System.Type.GetType("System.String"));
            dtYiWei.Columns.Add("Type", System.Type.GetType("System.String"));

            for (int i = 0; i < 14; i++)
            {
                comboBox9.Items.Add(i);
                comboBox8.Items.Add(i + 1);
            }

            comboBox8.Text = "1";
            comboBox9.Text = "7";




            //成都臻识
            VzClientSDK.VzLPRClient_Setup();


            //深圳芊熠 
            MyClass.Net_Init();

            int ptLen = 255;
            StringBuilder strVersion = new StringBuilder(ptLen);
            MyClass.Net_GetSdkVersion(strVersion, ref ptLen);

            if (Model.bStartZNYKT13)
            {
                rdbZNYKTY13.Visibility = System.Windows.Visibility.Visible;
                HHERR_CODE errCodeYw = YW7000NetClient.YW7000NET_Startup(((HwndSource)PresentationSource.FromVisual(dev_treeview)).Handle, WM_YW7000NET_COMMAND, 0, false, false, string.Empty);
                if (errCodeYw != HHERR_CODE.HHERR_SUCCESS)
                {
                }

                YW7000PlayerSDK.YW7000PLAYER_InitSDK(((HwndSource)PresentationSource.FromVisual(dev_treeview)).Handle);
                YW7000PlayerSDK.YW7000PLAYER_SetDecoderQulity(false);
            }

            //hwndMain = ((HwndSource)PresentationSource.FromVisual(dev_treeview)).Handle;

            //listbt.IsEnabled = false;
            button1.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;
            button7.IsEnabled = false;
            button8.IsEnabled = false;
            btnClose.IsEnabled = false;
            button5.Visibility = System.Windows.Visibility.Hidden;

            if (Model.bAppEnable)
            {
                //TabPage tp2 = tabPage2;//在这里先保存，以便以后还要显示
                //tabControl1.TabPages.Remove(tp2);//隐藏（删除）

                tcOfflineCPH.Items.Remove(tiCamera);
            }
        }

        #region 主板下载

        private void btnCPHDown_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            List<CardIssue> lstIssue;
            List<CheDaoSet> lstCheDao = null;
            List<QueryConditionGroup> lstCondition;

            lstCheDao = new List<CheDaoSet>();
            for (int i = 0; i < dgvOutName.Items.Count; i++)
            {
                FrameworkElement item = dgvOutName.Columns[0].GetCellContent(dgvOutName.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    lstCheDao.Add(dgvOutName.Items[i] as CheDaoSet);
                }
            }
            if (lstCheDao.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }

            btnCPHDown.IsEnabled = false;
            btnOverDown.IsEnabled = false;
            btnTKDown.IsEnabled = false;

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CardState", "=", "0");
            lstCondition[0].Add("SubSystem", "like", "1%");
            lstCondition[0].Add("CPHDownloadSignal", "like", "".PadLeft((Model.stationID < 1 ? 0 : Model.stationID - 1), '_') + "0%");
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[1].Add("CarCardType", "like", "Mth%", "or");
            lstCondition[1].Add("CarCardType", "like", "Fre%", "or");
            lstCondition[1].Add("CarCardType", "like", "Str%", "or");

            req = new Request();
            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CardNO");

            System.Threading.ThreadPool.QueueUserWorkItem(DownloadCPHToBoard, lstIssue);

            //Downds = Ibll.GetFaXingCPHDownLoad(Model.iWorkstationNo);
            //label7.Text = "0";
            //fThread = new Thread(new ThreadStart(SleepT));//开辟一个新的线程
            //fThread.Start();
            //threadState = 1;
            //LogCont = 1;
        }

        private void DownloadCPHToBoard(object state)
        {
            Request req;
            SedBll sendbll;
            List<CardIssue> lstIssue = null;
            List<CheDaoSet> lstCheDao = null;
            List<QueryConditionGroup> lstCondition;

            try
            {
                lstIssue = state as List<CardIssue>;

                this.Dispatcher.Invoke(() =>
                {
                    //初始化进度条
                    progressBar1.Value = 0;
                    progressBar1.Maximum = null == lstIssue ? 0 : lstIssue.Count;

                    //缓存已选择的车道
                    lstCheDao = new List<CheDaoSet>();
                    for (int i = 0; i < dgvOutName.Items.Count; i++)
                    {
                        FrameworkElement item = dgvOutName.Columns[0].GetCellContent(dgvOutName.Items[i]);
                        CheckBox cb = (item as CheckBox);
                        if (cb.IsChecked == true)
                        {
                            lstCheDao.Add(dgvOutName.Items[i] as CheDaoSet);
                        }
                    }
                });

                //没有数据的情况
                if (null == lstIssue || lstIssue.Count <= 0 || null == lstCheDao || lstCheDao.Count <= 0)
                {
                    ShowProgress(progressBar1, lblStep, 0);
                    DownloadCPHFinished();
                    return;
                }

                req = new Request();
                sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                for (int idx = 0; idx < lstIssue.Count; idx++)
                {
                    CardIssue ci = lstIssue[idx];

                    string strSumD = "";
                    string strD = "";
                    string strJiHaoSum = ci.CarValidMachine;
                    //string strJiHao = ci.CarValidMachine;   //dr["CarValidMachine"].ToString();

                    int biaozhi = Model.stationID;  //Model.PubVal.iWorkstationNo;//需要替换的标志位
                    string strbiaozhi = ci.CPHDownloadSignal;   //dr["CPHDownloadSignal"].ToString();
                    string sumBiao = "";
                    string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                    string str2 = strbiaozhi.Substring(biaozhi);
                    sumBiao = str1 + "1" + str2;

                    string CarNO = ci.CardNO;   //dr["CardNO"].ToString();

                    //foreach (char a in strJiHao.ToCharArray())
                    //{
                    //    strJiHaoSum += ConvertToBin(a);
                    //}

                    //if (ci.CPH != "" && ci.CPH != "66666666" && ci.CPH != "88888888" && ci.CPH.Length > 6)
                    if (null != ci.CPH && ci.CPH != "" && ci.CPH != "66666666" && ci.CPH != "88888888" && ci.CPH.Length > 6)
                    {
                        for (int i = 0; i < lstCheDao.Count; i++)
                        {
                            strSumD = strSumD + "1";
                            int JiHao = lstCheDao[i].CtrlNumber;
                            string strIP = lstCheDao[i].IP;
                            int xieYi = lstCheDao[i].XieYi;
                            if (strJiHaoSum.Substring(JiHao - 1, 1) == "0")
                            {
                                string a = "";
                                if (xieYi == 1)
                                {
                                    //if (Model.PubVal.bDetailLog)
                                    //{
                                    //    CR.CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "下载数据：" + CR.CR.GetDownLoadToCPH(dr));
                                    //}
                                    a = sendbll.DownloadCard(strIP, JiHao, CR.GetDownLoadToCPH(ci), xieYi);
                                    if (a == "0")
                                    {
                                        strD = strD + "1";
                                    }
                                }
                                //else if (xieYi == 0)
                                //{
                                //    short Ji = Convert.ToInt16(JiHao);
                                //    string strsend = CR.GetDownLoadToCPH(ci);
                                //    a = axZNYKT_1.LoadDinnerTime2010ZNYKT_(ref Ji, ref strsend);
                                //    if (a == "0")
                                //    {
                                //        strD = strD + "1";
                                //    }
                                //}
                                //bll.AddOffLine(ci.CPH, CR.GetCPHtoCardNO(ci.CPH), 0);
                            }
                            else
                            {
                                string a = "";
                                if (xieYi == 1)
                                {
                                    a = sendbll.DownLossloadCard(strIP, JiHao, CR.GetDownLoadToCPH(ci), xieYi);
                                    if (a == "0")
                                    {
                                        strD = strD + "1";
                                    }
                                }
                                //else if (xieYi == 0)
                                //{
                                //    short Ji = Convert.ToInt16(JiHao);
                                //    string strsend = CR.GetDownLoadToCPH(ci);
                                //    a = axZNYKT_1.DeleteDinnerTime2010ZNYKT_(ref Ji, ref strsend);
                                //    if (a == "0")
                                //    {
                                //        strD = strD + "1";
                                //    }
                                //}
                                //bll.AddOffLine(ci.CPH, CR.GetCPHtoCardNO(ci.CPH), 1);
                            }
                        }
                        if (strSumD.Length > 0)
                        {
                            if (strD == strSumD)  //2015-09-18
                            {
                                //Ibll.UpdateCPHDownLoad(CarNO, sumBiao);

                                //iCout++;
                                //this.Dispatcher.Invoke(() =>
                                //{
                                //    listBox2.Items.Add(ci.CPH + "：下载成功");
                                //    listBox2.Items.Add("===============================");
                                //    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                                //    label10.Text = iCout.ToString();
                                //    progressBar2.Value = iCout;
                                //});

                                lstCondition = new List<QueryConditionGroup>();
                                lstCondition.Add(new QueryConditionGroup());
                                lstCondition[0].Add("CardNO", "=", ci.CardNO);
                                ci.CPHDownloadSignal = sumBiao;
                                long result = req.UpdateField("UpdateCardIssueFields", new { CPHDownloadSignal = ci.CPHDownloadSignal }, lstCondition);

                                ShowMsg(ci.CPH + ":下载成功");
                                ShowMsg("===============================");
                                ShowProgress(progressBar1, lblStep, idx + 1);
                            }
                            else
                            {
                                //if (Model.PubVal.bDetailLog)
                                //{
                                //    CR.CR.WriteToTxtFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "****" + "strD=" + strD + "----strSumD=" + strSumD);
                                //}
                                //iCout++;
                                //this.Dispatcher.Invoke(() =>
                                //{
                                //    listBox2.Items.Add(ci.CPH + "：下载失败");
                                //    listBox2.Items.Add("===============================");
                                //    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                                //    label10.Text = iCout.ToString();
                                //    progressBar2.Value = iCout;
                                //});

                                ShowMsg(ci.CPH + ":下载失败");
                                ShowMsg("===============================");
                                ShowProgress(progressBar1, lblStep, idx + 1);
                            }
                        }
                    }
                    else
                    {
                        //Ibll.UpdateCPHDownLoad(CarNO, sumBiao);
                        //iCout++;
                        //this.Dispatcher.Invoke(() =>
                        //{
                        //    listBox2.Items.Add(ci.CPH + "：不规则请修改");
                        //    listBox2.Items.Add("===============================");
                        //    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                        //    label10.Text = iCout.ToString();
                        //    progressBar2.Value = iCout;
                        //});

                        lstCondition = new List<QueryConditionGroup>();
                        lstCondition.Add(new QueryConditionGroup());
                        lstCondition[0].Add("CardNO", "=", ci.CardNO);
                        ci.CPHDownloadSignal = sumBiao;
                        long result = req.UpdateField("UpdateCardIssueFields", new { CPHDownloadSignal = ci.CPHDownloadSignal }, lstCondition);

                        ShowMsg(ci.CPH + ":不规则请修改");
                        ShowMsg("===============================");
                        ShowProgress(progressBar1, lblStep, idx + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog("车场设置:脱机车牌下载异常", ex.Message + "\r\n" + ex.StackTrace);
            }

            DownloadCPHFinished();
        }

        private void ShowMsg(string msg)
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() =>
                {
                    ShowMsg(msg);
                });
                return;
            }

            listBox2.Items.Add(msg);
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        private void ShowProgress(ProgressBar pb, Label lbl, int step)
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() =>
                {
                    ShowProgress(pb, lbl, step);
                });
                return;
            }

            pb.Value = step;
            lbl.Content = string.Format("{0} / {1}", step, pb.Maximum);
        }

        private void DownloadCPHFinished()
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() =>
                {
                    DownloadCPHFinished();
                });
                return;
            }

            btnCPHDown.IsEnabled = true;
            btnOverDown.IsEnabled = true;
            btnTKDown.IsEnabled = true;
        }

        private void btnOverDown_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            List<CardIssue> lstIssue;
            List<CheDaoSet> lstCheDao = null;
            List<QueryConditionGroup> lstCondition;

            lstCheDao = new List<CheDaoSet>();
            for (int i = 0; i < dgvOutName.Items.Count; i++)
            {
                FrameworkElement item = dgvOutName.Columns[0].GetCellContent(dgvOutName.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    lstCheDao.Add(dgvOutName.Items[i] as CheDaoSet);
                }
            }
            if (lstCheDao.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }

            btnCPHDown.IsEnabled = false;
            btnOverDown.IsEnabled = false;
            btnTKDown.IsEnabled = false;

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CardState", "=", "0");
            lstCondition[0].Add("SubSystem", "like", "1%");
            //lstCondition[0].Add("CPHDownloadSignal", "like", "".PadLeft((Model.stationID < 1 ? 0 : Model.stationID - 1), '_') + "0%");
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[1].Add("CarCardType", "like", "Mth%", "or");
            lstCondition[1].Add("CarCardType", "like", "Fre%", "or");
            lstCondition[1].Add("CarCardType", "like", "Str%", "or");

            req = new Request();
            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CardNO");

            System.Threading.ThreadPool.QueueUserWorkItem(DownloadCPHToBoard, lstIssue);
        }

        private void btnTKDown_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            List<CardIssue> lstIssue;
            List<CheDaoSet> lstCheDao = null;
            List<QueryConditionGroup> lstCondition;

            lstCheDao = new List<CheDaoSet>();
            for (int i = 0; i < dgvOutName.Items.Count; i++)
            {
                FrameworkElement item = dgvOutName.Columns[0].GetCellContent(dgvOutName.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    lstCheDao.Add(dgvOutName.Items[i] as CheDaoSet);
                }
            }
            if (lstCheDao.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }

            btnCPHDown.IsEnabled = false;
            btnOverDown.IsEnabled = false;
            btnTKDown.IsEnabled = false;

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CardState", "=", "5");
            lstCondition[0].Add("SubSystem", "like", "1%");
            lstCondition[0].Add("CPHDownloadSignal", "like", "".PadLeft((Model.stationID < 1 ? 0 : Model.stationID - 1), '_') + "0%");
            //lstCondition.Add(new QueryConditionGroup());
            //lstCondition[1].Add("CarCardType", "like", "Mth%", "or");
            //lstCondition[1].Add("CarCardType", "like", "Fre%", "or");
            //lstCondition[1].Add("CarCardType", "like", "Str%", "or");

            req = new Request();
            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CardNO");

            System.Threading.ThreadPool.QueueUserWorkItem(DownloadCPHToBoard, lstIssue);
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            SelectDown frm = new SelectDown();
            frm.Owner = this;
            frm.ShowDialog();
        }
        #endregion

        #region 摄像机下载
        int nCamId = -1;
        int m_hLPRClient;
        int m_nSelIndex = 0;
        int m_nSerialHandle;
        DataTable dtYiWei;
        UI.ParkingMonitoring.VIDEO_WND[] VideoInfo = new UI.ParkingMonitoring.VIDEO_WND[11];
        private const int MSG_PLATE_INFO = 0x902;
        private VzClientSDK.VZLPRC_FIND_DEVICE_CALLBACK find_DeviceCB = null;
        private void startsearchdev_Click(object sender, RoutedEventArgs e)
        {
            dev_treeview.Items.Clear();
            find_DeviceCB = new VzClientSDK.VZLPRC_FIND_DEVICE_CALLBACK(FIND_DEVICE_CALLBACK);
            VzClientSDK.VZLPRClient_StartFindDevice(find_DeviceCB, IntPtr.Zero);
        }
        private void FIND_DEVICE_CALLBACK(string pStrDevName, string pStrIPAddr, ushort usPort1, ushort usPort2, uint SL, uint SH, IntPtr pUserData)
        {
            //             string pStrDev = pStrIPAddr.ToString() + ":" + usPort1.ToString();
            //             ShowDevice(pStrDev);

            string pStrDev = pStrIPAddr.ToString() + ":" + usPort1.ToString();
            string serialNO = SL.ToString() + ":" + SH.ToString();
            //ShowDevice(pStrDev, serialNO);

            VzClientSDK.VZ_LPR_MSG_PLATE_INFO plateInfo = new VzClientSDK.VZ_LPR_MSG_PLATE_INFO();
            plateInfo.plate = pStrDev;

            string path = serialNO;
            plateInfo.img_path = path;
            int size = Marshal.SizeOf(plateInfo);
            IntPtr intptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(plateInfo, intptr, true);

            //Win32API.PostMessage(((HwndSource)PresentationSource.FromVisual(dev_treeview)).Handle, MSG_PLATE_INFO, (int)intptr, 0);

            plateInfo = (VzClientSDK.VZ_LPR_MSG_PLATE_INFO)Marshal.PtrToStructure(intptr, typeof(VzClientSDK.VZ_LPR_MSG_PLATE_INFO));
            //TreeNode node = new TreeNode(plateInfo.plate);
            //node.Tag = plateInfo.img_path;

            this.Dispatcher.Invoke(() =>
            {
                dev_treeview.Items.Add(plateInfo.plate);
            });
        }

        private void stopsearchdev_Click(object sender, RoutedEventArgs e)
        {
            VzClientSDK.VZLPRClient_StopFindDevice();
        }

        private void listbt_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            List<CardIssue> lstIssue;
            List<QueryConditionGroup> lstCondition;

            listbt.IsEnabled = false;
            listAllbt.IsEnabled = false;
            btnTK.IsEnabled = false;

            btnCPHDown.IsEnabled = false;
            btnOverDown.IsEnabled = false;
            btnTKDown.IsEnabled = false;

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CardState", "=", "0");
            lstCondition[0].Add("SubSystem", "like", "1%");
            lstCondition[0].Add("CPHDownloadSignal", "like", "".PadLeft((Model.stationID < 1 ? 0 : Model.stationID - 1), '_') + "0%");
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[1].Add("CarCardType", "like", "Mth%", "or");
            lstCondition[1].Add("CarCardType", "like", "Fre%", "or");
            lstCondition[1].Add("CarCardType", "like", "Str%", "or");

            req = new Request();
            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CardNO");

            System.Threading.ThreadPool.QueueUserWorkItem(DownloadCPHToCamera, lstIssue);
        }

        bool bExit = false;
        private void DownloadCPHToCamera(object state)
        {
            Request req;
            List<CardIssue> lstIssue;
            List<NetCameraSet> lstCam;
            List<QueryConditionGroup> lstCondition;

            lstIssue = state as List<CardIssue>;

            this.Dispatcher.Invoke(() =>
            {
                //初始化进度条
                progressBar3.Value = 0;
                progressBar3.Maximum = null == lstIssue ? 0 : lstIssue.Count;

                ShowProgress(progressBar3, lblStepCam, 0);
            });

            if (null == lstIssue || lstIssue.Count <= 0)
            {
                this.Dispatcher.Invoke(() =>
                {
                    listbt.IsEnabled = true;
                    listAllbt.IsEnabled = true;
                    btnTK.IsEnabled = true;

                    btnCPHDown.IsEnabled = true;
                    btnOverDown.IsEnabled = true;
                    btnTKDown.IsEnabled = true;
                });
                return;
            }

            //thDown.Abort();

            req = new Request();
            Dictionary<int, int> dicList = new Dictionary<int, int>();
            string strCJiHao = "";
            int CheckType = 0;
            this.Dispatcher.Invoke(() =>
            {
                if (rdbZNYKTY5.IsChecked ?? false)
                {
                    CheckType = 0;
                }
                else if (rdbZNYKTY14.IsChecked ?? false)
                {
                    CheckType = 1;
                }
                else if (rdbZNYKTY13.IsChecked ?? false)
                {
                    CheckType = 2;
                }
            });
            //if (rdbZNYKTY5.IsChecked ?? false)
            if (CheckType == 0)
            {
                for (int Count = 0; Count < Model.iChannelCount; Count++)
                {
                    if (bExit)
                    {
                        return;
                    }

                    //DataTable lstCam = bll.SelectVideo(Convert.ToInt32(Model.Channels[Count].sIDAddress));

                    if (null == Model.Channels[Count].sIDAddress || Model.Channels[Count].sIDAddress.Trim().Length <= 0)
                    {
                        continue;
                    }

                    lstCondition = new List<QueryConditionGroup>();
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[0].Add("VideoIP", "=", Model.Channels[Count].sIDAddress);

                    lstCam = req.GetData<List<NetCameraSet>>("GetNetCameraSet", null, lstCondition);
                    if (null == lstCam || lstCam.Count <= 0)
                    {
                        continue;
                    }

                    strCJiHao += "1";

                    m_hLPRClient = VzClientSDK.VzLPRClient_Open(lstCam[0].VideoIP, Convert.ToUInt16(lstCam[0].VideoPort), lstCam[0].VideoUserName, lstCam[0].VideoPassWord);
                    if (m_hLPRClient > 0)
                    {
                        dicList[m_hLPRClient] = Model.Channels[Count].iCtrlID;
                        this.Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add("机号：【" + Model.Channels[Count].iCtrlID + "】---摄像机：" + lstCam[0].VideoIP + "  打开成功！");
                            listBox1.Items.Add("===============================");
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add("机号：【" + Model.Channels[Count].iCtrlID + "】---摄像机：" + lstCam[0].VideoIP + "  打开失败！");
                            listBox1.Items.Add("===============================");
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                    }
                }

                if (null == strCJiHao || "" == strCJiHao.Trim())
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        listbt.IsEnabled = true;
                        listAllbt.IsEnabled = true;
                        btnTK.IsEnabled = true;

                        btnCPHDown.IsEnabled = true;
                        btnOverDown.IsEnabled = true;
                        btnTKDown.IsEnabled = true;
                    });
                    return;
                }

                m_hLPRClient = 0;

                int k = 0;
                //this.Dispatcher.Invoke(() =>
                //{
                //    label5.Text = Downds.Tables[0].Rows.Count.ToString();
                //    progressBar2.Maximum = Downds.Tables[0].Rows.Count;
                //});

                //foreach (DataRow dr in Downds.Tables[0].Rows)
                foreach (var ci in lstIssue)
                {
                    if (bExit)
                    {
                        return;
                    }
                    k++;
                    //int biaozhi = Model.iWorkstationNo;//需要替换的标志位

                    int biaozhi = Model.stationID;
                    string strbiaozhi = ci.CPHDownloadSignal;   //dr["CPHDownloadSignal"].ToString();
                    string sumBiao = "";
                    string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                    string str2 = strbiaozhi.Substring(biaozhi);
                    sumBiao = str1 + "1" + str2;
                    //progressBar1.Value += progressBar1.Step;
                    string CarNO = ci.CardNO;   //dr["CardNO"].ToString();
                    //string strJiHao = ci.CarValidMachine;   //dr["CarValidMachine"].ToString();

                    string strJiHaoSum = ci.CarValidMachine;
                    //foreach (char a in strJiHao.ToCharArray())
                    //{
                    //    strJiHaoSum += ConvertToBin(a);
                    //}

                    string strYDownLoadJihao = "";//下载机号
                    for (int i = 0; i < 128; i++)
                    {
                        strYDownLoadJihao += "0";
                    }
                    string strSDownLoadJihao = "";//下载成功
                    for (int i = 0; i < 128; i++)
                    {
                        strSDownLoadJihao += "0";
                    }
                    //替换成车道
                    //Dictionary<int, string> dic = CR.CR.GetIP();
                    //dic[1] = "192.168.1.99";
                    //dic[3] = "192.168.1.98";

                    SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                    string QstrJiHao = "";
                    if (ci.CPH != "" && ci.CPH != "66666666" && ci.CPH != "88888888" && ci.CPH.Length > 6)
                    {
                        foreach (var dic in dicList)
                        {
                            //ShowProgress(progressBar3, lblStepCam, 0);

                            if (bExit)
                            {
                                return;
                            }
                            VzClientSDK.VzLPRClient_WhiteListDeleteVehicle(dic.Key, ci.CPH);
                            if (strJiHaoSum.Substring(dic.Value - 1, 1) == "0")//判断这张卡片是否发行这个车道
                            {
                                //                             strCJiHao = GetJihao(strYDownLoadJihao, dic.Value);
                                //                             strYDownLoadJihao = strCJiHao;
                                string a = "";

                                //                                 VzClientSDK.VZ_LPR_WLIST_VEHICLE wlistVehicle = new VzClientSDK.VZ_LPR_WLIST_VEHICLE();
                                // 
                                //                                 wlistVehicle.bAlarm = 0;
                                // 
                                //                                 wlistVehicle.bEnable = 1;
                                // 
                                //                                 wlistVehicle.uCustomerID = 1;
                                //                                 wlistVehicle.strPlateID = ci.CPH;
                                //                                 wlistVehicle.bUsingTimeSeg = 1;
                                //                                 //                             wlistVehicle.iColor = 0;											/**<车辆颜色*/
                                //                                 //                             wlistVehicle.iPlateType = 0;
                                // 
                                //                                 wlistVehicle.strCode = wlistVehicle.strPlateID;
                                // 
                                //                                 wlistVehicle.strComment = "";
                                // 
                                //                                 wlistVehicle.bUsingTimeSeg = 1;
                                // 
                                //                                 Console.WriteLine(wlistVehicle.strPlateID);
                                //                                 DateTime endTime = Convert.ToDateTime(dr["CarValidEndDate"].ToString());
                                //                                 VzClientSDK.VZ_TM struTMOverdule = new VzClientSDK.VZ_TM();
                                //                                 struTMOverdule.nHour = Int16.Parse(endTime.Hour.ToString());
                                //                                 struTMOverdule.nMin = Int16.Parse(endTime.Minute.ToString());
                                //                                 struTMOverdule.nSec = Int16.Parse(endTime.Second.ToString());
                                //                                 struTMOverdule.nYear = Int16.Parse(endTime.Year.ToString());
                                //                                 struTMOverdule.nMonth = Int16.Parse(endTime.Month.ToString());
                                //                                 struTMOverdule.nMDay = Int16.Parse(endTime.Day.ToString());
                                //                                 wlistVehicle.pStruTMOverdule = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VzClientSDK.VZ_TM)));
                                //                                 Marshal.StructureToPtr(struTMOverdule, wlistVehicle.pStruTMOverdule, true);
                                //                                 //string data = datalist.Value.ToString();
                                // 
                                // 
                                //                                 VzClientSDK.VZ_LPR_WLIST_ROW wlist = new VzClientSDK.VZ_LPR_WLIST_ROW();
                                //                                 wlist.pVehicle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VzClientSDK.VZ_LPR_WLIST_VEHICLE)));
                                //                                 Marshal.StructureToPtr(wlistVehicle, wlist.pVehicle, true);
                                // 
                                //                                 VzClientSDK.VZ_LPR_WLIST_IMPORT_RESULT importResult = new VzClientSDK.VZ_LPR_WLIST_IMPORT_RESULT();
                                // 
                                //                                 int iRst = VzClientSDK.VzLPRClient_WhiteListImportRows(dic.Key, 1, ref wlist, ref importResult);
                                // 
                                //                                 Marshal.FreeHGlobal(wlistVehicle.pStruTMOverdule);
                                //                                 Marshal.FreeHGlobal(wlist.pVehicle);
                                // 
                                // 
                                // 



                                VzClientSDK.VZ_LPR_WLIST_VEHICLE wlistVehicle = new VzClientSDK.VZ_LPR_WLIST_VEHICLE();


                                wlistVehicle.bAlarm = 0;
                                wlistVehicle.bEnable = 1;

                                wlistVehicle.uCustomerID = 1;
                                wlistVehicle.strPlateID = ci.CPH;
                                DateTime endTime = ci.CarValidEndDate;
                                wlistVehicle.struTMOverdule.nHour = Int16.Parse(endTime.Hour.ToString());
                                wlistVehicle.struTMOverdule.nMin = Int16.Parse(endTime.Minute.ToString());
                                wlistVehicle.struTMOverdule.nSec = Int16.Parse(endTime.Second.ToString());
                                wlistVehicle.struTMOverdule.nYear = Int16.Parse(endTime.Year.ToString());
                                wlistVehicle.struTMOverdule.nMonth = Int16.Parse(endTime.Month.ToString());
                                wlistVehicle.struTMOverdule.nMDay = Int16.Parse(endTime.Day.ToString());

                                //string data = datalist.Value.ToString();
                                wlistVehicle.bUsingTimeSeg = 1;
                                wlistVehicle.bEnableTMOverdule = 1;
                                wlistVehicle.iColor = 0;											/**<车辆颜色*/
                                wlistVehicle.iPlateType = 0;

                                wlistVehicle.strCode = ci.CPH;
                                wlistVehicle.strComment = "";

                                VzClientSDK.VZ_LPR_WLIST_ROW wlist = new VzClientSDK.VZ_LPR_WLIST_ROW();
                                wlist.pVehicle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VzClientSDK.VZ_LPR_WLIST_VEHICLE)));
                                Marshal.StructureToPtr(wlistVehicle, wlist.pVehicle, true);

                                VzClientSDK.VZ_LPR_WLIST_IMPORT_RESULT importResult = new VzClientSDK.VZ_LPR_WLIST_IMPORT_RESULT();

                                int iRst = VzClientSDK.VzLPRClient_WhiteListImportRows(dic.Key, 1, ref wlist, ref importResult);

                                Marshal.FreeHGlobal(wlist.pVehicle);



                                this.Dispatcher.Invoke(() =>
                                {
                                    if (iRst == 0)
                                    {
                                        QstrJiHao += "1";
                                        listBox1.Items.Add(ci.CPH + "  在【" + dic.Value + "】号机下载成功！");
                                        listBox1.Items.Add("===============================");
                                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                                        //Thread.Sleep(100);
                                        //                                      QstrJiHao = GetJihao(strSDownLoadJihao, dic.Value);
                                        //                                      strSDownLoadJihao = QstrJiHao;
                                        //Ibll.UpdateDownLoad(CarNO, sumBiao);
                                    }
                                    else
                                    {
                                        listBox1.Items.Add(ci.CPH + "  在【" + dic.Value + "】号机下载失败！");
                                        listBox1.Items.Add("===============================");
                                        listBox1.SelectedIndex = listBox1.Items.Count - 1;

                                    }
                                });
                                Thread.Sleep(100);
                                //Ibll.UpdateDownLoad(CarNO, sumBiao);
                            }
                            else
                            {
                                VzClientSDK.VzLPRClient_WhiteListDeleteVehicle(dic.Key, ci.CPH);
                                QstrJiHao += "1";
                                string a = "";
                            }
                        }
                        //string strJihao = "";
                        //for (int j = 0; j < QstrJiHao.Length / 4; j++)
                        //{
                        //    strJihao += string.Format("{0:X}", Convert.ToInt32(QstrJiHao.Substring(j * 4, 4), 2));
                        //}
                        //string strTemp1 = strJihao;
                        if (QstrJiHao == strCJiHao)
                        {
                            //Ibll.UpdateCPHDownLoad(CarNO, sumBiao);

                            lstCondition = new List<QueryConditionGroup>();
                            lstCondition.Add(new QueryConditionGroup());
                            lstCondition[0].Add("CardNO", "=", ci.CardNO);
                            ci.CPHDownloadSignal = sumBiao;
                            long result = req.UpdateField("UpdateCardIssueFields", new { CPHDownloadSignal = ci.CPHDownloadSignal }, lstCondition);
                        }
                    }
                    else
                    {
                        //Ibll.UpdateCPHDownLoad(CarNO, sumBiao);

                        lstCondition = new List<QueryConditionGroup>();
                        lstCondition.Add(new QueryConditionGroup());
                        lstCondition[0].Add("CardNO", "=", ci.CardNO);
                        ci.CPHDownloadSignal = sumBiao;
                        long result = req.UpdateField("UpdateCardIssueFields", new { CPHDownloadSignal = ci.CPHDownloadSignal }, lstCondition);
                    }

                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    //label7.Text = k.ToString();
                    //    //progressBar1.Value = k;

                    //    ShowProgress(progressBar3, lblStepCam, k);
                    //});
                    ShowProgress(progressBar3, lblStepCam, k);
                    //else
                    //{
                    //    Ibll.UpdateICLost(CarNO, 1, strTemp1, 2);
                    //}
                }
                if (this.CheckAccess())
                {
                    listbt.IsEnabled = true;
                    listAllbt.IsEnabled = true;
                    btnTK.IsEnabled = true;

                    btnCPHDown.IsEnabled = true;
                    btnOverDown.IsEnabled = true;
                    btnTKDown.IsEnabled = true;
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        listbt.IsEnabled = true;
                        listAllbt.IsEnabled = true;
                        btnTK.IsEnabled = true;

                        btnCPHDown.IsEnabled = true;
                        btnOverDown.IsEnabled = true;
                        btnTKDown.IsEnabled = true;
                    });
                }
                foreach (var dic in dicList)
                {
                    VzClientSDK.VzLPRClient_Close(dic.Key);
                    this.Dispatcher.Invoke(() =>
                    {
                        listBox1.Items.Add("机号：【" + dic.Value + "】---摄像机连接已关闭！");
                        listBox1.Items.Add("===============================");
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    });
                }
            }
            //else if (rdbZNYKTY14.IsChecked ?? false)
            else if (CheckType == 1)
            {
                m_hLPRClient = 0;

                int k = 0;
                //this.Dispatcher.Invoke(() =>
                //{
                //    label5.Text = Downds.Tables[0].Rows.Count.ToString();
                //    progressBar1.Maximum = Downds.Tables[0].Rows.Count;
                //});
                for (int Count = 0; Count < Model.iChannelCount; Count++)
                {
                    if (bExit)
                    {
                        return;
                    }

                    string strAucLplPathAdd = System.Windows.Forms.Application.StartupPath + "\\" + DateTime.Now.ToShortDateString() + "" + Model.Channels[Count].iCtrlID + "lprAdd.ini";
                    if (File.Exists(strAucLplPathAdd))
                    {
                        File.Delete(strAucLplPathAdd);
                    }
                    string strAucLplPathDelete = System.Windows.Forms.Application.StartupPath + "\\" + DateTime.Now.ToShortDateString() + "" + Model.Channels[Count].iCtrlID + "lprDelete.ini";
                    if (File.Exists(strAucLplPathDelete))
                    {
                        File.Delete(strAucLplPathDelete);
                    }

                    //foreach (DataRow dr in Downds.Tables[0].Rows)
                    foreach (CardIssue ci in lstIssue)
                    {
                        if (bExit)
                        {
                            return;
                        }
                        k++;
                       // int biaozhi = Model.iWorkstationNo;//需要替换的标志位

                        int biaozhi = Model.stationID;
                        string strbiaozhi = ci.CPHDownloadSignal;   //dr["CPHDownloadSignal"].ToString();
                        string sumBiao = "";
                        string str1 = strbiaozhi.Substring(0, biaozhi - 1);
                        string str2 = strbiaozhi.Substring(biaozhi);
                        sumBiao = str1 + "1" + str2;
                        //progressBar1.Value += progressBar1.Step;
                        string CarNO = ci.CardNO;   //dr["CardNO"].ToString();
                        string strJiHao = ci.CarValidMachine;    //dr["CarValidMachine"].ToString();

                        string strJiHaoSum = "";
                        foreach (char a in strJiHao.ToCharArray())
                        {
                            strJiHaoSum += ConvertToBin(a);
                        }

                        string strYDownLoadJihao = "";//下载机号
                        for (int i = 0; i < 128; i++)
                        {
                            strYDownLoadJihao += "0";
                        }
                        string strSDownLoadJihao = "";//下载成功
                        for (int i = 0; i < 128; i++)
                        {
                            strSDownLoadJihao += "0";
                        }

                        SedBll sendbll = new SedBll(Model.CtrIP, 1007, 1005);
                        string QstrJiHao = "";
                        if (ci.CPH != "" && ci.CPH != "66666666" && ci.CPH != "88888888" && ci.CPH.Length > 6)
                        {
                            if (strJiHaoSum.Substring(Model.Channels[Count].iCtrlID - 1, 1) == "0")//判断这张卡片是否发行这个车道
                            {

                                string strCPH = "&" + ci.CPH + "@" + ci.CarValidStartDate.ToString("yyyyMMddHHmmss") + "$" + ci.CarValidEndDate.ToString("yyyyMMddHHmmss");
                                //WriteToTxtFile(strCPH, strAucLplPathAdd);
                                this.Dispatcher.Invoke(() =>
                                {
                                    listBox1.Items.Add(ci.CPH + "  在【" + Model.Channels[Count].iCtrlID + "】号机下载成功！");
                                    listBox1.Items.Add("===============================");
                                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                                });
                            }
                            else
                            {
                                string strCPH = "&" + ci.CPH + "@" + ci.CarValidStartDate.ToString("yyyyMMddHHmmss") + "$" + ci.CarValidEndDate.ToString("yyyyMMddHHmmss");
                                //WriteToTxtFile(strCPH, strAucLplPathDelete);
                            }
                        }


                    }

                    //DataTable Videodt = bll.SelectVideo(Convert.ToInt32(Model.Channels[Count].sIDAddress));

                    lstCondition = new List<QueryConditionGroup>();
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[0].Add("VideoIP", "=", Model.Channels[Count].sIDAddress);

                    lstCam = req.GetData<List<NetCameraSet>>("GetNetCameraSet", null, lstCondition);


                    nCamId = MyClass.Net_AddCamera(lstCam[0].VideoIP);

                    int iRet = MyClass.Net_ConnCamera(nCamId, 30000, 10);


                    MyClass.T_BlackWhiteList tBalckWhiteList = new MyClass.T_BlackWhiteList();
                    tBalckWhiteList.LprMode = 1;
                    tBalckWhiteList.Lprnew = 1;
                    byte[] aucLplPath = System.Text.Encoding.Default.GetBytes(strAucLplPathAdd);
                    tBalckWhiteList.aucLplPath = new byte[256];
                    aucLplPath.CopyTo(tBalckWhiteList.aucLplPath, 0);

                    IntPtr ptBalckWhiteList = Marshal.AllocHGlobal(Marshal.SizeOf(tBalckWhiteList));
                    Marshal.StructureToPtr(tBalckWhiteList, ptBalckWhiteList, true);
                    int iRetQY = MyClass.Net_BlackWhiteListSend(nCamId, ptBalckWhiteList);
                    if (iRetQY == 0)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add("新增白名单成功");
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add("新增白名单失败");
                        });
                    }
                    Marshal.FreeHGlobal(ptBalckWhiteList);
                    MyClass.Net_DelCamera(nCamId);
                    MyClass.Net_DisConnCamera(nCamId);

                }
                this.Dispatcher.Invoke(() =>
                {
                    listbt.IsEnabled = true;
                    listAllbt.IsEnabled = true;
                    btnTK.IsEnabled = true;

                    btnCPHDown.IsEnabled = true;
                    btnOverDown.IsEnabled = true;
                    btnTKDown.IsEnabled = true;
                });
            }
            //else if (rdbZNYKTY13.IsChecked ?? false)
            else if (CheckType == 2)
            {
                int k = 0;
                //this.Dispatcher.Invoke(() =>
                //{
                //    label5.Text = Downds.Tables[0].Rows.Count.ToString();
                //    progressBar1.Maximum = Downds.Tables[0].Rows.Count;
                //});
                for (int Count = 0; Count < Model.iChannelCount; Count++)
                {
                    k = 0;
                    if (bExit)
                    {
                        return;
                    }
                    //DataTable Videodt = bll.SelectVideo(Convert.ToInt32(Model.Channels[Count].sIDAddress));
                    //// m_hLPRClient = VzClientSDK.VzLPRClient_Open(Videodt.Rows[0]["VideoIP"].ToString(), ushort.Parse(Videodt.Rows[0]["VideoPort"].ToString()), Videodt.Rows[0]["VideoUserName"].ToString(), Videodt.Rows[0]["VideoPassWord"].ToString());

                    lstCondition = new List<QueryConditionGroup>();
                    lstCondition.Add(new QueryConditionGroup());
                    lstCondition[0].Add("VideoIP", "=", Model.Channels[Count].sIDAddress);

                    lstCam = req.GetData<List<NetCameraSet>>("GetNetCameraSet", null, lstCondition);

                    HHERR_CODE errCode = YW7000NetClient.YW7000NET_LogonServer(lstCam[0].VideoIP, ushort.Parse(lstCam[0].VideoPort),
                        "", lstCam[0].VideoUserName, lstCam[0].VideoPassWord, (uint)Count, out VideoInfo[Count].m_hLogon, IntPtr.Zero);

                    int i, j;

                    YW7000NetClient.CAR_WHITELIST_INFO_S stWhiteListInfo;
                    uint dwAppend, config_len;
                    dtYiWei.Clear();
                    config_len = (uint)Marshal.SizeOf(typeof(YW7000NetClient.CAR_WHITELIST_INFO_S));
                    IntPtr ptrWhiteListInfo = Marshal.AllocHGlobal((int)config_len);
                    for (i = 0; i < 1000; i++)
                    {
                        dwAppend = (uint)i;
                        errCode = YW7000NetClient.YW7000NET_GetServerConfig(VideoInfo[Count].m_hLogon, HHCMD_NET.HHCMD_GET_WHITELIST, ptrWhiteListInfo, ref config_len, ref dwAppend);
                        if (errCode != HHERR_CODE.HHERR_SUCCESS)
                        {
                            MessageBox.Show(this, "获取白名单失败!", "提示");
                            break;
                        }
                        stWhiteListInfo = (YW7000NetClient.CAR_WHITELIST_INFO_S)Marshal.PtrToStructure(ptrWhiteListInfo, typeof(YW7000NetClient.CAR_WHITELIST_INFO_S));

                        //chkUseWhite.IsChecked ??  false = (stWhiteListInfo.bEnable & 1) != 0;  是否启用黑名单


                        for (j = 0; j < stWhiteListInfo.dwCount; j++)
                        {
                            ChangeWhiteList(stWhiteListInfo.stWhitelist[j], -1);
                        }
                        if (stWhiteListInfo.bFinished != 0)
                            break;
                    }
                    Marshal.FreeHGlobal(ptrWhiteListInfo);

                    //foreach (DataRow dr in Downds.Tables[0].Rows)
                    foreach (CardIssue ci in lstIssue)
                    {
                        if (bExit)
                        {
                            return;
                        }
                        k++;
                        DataRow[] drArr = dtYiWei.Select("CPH='" + ci.CPH + "'");
                        if (drArr.Length > 0)
                        {
                            dtYiWei.Rows.Remove(drArr[0]);
                        }
                        DataRow drr = dtYiWei.NewRow();

                        drr["CPH"] = ci.CPH;
                        drr["StartTime"] = ci.CarValidStartDate;    //dr["CarValidStartDate"].ToString();
                        drr["EndTime"] = ci.CarValidEndDate;    //dr["CarValidEndDate"].ToString();
                        drr["IsQiY"] = "是";
                        drr["Type"] = "白名单";
                        dtYiWei.Rows.Add(drr);

                        this.Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add(ci.CPH + "  车牌增加成功！");
                            listBox1.Items.Add("===============================");
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                            //label7.Text = k.ToString();
                            //progressBar1.Value = k;
                            ShowProgress(progressBar3, lblStepCam, k);
                        });

                    }

                    int iRst = YW7000NetClient.Whitelist(VideoInfo[Count].m_hLogon, dtYiWei);
                    if (iRst == 0)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add("  白名单增加成功！");
                            listBox1.Items.Add("===============================");
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add("  白名单增加失败！");
                            listBox1.Items.Add("===============================");
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        });
                    }

                    if (VideoInfo[Count].m_hLogon != IntPtr.Zero)
                    {
                        YW7000NetClient.YW7000NET_LogoffServer(VideoInfo[Count].m_hLogon);
                        VideoInfo[m_nSelIndex].m_hLogon = IntPtr.Zero;
                    }

                }

                this.Dispatcher.Invoke(() =>
                {
                    listbt.IsEnabled = true;
                    listAllbt.IsEnabled = true;
                    btnTK.IsEnabled = true;

                    btnCPHDown.IsEnabled = true;
                    btnOverDown.IsEnabled = true;
                    btnTKDown.IsEnabled = true;
                });
            }

            //thDown.Abort();

        }

        private void dev_treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (null == dev_treeview.SelectedItem)
            {
                return;
            }

            string ipstr = dev_treeview.SelectedItem.ToString();
            int temp = ipstr.IndexOf(":");
            txtIP.Text = ipstr.Substring(0, temp);
            txtPort.Text = ipstr.Substring(temp + 1);
        }

        private void ChangeWhiteList(YW7000NetClient.WHITELIST_ITEM_S WhiteItem, int Index)
        {
            DateTime dtTime;
            YW7000NetClient.WHITELIST_ITEM_S NewWhiteItem;

            string strTmp;
            if (Index == -1)
            {
                NewWhiteItem = new YW7000NetClient.WHITELIST_ITEM_S();
            }
            else
            {
            }


            //dtYiWei.Columns.Add("CPH", System.Type.GetType("System.String"));
            //dtYiWei.Columns.Add("StartTime", System.Type.GetType("System.String"));
            //dtYiWei.Columns.Add("EndTime", System.Type.GetType("System.String"));
            //dtYiWei.Columns.Add("IsQiY", System.Type.GetType("System.String"));
            //dtYiWei.Columns.Add("Type", System.Type.GetType("System.String"));




            DataRow dr = dtYiWei.NewRow();

            dr["CPH"] = WhiteItem.szPlateNumber;

            dtTime = YW7000NetClient.GetDateTimeFromValue(WhiteItem.stBgnTime);
            strTmp = dtTime.ToString("yyyy-MM-dd HH:mm:ss");

            dr["StartTime"] = strTmp;

            dtTime = YW7000NetClient.GetDateTimeFromValue(WhiteItem.stEndTime);
            strTmp = dtTime.ToString("yyyy-MM-dd HH:mm:ss");
            dr["EndTime"] = strTmp;
            if (WhiteItem.bEnable != 0)
                strTmp = "是";
            else
                strTmp = "否";
            dr["IsQiY"] = strTmp;
            if (WhiteItem.byLicenseType == 0)
                strTmp = "白名单";
            else
                strTmp = "黑名单";
            dr["Type"] = strTmp;
            dtYiWei.Rows.Add(dr);
        }

        private void listAllbt_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            List<CardIssue> lstIssue;
            List<QueryConditionGroup> lstCondition;

            listbt.IsEnabled = false;
            listAllbt.IsEnabled = false;
            btnTK.IsEnabled = false;

            btnCPHDown.IsEnabled = false;
            btnOverDown.IsEnabled = false;
            btnTKDown.IsEnabled = false;

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CardState", "=", "0");
            lstCondition[0].Add("SubSystem", "like", "1%");
            //lstCondition[0].Add("CPHDownloadSignal", "like", "".PadLeft((Model.stationID < 1 ? 0 : Model.stationID - 1), '_') + "0%");
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[1].Add("CarCardType", "like", "Mth%", "or");
            lstCondition[1].Add("CarCardType", "like", "Fre%", "or");
            lstCondition[1].Add("CarCardType", "like", "Str%", "or");

            req = new Request();
            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CardNO");

            System.Threading.ThreadPool.QueueUserWorkItem(DownloadCPHToCamera, lstIssue);
        }

        private void btnTK_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            List<CardIssue> lstIssue;
            List<QueryConditionGroup> lstCondition;

            listbt.IsEnabled = false;
            listAllbt.IsEnabled = false;
            btnTK.IsEnabled = false;

            btnCPHDown.IsEnabled = false;
            btnOverDown.IsEnabled = false;
            btnTKDown.IsEnabled = false;

            lstCondition = new List<QueryConditionGroup>();
            lstCondition.Add(new QueryConditionGroup());
            lstCondition[0].Add("CardState", "=", "5");
            lstCondition[0].Add("SubSystem", "like", "1%");
            lstCondition[0].Add("CPHDownloadSignal", "like", "".PadLeft((Model.stationID < 1 ? 0 : Model.stationID - 1), '_') + "0%");
            //lstCondition.Add(new QueryConditionGroup());
            //lstCondition[1].Add("CarCardType", "like", "Mth%", "or");
            //lstCondition[1].Add("CarCardType", "like", "Fre%", "or");
            //lstCondition[1].Add("CarCardType", "like", "Str%", "or");

            req = new Request();
            lstIssue = req.GetData<List<CardIssue>>("GetCardIssue", null, lstCondition, "CardNO");

            System.Threading.ThreadPool.QueueUserWorkItem(DownloadCPHToCamera, lstIssue);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (rdbZNYKTY5.IsChecked ?? false)
            {

                short nPort = Int16.Parse(txtPort.Text);

                if (m_hLPRClient > 0)
                {
                    MessageBox.Show("当前窗口已打开设备！");
                    return;
                }

                m_hLPRClient = VzClientSDK.VzLPRClient_Open(txtIP.Text, (ushort)nPort, txtUserName.Text, txtPwd.Password);
                if (m_hLPRClient == 0)
                {
                    MessageBox.Show("打开设备失败！");
                    return;
                }
                m_nSerialHandle = VzClientSDK.VzLPRClient_SerialStart(m_hLPRClient, 1, serialRECV, IntPtr.Zero);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                nCamId = MyClass.Net_AddCamera(txtIP.Text);

                int iRet = MyClass.Net_ConnCamera(nCamId, 30000, 10);
                if (iRet != 0)
                {
                    MyClass.Net_StopVideo(nCamId);
                    MessageBox.Show("连接相机【" + txtIP.Text + "】失败!", "提示");
                    return;
                }
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                short nPort = Int16.Parse(txtPort.Text);
                HHERR_CODE errCode = YW7000NetClient.YW7000NET_LogonServer(txtIP.Text, (ushort)nPort, "", txtUserName.Text, txtPwd.Password,
                    (uint)0, out VideoInfo[0].m_hLogon, IntPtr.Zero);
                if (errCode != HHERR_CODE.HHERR_SUCCESS)
                {
                    MessageBox.Show("连接相机【" + txtIP.Text + "】失败!", "提示");
                    return;
                }
            }
            btnOpen.IsEnabled = false;
            listbt.IsEnabled = false;
            btnTK.IsEnabled = false;
            btnClose.IsEnabled = true;
            button1.IsEnabled = true;
            listAllbt.IsEnabled = false;
            button7.IsEnabled = true;
            button8.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button5.IsEnabled = true;

            btnCPHDown.IsEnabled = false;
            btnOverDown.IsEnabled = false;
            btnTKDown.IsEnabled = false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (rdbZNYKTY5.IsChecked ?? false)
            {
                if (m_hLPRClient > 0)
                {
                    VzClientSDK.VzLPRClient_Close(m_hLPRClient);
                    m_hLPRClient = 0;
                }
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                MyClass.Net_DisConnCamera(m_nSerialHandle);
                MyClass.Net_DelCamera(m_nSerialHandle);
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                if (VideoInfo[0].m_hLogon != IntPtr.Zero)
                {
                    YW7000NetClient.YW7000NET_LogoffServer(VideoInfo[0].m_hLogon);
                    VideoInfo[m_nSelIndex].m_hLogon = IntPtr.Zero;
                }
            }


            btnOpen.IsEnabled = true;
            btnClose.IsEnabled = false;
            listbt.IsEnabled = true;
            btnTK.IsEnabled = true;
            button1.IsEnabled = false;
            listAllbt.IsEnabled = true;
            button7.IsEnabled = false;
            button8.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;

            btnCPHDown.IsEnabled = true;
            btnOverDown.IsEnabled = true;
            btnTKDown.IsEnabled = true;
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            string SendSum = "";
            string Jstrs = "";
            int sum = 0;
            SendSum += "A500BB4144";
            Jstrs = "060102960064" + Convert.ToInt32(comboBox8.Text).ToString("X2") + "01";
            byte[] array = CR.GetByteArray(Jstrs);
            foreach (byte by in array)
            {
                sum += by;
            }
            //sum = sum - (sum /255) * 255;
            sum = sum % 256;

            short iNo = 0;
            short cmd = 0xA5;
            string strS = SendSum + sum.ToString("X2") + Jstrs + "FF";

            byte[] bVZSend = GetArray(strS);
            GCHandle hObject = GCHandle.Alloc(bVZSend, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();
            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject, bVZSend.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            SedBll sendbll;

            string[] strTmp = new string[2];
            if (Model.iCtrlShowPlate > 0)
            {
                strTmp[0] = "55";
            }
            else
            {
                strTmp[0] = "AA";
            }
            if (Model.iCtrlShowStayTime > 0)
            {
                strTmp[1] = "55";
            }
            else
            {
                strTmp[1] = "AA";
            }
            //                     if (Model.PubVal.Channels[y].iXieYi == 1 || Model.PubVal.Channels[y].iXieYi == 3)
            //                     {
            //                         strRetun = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.PubVal.Channels[y].iCtrlID), 0x3D, 0x71, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], Model.PubVal.Channels[y].iXieYi);
            //                         //Thread.Sleep(50);
            //                     }


            byte[] bVZSend = VzSendPort.LoadLsNoX2010znykt(0, 0x3D, 0x71, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], 0);
            GCHandle hObject = GCHandle.Alloc(bVZSend, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();

            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject, bVZSend.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend);
            }

            //             if (Model.PubVal.Channels[y].iOutCard == 1)
            //             {
            //                 short cmd0 = 0x3D;
            //                 short cmd1 = 0x71;
            //                 short Ilen = 5;
            //                 string SendData = strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1];
            //                 short JiHao = Convert.ToInt16(Model.PubVal.Channels[y].iCtrlID);
            //                 strRetun = axZNYKT_1.LoadLsNoX2010ZNYKT_(ref JiHao, ref cmd0, ref cmd1, Ilen, ref SendData);
            //             }
            Thread.Sleep(100);
            //                     if (strRetun == "2")
            //                     {
            //                         MessageBox.Show("与控制机通讯不通", Language.LanguageXML.GetName("CR/Prompt"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                         // this.Close();
            //                         return;
            //                     }
            //判断带小数点
            if (Model.iXsd == 1)
            {
                strTmp[0] = "AA";
            }
            else
            {
                if (Model.iChargeType == 3)
                {
                    if (Model.iXsdNum == 1)
                    {
                        strTmp[0] = "AA";
                    }
                    else
                    {
                        strTmp[0] = "BB";
                    }
                }
                else
                {
                    strTmp[0] = "55";
                }
            }

            byte[] bVZSend1 = VzSendPort.LoadLsNoX2010znykt(0, 0x3D, 0x6F, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], 0); ;
            GCHandle hObject1 = GCHandle.Alloc(bVZSend1, GCHandleType.Pinned);
            IntPtr pObject1 = hObject1.AddrOfPinnedObject();

            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject1, bVZSend1.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend1, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend1);
            }
            //                     if (Model.PubVal.Channels[y].iXieYi == 1 || Model.PubVal.Channels[y].iXieYi == 3)
            //                     {
            //                         strRetun = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.PubVal.Channels[y].iCtrlID), 0x3D, 0x6F, strTmp[0] + strTmp[0] + "00" + "00", Model.PubVal.Channels[y].iXieYi);
            //                     }
            //             if (Model.PubVal.Channels[y].iOutCard == 1)
            //             {
            //                 short cmd0 = 0x3D;
            //                 short cmd1 = 0x6F;
            //                 short Ilen = 5;
            //                 string SendData = strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1];
            //                 short JiHao = Convert.ToInt16(Model.PubVal.Channels[y].iCtrlID);
            //                 strRetun = axZNYKT_1.LoadLsNoX2010ZNYKT_(ref JiHao, ref cmd0, ref cmd1, Ilen, ref SendData);
            //             }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (rdbZNYKTY5.IsChecked ?? false)
            {
                WhiteList_Form listform = new WhiteList_Form();
                listform.Owner = this;
                listform.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                listform.Setm_hLPRClient(m_hLPRClient);
                listform.ShowDialog();
            }
            //else if (rdbZNYKTY13.IsChecked ?? false)
            //{
            //    frmWhiteList frmWL = new frmWhiteList();
            //    frmWL.hLogon = VideoInfo[0].m_hLogon;
            //    frmWL.ShowDialog();
            //}
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            // string str3 = Convert.ToInt32(comboBox9.Text).ToString("00");
            //string str4 = Convert.ToInt32(comboBox9.Text).ToString("00");

            int i3 = Convert.ToInt32(comboBox9.Text);//2016-06-13
            if (i3 == 1)
            {
                i3 = 2;
            }

            string str3 = (i3 / 2).ToString("00");
            string str4 = (i3 / 2).ToString("00");


            string JiHao = Convert.ToInt32(comboBox8.Text, 16).ToString("X2");
            string strS = "A5" + JiHao + JiHao + "3D3D0505A0" + str3 + str4 + "0000";
            byte[] bVZSend = GetArray(strS);
            GCHandle hObject = GCHandle.Alloc(bVZSend, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();
            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject, bVZSend.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string[] strTmp = new string[2];
            if (Model.iCtrlShowPlate > 0)
            {
                strTmp[0] = "55";
            }
            else
            {
                strTmp[0] = "AA";
            }
            if (Model.iCtrlShowStayTime > 0)
            {
                strTmp[1] = "55";
            }
            else
            {
                strTmp[1] = "AA";
            }
            //                     if (Model.PubVal.Channels[y].iXieYi == 1 || Model.PubVal.Channels[y].iXieYi == 3)
            //                     {
            //                         strRetun = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.PubVal.Channels[y].iCtrlID), 0x3D, 0x71, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], Model.PubVal.Channels[y].iXieYi);
            //                         //Thread.Sleep(50);
            //                     }


            byte[] bVZSend = VzSendPort.LoadLsNoX2010znykt(0, 0x3D, 0x71, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], 0); ;
            GCHandle hObject = GCHandle.Alloc(bVZSend, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();

            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject, bVZSend.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend);
            }

            //             if (Model.PubVal.Channels[y].iOutCard == 1)
            //             {
            //                 short cmd0 = 0x3D;
            //                 short cmd1 = 0x71;
            //                 short Ilen = 5;
            //                 string SendData = strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1];
            //                 short JiHao = Convert.ToInt16(Model.PubVal.Channels[y].iCtrlID);
            //                 strRetun = axZNYKT_1.LoadLsNoX2010ZNYKT_(ref JiHao, ref cmd0, ref cmd1, Ilen, ref SendData);
            //             }
            Thread.Sleep(100);
            //                     if (strRetun == "2")
            //                     {
            //                         MessageBox.Show("与控制机通讯不通", Language.LanguageXML.GetName("CR/Prompt"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                         // this.Close();
            //                         return;
            //                     }
            //判断带小数点
            if (Model.iXsd == 1)
            {
                strTmp[0] = "AA";
            }
            else
            {
                if (Model.iChargeType == 3)
                {
                    if (Model.iXsdNum == 1)
                    {
                        strTmp[0] = "AA";
                    }
                    else
                    {
                        strTmp[0] = "BB";
                    }
                }
                else
                {
                    strTmp[0] = "55";
                }
            }

            byte[] bVZSend1 = VzSendPort.LoadLsNoX2010znykt(0, 0x3D, 0x6F, strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1], 0); ;
            GCHandle hObject1 = GCHandle.Alloc(bVZSend1, GCHandleType.Pinned);
            IntPtr pObject1 = hObject1.AddrOfPinnedObject();

            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject1, bVZSend1.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend1, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend1);
            }
            //                     if (Model.PubVal.Channels[y].iXieYi == 1 || Model.PubVal.Channels[y].iXieYi == 3)
            //                     {
            //                         strRetun = sendbll.LoadLsNoX2010znykt(Convert.ToByte(Model.PubVal.Channels[y].iCtrlID), 0x3D, 0x6F, strTmp[0] + strTmp[0] + "00" + "00", Model.PubVal.Channels[y].iXieYi);
            //                     }
            //             if (Model.PubVal.Channels[y].iOutCard == 1)
            //             {
            //                 short cmd0 = 0x3D;
            //                 short cmd1 = 0x6F;
            //                 short Ilen = 5;
            //                 string SendData = strTmp[0] + strTmp[0] + strTmp[1] + strTmp[1];
            //                 short JiHao = Convert.ToInt16(Model.PubVal.Channels[y].iCtrlID);
            //                 strRetun = axZNYKT_1.LoadLsNoX2010ZNYKT_(ref JiHao, ref cmd0, ref cmd1, Ilen, ref SendData);
            //             }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            byte[] bVZSend = AllCommand.LoadTime(Convert.ToInt32(comboBox8.Text), DateTime.Now);
            GCHandle hObject = GCHandle.Alloc(bVZSend, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();

            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject, bVZSend.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend);
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            string CutedStr;
            Encoding myEncoding;

            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("加载出厂广告不能为空", "提示");
                return;
            }
            int Bytes_Count = 0;

            byte[] array2;
            array2 = System.Text.Encoding.Default.GetBytes(textBox1.Text.Trim());//把字符串转换为byte数组
            //array2 = System.Text.Encoding.Default.GetBytes("CAA3D3E0B3B5CEBB3A393938");//把字符串转换为byte数组
            // CR.CR.UpdateAppConfig("fbxx", textBox1.Text.Trim());
            Bytes_Count = array2.Length;
            byte[] array1 = new byte[Bytes_Count];
            if (Bytes_Count > 80)
            {
                myEncoding = Encoding.GetEncoding("GB2312");

                Bytes_Count = 80;
                Array.Copy(array2, 0, array1, 0, 80);
                CutedStr = myEncoding.GetString(array1);
                CutedStr = CutedStr.Substring(0, CutedStr.Length);
                textBox1.Text = CutedStr;
            }
            else
            {
                array1 = array2;
            }
            string str = string.Empty;
            if (array1 != null)
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    str += array1[i].ToString("X2");
                }
            }

            byte Count = Convert.ToByte(Convert.ToString(Bytes_Count, 16), 16);

            byte[] bVZSend = AllCommand.LoadLsNoX2010znyktXor(Convert.ToByte(Convert.ToInt32(comboBox8.Text)), 0x3D, 0x61, Count.ToString("X2") + str + CR.YHXY(str).ToString("X2"));

            GCHandle hObject = GCHandle.Alloc(bVZSend, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();
            // int ret = CR.VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject, bVZSend.Length);
            if (rdbZNYKTY5.IsChecked ?? false)
            {
                int ret = VzClientSDK.VzLPRClient_SerialSend(m_nSerialHandle, pObject, bVZSend.Length);
            }
            else if (rdbZNYKTY14.IsChecked ?? false)
            {
                int iRst = MyClass.Net_TransRS485Data(nCamId, Model.i485TT, bVZSend, Convert.ToByte(bVZSend.Length));
            }
            else if (rdbZNYKTY13.IsChecked ?? false)
            {
                YW7000NetClient.Sen485(VideoInfo[0].m_hLogon, bVZSend);
            }
        }

        /// <summary>
        ///字符串转换为数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public byte[] GetArray(string str)
        {
            string[] HexStr = Getstring(str);
            byte[] Hexbyte = new byte[HexStr.Length];
            for (int j = 0; j < HexStr.Length; j++)
            {
                Hexbyte[j] = Convert.ToByte(HexStr[j], 16);
            }

            return Hexbyte;
        }

        public string[] Getstring(string str)
        {
            List<String> al = new List<String>();
            for (int i = 0; i < str.Length / 2; i++)
            {
                string strJihao = str.Substring(i * 2, 2);
                al.Add(strJihao);
            }
            string s = string.Join(",", al.ToArray());
            string[] HexStr = s.Split(',');
            return HexStr;
        }

        private void rdbZNYKTY5_Checked(object sender, RoutedEventArgs e)
        {
            if (null == startsearchdev)
            {
                return;
            }

            startsearchdev.IsEnabled = true;
            stopsearchdev.IsEnabled = true;
            dev_treeview.IsEnabled = true;
            txtPort.Text = "80";
        }

        private void rdbZNYKTY14_Checked(object sender, RoutedEventArgs e)
        {
            if (null == startsearchdev)
            {
                return;
            }

            startsearchdev.IsEnabled = false;
            stopsearchdev.IsEnabled = false;
            dev_treeview.IsEnabled = false;
            txtPort.Text = "80";
        }

        private void rdbZNYKTY13_Checked(object sender, RoutedEventArgs e)
        {
            if (null == startsearchdev)
            {
                return;
            }

            startsearchdev.IsEnabled = false;
            stopsearchdev.IsEnabled = false;
            dev_treeview.IsEnabled = false;
            txtPort.Text = "5000";
        }
        #endregion
        #endregion


        #region 搜索设备

        readonly object objLock = new object();
        List<int> lstDevNo;
        ObservableCollection<DeviceInfo> lstDeviceSearched;
        Dictionary<string, DeviceInfo> dicMac_Device;

        private void InitSearchDevice()
        {
            lstDevNo = new List<int>();
            for (int i = 1; i < 128; i++)
            {
                lstDevNo.Add(i);
            }
            cbbDevNo.ItemsSource = lstDevNo;

            dicMac_Device = new Dictionary<string, DeviceInfo>();
            lstDeviceSearched = new ObservableCollection<DeviceInfo>();
            dgDevice.ItemsSource = lstDeviceSearched;
            dgDevice.LoadingRow += (s, e) =>
            {
                e.Row.Header = e.Row.GetIndex() + 1;
            };
        }

        private void btnSearchDevice_Click(object sender, RoutedEventArgs e)
        {
            btnSearchDevice.IsEnabled = false;

            Task.Factory.StartNew(delegate { DeviceCommander.SearchDevice(SolveDeviceData, SearchFinished); });
        }

        private void SolveDeviceData(byte[] devData)
        {
            string HexData;
            DeviceInfo devinfo;
            DeviceInfo oldDev;

            if (null == devData || devData.Length <= 35)
            {
                return;
            }
            //System.Diagnostics.Trace.WriteLine(devData.Length);

            HexData = "";
            for (int i = 0; i < devData.Length; i++)
            {
                HexData += devData[i].ToString("X2");
            }

            //HexData中的数据
            // 发送的命令0,8         MAC地址8,24        服务器端口32,4    客户端端口36,4   IP地址40,8  子网掩码48,8   网关56,8  机号64,2  类型66,2
            // AABBCC88       35FFD705344B363530331043       03EF             03ED          C0A8026F      FFFFFF00    C0A80201     0A       00       41434143414341434141AD

            devinfo = new DeviceInfo();
            devinfo.ClientPort = Convert.ToInt32(HexData.Substring(36, 4), 16);
            devinfo.ServerPort = Convert.ToInt32(HexData.Substring(32, 4), 16);
            devinfo.DeviceNo = Convert.ToInt32(HexData.Substring(64, 2), 16);
            devinfo.DeviceType = Convert.ToInt32(HexData.Substring(66, 2), 16);
            devinfo.Gateway = Hex2Ip(HexData.Substring(56, 8));
            devinfo.IPAddress = Hex2Ip(HexData.Substring(40, 8));
            devinfo.MAC = HexData.Substring(8, 24);
            devinfo.Subnetwork = Hex2Ip(HexData.Substring(48, 8));

            lock (objLock)
            {
                //if (lst.Exists(dev => dev.MAC == devinfo.MAC))
                if (dicMac_Device.ContainsKey(devinfo.MAC.ToUpper()))
                {
                    oldDev = dicMac_Device[devinfo.MAC.ToUpper()];
                    oldDev.ClientPort = devinfo.ClientPort;
                    oldDev.DeviceNo = devinfo.DeviceNo;
                    oldDev.DeviceType = devinfo.DeviceType;
                    oldDev.Gateway = devinfo.Gateway;
                    oldDev.IPAddress = devinfo.IPAddress;
                    oldDev.MAC = devinfo.MAC;
                    oldDev.ServerPort = devinfo.ServerPort;
                    oldDev.Subnetwork = devinfo.Subnetwork;
                }
                else
                {
                    ShowDevice(devinfo);
                }
            }
        }

        private void ShowDevice(DeviceInfo devinfo)
        {
            dgDevice.Dispatcher.Invoke(delegate
            {
                dicMac_Device.Add(devinfo.MAC.ToUpper(), devinfo);
                lstDeviceSearched.Add(devinfo);
                System.Diagnostics.Trace.WriteLine(string.Format("New device found -> IP:{0},MAC:{1}", devinfo.IPAddress, devinfo.MAC));
            });
        }

        private void SearchFinished()
        {
            dgDevice.Dispatcher.Invoke(delegate
            {
                btnSearchDevice.IsEnabled = true;
                dgDevice.Items.Refresh();
            });
        }

        private string Hex2Ip(string HexIp)
        {
            string IP = "";

            IP += Convert.ToInt32(HexIp.Substring(0, 2), 16).ToString() + ".";
            IP += Convert.ToInt32(HexIp.Substring(2, 2), 16).ToString() + ".";
            IP += Convert.ToInt32(HexIp.Substring(4, 2), 16).ToString() + ".";
            IP += Convert.ToInt32(HexIp.Substring(6, 2), 16).ToString();

            return IP;
        }

        private string Ip2Hex(string Ip)
        {
            string Hex;
            string[] tmp;

            Hex = "";
            tmp = Ip.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Hex += int.Parse(tmp[0]).ToString("X2");
            Hex += int.Parse(tmp[1]).ToString("X2");
            Hex += int.Parse(tmp[2]).ToString("X2");
            Hex += int.Parse(tmp[3]).ToString("X2");

            return Hex;
        }

        private byte[] Hex2ByteArray(string Hex)
        {
            byte[] data;

            if (Hex.Length % 2 != 0)
            {
                Hex = "0" + Hex;
            }

            data = new byte[Hex.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(Hex.Substring(i * 2, 2), 16);
            }

            return data;
        }

        /// <summary>
        /// btye数组转换为字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string ByteToHexString(byte[] bytes)
        {
            string str = string.Empty;
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    str += bytes[i].ToString("X2");
                }
            }
            return str;
        }

        private byte GetXorChecksum(byte[] data)
        {
            byte checksum = 0;

            checksum = (byte)(data[0] ^ data[1]);
            for (int i = 2; i < data.Length; i++)
            {
                checksum ^= data[i];
            }

            return checksum;
        }

        private void btnChangeDevInfo_Click(object sender, RoutedEventArgs e)
        {
            byte[] buffer;
            byte[] cmdData;
            string result;
            string cmdContent;
            DeviceInfo dev;

            dev = dgDevice.SelectedItem as DeviceInfo;
            if (null == dev)
            {
                return;
            }

            cmdContent = dev.MAC + dev.ServerPort.ToString("X4") + dev.ClientPort.ToString("X4");
            cmdContent += Ip2Hex(txtIPSearch.Text) + Ip2Hex(txtSubnet.Text) + Ip2Hex(txtGateway.Text);
            cmdContent += ((int)cbbDevNo.SelectedValue).ToString("X2") + dev.DeviceType.ToString("X2") + "41434143414341434141";
            cmdContent += GetXorChecksum(Hex2ByteArray(cmdContent)).ToString("X2");

            buffer = Hex2ByteArray(cmdContent);
            cmdData = new byte[4 + buffer.Length];
            cmdData[0] = 0xAA;
            cmdData[1] = 0xBB;
            cmdData[2] = 0xCC;
            cmdData[3] = 0x99;
            Array.Copy(buffer, 0, cmdData, 4, buffer.Length);

            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 1005), out buffer);
            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 1005), out buffer);
            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 1005), out buffer);
            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 1005), out buffer);

            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 9801), out buffer);
            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 9801), out buffer);
            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 9801), out buffer);
            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Broadcast, 9801), out buffer);

            System.Threading.Thread.Sleep(5000);    //等待机器启用新的IP

            //检查是否更改成功
            cmdData = new byte[9];
            cmdData[0] = 0xA5;  //协议，暂时测试
            cmdData[1] = byte.Parse((cbbDevNo.SelectedValue ?? "1").ToString());
            cmdData[2] = cmdData[1];
            cmdData[3] = 0x34;
            cmdData[4] = 0x34;
            cmdData[5] = 80;
            cmdData[6] = 80;
            cmdData[7] = 1;
            cmdData[8] = 1;

            UdpCommander.SendDataTo(cmdData, new IPEndPoint(IPAddress.Parse(txtIPSearch.Text), dev.ClientPort), out buffer, new IPEndPoint(IPAddress.Any, 0));
            result = ByteToHexString(buffer);

            if (null != result && result.EndsWith("3030"))
            {
                dev.DeviceNo = (int)cbbDevNo.SelectedValue;
                dev.Gateway = txtGateway.Text;
                dev.IPAddress = txtIPSearch.Text;
                dev.Subnetwork = txtSubnet.Text;

                dgDevice.ItemsSource = null;
                dgDevice.ItemsSource = lstDeviceSearched;

                MessageBox.Show("修改成功");
            }
            else
            {
                MessageBox.Show("IP修改失败！");
            }
        }

        private void dgDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceInfo dev;

            try
            {
                dev = dgDevice.SelectedItem as DeviceInfo;
                if (null != dev)
                {
                    cbbDevNo.SelectedValue = dev.DeviceNo;
                    txtGateway.Text = dev.Gateway;
                    txtIPSearch.Text = dev.IPAddress;
                    txtSubnet.Text = dev.Subnetwork;
                }
            }
            catch (Exception)
            {
            }
        }

        public class DeviceInfo : INotifyPropertyChanged
        {
            public int DeviceNo { get; set; }
            public int DeviceType { get; set; }
            public string IPAddress { get; set; }
            public string Subnetwork { get; set; }
            public string Gateway { get; set; }
            public int ServerPort { get; set; }
            public int ClientPort { get; set; }
            public string MAC { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        #endregion


        #region 控制机设置

        #region 加载/初始化

        private void InitDeviceSetting()
        {
            InitialTimePage();
            InitialPublishMsgPage();
            InitialChargeStand();
            InitScreenSetPage();
            //InitWisdomPage();
            InitAdLoadPage();

            dgChannel.AutoGenerateColumns = false;
            dgChannel.ItemsSource = cds;
            dgChannel.PreviewMouseLeftButtonUp += DataGrid_PreviewMouseLeftButtonUp;

            tabWisdom.Visibility = System.Windows.Visibility.Hidden;
            tcControlSet.SelectionChanged += tcControlSet_SelectionChanged;
        }

        void tcControlSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem ti;

            ti = tcControlSet.SelectedItem as TabItem;
            if (null == ti || null == ti.Header)
            {
                return;
            }

            switch (ti.Header.ToString().ToLower())
            {
                case "收费标准设置":
                    if (dgChannel.ItemsSource == cds)
                    {
                        ObservableCollection<CheDaoSet> lstTmp = new ObservableCollection<CheDaoSet>();
                        var lstOut = from c in cds where c.InOut == 1 select c;
                        foreach (var chedao in lstOut)
                        {
                            lstTmp.Add(chedao);
                        }
                        dgChannel.ItemsSource = lstTmp;
                    }
                    break;
                default:
                    dgChannel.ItemsSource = cds;
                    break;
            }
        }
        #endregion

        #region 时间管理
        System.Windows.Threading.DispatcherTimer tmrShowTime;

        void InitialTimePage()
        {
            dpDate.SelectedDate = DateTime.Now;
            lblTime.Content = DateTime.Now.ToString("HH:mm:ss");

            tmrShowTime = new System.Windows.Threading.DispatcherTimer();
            tmrShowTime.Interval = new TimeSpan(0, 0, 1);
            //tmrShowTime.IsEnabled = true;
            tmrShowTime.Tick += tmrShowTime_Tick;

            dgChannel.LoadingRow += (s, ev) =>
            {
                ev.Row.Header = ev.Row.GetIndex() + 1;
            };
        }

        void tmrShowTime_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        void ShowWeekDay()
        {
            if (null == dpDate.SelectedDate)
            {
                lblWeekDay.Content = "";
                return;
            }

            switch (dpDate.SelectedDate.Value.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    lblWeekDay.Content = "星期五";
                    break;
                case DayOfWeek.Monday:
                    lblWeekDay.Content = "星期一";
                    break;
                case DayOfWeek.Saturday:
                    lblWeekDay.Content = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    lblWeekDay.Content = "星期日";
                    break;
                case DayOfWeek.Thursday:
                    lblWeekDay.Content = "星期四";
                    break;
                case DayOfWeek.Tuesday:
                    lblWeekDay.Content = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    lblWeekDay.Content = "星期三";
                    break;
            }
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowWeekDay();
        }

        List<object> lstDeviceTime;

        private void btnReadTime_Click(object sender, RoutedEventArgs e)
        {
            CheDaoSet channel;
            DeviceCommander cmder;
            DateTime? time;

            lstDeviceTime = new List<object>();

            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        lstDeviceTime.Add(new { InOutName = channel.InOutName, Msg = "无法转换为车道数据" });
                        continue;
                    }

                    try
                    {
                        cmder = DeviceCommander.GetCommander(channel);
                        time = cmder.GetTime();

                        lstDeviceTime.Add(new { InOutName = channel.InOutName, Time = time, Msg = "OK" });
                    }
                    catch (Exception ex)
                    {
                        lstDeviceTime.Add(new { InOutName = channel.InOutName, Msg = ex.Message });
                    }
                }
            }

            dgDeviceTime.AutoGenerateColumns = false;
            dgDeviceTime.ItemsSource = lstDeviceTime;

            if (lstDeviceTime.Count <= 0)
            {
                MessageBox.Show("请选择车道");
            }
        }

        private void btnLoadTime_Click(object sender, RoutedEventArgs e)
        {
            CheDaoSet channel;
            DeviceCommander cmder;
            DateTime? time;

            lstDeviceTime = new List<object>();

            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        lstDeviceTime.Add(new { InOutName = channel.InOutName, Msg = "无法转换为车道数据" });
                        continue;
                    }

                    try
                    {
                        time = null == dpDate.SelectedDate ? DateTime.Now : DateTime.Parse(dpDate.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                        cmder = DeviceCommander.GetCommander(channel);
                        cmder.SetTime(time);

                        lstDeviceTime.Add(new { InOutName = channel.InOutName, Time = time, Msg = "OK" });
                    }
                    catch (Exception ex)
                    {
                        lstDeviceTime.Add(new { InOutName = channel.InOutName, Msg = ex.Message });
                    }
                }
            }

            dgDeviceTime.AutoGenerateColumns = false;
            dgDeviceTime.ItemsSource = lstDeviceTime;

            if (lstDeviceTime.Count <= 0)
            {
                MessageBox.Show("请选择车道");
            }
        }
        #endregion

        #region 显示屏消息及音量
        void InitialPublishMsgPage()
        {
            dpValidEnd.SelectedDate = DateTime.Now;
            dpValidStart.SelectedDate = DateTime.Now;
        }

        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            byte[] buffer;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            buffer = Encoding.Default.GetBytes(txtPublishMsg.Text.Trim());
            if (null == buffer || buffer.Length <= 0)
            {
                txtPublishMsg.Focus();
                MessageBox.Show("请输入需要发布的信息");
                return;
            }

            if (null == dpValidStart.SelectedDate)
            {
                dpValidStart.Focus();
                MessageBox.Show("选择开始日期");
                return;
            }
            if (null == dpValidEnd.SelectedDate)
            {
                dpValidEnd.Focus();
                MessageBox.Show("选择结束日期");
                return;
            }
            if (buffer.Length > 80)
            {
                txtPublishMsg.Text = Encoding.Default.GetString(buffer, 0, 80);
            }

            int ret = -1;
            lstChannel = new List<CheDaoSet>();
            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        continue;
                    }
                    lstChannel.Add(channel);

                    try
                    {
                        cmder = DeviceCommander.GetCommander(channel);
                        ret = cmder.SetPublishMessage(txtPublishMsg.Text, dpValidStart.SelectedDate.Value, dpValidEnd.SelectedDate.Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("{0}({1}) 发布消息失败: {2}", channel.InOutName, channel.CtrlNumber, ex.Message));
                    }
                }
            }

            if (lstChannel.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }

            CR.UpdateAppConfig("fbxx", txtPublishMsg.Text.Trim());
            if (ret > 0)
            {
                gsd.AddLog(this.Title, "加载显示屏信息：" + txtPublishMsg.Text);
                MessageBox.Show("加载成功", "提示");
            }
        }

        private void btnSetVolume_Click(object sender, RoutedEventArgs e)
        {
            int result;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        continue;
                    }
                    lstChannel.Add(channel);

                    try
                    {
                        cmder = DeviceCommander.GetCommander(channel);
                        result = cmder.SetVolume((int)numHourStart.Value, (int)numHourEnd.Value, (int)numVol.Value, (int)numVolOth.Value);
                        if (result > 0)
                        {
                            cmder.PlayCombinationVoice("74"); //播放 "谢谢" 以便测验音量大小
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("{0}({1}) 设置音量失败: {2}", channel.InOutName, channel.CtrlNumber, ex.Message));
                    }
                }
            }

            if (lstChannel.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }
        }
        #endregion

        private void btnClearData_Click(object sender, RoutedEventArgs e)
        {
            int result;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        continue;
                    }
                    lstChannel.Add(channel);

                    try
                    {
                        cmder = DeviceCommander.GetCommander(channel);
                        result = cmder.ClearData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("{0}({1}) 清除数据失败: {2}", channel.InOutName, channel.CtrlNumber, ex.Message));
                    }
                }
            }

            if (lstChannel.Count <= 0)
            {
                MessageBox.Show("请选择车道");
            }
        }

        #region 收费标准
        bool IsReading = false;
        List<CardTypeDef> lstCardType;
        List<ChargeRules> lstRule;
        CardTypeChargeRules ChargeRule;

        private void InitialChargeStand()
        {
            ParkingInterface.Request req;
            List<ParkingModel.QueryConditionGroup> lstGroup;

            dgCharge.IsReadOnly = false;
            dgCharge.CanUserAddRows = false;
            dgCharge.CanUserDeleteRows = false;
            dgCharge.AutoGenerateColumns = false;
            dgCharge.VerticalGridLinesBrush = new SolidColorBrush(Color.FromArgb(255, 244, 244, 244));
            dgCharge.HorizontalGridLinesBrush = new SolidColorBrush(Color.FromArgb(255, 244, 244, 244));

            lstGroup = new List<QueryConditionGroup>();
            lstGroup.Add(new QueryConditionGroup());
            lstGroup[0].Add("Identifying", "like", "Tmp%", "or");
            lstGroup[0].Add("Identifying", "like", "Str%", "or");

            req = new ParkingInterface.Request();
            lstCardType = req.GetData<List<CardTypeDef>>("GetCardTypeDef", null, lstGroup);

            cbbCardType.ItemsSource = lstCardType;
            cbbCardType.DisplayMemberPath = "CardType";
            cbbCardType.SelectedValuePath = "Identifying";

            if (null != lstCardType && lstCardType.Count > 0)
            {
                cbbCardType.SelectedItem = lstCardType[0];
            }
        }

        private void cbbCardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardTypeDef cardType;
            ParkingInterface.Request req;
            List<ParkingModel.QueryConditionGroup> lstGroup;

            cardType = cbbCardType.SelectedItem as CardTypeDef;
            if (null == cardType)
            {
                return;
            }

            lstGroup = new List<QueryConditionGroup>();
            lstGroup.Add(new QueryConditionGroup());
            lstGroup[0].Add("CardType", "=", cardType.Identifying, "and");
            lstGroup[0].Add("ParkID", "=", ParkingModel.Model.iParkingNo, "and");

            req = new ParkingInterface.Request();
            lstRule = req.GetData<List<ChargeRules>>("GetChargeRules", null, lstGroup);

            ChargeRule = new CardTypeChargeRules(lstRule);
            if (null == lstRule || lstRule.Count <= 0)
            {
                ChargeRule.HoursRule.Hour1Money = 1;
                ChargeRule.HoursRule.Hour2Money = 2;
                ChargeRule.HoursRule.Hour3Money = 3;
                ChargeRule.HoursRule.Hour4Money = 4;
                ChargeRule.HoursRule.Hour5Money = 5;
                ChargeRule.HoursRule.Hour6Money = 6;
                ChargeRule.HoursRule.Hour7Money = 7;
                ChargeRule.HoursRule.Hour8Money = 8;
                ChargeRule.HoursRule.Hour9Money = 9;
                ChargeRule.HoursRule.Hour10Money = 10;
                ChargeRule.HoursRule.Hour11Money = 11;
                ChargeRule.HoursRule.Hour12Money = 12;
                ChargeRule.HoursRule.Hour13Money = 13;
                ChargeRule.HoursRule.Hour14Money = 14;
                ChargeRule.HoursRule.Hour15Money = 15;
                ChargeRule.HoursRule.Hour16Money = 16;
                ChargeRule.HoursRule.Hour17Money = 17;
                ChargeRule.HoursRule.Hour18Money = 18;
                ChargeRule.HoursRule.Hour19Money = 19;
                ChargeRule.HoursRule.Hour20Money = 20;
                ChargeRule.HoursRule.Hour21Money = 21;
                ChargeRule.HoursRule.Hour22Money = 22;
                ChargeRule.HoursRule.Hour23Money = 23;
                ChargeRule.HoursRule.Hour24Money = 24;
                MessageBox.Show(string.Format("{0} 没有加载过收费规则", cardType.CardType));
            }

            LoadChargeRule();
        }

        private void LoadChargeRule()
        {
            if (null == ChargeRule)
            {
                ChargeRule = new CardTypeChargeRules(null);
            }

            numFreeMinute.Value = ChargeRule.FreeMinutes;
            numTopSF.Value = ChargeRule.TopSF;
            ckbFreeMinuteNoCharge.IsChecked = ChargeRule.FreeMinutesNoCharge;

            switch (ChargeRule.ChargeMode)
            {
                case 1:
                    rdbDayNight.IsChecked = true;
                    numOverNightCharge.Value = 0;
                    numOverNightCharge.IsEnabled = false;
                    LoadDayNightCharge();
                    break;
                case 2:
                    rdbTimeUnit.IsChecked = true;
                    numOverNightCharge.Value = ChargeRule.TimeUnitRule.OverNightCharge;
                    numOverNightCharge.IsEnabled = true;
                    LoadTimeUnitCharge();
                    break;
                case 3:
                    rdbByCount.IsChecked = true;
                    numOverNightCharge.Value = 0;
                    numOverNightCharge.IsEnabled = false;
                    LoadByCountCharge();
                    break;
                default:
                    rdbHour.IsChecked = true;
                    numOverNightCharge.Value = ChargeRule.HoursRule.OverNightCharge;
                    numOverNightCharge.IsEnabled = true;
                    LoadHourCharge();
                    break;
            }

            //dgCharge.fit
        }

        private void LoadHourCharge()
        {
            List<ChargeRules> lst;
            DataGridTextColumn txtCol;
            System.Data.DataTable dt;

            dgCharge.ItemsSource = null;
            dgCharge.Columns.Clear();

            txtCol = new DataGridTextColumn();
            txtCol.Width = 150;
            txtCol.IsReadOnly = true;
            txtCol.Header = "小时";
            txtCol.Binding = new Binding("Hours");
            dgCharge.Columns.Add(txtCol);
            txtCol = new DataGridTextColumn();
            txtCol.Width = 150;
            txtCol.IsReadOnly = false;
            txtCol.Header = "收费";
            txtCol.Binding = new Binding("JE");
            dgCharge.Columns.Add(txtCol);

            dt = new System.Data.DataTable();
            dt.Columns.Add("Hours", typeof(int));
            dt.Columns.Add("JE", typeof(decimal));

            if (null == ChargeRule)
            {
                ChargeRule = new CardTypeChargeRules(null);
            }

            ChargeRule.ChangeChargeMode(0);
            lst = ChargeRule.ToRuleList();
            var rows = from r in lst where r.Hours <= 24 select r;
            foreach (var r in rows)
            {
                dt.Rows.Add(new object[] { r.Hours, r.JE ?? 0 });
            }

            dgCharge.ItemsSource = dt.DefaultView;
        }

        private void LoadDayNightCharge()
        {
            DataGridTextColumn txtCol;
            System.Data.DataTable dt;

            dgCharge.ItemsSource = null;
            dgCharge.Columns.Clear();

            txtCol = new DataGridTextColumn();
            //txtCol.Width = 150;
            txtCol.IsReadOnly = true;
            txtCol.Header = "说明";
            txtCol.Binding = new Binding("Desc");
            dgCharge.Columns.Add(txtCol);
            txtCol = new DataGridTextColumn();
            //txtCol.Width = 80;
            txtCol.IsReadOnly = false;
            txtCol.Header = "白天时段";
            txtCol.Binding = new Binding("Day");
            dgCharge.Columns.Add(txtCol);
            txtCol = new DataGridTextColumn();
            //txtCol.Width = 80;
            txtCol.IsReadOnly = false;
            txtCol.Header = "夜间时段";
            txtCol.Binding = new Binding("Night");
            dgCharge.Columns.Add(txtCol);

            dt = new System.Data.DataTable();
            dt.Columns.Add("Desc", typeof(string));
            dt.Columns.Add("Day", typeof(decimal));
            dt.Columns.Add("Night", typeof(decimal));

            if (null == ChargeRule)
            {
                ChargeRule = new CardTypeChargeRules(null);
            }

            ChargeRule.ChangeChargeMode(1);

            dt.Rows.Add(new object[] { "开始小时(时)", ChargeRule.DayNightRule.DayBeginHour, ChargeRule.DayNightRule.NightBeginHour });
            dt.Rows.Add(new object[] { "开始分钟(分)", ChargeRule.DayNightRule.DayBeginMinute, ChargeRule.DayNightRule.NightBeginMinute });
            dt.Rows.Add(new object[] { "计时单位(小时)", ChargeRule.DayNightRule.DayHourUnit, ChargeRule.DayNightRule.NightHourUnit });
            dt.Rows.Add(new object[] { "计时单位(分钟)", ChargeRule.DayNightRule.DayMinuteUnit, ChargeRule.DayNightRule.NightMinuteUnit });
            dt.Rows.Add(new object[] { "每计时单位收费额", ChargeRule.DayNightRule.DayUnitMoney, ChargeRule.DayNightRule.NightUnitMoney });
            dt.Rows.Add(new object[] { "最高收费额", ChargeRule.DayNightRule.DayMaxMoney, ChargeRule.DayNightRule.NightMaxMoney });
            //dt.Rows.Add(new object[] { "最低收费额", 0, 0 });
            dt.Rows.Add(new object[] { "首计时单位收费额", ChargeRule.DayNightRule.DayFirstUnitMoney, ChargeRule.DayNightRule.NightFirstUnitMoney });
            dt.Rows.Add(new object[] { "首计时单位(小时)", ChargeRule.DayNightRule.DayFirstUnitHour, ChargeRule.DayNightRule.NightFirstUnitHour });
            dt.Rows.Add(new object[] { "首计时单位(分钟)", ChargeRule.DayNightRule.DayFirstUnitMinute, ChargeRule.DayNightRule.NightFirstUnitMinute });
            dt.Rows.Add(new object[] { "跨时段时分两段时间计费", ChargeRule.DayNightRule.DayCutAtTheNextStartPoint, ChargeRule.DayNightRule.NightCutAtTheNextStartPoint });

            dgCharge.ItemsSource = dt.DefaultView;
        }

        private void LoadTimeUnitCharge()
        {
            DataGridTextColumn txtCol;
            System.Data.DataTable dt;

            dgCharge.ItemsSource = null;
            dgCharge.Columns.Clear();

            txtCol = new DataGridTextColumn();
            //txtCol.Width = 150;
            txtCol.IsReadOnly = true;
            txtCol.Header = "说明";
            txtCol.Binding = new Binding("Desc");
            dgCharge.Columns.Add(txtCol);
            txtCol = new DataGridTextColumn();
            txtCol.Width = 80;
            txtCol.IsReadOnly = false;
            txtCol.Header = "值";
            txtCol.Binding = new Binding("Value");
            dgCharge.Columns.Add(txtCol);

            dt = new System.Data.DataTable();
            dt.Columns.Add("Desc", typeof(string));
            dt.Columns.Add("Value", typeof(decimal));

            if (null == ChargeRule)
            {
                ChargeRule = new CardTypeChargeRules(null);
            }

            ChargeRule.ChangeChargeMode(2);

            dt.Rows.Add(new object[] { "计时单位(分钟)", ChargeRule.TimeUnitRule.TimeUnit });
            dt.Rows.Add(new object[] { "每计时单位收费额(元)", ChargeRule.TimeUnitRule.UnitMoney });
            dt.Rows.Add(new object[] { "首计时单位(分钟)", ChargeRule.TimeUnitRule.FirstUnit });
            dt.Rows.Add(new object[] { "首计时单位收费额(元)", ChargeRule.TimeUnitRule.FirstUnitMoney });

            dgCharge.ItemsSource = dt.DefaultView;
        }

        private void LoadByCountCharge()
        {
            dgCharge.ItemsSource = null;
            dgCharge.Columns.Clear();
        }

        private void rdbHour_Checked(object sender, RoutedEventArgs e)
        {
            if (IsReading)
            {
                return;
            }

            ChargeRule = ChargeRule ?? new CardTypeChargeRules(null);
            ChargeRule.ChargeMode = 0;
            LoadChargeRule();
        }

        private void rdbDayNight_Checked(object sender, RoutedEventArgs e)
        {
            if (IsReading)
            {
                return;
            }

            ChargeRule = ChargeRule ?? new CardTypeChargeRules(null);
            ChargeRule.ChargeMode = 1;
            LoadChargeRule();
        }

        private void rdbTimeUnit_Checked(object sender, RoutedEventArgs e)
        {
            if (IsReading)
            {
                return;
            }

            ChargeRule = ChargeRule ?? new CardTypeChargeRules(null);
            ChargeRule.ChargeMode = 2;
            LoadChargeRule();
        }

        private void rdbByCount_Checked(object sender, RoutedEventArgs e)
        {
            if (IsReading)
            {
                return;
            }

            ChargeRule = ChargeRule ?? new CardTypeChargeRules(null);
            ChargeRule.ChargeMode = 3;
            LoadChargeRule();
        }

        private void btnReadSF_Click(object sender, RoutedEventArgs e)
        {
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        continue;
                    }
                    lstChannel.Add(channel);
                }
            }

            if (lstChannel.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }
            if (lstChannel.Count > 1)
            {
                MessageBox.Show("一次只能读取一个车道，请重新选择车道");
                return;
            }


            IsReading = true;
            try
            {
                cmder = DeviceCommander.GetCommander(lstChannel[0]);
                ChargeRule = cmder.GetChargeRule((cbbCardType.SelectedValue ?? "").ToString());

                LoadChargeRule();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取收费标准失败: " + ex.Message);
            }
            IsReading = false;
        }

        private void btnSetSF_Click(object sender, RoutedEventArgs e)
        {
            long count = 0;
            Request req;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<ChargeRules> lstRule;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        continue;
                    }
                    lstChannel.Add(channel);
                }
            }

            if (lstChannel.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }
            if (lstChannel.Count > 1)
            {
                MessageBox.Show("一次只能设置一个车道，请重新选择车道");
                return;
            }

            try
            {
                BindSFStand(ChargeRule);
                lstRule = ChargeRule.ToRuleList();

                if (null == lstRule)
                {
                    return;
                }

                req = new Request();
                count = req.AddList<ChargeRules>("AddChargeRulesList", lstRule);

                cmder = DeviceCommander.GetCommander(lstChannel[0]);
                count = cmder.SetChargeRule(ChargeRule);

                MessageBox.Show("设置收费标准成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置收费标准失败: " + ex.Message);
            }
        }

        private void BindSFStand(CardTypeChargeRules rule)
        {
            CardTypeDef cardType;

            cardType = cbbCardType.SelectedItem as CardTypeDef;
            if (null == cardType)
            {
                return;
            }

            rule.ParkID = ParkingModel.Model.iParkingNo;
            rule.CardType = cardType.Identifying;
            rule.FreeMinutes = (int)numFreeMinute.Value;
            rule.FreeMinutesNoCharge = ckbFreeMinuteNoCharge.IsChecked ?? false;
            rule.TopSF = (int)numTopSF.Value;

            if (rdbByCount.IsChecked ?? false)
            {
                BindByCountCharge(rule);
            }
            else if (rdbDayNight.IsChecked ?? false)
            {
                BindDayNightCharge(rule);
            }
            else if (rdbHour.IsChecked ?? false)
            {
                BindHourCharge(rule);
            }
            else if (rdbTimeUnit.IsChecked ?? false)
            {
                BindTimeUnitCharge(rule);
            }
            else
            {

            }
        }

        private void BindHourCharge(CardTypeChargeRules rule)
        {
            int Hour;
            decimal JE;
            System.Data.DataView dv;
            System.Data.DataTable dt;

            rule.ChargeMode = 0;
            rule.HoursRule.OverNightCharge = numOverNightCharge.Value;
            dv = dgCharge.ItemsSource as System.Data.DataView;
            if (null == dv || null == dv.Table || dv.Table.Rows.Count <= 0)
            {
                return;
            }
            dt = dv.Table;

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                if (!int.TryParse(dr["Hours"].ToString(), out Hour))
                {
                    continue;
                }
                decimal.TryParse(dr["JE"].ToString(), out JE);

                switch (Hour)
                {
                    case 1:
                        rule.HoursRule.Hour1Money = JE;
                        break;
                    case 2:
                        rule.HoursRule.Hour2Money = JE;
                        break;
                    case 3:
                        rule.HoursRule.Hour3Money = JE;
                        break;
                    case 4:
                        rule.HoursRule.Hour4Money = JE;
                        break;
                    case 5:
                        rule.HoursRule.Hour5Money = JE;
                        break;
                    case 6:
                        rule.HoursRule.Hour6Money = JE;
                        break;
                    case 7:
                        rule.HoursRule.Hour7Money = JE;
                        break;
                    case 8:
                        rule.HoursRule.Hour8Money = JE;
                        break;
                    case 9:
                        rule.HoursRule.Hour9Money = JE;
                        break;
                    case 10:
                        rule.HoursRule.Hour10Money = JE;
                        break;
                    case 11:
                        rule.HoursRule.Hour11Money = JE;
                        break;
                    case 12:
                        rule.HoursRule.Hour12Money = JE;
                        break;
                    case 13:
                        rule.HoursRule.Hour13Money = JE;
                        break;
                    case 14:
                        rule.HoursRule.Hour14Money = JE;
                        break;
                    case 15:
                        rule.HoursRule.Hour15Money = JE;
                        break;
                    case 16:
                        rule.HoursRule.Hour16Money = JE;
                        break;
                    case 17:
                        rule.HoursRule.Hour17Money = JE;
                        break;
                    case 18:
                        rule.HoursRule.Hour18Money = JE;
                        break;
                    case 19:
                        rule.HoursRule.Hour19Money = JE;
                        break;
                    case 20:
                        rule.HoursRule.Hour20Money = JE;
                        break;
                    case 21:
                        rule.HoursRule.Hour21Money = JE;
                        break;
                    case 22:
                        rule.HoursRule.Hour22Money = JE;
                        break;
                    case 23:
                        rule.HoursRule.Hour23Money = JE;
                        break;
                    case 24:
                        rule.HoursRule.Hour24Money = JE;
                        break;
                    case 80:
                        rule.HoursRule.OverNightCharge = JE;
                        break;
                }
            }
        }

        private void BindDayNightCharge(CardTypeChargeRules rule)
        {
            decimal value;
            System.Data.DataView dv;
            System.Data.DataTable dt;

            rule.ChargeMode = 1;
            dv = dgCharge.ItemsSource as System.Data.DataView;
            if (null == dv || null == dv.Table || dv.Table.Rows.Count <= 0)
            {
                return;
            }
            dt = dv.Table;

            decimal.TryParse(dt.Rows[0][1].ToString(), out value);
            rule.DayNightRule.DayBeginHour = value;
            decimal.TryParse(dt.Rows[0][2].ToString(), out value);
            rule.DayNightRule.NightBeginHour = value;

            decimal.TryParse(dt.Rows[1][1].ToString(), out value);
            rule.DayNightRule.DayBeginMinute = value;
            decimal.TryParse(dt.Rows[1][2].ToString(), out value);
            rule.DayNightRule.NightBeginMinute = value;

            decimal.TryParse(dt.Rows[2][1].ToString(), out value);
            rule.DayNightRule.DayHourUnit = value;
            decimal.TryParse(dt.Rows[2][2].ToString(), out value);
            rule.DayNightRule.NightHourUnit = value;

            decimal.TryParse(dt.Rows[3][1].ToString(), out value);
            rule.DayNightRule.DayMinuteUnit = value;
            decimal.TryParse(dt.Rows[3][2].ToString(), out value);
            rule.DayNightRule.NightMinuteUnit = value;

            decimal.TryParse(dt.Rows[4][1].ToString(), out value);
            rule.DayNightRule.DayUnitMoney = value;
            decimal.TryParse(dt.Rows[4][2].ToString(), out value);
            rule.DayNightRule.NightUnitMoney = value;

            decimal.TryParse(dt.Rows[5][1].ToString(), out value);
            rule.DayNightRule.DayMaxMoney = value;
            decimal.TryParse(dt.Rows[5][2].ToString(), out value);
            rule.DayNightRule.NightMaxMoney = value;

            decimal.TryParse(dt.Rows[6][1].ToString(), out value);
            rule.DayNightRule.DayFirstUnitMoney = value;
            decimal.TryParse(dt.Rows[6][2].ToString(), out value);
            rule.DayNightRule.NightFirstUnitMoney = value;

            decimal.TryParse(dt.Rows[7][1].ToString(), out value);
            rule.DayNightRule.DayFirstUnitHour = value;
            decimal.TryParse(dt.Rows[7][2].ToString(), out value);
            rule.DayNightRule.NightFirstUnitHour = value;

            decimal.TryParse(dt.Rows[8][1].ToString(), out value);
            rule.DayNightRule.DayFirstUnitMinute = value;
            decimal.TryParse(dt.Rows[8][2].ToString(), out value);
            rule.DayNightRule.NightFirstUnitMinute = value;

            decimal.TryParse(dt.Rows[9][1].ToString(), out value);
            rule.DayNightRule.DayCutAtTheNextStartPoint = value > 0;
            decimal.TryParse(dt.Rows[9][2].ToString(), out value);
            rule.DayNightRule.NightCutAtTheNextStartPoint = value > 0;
        }

        private void BindTimeUnitCharge(CardTypeChargeRules rule)
        {
            decimal value;
            System.Data.DataView dv;
            System.Data.DataTable dt;

            rule.ChargeMode = 2;
            rule.TimeUnitRule.OverNightCharge = numOverNightCharge.Value;
            dv = dgCharge.ItemsSource as System.Data.DataView;
            if (null == dv || null == dv.Table || dv.Table.Rows.Count <= 0)
            {
                return;
            }
            dt = dv.Table;

            decimal.TryParse(dt.Rows[0][1].ToString(), out value);
            rule.TimeUnitRule.TimeUnit = value;
            decimal.TryParse(dt.Rows[1][1].ToString(), out value);
            rule.TimeUnitRule.UnitMoney = value;
            decimal.TryParse(dt.Rows[2][1].ToString(), out value);
            rule.TimeUnitRule.FirstUnit = value;
            decimal.TryParse(dt.Rows[3][1].ToString(), out value);
            rule.TimeUnitRule.FirstUnitMoney = value;
        }

        private void BindByCountCharge(CardTypeChargeRules rule)
        {
            rule.ChargeMode = 3;
        }
        #endregion

        #region 车位显示屏设置
        Dictionary<string, string> dicColor;
        Dictionary<string, string> dicRollType;
        Dictionary<string, string> dicShowWay;

        private void InitScreenSetPage()
        {
            List<KeyValuePair<int, string>> lstSource;

            //grdDefaultSet.Height = 0;
            grdDefaultSet.Visibility = System.Windows.Visibility.Collapsed;

            dicShowWay = new Dictionary<string, string>();
            dicShowWay.Add("3", "空车位");
            dicShowWay.Add("4", "车牌号");
            dicShowWay.Add("5", "问候语");
            dicShowWay.Add("6", "收费金额");

            dicRollType = new Dictionary<string, string>();
            dicRollType.Add("0", "0 - 无");
            dicRollType.Add("1", "1 - 连续左移");
            dicRollType.Add("2", "2 - 连续右移");
            dicRollType.Add("3", "3 - 连续上移");
            dicRollType.Add("4", "4 - 连续下移");
            dicRollType.Add("11", "11 - 立即打出");

            dicColor = new Dictionary<string, string>();
            dicColor.Add("0", "0 - 红色");
            dicColor.Add("1", "1 - 绿色");
            dicColor.Add("2", "2 - 黄色");

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0 - 无"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - 连续左移"));
            lstSource.Add(new KeyValuePair<int, string>(2, "2 - 连续右移"));
            lstSource.Add(new KeyValuePair<int, string>(3, "3 - 连续上移"));
            lstSource.Add(new KeyValuePair<int, string>(4, "4 - 连续下移"));
            lstSource.Add(new KeyValuePair<int, string>(11, "11 - 立即打出"));
            cbbRollType.DisplayMemberPath = "Value";
            cbbRollType.SelectedValuePath = "Key";
            cbbRollType.ItemsSource = lstSource;
            cbbRollType.SelectedValue = 1;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0 - 红色"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - 绿色"));
            lstSource.Add(new KeyValuePair<int, string>(2, "2 - 黄色"));
            cbbColor.DisplayMemberPath = "Value";
            cbbColor.SelectedValuePath = "Key";
            cbbColor.ItemsSource = lstSource;
            cbbColor.SelectedValue = 0;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1"));
            lstSource.Add(new KeyValuePair<int, string>(2, "2"));
            lstSource.Add(new KeyValuePair<int, string>(3, "3"));
            lstSource.Add(new KeyValuePair<int, string>(4, "4"));
            lstSource.Add(new KeyValuePair<int, string>(5, "5"));
            lstSource.Add(new KeyValuePair<int, string>(6, "6"));
            lstSource.Add(new KeyValuePair<int, string>(7, "7"));
            cbbRollSpeed.DisplayMemberPath = "Value";
            cbbRollSpeed.SelectedValuePath = "Key";
            cbbRollSpeed.ItemsSource = lstSource;
            cbbRollSpeed.SelectedValue = 5;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1"));
            lstSource.Add(new KeyValuePair<int, string>(5, "5"));
            lstSource.Add(new KeyValuePair<int, string>(10, "10"));
            lstSource.Add(new KeyValuePair<int, string>(20, "20"));
            lstSource.Add(new KeyValuePair<int, string>(50, "50"));
            lstSource.Add(new KeyValuePair<int, string>(80, "80"));
            lstSource.Add(new KeyValuePair<int, string>(100, "100"));
            lstSource.Add(new KeyValuePair<int, string>(150, "150"));
            lstSource.Add(new KeyValuePair<int, string>(200, "200"));
            cbbStopTime.DisplayMemberPath = "Value";
            cbbStopTime.SelectedValuePath = "Key";
            cbbStopTime.ItemsSource = lstSource;
            cbbStopTime.SelectedValue = 150;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(10, "10"));
            lstSource.Add(new KeyValuePair<int, string>(20, "20"));
            lstSource.Add(new KeyValuePair<int, string>(30, "30"));
            lstSource.Add(new KeyValuePair<int, string>(40, "40"));
            lstSource.Add(new KeyValuePair<int, string>(50, "50"));
            lstSource.Add(new KeyValuePair<int, string>(60, "60"));
            lstSource.Add(new KeyValuePair<int, string>(70, "70"));
            lstSource.Add(new KeyValuePair<int, string>(80, "80"));
            lstSource.Add(new KeyValuePair<int, string>(90, "90"));
            lstSource.Add(new KeyValuePair<int, string>(100, "100"));
            lstSource.Add(new KeyValuePair<int, string>(120, "120"));
            cbbShowTime.DisplayMemberPath = "Value";
            cbbShowTime.SelectedValuePath = "Key";
            cbbShowTime.ItemsSource = lstSource;
            cbbShowTime.SelectedValue = 100;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(2, "2"));
            lstSource.Add(new KeyValuePair<int, string>(6, "6"));
            lstSource.Add(new KeyValuePair<int, string>(8, "8"));
            cbbShowType.DisplayMemberPath = "Value";
            cbbShowType.SelectedValuePath = "Key";
            cbbShowType.ItemsSource = lstSource;
            cbbShowType.SelectedValue = 6;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(1, "1"));
            lstSource.Add(new KeyValuePair<int, string>(2, "2"));
            lstSource.Add(new KeyValuePair<int, string>(3, "3"));
            lstSource.Add(new KeyValuePair<int, string>(4, "4"));
            lstSource.Add(new KeyValuePair<int, string>(5, "5"));
            lstSource.Add(new KeyValuePair<int, string>(6, "6"));
            lstSource.Add(new KeyValuePair<int, string>(7, "7"));
            cbbModuleCount.DisplayMemberPath = "Value";
            cbbModuleCount.SelectedValuePath = "Key";
            cbbModuleCount.ItemsSource = lstSource;
            cbbModuleCount.SelectedValue = 3;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - T08单色"));
            lstSource.Add(new KeyValuePair<int, string>(2, "2 - T08双色"));
            lstSource.Add(new KeyValuePair<int, string>(3, "3 - T12单色"));
            lstSource.Add(new KeyValuePair<int, string>(4, "4 - T12双色"));
            lstSource.Add(new KeyValuePair<int, string>(5, "5 - T08小字库模式"));
            lstSource.Add(new KeyValuePair<int, string>(6, "6 - T08大字库模式"));
            cbbInterfaceType.DisplayMemberPath = "Value";
            cbbInterfaceType.SelectedValuePath = "Key";
            cbbInterfaceType.ItemsSource = lstSource;
            cbbInterfaceType.SelectedValue = 3;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0 - 滚动方式不变"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - 滚动随机变化"));
            cbbShowMode.DisplayMemberPath = "Value";
            cbbShowMode.SelectedValuePath = "Key";
            cbbShowMode.ItemsSource = lstSource;
            cbbShowMode.SelectedValue = 0;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0 - 反相"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - 不反相"));
            cbbEnabled.DisplayMemberPath = "Value";
            cbbEnabled.SelectedValuePath = "Key";
            cbbEnabled.ItemsSource = lstSource;
            cbbEnabled.SelectedValue = 0;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0 - 时间"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - 广告"));
            lstSource.Add(new KeyValuePair<int, string>(2, "2 - 空车位"));
            cbbDefauleShow.DisplayMemberPath = "Value";
            cbbDefauleShow.SelectedValuePath = "Key";
            cbbDefauleShow.ItemsSource = lstSource;
            cbbDefauleShow.SelectedValue = 2;

            LoadLedSetting();
        }

        private void LoadLedSetting()
        {
            int tmp;
            string[] strArray;
            StringBuilder strBldr;
            Request req;
            List<ParkingModel.LedSetting> lstSetting;

            req = new Request();
            lstSetting = req.GetData<List<ParkingModel.LedSetting>>("GetLedSetting");

            if (null != lstSetting)
            {
                foreach (var setting in lstSetting)
                {
                    setting.Color = dicColor.ContainsKey(setting.Color ?? "") ? dicColor[setting.Color] : setting.Color;
                    if (int.TryParse(setting.Move, out tmp))
                    {
                        tmp = tmp >> (tmp > 15 ? 4 : 0);
                        setting.Move = dicRollType.ContainsKey(tmp.ToString()) ? dicRollType[tmp.ToString()] : tmp.ToString();
                    }

                    strArray = null == setting.ShowWay ? null : setting.ShowWay.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (null == strArray || strArray.Length <= 0)
                    {
                        continue;
                    }

                    strBldr = new StringBuilder();
                    foreach (string str in strArray)
                    {
                        if (!dicShowWay.ContainsKey(str))
                        {
                            continue;
                        }

                        strBldr.AppendFormat("{0},", dicShowWay[str]);
                    }

                    if (strBldr.Length > 0)
                    {
                        strBldr.Remove(strBldr.Length - 1, 1);
                    }

                    setting.ShowWay = strBldr.ToString();
                }
            }

            dgScreenParam.CanUserAddRows = false;
            dgScreenParam.CanUserDeleteRows = false;
            dgScreenParam.AutoGenerateColumns = false;
            dgScreenParam.ItemsSource = lstSetting;
        }

        private ParkingModel.LedSetting BindLedSetting(CheDaoSet channel)
        {
            StringBuilder strBldr;
            ParkingModel.LedSetting Setting;

            if (null == channel)
            {
                return null;
            }

            Setting = new LedSetting();
            Setting.Color = ((KeyValuePair<int, string>)cbbColor.SelectedItem).Key.ToString();
            Setting.CPHEndStr = txtGreetings.Text;
            Setting.CtrID = channel.CtrlNumber;
            Setting.Move = (((KeyValuePair<int, string>)cbbRollType.SelectedItem).Key << (ckbShowInHigher.IsChecked ?? false ? 4 : 0)).ToString();
            Setting.Pattern = ((KeyValuePair<int, string>)cbbShowType.SelectedItem).Key.ToString();
            //Setting.ShowWay = ((KeyValuePair<int, string>)cbbColor.SelectedItem).Key.ToString();
            Setting.Speed = ((KeyValuePair<int, string>)cbbRollSpeed.SelectedItem).Key.ToString();
            Setting.StationID = ParkingModel.Model.stationID;
            Setting.StopTime = ((KeyValuePair<int, string>)cbbStopTime.SelectedItem).Key.ToString();
            Setting.SumTime = ((KeyValuePair<int, string>)cbbShowTime.SelectedItem).Key.ToString();
            Setting.SurplusID = numScreenNumber.Value.ToString();

            strBldr = new StringBuilder();
            strBldr.Append((ckbEmptyPlaceNumber.IsChecked ?? false) ? "3," : "");
            strBldr.Append((ckbCarPlateNumber.IsChecked ?? false) ? "4," : "");
            strBldr.Append((ckbGreetings.IsChecked ?? false) ? "5," : "");
            strBldr.Append((ckbCharge.IsChecked ?? false) ? "6," : "");
            if (strBldr.Length > 0)
            {
                strBldr.Remove(strBldr.Length - 1, 1);
            }
            Setting.ShowWay = strBldr.ToString();

            return Setting;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            StringBuilder strBldr;
            ParkingModel.CheDaoSet channel;
            ParkingModel.LedSetting Setting;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            for (int i = 0; i < dgChannel.Items.Count; i++)
            {
                FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                if (cb.IsChecked == true)
                {
                    channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                    if (null == channel)
                    {
                        continue;
                    }
                    lstChannel.Add(channel);
                }
            }

            if (lstChannel.Count <= 0)
            {
                MessageBox.Show("请选择车道");
                return;
            }
            if (lstChannel.Count > 1)
            {
                MessageBox.Show("一次只能设置一个车道，请重新选择车道");
                return;
            }

            Setting = BindLedSetting(lstChannel[0]);

            req = new Request();
            req.AddData("AddLedSetting", Setting);

            LoadLedSetting();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Request req;
            ParkingModel.LedSetting Setting;

            Setting = dgScreenParam.SelectedItem as ParkingModel.LedSetting;
            if (null == Setting)
            {
                return;
            }

            req = new Request();
            req.AddData("DeleteLedSetting", Setting);

            LoadLedSetting();
        }

        private void btnShowNow_Click(object sender, RoutedEventArgs e)
        {
            int result;
            CheDaoSet channel;
            LedSetting Setting;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        Setting = BindLedSetting(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        result = cmder.SetLedSetting(Setting, txtScreenText.Text, ckbShowFullWidth.IsChecked ?? false, (int)numCarPlaceNumber.Value);
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                MessageBox.Show("即时显示成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("即时显示失败: " + ex.Message);
            }
        }

        private void txtAdText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ("LEDSET" != txtAdText.Text.ToUpper())
            {
                return;
            }

            //grdDefaultSet.Height = 107;
            grdDefaultSet.Visibility = System.Windows.Visibility.Visible;

            tabWisdom.Visibility = System.Windows.Visibility.Visible;
            tcControlSet.SelectedItem = tabWisdom;
        }

        private void btnSetDefaultSet_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            StringBuilder strBldr;
            StringBuilder strBldrTmp;
            ParkingModel.CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                strBldrTmp = new StringBuilder();
                strBldrTmp.Append("05");
                int.TryParse((cbbRollType.SelectedValue ?? "").ToString(), out result);
                result = result << (ckbShowInHigher.IsChecked ?? false ? 4 : 0);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbRollSpeed.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbStopTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbColor.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbShowTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbModuleCount.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbInterfaceType.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbEnabled.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbDefauleShow.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbShowMode.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));

                buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                foreach (byte b in buffer)
                {
                    sum += b;
                }
                sum = sum % 256;

                strBldr = new StringBuilder();
                strBldr.Append("AA");
                strBldr.Append(numScreenNumber.Value.ToString("00"));
                strBldr.Append("BB4144");
                strBldr.Append(sum.ToString("X2"));
                strBldr.Append(strBldrTmp);
                strBldr.Append("FF");

                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        cmder.SetLedSetting(strBldr.ToString());
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                MessageBox.Show("加载出厂参数成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载出厂参数失败: " + ex.Message);
            }
        }

        private void btnSetAd_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            StringBuilder strBldr;
            StringBuilder strBldrTmp;
            ParkingModel.CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                strBldrTmp = new StringBuilder();
                strBldrTmp.Append("03");

                int.TryParse((cbbRollType.SelectedValue ?? "").ToString(), out result);
                result = result << (ckbShowInHigher.IsChecked ?? false ? 4 : 0);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbRollSpeed.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbStopTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbColor.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbShowTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));

                int.TryParse((cbbModuleCount.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                strBldrTmp.Append(DeviceCommander.ByteToHexString(Encoding.Default.GetBytes(txtAdText.Text)));

                buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                foreach (byte b in buffer)
                {
                    sum += b;
                }
                sum = sum % 256;

                strBldr = new StringBuilder();
                strBldr.Append("AA");
                strBldr.Append(numScreenNumber.Value.ToString("00"));
                strBldr.Append("BB4144");
                strBldr.Append(sum.ToString("X2"));
                strBldr.Append(strBldrTmp);
                strBldr.Append("FF");

                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        cmder.SetLedSetting(strBldr.ToString());
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                MessageBox.Show("加载广告成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载广告失败: " + ex.Message);
            }
        }

        private void btnSetDeviceNumber_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            StringBuilder strBldr;
            StringBuilder strBldrTmp;
            ParkingModel.CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                strBldrTmp = new StringBuilder();
                strBldrTmp.Append("060103200003");

                strBldrTmp.Append(numScreenNumber.Value.ToString("00"));

                strBldrTmp.Append("01");

                buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                foreach (byte b in buffer)
                {
                    sum += b;
                }
                sum = sum % 256;

                strBldr = new StringBuilder();
                strBldr.Append("AA00BB4144");
                strBldr.Append(sum.ToString("X2"));
                strBldr.Append(strBldrTmp);
                strBldr.Append("FF");

                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        cmder.SetLedSetting(strBldr.ToString());
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                MessageBox.Show("设置机号成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置机号失败: " + ex.Message);
            }
        }

        private void btnSetTime_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            string strTmp;
            StringBuilder strBldr;
            StringBuilder strBldrTmp;
            ParkingModel.CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                strBldrTmp = new StringBuilder();
                strBldrTmp.Append("01");

                int.TryParse((cbbRollType.SelectedValue ?? "").ToString(), out result);
                result = result << (ckbShowInHigher.IsChecked ?? false ? 4 : 0);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbRollSpeed.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbStopTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbColor.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbShowTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));

                strTmp = DateTime.Now.ToString("yyMMddHHmmss");
                for (int i = 0; i < strTmp.Length; i++)
                {
                    strBldrTmp.AppendFormat("3{0}", strTmp[i]);
                }

                buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                foreach (byte b in buffer)
                {
                    sum += b;
                }
                sum = sum % 256;

                strBldr = new StringBuilder();
                strBldr.Append("AA");
                strBldr.Append(numScreenNumber.Value.ToString("00"));
                strBldr.Append("BB4144");
                strBldr.Append(sum.ToString("X2"));
                strBldr.Append(strBldrTmp);
                strBldr.Append("FF");

                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        cmder.SetLedSetting(strBldr.ToString());
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                MessageBox.Show("加载时间成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载时间失败: " + ex.Message);
            }
        }

        private void btnSetPreCarPlace_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            StringBuilder strBldr;
            StringBuilder strBldrTmp;
            ParkingModel.CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                strBldrTmp = new StringBuilder();
                strBldrTmp.Append("07");

                int.TryParse((cbbRollType.SelectedValue ?? "").ToString(), out result);
                result = result << (ckbShowInHigher.IsChecked ?? false ? 4 : 0);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbRollSpeed.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbStopTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbColor.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbShowTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));

                strBldrTmp.Append(DeviceCommander.ByteToHexString(Encoding.Default.GetBytes(txtScreenText.Text)));

                buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                foreach (byte b in buffer)
                {
                    sum += b;
                }
                sum = sum % 256;

                strBldr = new StringBuilder();
                strBldr.Append("AA");
                strBldr.Append(numScreenNumber.Value.ToString("00"));
                strBldr.Append("BB4144");
                strBldr.Append(sum.ToString("X2"));
                strBldr.Append(strBldrTmp);
                strBldr.Append("FF");

                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        cmder.SetLedSetting(strBldr.ToString());
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                MessageBox.Show("加载车位数前辍成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载车位数前辍失败: " + ex.Message);
            }
        }
        #endregion

        #region 智慧眼
        //private void InitWisdomPage()
        //{
        //    List<KeyValuePair<int, string>> lstSource;

        //    lstSource = new List<KeyValuePair<int, string>>();
        //    lstSource.Add(new KeyValuePair<int, string>(0, "0 - 时间"));
        //    lstSource.Add(new KeyValuePair<int, string>(1, "1 - 广告"));
        //    lstSource.Add(new KeyValuePair<int, string>(2, "2 - 空车位"));
        //    cbbParam1.DisplayMemberPath = "Value";
        //    cbbParam1.SelectedValuePath = "Key";
        //    cbbParam1.ItemsSource = lstSource;
        //    cbbParam1.SelectedValue = 2;

        //    lstSource = new List<KeyValuePair<int, string>>();
        //    lstSource.Add(new KeyValuePair<int, string>(0, "0 - 不带P"));
        //    lstSource.Add(new KeyValuePair<int, string>(1, "1 - 带P"));
        //    cbbParam2.DisplayMemberPath = "Value";
        //    cbbParam2.SelectedValuePath = "Key";
        //    cbbParam2.ItemsSource = lstSource;
        //    cbbParam2.SelectedValue = 0;
        //}

        //private void btnWisdomAd_Click(object sender, RoutedEventArgs e)
        //{
        //    int sum = 0;
        //    int result = 0;
        //    byte[] buffer;
        //    StringBuilder strBldr;
        //    StringBuilder strBldr1;
        //    StringBuilder strBldrTmp;
        //    ParkingModel.CheDaoSet channel;
        //    DeviceCommander cmder;

        //    channel = dgChannel.SelectedItem as ParkingModel.CheDaoSet;
        //    if (null == channel)
        //    {
        //        MessageBox.Show("请选择车道");
        //        return;
        //    }
        //    if (IsChineseCh(txtWisdomAd.Text) == false && txtWisdomAd.Text != "")
        //    {
        //        MessageBox.Show("输入的不是汉字");
        //        return;
        //    }
        //    if (txtWisdomAd.Text.Length % 8 > 0 && txtWisdomAd.Text != "")
        //    {
        //        MessageBox.Show("第二三行广告请加载字数为8的倍数！", "提示");
        //        return;
        //    }

        //    try
        //    {
        //        buffer = Encoding.Default.GetBytes(txtWisdomAd.Text);

        //        strBldrTmp = new StringBuilder();
        //        strBldrTmp.Append("070105960064");
        //        strBldrTmp.Append(buffer[0].ToString("X2")[0]);

        //        buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
        //        foreach (byte b in buffer)
        //        {
        //            sum += b;
        //        }
        //        sum = sum % 256;

        //        strBldr = new StringBuilder();
        //        strBldr.Append("AA");
        //        strBldr.Append(numWisdomScreenNum.Value.ToString("00"));
        //        strBldr.Append("BB4144");
        //        strBldr.Append(sum.ToString("X2"));
        //        strBldr.Append(strBldrTmp);
        //        strBldr.Append("FF");

        //        buffer = Encoding.Default.GetBytes(txtWisdomAd.Text);

        //        strBldrTmp = new StringBuilder();
        //        strBldrTmp.Append("070105960064");
        //        strBldrTmp.Append(buffer[0].ToString("X2")[1]);

        //        buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
        //        foreach (byte b in buffer)
        //        {
        //            sum += b;
        //        }
        //        sum = sum % 256;

        //        strBldr1 = new StringBuilder();
        //        strBldr1.Append("AA");
        //        strBldr1.Append(numWisdomScreenNum.Value.ToString("00"));
        //        strBldr1.Append("BB4144");
        //        strBldr1.Append(sum.ToString("X2"));
        //        strBldr1.Append(strBldrTmp);
        //        strBldr1.Append("FF");

        //        foreach (var item in dgChannel.SelectedItems)
        //        {
        //            channel = dgChannel.SelectedItem as ParkingModel.CheDaoSet;
        //            if (null == channel)
        //            {
        //                continue;
        //            }

        //            cmder = DeviceCommander.GetCommander(channel);
        //            cmder.SetLedSetting(strBldr.ToString());
        //        }

        //        MessageBox.Show("加载成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("加载失败: " + ex.Message);
        //    }
        //}

        //private void btnWisdomAd2_Click(object sender, RoutedEventArgs e)
        //{
        //    int sum = 0;
        //    int result = 0;
        //    byte[] buffer;
        //    StringBuilder strBldr;
        //    StringBuilder strBldrTmp;
        //    ParkingModel.CheDaoSet channel;
        //    DeviceCommander cmder;

        //    channel = dgChannel.SelectedItem as ParkingModel.CheDaoSet;
        //    if (null == channel)
        //    {
        //        MessageBox.Show("请选择车道");
        //        return;
        //    }

        //    try
        //    {
        //        strBldrTmp = new StringBuilder();
        //        strBldrTmp.Append("031005960064");

        //        strBldrTmp.Append(((int)numWisdomScreenNum.Value).ToString("X2"));

        //        strBldrTmp.Append(DeviceCommander.ByteToHexString(Encoding.Default.GetBytes(txtWisdomAd2.Text)));

        //        buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
        //        foreach (byte b in buffer)
        //        {
        //            sum += b;
        //        }
        //        sum = sum % 256;

        //        strBldr = new StringBuilder();
        //        strBldr.Append("AA");
        //        strBldr.Append(numWisdomScreenNum.Value.ToString("00"));
        //        strBldr.Append("BB4144");
        //        strBldr.Append(sum.ToString("X2"));
        //        strBldr.Append(strBldrTmp);
        //        strBldr.Append("FF");

        //        foreach (var item in dgChannel.SelectedItems)
        //        {
        //            channel = dgChannel.SelectedItem as ParkingModel.CheDaoSet;
        //            if (null == channel)
        //            {
        //                continue;
        //            }

        //            cmder = DeviceCommander.GetCommander(channel);
        //            cmder.SetLedSetting(strBldr.ToString());
        //        }

        //        MessageBox.Show("加载成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("加载失败: " + ex.Message);
        //    }
        //}

        //private void btnWisomDefaultSet_Click(object sender, RoutedEventArgs e)
        //{
        //    int sum = 0;
        //    int result = 0;
        //    byte[] buffer;
        //    StringBuilder strBldr;
        //    StringBuilder strBldrTmp;
        //    ParkingModel.CheDaoSet channel;
        //    DeviceCommander cmder;

        //    channel = dgChannel.SelectedItem as ParkingModel.CheDaoSet;
        //    if (null == channel)
        //    {
        //        MessageBox.Show("请选择车道");
        //        return;
        //    }

        //    try
        //    {
        //        strBldrTmp = new StringBuilder();
        //        strBldrTmp.Append("05");

        //        int.TryParse((cbbRollType.SelectedValue ?? "").ToString(), out result);
        //        result = result << (ckbShowInHigher.IsChecked ?? false ? 4 : 0);
        //        strBldrTmp.Append(result.ToString("X2"));
        //        int.TryParse((cbbRollSpeed.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("00"));
        //        int.TryParse((cbbStopTime.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("X2"));
        //        int.TryParse((cbbColor.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("00"));
        //        int.TryParse((cbbShowTime.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("X2"));

        //        strBldrTmp.Append(((int)numWisdomScreenNum.Value).ToString("X2"));

        //        int.TryParse((cbbInterfaceType.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("00"));

        //        int.TryParse((cbbParam2.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("00"));
        //        int.TryParse((cbbParam1.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("00"));

        //        int.TryParse((cbbShowMode.SelectedValue ?? "").ToString(), out result);
        //        strBldrTmp.Append(result.ToString("00"));

        //        strBldrTmp.Append(DeviceCommander.ByteToHexString(Encoding.Default.GetBytes(txtWisdomAd2.Text)));

        //        buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
        //        foreach (byte b in buffer)
        //        {
        //            sum += b;
        //        }
        //        sum = sum % 256;

        //        strBldr = new StringBuilder();
        //        strBldr.Append("AA");
        //        strBldr.Append(numWisdomScreenNum.Value.ToString("00"));
        //        strBldr.Append("BB4144");
        //        strBldr.Append(sum.ToString("X2"));
        //        strBldr.Append(strBldrTmp);
        //        strBldr.Append("FF");

        //        foreach (var item in dgChannel.SelectedItems)
        //        {
        //            channel = dgChannel.SelectedItem as ParkingModel.CheDaoSet;
        //            if (null == channel)
        //            {
        //                continue;
        //            }

        //            cmder = DeviceCommander.GetCommander(channel);
        //            cmder.SetLedSetting(strBldr.ToString());
        //        }

        //        MessageBox.Show("加载成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("加载失败: " + ex.Message);
        //    }
        //}


        //private bool IsChineseCh(string input)   //正则表达式 汉字规则
        //{
        //    Regex regex = new Regex("^[\u4e00-\u9fa5]+$");
        //    return regex.IsMatch(input);
        //}
        #endregion

        #region 广告加载
        private void InitAdLoadPage()
        {
            List<KeyValuePair<int, string>> lstSource;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0 - 时间"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - 广告"));
            lstSource.Add(new KeyValuePair<int, string>(2, "2 - 空车位"));
            cmbDefault0.DisplayMemberPath = "Value";
            cmbDefault0.SelectedValuePath = "Key";
            cmbDefault0.ItemsSource = lstSource;
            cmbDefault0.SelectedValue = 2;

            lstSource = new List<KeyValuePair<int, string>>();
            lstSource.Add(new KeyValuePair<int, string>(0, "0 - 不带P"));
            lstSource.Add(new KeyValuePair<int, string>(1, "1 - 带P"));
            cmbRP0.DisplayMemberPath = "Value";
            cmbRP0.SelectedValuePath = "Key";
            cmbRP0.ItemsSource = lstSource;
            cmbRP0.SelectedValue = 0;

            rdbTwoTransverse.IsChecked = true;
        }

        private bool IsChineseCh(string input)   //正则表达式 汉字规则
        {
            Regex regex = new Regex("^[\u4e00-\u9fa5]+$");
            return regex.IsMatch(input);
        }

        private void ckbLower_Checked(object sender, RoutedEventArgs e)
        {
            gbxAd.Header = "显示内容（汉字、数字、字母，一般用于加载售后电话）";
        }

        private void ckbLower_Unchecked(object sender, RoutedEventArgs e)
        {
            gbxAd.Header = "显示内容(汉字, 字数8,16,24,32)";
        }

        private void rdbTwoTransverse_Checked(object sender, RoutedEventArgs e)
        {
            lblScreenRow2.Content = "自动识别";
            lblScreenRow3.Content = "减速慢行";

            ckbLower.IsEnabled = false;

            lblHorizontal.Visibility = System.Windows.Visibility.Collapsed;
            spnHorizontal.Visibility = System.Windows.Visibility.Collapsed;

            spnVertical.Visibility = System.Windows.Visibility.Visible;
            lblScreenRow1.Visibility = System.Windows.Visibility.Collapsed;
            lblScreenRow2.Visibility = System.Windows.Visibility.Visible;
            lblScreenRow3.Visibility = System.Windows.Visibility.Visible;
            lblScreenRow4.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void rdbTwoColumn_Checked(object sender, RoutedEventArgs e)
        {
            lblScreenCol1.Text = "自动识别";
            lblScreenCol2.Text = "减速慢行";
            lblHorizontal.Content = DateTime.Now.ToString("yyyy");

            ckbLower.IsEnabled = true;

            spnVertical.Visibility = System.Windows.Visibility.Collapsed;

            lblHorizontal.Visibility = System.Windows.Visibility.Visible;
            spnHorizontal.Visibility = System.Windows.Visibility.Visible;
        }

        private void rdbFourTransverse_Checked(object sender, RoutedEventArgs e)
        {
            lblScreenRow1.Content = "自动识别";
            lblScreenRow2.Content = "智慧停车";
            lblScreenRow3.Content = "无需取卡";
            lblScreenRow4.Content = DateTime.Now.ToString("yyyy年M");

            ckbLower.IsEnabled = true;

            lblHorizontal.Visibility = System.Windows.Visibility.Collapsed;
            spnHorizontal.Visibility = System.Windows.Visibility.Collapsed;

            spnVertical.Visibility = System.Windows.Visibility.Visible;
            lblScreenRow1.Visibility = System.Windows.Visibility.Visible;
            lblScreenRow2.Visibility = System.Windows.Visibility.Visible;
            lblScreenRow3.Visibility = System.Windows.Visibility.Visible;
            lblScreenRow4.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnSET1_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            string[] strArray;
            StringBuilder strBldr;
            StringBuilder strBldr1;
            StringBuilder strBldrTmp;
            StringBuilder strBldrErr;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            if (txtAdvertisement1.Text == "")
            {
                MessageBox.Show("输入不能为空！");
                return;
            }
            if (!(ckbLower.IsChecked ?? false))
            {
                if (IsChineseCh(txtAdvertisement1.Text) == false && txtAdvertisement1.Text != "")    //此为退格键可以输入
                {
                    MessageBox.Show("输入的不是汉字！");
                    return;
                }

                if (txtAdvertisement1.Text.Length % 8 > 0 && txtAdvertisement1.Text != "")
                {
                    MessageBox.Show("广告请加载字数为8的倍数！", "提示");
                    return;
                }
            }

            lstChannel = new List<CheDaoSet>();
            try
            {
                if (ckbLower.IsChecked ?? false)
                {
                    strBldrTmp = new StringBuilder();
                    strBldrTmp.Append("03100596006400");
                    strBldrTmp.Append(ByteToHexString(Encoding.Default.GetBytes(txtAdvertisement1.Text)));
                    buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                    foreach (byte b in buffer)
                    {
                        sum += b;
                    }
                    sum = sum % 256;

                    strBldr = new StringBuilder();
                    strBldr.Append("AA00BB4144");
                    strBldr.Append(sum.ToString("X2"));
                    strBldr.Append(strBldrTmp);
                    strBldr.Append("FF");

                    if (rdbFourTransverse.IsChecked ?? false)
                    {
                        lblScreenRow4.Content = txtAdvertisement1.Text.Length > 4 ? txtAdvertisement1.Text.Substring(0, 4) : txtAdvertisement1.Text;
                    }
                    else if (rdbTwoColumn.IsChecked ?? false)
                    {
                        lblHorizontal.Content = txtAdvertisement1.Text.Length > 2 ? txtAdvertisement1.Text.Substring(0, 2) : txtAdvertisement1.Text;
                    }

                    strBldrErr = new StringBuilder();
                    for (int i = 0; i < dgChannel.Items.Count; i++)
                    {
                        FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                        CheckBox cb = (item as CheckBox);
                        if (cb.IsChecked == true)
                        {
                            channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                            if (null == channel)
                            {
                                continue;
                            }
                            lstChannel.Add(channel);

                            cmder = DeviceCommander.GetCommander(channel);
                            result = cmder.SetLedSetting(strBldr.ToString(), (byte)(Model.bAppEnable ? 0x64 : 0x94));

                            if (result <= 0)
                            {
                                strBldrErr.AppendFormat("{0} 错误码: {1}\r\n", channel.InOutName, result);
                            }
                        }
                    }
                }
                else
                {
                    strArray = Getstr(txtAdvertisement1.Text);

                    strBldrTmp = new StringBuilder();
                    strBldrTmp.Append("071005960064");
                    strBldrTmp.Append(ByteToHexString(Encoding.Default.GetBytes(strArray[1])));
                    buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                    foreach (byte b in buffer)
                    {
                        sum += b;
                    }
                    sum = sum % 256;

                    strBldr1 = new StringBuilder();
                    strBldr1.Append("AA00BB4144");
                    strBldr1.Append(sum.ToString("X2"));
                    strBldr1.Append(strBldrTmp);
                    strBldr1.Append("FF");

                    strBldrTmp = new StringBuilder();
                    strBldrTmp.Append("070105960064");
                    strBldrTmp.Append(ByteToHexString(Encoding.Default.GetBytes(strArray[0])));
                    buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                    foreach (byte b in buffer)
                    {
                        sum += b;
                    }
                    sum = sum % 256;

                    strBldr = new StringBuilder();
                    strBldr.Append("AA00BB4144");
                    strBldr.Append(sum.ToString("X2"));
                    strBldr.Append(strBldrTmp);
                    strBldr.Append("FF");

                    if ((rdbTwoTransverse.IsChecked ?? false) || (rdbFourTransverse.IsChecked ?? false))
                    {
                        lblScreenRow2.Content = strArray[0].Substring(0, 4);
                        lblScreenRow3.Content = strArray[1].Substring(0, 4);
                    }
                    else
                    {
                        lblScreenCol1.Text = strArray[0].Substring(0, 4);
                        lblScreenCol2.Text = strArray[1].Substring(0, 4);
                    }

                    strBldrErr = new StringBuilder();
                    for (int i = 0; i < dgChannel.Items.Count; i++)
                    {
                        FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                        CheckBox cb = (item as CheckBox);
                        if (cb.IsChecked == true)
                        {
                            channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                            if (null == channel)
                            {
                                continue;
                            }
                            lstChannel.Add(channel);

                            cmder = DeviceCommander.GetCommander(channel);
                            result = cmder.SetLedSetting(strBldr.ToString(), (byte)(Model.bAppEnable ? 0x64 : 0x94));
                            if (result > 0)
                            {
                                Thread.Sleep(200);
                                result = cmder.SetLedSetting(strBldr1.ToString(), (byte)(Model.bAppEnable ? 0x64 : 0x94));
                            }

                            if (result <= 0)
                            {
                                strBldrErr.AppendFormat("{0} 错误码: {1}\r\n", channel.InOutName, result);
                            }
                        }
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                if (null != strBldrErr && strBldrErr.Length > 0)
                {
                    MessageBox.Show(strBldrErr.ToString());
                }
                else
                {
                    MessageBox.Show("广告加载成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("广告加载失败: " + ex.Message);
            }
        }
        private string[] Getstr(string str)
        {
            string[] strList = new string[2];
            strList[0] = "";
            strList[1] = "";
            for (int i = 0; i < str.Length / 4; i++)
            {
                if (Convert.ToBoolean(i % 2))
                {
                    strList[1] += str.Substring(i * 4, 4);
                }
                else
                {
                    strList[0] += str.Substring(i * 4, 4);
                }
            }
            return strList;
        }

        private void btnSET2_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            StringBuilder strBldr;
            StringBuilder strBldrTmp;
            StringBuilder strBldrErr;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                strBldrTmp = new StringBuilder();
                strBldrTmp.Append("03100596006400");
                buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                foreach (byte b in buffer)
                {
                    sum += b;
                }
                sum = sum % 256;

                strBldr = new StringBuilder();
                strBldr.Append("AA00BB4144");
                strBldr.Append(sum.ToString("X2"));
                strBldr.Append(strBldrTmp);
                strBldr.Append("FF");

                strBldrErr = new StringBuilder();
                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        result = cmder.SetLedSetting(strBldr.ToString(), (byte)(Model.bAppEnable ? 0x64 : 0x94));

                        if (result <= 0)
                        {
                            strBldrErr.AppendFormat("{0} 错误码: {1}\r\n", channel.InOutName, result);
                        }
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                if (null != strBldrErr && strBldrErr.Length > 0)
                {
                    MessageBox.Show(strBldrErr.ToString());
                }
                else
                {
                    MessageBox.Show("恢复默认值成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("恢复默认值失败: " + ex.Message);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int result = 0;
            byte[] buffer;
            StringBuilder strBldr;
            StringBuilder strBldrTmp;
            StringBuilder strBldrErr;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CheDaoSet> lstChannel;

            lstChannel = new List<CheDaoSet>();
            try
            {
                strBldrTmp = new StringBuilder();
                strBldrTmp.Append("05");

                int.TryParse((cbbRollType.SelectedValue ?? "").ToString(), out result);
                result = result << (ckbShowInHigher.IsChecked ?? false ? 4 : 0);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbRollSpeed.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbStopTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));
                int.TryParse((cbbColor.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cbbShowTime.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("X2"));

                strBldrTmp.Append("00");

                int.TryParse((cbbInterfaceType.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));

                int.TryParse((cmbRP0.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));
                int.TryParse((cmbDefault0.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));

                int.TryParse((cbbShowMode.SelectedValue ?? "").ToString(), out result);
                strBldrTmp.Append(result.ToString("00"));

                buffer = DeviceCommander.HexStringToByte(strBldrTmp.ToString());
                foreach (byte b in buffer)
                {
                    sum += b;
                }
                sum = sum % 256;

                strBldr = new StringBuilder();
                strBldr.Append("AA00BB4144");
                strBldr.Append(sum.ToString("X2"));
                strBldr.Append(strBldrTmp);
                strBldr.Append("FF");

                strBldrErr = new StringBuilder();
                for (int i = 0; i < dgChannel.Items.Count; i++)
                {
                    FrameworkElement item = dgChannel.Columns[0].GetCellContent(dgChannel.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannel.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }
                        lstChannel.Add(channel);

                        cmder = DeviceCommander.GetCommander(channel);
                        result = cmder.SetLedSetting(strBldr.ToString(), (byte)(Model.bAppEnable ? 0x64 : 0x94));

                        if (result <= 0)
                        {
                            strBldrErr.AppendFormat("{0} 错误码: {1}\r\n", channel.InOutName, result);
                        }
                    }
                }

                if (lstChannel.Count <= 0)
                {
                    MessageBox.Show("请选择车道");
                    return;
                }

                if (null != strBldrErr && strBldrErr.Length > 0)
                {
                    MessageBox.Show(strBldrErr.ToString());
                }
                else
                {
                    MessageBox.Show("加载成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败: " + ex.Message);
            }
        }
        #endregion

        private void btnAddCarPlateNumber_Click(object sender, RoutedEventArgs e)
        {
            string result;
            string DevFlag;
            Request req;
            CheDaoSet channel;
            DeviceCommander cmder;
            List<CardIssue> lst;
            List<QueryConditionGroup> lstCondition;

            channel = dgChannel.SelectedItem as ParkingModel.CheDaoSet;
            if (null == channel)
            {
                MessageBox.Show("请选择车道");
                return;
            }

            try
            {
                DevFlag = "0".PadLeft(channel.CtrlNumber, '_');

                lstCondition = new List<ParkingModel.QueryConditionGroup>();
                lstCondition.Add(new ParkingModel.QueryConditionGroup());
                lstCondition[0].Add("CarCardType", "like", "Mth%", "or");
                lstCondition[0].Add("CarCardType", "like", "Str%", "or");
                lstCondition[0].Add("CarCardType", "like", "Fre%", "or");
                lstCondition.Add(new ParkingModel.QueryConditionGroup());
                lstCondition[1].Add("CPHDownloadSignal", "like", string.Format("{0}%", DevFlag), "and");

                req = new Request();
                lst = req.GetData<List<ParkingModel.CardIssue>>("GetCardIssue", null, lstCondition);

                if (null == lst || lst.Count <= 0)
                {
                    MessageBox.Show("没有符合条件的数据");
                }

                cmder = DeviceCommander.GetCommander(channel);
                foreach (CardIssue ci in lst)
                {
                    if (ci.CarValidMachine[channel.CtrlNumber - 1] == '0')
                    {
                        result = cmder.SetCarPlateNumber(ci);
                    }
                    else
                    {
                        result = cmder.DeleteCarPlateNumber(ci);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败: " + ex.Message);
            }
        }

        private void btnDeleteCarPlateNumber_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion


        #region 脱机管理

        private void InitOfflineSetIP()
        {
            try
            {
                ObservableCollection<CheDaoSet> lstTmp = new ObservableCollection<CheDaoSet>();
                var lstOut = from c in cds where c.InOut == 1 select c;
                foreach (var chedao in lstOut)
                {
                    lstTmp.Add(chedao);
                }

                //dgChannelOffline.IsReadOnly = true;
                dgChannelOffline.CanUserAddRows = false;
                dgChannelOffline.AutoGenerateColumns = false;
                dgChannelOffline.ItemsSource = cds;
                dgChannelOffline.PreviewMouseLeftButtonUp += DataGrid_PreviewMouseLeftButtonUp;

                //dgOutChannel.IsReadOnly = true;
                dgOutChannel.CanUserAddRows = false;
                dgOutChannel.AutoGenerateColumns = false;
                dgOutChannel.ItemsSource = lstTmp;
                dgOutChannel.PreviewMouseLeftButtonUp += DataGrid_PreviewMouseLeftButtonUp;
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "Window_Loaded", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnReadParam_Click(object sender, RoutedEventArgs e)
        {
            ParkingModel.CheDaoSet channel;
            ParkingModel.CameraVersion verinfo;
            ParkingModel.CheDaoOfflineSet offlineSet;
            ParkingCommunication.DeviceCommander cmder;

            try
            {

                for (int i = 0; i < dgChannelOffline.Items.Count; i++)
                {
                    FrameworkElement item = dgChannelOffline.Columns[0].GetCellContent(dgChannelOffline.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannelOffline.Items[i] as ParkingModel.CheDaoSet;

                        //foreach (object obj in dgChannelOffline.SelectedItems)
                        //{
                        //    channel = obj as ParkingModel.CheDaoSet;
                        if (null == channel)
                        {
                            continue;
                        }

                        cmder = ParkingCommunication.DeviceCommander.GetCommander(channel);

                        //读取摄像机版本号
                        lblVersion.Content = "";
                        lblVersion0.Content = "";
                        lblVersion1.Content = "";
                        lblVersion2.Content = "";
                        if (Model.bAppEnable)
                        {
                            verinfo = cmder.GetCameraVersion();
                            if (null != verinfo)
                            {
                                lblVersion.Content = "摄像机版本号：" + verinfo.Version;
                                lblVersion0.Content = "摄像机版本时间：" + verinfo.ReleaseTime;
                                lblVersion1.Content = "摄像机版本名称：" + verinfo.VersionName;
                                lblVersion2.Content = string.Format("摄像机版本:{0}支持脱机收费", verinfo.IsOfflineChargeSupported ? "" : "不");
                            }
                        }

                        offlineSet = cmder.GetCheDaoOfflineSet();
                        ckbDisableInCharge.IsChecked = false;
                        ckbMthCarNoOpen.IsChecked = false;
                        ckbTmpCarAutoOpenGate.IsChecked = false;
                        ckbTmpCarShowChinese.IsChecked = false;
                        ckbWhiteCarAutoOpen.IsChecked = false;
                        if (null != offlineSet)
                        {
                            ckbDisableInCharge.IsChecked = offlineSet.IsOfflineCameraCannotInCharge;
                            ckbMthCarNoOpen.IsChecked = offlineSet.IsOnlineMthCarDonotOpenGate;
                            ckbTmpCarAutoOpenGate.IsChecked = offlineSet.IsAutoOPenGeteWithTmpCarAtInGate;
                            ckbTmpCarShowChinese.IsChecked = offlineSet.IsTmpCarReadChinese;
                            ckbWhiteCarAutoOpen.IsChecked = offlineSet.IsAutoOpenGateWithWhiteOrZeroPlate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取失败" + ex.Message);
            }
        }

        private void btnSetParam_Click(object sender, RoutedEventArgs e)
        {
            string result;
            ParkingModel.CheDaoSet channel;
            ParkingModel.CheDaoOfflineSet offlineSet;
            ParkingCommunication.DeviceCommander cmder;

            try
            {
                for (int i = 0; i < dgChannelOffline.Items.Count; i++)
                {
                    FrameworkElement item = dgChannelOffline.Columns[0].GetCellContent(dgChannelOffline.Items[i]);
                    CheckBox cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannelOffline.Items[i] as ParkingModel.CheDaoSet;

                        if (null == channel)
                        {
                            continue;
                        }

                        cmder = ParkingCommunication.DeviceCommander.GetCommander(channel);

                        offlineSet = new CheDaoOfflineSet();
                        offlineSet.IsOfflineCameraCannotInCharge = ckbDisableInCharge.IsChecked ?? false;
                        offlineSet.IsOnlineMthCarDonotOpenGate = ckbMthCarNoOpen.IsChecked ?? false;
                        offlineSet.IsAutoOPenGeteWithTmpCarAtInGate = ckbTmpCarAutoOpenGate.IsChecked ?? false;
                        offlineSet.IsTmpCarReadChinese = ckbTmpCarShowChinese.IsChecked ?? false;
                        offlineSet.IsAutoOpenGateWithWhiteOrZeroPlate = ckbWhiteCarAutoOpen.IsChecked ?? false;

                        result = cmder.SetCheDaoOfflineSet(offlineSet);

                        MessageBox.Show("设置成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置失败" + ex.Message);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dgChannelOffline.Items.Count; i++)
            {
                FrameworkElement item = dgChannelOffline.Columns[0].GetCellContent(dgChannelOffline.Items[i]);
                CheckBox cb = (item as CheckBox);
                cb.IsChecked = true;
            }
            for (int i = 0; i < dgOutChannel.Items.Count; i++)
            {
                FrameworkElement item = dgOutChannel.Columns[0].GetCellContent(dgOutChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                cb.IsChecked = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dgChannelOffline.Items.Count; i++)
            {
                FrameworkElement item = dgChannelOffline.Columns[0].GetCellContent(dgChannelOffline.Items[i]);
                CheckBox cb = (item as CheckBox);
                cb.IsChecked = false;
            }
            for (int i = 0; i < dgOutChannel.Items.Count; i++)
            {
                FrameworkElement item = dgOutChannel.Columns[0].GetCellContent(dgOutChannel.Items[i]);
                CheckBox cb = (item as CheckBox);
                cb.IsChecked = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int iCount = 0;
            bool bOK = false;
            string strMsg;
            string strReturn;
            string result;
            string resultShould;
            string FirstIP = null;
            string HexIP;
            string[] IPArray;
            ParkingModel.CheDaoSet channel;
            ParkingModel.CheDaoSet channelOut;
            ParkingCommunication.DeviceCommander cmder;
            ParkingCommunication.SedBll sedBll;
            CheckBox cb;
            FrameworkElement item;

            try
            {
                strMsg = "";
                result = "";
                strReturn = "";
                resultShould = "";

                for (int i = 0; i < dgChannelOffline.Items.Count; i++)
                {
                    item = dgChannelOffline.Columns[0].GetCellContent(dgChannelOffline.Items[i]);
                    cb = (item as CheckBox);
                    if (cb.IsChecked == true)
                    {
                        channel = dgChannelOffline.Items[i] as ParkingModel.CheDaoSet;
                        if (null == channel || channel.XieYi != 1)
                        {
                            continue;
                        }

                        if (null == FirstIP)
                        {
                            FirstIP = channel.IP;
                        }
                        if (FirstIP.Substring(0, FirstIP.LastIndexOf(".")) != channel.IP.Substring(0, channel.IP.LastIndexOf(".")))
                        {
                            if (MessageBox.Show("控制板IP不在同一网段，有可能影响脱机功能的使用，\r\n如果现场不在同一网段的设备能正常通讯，请点击【确定】继续设置；\r\n如果不能通讯请点击【取消】，并重新设置控制板IP。",
                               "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                            {
                                return;
                            }
                        }

                        iCount = 0;
                        sedBll = new ParkingCommunication.SedBll(channel.IP, 1007, 1005);

                        //foreach (var objOut in dgOutChannel.SelectedItems)
                        //{
                        //    channelOut = objOut as ParkingModel.CheDaoSet;
                        for (int j = 0; j < dgOutChannel.Items.Count; j++)
                        {
                            item = dgOutChannel.Columns[0].GetCellContent(dgOutChannel.Items[j]);
                            cb = (item as CheckBox);
                            if (cb.IsChecked == true)
                            {
                                channelOut = dgOutChannel.Items[j] as ParkingModel.CheDaoSet;
                                if (null == channelOut || channelOut.XieYi != 1)
                                {
                                    continue;
                                }

                                iCount++;
                                resultShould += "1";

                                HexIP = "";
                                IPArray = channelOut.IP.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                foreach (string ip in IPArray)
                                {
                                    HexIP += Convert.ToInt32(ip).ToString("X2");
                                }

                                strReturn = sedBll.SetOffLineIP(channel.CtrlNumber, "54" + iCount.ToString("X2") + HexIP + "FFFFFFFF", channel.XieYi);
                                if (strReturn == "0")
                                {
                                    result += "1";
                                }
                                else
                                {
                                    bOK = true;
                                    strMsg += "[" + channel.CtrlNumber + "]";
                                    break;
                                }
                            }

                            for (int idx = iCount; idx < 16; idx++)
                            {
                                if (bOK)
                                {
                                    break;
                                }
                                resultShould += "1";
                                strReturn = sedBll.SetOffLineIP(channel.CtrlNumber, "54" + iCount.ToString("X2") + "FFFFFFFFFFFFFFFF", channel.XieYi);
                                if (strReturn == "0")
                                {
                                    result += "1";
                                }
                                else
                                {
                                    strMsg += "[" + channel.CtrlNumber + "]";
                                    break;
                                }
                                iCount++;
                            }
                        }
                    }
                }

                if (result == resultShould)
                {
                    MessageBox.Show("设置成功！", "提示");
                }
                else
                {
                    MessageBox.Show(strMsg + "通讯不通", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置失败" + ex.Message);
            }
        }
        #endregion


        #region 操作U盘

        ParkingInterface.Request req = new ParkingInterface.Request();

        private void InitUDisk()
        {
            CR.BinDic(req.GetData<List<CardTypeDef>>("GetCardTypeDef"));

            FindUsbDevces();
        }

        #region 查找U盘
        private void FindUsbDevces()
        {
            DriveInfo[] s = DriveInfo.GetDrives();
            foreach (DriveInfo drive in s)
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    // UsbDevc.Add(drive.Name.ToString());
                    lbUdevices.Items.Add(drive.Name.ToString());
                    break;
                }
            }
            ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo.Properties["InterfaceType"].Value.ToString() == "USB")
                {
                    try
                    {
                        //产品名称
                        // Caption.Text = mo.Properties["Caption"].Value.ToString();

                        //总容量
                        // Size.Text = mo.Properties["Size"].Value.ToString();


                        string[] info = mo.Properties["PNPDeviceID"].Value.ToString().Split('&');
                        string[] xx = info[3].Split('\\');
                        //序列号
                        // MessageBox.Show("U盘序列号:" + xx[1]);
                        // PNPDeviceID.Text = xx[1];
                        xx = xx[0].Split('_');

                        //版本号
                        // REV.Text = xx[1];

                        //制造商ID
                        xx = info[1].Split('_');
                        // VID.Text = xx[1];

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }
        #endregion

        private void btReadusb_Click(object sender, RoutedEventArgs e)
        {
            List<ParkingModel.QueryConditionGroup> lstCondition;

            lbInfo.Items.Clear();
            //tishi.Text = "";
            if (lbUdevices.Items.Count < 0)
            {
                MessageBox.Show("没有找到U盘!", "提示", MessageBoxButton.OK);
            }
            else
            {
                if (lbUdevices.SelectedIndex < 0)
                {
                    MessageBox.Show(" 没有选择U盘!", "提示", MessageBoxButton.OK);
                }
                else
                {
                    if (lbUdevcFiles.Items.Count < 0)
                    {
                        MessageBox.Show("没有找到文件!", "提示", MessageBoxButton.OK);
                    }
                    else
                    {
                        if (lbUdevcFiles.SelectedIndex < 0)
                        {
                            MessageBox.Show(" 没有选择文件!", "提示", MessageBoxButton.OK);
                        }
                        else
                        {
                            string path = lbUdevices.SelectedItem.ToString() + lbUdevcFiles.SelectedItem.ToString();
                            string sFileName = lbUdevcFiles.SelectedItem.ToString();
                            if (sFileName.Substring(4, 3).ToUpper() == "REC")
                            {
                                byte[] ReadFileBytes = System.IO.File.ReadAllBytes(path);
                                if (ReadFileBytes.Length < 32)
                                {
                                    MessageBox.Show("文件内容有错误!", "提示", MessageBoxButton.OK);
                                }
                                else
                                {

                                    string DescStr = ParkingCommunication.DeviceCommander.ByteToHexString(DecUsb(ReadFileBytes, 35));//解密出来的十六进制字符串
                                    int scount = DescStr.Length / 64;

                                    string flag = "0"; //判断是进场还是出场
                                    int imainjh = Convert.ToInt32(sFileName.Substring(1, 3));

                                    lstCondition = new List<ParkingModel.QueryConditionGroup>();
                                    lstCondition.Add(new ParkingModel.QueryConditionGroup());
                                    lstCondition[0].Add("InOut", "=", 0, "or");
                                    lstCondition[0].Add("InOut", "=", 1, "or");
                                    lstCondition.Add(new ParkingModel.QueryConditionGroup());
                                    lstCondition[1].Add("CtrlNumber", "=", imainjh, "or");

                                    DataTable dtfl = req.GetData<DataTable>("GetCheDaoSet", null, lstCondition);
                                    if (null == dtfl || dtfl.Rows.Count < 1)
                                    {
                                        MessageBox.Show("车场设置中没有设置【" + imainjh + "】号控制机", "提示", MessageBoxButton.OK);
                                    }
                                    else
                                    {
                                        //tishi.Text = "正在读取,请稍后...";
                                        for (int i = 0; i < scount; i++)
                                        {
                                            string Data = DescStr.Substring(i * 64, 64);
                                            #region 解析十六进制字符串

                                            string sInName = "";//入口名称
                                            string sOutName = "";//出口名称

                                            if (dtfl.Rows[0]["InOut"].ToString() == "0")
                                            {
                                                flag = "0";
                                                sInName = dtfl.Rows[0]["InOutName"].ToString();
                                            }
                                            else
                                            {
                                                flag = "1";
                                                sOutName = dtfl.Rows[0]["InOutName"].ToString();
                                            }

                                            //车牌号
                                            string sumCPH = Data.Substring(46, 8);
                                            string CPH = "";
                                            //if (sumCPH != "38888888")
                                            //{
                                            //    CPH = ParkingInterface.CR.GetHexToCPH(sumCPH.Substring(0, 2)) + sumCPH.Substring(2, 6);
                                            //}
                                            sumCPH = Data.Substring(46, 16);
                                            if (sumCPH != "38383838383838")
                                            {
                                                CPH = CR.GetHexToCPH(sumCPH.Substring(0, 2)) + CR.GetAscii(sumCPH.Substring(2, 12));
                                            }

                                            //卡片类型
                                            string CardType = "";
                                            //卡号
                                            string CardNO = "";
                                            if (Data.Substring(0, 1) == "C")
                                            {
                                                CardType = CR.GetCardType(Data.Substring(16, 2));
                                                if (Data.Substring(8, 4) == "0000")
                                                {
                                                    CardNO = Data.Substring(12, 4);
                                                    CardNO = Convert.ToInt32(CardNO, 16).ToString("00000");
                                                }
                                                else
                                                {
                                                    CardNO = Data.Substring(8, 8);
                                                }
                                            }
                                            else
                                            {
                                                CardNO = Data.Substring(8, 8);
                                                //CardType = bll.GetCardType(CardNO);

                                                lstCondition = new List<ParkingModel.QueryConditionGroup>();
                                                lstCondition.Add(new ParkingModel.QueryConditionGroup());
                                                lstCondition[0].Add("CardNO", "=", CardNO);

                                                List<ParkingModel.CardIssue> lst = req.GetData<List<ParkingModel.CardIssue>>("GetCardIssue", null, lstCondition);
                                                CardType = (null != lst && lst.Count > 0) ? lst[0].CarCardType : "";

                                                if (CardType == "")
                                                {
                                                    CardType = "   ";
                                                }
                                            }

                                            //入场时间
                                            DateTime dtInTime = DateTime.Now;
                                            //if (CardNO.Length >= 8)
                                            //{

                                            //    dtInTime = DateTime.Now;
                                            //}
                                            //else
                                            //{
                                            string inYear = DateTime.Now.Year.ToString().Substring(0, 3) + Data.Substring(18, 1);
                                            if (Convert.ToInt32(inYear) > DateTime.Now.Year)
                                            {
                                                inYear = (Convert.ToInt32(inYear) - 10).ToString();
                                            }
                                            string inMouth = Convert.ToInt32(Data.Substring(19, 1), 16).ToString();
                                            string strIntime = inYear + "-" + inMouth + "-" + Data.Substring(20, 2) + " " + Data.Substring(22, 2) + ":" + Data.Substring(24, 2) + ":" + Data.Substring(26, 2);


                                            //}
                                            //出场时间
                                            //string OutTime = "";    
                                            string OutYear = DateTime.Now.Year.ToString().Substring(0, 3) + Data.Substring(28, 1);
                                            if (Convert.ToInt32(OutYear) > DateTime.Now.Year)
                                            {
                                                OutYear = (Convert.ToInt32(OutYear) - 10).ToString();
                                            }
                                            string OutMouth = Convert.ToInt32(Data.Substring(29, 1), 16).ToString();
                                            string strOuttime = OutYear + "-" + OutMouth + "-" + Data.Substring(30, 2) + " " + Data.Substring(32, 2) + ":" + Data.Substring(34, 2) + ":" + Data.Substring(36, 2);
                                            DateTime dtOutTime = DateTime.Now;

                                            if (ParkingInterface.CR.IsTime(strOuttime))
                                            {
                                                dtOutTime = Convert.ToDateTime(strOuttime);
                                            }
                                            else
                                            {
                                                dtOutTime = DateTime.Now;
                                            }

                                            if (ParkingInterface.CR.IsTime(strIntime))
                                            {
                                                dtInTime = Convert.ToDateTime(strIntime);
                                            }
                                            else
                                            {
                                                dtInTime = dtOutTime;
                                            }

                                            //余额
                                            int Balance = 0;
                                            if (CardType.Substring(0, 3) == "Str" && Data.Substring(0, 1) == "C")
                                            {
                                                Balance = Convert.ToInt32(Data.Substring(38, 4));
                                            }
                                            //收费金额
                                            int SFJE = Convert.ToInt32(Data.Substring(42, 4), 16);

                                            if (flag == "0" && CardType.Substring(0, 3) != "Opt")
                                            {
                                                //Model.AdmissionModel model = new Model.AdmissionModel();
                                                //model.CardNO = CardNO;
                                                //model.CPH = CPH;
                                                //model.CardType = CardType;
                                                //model.InTime = dtInTime;
                                                //model.OutTime = dtOutTime;
                                                //model.InGateName = sInName;
                                                //model.InOperator = Model.PubVal.sUserName;
                                                //model.InOperatorCard = Model.PubVal.sUserCard;
                                                //model.OutOperatorCard = "";
                                                //model.OutOperator = "";
                                                //model.SFJE = SFJE;
                                                //model.SFTime = DateTime.Now;
                                                //model.OvertimeSFTime = DateTime.Now;
                                                //model.InOut = Convert.ToInt32(dtfl.Rows[0]["InOut"].ToString());
                                                //model.BigSmall = Convert.ToInt32(dtfl.Rows[0]["BigSmall"].ToString());
                                                //bll.AddAdmission(model, 10);

                                                ParkingModel.CarIn inRecord = new ParkingModel.CarIn();
                                                inRecord.CardNO = CardNO;
                                                inRecord.CPH = CPH;
                                                inRecord.CardType = CardType;
                                                inRecord.InTime = dtInTime;
                                                inRecord.OutTime = dtOutTime;
                                                inRecord.InGateName = sInName;
                                                inRecord.InOperator = Model.sUserName;
                                                inRecord.InOperatorCard = Model.sUserCard;
                                                inRecord.OutOperatorCard = "";
                                                inRecord.OutOperator = "";
                                                inRecord.SFJE = SFJE;
                                                inRecord.SFTime = DateTime.Now;
                                                inRecord.OvertimeSFTime = DateTime.Now;
                                                //inRecord.InOut = Convert.ToInt32(dtfl.Rows[0]["InOut"].ToString());
                                                inRecord.BigSmall = Convert.ToInt32(dtfl.Rows[0]["BigSmall"].ToString());

                                                if (Convert.ToInt32(dtfl.Rows[0]["InOut"].ToString()) == 2)
                                                {
                                                    if (inRecord.CardType.Substring(0, 3) == "Tmp")
                                                    {
                                                        if (Model.iXsd == 0)
                                                        {
                                                            if (Model.iChargeType == 3)
                                                            {
                                                                if (Model.iXsdNum == 1)
                                                                {
                                                                    inRecord.SFJE = inRecord.SFJE / 10;
                                                                }
                                                                else
                                                                {
                                                                    inRecord.SFJE = inRecord.SFJE / 100;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                inRecord.SFJE = inRecord.SFJE;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            inRecord.SFJE = inRecord.SFJE / 10;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        inRecord.SFJE = 0;
                                                    }
                                                    inRecord.SFGate = inRecord.InGateName;
                                                    inRecord.SFOperatorCard = inRecord.InOperatorCard;
                                                    inRecord.SFOperator = inRecord.InOperator;
                                                }
                                                else
                                                {
                                                    inRecord.SFJE = 0;
                                                    inRecord.SFGate = inRecord.OutUser;
                                                    inRecord.SFOperatorCard = inRecord.SFOperatorCard;
                                                    inRecord.SFOperator = "";
                                                }

                                                inRecord.OnlineState_In = 2;
                                                req.SetCarIn(inRecord);

                                                //lbInfo.Items.Add("卡号:" + CardNO + "  " + "卡类:" + CardType + "  入场时间:" + dtInTime + "  入场名称:" + sInName);
                                                lbInfo.Items.Add("卡号:" + CardNO + "  车牌号码：" + CPH + " 卡类:" + CR.GetCardType(CardType, 1) + "  入场时间:" + dtInTime + "  入场名称:" + sInName);
                                            }
                                            else
                                            {
                                                if (CardType.Contains("Str"))
                                                {
                                                    //BLL.IssueCardBLL Ibll = new BLL.IssueCardBLL();
                                                    //Ibll.UpdateICYU(CardNO, Balance);

                                                    lstCondition = new List<QueryConditionGroup>();
                                                    lstCondition.Add(new QueryConditionGroup());
                                                    lstCondition[0].Add("CardNO", "=", CardNO);

                                                    req.UpdateField("UpdateCardIssueFields", new { Balance = Balance }, lstCondition);
                                                }
                                                //Model.OutNameModel model = new Model.OutNameModel();
                                                //model.CardNO = CardNO;
                                                //model.CPH = CPH;
                                                //model.CardType = CardType;
                                                //model.InTime = dtInTime;
                                                //model.OutTime = dtOutTime;
                                                //model.OutGateName = sOutName;
                                                //model.OutOperator = Model.PubVal.sUserName;
                                                //model.OutOperatorCard = Model.PubVal.sUserCard;
                                                //model.SFJE = SFJE;
                                                //model.Balance = Balance;
                                                //model.SFTime = dtOutTime;
                                                //model.OvertimeSFTime = DateTime.Now;
                                                //model.InOut = Convert.ToInt32(dtfl.Rows[0]["InOut"].ToString());
                                                //model.BigSmall = Convert.ToInt32(dtfl.Rows[0]["BigSmall"].ToString());
                                                //bll.AddOutName(model, 10);

                                                CarOut outRecord = new CarOut();
                                                outRecord.CardNO = CardNO;
                                                outRecord.CPH = CPH;
                                                outRecord.CardType = CardType;
                                                outRecord.InTime = dtInTime;
                                                outRecord.OutTime = dtOutTime;
                                                outRecord.OutGateName = sOutName;
                                                outRecord.OutOperator = Model.sUserName;
                                                outRecord.OutOperatorCard = Model.sUserCard;
                                                outRecord.SFJE = SFJE;
                                                outRecord.Balance = Balance;
                                                outRecord.SFTime = dtOutTime;
                                                outRecord.OvertimeSFTime = DateTime.Now;
                                                outRecord.ID = Convert.ToInt32(dtfl.Rows[0]["InOut"].ToString());
                                                outRecord.BigSmall = Convert.ToInt32(dtfl.Rows[0]["BigSmall"].ToString());
                                                outRecord.OnlineState_Out = 2;

                                                req.SetCarOut(outRecord);

                                                //lbInfo.Items.Add("卡号:" + CardNO + "  " + "卡类:" + CardType + "  出场时间:" + dtOutTime + "  出场名称:" + sOutName);
                                                lbInfo.Items.Add("卡号:" + CardNO + "  车牌号码：" + CPH + " 卡类:" + CR.GetCardType(CardType, 1) + "  出场时间:" + dtOutTime + "  出场名称:" + sOutName);
                                            }

                                            #endregion

                                        }
                                        //tishi.Text = "从" + sFileName + "文件中成功读取" + scount + "刷卡记录。";
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("请选择REC文件", "提示", MessageBoxButton.OK);
                            }
                        }
                    }
                }
            }
        }

        private void btDownLoadUSB_new_Click(object sender, RoutedEventArgs e)
        {
            List<QueryConditionGroup> lstCondition;

            try
            {
                lbInfo.Items.Clear();
                //tishi.Text = "";
                if (lbUdevices.Items.Count < 0)
                {
                    MessageBox.Show("没有找到U盘!", "提示", MessageBoxButton.OK);
                }
                else
                {
                    if (lbUdevices.SelectedIndex < 0)
                    {
                        MessageBox.Show(" 没有选择U盘!", "提示", MessageBoxButton.OK);
                    }
                    else
                    {
                        #region 处理控制机
                        //DataTable dt = bll.MacJihaoCheChang();
                        DataTable dt = req.GetData<DataTable>("GetCheDaoSet");
                        string path = "";
                        // string path = lbUdevices.SelectedItem.ToString() + lbUdevcFiles.SelectedItem.ToString();//文件路径
                        string CtrNumber = "";

                        byte[] byteMiw = null;
                        for (int i = 0; i < dt.Rows.Count; i++)//根据查询的机号轮循
                        {
                            CtrNumber = dt.Rows[i]["CtrlNumber"].ToString();//机号
                            path = lbUdevices.SelectedItem.ToString() + "U" + CtrNumber.PadLeft(3, '0') + "USR.txt";//文件路径
                            string strRec = "";
                            //DataTable dtFXinfo = bll.dtJmhdb(Convert.ToInt32(CtrNumber));//通过机号找到符合条件的发卡信息表

                            lstCondition = new List<QueryConditionGroup>();
                            lstCondition.Add(new QueryConditionGroup());
                            lstCondition[0].Add("CardNO", "like", "________");
                            lstCondition[0].Add("CarValidMachine", "like", "".PadLeft(Convert.ToInt32(CtrNumber) - 1, '_') + "0%");
                            lstCondition.Add(new QueryConditionGroup());
                            lstCondition[1].Add("CardState", "=", 0, "or");
                            lstCondition[1].Add("CardState", "=", 3, "or");

                            DataTable dtFXinfo = req.GetData<DataTable>("GetCardIssue", null, lstCondition, "CarIssueDate");

                            if (null == dtFXinfo || dtFXinfo.Rows.Count <= 0)
                            {
                                continue;
                            }

                            foreach (DataRow dr in dtFXinfo.Rows)
                            {
                                //if (checkBox2.Checked)
                                //{
                                if (dr["CPH"].ToString() != "" && dr["CPH"].ToString() != "66666666" && dr["CPH"].ToString() != "88888888")
                                {
                                    strRec = strRec + CR.GetDownLoadToCPH(dr);
                                }
                                else
                                {
                                    strRec = strRec + CR.GetDownLoad(dr);
                                }
                                //}
                                //else
                                //{
                                //    strRec = strRec + CR.CR.GetDownLoad(dr);
                                //}
                                lbInfo.Items.Add("【" + CtrNumber + "】 车辆编号:" + dr["CardNO"].ToString() + "  车辆类型:" + dr["CarCardType"].ToString() + "  期限:" + dr["CarValidStartDate"].ToString() + " 至 " + dr["CarValidEndDate"].ToString());

                            }
                            strRec = strRec + "0000000000000000000000000000AABB";

                            byteMiw = GetArray(strRec);//组合成的数组

                            byte[] encodbyte = EncodeUsb(byteMiw, 16);//加密

                            if (!File.Exists(path))
                            {
                                File.Create(path).Close();
                            }
                            else
                            {
                                File.Delete(path);
                                File.Create(path).Close();
                            }

                            File.WriteAllBytes(path, encodbyte);//写到文件中去

                        }
                        #endregion

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nbtDownLoadUSB_new_Click");

            }
        }


        #region 解密
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Filebytes">需要解密的十六进制</param>
        /// <param name="Bytescount">多少个字节 为一组</param>
        /// <returns>已经解好的十六进制</returns>
        private byte[] DecUsb(byte[] Filebytes, int Bytescount)
        {
            byte[] Decbytes = null;//已经解密的十六进制
            int maxnum = (Filebytes.Length / Bytescount); //除的数,查看是否整除，如果整除就是多少个35条，不整除就加一
            int lastnum = (Filebytes.Length % Bytescount);//是否整除
            if (lastnum > 0)
            {
                maxnum = maxnum + 1;
            }
            Decbytes = new byte[maxnum * (Bytescount - 3)];
            byte[] srcBytes = new byte[Bytescount];
            for (int i = 0; i < maxnum; i++)
            {
                for (int j = 0; j < Bytescount; j++)
                {
                    srcBytes[j] = Filebytes[i * Bytescount + j];//一条记录，每条记录有多少个可以有Bytescount自定义
                }
                for (int k = 0; k < Bytescount - 3; k++)
                {
                    Decbytes[i * (Bytescount - 3) + k] = DesBytes(srcBytes)[k];//减去三位密匙解密的是一条记录
                }
            }


            return Decbytes;
        }

        /// <summary>
        /// 为每一条数据解密，每一条数据会砍掉后面三位作为密匙
        /// </summary>
        /// <param name="byt"></param>
        /// <returns></returns>
        private byte[] DesBytes(byte[] byt)
        {
            int byleng = byt.Length;//一条解密的数据长度
            byte[] Desbyt = new byte[byleng - 3];
            int ikey = 0;
            ikey = (byt[byleng - 3] ^ byt[byleng - 2] ^ byt[byleng - 1]);//最后三位异或得到一个密匙

            for (int i = 0; i < byleng - 3; i++)
            {
                Desbyt[i] = (byte)(byt[i] ^ ikey); //和前面的每一个字节异或
                if (ikey == 255)
                {
                    ikey = 0;
                }
                else
                {
                    ikey = ikey + 1;
                }
            }

            return Desbyt;
        }
        #endregion

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Filebytes">需要加密的十六进制数组</param>
        /// <param name="Bytescount">需要加密的段，多少个数据作为一段</param>
        /// <returns>已经加密的数据</returns>
        private byte[] EncodeUsb(byte[] Filebytes, int Bytescount)
        {
            byte[] Encodebytes = null;//已经解密的十六进制
            int maxnum = (Filebytes.Length / Bytescount); //除的数,查看是否整除，如果整除就是多少个35条，不整除就加一
            int lastnum = (Filebytes.Length % Bytescount);//是否整除
            if (lastnum > 0)
            {
                maxnum = maxnum + 1;
            }
            byte[] EnBytes = new byte[Bytescount];//每一段数据要求的加密的
            Encodebytes = new byte[maxnum * (Bytescount + 3)];
            for (int i = 0; i < maxnum; i++)//按bytescount来分成maxnum段来处理
            {
                for (int j = 0; j < Bytescount; j++)
                {
                    EnBytes[j] = Filebytes[i * Bytescount + j];//一条记录，每条记录有多少个可以有Bytescount自定义
                }
                byte[] Yenbytes = EncodeBytes(EnBytes);
                for (int k = 0; k < Bytescount + 3; k++)
                {
                    Encodebytes[i * (Bytescount + 3) + k] = Yenbytes[k];//减去三位密匙解密的是一条记录
                }

            }

            return Encodebytes;
        }

        /// <summary>
        /// 为每一条数据解密，每一条数据会砍掉后面三位作为密匙
        /// </summary>
        /// <param name="byt"></param>
        /// <returns></returns>
        private byte[] EncodeBytes(byte[] byt)
        {
            int byleng = byt.Length;//一条加密的数据长度
            byte[] Encodebyt = new byte[byleng + 3];
            //生产三个随机数来产生一个新的密匙
            byte[] keyi = new byte[3];
            int ikey = 0;//生产新的密匙
            for (int k = 0; k < 3; k++)
            {
                keyi[k] = GetRundByte()[k];
            }
            ikey = (keyi[0] ^ keyi[1] ^ keyi[2]);//生产一个新的密匙
            for (int i = 0; i < byleng; i++)
            {
                Encodebyt[i] = (byte)(byt[i] ^ ikey); //和前面的每一个字节异或
                if (ikey == 255)
                {
                    ikey = 0;
                }
                else
                {
                    ikey = ikey + 1;
                }
            }
            Encodebyt[byleng] = keyi[0];
            Encodebyt[byleng + 1] = keyi[1];
            Encodebyt[byleng + 2] = keyi[2];

            return Encodebyt;
        }

        #region 生成随机十六进制数
        /// <summary>
        /// 生成随机十六进制数
        /// </summary>
        /// <returns></returns>
        public static byte[] GetRundByte()
        {


            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] data = new byte[26];
            provider.GetBytes(data);

            //int runint = new System.Random().Next(1000);
            //byte runbyte = Convert.ToByte(new System.Random().Next(1000).ToString().PadLeft(2, '0').Substring(0, 2), 16);
            return data;

        }
        #endregion

        private void lbUdevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileInfo[] files;
            DirectoryInfo dirInfo;

            try
            {
                if (null == lbUdevices.SelectedItem)
                {
                    return;
                }

                dirInfo = new DirectoryInfo(lbUdevices.SelectedItem.ToString());//U盘路径
                files = dirInfo.GetFiles("*.txt");

                if (null == files || files.Length <= 0)
                {
                    return;
                }

                lbUdevcFiles.Items.Clear();
                foreach (FileInfo file in files)
                {
                    lbUdevcFiles.Items.Add(file);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnHolidaySet_Click_1(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void numTopSF_KeyUp(object sender, KeyEventArgs e)
        {
            if (numTopSF.Text.Trim() != "")
            {
                if (Convert.ToDouble(numTopSF.Text.Trim()) > 9999)
                {
                    numTopSF.Text = "65535";
                }
            }
            else
            {
                numTopSF.Text = "0";
            }
        }

        void DataGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg;

            //this.Title = null == e.OriginalSource ? "" : e.OriginalSource.GetType().ToString();

            dg = sender as DataGrid;
            if (null == dg || null == dg.CurrentItem || null == e.OriginalSource ||
                e.OriginalSource is ScrollViewer || e.OriginalSource is DataGridColumnHeader)
            {
                return;
            }

            FrameworkElement item = dg.Columns[0].GetCellContent(dg.CurrentItem);
            CheckBox cb = (item as CheckBox);
            if (null != cb)
            {
                cb.IsChecked = !(cb.IsChecked ?? false);
            }
        }

      

      
    }
}