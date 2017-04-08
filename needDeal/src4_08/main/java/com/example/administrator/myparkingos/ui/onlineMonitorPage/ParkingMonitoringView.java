package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.graphics.Bitmap;
import android.graphics.Color;
import android.os.Handler;
import android.os.Message;
import android.view.GestureDetector;
import android.view.View;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.model.ModelNode;
import com.example.administrator.myparkingos.util.BitmapUtils;
import com.example.administrator.myparkingos.util.ConcurrentQueueHelper;
import com.example.administrator.myparkingos.util.L;
import com.example.administrator.myparkingos.util.T;
import com.vz.PlateResult;
import com.vz.monitor.player.MediaPlayer;
import com.vz.tcpsdk;

import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Administrator on 2017-03-08.
 */
public class ParkingMonitoringView implements tcpsdk.OnDataReceiver
{
    private ParkingMonitoringActivity monitoringActivity;
    private int mLayoutResourceId;
    private TextView tvChannel0;
    private MediaPlayer svVideo0;
    private Button btnCmdOPen0;
    private Button btnCmdClose0;
    private Button btnManual0;
    private TextView tvChannel2;
    private MediaPlayer svVideo2;
    private Button btnCmdOPen2;
    private Button btnCmdClose2;
    private Button btnManual2;
    private TextView tvChannel1;
    private MediaPlayer svVideo1;
    private Button btnCmdOPen1;
    private Button btnCmdClose1;
    private Button btnManual1;
    private TextView tvChannel3;
    private MediaPlayer svVideo3;
    private Button btnCmdOPen3;
    private Button btnCmdClose3;
    private Button btnManual3;
    private TextView btnChargeInfo;
    private TextView btnParkingSpace;
    private FrameLayout flchargeSpaceContainer;
    private LinearLayout llChargeInfo;
    private TextView tvCarInParkingDetail;
    private TextView tvCarChargeDetail;
    private FrameLayout flDetailList;
    private Button btnCarIn;
    private Button btnCarOut;
    private Button btnNoPlateCarIn;
    private Button btnNoPlateCarOut;
    private Button btnCarRegister;
    private Button btnBlackCarList;
    private Button btnDeadlineQuery;
    private Button btnShiftLogin;
    private TextView tvLoginName;
    private TextView etLoginNo;
    private TextView etWorkTime;
    private TextView etSysTime;
    private TextView etOperHint;

    private int[] handleList = new int[4];
    private tcpsdk.OnDataReceiver m_plateReciver;
    private int m_bEnableImg;
    private Button btnRefresh;
    private Button btnGroundVehicle;
    private Button btnChargeRecord;
    private TextView tvNoVideoText0;
    private TextView tvNoVideoText1;
    private TextView tvNoVideoText2;
    private TextView tvNoVideoText3;
    private TextView tvCarPlateText0;
    private TextView tvCarPlateText1;
    private TextView tvCarPlateText2;
    private TextView tvCarPlateText3;
    private ImageView ivPicture2; // 用于下面显示图片数据
    private ImageView ivPicture3;

    private Handler mHandler;
    private RelativeLayout rlContainerHint;
    private TextView tvResponeTxt;
    private RelativeLayout rlVideo0;
    private RelativeLayout rlVideo1;
    private RelativeLayout rlVideo2;
    private RelativeLayout rlVideo3;
    private LinearLayout llColumn1;
    private LinearLayout llColumn0;
    private LinearLayout llColumn2;
    private LinearLayout llColumn3;
    private LinearLayout llBtnContainer0;
    private LinearLayout llBtnContainer1;
    private LinearLayout llBtnContainer2;
    private LinearLayout llBtnContainer3;
    private LinearLayout llStatusBar;

    private List<LinearLayout> mBtnContainterList = new ArrayList<LinearLayout>();
    private View space0;
    private View space1;
    private List<View> spaceList = new ArrayList<View>();

    public ParkingMonitoringView()
    {

    }

    public ParkingMonitoringView(ParkingMonitoringActivity activity, int layoutId, Handler handler)
    {
        this.mHandler = handler;
        monitoringActivity = activity;
        mLayoutResourceId = layoutId;
        monitoringActivity.setContentView(layoutId);
        initView();
    }

    private List<View> allList = new ArrayList<View>();
    /**
     * 通道List
     */
    private List<TextView> channelList = new ArrayList<TextView>();

    /**
     * 没有视频显示时，打印"NoVideo"提示信息
     */
    private List<TextView> noVideoList = new ArrayList<TextView>();

    /**
     * 视频播放list
     */
    private List<MediaPlayer> videoList = new ArrayList<MediaPlayer>();
    /**
     * 开闸按钮list
     */
    private List<Button> openList = new ArrayList<Button>();
    /**
     * 关闸按钮list
     */
    private List<Button> closeList = new ArrayList<Button>();
    /**
     * 手动开闸列表
     */
    private List<Button> manualList = new ArrayList<Button>();

    /**
     * 车牌显示列表
     */
    private List<TextView> cphList = new ArrayList<TextView>();

    /**
     * 视频播放relativeLayout容器
     */
    private List<RelativeLayout> rlVideoList = new ArrayList<RelativeLayout>();

    /**
     * 存放包含有视频的列容器
     */
    private List<LinearLayout> llVideoCol = new ArrayList<LinearLayout>();

    private int mRow = 2; // 显示的2行视频
    private int mColumn = 2;// 显示2列视屏


    public void initView()
    {
        tvChannel0 = (TextView) monitoringActivity.findViewById(R.id.tvChannel0);
        svVideo0 = (MediaPlayer) monitoringActivity.findViewById(R.id.svVideo0);
        btnCmdOPen0 = (Button) monitoringActivity.findViewById(R.id.btnCmdOPen0);
        btnCmdClose0 = (Button) monitoringActivity.findViewById(R.id.btnCmdClose0);
        btnManual0 = (Button) monitoringActivity.findViewById(R.id.btnManual0);
        tvChannel2 = (TextView) monitoringActivity.findViewById(R.id.tvChannel2);
        svVideo2 = (MediaPlayer) monitoringActivity.findViewById(R.id.svVideo2);
        btnCmdOPen2 = (Button) monitoringActivity.findViewById(R.id.btnCmdOPen2);
        btnCmdClose2 = (Button) monitoringActivity.findViewById(R.id.btnCmdClose2);
        btnManual2 = (Button) monitoringActivity.findViewById(R.id.btnManual2);
        tvChannel1 = (TextView) monitoringActivity.findViewById(R.id.tvChannel1);
        svVideo1 = (MediaPlayer) monitoringActivity.findViewById(R.id.svVideo1);
        btnCmdOPen1 = (Button) monitoringActivity.findViewById(R.id.btnCmdOPen1);
        btnCmdClose1 = (Button) monitoringActivity.findViewById(R.id.btnCmdClose1);
        btnManual1 = (Button) monitoringActivity.findViewById(R.id.btnManual1);
        tvChannel3 = (TextView) monitoringActivity.findViewById(R.id.tvChannel3);
        svVideo3 = (MediaPlayer) monitoringActivity.findViewById(R.id.svVideo3);
        btnCmdOPen3 = (Button) monitoringActivity.findViewById(R.id.btnCmdOPen3);
        btnCmdClose3 = (Button) monitoringActivity.findViewById(R.id.btnCmdClose3);
        btnManual3 = (Button) monitoringActivity.findViewById(R.id.btnManual3);
        btnChargeInfo = (TextView) monitoringActivity.findViewById(R.id.btnChargeInfo);

        tvNoVideoText0 = (TextView) monitoringActivity.findViewById(R.id.tvNoVideoHint0);
        tvNoVideoText1 = (TextView) monitoringActivity.findViewById(R.id.tvNoVideoHint1);
        tvNoVideoText2 = (TextView) monitoringActivity.findViewById(R.id.tvNoVideoHint2);
        tvNoVideoText3 = (TextView) monitoringActivity.findViewById(R.id.tvNoVideoHint3);

        rlContainerHint = (RelativeLayout) monitoringActivity.findViewById(R.id.rlContainerHint);

        tvResponeTxt = (TextView) monitoringActivity.findViewById(R.id.tvResponeTxt);

        btnChargeInfo.setOnClickListener(myClickListener);

        btnGroundVehicle = (Button) monitoringActivity.findViewById(R.id.btnGroundVehicle);
        btnGroundVehicle.setOnClickListener(myClickListener);

        btnChargeRecord = (Button) monitoringActivity.findViewById(R.id.btnChargeRecord);
        btnChargeRecord.setOnClickListener(myClickListener);

        btnParkingSpace = (TextView) monitoringActivity.findViewById(R.id.btnParkingSpace);
        btnParkingSpace.setOnClickListener(myClickListener);

        flchargeSpaceContainer = (FrameLayout) monitoringActivity.findViewById(R.id.flchargeSpaceContainer);
        llChargeInfo = (LinearLayout) monitoringActivity.findViewById(R.id.llChargeInfo);

        tvCarInParkingDetail = (TextView) monitoringActivity.findViewById(R.id.tvCarInParkingDetail);
        tvCarInParkingDetail.setOnClickListener(myClickListener);
        tvCarChargeDetail = (TextView) monitoringActivity.findViewById(R.id.tvCarChargeDetail);
        tvCarChargeDetail.setOnClickListener(myClickListener);

        flDetailList = (FrameLayout) monitoringActivity.findViewById(R.id.flDetailList);
        btnCarIn = (Button) monitoringActivity.findViewById(R.id.btnCarIn);
        btnCarOut = (Button) monitoringActivity.findViewById(R.id.btnCarOut);
        btnRefresh = (Button) monitoringActivity.findViewById(R.id.btnRefresh);

        btnNoPlateCarIn = (Button) monitoringActivity.findViewById(R.id.btnNoPlateCarIn);
        btnNoPlateCarOut = (Button) monitoringActivity.findViewById(R.id.btnNoPlateCarOut);
        btnCarRegister = (Button) monitoringActivity.findViewById(R.id.btnCarRegister);
        btnBlackCarList = (Button) monitoringActivity.findViewById(R.id.btnBlackCarList);
        btnDeadlineQuery = (Button) monitoringActivity.findViewById(R.id.btnDeadlineQuery);
        btnShiftLogin = (Button) monitoringActivity.findViewById(R.id.btnShiftLogin);
        tvLoginName = (TextView) monitoringActivity.findViewById(R.id.tvLoginName);
        etLoginNo = (TextView) monitoringActivity.findViewById(R.id.etLoginNo);
        etWorkTime = (TextView) monitoringActivity.findViewById(R.id.etWorkTime);
        etSysTime = (TextView) monitoringActivity.findViewById(R.id.etSysTime);
        etOperHint = (TextView) monitoringActivity.findViewById(R.id.etOperHint);

        tvCarPlateText0 = (TextView) monitoringActivity.findViewById(R.id.tvCarPlateText0);
        tvCarPlateText1 = (TextView) monitoringActivity.findViewById(R.id.tvCarPlateText1);
        tvCarPlateText2 = (TextView) monitoringActivity.findViewById(R.id.tvCarPlateText2);
        tvCarPlateText3 = (TextView) monitoringActivity.findViewById(R.id.tvCarPlateText3);

        ivPicture2 = (ImageView) monitoringActivity.findViewById(R.id.ivPicture2);
        ivPicture3 = (ImageView) monitoringActivity.findViewById(R.id.ivPicture3);

        rlVideo0 = (RelativeLayout) monitoringActivity.findViewById(R.id.rlVideo0);
        registerDoubleClickListener(rlVideo0, mediaDoubleClickListener, 0);
        rlVideo1 = (RelativeLayout) monitoringActivity.findViewById(R.id.rlVideo1);
        registerDoubleClickListener(rlVideo1, mediaDoubleClickListener, 1);
        rlVideo2 = (RelativeLayout) monitoringActivity.findViewById(R.id.rlVideo2);
        registerDoubleClickListener(rlVideo2, mediaDoubleClickListener, 2);
        rlVideo3 = (RelativeLayout) monitoringActivity.findViewById(R.id.rlVideo3);
        registerDoubleClickListener(rlVideo3, mediaDoubleClickListener, 3);

        // 方便进行放大和缩写，设置各个空间隐藏和显示
        llColumn0 = (LinearLayout) monitoringActivity.findViewById(R.id.llColumn0);
        llColumn1 = (LinearLayout) monitoringActivity.findViewById(R.id.llColumn1);
        llVideoCol.add(llColumn0);
        llVideoCol.add(llColumn1);

        llColumn2 = (LinearLayout) monitoringActivity.findViewById(R.id.llColumn2);
        llColumn3 = (LinearLayout) monitoringActivity.findViewById(R.id.llColumn3);

        llBtnContainer0 = (LinearLayout) monitoringActivity.findViewById(R.id.llBtnContainer0);
        llBtnContainer1 = (LinearLayout) monitoringActivity.findViewById(R.id.llBtnContainer1);
        llBtnContainer2 = (LinearLayout) monitoringActivity.findViewById(R.id.llBtnContainer2);
        llBtnContainer3 = (LinearLayout) monitoringActivity.findViewById(R.id.llBtnContainer3);
        mBtnContainterList.add(llBtnContainer0);
        mBtnContainterList.add(llBtnContainer1);
        mBtnContainterList.add(llBtnContainer2);
        mBtnContainterList.add(llBtnContainer3);

        llStatusBar = (LinearLayout) monitoringActivity.findViewById(R.id.llStatusBar);
        space0 = monitoringActivity.findViewById(R.id.space0);
        space1 = monitoringActivity.findViewById(R.id.space1);
        spaceList.add(space0);
        spaceList.add(space1);

        btnCmdOPen0.setOnClickListener(myClickListener);
        btnCmdClose0.setOnClickListener(myClickListener);
        btnManual0.setOnClickListener(myClickListener);
        btnCmdOPen2.setOnClickListener(myClickListener);
        btnCmdClose2.setOnClickListener(myClickListener);
        btnManual2.setOnClickListener(myClickListener);
        btnCmdOPen1.setOnClickListener(myClickListener);
        btnCmdClose1.setOnClickListener(myClickListener);
        btnManual1.setOnClickListener(myClickListener);
        btnCmdOPen3.setOnClickListener(myClickListener);
        btnCmdClose3.setOnClickListener(myClickListener);
        btnManual3.setOnClickListener(myClickListener);
        btnCarIn.setOnClickListener(myClickListener);
        btnCarOut.setOnClickListener(myClickListener);
        btnNoPlateCarIn.setOnClickListener(myClickListener);
        btnNoPlateCarOut.setOnClickListener(myClickListener);
        btnCarRegister.setOnClickListener(myClickListener);
        btnBlackCarList.setOnClickListener(myClickListener);
        btnDeadlineQuery.setOnClickListener(myClickListener);
        btnShiftLogin.setOnClickListener(myClickListener);
        btnRefresh.setOnClickListener(myClickListener);

        channelList.add(tvChannel0);
        channelList.add(tvChannel1);
        channelList.add(tvChannel2);
        channelList.add(tvChannel3);

        // 显示的视频通道
        videoList.add(svVideo0);
        videoList.add(svVideo1);
        videoList.add(svVideo2);
        videoList.add(svVideo3);

        openList.add(btnCmdOPen0);
        openList.add(btnCmdOPen1);
        openList.add(btnCmdOPen2);
        openList.add(btnCmdOPen3);

        closeList.add(btnCmdClose0);
        closeList.add(btnCmdClose1);
        closeList.add(btnCmdClose2);
        closeList.add(btnCmdClose3);

        manualList.add(btnManual0);
        manualList.add(btnManual1);
        manualList.add(btnManual2);
        manualList.add(btnManual3);

        // 显示的video的提示信息
        noVideoList.add(tvNoVideoText0);
        noVideoList.add(tvNoVideoText1);
        noVideoList.add(tvNoVideoText2);
        noVideoList.add(tvNoVideoText3);

        // 显示的车牌号
        cphList.add(tvCarPlateText0);
        cphList.add(tvCarPlateText1);
        cphList.add(tvCarPlateText2);
        cphList.add(tvCarPlateText3);

        // 当点击后，放大的情况
        rlVideoList.add(rlVideo0);
        rlVideoList.add(rlVideo1);
        rlVideoList.add(rlVideo2);
        rlVideoList.add(rlVideo3);

        initViewData();
    }

    /**
     * 清空界面数据
     */
    private void initViewData()
    {
        /**
         * 清空channel的信息
         */
        for (TextView o : channelList)
        {
            o.setText("");
        }

        /**
         * 清空车牌号
         */
        for (TextView o : cphList)
        {
            o.setText("");
        }
    }

    private View.OnClickListener myClickListener = new View.OnClickListener()
    {
        @Override
        public void onClick(View v)
        {
            checkView(v);
        }
    };

    private boolean[] clickForVisible = {true, true, true, true}; // 刚开始时是可以显示的

    public void checkView(View v)
    {
        switch (v.getId())
        {
            case R.id.rlVideo0:
//                showVisibleAndHidden(0);
                break;
            case R.id.rlVideo1:
//                showVisibleAndHidden(1);
                break;
            case R.id.rlVideo2:
//                showVisibleAndHidden(2);
                break;
            case R.id.rlVideo3:
//                showVisibleAndHidden(3);
                break;
            case R.id.btnCmdOPen0:
                showInChargeFragment("测试数据");
                break;
            case R.id.btnCmdClose0:
                resumeInChargeFragment();
                break;
            case R.id.btnManual0:

                break;
            case R.id.btnCmdOPen2:

                break;
            case R.id.btnCmdClose2:

                break;
            case R.id.btnManual2:

                break;
            case R.id.btnCmdOPen1:
                break;
            case R.id.btnCmdClose1:

                break;
            case R.id.btnManual1:

                break;
            case R.id.btnCmdOPen3:

                break;
            case R.id.btnCmdClose3:

                break;
            case R.id.btnManual3:

                break;
            case R.id.btnCarIn:
                // 弹出对话框，然后进行相应的输入
                onClickCarInBtn();
                break;
            case R.id.btnCarOut:
                onClickCarOutBtn();
                break;
            case R.id.btnNoPlateCarIn:
                onClickNoPlateCarInBtn();
                break;
            case R.id.btnNoPlateCarOut:
                onClickNoPlateCarOutBtn();
                break;
            case R.id.btnCarRegister:
                onClickCarRegisterBtn();
                break;
            case R.id.btnBlackCarList:
                onClickBlackListBtn();
                break;
            case R.id.btnDeadlineQuery:
                onClickDealLineQueryBtn();
                break;
            case R.id.btnChargeRecord:
                onClickChargeRecordBtn();
                break;
            case R.id.btnGroundVehicle:
                onClickGroundVehicleBtn();
                break;
            case R.id.btnShiftLogin:
                onClickShiftLoginBtn();
                break;
            case R.id.btnChargeInfo: //
                onClickInCarChargeInfo();
                chargeInfoToFragmentChange();
                break;
            case R.id.btnParkingSpace:
                onClickInParkingSpace();
                carSpaceInfoToFragmentChange();
                break;
            case R.id.tvCarInParkingDetail:
            {
                onClickInCarInParkingDetail();
                carInParkingDetailToFragmentChange();
                break;
            }
            case R.id.tvCarChargeDetail:
            {
                onClickInCarChargeDetail();
                chargeDetailToFragmentChange();
                break;
            }
            case R.id.btnRefresh:
            {
                onClickRefreshDetail();
            }
            default:
            {
                break;
            }
        }
    }

    private void showVisibleAndHidden(int index)
    {
        if (clickForVisible[index])
        {
            showVideoViewByIndex(index, View.GONE);
            clickForVisible[index] = false;
        }
        else
        {
            showVideoViewByIndex(index, View.VISIBLE);
            clickForVisible[index] = true;
        }
    }

    private void showOtherColumn(int col, int type)
    {
        llColumn2.setVisibility(type);
        llColumn3.setVisibility(type);
        llStatusBar.setVisibility(type);

        llVideoCol.get(col).setVisibility(type); // 当前是第1列，那么隐藏的是第0列

//        // surface显示和textview不一样的，需要单独来隐藏
//        for (int i = 0; i < mRow; i++) //行数
//        {
//            videoList.get(i * mRow + col).setVisibility(type);
//        }
    }

    private void showOtherVideoList(int index, int type)
    {
        for (int i = 0; i < mRow * mColumn; i++)
        {
            if (index != i)
            {
                videoList.get(i).setVisibility(type);
            }
        }
    }

    private void showInnterView(int row, int col, int type) // 1 1
    {
        // 1, 先隐藏本身的 channel和 btnContainer
        channelList.get(row * mRow + col).setVisibility(type);
        mBtnContainterList.get(row * mRow + col).setVisibility(type);

        // 2, 隐藏相对的那个列，行数不同，列数相同;
        int otherRow = mRow - 1 - row;
        channelList.get(otherRow * mRow + col).setVisibility(type);
        mBtnContainterList.get(otherRow * mRow + col).setVisibility(type);

        // 3, rlVideoList
        rlVideoList.get(otherRow * mRow + col).setVisibility(type);

        // surface显示和textview不一样的，需要单独来设置
        showOtherVideoList(row * mRow + col, type);
    }

    // 表示第0行, 第0列
    private void showVideoViewByIndex(int index, int type) // 1
    {
        int row = index / 2; // 1 0
        int col = index % 2; // 1 1

        showOtherColumn(mColumn - 1 - col, type);// 当前是第1列，那么隐藏的是 1 - 1 = 0列 那么久
        showInnterView(row, col, type);

        for (View v : spaceList)
            v.setVisibility(type);
    }


    public void onClickGroundVehicleBtn()
    {

    }

    public void onClickChargeRecordBtn()
    {

    }

    public void onClickShiftLoginBtn()
    {
    }

    public void onClickDealLineQueryBtn()
    {

    }

    public void onClickBlackListBtn()
    {

    }

    public void onClickRefreshDetail()
    {

    }

    public void onClickCarRegisterBtn()
    {
    }

    public void onClickNoPlateCarOutBtn()
    {

    }

    public void onClickNoPlateCarInBtn()
    {

    }

    public void onClickCarOutBtn()
    {

    }

    public void onClickCarInBtn()
    {

    }

    /**
     * 点击车辆收费信息
     */
    public void onClickInCarChargeInfo()
    {
        btnParkingSpace.setBackgroundResource(R.color.colorClick);
        btnChargeInfo.setBackgroundResource(R.color.colorNoClick);
    }

    /**
     * 点击车位信息
     */
    public void onClickInParkingSpace()
    {
        btnChargeInfo.setBackgroundResource(R.color.colorClick);
        btnParkingSpace.setBackgroundResource(R.color.colorNoClick);
    }

    /**
     * 点击在场车辆信息
     */
    public void onClickInCarInParkingDetail()
    {
        tvCarChargeDetail.setBackgroundResource(R.color.colorClick);
        tvCarInParkingDetail.setBackgroundResource(R.color.colorNoClick);
    }

    /**
     * 点击车场收费
     */
    public void onClickInCarChargeDetail()
    {
        tvCarInParkingDetail.setBackgroundResource(R.color.colorClick);
        tvCarChargeDetail.setBackgroundResource(R.color.colorNoClick);
    }

    public void chargeInfoToFragmentChange()
    {

    }

    public void carSpaceInfoToFragmentChange()
    {

    }

    public void carInParkingDetailToFragmentChange()
    {

    }

    public void chargeDetailToFragmentChange()
    {

    }

    /**
     * 播放视频
     */
    private void startPlayVideo(MediaPlayer mediaPlayer, String cameraIp, Handler inHandler)
    {
        mediaPlayer.setUrlip(cameraIp);

        L.i("startPlayVideo, " + cameraIp);
        mediaPlayer.setHandler(inHandler);
        mediaPlayer.startPlay();
    }

    private void stopPlayVideo(MediaPlayer mediaPlayer)
    {
        mediaPlayer.stopPlay();
    }

    /**
     * 显示状态栏数据
     */
    public void showStatusBar(String userName, String loginNo, String workTime, String sysTime)
    {
        tvLoginName.setText(userName);
        etLoginNo.setText(loginNo);
        etWorkTime.setText(workTime);
        etSysTime.setText(sysTime);
    }

    private int TCPSOCK_PORT = 8131;

    public void playVideoByIndex(int index, String ip, Handler handler)
    {
        startPlayVideo(videoList.get(index), ip, handler);
        //        device_info.DeviceName:设备1
//        03-09 10:54:00.475 19437-19437/com.example.vzvision I/OPEN: device_info.ip:192.168.2.248
//        03-09 10:54:00.475 19437-19437/com.example.vzvision I/OPEN: device_info.port:8131
//        03-09 10:54:00.475 19437-19437/com.example.vzvision I/OPEN: device_info.username:admin
//        03-09 10:54:00.475 19437-19437/com.example.vzvision I/OPEN: device_info.userpassword:admin

        // 对于tcpsdk进行初始化
        String userName = "admin";
        String userPassword = "admin";
        handleList[index] = tcpsdk.getInstance().open(ip.getBytes(), ip.length(), TCPSOCK_PORT, userName.getBytes(),
                userName.length(), userPassword.getBytes(), userPassword.length());
        if (handleList[index] != 0)
        {
            tcpsdk.getInstance().setPlateInfoCallBack(handleList[index], this, 1);
        }
    }

    public void stopVideoByIndex(int index)
    {
        L.i("stopVideoByIndex index:" + index);
        stopPlayVideo(videoList.get(index));
        if (handleList[index] != 0)
            tcpsdk.getInstance().close(handleList[index]);
    }

    @Override
    public void onDataReceive(int handle, PlateResult plateResult, int uNumPlates, int eResultType, byte[] pImgFull, int nFullSize, byte[] pImgPlateClip, int nClipSize)
    {
        try
        {
            int channel = -1;
            for (int i = 0; i < handleList.length; i++)
            {
                L.i("handle:" + handle + ", handeList[i]" + handleList[i] + ",i:" + i);
                if (handleList[i] == handle)
                {
                    channel = i; // 第几个通道
                    break;
                }
            }

            String plateText = new String(plateResult.license, "GBK");
            ModelNode modelNode = new ModelNode();

            modelNode.type = ModelNode.E_CarInOutType.CAR_INOUT_TYPE_recognition;

            modelNode.setsDzScan("");
            modelNode.setiDzIndex(channel);
            modelNode.setStrCPH(plateText);
            modelNode.picture = BitmapUtils.byteToBitmap(pImgFull, 2);

            //把图片放到文件后再放到传递获取；
            // 直接把byte转换Bitmap图像数据；
            ConcurrentQueueHelper.getInstance().put(modelNode);
            L.i("onDataReceive ======================================" + Thread.currentThread().getName() + "," + plateText);

        }
        catch (UnsupportedEncodingException e)
        {
            e.printStackTrace();
            Toast.makeText(monitoringActivity, "不支持的解码异常", Toast.LENGTH_SHORT).show();
        }
    }

    /**
     * 设置当视频播放时，surface 使能， textView 提示信息 disable
     *
     * @param index
     */
    public void setSurfaceEnableWhenPlayVideo(int index)
    {
        if (index >= 0 && index < videoList.size())
        {
            MediaPlayer mediaPlayer = videoList.get(index);
            TextView textView = noVideoList.get(index);
            if (mediaPlayer.getVisibility() != View.VISIBLE)
            {
                mediaPlayer.setVisibility(View.VISIBLE);
                textView.setVisibility(View.INVISIBLE);
            }
            else
            {
                textView.setVisibility(View.INVISIBLE);
            }
        }
        else
        {
            T.show(monitoringActivity, "setSurfaceEnableWhenPlayVideo,索引错误", index);
        }
    }

    /**
     * 设置视频停止播放时，surface disable; textView 提示信息 enable
     *
     * @param index
     */
    public void setTextEnableWhenStopVideo(int index)
    {
        if (index >= 0 && index < videoList.size())
        {
            MediaPlayer mediaPlayer = videoList.get(index);
            TextView textView = noVideoList.get(index);
            if (mediaPlayer.getVisibility() != View.VISIBLE)
            {
                textView.setVisibility(View.VISIBLE);
                mediaPlayer.setVisibility(View.INVISIBLE);
            }
            else
            {
                textView.setVisibility(View.VISIBLE);
                mediaPlayer.setVisibility(View.INVISIBLE);
            }
        }
        else
        {
            T.show(monitoringActivity, "setSurfaceEnableWhenPlayVideo,索引错误", index);
        }
    }

    public void setChannelText(int index, String text)
    {
        if (index >= 0 && index < channelList.size())
        {
            TextView textView = channelList.get(index);
            textView.setText(text);
        }
    }

    public void setCPHText(int index, String text)
    {
        if (index >= 0 && index < cphList.size())
        {
            cphList.get(index).setText(text);
        }
    }

    public void setInPicture(int index, Bitmap b)
    {
        if (index >= 0 && index < videoList.size())
        {
            videoList.get(index).setVisibility(View.VISIBLE);
            noVideoList.get(index).setVisibility(View.INVISIBLE);
            ivPicture2.setVisibility(View.VISIBLE);
            ivPicture2.setImageBitmap(b);
        }
    }

    public void setOutPicture(int index, Bitmap b)
    {
        if (index >= 0 && index < cphList.size())
        {
            videoList.get(index).setVisibility(View.VISIBLE);
            noVideoList.get(index).setVisibility(View.INVISIBLE);
            ivPicture3.setVisibility(View.VISIBLE);
            ivPicture3.setImageBitmap(b);
        }
    }

    public void setInCarPictureChannelText(String str)
    {
        tvChannel2.setText(str);
    }

    public void setOutCarPictureChannelText(String str)
    {
        tvChannel3.setText(str);
    }


    public Bitmap getCurrentSnapture(int index) // 从指定mediaPlay来获取一帧相应的数据即可
    {
        return null;
    }

    public boolean saveImage(String fileName, int index)
    {
        if (index >= 0 && index < videoList.size())
        {
            MediaPlayer mediaPlayer = videoList.get(index);
            return mediaPlayer.snapshot(fileName); //如果没有播放视频的情况下,会出现什么情况呢?
        }

        return false;
    }

    // 全屏的显示提示信息 llChargeInfo存放的是Fragment数据; 将Fragment隐藏，然后显示数据即可

    public void showInChargeFragment(String text)
    {
        rlContainerHint.setBackgroundColor(Color.GRAY);
        if (tvResponeTxt.getVisibility() != View.VISIBLE)
        {
            tvResponeTxt.setVisibility(View.VISIBLE);
        }
        tvResponeTxt.setText(text);

        if (llChargeInfo.getVisibility() == View.VISIBLE)
        {
            llChargeInfo.setVisibility(View.INVISIBLE);
        }
    }

    public void resumeInChargeFragment()
    {
        rlContainerHint.setBackgroundColor(Color.TRANSPARENT);
        if (tvResponeTxt.getVisibility() == View.VISIBLE)
        {
            tvResponeTxt.setVisibility(View.INVISIBLE);
        }

        if (llChargeInfo.getVisibility() != View.VISIBLE)
        {
            llChargeInfo.setVisibility(View.VISIBLE);
        }
    }

    /**
     * 自己点击的双击操作 350ms操作
     */
    public interface OnDoubleClickListener
    {
        public void OnSingleClick(View v, int index);

        public void OnDoubleClick(View v, int index);
    }

    private OnDoubleClickListener mediaDoubleClickListener = new OnDoubleClickListener()
    {
        @Override
        public void OnSingleClick(View v, int index)
        {

        }

        @Override
        public void OnDoubleClick(View v, int index)
        {
            showVisibleAndHidden(index);
        }
    };

    // 这里会出现内存泄漏吗? 还是需要测试;
    private static void registerDoubleClickListener(View view, final OnDoubleClickListener listener, final int index)
    {
        if (listener == null)
        {
            return;
        }

        view.setOnClickListener(new View.OnClickListener()
        {
            private int mIndex = index;
            private final int DOUBLE_CLICK_TIME = 350;        //双击间隔时间350毫秒
            private boolean waitDouble = true;// 点击
            private Handler handler = new Handler()
            {
                @Override
                public void handleMessage(Message msg)
                {
                    listener.OnSingleClick((View) msg.obj, mIndex);
                }
            };

            @Override
            public void onClick(final View v)
            {
                if (waitDouble)
                {
                    waitDouble = false;        //与执行双击事件
                    new Thread()
                    {

                        public void run()
                        {
                            try
                            {
                                Thread.sleep(DOUBLE_CLICK_TIME);
                            }
                            catch (InterruptedException e)
                            {
                                e.printStackTrace();
                            }    //等待双击时间，否则执行单击事件
                            if (!waitDouble)
                            {
                                //如果过了等待事件还是预执行双击状态，则视为单击
                                waitDouble = true;
                                Message msg = handler.obtainMessage();
                                msg.obj = v;
                                handler.sendMessage(msg);
                            }
                        }

                    }.start();
                }
                else
                {
                    waitDouble = true;
                    listener.OnDoubleClick(v, mIndex);    //执行双击
                }
            }
        });
    }

    /**
     * 启用网络视频
     */
    private boolean bEnableNetVideo = true;

    /**
     * 是否启用4路视频, 默认是启动两路;
     */
    private boolean bVideo4 = false; // true则是四路，false即为两路

    /**
     * 默认情况是启动网络视频和两路视频
     */

    /**
     * 4_06 视频显示的画面的逻辑：必定是先显示入场，然后显示出场；
     *                         入场的图片是在视频播放下面;
     *      存在的情况：
     *          前提：最多只有4路显示视频；且不是副摄像头拍摄；
     *          情况的处理情况如下：
     *              1，如果入场和出场的总数大于4，即只取前面四路；
     *              2，如果总通道是1，或入场或出场 且左上角和左下角分别显示视频和图片； 右侧的画面显示数据不存在；
     *              3，如果总通道是2，且一进一出，那么显示入场再显示出场；
     *              4，             且两个入场或出场，则分开显示，且下面显示图片；
     *              5，如果总通数是3，那么全部显示视频，不显示图片；且同样是先入场后出场的顺序
     *              6，如果总通道是4，那么也是先入口，后出口的方式显示；
     *         怎么逻辑封装?
     *              1,影响因子，总通道数，通道数 > 2,则不显示图片；
     *              2,优先显示入场视频；
     */

    public void setOperatorHintInfo(String text)
    {
        etOperHint.setText(text);
    }

}


