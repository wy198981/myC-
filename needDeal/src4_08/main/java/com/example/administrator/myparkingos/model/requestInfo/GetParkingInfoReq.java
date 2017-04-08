package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class GetParkingInfoReq
{
    private String token;  // Y 用户登录时候获取的token值
    private String StartTime;  // Y 统计信息的开始时间。如20160101123000
    private Integer StationId;  // Y 工作站编号。
    private String CallBack;  // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetParkingInfoReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", StartTime='").append(StartTime).append('\'');
        sb.append(", StationId=").append(StationId);
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

    public String getStartTime()
    {
        return StartTime;
    }

    public void setStartTime(String startTime)
    {
        StartTime = startTime;
    }

    public Integer getStationId()
    {
        return StationId;
    }

    public void setStationId(Integer stationId)
    {
        StationId = stationId;
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
