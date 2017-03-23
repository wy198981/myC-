package com.example.vzvision;

import java.io.UnsupportedEncodingException;




import android.content.*;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.PopupWindow;
import android.widget.Spinner;
import android.widget.Toast;
import android.widget.RelativeLayout.LayoutParams;

import com.device.*;
import com.example.vzvision.SerialPopWindow.SpinnerSelectedListener;

import android.widget.*;
import android.app.Dialog;
import  android.view.Window;
import android.widget.*;

public class GatePopWindow extends BussionPopWindow{
	private Button  openGateBtn;
	 
	private Spinner  gateNum;
	private String   gateNumText="1";

	public  GatePopWindow(Context content,DeviceSet ds)
	{
		  super(content, ds);
		  
		  
	}
	
	
	protected void init()
	{
//		mPop = new PopupWindow( lf.inflate(R.layout.serial, null),
//				 LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT);
		mPop = new Dialog(mContent );
		
		mPop.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mPop.setContentView(R.layout.gate);
		
//		mPop.setFocusable(true);
//		mPop.setOutsideTouchable(true);
		 
		openGateBtn = (Button) (mPop.findViewById(R.id.button_openGate));
		
		gateNum = (Spinner)(mPop.findViewById(R.id.spinner_gateNum));
		gateNum.setOnItemSelectedListener(new SpinnerSelectedListener());
		
		sendClickListener =  new View.OnClickListener(){
			@Override
			public void onClick(View view)
			{
				short gateNum =(short)(Integer.parseInt(gateNumText)-1);
  
				   int res =  GatePopWindow.this.mDs.setIoOutputAuto(gateNum ,500 );
					 
				   if(res == 0)
					  Toast.makeText(GatePopWindow.this.mContent, "开闸成功", Toast.LENGTH_SHORT).show();
				   else
					  Toast.makeText(GatePopWindow.this.mContent, "开闸失败", Toast.LENGTH_SHORT).show();
			   
			}
			
		};
		
		openGateBtn.setOnClickListener(sendClickListener);
		 
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
        	gateNumText = (String)parent.getItemAtPosition(pos);
        	
         
        }
 
        public void onNothingSelected(AdapterView<?> arg0) {
        }
    }
}
