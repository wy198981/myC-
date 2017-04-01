package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.RadioButton;
import android.widget.TextView;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.myUserControlLibrary.niceSpinner.NiceSpinner;

/**
 * Created by Administrator on 2017-02-16.
 * 【在线监控】 -->>【无牌车出场】
 */
public class ParkingOutNOPlateNoActivity extends AppCompatActivity implements View.OnClickListener
{
    private Button btnNoPlateOutSearch;
    private Button btnNoPlateOutCalc;
    private Button btnNoPlateOutOpen;
    private Button btnNoPlateOutFreeOpen;
    private Button btnNoPlateOutPrint;
    private Button btnNoPlateOutCancel;
    private RadioButton rgWeixin;
    private RadioButton rgZhifubao;
    private ListView lvNoPlate;
    private TextView tvMoney;
    private ImageView ivInPicture;
    private TextView tvInPicHint;
    private ImageView ivOutPicture;
    private TextView tvOutPicHint;
    private TextView tvTimeStart;
    private ImageButton bt_selectCarlendarStart;
    private TextView tvTimeEnd;
    private ImageButton bt_selectCarlendarEnd;
    private NiceSpinner nSpCarColor;
    private NiceSpinner nSpCarType;
    private EditText etInputTempCar;
    private NiceSpinner nSpCarBrand;
    private NiceSpinner nSpDiscountSpace;
    private NiceSpinner nSpOutChannelName;
    private NiceSpinner nSpFreeReason;
    private TextView tvNeedMoney;

    //

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.parkingout_noplate);
        initView();
    }

    private void initView()
    {
        btnNoPlateOutSearch = (Button) findViewById(R.id.btnNoPlateOutSearch);
        btnNoPlateOutCalc = (Button) findViewById(R.id.btnNoPlateOutCalc);
        btnNoPlateOutOpen = (Button) findViewById(R.id.btnNoPlateOutOpen);
        btnNoPlateOutFreeOpen = (Button) findViewById(R.id.btnNoPlateOutFreeOpen);
        btnNoPlateOutPrint = (Button) findViewById(R.id.btnNoPlateOutPrint);
        btnNoPlateOutCancel = (Button) findViewById(R.id.btnNoPlateOutCancel);
        rgWeixin = (RadioButton) findViewById(R.id.rgWeixin);
        rgZhifubao = (RadioButton) findViewById(R.id.rgZhifubao);
        lvNoPlate = (ListView) findViewById(R.id.lvNoPlate);
        tvMoney = (TextView) findViewById(R.id.tvMoney);
        ivInPicture = (ImageView) findViewById(R.id.ivInPicture);
        tvInPicHint = (TextView) findViewById(R.id.tvInPicHint);
        ivOutPicture = (ImageView) findViewById(R.id.ivOutPicture);
        tvOutPicHint = (TextView) findViewById(R.id.tvOutPicHint);

        btnNoPlateOutSearch.setOnClickListener(this);
        btnNoPlateOutCalc.setOnClickListener(this);
        btnNoPlateOutOpen.setOnClickListener(this);
        btnNoPlateOutFreeOpen.setOnClickListener(this);
        btnNoPlateOutPrint.setOnClickListener(this);
        btnNoPlateOutCancel.setOnClickListener(this);

        tvTimeStart = (TextView) findViewById(R.id.tvTimeStart);
        tvTimeStart.setOnClickListener(this);
        bt_selectCarlendarStart = (ImageButton) findViewById(R.id.bt_selectCarlendarStart);
        bt_selectCarlendarStart.setOnClickListener(this);
        tvTimeEnd = (TextView) findViewById(R.id.tvTimeEnd);
        tvTimeEnd.setOnClickListener(this);
        bt_selectCarlendarEnd = (ImageButton) findViewById(R.id.bt_selectCarlendarEnd);
        bt_selectCarlendarEnd.setOnClickListener(this);
        nSpCarColor = (NiceSpinner) findViewById(R.id.nSpCarColor);
        nSpCarColor.setOnClickListener(this);
        nSpCarType = (NiceSpinner) findViewById(R.id.nSpCarType);
        nSpCarType.setOnClickListener(this);
        etInputTempCar = (EditText) findViewById(R.id.etInputTempCar);
        etInputTempCar.setOnClickListener(this);
        nSpCarBrand = (NiceSpinner) findViewById(R.id.nSpCarBrand);
        nSpCarBrand.setOnClickListener(this);
        nSpDiscountSpace = (NiceSpinner) findViewById(R.id.nSpDiscountSpace);
        nSpDiscountSpace.setOnClickListener(this);
        nSpOutChannelName = (NiceSpinner) findViewById(R.id.nSpOutChannelName);
        nSpOutChannelName.setOnClickListener(this);
        nSpFreeReason = (NiceSpinner) findViewById(R.id.nSpFreeReason);
        nSpFreeReason.setOnClickListener(this);
        tvNeedMoney = (TextView) findViewById(R.id.tvNeedMoney);
        tvNeedMoney.setOnClickListener(this);
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnNoPlateOutSearch:

                break;
            case R.id.btnNoPlateOutCalc:

                break;
            case R.id.btnNoPlateOutOpen:

                break;
            case R.id.btnNoPlateOutFreeOpen:

                break;
            case R.id.btnNoPlateOutPrint:

                break;
            case R.id.btnNoPlateOutCancel:

                break;
            case R.id.bt_selectCarlendarStart:
                break;
            case R.id.bt_selectCarlendarEnd:
                break;
        }
    }


}
