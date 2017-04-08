package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-03-28.
 */
public class SetCarInConfirmResp
{
    private String rcode;
    private String msg;
    private DataBean data;

    public static class DataBean
    {
        private String CPH; // Y 车牌号
        private String CPHConfirmed; // Y 已确认车牌号
        private Integer CtrlNumber; // Y 车道机号
        private Integer CtrlNumberConfirmed; // Y 已确认车道机号
        private Integer StationId; // Y 工作站编号
        private Integer CPColor; // N 车牌颜色。0为蓝色，1为黄色，2为白色，3为黑色，4为未识别
        private String Reserved; // N 备用字段
        private String InOutName; // N 车道名称
        private String UserNO; // N 用户编号
        private String UserName; // N 用户名称
        private String DeptName; // N 部门名称
        private Float Balance; // N 余额
        private String CarValidEndDate; // N 有效期结束日
        private Integer RemainingDays; // N 剩余有效天数
        private String CarPlace; // N 车位
        private Integer PersonalPlaceCount; // N 个人车位数量
        private Integer RemainingPlaceCount; // N 剩余车位数
        private String OpenMode; // N 开闸方式。0为自动开闸；1为确认开闸；2为不开闸
        private String CardNO; // N 车辆编号
        private String CardType; // N 车辆卡类
        private String ImageName; // N 图片名称
        private String ImagePath; // N 图片路径(服务器上的相对路径)
        private String InTime; // N 进场时间

        public String getCPH()
        {
            return CPH;
        }

        public void setCPH(String CPH)
        {
            this.CPH = CPH;
        }

        public String getCPHConfirmed()
        {
            return CPHConfirmed;
        }

        public void setCPHConfirmed(String CPHConfirmed)
        {
            this.CPHConfirmed = CPHConfirmed;
        }

        public Integer getCtrlNumber()
        {
            return CtrlNumber;
        }

        public void setCtrlNumber(Integer ctrlNumber)
        {
            CtrlNumber = ctrlNumber;
        }

        public Integer getCtrlNumberConfirmed()
        {
            return CtrlNumberConfirmed;
        }

        public void setCtrlNumberConfirmed(Integer ctrlNumberConfirmed)
        {
            CtrlNumberConfirmed = ctrlNumberConfirmed;
        }

        public Integer getStationId()
        {
            return StationId;
        }

        public void setStationId(Integer stationId)
        {
            StationId = stationId;
        }

        public Integer getCPColor()
        {
            return CPColor;
        }

        public void setCPColor(Integer CPColor)
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

        public Float getBalance()
        {
            return Balance;
        }

        public void setBalance(Float balance)
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

        public Integer getRemainingDays()
        {
            return RemainingDays;
        }

        public void setRemainingDays(Integer remainingDays)
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

        public Integer getPersonalPlaceCount()
        {
            return PersonalPlaceCount;
        }

        public void setPersonalPlaceCount(Integer personalPlaceCount)
        {
            PersonalPlaceCount = personalPlaceCount;
        }

        public Integer getRemainingPlaceCount()
        {
            return RemainingPlaceCount;
        }

        public void setRemainingPlaceCount(Integer remainingPlaceCount)
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

        public String getInTime()
        {
            return InTime;
        }

        public void setInTime(String inTime)
        {
            InTime = inTime;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("CPH='").append(CPH).append('\'');
            sb.append(", CPHConfirmed='").append(CPHConfirmed).append('\'');
            sb.append(", CtrlNumber=").append(CtrlNumber);
            sb.append(", CtrlNumberConfirmed=").append(CtrlNumberConfirmed);
            sb.append(", StationId=").append(StationId);
            sb.append(", CPColor=").append(CPColor);
            sb.append(", Reserved='").append(Reserved).append('\'');
            sb.append(", InOutName='").append(InOutName).append('\'');
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
            sb.append(", InTime='").append(InTime).append('\'');
            sb.append('}');
            return sb.toString();
        }
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

    public DataBean getData()
    {
        return data;
    }

    public void setData(DataBean data)
    {
        this.data = data;
    }

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("SetCarInConfirmResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }
}
