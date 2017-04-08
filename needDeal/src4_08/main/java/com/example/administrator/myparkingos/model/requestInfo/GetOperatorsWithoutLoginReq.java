package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetOperatorsWithoutLoginReq
{
    private String jsonModel;       // N
    private String jsonSearchParam; // N
    private String OrderField;      // N
    private String CallBack;        // N

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetOperatorsWithoutLoginReq{");
        sb.append("jsonModel='").append(jsonModel).append('\'');
        sb.append(", jsonSearchParam='").append(jsonSearchParam).append('\'');
        sb.append(", OrderField='").append(OrderField).append('\'');
        sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }

    public String getJsonModel()
    {
        return jsonModel;
    }

    public void setJsonModel(String jsonModel)
    {
        this.jsonModel = jsonModel;
    }

    public String getJsonSearchParam()
    {
        return jsonSearchParam;
    }

    public void setJsonSearchParam(String jsonSearchParam)
    {
        this.jsonSearchParam = jsonSearchParam;
    }

    public String getOrderField()
    {
        return OrderField;
    }

    public void setOrderField(String orderField)
    {
        OrderField = orderField;
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
