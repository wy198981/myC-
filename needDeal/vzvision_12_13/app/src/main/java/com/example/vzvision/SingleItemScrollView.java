package com.example.vzvision;

import android.content.Context;
import android.util.AttributeSet;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.ScrollView;

public class SingleItemScrollView extends ScrollView implements OnClickListener
{

	/**
	 * Item点击的回�?
	 */
	private OnItemClickListener mListener;

//	private Adapter mAdapter;
	/**
	 * 屏幕的高�?
	 */
	private int mScreenHeight;
	/**
	 * 每个条目的高�?
	 */
	private int mItemHeight;
	private ViewGroup mContainer;

	/**
	 * 条目总数
	 */
	private int mItemCount;

	private boolean flag;

	/**
	 * 适配�?
	 * @author zhy
	 *
	 */
	public static abstract class Adapter
	{
		public abstract View getView(SingleItemScrollView parent, int pos);

		public abstract int getCount();
	}

	/**
	 * 点击的回�?
	 */
	public interface OnItemClickListener
	{
		void onItemClick(int pos, View view);
	}

	public SingleItemScrollView(Context context, AttributeSet attrs)
	{
		super(context, attrs);

		// 计算屏幕的高�?
		WindowManager wm = (WindowManager) context
				.getSystemService(Context.WINDOW_SERVICE);
		DisplayMetrics outMetrics = new DisplayMetrics();
		wm.getDefaultDisplay().getMetrics(outMetrics);
		mScreenHeight = outMetrics.heightPixels;
		mScreenHeight -= getStatusHeight(context);

	}

	@Override
	protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec)
	{
		//防止多次调用
		if (!flag)
		{
			mContainer = (ViewGroup) getChildAt(0);

			//根据Adapter的方法，为容器添加Item
//			if (mAdapter != null)
//			{
//				mItemCount = mAdapter.getCount();
//				mItemHeight = mScreenHeight / mItemCount;
//				mContainer.removeAllViews();
//				for (int i = 0; i < mAdapter.getCount(); i++)
//				{
//					addChildView(i);
//				}
//			}
//			addChildView(0);
		}

		super.onMeasure(widthMeasureSpec, heightMeasureSpec);
	}

	/**
	 * 在容器末尾添加一个Item
	 * @param i
	 */
	public void addChildView(  View item )
	{
		 
		//设置参数
		android.view.ViewGroup.LayoutParams lp = new ViewGroup.LayoutParams(
				android.view.ViewGroup.LayoutParams.MATCH_PARENT, mItemHeight);
		item.setLayoutParams(lp);
		 
		//添加事件
		item.setOnClickListener(this);
		mContainer.addView(item);
	}
 

	@Override
	public boolean onTouchEvent(MotionEvent ev)
	{
		flag = true;
		int action = ev.getAction();
		int scrollY = getScrollY();
		switch (action)
		{
		case MotionEvent.ACTION_MOVE:
			Log.e("TAG", "scrollY = " + scrollY);
		 
			break;
		case MotionEvent.ACTION_UP:
			 
			return true;
		default:
			break;
		}
		return super.onTouchEvent(ev);
	}
	 
	

	/**
	 * 获得状�?栏的高度
	 * 
	 * @param context
	 * @return
	 */
	public int getStatusHeight(Context context)
	{

		int statusHeight = -1;
		try
		{
			Class<?> clazz = Class.forName("com.android.internal.R$dimen");
			Object object = clazz.newInstance();
			int height = Integer.parseInt(clazz.getField("status_bar_height")
					.get(object).toString());
			statusHeight = context.getResources().getDimensionPixelSize(height);
		} catch (Exception e)
		{
			e.printStackTrace();
		}
		return statusHeight;
	}

 
	public void setOnItemClickListener(OnItemClickListener mListener)
	{
		this.mListener = mListener;
	}

	@Override
	public void onClick(View v)
	{
		int pos = (Integer) v.getTag();
		if (mListener != null)
		{
			mListener.onItemClick(pos, v);
		}
	}

}
