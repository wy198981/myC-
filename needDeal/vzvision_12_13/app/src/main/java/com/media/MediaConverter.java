package com.media;

public class MediaConverter {
	/**
	 * ��YUV420SPת����RGB565
	 * @param yuv YUV420SPý������
	 * @param rgb RGB565ý������
	 * @param width ת�����RGB565���ݵĿ��
	 * @param height ת�����RGB565���ݵĿ��
	 * @return ת�������1���ɹ���0��ʧ��
	 */
	public native int YUV420SP2RGB565(byte[] yuv, byte[] rgb, int width, int height);
	
	
	static {
		System.loadLibrary("MediaConverter");
	}
}
