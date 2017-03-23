package com.zbar.lib;

import android.app.Activity;
import android.content.DialogInterface;

/**
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷(1076559197@qq.com)
 * 
 * 时锟斤拷: 2014锟斤拷5锟斤拷9锟斤拷 锟斤拷锟斤拷12:24:51
 *
 * 锟芥本: V_1.0.0
 *
 */
public final class FinishListener
    implements DialogInterface.OnClickListener, DialogInterface.OnCancelListener, Runnable {

  private final Activity activityToFinish;

  public FinishListener(Activity activityToFinish) {
    this.activityToFinish = activityToFinish;
  }

  public void onCancel(DialogInterface dialogInterface) {
    run();
  }

  public void onClick(DialogInterface dialogInterface, int i) {
    run();
  }

  public void run() {
    activityToFinish.finish();
  }

}
