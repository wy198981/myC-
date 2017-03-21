using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingModel;
using ParkingInterface;
using System.Data;

namespace UI
{
    public class GetServiceData
    {
        Request request = new Request();

        public bool GetToken()
        {
            return (Model.token = request.GetToken("666666", "")) == "" ? false : true;
        }


        #region Operators
        public List<Operators> GetOperators(string userNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["UserNO"] = userNo;
            //dic["Pwd"] = pwd;
            string where = JsonJoin.ToJson(dic);
            List<Operators> lstOt = request.FindData<List<Operators>>(Model.token, "tOperators", where);
            return lstOt;
        }

        public List<Operators> GetUser(string cardNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["UserNO"] = cardNo;
            string where = JsonJoin.ToJson(dic, true);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["ID"] = "desc";
            List<Operators> lstOT = request.FindData<List<Operators>>(Model.token, "tOperators", dic0, where);
            return lstOT;
        }

        public int AddOperator(CardIssue model)
        {
            if (null == model) return 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = model.CardNO;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["UserNO"] = model.UserNO;
            dic0["UserName"] = model.UserName;
            dic0["DeptName"] = model.DeptName;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tOperators", updstr, where);
            if (ret > 0)
            {

            }
            else
            {
                Operators ot = new Operators();
                ot.CardNO = model.CardNO;
                ot.CardType = model.CarCardType;
                ot.UserNO = model.UserNO;
                ot.UserName = model.UserName;
                ot.DeptName = model.DeptName;
                ot.Pwd = "";
                ot.CardState = 0;
                ot.UserLevel = 2;
                string addstr = JsonJoin.ModelToJson(ot);
                ret = request.AddData(Model.token, "tOperators", addstr);
            }
            return ret;
        }
        #endregion


        #region Log
        /// <summary>
        /// 添加一条日志记录
        /// </summary>
        /// <param name="OptMenu"></param>
        /// <param name="OptContent"></param>
        public void AddLog(string OptMenu, string OptContent)
        {
            OptLog opt = new OptLog();
            opt.OptNO = Model.sUserCard;
            opt.UserName = Model.sUserName;
            opt.OptMenu = OptMenu;
            opt.OptContent = OptContent;
            opt.OptTime = DateTime.Now;
            opt.StationID = Model.stationID;
            var addStr = JsonJoin.ModelToJson(opt);
            int ret = request.AddData(Model.token, "tOptLog", addStr);
        }
        #endregion


        #region CheDaoSet
        /// <summary>
        /// 获取车道设置
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<CheDaoSet> GetChannelSet(int stationId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["StationID"] = stationId;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["OnLine"] = "desc";
            dic0["CtrlNumber"] = "asc";
            dic0["InOut"] = "asc";
            List<CheDaoSet> lstCDS = request.FindData<List<CheDaoSet>>(Model.token, "tCheDaoSet", dic0, where);
            return lstCDS;
        }

        /// <summary>
        /// 保存车道设置(包含新增和修改)
        /// </summary>
        /// <param name="lstCDS"></param>
        /// <returns></returns>
        public bool SaveChannelSet(List<CheDaoSet> lstCDS)
        {
            if (null == lstCDS || lstCDS.Count == 0) { return false; }
            string addstr = JsonJoin.ModelToJson(lstCDS);
            int ret = request.SaveData(Model.token, "tCheDaoSet", addstr);
            return (ret > 0) ? true : false;
        }

        public int DeleteChannelSet(long ID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string delStr = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tCheDaoSet", delStr);
            return ret;
        }

        /// <summary>
        /// 查询挂失车场机号
        /// </summary>
        /// <param name="strState"></param>
        /// <returns></returns>
        public List<CheDaoSet> GetParking()
        {
            List<CheDaoSet> lstCDS = request.FindData<List<CheDaoSet>>(Model.token, "tCheDaoSet");
            return lstCDS;
        }

        /// <summary>
        /// 获取车道名称，机号，IP
        /// </summary>
        /// <returns></returns>
        public List<CheDaoSet> GetCtrName(int stationID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["StationID"] = stationID;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InOut"] = "asc";
            List<CheDaoSet> lstCDS = request.FindData<List<CheDaoSet>>(Model.token, "tCheDaoSet", dic0, where);
            return lstCDS;
        }

        /// <summary>
        /// 读取记录IP 机号
        /// </summary>
        /// <returns></returns>
        public List<CheDaoSet> GetReadName()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["StationID"] = Model.stationID;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["OnLine"] = "desc";
            dic0["CtrlNumber"] = "asc";
            dic0["InOut"] = "asc";
            List<CheDaoSet> lstCDS = request.FindData<List<CheDaoSet>>(Model.token, "tCheDaoSet", dic0, where);
            return lstCDS;
        }
        #endregion


        #region SysSetting(参数设置)
        /// <summary>
        /// 获取车场所有全局变量
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParkSysSet(int stationId)
        {
            Dictionary<string, string> dic = request.GetSysSettingObject(Model.token, stationId);
            return dic;
        }


        /// <summary>
        /// 设置车场所有全局变量
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public bool SaveParkSysSet(Dictionary<string, string> dic, int stationId)
        {
            if (null == dic || dic.Count == 0) return false;
            string addStr = JsonJoin.ModelToJson(dic);
            int ret = request.SetSysSettingObject(Model.token, stationId, addStr);
            return (ret > 0) ? true : false;
        }

        public void DataSourceToPubVar()
        {
            Dictionary<string, string> dic = GetParkSysSet(Model.stationID);

            if (dic != null && dic.Count > 0)
            {
                //2016-11-25
                //Model.bAppEnable = dic["bAppEnable"].ToString() == "1" ? true : false;

     
                Model.bAppEnable = ConvertValue<bool>(dic, "bAppEnable");
                Model.bSpecilCPH = ConvertValue<bool>(dic, "bSpecilCPH");
                Model.bCphAllEn = ConvertValue<bool>(dic, "bCphAllEn");
                Model.bCphAllSame = ConvertValue<bool>(dic, "bCphAllSame");


                #region 保存图像视频设置 (视频卡操作去掉)
       
                Model.iEnableNetVideo = ConvertValue<int>(dic, "iEnableNetVideo");
                Model.iVideo4 = ConvertValue<int>(dic, "bVideo4");
                Model.iPersonVideo = ConvertValue<int>(dic, "iPersonVideo");
                Model.iIDCapture = ConvertValue<int>(dic, "iIDCapture");
                Model.sImageSavePath = dic.ContainsKey("sImageSavePath") ? (dic["sImageSavePath"] ?? "D:\\") : "D:\\";
        
                Model.iImageAutoDel = ConvertValue<int>(dic, "bImageAutoDel");
                Model.iImageSaveDays = ConvertValue<int>(dic, "iImageSaveDays");
                Model.iImageAutoDelTime = ConvertValue<int>(dic, "iImageAutoDelTime");
                #endregion

                #region 保存收费设置变量

             
                Model.iChargeType = ConvertValue<int>(dic, "iChargeType");
                Model.iXsdNum = ConvertValue<int>(dic, "iXsdNum");
                Model.iDiscount = ConvertValue<int>(dic, "iDiscount");
                Model.iXsd = ConvertValue<int>(dic, "iXsd");
                Model.iFreeCar = ConvertValue<int>(dic, "iFreeCar");
                Model.iSetTempMoney = ConvertValue<int>(dic, "bSetTempMoney");
                Model.iYKOverTimeCharge = ConvertValue<int>(dic, "iYKOverTimeCharge");
                Model.iZGXE = ConvertValue<int>(dic, "iZGXE");
                Model.iModiTempType_VoiceSF = ConvertValue<int>(dic, "bModiTempType_VoiceSF");
                Model.iSFCancel = ConvertValue<int>(dic, "bSFCancel");
                Model.iMonthRule = ConvertValue<int>(dic, "bMonthRule");
                Model.iZGType = ConvertValue<int>(dic, "iZGType");
                Model.iZGXEType = ConvertValue<int>(dic, "iZGXEType");
                Model.iMothOverDay = ConvertValue<int>(dic, "iMothOverDay");
                Model.iSetTempCardType = ConvertValue<int>(dic, "bSetTempCardType");
                Model.iTempFree = ConvertValue<int>(dic, "iTempFree");
                Model.bCarYellowTmp = ConvertValue<bool>(dic, "bCarYellowTmp");
                Model.strCarYellowTmpType = dic.ContainsKey("strCarYellowTmpType") ? (dic["strCarYellowTmpType"] ?? "TmpB") : "TmpB";
                Model.sMonthOutChargeType = dic.ContainsKey("sMonthOutChargeType") ? (dic["sMonthOutChargeType"] ?? "TmpA") : "TmpA";
                #endregion

                #region 保存在线监控设置

             
                Model.iLoadTimeInterval = ConvertValue<int>(dic, "iLoadTimeInterval");
                Model.iDisplayTime = ConvertValue<int>(dic, "bDisplayTime");
                //软件控制道闸开关

                Model.iShowGateState = ConvertValue<int>(dic, "iShowGateState");
                Model.iExitOnlineByPwd = ConvertValue<int>(dic, "iExitOnlineByPwd");
                Model.iSoftOpenNoPlate = ConvertValue<int>(dic, "bSoftOpenNoPlate");
                Model.iCheDui = ConvertValue<int>(dic, "bCheDui");
                Model.iExceptionHandle = ConvertValue<int>(dic, "bExceptionHandle");
                Model.iShowBoxCardNum = ConvertValue<int>(dic, "bShowBoxCardNum");
                //Model.bOneKeyShortCut = chkOneKeyShortCut.Checked;

   
                Model.iAutoPrePlate = ConvertValue<int>(dic, "bAutoPrePlate");
                Model.iForbidSamePosition = ConvertValue<int>(dic, "bForbidSamePosition");
                Model.iCheckPortFirst = ConvertValue<int>(dic, "bCheckPortFirst");
                Model.iFullLight = ConvertValue<int>(dic, "iFullLight");
                Model.iDelayed = ConvertValue<int>(dic, "iDelayed");
                Model.bFullComfirmOpen = ConvertValue<bool>(dic, "bFullComfirmOpen");
                #endregion

                #region 保存ID卡功能设置

         
                Model.iIDSoftOpen = ConvertValue<int>(dic, "bIDSoftOpen");
                Model.iInOutLimitSeconds = ConvertValue<int>(dic, "iInOutLimitSeconds");
                Model.iRealTimeDownLoad = ConvertValue<int>(dic, "iRealTimeDownLoad");
                Model.iIdSfCancel = ConvertValue<int>(dic, "bIdSfCancel");
                Model.iICCardDownLoad = ConvertValue<int>(dic, "iICCardDownLoad");
                Model.iIdReReadHandle = ConvertValue<int>(dic, "bIdReReadHandle");
                Model.iIdPlateDownLoad = ConvertValue<int>(dic, "bIdPlateDownLoad");
                Model.iIDOneInOneOut = ConvertValue<int>(dic, "iIDOneInOneOut"); //ID控制一进一出有问题
                Model.iIDComfirmOpen = ConvertValue<int>(dic, "iIDComfirmOpen");
                Model.sID1In1OutCardType = dic.ContainsKey("sID1In1OutCardType") ? (dic["sID1In1OutCardType"] ?? "") : "";
                //Model.sID1In1OutCardType=
                //Model.sIDComfirmOpenCardType=
                #endregion

                #region 保存语音显示功能

                //Model.iCtrlShowPlate = Convert.ToInt32(dic.ContainsKey("bAppEnable") ? (null == dic["bCtrlShowPlate"] || dic["bCtrlShowPlate"].Trim().Length <= 0 ? "0" : dic["bCtrlShowPlate"]); 播报车牌号码
      
                Model.iCtrlShowStayTime = ConvertValue<int>(dic, "bCtrlShowStayTime");
                Model.iCtrlShowCW = ConvertValue<int>(dic, "bCtrlShowCW");
                Model.iCtrlShowInfo = ConvertValue<int>(dic, "bCtrlShowInfo");
                Model.iCtrlShowRemainPos = ConvertValue<int>(dic, "bCtrlShowRemainPos");
                Model.iCtrlVoiceLedVersion = ConvertValue<int>(dic, "iCtrlVoiceLedVersion");
                Model.iCtrlVoiceMode = ConvertValue<int>(dic, "iCtrlVoiceMode");
                Model.iIDNoticeDay = ConvertValue<int>(dic, "iIDNoticeDay");
                #endregion

                #region 外接附加设备设置(包含多功能语音模块)

     
                Model.iBillPrint = ConvertValue<int>(dic, "iBillPrint");
                Model.iBillPrintAuto = ConvertValue<int>(dic, "bBillPrintAuto");
                Model.iPrintFontSize = ConvertValue<int>(dic, "iPrintFontSize");
                //Model.iCarPosLed = carShow.Checked ? 1 : 0; //出入场图片不加水印

          
                Model.iCarPosCom = ConvertValue<int>(dic, "iCarPosCom");
                //Model.iCarPosLedJH = Convert.ToInt32(Combo5cwjh.Text);   //combo5cwjh 车牌识别

           
                Model.iCarPosLedLen = ConvertValue<int>(dic, "iCarPosLedLen");
                //Model.iSFLed = ClientS.Checked ? 1 : 0;   //脱机车牌（车牌识别）


                Model.iSFLedCom = ConvertValue<int>(dic, "iSFLedCom");
                Model.iSFLedType = ConvertValue<int>(dic, "iSFLedType");
                Model.iRemainPosLedShowInfo = ConvertValue<int>(dic, "bRemainPosLedShowInfo");
                Model.iRemainPosLedShowPlate = ConvertValue<int>(dic, "bRemainPosLedShowPlate");
                Model.iReLoginPrint = ConvertValue<int>(dic, "bReLoginPrint");


              
                Model.bOnlyLocation = ConvertValue<bool>(dic, "bOnlyLocation");
                Model.iBarCodePrint = ConvertValue<int>(dic, "bBarCodePrint");
                //Model.IsCPHAuto = ckbIsCPHAuto.Checked ? 1 : 0;  在线识别月卡不开闸
                #endregion

                #region 其它设置

     
                Model.iCtrlSetHasPwd = ConvertValue<int>(dic, "bCtrlSetHasPwd");
                Model.iQueryName = ConvertValue<int>(dic, "bQueryName");
                //Model.iWorkstationNo = Convert.ToInt32(dic.ContainsKey("bAppEnable") ? (null == dic["iWorkstationNo"] || dic["iWorkstationNo"].Trim().Length <= 0 ? "0" : dic["iWorkstationNo"]):"0");  //工作站编号
                //Model.iParkingNo = Convert.ToInt32(dic.ContainsKey("bAppEnable") ? (null == dic["iParkingNo"] || dic["iParkingNo"].Trim().Length <= 0 ? "0" : dic["iParkingNo"]):"0");

                Model.strAreaDefault = dic.ContainsKey("strAreaDefault") ? (dic["strAreaDefault"] ?? "粤") : "粤";

                Model.iFreeCardNoInPlace = ConvertValue<int>(dic, "bFreeCardNoInPlace");
                Model.iDetailLog = ConvertValue<int>(dic, "bDetailLog");
                Model.iSumMoneyHide = ConvertValue<int>(dic, "bSumMoneyHide");
                Model.iParkTotalSpaces = ConvertValue<int>(dic, "iParkTotalSpaces");
                Model.iTempCarPlaceNum = ConvertValue<int>(dic, "iTempCarPlaceNum");
                Model.iMonthCarPlaceNum = ConvertValue<int>(dic, "iMonthCarPlaceNum");
                Model.iMoneyCarPlaceNum = ConvertValue<int>(dic, "iMoneyCarPlaceNum");
                Model.iOnlyShowThisRemainPos = ConvertValue<int>(dic, "iOnlyShowThisRemainPos");


   
                Model.iOnlinePayEnabled = ConvertValue<int>(dic, "bOnlinePayEnabled");

                Model.strWXAppID = dic.ContainsKey("strWXAppID") ? (dic["strWXAppID"] ?? "") : "";
                Model.strWXMCHID = dic.ContainsKey("strWXMCHID") ? (dic["strWXMCHID"] ?? "") : "";
                Model.strWXKEY = dic.ContainsKey("strWXKEY") ? (dic["strWXKEY"] ?? "") : "";
                Model.strZFBAppID = dic.ContainsKey("strZFBAppID") ? (dic["strZFBAppID"] ?? "") : "";
                Model.strZFBPID = dic.ContainsKey("strZFBPID") ? (dic["strZFBPID"] ?? "") : "";

                if (Model.iOnlyShowThisRemainPos == 1)
                {
                    Model.bTempCarPlace = true;
                    Model.bMonthCarPlace = false;
                    Model.bMoneyCarPlace = false;
                }
                else if (Model.iOnlyShowThisRemainPos == 2)
                {
                    Model.bTempCarPlace = false;
                    Model.bMonthCarPlace = true;
                    Model.bMoneyCarPlace = false;
                }
                else if (Model.iOnlyShowThisRemainPos == 3)
                {
                    Model.bTempCarPlace = false;
                    Model.bMonthCarPlace = false;
                    Model.bMoneyCarPlace = true;
                }
                else
                {
                    Model.bTempCarPlace = false;
                    Model.bMonthCarPlace = false;
                    Model.bMoneyCarPlace = false;
                }


         
                Model.iOneKeyShortCut = ConvertValue<int>(dic, "bOneKeyShortCut");
                Model.iTempDown = ConvertValue<int>(dic, "bTempDown");
                Model.iAutoMinutes = ConvertValue<int>(dic, "bAutoMinutes");
                Model.LocalProvince = dic.ContainsKey("LocalProvince") ? (dic["LocalProvince"] ?? "粤") : "粤";

                Model.iAutoUpdateJiHao = ConvertValue<int>(dic, "bAutoUpdateJiHao");
                Model.iSFLed = ConvertValue<int>(dic, "iSFLed");
                Model.iAutoSetMinutes = ConvertValue<int>(dic, "iAutoSetMinutes");
                Model.iPromptDelayed = ConvertValue<int>(dic, "iPromptDelayed");
                #endregion

                #region 车牌参数设置

          
                Model.iAutoPlateEn = ConvertValue<int>(dic, "bAutoPlateEn");
                Model.iAutoPlateDBJD = ConvertValue<int>(dic, "iAutoPlateDBJD");
                Model.iInAutoOpenModel = ConvertValue<int>(dic, "iInAutoOpenModel");
                Model.iOutAutoOpenModel = ConvertValue<int>(dic, "iOutAutoOpenModel");
                Model.iInMothOpenModel = ConvertValue<int>(dic, "iInMothOpenModel");
                Model.iOutMothOpenModel = ConvertValue<int>(dic, "iOutMothOpenModel");
                Model.iCPHPhoto = ConvertValue<int>(dic, "bCPHPhoto");
                Model.iAutoDeleteImg = ConvertValue<int>(dic, "iAutoDeleteImg");
                Model.iSameCphDelay = dic.ContainsKey("iSameCphDelay") ? (null == dic["iSameCphDelay"] || dic["iSameCphDelay"].Trim().Length <= 0 ? "0" : dic["iSameCphDelay"]) : "0"; //--------------i
          
                Model.iCarPosLed = ConvertValue<int>(dic, "iCarPosLed");
                Model.iAutoKZ = ConvertValue<int>(dic, "iAutoKZ");
                Model.iAutoColorSet = ConvertValue<int>(dic, "iAutoColorSet");
                Model.iAuto0Set = ConvertValue<int>(dic, "iAuto0Set");
                Model.iNoCPHAutoKZ = ConvertValue<int>(dic, "bNoCPHAutoKZ");
                Model.iTempCanNotInSmall = ConvertValue<int>(dic, "bTempCanNotInSmall");
                Model.iAutoCPHDZ = ConvertValue<int>(dic, "bAutoCPHDZ");
                Model.iCentralCharge = ConvertValue<int>(dic, "bCentralCharge");
                Model.iOutCharge = ConvertValue<int>(dic, "bOutCharge");
                Model.iMorePaingCar = ConvertValue<int>(dic, "bMorePaingCar");
                Model.iMorePaingType = ConvertValue<int>(dic, "bMorePaingType");
                Model.iCarPosLedJH = ConvertValue<int>(dic, "iCarPosLedJH");
         
                Model.iCphDelay = dic.ContainsKey("iCphDelay") ? (dic["iCphDelay"] ?? "0") : "0";
                #endregion

            }

            #region 车道设置
            List<CheDaoSet> lstCDS = GetChannelSet(Model.stationID);
            Model.iChannelCount = lstCDS.Count;
            for (int i = 0; i < lstCDS.Count; i++)
            {
                Model.Channels[i].iInOut = lstCDS[i].InOut;
                Model.Channels[i].sInOutName = lstCDS[i].InOutName;
                Model.Channels[i].iCtrlID = lstCDS[i].CtrlNumber;
                Model.Channels[i].iOpenID = lstCDS[i].OpenID;
                Model.Channels[i].iOpenType = lstCDS[i].OpenType;
                //Model.PubVal.Channels[i].sCarVideo = lstCDS[i].;
                Model.Channels[i].sCarVideo = lstCDS[i].CameraIP;
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
            #endregion
        }

        protected T ConvertValue<T>(Dictionary<string, string> dic, string Key)
        {
            string strValue;

            if (null == Key || !dic.ContainsKey(Key))
            {
                return default(T);
            }
            strValue = dic[Key];

            //布尔型
            if (typeof(T) == typeof(bool))
            {
                decimal tmpNumber;
                if (decimal.TryParse(strValue, out tmpNumber))
                {
                    return (T)Convert.ChangeType(tmpNumber > 0, typeof(T));
                }
                else
                {
                    bool tmpValue;
                    if (bool.TryParse(strValue, out tmpValue))
                    {
                        return (T)Convert.ChangeType(tmpValue, typeof(T));
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }

            //数字型
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(int) || typeof(T) == typeof(uint) || typeof(T) == typeof(long) || typeof(T) == typeof(ulong) || typeof(T) == typeof(decimal) ||
                typeof(T) == typeof(byte?) || typeof(T) == typeof(sbyte?) || typeof(T) == typeof(short?) || typeof(T) == typeof(ushort?) ||
                typeof(T) == typeof(int?) || typeof(T) == typeof(uint?) || typeof(T) == typeof(long?) || typeof(T) == typeof(ulong?) || typeof(T) == typeof(decimal?))
            {
                decimal tmpValue;
                decimal.TryParse(strValue, out tmpValue);
                if (decimal.TryParse(strValue, out tmpValue))
                {
                    return (T)Convert.ChangeType(tmpValue, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }

            //日期型
            if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
            {
                DateTime tmpValue;
                if (DateTime.TryParse(strValue, out tmpValue))
                {
                    return (T)Convert.ChangeType(tmpValue, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }

            return (T)Convert.ChangeType(strValue, typeof(T));
        }
        #endregion


        #region ChargeRules
        public List<ChargeRules> GetMonthRuleDefine()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardType"] = "Mth%";
            string where = JsonJoin.ToJson(dic, false, true);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["CardType"] = "asc";
            List<ChargeRules> lstCR = request.FindData<List<ChargeRules>>(Model.token, "tChargeRules", dic0, where);
            return lstCR;
        }

        public List<ChargeRules> GetMonthRuleDefine(string cardType)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardType"] = cardType;
            string where = JsonJoin.ToJson(dic);
            List<ChargeRules> lstCR = request.FindData<List<ChargeRules>>(Model.token, "tChargeRules", where);
            return lstCR;
        }


        /// <summary>
        /// 保存月卡收费自定义
        /// </summary>
        /// <param name="lstCR"></param>
        /// <returns></returns>
        public bool SaveMonthRuleDefine(List<ChargeRules> lstCR)
        {
            int ret = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardType"] = "Mth%";
            string where = JsonJoin.ToJson(dic, false, true);
            ret = request.DeleteDataBy(Model.token, "tChargeRules", where);
            for (int i = 0; i < lstCR.Count; i++)
            {
                if (CR.IsChineseCharacters(lstCR[i].CardType))
                {
                    lstCR[i].CardType = CR.GetCardType(lstCR[i].CardType, 0);
                }
            }
            string addstr = JsonJoin.ModelToJson(lstCR);
            ret = request.AddDataList(Model.token, "tChargeRules", addstr);
            return (ret > 0) ? true : false;
        }
        #endregion


        #region FreeReason
        public List<FreeReason> GetFreeReasonDefine()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["ItemID"] = "asc";
            List<FreeReason> lstFR = request.FindData<List<FreeReason>>(Model.token, "tFreeReason", dic);
            return lstFR;
        }


        public bool SaveFreeReasonDefine(List<FreeReason> lstFR)
        {
            int ret = request.DeleteDataBy(Model.token, "tFreeReason");
            string addstr = JsonJoin.ModelToJson(lstFR);
            ret = request.AddDataList(Model.token, "tFreeReason", addstr);
            return (ret > 0) ? true : false;
            //for (int i = 0; i < lstFR.Count; i++)
            //{
            //    string addStr = JsonJoin.ModelToJson<FreeReason>(lstFR[i]);
            //    ret = request.AddData(Model.token, "tFreeReason", addStr);
            //}
        }


        #endregion


        #region NetCameraSet(网络摄像机)
        public bool AddNetVideo(List<NetCameraSet> lstNCS)
        {
            if (null == lstNCS || lstNCS.Count == 0) return false;
            string addstr = JsonJoin.ModelToJson(lstNCS);
            int ret = request.AddDataList(Model.token, "tNetCameraSet", addstr);
            return (ret > 0) ? true : false;
        }

        public bool SaveNetVideo(List<NetCameraSet> lstNCS)
        {
            if (null == lstNCS || lstNCS.Count == 0) return false;
            string addstr = JsonJoin.ModelToJson(lstNCS);
            int ret = request.SaveData(Model.token, "tNetCameraSet", addstr);
            return (ret > 0) ? true : false;
        }

        public int DeleteVideo(string ip)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["VideoIP"] = ip;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tNetCameraSet", where);
            return ret;
        }

        public int SelectVideoIP(string IP)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["VideoIP"] = IP;
            string where = JsonJoin.ToJson(dic);
            List<NetCameraSet> lstNCS = request.FindData<List<NetCameraSet>>(Model.token, "tNetCameraSet", where);
            return lstNCS.Count;
        }

        public List<NetCameraSet> SelectVideo(string IP)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["VideoIP"] = IP;
            string where = JsonJoin.ToJson(dic);
            List<NetCameraSet> lstNCS = request.FindData<List<NetCameraSet>>(Model.token, "tNetCameraSet", where);
            return lstNCS;
        }

        public List<NetCameraSet> SelectVideo(int ID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string where = JsonJoin.ToJson(dic);
            List<NetCameraSet> lstNCS = request.FindData<List<NetCameraSet>>(Model.token, "tNetCameraSet", where);
            return lstNCS;
        }

        public List<NetCameraSet> SelectVideoAllIP()
        {
            List<NetCameraSet> lstNCS = request.FindData<List<NetCameraSet>>(Model.token, "tNetCameraSet");
            return lstNCS;
        }
        #endregion


        #region CardTypeDef(卡片类型定义)
        /// <summary>
        /// 查询月卡数目
        /// </summary>
        /// <returns></returns>
        public List<CardTypeDef> GetMonth()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Enabled"] = "1%";
            dic["Identifying"] = "Mth%";
            string where = JsonJoin.ToJson(dic, false, true);
            List<CardTypeDef> lstCTD = request.FindData<List<CardTypeDef>>(Model.token, "tCardTypeDef", where);
            return lstCTD;
        }

        public List<CardTypeDef> GetTemp()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Enabled"] = "%1%";
            dic["Identifying"] = "Tmp%";
            string where = JsonJoin.ToJson(dic, false, true);
            List<CardTypeDef> lstCTD = request.FindData<List<CardTypeDef>>(Model.token, "tCardTypeDef", where);
            return lstCTD;
        }

        public List<CardTypeDef> GetCardType()
        {
            List<CardTypeDef> lstCtd = request.FindData<List<CardTypeDef>>(Model.token, "tCardTypeDef");
            return lstCtd;
        }

        public List<CardTypeDef> GetGetFXCardTypeToTrue()
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Identifying", Operator = "like", FieldValue = "Mth%", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Identifying", Operator = "like", FieldValue = "Fre%", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Identifying", Operator = "like", FieldValue = "Str%", Combinator = "or" });
            lstSM.Add(sm);
            sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Enabled", Operator = "=", FieldValue = "1", Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardTypeDef> lstCTD = request.FindData<List<CardTypeDef>>(Model.token, "tCardTypeDef", where);
            return lstCTD;
        }

        public List<CardTypeDef> GetGetCardTypeToTrue()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Enabled"] = 1;
            string where = JsonJoin.ToJson(dic);
            List<CardTypeDef> lstCTD = request.FindData<List<CardTypeDef>>(Model.token, "tCardTypeDef", where);
            return lstCTD;
        }

        public List<CardTypeDef> GetIDCardType()
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Identifying", Operator = "like", FieldValue = "Mth%", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Identifying", Operator = "like", FieldValue = "Fre%", Combinator = "or" });
            lstSM.Add(sm);
            sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Enabled", Operator = "=", FieldValue = "1", Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["CardType"] = "asc";
            List<CardTypeDef> lstCTD = request.FindData<List<CardTypeDef>>(Model.token, "tCardTypeDef", dic, where);
            return lstCTD;
        }

        public List<CardTypeDef> GetTypeTo(string CardType)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Identifying", Operator = "like", FieldValue = CardType.Substring(0, 3) + "%", Combinator = "or" });
            lstSM.Add(sm);
            sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Enabled", Operator = "=", FieldValue = "1", Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardTypeDef> lstCTD = request.FindData<List<CardTypeDef>>(Model.token, "tCardTypeDef", where);
            return lstCTD;
        }
        #endregion


        #region CardIssue
        public List<CardIssue> GetFaXingCPHDownLoad(int workStationNo)
        {
            //select MYFAXINGSSUE where left(SubSystem,1)='1' and substring(CPHDownloadSignal,workStationNo,1)='0' and  CardState='0' 
            //and (left(CarCardType,3)='Mth' or left(CarCardType,3)='Fre' or left(CarCardType,3)='Str')

            //CPHDownloadSignal ->  CPHDownloadSignal  CPHDownloadSignal

            string tmpstr = "";
            for (int i = 1; i < workStationNo; i++)
            {
                tmpstr += "_";
            }
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Subsystem", Operator = "like", FieldValue = "1%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPHDownloadSignal", Operator = "like", FieldValue = tmpstr + "0%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = 0, Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Mth%", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Fre%", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Str%", Combinator = "or" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);

            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }


        public int UpdateCPHDownLoad(int ID, string Down)
        {
            //CardIssue ci = new CardIssue();
            //ci.ID = ID;
            //ci.CPHDownloadSignal = Down;
            //string updStr = JsonJoin.ModelToJson<CardIssue>(ci);
            //int ret = request.UpdateData(Model.token, "tCardIssue", updStr);
            //return ret;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["CPHDownloadSignal"] = Down;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }


        /// <summary>
        /// 查询是否下载
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public List<CardIssue> GetFaXingDownLoad(int BiaoZhi, int workstationNo)
        {
            string tmpstr = "";
            for (int i = 1; i < workstationNo; i++)
            {
                tmpstr += "_";
            }
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "Subsystem", Operator = "like", FieldValue = "1%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPHDownloadSignal", Operator = "like", FieldValue = tmpstr + "0%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = 0, Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            sm = new SelectModel();
            if (BiaoZhi == 1)
            {
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Mth%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Fre%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Opt%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Tmp%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "like", FieldValue = "%________%", Combinator = "or" });
            }
            else
            {
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Mth%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Fre%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Str%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Opt%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Tmp%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Cnt%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "like", FieldValue = "%________%", Combinator = "or" });
            }

            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }


        public int UpdateDownLoad(int ID, string Down)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["DownloadSignal"] = Down;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }


        /// <summary>
        /// 查询解挂
        /// </summary>
        /// <param name="strState"></param>
        /// <returns></returns>
        public List<CardIssue> GetJLost(string strState)
        {
            string tmpstr = "";
            for (int i = 1; i < Model.stationID; i++)
            {
                tmpstr += "_";
            }
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = strState, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CaRFailOKNO", Operator = "=", FieldValue = 0, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "SubSystem", Operator = "like", FieldValue = "1%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "DownloadSignal", Operator = "like", FieldValue = "%" + tmpstr + "0%", Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["CardState"] = "asc";
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", dic, where);
            return lstCI;
        }


        /// <summary>
        /// 查询发行信息
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public List<CardIssue> GetFaXing(string CarNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CarNo;
            string where = JsonJoin.ToJson(dic);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }


        /// <summary>
        /// 挂失修改成功机号和岗位口
        /// </summary>
        /// <param name="strState"></param>
        /// <returns></returns>
        public int GetDownLoad(string strDown, string strCardLossMachine, int ID)
        {
            CardIssue ci = new CardIssue();
            ci.DownloadSignal = strDown;
            ci.CardLossMachine = strCardLossMachine;
            ci.ID = ID;
            string updstr = JsonJoin.ModelToJson(ci);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr);
            return ret;
        }


        /// <summary>
        /// 查询退卡
        /// </summary>
        /// <param name="strState"></param>
        /// <returns></returns>
        public List<CardIssue> GetOutCard(int DownloadSignal)
        {
            string tmpstr = "";
            for (int i = 1; i < DownloadSignal; i++)
            {
                tmpstr += "_";
            }
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = 5, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "SubSystem", Operator = "like", FieldValue = "1%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "DownloadSignal", Operator = "like", FieldValue = "%" + tmpstr + "0%", Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }


        /// <summary>
        /// 修改退卡好卡下载标识符
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="Down"></param>
        /// <returns></returns>
        public int UpdateTKDownLoad(int ID, string Down)
        {
            CardIssue ci = new CardIssue();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["DownloadSignal"] = Down;
            string updstr = JsonJoin.ObjectToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }


        /// <summary>
        /// 查询车牌号退卡
        /// </summary>
        /// <param name="strState"></param>
        /// <returns></returns>
        public List<CardIssue> GetOutCPHCard(int DownloadSignal)
        {
            string tmpstr = "";
            for (int i = 1; i < DownloadSignal; i++)
            {
                tmpstr += "_";
            }
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = 5, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "SubSystem", Operator = "like", FieldValue = "1%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPHDownloadSignal", Operator = "like", FieldValue = tmpstr + "0%", Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }


        /// <summary>
        /// 查询脱机车牌查询卡号
        /// </summary>
        /// <param name="CardNO"></param>
        /// <returns></returns>
        public string GetCardNO(string strCPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            string where = JsonJoin.ToJson(dic);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            if (lstCI.Count > 0)
            {
                return lstCI[0].CardNO;
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 查询ID卡卡类
        /// </summary>
        /// <param name="CardNO"></param>
        /// <returns></returns>
        public string GetCardType(string CardNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            string where = JsonJoin.ToJson(dic);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            if (lstCI.Count > 0)
            {
                return lstCI[0].CarCardType;
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 根据对位数位查找发行表中符合条件的记录
        /// </summary>
        /// <param name="CPH">对比车牌号</param>
        /// <param name="JingDu">对比位数</param>
        /// <param name="CardType1">卡类1</param>
        /// <param name="CardType2">卡类2</param>
        /// <param name="CardType3">卡类3</param>
        /// <returns>符合条件的记录</returns>
        public List<CardIssue> SelectFxCPH(string CPH, int JingDu, string CardType1, string CardType2, string CardType3)
        {
            //武警车怎么处理
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = CardType1 + "%", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = CardType2 + "%", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = CardType3 + "%", Combinator = "or" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = "0", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "<>", FieldValue = "", Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.GetCardIssueByCarPlateNumberLike<List<CardIssue>>(Model.token, CPH, JingDu, where);
            return lstCI;
        }


        /// <summary>
        /// 更新发行下载状态
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int UpdateICFaXingDate(int ID)
        {
            CardIssue ci = new CardIssue();
            ci.ID = ID;
            ci.DownloadSignal = "00000000000000000000000000000000000000000000000000";
            ci.CPHDownloadSignal = "00000000000000000000000000000000000000000000000000";
            string updstr = JsonJoin.ModelToJson(ci);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr);
            return ret;
        }

        /// <summary>
        /// IC储值卡充值
        /// </summary>
        /// <param name="CarNo">卡号</param>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <returns></returns>
        public int UpdateICYU(string CarNo, decimal yue)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CarNo;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["Balance"] = yue;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }

        public int UpdateICFaXingDate(string cardNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNO;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["DownloadSignal"] = "00000000000000000000000000000000000000000000000000";
            dic0["CPHDownloadSignal"] = "00000000000000000000000000000000000000000000000000";
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }

        public int UpdateICFaXingDate(string CarNo, DateTime StartDate, DateTime EndDate)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CarNo;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["CarValidStartDate"] = StartDate;
            dic0["CarValidEndDate"] = EndDate;
            dic0["DownloadSignal"] = "00000000000000000000000000000000000000000000000000";
            dic0["CPHDownloadSignal"] = "00000000000000000000000000000000000000000000000000";
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }

        public int UpdateBalance(string CardNO, decimal Money)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "=", FieldValue = CardNO, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "SubSystem", Operator = "like", FieldValue = "1%", Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["Balance"] = Money;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }

        public List<CardIssue> SelectFxCPH_Like(string CPH)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "like", FieldValue = "%" + CPH + "%", Combinator = "and" });

            lstSM.Add(sm);
            sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = 0, Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = 2, Combinator = "or" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["CPH"] = "asc";
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", dic0, where);
            return lstCI;
        }

        public List<CardIssue> GetCarChePIss(Dictionary<string, object> dic = null)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "SubSystem", Operator = "like", FieldValue = "1%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "=", FieldValue = "0", Combinator = "and" });
            lstSM.Add(sm);

            if (null != dic && dic.Count > 0)
            {
                foreach (var vr in dic)
                {
                    sm.Conditions.Add(new SelectModel.conditions { FieldName = vr.Key, Operator = "like", FieldValue = vr.Value, Combinator = "and" });
                }
                lstSM.Add(sm);
            }

            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["CarissueDate"] = "desc";

            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", dic0, where);
            return lstCI;
        }

        public List<CardIssue> GetAutoCardNo()
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            //sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "like", FieldValue = "________", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "like", FieldValue = "88______%", Combinator = "or" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["CardNO"] = "desc";
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", dic0, 1, where, 1);
            return lstCI;
        }

        public int UpdateICState(string CarNo, string start)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CarNo;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["CardState"] = start;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }

        public int UpdateOutCard(string subSystem, string CardNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNo;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["DownloadSignal"] = subSystem;
            dic0["CPHDownloadSignal"] = subSystem;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr, where);
            return ret;
        }

        public bool IfCPHExitsbt(string strCPH)
        {
            if (strCPH.Length < 6) { return false; }
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "like", FieldValue = "%" + strCPH.Substring(1) + "%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "<>", FieldValue = "5", Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            if (lstCI.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IfCardNOExitsbt(string strCardNO)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "=", FieldValue = strCardNO, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "<>", FieldValue = "5", Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            if (lstCI.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int UpdateCPdjfx(CardIssue model)
        {
            if (null == model) return 0;
            string updstr = JsonJoin.ModelToJson(model);
            int ret = request.UpdateData(Model.token, "tCardIssue", updstr);
            return ret;
        }

        public int DeleteFaXing(string CarNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CarNo;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tCardIssue", where);
            return ret;
        }

        public int Addchdj(CardIssue model)
        {
            if (null == model) return 0;
            if ((model.CarCardType.Substring(0, 3) == "Mth" || model.CarCardType.Substring(0, 3) == "Fre" || model.CarCardType.Substring(0, 3) == "Str") && model.CPH != "")
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["CPH"] = model.CPH;
                string where = JsonJoin.ToJson(dic);
                int ret = request.DeleteDataBy(Model.token, "tCardIssue", where);
            }
            string addstr = JsonJoin.ModelToJson(model);
            int ret0 = request.AddData(Model.token, "tCardIssue", addstr);
            return ret0;
        }

        public List<CardIssue> GetMyfaxingssue(string cardNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNo;
            dic["CardState"] = 0;
            string where = JsonJoin.ToJson(dic);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }

        public List<CardIssue> GetReadFaxing(string CardNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNo;
            string where = JsonJoin.ToJson(dic);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }

        public List<CardIssue> GetMonthCardEndDate(string cardNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNo;
            string where = JsonJoin.ToJson(dic);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }

        public List<CardIssue> SelectPersonBirthDate(string cardNo)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "=", FieldValue = cardNo, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "BirthDate", Operator = "like", FieldValue = "____-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00") + "%", Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }

        public List<CardIssue> SelectFaXing(string CPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = CPH;
            dic["CardState"] = 0;
            string where = JsonJoin.ToJson(dic);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            return lstCI;
        }

        /// <summary>
        /// 查询固定卡期限
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CardIssue> GetWhereDeadline(List<SelectModel> lstSM)
        {
            try
            {
                SelectModel sm = new SelectModel();
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Mth%", Combinator = "or" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarCardType", Operator = "like", FieldValue = "Fre%", Combinator = "or" });
                lstSM.Add(sm);
                sm = new SelectModel();
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardState", Operator = "<>", FieldValue = "5", Combinator = "and" });
                lstSM.Add(sm);
                string where = JsonJoin.ModelToJson(lstSM);
                List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
                for (int i = 0; i < lstCI.Count; i++)
                {
                    lstCI[i].SurplusDays = CR.DateDiff(CR.DateInterval.Day, Convert.ToDateTime(DateTime.Now.ToShortDateString()), lstCI[i].CarValidEndDate);
                }
                return lstCI;
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region Blacklist
        /// <summary>
        /// 删除黑名单下载
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public List<Blacklist> GetBlacklistDCPHDownLoad(int WorkstationNo)
        {
            string tmpstr = "";
            for (int i = 1; i < WorkstationNo; i++)
            {
                tmpstr += "_";
            }

            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "AddDelete", Operator = "=", FieldValue = "1", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "DownloadSignal", Operator = "like", FieldValue = tmpstr + "1%", Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<Blacklist> lstBL = request.FindData<List<Blacklist>>(Model.token, "tBlacklist", where);
            return lstBL;
        }

        /// <summary>
        /// 修改车牌号下载标识符
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="Down"></param>
        /// <returns></returns>
        public int UpdateBlacklistDownLoad(int ID, string Down)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["DownloadSignal"] = Down;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tBlacklist", updstr, where);
            return ret;
        }

        /// <summary>
        /// 删除黑名单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteMYBlacklist(int ID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tBlacklist", where);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateMYBlacklist(int ID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["ID"] = ID;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["AddDelete"] = 1;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tBlacklist", updstr, where);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查询是否下载
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public List<Blacklist> GetBlacklistCPHDownLoad(int WorkstationNo)
        {
            string tmpstr = "";
            for (int i = 1; i < WorkstationNo; i++)
            {
                tmpstr += "_";
            }

            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "AddDelete", Operator = "=", FieldValue = "0", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "DownloadSignal", Operator = "like", FieldValue = tmpstr + "0%", Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<Blacklist> lstBL = request.FindData<List<Blacklist>>(Model.token, "tBlacklist", where);
            return lstBL;
        }

        /// <summary>
        /// 查询黑名单
        /// </summary>
        /// <returns></returns>
        public List<Blacklist> SelectBlacklist()
        {
            List<Blacklist> lstBL = request.FindData<List<Blacklist>>(Model.token, "tBlacklist");
            return lstBL;
        }

        /// <summary>
        /// 通过车牌号查找黑名单
        /// </summary>
        /// <returns></returns>
        public List<Blacklist> SelectBlacklist(string strCPH)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "=", FieldValue = strCPH, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "StartTime", Operator = "<", FieldValue = DateTime.Now, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "EndTime", Operator = ">", FieldValue = DateTime.Now, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "AddDelete", Operator = "=", FieldValue = 0, Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<Blacklist> lstBL = request.FindData<List<Blacklist>>(Model.token, "tBlacklist", where);
            return lstBL;
        }

        /// <summary>
        /// 新增一条黑名单数据
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        public int AddMYBlacklist(Blacklist bl)
        {
            if (null == bl) return 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = bl.CPH;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tBlacklist", where);
            string addstr = JsonJoin.ModelToJson(bl);
            ret = request.AddData(Model.token, "tBlacklist", addstr);
            return ret;
        }
        #endregion


        #region CarIn
        /// <summary>
        /// 查询入场表车牌
        /// </summary>
        public List<CarIn> SelectComeCPH(string CPH, int JingDu, string CardType1, string CardType2)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardType", Operator = "like", FieldValue = CardType1 + "%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardType", Operator = "like", FieldValue = CardType2 + "%", Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CarIn> lstCI = request.GetCarInByCarPlateNumberLike<List<CarIn>>(Model.token, CPH, JingDu, where);
            return lstCI;
        }

        public List<CarIn> SelectComeCPHLike(string CPH, int JingDu, string CardType1, string CardType2, int iBigSmall)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardType", Operator = "like", FieldValue = CardType1 + "%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardType", Operator = "like", FieldValue = CardType2 + "%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "BigSmall", Operator = "like", FieldValue = iBigSmall, Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CarIn> lstCI = request.GetCarInByCarPlateNumberLike<List<CarIn>>(Model.token, CPH, JingDu, where);
            return lstCI;
        }

        /// <summary>
        /// 修改入场表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CarparkNO"></param>
        /// <param name="BigSmall"></param>
        /// <returns></returns>
        public int UpdateIn(CarIn ci, int carParkNO, int bigSmall)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = ci.CardNO;
            dic["CarparkNO"] = carParkNO;
            dic["BigSmall"] = bigSmall;
            dic["InTime"] = ci.InTime;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["InGateName"] = ci.InGateName;
            dic0["InOperatorCard"] = ci.InOperatorCard;
            dic0["InOperator"] = ci.InOperator;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarIn", updstr, where);
            return ret;
        }

        /// <summary>
        /// 查询入场记录
        /// </summary>
        /// <param name="MyCardNO">卡片编号</param>
        /// <returns></returns>
        public List<CarIn> GetMyRsX(string MyCardNO, string CardType, int CarparkNO, int BigSmall)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = MyCardNO;
            dic["CarparkNO"] = CarparkNO;
            dic["BigSmall"] = BigSmall;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InTime"] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, 1, where);
            return lstCI;
            //DataSet ds = TypeCovert.ToDataSet<CarIn>(lstCI);
            //return ds;
        }

        /// <summary>
        /// 查询入场记录
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public List<CarIn> GetInGate(Dictionary<string, object> dic)
        {
            dic["CarparkNO"] = Model.iParkingNo;
            string where = JsonJoin.ToJson(dic, false, true);
            List<CarIn> lstCI = new List<CarIn>();
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["ID"] = "desc";
            if (where == "")
            {
                lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0);
            }
            else
            {
                lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, where);
            }

            return lstCI;
        }

        /// <summary>
        /// 统计场内各类车数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="carParkNo"></param>
        /// <returns></returns>
        public ParkingInfo GetParkingInfo(string startTime, string carParkNo)
        {
            ParkingInfo parkinfo = request.GetParkingInfo<ParkingInfo>(Model.token, startTime, carParkNo);
            return parkinfo;
        }

        /// <summary>
        /// 根据卡号查询最近入场记录
        /// </summary>
        /// <param name="cardNO"></param>
        /// <returns></returns>
        public List<CarIn> GetCarcomerecord(string cardNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNO;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InTime"] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, 1, where);
            return lstCI;
        }

        public List<CarIn> SelectComeCPH_Like(string CPH)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "like", FieldValue = "%" + CPH + "%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "bigsmall", Operator = "=", FieldValue = 0, Combinator = "and" });
            //2016-10-14 新增
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarParkNo", Operator = "=", FieldValue = Model.iParkingNo, Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InTime"] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, where);
            return lstCI;
        }

        public List<CarIn> GetInNoCPH(Dictionary<string, object> dic, DateTime dtStart, DateTime dtEnd, string fieldName = "InTime")
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "SFOperatorCard", Operator = "=", FieldValue = "无牌车", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "BigSmall", Operator = "=", FieldValue = 0, Combinator = "and" });
            foreach (var v in dic)
            {
                sm.Conditions.Add(new SelectModel.conditions { FieldName = v.Key, Operator = "like", FieldValue = "%" + v.Value + "%", Combinator = "and" });
            }
            sm.Conditions.Add(new SelectModel.conditions { FieldName = fieldName, Operator = ">=", FieldValue = dtStart, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = fieldName, Operator = "<=", FieldValue = dtEnd, Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0[fieldName] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, where);
            return lstCI;
        }

        public List<CarIn> GetNoCentralCharge(string cardNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNo;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InTime"] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, where);
            return lstCI;
        }

        public int GetInCPH(string strCPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            string where = JsonJoin.ToJson(dic);
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", where);
            return lstCI.Count;
        }

        public int UpdateInCPH(string strCardNONew, string sCardNO, string strCPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            dic["CardNO"] = sCardNO;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["CardNO"] = strCardNONew;
            dic0["InTime"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dic0["CardType"] = "TmpA";
            dic0["FreeReason"] = "退卡切换为临时车";
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarIn", updstr, where);
            return ret;
        }

        public int GetInRecordIsTmp(string strCPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = "%" + strCPH.Substring(1);
            dic["CardType"] = "Tmp%";
            string where = JsonJoin.ToJson(dic, false, true);
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", where);
            return lstCI.Count;
        }

        public int UpdateInCPHCardType(string strCPH, string sCardType, string strCardNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = "%" + strCPH.Substring(1);
            dic["CardType"] = "Tmp%";
            string where = JsonJoin.ToJson(dic, false, true);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["CardType"] = sCardType;
            dic0["CPH"] = strCPH;
            dic0["CardNO"] = strCardNO;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarIn", updstr, where);
            return ret;
        }

        public List<CarIn> GetMyCarComeRecord(Dictionary<string, object> dic)
        {
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, where);
            return lstCI;
        }

        public int GetComeCount(string userNo, string strCPH)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "UserNO", Operator = "=", FieldValue = userNo, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "<>", FieldValue = strCPH, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "SFOperatorCard", Operator = "=", FieldValue = "", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "BigSmall", Operator = "=", FieldValue = 0, Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", where);
            return lstCI.Count;
        }

        public int GetReadIn(string carPlace, string cardNo)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CarPlace", Operator = "=", FieldValue = carPlace, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "<>", FieldValue = cardNo, Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);

            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where);
            int ret = 0;

            for (int i = 0; i < lstCI.Count; i++)
            {
                sm.Conditions.Clear();
                lstSM.Clear();
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "=", FieldValue = lstCI[i].CardNO, Combinator = "and" });
                lstSM.Add(sm);
                where = JsonJoin.ModelToJson(lstSM);
                List<CarIn> lstCI0 = request.FindData<List<CarIn>>(Model.token, "tCarIn", where);
                if (lstCI0.Count > 0)
                {
                    ret++;
                }
            }
            return ret;
        }

        public int GetInMonth(string cardNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNO;
            dic["SFOperatorCard"] = "123456";
            dic["BigSmall"] = 0;
            string where = JsonJoin.ToJson(dic);
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", where);
            return lstCI.Count;
        }

        public List<CarIn> GetCentralCharge(string strCPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InTime"] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, where);
            return lstCI;
        }

        public int AddAdmission(CarIn ci, int OnLine)
        {
            if (null == ci) return 0;
            if (OnLine == 10)
            {
                ci.ID = ci.ID | 0x80;
            }
            string addstr = JsonJoin.ModelToJson(ci);
            int ret = request.SetCarIn(Model.token, addstr);
            return ret;
        }

        public int UpdateComerecord(string UpdateCardNO, string CarType, string CPH, string InPic, string CardNO, DateTime InTime)
        {
            string updstr = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            dic["InTime"] = InTime;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            if (CarType == "")
            {
                dic0["CPH"] = CPH;
                dic0["InPic"] = InPic;
            }
            else
            {
                dic0["CardNO"] = UpdateCardNO;
                dic0["CardType"] = CarType;
                dic0["CPH"] = CPH;
                dic0["InPic"] = InPic;
            }
            updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarIn", updstr, where);
            return ret;
        }

        public int UpdateCarIn(string cardNo, string path)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CarparkNO"] = Model.iParkingNo;
            dic["CardNO"] = cardNo;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["InPic"] = path;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarIn", updstr, where);
            return ret;
        }


        public int DeleteComerecord(string CardNO, DateTime InTime)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            dic["InTime"] = InTime;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tCarIn", where);
            return ret;
        }

        public int AddYiChang(CarIn ci)
        {
            string sStayTime = GetStayTime(ci.InTime, ci.OutTime);
            ci.StayTime = sStayTime;
            string addstr = JsonJoin.ModelToJson(ci);
            int ret = request.AddData(Model.token, "tCarOut", addstr);
            return ret;
        }

        public List<CarIn> SelectCome(string CardNO, int TTCBianHao, int BigSmall)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            dic["CarparkNO"] = TTCBianHao;
            dic["BigSmall"] = BigSmall;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InTime"] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, 1, where);
            return lstCI;
        }

        public int UpdateComeCPH(string UpdateCardNO, string CardType, string CarNo, string CPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CarNo;
            string where = JsonJoin.ToJson(dic);

            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["CPH"] = CPH;
            if (CardType == "")
            {
                dic0["SFOperator"] = "已校验";
            }
            else
            {
                dic0["CardNO"] = UpdateCardNO;
                dic0["CardType"] = CardType;
            }
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarIn", updstr, where);
            return ret;
        }

        public List<CarIn> GetWhereInCarCharge(List<SelectModel> lstSM)
        {
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["InTime"] = "desc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic, where);
            return lstCI;
        }

        public List<CarIn> GetCarIn(string field, string date)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "InTime", Operator = ">=", FieldValue = date, Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic[field] = "asc";
            List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic, 50, where);
            return lstCI;
        }

        public int DeleteMYCARCOMERECORD(string cardNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNO;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tCarIn", where);
            return ret;
        }

        public ReportCar GetWhereInStatistics(List<SelectModel> lstSM)
        {
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["InTime"] = "desc";
            ReportCar rc = request.GetCarInsummaryInfo<ReportCar>(Model.token, dic, where);
            return rc;
        }
        #endregion


        #region CarOut
        public int GetOffLineOut(string CPH, DateTime dtIn)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = CPH;
            dic["InTime"] = dtIn;
            string where = JsonJoin.ToJson(dic);
            List<CarOut> lstCO = request.FindData<List<CarOut>>(Model.token, "tCarOut", where);
            return lstCO.Count;
        }


        /// <summary>
        /// 删除多进多出卡号 (将指定入场记录移动到出场记录)
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public int DeleteInOutCPH(string CardNo, int iBigSmall, DateTime? dtIn = null)
        {
            if (CardNo.Length > 6)
            {
                List<SelectModel> lstSM = new List<SelectModel>();
                SelectModel sm = new SelectModel();
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "like", FieldValue = "%" + CardNo.Substring(1) + "%", Combinator = "and" });
                sm.Conditions.Add(new SelectModel.conditions { FieldName = "BigSmall", Operator = "=", FieldValue = iBigSmall, Combinator = "and" });
                if (dtIn == null)
                {

                }
                else
                    sm.Conditions.Add(new SelectModel.conditions { FieldName = "InTime", Operator = "<", FieldValue = dtIn, Combinator = "and" });
                sm.Combinator = "and";
                lstSM.Add(sm);
                string where = JsonJoin.ModelToJson(lstSM);
                List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", where);
                int ret = request.DeleteDataBy(Model.token, "tCarIn", where);
                if (ret > 0)
                {
                    string addstr = JsonJoin.ModelToJson(lstCI);
                    int ret0 = request.AddDataList(Model.token, "tCarOut", addstr);
                    return ret0;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public int UpdateIn(CarIn ci)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = ci.CPH;
            dic["InTime"] = ci.InTime;
            string where = JsonJoin.ToJson(dic);
            List<CarOut> lstCO = request.FindData<List<CarOut>>(Model.token, "tCarOut", where);
            string stayTime = "";
            if (lstCO.Count > 0)
            {
                stayTime = GetStayTime(ci.InTime, lstCO[0].OutTime);
            }

            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic["InGateName"] = ci.InGateName;
            dic["InOperatorCard"] = ci.InOperatorCard;
            string updstr = JsonJoin.ModelToJson(dic0);

            //查询条件不同
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "like", FieldValue = "%" + ci.CPH.Substring(1) + "%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "InTime", Operator = "=", FieldValue = ci.InTime, Combinator = "and" });
            where = JsonJoin.ModelToJson(lstSM);

            int ret = request.UpdateData(Model.token, "tCarOut", updstr, where);
            return ret;
        }

        public int UpdateInOut(CarOut co)
        {
            if (null == co) return 0;
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CPH", Operator = "like", FieldValue = "%" + co.CPH.Substring(1) + "%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "InTime", Operator = "=", FieldValue = co.InTime, Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["SFJE"] = co.SFJE;
            dic["YSJE"] = co.YSJE;
            dic["OutTime"] = co.OutTime;
            dic["OutGateName"] = co.OutGateName;
            dic["OutOperatorCard"] = co.OutOperatorCard;
            dic["OutOperator"] = co.OutOperator;
            string updstr = JsonJoin.ModelToJson(dic);
            int ret = request.UpdateData(Model.token, "tCarOut", updstr, where);
            return ret;
        }

        /// <summary>
        /// 查询出场记录
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public List<CarOut> GetOutGate(Dictionary<string, object> dic)
        {
            dic["CarparkNO"] = Model.iParkingNo;
            string where = JsonJoin.ToJson(dic, false, true);
            List<CarOut> lstCO = new List<CarOut>();
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            //根据ID排序
            dic0["InTime"] = "desc";
            if (where == "")
            {
                lstCO = request.FindData<List<CarOut>>(Model.token, "tCarOut", dic0);
            }
            else
            {
                lstCO = request.FindData<List<CarOut>>(Model.token, "tCarOut", dic0, where);
            }
            return lstCO;
        }

        /// <summary>
        /// 查询出场表受限制数据
        /// </summary>
        /// <returns>卡片编号</returns>
        public List<CarOut> GetCargooutrecord(string cardNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = cardNO;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["OutTime"] = "desc";
            List<CarOut> lstCO = request.FindData<List<CarOut>>(Model.token, "tCarOut", dic0, 1, where);
            return lstCO;
        }

        public List<CarOut> GetOutCardNo2(DateTime InDate, DateTime OutDate, string CardNo, int TTCBianHao, int BigSmall, string SFJE)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["InTime"] = InDate;
            dic["OutTime"] = OutDate;
            dic["CardNO"] = CardNo;
            dic["CarparkNO"] = TTCBianHao;
            dic["BigSmall"] = BigSmall;
            dic["SFJE"] = SFJE;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["OutTime"] = "desc";
            List<CarOut> lstCO = request.FindData<List<CarOut>>(Model.token, "tCarOut", dic0, where);
            return lstCO;
        }

        public int UpdateYSJE(decimal YSJE, string CardNO, int CarparkNO, DateTime inTime, DateTime outTime)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            dic["BigSmall"] = 0;
            dic["CarparkNO"] = CarparkNO;
            dic["InTime"] = inTime;
            dic["OutTime"] = outTime;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["YSJE"] = YSJE;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarOut", updstr, where);
            return ret;
        }

        /// <summary>
        /// 修改免费原因
        /// </summary>
        /// <param name="Free">免费原因</param>
        /// <param name="CardNO">卡片编号</param>
        /// <param name="CarparkNO">车场编号</param>
        /// <param name="inTime">入场时间</param>
        /// <param name="outTime">出场时间</param>
        /// <returns></returns>
        public int UpdateFreeReason(string CardType, string Free, string CardNO, int CarparkNO, DateTime inTime, string File, DateTime outTime)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            dic["BigSmall"] = 0;
            dic["CarparkNO"] = CarparkNO;
            dic["InTime"] = inTime;
            dic["OutTime"] = outTime;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["CardType"] = CardType;
            dic0["SFJE"] = 0;
            dic0["FreeReason"] = Free;
            dic0["ZJPic"] = File;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarOut", updstr, where);
            return ret;
        }

        public long AddOutName(CarOut co, int OnLine)
        {
            if (OnLine == 10)
            {
                co.ID = co.ID | 0x80;
            }
            string addstr = JsonJoin.ModelToJson(co);
            long ret = request.SetCarOut(co);
            return ret;
        }

        public int UpdateCarOut(string CardNO, string path)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["OutPic"] = path;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarOut", updstr, where);
            return ret;
        }

        public List<CarOut> GetWhereCarCharge(List<SelectModel> lstSM)
        {
            //归档表（未处理）
            try
            {
                string where = JsonJoin.ObjectToJson(lstSM);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["OutTime"] = "desc";
                List<CarOut> lstCO = request.FindData<List<CarOut>>(Model.token, "tCarOut", dic, where);
                return lstCO;
            }
            catch
            {
                throw;
            }
        }

        public ReportCar GetCarChargeReport(List<SelectModel> lstSM)
        {
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["OutTime"] = "desc";
            ReportCar rc = request.GetCarOutsummaryInfo<ReportCar>(Model.token, dic, where);
            return rc;
        }

        public void UpdateSFJE(string CardNO, string CardType, DateTime InDateTime, decimal SFJE, DateTime OutDateTime, string OutName, int Flag, string ZjPic, decimal YSJE, string strSFGateName, string strFreeReason, string strPayType)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CardNO"] = CardNO;
            dic["InTime"] = InDateTime;
            dic["OutTime"] = OutDateTime;
            dic["Bigsmall"] = 0;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            if (Flag == 1)
            {
                dic0["CardType"] = CardType;
                dic0["SFJE"] = 0;
                dic0["OutGateName"] = strSFGateName;
                dic0["ZJPic"] = ZjPic;
            }
            else if (Flag == 2)
            {
                dic0["CardType"] = CardType;
                dic0["SFJE"] = SFJE;
                dic0["OutGateName"] = strSFGateName;
            }
            else
            {
                dic0["OutGateName"] = strSFGateName;
            }
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tCarOut", updstr, where);
        }
        #endregion


        #region RawRecord
        /// <summary>
        /// 增加一条历史记录数据
        /// </summary>
        public int AddRecordMemory(RawRecord rr)
        {
            if (null == rr) return 0;
            string addstr = JsonJoin.ModelToJson(rr);
            int ret = request.AddData(Model.token, "tRawRecord", addstr);
            return ret;
        }

        public int UpdateCORDMEMORY(decimal dSFJE, string strCardNO, string strGate, int iFlag, string strY, string sOutTime)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["CardNO"] = strCardNO;
                dic["InGateName"] = strGate;
                dic["OutTime"] = sOutTime;
                string where = JsonJoin.ToJson(dic);
                Dictionary<string, object> dic0 = new Dictionary<string, object>();
                if (iFlag == 0)
                {
                    dic0["YSJE"] = dSFJE;
                }
                else if (iFlag == 1)
                {
                    dic0["Balance"] = dSFJE;
                }
                else
                {
                    dic0["SFJE"] = dSFJE;
                }
                string updstr = JsonJoin.ObjectToJson(dic0);

                int ret = request.UpdateData(Model.token, "tRawRecord", updstr, where);
                return ret;
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region LedSetting
        /// <summary>
        /// 根据条件查询剩余车位屏信息
        /// </summary>
        /// <returns></returns>
        public List<LedSetting> GetSurplusCar(int JiHao)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CtrID"] = JiHao;
            dic["StationID"] = Model.stationID;
            string where = JsonJoin.ToJson(dic);
            List<LedSetting> lstLS = request.FindData<List<LedSetting>>(Model.token, "tLedSetting", where);
            return lstLS;
        }
        #endregion


        #region ParkCPHDiscountSet(根据车牌号打折)
        public List<ParkCPHDiscountSet> GetAutoCPHDZ(Dictionary<string, object> dic)
        {
            string where = JsonJoin.ToJson(dic, false, true);
            List<ParkCPHDiscountSet> lstPCD = request.FindData<List<ParkCPHDiscountSet>>(Model.token, "tParkCPHDiscountSet", where);
            return lstPCD;
        }
        #endregion


        #region ParkDiscountJHSet(根据机号打折)
        public List<ParkDiscountJHSet> GetJiHaoDZ()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic["CtrlNumber"] = "asc";
            List<ParkDiscountJHSet> lstPDS = request.FindData<List<ParkDiscountJHSet>>(Model.token, "tParkDiscountJHSet");
            return lstPDS;
        }

        //优惠机号改为优惠地址查询
        public List<ParkDiscountJHSet> GetJiHaoDZ(string address)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Address"] = address;
            string where = JsonJoin.ToJson(dic);
            //Dictionary<string, string> dic0 = new Dictionary<string, string>();
            //dic0["CtrlNumber"] = "asc";
            List<ParkDiscountJHSet> lstPD = request.FindData<List<ParkDiscountJHSet>>(Model.token, "tParkDiscountJHSet", where);
            return lstPD;
        }
        #endregion


        #region UserInfo
        public List<UserInfo> GetPersonnel()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["UserNo"] = "asc";
            List<UserInfo> lstUI = request.FindData<List<UserInfo>>(Model.token, "tUserInfo", dic);
            return lstUI;
        }

        public List<UserInfo> GetAutoUsernoPersonnel()
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            //sm.Conditions.Add(new SelectModel.conditions { FieldName = "UserNO", Operator = "like", FieldValue = "______", Combinator = "or" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "UserNO", Operator = "like", FieldValue = "A_____%", Combinator = "or" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["UserNo"] = "desc";
            List<UserInfo> lstUI = request.FindData<List<UserInfo>>(Model.token, "tUserInfo", dic0, 1, where, 1);
            return lstUI;
        }

        public int PersonnelAddCpdj(UserInfo ui)
        {
            if (null == ui) return 0;
            int ret;
            if (IsExists(ui.UserNO) > 0)
            {
                ret = UpdateUserInfo(ui);
            }
            else
            {
                string addstr = JsonJoin.ModelToJson(ui);
                ret = request.AddData(Model.token, "tUserInfo", addstr);
            }
            return ret;
        }

        public int IsExists(string UserNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["UserNO"] = UserNO;
            string where = JsonJoin.ToJson(dic);
            List<UserInfo> lstUserInfo = request.FindData<List<UserInfo>>(Model.token, "tUserInfo", where);
            return lstUserInfo == null ? 0 : lstUserInfo.Count;
        }

        public int UpdateUserInfo(UserInfo ui)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["UserNO"] = ui.UserNO ?? "";
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["UserName"] = ui.UserName ?? "";
            dic0["HomeAddress"] = ui.HomeAddress ?? "";
            dic0["IDCard"] = ui.IDCard ?? "";
            dic0["MobNumber"] = ui.MobNumber ?? "";
            dic0["CarPlaceNo"] = ui.CarPlaceNo == 0 ? "1" : ui.CarPlaceNo.ToString();
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tUserInfo", updstr, where);
            return ret;
        }

        public int GetPersonCount(string UserNO)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["UserNO"] = UserNO;
            string where = JsonJoin.ToJson(dic);
            List<UserInfo> lstUI = request.FindData<List<UserInfo>>(Model.token, "tUserInfo", where);

            //if (lstUI.Count > 0) { return Convert.ToInt32(lstUI[0].ZipCode) - 1; }
            //else { return 0; }

            if (null == lstUI || lstUI.Count <= 0 || lstUI[0].CarPlaceNo <= 1)
            {
                return 0;
            }

            return lstUI[0].CarPlaceNo - 1;
        }
        #endregion


        #region 语音设置
        public List<VoiceMultiSet> GetMyVoiceCount(string cardNo)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "CardNO", Operator = "=", FieldValue = cardNo, Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "StartTime", Operator = "<=", FieldValue = DateTime.Now.ToShortDateString(), Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "EndTime", Operator = ">=", FieldValue = DateTime.Now.ToShortDateString(), Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<VoiceMultiSet> lstVMS = request.FindData<List<VoiceMultiSet>>(Model.token, "tVoiceMultiSet", where);
            return lstVMS;
        }

        public List<VoiceSelfDefine> SelectCardNOVoice(string cardNo, int jihao)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["VoiceCardNO"] = cardNo;
            dic["CtrlNumber"] = jihao;
            string where = JsonJoin.ToJson(dic);
            List<VoiceSelfDefine> lstVSD = request.FindData<List<VoiceSelfDefine>>(Model.token, "tVoiceSelfDefine", where);
            return lstVSD;
        }
        #endregion


        #region AutoTempDownLoad
        public int AddTemp(string strCPH, DateTime? dtInTime)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tAutoTempDownLoad", where);
            AutoTempDownLoad atdl = new AutoTempDownLoad();
            atdl.CPH = strCPH;
            atdl.InTime = dtInTime.Value;
            atdl.DownloadSignal = "000000000000000";
            atdl.InOut = 0;
            string addstr = JsonJoin.ModelToJson(atdl);
            ret = request.AddData(Model.token, "tAutoTempDownLoad", addstr);
            return ret;
        }

        public int UpdateTempInOut(string strCPH, int iInOut)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, string> dic0 = new Dictionary<string, string>();
            dic0["InOut"] = iInOut.ToString();
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tAutoTempDownLoad", updstr, where);
            return ret;
        }
        #endregion


        #region Query
        public List<Query> GetSaveQuery(string window, string currentShow)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Window"] = window;
            dic["CurrentShow"] = currentShow;
            string where = JsonJoin.ToJson(dic);
            List<Query> lstQ = request.FindData<List<Query>>(Model.token, "tQuery", where);
            return lstQ;
        }
        #endregion


        #region QueryScheme
        public List<QueryScheme> InParkSelectSchemeInfo(string schName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["SchName"] = schName;
            string where = JsonJoin.ToJson(dic);
            List<QueryScheme> lstQS = request.FindData<List<QueryScheme>>(Model.token, "tQueryScheme", where);
            return lstQS;
        }

        public bool InParkInsertSchem(List<QueryScheme> lstQS)
        {
            if (lstQS == null && lstQS.Count == 0) return false;
            string addstr = JsonJoin.ModelToJson(lstQS);
            int ret = request.AddDataList(Model.token, "tQueryScheme", addstr);
            return ret > 0 ? true : false;
        }

        public bool InParkDeleteScheme(string ScheName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["SchName"] = ScheName;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tQueryScheme", where);
            return ret > 0 ? true : false;
        }

        public List<QueryScheme> InParkSelectSchemeName(string queryTable)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["QueryTable"] = queryTable;
            string where = JsonJoin.ToJson(dic);
            //List<QueryScheme> lstQS = request.FindData<List<QueryScheme>>(Model.token, "tQueryScheme", where);
            List<QueryScheme> lstQS = request.FindDistinctFields<List<QueryScheme>>(Model.token, "tQueryScheme", "SchName", null, where);
            return lstQS;
        }
        #endregion


        #region Money
        public int Add(Money money)
        {
            if (null == money) return 0;
            string addstr = JsonJoin.ModelToJson(money);
            int ret = request.AddData(Model.token, "tMoney", addstr);
            return ret;
        }
        #endregion


        #region ParkJHSet
        public List<ParkJHSet> GetCCJiHao()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["CtrlNumber"] = "asc";
            List<ParkJHSet> lstPJS = request.FindData<List<ParkJHSet>>(Model.token, "tParkJHSet", dic);
            return lstPJS;
        }
        #endregion


        #region 取消收费(将出场记录重写到入场记录)
        public long RstInGateRetrography(dynamic dy)
        {
            long ret = request.MoveCarOutToCarIn(null, dy);
            return ret;
        }
        #endregion


        #region 计算停车时间
        /// <summary>
        /// 返回停车时间
        /// </summary>
        /// <param name="dInTime"></param>
        /// <param name="dOutTime"></param>
        /// <returns></returns>
        private string GetStayTime(DateTime? dInTime, DateTime? dOutTime)
        {
            string sInTime = "";
            string sOutTime = "";
            string sRet = "";
            try
            {
                if (dOutTime == null)
                {
                    dOutTime = DateTime.Now;
                }

                sInTime = (dInTime ?? dOutTime ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:00");
                sOutTime = (dOutTime ?? dInTime ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:00");

                TimeSpan stime = Convert.ToDateTime(sOutTime) - Convert.ToDateTime(sInTime);

                if (stime.TotalMinutes < 1)
                {
                    return "0分钟";
                }

                if (stime.Days > 0)
                {
                    sRet = stime.Days + "天";
                }

                if (stime.Hours > 0)
                {
                    sRet += stime.Hours + "小时";
                }

                if (stime.Minutes > 0)
                {
                    sRet += stime.Minutes + "分钟";
                }
                return sRet;
            }
            catch
            {
                return "0";
            }
        }
        #endregion


        #region 查询表中不重复字段
        public List<Dictionary<string, object>> GetQueryValue(string tableName, string field, string date)
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions { FieldName = "InTime", Operator = ">=", FieldValue = date, Combinator = "and" });
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic[field] = "asc";
            List<Dictionary<string, object>> dic0 = request.FindDistinctFields<List<Dictionary<string, object>>>(Model.token, tableName, field, dic, where);
            //List<Dictionary<string, object>> dic0 = request.FindData<List<Dictionary<string, object>>>(Model.token, tableName, dic, 50, where);
            return dic0;
        }
        #endregion


        #region 计算收费
        /// <summary>
        /// 处理收费标准
        /// </summary>
        /// <param name="CardType"></param>
        /// <param name="InTime"></param>
        /// <param name="OutTime"></param>
        /// <returns></returns>
        public CaclMoneyResult GetMONEY(string CardType, DateTime InTime, DateTime OutTime, string CPH = null)
        {
            CaclMoneyResult dRst = null;
            if (Model.iChargeType == 3)  //北京收费标准处理
            {

            }
            else if (Model.iChargeType == 4) //广州收费标准处理
            {

            }
            else
            {
                dRst = request.CaclMoney(CardType, InTime, OutTime, CPH);
            }
            Model.TbBaoJia = true;
            return dRst;
        }

        public CaclMoneyResult GetMoney(string cardType, DateTime InTime, DateTime OutTime, string CPH = null)
        {
            CaclMoneyResult money = request.CaclMoney(cardType, InTime, OutTime, CPH);
            return money;
        }
        #endregion


        #region 换班
        public int SetHandover(string beforeNo, string afterNo, DateTime beforeTime)
        {
            int ret = request.SetHandover(Model.token, Model.iParkingNo, beforeNo, afterNo, beforeTime.ToString("yyyyMMddHHmmss"), DateTime.Now.ToString("yyyyMMddHHmmss"));
            return ret;
        }

        public Handover GetHuanBan(string jiaoWorkCard, string jieWorkCard, DateTime jiaoTime, DateTime jieTime)
        {
            Handover HD = request.GetHandoverPrint<Handover>(Model.token, Model.iParkingNo, jiaoWorkCard, jiaoTime.ToString("yyyyMMddHHmmss"), jieTime.ToString("yyyyMMddHHmmss"),jieWorkCard);
            return HD;
        }
        #endregion


        #region 获取系统时间
        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        public string GetSysTime()
        {
            DateTime serverTime = request.GetServerTime(Model.token);
            string SysTime = serverTime.ToString("yyyyMMddHHmmss");
            return SysTime;
        }
        #endregion


        #region 上传和下载图片
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url">下载URL(由上传图片提供)</param>
        /// <param name="path">文件保存路径</param>
        /// <returns></returns>
        public bool DownLoadPic(string url, string path)
        {
            return request.DownLoadFile(Model.token, url, path);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns>网络路径(为下载图片提供)</returns>
        public string UpLoadPic(string path)
        {
            string[] str = request.SendFiles(Model.token, Model.stationID, path);
            if (str != null)
                return str[0];
            else
                return "";
        }
        #endregion


        #region 无token验证
        public List<Operators> GetOperatorsNoToken()
        {
            List<Operators> lstOT = request.GetOperatorsWithoutLogin<List<Operators>>();
            return lstOT;
        }

        public List<StationSet> GetStationDefNoToken()
        {
            List<StationSet> lstSD = request.GetStationWithoutLogin<List<StationSet>>();
            return lstSD;
        }
        #endregion


        #region Rights(权限分配)
        public List<Rights> GetRights(int groupNo, string fromName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["FormName"] = fromName;
            dic["GroupNo"] = groupNo;
            string where = JsonJoin.ToJson(dic);

            List<Rights> lstRI = request.FindData<List<Rights>>(Model.token, "tRights", where);
            return lstRI;
        }

        public List<Rights> GetRights(int groupNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["GroupNo"] = groupNo;
            string where = JsonJoin.ToJson(dic);

            List<Rights> lstRI = request.GetRights<List<Rights>>(Model.token, groupNo);
            return lstRI;
        }


        public bool SetRightsItem(List<RightsItem> lstRI)
        {
            if (lstRI.Count == 0 || lstRI == null)
                return false;
            string addstr = JsonJoin.ObjectToJson(lstRI);
            int ret = request.AddDataList(Model.token, "tRightsItem", addstr);
            return ret > 0 ? true : false;
        }

        public List<RightsItem> GetRightsItem(string des)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Description"] = des;
            string where = JsonJoin.ToJson(dic);
            List<RightsItem> lstRI = request.FindData<List<RightsItem>>(Model.token, "tRightsItem", where);
            return lstRI;
        }

        public List<RightsItem> GetRightsItem(string fromName, string itemName, string category = "车场")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["FormName"] = fromName;
            dic["Description"] = itemName;
            dic["Category"] = category;
            string where = JsonJoin.ToJson(dic);
            List<RightsItem> lstRI = request.FindData<List<RightsItem>>(Model.token, "tRightsItem", where);
            return lstRI;
        }

        public List<Rights> GetRightsByName(string fromName, string itemName)
        {
            var resultList = from item in Model.lstRights
                             where item.ItemName == itemName && item.FormName == fromName
                             select item;
            var escortList = resultList.ToList();
            return escortList;
        }

        public long GetIDByName(string fromName, string itemName)
        {
            var resultList = from item in Model.lstRights
                             where item.ItemName == itemName && item.FormName == fromName
                             select item;
            var escortList = resultList.ToList();
            if (escortList.Count > 0)
            {
                return escortList[0].RightsItemID;
            }
            else
            {
                return 0;
            }
        }
        #endregion


        #region BillPrintSet(打印设置)
        public List<BillPrintSet> GetPrint()
        {
            List<BillPrintSet> lstBPS = request.FindData<List<BillPrintSet>>(Model.token, "tBillPrintSet");
            return lstBPS;
        }
        #endregion


        #region 节假日设置
        public List<Holiday> GetHoliday(string strType)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Types"] = strType;
            string where = JsonJoin.ToJson(dic);
            List<Holiday> lstHd = request.FindData<List<Holiday>>(Model.token, "tHoliday", where);
            return lstHd;
        }

        public int DeleteHoliday(DateTime dtHoliday, string strType)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Dates"] = dtHoliday;
            dic["Types"] = strType;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tHoliday", where);
            return ret;
        }

        public int AddHoliday(DateTime dtHoliday, string strType)
        {
            Holiday hi = new Holiday();
            hi.Dates = dtHoliday;
            hi.Types = strType;
            string addstr = JsonJoin.ObjectToJson(hi);
            int ret = request.AddData(Model.token, "tHoliday", addstr);
            return ret;
        }
        #endregion


        #region 临时车牌下载
        public List<AutoTempDownLoad> GetTempDownLoad(int workStationNo, int inout, int flag)
        {
            string tmpstr = "";
            for (int i = 1; i < workStationNo; i++)
            {
                tmpstr += "_";
            }
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Conditions.Add(new SelectModel.conditions() { FieldName = "DownloadSignal", Operator = "like", FieldValue = tmpstr + "1%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions() { FieldName = "InOut", Operator = "=", FieldValue = inout, Combinator = "and" });
            sm.Combinator = "and";
            lstSM.Add(sm);
            string where = JsonJoin.ModelToJson(lstSM);
            List<AutoTempDownLoad> lstATD = request.FindData<List<AutoTempDownLoad>>(Model.token, "tAutoTempDownLoad", where);
            return lstATD;
        }

        public int DeleteTemp(string strCPH)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            string where = JsonJoin.ToJson(dic);
            int ret = request.DeleteDataBy(Model.token, "tAutoTempDownLoad", where);
            return ret;
        }

        public int UpdateTempDownLoad(string strCPH, string Down)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = strCPH;
            string where = JsonJoin.ToJson(dic);
            Dictionary<string, object> dic0 = new Dictionary<string, object>();
            dic0["DownloadSignal"] = Down;
            string updstr = JsonJoin.ModelToJson(dic0);
            int ret = request.UpdateData(Model.token, "tAutoTempDownLoad", updstr, where);
            return ret;

        }

        public void GetDeleteTemp()
        {
            List<SelectModel> lstSM = new List<SelectModel>();
            SelectModel sm = new SelectModel();
            sm.Combinator = "and";
            sm.Conditions.Add(new SelectModel.conditions() { FieldName = "CarCardType", Operator = "like", FieldValue = "Mth%", Combinator = "and" });
            sm.Conditions.Add(new SelectModel.conditions() { FieldName = "CardState", Operator = "=", FieldValue = 0, Combinator = "and" });
            lstSM.Add(sm);
            string where0 = JsonJoin.ModelToJson(lstSM);
            List<CardIssue> lstCI = request.FindData<List<CardIssue>>(Model.token, "tCardIssue", where0);
            int iFXCount = lstCI.Count;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["InOut"] = 1;
            string where1 = JsonJoin.ToJson(dic);
            List<AutoTempDownLoad> lstATD = request.FindData<List<AutoTempDownLoad>>(Model.token, "tAutoTempDownLoad", where1);
            int iAutoCount = lstATD.Count;

            int iSum = iAutoCount - (10000 - iFXCount);
            if (iSum > 0)
            {
                Dictionary<string, object> dic1 = new Dictionary<string, object>();
                dic1["InOut"] = 0;
                string where2 = JsonJoin.ToJson(dic1);
                Dictionary<string, string> dic2 = new Dictionary<string, string>();
                dic2["InTime"] = "asc";
                List<AutoTempDownLoad> lstATD1 = request.FindData<List<AutoTempDownLoad>>(Model.token, "tAutoTempDownLoad", dic2, iSum, where2);
                if (lstATD1.Count > 0)
                {
                    for (int i = 0; i < lstATD1.Count; i++)
                    {
                        lstATD1[0].InOut = 1;
                    }
                    string updstr = JsonJoin.ModelToJson(lstATD1);
                    request.UpdateDataList(Model.token, "tAutoTempDownLoad", updstr);
                }
            }
        }
        #endregion


        #region 最高收费限额(0-0 0-24)
        public decimal GetDayMoneyLimit(string strCardNO, string strCardType, string strCPH, DateTime InTime, DateTime OutTime, int iPointNum, decimal dMoney, int iType, int iIndex)
        {
            decimal dRst = 0;
            if (strCPH != "" && strCPH != "66666666" && strCPH != "00000000" && strCPH != "88888888")
            {
                dRst = request.CaclDayMoneyLimit(Model.token, Model.iParkingNo, "", strCardType, strCPH, InTime, OutTime, iPointNum.ToString(), dMoney, Model.iZGXEType);
                Model.TbBaoJia = true;
            }
            else
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["CardNO"] = strCardNO;
                dic["CarparkNO"] = Model.iParkingNo;
                dic["BigSmall"] = Model.Channels[iIndex].iBigSmall;
                string where = JsonJoin.ToJson(dic);
                Dictionary<string, string> dic0 = new Dictionary<string, string>();
                dic0["InTime"] = "desc";
                List<CarIn> lstCI = request.FindData<List<CarIn>>(Model.token, "tCarIn", dic0, where);
                if (lstCI.Count > 0)
                {
                    strCPH = lstCI[0].CPH;
                    if (strCPH != "" && strCPH != "66666666" && strCPH != "00000000" && strCPH != "88888888")
                    {
                        dRst = request.CaclDayMoneyLimit(Model.token, Model.iParkingNo, "", strCardType, strCPH, InTime, OutTime, iPointNum.ToString(), dMoney, Model.iZGXEType);
                        Model.TbBaoJia = true;
                    }
                }
            }
            return dRst;
        }

        #endregion


        #region tSpecialCPH( 查询特殊车牌)
        public string SearchSpecialCPH(string sCPH)
        {
            string sRetCPH = ""; //返回车牌
            int iJingDu = 1;     //查询精度
            string sMode = "";   //如何处理

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["CPH"] = sCPH;
            dic["Type"] = 1;
            string where = JsonJoin.ToJson(dic);
            List<SpecialCPH> lstSC = request.FindData<List<SpecialCPH>>(Model.token, "tSpecialCPH", where);
            if (lstSC.Count > 0)
            {
                sMode = lstSC[0].Mode;
                iJingDu = Convert.ToInt32(sMode) + 4;
                List<CardIssue> lstCI = SelectFxCPH(sCPH, iJingDu, "Mth", "Fre", "Str");
                if (lstCI.Count > 0)
                {
                    sRetCPH = lstCI[0].CPH;
                }
                else
                {
                    List<CarIn> lstCI0 = SelectComeCPH(sCPH, 6, "Tmp", "Tmp");
                    if (lstCI0.Count > 0)
                    {
                        sRetCPH = lstCI0[0].CPH;
                    }
                }
            }
            else
            {
                string sDbCPH = "";
                Dictionary<string, object> dic0 = new Dictionary<string, object>();
                dic0["Type"] = 2;
                string where0 = JsonJoin.ToJson(dic0);
                lstSC = request.FindData<List<SpecialCPH>>(Model.token, "tSpecialCPH", where0);
                if (lstSC.Count > 0)
                {
                    for (int i = 0; i < lstSC.Count; i++)
                    {
                        sDbCPH = lstSC[i].CPH;
                        if (sCPH.Contains(sDbCPH))
                        {
                            sRetCPH = lstSC[i].Mode;
                            break;
                        }
                    }
                }
            }
            return sRetCPH;
        }
        #endregion


        public bool UploadOnlinePayCredentialFile(string fileName)
        {
            return request.UploadOnlinePayCredentialFile(Model.token, fileName);
        }
    }
}
