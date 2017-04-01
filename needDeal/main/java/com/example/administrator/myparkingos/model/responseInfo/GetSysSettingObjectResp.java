package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetSysSettingObjectResp
{
    private String rcode;    // Y 参考错误码列表
    private String msg;    // Y 错误信息
    private DataBean data;    // N 由指定工作站下面的所有参数的键值对组成的对象。

    public String getRcode()
    {
        return rcode;
    }

    public void setRcode(String rcode)
    {
        this.rcode = rcode;
    }

    public String getMsg()
    {
        return msg;
    }

    public void setMsg(String msg)
    {
        this.msg = msg;
    }

    public DataBean getData()
    {
        return data;
    }

    public void setData(DataBean data)
    {
        this.data = data;
    }

    public static class DataBean
    {
        private String bAutoCPHDZ;
        private String bCentralCharge;
        private String bOutCharge;
        private String bMorePaingCar;
        private String bMorePaingType;
        private String bOnlinePayEnabled;
        private String strWXAppID;
        private String strWXMCHID;
        private String strWXKEY;
        private String strZFBAppID;
        private String strZFBPID;
        private String iChargeType;
        private String iXsdNum;
        private String iXsd;
        private String iZGXE;
        private String iZGType;
        private String iZGXEType;
        private String iDiscount;
        private String iYKOverTimeCharge;
        private String iMothOverDay;
        private String bForbidSamePosition;
        private String bMonthRule;
        private String iEnableNetVideo;
        private String bVideo4;
        private String iPersonVideo;
        private String iIDCapture;
        private String sImageSavePath;
        private String bImageAutoDel;
        private String iImageSaveDays;
        private String iImageAutoDelTime;
        private String iFreeCar;
        private String bSetTempMoney;
        private String bModiTempType_VoiceSF;
        private String bSFCancel;
        private String bSetTempCardType;
        private String iLoadTimeInterval;
        private String bDisplayTime;
        private String iShowGateState;
        private String iExitOnlineByPwd;
        private String bSoftOpenNoPlate;
        private String bCheDui;
        private String bExceptionHandle;
        private String bShowBoxCardNum;
        private String bAutoPrePlate;
        private String bCheckPortFirst;
        private String iFullLight;
        private String iVideoShiftTime;
        private String bIDSoftOpen;
        private String iInOutLimitSeconds;
        private String iRealTimeDownLoad;
        private String bIdSfCancel;
        private String iICCardDownLoad;
        private String bIdReReadHandle;
        private String bIdPlateDownLoad;
        private String iIDOneInOneOut;
        private String iIDComfirmOpen;
        private String bCtrlShowPlate;
        private String bCtrlShowStayTime;
        private String bCtrlShowCW;
        private String bCtrlShowInfo;
        private String bCtrlShowRemainPos;
        private String iCtrlVoiceLedVersion;
        private String iCtrlVoiceMode;
        private String iIDNoticeDay;
        private String iBillPrint;
        private String bBillPrintAuto;
        private String iPrintFontSize;
        private String iCarPosCom;
        private String iCarPosLedLen;
        private String iSFLedCom;
        private String iSFLedType;
        private String bRemainPosLedShowInfo;
        private String bRemainPosLedShowPlate;
        private String bReLoginPrint;
        private String bBarCodePrint;
        private String bCtrlSetHasPwd;
        private String bQueryName;
        private String iWorkstationNo;
        private String iParkingNo;
        private String strAreaDefault;
        private String bFreeCardNoInPlace;
        private String bDetailLog;
        private String bSumMoneyHide;
        private String iParkTotalSpaces;
        private String iTempCarPlaceNum;
        private String iMonthCarPlaceNum;
        private String iMoneyCarPlaceNum;
        private String iOnlyShowThisRemainPos;
        private String bOneKeyShortCut;
        private String bTempDown;
        private String bAutoMinutes;
        private String LocalProvince;
        private String bAutoUpdateJiHao;
        private String iSFLed;
        private String iAutoSetMinutes;
        private String bAutoPlateEn;
        private String iAutoPlateDBJD;
        private String iInAutoOpenModel;
        private String iOutAutoOpenModel;
        private String iInMothOpenModel;
        private String iOutMothOpenModel;
        private String bCPHPhoto;
        private String iAutoDeleteImg;
        private String iSameCphDelay;
        private String iCarPosLed;
        private String iAutoKZ;
        private String iAutoColorSet;
        private String iAuto0Set;
        private String bNoCPHAutoKZ;
        private String bTempCanNotInSmall;
        private String bOutSF;
        private String iCarPosLedJH;
        private String iCphDelay;
        private String iTempFree;
        private String sID1In1OutCardType;
        private String iDelayed;
        private String iPromptDelayed;
        private String OCar;
        private String bSpecilCPH;
        private String bCphAllEn;
        private String bCphAllSame;
        private String bCarYellowTmp;
        private String strCarYellowTmpType;
        private String sMonthOutChargeType;
        private String bOnlyLocation;
        private String bFullComfirmOpen;
        private String bAppEnable;

        public String getbAutoCPHDZ()
        {
            return bAutoCPHDZ;
        }

        public void setbAutoCPHDZ(String bAutoCPHDZ)
        {
            this.bAutoCPHDZ = bAutoCPHDZ;
        }

        public String getbCentralCharge()
        {
            return bCentralCharge;
        }

        public void setbCentralCharge(String bCentralCharge)
        {
            this.bCentralCharge = bCentralCharge;
        }

        public String getbOutCharge()
        {
            return bOutCharge;
        }

        public void setbOutCharge(String bOutCharge)
        {
            this.bOutCharge = bOutCharge;
        }

        public String getbMorePaingCar()
        {
            return bMorePaingCar;
        }

        public void setbMorePaingCar(String bMorePaingCar)
        {
            this.bMorePaingCar = bMorePaingCar;
        }

        public String getbMorePaingType()
        {
            return bMorePaingType;
        }

        public void setbMorePaingType(String bMorePaingType)
        {
            this.bMorePaingType = bMorePaingType;
        }

        public String getbOnlinePayEnabled()
        {
            return bOnlinePayEnabled;
        }

        public void setbOnlinePayEnabled(String bOnlinePayEnabled)
        {
            this.bOnlinePayEnabled = bOnlinePayEnabled;
        }

        public String getStrWXAppID()
        {
            return strWXAppID;
        }

        public void setStrWXAppID(String strWXAppID)
        {
            this.strWXAppID = strWXAppID;
        }

        public String getStrWXMCHID()
        {
            return strWXMCHID;
        }

        public void setStrWXMCHID(String strWXMCHID)
        {
            this.strWXMCHID = strWXMCHID;
        }

        public String getStrWXKEY()
        {
            return strWXKEY;
        }

        public void setStrWXKEY(String strWXKEY)
        {
            this.strWXKEY = strWXKEY;
        }

        public String getStrZFBAppID()
        {
            return strZFBAppID;
        }

        public void setStrZFBAppID(String strZFBAppID)
        {
            this.strZFBAppID = strZFBAppID;
        }

        public String getStrZFBPID()
        {
            return strZFBPID;
        }

        public void setStrZFBPID(String strZFBPID)
        {
            this.strZFBPID = strZFBPID;
        }

        public String getiChargeType()
        {
            return iChargeType;
        }

        public void setiChargeType(String iChargeType)
        {
            this.iChargeType = iChargeType;
        }

        public String getiXsdNum()
        {
            return iXsdNum;
        }

        public void setiXsdNum(String iXsdNum)
        {
            this.iXsdNum = iXsdNum;
        }

        public String getiXsd()
        {
            return iXsd;
        }

        public void setiXsd(String iXsd)
        {
            this.iXsd = iXsd;
        }

        public String getiZGXE()
        {
            return iZGXE;
        }

        public void setiZGXE(String iZGXE)
        {
            this.iZGXE = iZGXE;
        }

        public String getiZGType()
        {
            return iZGType;
        }

        public void setiZGType(String iZGType)
        {
            this.iZGType = iZGType;
        }

        public String getiZGXEType()
        {
            return iZGXEType;
        }

        public void setiZGXEType(String iZGXEType)
        {
            this.iZGXEType = iZGXEType;
        }

        public String getiDiscount()
        {
            return iDiscount;
        }

        public void setiDiscount(String iDiscount)
        {
            this.iDiscount = iDiscount;
        }

        public String getiYKOverTimeCharge()
        {
            return iYKOverTimeCharge;
        }

        public void setiYKOverTimeCharge(String iYKOverTimeCharge)
        {
            this.iYKOverTimeCharge = iYKOverTimeCharge;
        }

        public String getiMothOverDay()
        {
            return iMothOverDay;
        }

        public void setiMothOverDay(String iMothOverDay)
        {
            this.iMothOverDay = iMothOverDay;
        }

        public String getbForbidSamePosition()
        {
            return bForbidSamePosition;
        }

        public void setbForbidSamePosition(String bForbidSamePosition)
        {
            this.bForbidSamePosition = bForbidSamePosition;
        }

        public String getbMonthRule()
        {
            return bMonthRule;
        }

        public void setbMonthRule(String bMonthRule)
        {
            this.bMonthRule = bMonthRule;
        }

        public String getiEnableNetVideo()
        {
            return iEnableNetVideo;
        }

        public void setiEnableNetVideo(String iEnableNetVideo)
        {
            this.iEnableNetVideo = iEnableNetVideo;
        }

        public String getbVideo4()
        {
            return bVideo4;
        }

        public void setbVideo4(String bVideo4)
        {
            this.bVideo4 = bVideo4;
        }

        public String getiPersonVideo()
        {
            return iPersonVideo;
        }

        public void setiPersonVideo(String iPersonVideo)
        {
            this.iPersonVideo = iPersonVideo;
        }

        public String getiIDCapture()
        {
            return iIDCapture;
        }

        public void setiIDCapture(String iIDCapture)
        {
            this.iIDCapture = iIDCapture;
        }

        public String getsImageSavePath()
        {
            return sImageSavePath;
        }

        public void setsImageSavePath(String sImageSavePath)
        {
            this.sImageSavePath = sImageSavePath;
        }

        public String getbImageAutoDel()
        {
            return bImageAutoDel;
        }

        public void setbImageAutoDel(String bImageAutoDel)
        {
            this.bImageAutoDel = bImageAutoDel;
        }

        public String getiImageSaveDays()
        {
            return iImageSaveDays;
        }

        public void setiImageSaveDays(String iImageSaveDays)
        {
            this.iImageSaveDays = iImageSaveDays;
        }

        public String getiImageAutoDelTime()
        {
            return iImageAutoDelTime;
        }

        public void setiImageAutoDelTime(String iImageAutoDelTime)
        {
            this.iImageAutoDelTime = iImageAutoDelTime;
        }

        public String getiFreeCar()
        {
            return iFreeCar;
        }

        public void setiFreeCar(String iFreeCar)
        {
            this.iFreeCar = iFreeCar;
        }

        public String getbSetTempMoney()
        {
            return bSetTempMoney;
        }

        public void setbSetTempMoney(String bSetTempMoney)
        {
            this.bSetTempMoney = bSetTempMoney;
        }

        public String getbModiTempType_VoiceSF()
        {
            return bModiTempType_VoiceSF;
        }

        public void setbModiTempType_VoiceSF(String bModiTempType_VoiceSF)
        {
            this.bModiTempType_VoiceSF = bModiTempType_VoiceSF;
        }

        public String getbSFCancel()
        {
            return bSFCancel;
        }

        public void setbSFCancel(String bSFCancel)
        {
            this.bSFCancel = bSFCancel;
        }

        public String getbSetTempCardType()
        {
            return bSetTempCardType;
        }

        public void setbSetTempCardType(String bSetTempCardType)
        {
            this.bSetTempCardType = bSetTempCardType;
        }

        public String getiLoadTimeInterval()
        {
            return iLoadTimeInterval;
        }

        public void setiLoadTimeInterval(String iLoadTimeInterval)
        {
            this.iLoadTimeInterval = iLoadTimeInterval;
        }

        public String getbDisplayTime()
        {
            return bDisplayTime;
        }

        public void setbDisplayTime(String bDisplayTime)
        {
            this.bDisplayTime = bDisplayTime;
        }

        public String getiShowGateState()
        {
            return iShowGateState;
        }

        public void setiShowGateState(String iShowGateState)
        {
            this.iShowGateState = iShowGateState;
        }

        public String getiExitOnlineByPwd()
        {
            return iExitOnlineByPwd;
        }

        public void setiExitOnlineByPwd(String iExitOnlineByPwd)
        {
            this.iExitOnlineByPwd = iExitOnlineByPwd;
        }

        public String getbSoftOpenNoPlate()
        {
            return bSoftOpenNoPlate;
        }

        public void setbSoftOpenNoPlate(String bSoftOpenNoPlate)
        {
            this.bSoftOpenNoPlate = bSoftOpenNoPlate;
        }

        public String getbCheDui()
        {
            return bCheDui;
        }

        public void setbCheDui(String bCheDui)
        {
            this.bCheDui = bCheDui;
        }

        public String getbExceptionHandle()
        {
            return bExceptionHandle;
        }

        public void setbExceptionHandle(String bExceptionHandle)
        {
            this.bExceptionHandle = bExceptionHandle;
        }

        public String getbShowBoxCardNum()
        {
            return bShowBoxCardNum;
        }

        public void setbShowBoxCardNum(String bShowBoxCardNum)
        {
            this.bShowBoxCardNum = bShowBoxCardNum;
        }

        public String getbAutoPrePlate()
        {
            return bAutoPrePlate;
        }

        public void setbAutoPrePlate(String bAutoPrePlate)
        {
            this.bAutoPrePlate = bAutoPrePlate;
        }

        public String getbCheckPortFirst()
        {
            return bCheckPortFirst;
        }

        public void setbCheckPortFirst(String bCheckPortFirst)
        {
            this.bCheckPortFirst = bCheckPortFirst;
        }

        public String getiFullLight()
        {
            return iFullLight;
        }

        public void setiFullLight(String iFullLight)
        {
            this.iFullLight = iFullLight;
        }

        public String getiVideoShiftTime()
        {
            return iVideoShiftTime;
        }

        public void setiVideoShiftTime(String iVideoShiftTime)
        {
            this.iVideoShiftTime = iVideoShiftTime;
        }

        public String getbIDSoftOpen()
        {
            return bIDSoftOpen;
        }

        public void setbIDSoftOpen(String bIDSoftOpen)
        {
            this.bIDSoftOpen = bIDSoftOpen;
        }

        public String getiInOutLimitSeconds()
        {
            return iInOutLimitSeconds;
        }

        public void setiInOutLimitSeconds(String iInOutLimitSeconds)
        {
            this.iInOutLimitSeconds = iInOutLimitSeconds;
        }

        public String getiRealTimeDownLoad()
        {
            return iRealTimeDownLoad;
        }

        public void setiRealTimeDownLoad(String iRealTimeDownLoad)
        {
            this.iRealTimeDownLoad = iRealTimeDownLoad;
        }

        public String getbIdSfCancel()
        {
            return bIdSfCancel;
        }

        public void setbIdSfCancel(String bIdSfCancel)
        {
            this.bIdSfCancel = bIdSfCancel;
        }

        public String getiICCardDownLoad()
        {
            return iICCardDownLoad;
        }

        public void setiICCardDownLoad(String iICCardDownLoad)
        {
            this.iICCardDownLoad = iICCardDownLoad;
        }

        public String getbIdReReadHandle()
        {
            return bIdReReadHandle;
        }

        public void setbIdReReadHandle(String bIdReReadHandle)
        {
            this.bIdReReadHandle = bIdReReadHandle;
        }

        public String getbIdPlateDownLoad()
        {
            return bIdPlateDownLoad;
        }

        public void setbIdPlateDownLoad(String bIdPlateDownLoad)
        {
            this.bIdPlateDownLoad = bIdPlateDownLoad;
        }

        public String getiIDOneInOneOut()
        {
            return iIDOneInOneOut;
        }

        public void setiIDOneInOneOut(String iIDOneInOneOut)
        {
            this.iIDOneInOneOut = iIDOneInOneOut;
        }

        public String getiIDComfirmOpen()
        {
            return iIDComfirmOpen;
        }

        public void setiIDComfirmOpen(String iIDComfirmOpen)
        {
            this.iIDComfirmOpen = iIDComfirmOpen;
        }

        public String getbCtrlShowPlate()
        {
            return bCtrlShowPlate;
        }

        public void setbCtrlShowPlate(String bCtrlShowPlate)
        {
            this.bCtrlShowPlate = bCtrlShowPlate;
        }

        public String getbCtrlShowStayTime()
        {
            return bCtrlShowStayTime;
        }

        public void setbCtrlShowStayTime(String bCtrlShowStayTime)
        {
            this.bCtrlShowStayTime = bCtrlShowStayTime;
        }

        public String getbCtrlShowCW()
        {
            return bCtrlShowCW;
        }

        public void setbCtrlShowCW(String bCtrlShowCW)
        {
            this.bCtrlShowCW = bCtrlShowCW;
        }

        public String getbCtrlShowInfo()
        {
            return bCtrlShowInfo;
        }

        public void setbCtrlShowInfo(String bCtrlShowInfo)
        {
            this.bCtrlShowInfo = bCtrlShowInfo;
        }

        public String getbCtrlShowRemainPos()
        {
            return bCtrlShowRemainPos;
        }

        public void setbCtrlShowRemainPos(String bCtrlShowRemainPos)
        {
            this.bCtrlShowRemainPos = bCtrlShowRemainPos;
        }

        public String getiCtrlVoiceLedVersion()
        {
            return iCtrlVoiceLedVersion;
        }

        public void setiCtrlVoiceLedVersion(String iCtrlVoiceLedVersion)
        {
            this.iCtrlVoiceLedVersion = iCtrlVoiceLedVersion;
        }

        public String getiCtrlVoiceMode()
        {
            return iCtrlVoiceMode;
        }

        public void setiCtrlVoiceMode(String iCtrlVoiceMode)
        {
            this.iCtrlVoiceMode = iCtrlVoiceMode;
        }

        public String getiIDNoticeDay()
        {
            return iIDNoticeDay;
        }

        public void setiIDNoticeDay(String iIDNoticeDay)
        {
            this.iIDNoticeDay = iIDNoticeDay;
        }

        public String getiBillPrint()
        {
            return iBillPrint;
        }

        public void setiBillPrint(String iBillPrint)
        {
            this.iBillPrint = iBillPrint;
        }

        public String getbBillPrintAuto()
        {
            return bBillPrintAuto;
        }

        public void setbBillPrintAuto(String bBillPrintAuto)
        {
            this.bBillPrintAuto = bBillPrintAuto;
        }

        public String getiPrintFontSize()
        {
            return iPrintFontSize;
        }

        public void setiPrintFontSize(String iPrintFontSize)
        {
            this.iPrintFontSize = iPrintFontSize;
        }

        public String getiCarPosCom()
        {
            return iCarPosCom;
        }

        public void setiCarPosCom(String iCarPosCom)
        {
            this.iCarPosCom = iCarPosCom;
        }

        public String getiCarPosLedLen()
        {
            return iCarPosLedLen;
        }

        public void setiCarPosLedLen(String iCarPosLedLen)
        {
            this.iCarPosLedLen = iCarPosLedLen;
        }

        public String getiSFLedCom()
        {
            return iSFLedCom;
        }

        public void setiSFLedCom(String iSFLedCom)
        {
            this.iSFLedCom = iSFLedCom;
        }

        public String getiSFLedType()
        {
            return iSFLedType;
        }

        public void setiSFLedType(String iSFLedType)
        {
            this.iSFLedType = iSFLedType;
        }

        public String getbRemainPosLedShowInfo()
        {
            return bRemainPosLedShowInfo;
        }

        public void setbRemainPosLedShowInfo(String bRemainPosLedShowInfo)
        {
            this.bRemainPosLedShowInfo = bRemainPosLedShowInfo;
        }

        public String getbRemainPosLedShowPlate()
        {
            return bRemainPosLedShowPlate;
        }

        public void setbRemainPosLedShowPlate(String bRemainPosLedShowPlate)
        {
            this.bRemainPosLedShowPlate = bRemainPosLedShowPlate;
        }

        public String getbReLoginPrint()
        {
            return bReLoginPrint;
        }

        public void setbReLoginPrint(String bReLoginPrint)
        {
            this.bReLoginPrint = bReLoginPrint;
        }

        public String getbBarCodePrint()
        {
            return bBarCodePrint;
        }

        public void setbBarCodePrint(String bBarCodePrint)
        {
            this.bBarCodePrint = bBarCodePrint;
        }

        public String getbCtrlSetHasPwd()
        {
            return bCtrlSetHasPwd;
        }

        public void setbCtrlSetHasPwd(String bCtrlSetHasPwd)
        {
            this.bCtrlSetHasPwd = bCtrlSetHasPwd;
        }

        public String getbQueryName()
        {
            return bQueryName;
        }

        public void setbQueryName(String bQueryName)
        {
            this.bQueryName = bQueryName;
        }

        public String getiWorkstationNo()
        {
            return iWorkstationNo;
        }

        public void setiWorkstationNo(String iWorkstationNo)
        {
            this.iWorkstationNo = iWorkstationNo;
        }

        public String getiParkingNo()
        {
            return iParkingNo;
        }

        public void setiParkingNo(String iParkingNo)
        {
            this.iParkingNo = iParkingNo;
        }

        public String getStrAreaDefault()
        {
            return strAreaDefault;
        }

        public void setStrAreaDefault(String strAreaDefault)
        {
            this.strAreaDefault = strAreaDefault;
        }

        public String getbFreeCardNoInPlace()
        {
            return bFreeCardNoInPlace;
        }

        public void setbFreeCardNoInPlace(String bFreeCardNoInPlace)
        {
            this.bFreeCardNoInPlace = bFreeCardNoInPlace;
        }

        public String getbDetailLog()
        {
            return bDetailLog;
        }

        public void setbDetailLog(String bDetailLog)
        {
            this.bDetailLog = bDetailLog;
        }

        public String getbSumMoneyHide()
        {
            return bSumMoneyHide;
        }

        public void setbSumMoneyHide(String bSumMoneyHide)
        {
            this.bSumMoneyHide = bSumMoneyHide;
        }

        public String getiParkTotalSpaces()
        {
            return iParkTotalSpaces;
        }

        public void setiParkTotalSpaces(String iParkTotalSpaces)
        {
            this.iParkTotalSpaces = iParkTotalSpaces;
        }

        public String getiTempCarPlaceNum()
        {
            return iTempCarPlaceNum;
        }

        public void setiTempCarPlaceNum(String iTempCarPlaceNum)
        {
            this.iTempCarPlaceNum = iTempCarPlaceNum;
        }

        public String getiMonthCarPlaceNum()
        {
            return iMonthCarPlaceNum;
        }

        public void setiMonthCarPlaceNum(String iMonthCarPlaceNum)
        {
            this.iMonthCarPlaceNum = iMonthCarPlaceNum;
        }

        public String getiMoneyCarPlaceNum()
        {
            return iMoneyCarPlaceNum;
        }

        public void setiMoneyCarPlaceNum(String iMoneyCarPlaceNum)
        {
            this.iMoneyCarPlaceNum = iMoneyCarPlaceNum;
        }

        public String getiOnlyShowThisRemainPos()
        {
            return iOnlyShowThisRemainPos;
        }

        public void setiOnlyShowThisRemainPos(String iOnlyShowThisRemainPos)
        {
            this.iOnlyShowThisRemainPos = iOnlyShowThisRemainPos;
        }

        public String getbOneKeyShortCut()
        {
            return bOneKeyShortCut;
        }

        public void setbOneKeyShortCut(String bOneKeyShortCut)
        {
            this.bOneKeyShortCut = bOneKeyShortCut;
        }

        public String getbTempDown()
        {
            return bTempDown;
        }

        public void setbTempDown(String bTempDown)
        {
            this.bTempDown = bTempDown;
        }

        public String getbAutoMinutes()
        {
            return bAutoMinutes;
        }

        public void setbAutoMinutes(String bAutoMinutes)
        {
            this.bAutoMinutes = bAutoMinutes;
        }

        public String getLocalProvince()
        {
            return LocalProvince;
        }

        public void setLocalProvince(String localProvince)
        {
            LocalProvince = localProvince;
        }

        public String getbAutoUpdateJiHao()
        {
            return bAutoUpdateJiHao;
        }

        public void setbAutoUpdateJiHao(String bAutoUpdateJiHao)
        {
            this.bAutoUpdateJiHao = bAutoUpdateJiHao;
        }

        public String getiSFLed()
        {
            return iSFLed;
        }

        public void setiSFLed(String iSFLed)
        {
            this.iSFLed = iSFLed;
        }

        public String getiAutoSetMinutes()
        {
            return iAutoSetMinutes;
        }

        public void setiAutoSetMinutes(String iAutoSetMinutes)
        {
            this.iAutoSetMinutes = iAutoSetMinutes;
        }

        public String getbAutoPlateEn()
        {
            return bAutoPlateEn;
        }

        public void setbAutoPlateEn(String bAutoPlateEn)
        {
            this.bAutoPlateEn = bAutoPlateEn;
        }

        public String getiAutoPlateDBJD()
        {
            return iAutoPlateDBJD;
        }

        public void setiAutoPlateDBJD(String iAutoPlateDBJD)
        {
            this.iAutoPlateDBJD = iAutoPlateDBJD;
        }

        public String getiInAutoOpenModel()
        {
            return iInAutoOpenModel;
        }

        public void setiInAutoOpenModel(String iInAutoOpenModel)
        {
            this.iInAutoOpenModel = iInAutoOpenModel;
        }

        public String getiOutAutoOpenModel()
        {
            return iOutAutoOpenModel;
        }

        public void setiOutAutoOpenModel(String iOutAutoOpenModel)
        {
            this.iOutAutoOpenModel = iOutAutoOpenModel;
        }

        public String getiInMothOpenModel()
        {
            return iInMothOpenModel;
        }

        public void setiInMothOpenModel(String iInMothOpenModel)
        {
            this.iInMothOpenModel = iInMothOpenModel;
        }

        public String getiOutMothOpenModel()
        {
            return iOutMothOpenModel;
        }

        public void setiOutMothOpenModel(String iOutMothOpenModel)
        {
            this.iOutMothOpenModel = iOutMothOpenModel;
        }

        public String getbCPHPhoto()
        {
            return bCPHPhoto;
        }

        public void setbCPHPhoto(String bCPHPhoto)
        {
            this.bCPHPhoto = bCPHPhoto;
        }

        public String getiAutoDeleteImg()
        {
            return iAutoDeleteImg;
        }

        public void setiAutoDeleteImg(String iAutoDeleteImg)
        {
            this.iAutoDeleteImg = iAutoDeleteImg;
        }

        public String getiSameCphDelay()
        {
            return iSameCphDelay;
        }

        public void setiSameCphDelay(String iSameCphDelay)
        {
            this.iSameCphDelay = iSameCphDelay;
        }

        public String getiCarPosLed()
        {
            return iCarPosLed;
        }

        public void setiCarPosLed(String iCarPosLed)
        {
            this.iCarPosLed = iCarPosLed;
        }

        public String getiAutoKZ()
        {
            return iAutoKZ;
        }

        public void setiAutoKZ(String iAutoKZ)
        {
            this.iAutoKZ = iAutoKZ;
        }

        public String getiAutoColorSet()
        {
            return iAutoColorSet;
        }

        public void setiAutoColorSet(String iAutoColorSet)
        {
            this.iAutoColorSet = iAutoColorSet;
        }

        public String getiAuto0Set()
        {
            return iAuto0Set;
        }

        public void setiAuto0Set(String iAuto0Set)
        {
            this.iAuto0Set = iAuto0Set;
        }

        public String getbNoCPHAutoKZ()
        {
            return bNoCPHAutoKZ;
        }

        public void setbNoCPHAutoKZ(String bNoCPHAutoKZ)
        {
            this.bNoCPHAutoKZ = bNoCPHAutoKZ;
        }

        public String getbTempCanNotInSmall()
        {
            return bTempCanNotInSmall;
        }

        public void setbTempCanNotInSmall(String bTempCanNotInSmall)
        {
            this.bTempCanNotInSmall = bTempCanNotInSmall;
        }

        public String getbOutSF()
        {
            return bOutSF;
        }

        public void setbOutSF(String bOutSF)
        {
            this.bOutSF = bOutSF;
        }

        public String getiCarPosLedJH()
        {
            return iCarPosLedJH;
        }

        public void setiCarPosLedJH(String iCarPosLedJH)
        {
            this.iCarPosLedJH = iCarPosLedJH;
        }

        public String getiCphDelay()
        {
            return iCphDelay;
        }

        public void setiCphDelay(String iCphDelay)
        {
            this.iCphDelay = iCphDelay;
        }

        public String getiTempFree()
        {
            return iTempFree;
        }

        public void setiTempFree(String iTempFree)
        {
            this.iTempFree = iTempFree;
        }

        public String getsID1In1OutCardType()
        {
            return sID1In1OutCardType;
        }

        public void setsID1In1OutCardType(String sID1In1OutCardType)
        {
            this.sID1In1OutCardType = sID1In1OutCardType;
        }

        public String getiDelayed()
        {
            return iDelayed;
        }

        public void setiDelayed(String iDelayed)
        {
            this.iDelayed = iDelayed;
        }

        public String getiPromptDelayed()
        {
            return iPromptDelayed;
        }

        public void setiPromptDelayed(String iPromptDelayed)
        {
            this.iPromptDelayed = iPromptDelayed;
        }

        public String getOCar()
        {
            return OCar;
        }

        public void setOCar(String OCar)
        {
            this.OCar = OCar;
        }

        public String getbSpecilCPH()
        {
            return bSpecilCPH;
        }

        public void setbSpecilCPH(String bSpecilCPH)
        {
            this.bSpecilCPH = bSpecilCPH;
        }

        public String getbCphAllEn()
        {
            return bCphAllEn;
        }

        public void setbCphAllEn(String bCphAllEn)
        {
            this.bCphAllEn = bCphAllEn;
        }

        public String getbCphAllSame()
        {
            return bCphAllSame;
        }

        public void setbCphAllSame(String bCphAllSame)
        {
            this.bCphAllSame = bCphAllSame;
        }

        public String getbCarYellowTmp()
        {
            return bCarYellowTmp;
        }

        public void setbCarYellowTmp(String bCarYellowTmp)
        {
            this.bCarYellowTmp = bCarYellowTmp;
        }

        public String getStrCarYellowTmpType()
        {
            return strCarYellowTmpType;
        }

        public void setStrCarYellowTmpType(String strCarYellowTmpType)
        {
            this.strCarYellowTmpType = strCarYellowTmpType;
        }

        public String getsMonthOutChargeType()
        {
            return sMonthOutChargeType;
        }

        public void setsMonthOutChargeType(String sMonthOutChargeType)
        {
            this.sMonthOutChargeType = sMonthOutChargeType;
        }

        public String getbOnlyLocation()
        {
            return bOnlyLocation;
        }

        public void setbOnlyLocation(String bOnlyLocation)
        {
            this.bOnlyLocation = bOnlyLocation;
        }

        public String getbFullComfirmOpen()
        {
            return bFullComfirmOpen;
        }

        public void setbFullComfirmOpen(String bFullComfirmOpen)
        {
            this.bFullComfirmOpen = bFullComfirmOpen;
        }

        public String getbAppEnable()
        {
            return bAppEnable;
        }

        public void setbAppEnable(String bAppEnable)
        {
            this.bAppEnable = bAppEnable;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("bAutoCPHDZ='").append(bAutoCPHDZ).append('\'');
            sb.append(", bCentralCharge='").append(bCentralCharge).append('\'');
            sb.append(", bOutCharge='").append(bOutCharge).append('\'');
            sb.append(", bMorePaingCar='").append(bMorePaingCar).append('\'');
            sb.append(", bMorePaingType='").append(bMorePaingType).append('\'');
            sb.append(", bOnlinePayEnabled='").append(bOnlinePayEnabled).append('\'');
            sb.append(", strWXAppID='").append(strWXAppID).append('\'');
            sb.append(", strWXMCHID='").append(strWXMCHID).append('\'');
            sb.append(", strWXKEY='").append(strWXKEY).append('\'');
            sb.append(", strZFBAppID='").append(strZFBAppID).append('\'');
            sb.append(", strZFBPID='").append(strZFBPID).append('\'');
            sb.append(", iChargeType='").append(iChargeType).append('\'');
            sb.append(", iXsdNum='").append(iXsdNum).append('\'');
            sb.append(", iXsd='").append(iXsd).append('\'');
            sb.append(", iZGXE='").append(iZGXE).append('\'');
            sb.append(", iZGType='").append(iZGType).append('\'');
            sb.append(", iZGXEType='").append(iZGXEType).append('\'');
            sb.append(", iDiscount='").append(iDiscount).append('\'');
            sb.append(", iYKOverTimeCharge='").append(iYKOverTimeCharge).append('\'');
            sb.append(", iMothOverDay='").append(iMothOverDay).append('\'');
            sb.append(", bForbidSamePosition='").append(bForbidSamePosition).append('\'');
            sb.append(", bMonthRule='").append(bMonthRule).append('\'');
            sb.append(", iEnableNetVideo='").append(iEnableNetVideo).append('\'');
            sb.append(", bVideo4='").append(bVideo4).append('\'');
            sb.append(", iPersonVideo='").append(iPersonVideo).append('\'');
            sb.append(", iIDCapture='").append(iIDCapture).append('\'');
            sb.append(", sImageSavePath='").append(sImageSavePath).append('\'');
            sb.append(", bImageAutoDel='").append(bImageAutoDel).append('\'');
            sb.append(", iImageSaveDays='").append(iImageSaveDays).append('\'');
            sb.append(", iImageAutoDelTime='").append(iImageAutoDelTime).append('\'');
            sb.append(", iFreeCar='").append(iFreeCar).append('\'');
            sb.append(", bSetTempMoney='").append(bSetTempMoney).append('\'');
            sb.append(", bModiTempType_VoiceSF='").append(bModiTempType_VoiceSF).append('\'');
            sb.append(", bSFCancel='").append(bSFCancel).append('\'');
            sb.append(", bSetTempCardType='").append(bSetTempCardType).append('\'');
            sb.append(", iLoadTimeInterval='").append(iLoadTimeInterval).append('\'');
            sb.append(", bDisplayTime='").append(bDisplayTime).append('\'');
            sb.append(", iShowGateState='").append(iShowGateState).append('\'');
            sb.append(", iExitOnlineByPwd='").append(iExitOnlineByPwd).append('\'');
            sb.append(", bSoftOpenNoPlate='").append(bSoftOpenNoPlate).append('\'');
            sb.append(", bCheDui='").append(bCheDui).append('\'');
            sb.append(", bExceptionHandle='").append(bExceptionHandle).append('\'');
            sb.append(", bShowBoxCardNum='").append(bShowBoxCardNum).append('\'');
            sb.append(", bAutoPrePlate='").append(bAutoPrePlate).append('\'');
            sb.append(", bCheckPortFirst='").append(bCheckPortFirst).append('\'');
            sb.append(", iFullLight='").append(iFullLight).append('\'');
            sb.append(", iVideoShiftTime='").append(iVideoShiftTime).append('\'');
            sb.append(", bIDSoftOpen='").append(bIDSoftOpen).append('\'');
            sb.append(", iInOutLimitSeconds='").append(iInOutLimitSeconds).append('\'');
            sb.append(", iRealTimeDownLoad='").append(iRealTimeDownLoad).append('\'');
            sb.append(", bIdSfCancel='").append(bIdSfCancel).append('\'');
            sb.append(", iICCardDownLoad='").append(iICCardDownLoad).append('\'');
            sb.append(", bIdReReadHandle='").append(bIdReReadHandle).append('\'');
            sb.append(", bIdPlateDownLoad='").append(bIdPlateDownLoad).append('\'');
            sb.append(", iIDOneInOneOut='").append(iIDOneInOneOut).append('\'');
            sb.append(", iIDComfirmOpen='").append(iIDComfirmOpen).append('\'');
            sb.append(", bCtrlShowPlate='").append(bCtrlShowPlate).append('\'');
            sb.append(", bCtrlShowStayTime='").append(bCtrlShowStayTime).append('\'');
            sb.append(", bCtrlShowCW='").append(bCtrlShowCW).append('\'');
            sb.append(", bCtrlShowInfo='").append(bCtrlShowInfo).append('\'');
            sb.append(", bCtrlShowRemainPos='").append(bCtrlShowRemainPos).append('\'');
            sb.append(", iCtrlVoiceLedVersion='").append(iCtrlVoiceLedVersion).append('\'');
            sb.append(", iCtrlVoiceMode='").append(iCtrlVoiceMode).append('\'');
            sb.append(", iIDNoticeDay='").append(iIDNoticeDay).append('\'');
            sb.append(", iBillPrint='").append(iBillPrint).append('\'');
            sb.append(", bBillPrintAuto='").append(bBillPrintAuto).append('\'');
            sb.append(", iPrintFontSize='").append(iPrintFontSize).append('\'');
            sb.append(", iCarPosCom='").append(iCarPosCom).append('\'');
            sb.append(", iCarPosLedLen='").append(iCarPosLedLen).append('\'');
            sb.append(", iSFLedCom='").append(iSFLedCom).append('\'');
            sb.append(", iSFLedType='").append(iSFLedType).append('\'');
            sb.append(", bRemainPosLedShowInfo='").append(bRemainPosLedShowInfo).append('\'');
            sb.append(", bRemainPosLedShowPlate='").append(bRemainPosLedShowPlate).append('\'');
            sb.append(", bReLoginPrint='").append(bReLoginPrint).append('\'');
            sb.append(", bBarCodePrint='").append(bBarCodePrint).append('\'');
            sb.append(", bCtrlSetHasPwd='").append(bCtrlSetHasPwd).append('\'');
            sb.append(", bQueryName='").append(bQueryName).append('\'');
            sb.append(", iWorkstationNo='").append(iWorkstationNo).append('\'');
            sb.append(", iParkingNo='").append(iParkingNo).append('\'');
            sb.append(", strAreaDefault='").append(strAreaDefault).append('\'');
            sb.append(", bFreeCardNoInPlace='").append(bFreeCardNoInPlace).append('\'');
            sb.append(", bDetailLog='").append(bDetailLog).append('\'');
            sb.append(", bSumMoneyHide='").append(bSumMoneyHide).append('\'');
            sb.append(", iParkTotalSpaces='").append(iParkTotalSpaces).append('\'');
            sb.append(", iTempCarPlaceNum='").append(iTempCarPlaceNum).append('\'');
            sb.append(", iMonthCarPlaceNum='").append(iMonthCarPlaceNum).append('\'');
            sb.append(", iMoneyCarPlaceNum='").append(iMoneyCarPlaceNum).append('\'');
            sb.append(", iOnlyShowThisRemainPos='").append(iOnlyShowThisRemainPos).append('\'');
            sb.append(", bOneKeyShortCut='").append(bOneKeyShortCut).append('\'');
            sb.append(", bTempDown='").append(bTempDown).append('\'');
            sb.append(", bAutoMinutes='").append(bAutoMinutes).append('\'');
            sb.append(", LocalProvince='").append(LocalProvince).append('\'');
            sb.append(", bAutoUpdateJiHao='").append(bAutoUpdateJiHao).append('\'');
            sb.append(", iSFLed='").append(iSFLed).append('\'');
            sb.append(", iAutoSetMinutes='").append(iAutoSetMinutes).append('\'');
            sb.append(", bAutoPlateEn='").append(bAutoPlateEn).append('\'');
            sb.append(", iAutoPlateDBJD='").append(iAutoPlateDBJD).append('\'');
            sb.append(", iInAutoOpenModel='").append(iInAutoOpenModel).append('\'');
            sb.append(", iOutAutoOpenModel='").append(iOutAutoOpenModel).append('\'');
            sb.append(", iInMothOpenModel='").append(iInMothOpenModel).append('\'');
            sb.append(", iOutMothOpenModel='").append(iOutMothOpenModel).append('\'');
            sb.append(", bCPHPhoto='").append(bCPHPhoto).append('\'');
            sb.append(", iAutoDeleteImg='").append(iAutoDeleteImg).append('\'');
            sb.append(", iSameCphDelay='").append(iSameCphDelay).append('\'');
            sb.append(", iCarPosLed='").append(iCarPosLed).append('\'');
            sb.append(", iAutoKZ='").append(iAutoKZ).append('\'');
            sb.append(", iAutoColorSet='").append(iAutoColorSet).append('\'');
            sb.append(", iAuto0Set='").append(iAuto0Set).append('\'');
            sb.append(", bNoCPHAutoKZ='").append(bNoCPHAutoKZ).append('\'');
            sb.append(", bTempCanNotInSmall='").append(bTempCanNotInSmall).append('\'');
            sb.append(", bOutSF='").append(bOutSF).append('\'');
            sb.append(", iCarPosLedJH='").append(iCarPosLedJH).append('\'');
            sb.append(", iCphDelay='").append(iCphDelay).append('\'');
            sb.append(", iTempFree='").append(iTempFree).append('\'');
            sb.append(", sID1In1OutCardType='").append(sID1In1OutCardType).append('\'');
            sb.append(", iDelayed='").append(iDelayed).append('\'');
            sb.append(", iPromptDelayed='").append(iPromptDelayed).append('\'');
            sb.append(", OCar='").append(OCar).append('\'');
            sb.append(", bSpecilCPH='").append(bSpecilCPH).append('\'');
            sb.append(", bCphAllEn='").append(bCphAllEn).append('\'');
            sb.append(", bCphAllSame='").append(bCphAllSame).append('\'');
            sb.append(", bCarYellowTmp='").append(bCarYellowTmp).append('\'');
            sb.append(", strCarYellowTmpType='").append(strCarYellowTmpType).append('\'');
            sb.append(", sMonthOutChargeType='").append(sMonthOutChargeType).append('\'');
            sb.append(", bOnlyLocation='").append(bOnlyLocation).append('\'');
            sb.append(", bFullComfirmOpen='").append(bFullComfirmOpen).append('\'');
            sb.append(", bAppEnable='").append(bAppEnable).append('\'');
            sb.append('}');
            return sb.toString();
        }
    }

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetSysSettingObjectResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }
}
