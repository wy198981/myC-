package com.zbar.lib;

import android.hardware.Camera;
import android.os.Handler;
import android.os.Message;
import android.util.Log;

/**
 * ����: ����(1076559197@qq.com)
 * <p>
 * ʱ��: 2014��5��9�� ����12:21:30
 * <p>
 * �汾: V_1.0.0
 * <p>
 * ����: ����Զ��Խ�?
 */
final class AutoFocusCallback implements Camera.AutoFocusCallback
{

    private static final String TAG = AutoFocusCallback.class.getSimpleName();

    private static final long AUTOFOCUS_INTERVAL_MS = 1500L;

    private Handler autoFocusHandler;
    private int autoFocusMessage;

    void setHandler(Handler autoFocusHandler, int autoFocusMessage)
    {
        this.autoFocusHandler = autoFocusHandler;
        this.autoFocusMessage = autoFocusMessage;
    }

    public void onAutoFocus(boolean success, Camera camera)
    {
        if (autoFocusHandler != null)
        {
            Message message = autoFocusHandler.obtainMessage(autoFocusMessage, success);
            autoFocusHandler.sendMessageDelayed(message, AUTOFOCUS_INTERVAL_MS);
            autoFocusHandler = null;
        }
        else
        {
            Log.d(TAG, "Got auto-focus callback, but no handler for it");
        }
    }

}
