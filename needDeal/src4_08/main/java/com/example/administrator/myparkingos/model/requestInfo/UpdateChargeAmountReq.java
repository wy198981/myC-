package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class UpdateChargeAmountReq
{
    private String token; // Y 用户登录时候获取的token值                                                         
    private String CardNO; // Y 车辆编号。                                                         
    private String CardType; // Y 车辆卡类                                                         
    private String OutTime; // Y 出场时间。如20170303100028                                                         
    private String NewPayType; // Y 新的付款方式。0 现金；1 微信；2 支付宝                                                         
    private Double NewYSJE; // Y 新的应收金额                                                         
    private Double NewSFJE; // Y 新的收费金额                                                         
    private String Reserved; // N 备用字段                                                         
    private String CallBack; // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。       

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("UpdateChargeAmountReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", CardNO='").append(CardNO).append('\'');
        sb.append(", CardType='").append(CardType).append('\'');
        sb.append(", OutTime='").append(OutTime).append('\'');
        sb.append(", NewPayType='").append(NewPayType).append('\'');
        sb.append(", NewYSJE=").append(NewYSJE);
        sb.append(", NewSFJE=").append(NewSFJE);
        sb.append(", Reserved='").append(Reserved).append('\'');
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

    public String getNewPayType()
    {
        return NewPayType;
    }

    public void setNewPayType(String newPayType)
    {
        NewPayType = newPayType;
    }

    public Double getNewYSJE()
    {
        return NewYSJE;
    }

    public void setNewYSJE(Double newYSJE)
    {
        NewYSJE = newYSJE;
    }

    public Double getNewSFJE()
    {
        return NewSFJE;
    }

    public void setNewSFJE(Double newSFJE)
    {
        NewSFJE = newSFJE;
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
}
