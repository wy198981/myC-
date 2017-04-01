package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.app.Dialog;
import android.view.Display;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.RadioButton;
import android.widget.TextView;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.myUserControlLibrary.niceSpinner.NiceSpinner;
import com.example.administrator.myparkingos.util.TimeConvertUtils;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.LinkedList;
import java.util.List;

/**
 * Created by Administrator on 2017-03-10.
 */
public class ParkingOutNOPlateNoView implements View.OnClickListener
{
    private final Activity mActivity;
    private Dialog dialog;
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

    private NoPlateOutListAdapter noPlateAdapter;
    private ArrayList<Object> outList;
    private LinkedList<Object> colorLinkedList;
    private LinkedList<Object> carTypelinkedList;
    private LinkedList<Object> brandLinkedList;
    private LinkedList<Object> discountSpaceLinkedList;
    private LinkedList<Object> channelNameLinkedList;
    private LinkedList<Object> reasonLinkedList;
    private Calendar calendar;
    private int mYear;
    private int mMonth;
    private int mDay;

    public ParkingOutNOPlateNoView(Activity activity)
    {
        mActivity = activity;
        dialog = new Dialog(activity); // @android:style/Theme.Dialog
        dialog.setContentView(R.layout.parkingout_noplate);
        dialog.setCanceledOnTouchOutside(true);

        Window window = dialog.getWindow();
        WindowManager m = activity.getWindowManager();
        Display d = m.getDefaultDisplay(); // 获取屏幕宽、高用
        WindowManager.LayoutParams p = window.getAttributes(); // 获取对话框当前的参数值
        p.height = (int) (d.getHeight() * 1 / 1.5); // 改变的是dialog框在屏幕中的位置而不是大小
        p.width = (int) (d.getWidth() * 1 / 1.5); // 宽度设置为屏幕的0.65
        window.setAttributes(p);
//        initView();
        dialog.getWindow().setBackgroundDrawableResource(R.drawable.parkdowncard_background);
        dialog.setTitle(activity.getResources().getString(R.string.parkMontior_unlicensedVehicleAppearance));

        initView();
    }

    private void initView()
    {
        btnNoPlateOutSearch = (Button) dialog.findViewById(R.id.btnNoPlateOutSearch);
        btnNoPlateOutCalc = (Button) dialog.findViewById(R.id.btnNoPlateOutCalc);
        btnNoPlateOutOpen = (Button) dialog.findViewById(R.id.btnNoPlateOutOpen);
        btnNoPlateOutFreeOpen = (Button) dialog.findViewById(R.id.btnNoPlateOutFreeOpen);
        btnNoPlateOutPrint = (Button) dialog.findViewById(R.id.btnNoPlateOutPrint);
        btnNoPlateOutCancel = (Button) dialog.findViewById(R.id.btnNoPlateOutCancel);
        rgWeixin = (RadioButton) dialog.findViewById(R.id.rgWeixin);
        rgZhifubao = (RadioButton) dialog.findViewById(R.id.rgZhifubao);
        lvNoPlate = (ListView) dialog.findViewById(R.id.lvNoPlate);
        tvMoney = (TextView) dialog.findViewById(R.id.tvMoney);
        ivInPicture = (ImageView) dialog.findViewById(R.id.ivInPicture);
        tvInPicHint = (TextView) dialog.findViewById(R.id.tvInPicHint);
        ivOutPicture = (ImageView) dialog.findViewById(R.id.ivOutPicture);
        tvOutPicHint = (TextView) dialog.findViewById(R.id.tvOutPicHint);

        btnNoPlateOutSearch.setOnClickListener(this);
        btnNoPlateOutCalc.setOnClickListener(this);
        btnNoPlateOutOpen.setOnClickListener(this);
        btnNoPlateOutFreeOpen.setOnClickListener(this);
        btnNoPlateOutPrint.setOnClickListener(this);
        btnNoPlateOutCancel.setOnClickListener(this);


        tvTimeStart = (TextView) dialog.findViewById(R.id.tvTimeStart);
        tvTimeStart.setOnClickListener(this);
        tvTimeStart.setText(TimeConvertUtils.longToString("yyyy/MM/dd", System.currentTimeMillis()));

        bt_selectCarlendarStart = (ImageButton) dialog.findViewById(R.id.bt_selectCarlendarStart);
        bt_selectCarlendarStart.setOnClickListener(this);
        tvTimeEnd = (TextView) dialog.findViewById(R.id.tvTimeEnd);
        tvTimeEnd.setOnClickListener(this);
        tvTimeEnd.setText(TimeConvertUtils.longToString("yyyy/MM/dd", System.currentTimeMillis()));

        bt_selectCarlendarEnd = (ImageButton) dialog.findViewById(R.id.bt_selectCarlendarEnd);
        bt_selectCarlendarEnd.setOnClickListener(this);

//        nSpCarColor = (NiceSpinner) dialog.findViewById(R.id.nSpCarColor);
//        nSpCarColor.setOnClickListener(this);
//        nSpCarType = (NiceSpinner) dialog.findViewById(R.id.nSpCarType);
//        nSpCarType.setOnClickListener(this);
        etInputTempCar = (EditText) dialog.findViewById(R.id.etInputTempCar);
        etInputTempCar.setOnClickListener(this);
//        nSpCarBrand = (NiceSpinner) dialog.findViewById(R.id.nSpCarBrand);
//        nSpCarBrand.setOnClickListener(this);
//        nSpDiscountSpace = (NiceSpinner) dialog.findViewById(R.id.nSpDiscountSpace);
//        nSpDiscountSpace.setOnClickListener(this);
//        nSpOutChannelName = (NiceSpinner) dialog.findViewById(R.id.nSpOutChannelName);
//        nSpOutChannelName.setOnClickListener(this);
//        nSpFreeReason = (NiceSpinner) dialog.findViewById(R.id.nSpFreeReason);
//        nSpFreeReason.setOnClickListener(this);
        tvNeedMoney = (TextView) dialog.findViewById(R.id.tvNeedMoney);
        tvNeedMoney.setOnClickListener(this);

        initListView();
//        initNiceSpinnerView();
    }

    private void initNiceSpinnerView()
    {
        // 初始化车辆颜色
        colorLinkedList = new LinkedList<>();
        String[] stringArray = mActivity.getResources().getStringArray(R.array.noPlateColor);
        colorLinkedList.addAll(Arrays.asList(stringArray));
//        nSpCarColor.attachDataSource(colorLinkedList);

        // 初始化车辆类型
        carTypelinkedList = new LinkedList<>();
        carTypelinkedList.addAll(Arrays.asList("临时车A", "临时车B", "临时车C"));
//        nSpCarType.attachDataSource(carTypelinkedList);

        // 初始化车辆品牌
        brandLinkedList = new LinkedList<>();
        String[] array = mActivity.getResources().getStringArray(R.array.noPlateBrand);
        brandLinkedList.addAll(Arrays.asList(array));
//        nSpCarBrand.attachDataSource(brandLinkedList);

        // 初始化打折地点
        discountSpaceLinkedList = new LinkedList<>();
        discountSpaceLinkedList.addAll(Arrays.asList("地点1", "地点2"));
//        nSpDiscountSpace.attachDataSource(discountSpaceLinkedList);

        // 初始化车辆车道
        channelNameLinkedList = new LinkedList<>();
        channelNameLinkedList.addAll(Arrays.asList("出口车道1", "出口车道2"));
//        nSpOutChannelName.attachDataSource(channelNameLinkedList);

        // 初始化免费原因
        reasonLinkedList = new LinkedList<>();
        reasonLinkedList.addAll(Arrays.asList("reason1", "reason2"));
//        nSpFreeReason.attachDataSource(reasonLinkedList);
    }


    private void initListView()
    {
        /**
         * 显示listView的列表信息
         */
        outList = new ArrayList<>();
        noPlateAdapter = new NoPlateOutListAdapter(outList);
        lvNoPlate.setAdapter(noPlateAdapter);
    }

    public void setData(List inList)
    {
        if (inList == null)
        {
            return;
        }

        outList.clear(); // 如果换成 mEntityBlackLists = entityBlackList; //这里的数据为空??
        outList.addAll(inList);
        noPlateAdapter.notifyDataSetChanged();
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
                calendar = Calendar.getInstance();
                new DatePickerDialog(mActivity, new DatePickerDialog.OnDateSetListener()
                {
                    @Override
                    public void onDateSet(DatePicker view, int year, int month, int day)
                    {
                        mYear = year;
                        mMonth = month;
                        mDay = day;
                        //更新EditText控件日期 小于10加0
                        tvTimeStart.setText(new StringBuilder().append(mYear).append("/")
                                .append((mMonth + 1) < 10 ? "0" + (mMonth + 1) : (mMonth + 1))
                                .append("/").append((mDay < 10) ? "0" + mDay : mDay));
                    }
                }, calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH),
                        calendar.get(Calendar.DAY_OF_MONTH)).show();
                break;
            case R.id.bt_selectCarlendarEnd:
                calendar = Calendar.getInstance();
                new DatePickerDialog(mActivity, new DatePickerDialog.OnDateSetListener()
                {
                    @Override
                    public void onDateSet(DatePicker view, int year, int month, int day)
                    {
                        mYear = year;
                        mMonth = month;
                        mDay = day;
                        //更新EditText控件日期 小于10加0
                        tvTimeEnd.setText(new StringBuilder().append(mYear).append("/")
                                .append((mMonth + 1) < 10 ? "0" + (mMonth + 1) : (mMonth + 1))
                                .append("/").append((mDay < 10) ? "0" + mDay : mDay));
                    }
                }, calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH),
                        calendar.get(Calendar.DAY_OF_MONTH)).show();
                break;
        }
    }

    protected void onBtnOk()
    {

    }

    protected void onBtnCancel()
    {

    }

    public void show()
    {
        if (dialog != null)
        {
            dialog.show();
        }
    }

    public void dismiss()
    {
        if (dialog != null && dialog.isShowing())
        {
            dialog.dismiss();
        }
    }

    class NoPlateOutListAdapter extends BaseAdapter
    {
        private List mList;

        public NoPlateOutListAdapter(List list)
        {
            mList = list;
        }

        @Override
        public int getCount()
        {
            int count = 0;
            if (mList == null)
            {
                count = 1;
            }
            else
            {
                count = mList.size() + 1;
            }
            return count;
        }

        @Override
        public Object getItem(int position)
        {
            if (position != 0)
            {
                return mList.get(position - 1);
            }
            else
            {
                return mList.get(0);
            }

        }

        @Override
        public long getItemId(int position)
        {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = (mActivity).getLayoutInflater().inflate(
                        R.layout.noplate_item, null); // 需要使用getLayoutInflater方法，即需要在该context中
                holder.tv1 = (TextView) convertView.findViewById(R.id.tvText1);
                holder.tv2 = (TextView) convertView.findViewById(R.id.tvText2);
                holder.tv3 = (TextView) convertView.findViewById(R.id.tvText3);
                holder.tv4 = (TextView) convertView.findViewById(R.id.tvText4);
                holder.tv5 = (TextView) convertView.findViewById(R.id.tvText5);
                holder.tv6 = (TextView) convertView.findViewById(R.id.tvText6);
                holder.tv7 = (TextView) convertView.findViewById(R.id.tvText7);
                holder.tv8 = (TextView) convertView.findViewById(R.id.tvText8);
                convertView.setTag(holder);// 保存起来，设置标记
            }
            else
            {
                holder = (ViewHolder) convertView.getTag();// 获取标记
            }

            if (position == 0)
            {
                holder.tv1.setText(R.string.parkMontior_carId);
                holder.tv2.setText(R.string.noPlateOut_Color);
                holder.tv3.setText(R.string.noPlateOut_Brand);
                holder.tv4.setText(R.string.parkMontior_carNo);
                holder.tv5.setText(R.string.parkMontior_carType);
                holder.tv6.setText(R.string.dealLineQuery_personNo);
                holder.tv7.setText(R.string.parkMontior_enterTime);
                holder.tv8.setText(R.string.parkMontior_enterName);
            }
            else
            {

            }
            return convertView;
        }

        class ViewHolder
        {
            public TextView tv1;
            public TextView tv2;
            public TextView tv3;
            public TextView tv4;
            public TextView tv5;
            public TextView tv6;
            public TextView tv7;
            public TextView tv8;
        }
    }

}
