package com.example.administrator.myparkingos.model.responseInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class UpdateChargeInfoResp
{
    private String rcode; // Y 参考错误码列表
    private String msg; // Y   错误信息
    /**
     * 返回值：
     * 40000            token已经失效,请重新获取
     * 40001            未知异常, 具体原因在msg字段中说明
     * 40002            输入参数缺失或错误
     * 40003            Token已过期
     * 40006            权限不足
     * 40012            余额不足
     * 40040            找不到入场记录
     */

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
