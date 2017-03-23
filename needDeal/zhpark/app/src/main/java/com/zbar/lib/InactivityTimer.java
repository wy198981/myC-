package com.zbar.lib;

import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.ThreadFactory;
import java.util.concurrent.TimeUnit;

import android.app.Activity;

/**
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷(1076559197@qq.com)
 * 
 * 时锟斤拷: 2014锟斤拷5锟斤拷9锟斤拷 锟斤拷锟斤拷12:25:12
 *
 * 锟芥本: V_1.0.0
 * 
 */
public final class InactivityTimer {

	private static final int INACTIVITY_DELAY_SECONDS = 5 * 60;

	private final ScheduledExecutorService inactivityTimer = Executors.newSingleThreadScheduledExecutor(new DaemonThreadFactory());
	private final Activity activity;
	private ScheduledFuture<?> inactivityFuture = null;

	public InactivityTimer(Activity activity) {
		this.activity = activity;
		onActivity();
	}

	public void onActivity() {
		cancel();
		inactivityFuture = inactivityTimer.schedule(new FinishListener(activity), INACTIVITY_DELAY_SECONDS, TimeUnit.SECONDS);
	}

	private void cancel() {
		if (inactivityFuture != null) {
			inactivityFuture.cancel(true);
			inactivityFuture = null;
		}
	}

	public void shutdown() {
		cancel();
		inactivityTimer.shutdown();
	}

	private static final class DaemonThreadFactory implements ThreadFactory {
		public Thread newThread(Runnable runnable) {
			Thread thread = new Thread(runnable);
			thread.setDaemon(true);
			return thread;
		}
	}

}
