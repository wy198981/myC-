package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-04-05.
 */
public class GetCarInReq
{
    private String token;           // Y
    private String jsonModel;       // N
    private String jsonSearchParam; // N
    private Integer PageSize;       // N
    private Integer Page;           // N
    private String OrderField;      // N
    private String ExportFields;    // N
    private String CallBack;        // N

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetCarInReq{");
        sb.append("token='").append(token).append('\'');
        sb.append(", jsonModel='").append(jsonModel).append('\'');
        sb.append(", jsonSearchParam='").append(jsonSearchParam).append('\'');
        sb.append(", PageSize=").append(PageSize);
        sb.append(", Page=").append(Page);
        sb.append(", OrderField='").append(OrderField).append('\'');
        sb.append(", ExportFields='").append(ExportFields).append('\'');
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

    public Integer getPageSize()
    {
        return PageSize;
    }

    public void setPageSize(Integer pageSize)
    {
        PageSize = pageSize;
    }

    public Integer getPage()
    {
        return Page;
    }

    public void setPage(Integer page)
    {
        Page = page;
    }

    public String getOrderField()
    {
        return OrderField;
    }

    public void setOrderField(String orderField)
    {
        OrderField = orderField;
    }

    public String getExportFields()
    {
        return ExportFields;
    }

    public void setExportFields(String exportFields)
    {
        ExportFields = exportFields;
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
