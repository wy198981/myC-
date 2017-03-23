package com.example.vzvision;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.database.SnapImageTable;
import com.database.SnapImageTable.SnapImageElement;
import com.example.vzvision.PlateActivity.MyAdspter;
import com.example.vzvision.PlateActivity.MyAdspter.Zujian;

import android.app.Activity;
import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.util.Base64;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.graphics.Typeface;
import android.graphics.Bitmap;  
import android.graphics.BitmapFactory;  
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;

public class SnapImageActivity extends Activity {
	private ListView listView_snapImage = null;
    private MyAdspter  myAdpter = null;
	
	private GlobalVariable m_gb = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_snap_image);
		
		
		listView_snapImage = (ListView)this.findViewById(R.id.listView_snapImage);
		
		m_gb = (GlobalVariable)getApplicationContext();
		
		
		 List<Map<String, Object>> list=getData();  
		 myAdpter = new MyAdspter(this, list);
		 listView_snapImage.setAdapter(myAdpter);  
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.snap_image, menu);
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
	
	protected  List<Map<String, Object>> getData()
	{
		 List<Map<String, Object>> list=new ArrayList<Map<String,Object>>();  
	        
		  
	        SnapImageTable pci = m_gb.getSnapImageTable();
	        
	        if( pci != null)
	        {
	        	int rowcount = pci.getRowCount();
	        	
	        	SnapImageTable.SnapImageElement ele = pci.new SnapImageElement();
	        	
	        	Bitmap bmp;
	        	
	        	 for (int i = 0; i < rowcount; i++) {  
	 	            Map<String, Object> map=new HashMap<String, Object>();
	 	            
	 	            
//	 		        String mstrTitle = "无视频";
//	 			    Bitmap bmp = Bitmap.createBitmap(256, 256, Bitmap.Config.ARGB_8888);
//	 		        Canvas canvasTemp = new Canvas(bmp);
//	 		        canvasTemp.drawColor(Color.BLACK);
//	 		        Paint p = new Paint();
//	 		        String familyName = "宋体";
//	 		        Typeface font = Typeface.create(familyName, Typeface.BOLD);
//	 		        p.setColor(Color.RED);
//	 		        p.setTypeface(font);
//	 		        p.setTextSize(27);
//	 		        canvasTemp.drawText(mstrTitle, 0, 100, p);
	 	            
	 	        //   BitmapFactory.decodeByteArray(data, 0, data.length);  
	 	            
	 	            if(pci.get(i, ele))
	 	            {
	 	            	try
	 	            	{
	 	            		 byte [] realImgData =  Base64.decode(ele.ImageData,  Base64.NO_PADDING);
	 	            		 
	 	            		 BitmapFactory.Options options = new BitmapFactory.Options(); 
	 	            		 
	 	            		 if( realImgData.length > 50000)
	 	            		 {
	 	            			options.inSampleSize = 4;//图片宽高都为原来的二分之一，即图片为原来的四分之一 
		 	                     options.inInputShareable  = true; 
	 	            		 }
	 	            		 else
	 	            		 {
	 	            			options.inSampleSize = 1;//图片宽高都为原来的二分之一，即图片为原来的四分之一 
		 	                     options.inInputShareable  = true;
	 	            		 }
	 	            		 
	 	            		 
	 	 	            	
	 	 	            	bmp = BitmapFactory.decodeByteArray(realImgData, 0, realImgData.length,options);  
	 	 	            	
	 	 	            	if( bmp !=null)
	 	 	            	{
	 	 	            	  map.put("date", ele.date);  
	 		 	              map.put("img",bmp);  
	 	 	            	}
	 	 	            	 list.add(map); 
	 	            	}
	 	            	catch(IllegalArgumentException e)
	 	            	{
	 	            		int a;
	 	                	 a= 0;
	 	            	}
	 	                catch(Exception e)
	 	                {
	 	                	int a;
	 	                	 a= 0;
	 	                }
	 	            }
	 	             
	 	        }  
	        }
	        	 
	       return list;
	       
	}
	
	
	
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
			public ImageView snapImage;
			public TextView dateText;
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
				convertView=layoutInflater.inflate(R.layout.snap_image_item, null);
				zujian.snapImage=(ImageView)convertView.findViewById(R.id.imageView_snapImage);
				zujian.dateText=(TextView)convertView.findViewById(R.id.textView_date);
				 
				convertView.setTag(zujian);
			}else{
				zujian=(Zujian)convertView.getTag();
			}
			Bitmap t =  (Bitmap)data.get(position).get("img");
			//绑定数据
			zujian.snapImage.setImageBitmap((Bitmap)data.get(position).get("img") );//setBackgroundResource((Integer)data.get(position).get("image"));
			zujian.dateText.setText((String)data.get(position).get("date"));
			 
			
			return convertView;
		}
		
		
 

	}
}
