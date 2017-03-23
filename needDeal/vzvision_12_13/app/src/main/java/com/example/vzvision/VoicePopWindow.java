package com.example.vzvision;

import android.app.Dialog;
import android.content.*;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.PopupWindow;
import android.widget.Toast;
import android.widget.RelativeLayout.LayoutParams;

import java.io.UnsupportedEncodingException;

import com.device.*;

import android.widget.*;

public class VoicePopWindow extends BussionPopWindow{
	
	private Button sendBtn;
	private EditText infoText;
	
	public  VoicePopWindow(Context content,DeviceSet ds)
	{
		  super(content, ds);
	}
	
	
	protected void init()
	{
//		mPop = new PopupWindow( lf.inflate(R.layout.voice, null),
//				 LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT);	
//		
//		mPop.setFocusable(true);
//		mPop.setOutsideTouchable(true);
//		sendBtn = (Button) (mPop.getContentView().findViewById(R.id.btn_voice_send));
//		
//		sendBtn.setOnClickListener(sendClickListener);
//		
//		mPop.getContentView().setOnClickListener(sendClickListener);
//		
//		infoText = (EditText) (mPop.getContentView().findViewById(R.id.editText_voiceInfo ));
//		
//		mPop.setTouchable(true);
//		mPop.setTouchInterceptor(new View.OnTouchListener() {
//			
//			@Override
//			public boolean onTouch(View v, MotionEvent event) {
//				// TODO Auto-generated method stub
//				 
//				return false;
//			}
//		});
		
        mPop = new Dialog(mContent );
		
		mPop.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mPop.setContentView(R.layout.voice);
		
		infoText = (EditText) (mPop.findViewById(R.id.editText_voiceInfo ));
			
		sendBtn = (Button) (mPop.findViewById(R.id.btn_voice_send));
		
		sendClickListener =  new View.OnClickListener(){
			@Override
			public void onClick(View view)
			{
				String text = infoText.getText().toString();
				
				if(text.isEmpty())
				{
					Toast.makeText(VoicePopWindow.this.mContent, "发送的语音数据为空", Toast.LENGTH_SHORT).show();
					return;
				}
				
				try
				{
					byte [] voice = text.getBytes("GBK");
					VoicePopWindow.this.mDs.playVoice(voice, 10, 50, 1);
					Toast.makeText(VoicePopWindow.this.mContent, "发送成功", Toast.LENGTH_SHORT).show();
				}
				catch(UnsupportedEncodingException e)
				{
					 Toast.makeText(VoicePopWindow.this.mContent, "语音转码失败", Toast.LENGTH_SHORT).show();
				}
				
			}
			
		};
		
		sendBtn.setOnClickListener(sendClickListener);
		
		
	}
	 
 
	public void show()
	{
		//mPop.showAtLocation(infoText, Gravity.CENTER, 0, 0);
		
		mPop.show();
	}
	 
	private View.OnClickListener sendClickListener = null;

}
