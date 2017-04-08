package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-04-08.
 */
public class CaclChargeAmountResp
{
    private String rcode; // Y 参考错误码列表
    private String msg; // Y 错误信息
    private DataBean data; // N 结果对象。

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
        final StringBuffer sb = new StringBuffer("CaclChargeAmountResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }

    public static class DataBean
    {
        private float YSJE;    // Y 参考错误码列表
        private float SFJE;    // Y 错误信息
        private Object DiscountSet;     // N 优惠信息对象，结构参考打折车牌设置接口的Model结构。
        // 如果有多个优惠则只有优惠力度最大的一个生效。生效的优惠信息通过此对象返回。
        // 如果对象中的DeptId字段值大于或等于0则为打折车牌中的优惠，否则为优惠方式表中的优惠。
        private String InTime;     // N   入场时间
        private String OutTime;    // N   出场时间
        private String CardType;    // N   卡类型
        private int RemainingPlaceCount;    // N    当前剩余车位数

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("YSJE=").append(YSJE);
            sb.append(", SFJE=").append(SFJE);
            sb.append(", DiscountSet=").append(DiscountSet);
            sb.append(", InTime='").append(InTime).append('\'');
            sb.append(", OutTime='").append(OutTime).append('\'');
            sb.append(", CardType='").append(CardType).append('\'');
            sb.append(", RemainingPlaceCount=").append(RemainingPlaceCount);
            sb.append('}');
            return sb.toString();
        }

        public float getYSJE()
        {
            return YSJE;
        }

        public void setYSJE(float YSJE)
        {
            this.YSJE = YSJE;
        }

        public float getSFJE()
        {
            return SFJE;
        }

        public void setSFJE(float SFJE)
        {
            this.SFJE = SFJE;
        }

        public Object getDiscountSet()
        {
            return DiscountSet;
        }

        public void setDiscountSet(Object discountSet)
        {
            DiscountSet = discountSet;
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

        public String getCardType()
        {
            return CardType;
        }

        public void setCardType(String cardType)
        {
            CardType = cardType;
        }

        public int getRemainingPlaceCount()
        {
            return RemainingPlaceCount;
        }

        public void setRemainingPlaceCount(int remainingPlaceCount)
        {
            RemainingPlaceCount = remainingPlaceCount;
        }
    }
}
