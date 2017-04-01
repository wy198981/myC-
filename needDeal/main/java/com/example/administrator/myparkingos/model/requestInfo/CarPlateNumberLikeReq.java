package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-29.
 */
public class CarPlateNumberLikeReq
{
    private String token;              // Y     用户登录时候获取的token值
    private String CarPlateNumber;     // Y     车牌号
    private Integer Precision;         // N     精确度，表示表中的车牌号的后6位与给定的车牌号匹配多少位即认为是相同的车牌。4、5或6，默认值为6。
    private String jsonSearchParam;    // N     查询条件。参考《数据查询条件结构》一节
    private String CallBack;           // N     是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public CarPlateNumberLikeReq()
    {

    }

    public CarPlateNumberLikeReq(String token, String carPlateNumber, Integer precision, String jsonSearchParam, String callBack)
    {
        this.token = token;
        CarPlateNumber = carPlateNumber;
        Precision = precision;
        this.jsonSearchParam = jsonSearchParam;
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

    public String getCarPlateNumber()
    {
        return CarPlateNumber;
    }

    public void setCarPlateNumber(String carPlateNumber)
    {
        CarPlateNumber = carPlateNumber;
    }

    public Integer getPrecision()
    {
        return Precision;
    }

    public void setPrecision(Integer precision)
    {
        Precision = precision;
    }

    public String getJsonSearchParam()
    {
        return jsonSearchParam;
    }

    public void setJsonSearchParam(String jsonSearchParam)
    {
        this.jsonSearchParam = jsonSearchParam;
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
        final StringBuffer sb = new StringBuffer("CarPlateNumberLikeReq{");
        if (token != null) sb.append("token='").append(token).append('\'');
        if (CarPlateNumber != null) sb.append(", CarPlateNumber='").append(CarPlateNumber).append('\'');
        if (Precision != null) sb.append(", Precision=").append(Precision);
        if (jsonSearchParam != null) sb.append(", jsonSearchParam='").append(jsonSearchParam).append('\'');
        if (CallBack != null) sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
