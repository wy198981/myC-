package com.example.administrator.myparkingos.model.responseInfo;

import java.util.List;

/**
 * Created by Administrator on 2017-04-01.
 */
public class GetCheDaoSetResp
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
        private long ID;                       //   自增长唯一标识
        private int StationID;                 //   工作站编号
        private int InOut;                     //   入出类型。0为入，1为出
        private String InOutName;              //   车道名称
        private int CtrlNumber;                //   机号
        private int OpenID;                    //   开闸机号
        private int OpenType;                  //   开闸方式
        private int PersonVideo;               //   人相视频
        private int BigSmall;                  //   大小车场。0为大车场，1为小车场。
        private int CheckPortID;               //   检测口
        private int OnLine;                    //   是否在线
        private int TempOut;                   //   临时出口
        private int HasOutCard;                //   出卡机
        private String SubJH;                  //   子机号
        private int XieYi;                     //   协议。0为485，1为TCP/IP
        private String IP;                     //   IP地址
        private String CameraIP;               //   相机IP
        private boolean OfflineSendSignal;     //   脱机使用时是否发送车辆进出场信号
        private boolean OfflineReciveSignal;   //   脱机使用时是否接收车辆进出场信号
        private int Temp1;                     //   备用
        private int Temp2;                     //   备用
        private int Temp3;                     //   备用
        private String StationName;            //   参考工作站设置Model同名字段
        private String CarparkNO;              //   参考工作站设置Model CarparkNO字段

        @Override
        public String toString()
        {
            final StringBuffer sb = new StringBuffer("DataBean{");
            sb.append("ID=").append(ID);
            sb.append(", StationID=").append(StationID);
            sb.append(", InOut=").append(InOut);
            sb.append(", InOutName='").append(InOutName).append('\'');
            sb.append(", CtrlNumber=").append(CtrlNumber);
            sb.append(", OpenID=").append(OpenID);
            sb.append(", OpenType=").append(OpenType);
            sb.append(", PersonVideo=").append(PersonVideo);
            sb.append(", BigSmall=").append(BigSmall);
            sb.append(", CheckPortID=").append(CheckPortID);
            sb.append(", OnLine=").append(OnLine);
            sb.append(", TempOut=").append(TempOut);
            sb.append(", HasOutCard=").append(HasOutCard);
            sb.append(", SubJH='").append(SubJH).append('\'');
            sb.append(", XieYi=").append(XieYi);
            sb.append(", IP='").append(IP).append('\'');
            sb.append(", CameraIP='").append(CameraIP).append('\'');
            sb.append(", OfflineSendSignal=").append(OfflineSendSignal);
            sb.append(", OfflineReciveSignal=").append(OfflineReciveSignal);
            sb.append(", Temp1=").append(Temp1);
            sb.append(", Temp2=").append(Temp2);
            sb.append(", Temp3=").append(Temp3);
            sb.append(", StationName='").append(StationName).append('\'');
            sb.append(", CarparkNO='").append(CarparkNO).append('\'');
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

        public int getStationID()
        {
            return StationID;
        }

        public void setStationID(int stationID)
        {
            StationID = stationID;
        }

        public int getInOut()
        {
            return InOut;
        }

        public void setInOut(int inOut)
        {
            InOut = inOut;
        }

        public String getInOutName()
        {
            return InOutName;
        }

        public void setInOutName(String inOutName)
        {
            InOutName = inOutName;
        }

        public int getCtrlNumber()
        {
            return CtrlNumber;
        }

        public void setCtrlNumber(int ctrlNumber)
        {
            CtrlNumber = ctrlNumber;
        }

        public int getOpenID()
        {
            return OpenID;
        }

        public void setOpenID(int openID)
        {
            OpenID = openID;
        }

        public int getOpenType()
        {
            return OpenType;
        }

        public void setOpenType(int openType)
        {
            OpenType = openType;
        }

        public int getPersonVideo()
        {
            return PersonVideo;
        }

        public void setPersonVideo(int personVideo)
        {
            PersonVideo = personVideo;
        }

        public int getBigSmall()
        {
            return BigSmall;
        }

        public void setBigSmall(int bigSmall)
        {
            BigSmall = bigSmall;
        }

        public int getCheckPortID()
        {
            return CheckPortID;
        }

        public void setCheckPortID(int checkPortID)
        {
            CheckPortID = checkPortID;
        }

        public int getOnLine()
        {
            return OnLine;
        }

        public void setOnLine(int onLine)
        {
            OnLine = onLine;
        }

        public int getTempOut()
        {
            return TempOut;
        }

        public void setTempOut(int tempOut)
        {
            TempOut = tempOut;
        }

        public int getHasOutCard()
        {
            return HasOutCard;
        }

        public void setHasOutCard(int hasOutCard)
        {
            HasOutCard = hasOutCard;
        }

        public String getSubJH()
        {
            return SubJH;
        }

        public void setSubJH(String subJH)
        {
            SubJH = subJH;
        }

        public int getXieYi()
        {
            return XieYi;
        }

        public void setXieYi(int xieYi)
        {
            XieYi = xieYi;
        }

        public String getIP()
        {
            return IP;
        }

        public void setIP(String IP)
        {
            this.IP = IP;
        }

        public String getCameraIP()
        {
            return CameraIP;
        }

        public void setCameraIP(String cameraIP)
        {
            CameraIP = cameraIP;
        }

        public boolean isOfflineSendSignal()
        {
            return OfflineSendSignal;
        }

        public void setOfflineSendSignal(boolean offlineSendSignal)
        {
            OfflineSendSignal = offlineSendSignal;
        }

        public boolean isOfflineReciveSignal()
        {
            return OfflineReciveSignal;
        }

        public void setOfflineReciveSignal(boolean offlineReciveSignal)
        {
            OfflineReciveSignal = offlineReciveSignal;
        }

        public int getTemp1()
        {
            return Temp1;
        }

        public void setTemp1(int temp1)
        {
            Temp1 = temp1;
        }

        public int getTemp2()
        {
            return Temp2;
        }

        public void setTemp2(int temp2)
        {
            Temp2 = temp2;
        }

        public int getTemp3()
        {
            return Temp3;
        }

        public void setTemp3(int temp3)
        {
            Temp3 = temp3;
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
    }
}
