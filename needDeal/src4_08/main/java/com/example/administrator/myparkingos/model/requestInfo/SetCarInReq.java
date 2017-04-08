package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-27.
 */
public class SetCarInReq
{
    private String token; //用户登录时候获取的token值 Y
    private String CPH; //车牌号 Y
    private Integer CtrlNumber; //车道机号 Y
    private Integer StationId; //工作站编号 Y
    private Integer CPColor; //车牌颜色。0为蓝色，1为黄色，2为白色，3为黑色，4为未识别 N
    private String Reserved; //备用字段 N
    private String CallBack; //是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。 N

    public SetCarInReq()
    {

    }

    public SetCarInReq(String token, String CPH, Integer ctrlNumber, Integer stationId, Integer CPColor, String reserved, String callBack)
    {
        this.token = token;
        this.CPH = CPH;
        CtrlNumber = ctrlNumber;
        StationId = stationId;
        this.CPColor = CPColor;
        Reserved = reserved;
        CallBack = callBack;
    }

    public String getToken()
    {
        return token;
    }

    public void setToken(String token)
    {
        this.token = token;
    }

    public String getCPH()
    {
        return CPH;
    }

    public void setCPH(String CPH)
    {
        this.CPH = CPH;
    }

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
        final StringBuffer sb = new StringBuffer("SetCarInReq{");

        if (token != null) sb.append("token='").append(token).append('\'');
        if (CPH != null) sb.append(", CPH='").append(CPH).append('\'');
        if (CtrlNumber != null) sb.append(", CtrlNumber=").append(CtrlNumber);
        if (StationId != null) sb.append(", StationId=").append(StationId);
        if (CPColor != null) sb.append(", CPColor=").append(CPColor);
        if (Reserved != null) sb.append(", Reserved='").append(Reserved).append('\'');
        if (CallBack != null) sb.append(", CallBack='").append(CallBack).append('\'');
        return sb.toString();
    }
}
