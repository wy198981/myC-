package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class UpdateChargeWithCaptureImageResp
{
    private String rcode; // Y 参考错误码列表
    private String msg; // Y   错误信息

    private int data; // N     受影响的行数

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("UpdateChargeAmountResp{");
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

    public int getData()
    {
        return data;
    }

    public void setData(int data)
    {
        this.data = data;
    }
}
