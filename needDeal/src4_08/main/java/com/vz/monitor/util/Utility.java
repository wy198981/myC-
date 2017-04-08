package com.vz.monitor.util;

import java.io.File;

import android.content.Context;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.os.Environment;
import android.os.StatFs;

public class Utility
{

    public static final String getVersionName(Context context)
    {
        try
        {
            PackageManager manager = context.getPackageManager();
            PackageInfo info = manager.getPackageInfo(context.getPackageName(), 0);
            return info.versionName;
        }
        catch (Exception e)
        {
            return "";
        }
    }


    public static final boolean hasSDCard()
    {
        if (Environment.MEDIA_MOUNTED.equals(Environment.getExternalStorageState()))
        {
            return true;
        }
        return false;
    }


    public static final long getSDCardAvailableSize()
    {
        File path = Environment.getExternalStorageDirectory();
        StatFs sf = new StatFs(path.getPath());
        long blockSize = sf.getBlockSize();
        long freeBlocks = sf.getAvailableBlocks();
        //return freeBlocks * blockSize;  //��λByte
        //return (freeBlocks * blockSize)/1024;   //��λKB
        return (freeBlocks * blockSize) / 1024 / 1024; //��λMB
    }
}
