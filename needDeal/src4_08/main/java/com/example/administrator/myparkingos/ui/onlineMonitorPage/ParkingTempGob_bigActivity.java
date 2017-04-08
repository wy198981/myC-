package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.Display;
import android.view.Window;
import android.view.WindowManager;

import com.example.administrator.mydistributedparkingos.R;

/**
 * Created by Administrator on 2017-04-07.
 */
public class ParkingTempGob_bigActivity extends AppCompatActivity
{
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_parkingtempgob_big);

        Window window = getWindow();
        WindowManager m = getWindowManager();
        Display d = m.getDefaultDisplay(); // 获取屏幕宽、高用
        WindowManager.LayoutParams p = window.getAttributes(); // 获取对话框当前的参数值
        p.height = (int) (d.getHeight() * 1 / 1.5); // 改变的是dialog框在屏幕中的位置而不是大小
        p.width = (int) (d.getWidth() * 1 / 2); // 宽度设置为屏幕的0.65
        window.setAttributes(p);

//        window.setBackgroundDrawableResource(R.drawable.parkdowncard_background);
        setTitle(getResources().getString(R.string.charge_title));
    }
}
