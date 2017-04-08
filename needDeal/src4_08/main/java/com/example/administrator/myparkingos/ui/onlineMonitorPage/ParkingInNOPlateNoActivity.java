package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.text.TextUtils;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.Toast;

import com.example.administrator.mydistributedparkingos.R;

/**
 * Created by Administrator on 2017-02-16.
 * 【在线监控】 -->> 【无牌车入场】
 */
public class ParkingInNOPlateNoActivity extends AppCompatActivity implements View.OnClickListener
{
    private Spinner spinnerColor;
    private Spinner spinnerCarBrand;
    private Button btnAdd;
    private Spinner spinnerProvince;
    private EditText etInputCarNo;
    private Spinner spinnerRoadName;
    private Button btnCancel;
    private ImageView imagePicture;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.parkingin_noplate);
        initView();

    }

    private void initView()
    {
        spinnerColor = (Spinner) findViewById(R.id.spinnerColor);
        spinnerCarBrand = (Spinner) findViewById(R.id.spinnerCarBrand);
        btnAdd = (Button) findViewById(R.id.btnAdd);
        spinnerProvince = (Spinner) findViewById(R.id.spinnerProvince);
        etInputCarNo = (EditText) findViewById(R.id.etInputCarNo);
        spinnerRoadName = (Spinner) findViewById(R.id.spinnerRoadName);
        btnCancel = (Button) findViewById(R.id.btnCancel);
        imagePicture = (ImageView) findViewById(R.id.imagePicture);

        btnAdd.setOnClickListener(this);
        btnCancel.setOnClickListener(this);
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnAdd:

                break;
            case R.id.btnCancel:

                break;
        }
    }

    private void submit()
    {
        // validate
        String etInputCarNoString = etInputCarNo.getText().toString().trim();
        if (TextUtils.isEmpty(etInputCarNoString))
        {
            Toast.makeText(this, "etInputCarNoString不能为空", Toast.LENGTH_SHORT).show();
            return;
        }

        // TODO validate success, do something
    }
}
