package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-08.
 */
public class AddOptLogReq
{
    private String token; // Y 用户登录时候获取的token值
    private String jsonModel; // Y JSON格式的数据Model。
    private String CallBack; // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public String getToken()
    {
        return token;
    }

    public void setToken(String token)
    {
        this.token = token;
    }

    public String getJsonModel()
    {
        return jsonModel;
    }

    public void setJsonModel(String jsonModel)
    {
        this.jsonModel = jsonModel;
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
        return "AddOptLogReq{" +
                "token='" + token + '\'' +
                ", jsonModel='" + jsonModel + '\'' +
                ", CallBack='" + CallBack + '\'' +
                '}';
    }
}
