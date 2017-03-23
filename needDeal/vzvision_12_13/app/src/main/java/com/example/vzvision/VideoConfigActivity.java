package com.example.vzvision;

import android.app.Activity;

import com.device.*;

import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Spinner;
import android.widget.AdapterView;
import android.widget.Toast;
import android.util.Log ;
import android.os.Handler;  
import android.os.Message;   


public class VideoConfigActivity extends Activity {
	
	private GlobalVariable m_gb = null;
	private DeviceSet      mds = null;
	private Spinner     videoFrameSize = null;
	private Spinner     videoFrameRate = null;
	private Spinner     videoEncodeType = null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_video_config);
		
		
		m_gb = (GlobalVariable)getApplicationContext();
		
		mds = m_gb.getDeviceSet();
		
		
		videoFrameSize = (Spinner)this.findViewById(R.id.spinner_videoFrameSize);
		videoFrameRate = (Spinner)this.findViewById(R.id.spinner_videoFrameRate);
		videoEncodeType = (Spinner)this.findViewById(R.id.spinner_videoEncodeType);
		
		
		videoFrameSize.setOnItemSelectedListener( itemClickListener);
		videoFrameRate.setOnItemSelectedListener( itemClickListener);
		videoEncodeType.setOnItemSelectedListener( itemClickListener);
		
		
		  new Thread(runnable).start();
	 
	}
	
	private Handler handler = new Handler(){
	    @Override
	    public void handleMessage(Message msg) {
	        super.handleMessage(msg);
	        Bundle data = msg.getData();
	        String val = data.getString("value");
	        Log.i("mylog","请求结果为-->" + val);
	    }
	};

	Runnable runnable = new Runnable(){
	    @Override
	    public void run() {
	        //
	        // TODO: http request.
	        //
	    	StringBuffer text = new StringBuffer();
	    	mds.getFrameSize(text);
//	        Message msg = new Message();
//	        Bundle data = new Bundle();
//	        data.putString("value","请求结果");
//	        msg.setData(data);
//	        handler.sendMessage(msg);
	    }
	};
	
	  

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.video_config, menu);
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
	 
	
	private AdapterView.OnItemSelectedListener itemClickListener = new AdapterView.OnItemSelectedListener()
	{
		  public void onItemSelected(AdapterView<?> parent, View view, int position, long id)
		  {
			  String sInfo=parent.getItemAtPosition(position).toString();  
	            Toast.makeText(getApplicationContext(), sInfo, Toast.LENGTH_LONG).show();   
	            
	            int viewid =  parent.getId();
	             
	            
	            Log.v("fuck",String.valueOf(viewid));
	            Log.v("fuck",String.valueOf(R.id.spinner_videoFrameSize));
	            Log.v("fuck",String.valueOf(viewid - R.id.spinner_videoFrameSize));
	             
	            
	            if( (viewid - R.id.spinner_videoFrameSize) == 0 )
	            {
	            	int a;
	            	a =0;
	            }
	            
	            if( (viewid - R.id.spinner_videoFrameRate) == 0 )
	            {
	            	int a;
	            	a =0;
	            }
	            
	            if( (viewid - R.id.spinner_videoEncodeType) == 0 )
	            {
	            	int a;
	            	a =0;
	            }
	           
			 
		  }
		  
		  public void onNothingSelected(AdapterView<?> parent)
		  {
			  
		  }
	};
}
