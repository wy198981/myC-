package com.zbar.lib;

import java.io.File;
import java.io.FileOutputStream;

import com.nostra13.universalimageloader.utils.L;
import com.znykt.zhpark.R;

import android.graphics.Bitmap;
import android.os.Environment;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.util.Log;


/**
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷(1076559197@qq.com)
 * <p/>
 * 时锟斤拷: 2014锟斤拷5锟斤拷9锟斤拷 锟斤拷锟斤拷12:24:13
 * <p/>
 * 锟芥本: V_1.0.0
 * <p/>
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷锟斤拷息锟斤拷锟斤拷锟�
 */
final class DecodeHandler extends Handler
{

    private int imageCount = 0;
    CaptureActivity activity = null;

    DecodeHandler(CaptureActivity activity)
    {
        this.activity = activity;
    }

    @Override
    public void handleMessage(Message message)
    {
        switch (message.what)
        {
            case R.id.decode:
                decode((byte[]) message.obj, message.arg1, message.arg2); // 解码数据
                break;
            case R.id.quit:
                Looper.myLooper().quit();
                break;
        }
    }

    private void decode(byte[] data, int width, int height)//解码之后变成Bitmap,然后进行faceRegconinze
    {
        imageCount++;

        if (imageCount == 5)
        {
            return ;
        }
        byte[] rotatedData = new byte[data.length];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                rotatedData[x * height + height - y - 1] = data[x + y * width];
        }
        int tmp = width;// Here we are swapping, that's the difference to #11
        width = height;
        height = tmp;

        Log.i("debug", "decode.........................." + data.length + ", width:" + width + ",height:" + height);
        ZbarManager manager = new ZbarManager();
        Log.i("debug", "decode  manager..........................");

        // 对于数据进行解析出来，变成一个String, 自己要做的是应该是对于图像解析成一个字符串
//        String result = manager.decode(rotatedData, width, height, true, activity.getX(), activity.getY(), activity.getCropWidth(),
//                activity.getCropHeight());

        Log.i("debug", "decode manager.decode..........................");
//        if (result != null)
        {


//            if (activity.isNeedCapture())
            {
                PlanarYUVLuminanceSource source = new PlanarYUVLuminanceSource(rotatedData, width, height, activity.getX(), activity.getY(),
                        activity.getCropWidth(), activity.getCropHeight(), false);//按照指定的方式来获取bitmap;


                int[] pixels = source.renderThumbnail();
                int w = source.getThumbnailWidth();
                int h = source.getThumbnailHeight();

                Bitmap bitmap = Bitmap.createBitmap(pixels, 0, w, w, h, Bitmap.Config.ARGB_8888);// 设置一个回调的对比即可；
                mIOnRecvBitmap.onSuccessRecvBitmap(bitmap);
                try
                {
                    String rootPath = Environment.getExternalStorageDirectory().getAbsolutePath() + "/Qrcode/";
                    File root = new File(rootPath);
                    if (!root.exists())
                    {
                        root.mkdirs();
                    }
                    File f = new File(rootPath + "Qrcode.jpg");
                    if (f.exists())
                    {
                        f.delete();
                    }
                    f.createNewFile();

                    FileOutputStream out = new FileOutputStream(f);
                    bitmap.compress(Bitmap.CompressFormat.JPEG, 100, out); // 获取图像数据

                    Log.i("debug", "create bitmap .........................." + f.getAbsolutePath()); // 可以获取图片；
                    out.flush();
                    out.close();
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
            }

//            if (null != activity.getHandler())
//            {
//                Message msg = new Message();
//                msg.obj = result;
//                msg.what = R.id.decode_succeeded;
//                activity.getHandler().sendMessage(msg); //即获取bitmap成功之后，传递handler数据到新的页面中，即完成显示即可；
//            }
        }
//        else
//        {
//            if (null != activity.getHandler())
//            {
//                activity.getHandler().sendEmptyMessage(R.id.decode_failed);
//            }
//        }
    }

    public interface IOnRecvBitmap
    {
        public void onSuccessRecvBitmap(Bitmap b);
//        public void onFailedRecvBitmap();
    }

    private IOnRecvBitmap mIOnRecvBitmap = null;
    public void setOnRecvBitmapListener(IOnRecvBitmap IOnRecvBitmap)
    {
        this.mIOnRecvBitmap = IOnRecvBitmap ;
    }
}
