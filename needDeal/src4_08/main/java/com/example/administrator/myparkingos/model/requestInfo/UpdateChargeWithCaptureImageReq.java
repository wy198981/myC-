package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class UpdateChargeWithCaptureImageReq
{
    private String token; // token 用户登录时候获取的token值
    private String CardNO; // CardNO 车辆编号。
    private String CardType; // CardType 车辆卡类
    private String OutTime; // OutTime 出场时间。
    private String Reserved; // Reserved 备用字段
    private String CallBack; // CallBack 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("UpdateChargeWithCaptureImageReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", CardNO='").append(CardNO).append('\'');
        sb.append(", CardType='").append(CardType).append('\'');
        sb.append(", OutTime='").append(OutTime).append('\'');
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
