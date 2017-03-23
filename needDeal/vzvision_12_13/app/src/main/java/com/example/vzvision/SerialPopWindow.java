package com.example.vzvision;


import java.io.UnsupportedEncodingException;


import android.content.*;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.widget.PopupWindow;
import android.widget.Toast;
import android.widget.RelativeLayout.LayoutParams;

import com.device.*;

import android.widget.*;
import android.app.Dialog;
import  android.view.Window;
import android.widget.*;

public class SerialPopWindow extends BussionPopWindow{
 
	private Button sendBtn;
	private EditText infoText;
	private Spinner  serialNum;
	private String   serialNumText="1";

	public  SerialPopWindow(Context content,DeviceSet ds)
	{
		  super(content, ds);
		  
		  
	}
	
	
	protected void init()
	{
//		mPop = new PopupWindow( lf.inflate(R.layout.serial, null),
//				 LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT);
		mPop = new Dialog(mContent );
		
		mPop.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mPop.setContentView(R.layout.serial);
		
//		mPop.setFocusable(true);
//		mPop.setOutsideTouchable(true);
		 
		sendBtn = (Button) (mPop.findViewById(R.id.btn_serial_send));
		
		serialNum = (Spinner)(mPop.findViewById(R.id.spinner_serialNum));
		serialNum.setOnItemSelectedListener(new SpinnerSelectedListener());
		
		sendClickListener =  new View.OnClickListener(){
			@Override
			public void onClick(View view)
			{
				String serialText = infoText.getText().toString();
				
				if(serialText.isEmpty())
				{
					Toast.makeText(SerialPopWindow.this.mContent, "发送的串口数据为空", Toast.LENGTH_SHORT).show();
					return;
				}
				
				try
				{
					byte[]  tempByte = serialText.getBytes("GBK");
					
	                boolean oneFlag = false,twoFlag = false;
					
					if(serialNumText.compareTo("1")== 0)
						oneFlag = true;
					else if(serialNumText.compareTo("2")== 0)
						twoFlag = true;
					else
					{
						oneFlag = true;
						twoFlag = true;
					}
					
					if(oneFlag)
					  SerialPopWindow.this.mDs.serialSend(0, tempByte, tempByte.length);
					if(twoFlag)
					  SerialPopWindow.this.mDs.serialSend(1, tempByte, tempByte.length);
					Toast.makeText(SerialPopWindow.this.mContent, "发送成功", Toast.LENGTH_SHORT).show();
				
			    }
			    catch(UnsupportedEncodingException e)
			    {
				    Toast.makeText(SerialPopWindow.this.mContent, "串口数据转码失败", Toast.LENGTH_SHORT).show();
			    }
				
			 
			}
			
		};
		
		sendBtn.setOnClickListener(sendClickListener);
		
		infoText = (EditText) (mPop.findViewById(R.id.editText_serialInfo ));
		
		
//		mPop.getContentView().setFocusable(true);
//		mPop.getContentView().setFocusableInTouchMode(true);
//		mPop.getContentView().setOnClickListener(sendClickListener);
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
		
	 
	}
	 
 
	public void show()
	{
	//	mPop.showAtLocation(infoText, Gravity.CENTER, 0, 0);
		mPop.show();
	}
	 
	private View.OnClickListener sendClickListener = null; 
	
	   //使用数组形式操作
    class SpinnerSelectedListener implements AdapterView.OnItemSelectedListener{
 
        public void onItemSelected(AdapterView<?> parent, View view, int pos,
                long id) {
        	serialNumText = (String)parent.getItemAtPosition(pos);
        	
         
        }
 
        public void onNothingSelected(AdapterView<?> arg0) {
        }
    }
	
}
