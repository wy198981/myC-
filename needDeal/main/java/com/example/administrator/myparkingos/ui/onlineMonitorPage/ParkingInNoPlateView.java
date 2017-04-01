package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.app.Activity;
import android.app.Dialog;
import android.graphics.Bitmap;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.Log;
import android.view.Display;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.Toast;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInWithoutCPHReq;
import com.example.administrator.myparkingos.myUserControlLibrary.niceSpinner.NiceSpinner;
import com.example.administrator.myparkingos.myUserControlLibrary.radioBtn.FlowRadioGroup;
import com.example.administrator.myparkingos.util.BitmapUtils;
import com.example.administrator.myparkingos.util.CommUtils;
import com.example.administrator.myparkingos.util.L;
import com.example.administrator.myparkingos.util.RegexUtil;
import com.example.administrator.myparkingos.util.T;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Created by Administrator on 2017-03-10.
 */
public class ParkingInNoPlateView implements View.OnClickListener
{
    private Dialog dialog;
    private NiceSpinner spinnerColor;
    private NiceSpinner spinnerCarBrand;
    private Button btnAdd;
    private Spinner spinnerProvince;
    private EditText etInputCarNo;
    private NiceSpinner spinnerRoadName;
    private Button btnCancel;
    private ImageView imagePicture;
    private Activity mActivity;
    private ArrayAdapter colorAdapter;
    private ArrayAdapter provinceAdapter;
    private ArrayAdapter brandAdapter;
    private ArrayList<String> roadList;
    private ArrayAdapter roadAdapter;

    public ParkingInNoPlateView(Activity activity)
    {
        mActivity = activity;
        dialog = new Dialog(activity); // @android:style/Theme.Dialog
        dialog.setContentView(R.layout.parkingin_noplate);
        dialog.setCanceledOnTouchOutside(true);

        Window window = dialog.getWindow();
        WindowManager m = activity.getWindowManager();
        Display d = m.getDefaultDisplay(); // 获取屏幕宽、高用
        WindowManager.LayoutParams p = window.getAttributes(); // 获取对话框当前的参数值
        p.height = (int) (d.getHeight() * 2 / 3); // 改变的是dialog框在屏幕中的位置而不是大小
        p.width = (int) (d.getWidth() * 1 / 3); // 宽度设置为屏幕的0.65
        window.setAttributes(p);

        initView();
        dialog.getWindow().setBackgroundDrawableResource(R.drawable.parkdowncard_background);
        dialog.setTitle(activity.getResources().getString(R.string.parkMontior_unlicensedVehicleAdmission));
    }

    private void initView()
    {
        spinnerColor = (NiceSpinner) dialog.findViewById(R.id.spinnerColor);
        spinnerCarBrand = (NiceSpinner) dialog.findViewById(R.id.spinnerCarBrand);
        btnAdd = (Button) dialog.findViewById(R.id.btnAdd);
        spinnerProvince = (Spinner) dialog.findViewById(R.id.spinnerProvince);
        etInputCarNo = (EditText) dialog.findViewById(R.id.etInputCarNo);
        spinnerRoadName = (NiceSpinner) dialog.findViewById(R.id.spinnerRoadName);
        btnCancel = (Button) dialog.findViewById(R.id.btnCancel);
        imagePicture = (ImageView) dialog.findViewById(R.id.imagePicture);

        btnAdd.setOnClickListener(this);
        btnCancel.setOnClickListener(this);

        String[] stringArray = mActivity.getResources().getStringArray(R.array.noPlateColor);
        spinnerColor.refreshData(Arrays.asList(stringArray), 0);

        provinceAdapter = new ArrayAdapter(mActivity.getApplicationContext(), R.layout.blacklist_spinner_province
                , mActivity.getResources().getStringArray(R.array.provinceArray));
        spinnerProvince.setAdapter(provinceAdapter);

        String[] tempArray = mActivity.getResources().getStringArray(R.array.noPlateBrand);
        spinnerCarBrand.refreshData(Arrays.asList(tempArray), 0);

        spinnerRoadName.refreshData(roadList, 0);

        final int charMaxNum = 6;// 个数；
        etInputCarNo.addTextChangedListener(new TextWatcher()
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
                int editStart = etInputCarNo.getSelectionStart();
                int editEnd = etInputCarNo.getSelectionEnd();
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
                    etInputCarNo.setText(s);
                    etInputCarNo.setSelection(editStart - 1);
                    return;
                }

                if (temp.length() > charMaxNum)
                {
                    Toast.makeText(mActivity, "你输入的字数已经超过了限制！", Toast.LENGTH_LONG).show();
                    s.delete(editStart - 1, editEnd);
                    int tempSelection = editStart;
                    etInputCarNo.setText(s);
                    etInputCarNo.setSelection(tempSelection - 1);
                    return;
                }
            }
        });
    }

    /**
     * 设置车道的数据
     *
     * @param inList
     */
    public void setRoadNameData(List<String> inList)
    {
        if (inList == null || inList.size() <= 0)
        {
            return;
        }

        spinnerRoadName.refreshData(inList, 0);
    }

    /**
     * 设置显示的图像数据
     */
    public void setImage(String path)
    {
        Bitmap bitmap = BitmapUtils.fileToBitmap(path);
        imagePicture.setImageBitmap(bitmap);
    }

    protected void onBtnOk(SetCarInWithoutCPHReq carInWithoutCPHReq, String roadName)
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

    /**
     * 提前加载数据
     */
    public void prepareLoadData()
    {

    }

    public void dismiss()
    {
        if (dialog != null && dialog.isShowing())
        {
            dialog.dismiss();
        }
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnAdd:
            {
                String resultCPH = null;
                String cph = etInputCarNo.getText().toString().trim();

                L.i("cph:" + cph);
                if (TextUtils.isEmpty(cph))
                {
                    L.i("车牌为空");
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
                SetCarInWithoutCPHReq setCarInWithoutCPHReq = new SetCarInWithoutCPHReq();

                setCarInWithoutCPHReq.setCarColor(spinnerColor.getCurrentText());
                setCarInWithoutCPHReq.setCarBrand(spinnerCarBrand.getCurrentText());
                if (resultCPH != null)
                    setCarInWithoutCPHReq.setCPH(resultCPH);

                onBtnOk(setCarInWithoutCPHReq, spinnerRoadName.getCurrentText());
                break;
            }
            case R.id.btnCancel:
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

    public void cleanCarNo()
    {
        etInputCarNo.setText("");
    }

}
