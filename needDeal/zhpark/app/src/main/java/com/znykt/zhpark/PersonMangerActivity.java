package com.znykt.zhpark;

import java.util.List;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.google.gson.JsonObject;
import com.znykt.zhpark.Until.ACache;
import com.znykt.zhpark.Until.AppManager;
import com.znykt.zhpark.Until.ToastUtil;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.ImageView;
import android.widget.Toast;

public class PersonMangerActivity extends Activity {
	private WebView webView;
	private ACache mCache;
	private static final String TAG = "PersonMangerActivity";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_personmanger);
		webView = (WebView) this.findViewById(R.id.webView);
		WebSettings webset = webView.getSettings();
		webset.setJavaScriptEnabled(true);// 表示webview可以执行服务器端的js代码
		webView.setWebChromeClient(new WebChromeClient() {
		});
		webView.addJavascriptInterface(new JsObject(), "jsObject");
		webView.loadUrl("file:///android_asset/UserCenter.html");

		
		 AppManager.getAppManager().addActivity(this);
		  ImageView iv_back= (ImageView) findViewById(R.id.iv_back);
		  iv_back.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				 AppManager.getAppManager().finishActivity(PersonMangerActivity.this);
			}
		});
	}

	public class JsObject {

		@JavascriptInterface
		public String onload() {

			mCache = ACache.get(PersonMangerActivity.this);

			JSONObject reljob = mCache.getAsJSONObject("wx_user");
			String relust = reljob.toString();
			// webView.loadUrl("javascript:show()");
			return relust;
		}

		@JavascriptInterface
		public void onClickParkOrder() {
			Intent intent = new Intent(PersonMangerActivity.this,
					ParkListActivity.class);
			startActivity(intent);
		}
		
		@JavascriptInterface
		public void onClickMyCarManager() {
			Intent intent = new Intent(PersonMangerActivity.this,
					MyCarManagerActivity.class);
			startActivity(intent);
		}

	}
}