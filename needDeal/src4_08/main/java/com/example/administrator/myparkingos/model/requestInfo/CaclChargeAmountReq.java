package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-08.
 */
public class CaclChargeAmountReq
{
    private String token;     // Y 用户登录时候获取的token值
    private Integer StationId;   // Y 工作站编号
    private String CardNO;       // Y 车辆编号
    private String CardType;     // Y 车辆卡类。如TmpA
    private String CPH;          // Y 车牌号。用于计算车牌打折。如果同时存在车牌打折和打折设置，则以优惠力度最大的计算。
    private String InTime;       // Y 进场时间。如20161230235959
    private String OutTime;      // N 出场时间。如20161230235959
    private Long DiscountSetId;  // N 打折设置的ID。如果同时存在车牌打折和打折设置，则以优惠力度最大的计算。
    private String CallBack;     // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("CaclChargeAmountReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", StationId=").append(StationId);
        sb.append(", CardNO='").append(CardNO).append('\'');
        sb.append(", CardType='").append(CardType).append('\'');
        sb.append(", CPH='").append(CPH).append('\'');
        sb.append(", InTime='").append(InTime).append('\'');
        sb.append(", OutTime='").append(OutTime).append('\'');
        sb.append(", DiscountSetId=").append(DiscountSetId);
        sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }

    public String getToken()
    {
        return token;
    }

    public void setToken(String token)
    {
        this.token = token;
    }

    public Integer getStationId()
    {
        return StationId;
    }

    public void setStationId(Integer stationId)
    {
        StationId = stationId;
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

    public String getCPH()
    {
        return CPH;
    }

    public void setCPH(String CPH)
    {
        this.CPH = CPH;
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

    public Long getDiscountSetId()
    {
        return DiscountSetId;
    }

    public void setDiscountSetId(Long discountSetId)
    {
        DiscountSetId = discountSetId;
    }

    public String getCallBack()
    {
        return CallBack;
    }

    public void setCallBack(String callBack)
    {
        CallBack = callBack;
    }
}
