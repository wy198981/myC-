package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-03-31.
 */
public class GetStationSetWithoutLoginResp
{
    private String rcode;
    private String msg;

    private List<DataBean> data;

    public static class DataBean
    {
        private long ID;              // Y 自增长唯一标识
        private Integer StationId;    // Y 机号
        private String StationName;   // Y 名称
        private Integer CarparkNO;    // Y 所属车场编号

        public Integer getCarparkNO()
        {
            return CarparkNO;
        }

        public void setCarparkNO(Integer carparkNO)
        {
            CarparkNO = carparkNO;
        }

        public long getID()
        {
            return ID;
        }

        public void setID(long ID)
        {
            this.ID = ID;
        }

        public Integer getStationId()
        {
            return StationId;
        }

        public void setStationId(Integer stationId)
        {
            StationId = stationId;
        }

        public String getStationName()
        {
            return StationName;
        }

        public void setStationName(String stationName)
        {
            StationName = stationName;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("ID=").append(ID);
            sb.append(", StationId=").append(StationId);
            sb.append(", StationName='").append(StationName).append('\'');
            sb.append(", CarparkNO=").append(CarparkNO);
            sb.append('}');
            return sb.toString();
        }
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

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("GetStationSetWithoutLoginResp{");
        sb.append("rcode='").append(rcode).append('\'');
        sb.append(", msg='").append(msg).append('\'');
        sb.append(", data=").append(data);
        sb.append('}');
        return sb.toString();
    }
}
