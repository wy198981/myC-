package com.vz;

 

import android.util.Log;

public class tcpsdk {
	
	private static tcpsdk  m_tcpsdk = null;
	
	private void tcpsdk()
	{
		
	}
	
	public static tcpsdk getInstance()
	{
		if(m_tcpsdk == null)
			m_tcpsdk = new tcpsdk();
		return m_tcpsdk;
	}
	
	/**
	*  @brief ȫ�ֳ�ʼ��
	*  @return 0��ʾ�ɹ���-1��ʾʧ��
	*  @note �����нӿڵ���֮ǰ����
	*  @ingroup group_global
	*/
    public native int   setup();
    /**
    *  @brief ȫ���ͷ�
    *  @note �ڳ������ʱ���ã��ͷ�SDK����Դ
    *  @ingroup group_global
    */
    public native void  cleanup();
     
    /**
    *  @brief ��һ���豸
    *  @param  [IN] ip �豸��IP��ַ
    *  @param  [IN] ipLength �豸��IP��ַ����
    *  @param  [IN] port �豸�Ķ˿ں�
    *  @param  [IN] username �����豸�����û���
    *  @param  [IN] userpassword �����豸��������
    *  @return �����豸�Ĳ������������ʧ��ʱ������0
    *  @ingroup group_device
    */
    public native int   open(byte[] ip,int ipLength,int port,byte[] username,int userLength,byte[] userpassword ,int passwordLenth);
    
    /**
    *  @brief �ر�һ���豸
    *  @param  [IN] handle ��VzLPRTcp_Open������õľ��
    *  @return 0��ʾ�ɹ���-1��ʾʧ��
    *  @ingroup group_device
    */
    public native int   close(int tcphandle);
   public native  int  setIoOutput(int handle, short uChnId, int nOutput);
   public native  int   getIoOutput(int  handle,  int  uChnId , int[] nOutput);
   public native  int  setIoOutputAuto(int handle, short uChnId, int nDuration);
   
   public native  boolean  isConnected(int handle);
   
   public native int setPlateInfoCallBack( int handle,  OnDataReceiver  onDataReceiver ,int bEnableImage);
 //  public native int setPlateInfoCallBack( OnDataReceiver  onDataReceiver );
   
   public native int forceTrigger(int handle);
   
   public native int serialStart(int handle, int nSerialPort);
   public native int  serialSend(int handle, int nSerialPort, byte[] pData, long uSizeData);
   public native int serialStop(int handle);
   public native int  getSnapImageData(int handle, byte[] imgBuffer, int imgBufferMaxLength);
   public native int  getRtspUrl(int handle,  byte[] url, int urlMaxLength);
   public native int playVoice( int handle, byte[] voice, int interval, int volume, int male);
   
   public native int setWlistInfoCallBack(int handle,onWlistReceiver recevier);
   public native int  importWlistVehicle(int handle,WlistVehicle wllist);
   public native int  deleteWlistVehicle(int handle,byte[] plateCode);
   public native int  queryWlistVehicle(int handle,byte[] plateCode);
   
	public interface OnDataReceiver {
		
		void onDataReceive(int handle,PlateResult plateResult,int uNumPlates,int eResultType,
				byte[] pImgFull,int nFullSize, byte[] pImgPlateClip,int nClipSize  );
//		void onDataReceive(int handle,byte[] szPlateData,int plateLength,int plateConfidence,int plateType,byte[] bdTimeData,int timeLength,
//				byte[] pImgFull,int nFullSize, byte[] pImgPlateClip,int nClipSize);
		
		
	}
	
	public interface onWlistReceiver {
		void onWlistReceive(int handle, WlistVehicle wlist  );
	}
    static {
    	try {
    		//System.loadLibrary("log");
    		System.loadLibrary("vztcpsdk_dynamic");
            System.loadLibrary("tcpsdk");
    	}
    	catch(UnsatisfiedLinkError e) {
			// fatal error, we can't load some our libs
			Log.d("tcpsdk", "Couldn't load lib: " + " - " + e.getMessage());
			
		}
    }
}
