package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetRightsByGroupIDResp
{
    private String rcode;
    private String msg;

    private List<DataBean> data;

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetRightsByGroupIDResp{");
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
        private long ID;              // Y 自增长唯一标识
        private long GroupID;         // Y 权限组编号
        private String GroupName;     // N 权限组名称
        private long RightsItemID;    // Y 权限明细的ID
        private String FormName;      // N 窗体名称
        private String ItemName;      // N 项目名称。如按钮名，菜单名等。
        private String Category;      // N 类别。如”门禁”，”车场”。
        private long PID;             // N 父级ID
        private String Description;   // N 描述
        private String Orders;        // N 排序标识
        private boolean CanView;      // N 是否可见
        private boolean CanOperate;   // N 是否可操作

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("ID=").append(ID);
            sb.append(", GroupID=").append(GroupID);
            sb.append(", GroupName='").append(GroupName).append('\'');
            sb.append(", RightsItemID=").append(RightsItemID);
            sb.append(", FormName='").append(FormName).append('\'');
            sb.append(", ItemName='").append(ItemName).append('\'');
            sb.append(", Category='").append(Category).append('\'');
            sb.append(", PID=").append(PID);
            sb.append(", Description='").append(Description).append('\'');
            sb.append(", Orders='").append(Orders).append('\'');
            sb.append(", CanView=").append(CanView);
            sb.append(", CanOperate=").append(CanOperate);
            sb.append('}');
            return sb.toString();
        }

        public long getID()
        {
            return ID;
        }

        public void setID(long ID)
        {
            this.ID = ID;
        }

        public long getGroupID()
        {
            return GroupID;
        }

        public void setGroupID(long groupID)
        {
            GroupID = groupID;
        }

        public String getGroupName()
        {
            return GroupName;
        }

        public void setGroupName(String groupName)
        {
            GroupName = groupName;
        }

        public long getRightsItemID()
        {
            return RightsItemID;
        }

        public void setRightsItemID(long rightsItemID)
        {
            RightsItemID = rightsItemID;
        }

        public String getFormName()
        {
            return FormName;
        }

        public void setFormName(String formName)
        {
            FormName = formName;
        }

        public String getItemName()
        {
            return ItemName;
        }

        public void setItemName(String itemName)
        {
            ItemName = itemName;
        }

        public String getCategory()
        {
            return Category;
        }

        public void setCategory(String category)
        {
            Category = category;
        }

        public long getPID()
        {
            return PID;
        }

        public void setPID(long PID)
        {
            this.PID = PID;
        }

        public String getDescription()
        {
            return Description;
        }

        public void setDescription(String description)
        {
            Description = description;
        }

        public String getOrders()
        {
            return Orders;
        }

        public void setOrders(String orders)
        {
            Orders = orders;
        }

        public boolean isCanView()
        {
            return CanView;
        }

        public void setCanView(boolean canView)
        {
            CanView = canView;
        }

        public boolean isCanOperate()
        {
            return CanOperate;
        }

        public void setCanOperate(boolean canOperate)
        {
            CanOperate = canOperate;
        }
    }
}
