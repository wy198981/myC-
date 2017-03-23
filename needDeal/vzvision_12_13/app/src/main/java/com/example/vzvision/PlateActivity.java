package com.example.vzvision;

import com.database.*;
import com.vz.tcpsdk;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.*;
import android.widget.*;

import java.util.*;

import android.view.LayoutInflater;
import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.graphics.Bitmap;  
import android.graphics.BitmapFactory;  
 
public class PlateActivity extends Activity {
	private ListView listView_plate = null;
    private MyAdspter  myAdpter = null;
	
	private GlobalVariable m_gb = null;
	private boolean      exitFlag = false;
	private SearchPlateThread    m_SearchPlateThread;
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_plate);
		
		listView_plate = (ListView) findViewById(R.id.listView_plate);
		
		
		m_gb = (GlobalVariable)getApplicationContext();;
		
		 List<Map<String, Object>> list=getData();  
		 myAdpter = new MyAdspter(this, list);
		 listView_plate.setAdapter(myAdpter);  
		 
		 m_SearchPlateThread = new SearchPlateThread();
		 m_SearchPlateThread.start();
		 
		 
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.plate, menu);
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
	
	@Override
	protected void onDestroy() {
		super.onDestroy();
		exitFlag = true;
		try
		{
			m_SearchPlateThread.join(1000);
		}
		catch (InterruptedException e)
		{
			
		}
	
	}
	
	private int plateSearchPos = 0;
	public class SearchPlateThread extends Thread
	{
		public  void  run()
		{
			
			while(true)
			{
				 if(exitFlag)
        			 break;
				 
                plateCallbackInfoTable pci = m_gb.getplateCallbackInfoTable();
		        
		        if( pci == null)
		        {
		            break;	
		        }
		        	
		        int rowcount = pci.getRowCount();
		        	
		        plateCallbackInfoTable.plateCallbackElement ele = pci.new plateCallbackElement();
		        	
		        Bitmap bmp;
		        	   
		        if( plateSearchPos >=  rowcount )
		        { 
		        		
		        	try
		        	{
		        		this.sleep(1000);
		        	}
		        	catch(InterruptedException  e)
		        	{
		        	   break;
		        	}
		        	continue;
		       	}
		        		 
		 	    Map<String, Object> map=new HashMap<String, Object>();
		 	             
		 	            if(pci.GetCallbackInfo(plateSearchPos, ele))
		 	            {
		 	            	bmp = BitmapFactory.decodeByteArray(ele.ImageSmallData, 0, ele.ImageSmallData.length);  
		 	            	map.put("shibieImage", bmp);  
			 	            map.put("deviceName",ele.devicename);  
			 	            map.put("plateName",  ele.plateNumber);  
			 	            map.put("shibieTIme", ele.recongnizetime );  
			 	            
			 	           //myAdpter.Add(map);
			 	            
			 	            Message message = new Message();
			 	            message.what = 1;
			 	            message.obj = map;
			 	            handler.sendMessage(message);
			 	            
			 	           plateSearchPos++;
		 	            }
		 	             
		 	            
			 	       	try
		        		{
		        			this.sleep(100);
		        		}
		        		catch(InterruptedException  e)
		        		{
		        		   break;
		        		}
		 	        }  
		  
		}
	};
	
	 public List<Map<String, Object>> getData(){  
	        List<Map<String, Object>> list=new ArrayList<Map<String,Object>>();  
	        return list;
	        
	      
	    }  
	 
	 @SuppressLint("HandlerLeak")
		private Handler handler = new Handler() {
			public void handleMessage(android.os.Message msg) {
				 Map<String, Object> map = ( Map<String, Object>)msg.obj;
				 
				 myAdpter.Add(map);
			}
		
	 };
				
	 
	public class MyAdspter extends BaseAdapter {

		private List<Map<String, Object>> data;
		private LayoutInflater layoutInflater;
		private Context context;
		public MyAdspter(Context context,List<Map<String, Object>> data){
			this.context=context;
			this.data=data;
			this.layoutInflater=LayoutInflater.from(context);
		}
		/**
		 * 组件集合，对应list.xml中的控件
		 * @author Administrator
		 */
		public final class Zujian{
			public ImageView image;
			public TextView plateName;
			public TextView deviceName;
			public TextView shibieTime;
		}
		@Override
		public int getCount() {
			return data.size();
		}
		/**
		 * 获得某一位置的数据
		 */
		@Override
		public Object getItem(int position) {
			return data.get(position);
		}
		/**
		 * 获得唯一标识
		 */
		@Override
		public long getItemId(int position) {
			return position;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			Zujian zujian=null;
			if(convertView==null){
				zujian=new Zujian();
				//获得组件，实例化组件
				convertView=layoutInflater.inflate(R.layout.list, null);
				zujian.image=(ImageView)convertView.findViewById(R.id.imageView_plate);
				zujian.plateName=(TextView)convertView.findViewById(R.id.textView_plate);
				zujian.deviceName=(TextView)convertView.findViewById(R.id.textView_VideoDeviceName);
			 
				zujian.shibieTime=(TextView)convertView.findViewById(R.id.textView_shibieTime);
				convertView.setTag(zujian);
			}else{
				zujian=(Zujian)convertView.getTag();
			}
			Bitmap t =  (Bitmap)data.get(position).get("shibieImage");
			//绑定数据
			zujian.image.setImageBitmap((Bitmap)data.get(position).get("shibieImage") );//setBackgroundResource((Integer)data.get(position).get("image"));
			zujian.plateName.setText((String)data.get(position).get("plateName"));
			zujian.deviceName.setText((String)data.get(position).get("deviceName"));
			
			String text = (String)data.get(position).get("shibieTIme");
			zujian.shibieTime.setText((String)data.get(position).get("shibieTIme"));
			
			return convertView;
		}
		
		
		public void Add(Map<String, Object> item)
		{
//			Map<String, Object> map=new HashMap<String, Object>();  
//            map.put("image", R.drawable.ic_launcher);  
//            map.put("title", "这是一个标题"+data.size());  
//            map.put("info", "这是一个详细信息"+data.size());  
//            
//            data.add(map);
			data.add(item);
            this.notifyDataSetChanged();   
		}

	}
	
//	  private View.OnClickListener clickListener =  new View.OnClickListener(){
//			@Override
//			public void onClick(View view)
//			{
//				if(!myAdpter.isEmpty())
//				{
//					myAdpter.Add();
//				
//					myAdpter.notifyDataSetChanged();
//				}
//			}
//	  };
	
     
//     String mstrTitle = "无视频";
//	    Bitmap bmp = Bitmap.createBitmap(256, 256, Bitmap.Config.ARGB_8888);
//     Canvas canvasTemp = new Canvas(bmp);
//     canvasTemp.drawColor(Color.BLACK);
//     Paint p = new Paint();
//     String familyName = "宋体";
//     Typeface font = Typeface.create(familyName, Typeface.BOLD);
//     p.setColor(Color.RED);
//     p.setTypeface(font);
//     p.setTextSize(27);
//     canvasTemp.drawText(mstrTitle, 0, 100, p);
     
 //   BitmapFactory.decodeByteArray(data, 0, data.length);  

}





