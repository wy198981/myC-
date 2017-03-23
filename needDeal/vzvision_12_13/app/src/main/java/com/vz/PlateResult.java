package com.vz;

import java.nio.charset.*;


import java.nio.ByteBuffer;
import java.nio.CharBuffer;

import java.io.UnsupportedEncodingException;

public class PlateResult
{ 
	
	public PlateResult()
	{
		
	}
	
	 public  byte[] license;  	/**<���ƺ���*///GBK
	 public  byte[] color;      		/**<������ɫ*/
	 public   int nColor;					/**<������ɫ��ţ����������ɫ����LC_X*/
	 public   int nType;					/**<�������ͣ�����������Ͷ���LT_X*/
	    public   int nConfidence;			/**<���ƿ��Ŷ�*/
	    public   int nBright;				/**<��������*/
	    public   int nDirection;				/**<�˶���������˶������� DIRECTION_X*/
	    public   THRECT rcLocation; 		/**<����λ��*/
	    public int nTime;          		/**<ʶ������ʱ��*/
	    public   VZ_TIMEVAL tvPTS;			/**<ʶ��ʱ���*/
	    public    int uBitsTrigType;		/**<ǿ�ƴ������������,��TH_TRIGGER_TYPE_BIT*/
	     public    char nCarBright;	/**<��������*/
	    public    char nCarColor;	/**<������ɫ�����������ɫ����LCOLOUR_X*/
//	    char reserved0[2];			/**<Ϊ�˶���*/
	    public    int uId; 				/**<��¼�ı��*/
	    public    VzBDTime    struBDTime;     /**<�ֽ�ʱ��*/
	   // char reserved[68];			/**<����*/
	     
	    
	    public static PlateResult createFromData(byte [] data)
	    {
//	    	   Charset cs = Charset.forName ("GBK");
//	    	      ByteBuffer bb = ByteBuffer.allocate (data.length);
//	    	      bb.put (data);
//	    	        bb.flip ();
//	    	       CharBuffer cb = cs.decode (bb);
//	    	       
//	    	   char [] tempData =    cb.array();
	    	
	    	PlateResult pr = new PlateResult();
//	    	int offset = 0 ;
//	    	
//	    	try{
//	    		pr.license = new String(data,0,16,"GBk");
//	    	}
//	    	catch (UnsupportedEncodingException e)
//	    	{
//	    		pr.license = "";
//	    	}
//	    	
//	    	offset += 16;
//	    	try{
//	    	pr.color = new String(data,offset,8,"GBk");
//	        }
//    	   catch (UnsupportedEncodingException e)
//    	   {
//    		   pr.license = "";
//    	   }
//    	
//	    	offset += 8;
//	    	
//	    	try
//	    	{
//	    		pr.nColor =Integer.valueOf( new String(data,offset,4));
//	    	}
//	    	catch (NumberFormatException  e)
//	    	{
//	    		pr.nColor = 0;
//	    	}
//	    	offset += 4;
//	    	
//	    	
//	    	try
//	    	{
//	    		pr.nType =Integer.valueOf( new String(data,offset,4));
//	    	}
//	    	catch (NumberFormatException  e)
//	    	{
//	    		pr.nType = 0;
//	    	}
//	    	offset += 4;
//	    	
//	    	try
//	    	{
//	    		pr.nType =Integer.valueOf( new String(data,offset,4));
//	    	}
//	    	catch (NumberFormatException  e)
//	    	{
//	    		pr.nType = 0;
//	    	}
//	    	offset += 4;
//	    	offset = 92;
//	    	 
//	    	pr. struBDTime = pr.new  VzBDTime();
//	     
//	    	pr. struBDTime.bdt_year =  toInt(data,offset,(int)2);
//	 
//	    	offset += 1;
//	    	
	    	 
	    	
	    	return pr;
	    }
	    
	 // ��byte����bRefArrתΪһ������,�ֽ�����ĵ�λ�����͵ĵ��ֽ�λ
	    public static int toInt(byte[] bRefArr,int offset,int length) {
	        int iOutcome = 0;
	        byte bLoop;

	        for (int i = offset; i < length; i++) {
	            bLoop = bRefArr[i];
	            iOutcome += (bLoop & 0xFF) << (8 * (i-offset));
	        }
	        return iOutcome;
	    }
	    
	    //System.arraycopy(src, srcPos, dest, destPos, length)
}


 

 