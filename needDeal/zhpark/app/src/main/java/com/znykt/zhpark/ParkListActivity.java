package com.znykt.zhpark;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.google.gson.Gson;
import com.google.gson.annotations.Until;

import com.znykt.zhpark.PersonMangerActivity.JsObject;
import com.znykt.zhpark.Model.ParkOderrl;
import com.znykt.zhpark.Model.ParkOrderList;
import com.znykt.zhpark.Model.ParkOrderPay;
import com.znykt.zhpark.Model.RelustModel;
import com.znykt.zhpark.Until.ACache;
import com.znykt.zhpark.Until.AppManager;
import com.znykt.zhpark.Until.CommonUtil;
import com.znykt.zhpark.Until.Constant;
import com.znykt.zhpark.Until.HttpUtil;
import com.znykt.zhpark.Until.JsonUtils;
import com.znykt.zhpark.Until.ToastUtil;
import com.znykt.zhpark.pull.PullToRefreshView;
import com.znykt.zhpark.pull.PullToRefreshView.OnRefreshListener;
import com.znykt.zhpark.ui.pageloading;

import android.app.Activity;
import android.content.Intent;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.webkit.JavascriptInterface;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.ImageView;

/**
 * * @author 作者 E-mail: * @date 创建时间：2016-9-10 上午8:34:02 * @version 1.0 * @parameter
 * * @since * @return
 */
public class ParkListActivity extends Activity
{
    private WebView webView;
    private ACache mCache;
    private static final String TAG = "ParkListActivity";
    private pageloading lv_pageload;// 加载动画
    private PullToRefreshView mPullToRefreshView;

    protected void onCreate(android.os.Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_parklist);

        mPullToRefreshView = (PullToRefreshView) this
                .findViewById(R.id.pull_view_main);

        webView = (WebView) this.findViewById(R.id.webView);

        mPullToRefreshView.setOnRefreshListener(new OnRefreshListener()
        {
            @Override
            public void onRefresh()
            {
                // TODO Auto-generated method stub
                webView.loadUrl(webView.getUrl());
            }
        });

        // lv_pageload = (pageloading) findViewById(R.id.lv_pageload);

        WebSettings webset = webView.getSettings();
        webset.setJavaScriptEnabled(true);// 表示webview可以执行服务器端的js代码

        webset.setCacheMode(WebSettings.LOAD_DEFAULT); // 设置 缓存模式
        webView.setWebChromeClient(new WebChromeClient()
        {
        });
        webView.addJavascriptInterface(new JsObject(), "jsObject");
        webView.loadUrl("file:///android_asset/OrderList.html");

        AppManager.getAppManager().addActivity(this);
        ImageView iv_back = (ImageView) findViewById(R.id.iv_back);
        iv_back.setOnClickListener(new OnClickListener()
        {

            @Override
            public void onClick(View arg0)
            {
                AppManager.getAppManager()
                        .finishActivity(ParkListActivity.this);
            }
        });

        webView.setWebChromeClient(new WebChromeClient()
        {
            @Override
            public void onProgressChanged(WebView view, int newProgress)
            {
                if (newProgress == 100)
                {
                    // 网页加载完成
                    mPullToRefreshView.onRefreshComplete();
                }
                else
                {
                    // 加载中

                }
            }
        });
    }

    ;

    public class JsObject
    {

        private static final int SSUCCES = 0;

        /**
         * 加载订单
         *
         * @return 加载结果
         * @throws UnsupportedEncodingException
         * @throws JSONException
         */
        @JavascriptInterface
        public String onload() throws UnsupportedEncodingException,
                JSONException
        {

            // lv_pageload.setVisibility(View.VISIBLE);

            mCache = ACache.get(ParkListActivity.this);
            JSONObject reljob = mCache.getAsJSONObject("wx_user");

            String contenstr = "Openid="
                    + URLEncoder.encode(reljob.get("wx_openid").toString(),
                    "utf-8")
                    + "&Unionid="
                    + URLEncoder.encode(reljob.get("wx_unionid").toString(),
                    "utf-8");
            String reluststr = HttpUtil.httpsGet("getOrderList", contenstr);
            System.out.println(reluststr);

            Gson gson = new Gson();
            // 将返回的JSON数据转换为对象JsonRequestResult
            ParkOderrl relustmodel = gson.fromJson(reluststr, ParkOderrl.class);

            if (relustmodel.relustcode == 1)
            {

            }
            else
            {

                ToastUtil.TextToast(ParkListActivity.this, relustmodel.msg, 1);
            }

            return reluststr;

        }

        Handler saleHandler = new Handler()
        {
            @Override
            public void handleMessage(Message msg)
            {

                if (lv_pageload.getVisibility() == View.VISIBLE)
                {
                    lv_pageload.setVisibility(View.GONE);
                }
                else
                {

                    lv_pageload.setVisibility(View.VISIBLE);
                }
            }

        };

        /**
         * 订单详情
         *
         * @param OrderNo 订单编码
         * @throws UnsupportedEncodingException
         */
        @JavascriptInterface
        public void orderPay(String orderNo, String parkKey)
                throws UnsupportedEncodingException
        {
            if (!TextUtils.isEmpty(orderNo) && !TextUtils.isEmpty(parkKey))
            {

                String contenstr = "OrderNo="
                        + URLEncoder.encode(orderNo, "utf-8") + "&ParkKey="
                        + URLEncoder.encode(parkKey, "utf-8");

                String reluststr = HttpUtil.httpsGet("getOrderPay", contenstr);
                Log.e(TAG, reluststr);

                Gson gson = new Gson();
                // 将返回的JSON数据转换为对象JsonRequestResult
                ParkOrderPay relustmodel = gson.fromJson(reluststr,
                        ParkOrderPay.class);
                gson.fromJson(reluststr, ParkOderrl.class);
                if (relustmodel.relustcode == 1)
                {
                    relustmodel.getOrder().setParkOrder_EnterImgPath(Constant.cloudUrl + relustmodel.getOrder().ParkOrder_EnterImgPath);


                    Intent intent = new Intent(ParkListActivity.this,
                            ParkOrderPayActivity.class);
                    intent.putExtra("extra", JsonUtils.toJson(relustmodel));
                    startActivity(intent);


                }
                else
                {

                    ToastUtil.TextToast(ParkListActivity.this, relustmodel.msg,
                            1);
                }


            }
            else
            {
                ToastUtil.TextToast(ParkListActivity.this, "订单号码错误!", 1);
            }

        }

        /**
         * @param parkKey 停车场Key
         * @param orderNo 订单编码
         * @param status  锁车状态
         * @return 设置结果
         * @throws UnsupportedEncodingException
         */
        @JavascriptInterface
        public String LockCar(String parkKey, String orderNo, int status)
                throws UnsupportedEncodingException
        {

            // Message obj = new Message();
            // obj.obj = SSUCCES;
            // saleHandler.getMessageName(obj);

            String contenstr = "parkKey=" + URLEncoder.encode(parkKey, "utf-8")
                    + "&orderNo=" + URLEncoder.encode(orderNo, "utf-8")
                    + "&status=" + status;
            String reluststr = HttpUtil.httpsGet("lockCar", contenstr);
            Log.e(TAG, reluststr);

            Gson gson = new Gson();
            // 将返回的JSON数据转换为对象JsonRequestResult
            RelustModel relustmodel = gson.fromJson(reluststr,
                    RelustModel.class);
            gson.fromJson(reluststr, ParkOderrl.class);
            if (relustmodel.relustcode == 1)
            {

                ToastUtil.TextToast(ParkListActivity.this, relustmodel.msg, 1);
            }
            else
            {

                ToastUtil.TextToast(ParkListActivity.this, relustmodel.msg, 1);
            }

            return reluststr;

        }

    }

}
