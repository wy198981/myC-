package com.example.vzvision;

import java.io.UnsupportedEncodingException;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.*;
import android.view.*;

import java.util.Calendar;
import java.util.Date;
import java.text.SimpleDateFormat;
import java.text.ParseException;

import com.vz.*;
import com.xjs.example.time.view.DateTimePickerView;

public class WListEditActivity extends Activity {
	private Button      chooseStartTime;
	private Button      chooseOverTime;
	private Button      confirm;
	private boolean     addFlag= false;
	 private GlobalVariable m_gb = null;
	 
	 private  int       m_editDateId=0;
	 private TextView   m_title;
	 
	 private   TextView  m_staticStartTime;
	 
	 private	TextView m_plateName;
	 private	Spinner m_plateEnable;
	 private	TextView m_plateStartTime;
	 private	TextView m_plateOverTime;
	 private	Spinner m_plateBlackList;
	 private	Spinner m_plateColor;
	 private	Spinner m_plateType;
	 private	TextView m_plateCode;
	 private	EditText m_plateComment;
		
	private DateTimePickerView mDateTimePickerView;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_wlist_edit);
		
		 
		m_title = (TextView) findViewById(R.id.textView_wlistTitle);
		 m_plateName = (TextView)  findViewById(R.id.editText_plateName);
		 m_plateEnable = (Spinner)  findViewById(R.id.spinner_enable);
		 m_plateStartTime = (TextView)  findViewById(R.id.editText_startTime);
		
		 m_plateOverTime = (TextView)  findViewById(R.id.editText_overTime);
		 m_plateBlackList = (Spinner)  findViewById(R.id.spinner_blacklistEnable);
		 m_plateColor = (Spinner)  findViewById(R.id.spinner_plateColor);
		
		 m_plateType = (Spinner)  findViewById(R.id.spinner_plateType);
		// m_plateCode = (TextView)  findViewById(R.id.textView_plateCode);
		 m_plateComment = (EditText)  findViewById(R.id.editText_plateComment);
		 
		 chooseStartTime = (Button) findViewById(R.id.button_chooseStartDate);
		 chooseStartTime.setOnClickListener(clickListener);
		 
		 chooseOverTime = (Button) findViewById(R.id.button_chooseOverDate);
		 chooseOverTime.setOnClickListener(clickListener);
		 
		 confirm = (Button)findViewById(R.id.button_WlistEdit_Ok);
		 confirm.setOnClickListener(clickListener);
		 
		 m_staticStartTime = (TextView)  findViewById(R.id.textView_staticStartTime);
		 
		 mDateTimePickerView = new DateTimePickerView(this, new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					int id = v.getId();
					if(id == R.id.btn_ok){
						 if(R.id.button_chooseStartDate == m_editDateId )
						 {
							 m_plateStartTime.setText(mDateTimePickerView.wheelMain.getTime());
						 }
						 else if(R.id.button_chooseOverDate == m_editDateId )
						 {
							 m_plateOverTime.setText(mDateTimePickerView.wheelMain.getTime());
						 }
					} 
					
					mDateTimePickerView.dismiss();
				}
			});
	        mDateTimePickerView.setOutsideTouchable(true);
//	        mDateTimePickerView.setWidth(mTime.getWidth());
//	        mDateTimePickerView.showAsDropDown(mTime);
		
		
		m_gb = (GlobalVariable)getApplicationContext();
		
		Intent intent = this.getIntent();
		String operation = intent.getStringExtra("oper");
		
		if(operation.compareTo("edit") == 0)
		{
			m_title.setText("编辑白名单");
			FillUIData();
			addFlag = false;
		}
		else 
		{
			m_title.setText("添加白名单");
			addFlag = true;
		}
		
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.wlist_edit, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings) {
			return true;
		}
		return super.onOptionsItemSelected(item);
	}
	
    private void  FillUIData()
    {
    	WlistVehicle vehicle = m_gb.getWlistVehicle();
    	
    	try
		{
			  String plateName = new String(vehicle.strPlateID,"UTF-8");
			  m_plateName.setText( plateName);
				
		}
		catch( UnsupportedEncodingException e)
		{
			Toast.makeText(WListEditActivity.this, "车牌号转换失败", Toast.LENGTH_SHORT).show();
		}
    	
    	if(vehicle.bEnable == 0)
    		m_plateEnable.setSelection(0);
    	else
    		m_plateEnable.setSelection(1);
    	
    	 
    	
    	if(vehicle.bEnableTMEnable >0 )
    	{
    		String DateTime = new String("");
    		   
    		DateTime += vehicle.struTMEnable.nYear;
    		DateTime += "-";
    		
    		DateTime += vehicle.struTMEnable.nMonth;
    		DateTime += "-";
    		
    		DateTime += vehicle.struTMEnable.nMDay;
    		DateTime += " ";
    		
    		DateTime += vehicle.struTMEnable.nHour;
    		DateTime += ":";
    		
    		DateTime += vehicle.struTMEnable.nMin;
    		DateTime += ":";
    		
    		DateTime += vehicle.struTMEnable.nSec;
    		DateTime += ":";
    		  
    		m_plateStartTime.setText(DateTime);
    	}
    	
    	if(vehicle.bEnableTMOverdule >0 )
    	{
    		String DateTime = new String("");
    		   
    		DateTime += vehicle.struTMOverdule.nYear;
    		DateTime += "-";
    		
    		DateTime += vehicle.struTMOverdule.nMonth;
    		DateTime += "-";
    		
    		DateTime += vehicle.struTMOverdule.nMDay;
    		DateTime += " ";
    		
    		DateTime += vehicle.struTMOverdule.nHour;
    		DateTime += ":";
    		
    		DateTime += vehicle.struTMOverdule.nMin;
    		DateTime += ":";
    		
    		DateTime += vehicle.struTMOverdule.nSec;
    		DateTime += ":";
    		  
    		m_plateOverTime.setText(DateTime);
    	}
    	  
    	
    	m_plateColor.setSelection(vehicle.iColor);
    	
    	m_plateType.setSelection(vehicle.iPlateType);
    	
    	m_plateBlackList.setSelection((int)vehicle.bAlarm); 
     
    	try
    	{
//    		String codeText = new String(vehicle.strCode,"GBK");
//        	m_plateCode.setText( codeText);
        	
        	String commentText = new String(vehicle.strComment,"UTF-8");
        	m_plateComment.setText( commentText);
    	}
    	catch(UnsupportedEncodingException e)
    	{
    		Toast.makeText(WListEditActivity.this, "车牌号转换失败", Toast.LENGTH_SHORT).show();
    	}
    	
    	 
    }
    
    public void getWlistVehicle(WlistVehicle vehicle)
    {
    	
    	try
		{ 
			  String plateName = m_plateName.getText().toString();
			  
			  vehicle.strPlateID = plateName.getBytes("UTF-8");
			  //m_plateName.setText( plateName);
			  if(addFlag)
			    vehicle.strCode =  vehicle.strPlateID;
			  
			  
		}
		catch( UnsupportedEncodingException e)
		{
			Toast.makeText(WListEditActivity.this, "车牌号转换失败", Toast.LENGTH_SHORT).show();
		}
    	
    	
       SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");  
       	String DateTime = m_plateStartTime.getText().toString();
       	
       	if(DateTime.compareTo("") != 0)
       	{
       		vehicle.bEnableTMEnable = 1;
       		
       		try{
       			Date date = dateFormat.parse( DateTime);
          		 
       			Calendar calendar = Calendar.getInstance();    //静态方法初始化，默认得到当前
       			calendar.setTime(date);  
          
          		 
           		vehicle.struTMEnable.nYear = (short)calendar.get(Calendar.YEAR);
           		vehicle.struTMEnable.nMonth = (short)calendar.get(Calendar.MONTH);
           		vehicle.struTMEnable.nMDay = (short)calendar.get(Calendar.DAY_OF_MONTH);
           		
           		vehicle.struTMEnable.nHour = (short)calendar.get(Calendar.MINUTE);
           		vehicle.struTMEnable.nMin = (short)calendar.get(Calendar.MINUTE);
           		vehicle.struTMEnable.nSec = (short)calendar.get(Calendar.SECOND);
       		}
       		catch(  ParseException e)
       		{
       			Toast.makeText(WListEditActivity.this, "车牌号转换失败", Toast.LENGTH_SHORT).show();
       		}
       		
       	}
       	else
       	{
       		vehicle.bEnableTMEnable = 0;
       	}
       	
       	DateTime = m_plateOverTime.getText().toString();
       	
       	if(DateTime.compareTo("") != 0)
       	{
       		vehicle.bEnableTMOverdule = 1;
       		
       		try{

           		Date date = dateFormat.parse( DateTime);
           		
           		Calendar calendar = Calendar.getInstance();    //静态方法初始化，默认得到当前
       			calendar.setTime(date);  
          
          		 
           		vehicle.struTMOverdule.nYear = (short)calendar.get(Calendar.YEAR);
           		vehicle.struTMOverdule.nMonth = (short)calendar.get(Calendar.MONTH);
           		vehicle.struTMOverdule.nMDay = (short)calendar.get(Calendar.DAY_OF_MONTH);
           		
           		vehicle.struTMOverdule.nHour = (short)calendar.get(Calendar.MINUTE);
           		vehicle.struTMOverdule.nMin = (short)calendar.get(Calendar.MINUTE);
           		vehicle.struTMOverdule.nSec = (short)calendar.get(Calendar.SECOND);
           	
       		}
       		catch( ParseException e )
       		{
       			Toast.makeText(WListEditActivity.this, "车牌号转换失败", Toast.LENGTH_SHORT).show();
       		}
       		 
       		 
       	
       	}
       	else
       	{
       		vehicle.bEnableTMOverdule = 0;
       	}
       	
    	 
    	vehicle.bEnable = m_plateEnable.getSelectedItemPosition();
    	vehicle.iColor = m_plateColor.getSelectedItemPosition();
    	vehicle.iPlateType = m_plateType.getSelectedItemPosition();
    	
    	vehicle.bAlarm = m_plateBlackList.getSelectedItemPosition();
    	 
    	
    	try
    	{
//    	    if(!addFlag)
//    		  vehicle.strCode = m_plateCode.getText().toString().getBytes("GBK");
    		vehicle.strComment = m_plateComment.getText().toString().getBytes("UTF-8");
        	
         
    	}
    	catch(UnsupportedEncodingException e)
    	{
    		Toast.makeText(WListEditActivity.this, "车牌号转换失败", Toast.LENGTH_SHORT).show();
    	}
    	 
    }
    
    private void  editDate( int editDateId )
    {
    	m_editDateId =editDateId;
    	
    	SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
    	Calendar calendar = Calendar.getInstance();    //静态方法初始化，默认得到当前
    	
    	
    	switch(m_editDateId)
    	{
    	case R.id.button_chooseStartDate:
    	{
            String DateTime = m_plateStartTime.getText().toString();
    	       	
    	    try
    	    {
    	       	Date date = dateFormat.parse( DateTime);
    	       		 
    	   		calendar.setTime(date);  
    	    		 
    		    mDateTimePickerView.showAsDropDown(m_plateStartTime); 
    	    }
    	    catch(ParseException  e)
    	    {
    	    	Toast.makeText(WListEditActivity.this, "分析日期出错", Toast.LENGTH_SHORT).show();
    	    }
    	     
       	
    	}
    		break;
    	case R.id.button_chooseOverDate:
    	{  
    		String DateTime = m_plateOverTime.getText().toString();
  	       	
      	    try
      	    {
      	       	Date date = dateFormat.parse( DateTime);
      	       		 
      	   		calendar.setTime(date);  
      	    		 
      		    mDateTimePickerView.showAsDropDown(m_plateOverTime); 
      	    }
      	    catch(ParseException  e)
      	    {
      	    	Toast.makeText(WListEditActivity.this, "分析日期出错", Toast.LENGTH_SHORT).show();
      	    }
    	}
    		break;
    	}
    }
    
	private buttonclickListener clickListener = new buttonclickListener();
	private class buttonclickListener implements  View.OnClickListener
    {
//		private  int m_postion = 0;
//		public buttonclickListener( int pos)
//		{
//			m_postion = pos;
//		}
		 
        @Override
        public void onClick(View v)
        {
        	int id = v.getId();
        	
        	if(id != R.id.button_WlistEdit_Ok)
        	  editDate(id);
        	else
        	{
        		WlistVehicle  vehicle = m_gb.getWlistVehicle();
        		
        		getWlistVehicle(vehicle);
        		
        		
        		m_gb.getDeviceSet().importWlistVehicle( vehicle);
        		
        		WListEditActivity.this.setResult(RESULT_OK);
        		WListEditActivity.this.finish();
        	}
        }
    };
    
    
        
}
