package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-04-01.
 */
public class GetNetCameraSetResp
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
        final StringBuffer sb = new StringBuffer("GetNetCameraSetResp{");
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
        private long ID;              // Y 自增长唯一标识
        private int StationID;        // Y 工作站编号
        private String VideoIP;       // Y IP地址
        private String VideoPort;     // Y 端口
        private String VideoUserName; // Y 连接用户名
        private String VideoPassWord; // Y 连接密码
        private String VideoType;     // Y 摄像机类型
        private String StationName;   // N 参考工作站设置Model同名字段
        private String CarparkNO;     // N 参考工作站设置Model CarparkNO字段

        public long getID()
        {
            return ID;
        }

        public void setID(long ID)
        {
            this.ID = ID;
        }

        public int getStationID()
        {
            return StationID;
        }

        public void setStationID(int stationID)
        {
            StationID = stationID;
        }

        public String getVideoIP()
        {
            return VideoIP;
        }

        public void setVideoIP(String videoIP)
        {
            VideoIP = videoIP;
        }

        public String getVideoPort()
        {
            return VideoPort;
        }

        public void setVideoPort(String videoPort)
        {
            VideoPort = videoPort;
        }

        public String getVideoUserName()
        {
            return VideoUserName;
        }

        public void setVideoUserName(String videoUserName)
        {
            VideoUserName = videoUserName;
        }

        public String getVideoPassWord()
        {
            return VideoPassWord;
        }

        public void setVideoPassWord(String videoPassWord)
        {
            VideoPassWord = videoPassWord;
        }

        public String getVideoType()
        {
            return VideoType;
        }

        public void setVideoType(String videoType)
        {
            VideoType = videoType;
        }

        public String getStationName()
        {
            return StationName;
        }

        public void setStationName(String stationName)
        {
            StationName = stationName;
        }

        public String getCarparkNO()
        {
            return CarparkNO;
        }

        public void setCarparkNO(String carparkNO)
        {
            CarparkNO = carparkNO;
        }

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("ID=").append(ID);
            sb.append(", StationID=").append(StationID);
            sb.append(", VideoIP='").append(VideoIP).append('\'');
            sb.append(", VideoPort='").append(VideoPort).append('\'');
            sb.append(", VideoUserName='").append(VideoUserName).append('\'');
            sb.append(", VideoPassWord='").append(VideoPassWord).append('\'');
            sb.append(", VideoType='").append(VideoType).append('\'');
            sb.append(", StationName='").append(StationName).append('\'');
            sb.append(", CarparkNO='").append(CarparkNO).append('\'');
            sb.append('}');
            return sb.toString();
        }
    }


}
