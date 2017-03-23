package com.znykt.zhpark.wxapi;

import java.net.URLEncoder;
import java.util.UUID;

import org.json.JSONObject;

import android.app.Activity;
import android.app.PendingIntent.OnFinished;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.tencent.mm.sdk.constants.ConstantsAPI;
import com.tencent.mm.sdk.modelbase.BaseReq;
import com.tencent.mm.sdk.modelbase.BaseResp;
import com.tencent.mm.sdk.modelmsg.SendAuth;
import com.tencent.mm.sdk.openapi.IWXAPI;
import com.tencent.mm.sdk.openapi.IWXAPIEventHandler;
import com.tencent.mm.sdk.openapi.WXAPIFactory;
import com.znykt.zhpark.MainActivity;
import com.znykt.zhpark.R;
import com.znykt.zhpark.Until.ACache;
import com.znykt.zhpark.Until.AppManager;
import com.znykt.zhpark.Until.CommonUtil;
import com.znykt.zhpark.Until.Constant;
import com.znykt.zhpark.Until.HttpUtil;
import com.znykt.zhpark.Until.JsonUtils;
import com.znykt.zhpark.Until.SharedPrefsUtil;

public class WXEntryActivity extends Activity implements IWXAPIEventHandler,
		OnClickListener {

	private LinearLayout weixin_login;
	private IWXAPI api;
	private ACache mCache;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.activity_wxlogin);

		weixin_login = (LinearLayout) findViewById(R.id.weixin_login);
		weixin_login.setOnClickListener(this);

		api = WXAPIFactory.createWXAPI(this, Constant.WEIXIN_APP_ID, false);
		api.registerApp(Constant.WEIXIN_APP_ID);

		api.handleIntent(getIntent(), this);

		// this.finish();

	}

	@Override
	public void onClick(View v) {
		SendAuth.Req req = new SendAuth.Req();
		req.scope = "snsapi_userinfo";
		req.state = UUID.randomUUID().toString();
		api.sendReq(req);
		this.finish();
	}

	@Override
	public void onNewIntent(Intent intent) {
		super.onNewIntent(intent);
		setIntent(intent);
		api.handleIntent(intent, this);
	}

	// protected void onNewIntent(Intent intent) {
	// super.onNewIntent(intent);
	// setIntent(intent);
	// Constant.api.handleIntent(intent, this);
	// }

	@Override
	public void onReq(BaseReq req) {
		// switch (req.getType()) {
		//
		// case ConstantsAPI.COMMAND_GETMESSAGE_FROM_WX:
		// break;
		// case ConstantsAPI.COMMAND_SHOWMESSAGE_FROM_WX:
		// break;
		// default:
		// break;
		// }
		// this.finish();
	}

	@Override
	public void onResp(BaseResp resp) {
		switch (resp.errCode) {
		case BaseResp.ErrCode.ERR_OK:
			String code = ((SendAuth.Resp) resp).code;
			getAccess_token(code);
			Toast.makeText(this, R.string.auth_success, Toast.LENGTH_SHORT)
					.show();

			
			break;
		case BaseResp.ErrCode.ERR_USER_CANCEL:
			Toast.makeText(this, R.string.auth_cancel, Toast.LENGTH_SHORT)
					.show();
			break;
		case BaseResp.ErrCode.ERR_AUTH_DENIED:
			Toast.makeText(this, R.string.auth_failure, Toast.LENGTH_SHORT)
					.show();
			break;
		default:
			break;
		}

		// AppManager.getAppManager().finishActivity(this);

	}

	public void getAccess_token(String code) {

		final String path = "https://api.weixin.qq.com/sns/oauth2/access_token?appid="
				+ Constant.WEIXIN_APP_ID
				+ "&secret="
				+ Constant.WEIXIN_APP_SECRET
				+ "&code="
				+ code
				+ "&grant_type=authorization_code";
		new Thread(new Runnable() {
			@Override
			public void run() {

				try {
					JSONObject jsonObject = JsonUtils
							.initSSLWithHttpClinet(path);// 请求https连接并得到json结果
					if (null != jsonObject) {
						String openid = jsonObject.getString("openid")
								.toString().trim();
						String access_token = jsonObject
								.getString("access_token").toString().trim();
						getUserMesg(access_token, openid);
					}

				} catch (Exception e) {
					e.printStackTrace();
				 
				}
			}
		}).start();

	}

	private void getUserMesg(final String access_token, final String openid) {
		String path = "https://api.weixin.qq.com/sns/userinfo?access_token="
				+ access_token + "&openid=" + openid;
		try {
			JSONObject jsonObject = JsonUtils.initSSLWithHttpClinet(path);// 请求https连接并得到json结果
			if (null != jsonObject) {
				String nickname = jsonObject.getString("nickname");
				String sex = jsonObject.get("sex").toString();
				String headimgurl = jsonObject.getString("headimgurl");
				String unionid = jsonObject.getString("unionid");
				if (sex == "1") {
					sex = "男";
				} else if (sex == "0") {
					sex = "女";
				} else {
					sex = "未知";
				}
				
				Constant.isLogin = true;
				Log.e("wx", "getUserMesg 拿到了用户Wx基本信息.. nickname:" + nickname);
				String contenstr = "Openid=" +URLEncoder.encode(openid,"utf-8")  + "&LoginTime="
						+URLEncoder.encode(CommonUtil.getStringDate(),"utf-8")   + "&NikeName="  +URLEncoder.encode(nickname,"utf-8") 
						+ "&Heading=" +URLEncoder.encode(headimgurl,"utf-8")   + "&Unionid=" + URLEncoder.encode(unionid,"utf-8")  ;
			 	HttpUtil.httpsGet("wxLogin",   contenstr);
				
				
				JSONObject persbject = new JSONObject();
				persbject.put("wx_openid", openid);
				persbject.put("wx_userimg", headimgurl);
				persbject.put("wx_nikename", nickname);
				persbject.put("wx_sex", sex);
				persbject.put("wx_unionid", unionid);
				persbject.put("wx_id", 0);

				mCache = ACache.get(this);
				mCache.put("wx_user", persbject);


				SharedPrefsUtil
						.putValue(this, "ConfigParking", "IsLogin", true);
				
				
				Intent intent = new Intent();
				intent.putExtra("isLogin", "true");
				setResult(1, intent);

				this.finish();
				 
			}
		} catch (Exception e) {
			e.printStackTrace();
			
			Log.e("wx", e.getMessage());
			Intent intent = new Intent();
			intent.putExtra("isLogin", "false");
			setResult(1, intent);
			this.finish();
			AppManager.getAppManager().finishActivity(this);
		}
		return;
	}

}
