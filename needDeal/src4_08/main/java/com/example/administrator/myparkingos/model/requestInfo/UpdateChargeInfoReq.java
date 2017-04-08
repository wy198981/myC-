package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class UpdateChargeInfoReq
{
    private String token;// Y          用户登录时候获取的token值
    private String CardNO;// Y         车辆编号。
    private String CardType;// Y       车辆卡类
    private String OutTime;// Y        出场时间。
    private Double YSJE;// Y           应收金额。用于与服务端计费结果进行核对。
    private Double SFJE;// Y           收费金额。用于与服务端计费结果进行核对。
    private String NewCardType;// Y    新的车辆卡类。如果没有改卡类则传原来的卡类。
    private String NewPayType;// Y     新的付款方式。0 现金；1 微信；2 支付宝
    private Integer DiscountSetId;// N  打折设置的ID
    private String Reserved;// N       备用字段
    private String CallBack;// N       是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public String getToken()
    {
        return token;
    }

    public void setToken(String token)
    {
        this.token = token;
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

    public String getOutTime()
    {
        return OutTime;
    }

    public void setOutTime(String outTime)
    {
        OutTime = outTime;
    }

    public Double getYSJE()
    {
        return YSJE;
    }

    public void setYSJE(Double YSJE)
    {
        this.YSJE = YSJE;
    }

    public Double getSFJE()
    {
        return SFJE;
    }

    public void setSFJE(Double SFJE)
    {
        this.SFJE = SFJE;
    }

    public String getNewCardType()
    {
        return NewCardType;
    }

    public void setNewCardType(String newCardType)
    {
        NewCardType = newCardType;
    }

    public String getNewPayType()
    {
        return NewPayType;
    }

    public void setNewPayType(String newPayType)
    {
        NewPayType = newPayType;
    }

    public Integer getDiscountSetId()
    {
        return DiscountSetId;
    }

    public void setDiscountSetId(Integer discountSetId)
    {
        DiscountSetId = discountSetId;
    }

    public String getReserved()
    {
        return Reserved;
    }

    public void setReserved(String reserved)
    {
        Reserved = reserved;
    }

    public String getCallBack()
    {
        return CallBack;
    }

    public void setCallBack(String callBack)
    {
        CallBack = callBack;
    }

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("UpdateChargeInfoReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", CardNO='").append(CardNO).append('\'');
        sb.append(", CardType='").append(CardType).append('\'');
        sb.append(", OutTime='").append(OutTime).append('\'');
        sb.append(", YSJE=").append(YSJE);
        sb.append(", SFJE=").append(SFJE);
        sb.append(", NewCardType='").append(NewCardType).append('\'');
        sb.append(", NewPayType='").append(NewPayType).append('\'');
        sb.append(", DiscountSetId=").append(DiscountSetId);
        sb.append(", Reserved='").append(Reserved).append('\'');
        sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
