package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-03-27.
 */
public class SetCarInResp
{
    /**
     * rcode : -3
     * msg : 重复入场
     * data : {"CPH":"?a48909","CtrlNumber":9,"StationId":1,"CPColor":null,"Reserved":null,"InOutName":"入口车道9","RemainingPlaceCount":904,"OpenMode":0,"CardNO":"3C481949","CardType":"TmpA","ImageName":"3C481949170327112006ineaffee1e-5889-447b-b12f-29bb57d92320.jpg","ImagePath":"CaptureImage\\1\\20170327\\3C481949170327112006ineaffee1e-5889-447b-b12f-29bb57d92320.jpg","intime":"2017-03-27 11:20:06"}
     */

    private String rcode;
    private String msg;
    private DataBean data;

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
        private String CPH;                         //  Y         车牌号
        private int CtrlNumber;                  //  Y         车道机号
        private int StationId;                   //  Y         工作站编号
        private int CPColor;                     //  N         车牌颜色。0为蓝色，1为黄色，2为白色，3为黑色，4为未识别
        private String Reserved;                    //  N         备用字段
        private String InOutName;                   //  N         车道名称
        private String BlackReason;                 //  N         黑名单原因
        private String UserNO;                      //  N         用户编号
        private String UserName;                    //  N         用户名称
        private String DeptName;                    //  N         部门名称
        private float Balance;                     //  N         余额
        private String CarValidEndDate;             //  N         有效期结束日
        private int RemainingDays;               //  N         剩余有效天数
        private String CarPlace;                    //  N         车位
        private int PersonalPlaceCount;          //  N         个人车位数量
        private int RemainingPlaceCount;         //  N         剩余车位数
        private String OpenMode;                    //  N         开闸方式。0为自动开闸；1为确认开闸；2为不开闸
        private String CardNO;                      //  N         车辆编号
        private String CardType;                    //  N         车辆卡类
        private String ImageName;                   //  N         图片名称
        private String ImagePath;                   //  N         图片路径(服务器上的相对路径)
        private String intime;                      //  N         进场时间

        public String getCPH()
        {
            return CPH;
        }

        public void setCPH(String CPH)
        {
            this.CPH = CPH;
        }

        public int getCtrlNumber()
        {
            return CtrlNumber;
        }

        public void setCtrlNumber(int ctrlNumber)
        {
            CtrlNumber = ctrlNumber;
        }

        public int getStationId()
        {
            return StationId;
        }

        public void setStationId(int stationId)
        {
            StationId = stationId;
        }

        public int getCPColor()
        {
            return CPColor;
        }

        public void setCPColor(int CPColor)
        {
            this.CPColor = CPColor;
        }

        public String getReserved()
        {
            return Reserved;
        }

        public void setReserved(String reserved)
        {
            Reserved = reserved;
        }

        public String getInOutName()
        {
            return InOutName;
        }

        public void setInOutName(String inOutName)
        {
            InOutName = inOutName;
        }

        public String getBlackReason()
        {
            return BlackReason;
        }

        public void setBlackReason(String blackReason)
        {
            BlackReason = blackReason;
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

        public float getBalance()
        {
            return Balance;
        }

        public void setBalance(float balance)
        {
            Balance = balance;
        }

        public String getCarValidEndDate()
        {
            return CarValidEndDate;
        }

        public void setCarValidEndDate(String carValidEndDate)
        {
            CarValidEndDate = carValidEndDate;
        }

        public int getRemainingDays()
        {
            return RemainingDays;
        }

        public void setRemainingDays(int remainingDays)
        {
            RemainingDays = remainingDays;
        }

        public String getCarPlace()
        {
            return CarPlace;
        }

        public void setCarPlace(String carPlace)
        {
            CarPlace = carPlace;
        }

        public int getPersonalPlaceCount()
        {
            return PersonalPlaceCount;
        }

        public void setPersonalPlaceCount(int personalPlaceCount)
        {
            PersonalPlaceCount = personalPlaceCount;
        }

        public int getRemainingPlaceCount()
        {
            return RemainingPlaceCount;
        }

        public void setRemainingPlaceCount(int remainingPlaceCount)
        {
            RemainingPlaceCount = remainingPlaceCount;
        }

        public String getOpenMode()
        {
            return OpenMode;
        }

        public void setOpenMode(String openMode)
        {
            OpenMode = openMode;
        }

        public String getCardNO()
        {
            return CardNO;
        }

        public void setCardNO(String cardNO)
        {
            CardNO = cardNO;
        }

        public String getCardType()
        {
            return CardType;
        }

        public void setCardType(String cardType)
        {
            CardType = cardType;
        }

        public String getImageName()
        {
            return ImageName;
        }

        public void setImageName(String imageName)
        {
            ImageName = imageName;
        }

        public String getImagePath()
        {
            return ImagePath;
        }

        public void setImagePath(String imagePath)
        {
            ImagePath = imagePath;
        }

        public String getIntime()
        {
            return intime;
        }

        public void setIntime(String intime)
        {
            this.intime = intime;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("CPH='").append(CPH).append('\'');
            sb.append(", CtrlNumber=").append(CtrlNumber);
            sb.append(", StationId=").append(StationId);
            sb.append(", CPColor=").append(CPColor);
            sb.append(", Reserved='").append(Reserved).append('\'');
            sb.append(", InOutName='").append(InOutName).append('\'');
            sb.append(", BlackReason='").append(BlackReason).append('\'');
            sb.append(", UserNO='").append(UserNO).append('\'');
            sb.append(", UserName='").append(UserName).append('\'');
            sb.append(", DeptName='").append(DeptName).append('\'');
            sb.append(", Balance=").append(Balance);
            sb.append(", CarValidEndDate='").append(CarValidEndDate).append('\'');
            sb.append(", RemainingDays=").append(RemainingDays);
            sb.append(", CarPlace='").append(CarPlace).append('\'');
            sb.append(", PersonalPlaceCount=").append(PersonalPlaceCount);
            sb.append(", RemainingPlaceCount=").append(RemainingPlaceCount);
            sb.append(", OpenMode='").append(OpenMode).append('\'');
            sb.append(", CardNO='").append(CardNO).append('\'');
            sb.append(", CardType='").append(CardType).append('\'');
            sb.append(", ImageName='").append(ImageName).append('\'');
            sb.append(", ImagePath='").append(ImagePath).append('\'');
            sb.append(", intime='").append(intime).append('\'');
            sb.append('}');
            return sb.toString();
        }
    }

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("SetCarInResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }
}
