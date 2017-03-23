package com.znykt.zhpark;

 
import com.znykt.zhpark.ParkListActivity.JsObject;
import com.znykt.zhpark.Image.ImageShower;
import com.znykt.zhpark.Until.ACache;
import com.znykt.zhpark.Until.AppManager;
import com.znykt.zhpark.Until.JsonUtils;
import com.znykt.zhpark.ui.pageloading;

import android.app.Activity;
import android.content.Intent;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.ImageView;

/**
 * * @author 作者 E-mail: * @date 创建时间：2016-9-10 下午3:32:51 * @version 1.0 * @parameter
 * * @since * @return
 */
public class ParkOrderPayActivity extends Activity {
	private WebView webView;
	private pageloading lv_pageload;// 加载动画
	private static final String TAG = "ParkOrderPayActivity";
	private String orderPayStr;// 传过来的订单值

	protected void onCreate(android.os.Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_parklist);

		// lv_pageload = (pageloading) findViewById(R.id.lv_pageload);

		Intent intent = getIntent();
		orderPayStr = intent.getStringExtra("extra");

		webView = (WebView) this.findViewById(R.id.webView);
		WebSettings webset = webView.getSettings();
		webset.setJavaScriptEnabled(true);// 表示webview可以执行服务器端的js代码
		webView.setWebChromeClient(new WebChromeClient() {
		});
		webView.addJavascriptInterface(new JsObject(), "jsObject");
		webView.loadUrl("file:///android_asset/OrderPay.html");

		AppManager.getAppManager().addActivity(this);
		ImageView iv_back = (ImageView) findViewById(R.id.iv_back);
		iv_back.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				AppManager.getAppManager().finishActivity(
						ParkOrderPayActivity.this);
			}
		});

	};

	public class JsObject {

		@JavascriptInterface
		public String onload() {
			Log.e( "xxxx",orderPayStr );
			return orderPayStr;
		}
		@JavascriptInterface
		public void ShowImg(String picth) {
			 
			
			Intent intent = new Intent(ParkOrderPayActivity.this,
					ImageShower.class);
			intent.putExtra("picth", picth);
			startActivity(intent);
			
			 

		}
	}

	Handler saleHandler = new Handler() {
		@Override
		public void handleMessage(Message msg) {

			if (lv_pageload.getVisibility() == View.VISIBLE) {
				lv_pageload.setVisibility(View.GONE);
			} else {

				lv_pageload.setVisibility(View.VISIBLE);
			}
		}

	};

}
