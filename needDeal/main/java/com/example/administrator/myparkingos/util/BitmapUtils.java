package com.example.administrator.myparkingos.util;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.util.Log;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.lang.ref.SoftReference;

/**
 * Created by Administrator on 2017-03-23.
 */
public class BitmapUtils
{
    /**
     * 将 byte 字节的数据转换成Bitmap图像数据，注意这里可以使用 SoftReference ，有效防止Bitmap的内存不足
     * @param imgByte
     * @return
     */
    public static Bitmap byteToBitmap(byte[] imgByte, int inSampleSize)
    {
//        Log.i(TAG, "### byteToBitmap");
        InputStream input = null;
        Bitmap bitmap = null;
        BitmapFactory.Options options = new BitmapFactory.Options();
        options.inSampleSize = inSampleSize;
        input = new ByteArrayInputStream(imgByte);
//        Log.i(TAG, "### SoftReference");
        SoftReference softRef = new SoftReference(BitmapFactory.decodeStream(
                input, null, options));
//        Log.i(TAG, "### BitmapFactory.decodeStream");
        bitmap = (Bitmap) softRef.get();
        if (imgByte != null)
        {
            imgByte = null;
        }

        try
        {
            if (input != null)
            {
                input.close();
            }
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
        return bitmap;
    }

    /**
     * 从文件中获取一副 Bitmap 图像数据
     * @param fileName
     * @return
     */
    public static Bitmap fileToBitmap(String fileName)
    {
        if (fileName == null)
        {
            return null;
        }
        return BitmapFactory.decodeFile(fileName);
    }
}
