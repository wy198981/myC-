package com.vz;

public class VzBDTime {
	public byte   bdt_sec;    /**<�룬ȡֵ��Χ[0,59]*/
	public byte   bdt_min;    /**<�֣�ȡֵ��Χ[0,59]*/
	public byte   bdt_hour;   /**<ʱ��ȡֵ��Χ[0,23]*/
	public byte   bdt_mday;   /**<һ�����е����ڣ�ȡֵ��Χ[1,31]*/
	public byte   bdt_mon;    /**<�·ݣ�ȡֵ��Χ[1,12]*/
//	byte   res1[3];    /**<Ԥ��*/
	public int   bdt_year;   /**<���*/
//	byte   res2[4];    /**<Ԥ��*/
}
