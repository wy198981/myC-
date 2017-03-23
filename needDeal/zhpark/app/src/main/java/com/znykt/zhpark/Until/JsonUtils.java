/**
 * Copyright (C) 2014-2015 Imtoy Technologies. All rights reserved.
 * @charset UTF-8
 * @author xiongxunxiang
 */
package com.znykt.zhpark.Until;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.json.JSONException;
import org.json.JSONObject;

import com.google.gson.Gson;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.lang.reflect.Type;

/**
 * @description 鏇村绉诲姩寮�鍙戝唴�?�硅鍏虫敞锛� http://blog.csdn.net/xiong_it
 * @charset UTF-8
 * @author xiong_it
 * @date 2015-7-16涓嬪�?5:41:05
 * @version 
 */
public class JsonUtils {
	
	 
     
	    /** 
	     * 对象转换成json字符串 
	     * @param obj  
	     * @return  
	     */  
	    public static String toJson(Object obj) {  
	        Gson gson = new Gson();  
	        return gson.toJson(obj);  
	    }  
	  
	    /** 
	     * json字符串转成对象 
	     * @param str   
	     * @param type 
	     * @return  
	     */  
	    public static <T> T fromJson(String str, Type type) {  
	        Gson gson = new Gson();  
	        return gson.fromJson(str, type);  
	    }  
	  
	    /** 
	     * json字符串转成对象 
	     * @param str   
	     * @param type  
	     * @return  
	     */  
	    public static <T> T fromJson(String str, Class<T> type) {  
	        Gson gson = new Gson();  
	        return gson.fromJson(str, type);  
	    }  
	    
	public static JSONObject initSSLWithHttpClinet(String path)
			throws ClientProtocolException, IOException {
		HTTPSTrustManager.allowAllSSL();
		JSONObject jsonObject = null;
		int timeOut = 30 * 1000;
		HttpParams param = new BasicHttpParams();
		HttpConnectionParams.setConnectionTimeout(param, timeOut);
		HttpConnectionParams.setSoTimeout(param, timeOut);
		HttpConnectionParams.setTcpNoDelay(param, true);

		SchemeRegistry registry = new SchemeRegistry();
		registry.register(new Scheme("http", PlainSocketFactory .getSocketFactory(), 80));
		registry.register(new Scheme("https", TrustAllSSLSocketFactory .getDefault(), 443));
		ClientConnectionManager manager = new ThreadSafeClientConnManager( param, registry);
		DefaultHttpClient client = new DefaultHttpClient(manager, param);

		HttpGet request = new HttpGet(path);
		// HttpGet request = new HttpGet("https://www.alipay.com/");
		HttpResponse response = client.execute(request);
		HttpEntity entity = response.getEntity();
		BufferedReader reader = new BufferedReader(new InputStreamReader( entity.getContent()));
		StringBuilder result = new StringBuilder();
		String line = "";
		while ((line = reader.readLine()) != null) {
			result.append(line);
			try {
				jsonObject = new JSONObject(line);
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return jsonObject;
	}
}
