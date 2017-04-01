package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-28.
 */
public class SetCarInConfirmReq
{
    private String token;                   // Y 用户登录时候获取的token值
    private String CPH;                     // Y 车牌号
    private String CPHConfirmed;            // Y 已确认车牌号
    private Integer CtrlNumber;                 // Y 车道机号
    private Integer StationId;                  // Y 工作站编号
    private Integer CPColor;                    // N 车牌颜色。0为蓝色，1为黄色，2为白色，3为黑色，4为未识别
    private String Reserved;                // N 备用字段
    private String CallBack;                // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public SetCarInConfirmReq()
    {

    }

    public SetCarInConfirmReq(String token, String CPH, String CPHConfirmed, Integer ctrlNumber, Integer stationId, Integer CPColor, String reserved, String callBack)
    {
        this.token = token;
        this.CPH = CPH;
        this.CPHConfirmed = CPHConfirmed;
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
        final StringBuffer sb = new StringBuffer("SetCarInConfirmReq{");
        if (token != null) sb.append("token='").append(token).append('\'');
        if (CPH != null) sb.append(", CPH='").append(CPH).append('\'');
        if (CPHConfirmed != null) sb.append(", CPHConfirmed='").append(CPHConfirmed).append('\'');
        if (CtrlNumber != null) sb.append(", CtrlNumber=").append(CtrlNumber);
        if (StationId != null) sb.append(", StationId=").append(StationId);
        if (CPColor != null) sb.append(", CPColor=").append(CPColor);
        if (Reserved != null) sb.append(", Reserved='").append(Reserved).append('\'');
        if (CallBack != null) sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
