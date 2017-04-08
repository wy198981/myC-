package com.example.administrator.myparkingos.util;

/**
 * Created by Administrator on 2017-03-06.
 */

import android.os.Environment;
import android.os.StatFs;

import java.io.File;

/**
 *
 * @author zhy
 */
public class SDCardUtils
{
    private SDCardUtils()
    {
        /* cannot be instantiated */
        throw new UnsupportedOperationException("cannot be instantiated");
    }

    /**
     * 判断当前的sd是否是能
     *
     * @return
     */
    public static boolean isSDCardEnable()
    {
        return Environment.getExternalStorageState().equals(
                Environment.MEDIA_MOUNTED);

    }

    /**
     * 获取当前sdcard的路径
     *
     * @return
     */
    public static String getSDCardPath()
    {
        return Environment.getExternalStorageDirectory().getAbsolutePath()
                + File.separator;
    }

    /**
     * 获取当前sdcard可用block的大小
     *
     * @return
     */
    @SuppressWarnings("deprecation")
    public static long getSDCardAllSize()
    {
        if (isSDCardEnable())
        {
            StatFs stat = new StatFs(getSDCardPath());
            long availableBlocks = (long) stat.getAvailableBlocks() - 4;

            long freeBlocks = stat.getAvailableBlocks();
            return freeBlocks * availableBlocks;
        }
        return 0;
    }

    /**

     * 获取文件所在路径的可用空间
     * @param filePath
     * @return
     */
    @SuppressWarnings("deprecation")
    public static long getFreeBytes(String filePath)
    {
        if (filePath.startsWith(getSDCardPath()))
        {
            filePath = getSDCardPath();
        }
        else
        {
            filePath = Environment.getDataDirectory().getAbsolutePath();
        }
        StatFs stat = new StatFs(filePath);
        long availableBlocks = (long) stat.getAvailableBlocks() - 4;
        return stat.getBlockSize() * availableBlocks;
    }

    /**
     * 获取根目录的路径
     *
     * @return
     */
    public static String getRootDirectoryPath()
    {
        return Environment.getRootDirectory().getAbsolutePath();
    }

}

