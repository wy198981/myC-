package com.vz.monitor.util;

public class NumberUtil {
	/**
	 * 4�ֽ�����ת������
	 *
	 * @param b ת�����ֽ�����
	 * @return ����
	 */
	public static final int byte2Int(byte b[]) {
		if(null == b || b.length < 4) {
			return 0;
		}
		return b[3] & 0xff | (b[2] & 0xff) << 8 | (b[1] & 0xff) << 16
				| (b[0] & 0xff) << 24;
	}

	/**
	 * С���ֽ����4�ֽ�����ת������ֵ
	 * @param b С���ֽ����4�ֽ�����
	 * @return ����ֵ
	 */
	public static final int leByte2Int(byte[] b) {
		if(null == b || b.length < 4) {
			return 0;
		}
		return b[0] & 0xff | (b[1] & 0xff) << 8 | (b[2] & 0xff) << 16
				| (b[3] & 0xff) << 24;
	}

	/**
	 * ������ת��4�ֽ�����
	 *
	 * @param intValue ת����������
	 * @return 4�ֽ�����
	 */
	public static final byte[] int2Byte(int intValue) {
		byte[] b = new byte[4];
		for (int i = 0; i < 4; i++) {
			b[i] = (byte) (intValue >> 8 * (3 - i) & 0xFF);
		}
		return b;
	}

	/**
	 * ������ֵת��С���ֽ���(little-endian)��4�ֽ�����
	 * @param value ����ֵ
	 * @return ת�����4�ֽ�����
	 */
	public static final byte[] int2LeByte(int value) {
		byte[] b = new byte[4];
		for(int i = 0; i < 4; i++) {
			b[i] = (byte)(value >> 8 * i & 0xFF);
		}
		return b;
	}

	/**
	 * ����һλС��
	 * @param num
	 * @return ת������ַ�����ʽ
	 */
	public static final String limitOneDecimal(float num) {
		java.text.DecimalFormat df = new java.text.DecimalFormat("#.0");
		return df.format(num);
	}
}
