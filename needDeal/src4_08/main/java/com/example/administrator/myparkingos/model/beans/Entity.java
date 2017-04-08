package com.example.administrator.myparkingos.model.beans;

import java.util.List;

/**
 * Created by Administrator on 2017-03-29.
 */
public class Entity
{

    /**
     * rcode : 200
     * msg : OK
     * data : [{"UserName":"dfa","HomeAddress":"","DeptId":0,"WorkTime":"2017-03-22 16:32:13","BirthDate":"2017-03-22 16:32:13","MobNumber":"","CarPlaceNo":1,"CardType":"月租车A","CardStateCaption":"正常","SFJE":0,"RemainderDays":25,"CardNO":"88000037","UserNO":"A00029","CardState":"0","CardYJ":0,"SubSystem":"10000","CarCardType":"MthA","CarIssueDate":"2017-03-22 16:30:23","CarIssueUserCard":"888888","Balance":0,"CarValidStartDate":"2017-03-22 00:00:00","CarValidEndDate":"2017-04-22 00:00:00","CPH":"鄂A23123","CarType":"本田","CarPlace":"","CarValidMachine":"00111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111","CarValidZone":"0000000001000000","CarMemo":"","IssueDate":"2017-03-22 16:30:23","IssueUserCard":"888888","DownloadSignal":"0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000","Tempnumber":0,"TimeTeam":"","HolidayLimited":"0000000","UserInfo":"","CPHDownloadSignal":"0000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000","MonthType":0,"ID":41}]
     */

    private String rcode;
    private String msg;
    private List<DataBean> data;

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
        /**
         * UserName : dfa
         * HomeAddress :
         * DeptId : 0
         * WorkTime : 2017-03-22 16:32:13
         * BirthDate : 2017-03-22 16:32:13
         * MobNumber :
         * CarPlaceNo : 1
         * CardType : 月租车A
         * CardStateCaption : 正常
         * SFJE : 0.0
         * RemainderDays : 25
         * CardNO : 88000037
         * UserNO : A00029
         * CardState : 0
         * CardYJ : 0.0
         * SubSystem : 10000
         * CarCardType : MthA
         * CarIssueDate : 2017-03-22 16:30:23
         * CarIssueUserCard : 888888
         * Balance : 0.0
         * CarValidStartDate : 2017-03-22 00:00:00
         * CarValidEndDate : 2017-04-22 00:00:00
         * CPH : 鄂A23123
         * CarType : 本田
         * CarPlace :
         * CarValidMachine : 00111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
         * CarValidZone : 0000000001000000
         * CarMemo :
         * IssueDate : 2017-03-22 16:30:23
         * IssueUserCard : 888888
         * DownloadSignal : 0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
         * Tempnumber : 0
         * TimeTeam :
         * HolidayLimited : 0000000
         * UserInfo :
         * CPHDownloadSignal : 0000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
         * MonthType : 0
         * ID : 41
         */

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

        public void setUserName(String UserName)
        {
            this.UserName = UserName;
        }

        public String getHomeAddress()
        {
            return HomeAddress;
        }

        public void setHomeAddress(String HomeAddress)
        {
            this.HomeAddress = HomeAddress;
        }

        public int getDeptId()
        {
            return DeptId;
        }

        public void setDeptId(int DeptId)
        {
            this.DeptId = DeptId;
        }

        public String getWorkTime()
        {
            return WorkTime;
        }

        public void setWorkTime(String WorkTime)
        {
            this.WorkTime = WorkTime;
        }

        public String getBirthDate()
        {
            return BirthDate;
        }

        public void setBirthDate(String BirthDate)
        {
            this.BirthDate = BirthDate;
        }

        public String getMobNumber()
        {
            return MobNumber;
        }

        public void setMobNumber(String MobNumber)
        {
            this.MobNumber = MobNumber;
        }

        public int getCarPlaceNo()
        {
            return CarPlaceNo;
        }

        public void setCarPlaceNo(int CarPlaceNo)
        {
            this.CarPlaceNo = CarPlaceNo;
        }

        public String getCardType()
        {
            return CardType;
        }

        public void setCardType(String CardType)
        {
            this.CardType = CardType;
        }

        public String getCardStateCaption()
        {
            return CardStateCaption;
        }

        public void setCardStateCaption(String CardStateCaption)
        {
            this.CardStateCaption = CardStateCaption;
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

        public void setRemainderDays(int RemainderDays)
        {
            this.RemainderDays = RemainderDays;
        }

        public String getCardNO()
        {
            return CardNO;
        }

        public void setCardNO(String CardNO)
        {
            this.CardNO = CardNO;
        }

        public String getUserNO()
        {
            return UserNO;
        }

        public void setUserNO(String UserNO)
        {
            this.UserNO = UserNO;
        }

        public String getCardState()
        {
            return CardState;
        }

        public void setCardState(String CardState)
        {
            this.CardState = CardState;
        }

        public double getCardYJ()
        {
            return CardYJ;
        }

        public void setCardYJ(double CardYJ)
        {
            this.CardYJ = CardYJ;
        }

        public String getSubSystem()
        {
            return SubSystem;
        }

        public void setSubSystem(String SubSystem)
        {
            this.SubSystem = SubSystem;
        }

        public String getCarCardType()
        {
            return CarCardType;
        }

        public void setCarCardType(String CarCardType)
        {
            this.CarCardType = CarCardType;
        }

        public String getCarIssueDate()
        {
            return CarIssueDate;
        }

        public void setCarIssueDate(String CarIssueDate)
        {
            this.CarIssueDate = CarIssueDate;
        }

        public String getCarIssueUserCard()
        {
            return CarIssueUserCard;
        }

        public void setCarIssueUserCard(String CarIssueUserCard)
        {
            this.CarIssueUserCard = CarIssueUserCard;
        }

        public double getBalance()
        {
            return Balance;
        }

        public void setBalance(double Balance)
        {
            this.Balance = Balance;
        }

        public String getCarValidStartDate()
        {
            return CarValidStartDate;
        }

        public void setCarValidStartDate(String CarValidStartDate)
        {
            this.CarValidStartDate = CarValidStartDate;
        }

        public String getCarValidEndDate()
        {
            return CarValidEndDate;
        }

        public void setCarValidEndDate(String CarValidEndDate)
        {
            this.CarValidEndDate = CarValidEndDate;
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

        public void setCarType(String CarType)
        {
            this.CarType = CarType;
        }

        public String getCarPlace()
        {
            return CarPlace;
        }

        public void setCarPlace(String CarPlace)
        {
            this.CarPlace = CarPlace;
        }

        public String getCarValidMachine()
        {
            return CarValidMachine;
        }

        public void setCarValidMachine(String CarValidMachine)
        {
            this.CarValidMachine = CarValidMachine;
        }

        public String getCarValidZone()
        {
            return CarValidZone;
        }

        public void setCarValidZone(String CarValidZone)
        {
            this.CarValidZone = CarValidZone;
        }

        public String getCarMemo()
        {
            return CarMemo;
        }

        public void setCarMemo(String CarMemo)
        {
            this.CarMemo = CarMemo;
        }

        public String getIssueDate()
        {
            return IssueDate;
        }

        public void setIssueDate(String IssueDate)
        {
            this.IssueDate = IssueDate;
        }

        public String getIssueUserCard()
        {
            return IssueUserCard;
        }

        public void setIssueUserCard(String IssueUserCard)
        {
            this.IssueUserCard = IssueUserCard;
        }

        public String getDownloadSignal()
        {
            return DownloadSignal;
        }

        public void setDownloadSignal(String DownloadSignal)
        {
            this.DownloadSignal = DownloadSignal;
        }

        public int getTempnumber()
        {
            return Tempnumber;
        }

        public void setTempnumber(int Tempnumber)
        {
            this.Tempnumber = Tempnumber;
        }

        public String getTimeTeam()
        {
            return TimeTeam;
        }

        public void setTimeTeam(String TimeTeam)
        {
            this.TimeTeam = TimeTeam;
        }

        public String getHolidayLimited()
        {
            return HolidayLimited;
        }

        public void setHolidayLimited(String HolidayLimited)
        {
            this.HolidayLimited = HolidayLimited;
        }

        public String getUserInfo()
        {
            return UserInfo;
        }

        public void setUserInfo(String UserInfo)
        {
            this.UserInfo = UserInfo;
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

        public void setMonthType(int MonthType)
        {
            this.MonthType = MonthType;
        }

        public int getID()
        {
            return ID;
        }

        public void setID(int ID)
        {
            this.ID = ID;
        }
    }
}
