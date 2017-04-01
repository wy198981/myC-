package com.vz.monitor.util;

import java.io.ByteArrayOutputStream;
import java.io.InputStream;

public class StreamUtil {
	
	public static final byte[] readStream(InputStream is) {
		if(null == is) {
			return null;
		}
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		byte[] buffer = new byte[1024];
		int length = 0;
		try { 
			while(-1 != (length = is.read(buffer, 0, buffer.length))) {
				baos.write(buffer, 0, length);
			}
		} catch (Exception e) {
			e.printStackTrace();
		} 
		return baos.toByteArray();
	}
	
	/**
	 * �������������ж�ȡָ�����ȵ����ݣ�����������ĳ���С��ָ�����ȣ��򷵻���������ʵ�ʳ���
	 * @param is ������
	 * @param length Ҫ��ȡ�����ݳ���
	 * @return ��ȡ���Ķ���������
	 */
	public static final byte[] readStream(InputStream is, int length) {
		if(null == is || length < 0) {
			return null;
		}
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		byte[] buffer = null;
		int readLen = 0;
		int realLength = length;
		try {
			buffer = new byte[realLength];
			
			while(-1 != (readLen = is.read(buffer, 0, realLength))) {
				baos.write(buffer, 0, readLen);
				realLength -= readLen;
				if(realLength <= 0) {
					break;
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return baos.toByteArray();
	}

}
