package com.vz.monitor.util;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class NetworkUtil
{

    /**
     * 测试当前网络是否连接，包含wifi 4G/3G信号
     * @param context
     * @return
     */
    public static final boolean isNetworkConnected(Context context)
    {
        NetworkInfo info = getNetworkInfo(context);
        if (null != info)
        {
            return info.isConnected();
        }
        return false;
    }

    /**
     * 检测当前系统的wifi 是否连接上；
     * @param context
     * @return
     */
    public static final boolean isWifiConnected(Context context)
    {
        NetworkInfo info = getNetworkInfo(context);
        if (null != info && info.isConnected())
        {
            return info.getType() == ConnectivityManager.TYPE_WIFI;
        }
        return false;
    }

    /**
     * 检测当前手机的网络是否连接上
     * @param context
     * @return
     */
    public static final boolean isMobileConnected(Context context)
    {
        NetworkInfo info = getNetworkInfo(context);
        if (null != info && info.isConnected())
        {
            return info.getType() == ConnectivityManager.TYPE_MOBILE;
        }
        return false;
    }


    private static final NetworkInfo getNetworkInfo(Context context)
    {
        ConnectivityManager connManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        if (null != connManager)
        {
            return connManager.getActiveNetworkInfo();
        }
        return null;
    }

}
