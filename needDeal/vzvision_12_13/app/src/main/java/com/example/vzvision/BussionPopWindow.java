package com.example.vzvision;

import android.content.Context;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.widget.PopupWindow;
import android.widget.RelativeLayout.LayoutParams;

import com.device.DeviceSet;

import android.content.*;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.widget.PopupWindow;
import android.widget.RelativeLayout.LayoutParams;
import android.app.Dialog;

import com.device.*;

public class BussionPopWindow {
	protected Dialog mPop;
	protected DeviceSet mDs=null;
	protected  LayoutInflater lf;
	protected Context   mContent;

	public  BussionPopWindow(Context content,DeviceSet ds)
	{
		  lf = LayoutInflater.from(content);
		  mContent = content;
			 
		 mDs  = ds;
		 
		 init();
	}
	
	
	protected void init()
	{
		 
		 
	}
	
	
	public boolean isShowing()
	{
	
		return mPop.isShowing();
	}
	
	public void dismiss()
	{
		mPop.dismiss();
	}
	
	public void show()
	{
		 
	}
}
