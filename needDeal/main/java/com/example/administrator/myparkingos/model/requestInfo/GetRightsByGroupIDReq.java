package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetRightsByGroupIDReq
{
    private String token;    // Y 用户登录时候获取的token值
    private Integer GroupID; // Y 权限组的编号。
    private String CallBack; // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetRightsByGroupIDReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", GroupID=").append(GroupID);
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

    public Integer getGroupID()
    {
        return GroupID;
    }

    public void setGroupID(Integer groupID)
    {
        GroupID = groupID;
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
