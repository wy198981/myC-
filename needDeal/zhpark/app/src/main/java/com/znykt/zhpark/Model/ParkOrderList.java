package com.znykt.zhpark.Model;

import java.util.ArrayList;

/**
 * * @author 作者 E-mail: * @date 创建时间：2016-9-10 上午9:57:07 * @version 1.0 * @parameter
 * * @since * @return
 */


public class ParkOrderList {
	public int ParkOrder_ID;
	public String ParkOrder_OrderNo;
	public String ParkOrder_CarType_Name_head;
	public int PayOrder_Status;//订单状态
	public String Parking_Name;
	public String Parking_TimeCount;
	public String ParkOrder_EnterTime;
	public String ParkOrder_CarNo;
	public int ParkOrder_Lock;
	public String PayOrder_Money;//应缴金额
	public String ParkOrder_DiscountMoney;//优惠金额
	public String ParkOrder_TotalMoney; //总金额（接口获取到的金额）= 应缴金额 + 优惠金额
	public String ParkOrder_EnterImgPath;
	public String ParkOrder_EnterGateName;

	public int getParkOrder_ID() {
		return ParkOrder_ID;
	}

	public void setParkOrder_ID(int parkOrder_ID) {
		ParkOrder_ID = parkOrder_ID;
	}

	public String getParkOrder_OrderNo() {
		return ParkOrder_OrderNo;
	}

	public void setParkOrder_OrderNo(String parkOrder_OrderNo) {
		ParkOrder_OrderNo = parkOrder_OrderNo;
	}

	public String getParkOrder_CarType_Name_head() {
		return ParkOrder_CarType_Name_head;
	}

	public void setParkOrder_CarType_Name_head(
			String parkOrder_CarType_Name_head) {
		ParkOrder_CarType_Name_head = parkOrder_CarType_Name_head;
	}

	public int getPayOrder_Status() {
		return PayOrder_Status;
	}

	public void setPayOrder_Status(int payOrder_Status) {
		PayOrder_Status = payOrder_Status;
	}

	public String getParking_Name() {
		return Parking_Name;
	}

	public void setParking_Name(String parking_Name) {
		Parking_Name = parking_Name;
	}

	public String getParking_TimeCount() {
		return Parking_TimeCount;
	}

	public void setParking_TimeCount(String parking_TimeCount) {
		Parking_TimeCount = parking_TimeCount;
	}

	public String getParkOrder_EnterTime() {
		return ParkOrder_EnterTime;
	}

	public void setParkOrder_EnterTime(String parkOrder_EnterTime) {
		ParkOrder_EnterTime = parkOrder_EnterTime;
	}

	public String getParkOrder_CarNo() {
		return ParkOrder_CarNo;
	}

	public void setParkOrder_CarNo(String parkOrder_CarNo) {
		ParkOrder_CarNo = parkOrder_CarNo;
	}

	public int getParkOrder_Lock() {
		return ParkOrder_Lock;
	}

	public void setParkOrder_Lock(int parkOrder_Lock) {
		ParkOrder_Lock = parkOrder_Lock;
	}

	public String getPayOrder_Money() {
		return PayOrder_Money;
	}

	public void setPayOrder_Money(String payOrder_Money) {
		PayOrder_Money = payOrder_Money;
	}

	public String getParkOrder_DiscountMoney() {
		return ParkOrder_DiscountMoney;
	}

	public void setParkOrder_DiscountMoney(String parkOrder_DiscountMoney) {
		ParkOrder_DiscountMoney = parkOrder_DiscountMoney;
	}
	
	public String getParkOrder_TotalMoney() {
		return ParkOrder_TotalMoney;
	}

	public void setParkOrder_TotalMoney(String parkOrder_TotalMoney) {
		ParkOrder_TotalMoney = parkOrder_TotalMoney;
	}

	public String getParkOrder_EnterImgPath() {
		return ParkOrder_EnterImgPath;
	}

	public void setParkOrder_EnterImgPath(String parkOrder_EnterImgPath) {
		ParkOrder_EnterImgPath = parkOrder_EnterImgPath;
	}

	public String getParkOrder_EnterGateName() {
		return ParkOrder_EnterGateName;
	}

	public void setParkOrder_EnterGateName(String parkOrder_EnterGateName) {
		ParkOrder_EnterGateName = parkOrder_EnterGateName;
	}


}
