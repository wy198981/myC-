package com.zbar.lib;

import android.graphics.Point;
import android.hardware.Camera;
import android.os.Handler;
import android.os.Message;
import android.util.Log;

/**
 * 锟斤拷锟斤拷: 锟斤拷锟斤拷(1076559197@qq.com)
 * <p>
 * 时锟斤拷: 2014锟斤拷5锟斤拷9锟斤拷 锟斤拷锟斤拷12:23:14
 * <p>
 * 锟芥本: V_1.0.0
 * <p>
 * 锟斤拷锟斤拷: 锟斤拷锟皆わ拷锟斤拷氐锟�
 */
final class PreviewCallback implements Camera.PreviewCallback
{

    private static final String TAG = PreviewCallback.class.getSimpleName();

    private final CameraConfigurationManager configManager;
    private final boolean useOneShotPreviewCallback;
    private Handler previewHandler;
    private int previewMessage;

    PreviewCallback(CameraConfigurationManager configManager, boolean useOneShotPreviewCallback)
    {
        this.configManager = configManager;
        this.useOneShotPreviewCallback = useOneShotPreviewCallback;
    }

    void setHandler(Handler previewHandler, int previewMessage)
    {
        this.previewHandler = previewHandler;
        this.previewMessage = previewMessage;
    }

    public void onPreviewFrame(byte[] data, Camera camera) // 获取到预览数据的回调
    {
        Point cameraResolution = configManager.getCameraResolution();
        if (!useOneShotPreviewCallback)
        {
            camera.setPreviewCallback(null); // 一次使用
        }
        if (previewHandler != null)
        {
            Message message = previewHandler.obtainMessage(previewMessage, cameraResolution.x,
                    cameraResolution.y, data);
            message.sendToTarget();
            previewHandler = null;
        }
        else
        {
            Log.d(TAG, "Got preview callback, but no handler for it");
        }
    }

}
