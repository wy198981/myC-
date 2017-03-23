package com.znykt.zhpark.Image;

import com.znykt.zhpark.R;
import com.znykt.zhpark.Until.BitmapUtils;
import com.znykt.zhpark.Until.HttpUtil;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.os.StrictMode;
import android.view.MotionEvent;
import android.widget.ImageView;

/**
 * @package：com.example.imageshowerdemo
 * @author：Allen
 * @email：jaylong1302@163.com
 * @data：2012-9-27 上午10:58:13
 * @description：The class is for...
 */
public class ImageShower extends Activity {
	private ImageView iv_carinpic;
	private String picthStr;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.imageshower);

		Intent intent = getIntent();
		picthStr = intent.getStringExtra("picth");

		iv_carinpic = (ImageView) findViewById(R.id.iv_carinpic);

		try {
			HttpUtil.setinitStrictMode();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		iv_carinpic.setImageBitmap(BitmapUtils.getURLimage(picthStr));

		final ImageLoadingDialog dialog = new ImageLoadingDialog(this);
		dialog.setCanceledOnTouchOutside(false);
		dialog.show();
		// 两秒后关闭后dialog
		new Handler().postDelayed(new Runnable() {
			@Override
			public void run() {
				dialog.dismiss();
			}
		}, 1000 * 2);
	}

	@Override
	public boolean onTouchEvent(MotionEvent event) {
		// TODO Auto-generated method stub
		finish();
		return true;
	}

}
