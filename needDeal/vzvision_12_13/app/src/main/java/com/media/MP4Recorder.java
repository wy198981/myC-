package com.media;

public class MP4Recorder {
	public static final int TYPE_VIDEO = 0;
	public static final int TYPE_AUDIO = 1;
	
	/**
	 * ����¼����
	 * @param strFileName ¼�Ʊ�����ļ���
	 * @return �������,�ɹ���true��ʧ�ܣ�false
	 */
	public native boolean startRecorder(String strFileName);
	/**
	 * ֹͣ¼����
	 */
	public native void stopRecorder();
	/**
	 * �������Ƶ����
	 * @param data ����Ƶ����
	 * @param length ���ݳ���
	 * @param type �������ͣ�TYPE_VIDEO����Ƶ��TYPE_AUDIO����Ƶ
	 * @return
	 */
	public native boolean addSample(byte[] data,int length,int type);
	
	static {
		System.loadLibrary("MP4Recorder");
	}
}
