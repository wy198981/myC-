package com.znykt.zhpark;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.v4.view.ViewPager;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Toast;

 

import java.util.ArrayList;

 
import com.znykt.zhpark.R;
import com.znykt.zhpark.Until.AppManager;

/**
 * Created by Administrator on 2016-08-31.
 */
public class SplashActivity extends Activity implements View.OnClickListener, ViewPager.OnPageChangeListener {

    private boolean misScrolled = false;

    // 定义ViewPager对象
    private ViewPager viewPager;
    // 定义ViewPager适配器
    private ViewPagerAdapter vpAdapter;
    // 定义一个ArrayList来存放View
    private ArrayList<View> views;
    // 引导图片资源
    private static final int[] pics = {R.drawable.guide1, R.drawable.guide2,
            R.drawable.guide3, R.drawable.guide4};
    // 底部小点的图片
    private ImageView[] points;
    // 记录当前选中位置
    private int currentIndex;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);
        initView();
        initData(vpAdapter);
        AppManager.getAppManager().addActivity(this);
    }

    /**
     * 初始化组件
     */
    private void initView() {
        // 实例化ArrayList对象
        views = new ArrayList<View>();
        // 实例化ViewPager
        viewPager = (ViewPager) findViewById(R.id.viewpager);
        // 实例化ViewPager适配器
        vpAdapter = new ViewPagerAdapter(views);
    }

    /**
     * 初始化底部小点
     */
    private void initPoint() {
        LinearLayout linearLayout = (LinearLayout) findViewById(R.id.ll);

        points = new ImageView[pics.length];

        // 循环取得小点图片
        for (int i = 0; i < pics.length; i++) {
            // 得到一个LinearLayout下面的每一个子元素
            points[i] = (ImageView) linearLayout.getChildAt(i);
            // 默认都设为灰色
            points[i].setEnabled(true);
            // 给每个小点设置监听
            points[i].setOnClickListener(this);
            // 设置位置tag，方便取出与当前位置对应
            points[i].setTag(i);
        }

        // 设置当面默认的位置
        currentIndex = 0;
        // 设置为白色，即选中状态
        points[currentIndex].setEnabled(false);
    }

    /**
     * 滑动状态改变时调用
     */
    @Override
    public void onPageScrollStateChanged(int state) {
        switch (state) {

            case ViewPager.SCROLL_STATE_DRAGGING://1 dragging（拖动），理解为：只要触发拖动/滑动事件时，则 state = ViewPager.SCROLL_STATE_DRAGGING
                misScrolled = false;

                break;
            case ViewPager.SCROLL_STATE_SETTLING://2 settling(安放、定居、解决)，理解为：通过拖动/滑动，安放到了目标页，则 state = ViewPager.SCROLL_STATE_SETTLING
                misScrolled = true;

                break;
            case ViewPager.SCROLL_STATE_IDLE://0 idle(空闲，挂空挡)， 理解为：只要拖动/滑动结束，无论是否安放到了目标页，则 state = ViewPager.SCROLL_STATE_IDLE
                if (viewPager.getCurrentItem() == viewPager.getAdapter().getCount() - 1 && !misScrolled) {//如果当前页是最后一页，并且滑动，则触发finish()
                    /*此处可写一些逻辑，如finish() 或 startActivity()
                    finish();*/
                    Log.e("在末页向左滑", "state:" + state + "---------->misScrolled:" + misScrolled + "---------->现在的页码索引:" + viewPager.getCurrentItem());
                    Toast.makeText(this, "已经是最后一页", 2).show();

                    AppManager.getAppManager().finishActivity(this);
                }

                if (viewPager.getCurrentItem() == 0 && !misScrolled) {//如果当前页是第一页，并且滑动，则触发finish()
                    /*此处可写一些逻辑，如finish() 或 startActivity()
                    finish();*/
                    Log.e("在首页向右滑", "state:" + state + "---------->misScrolled:" + misScrolled + "---------->现在的页码索引:" + viewPager.getCurrentItem());
                    Toast.makeText(this, "智慧停车带您开启智能云生活", 2).show();
                }
                misScrolled = true;

                break;
        }

    }

    /**
     * 当前页面滑动时调用
     */
    @Override
    public void onPageScrolled(int arg0, float arg1, int arg2) {

    }

    /**
     * 新的页面被选中时调用
     */
    @Override
    public void onPageSelected(int arg0) {
        // 设置底部小点选中状态
        setCurDot(arg0);
    }

    @Override
    public void onClick(View v) {
        int position = (Integer) v.getTag();
        setCurView(position);
        setCurDot(position);
    }

    /**
     * 设置当前页面的位置
     */
    private void setCurView(int position) {
        if (position < 0 || position >= pics.length) {
            return;
        }
        viewPager.setCurrentItem(position);
    }

    /**
     * 设置当前的小点的位置
     */
    private void setCurDot(int positon) {
        if (positon < 0 || positon > pics.length - 1 || currentIndex == positon) {
            return;
        }
        points[positon].setEnabled(false);
        points[currentIndex].setEnabled(true);

        currentIndex = positon;
    }

    /**
     * 初始化数据
     *
     * @param viewPagerAdapter
     */
    void initData(ViewPagerAdapter viewPagerAdapter) {
        // 定义一个布局并设置参数
        LinearLayout.LayoutParams mParams = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.MATCH_PARENT);

        // 初始化引导图片列表
        for (int i = 0; i < pics.length; i++) {
            ImageView iv = new ImageView(this);
            iv.setLayoutParams(mParams);
            //防止图片不能填满屏幕444
            iv.setScaleType(ImageView.ScaleType.FIT_XY);
            //加载图片资源
            iv.setImageResource(pics[i]);
            views.add(iv);
        }

        // 设置数据
        viewPager.setAdapter(viewPagerAdapter);
        // 设置监听
        viewPager.setOnPageChangeListener(this);

        // 初始化底部小点
        initPoint();
    }
}

