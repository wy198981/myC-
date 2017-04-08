package com.example.administrator.myparkingos.model;

import android.graphics.Bitmap;

/**
 * Created by Administrator on 2017-03-09.
 */
public class ModelNode<T>
{

    /**
     * 车道索引,即当前第几个车道
     */
    private int iDzIndex;

    /**
     * 压地感识别车牌，这里很多的位置直接是""
     */
    private String sDzScan;
    /**
     * 图片名
     */
    private String strFile;
    /**
     * 路径
     */
    private String strFileJpg;
    /**
     * 车牌号
     */
    private String strCPH;

    /**
     * 图像数据 自己加的
     */
    public Bitmap picture;

    public E_CarInOutType type;

    public T data;
    public String momo; // 备用

    public enum E_CarInOutType
    {
        CAR_IN_TYPE_auto,           // 手动入场
        CAR_IN_TYPE_auto_noPlate,   // 无牌车手动入场
        CAR_IN_TYPE_recognition,     // 车牌识别入场

        CAR_OUT_TYPE_auto,          // 手动出场
        CAR_OUT_TYPE_auto_noPlate,   // 无牌车手动出场
        CAR_OUT_TYPE_recognition,     // 车牌识别出场

        CAR_INOUT_TYPE_recognition   // 相机自动识别
    }

    public ModelNode()
    {

    }

    public ModelNode(int iDzIndex, String sDzScan, String strFile, String strFileJpg, String strCPH)
    {
        this.iDzIndex = iDzIndex;
        this.sDzScan = sDzScan;
        this.strFile = strFile;
        this.strFileJpg = strFileJpg;
        this.strCPH = strCPH;
    }

    public int getiDzIndex()
    {
        return iDzIndex;
    }

    public void setiDzIndex(int iDzIndex)
    {
        this.iDzIndex = iDzIndex;
    }

    public String getsDzScan()
    {
        return sDzScan;
    }

    public void setsDzScan(String sDzScan)
    {
        this.sDzScan = sDzScan;
    }

    public String getStrFile()
    {
        return strFile;
    }

    public void setStrFile(String strFile)
    {
        this.strFile = strFile;
    }

    public String getStrFileJpg()
    {
        return strFileJpg;
    }

    public void setStrFileJpg(String strFileJpg)
    {
        this.strFileJpg = strFileJpg;
    }

    public String getStrCPH()
    {
        return strCPH;
    }

    public void setStrCPH(String strCPH)
    {
        this.strCPH = strCPH;
    }

    @Override
    public String toString()
    {
        return "ModelNode{" +
                "iDzIndex=" + iDzIndex +
                ", sDzScan='" + sDzScan + '\'' +
                ", strFile='" + strFile + '\'' +
                ", strFileJpg='" + strFileJpg + '\'' +
                ", strCPH='" + strCPH + '\'' +
                '}';
    }
}
