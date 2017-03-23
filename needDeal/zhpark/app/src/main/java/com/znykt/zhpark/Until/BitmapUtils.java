package com.znykt.zhpark.Until;

import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

public class BitmapUtils {
	/**
	 * 根据图片的url路径获得Bitmap对象
	 * @param url
	 * @return
	 */
	public static Bitmap returnBitmap(String url) {
		URL fileUrl = null;
		Bitmap bitmap = null;

		try {
			fileUrl = new URL(url);
		} catch (MalformedURLException e) {
			e.printStackTrace();
		}

		try {
			HttpURLConnection conn = (HttpURLConnection) fileUrl
					.openConnection();
			conn.setDoInput(true);
			conn.connect();
			InputStream is = conn.getInputStream();
			bitmap = BitmapFactory.decodeStream(is);
			is.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
		return bitmap;

	}
	
	 //加载图片  
    public static Bitmap getURLimage(String url) {  
        Bitmap bmp = null;  
        try {  
            URL myurl = new URL(url);  
            // 获得连接  
            HttpURLConnection conn = (HttpURLConnection) myurl.openConnection();  
            conn.setConnectTimeout(6000);//设置超时  
            conn.setDoInput(true);  
            conn.setUseCaches(false);//不缓存  
            conn.connect();  
            InputStream is = conn.getInputStream();//获得图片的数据流  
            bmp = BitmapFactory.decodeStream(is);  
            is.close();  
        } catch (Exception e) {  
            e.printStackTrace();  
        }  
        return bmp;  
    }  
}
