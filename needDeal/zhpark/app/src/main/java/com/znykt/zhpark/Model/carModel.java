package com.znykt.zhpark.Model;

import java.util.ArrayList;

/**
 * * @author 作者 E-mail: * @date 创建时间：2016-9-14 上午11:59:33 * @version 1.0 * @parameter
 * * @since * @return
 */

public class carModel {
	public int relustcode;
	public String msg;
	ArrayList<carModelt> carlt;

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

	public ArrayList<carModelt> getcarlt() {
		return carlt;
	}

	public void setcarlt(ArrayList<carModelt> carlt) {
		this.carlt = carlt;
	}

}
