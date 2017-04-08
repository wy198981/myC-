package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.app.Activity;
import android.app.Dialog;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.Display;
import android.view.Gravity;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.myUserControlLibrary.radioBtn.FlowRadioGroup;
import com.example.administrator.myparkingos.myUserControlLibrary.spinnerList.AbstractSpinerAdapter;
import com.example.administrator.myparkingos.myUserControlLibrary.spinnerList.SpinerPopWindow;
import com.example.administrator.myparkingos.util.CommUtils;
import com.example.administrator.myparkingos.util.L;
import com.example.administrator.myparkingos.util.ScreenUtils;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Administrator on 2017-03-10.
 */
public class ParkingPlateNoInputView implements AbstractSpinerAdapter.IOnItemSelectListener
{
    private Dialog dialog;
    private FlowRadioGroup rgSelectProvince;
    private Button btnCarInputOk;
    private Button btnCarIntCancel;
    private Activity mActivity;
    private EditText etCPH;
    private String province = null;
    private int mInOut = 0;// 0,in;1,out
    private TextView tvProvinceText;
    private SpinerPopWindow mPopWindow;
    private AbstractSpinerAdapter popAdapter;
    private ArrayList<String> carPopData;
    private RadioGroup rgCarPlateClass;

    public ParkingPlateNoInputView(Activity activity, int inOut)
    {
        this.mInOut = inOut;
        this.mActivity = activity;
        dialog = new Dialog(activity); // @android:style/Theme.Dialog
        dialog.setContentView(R.layout.dialog_carin);
        dialog.setCanceledOnTouchOutside(true);

        Window window = dialog.getWindow();
        WindowManager m = activity.getWindowManager();
        Display d = m.getDefaultDisplay(); // 获取屏幕宽、高用
        WindowManager.LayoutParams p = window.getAttributes(); // 获取对话框当前的参数值
        p.height = (int) (d.getHeight() * 1 / 1.7); // 改变的是dialog框在屏幕中的位置而不是大小
        p.width = (int) (d.getWidth() * 1 / 4.5); // 宽度设置为屏幕的0.65 (这个高度正好 1/1.7 1/4.5)
        window.setAttributes(p);

        dialog.setTitle(activity.getResources().getString(R.string.parkMontior_carInTitle));

        initView();
    }


    private void initView()
    {
        rgSelectProvince = (FlowRadioGroup) dialog.findViewById(R.id.rgSelectProvince);

        btnCarInputOk = (Button) dialog.findViewById(R.id.btnCarInputOk);
        btnCarIntCancel = (Button) dialog.findViewById(R.id.btnCarIntCancel);

        etCPH = (EditText) dialog.findViewById(R.id.etCPH);
        tvProvinceText = (TextView) dialog.findViewById(R.id.tvProvinceText);

        rgCarPlateClass = (RadioGroup) dialog.findViewById(R.id.rgCarPlateClass);

        btnCarInputOk.setOnClickListener(diaListern);
        btnCarIntCancel.setOnClickListener(diaListern);

        RadioButton childAt = (RadioButton) rgCarPlateClass.getChildAt(0);
        childAt.setChecked(true);

        rgCarPlateClass.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener()
        {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId)
            {
                for (int i = 0; i < group.getChildCount(); i++)
                {
                    if (group.getChildAt(i).getId() == checkedId)
                    {
                        // 对于选中的RadioButton 来设置
                        break;
                    }
                }
            }
        });

        rgSelectProvince.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener()
        {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId)
            {
                for (int i = 0; i < group.getChildCount(); i++)
                {
                    if (group.getChildAt(i).getId() == checkedId)
                    {
                        String text = mActivity.getResources().getStringArray(R.array.provinceArray)[i];
                        tvProvinceText.setText(text);
                        break;
                    }
                }
            }
        });


        final int charMaxNum = 6;// 个数；
        etCPH.addTextChangedListener(new TextWatcher()
        {
            private CharSequence temp;//监听前的文本

            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after)
            {
                temp = s;
            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count)
            {

            }

            @Override
            public void afterTextChanged(Editable s)
            {
                /** 得到光标开始和结束位置 ,超过最大数后记录刚超出的数字索引进行控制 */
                int editStart = etCPH.getSelectionStart();
                int editEnd = etCPH.getSelectionEnd();
                L.i("editStart:" + editStart + ", editEnd:" + editEnd + "temp.length():" + temp.length());


                String currentText = s.toString();
                if (editStart == 0)
                {
                    return;
                }

                String substring = currentText.substring(editStart - 1, editStart);
                if (substring.equals("o") || substring.equals("i"))
                {
                    Toast.makeText(mActivity, "你输入的字数含o或者i！", Toast.LENGTH_LONG).show();
                    s.delete(editStart - 1, editEnd);
                    etCPH.setText(s);
                    etCPH.setSelection(editStart - 1);
                    return;
                }

                if (temp.length() > charMaxNum)
                {
                    Toast.makeText(mActivity, "你输入的字数已经超过了限制！", Toast.LENGTH_LONG).show();
                    s.delete(editStart - 1, editEnd);
                    int tempSelection = editStart;
                    etCPH.setText(s);
                    etCPH.setSelection(tempSelection - 1);
                    return;
                }

                String CPH = s.toString().trim();
                if (temp.length() >= 3) // 长度大于3之后，就可以提供相应的提示
                {
                    String resultCPH = province + CPH;
                    if (mInOut == 0)
                    {
                        onClickInTextInput(resultCPH, temp.length()); // 在入场时，也是模糊对比进场获取车牌
                    }
                    else if (mInOut == 1)
                    {
                        L.i("mInOut == 1" + resultCPH);
                        onClickOutTextInput(resultCPH, temp.length()); // 在出场时，也是模糊对比进场获取车牌
                    }
                }
            }
        });

        // 显示PopWindow弹出框
        mPopWindow = new SpinerPopWindow(mActivity);
        popAdapter = new AbstractSpinerAdapter(mActivity);
        carPopData = new ArrayList<>();

        popAdapter.refreshData(carPopData, 0);
        mPopWindow.setAdatper(popAdapter);
        mPopWindow.setItemListener(this);
    }

    public void onClickOutTextInput(String resultCPH, int length)
    {

    }

    public void onClickInTextInput(final String s, final int Precision)
    {

    }

    private View.OnClickListener diaListern = new View.OnClickListener()
    {
        @Override
        public void onClick(View v)
        {
            switch (v.getId())
            {
                case R.id.btnCarInputOk:
                {

                    for (int i = 0; i < rgSelectProvince.getChildCount(); i++)
                    {
                        RadioButton tempRadio = (RadioButton) rgSelectProvince.getChildAt(i);
                        if (tempRadio.isChecked())
                        {
                            province = mActivity.getResources().getStringArray(R.array.provinceArray)[i];
                            break;
                        }
                    }
                    String allCPH = province + etCPH.getText().toString();
                    L.i("显示的车牌:" + allCPH);
                    if (CommUtils.CheckUpCPH(allCPH))
                    {
                        if (mInOut == 0) //入场
                        {
                            onCarInBtnOk(allCPH);
                        }
                        else//出场
                        {
                            onCarOutBtnOk(allCPH);
                        }
                    }
                    else
                    {
                        Toast.makeText(mActivity, "输入的车牌" + allCPH + "格式不符合", Toast.LENGTH_LONG).show();
                    }
                    break;
                }
                case R.id.btnCarIntCancel:
                {
                    onBtnCancel();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    };

    public void onCarOutBtnOk(String s)
    {

    }

    protected void onCarInBtnOk(final String getCPH)
    {

    }

    protected void onBtnCancel()
    {

    }

    public void show()
    {
        if (dialog != null)
        {
            prepareLoadData();
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

    /**
     * 设置默认的省份
     *
     * @param str
     */
    public void setProvince(String str)
    {
        String[] stringArray = mActivity.getResources().getStringArray(R.array.provinceArray);
        province = str;
        for (int i = 0; i < stringArray.length; i++)
        {
            if (str.equals(stringArray[i]))
            {
                RadioButton tempRadio = (RadioButton) rgSelectProvince.getChildAt(i);
                tempRadio.setChecked(true);
                province = str;
                break;
            }
        }
        tvProvinceText.setText(province);
    }

    public void prepareLoadData()
    {
        etCPH.setText("");
    }


    public void setCompleteCPHText(List<String> cphList)
    {
        carPopData.clear();
        carPopData.addAll(cphList);
        popAdapter.notifyDataSetChanged();
    }


    @Override
    public void onItemClick(int pos)
    {
        if (pos >= 0 && pos < carPopData.size())
        {
            String substring = carPopData.get(pos).substring(0, 1);// 截取省份简称，设置到界面
            setProvince(substring);
            etCPH.setText(carPopData.get(pos).substring(1));
        }
    }

    public void showPopWindow()
    {
        mPopWindow.setWidth(etCPH.getWidth());
        mPopWindow.showAsDropDown(etCPH);
    }

}



