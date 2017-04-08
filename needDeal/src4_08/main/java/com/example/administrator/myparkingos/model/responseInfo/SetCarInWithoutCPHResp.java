package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-03-28.
 */
public class SetCarInWithoutCPHResp
{
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
        private Integer CtrlNumber;             // Y    车道机号                                                                                                                   
        private Integer StationId;              // Y    工作站编号                                                                                                               
        private String CPH;                    // N    车牌号                                                                                                              
        private String CarBrand;               // N    车牌品牌                                                                                                                  
        private String CarColor;               // N    车辆颜色                                                                                                                  
        private String Reserved;               // N    备用字段                                                                                                                  
        private String InOutName;              // N    车道名称                                                                                                                  
        private Integer RemainingPlaceCount;    // N    剩余车位数                                                                                                                           
        private String OpenMode;               // N    开闸方式。0为自动开闸；1为确认开闸；2为不开闸                                                                                                                  
        private String CardNO;                 // N    车辆编号                                                                                                                  
        private String CardType;               // N    车辆卡类                                                                                                                  
        private String ImageName;              // N    图片名称                                                                                                                  
        private String ImagePath;              // N    图片路径(服务器上的相对路径)                                                                                                                  
        private String InTime;                 // N    进场时间

        public Integer getCtrlNumber()
        {
            return CtrlNumber;
        }

        public void setCtrlNumber(Integer ctrlNumber)
        {
            CtrlNumber = ctrlNumber;
        }

        public Integer getStationId()
        {
            return StationId;
        }

        public void setStationId(Integer stationId)
        {
            StationId = stationId;
        }

        public String getCPH()
        {
            return CPH;
        }

        public void setCPH(String CPH)
        {
            this.CPH = CPH;
        }

        public String getCarBrand()
        {
            return CarBrand;
        }

        public void setCarBrand(String carBrand)
        {
            CarBrand = carBrand;
        }

        public String getCarColor()
        {
            return CarColor;
        }

        public void setCarColor(String carColor)
        {
            CarColor = carColor;
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
            sb.append("CtrlNumber=").append(CtrlNumber);
            sb.append(", StationId=").append(StationId);
            sb.append(", CPH='").append(CPH).append('\'');
            sb.append(", CarBrand='").append(CarBrand).append('\'');
            sb.append(", CarColor='").append(CarColor).append('\'');
            sb.append(", Reserved='").append(Reserved).append('\'');
            sb.append(", InOutName='").append(InOutName).append('\'');
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

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("SetCarInWithoutCPHResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }
}
