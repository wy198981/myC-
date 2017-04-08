package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetOperatorsWithoutLoginResp
{
    private String rcode;         // Y
    private String msg;           // Y
    private List<DataBean> data;  // N

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetOperatorsWithoutLoginResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }

    public String getRcode()
    {
        return rcode;
    }

    public void setRcode(String rcode)
    {
        this.rcode = rcode;
    }

    public String getMsg()
    {
        return msg;
    }

    public void setMsg(String msg)
    {
        this.msg = msg;
    }

    public List<DataBean> getData()
    {
        return data;
    }

    public void setData(List<DataBean> data)
    {
        this.data = data;
    }

    public static class DataBean
    {
        private String UserNO;
        private String UserName;

        public String getUserNO()
        {
            return UserNO;
        }

        public void setUserNO(String userNO)
        {
            UserNO = userNO;
        }

        public String getUserName()
        {
            return UserName;
        }

        public void setUserName(String userName)
        {
            UserName = userName;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("UserNO='").append(UserNO).append('\'');
            sb.append(", UserName='").append(UserName).append('\'');
            sb.append('}');
            return sb.toString();
        }
    }


}
