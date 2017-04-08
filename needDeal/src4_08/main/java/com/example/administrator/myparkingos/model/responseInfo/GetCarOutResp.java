package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-04-05.
 */
public class GetCarOutResp
{
    private String rcode;  // 参考错误码列表
    private String msg;  //错误信息
    private int PageIndex;  //当前页码。仅当查询时指定了分页参数才有此值。
    private int PageSize;  // 分页大小。仅当查询时指定了分页参数才有此值。
    private int TotalRows;  //总行数。仅当查询时指定了分页参数才有此值。
    //Data  参考说明中的描述    如果没有指定ExportFields参数则为数据Model数组，否则为下载导出文件的完整URL

    private List<DataBean> data;

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetCarOutResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", PageIndex=").append(PageIndex);
        sb.append(", PageSize=").append(PageSize);
        sb.append(", TotalRows=").append(TotalRows);
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }

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

    public int getPageIndex()
    {
        return PageIndex;
    }

    public void setPageIndex(int pageIndex)
    {
        PageIndex = pageIndex;
    }

    public int getPageSize()
    {
        return PageSize;
    }

    public void setPageSize(int pageSize)
    {
        PageSize = pageSize;
    }

    public int getTotalRows()
    {
        return TotalRows;
    }

    public void setTotalRows(int totalRows)
    {
        TotalRows = totalRows;
    }

    public List<DataBean> getData()
    {
        return data;
    }

    public void setData(List<DataBean> data)
    {
        this.data = data;
    }

    public static class DataBean
    {
        private long ID;  // Y  自增长唯一标识。新增时复用为在线状态及收费标识：最低7位为收费标识，第8位表示是否是脱机记录(1为脱机记录)
        private long InID;  // N  入场ID
        private String CardNO;  // Y  卡号
        private String CPH;  // N  车牌
        private byte CPColor;  // N  车牌颜色。0为蓝色，1为黄色，2为白色，3为黑色，4为未识别
        private String CardType;  // N  卡类
        private String InTime;  // Y  入场时间
        private String OutTime;  // Y  出场时间
        private String InGateName;  // N  入场名称
        private String OutGateName;  // N  出场名称
        private String InOperatorCard;  // N  入场操作员卡号
        private String OutOperatorCard;  // N  出场操作员卡号
        private String InOperator;  // N  入场操作员
        private String OutOperator;  // N  出场操作员
        private String InPic;  // N  入场车辆图片
        private String InUser;  // N  入场人相图片
        private String OutPic;  // N  出场车辆图片
        private String OutUser;  // N  出场人相图片
        private String ZJPic;  // N  证件图片
        private double SFJE;  // N  收费金额
        private double Balance;  // N  余额
        private double YSJE;  // N  应收金额
        private String SFTime;  // N  中央收费时间
        private String SFOperator;  // N  中央收费操作员
        private String SFOperatorCard;  // N  中央收费操作员卡号
        private String SFGate;  // N  中央收费口名
        private String OvertimeSymbol;  // N  超时标志
        private String OvertimeSFTime;  // N  超时时间
        private double OvertimeSFJE;  // N  超时金额
        private int CarparkNO;  // N  车场编号
        private int BigSmall;  // N  大小标志
        private String FreeReason;  // N  免费原因
        private String StayTime;  // N  停车时间
        private int StationID;  // N
        private String YHAddress;  // N  优惠场所
        private String YHType;  // N  优惠方式
        private double YHJE;  // N  优惠金额
        private String Temp1;  // N  备用
        private String Temp2;  // N  备用
        private String Temp3;  // N  备用
        private String Temp4;  // N  备用
        private String Temp5;  // N  备用
        private int PayType;  // N  支付方式。0 现金；1 微信；2 支付宝
        private String PayTypeCaption;  // N  中文支付方式。
        private String UserNO;  // N  参考人员Model结构同名字段
        private String UserName;  // N  参考人员Model结构同名字段
        private String DeptName;  // N  参考人员Model结构同名字段
        private String ChineseName;  // N  参考卡类型定义Model的CardType字段
        private String StationName;  // N  参考工作站设置Model同名字段
        private String CarparkNOInStation;  // N  参考工作站设置Model CarparkNO字段
        private int OnlineState_In;  // N  入场记录在线状态。0或空为在线记录，1为在线获取到的离线记录，2为通过U盘获取到的离线记录
        private int OnlineState_Out;  // N  出场记录在线状态。0或空为在线记录，1为在线获取到的离线记录，2为通过U盘获取到的离线记录
        private boolean IsFreeOut;  // N  是否是免费出场记录
        private boolean IsCentralCharge;  // N  是否是中央收费记录
        private boolean IsAsMtp;  // N  是否是月卡按临时卡计费记录
        private boolean IsUnlicensed;  // N  是否是无牌车记录
        private int AsMtpType;  // N  月卡按临时卡计费的原因。0为多车位多车按临时车计费；1为过期按临时车计费；2为按规则参数中指定的临时车计费。
        private String CarColor;  // N  车辆颜色
        private String CarBrand;  // N  车辆品牌

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("ID=").append(ID);
            sb.append(", InID=").append(InID);
            sb.append(", CardNO='").append(CardNO).append('\'');
            sb.append(", CPH='").append(CPH).append('\'');
            sb.append(", CPColor=").append(CPColor);
            sb.append(", CardType='").append(CardType).append('\'');
            sb.append(", InTime='").append(InTime).append('\'');
            sb.append(", OutTime='").append(OutTime).append('\'');
            sb.append(", InGateName='").append(InGateName).append('\'');
            sb.append(", OutGateName='").append(OutGateName).append('\'');
            sb.append(", InOperatorCard='").append(InOperatorCard).append('\'');
            sb.append(", OutOperatorCard='").append(OutOperatorCard).append('\'');
            sb.append(", InOperator='").append(InOperator).append('\'');
            sb.append(", OutOperator='").append(OutOperator).append('\'');
            sb.append(", InPic='").append(InPic).append('\'');
            sb.append(", InUser='").append(InUser).append('\'');
            sb.append(", OutPic='").append(OutPic).append('\'');
            sb.append(", OutUser='").append(OutUser).append('\'');
            sb.append(", ZJPic='").append(ZJPic).append('\'');
            sb.append(", SFJE=").append(SFJE);
            sb.append(", Balance=").append(Balance);
            sb.append(", YSJE=").append(YSJE);
            sb.append(", SFTime='").append(SFTime).append('\'');
            sb.append(", SFOperator='").append(SFOperator).append('\'');
            sb.append(", SFOperatorCard='").append(SFOperatorCard).append('\'');
            sb.append(", SFGate='").append(SFGate).append('\'');
            sb.append(", OvertimeSymbol='").append(OvertimeSymbol).append('\'');
            sb.append(", OvertimeSFTime='").append(OvertimeSFTime).append('\'');
            sb.append(", OvertimeSFJE=").append(OvertimeSFJE);
            sb.append(", CarparkNO=").append(CarparkNO);
            sb.append(", BigSmall=").append(BigSmall);
            sb.append(", FreeReason='").append(FreeReason).append('\'');
            sb.append(", StayTime='").append(StayTime).append('\'');
            sb.append(", StationID=").append(StationID);
            sb.append(", YHAddress='").append(YHAddress).append('\'');
            sb.append(", YHType='").append(YHType).append('\'');
            sb.append(", YHJE=").append(YHJE);
            sb.append(", Temp1='").append(Temp1).append('\'');
            sb.append(", Temp2='").append(Temp2).append('\'');
            sb.append(", Temp3='").append(Temp3).append('\'');
            sb.append(", Temp4='").append(Temp4).append('\'');
            sb.append(", Temp5='").append(Temp5).append('\'');
            sb.append(", PayType=").append(PayType);
            sb.append(", PayTypeCaption='").append(PayTypeCaption).append('\'');
            sb.append(", UserNO='").append(UserNO).append('\'');
            sb.append(", UserName='").append(UserName).append('\'');
            sb.append(", DeptName='").append(DeptName).append('\'');
            sb.append(", ChineseName='").append(ChineseName).append('\'');
            sb.append(", StationName='").append(StationName).append('\'');
            sb.append(", CarparkNOInStation='").append(CarparkNOInStation).append('\'');
            sb.append(", OnlineState_In=").append(OnlineState_In);
            sb.append(", OnlineState_Out=").append(OnlineState_Out);
            sb.append(", IsFreeOut=").append(IsFreeOut);
            sb.append(", IsCentralCharge=").append(IsCentralCharge);
            sb.append(", IsAsMtp=").append(IsAsMtp);
            sb.append(", IsUnlicensed=").append(IsUnlicensed);
            sb.append(", AsMtpType=").append(AsMtpType);
            sb.append(", CarColor='").append(CarColor).append('\'');
            sb.append(", CarBrand='").append(CarBrand).append('\'');
            sb.append('}');
            return sb.toString();
        }

        public long getID()
        {
            return ID;
        }

        public void setID(long ID)
        {
            this.ID = ID;
        }

        public long getInID()
        {
            return InID;
        }

        public void setInID(long inID)
        {
            InID = inID;
        }

        public String getCardNO()
        {
            return CardNO;
        }

        public void setCardNO(String cardNO)
        {
            CardNO = cardNO;
        }

        public String getCPH()
        {
            return CPH;
        }

        public void setCPH(String CPH)
        {
            this.CPH = CPH;
        }

        public byte getCPColor()
        {
            return CPColor;
        }

        public void setCPColor(byte CPColor)
        {
            this.CPColor = CPColor;
        }

        public String getCardType()
        {
            return CardType;
        }

        public void setCardType(String cardType)
        {
            CardType = cardType;
        }

        public String getInTime()
        {
            return InTime;
        }

        public void setInTime(String inTime)
        {
            InTime = inTime;
        }

        public String getOutTime()
        {
            return OutTime;
        }

        public void setOutTime(String outTime)
        {
            OutTime = outTime;
        }

        public String getInGateName()
        {
            return InGateName;
        }

        public void setInGateName(String inGateName)
        {
            InGateName = inGateName;
        }

        public String getOutGateName()
        {
            return OutGateName;
        }

        public void setOutGateName(String outGateName)
        {
            OutGateName = outGateName;
        }

        public String getInOperatorCard()
        {
            return InOperatorCard;
        }

        public void setInOperatorCard(String inOperatorCard)
        {
            InOperatorCard = inOperatorCard;
        }

        public String getOutOperatorCard()
        {
            return OutOperatorCard;
        }

        public void setOutOperatorCard(String outOperatorCard)
        {
            OutOperatorCard = outOperatorCard;
        }

        public String getInOperator()
        {
            return InOperator;
        }

        public void setInOperator(String inOperator)
        {
            InOperator = inOperator;
        }

        public String getOutOperator()
        {
            return OutOperator;
        }

        public void setOutOperator(String outOperator)
        {
            OutOperator = outOperator;
        }

        public String getInPic()
        {
            return InPic;
        }

        public void setInPic(String inPic)
        {
            InPic = inPic;
        }

        public String getInUser()
        {
            return InUser;
        }

        public void setInUser(String inUser)
        {
            InUser = inUser;
        }

        public String getOutPic()
        {
            return OutPic;
        }

        public void setOutPic(String outPic)
        {
            OutPic = outPic;
        }

        public String getOutUser()
        {
            return OutUser;
        }

        public void setOutUser(String outUser)
        {
            OutUser = outUser;
        }

        public String getZJPic()
        {
            return ZJPic;
        }

        public void setZJPic(String ZJPic)
        {
            this.ZJPic = ZJPic;
        }

        public double getSFJE()
        {
            return SFJE;
        }

        public void setSFJE(double SFJE)
        {
            this.SFJE = SFJE;
        }

        public double getBalance()
        {
            return Balance;
        }

        public void setBalance(double balance)
        {
            Balance = balance;
        }

        public double getYSJE()
        {
            return YSJE;
        }

        public void setYSJE(double YSJE)
        {
            this.YSJE = YSJE;
        }

        public String getSFTime()
        {
            return SFTime;
        }

        public void setSFTime(String SFTime)
        {
            this.SFTime = SFTime;
        }

        public String getSFOperator()
        {
            return SFOperator;
        }

        public void setSFOperator(String SFOperator)
        {
            this.SFOperator = SFOperator;
        }

        public String getSFOperatorCard()
        {
            return SFOperatorCard;
        }

        public void setSFOperatorCard(String SFOperatorCard)
        {
            this.SFOperatorCard = SFOperatorCard;
        }

        public String getSFGate()
        {
            return SFGate;
        }

        public void setSFGate(String SFGate)
        {
            this.SFGate = SFGate;
        }

        public String getOvertimeSymbol()
        {
            return OvertimeSymbol;
        }

        public void setOvertimeSymbol(String overtimeSymbol)
        {
            OvertimeSymbol = overtimeSymbol;
        }

        public String getOvertimeSFTime()
        {
            return OvertimeSFTime;
        }

        public void setOvertimeSFTime(String overtimeSFTime)
        {
            OvertimeSFTime = overtimeSFTime;
        }

        public double getOvertimeSFJE()
        {
            return OvertimeSFJE;
        }

        public void setOvertimeSFJE(double overtimeSFJE)
        {
            OvertimeSFJE = overtimeSFJE;
        }

        public int getCarparkNO()
        {
            return CarparkNO;
        }

        public void setCarparkNO(int carparkNO)
        {
            CarparkNO = carparkNO;
        }

        public int getBigSmall()
        {
            return BigSmall;
        }

        public void setBigSmall(int bigSmall)
        {
            BigSmall = bigSmall;
        }

        public String getFreeReason()
        {
            return FreeReason;
        }

        public void setFreeReason(String freeReason)
        {
            FreeReason = freeReason;
        }

        public String getStayTime()
        {
            return StayTime;
        }

        public void setStayTime(String stayTime)
        {
            StayTime = stayTime;
        }

        public int getStationID()
        {
            return StationID;
        }

        public void setStationID(int stationID)
        {
            StationID = stationID;
        }

        public String getYHAddress()
        {
            return YHAddress;
        }

        public void setYHAddress(String YHAddress)
        {
            this.YHAddress = YHAddress;
        }

        public String getYHType()
        {
            return YHType;
        }

        public void setYHType(String YHType)
        {
            this.YHType = YHType;
        }

        public double getYHJE()
        {
            return YHJE;
        }

        public void setYHJE(double YHJE)
        {
            this.YHJE = YHJE;
        }

        public String getTemp1()
        {
            return Temp1;
        }

        public void setTemp1(String temp1)
        {
            Temp1 = temp1;
        }

        public String getTemp2()
        {
            return Temp2;
        }

        public void setTemp2(String temp2)
        {
            Temp2 = temp2;
        }

        public String getTemp3()
        {
            return Temp3;
        }

        public void setTemp3(String temp3)
        {
            Temp3 = temp3;
        }

        public String getTemp4()
        {
            return Temp4;
        }

        public void setTemp4(String temp4)
        {
            Temp4 = temp4;
        }

        public String getTemp5()
        {
            return Temp5;
        }

        public void setTemp5(String temp5)
        {
            Temp5 = temp5;
        }

        public int getPayType()
        {
            return PayType;
        }

        public void setPayType(int payType)
        {
            PayType = payType;
        }

        public String getPayTypeCaption()
        {
            return PayTypeCaption;
        }

        public void setPayTypeCaption(String payTypeCaption)
        {
            PayTypeCaption = payTypeCaption;
        }

        public String getUserNO()
        {
            return UserNO;
        }

        public void setUserNO(String userNO)
        {
            UserNO = userNO;
        }

        public String getUserName()
        {
            return UserName;
        }

        public void setUserName(String userName)
        {
            UserName = userName;
        }

        public String getDeptName()
        {
            return DeptName;
        }

        public void setDeptName(String deptName)
        {
            DeptName = deptName;
        }

        public String getChineseName()
        {
            return ChineseName;
        }

        public void setChineseName(String chineseName)
        {
            ChineseName = chineseName;
        }

        public String getStationName()
        {
            return StationName;
        }

        public void setStationName(String stationName)
        {
            StationName = stationName;
        }

        public String getCarparkNOInStation()
        {
            return CarparkNOInStation;
        }

        public void setCarparkNOInStation(String carparkNOInStation)
        {
            CarparkNOInStation = carparkNOInStation;
        }

        public int getOnlineState_In()
        {
            return OnlineState_In;
        }

        public void setOnlineState_In(int onlineState_In)
        {
            OnlineState_In = onlineState_In;
        }

        public int getOnlineState_Out()
        {
            return OnlineState_Out;
        }

        public void setOnlineState_Out(int onlineState_Out)
        {
            OnlineState_Out = onlineState_Out;
        }

        public boolean isFreeOut()
        {
            return IsFreeOut;
        }

        public void setFreeOut(boolean freeOut)
        {
            IsFreeOut = freeOut;
        }

        public boolean isCentralCharge()
        {
            return IsCentralCharge;
        }

        public void setCentralCharge(boolean centralCharge)
        {
            IsCentralCharge = centralCharge;
        }

        public boolean isAsMtp()
        {
            return IsAsMtp;
        }

        public void setAsMtp(boolean asMtp)
        {
            IsAsMtp = asMtp;
        }

        public boolean isUnlicensed()
        {
            return IsUnlicensed;
        }

        public void setUnlicensed(boolean unlicensed)
        {
            IsUnlicensed = unlicensed;
        }

        public int getAsMtpType()
        {
            return AsMtpType;
        }

        public void setAsMtpType(int asMtpType)
        {
            AsMtpType = asMtpType;
        }

        public String getCarColor()
        {
            return CarColor;
        }

        public void setCarColor(String carColor)
        {
            CarColor = carColor;
        }

        public String getCarBrand()
        {
            return CarBrand;
        }

        public void setCarBrand(String carBrand)
        {
            CarBrand = carBrand;
        }
    }
}
