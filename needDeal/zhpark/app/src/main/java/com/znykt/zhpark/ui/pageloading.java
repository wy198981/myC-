package com.znykt.zhpark.ui;

 

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.util.AttributeSet;
import android.view.View;
import android.widget.RelativeLayout;

public class pageloading extends   RelativeLayout {

	public pageloading(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);iniView(context);
		// TODO Auto-generated constructor stub
	}

	public pageloading(Context context, AttributeSet attrs) {
		super(context, attrs);iniView(context);
		// TODO Auto-generated constructor stub
	}

	public pageloading(Context context) {
		super(context);
		iniView(context);
		// TODO Auto-generated constructor stub
	}
	
	//把布局文件放在view中
		private void iniView(Context context) {

			View.inflate(context,com.znykt.zhpark.R.layout.page_loding, this);
		}
	 
}
