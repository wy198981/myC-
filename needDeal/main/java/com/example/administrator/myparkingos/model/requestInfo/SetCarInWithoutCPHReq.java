package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-28.
 */
public class SetCarInWithoutCPHReq
{
    private String token;       // Y       用户登录时候获取的token值
    private Integer CtrlNumber; // Y       车道机号
    private Integer StationId;  // Y       工作站编号
    private String CPH;         // N       车牌号
    private String CarBrand;    // N       车牌品牌
    private String CarColor;    // N       车辆颜色
    private String Reserved;    // N       备用字段
    private String CallBack;    // N       是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public String getToken()
    {
        return token;
    }

    public void setToken(String token)
    {
        this.token = token;
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
        final StringBuffer sb = new StringBuffer("SetCarInWithoutCPHReq{");
        if (token != null) sb.append("token='").append(token).append('\'');
        if (CtrlNumber != null) sb.append(", CtrlNumber=").append(CtrlNumber);
        if (StationId != null) sb.append(", StationId=").append(StationId);
        if (CPH != null) sb.append(", CPH='").append(CPH).append('\'');
        if (CarBrand != null) sb.append(", CarBrand='").append(CarBrand).append('\'');
        if (CarColor != null) sb.append(", CarColor='").append(CarColor).append('\'');
        if (Reserved != null) sb.append(", Reserved='").append(Reserved).append('\'');
        if (CallBack != null) sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
