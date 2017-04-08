package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class SetCarOutWithoutEntryRecordReq
{
    private String token; // token             用户登录时候获取的token值
    private String CPH; // CPH                 车牌号
    private Integer CtrlNumber; // CtrlNumber  车道机号
    private Integer StationId; // StationId    工作站编号
    private Integer CPColor; // CPColor        车牌颜色。0为蓝色，1为黄色，2为白色，3为黑色，4为未识别
    private String Reserved; // N 备用字段
    private String CallBack; // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("SetCarOutWithoutEntryRecordReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", CPH='").append(CPH).append('\'');
        sb.append(", CtrlNumber=").append(CtrlNumber);
        sb.append(", StationId=").append(StationId);
        sb.append(", CPColor=").append(CPColor);
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
}
