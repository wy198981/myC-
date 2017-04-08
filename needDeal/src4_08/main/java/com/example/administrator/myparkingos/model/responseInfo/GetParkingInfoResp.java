package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class GetParkingInfoResp
{
    private String rcode; // Y 参考错误码列表
    private String msg; // Y 错误信息
    private DataBean data; // N 车场当前信息Model

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetParkingInfoResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
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
        private float TotalDiscount; // N 免费金额
        private float TotalCharge; // N 累计金额
        private int ManualOpenCarCount; // N 人工开闸车数量
        private int TotalCarCountInPark; // N 场内车辆总数
        private int MonthCarCountInPark; // N 场内月租车数量
        private int TempCarCountInPark; // N 场内临时车数量
        private int StrCarCountInPark; // N 场内储值车数量
        private int FreeCarCountInPark; // N 场内免费车数量

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("TotalDiscount=").append(TotalDiscount);
            sb.append(", TotalCharge=").append(TotalCharge);
            sb.append(", ManualOpenCarCount=").append(ManualOpenCarCount);
            sb.append(", TotalCarCountInPark=").append(TotalCarCountInPark);
            sb.append(", MonthCarCountInPark=").append(MonthCarCountInPark);
            sb.append(", TempCarCountInPark=").append(TempCarCountInPark);
            sb.append(", StrCarCountInPark=").append(StrCarCountInPark);
            sb.append(", FreeCarCountInPark=").append(FreeCarCountInPark);
            sb.append('}');
            return sb.toString();
        }

        public float getTotalDiscount()
        {
            return TotalDiscount;
        }

        public void setTotalDiscount(float totalDiscount)
        {
            TotalDiscount = totalDiscount;
        }

        public float getTotalCharge()
        {
            return TotalCharge;
        }

        public void setTotalCharge(float totalCharge)
        {
            TotalCharge = totalCharge;
        }

        public int getManualOpenCarCount()
        {
            return ManualOpenCarCount;
        }

        public void setManualOpenCarCount(int manualOpenCarCount)
        {
            ManualOpenCarCount = manualOpenCarCount;
        }

        public int getTotalCarCountInPark()
        {
            return TotalCarCountInPark;
        }

        public void setTotalCarCountInPark(int totalCarCountInPark)
        {
            TotalCarCountInPark = totalCarCountInPark;
        }

        public int getMonthCarCountInPark()
        {
            return MonthCarCountInPark;
        }

        public void setMonthCarCountInPark(int monthCarCountInPark)
        {
            MonthCarCountInPark = monthCarCountInPark;
        }

        public int getTempCarCountInPark()
        {
            return TempCarCountInPark;
        }

        public void setTempCarCountInPark(int tempCarCountInPark)
        {
            TempCarCountInPark = tempCarCountInPark;
        }

        public int getStrCarCountInPark()
        {
            return StrCarCountInPark;
        }

        public void setStrCarCountInPark(int strCarCountInPark)
        {
            StrCarCountInPark = strCarCountInPark;
        }

        public int getFreeCarCountInPark()
        {
            return FreeCarCountInPark;
        }

        public void setFreeCarCountInPark(int freeCarCountInPark)
        {
            FreeCarCountInPark = freeCarCountInPark;
        }
    }
}
