package com.znykt.zhpark;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.igexin.sdk.PushManager;
import com.nostra13.universalimageloader.cache.disc.naming.Md5FileNameGenerator;
import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.QueueProcessingType;
import com.zbar.lib.CaptureActivity;

import com.znykt.zhpark.Cycleviewpager.CycleViewPager;
import com.znykt.zhpark.Cycleviewpager.ViewFactory;
import com.znykt.zhpark.Model.ADInfo;
import com.znykt.zhpark.Notice.PushDemoReceiver;
import com.znykt.zhpark.Until.Constant;
import com.znykt.zhpark.Until.SharedPrefsUtil;
import com.znykt.zhpark.wxapi.WXEntryActivity;

public class MainActivity extends Activity
{

    /**
     * 第三方应用Master Secret，修改为正确的值
     */
    private static final String MASTERSECRET = "D7POYyh8KN8qPYmfPs0Tl6";

    private static final int REQUEST_PERMISSION = 0;
    /**
     * SDK服务是否启动
     */
    private boolean isServiceRunning = true;
    private Context context;
    private SimpleDateFormat formatter;
    private Date curDate;

    // SDK参数，会自动从Manifest文件中读取，第三方无需修改下列变量，请修改AndroidManifest.xml文件中相应的meta-data信息。
    // 修改方式参见个推SDK文档
    private String appkey = "";
    private String appsecret = "";
    private String appid = "";

    // 表示扫码付款返回的结果码
    private final static int SCANREQUESTCODE = 1;// 表示返回的结果码
    private final static int ParkListREQUESTCODE = 2;// 表示返回的停车订单

    private final static int PERSONCENTERREQUESTCODE = 3;// 用户中心返回码
    private ImageView iv_personManger;// 用户中心 按钮
    private LinearLayout ll_scanPay;// 扫码缴费按钮
    private LinearLayout ll_parkorder;// 停车订单 按钮
    private LinearLayout ll_parkfind;// 停车导航 按钮
    private List<ImageView> views = new ArrayList<ImageView>();
    private List<ADInfo> infos = new ArrayList<ADInfo>();
    private CycleViewPager cycleViewPager;
    private String[] imageUrls = {
            "http://new.168parking.com/OrderImg/33bdc6ff0469c21cfbe96e3c61f8043b/1.jpg",
            "http://new.168parking.com/OrderImg/33bdc6ff0469c21cfbe96e3c61f8043b/2.jpg",
            "http://new.168parking.com/OrderImg/33bdc6ff0469c21cfbe96e3c61f8043b/3.jpg",
            "http://new.168parking.com/OrderImg/33bdc6ff0469c21cfbe96e3c61f8043b/4.jpg"
    };

    @Override
    protected void onCreate(Bundle savedInstanceState)
    {

        super.onCreate(savedInstanceState);
        // 判断是否是第一次登录，如果是就加载欢迎引导翻页效果
        boolean Isfrist = SharedPrefsUtil.getValue(this, "ConfigParking",
                "IsFrist", true);
        if (Isfrist)
        {
            Intent intent = new Intent(this, SplashActivity.class); // 进入引导轮播
            startActivity(intent);
        }
        SharedPrefsUtil.putValue(this, "ConfigParking", "IsFrist", false);

        Constant.isLogin = SharedPrefsUtil.getValue(this, "ConfigParking",
                "IsLogin", false);
        Constant.isLogin = true;

        setContentView(R.layout.activity_main);

        // 加载首页显示的轮播图片
        configImageLoader();
        initialize();
        InistalView();// 初始化主页按钮

        loadPullInist();

    }

    private void loadPullInist()
    {
        formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

        // 从AndroidManifest.xml的meta-data中读取SDK配置信息
        String packageName = getApplicationContext().getPackageName();
        try
        {
            ApplicationInfo appInfo = getPackageManager().getApplicationInfo(
                    packageName, PackageManager.GET_META_DATA);
            if (appInfo.metaData != null)
            {
                appid = appInfo.metaData.getString("PUSH_APPID");
                appsecret = appInfo.metaData.getString("PUSH_APPSECRET");
                appkey = appInfo.metaData.getString("PUSH_APPKEY");

                PushManager.getInstance().bindAlias(MainActivity.this, "378367732");//判断别名
            }
        }
        catch (NameNotFoundException e)
        {
            e.printStackTrace();
        }

        // SDK初始化，第三方程序启动时，都要进行SDK初始化工作
        Log.d("GetuiSdkDemo", "initializing sdk...");
        PackageManager pkgManager = getPackageManager();
        // 读写 sd card 权限非常重要, android6.0默认禁止的, 建议初始化之前就弹窗让用户赋予该权限
        boolean sdCardWritePermission = pkgManager.checkPermission(
                android.Manifest.permission.WRITE_EXTERNAL_STORAGE,
                getPackageName()) == PackageManager.PERMISSION_GRANTED;

        // read phone state用于获取 imei 设备信息
        boolean phoneSatePermission = pkgManager.checkPermission(
                android.Manifest.permission.READ_PHONE_STATE, getPackageName()) == PackageManager.PERMISSION_GRANTED;

        if (Build.VERSION.SDK_INT >= 23 && !sdCardWritePermission
                || !phoneSatePermission)
        {
            requestPermission();

        }
        else
        {
            // SDK初始化，第三方程序启动时，都要进行SDK初始化工作
            PushManager.getInstance().initialize(this.getApplicationContext());
        }

        /**
         * 应用未启动, 个推 service已经被唤醒,显示该时间段内离线消息
         */
        if (PushDemoReceiver.payloadData != null)
        {
            // tLogView.append(PushDemoReceiver.payloadData);
        }

    }

    private void requestPermission()
    {
//        ActivityCompat.requestPermissions(this, new String[]{
//                        android.Manifest.permission.WRITE_EXTERNAL_STORAGE,
//                        android.Manifest.permission.READ_PHONE_STATE
//                },
//                REQUEST_PERMISSION);

    }

    public void onRequestPermissionsResult(
            int requestCode,
            String[] permissions, int[] grantResults
    )
    {
        if (requestCode == REQUEST_PERMISSION)
        {
            if ((
                    grantResults.length == 2
                            && grantResults[0] == PackageManager.PERMISSION_GRANTED && grantResults[1] == PackageManager.PERMISSION_GRANTED
            ))
            {
                PushManager.getInstance().initialize(
                        this.getApplicationContext());
            }
            else
            {
                Log.e("GetuiSdkDemo",
                        "we highly recommend that you need to grant the special permissions before initializing the SDK, otherwise some "
                                + "functions will not work");
                PushManager.getInstance().initialize(
                        this.getApplicationContext());
            }
        }
        else
        {
            onRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    @Override
    public void onDestroy()
    {
        Log.d("GetuiSdkDemo", "onDestroy()");
        PushDemoReceiver.payloadData.delete(0,
                PushDemoReceiver.payloadData.length());
        super.onDestroy();
    }

    @Override
    public void onStop()
    {
        Log.d("GetuiSdkDemo", "onStop()");
        super.onStop();
    }


    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event)
    {
        if (keyCode == KeyEvent.KEYCODE_BACK)
        {
            // 返回键最小化程序
            Intent intent = new Intent(Intent.ACTION_MAIN);
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            intent.addCategory(Intent.CATEGORY_HOME);
            startActivity(intent);
            return true;
        }

        return super.onKeyDown(keyCode, event);
    }

    public boolean isNetworkConnected()
    {
        // 判断网络是否连接
        ConnectivityManager mConnectivityManager = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo mNetworkInfo = mConnectivityManager.getActiveNetworkInfo();
        return mNetworkInfo != null && mNetworkInfo.isAvailable();
    }


    /*
     * 初始化主页按钮
     */
    private void InistalView()
    {
        iv_personManger = (ImageView) findViewById(R.id.iv_personManger);
        ll_scanPay = (LinearLayout) findViewById(R.id.ll_scanPay);
        ll_parkorder = (LinearLayout) findViewById(R.id.ll_parkorder);
        ll_parkfind = (LinearLayout) findViewById(R.id.ll_parkfind);
        // 用户管理按钮
        iv_personManger.setOnClickListener(new OnClickListener()
        {
            @Override
            public void onClick(View arg0)
            {
                if (!Constant.isLogin)
                {// 是否已经登录 如果没有登录，跳转到登录微信按钮
                    Intent intent = new Intent(MainActivity.this,
                            WXEntryActivity.class);

                    startActivityForResult(intent, PERSONCENTERREQUESTCODE);

                }
                else
                {
                    Intent intent = new Intent(MainActivity.this,
                            PersonMangerActivity.class);
                    startActivity(intent);
                }
            }
        });

        // 扫码缴费按钮
        ll_scanPay.setOnClickListener(new OnClickListener()
        {
            @Override
            public void onClick(View arg0)
            {
                // 判断有没有登录

                if (!Constant.isLogin)
                {// 是否已经登录 如果没有登录，跳转到登录微信按钮
                    Intent intent = new Intent(MainActivity.this,
                            WXEntryActivity.class);

                    startActivityForResult(intent, SCANREQUESTCODE);
                }
                else
                {
                    Intent intent = new Intent(MainActivity.this,
                            CaptureActivity.class);
                    startActivity(intent);

                }

            }
        });

        // 停车记录按钮
        ll_parkorder.setOnClickListener(new OnClickListener()
        {
            @Override
            public void onClick(View arg0)
            {
                if (!Constant.isLogin)
                {// 是否已经登录 如果没有登录，跳转到登录微信按钮
                    Intent intent = new Intent(MainActivity.this,
                            WXEntryActivity.class);
                    startActivityForResult(intent, ParkListREQUESTCODE);
                }
                else
                {

                    Intent intent = new Intent(MainActivity.this,
                            ParkListActivity.class);
                    startActivity(intent);
                }
            }
        });

        // 停车导航
        ll_parkfind.setOnClickListener(new OnClickListener()
        {
            @Override
            public void onClick(View arg0)
            {
                Intent intent = new Intent(MainActivity.this,
                        ErrorActivity.class);
                startActivity(intent);
            }
        });

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data)
    {
        // TODO Auto-generated method stub
        super.onActivityResult(requestCode, resultCode, data);

        if (resultCode == 0)
        {

            if (requestCode == SCANREQUESTCODE)
            {
                if (Constant.isLogin)
                {
                    Intent intent = new Intent(MainActivity.this,
                            CaptureActivity.class);
                    startActivity(intent);
                }

            }
            else if (requestCode == ParkListREQUESTCODE)
            {
                if (Constant.isLogin)
                {
                    Intent intent = new Intent(MainActivity.this,
                            ParkListActivity.class);
                    startActivity(intent);
                }
            }
            else if (requestCode == PERSONCENTERREQUESTCODE)
            {
                if (Constant.isLogin)
                {
                    Intent intent = new Intent(MainActivity.this,
                            PersonMangerActivity.class);
                    startActivity(intent);
                }

            }
        }
    }

    @SuppressLint("NewApi")
    private void initialize()
    {

        cycleViewPager = (CycleViewPager) getFragmentManager()
                .findFragmentById(R.id.mViewpager);

        for (int i = 0; i < imageUrls.length; i++)
        {
            ADInfo info = new ADInfo();
            info.setUrl(imageUrls[i]);
            info.setContent("图片-->" + i);
            infos.add(info);
        }

        // 将最后一个ImageView添加进来
        views.add(ViewFactory.getImageView(this, infos.get(infos.size() - 1)
                .getUrl()));
        for (int i = 0; i < infos.size(); i++)
        {
            views.add(ViewFactory.getImageView(this, infos.get(i).getUrl()));
        }
        // 将第一个ImageView添加进来
        views.add(ViewFactory.getImageView(this, infos.get(0).getUrl()));

        // 设置循环，在调用setData方法前调用
        cycleViewPager.setCycle(true);

        // 在加载数据前设置是否循环
        cycleViewPager.setData(views, infos, mAdCycleViewListener);
        // 设置轮播
        cycleViewPager.setWheel(true);

        // 设置轮播时间，默认5000ms
        cycleViewPager.setTime(6000);
        // 设置圆点指示图标组居中显示，默认靠右
        cycleViewPager.setIndicatorCenter();
    }

    private CycleViewPager.ImageCycleViewListener mAdCycleViewListener = new CycleViewPager.ImageCycleViewListener()
    {

        @Override
        public void onImageClick(ADInfo info, int position, View imageView)
        {
            if (cycleViewPager.isCycle())
            {
                position = position - 1;
                Toast.makeText(MainActivity.this,
                        "position-->" + info.getContent(), Toast.LENGTH_SHORT)
                        .show();

                Intent intent = new Intent(MainActivity.this,
                        WebviewActivity.class); // 设置图片下载期间显示的图片
                startActivity(intent);
            }

        }

    };

    /**
     * 配置ImageLoder
     */
    private void configImageLoader()
    {
        // 初始化ImageLoader
        @SuppressWarnings("deprecation")
        DisplayImageOptions options = new DisplayImageOptions.Builder()
                .showStubImage(R.drawable.icon_stub) // 设置图片下载期间显示的图片
                .showImageForEmptyUri(R.drawable.icon_empty) // 设置图片Uri为空或是错误的时候显示的图片
                .showImageOnFail(R.drawable.icon_error) // 设置图片加载或解码过程中发生错误显示的图片
                .cacheInMemory(true) // 设置下载的图片是否缓存在内存中
                .cacheOnDisc(true) // 设置下载的图片是否缓存在SD卡中
                // .displayer(new RoundedBitmapDisplayer(20)) // 设置成圆角图片
                .build(); // 创建配置过得DisplayImageOption对象

        ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(
                getApplicationContext()).defaultDisplayImageOptions(options)
                .threadPriority(Thread.NORM_PRIORITY - 2)
                .denyCacheImageMultipleSizesInMemory()
                .discCacheFileNameGenerator(new Md5FileNameGenerator())
                .tasksProcessingOrder(QueueProcessingType.LIFO).build();
        ImageLoader.getInstance().init(config);
    }
}
