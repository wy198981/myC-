package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-03-29.
 */
public class GetCardIssueResp
{
    private String rcode;      // Y 参考错误码列表
    private String msg;        // Y 错误信息
    private int PageIndex; // N 当前页码。仅当查询时指定了分页参数才有此值。
    private int PageSize;  // N 分页大小。仅当查询时指定了分页参数才有此值。
    private int TotalRows; // N 总行数。仅当查询时指定了分页参数才有此值。
    //Data	参考说明中的描述	N	如果没有指定ExportFields参数则为数据Model数组，否则为下载导出文件的完整URL
    private List<DataBean> data;

//    private DataBean data;

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
        private String UserName;
        private String HomeAddress;
        private int DeptId;
        private String WorkTime;
        private String BirthDate;
        private String MobNumber;
        private int CarPlaceNo;
        private String CardType;
        private String CardStateCaption;
        private double SFJE;
        private int RemainderDays;
        private String CardNO;
        private String UserNO;
        private String CardState;
        private double CardYJ;
        private String SubSystem;
        private String CarCardType;
        private String CarIssueDate;
        private String CarIssueUserCard;
        private double Balance;
        private String CarValidStartDate;
        private String CarValidEndDate;
        private String CPH;
        private String CarType;
        private String CarPlace;
        private String CarValidMachine;
        private String CarValidZone;
        private String CarMemo;
        private String IssueDate;
        private String IssueUserCard;
        private String DownloadSignal;
        private int Tempnumber;
        private String TimeTeam;
        private String HolidayLimited;
        private String UserInfo;
        private String CPHDownloadSignal;
        private int MonthType;
        private int ID;

        public String getUserName()
        {
            return UserName;
        }

        public void setUserName(String userName)
        {
            UserName = userName;
        }

        public String getHomeAddress()
        {
            return HomeAddress;
        }

        public void setHomeAddress(String homeAddress)
        {
            HomeAddress = homeAddress;
        }

        public int getDeptId()
        {
            return DeptId;
        }

        public void setDeptId(int deptId)
        {
            DeptId = deptId;
        }

        public String getWorkTime()
        {
            return WorkTime;
        }

        public void setWorkTime(String workTime)
        {
            WorkTime = workTime;
        }

        public String getBirthDate()
        {
            return BirthDate;
        }

        public void setBirthDate(String birthDate)
        {
            BirthDate = birthDate;
        }

        public String getMobNumber()
        {
            return MobNumber;
        }

        public void setMobNumber(String mobNumber)
        {
            MobNumber = mobNumber;
        }

        public int getCarPlaceNo()
        {
            return CarPlaceNo;
        }

        public void setCarPlaceNo(int carPlaceNo)
        {
            CarPlaceNo = carPlaceNo;
        }

        public String getCardType()
        {
            return CardType;
        }

        public void setCardType(String cardType)
        {
            CardType = cardType;
        }

        public String getCardStateCaption()
        {
            return CardStateCaption;
        }

        public void setCardStateCaption(String cardStateCaption)
        {
            CardStateCaption = cardStateCaption;
        }

        public double getSFJE()
        {
            return SFJE;
        }

        public void setSFJE(double SFJE)
        {
            this.SFJE = SFJE;
        }

        public int getRemainderDays()
        {
            return RemainderDays;
        }

        public void setRemainderDays(int remainderDays)
        {
            RemainderDays = remainderDays;
        }

        public String getCardNO()
        {
            return CardNO;
        }

        public void setCardNO(String cardNO)
        {
            CardNO = cardNO;
        }

        public String getUserNO()
        {
            return UserNO;
        }

        public void setUserNO(String userNO)
        {
            UserNO = userNO;
        }

        public String getCardState()
        {
            return CardState;
        }

        public void setCardState(String cardState)
        {
            CardState = cardState;
        }

        public double getCardYJ()
        {
            return CardYJ;
        }

        public void setCardYJ(double cardYJ)
        {
            CardYJ = cardYJ;
        }

        public String getSubSystem()
        {
            return SubSystem;
        }

        public void setSubSystem(String subSystem)
        {
            SubSystem = subSystem;
        }

        public String getCarCardType()
        {
            return CarCardType;
        }

        public void setCarCardType(String carCardType)
        {
            CarCardType = carCardType;
        }

        public String getCarIssueDate()
        {
            return CarIssueDate;
        }

        public void setCarIssueDate(String carIssueDate)
        {
            CarIssueDate = carIssueDate;
        }

        public String getCarIssueUserCard()
        {
            return CarIssueUserCard;
        }

        public void setCarIssueUserCard(String carIssueUserCard)
        {
            CarIssueUserCard = carIssueUserCard;
        }

        public double getBalance()
        {
            return Balance;
        }

        public void setBalance(double balance)
        {
            Balance = balance;
        }

        public String getCarValidStartDate()
        {
            return CarValidStartDate;
        }

        public void setCarValidStartDate(String carValidStartDate)
        {
            CarValidStartDate = carValidStartDate;
        }

        public String getCarValidEndDate()
        {
            return CarValidEndDate;
        }

        public void setCarValidEndDate(String carValidEndDate)
        {
            CarValidEndDate = carValidEndDate;
        }

        public String getCPH()
        {
            return CPH;
        }

        public void setCPH(String CPH)
        {
            this.CPH = CPH;
        }

        public String getCarType()
        {
            return CarType;
        }

        public void setCarType(String carType)
        {
            CarType = carType;
        }

        public String getCarPlace()
        {
            return CarPlace;
        }

        public void setCarPlace(String carPlace)
        {
            CarPlace = carPlace;
        }

        public String getCarValidMachine()
        {
            return CarValidMachine;
        }

        public void setCarValidMachine(String carValidMachine)
        {
            CarValidMachine = carValidMachine;
        }

        public String getCarValidZone()
        {
            return CarValidZone;
        }

        public void setCarValidZone(String carValidZone)
        {
            CarValidZone = carValidZone;
        }

        public String getCarMemo()
        {
            return CarMemo;
        }

        public void setCarMemo(String carMemo)
        {
            CarMemo = carMemo;
        }

        public String getIssueDate()
        {
            return IssueDate;
        }

        public void setIssueDate(String issueDate)
        {
            IssueDate = issueDate;
        }

        public String getIssueUserCard()
        {
            return IssueUserCard;
        }

        public void setIssueUserCard(String issueUserCard)
        {
            IssueUserCard = issueUserCard;
        }

        public String getDownloadSignal()
        {
            return DownloadSignal;
        }

        public void setDownloadSignal(String downloadSignal)
        {
            DownloadSignal = downloadSignal;
        }

        public int getTempnumber()
        {
            return Tempnumber;
        }

        public void setTempnumber(int tempnumber)
        {
            Tempnumber = tempnumber;
        }

        public String getTimeTeam()
        {
            return TimeTeam;
        }

        public void setTimeTeam(String timeTeam)
        {
            TimeTeam = timeTeam;
        }

        public String getHolidayLimited()
        {
            return HolidayLimited;
        }

        public void setHolidayLimited(String holidayLimited)
        {
            HolidayLimited = holidayLimited;
        }

        public String getUserInfo()
        {
            return UserInfo;
        }

        public void setUserInfo(String userInfo)
        {
            UserInfo = userInfo;
        }

        public String getCPHDownloadSignal()
        {
            return CPHDownloadSignal;
        }

        public void setCPHDownloadSignal(String CPHDownloadSignal)
        {
            this.CPHDownloadSignal = CPHDownloadSignal;
        }

        public int getMonthType()
        {
            return MonthType;
        }

        public void setMonthType(int monthType)
        {
            MonthType = monthType;
        }

        public int getID()
        {
            return ID;
        }

        public void setID(int ID)
        {
            this.ID = ID;
        }

        // 和 文档中的字符串不一样
        //        private long ID;              //  Y   自增长唯一标识
//        private String CardNO;              //  Y   卡号
//        private String UserNO;              //  Y   人员编号
//        private String CarCardType;              //  Y   车场卡类。MthA、MthB、TmpA、TmpB等。该字段的值应在卡类定义表(CardTypeDef)的Identifying字段值中选取。
//        private String CardState;              //  N   卡片状态。0正常，1挂失指令，2挂失完毕，3解挂指令，4挂失退款，5退卡完毕，6补卡完毕，其它状态未知
//        private double CardYJ;              //  N   卡片押金
//        private String SubSystem;              //  N   开通子系统。Bit位形式的字符串，第1位表示车场，第二位表示门禁，值1表示开通，值0表示不开通。
//        private String CarIssueDate;              //  N   车场发行日期
//        private String CarIssueUserCard;              //  N   车场发行人卡号
//        private double Balance;              //  N   余额
//        private String CarValidStartDate;              //  N   车场有效起日
//        private String CarValidEndDate;              //  N   车场有效止日
//        private String CPH;              //  N   车牌号码
//        private String CarColor;              //  N   车颜色。RGB表示法，使用6位十六进制数表示。参考颜色表示一节
//        private String CarType;              //  N   车型。如奔驰、宝马等
//        private String CarPlace;              //  N   车位位置。
//        private String CarWithdrawCardDate;              //  N   车场退卡时间
//        private String CarWithdrawOptCard;              //  N   车场退卡操作员
//        private String CarValidMachine;              //  N   车场有效机号。此字段的值为二进制字符串，一个字符代表一个机器，字符的位置即是机号；字符值为0表示在该机器上有通行权限。如：11101表示在4号机上有通行权限。
//        private String CarValidZone;              //  N   未使用
//        private String CarMemo;              //  N   车场备注
//        private String IssueDate;              //  N   门禁发行日期
//        private String WithdrawDate;              //  N   门禁退卡日期
//        private String IssueUserCard;              //  N   门禁发行操作员
//        private String WithdrawUserCard;              //  N   门禁退卡操作员
//        private String LossregDate;              //  N   门禁挂失日期
//        private String LossregUserCard;              //  N   门禁挂失操作员
//        private String CardIDNO;              //  N   物理卡号。(原车场软件只保存IC卡的)
//        private int Tempnumber;              //  N
//        private String TimeTeam;              //  N
//        private String HolidayLimited;              //  N
//        private String UserInfo;              //  N   用户信息
//        private String BackCardNum;              //  N
//        private String CardIdLossState;              //  N
//        private String CardIdUnLossState;              //  N
//        private String UnLossDate;              //  N
//        private String UnLossUer;              //  N
//        private String CarFailID;              //  N
//        private String CaRFailOKNO;              //  N
//        private String CardLossMachine;              //  N   卡片挂失机号
//        private String DownLoadState;              //  N   下载状态。0失败，1成功。下载卡号到设备、挂失卡号、解挂卡号、退卡操作，失败为0，成功为1。
//        private String DownloadSignal;              //  N   卡号下载标识。此字段的值为二进制字符串，一个字符代表一个工作站，字符的位置即是工作站编号；字符值为0表示在该工作站需要下载卡号。如：11101表示4号工作站需要执行下载卡号操作。
//        private String CPHDownloadSignal;              //  N   车牌下载标记。此字段的值为二进制字符串，一个字符代表一个工作站，字符的位置即是工作站编号；字符值为0表示该工作站需要下载车牌。如：11101表示4号工作站需要执行下载车牌操作。
//        private String Res1;              //  N
//        private String Res2;              //  N
//        private int MonthType;              //  N   月卡类型(全包、包白天、包晚上)
//        private String UserName;              //  N   参考人员Model结构同名字段
//        private String Sex;              //  N   参考人员Model结构同名字段
//        private int HomeAddress;              //  N   参考人员Model结构同名字段
////        private String DeptId;              //  N   参考人员Model结构同名字段
//        private int DeptId;              //  N   参考人员Model结构同名字段 // 由gson数据的返回
//        private String DeptName;              //  N   参考人员Model结构同名字段
//        private String Job;              //  N   参考人员Model结构同名字段
//        private String WorkTime;              //  N   参考人员Model结构同名字段
//        private String BirthDate;              //  N   参考人员Model结构同名字段
//        private String IDCard;              //  N   参考人员Model结构同名字段
//        private String MaritalStatus;              //  N   参考人员Model结构同名字段
//        private String HighestDegree;              //  N   参考人员Model结构同名字段
//        private String PoliticalStatus;              //  N   参考人员Model结构同名字段
//        private String School;              //  N   参考人员Model结构同名字段
//        private String Speciality;              //  N   参考人员Model结构同名字段
//        private String ForeignLanguage;              //  N   参考人员Model结构同名字段
//        private String Skill;              //  N   参考人员Model结构同名字段
//        private String TelNumber;              //  N   参考人员Model结构同名字段
//        private String MobNumber;              //  N   参考人员Model结构同名字段
//        private String ZipCode;              //  N   参考人员Model结构同名字段
//        private String NativePlace;              //  N   参考人员Model结构同名字段
//        private int CarPlaceNo;              //  N   参考人员Model结构同名字段
//        private String CardType;              //  N   参考	卡类型定义Model结构同名字段
//        private String CardStateCaption;              //  N   卡状态标题
//        private double SFJE;              //  N   收费金额。在延期及充值接口中保存本次操作收取的金额，在其它接口中没有意义。
//        private int RemainderDays;              //  N   剩余有效天数。只读字段。

        public DataBean()
        {

        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("UserName='").append(UserName).append('\'');
            sb.append(", HomeAddress='").append(HomeAddress).append('\'');
            sb.append(", DeptId=").append(DeptId);
            sb.append(", WorkTime='").append(WorkTime).append('\'');
            sb.append(", BirthDate='").append(BirthDate).append('\'');
            sb.append(", MobNumber='").append(MobNumber).append('\'');
            sb.append(", CarPlaceNo=").append(CarPlaceNo);
            sb.append(", CardType='").append(CardType).append('\'');
            sb.append(", CardStateCaption='").append(CardStateCaption).append('\'');
            sb.append(", SFJE=").append(SFJE);
            sb.append(", RemainderDays=").append(RemainderDays);
            sb.append(", CardNO='").append(CardNO).append('\'');
            sb.append(", UserNO='").append(UserNO).append('\'');
            sb.append(", CardState='").append(CardState).append('\'');
            sb.append(", CardYJ=").append(CardYJ);
            sb.append(", SubSystem='").append(SubSystem).append('\'');
            sb.append(", CarCardType='").append(CarCardType).append('\'');
            sb.append(", CarIssueDate='").append(CarIssueDate).append('\'');
            sb.append(", CarIssueUserCard='").append(CarIssueUserCard).append('\'');
            sb.append(", Balance=").append(Balance);
            sb.append(", CarValidStartDate='").append(CarValidStartDate).append('\'');
            sb.append(", CarValidEndDate='").append(CarValidEndDate).append('\'');
            sb.append(", CPH='").append(CPH).append('\'');
            sb.append(", CarType='").append(CarType).append('\'');
            sb.append(", CarPlace='").append(CarPlace).append('\'');
            sb.append(", CarValidMachine='").append(CarValidMachine).append('\'');
            sb.append(", CarValidZone='").append(CarValidZone).append('\'');
            sb.append(", CarMemo='").append(CarMemo).append('\'');
            sb.append(", IssueDate='").append(IssueDate).append('\'');
            sb.append(", IssueUserCard='").append(IssueUserCard).append('\'');
            sb.append(", DownloadSignal='").append(DownloadSignal).append('\'');
            sb.append(", Tempnumber=").append(Tempnumber);
            sb.append(", TimeTeam='").append(TimeTeam).append('\'');
            sb.append(", HolidayLimited='").append(HolidayLimited).append('\'');
            sb.append(", UserInfo='").append(UserInfo).append('\'');
            sb.append(", CPHDownloadSignal='").append(CPHDownloadSignal).append('\'');
            sb.append(", MonthType=").append(MonthType);
            sb.append(", ID=").append(ID);
            sb.append('}');
            return sb.toString();
        }
    }

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetCardIssueResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", PageIndex=").append(PageIndex);
        sb.append(", PageSize=").append(PageSize);
        sb.append(", TotalRows=").append(TotalRows);
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }
}
