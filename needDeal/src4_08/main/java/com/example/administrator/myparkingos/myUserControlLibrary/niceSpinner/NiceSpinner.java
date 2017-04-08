package com.example.administrator.myparkingos.myUserControlLibrary.niceSpinner;

import android.content.Context;
import android.util.AttributeSet;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.ImageButton;
import android.widget.TextView;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.util.L;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Administrator on 2017-03-31.
 */
public class NiceSpinner extends FrameLayout implements View.OnClickListener
{

    private CustemSpinerAdapter custemSpinerAdapter;
    private List<String> nameList = new ArrayList<String>();
    private SpinerPopWindow spinerPopWindow;
    private ImageButton imageButton;
    private TextView textView;

    public NiceSpinner(Context context)
    {
        super(context);
        initView(context);
    }

    public NiceSpinner(Context context, AttributeSet attrs)
    {
        super(context, attrs);
        initView(context);
    }

    public NiceSpinner(Context context, AttributeSet attrs, int defStyleAttr)
    {
        super(context, attrs, defStyleAttr);
        initView(context);
    }

    private void initView(Context context)
    {
        View.inflate(context, R.layout.popu_window, this);
        setFocusableInTouchMode(true);

        custemSpinerAdapter = new CustemSpinerAdapter(context);
        custemSpinerAdapter.refreshData(nameList, 0);
        spinerPopWindow = new SpinerPopWindow(context);
        spinerPopWindow.setAdatper(custemSpinerAdapter);

        imageButton = (ImageButton) findViewById(R.id.bt_Stationdropdown);
        imageButton.setOnClickListener(this);

        textView = (TextView) findViewById(R.id.tv_Stationvalue);
        spinerPopWindow.setItemListener(new AbstractSpinerAdapter.IOnItemSelectListener()
        {
            @Override
            public void onItemClick(int pos)
            {
                String listDataByIndex = getListDataByIndex(pos);
                textView.setText(listDataByIndex);

                if (mListener != null)
                    mListener.OnSpinnerItemClick(pos);
            }
        });
    }

    private SpinnerListener mListener;

    public void setSpinnerListener(SpinnerListener listener)
    {
        mListener = listener;
    }

    public interface SpinnerListener
    {
        public void OnSpinnerItemClick(int pos);
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.bt_Stationdropdown:
                spinerPopWindow.setWidth(textView.getWidth());
                spinerPopWindow.showAsDropDown(textView);
                break;
        }
    }

    private String getListDataByIndex(int index)
    {
        if (!checkList(nameList, index))
            return null;

        return nameList.get(index);
    }


    public void refreshData(List<String> list, int index)
    {
        if (!checkList(list, index))
            return;

        nameList.clear();
        nameList.addAll(list);
        custemSpinerAdapter.refreshData(nameList, index);
        textView.setText(getListDataByIndex(index));
    }

    public void addEachData(String item, int index)
    {
        if (item == null)
            return;
        nameList.add(0, item);
        custemSpinerAdapter.refreshData(nameList, index);
        if (!checkList(nameList, index))
            return;

        textView.setText(getListDataByIndex(index));
    }

    public void addDataList(List<String> list, int index)
    {
        if (!checkList(list, index))
            return;

        custemSpinerAdapter.refreshData(nameList, index);
        textView.setText(getListDataByIndex(index));
    }

    class CustemSpinerAdapter extends AbstractSpinerAdapter<String>
    {
        public CustemSpinerAdapter(Context context)
        {
            super(context);
        }
    }

    public void setSelectIndex(int selectIndex)
    {
        if (!checkList(nameList, selectIndex))
            return;
        textView.setText(nameList.get(selectIndex));
    }

    private boolean checkList(List<String> list, int index)
    {
        if (list == null || list.size() == 0)
        {
            L.e("if (list == null || list.size() == 0)");
            return false;
        }

        int size = list.size();
        if (index >= 0 && index < size)
        {
            return true;
        }
        L.e("index < 0 || index >= size");
        return false;
    }

    public String getCurrentText()
    {
        return textView.getText().toString();
    }

}
