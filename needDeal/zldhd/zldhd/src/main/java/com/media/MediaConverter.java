package com.media;

public class MediaConverter {
	/**
	 * 鎶奩UV420SP杞寲鎴怰GB565
	 * @param yuv YUV420SP濯掍綋鏁版嵁
	 * @param rgb RGB565濯掍綋鏁版嵁
	 * @param width 杞寲鍚庣殑RGB565鏁版嵁鐨勫搴?
	 * @param height 杞寲鍚庣殑RGB565鏁版嵁鐨勫搴?
	 * @return 杞寲缁撴灉锛?1锛氭垚鍔燂紱0锛氬け璐?
	 */
	public native int YUV420SP2RGB565(byte[] yuv, byte[] rgb, int width, int height);
	
	
	static {
		System.loadLibrary("MediaConverter");
	}
}
