package com.example.administrator.myparkingos.model.requestInfo;

/**
 * Created by Administrator on 2017-03-29.
 */
public class GetCardIssueReq
{
    /**
     * 获取发卡行信息的请求结构
     */

    private String token;          // Y  用户登录时候获取的token值
    private String jsonModel;      // N
    /**
     * 查询Model。查询Model结构与数据的Model结构基本一致。查询Model内各个字段间的逻辑关系为与关系。
     * 对于需要按范围查询的字段，可修改查询Model结构，增加两个字段，字段名分别为需要查询的字段名后加_start和_end(不分大小写)，
     * 并在新增的字段中设置查询的值。对于后加_start的字段服务器采用>=比较符，对于后加_end的字段服务器采用<比较符。
     * 对于查询Model中字段值为空(null)的字段，服务器将忽略。
     * 例如，查询生日在90年代范围内的JSON如下：
     * {“BirthDate_Start”:”1990-1-1”,”BirthDate_End”:”2000-1-1”}
     * 如果同时指定了jsonModel和jsonSearchParam，则两组条件都有效，两组条件间是逻辑与的关系。
     * 查询条件。参考《数据查询条件结构》一节。 如果同时指定了jsonModel和jsonSearchParam，则两组条件都有效，两组条件间是逻辑与的关系。
     */

    private String jsonSearchParam;// N 查询条件。参考《数据查询条件结构》一节。 如果同时指定了jsonModel和jsonSearchParam，则两组条件都有效，两组条件间是逻辑与的关系。
    private Integer PageSize;      // N 每页最大记录数。必须和Page参数一起使用。
    private Integer Page;          // N 表示选取分页后的第几页的数据。从1开始编号。必须和PageSize参数一起使用。
    private String OrderField;     // N 指定查询的排序规则。如：”ID asc, PID desc”。ID和PID是数据结构中的字段名，asc表示升序排列，desc表示降序排列
    private String ExportFields;   // N 需要导出的字段列表。JSON格式的对象。如{“UserNO”:”用户编号”,”UserName”:”姓名”,……}
    private String CallBack;       // N 是否使用JSONP方式。关于JSONP方式请参考Javascript跨域访问一节。

    public GetCardIssueReq()
    {

    }

    public GetCardIssueReq(String token, String jsonModel, String jsonSearchParam, Integer pageSize, Integer page, String orderField, String exportFields, String callBack)
    {
        this.token = token;
        this.jsonModel = jsonModel;
        this.jsonSearchParam = jsonSearchParam;
        PageSize = pageSize;
        Page = page;
        OrderField = orderField;
        ExportFields = exportFields;
        CallBack = callBack;
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

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetCardIssueReq{");
        if (token != null) sb.append("token='").append(token).append('\'');
        if (jsonModel != null) sb.append(", jsonModel='").append(jsonModel).append('\'');
        if (jsonSearchParam != null) sb.append(", jsonSearchParam='").append(jsonSearchParam).append('\'');
        if (PageSize != null) sb.append(", PageSize=").append(PageSize);
        if (Page != null) sb.append(", Page=").append(Page);
        if (OrderField != null) sb.append(", OrderField='").append(OrderField).append('\'');
        if (ExportFields != null) sb.append(", ExportFields='").append(ExportFields).append('\'');
        if (CallBack != null) sb.append(", CallBack='").append(CallBack).append('\'');
        sb.append('}');
        return sb.toString();
    }
}
