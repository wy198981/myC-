package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-31.
 */
public class LoginUserReq
{
    private String UserNo;      // Y 停车场分配的用户编号如默认管理员88888
    private String password;    // Y 用户的密码的md5的32位加密后的字符串
    private String Language;    // N 指定语言类型。主要是一些提示信息的语言类型。如果不指定此参数则默认为服务端指定的语言。
    private Integer StationId;  // N 登陆的工作站编号。同一个工作站同一时刻只能有一个用户登陆。建议实现监控和收费功能的客户端输入此参数。
    private Boolean ForceLogin; // N 是否强制登陆。默认为false。本系统对同一用户限制为只能启动一个会话。如果出现同一个用户多次登陆的情况，可选择是否强制登陆，如果是则会把前一次登陆的同一个用户挤下线。
    private String CallBack;    // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public String getUserNo()
    {
        return UserNo;
    }

    public void setUserNo(String userNo)
    {
        UserNo = userNo;
    }

    public String getPassword()
    {
        return password;
    }

    public void setPassword(String password)
    {
        this.password = password;
    }

    public String getLanguage()
    {
        return Language;
    }

    public void setLanguage(String language)
    {
        Language = language;
    }

    public Integer getStationId()
    {
        return StationId;
    }

    public void setStationId(Integer stationId)
    {
        StationId = stationId;
    }

    public Boolean getForceLogin()
    {
        return ForceLogin;
    }

    public void setForceLogin(Boolean forceLogin)
    {
        ForceLogin = forceLogin;
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
        final StringBuffer sb = new StringBuffer("LoginUserReq{");
        sb.append("UserNo='").append(UserNo).append('\'');
        sb.append(", password='").append(password).append('\'');
        sb.append(", Language='").append(Language).append('\'');
        sb.append(", StationId=").append(StationId);
        sb.append(", ForceLogin=").append(ForceLogin);
        sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
