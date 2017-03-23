package com.znykt.zhpark.Model;

import java.util.ArrayList;

/**
 * * @author 获取订单详情支付
 * * @date 创建时间：2016-9-14 上午11:59:33
 *  * @version 1.0 * @parameter
 * * @since * @return
 */

public class ParkOrderPay {
	public int relustcode;
	public String msg;
	 ParkOrderList  order;

	public int getRelustcode() {
		return relustcode;
	}

	public void setRelustcode(int relustcode) {
		this.relustcode = relustcode;
	}

	public String getMsg() {
		return msg;
	}

	public void setMsg(String msg) {
		this.msg = msg;
	}

	public  ParkOrderList getOrder() {
		return order;
	}

	public void setOrder(ParkOrderList order) {
		this.order = order;
	}

}
