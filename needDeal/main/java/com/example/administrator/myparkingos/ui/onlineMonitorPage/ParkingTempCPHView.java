package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.app.Activity;
import android.app.Dialog;
import android.text.TextUtils;
import android.util.Log;
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
import com.example.administrator.myparkingos.model.requestInfo.SetCarInConfirmReq;
import com.example.administrator.myparkingos.util.CommUtils;
import com.example.administrator.myparkingos.util.L;
import com.example.administrator.myparkingos.util.T;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Administrator on 2017-03-28.
 */
public class ParkingTempCPHView implements View.OnClickListener
{
    private final Activity mActivity;
    private final Dialog dialog;
    private RadioButton rbSelect;
    private Spinner spinnerProvince;
    private EditText etCarNo;
    private Spinner spinnerRoadName;
    private Button btnOk;
    private Button btnCancel;
    private ArrayAdapter provinceAdapter;
    private ArrayList<String> roadList;
    private ArrayAdapter roadAdapter;


    public ParkingTempCPHView(Activity activity)
    {
        this.mActivity = activity;

        dialog = new Dialog(activity); // @android:style/Theme.Dialog
        dialog.setContentView(R.layout.parkingin_tempcph);
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
        rbSelect = (RadioButton) dialog.findViewById(R.id.rbSelect);
        spinnerProvince = (Spinner) dialog.findViewById(R.id.spinnerProvince);
        etCarNo = (EditText) dialog.findViewById(R.id.etCarNo);
        spinnerRoadName = (Spinner) dialog.findViewById(R.id.spinnerRoadName);
        btnOk = (Button) dialog.findViewById(R.id.btnOk);
        btnCancel = (Button) dialog.findViewById(R.id.btnCancel);

        btnOk.setOnClickListener(this);
        btnCancel.setOnClickListener(this);

        rbSelect.setChecked(true);
        provinceAdapter = new ArrayAdapter(mActivity.getApplicationContext(), R.layout.blacklist_spinner_province
                , mActivity.getResources().getStringArray(R.array.provinceArray));
        spinnerProvince.setAdapter(provinceAdapter);

        roadList = new ArrayList<>();
        roadAdapter = new ArrayAdapter(mActivity.getApplicationContext(), R.layout.blacklist_spinner_province
                , roadList);
        spinnerRoadName.setAdapter(roadAdapter);
    }

    private String srcCPH = null;
    public void setEtCarNoContent(String txt)
    {
        etCarNo.setText(txt);
    }

    public void setCPH(String cph)
    {
        setSpinnerProvinceContent(cph.substring(0, 1));
        setEtCarNoContent(cph.substring(1));
        srcCPH = cph;
    }


    public void setSpinnerProvinceContent(String txt)
    {
        if (txt == null) return;
        L.i("text:" + txt);
        String[] provinceArray = mActivity.getResources().getStringArray(R.array.provinceArray);
        for (int i = 0; i  < provinceArray.length; i++)
        {
            if (txt.equals(provinceArray[i]))
            {
                spinnerProvince.setSelection(i);
                break;
            }
        }
    }

    public void setSpinnerRoadName(List<String> inList)
    {
        if (inList == null || inList.size() < 0)
        {
            return;
        }

        roadList.clear();
        roadList.addAll(inList);
        roadAdapter.notifyDataSetChanged();
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnOk:
                String cph = etCarNo.getText().toString().trim();
                String resultCPH;
                if (TextUtils.isEmpty(cph))
                {
                    T.showShort(mActivity, "车牌号码为空" );
                    return ;
                }
                else
                {
                    String province = spinnerProvince.getSelectedItem().toString();
                    resultCPH = province + cph;
                    if (!CommUtils.CheckUpCPH(resultCPH))
                    {
                        T.showShort(mActivity, "车牌号码[" + resultCPH + "]不符号要求");
                        return;
                    }
                }
                SetCarInConfirmReq setCarInConfirmReq = new SetCarInConfirmReq();
                setCarInConfirmReq.setCPHConfirmed(resultCPH);
                setCarInConfirmReq.setCPH(srcCPH);
                onClickOk(setCarInConfirmReq, spinnerRoadName.getSelectedItem().toString());
                break;
            case R.id.btnCancel:
                onClickCancel();
                break;
        }
    }

    public void onClickCancel()
    {

    }

    /**
     * 从界面上获取数据，进行相应的处理
     * @param setCarInConfirmReq
     * @param roadName
     */
    public void onClickOk(SetCarInConfirmReq setCarInConfirmReq, String roadName)
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

    private void clearDataInView()
    {
        etCarNo.setText("");
    }
}
