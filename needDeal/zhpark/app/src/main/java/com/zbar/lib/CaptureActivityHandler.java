package com.zbar.lib;

import com.znykt.zhpark.R;

import android.os.Handler;
import android.os.Message;


/**
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷(1076559197@qq.com)
 * <p>
 * 时锟斤拷: 2014锟斤拷5锟斤拷9锟斤拷 锟斤拷锟斤拷12:23:32
 * <p>
 * 锟芥本: V_1.0.0
 * <p>
 * 锟斤拷锟斤拷: 扫锟斤拷锟斤拷息转锟斤拷
 */
public final class CaptureActivityHandler extends Handler
{

    DecodeThread decodeThread = null;
    CaptureActivity activity = null;
    private State state;

    private enum State
    {
        PREVIEW, SUCCESS, DONE
    }

    public CaptureActivityHandler(CaptureActivity activity)
    {
        this.activity = activity;
        decodeThread = new DecodeThread(activity);
        decodeThread.start();
        state = State.SUCCESS;
        CameraManager.get().startPreview();
        restartPreviewAndDecode(); // 开启预览视频流
    }

    @Override
    public void handleMessage(Message message)
    {

        switch (message.what)
        {
            case R.id.auto_focus:
                if (state == State.PREVIEW)
                {
                    CameraManager.get().requestAutoFocus(this, R.id.auto_focus);
                }
                break;
            case R.id.restart_preview:
                restartPreviewAndDecode();
                break;
            case R.id.decode_succeeded:
                state = State.SUCCESS;
                activity.handleDecode((String) message.obj);
                break;

            case R.id.decode_failed:
                state = State.PREVIEW;
                CameraManager.get().requestPreviewFrame(decodeThread.getHandler(),
                        R.id.decode);
                break;
        }

    }

    public void quitSynchronously()
    {
        state = State.DONE;
        CameraManager.get().stopPreview();
        removeMessages(R.id.decode_succeeded);
        removeMessages(R.id.decode_failed);
        removeMessages(R.id.decode);
        removeMessages(R.id.auto_focus);
    }

    private void restartPreviewAndDecode()
    {
        if (state == State.SUCCESS)
        {
            state = State.PREVIEW;
            CameraManager.get().requestPreviewFrame(decodeThread.getHandler(),
                    R.id.decode); // 即获取到 camera 数据后，引发线程的回调;
            CameraManager.get().requestAutoFocus(this, R.id.auto_focus);
        }
    }

}
