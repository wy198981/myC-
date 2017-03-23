package com.vzvison.monitor.player;

import java.util.Vector;

/**
 * 鏁版嵁锟??
 * 瀹炴椂瑙嗛娴佷腑鍙互鍒嗘垚鑻ュ共涓狿acket锟??
 * @author Administrator
 */
public class Packet {
	private int amount; //鏁版嵁鍖呯殑鏁伴噺
	private Vector<Integer> positionList;	//姣忎釜鏁版嵁鍖呯殑璧峰浣嶇疆鐨勫垪锟??

	public int getAmount() {
		return amount;
	}

	public void setAmount(int amount) {
		this.amount = amount;
	}

	public Vector<Integer> getPositionList() {
		return positionList;
	}

	public void setPositionList(Vector<Integer> positionList) {
		this.positionList = positionList;
	}
}
