package com.example.administrator.myparkingos.ui;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.util.L;

/**
 * Created by Administrator on 2017-03-27.
 */
public class ChargePromptFragment extends Fragment
{
    private TextView textView;

    /**
     * 显示提示信息
     */
    @Override
    public void onCreate(@Nullable Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
    }

    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState)
    {
        View root = inflater.inflate(R.layout.parkmonitor_prompt_item, container, false);
        initView(root);
        L.i("debug" + "ChargePromptFragment onCreateView");
        return root;
    }

    private void initView(View root)
    {
        textView = (TextView) root.findViewById(R.id.tvTextPrompt);
    }

    public void setViewPrompt(String text)
    {
        L.i("setViewPrompt:" + text);
        textView.setText(text);
    }
}
