package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.Display;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.Spinner;

import com.example.administrator.mydistributedparkingos.R;

import java.util.ArrayList;

/**
 * Created by Administrator on 2017-02-16.
 * 【临时车入场确认开闸，弹出界面，提前在车场设置中设置开闸方式】 CPH (che pai hao缩写)
 */
public class ParkingTempCPHActivity extends AppCompatActivity implements View.OnClickListener
{
    private RadioButton rbSelect;
    private Spinner spinnerProvince;
    private EditText etCarNo;
    private Spinner spinnerRoadName;
    private Button btnOk;
    private Button btnCancel;
    private ArrayAdapter provinceAdapter;
    private ArrayList<String> roadList;
    private ArrayAdapter roadAdapter;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.parkingin_tempcph);
        initView();

        Window window = getWindow();
        WindowManager m = getWindowManager();
        Display d = m.getDefaultDisplay(); // 获取屏幕宽、高用
        WindowManager.LayoutParams p = window.getAttributes(); // 获取对话框当前的参数值
        p.height = (int) (d.getHeight() * 1 / 3); // 改变的是dialog框在屏幕中的位置而不是大小
        p.width = (int) (d.getWidth() * 1 / 3); // 宽度设置为屏幕的0.65
        window.setAttributes(p);

//        window.setBackgroundDrawableResource(R.drawable.parkdowncard_background);
        setTitle(getResources().getString(R.string.tempPlate_title));

    }

    private void initView()
    {
        rbSelect = (RadioButton) findViewById(R.id.rbSelect);
        spinnerProvince = (Spinner) findViewById(R.id.spinnerProvince);
        etCarNo = (EditText) findViewById(R.id.etCarNo);
        spinnerRoadName = (Spinner) findViewById(R.id.spinnerRoadName);
        btnOk = (Button) findViewById(R.id.btnOk);
        btnCancel = (Button) findViewById(R.id.btnCancel);

        btnOk.setOnClickListener(this);
        btnCancel.setOnClickListener(this);


        rbSelect.setChecked(true);
        etCarNo.setText("A23455");
        provinceAdapter = new ArrayAdapter(getApplicationContext(), R.layout.blacklist_spinner_province
                , getResources().getStringArray(R.array.provinceArray));
        spinnerProvince.setAdapter(provinceAdapter);

        roadList = new ArrayList<>();
        roadAdapter = new ArrayAdapter(getApplicationContext(), R.layout.blacklist_spinner_province
                , roadList);
        roadList.add("入口车道1");
        roadList.add("入口车道2");
        roadList.add("入口车道3");

        spinnerRoadName.setAdapter(roadAdapter);

    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnOk:

                break;
            case R.id.btnCancel:

                break;
        }
    }

}
