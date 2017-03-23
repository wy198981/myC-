package com.znykt.zhpark.Until;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.security.SecureRandom;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.ParseException;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.CoreConnectionPNames;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.util.EntityUtils;
import org.json.JSONException;
import org.json.JSONObject;

import com.google.gson.Gson;
import com.znykt.zhpark.Model.RelustModel;
import com.znykt.zhpark.Model.carModel;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.StrictMode;
import android.util.Log;

public class HttpUtil {

	private static HttpResponse httpResponse = null;
	private static Gson gson = new Gson();
	private HttpUtil() {
	}

	/*
	 * 设置启动线程状态
	 */
	public static void setinitStrictMode() throws Exception {

		StrictMode.setThreadPolicy(new StrictMode.ThreadPolicy.Builder()
				.detectDiskReads().detectDiskWrites().detectNetwork()
				.penaltyLog().build());
		StrictMode.setVmPolicy(new StrictMode.VmPolicy.Builder()
				.detectLeakedSqlLiteObjects().detectLeakedClosableObjects()
				.penaltyLog().penaltyDeath().build());

	}

	/**
	 * @param Method
	 *            方法名称
	 * @param contenstr
	 *            参数拼接字符串
	 * @return 返回服务器的json字符串
	 */
	@SuppressWarnings("deprecation")
	public static String httpsGet(String Method, String contenstr) {

		String httpUrl = Constant.apiUrl + Method;
		if (!contenstr.equals("")) {
			httpUrl += "?" + contenstr;
		}
		HttpGet httpGet = new HttpGet(httpUrl);
		// 请求超时
		httpGet.getParams().setParameter(
				CoreConnectionPNames.CONNECTION_TIMEOUT, 5000);
		// 读取超时
		httpGet.getParams().setParameter(CoreConnectionPNames.SO_TIMEOUT, 5000);
		try {
			httpResponse = new DefaultHttpClient().execute(httpGet);

			if (httpResponse.getStatusLine().getStatusCode() == 200) {
				String result = EntityUtils.toString(httpResponse.getEntity());
				return result;
			} else {
				RelustModel model = new RelustModel();
				model.setRelustcode(0);
				model.setMsg("网络异常！");

				 return   gson.toJson(model);
			}
		} catch (Exception e) {

			RelustModel model = new RelustModel();
			model.setRelustcode(0);
			model.setMsg(e.getMessage());
			
			
			 return   gson.toJson(model);
	            
			  
		}

	}

	public static Bitmap getBitmapFromURL(String src) {
		try {
			Log.e("src", src);
			URL url = new URL(src);
			HttpURLConnection connection = (HttpURLConnection) url
					.openConnection();
			connection.setDoInput(true);
			connection.setRequestMethod("GET");
			connection.connect();
			InputStream input = connection.getInputStream();
			Bitmap myBitmap = BitmapFactory.decodeStream(input);
			Log.e("Bitmap", "returned");
			return myBitmap;
		} catch (IOException e) {
			e.printStackTrace();
			Log.e("Exception", e.getMessage());
			return null;
		}
	}

}
