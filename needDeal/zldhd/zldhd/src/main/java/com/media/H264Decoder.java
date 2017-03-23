package com.media;

public class H264Decoder {
	
	 private static H264Decoder uniqueInstance = null;
	 
	    private H264Decoder() {
	       // Exists only to defeat instantiation.
	    }
	 
	    public static H264Decoder getInstance() {
	       if (uniqueInstance == null) {
	           uniqueInstance = new H264Decoder();
	           uniqueInstance.init();
	       }
	       return uniqueInstance;
	    }
	
	/**
	 *
	 * @return
	 */
	public native int init();
	
	
	public native int add(int decodeType);
	
	/**
	 *
	 * @param src
	 * @param length
	 * @param dst
	 * @param wah
	 * @return
	 */
	public native synchronized  int decode(int handle,byte[] src, int length, byte[] dst,int[] wah);
	
	/**
	 *
	 */
	public native void release(int handle);
	
	static {
		System.loadLibrary("H264Decoder");
	}
}
