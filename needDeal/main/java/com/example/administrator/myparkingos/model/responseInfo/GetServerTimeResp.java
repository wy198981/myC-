package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetServerTimeResp
{
    private String rcode;    // Y
    private String msg;   // Y
    private DataBean data;   // Y


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

    public DataBean getData()
    {
        return data;
    }

    public void setData(DataBean data)
    {
        this.data = data;
    }

    public static class DataBean
    {
        private String serverTime;// Y 服务器时间格式yyyy-MM-dd HH:mm:ss

        public String getServerTime()
        {
            return serverTime;
        }

        public void setServerTime(String serverTime)
        {
            this.serverTime = serverTime;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("serverTime='").append(serverTime).append('\'');
            sb.append('}');
            return sb.toString();
        }
    }

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetServerTimeResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }
}
