package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.Display;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.myUserControlLibrary.niceSpinner.NiceSpinner;

/**
 * Created by Administrator on 2017-04-07.
 */
public class ParkingChannelSelectActivity extends AppCompatActivity implements View.OnClickListener
{
    private NiceSpinner niceSpinnerChannel;
    private Button btnChannelSelectOk;
    private Button btnChannelSelectCancel;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.parking_channelselect);
        initView();

        Window window = getWindow();
        WindowManager m = getWindowManager();
        Display d = m.getDefaultDisplay(); // 获取屏幕宽、高用
        WindowManager.LayoutParams p = window.getAttributes(); // 获取对话框当前的参数值
        p.height = (int) (d.getHeight() * 1 / 3); // 改变的是dialog框在屏幕中的位置而不是大小
        p.width = (int) (d.getWidth() * 1 / 3); // 宽度设置为屏幕的0.65
        window.setAttributes(p);

//        window.setBackgroundDrawableResource(R.drawable.parkdowncard_background);
        setTitle(getResources().getString(R.string.channelSelectTitle));
    }


    private void initView()
    {
        niceSpinnerChannel = (NiceSpinner) findViewById(R.id.niceSpinnerChannel);
        btnChannelSelectOk = (Button) findViewById(R.id.btnChannelSelectOk);
        btnChannelSelectCancel = (Button) findViewById(R.id.btnChannelSelectCancel);

        btnChannelSelectOk.setOnClickListener(this);
        btnChannelSelectCancel.setOnClickListener(this);
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnChannelSelectOk:

                break;
            case R.id.btnChannelSelectCancel:

                break;
        }
    }
}
