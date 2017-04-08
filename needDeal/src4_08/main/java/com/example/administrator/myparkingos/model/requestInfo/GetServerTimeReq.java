package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetServerTimeReq
{
    private String token;      // Y 用户登录时候获取的token值
    private String CallBack;   // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public String getToken()
    {
        return token;
    }

    public void setToken(String token)
    {
        this.token = token;
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
        final StringBuffer sb = new StringBuffer("GetServerTimeReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
