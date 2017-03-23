
package com.xjs.example.time.view;

import android.app.Activity;
import android.content.Context;
import android.graphics.drawable.ColorDrawable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup.LayoutParams;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.PopupWindow;

import com.jovision.mobbroadcast.time.WheelMain;
import com.example.vzvision.R;

public class DateTimePickerView extends PopupWindow {

    private View mContentView;
    private View timepickerview;
    public WheelMain wheelMain;
    private Button mSave, mCancel;

    public DateTimePickerView(Activity context, OnClickListener itemsOnClick) {
        super(context);
        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
       
        mContentView = inflater.inflate(R.layout.dialog_time, null);
        timepickerview = (LinearLayout) mContentView.findViewById(R.id.timePicker1);
        wheelMain = new WheelMain(timepickerview, 0);
        mSave = (Button) mContentView.findViewById(R.id.btn_ok);
        mCancel = (Button) mContentView.findViewById(R.id.btn_ng);
        wheelMain.initDateTimePicker();

        mSave.setOnClickListener(itemsOnClick);
        mCancel.setOnClickListener(itemsOnClick);
        // 璁剧疆SelectPicPopupWindow鐨刅iew
        this.setContentView(mContentView);
        // 璁剧疆SelectPicPopupWindow寮瑰嚭绐椾綋鐨勫
        this.setWidth(LayoutParams.MATCH_PARENT);
        // 璁剧疆SelectPicPopupWindow寮瑰嚭绐椾綋鐨勯珮
        this.setHeight(LayoutParams.WRAP_CONTENT);
        // 璁剧疆SelectPicPopupWindow寮瑰嚭绐椾綋鍙偣鍑�
        this.setFocusable(true);
        ColorDrawable dw = new ColorDrawable(0x00);
        setBackgroundDrawable(dw);
        // 璁剧疆SelectPicPopupWindow寮瑰嚭绐椾綋鍔ㄧ敾鏁堟灉
        // this.setAnimationStyle(R.style.popupAnimation);
        // mMenuView娣诲姞OnTouchListener鐩戝惉鍒ゆ柇鑾峰彇瑙﹀睆浣嶇疆濡傛灉鍦ㄩ�鎷╂澶栭潰鍒欓攢姣佸脊鍑烘
    }

}
