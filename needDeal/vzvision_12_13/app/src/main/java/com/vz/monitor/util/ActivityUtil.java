package com.vz.monitor.util;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;

public class ActivityUtil {
	
	/**
	 * 各个Activity之间的无参数调转
	 * 
	 * @param activity
	 * @param clazz
	 */
	public static final void invoke(Activity activity, Class<?> clazz) {
		Intent intent = new Intent(activity, clazz);
		activity.startActivity(intent);
	}

	/**
	 * 各个Activity之间的携带参数调转
	 * 
	 * @param activity
	 * @param clazz
	 * @param bundle 所携带的参数
	 */
	public static final void invokeWithArgs(Activity activity, Class<?> clazz, Bundle bundle) {
		Intent intent = new Intent(activity, clazz);
		intent.putExtra("bundle", bundle);
		activity.startActivity(intent);
	}

}
