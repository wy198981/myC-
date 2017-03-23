package com.example.vzvision;

import android.annotation.SuppressLint;

import java.io.UnsupportedEncodingException;

import android.app.Activity;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.Menu;
import android.view.MenuItem;
 
















import java.util.List;
import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;

 














import com.vz.WlistVehicle;
import com.vz.tcpsdk;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.View.OnClickListener;
 
import android.widget.BaseExpandableListAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.ExpandableListView;
import android.widget.Toast;
import android.widget.ExpandableListView.OnChildClickListener;
import android.widget.LinearLayout;
import android.widget.Button;
 
public class WlistActivity extends Activity implements tcpsdk.onWlistReceiver {
	public static final int   wlist_edit = 0x0001;
	public static final int   wlist_add = 0x0002;
	public static final int   wlist_delete = 0x0003;
	public static final int   wlist_recv = 0x0004;
	public static final String[] colorTextGroup={  "��", "����", "��","��","��","��","��","��", "��",};
	public static final String[] plateTypeGroup = {"δ֪����", "����С����", "����С����", "���Ż���", "˫�Ż���" ,"��������",  "�侯����",
	         "���Ի�����", "���ž���","˫�ų���", "ʹ�ݳ���", "��۽����ڳ���", "ũ�ó���", "��������", "���Ž����й�", "˫���侯����", "�侯�ܶӳ���", "˫���侯�ܶӳ���",
	};
	public static final String[] enableGroup = {"��","��"};
	
	
	
	  private MainListExpandableListAdapter adapter = null;  
	  private  List<Map<String, Object>> groups;  
	  private  List<List<Map<String, Object>>> childs;  
	  private  ExpandableListView expandableListView;  
	  private  ImageView         addWlistView;
	  private GlobalVariable m_gb = null;
	  private SearchWlistThread  searchThread =null;
	  private int      operationItemNum = 0;       
	    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_wlist);
		
		m_gb = (GlobalVariable)getApplicationContext();
		 
		addWlistView =  (ImageView)findViewById(R.id.imageView_addWlist);  
		
		addWlistView.setOnClickListener(new  View.OnClickListener(){
			  @Override
		        public void onClick(View v)
		        {
				  Message message = new Message();
                  message.what = wlist_add;
                  message.arg1 = 0;
                  handler.sendMessage(message);
				   
		        }
		});
	       
        //ΪExpandableListView׼������  
        groups = new ArrayList<Map<String, Object>>();  
        
//        for(int a = 0 ; a < 10 ; a++)
//        {
//        	 Map<String, Object> group = new HashMap<String, Object>();  
//             group.put("title", "�ҵļ���");  
//             groups.add(group);
//        }
         
  
        childs = new ArrayList<List<Map<String, Object>>>();  
//        for(int j = 0 ; j < 10 ; j++ )
//        {  
//        	  List<Map<String, Object>> child1 = new ArrayList<Map<String, Object>>();  
//              for(int i = 0 ; i < 1 ; i++ )
//             {  
//                 Map<String, Object> child1Data1 = new HashMap<String, Object>();  
//                 child1Data1.put("nickName", "sdfsdf");  
//                 child1Data1.put("phone", "sdfsdf");  
//                 child1Data1.put("ico", "sdfsdf");  
//                 child1.add(child1Data1);  
//             }  
//              
//              childs.add(child1);  
//        }
        
       
  
        // ʵ����ExpandableListView����  
        expandableListView = (ExpandableListView) findViewById(R.id.expandableListView_Wlist);  
        // ʵ����ExpandableListView��������  
        adapter = new MainListExpandableListAdapter(getApplicationContext(), groups, childs);  
  
        // ����������  
        expandableListView.setAdapter(adapter);  
         
        // ���ü�����  
        expandableListView.setOnChildClickListener(new OnChildClickListener() {  
  
            public boolean onChildClick(ExpandableListView parent, View v,  
                    int groupPosition, int childPosition, long id) {  
//                Log.d("test", "GroupPosition is " + groupPosition);  
//                Log.d("test", "ChildPosition is" + childPosition);  
                return false;  
            }  
        }); 
        
        
        
        m_gb.getDeviceSet().setWlistInfoCallBack(this);
     
        searchThread = new SearchWlistThread();
        
        searchThread.start();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.wlist, menu);
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
		 
		m_gb.getDeviceSet().setWlistInfoCallBack(null);
		try
		{
			searchThread.join(1000);
		}
		catch (InterruptedException e)
		{
			
		}
	
	}
	
	public void onWlistReceive(int handle, WlistVehicle wlist  )
	{ 
		 Message message = new Message();
         message.what = wlist_recv;
         message.obj = wlist;
         handler.sendMessage(message);
	}
	
	public class SearchWlistThread extends Thread
	{
		public  void  run()
		{
			byte[] plateCode = new byte[5];
		    m_gb.getDeviceSet().queryWlistVehicle(plateCode);
		}
	}
	
	 
	public class MainListExpandableListAdapter extends BaseExpandableListAdapter {
		
		//��Ԫ��
		class ExpandableListHolder {
			TextView plateID;
			TextView plateName;
			TextView plateEnable;
			TextView plateStartTime;
			TextView plateOverTime;
			TextView plateBlackList;
			TextView plateColor;
			TextView plateType;
			TextView plateCode;
			TextView plateComment;
			
			
//			TextView nickName;
//			TextView phone;
//			ImageView ioc;
		} 
		
		//����Ԫ
		class ExpandableGroupHolder {
			TextView title;
		} 
		
		private List<Map<String, Object>> groupData;//����ʾ
		private List<List<Map<String, Object>>> childData;//���б�
		
		private List<WlistVehicle>    wlistGroup;
		
		private LayoutInflater mGroupInflater; //���ڼ���group�Ĳ���xml
		private LayoutInflater mChildInflater; //���ڼ���listitem�Ĳ���xml
		
		//�Ա��幹��
		public MainListExpandableListAdapter(Context context, List<Map<String, Object>> groupData, List<List<Map<String, Object>>> childData) {
			this.childData=childData;
			this.groupData=groupData;
			
			wlistGroup = new ArrayList<WlistVehicle>();
			
			mChildInflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
			mGroupInflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		}
		
		//����ʵ�� �õ�������
		@Override
		public Object getChild(int groupPosition, int j) {
			return null;//childData.get(groupPosition).get(j);
		}

		@Override
		public long getChildId(int groupPosition, int j) {
			return groupPosition;
		}

		@Override
		public int getChildrenCount(int i) {
			return 1;//wlistGroup.size();//childData.get(i).size();
		}

		@Override
		public Object getGroup(int i) {
			return null;//groupData.get(i);
		}

		@Override
		public int getGroupCount() {
			return wlistGroup.size();//groupData.size();
		}

		@Override
		public long getGroupId(int i) {
			return i;
		}

		@Override
		public boolean hasStableIds() {//���Ƿ����Ψһid
			return false;
		}

		@Override
		public boolean isChildSelectable(int i, int j) {//���Ƿ��ѡ
			return false;
		}
		
		@Override
		public View getGroupView(int groupPosition, boolean flag, View convertView, ViewGroup viewgroup) {
			ExpandableGroupHolder holder = null; //�����ʱ����holder
			if (convertView == null) { //�ж�view����view�Ƿ��ѹ����ã��Ƿ�Ϊ��

				convertView = mGroupInflater.inflate(R.layout.wlist_group, null);
				Button button = (Button) convertView.findViewById(R.id.button_wlistEdit);
				button.setOnClickListener(new  buttonclickListener(groupPosition));
				
				
				Button button2 = (Button) convertView.findViewById(R.id.Button_wlistDelete);
				button2.setOnClickListener(new  buttonclickListener(groupPosition));
				
				holder = new ExpandableGroupHolder();
				holder.title=(TextView) convertView.findViewById(R.id.textView_wlistPlate);
				convertView.setTag(holder);
			} else { //��view��Ϊ�գ�ֱ�Ӵ�view��tag�����л�ø�����ͼ������
				holder = (ExpandableGroupHolder) convertView.getTag();
			}
			//String title=(String)this.groupData.get(groupPosition).get("title");\
			try
			{
				String title = new String(wlistGroup.get(groupPosition).strPlateID,"UTF-8");
				holder.title.setText(title);
				
			}
			catch (UnsupportedEncodingException e)
			{
				
			}
		
			
			
			   LinearLayout layout = (LinearLayout) convertView.findViewById(R.id.LinearLayout_wlistGroup);
			   
			   expendclickListener listener = new expendclickListener(flag,groupPosition);
	            layout.setOnClickListener(listener);
			return convertView;
		}
		
		@Override
		public View getChildView(int groupPosition, int childPosition, boolean isLastChild, View convertView,
				ViewGroup viewgroup) {
			ExpandableListHolder holder = null;
			if (convertView == null) {
				convertView = mChildInflater.inflate(R.layout.wlist_group_child, null);
			  
				holder = new ExpandableListHolder();
				holder.plateID = (TextView) convertView.findViewById(R.id.textView_plateID);
				holder.plateName = (TextView) convertView.findViewById(R.id.textView_plateName);
				holder.plateEnable = (TextView) convertView.findViewById(R.id.TextView_plateEnable);
				holder.plateStartTime = (TextView) convertView.findViewById(R.id.TextView_plateStartTime);
				
				holder.plateBlackList = (TextView) convertView.findViewById(R.id.TextView_plateBlackList);
				holder.plateColor = (TextView) convertView.findViewById(R.id.textView_plateColor);
				holder.plateType = (TextView) convertView.findViewById(R.id.TextView_plateType);
				
				holder.plateCode = (TextView) convertView.findViewById(R.id.TextView_plateCode);
				holder.plateComment = (TextView) convertView.findViewById(R.id.TextView_plateComment);
				holder.plateOverTime = (TextView) convertView.findViewById(R.id.TextView_plateOverTime);
				convertView.setTag(holder);
			} else {//�����ѳ�ʼ����ֱ�Ӵ�tag���Ի������ͼ������
				holder = (ExpandableListHolder) convertView.getTag();
			} 
//			Map<String,Object> unitData=this.childData.get(groupPosition).get(childPosition);
			
			WlistVehicle vehicle = get(groupPosition);
			
			String plateID = String.valueOf(vehicle.uVehicleID);
			holder.plateID.setText(plateID);
			
			if(vehicle.bEnable == 0)
			  holder.plateEnable.setText("��");
			else
			  holder.plateEnable.setText("��"); 
			 		 
			try
			{
				  String plateName = new String(vehicle.strPlateID,"UTF-8");
					holder.plateName.setText( plateName);
					
			}
			catch( UnsupportedEncodingException e)
			{
				
			}
			
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
			
			holder.plateStartTime.setText( DateTime);
			
			
			if(vehicle.bEnableTMOverdule >0 )
	    	{
	    		  DateTime = new String("");
	    		   
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
	    		  
	    		holder.plateOverTime.setText(DateTime);
	    	}
	    	  
	    	
			holder.plateColor.setText(colorTextGroup[vehicle.iColor]);
	    	
			holder.plateType.setText(plateTypeGroup[vehicle.iPlateType]);
	    	
			holder.plateBlackList.setText(enableGroup[(int)vehicle.bAlarm]); 
	     
	    	try
	    	{
//	    		String codeText = new String(vehicle.strCode,"GBK");
//	        	m_plateCode.setText( codeText);
	        	
	        	String commentText = new String(vehicle.strComment,"UTF-8");
	        	holder.plateComment.setText( commentText);
	    	}
	    	catch(UnsupportedEncodingException e)
	    	{
	    		Toast.makeText(WlistActivity.this, "���ƺ�ת��ʧ��", Toast.LENGTH_SHORT).show();
	    	}
	    	
           
			return convertView;
		}
		
		
		void  add(WlistVehicle wlist)
		{
			wlistGroup.add(wlist);
			notifyDataSetChanged();
		}
		
		void  edit(int num,WlistVehicle wlist)
		{
			wlistGroup.set(num, wlist);
			notifyDataSetChanged();
		}
		
		void  delete( int num )
		{
			wlistGroup.remove(num);
			notifyDataSetChanged();
		}
		
		WlistVehicle get(int num)
		{
			return wlistGroup.get(num);
		}
	}
	
	 @SuppressLint("HandlerLeak")
		private Handler handler = new Handler() {
			public void handleMessage(android.os.Message msg) {
				switch (msg.what) {
				case wlist_add:
				{
					Intent intent = new Intent(WlistActivity.this,WListEditActivity.class);
					
					WlistVehicle  vehicle = new WlistVehicle();
					
					m_gb.setWlistVehicle(vehicle);
					
					intent.putExtra("oper", "add");
				    
					startActivityForResult(intent,wlist_add);
				}
				break;
				case wlist_delete:
				{
					WlistVehicle  vehicle = adapter.get(msg.arg1);
					if(vehicle != null)
					    m_gb.getDeviceSet().deleteWlistVehicle( vehicle.strPlateID);
					else
						Toast.makeText(WlistActivity.this, "ɾ���豸������ʧ��", Toast.LENGTH_SHORT).show();
					
					adapter.delete(msg.arg1);
				}
				break;
				case wlist_edit:
				{
					WlistVehicle  vehicle = adapter.get(msg.arg1);
					
					operationItemNum = msg.arg1;
					
					Intent intent = new Intent(WlistActivity.this,WListEditActivity.class);
					 
					m_gb.setWlistVehicle(vehicle);
					
					intent.putExtra("oper", "edit");
				
				    startActivityForResult(intent,wlist_edit);
				}
				break;
				case wlist_recv:
				{
					WlistVehicle  vehicle = (WlistVehicle)msg.obj;
					adapter.add(vehicle);
				}
				break;
			}
		 }
	 };
	 
	protected void onActivityResult(int requestCode, int resultCode, Intent intent)
	{
		if(wlist_add == requestCode )
		{
			if(resultCode == RESULT_OK )
			{
				WlistVehicle  vehicle = m_gb.getWlistVehicle();
				
				adapter.add(vehicle);
			}
			else
			{
				
			}
		}
		else if( wlist_edit == requestCode)
		{
			if(resultCode == RESULT_OK )
			{
				WlistVehicle  vehicle = m_gb.getWlistVehicle();
				
				adapter.edit(this.operationItemNum,vehicle);
			}
			else
			{
				
			}
		}
			
 	}
	
	private class expendclickListener implements  View.OnClickListener
    {
		private boolean isExpanded = false;
		private int groupPosition = 0;
		
		public expendclickListener(boolean explandflag, int position)
		{
			isExpanded = explandflag;
			groupPosition = position;
		}
		
        @Override
        public void onClick(View v)
        {
            if (isExpanded)
            {
                expandableListView.collapseGroup(groupPosition);
            } else
            {
                expandableListView.expandGroup(groupPosition);
            }
        }
    }
	
	private class buttonclickListener implements  View.OnClickListener
    {
		private  int m_postion = 0;
		public buttonclickListener( int pos)
		{
			m_postion = pos;
		}
		 
        @Override
        public void onClick(View v)
        {
        	int  id = v.getId();
            switch(v.getId())
            {
            case R.id.button_wlistEdit:
            {
            	   Message message = new Message();
                   message.what = wlist_edit;
                   message.arg1 = m_postion;
                   handler.sendMessage(message);
            }
            	break;
            case R.id.Button_wlistDelete:
            {
            	 Message message = new Message();
                 message.what = wlist_delete;
                 message.arg1 = m_postion;
                 handler.sendMessage(message);
            }
            	break;	
            
            }
        }
    }
	 
}
