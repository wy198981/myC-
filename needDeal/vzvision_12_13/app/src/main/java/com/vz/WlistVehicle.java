package com.vz;

public class WlistVehicle {
	public  long	uVehicleID = 0;										/**<���������ݿ��ID*/
	public    byte[]		strPlateID ;			/**<�����ַ���*/
	public    long	uCustomerID= 0;									/**<�ͻ������ݿ��ID����VZ_LPR_WLIST_CUSTOMER::uCustomerID��Ӧ*/
	public    long	bEnable= 0;										/**<�ü�¼��Ч���*/
	public    long	bEnableTMEnable= 0;								/**<�Ƿ�����Чʱ��*/
	public    long	bEnableTMOverdule= 0;								/**<�Ƿ�������ʱ��*/
	public   VzDateTime		struTMEnable;									/**<�ü�¼��Чʱ��*/
	public VzDateTime		struTMOverdule;									/**<�ü�¼����ʱ��*/
	public long	bUsingTimeSeg;									/**<�Ƿ�ʹ������ʱ���*/
	    // VZ_TM_PERIOD struTimeSeg;								/**<����ʱ�����Ϣ*/
	public long	   bAlarm= 0;						 	/**<�Ƿ񴥷���������������¼��*/
	public int			iColor= 0;											/**<������ɫ*/
	public int			iPlateType= 0;										/**<��������*/
	public byte[]		strCode;			/**<��������*/
	public byte[]		strComment;	/**<��������*/
	
	public WlistVehicle()
	{
		struTMEnable = new VzDateTime();
		
		struTMOverdule = new VzDateTime();
	}
}
