package com.znykt.zhpark;

import com.znykt.zhpark.R;
import com.znykt.zhpark.Until.AppManager;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ImageView;

public class ErrorActivity extends Activity {
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_error);

		  AppManager.getAppManager().addActivity(this);
		  ImageView iv_back= (ImageView) findViewById(R.id.iv_back);
		  iv_back.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				 AppManager.getAppManager().finishActivity(ErrorActivity.this);
			}
		});
		
	}
}
