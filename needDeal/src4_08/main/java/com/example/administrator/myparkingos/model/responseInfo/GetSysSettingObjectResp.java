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
        private String bAutoPlateEn; // 启用车牌自动识别
        private String iAutoPlateDBJD; // 车牌对比精度。0为4位；1为5位；2为6位。
        private String LocalProvince; // 本地省份的简称。如京、粤、沪。
        private String bCPHPhoto; // 是否显示车牌小图
        private String iCarPosLed; // 出入场图片不加水印
        private String iCphDelay; // 识别的车牌的有效时间(秒)
        private String iSameCphDelay; // 在指定的时间内识别到重复的车牌不处理(秒)
        private String bAutoMinutes; // 是否自动关闭收费确认窗口
        private String iAutoSetMinutes; // 弹出收费确认窗口后自动关闭的等待时间(秒)
        private String iAutoDeleteImg; // 是否删除原图
        private String iInAutoOpenModel; // 入场识别临时车牌开闸方式。0为自动开闸；1为确认开闸；2为不开闸；
        private String bAutoTemp; // 取卡(当入场识别临时车牌的开闸方式为2时有效)
        private String iOutAutoOpenModel; // 出场识别临时车牌开闸方式。0为自动开闸；1为确认开闸；2为不开闸；
        private String iInMothOpenModel; // 入场识别月租车牌开闸方式。0为自动开闸；1为确认开闸；
        private String iOutMothOpenModel; // 出场识别月租车牌开闸方式。0为自动开闸；1为确认开闸；
        private String iAutoKZ; // 临时车出场收费0元自动开闸
        private String iAuto0Set; // 0牌车自动放行
        private String iAutoColorSet; // 军警车自动放行
        private String bNoCPHAutoKZ; // 无牌车入场自动开闸
        private String bTempCanNotInSmall; // 临时车禁止进入小车场
        private String bAutoCPHDZ;    //          车牌识别打折(车场共用参数)
        private String bTempDown;    //           临时车不脱机下载0.01元）
        private String bOutConfirm;    //         IC读卡加识别参数)
        private String bOneKeyShortCut;    //     临时车不播报汉字数)
        private String bCentralCharge;    //      是否启用中央收费(车场共用参数)
        private String bOutCharge;    //          当启用了中央收费功能，是否允许在出口收费(车场共用参数)
        private String iSFLed;    //              只下载脱机车牌)
        private String bMorePaingCar;    //       是否启用多车位多车功能(车场共用参数)
        private String bMorePaingType;    //      多车位多车处理方式。0为禁止入场；1为按临时车收费。(车场共用参数)
        private String iCarPosLedJH;    //        单车道双相机识别车牌间隔时间(秒)
        private String bAutoUpdateJiHao;    //    自动修改显示屏机号(需启用计费型相机)
        private String bSMPayment;    //          启用扫描支付
        private String bPayTest;    //            在线支付测试模式（收费金额都为0.01元）
        private String bOnlinePayEnabled;    //   是否启用扫码支付功能(车场共用参数)
        private String strWXAppID;    //          微信支付AppID(车场共用参数)
        private String strWXMCHID;    //          微信商户号(车场共用参数)
        private String strWXKEY;    //            微信支付密钥(车场共用参数)
        private String strZFBAppID;    //         支付宝AppID(车场共用参数)
        private String strZFBPID;    //           支付宝PID(车场共用参数)
        private String iEnableNetVideo;    //     启用网络视频
        private String bVideo4;    //             是否启用4路视频
        private String iPersonVideo;    //        启用人像抓拍
        private String iIDCapture;    //          证件抓拍
        private String sImageSavePathBD;   //     原图保存路径
        private String sImageSavePath;     //     图像保存路径
        private String bImageAutoDel;      //     是否启用自动删除照片功能
        private String iImageAutoDelTime;  //     每天自动删除照片的时间点
        private String iImageSaveDays;     //     照片保留天数
        private String bAppEnable;         //     是否启用计费型相机。同一车场不能同时使用主板和计费型相机，启用此选项后主板无法使用。
        private String iChargeType;          //  收费标准。0为标准收费；1为深圳收费；2为非标收费；3为北京收费；4为广州收费；5为南京收费。 (车场共用参数)
        private String iXsdNum;              //  收费小数位数(iChargeType为3时有效)(车场共用参数)
        private String iXsd;                 //  收费是否有小数点(iChargeType不为3时有效)(车场共用参数)
        private String iFreeCar;             //  是否可进出免费车辆
        private String bSetTempCardPlate;    //  临时车入场可预置车牌卡类
        private String iTempFree;            //  临时车出场可选择免费
        private String iZGXE;                //  是否启用多次进出有最高收费限额功能(车场共用参数)
        private String iZGType;              //  多次进出有最高限额的卡类(车场共用参数)。0为临时车，1为储值车，2为临时车和储值车。
        private String iZGXEType;            //  多次进出有最高限额的一天的计算方式。0为按自然天计算；1为按停满24小时计算。(车场共用参数)
        private String iDiscount;            //  收费是否可以打折。(车场共用参数)
        private String bSetTempCardType;     //  临时车收费可更改卡类
        private String bModiTempType_VoiceSF;//  播报收费
        private String bSetTempMoney;        //  出场可更改收费金额
        private String bMonthRule;           //  是否启用月卡收费规则自定义(所有车场共用参数)
        private String iYKOverTimeCharge;    //  月卡过期后的处理方式。0为禁止入场；1为按临时卡收费；2为过期多少天后禁止入场。(车场共用参数)
        private String iMothOverDay;         //  月卡过期多少天后禁止入场(车场共用参数)
        private String bSFCancel;//             出场收费可取消
        private String bCarYellowTmp;//         黄牌车按临时车计费
        private String strCarYellowTmpType;//   黄牌车按哪种临时车计费。如TmpA表示按临时车A计费。
        private String sMonthOutChargeType;//   月租车出场计费类型。如MthA:TmpA|MthB:TmpB|MthC:TmpC表示月租车A按临时车A计费，月租车B按临时车B计费, 月租车C按临时车C计费。
        private String iLoadTimeInterval;//     加载时间间隔(分钟)
        private String iExitOnlineByPwd;//      凭密码退出在线监控。0不启用；1为使用登录软件的密码；2为使用打开车场设置的密码。
        private String iShowGateState;//        软件控制道闸开关
        private String bSoftOpenNoPlate;//      软件开闸不输入车牌
        private String bExceptionHandle;//      车辆异常处理/无卡放行
        private String iFullLight;//            车场满位后对到场车辆的处理方式。0为不启用；1为固定卡禁止入场；2为临时卡禁止入场；3为储值卡禁止入场；5为所有车禁入场；
        private String bFullComfirmOpen;//      满位入场确定开闸
        private String bForbidSamePosition;//   月卡限制同车位入场(车场共用参数)
        private String bCheDui;//               是否启用车队模式(道闸常开)
        private String bCheckPortFirst;//       检测卡口优先提取记录
        private String bAutoPrePlate;//         临时卡自动弹出预置窗口
        private String bShowBoxCardNum;//       显示发卡机卡箱内卡片数量
        private String bDisplayTime;//          显示时钟
        private String bIDSoftOpen; //           ID卡软件开闸
        private String iInOutLimitSeconds; //    ID卡进出间隔(秒)
        private String iRealTimeDownLoad; //     实时下载有效卡号
        private String iIDOneInOneOut; //        ID固定卡控制一进一出
        private String sID1In1OutCardType; //    实行一进一出的卡类(以/分隔)
        private String bIdReReadHandle; //       远距离ID卡重复读卡优化处理
        private String iICCardDownLoad; //       下载有效【IC月卡无卡延期】
        private String iIDComfirmOpen; //        ID按卡类确认开闸
        private String sIDComfirmOpenCardType; //实现确认开闸的ID卡类(以/分隔)
        private String bIdSfCancel; //           ID临时卡出场收费可取消
        private String bIdPlateDownLoad; //      ID卡脱机语音播报并显示车牌
        private String bCtrlShowPlate;//         控制机播报并显示车牌
        private String bCtrlShowStayTime;//      控制机播报并显示停车时间
        private String bCtrlShowInfo;//          控制机显示发布信息
        private String bCtrlShowRemainPos;//     控制机显示剩余车位
        private String iIDNoticeDay;//           固定卡有效期少于此天数则语音提示
        private String bCtrlShowCW;//            控制机显示车位号
        private String bRemainPosLedShowPlate;// 控制机播报多功能语音（需硬件支持）
        private String bRemainPosLedShowInfo;//  自动发送预约语音（需硬件支持）
        private String iCtrlVoiceMode;//         语音模式
        private String iCtrlVoiceLedVersion;//   语音版本
        private String iWorkstationNo;//         工作站编号
        private String bQueryName;//             入出场记录显示姓名编号
        private String iParkingNo;//             车场编号
        private String iPromptDelayed;//         在线监控界面上的伸缩式提示框显示后自动关闭的时间
        private String strAreaDefault;//         车牌省区(本地省份简称)
        private String iParkTotalSpaces;//       车位总个数
        private String bFreeCardNoInPlace;//     免费卡不计入车位数
        private String bSumMoneyHide;//          在线监控不显示累计金额
        private String bDetailLog;//             保存详细日志
        private String iOnlyShowThisRemainPos;// 剩余车位显示模式。0为显示总剩余车位数；1为显示临时车位剩余个数，2为显示月卡车位剩余个数，3为显示储值车位剩余个数
        private String iTempCarPlaceNum;//       临时车车位个数
        private String iMonthCarPlaceNum;//      月卡车车位个数
        private String iMoneyCarPlaceNum;//      储值车车位个数
        private String iBillPrint; //           收费票据打印[支持普通/微型/热敏/针式]
        private String bBillPrintAuto; //       自动打印收费票据
        private String iPrintFontSize; //       票据打印字体
        private String bReLoginPrint; //        换班登录可打印小票
        private String bOnlyLocation; //        自动打印交接班小票
        private String iDelayed; //             通讯延时(秒)
        private String bBarCodePrint; //        入场打印条码
        private String iVideoShiftTime; //      单视频卡轮流监控间隔时间
        private String iEnableNetVideoType;//   网络摄像头类型
        private String iEnableVideo;//          启用视频卡
        private String iVideoCardType;//        视频卡型号
        private String bSpecilCPH;//            是否启用特殊车牌处理功能
        private String bCphAllEn;//             全字母车牌不处理
        private String bCphAllSame;//           车牌字符完全一样不处理
        private String bTempTypeDefine;//        收费窗口卡类自定义  /* 下面的全部是过期的*/
        private String sTempTypeDefine;//        收费窗口自定义卡类列表(用$分隔)
        private String iVideoBrightness;//       视频亮度
        private String iVideoResolution;//       视频分辨率
        private String iCarPosCom;//             串口(外接设备)
        private String iCarPosLedLen;//          串口模式
        private String iSFLedCom;//              串口(外接设备)
        private String iSFLedType;//             串口类型
        private String IsCPHAuto;//              在线识别月卡不开闸
        private String bCtrlSetHasPwd;//         凭密码进入控制机设置

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

        public String getLocalProvince()
        {
            return LocalProvince;
        }

        public void setLocalProvince(String localProvince)
        {
            LocalProvince = localProvince;
        }

        public String getbCPHPhoto()
        {
            return bCPHPhoto;
        }

        public void setbCPHPhoto(String bCPHPhoto)
        {
            this.bCPHPhoto = bCPHPhoto;
        }

        public String getiCarPosLed()
        {
            return iCarPosLed;
        }

        public void setiCarPosLed(String iCarPosLed)
        {
            this.iCarPosLed = iCarPosLed;
        }

        public String getiCphDelay()
        {
            return iCphDelay;
        }

        public void setiCphDelay(String iCphDelay)
        {
            this.iCphDelay = iCphDelay;
        }

        public String getiSameCphDelay()
        {
            return iSameCphDelay;
        }

        public void setiSameCphDelay(String iSameCphDelay)
        {
            this.iSameCphDelay = iSameCphDelay;
        }

        public String getbAutoMinutes()
        {
            return bAutoMinutes;
        }

        public void setbAutoMinutes(String bAutoMinutes)
        {
            this.bAutoMinutes = bAutoMinutes;
        }

        public String getiAutoSetMinutes()
        {
            return iAutoSetMinutes;
        }

        public void setiAutoSetMinutes(String iAutoSetMinutes)
        {
            this.iAutoSetMinutes = iAutoSetMinutes;
        }

        public String getiAutoDeleteImg()
        {
            return iAutoDeleteImg;
        }

        public void setiAutoDeleteImg(String iAutoDeleteImg)
        {
            this.iAutoDeleteImg = iAutoDeleteImg;
        }

        public String getiInAutoOpenModel()
        {
            return iInAutoOpenModel;
        }

        public void setiInAutoOpenModel(String iInAutoOpenModel)
        {
            this.iInAutoOpenModel = iInAutoOpenModel;
        }

        public String getbAutoTemp()
        {
            return bAutoTemp;
        }

        public void setbAutoTemp(String bAutoTemp)
        {
            this.bAutoTemp = bAutoTemp;
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

        public String getiAutoKZ()
        {
            return iAutoKZ;
        }

        public void setiAutoKZ(String iAutoKZ)
        {
            this.iAutoKZ = iAutoKZ;
        }

        public String getiAuto0Set()
        {
            return iAuto0Set;
        }

        public void setiAuto0Set(String iAuto0Set)
        {
            this.iAuto0Set = iAuto0Set;
        }

        public String getiAutoColorSet()
        {
            return iAutoColorSet;
        }

        public void setiAutoColorSet(String iAutoColorSet)
        {
            this.iAutoColorSet = iAutoColorSet;
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

        public String getbAutoCPHDZ()
        {
            return bAutoCPHDZ;
        }

        public void setbAutoCPHDZ(String bAutoCPHDZ)
        {
            this.bAutoCPHDZ = bAutoCPHDZ;
        }

        public String getbTempDown()
        {
            return bTempDown;
        }

        public void setbTempDown(String bTempDown)
        {
            this.bTempDown = bTempDown;
        }

        public String getbOutConfirm()
        {
            return bOutConfirm;
        }

        public void setbOutConfirm(String bOutConfirm)
        {
            this.bOutConfirm = bOutConfirm;
        }

        public String getbOneKeyShortCut()
        {
            return bOneKeyShortCut;
        }

        public void setbOneKeyShortCut(String bOneKeyShortCut)
        {
            this.bOneKeyShortCut = bOneKeyShortCut;
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

        public String getiSFLed()
        {
            return iSFLed;
        }

        public void setiSFLed(String iSFLed)
        {
            this.iSFLed = iSFLed;
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

        public String getiCarPosLedJH()
        {
            return iCarPosLedJH;
        }

        public void setiCarPosLedJH(String iCarPosLedJH)
        {
            this.iCarPosLedJH = iCarPosLedJH;
        }

        public String getbAutoUpdateJiHao()
        {
            return bAutoUpdateJiHao;
        }

        public void setbAutoUpdateJiHao(String bAutoUpdateJiHao)
        {
            this.bAutoUpdateJiHao = bAutoUpdateJiHao;
        }

        public String getbSMPayment()
        {
            return bSMPayment;
        }

        public void setbSMPayment(String bSMPayment)
        {
            this.bSMPayment = bSMPayment;
        }

        public String getbPayTest()
        {
            return bPayTest;
        }

        public void setbPayTest(String bPayTest)
        {
            this.bPayTest = bPayTest;
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

        public String getsImageSavePathBD()
        {
            return sImageSavePathBD;
        }

        public void setsImageSavePathBD(String sImageSavePathBD)
        {
            this.sImageSavePathBD = sImageSavePathBD;
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

        public String getiImageAutoDelTime()
        {
            return iImageAutoDelTime;
        }

        public void setiImageAutoDelTime(String iImageAutoDelTime)
        {
            this.iImageAutoDelTime = iImageAutoDelTime;
        }

        public String getiImageSaveDays()
        {
            return iImageSaveDays;
        }

        public void setiImageSaveDays(String iImageSaveDays)
        {
            this.iImageSaveDays = iImageSaveDays;
        }

        public String getbAppEnable()
        {
            return bAppEnable;
        }

        public void setbAppEnable(String bAppEnable)
        {
            this.bAppEnable = bAppEnable;
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

        public String getiFreeCar()
        {
            return iFreeCar;
        }

        public void setiFreeCar(String iFreeCar)
        {
            this.iFreeCar = iFreeCar;
        }

        public String getbSetTempCardPlate()
        {
            return bSetTempCardPlate;
        }

        public void setbSetTempCardPlate(String bSetTempCardPlate)
        {
            this.bSetTempCardPlate = bSetTempCardPlate;
        }

        public String getiTempFree()
        {
            return iTempFree;
        }

        public void setiTempFree(String iTempFree)
        {
            this.iTempFree = iTempFree;
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

        public String getbSetTempCardType()
        {
            return bSetTempCardType;
        }

        public void setbSetTempCardType(String bSetTempCardType)
        {
            this.bSetTempCardType = bSetTempCardType;
        }

        public String getbModiTempType_VoiceSF()
        {
            return bModiTempType_VoiceSF;
        }

        public void setbModiTempType_VoiceSF(String bModiTempType_VoiceSF)
        {
            this.bModiTempType_VoiceSF = bModiTempType_VoiceSF;
        }

        public String getbSetTempMoney()
        {
            return bSetTempMoney;
        }

        public void setbSetTempMoney(String bSetTempMoney)
        {
            this.bSetTempMoney = bSetTempMoney;
        }

        public String getbMonthRule()
        {
            return bMonthRule;
        }

        public void setbMonthRule(String bMonthRule)
        {
            this.bMonthRule = bMonthRule;
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

        public String getbSFCancel()
        {
            return bSFCancel;
        }

        public void setbSFCancel(String bSFCancel)
        {
            this.bSFCancel = bSFCancel;
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

        public String getiLoadTimeInterval()
        {
            return iLoadTimeInterval;
        }

        public void setiLoadTimeInterval(String iLoadTimeInterval)
        {
            this.iLoadTimeInterval = iLoadTimeInterval;
        }

        public String getiExitOnlineByPwd()
        {
            return iExitOnlineByPwd;
        }

        public void setiExitOnlineByPwd(String iExitOnlineByPwd)
        {
            this.iExitOnlineByPwd = iExitOnlineByPwd;
        }

        public String getiShowGateState()
        {
            return iShowGateState;
        }

        public void setiShowGateState(String iShowGateState)
        {
            this.iShowGateState = iShowGateState;
        }

        public String getbSoftOpenNoPlate()
        {
            return bSoftOpenNoPlate;
        }

        public void setbSoftOpenNoPlate(String bSoftOpenNoPlate)
        {
            this.bSoftOpenNoPlate = bSoftOpenNoPlate;
        }

        public String getbExceptionHandle()
        {
            return bExceptionHandle;
        }

        public void setbExceptionHandle(String bExceptionHandle)
        {
            this.bExceptionHandle = bExceptionHandle;
        }

        public String getiFullLight()
        {
            return iFullLight;
        }

        public void setiFullLight(String iFullLight)
        {
            this.iFullLight = iFullLight;
        }

        public String getbFullComfirmOpen()
        {
            return bFullComfirmOpen;
        }

        public void setbFullComfirmOpen(String bFullComfirmOpen)
        {
            this.bFullComfirmOpen = bFullComfirmOpen;
        }

        public String getbForbidSamePosition()
        {
            return bForbidSamePosition;
        }

        public void setbForbidSamePosition(String bForbidSamePosition)
        {
            this.bForbidSamePosition = bForbidSamePosition;
        }

        public String getbCheDui()
        {
            return bCheDui;
        }

        public void setbCheDui(String bCheDui)
        {
            this.bCheDui = bCheDui;
        }

        public String getbCheckPortFirst()
        {
            return bCheckPortFirst;
        }

        public void setbCheckPortFirst(String bCheckPortFirst)
        {
            this.bCheckPortFirst = bCheckPortFirst;
        }

        public String getbAutoPrePlate()
        {
            return bAutoPrePlate;
        }

        public void setbAutoPrePlate(String bAutoPrePlate)
        {
            this.bAutoPrePlate = bAutoPrePlate;
        }

        public String getbShowBoxCardNum()
        {
            return bShowBoxCardNum;
        }

        public void setbShowBoxCardNum(String bShowBoxCardNum)
        {
            this.bShowBoxCardNum = bShowBoxCardNum;
        }

        public String getbDisplayTime()
        {
            return bDisplayTime;
        }

        public void setbDisplayTime(String bDisplayTime)
        {
            this.bDisplayTime = bDisplayTime;
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

        public String getiIDOneInOneOut()
        {
            return iIDOneInOneOut;
        }

        public void setiIDOneInOneOut(String iIDOneInOneOut)
        {
            this.iIDOneInOneOut = iIDOneInOneOut;
        }

        public String getsID1In1OutCardType()
        {
            return sID1In1OutCardType;
        }

        public void setsID1In1OutCardType(String sID1In1OutCardType)
        {
            this.sID1In1OutCardType = sID1In1OutCardType;
        }

        public String getbIdReReadHandle()
        {
            return bIdReReadHandle;
        }

        public void setbIdReReadHandle(String bIdReReadHandle)
        {
            this.bIdReReadHandle = bIdReReadHandle;
        }

        public String getiICCardDownLoad()
        {
            return iICCardDownLoad;
        }

        public void setiICCardDownLoad(String iICCardDownLoad)
        {
            this.iICCardDownLoad = iICCardDownLoad;
        }

        public String getiIDComfirmOpen()
        {
            return iIDComfirmOpen;
        }

        public void setiIDComfirmOpen(String iIDComfirmOpen)
        {
            this.iIDComfirmOpen = iIDComfirmOpen;
        }

        public String getsIDComfirmOpenCardType()
        {
            return sIDComfirmOpenCardType;
        }

        public void setsIDComfirmOpenCardType(String sIDComfirmOpenCardType)
        {
            this.sIDComfirmOpenCardType = sIDComfirmOpenCardType;
        }

        public String getbIdSfCancel()
        {
            return bIdSfCancel;
        }

        public void setbIdSfCancel(String bIdSfCancel)
        {
            this.bIdSfCancel = bIdSfCancel;
        }

        public String getbIdPlateDownLoad()
        {
            return bIdPlateDownLoad;
        }

        public void setbIdPlateDownLoad(String bIdPlateDownLoad)
        {
            this.bIdPlateDownLoad = bIdPlateDownLoad;
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

        public String getiIDNoticeDay()
        {
            return iIDNoticeDay;
        }

        public void setiIDNoticeDay(String iIDNoticeDay)
        {
            this.iIDNoticeDay = iIDNoticeDay;
        }

        public String getbCtrlShowCW()
        {
            return bCtrlShowCW;
        }

        public void setbCtrlShowCW(String bCtrlShowCW)
        {
            this.bCtrlShowCW = bCtrlShowCW;
        }

        public String getbRemainPosLedShowPlate()
        {
            return bRemainPosLedShowPlate;
        }

        public void setbRemainPosLedShowPlate(String bRemainPosLedShowPlate)
        {
            this.bRemainPosLedShowPlate = bRemainPosLedShowPlate;
        }

        public String getbRemainPosLedShowInfo()
        {
            return bRemainPosLedShowInfo;
        }

        public void setbRemainPosLedShowInfo(String bRemainPosLedShowInfo)
        {
            this.bRemainPosLedShowInfo = bRemainPosLedShowInfo;
        }

        public String getiCtrlVoiceMode()
        {
            return iCtrlVoiceMode;
        }

        public void setiCtrlVoiceMode(String iCtrlVoiceMode)
        {
            this.iCtrlVoiceMode = iCtrlVoiceMode;
        }

        public String getiCtrlVoiceLedVersion()
        {
            return iCtrlVoiceLedVersion;
        }

        public void setiCtrlVoiceLedVersion(String iCtrlVoiceLedVersion)
        {
            this.iCtrlVoiceLedVersion = iCtrlVoiceLedVersion;
        }

        public String getiWorkstationNo()
        {
            return iWorkstationNo;
        }

        public void setiWorkstationNo(String iWorkstationNo)
        {
            this.iWorkstationNo = iWorkstationNo;
        }

        public String getbQueryName()
        {
            return bQueryName;
        }

        public void setbQueryName(String bQueryName)
        {
            this.bQueryName = bQueryName;
        }

        public String getiParkingNo()
        {
            return iParkingNo;
        }

        public void setiParkingNo(String iParkingNo)
        {
            this.iParkingNo = iParkingNo;
        }

        public String getiPromptDelayed()
        {
            return iPromptDelayed;
        }

        public void setiPromptDelayed(String iPromptDelayed)
        {
            this.iPromptDelayed = iPromptDelayed;
        }

        public String getStrAreaDefault()
        {
            return strAreaDefault;
        }

        public void setStrAreaDefault(String strAreaDefault)
        {
            this.strAreaDefault = strAreaDefault;
        }

        public String getiParkTotalSpaces()
        {
            return iParkTotalSpaces;
        }

        public void setiParkTotalSpaces(String iParkTotalSpaces)
        {
            this.iParkTotalSpaces = iParkTotalSpaces;
        }

        public String getbFreeCardNoInPlace()
        {
            return bFreeCardNoInPlace;
        }

        public void setbFreeCardNoInPlace(String bFreeCardNoInPlace)
        {
            this.bFreeCardNoInPlace = bFreeCardNoInPlace;
        }

        public String getbSumMoneyHide()
        {
            return bSumMoneyHide;
        }

        public void setbSumMoneyHide(String bSumMoneyHide)
        {
            this.bSumMoneyHide = bSumMoneyHide;
        }

        public String getbDetailLog()
        {
            return bDetailLog;
        }

        public void setbDetailLog(String bDetailLog)
        {
            this.bDetailLog = bDetailLog;
        }

        public String getiOnlyShowThisRemainPos()
        {
            return iOnlyShowThisRemainPos;
        }

        public void setiOnlyShowThisRemainPos(String iOnlyShowThisRemainPos)
        {
            this.iOnlyShowThisRemainPos = iOnlyShowThisRemainPos;
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

        public String getbReLoginPrint()
        {
            return bReLoginPrint;
        }

        public void setbReLoginPrint(String bReLoginPrint)
        {
            this.bReLoginPrint = bReLoginPrint;
        }

        public String getbOnlyLocation()
        {
            return bOnlyLocation;
        }

        public void setbOnlyLocation(String bOnlyLocation)
        {
            this.bOnlyLocation = bOnlyLocation;
        }

        public String getiDelayed()
        {
            return iDelayed;
        }

        public void setiDelayed(String iDelayed)
        {
            this.iDelayed = iDelayed;
        }

        public String getbBarCodePrint()
        {
            return bBarCodePrint;
        }

        public void setbBarCodePrint(String bBarCodePrint)
        {
            this.bBarCodePrint = bBarCodePrint;
        }

        public String getiVideoShiftTime()
        {
            return iVideoShiftTime;
        }

        public void setiVideoShiftTime(String iVideoShiftTime)
        {
            this.iVideoShiftTime = iVideoShiftTime;
        }

        public String getiEnableNetVideoType()
        {
            return iEnableNetVideoType;
        }

        public void setiEnableNetVideoType(String iEnableNetVideoType)
        {
            this.iEnableNetVideoType = iEnableNetVideoType;
        }

        public String getiEnableVideo()
        {
            return iEnableVideo;
        }

        public void setiEnableVideo(String iEnableVideo)
        {
            this.iEnableVideo = iEnableVideo;
        }

        public String getiVideoCardType()
        {
            return iVideoCardType;
        }

        public void setiVideoCardType(String iVideoCardType)
        {
            this.iVideoCardType = iVideoCardType;
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

        public String getbTempTypeDefine()
        {
            return bTempTypeDefine;
        }

        public void setbTempTypeDefine(String bTempTypeDefine)
        {
            this.bTempTypeDefine = bTempTypeDefine;
        }

        public String getsTempTypeDefine()
        {
            return sTempTypeDefine;
        }

        public void setsTempTypeDefine(String sTempTypeDefine)
        {
            this.sTempTypeDefine = sTempTypeDefine;
        }

        public String getiVideoBrightness()
        {
            return iVideoBrightness;
        }

        public void setiVideoBrightness(String iVideoBrightness)
        {
            this.iVideoBrightness = iVideoBrightness;
        }

        public String getiVideoResolution()
        {
            return iVideoResolution;
        }

        public void setiVideoResolution(String iVideoResolution)
        {
            this.iVideoResolution = iVideoResolution;
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

        public String getIsCPHAuto()
        {
            return IsCPHAuto;
        }

        public void setIsCPHAuto(String isCPHAuto)
        {
            IsCPHAuto = isCPHAuto;
        }

        public String getbCtrlSetHasPwd()
        {
            return bCtrlSetHasPwd;
        }

        public void setbCtrlSetHasPwd(String bCtrlSetHasPwd)
        {
            this.bCtrlSetHasPwd = bCtrlSetHasPwd;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            if (bAutoPlateEn != null) sb.append("bAutoPlateEn='").append(bAutoPlateEn).append('\'');
            if (iAutoPlateDBJD != null)
                sb.append(", iAutoPlateDBJD='").append(iAutoPlateDBJD).append('\'');
            if (LocalProvince != null)
                sb.append(", LocalProvince='").append(LocalProvince).append('\'');
            if (bCPHPhoto != null) sb.append(", bCPHPhoto='").append(bCPHPhoto).append('\'');
            if (iCarPosLed != null) sb.append(", iCarPosLed='").append(iCarPosLed).append('\'');
            if (iCphDelay != null) sb.append(", iCphDelay='").append(iCphDelay).append('\'');
            if (iSameCphDelay != null)
                sb.append(", iSameCphDelay='").append(iSameCphDelay).append('\'');
            if (bAutoMinutes != null)
                sb.append(", bAutoMinutes='").append(bAutoMinutes).append('\'');
            if (iAutoSetMinutes != null)
                sb.append(", iAutoSetMinutes='").append(iAutoSetMinutes).append('\'');
            if (iAutoDeleteImg != null)
                sb.append(", iAutoDeleteImg='").append(iAutoDeleteImg).append('\'');
            if (iInAutoOpenModel != null)
                sb.append(", iInAutoOpenModel='").append(iInAutoOpenModel).append('\'');
            if (bAutoTemp != null) sb.append(", bAutoTemp='").append(bAutoTemp).append('\'');
            if (iOutAutoOpenModel != null)
                sb.append(", iOutAutoOpenModel='").append(iOutAutoOpenModel).append('\'');
            if (iInMothOpenModel != null)
                sb.append(", iInMothOpenModel='").append(iInMothOpenModel).append('\'');
            if (iOutMothOpenModel != null)
                sb.append(", iOutMothOpenModel='").append(iOutMothOpenModel).append('\'');
            if (iAutoKZ != null) sb.append(", iAutoKZ='").append(iAutoKZ).append('\'');
            if (iAuto0Set != null) sb.append(", iAuto0Set='").append(iAuto0Set).append('\'');
            if (iAutoColorSet != null)
                sb.append(", iAutoColorSet='").append(iAutoColorSet).append('\'');
            if (bNoCPHAutoKZ != null)
                sb.append(", bNoCPHAutoKZ='").append(bNoCPHAutoKZ).append('\'');
            if (bTempCanNotInSmall != null)
                sb.append(", bTempCanNotInSmall='").append(bTempCanNotInSmall).append('\'');
            if (bAutoCPHDZ != null) sb.append(", bAutoCPHDZ='").append(bAutoCPHDZ).append('\'');
            if (bTempDown != null) sb.append(", bTempDown='").append(bTempDown).append('\'');
            if (bOutConfirm != null) sb.append(", bOutConfirm='").append(bOutConfirm).append('\'');
            if (bOneKeyShortCut != null)
                sb.append(", bOneKeyShortCut='").append(bOneKeyShortCut).append('\'');
            if (bCentralCharge != null)
                sb.append(", bCentralCharge='").append(bCentralCharge).append('\'');
            if (bOutCharge != null) sb.append(", bOutCharge='").append(bOutCharge).append('\'');
            if (iSFLed != null) sb.append(", iSFLed='").append(iSFLed).append('\'');
            if (bMorePaingCar != null)
                sb.append(", bMorePaingCar='").append(bMorePaingCar).append('\'');
            if (bMorePaingType != null)
                sb.append(", bMorePaingType='").append(bMorePaingType).append('\'');
            if (iCarPosLedJH != null)
                sb.append(", iCarPosLedJH='").append(iCarPosLedJH).append('\'');
            if (bAutoUpdateJiHao != null)
                sb.append(", bAutoUpdateJiHao='").append(bAutoUpdateJiHao).append('\'');
            if (bSMPayment != null) sb.append(", bSMPayment='").append(bSMPayment).append('\'');
            if (bPayTest != null) sb.append(", bPayTest='").append(bPayTest).append('\'');
            if (bOnlinePayEnabled != null)
                sb.append(", bOnlinePayEnabled='").append(bOnlinePayEnabled).append('\'');
            if (strWXAppID != null) sb.append(", strWXAppID='").append(strWXAppID).append('\'');
            if (strWXMCHID != null) sb.append(", strWXMCHID='").append(strWXMCHID).append('\'');
            if (strWXKEY != null) sb.append(", strWXKEY='").append(strWXKEY).append('\'');
            if (strZFBAppID != null) sb.append(", strZFBAppID='").append(strZFBAppID).append('\'');
            if (strZFBPID != null) sb.append(", strZFBPID='").append(strZFBPID).append('\'');
            if (iEnableNetVideo != null)
                sb.append(", iEnableNetVideo='").append(iEnableNetVideo).append('\'');
            if (bVideo4 != null) sb.append(", bVideo4='").append(bVideo4).append('\'');
            if (iPersonVideo != null)
                sb.append(", iPersonVideo='").append(iPersonVideo).append('\'');
            if (iIDCapture != null) sb.append(", iIDCapture='").append(iIDCapture).append('\'');
            if (sImageSavePathBD != null)
                sb.append(", sImageSavePathBD='").append(sImageSavePathBD).append('\'');
            if (sImageSavePath != null)
                sb.append(", sImageSavePath='").append(sImageSavePath).append('\'');
            if (bImageAutoDel != null)
                sb.append(", bImageAutoDel='").append(bImageAutoDel).append('\'');
            if (iImageAutoDelTime != null)
                sb.append(", iImageAutoDelTime='").append(iImageAutoDelTime).append('\'');
            if (iImageSaveDays != null)
                sb.append(", iImageSaveDays='").append(iImageSaveDays).append('\'');
            if (bAppEnable != null) sb.append(", bAppEnable='").append(bAppEnable).append('\'');
            if (iChargeType != null) sb.append(", iChargeType='").append(iChargeType).append('\'');
            if (iXsdNum != null) sb.append(", iXsdNum='").append(iXsdNum).append('\'');
            if (iXsd != null) sb.append(", iXsd='").append(iXsd).append('\'');
            if (iFreeCar != null) sb.append(", iFreeCar='").append(iFreeCar).append('\'');
            if (bSetTempCardPlate != null)
                sb.append(", bSetTempCardPlate='").append(bSetTempCardPlate).append('\'');
            if (iTempFree != null) sb.append(", iTempFree='").append(iTempFree).append('\'');
            if (iZGXE != null) sb.append(", iZGXE='").append(iZGXE).append('\'');
            if (iZGType != null) sb.append(", iZGType='").append(iZGType).append('\'');
            if (iZGXEType != null) sb.append(", iZGXEType='").append(iZGXEType).append('\'');
            if (iDiscount != null) sb.append(", iDiscount='").append(iDiscount).append('\'');
            if (bSetTempCardType != null)
                sb.append(", bSetTempCardType='").append(bSetTempCardType).append('\'');
            if (bModiTempType_VoiceSF != null)
                sb.append(", bModiTempType_VoiceSF='").append(bModiTempType_VoiceSF).append('\'');
            if (bSetTempMoney != null)
                sb.append(", bSetTempMoney='").append(bSetTempMoney).append('\'');
            if (bMonthRule != null) sb.append(", bMonthRule='").append(bMonthRule).append('\'');
            if (iYKOverTimeCharge != null)
                sb.append(", iYKOverTimeCharge='").append(iYKOverTimeCharge).append('\'');
            if (iMothOverDay != null)
                sb.append(", iMothOverDay='").append(iMothOverDay).append('\'');
            if (bSFCancel != null) sb.append(", bSFCancel='").append(bSFCancel).append('\'');
            if (bCarYellowTmp != null)
                sb.append(", bCarYellowTmp='").append(bCarYellowTmp).append('\'');
            if (strCarYellowTmpType != null)
                sb.append(", strCarYellowTmpType='").append(strCarYellowTmpType).append('\'');
            if (sMonthOutChargeType != null)
                sb.append(", sMonthOutChargeType='").append(sMonthOutChargeType).append('\'');
            if (iLoadTimeInterval != null)
                sb.append(", iLoadTimeInterval='").append(iLoadTimeInterval).append('\'');
            if (iExitOnlineByPwd != null)
                sb.append(", iExitOnlineByPwd='").append(iExitOnlineByPwd).append('\'');
            if (iShowGateState != null)
                sb.append(", iShowGateState='").append(iShowGateState).append('\'');
            if (bSoftOpenNoPlate != null)
                sb.append(", bSoftOpenNoPlate='").append(bSoftOpenNoPlate).append('\'');
            if (bExceptionHandle != null)
                sb.append(", bExceptionHandle='").append(bExceptionHandle).append('\'');
            if (iFullLight != null) sb.append(", iFullLight='").append(iFullLight).append('\'');
            if (bFullComfirmOpen != null)
                sb.append(", bFullComfirmOpen='").append(bFullComfirmOpen).append('\'');
            if (bForbidSamePosition != null)
                sb.append(", bForbidSamePosition='").append(bForbidSamePosition).append('\'');
            if (bCheDui != null) sb.append(", bCheDui='").append(bCheDui).append('\'');
            if (bCheckPortFirst != null)
                sb.append(", bCheckPortFirst='").append(bCheckPortFirst).append('\'');
            if (bAutoPrePlate != null)
                sb.append(", bAutoPrePlate='").append(bAutoPrePlate).append('\'');
            if (bShowBoxCardNum != null)
                sb.append(", bShowBoxCardNum='").append(bShowBoxCardNum).append('\'');
            if (bDisplayTime != null)
                sb.append(", bDisplayTime='").append(bDisplayTime).append('\'');
            if (bIDSoftOpen != null) sb.append(", bIDSoftOpen='").append(bIDSoftOpen).append('\'');
            if (iInOutLimitSeconds != null)
                sb.append(", iInOutLimitSeconds='").append(iInOutLimitSeconds).append('\'');
            if (iRealTimeDownLoad != null)
                sb.append(", iRealTimeDownLoad='").append(iRealTimeDownLoad).append('\'');
            if (iIDOneInOneOut != null)
                sb.append(", iIDOneInOneOut='").append(iIDOneInOneOut).append('\'');
            if (sID1In1OutCardType != null)
                sb.append(", sID1In1OutCardType='").append(sID1In1OutCardType).append('\'');
            if (bIdReReadHandle != null)
                sb.append(", bIdReReadHandle='").append(bIdReReadHandle).append('\'');
            if (iICCardDownLoad != null)
                sb.append(", iICCardDownLoad='").append(iICCardDownLoad).append('\'');
            if (iIDComfirmOpen != null)
                sb.append(", iIDComfirmOpen='").append(iIDComfirmOpen).append('\'');
            if (sIDComfirmOpenCardType != null)
                sb.append(", sIDComfirmOpenCardType='").append(sIDComfirmOpenCardType).append('\'');
            if (bIdSfCancel != null) sb.append(", bIdSfCancel='").append(bIdSfCancel).append('\'');
            if (bIdPlateDownLoad != null)
                sb.append(", bIdPlateDownLoad='").append(bIdPlateDownLoad).append('\'');
            if (bCtrlShowPlate != null)
                sb.append(", bCtrlShowPlate='").append(bCtrlShowPlate).append('\'');
            if (bCtrlShowStayTime != null)
                sb.append(", bCtrlShowStayTime='").append(bCtrlShowStayTime).append('\'');
            if (bCtrlShowInfo != null)
                sb.append(", bCtrlShowInfo='").append(bCtrlShowInfo).append('\'');
            if (bCtrlShowRemainPos != null)
                sb.append(", bCtrlShowRemainPos='").append(bCtrlShowRemainPos).append('\'');
            if (iIDNoticeDay != null)
                sb.append(", iIDNoticeDay='").append(iIDNoticeDay).append('\'');
            if (bCtrlShowCW != null) sb.append(", bCtrlShowCW='").append(bCtrlShowCW).append('\'');
            if (bRemainPosLedShowPlate != null)
                sb.append(", bRemainPosLedShowPlate='").append(bRemainPosLedShowPlate).append('\'');
            if (bRemainPosLedShowInfo != null)
                sb.append(", bRemainPosLedShowInfo='").append(bRemainPosLedShowInfo).append('\'');
            if (iCtrlVoiceMode != null)
                sb.append(", iCtrlVoiceMode='").append(iCtrlVoiceMode).append('\'');
            if (iCtrlVoiceLedVersion != null)
                sb.append(", iCtrlVoiceLedVersion='").append(iCtrlVoiceLedVersion).append('\'');
            if (iWorkstationNo != null)
                sb.append(", iWorkstationNo='").append(iWorkstationNo).append('\'');
            if (bQueryName != null) sb.append(", bQueryName='").append(bQueryName).append('\'');
            if (iParkingNo != null) sb.append(", iParkingNo='").append(iParkingNo).append('\'');
            if (iPromptDelayed != null)
                sb.append(", iPromptDelayed='").append(iPromptDelayed).append('\'');
            if (strAreaDefault != null)
                sb.append(", strAreaDefault='").append(strAreaDefault).append('\'');
            if (iParkTotalSpaces != null)
                sb.append(", iParkTotalSpaces='").append(iParkTotalSpaces).append('\'');
            if (bFreeCardNoInPlace != null)
                sb.append(", bFreeCardNoInPlace='").append(bFreeCardNoInPlace).append('\'');
            if (bSumMoneyHide != null)
                sb.append(", bSumMoneyHide='").append(bSumMoneyHide).append('\'');
            if (bDetailLog != null) sb.append(", bDetailLog='").append(bDetailLog).append('\'');
            if (iOnlyShowThisRemainPos != null)
                sb.append(", iOnlyShowThisRemainPos='").append(iOnlyShowThisRemainPos).append('\'');
            if (iTempCarPlaceNum != null)
                sb.append(", iTempCarPlaceNum='").append(iTempCarPlaceNum).append('\'');
            if (iMonthCarPlaceNum != null)
                sb.append(", iMonthCarPlaceNum='").append(iMonthCarPlaceNum).append('\'');
            if (iMoneyCarPlaceNum != null)
                sb.append(", iMoneyCarPlaceNum='").append(iMoneyCarPlaceNum).append('\'');
            if (iBillPrint != null) sb.append(", iBillPrint='").append(iBillPrint).append('\'');
            if (bBillPrintAuto != null)
                sb.append(", bBillPrintAuto='").append(bBillPrintAuto).append('\'');
            if (iPrintFontSize != null)
                sb.append(", iPrintFontSize='").append(iPrintFontSize).append('\'');
            if (bReLoginPrint != null)
                sb.append(", bReLoginPrint='").append(bReLoginPrint).append('\'');
            if (bOnlyLocation != null)
                sb.append(", bOnlyLocation='").append(bOnlyLocation).append('\'');
            if (iDelayed != null) sb.append(", iDelayed='").append(iDelayed).append('\'');
            if (bBarCodePrint != null)
                sb.append(", bBarCodePrint='").append(bBarCodePrint).append('\'');
            if (iVideoShiftTime != null)
                sb.append(", iVideoShiftTime='").append(iVideoShiftTime).append('\'');
            if (iEnableNetVideoType != null)
                sb.append(", iEnableNetVideoType='").append(iEnableNetVideoType).append('\'');
            if (iEnableVideo != null)
                sb.append(", iEnableVideo='").append(iEnableVideo).append('\'');
            if (iVideoCardType != null)
                sb.append(", iVideoCardType='").append(iVideoCardType).append('\'');
            if (bSpecilCPH != null) sb.append(", bSpecilCPH='").append(bSpecilCPH).append('\'');
            if (bCphAllEn != null) sb.append(", bCphAllEn='").append(bCphAllEn).append('\'');
            if (bCphAllSame != null) sb.append(", bCphAllSame='").append(bCphAllSame).append('\'');
            if (bTempTypeDefine != null)
                sb.append(", bTempTypeDefine='").append(bTempTypeDefine).append('\'');
            if (sTempTypeDefine != null)
                sb.append(", sTempTypeDefine='").append(sTempTypeDefine).append('\'');
            if (iVideoBrightness != null)
                sb.append(", iVideoBrightness='").append(iVideoBrightness).append('\'');
            if (iVideoResolution != null)
                sb.append(", iVideoResolution='").append(iVideoResolution).append('\'');
            if (iCarPosCom != null) sb.append(", iCarPosCom='").append(iCarPosCom).append('\'');
            if (iCarPosLedLen != null)
                sb.append(", iCarPosLedLen='").append(iCarPosLedLen).append('\'');
            if (iSFLedCom != null) sb.append(", iSFLedCom='").append(iSFLedCom).append('\'');
            if (iSFLedType != null) sb.append(", iSFLedType='").append(iSFLedType).append('\'');
            if (IsCPHAuto != null) sb.append(", IsCPHAuto='").append(IsCPHAuto).append('\'');
            if (bCtrlSetHasPwd != null)
                sb.append(", bCtrlSetHasPwd='").append(bCtrlSetHasPwd).append('\'');
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
