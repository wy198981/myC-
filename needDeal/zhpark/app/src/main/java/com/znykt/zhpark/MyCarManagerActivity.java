package com.znykt.zhpark;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;

import org.json.JSONException;
import org.json.JSONObject;

import com.google.gson.Gson;
import com.znykt.zhpark.Model.ParkOderrl;
import com.znykt.zhpark.Model.carModel;
import com.znykt.zhpark.PersonMangerActivity.JsObject;
import com.znykt.zhpark.Until.ACache;
import com.znykt.zhpark.Until.AppManager;
import com.znykt.zhpark.Until.HttpUtil;
import com.znykt.zhpark.Until.ToastUtil;
import com.znykt.zhpark.pull.PullToRefreshView;
import com.znykt.zhpark.pull.PullToRefreshView.OnRefreshListener;

import android.app.Activity;
import android.os.Bundle;
import android.os.PersistableBundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.ImageView;

/**
 * * @author 作者 E-mail: * @date 创建时间：2016-9-10 下午3:19:08 * @version 1.0 * @parameter
 * * @since * @return
 */
public class MyCarManagerActivity extends Activity {

	private WebView webView;
	private ACache mCache;
	private PullToRefreshView mPullToRefreshView;
	private static final String TAG = "MyCarManagerActivity";

	@Override
	public void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);

		setContentView(R.layout.activity_carmanager);

		mPullToRefreshView = (PullToRefreshView) this
				.findViewById(R.id.pull_view_main);

		mPullToRefreshView.setOnRefreshListener(new OnRefreshListener() {
			@Override
			public void onRefresh() {
				// TODO Auto-generated method stub
				webView.loadUrl(webView.getUrl());
			}
		});

		webView = (WebView) this.findViewById(R.id.webView);
		
		
		WebSettings webset = webView.getSettings();
		webset.setJavaScriptEnabled(true);// 表示webview可以执行服务器端的js代码
	 
		webView.setWebChromeClient(new WebChromeClient() {
		});
		webView.addJavascriptInterface(new JsObject(), "jsObject");
		webView.loadUrl("file:///android_asset/Car.html");
		webView.setBackgroundColor(0);
		
		
		AppManager.getAppManager().addActivity(this);
		ImageView iv_back = (ImageView) findViewById(R.id.iv_back);
		iv_back.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				AppManager.getAppManager().finishActivity(
						MyCarManagerActivity.this);
			}
		});

		webView.setWebChromeClient(new WebChromeClient() {
			@Override
			public void onProgressChanged(WebView view, int newProgress) {
				if (newProgress == 100) {
					// 网页加载完成
					mPullToRefreshView.onRefreshComplete();
				} else {
					// 加载中

				}
			}
		});

	}

	public class JsObject {

		@JavascriptInterface
		public String onload() throws UnsupportedEncodingException,
				JSONException {

			mCache = ACache.get(MyCarManagerActivity.this);
			JSONObject reljob = mCache.getAsJSONObject("wx_user");

			String contenstr = "Openid="
					+ URLEncoder.encode(reljob.get("wx_openid").toString(),
							"utf-8")
					+ "&Unionid="
					+ URLEncoder.encode(reljob.get("wx_unionid").toString(),
							"utf-8");
			String reluststr = HttpUtil.httpsGet("getCarList", contenstr);
			System.out.println(reluststr);

			Gson gson = new Gson();
			// 将返回的JSON数据转换为对象JsonRequestResult
			carModel relustmodel = gson.fromJson(reluststr, carModel.class);
			gson.fromJson(reluststr, ParkOderrl.class);
			if (relustmodel.relustcode == 1) {
				reljob.put("wx_id", relustmodel.getcarlt().get(0).WXUser_ID);
			 
				
			 
				mCache.put("wx_user", reljob);
			} else {

				ToastUtil.TextToast(MyCarManagerActivity.this, relustmodel.msg,1);
			}

			return reluststr;

		}

		@JavascriptInterface
		public String bindCar( String carNo)
				throws UnsupportedEncodingException, JSONException {
			mCache = ACache.get(MyCarManagerActivity.this);
			JSONObject reljob = mCache.getAsJSONObject("wx_user");

			String contenstr = "Openid="
					+ URLEncoder.encode(reljob.get("wx_openid").toString(),"utf-8") + "&WXUser_ID="
					+  URLEncoder.encode(reljob.get("wx_id").toString(),"utf-8")+ "&Carno="
					+ URLEncoder.encode(carNo.toString(), "utf-8");
			String reluststr = HttpUtil.httpsGet("bindCar", contenstr);
			System.out.println(reluststr);

			Gson gson = new Gson();
			// 将返回的JSON数据转换为对象JsonRequestResult
			ParkOderrl relustmodel = gson.fromJson(reluststr, ParkOderrl.class);
			gson.fromJson(reluststr, ParkOderrl.class);
			if (relustmodel.relustcode == 1) {

			} else {

				ToastUtil.TextToast(MyCarManagerActivity.this, relustmodel.msg,
						1);
			}

			return reluststr;

		}
		@JavascriptInterface
		public String unbindCar( String carNo)
				throws UnsupportedEncodingException, JSONException {
			mCache = ACache.get(MyCarManagerActivity.this);
			JSONObject reljob = mCache.getAsJSONObject("wx_user");

			String contenstr = "Openid="
					+ URLEncoder.encode(reljob.get("wx_openid").toString(),"utf-8") + "&WXUser_ID="
					+  URLEncoder.encode(reljob.get("wx_id").toString(),"utf-8")+ "&Carno="
					+ URLEncoder.encode(carNo.toString(), "utf-8");
			String reluststr = HttpUtil.httpsGet("unbindCar", contenstr);
			System.out.println(reluststr);

			Gson gson = new Gson();
			// 将返回的JSON数据转换为对象JsonRequestResult
			ParkOderrl relustmodel = gson.fromJson(reluststr, ParkOderrl.class);
			gson.fromJson(reluststr, ParkOderrl.class);
			if (relustmodel.relustcode == 1) {

			} else {

				ToastUtil.TextToast(MyCarManagerActivity.this, relustmodel.msg,
						1);
			}

			return reluststr;

		}
		
	}
}
