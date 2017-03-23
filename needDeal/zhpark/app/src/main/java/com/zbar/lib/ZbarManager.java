package com.zbar.lib;

/**
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷(1076559197@qq.com)
 * <p/>
 * 时锟斤拷: 2014锟斤拷5锟斤拷9锟斤拷 锟斤拷锟斤拷12:25:46
 * <p/>
 * 锟芥本: V_1.0.0
 * <p/>
 * 锟斤拷锟斤拷: zbar锟斤拷锟斤拷锟斤拷
 */
public class ZbarManager
{

    static
    {
        try
        {
            System.loadLibrary("zbar");
        }
        catch (Throwable e)
        {
            System.out.println("System.loadLibrary(\"zbar\");" + e.getMessage());
        }
    }

    public native String decode(byte[] data, int width, int height, boolean isCrop, int x, int y, int cwidth, int cheight);
}
