package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.app.Activity;
import android.app.Dialog;
import android.text.TextUtils;
import android.view.Display;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.myUserControlLibrary.niceSpinner.NiceSpinner;
import com.example.administrator.myparkingos.util.T;

import java.util.List;

/**
 * Created by Administrator on 2017-04-07.
 */
public class ParkingChannelSelectView implements View.OnClickListener // 入场通道的选择
{
    private final Dialog dialog;
    private Activity mActivity;
    private NiceSpinner niceSpinnerChannel;
    private Button btnChannelSelectOk;
    private Button btnChannelSelectCancel;

    public ParkingChannelSelectView(Activity activity, int type) // 0, 表示入口; 1,表示出口
    {
        this.mActivity = activity;

        dialog = new Dialog(activity); // @android:style/Theme.Dialog
        dialog.setContentView(R.layout.parking_channelselect);
        dialog.setCanceledOnTouchOutside(true);

        Window window = dialog.getWindow();
        WindowManager m = activity.getWindowManager();
        Display d = m.getDefaultDisplay(); // 获取屏幕宽、高用
        WindowManager.LayoutParams p = window.getAttributes(); // 获取对话框当前的参数值
        p.height = (int) (d.getHeight() * 1 / 3); // 改变的是dialog框在屏幕中的位置而不是大小
        p.width = (int) (d.getWidth() * 1 / 3); // 宽度设置为屏幕的0.65
        window.setAttributes(p);

        initView();
//        dialog.getWindow().setBackgroundDrawableResource(R.drawable.parkdowncard_background);
        dialog.setTitle(activity.getResources().getString(R.string.tempPlate_title));

    }

    private void initView()
    {
        niceSpinnerChannel = (NiceSpinner) dialog.findViewById(R.id.niceSpinnerChannel);
        btnChannelSelectOk = (Button) dialog.findViewById(R.id.btnChannelSelectOk);
        btnChannelSelectCancel = (Button) dialog.findViewById(R.id.btnChannelSelectCancel);

        btnChannelSelectOk.setOnClickListener(this);
        btnChannelSelectCancel.setOnClickListener(this);
    }

    public void setSpinnerData(List<String> inOutName)
    {
        niceSpinnerChannel.refreshData(inOutName, 0);
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnChannelSelectOk:
                String currentText = niceSpinnerChannel.getCurrentText().trim();
                if (TextUtils.isEmpty(currentText))
                {
                    T.showShort(mActivity, "选择的通道为空");
                    return;
                }
                onSelectInOutName(currentText);
                dismiss();
                break;
            case R.id.btnChannelSelectCancel:
                dismiss();
                break;
        }
    }

    public void onSelectInOutName(final String currentText)
    {

    }

    public void show()
    {
        if (dialog != null)
        {
            prepareLoadData();// 显示之前加载数据
            dialog.show();
        }
    }

    public void prepareLoadData()
    {

    }

    public void dismiss()
    {
        if (dialog != null && dialog.isShowing())
        {
            clearDataInView();
            dialog.dismiss();
        }
    }

    public void clearDataInView()
    {

    }
}
