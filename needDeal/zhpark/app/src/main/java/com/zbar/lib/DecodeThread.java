package com.zbar.lib;

import java.util.concurrent.CountDownLatch;


import android.graphics.Bitmap;
import android.os.Environment;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.widget.Toast;

import com.techshino.facerecognize.algorithm.FaceRecognize;
import com.techshino.facerecognize.interfaces.FaceInterface;
import com.techshino.utils.Logs.MyLog;

/**
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷(1076559197@qq.com)
 * <p/>
 * 时锟斤拷: 2014锟斤拷5锟斤拷9锟斤拷 锟斤拷锟斤拷12:24:34
 * <p/>
 * 锟芥本: V_1.0.0
 * <p/>
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷锟竭筹拷
 */
final class DecodeThread extends Thread
{
    //保存图片本地路径
    public static final String ACCOUNT_DIR = Environment.getExternalStorageDirectory().getPath()
            + "/account/";
    public static final String ACCOUNT_MAINTRANCE_ICON_CACHE = "icon_cache/";
    private static final String IMGPATH = ACCOUNT_DIR + ACCOUNT_MAINTRANCE_ICON_CACHE;
    private static final String IMAGE_FILE_NAME = "faceImage.jpeg";

    private FaceRecognize myface;                //算法SDK实例
    long[] multithreadSupport = new long[2];    //保存算法句柄，保存内容为指针，使用时作为传参，此处为2线程


    CaptureActivity activity;
    private DecodeHandler handler;
    private final CountDownLatch handlerInitLatch;

    DecodeThread(CaptureActivity activity)
    {
        this.activity = activity;
        handlerInitLatch = new CountDownLatch(1);

        initFaceSdk();
    }

    private void initFaceSdk()
    {
        //第一次开启应用时尝试激活并初始化
        myface = FaceRecognize.getInstance();
        try
        {

            int temp = myface.TESO_Init(activity, multithreadSupport);    //激活并初始化算法
            if (temp < 0)
            {
//                bt_activationCode.setEnabled(true);
//                bt_activationCode.setText("未激活");
                Log.i("debug", "未激活");
            }
            else
            {
                Log.i("debug", "激活成功");
//                Toast.makeText(mContext, "激活成功", Toast.LENGTH_LONG).show();
//                bt_activationCode.setText("已激活");
            }
        }
        catch (FaceInterface.FaceException e)
        {
            e.printStackTrace();
            Log.i("debug onCreate", "TESO_Init get FaceException " + e.getMessage());
            if (e.getMessage().equals("-31"))
                Log.i("debug", "还未激活");
        }
    }

    Handler getHandler()
    {
        try
        {
            handlerInitLatch.await();
        }
        catch (InterruptedException ie)
        {
            // continue?
        }
        return handler;
    }

    @Override
    public void run()
    {
        Looper.prepare();
        handler = new DecodeHandler(activity);
        handler.setOnRecvBitmapListener(new DecodeHandler.IOnRecvBitmap()
        {
            @Override
            public void onSuccessRecvBitmap(Bitmap currentBitmap)
            {
                // 在文件中存储一张图片，然后进行对比即可;
            }
        });
        handlerInitLatch.countDown(); // 保证DecodeHandler一定是在getHandler前执行；
        Looper.loop();
    }

}
