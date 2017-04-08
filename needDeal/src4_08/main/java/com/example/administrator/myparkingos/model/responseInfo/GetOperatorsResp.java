package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetOperatorsResp
{
    private String rcode;  // 参考错误码列表
    private String msg;  //错误信息
    private int PageIndex;  //当前页码。仅当查询时指定了分页参数才有此值。
    private int PageSize;  // 分页大小。仅当查询时指定了分页参数才有此值。
    private int TotalRows;  //总行数。仅当查询时指定了分页参数才有此值。
    //Data  参考说明中的描述    如果没有指定ExportFields参数则为数据Model数组，否则为下载导出文件的完整URL

    private List<DataBean> data;

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetOperatorsWithoutLoginResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", PageIndex=").append(PageIndex);
        sb.append(", PageSize=").append(PageSize);
        sb.append(", TotalRows=").append(TotalRows);
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

    public int getPageIndex()
    {
        return PageIndex;
    }

    public void setPageIndex(int pageIndex)
    {
        PageIndex = pageIndex;
    }

    public int getPageSize()
    {
        return PageSize;
    }

    public void setPageSize(int pageSize)
    {
        PageSize = pageSize;
    }

    public int getTotalRows()
    {
        return TotalRows;
    }

    public void setTotalRows(int totalRows)
    {
        TotalRows = totalRows;
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
        private long ID;          // Y   自增长唯一标识
        private String CardNO;    // Y   卡号
        private String CardType;  // Y   卡类
        private String UserNO;    // Y   用户编号
        private String UserName;  // Y   用户姓名
        private long DeptId;      // Y   部门Id
        private String DeptName;  // N   参考部门数据结构同名字段
        private String Pwd;       // Y   密码(MD5编码的十六进制数字符串。)
        private int CardState;    // Y   卡状态
        private int UserLevel;    // Y   所属权限组(权限组ID字段的值)
        private String GroupName; // N   参考权限组数据结构同名字段

        public long getID()
        {
            return ID;
        }

        public void setID(long ID)
        {
            this.ID = ID;
        }

        public String getCardNO()
        {
            return CardNO;
        }

        public void setCardNO(String cardNO)
        {
            CardNO = cardNO;
        }

        public String getCardType()
        {
            return CardType;
        }

        public void setCardType(String cardType)
        {
            CardType = cardType;
        }

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

        public long getDeptId()
        {
            return DeptId;
        }

        public void setDeptId(long deptId)
        {
            DeptId = deptId;
        }

        public String getDeptName()
        {
            return DeptName;
        }

        public void setDeptName(String deptName)
        {
            DeptName = deptName;
        }

        public String getPwd()
        {
            return Pwd;
        }

        public void setPwd(String pwd)
        {
            Pwd = pwd;
        }

        public int getCardState()
        {
            return CardState;
        }

        public void setCardState(int cardState)
        {
            CardState = cardState;
        }

        public int getUserLevel()
        {
            return UserLevel;
        }

        public void setUserLevel(int userLevel)
        {
            UserLevel = userLevel;
        }

        public String getGroupName()
        {
            return GroupName;
        }

        public void setGroupName(String groupName)
        {
            GroupName = groupName;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("ID=").append(ID);
            sb.append(", CardNO='").append(CardNO).append('\'');
            sb.append(", CardType='").append(CardType).append('\'');
            sb.append(", UserNO='").append(UserNO).append('\'');
            sb.append(", UserName='").append(UserName).append('\'');
            sb.append(", DeptId=").append(DeptId);
            sb.append(", DeptName='").append(DeptName).append('\'');
            sb.append(", Pwd='").append(Pwd).append('\'');
            sb.append(", CardState=").append(CardState);
            sb.append(", UserLevel=").append(UserLevel);
            sb.append(", GroupName='").append(GroupName).append('\'');
            sb.append('}');
            return sb.toString();
        }
    }
}
