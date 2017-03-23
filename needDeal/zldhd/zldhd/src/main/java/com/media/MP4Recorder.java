package com.media;

public class MP4Recorder {
	public static final int TYPE_VIDEO = 0;
	public static final int TYPE_AUDIO = 1;
	
	/**
	 * 寮?鍚綍鍒跺櫒
	 * @param strFileName 褰曞埗淇濆瓨鐨勬枃浠跺悕
	 * @return 寮?鍚粨鏋?,鎴愬姛锛歵rue锛涘け璐ワ細false
	 */
	public native boolean startRecorder(String strFileName);
	/**
	 * 鍋滄褰曞埗鍣?
	 */
	public native void stopRecorder();
	/**
	 * 娣诲姞闊宠棰戞暟鎹?
	 * @param data 闊宠棰戞暟鎹?
	 * @param length 鏁版嵁闀垮害
	 * @param type 鏁版嵁绫诲瀷锛歍YPE_VIDEO锛氳棰戯紱TYPE_AUDIO锛氶煶棰?
	 * @return
	 */
	public native boolean addSample(byte[] data,int length,int type);
	
	static {
		System.loadLibrary("MP4Recorder");
	}
}
