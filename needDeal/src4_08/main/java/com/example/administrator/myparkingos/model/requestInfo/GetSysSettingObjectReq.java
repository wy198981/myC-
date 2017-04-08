package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetSysSettingObjectReq
{
    private String token;         // Y 用户登录时候获取的token值
    private Integer StationID;   // Y 工作站编号
    private String CallBack;      // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public String getToken()
    {
        return token;
    }

    public void setToken(String token)
    {
        this.token = token;
    }

    public Integer getStationID()
    {
        return StationID;
    }

    public void setStationID(Integer stationID)
    {
        StationID = stationID;
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
        final StringBuffer sb = new StringBuffer("GetSysSettingObjectReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", StationID=").append(StationID);
        sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
