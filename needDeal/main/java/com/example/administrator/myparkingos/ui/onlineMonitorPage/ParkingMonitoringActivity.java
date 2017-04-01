package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.graphics.Bitmap;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.text.TextPaint;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.constant.ColumnName;
import com.example.administrator.myparkingos.constant.JsonSearchParam;
import com.example.administrator.myparkingos.constant.OrderField;
import com.example.administrator.myparkingos.constant.RCodeDeal;
import com.example.administrator.myparkingos.model.GetServiceData;
import com.example.administrator.myparkingos.model.ModelNode;
import com.example.administrator.myparkingos.model.MonitorRemoteRequest;
import com.example.administrator.myparkingos.model.RequestByURL;
import com.example.administrator.myparkingos.model.beans.BlackListOpt;
import com.example.administrator.myparkingos.model.beans.Model;
import com.example.administrator.myparkingos.model.beans.gson.EntityBlackList;
import com.example.administrator.myparkingos.model.beans.gson.EntityCarIn;
import com.example.administrator.myparkingos.model.beans.gson.EntityCarOut;
import com.example.administrator.myparkingos.model.beans.gson.EntityCarTypeInfo;
import com.example.administrator.myparkingos.model.beans.gson.EntityCardIssue;
import com.example.administrator.myparkingos.model.beans.gson.EntityMoney;
import com.example.administrator.myparkingos.model.beans.gson.EntityNetCameraSet;
import com.example.administrator.myparkingos.model.beans.gson.EntityParkJHSet;
import com.example.administrator.myparkingos.model.beans.gson.EntityParkingInfo;
import com.example.administrator.myparkingos.model.beans.gson.EntityPersonnelInfo;
import com.example.administrator.myparkingos.model.beans.gson.EntityRoadWaySet;
import com.example.administrator.myparkingos.model.beans.gson.EntitySetCarOut;
import com.example.administrator.myparkingos.model.beans.gson.EntityUserInfo;
import com.example.administrator.myparkingos.model.requestInfo.GetCardIssueReq;
import com.example.administrator.myparkingos.model.requestInfo.GetCheDaoSetReq;
import com.example.administrator.myparkingos.model.requestInfo.GetNetCameraSetReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInConfirmReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInWithoutCPHReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarOutReq;
import com.example.administrator.myparkingos.model.responseInfo.GetCardIssueResp;
import com.example.administrator.myparkingos.model.responseInfo.GetCheDaoSetResp;
import com.example.administrator.myparkingos.model.responseInfo.GetNetCameraSetResp;
import com.example.administrator.myparkingos.model.responseInfo.GetRightsByGroupIDResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInConfirmResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInWithoutCPHResp;
import com.example.administrator.myparkingos.ui.FragmentChargeManager;
import com.example.administrator.myparkingos.ui.FragmentDetailManager;
import com.example.administrator.myparkingos.ui.onlineMonitorPage.report.ReportDealLineView;
import com.example.administrator.myparkingos.util.BitmapUtils;
import com.example.administrator.myparkingos.util.CommUtils;
import com.example.administrator.myparkingos.util.ConcurrentQueueHelper;
import com.example.administrator.myparkingos.util.ExeUtil;
import com.example.administrator.myparkingos.util.L;
import com.example.administrator.myparkingos.util.SDCardUtils;
import com.example.administrator.myparkingos.util.T;
import com.example.administrator.myparkingos.util.TimeConvertUtils;
import com.vz.tcpsdk;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;

/**
 * Created by Administrator on 2017-02-16.
 * 【在线监控】主界面
 */
public class ParkingMonitoringActivity extends AppCompatActivity
{
    private FragmentChargeManager fragmentChargeManager = null;
    private FragmentDetailManager fragmentDetailManager = null;
    private ParkingMonitoringView parkingMonitoringView;
    private List<EntityRoadWaySet> entityRoadWaySets;
    private List<EntityCarIn> entityCarIns;
    private List<EntityCarOut> entityCarOuts;
    private EntityParkingInfo entityParkingInfo;
    private ParkingPlateNoInputView CarInDialog;
    private ParkingPlateNoInputView CarOutDialog;
    private ParkingInNoPlateView parkingInNoPlateView;
    private ParkingOutNOPlateNoView parkingOutNOPlateNoView;
    private ParkingPlateRegisterView parkingPlateRegisterView;
    private FormAddBlackListView formAddBlackListView;
    private ReportDealLineView reportDealLineView;
    private ParkingChangeView parkingChangeShifts;
    private QueueTask queueTask;
    private ParkingTempCPHView parkingTempCPHView;
    private ExeUtil exe;
    private GetCheDaoSetResp getCheDaoSetResp;


    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        parkingMonitoringView = new ParkingMonitoringView(this, R.layout.activity_parkingmonitor, mHandler)
        {
            @Override
            public void chargeInfoToFragmentChange()
            {
                fragmentChargeManager.showFragment(0);
                mHandler.sendEmptyMessage(MSG_ChargeInfo);
            }

            @Override
            public void carSpaceInfoToFragmentChange()
            {
                fragmentChargeManager.showFragment(1);
                mHandler.sendEmptyMessage(MSG_ParkingInfo);
            }

            @Override
            public void carInParkingDetailToFragmentChange()
            {
                fragmentDetailManager.showFragment(0);
                mHandler.sendEmptyMessage(MSG_CarIn);
            }

            @Override
            public void chargeDetailToFragmentChange()
            {
                fragmentDetailManager.showFragment(1);
                mHandler.sendEmptyMessage(MSG_CarOut);
            }

            @Override
            public void onClickCarInBtn()
            {
                /**
                 * 弹出车辆进场的画面
                 */
                if (CarInDialog == null)
                {
                    CarInDialog = new ParkingPlateNoInputView(ParkingMonitoringActivity.this, 0)
                    {
                        /**
                         * 提前加载数据
                         */
                        @Override
                        public void prepareLoadData()
                        {
                            super.prepareLoadData();
                            CarInDialog.setProvince(Model.LocalProvince);
                        }

                        @Override
                        protected void onCarInBtnOk(final String CPH)
                        {
                            /**
                             * 获取进场的数据返回，然后将数据放到界面上
                             */
                            SetCarInReq setCarInReq = new SetCarInReq(); // 发送进场数据
                            setCarInReq.setCPH(CPH);
                            setCarInReq.setToken(Model.token);
                            setCarInReq.setCtrlNumber(getCtrlNumber(CAR_CHANNEL_IN));
                            setCarInReq.setStationId(Model.stationID);

                            sendModeToQueue(ModelNode.E_CarInType.CAR_IN_TYPE_auto, CPH, setCarInReq, getChannelIndex(CAR_CHANNEL_IN));
                            CarInDialog.dismiss();
                        }

                        @Override
                        protected void onBtnCancel()
                        {
                            CarInDialog.dismiss();
                        }

                        /**
                         * 当text查询时，出现模糊查找
                         * @param Precision
                         */
                        @Override
                        public void onClickInTextInput(final String cph, final int Precision)
                        {
                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    GetCardIssueReq getCardIssueReq = new GetCardIssueReq(); // 请求发卡行信息
                                    getCardIssueReq.setToken(Model.token);
                                    getCardIssueReq.setJsonSearchParam(JsonSearchParam.getWhenGetCardIssue(cph.substring(1)));  // 过滤掉第一个省份简称
                                    getCardIssueReq.setOrderField(OrderField.getWhenGetCardIssue("asc"));
                                    final GetCardIssueResp respList = GetServiceData.getInstance().SelectFxCPH_Like(getCardIssueReq);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            ArrayList<String> strings = new ArrayList<>(); // 测试
                                            if (respList.getData() != null && respList.getData().size() > 0)
                                            {
                                                for (GetCardIssueResp.DataBean o : respList.getData())
                                                {
                                                    strings.add(o.getCPH()); // 过滤掉第一个省份简称
                                                }
                                                CarInDialog.showPopWindow();
                                                CarInDialog.setCompleteCPHText(strings);
                                            }
                                        }
                                    });
                                }
                            }).start();
                        }
                    };
                }

                CarInDialog.show();
            }

            /**
             * 弹出车辆出场的画面
             */
            @Override
            public void onClickCarOutBtn()
            {
                if (CarOutDialog == null)
                {
                    CarOutDialog = new ParkingPlateNoInputView(ParkingMonitoringActivity.this, 1)
                    {
                        @Override
                        public void onCarOutBtnOk(final String CPH) // 车辆出场，直接发送出场消息
                        {


                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    //        LinkedHashMap<String, String> tmpContainer = new LinkedHashMap<String, String>();
//        tmpContainer.put("token", token);
//        tmpContainer.put("CPH", CPH);
//        tmpContainer.put("CtrlNumber", ctrlNo);
//        tmpContainer.put("StationId", stationId);
//        EntitySetCarOut setCarOut = RequestByURL.getDataWithoutTokenField("SetCarOut", EntitySetCarOut.class, null, RequestByURL.mapConvertString(tmpContainer));
//        RequestByURL.displaySimpleInfo(setCarOut, "SetCarOut(String token, String CPH, String ctrlNo, String stationId)");
//        return setCarOut;
                                    SetCarOutReq setCarOutReq = new SetCarOutReq();
                                    setCarOutReq.setToken(Model.token);
                                    setCarOutReq.setCPH(CPH);
                                    setCarOutReq.setStationId(Model.stationID);
//                                    setCarOutReq.setCtrlNumber();


                                    GetServiceData.getInstance().SetCarOut(setCarOutReq);


                                    EntitySetCarOut entitySetCarOut = MonitorRemoteRequest.SetCarOut(Model.token, CPH, String.valueOf(entityRoadWaySets.get(1).getCtrlNumber()), String.valueOf(Model.stationID));// 1是出场
                                    Message message = mHandler.obtainMessage();
                                    if (entitySetCarOut != null)
                                    {
                                        message.obj = entitySetCarOut;
                                        message.what = MSG_SETCarOut;
                                        mHandler.sendMessage(message);
                                    }
                                }
                            }).start();

                            CarOutDialog.dismiss();
                        }

                        @Override
                        protected void onBtnCancel()
                        {
                            CarOutDialog.dismiss();
                        }
                    };
                }
                CarOutDialog.show();
            }

            /**
             * 弹出无牌车入场界面
             */
            @Override
            public void onClickNoPlateCarInBtn()
            {
                if (parkingInNoPlateView == null)
                {
                    parkingInNoPlateView = new ParkingInNoPlateView(ParkingMonitoringActivity.this)
                    {
                        @Override
                        protected void onBtnOk(final SetCarInWithoutCPHReq carInWithoutCPHReq, final String roadName)
                        {
                            carInWithoutCPHReq.setToken(Model.token);
                            carInWithoutCPHReq.setStationId(Model.stationID);

                            int ctrlNoByInOutName = getCtrlNoByInOutName(roadName);// 找机号
                            if (ctrlNoByInOutName < 0)
                            {
                            }
                            else
                                carInWithoutCPHReq.setCtrlNumber(ctrlNoByInOutName);

                            String CPH = carInWithoutCPHReq.getCPH();
                            sendModeToQueue(ModelNode.E_CarInType.CAR_IN_TYPE_auto_noPlate, CPH, carInWithoutCPHReq, getChannelIndex(CAR_CHANNEL_IN));
                            parkingInNoPlateView.dismiss();
                        }

                        @Override
                        protected void onBtnCancel()
                        {
                            parkingInNoPlateView.dismiss();
                        }

                        @Override
                        public void prepareLoadData()
                        {
                            if (entityRoadWaySets != null && entityRoadWaySets.size() > 0)
                            {
                                ArrayList<String> strings = new ArrayList<>();

                                List<String> roadNameByType = getRoadNameByType(CAR_CHANNEL_IN);
                                parkingInNoPlateView.setRoadNameData(roadNameByType); // 显示车牌名
                                String fileName = SDCardUtils.getSDCardPath() + "picture1.jpg";
                                L.i("获取到的fileName:" + fileName);
                                parkingMonitoringView.saveImage(fileName, getChannelIndex(CAR_CHANNEL_IN));
                                parkingInNoPlateView.setImage(fileName); //显示图像
                                parkingInNoPlateView.cleanCarNo();
                            }
                        }
                    };
                }
                parkingInNoPlateView.show();
            }

            /**
             * 弹出无牌车出场界面
             */
            @Override
            public void onClickNoPlateCarOutBtn()
            {
                if (parkingOutNOPlateNoView == null)
                {
                    parkingOutNOPlateNoView = new ParkingOutNOPlateNoView(ParkingMonitoringActivity.this);
                }
                parkingOutNOPlateNoView.show();
            }

            /**
             * 弹出车辆注册界面
             */
            @Override
            public void onClickCarRegisterBtn()
            {
                if (parkingPlateRegisterView == null) // TODO  parkingPlateRegisterView
                {
                    parkingPlateRegisterView = new ParkingPlateRegisterViewSub(ParkingMonitoringActivity.this);
                }
                parkingPlateRegisterView.show();
            }

            /**
             * 点击刷新重新获取数据
             */
            @Override
            public void onClickRefreshDetail()
            {
                requestParkingDetailFromNet();
            }

            /**
             * 点击黑名单按钮
             */
            @Override
            public void onClickBlackListBtn()
            {
                if (formAddBlackListView == null)
                {
                    formAddBlackListView = new FormAddBlackListView(ParkingMonitoringActivity.this)
                    {
                        @Override
                        public void prepareLoadData() //提前加载数据
                        {

                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    List<EntityBlackList> entityBlackLists = MonitorRemoteRequest.GetBlacklist(Model.token);
                                    Message message = mHandler.obtainMessage();
                                    message.obj = entityBlackLists;
                                    message.what = MSG_UpdateBlackListData;
                                    mHandler.sendMessage(message);
                                }
                            }).start();
                        }

                        @Override
                        public void onClickBlackListQuit()
                        {
                            formAddBlackListView.dismiss();
                        }

                        @Override
                        public void onClickBlackListDelEach(final List<EntityBlackList> param)//一个个的进行删除
                        {
                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    int returnValue = -1;
                                    for (int i = 0; i < param.size(); i++)
                                    {
                                        if (param.get(i).getDownloadSignal().equals("000000000000000"))
                                        {
                                            returnValue = MonitorRemoteRequest.DeleteBlacklistBy(Model.token, String.valueOf(param.get(i).getID()));
                                        }
                                        else
                                        {
                                            returnValue = MonitorRemoteRequest.UpdateBlacklist(Model.token, String.valueOf(param.get(i).getID()));
                                        }
                                        if (returnValue > 0)
                                        {
                                            MonitorRemoteRequest.AddOptLog(Model.token, "黑名单车辆", Model.sUserName + ":删除黑名单车辆：" + param.get(i).getCPH()
                                                    , Model.sUserCard, Model.sUserName, String.valueOf(Model.stationID));// 添加成功
                                        }
                                    }

                                    if (returnValue > 0)
                                    {
                                        List<EntityBlackList> entityBlackLists = MonitorRemoteRequest.GetBlacklist(Model.token);
                                        Message message = mHandler.obtainMessage();
                                        message.obj = entityBlackLists;
                                        message.what = MSG_UpdateBlackListData;
                                        mHandler.sendMessage(message);
                                    }
                                }
                            }).start();
                        }

                        @Override
                        public void onClickBlackListDelBtn(final EntityBlackList param)//删除
                        {
                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    int returnValue = -1;
                                    L.i("param.getDownloadSignal().equals:" + param.getDownloadSignal());
                                    if (param.getDownloadSignal().equals("000000000000000"))
                                    {
                                        returnValue = MonitorRemoteRequest.DeleteBlacklistBy(Model.token, String.valueOf(param.getID()));
                                    }
                                    else
                                    {
                                        returnValue = MonitorRemoteRequest.UpdateBlacklist(Model.token, String.valueOf(param.getID()));
                                    }

                                    if (returnValue > 0)
                                    {
                                        List<EntityBlackList> entityBlackLists = MonitorRemoteRequest.GetBlacklist(Model.token);
                                        MonitorRemoteRequest.AddOptLog(Model.token, "黑名单车辆", Model.sUserName + ":删除黑名单车辆：" + param.getCPH()
                                                , Model.sUserCard, Model.sUserName, String.valueOf(Model.stationID));// 添加成功

                                        Message message = mHandler.obtainMessage();
                                        message.obj = entityBlackLists;
                                        message.what = MSG_UpdateBlackListData;
                                        mHandler.sendMessage(message);
                                    }
                                    else
                                    {// 删除
                                        List<EntityBlackList> entityBlackLists = MonitorRemoteRequest.GetBlacklist(Model.token);
                                        Message message = mHandler.obtainMessage();
                                        message.obj = entityBlackLists;
                                        message.what = MSG_UpdateBlackListData;
                                        mHandler.sendMessage(message);
                                    }
                                }
                            }).start();
                        }

                        @Override
                        public void onClickBlackListQueryBtn()//查询
                        {
                            final String cphDataFromUI = formAddBlackListView.getCPHDataFromUI();
                            if (cphDataFromUI == null)
                            {
                                return;
                            }

                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    List<EntityBlackList> entityBlackLists = MonitorRemoteRequest.GetBlacklistWhenSelect(Model.token, cphDataFromUI);
                                    if (entityBlackLists != null)
                                    {
                                        Message message = mHandler.obtainMessage();
                                        message.obj = entityBlackLists;
                                        message.what = MSG_UpdateBlackListData;
                                        mHandler.sendMessage(message);
                                    }
                                }
                            }).start();
                        }

                        @Override
                        public void onClickBlackListAddBtn()//添加
                        {
                            final BlackListOpt listOpt = formAddBlackListView.getAllDataFromUI();
                            if (listOpt == null)
                            {
                                return;
                            }
                            L.i("onClickBlackListAddBtn:" + formAddBlackListView.getAllDataFromUI());

                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    MonitorRemoteRequest.DeleteBlacklist(Model.token, listOpt.getCPH());
                                    MonitorRemoteRequest.AddBlacklist(Model.token, listOpt);  //先删除后添加数据

                                    /*
                                    更新界面数据
                                    * */
                                    List<EntityBlackList> entityBlackLists = MonitorRemoteRequest.GetBlacklist(Model.token);
                                    if (entityBlackLists != null)
                                    {
                                        MonitorRemoteRequest.AddOptLog(Model.token, "黑名单车辆", Model.sUserName + ":添加黑名单车辆：" + listOpt.getCPH()
                                                , Model.sUserCard, Model.sUserName, String.valueOf(Model.stationID));// 添加成功
                                        Message message = mHandler.obtainMessage();
                                        message.obj = entityBlackLists;
                                        message.what = MSG_UpdateBlackListData;
                                        mHandler.sendMessage(message);

                                        // 表示添加成功
                                    }

                                }
                            }).start();
                        }
                    };
                }
                formAddBlackListView.show();
            }

            /**
             * 点击弹出期限查询按钮
             */
            @Override
            public void onClickDealLineQueryBtn()
            {
                if (reportDealLineView == null)
                {
                    reportDealLineView = new ReportDealLineView(ParkingMonitoringActivity.this);
                }
                reportDealLineView.show();
            }

            /**
             * 点击弹出换班登录按钮
             */
            @Override
            public void onClickShiftLoginBtn()
            {
                if (parkingChangeShifts == null)
                {
                    parkingChangeShifts = new ParkingChangeView(ParkingMonitoringActivity.this);
                }
                parkingChangeShifts.show();
            }

            /**
             * 点击弹出收费车辆按钮
             */
            @Override
            public void onClickChargeRecordBtn()
            {
            }

            /**
             * 点击弹出场内车辆按钮
             */
            @Override
            public void onClickGroundVehicleBtn()
            {
                ConcurrentQueueHelper.getInstance().put(new ModelNode(1, "hello", "strFile", "strFileJpg", "strCPH"));
            }
        };

        tcpsdk.getInstance().setup(); // tcpsdk的初始化
        initView(savedInstanceState);

        initFields();

//        initControl();

        startAliveTime = System.currentTimeMillis();
        mHandler.sendEmptyMessage(MSG_KeppAlive);

        exe = new ExeUtil();

        // 使用阻塞的队列来获取车牌数据
        queueTask = new QueueTask(true);
        queueTask.start();
    }

    @Nullable
    private int requestUrlRequestWhenNoPlateIn(SetCarInWithoutCPHReq carInWithoutCPHReq)
    {
        L.i("无牌车进场数据:" + carInWithoutCPHReq);
        final SetCarInWithoutCPHResp setCarInWithoutCPHResp = GetServiceData.getInstance().SetCarInWithoutCPH(carInWithoutCPHReq);
        if (setCarInWithoutCPHResp == null)
        {
            return -1;
        }

        dealCarInWithOutCPH(setCarInWithoutCPHResp);

        // 更新画面提示信息
        updateCarHintToFragment(MSG_CarHintInfoAfterResume, setCarInWithoutCPHResp.getMsg());

        mHandler.post(new Runnable()
        {
            @Override
            public void run()
            {
                updateCarChargeToFragment(MSG_SETCarInWithOutCPH, setCarInWithoutCPHResp.getData());
            }
        });
        return 0;
    }

    private List<String> getRoadNameByType(int type)
    {
        ArrayList<String> strings = new ArrayList<>();
        int index = -1;
        for (int i = 0; i < entityRoadWaySets.size(); i++)
        {
            if (entityRoadWaySets.get(i).getInOut() == type)
            {
                strings.add(entityRoadWaySets.get(i).getInOutName());
            }
        }
        return strings;
    }

    private void sendModeToQueue(ModelNode.E_CarInType type, String cph, Object data, int index)
    {
        ModelNode modelNode = new ModelNode();
        modelNode.data = data;

        modelNode.type = type;
        modelNode.setsDzScan("");
        modelNode.setiDzIndex(index);
        String fileName = SDCardUtils.getSDCardPath() + "picture.jpg";
        L.i("获取到的fileName:" + fileName);
        parkingMonitoringView.saveImage(fileName, index);
        modelNode.setStrFileJpg(fileName);
        modelNode.setStrCPH(cph);
        ConcurrentQueueHelper.getInstance().put(modelNode);
    }

    private int getCtrlNoByInOutName(String roadName)
    {
        for (int i = 0; i < entityRoadWaySets.size(); i++)
        {
            if (entityRoadWaySets.get(i).getInOut() == 0
                    && roadName.equals(entityRoadWaySets.get(i).getInOutName()))// 表示入场
            {
                return entityRoadWaySets.get(i).getCtrlNumber();
            }
        }


        L.i("getCtrlNoByInOutName no find ctrlNO, roadName", roadName);
        return -1;
    }

    private int getChannelIndex(int type)
    {
        int index = -1;
        for (int i = 0; i < entityRoadWaySets.size(); i++)
        {
            if (entityRoadWaySets.get(i).getInOut() == type)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    private int getCtrlNumber(int type)
    {
        int result = -1;
        for (int i = 0; i < entityRoadWaySets.size(); i++)
        {
            if (entityRoadWaySets.get(i).getInOut() == type)
            {
                result = entityRoadWaySets.get(i).getCtrlNumber();
                return result;
            }
        }
        return result;
    }

    private void initView(@Nullable Bundle savedInstanceState)
    {
        // 初始化按钮颜色
        parkingMonitoringView.onClickInCarChargeInfo();
        parkingMonitoringView.onClickInCarInParkingDetail();

        // 初始化fragment
        initFragment(savedInstanceState);
    }


    private int requestUrlUpdateUiWhenSetIn(SetCarInReq setCarInReq)
    {
        /**
         *  由车牌号，来请求服务器数据
         */
        final SetCarInResp setCarInResp = GetServiceData.getInstance().SetCarIn(setCarInReq);
        if (setCarInReq == null)
        {
            return -1;
        }

        if (dealSetCarInResponse(setCarInResp, setCarInReq.getCPH()) == 4) // 等待相应的确认开闸之后，再进行更新界面数据
        {
            return -1;
        }

        /**
         * 更新提示信息，并设置提示之后，显示原来的界面
         */
        updateCarHintToFragment(MSG_CarHintInfoAfterResume, setCarInResp.getMsg());

        /**
         * 更新车辆收费信息
         */
        mHandler.post(new Runnable()
        {
            @Override
            public void run()
            {
                updateCarChargeToFragment(MSG_SETCarIn, setCarInResp.getData());
            }
        });

        return 0;
    }

    /**
     * 根据无牌车返回的数据，提示发送语音
     */
    private void dealCarInWithOutCPH(SetCarInWithoutCPHResp setCarInWithoutCPHResp)
    {
        /**
         * 发送开闸指令
         */

        /**
         * 发送语音
         */

        /**
         * 结束
         */
    }


    /**
     * 根据车辆进场，提示发送语音数据
     * 注意返回数据的车牌和原始的车牌号可能不同
     *
     * @param setCarInResp
     */
    private int dealSetCarInResponse(final SetCarInResp setCarInResp, final String srcCPH)
    {
        int flag = RCodeDeal.detectionRcode(Integer.parseInt(setCarInResp.getRcode()));
        switch (flag)
        {
            case -1:
                // 直接结束
                break;
            case 0:
                // 正常入场
                break;
            case 1:
                // 发送语音提示信息，后结束，针对于固定月租车
                break;
            case 2:
                // 发送语音提示信息，后结束，针对于非固定车
                break;
            case 3:
                // 发送开闸指令,发语音，结束
                break;
            case 4:
                // 判断是否放行，发送开闸指令，结束
                // 弹出界面后，然后是否放行
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        // 弹出界面
                        parkingTempCPHView = new ParkingTempCPHView(ParkingMonitoringActivity.this)
                        {
                            @Override
                            public void onClickOk(final SetCarInConfirmReq setCarInConfirmReq, String roadName)
                            {
                                new Thread(new Runnable()
                                {
                                    @Override
                                    public void run()
                                    {
                                        // 请求数据
                                        setCarInConfirmReq.setStationId(Model.stationID);
                                        setCarInConfirmReq.setToken(Model.token);
                                        setCarInConfirmReq.setCPH(srcCPH);
                                        setCarInConfirmReq.setCtrlNumber(entityRoadWaySets.get(0).getCtrlNumber());

                                        final SetCarInConfirmResp setCarInConfirmResp = GetServiceData.getInstance().SetCarInConfirmed(setCarInConfirmReq);
                                        if (setCarInConfirmResp == null)
                                            return;
                                        L.i("确认车牌信息:" + setCarInConfirmReq);

                                        // 更新界面提示信息
                                        mHandler.post(new Runnable()
                                        {
                                            @Override
                                            public void run()
                                            {
                                                updateCarHintToFragment(MSG_CarHintInfoAfterResume, setCarInConfirmResp.getMsg());
                                            }
                                        });

                                        if (setCarInConfirmResp.getData() == null)
                                            return;
                                        // 更新收费信息
                                        mHandler.post(new Runnable()
                                        {
                                            @Override
                                            public void run()
                                            {
                                                updateCarChargeToFragment(MSG_SETCarInComfirmed, setCarInConfirmResp.getData());
                                            }
                                        });

                                        final int channelIndex = getChannelIndex(CAR_CHANNEL_IN);
                                        String fileName = SDCardUtils.getSDCardPath() + "picture.jpg";
                                        parkingMonitoringView.saveImage(fileName, channelIndex);
                                        final Bitmap bitmap = BitmapUtils.fileToBitmap(fileName);
                                        mHandler.post(new Runnable()
                                        {
                                            @Override
                                            public void run()
                                            {
                                                dealCPHInfoFromCamera(channelIndex, setCarInConfirmResp.getData().getCPH(), bitmap);
                                            }
                                        });
                                    }
                                }).start();
                                parkingTempCPHView.dismiss();
                            }

                            @Override
                            public void onClickCancel()
                            {
                                parkingTempCPHView.dismiss();
                            }

                            @Override
                            public void prepareLoadData()
                            {
                                parkingTempCPHView.setCPH(srcCPH);
                                ArrayList<String> list = new ArrayList<>();
                                for (int i = 0; i < entityRoadWaySets.size(); i++)
                                {
                                    if (entityRoadWaySets.get(i).getInOut() == 0)// 表示入场
                                    {
                                        list.add(entityRoadWaySets.get(i).getInOutName());
                                    }
                                }
                                parkingTempCPHView.setSpinnerRoadName(list);
                            }
                        };
                        parkingTempCPHView.show();
                    }
                });
                break;
            default:
                L.i("SetCarInResp 返回信息:" + setCarInResp.getMsg());
                break;
        }
        return flag;
    }


    private void updateCarChargeToFragment(int what, Object obj)
    {
        Message message = mHandler.obtainMessage();
        message.what = what;
        message.obj = obj;
        mHandler.sendMessage(message);
    }

    private void updateCarHintToFragment(int what, String msg)
    {
        int currentIndex = fragmentChargeManager.getCurrentIndex();
//        fragmentChargeManager.showFragment(2);//更新数据
        Message message = mHandler.obtainMessage();
        message.what = what;
        message.arg1 = currentIndex;
        message.obj = msg;
        mHandler.sendMessage(message);
    }


    /**
     * 通过网络上获取相应的权限来获取当前按钮的使能情况;
     */
    private void dealRights()
    {
        boolean[] booleen = new boolean[]{false, true, false, false, true};//五个按钮
        for (GetRightsByGroupIDResp.DataBean o : Model.lstRights)
        {
            if (o.getFormName().equals(getResources().getString(R.string.parkMontior_licenseVehicleRegister)))
            {
                String[] btnTagText = parkingPlateRegisterView.getBtnTagText();
                for (int i = 0; i < btnTagText.length; i++)
                {
                    if (o.getItemName().equals(btnTagText[i]))
                    {
                        booleen[i] = o.isCanOperate();
                        L.i("dealRights:" + "itemName:" + o.getItemName());

                        if (o.getItemName().equals("btnAdd"))
                        {
                            parkingPlateRegisterView.setIsAdd(o.isCanOperate());
                        }

                        if (o.getItemName().equals("btnDelete"))
                        {
                            parkingPlateRegisterView.setIsDelete(o.isCanOperate());
                        }
                        break;
                    }
                }
            }
        }

        parkingPlateRegisterView.setBtnEnable(booleen);
    }


    private String[] dealCarType()
    {
        final List<EntityCarTypeInfo> entityCarTypeInfos = MonitorRemoteRequest.GetGetFXCardTypeToTrue(Model.token);
        String[] data = new String[entityCarTypeInfos.size()];
        for (int i = 0; i < entityCarTypeInfos.size(); i++)
        {
            data[i] = entityCarTypeInfos.get(i).getCardType();
        }

        return data;
    }

    private List<String> dealUserNo()
    {
        final List<EntityPersonnelInfo> entityUserInfoList = MonitorRemoteRequest.GetPersonnel(Model.token);
        List<String> list = new ArrayList<String>();
        for (int i = 0; i < entityUserInfoList.size(); i++)
        {
            list.add(entityUserInfoList.get(i).getUserNO());
        }
        return list;
    }

    private String checkStringIsNull(String valueStr)
    {
        if (valueStr == null)
            return "";
        else
            return valueStr;
    }

    private ArrayList<HashMap<String, String>> dealDataGridView()
    {
        final List<EntityCardIssue> EntityCardIssues = MonitorRemoteRequest.GetCarChePIss(Model.token, null);
        if (EntityCardIssues != null && EntityCardIssues.size() >= 0)
        {
            final ArrayList<HashMap<String, String>> items = new ArrayList<HashMap<String, String>>();
            for (EntityCardIssue carIssue : EntityCardIssues)
            {
                HashMap<String, String> item = new HashMap<String, String>();
                item.put(ColumnName.c1, checkStringIsNull(carIssue.getCPH()));
                item.put(ColumnName.c2, checkStringIsNull(carIssue.getCardNO()));
                item.put(ColumnName.c3, checkStringIsNull(carIssue.getCardType()));
                item.put(ColumnName.c4, checkStringIsNull(carIssue.getCardState()));
                item.put(ColumnName.c5, checkStringIsNull(carIssue.getUserNO()));
                item.put(ColumnName.c6, checkStringIsNull(carIssue.getUserName()));
                item.put(ColumnName.c7, checkStringIsNull(carIssue.getCarValidStartDate()));
                item.put(ColumnName.c8, checkStringIsNull(carIssue.getCarValidEndDate()));
                item.put(ColumnName.c9, checkStringIsNull(carIssue.getMobNumber()));
                item.put(ColumnName.c10, checkStringIsNull(carIssue.getHomeAddress()));
                item.put(ColumnName.c11, String.valueOf(carIssue.getBalance())); // 充值余额
                item.put(ColumnName.c12, String.valueOf(carIssue.getCardYJ())); // 车辆押金
                item.put(ColumnName.c13, checkStringIsNull(carIssue.getCarType()));
                item.put(ColumnName.c14, checkStringIsNull(carIssue.getCarPlace())); //车位
                item.put(ColumnName.c15, checkStringIsNull(carIssue.getCarIssueDate()));
                item.put(ColumnName.c16, checkStringIsNull(carIssue.getCarIssueUserCard()));
                item.put(ColumnName.c17, checkStringIsNull(carIssue.getCarValidZone()));//下载标识
                item.put(ColumnName.c18, checkStringIsNull(carIssue.getCarMemo()));// 车场备注
                item.put(ColumnName.c19, String.valueOf(carIssue.getCarPlaceNo()));
                items.add(item);
            }
            return items;
        }
        return null;
    }

    /**
     * 发送心跳消息，不然30秒就会出现过期的情况
     */
    private void sendKeepAlive()
    {
        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                int retValue = MonitorRemoteRequest.KeepAlive(Model.token);
                if (retValue == RequestByURL.ERROR_BASE_CODE
                        || RequestByURL.TOKEN_OVERDUE == retValue)
                {
                    mHandler.sendEmptyMessage(MSG_TokenFailed);
                }
                else
                {
                    mHandler.sendEmptyMessageDelayed(MSG_KeppAlive, 5 * 1000);
                }
            }
        }).start();
    }

    /**
     * 实现视频数据
     */
    private void Myinitcaptrure()
    {
//        for (int i = 0; i < Model.iChannelCount; i++)
        for (int i = 0; i < entityRoadWaySets.size(); i++)
        {
            List<EntityNetCameraSet> entityNetCameraSets = MonitorRemoteRequest.GetNetCameraSet(Model.token, entityRoadWaySets.get(i).getCameraIP());

            if (entityNetCameraSets.size() > 0)
            {
                L.i("getVideoType", entityNetCameraSets.get(0).getVideoType());
                String videoType = entityNetCameraSets.get(0).getVideoType();
                switch (videoType)
                {
//                    case
                }
            }

        }
    }


    private void initFields()
    {
        /**
         * 1，获取车辆类型 即固定车和储值车，临时车
         */

        /**
         * 2，获取车道信息，来播放视频数据
         */
        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                if (!requestGetCheDaoSet()) // 获取车道信息
                {
                    return;
                }
                final List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();
                for (int i = 0; i < data.size(); i++) // i 表示第几个车道，下标从0开始
                {
                    final int t = i;
                    mHandler.post(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            updateChannelText(data, t);
                        }
                    });

                    if (requestGetNetCameraSet(data.get(i).getCameraIP()))
                    {
                        if (netCameraList.get(i) != null
                                && netCameraList.get(i).getData() != null
                                && netCameraList.get(i).getData().size() > 0)
                        {
                            List<GetNetCameraSetResp.DataBean> data1 = netCameraList.get(i).getData();
                            String videoType = data1.get(0).getVideoType();
                            switch (videoType)
                            {
                                case "ZNYKTY5":
                                {
                                    parkingMonitoringView.playVideoByIndex(i, data1.get(0).getVideoIP(), mHandler);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }).start();
    }

    private void updateChannelText(List<GetCheDaoSetResp.DataBean> data, int t)
    {
        String inOutName = data.get(t).getInOutName();
        if (data.get(t).getInOut() == CAR_CHANNEL_OUT)
        {
            parkingMonitoringView.setChannelText(t, inOutName);

        }
        else if (data.get(t).getInOut() == CAR_CHANNEL_IN)
        {
            parkingMonitoringView.setChannelText(t, inOutName);
        }

        if (data.size() <= 2) // 在2路之内 这里的逻辑要修改
        {
            parkingMonitoringView.setInCarPictureChannelText("入口图片显示");
            parkingMonitoringView.setOutCarPictureChannelText("出口图片显示");
        }
    }


    private void initControl()
    {
        /**
         * 1,从服务器端加载收费标准，然后设置控制板
         */

        /**
         * 2，显示状态栏
         */
        parkingMonitoringView.showStatusBar(Model.sUserName, Model.sUserCard,
                TimeConvertUtils.longToString(Model.dLoginTime), TimeConvertUtils.longToString(System.currentTimeMillis()));

        /**
         * 3，获取进场和出场信息对应着 场内车辆明细和车辆收费明细
         */
        requestParkingDetailFromNet();

        /**
         * 统计获取车位信息，显示到界面即可
         */
        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                entityParkingInfo = MonitorRemoteRequest.GetParkingInfo(Model.token, String.valueOf(Model.stationID));
            }
        }).start();
    }

    private void requestParkingDetailFromNet()
    {
        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                entityCarOuts = MonitorRemoteRequest.GetCarOut(Model.token, String.valueOf(Model.iParkingNo));
                entityCarIns = MonitorRemoteRequest.GetCarIn(Model.token, String.valueOf(Model.iParkingNo));

                if (fragmentDetailManager.getCurrentIndex() == 0)
                {
                    mHandler.sendEmptyMessage(MSG_CarIn);
                }
                else if (fragmentDetailManager.getCurrentIndex() == 1)
                {
                    mHandler.sendEmptyMessage(MSG_CarOut);
                }
            }
        }).start();
    }

    @Override
    protected void onSaveInstanceState(Bundle outState)
    {
        //“内存重启”时保存当前的fragment名字
        super.onSaveInstanceState(outState);
        outState.putString(FragmentChargeManager.CURRENT_FRAGMENT, fragmentChargeManager.getCurrentFragmentName());
        outState.putString(FragmentDetailManager.CURRENT_FRAGMENT, fragmentDetailManager.getCurrentFragmentName());
    }

    private void initFragment(Bundle savedInstanceState)
    {
        fragmentChargeManager = new FragmentChargeManager(getSupportFragmentManager());
        fragmentChargeManager.init(savedInstanceState);

        fragmentDetailManager = new FragmentDetailManager(getSupportFragmentManager());
        fragmentDetailManager.init(savedInstanceState);
    }


    public static final int MSG_CarIn = 0x01;
    public static final int MSG_CarOut = 0x02;
    public static final int MSG_ParkingInfo = 0x3;
    public static final int MSG_ChargeInfo = 0x04;
    public static final int MSG_SETCarIn = 0X05;
    public static final int MSG_SETCarOut = 0X06;
    public static final int MSG_KeppAlive = 0x07;
    public long startAliveTime;

    public static final int MSG_UpdateBlackListData = 0x08;
    public static final int MSG_SET_CarChannel = 0x11;

    public static final int MSG_START_VIDEO_PLAY = 0x09;
    public static final int MSG_STOIP_VIDEO_PLAY = 0x10;

    public int CAR_CHANNEL_OUT = 1; // 表示车辆入口标记
    public int CAR_CHANNEL_IN = 0; // 表示车辆出口标记

    public final static int MSG_TokenFailed = 0x12;
    public final static int MSG_CarHintInfoAfterResume = 013; // 恢复到之前的收费信息
    public static final int MSG_SETCarInWithOutCPH = 0x14;
    public static final int MSG_SETCarInComfirmed = 0x15;
    public static final int MSG_updateInfoWhenRecvRecognitionCPH = 0x16; // 当相机识别到车牌数据后，开始进场

    private Handler mHandler = new Handler()
    {
        @Override
        public void handleMessage(final Message msg)
        {
            switch (msg.what)
            {
                case MSG_CarIn: // 场内车辆
                {
                    updateCarInParkingData();
                    break;
                }
                case MSG_CarOut: // 车辆收费明细
                {
                    updateCarDetailData();
                    break;
                }
                case MSG_ParkingInfo:
                {
                    updateParkingInfo();
                    break;
                }
                case MSG_ChargeInfo:
                {
                    updateChargeInfo();
                    break;
                }
                case MSG_SETCarIn:
                {
                    SetCarInResp.DataBean entitySetCarIn = (SetCarInResp.DataBean) msg.obj;
                    updateSetCarIn(entitySetCarIn);
                    break;
                }
                case MSG_KeppAlive:
                {
                    sendKeepAlive();
                    break;
                }
                case MSG_SETCarOut:
                {
                    EntitySetCarOut entitySetCarOut = (EntitySetCarOut) msg.obj;
                    updateSetCarInByOut(entitySetCarOut);
                    break;
                }
                case MSG_UpdateBlackListData:
                {
                    List<EntityBlackList> entityBlackList = (List<EntityBlackList>) msg.obj;
                    formAddBlackListView.setData(entityBlackList);
                    break;
                }
                case MSG_START_VIDEO_PLAY:
                {
                    String objIP = (String) msg.obj;
                    L.i("MSG_START_VIDEO_PLAY ### ip==>>", objIP);
                    if (entityRoadWaySets == null || entityRoadWaySets.size() <= 0)
                    {
                        return;
                    }

                    for (int channel = 0; channel < entityRoadWaySets.size(); channel++)
                    {
                        if (entityRoadWaySets.get(channel).getCameraIP().equals(objIP))
                        {
                            L.i("MSG_START_VIDEO_PLAY find index:" + channel + ", ip:" + objIP);
                            parkingMonitoringView.setSurfaceEnableWhenPlayVideo(channel);
                            break;
                        }
                    }
                    break;
                }
                case MSG_STOIP_VIDEO_PLAY:
                {
                    String objIP = (String) msg.obj;
                    L.i("MSG_STOIP_VIDEO_PLAY ### ip==>>", objIP);
                    if (entityRoadWaySets == null || entityRoadWaySets.size() <= 0)
                    {
                        return;
                    }

                    for (int channel = 0; channel < entityRoadWaySets.size(); channel++)
                    {
                        if (entityRoadWaySets.get(channel).getCameraIP().equals(objIP))
                        {
                            L.i("MSG_STOIP_VIDEO_PLAY find index:" + channel + ", ip:" + objIP);
                            parkingMonitoringView.setTextEnableWhenStopVideo(channel);
                            break;
                        }
                    }
                    break;
                }
                case MSG_SET_CarChannel:
                {
                    int index = (int) msg.obj;
                    if (entityRoadWaySets == null || entityRoadWaySets.size() <= 0)
                    {
                        return;
                    }

                    EntityRoadWaySet entityRoadWaySet = entityRoadWaySets.get(index);
                    if (entityRoadWaySet.getInOut() == CAR_CHANNEL_OUT)
                    {
                        parkingMonitoringView.setChannelText(index, entityRoadWaySet.getInOutName());

                    }
                    else if (entityRoadWaySet.getInOut() == CAR_CHANNEL_IN)
                    {
                        parkingMonitoringView.setChannelText(index, entityRoadWaySet.getInOutName());
                    }

                    parkingMonitoringView.setInCarPictureChannelText("入口图片显示");
                    parkingMonitoringView.setOutCarPictureChannelText("出口图片显示");

                    break;
                }
                case MSG_TokenFailed:
                {
                    finish();
                    break;
                }
                case MSG_CarHintInfoAfterResume:
                {
                    final int arg1 = msg.arg1;
                    String prompt = (String) msg.obj;

                    parkingMonitoringView.showInChargeFragment(prompt);
//                    fragmentChargeManager.setTextData(prompt);

                    mHandler.postDelayed(new Runnable()
                    {
                        @Override
                        public void run()
                        {
//                            fragmentChargeManager.showFragment(arg1);
                            parkingMonitoringView.resumeInChargeFragment();
                        }
                    }, 1000 * 2);
                    break;
                }
                case MSG_SETCarInWithOutCPH:
                {
                    SetCarInWithoutCPHResp.DataBean entitySetCarInWithoutCPH = (SetCarInWithoutCPHResp.DataBean) msg.obj;
                    updateSetCarInWithoutCPH(entitySetCarInWithoutCPH);
                    break;
                }
                case MSG_SETCarInComfirmed:
                {
                    updateSetCarInConfirmed((SetCarInConfirmResp.DataBean) msg.obj);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    };

    private String checkValue(String value)
    {
        if (value == null)
        {
            return "";
        }
        return value;
    }

    private void updateSetCarInWithoutCPH(SetCarInWithoutCPHResp.DataBean carInWithoutCPH)
    {
        if (carInWithoutCPH == null) return;
        ArrayList<String> arrayList = new ArrayList<>();

        arrayList.add(checkValue(""));// 11行数据
        arrayList.add(checkValue(""));
        arrayList.add(checkValue(carInWithoutCPH.getCardNO()));
        arrayList.add(checkValue(""));
        arrayList.add(checkValue(carInWithoutCPH.getCardType()));
        arrayList.add(checkValue("0.00")); //免费金额
        if (carInWithoutCPH.getInTime() == null || carInWithoutCPH.getInTime().equals(""))
        {
            arrayList.add(TimeConvertUtils.longToString(System.currentTimeMillis()));
        }
        else
        {
            arrayList.add(carInWithoutCPH.getInTime());
        }
        arrayList.add(checkValue("")); // 出场时间
        arrayList.add(checkValue("0.00"));//收费金额
        arrayList.add(checkValue("0.00"));//累计金额
        arrayList.add(checkValue("0.00"));//剩余金额
        fragmentChargeManager.setData(arrayList, null);
    }

    private void updateSetCarInConfirmed(SetCarInConfirmResp.DataBean carInConfirmed)
    {
        if (carInConfirmed == null)
            return;
        ArrayList<String> arrayList = new ArrayList<>();

        arrayList.add(checkValue(carInConfirmed.getUserNO()));// 11行数据
        arrayList.add(checkValue(carInConfirmed.getUserName()));
        arrayList.add(checkValue(carInConfirmed.getCardNO()));
        arrayList.add(checkValue(carInConfirmed.getDeptName()));
        arrayList.add(checkValue(carInConfirmed.getCardType()));
        arrayList.add(checkValue("0.00")); //免费金额
        if (carInConfirmed.getInTime() == null || carInConfirmed.getInTime().equals(""))
        {
            arrayList.add(TimeConvertUtils.longToString(System.currentTimeMillis()));
        }
        else
        {
            arrayList.add(carInConfirmed.getInTime());
        }
        arrayList.add(checkValue(carInConfirmed.getCarValidEndDate())); // 出场时间
        arrayList.add(checkValue("0.00"));//收费金额
        arrayList.add(checkValue("0.00"));//累计金额
        arrayList.add(checkValue("0.00"));//剩余金额
        fragmentChargeManager.setData(arrayList, null);
    }

    private void updateSetCarIn(SetCarInResp.DataBean entitySetCarIn)
    {
        if (entitySetCarIn == null)
            return;
        ArrayList<String> arrayList = new ArrayList<>();

        arrayList.add(checkValue(entitySetCarIn.getUserNO()));// 11行数据
        arrayList.add(checkValue(entitySetCarIn.getUserName()));
        arrayList.add(checkValue(entitySetCarIn.getCardNO()));
        arrayList.add(checkValue(entitySetCarIn.getDeptName()));
        arrayList.add(checkValue(entitySetCarIn.getCardType()));
        arrayList.add(checkValue("0.00")); //免费金额
        if (entitySetCarIn.getIntime() == null || entitySetCarIn.getIntime().equals(""))
        {
            arrayList.add(TimeConvertUtils.longToString(System.currentTimeMillis()));
        }
        else
        {
            arrayList.add(entitySetCarIn.getIntime());
        }
        arrayList.add(checkValue(entitySetCarIn.getCarValidEndDate())); // 出场时间
        arrayList.add(checkValue("0.00"));//收费金额
        arrayList.add(checkValue("0.00"));//累计金额
        arrayList.add(checkValue("0.00"));//剩余金额
        fragmentChargeManager.setData(arrayList, null);
    }

    private void updateSetCarInByOut(EntitySetCarOut entitySetCarOut)
    {
        ArrayList<String> arrayList = new ArrayList<>();
        arrayList.add(checkValue(entitySetCarOut.getUserNO()));
        arrayList.add(checkValue(entitySetCarOut.getUserName()));
        arrayList.add(checkValue(entitySetCarOut.getCardNO()));
        arrayList.add(checkValue(entitySetCarOut.getDeptName()));
        arrayList.add(checkValue(entitySetCarOut.getCardType()));
        arrayList.add(checkValue("0.00"));//收费金额
        arrayList.add(checkValue(entitySetCarOut.getInTime()));
        arrayList.add(checkValue(entitySetCarOut.getOutTime()));
        arrayList.add(checkValue("0.00"));//收费金额
        arrayList.add(checkValue(entitySetCarOut.getSFJE() + ""));
        arrayList.add(checkValue(entitySetCarOut.getBalance() + ""));
        fragmentChargeManager.setData(arrayList, null);
    }

    /**
     * 更新收费信息
     */
    private void updateChargeInfo()
    {

    }

    /**
     * 更新车场数据
     */
    private void updateParkingInfo()
    {
        if (entityParkingInfo == null)
        {
            return;
        }

        ArrayList<String> arrayList = new ArrayList<String>();
        arrayList.add(entityParkingInfo.getMonthCarCountInPark() + "");
        arrayList.add(entityParkingInfo.getFreeCarCountInPark() + "");
        arrayList.add(entityParkingInfo.getTempCarCountInPark() + "");
        arrayList.add(entityParkingInfo.getPrepaidCarCountInPark() + "");
        arrayList.add(entityParkingInfo.getManualOpenCarCount() + "");

        fragmentChargeManager.setData(null, arrayList);
    }

    private void updateCarDetailData()
    {
        if (entityCarOuts != null && entityCarOuts.size() >= 0)
        {
            L.i("entityCarOuts.size():" + entityCarOuts.size());
            ArrayList<HashMap<String, String>> items = new ArrayList<HashMap<String, String>>();
            for (int i = 0; i < entityCarOuts.size(); i++)
            {
                HashMap<String, String> item = new HashMap<String, String>();
                item.put(ColumnName.c1, checkStringIsNull(entityCarOuts.get(i).getCPH()));
                item.put(ColumnName.c2, checkStringIsNull(entityCarOuts.get(i).getChineseName()));
                item.put(ColumnName.c3, checkStringIsNull(entityCarOuts.get(i).getInTime()));
                item.put(ColumnName.c4, checkStringIsNull(entityCarOuts.get(i).getOutTime()));
                item.put(ColumnName.c5, String.valueOf(entityCarOuts.get(i).getSFJE()));
                item.put(ColumnName.c6, checkStringIsNull(entityCarOuts.get(i).getInGateName()));
                item.put(ColumnName.c7, checkStringIsNull(entityCarOuts.get(i).getOutGateName()));
                item.put(ColumnName.c8, checkStringIsNull(entityCarOuts.get(i).getUserNO()));
                item.put(ColumnName.c9, checkStringIsNull(entityCarOuts.get(i).getUserName()));
                item.put(ColumnName.c10, checkStringIsNull(entityCarOuts.get(i).getCardNO()));
                item.put(ColumnName.c11, checkStringIsNull(entityCarOuts.get(i).getBalance() + ""));//免费原因 FreeReason
                item.put(ColumnName.c12, String.valueOf(entityCarOuts.get(i).getYSJE()));
                item.put(ColumnName.c13, checkStringIsNull(entityCarOuts.get(i).getSFTime()));
                item.put(ColumnName.c14, checkStringIsNull(entityCarOuts.get(i).getInOperator())); // 收费人员 SFOperator
                item.put(ColumnName.c15, checkStringIsNull(entityCarOuts.get(i).getInOperatorCard()));//收费人员编号
                item.put(ColumnName.c16, checkStringIsNull(entityCarOuts.get(i).getInGateName()));//收费口名 SFGate

                item.put(ColumnName.c17, checkStringIsNull(""));//超时标志 OvertimeSymbol
                item.put(ColumnName.c18, checkStringIsNull(entityCarOuts.get(i).getOvertimeSFTime()));//超时收费时间 OvertimeSFTime
                item.put(ColumnName.c29, String.valueOf(entityCarOuts.get(i).getOvertimeSFJE()));//超时收费金额 OvertimeSFJE
                item.put(ColumnName.c20, String.valueOf(entityCarOuts.get(i).getCarparkNO()));//车场编号 CarparkNO
                item.put(ColumnName.c21, String.valueOf(entityCarOuts.get(i).getBigSmall()));//大小标识 BigSmall
                item.put(ColumnName.c22, checkStringIsNull(entityCarOuts.get(i).getFreeReason()));//免费原因 FreeReason
                item.put(ColumnName.c23, checkStringIsNull(entityCarOuts.get(i).getInUser()));//人场人员 InUser
                item.put(ColumnName.c24, checkStringIsNull(entityCarOuts.get(i).getOutUser()));//出场人员 OutUser
                item.put(ColumnName.c25, checkStringIsNull(entityCarOuts.get(i).getInPic()));//入场图片 InPic
                item.put(ColumnName.c26, checkStringIsNull(entityCarOuts.get(i).getOutPic()));//出场图片 OutPic
                item.put(ColumnName.c27, checkStringIsNull(entityCarOuts.get(i).getDeptName()));//部门名称 DeptName
                item.put(ColumnName.c28, "");//证件图片 ZJPic
                item.put(ColumnName.c29, checkStringIsNull(entityCarOuts.get(i).getInOperatorCard()));//入场操作编号 InOperatorCard
                item.put(ColumnName.c30, checkStringIsNull(entityCarOuts.get(i).getOutOperatorCard()));//出场操作编号 OutOperatorCard
                item.put(ColumnName.c31, checkStringIsNull(entityCarOuts.get(i).getInOperator()));//入场操作员 InOperator
                item.put(ColumnName.c32, checkStringIsNull(entityCarOuts.get(i).getOutOperator()));//出场操作员 OutOperator
                items.add(item);
            }
            fragmentDetailManager.setData(null, null, items, null);
        }
    }

    private void updateCarInParkingData()
    {
        if (entityCarIns != null && entityCarIns.size() >= 0)
        {
            int maxTextWidth[] = new int[16];
            for (int m = 0; m < 16; m++)
            {
                maxTextWidth[m] = -1;
            }

            ArrayList<HashMap<String, String>> items = new ArrayList<HashMap<String, String>>();
            for (int i = 0; i < entityCarIns.size(); i++)
            {
                HashMap<String, String> item = new HashMap<String, String>();
                item.put(ColumnName.c1, checkStringIsNull(entityCarIns.get(i).getCPH()));
                item.put(ColumnName.c2, checkStringIsNull(entityCarIns.get(i).getChineseName()));
                item.put(ColumnName.c3, checkStringIsNull(entityCarIns.get(i).getInTime()));
                item.put(ColumnName.c4, checkStringIsNull(entityCarIns.get(i).getInGateName()));
                item.put(ColumnName.c5, checkStringIsNull(entityCarIns.get(i).getUserNO()));
                item.put(ColumnName.c6, checkStringIsNull(entityCarIns.get(i).getUserName()));
                item.put(ColumnName.c7, String.valueOf(entityCarIns.get(i).getBalance()));
                item.put(ColumnName.c8, checkStringIsNull(entityCarIns.get(i).getCardNO()));
                item.put(ColumnName.c9, String.valueOf(entityCarIns.get(i).getCarparkNO()));
                item.put(ColumnName.c10, String.valueOf(entityCarIns.get(i).getBigSmall()));
                item.put(ColumnName.c11, "");//免费原因 FreeReason
                item.put(ColumnName.c12, checkStringIsNull(entityCarIns.get(i).getInPic()));
                item.put(ColumnName.c13, checkStringIsNull(entityCarIns.get(i).getDeptName()));
                item.put(ColumnName.c14, ""); // ZJPic
                item.put(ColumnName.c15, checkStringIsNull(entityCarIns.get(i).getInOperatorCard()));
                item.put(ColumnName.c16, checkStringIsNull(entityCarIns.get(i).getInOperator()));
                items.add(item);

//                maxTextWidth[0] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[0]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[0]);
//                maxTextWidth[1] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[1]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[1]);
                maxTextWidth[2] = ((entityCarIns.get(i).getInTime().length() > maxTextWidth[2]) ? entityCarIns.get(i).getInTime().length() : maxTextWidth[2]);
//                maxTextWidth[3] = ((entityCarIns.get(i).getInTime().length() > maxTextWidth[3]) ? entityCarIns.get(i).getInTime().length() : maxTextWidth[3]);
//                maxTextWidth[4] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[4]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[4]);
//                maxTextWidth[5] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[5]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[5]);
//                maxTextWidth[6] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[6]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[6]);
//                maxTextWidth[7] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[7]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[7]);
//                maxTextWidth[8] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[8]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[8]);
//                maxTextWidth[9] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[9]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[9]);
//                maxTextWidth[10] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[10]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[10]);
//                maxTextWidth[11] = ((entityCarIns.get(i).getInPic().length() > maxTextWidth[11]) ? entityCarIns.get(i).getInPic().length() : maxTextWidth[11]);
//                maxTextWidth[12] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[12]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[12]);
//                maxTextWidth[13] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[13]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[13]);
//                maxTextWidth[14] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[14]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[14]);
//                maxTextWidth[15] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[15]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[15]);
            }

            // 每一列的长度是不同的,取最长的宽度然后设置相应的界面
            StringBuffer stringBuffer = new StringBuffer();
            for (int i = 0; i < maxTextWidth[2]; i++)
            {
                stringBuffer.append(" ");
            }

            TextPaint paint = new TextPaint();
            float scaledDensity = getResources().getDisplayMetrics().scaledDensity;
            paint.setTextSize(scaledDensity * 12); // 12sp
            int textSize = (int) paint.measureText(stringBuffer.toString());
            L.i("maxTextWidth[2]:" + maxTextWidth[2] + ",maxTextWidth[11]:" + maxTextWidth[11] + ",textSize:" + textSize);
            maxTextWidth[2] = textSize;
            fragmentDetailManager.setData(items, maxTextWidth, null, null);
        }
    }

    @Override
    protected void onDestroy()
    {
        for (int i = 0; i < Model.iChannelCount; i++)
        {
            L.i("debug," + "stopVideoByIndex-----------------------------------i:----" + i);
            parkingMonitoringView.stopVideoByIndex(i);
        }

        mHandler.removeMessages(MSG_CarIn);
        mHandler.removeMessages(MSG_CarOut);
        mHandler.removeMessages(MSG_ParkingInfo);
        mHandler.removeMessages(MSG_SETCarIn);
        mHandler.removeMessages(MSG_SETCarOut);
        mHandler.removeMessages(MSG_KeppAlive);
        mHandler.removeMessages(MSG_UpdateBlackListData);

        mHandler.removeMessages(MSG_SET_CarChannel);
        mHandler.removeMessages(MSG_START_VIDEO_PLAY);
        mHandler.removeMessages(MSG_STOIP_VIDEO_PLAY);
        mHandler.removeMessages(MSG_TokenFailed);

//        parkingPlateRegisterView.destoryContainer(); // 不能放到plate中，这里出现问题；
        tcpsdk.getInstance().cleanup();
        mHandler.removeCallbacksAndMessages(null); // 清除所有的handler消息和runnable等;

        queueTask.end();
        ConcurrentQueueHelper.getInstance().destory();
        L.i("debug," + "onDestroy---------------------------------------");
        super.onDestroy();
    }


    @Override
    public void onStart()
    {
        super.onStart(); //？？？ // 重新启动，出现问题即activity不能及时创建，有其它地方使用了activity？
    }

    @Override
    public void onStop()
    {

        super.onStop();
    }

    /**
     * 车牌登记的view
     */
    public class ParkingPlateRegisterViewSub extends ParkingPlateRegisterView
    {
        public ParkingPlateRegisterViewSub(ParkingMonitoringActivity activity)
        {
            super(activity);
        }

        @Override
        public void startLoadData()
        {
            super.startLoadData();
//            exe.post(new Runnable()
            new Thread(new Runnable()
            {
                @Override
                public void run() // 获取机号
                {
                    final List<EntityParkJHSet> entityParkJHSets = MonitorRemoteRequest.GetCCJiHao(Model.token);
                    mHandler.post(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            parkingPlateRegisterView.setJiHaoData(entityParkJHSets);
                        }
                    });

                }
            }).start();

            new Thread(new Runnable()
            {
                @Override
                public void run()// 更新列表数据
                {
                    final ArrayList<HashMap<String, String>> hashMaps = dealDataGridView();
                    mHandler.post(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            parkingPlateRegisterView.setGridViewData(hashMaps);
                        }
                    });

                }
            }).start();

            new Thread(new Runnable()
            {
                @Override
                public void run()// 更新列表数据
                {
                    final List<String> strings = dealUserNo();
                    mHandler.post(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            parkingPlateRegisterView.setSpinnerUserNO(strings);
                        }
                    });

                }
            }).start();

            new Thread(new Runnable()
            {
                @Override
                public void run()// 更新列表数据
                {
                    final List<EntityCarTypeInfo> entityCarTypeInfos = MonitorRemoteRequest.GetGetFXCardTypeToTrue(Model.token);
                    mHandler.post(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            parkingPlateRegisterView.setSpinnerCarType(entityCarTypeInfos);
                        }
                    });

                }
            }).start();

            dealRights();
        }

        @Override
        public void onSelectOnCarAutoNo()
        {
            showCarTextByNetRequest();
        }

        @Override
        public void onSelectOnUserAutoNo()
        {
            showUserNoByNetRequest();
        }

        @Override
        public void onPlateRegisterQuitBtn()
        {
            parkingPlateRegisterView.dismiss();
        }

        @Override
        public void onPlateRegisterPrintBtn()
        {
        }

        @Override
        public void onPlateRegisterDeleteBtn()
        {
            List<String> selectJiHao = parkingPlateRegisterView.getSelectJiHao();
            for (String o : selectJiHao)
            {
                L.i("" + "获取点击的机号为:" + o.toString());
            }
        }

        @Override
        public void onPlateRegisterSaveWhenCheckMultiPace(final EntityCardIssue carIssue)
        {
            if (carIssue == null)
                return;
            new Thread(new Runnable()
            {
                @Override
                public void run()
                {
                    String strCPHS = "";
                    String strCPHR = "";
                    List<String> mutliCarCPH = parkingPlateRegisterView.getMutliCarCPH();
                    for (String item : mutliCarCPH)
                    {
                        carIssue.setCPH(item);
                        strCPHS = strCPHS == "" ? carIssue.getCPH() : strCPHS + "," + carIssue;

                        List<EntityCardIssue> lstCI = MonitorRemoteRequest.GetAutoCardNo(Model.token);
                        if (lstCI.size() > 0)
                        {
                            int cardnomax = Integer.parseInt(lstCI.get(0).getCardNO().substring(2, 2 + 6));
                            carIssue.setCardNO("88" + CommUtils.stringPadLeft(String.valueOf(cardnomax + 1), 6, '0'));
                        }
                        else
                        {
                            carIssue.setCardNO("88000001");
                        }


                        boolean b = MonitorRemoteRequest.IfCardNOExitsbt(Model.token, item);
                        if (b)
                        {
                            mHandler.post(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    T.showShort(ParkingMonitoringActivity.this, "车牌已重复是否覆盖添加"); // 需要对话框显示是否覆盖
                                }
                            });
                        }

                        String carType = parkingPlateRegisterView.GetCarType(0);
                        L.i("carType:" + carType);
                        //判断车牌是否是已入场的临时车,如果是,则把入场记录改为发行卡类  2015-12-26  2016-06-21
                        if (carType.substring(0, 3 - 0).equals("Mth")
                                || carType.substring(0, 3 - 0).equals("Fre")
                                || carType.substring(0, 3 - 0).equals("Str"))
                        {
                            int value = MonitorRemoteRequest.GetInRecordIsTmp(Model.token, item);
                            if (value > 0)
                            {// 确认三次
//                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
//                                {
//                                    return;
//                                }
//                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【再次】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
//                                {
//                                    return;
//                                }
//                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【最后】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
//                                {
//                                    return;
//                                }
                            }
                        }


                    }
                }
            }).start();
        }

        @Override
        public void onPlateRegisterSaveNormal(final EntityCardIssue carIssue)
        {
            new Thread(new Runnable()
            {
                @Override
                public void run()
                {
                    //1,判断卡片号码是否重复  2016-06-28 th
                    boolean value = MonitorRemoteRequest.IfCardNOExitsbt(Model.token, carIssue.getCardNO());
                    if (value)
                    {
                        mHandler.post(new Runnable()
                        {
                            @Override
                            public void run()
                            {
                                T.showShort(ParkingMonitoringActivity.this, "车辆编号已重复是否覆盖添加！\n\n【" + carIssue.getCardNO() + "】");
                            }
                        });
                    }

                    //2,判断车牌是否重复
                    if (MonitorRemoteRequest.IfCPHExitsbt(Model.token, carIssue.getCPH()))
                    {
                        mHandler.post(new Runnable()
                        {
                            @Override
                            public void run()
                            {
                                T.showShort(ParkingMonitoringActivity.this, "车牌已重复是否覆盖添加！\n\n【" + carIssue.getCPH() + "】");
                            }
                        });
                    }
                    String carType = parkingPlateRegisterView.GetCarType(0);
                    L.i("carType:" + carType + "," + carIssue.getCPH());
                    //判断车牌是否是已入场的临时车,如果是,则把入场记录改为发行卡类  2015-12-26  2016-06-21
                    if (carType.substring(0, 3 - 0).equals("Mth")
                            || carType.substring(0, 3 - 0).equals("Fre")
                            || carType.substring(0, 3 - 0).equals("Str"))

                    {
                        int iRet = MonitorRemoteRequest.GetInRecordIsTmp(Model.token, carIssue.getCPH());
                        if (iRet > 0)
                        {
//                            if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
//                            {
//                                return;
//                            }
//                            if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【再次】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
//                            {
//                                return;
//                            }
//                            if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【最后】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
//                            {
//                                return;
//                            }
                        }
                        /**
                         * 有入场记录，//则把入场记录改为发行卡类
                         */
                        MonitorRemoteRequest.UpdateInCPHCardType(Model.token, carIssue.getCPH(), parkingPlateRegisterView.GetCarType(0), carIssue.getCardNO());
                        MonitorRemoteRequest.AddOptLog(Model.token, "车牌登记", carIssue.getCPH() + " 已入场临时车，改为" + carIssue.getCardType()
                                , Model.sUserCard, Model.sUserName, String.valueOf(Model.stationID));
                        carIssue.setID(0);
                    }

                    if (parkingPlateRegisterView.getCheckAutoCarNoIsChecked())
                    {
                        EntityUserInfo entityUserInfo = parkingPlateRegisterView.GetPersonnel();
                        if (entityUserInfo == null)
                        {
                            return;
                        }

                        MonitorRemoteRequest.PersonnelAddCpdj(Model.token, entityUserInfo, MonitorRemoteRequest.IsExsits(Model.token, entityUserInfo.getUserNO()));
                    }

                    if (carIssue.getID() != 0)
                    {
                        MonitorRemoteRequest.UpdateCPdjfx(Model.token, carIssue);
                    }
                    else
                    {
                        MonitorRemoteRequest.DeleteFaXing(Model.token, carIssue.getCardNO());

                        EntityMoney money = new EntityMoney();
                        money.setCardNO(carIssue.getCardNO());
                        money.setOptDate(TimeConvertUtils.longToString(System.currentTimeMillis()));
                        money.setOperatorNO(Model.sUserCard);
                        money.setSFJE(carIssue.getBalance());
                        money.setOptType("1");

                        money.setNewStartDate(carIssue.getCarValidStartDate());
                        money.setNewEndDate(carIssue.getCarValidEndDate());
                        money.setLastEndDate(carIssue.getCarValidStartDate());

                        MonitorRemoteRequest.AddMoney(Model.token, money);

                        if (carIssue.getCardYJ() > 0)
                        {
                            money.setSFJE(carIssue.getCardYJ());
                            money.setOptType("E");
                            MonitorRemoteRequest.AddMoney(Model.token, money);
                        }

                        //2015-07-29
                        if (carIssue.getCarCardType().equals("Opt"))
                        {
                            MonitorRemoteRequest.AddOperator(Model.token, carIssue);//插入操作员表
                        }

                        if (carIssue.getCarCardType().substring(0, 3 + 0).equals("Str"))
                        {
                            carIssue.setBalance(0);
                        }

                        MonitorRemoteRequest.Addchdj(Model.token, carIssue);

                        // 更新界面数据重新获取数据即可
                        final ArrayList<HashMap<String, String>> hashMaps = dealDataGridView();
                        mHandler.post(new Runnable()
                        {
                            @Override
                            public void run()
                            {
                                parkingPlateRegisterView.setGridViewData(hashMaps);
                                T.showShort(ParkingMonitoringActivity.this, "车牌添加成功!");
                                parkingPlateRegisterView.setUIEnableWhenAddSuccess();
                            }
                        });

                        MonitorRemoteRequest.AddOptLog(Model.token, "车牌登记", carIssue.getCPH() + " 车牌添加成功---车辆号码:"
                                , Model.sUserCard, Model.sUserName, String.valueOf(Model.stationID));
                    }
                }
            }).start();
        }

        /**
         * 点击了增加按钮
         */
        @Override
        public void onPlateRegisterAddBtn()
        {
            showCarTextByNetRequest();//重新获取编号
            showUserNoByNetRequest();
        }

        private void showCarTextByNetRequest()
        {
            new PlateRegisterAsyncTask(parkingPlateRegisterView)
            {
                @Override
                public Object onExecuteExpectedTask() // 开始执行
                {
                    String objectText = "";
                    List<EntityCardIssue> EntityCardIssues = MonitorRemoteRequest.GetAutoCardNo(Model.token);
                    if (EntityCardIssues.size() > 0)
                    {
                        int max = Integer.parseInt(EntityCardIssues.get(0).getCardNO().substring(2, 2 + 6));
                        objectText = ParkingPlateRegisterView.CONST_CAR_NO_PREFIX + CommUtils.stringPadLeft(String.valueOf(max + 1), 6, '0');
                    }
                    else
                    {
                        objectText = ParkingPlateRegisterView.CONST_CAR_NO;
                    }
                    return objectText;
                }

                @Override
                public void onEndExecuteTask(Object o)
                {
                    parkingPlateRegisterView.setCarNoText((String) o);
                }
            }.execute("获取车辆自动编号自动-_- -_-");
        }

        private void showUserNoByNetRequest()
        {
            new PlateRegisterAsyncTask(parkingPlateRegisterView)
            {
                @Override
                public Object onExecuteExpectedTask() // 开始执行
                {
                    String objectText = "";
                    List<EntityUserInfo> entityUserInfo = MonitorRemoteRequest.GetAutoUsernoPersonnel(Model.token);
                    if (entityUserInfo.size() > 0)
                    {
                        int max = Integer.parseInt(entityUserInfo.get(0).getUserNO().substring(1, 1 + 5));
                        objectText = ParkingPlateRegisterView.CONST_USER_NO_PREFIX + CommUtils.stringPadLeft(String.valueOf(max + 1), 5, '0');
                    }
                    else
                    {
                        objectText = ParkingPlateRegisterView.CONST_USER_NO;
                    }
                    return objectText;
                }

                @Override
                public void onEndExecuteTask(Object o)
                {
                    parkingPlateRegisterView.setUserNoText((String) o);
                }
            }.execute("获取人员自动编号自动-_- -_-");

        }
    }

    /**
     * 从 Model rights中获取指定的权限
     *
     * @param title
     * @param itemName
     * @return
     */
    public List<GetRightsByGroupIDResp.DataBean> GetRightsByName(String title, String itemName)
    {
        if (Model.lstRights == null || Model.lstRights.size() == 0)
        {
            return null;
        }

        List<GetRightsByGroupIDResp.DataBean> temp = new ArrayList<GetRightsByGroupIDResp.DataBean>();
        for (GetRightsByGroupIDResp.DataBean val : Model.lstRights)
        {
            if (val.getFormName().equals(title) && val.getItemName().equals(itemName))
            {
                temp.add(val);
            }
        }
        return temp;
    }

    class QueueTask extends Thread
    {
        private boolean taskFlag;

        public QueueTask(boolean runningFlag)
        {
            taskFlag = runningFlag;
        }

        @Override
        public void run()
        {
            try
            {
                while (taskFlag)
                {
                    final ModelNode modelNode = ConcurrentQueueHelper.getInstance().get();
                    if (modelNode != null)
                    {
                        L.i("###　接受到车牌数据:" + modelNode.toString());
                        // 显示车牌，然后更新图像数据即可
                        switch (modelNode.type)
                        {
                            case CAR_IN_TYPE_auto: // 手动输入车牌
                                if (requestUrlUpdateUiWhenSetIn((SetCarInReq) modelNode.data) >= 0)
                                {
                                    String strFileJpg = modelNode.getStrFileJpg();
                                    final Bitmap bitmap = BitmapUtils.fileToBitmap(strFileJpg);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap);
                                        }
                                    });
                                }
                                break;
                            case CAR_IN_TYPE_auto_noPlate:// 无牌车手动输入车牌
                                if (requestUrlRequestWhenNoPlateIn((SetCarInWithoutCPHReq) modelNode.data) >= 0)
                                {
                                    String strFileJpg = modelNode.getStrFileJpg();
                                    final Bitmap bitmap = BitmapUtils.fileToBitmap(strFileJpg);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap);
                                        }
                                    });
                                }
                                break;
                            case CAR_IN_TYPE_recognition:// 相机识别车牌
                                SetCarInReq setCarInReq = new SetCarInReq(); // 发送进场数据
                                setCarInReq.setCPH(modelNode.getStrCPH());
                                setCarInReq.setToken(Model.token);
                                setCarInReq.setCtrlNumber(getCtrlNumber(CAR_CHANNEL_IN));
                                setCarInReq.setStationId(Model.stationID);
                                if (requestUrlUpdateUiWhenSetIn(setCarInReq) >= 0)
                                {
                                    String strFileJpg = modelNode.getStrFileJpg();
                                    final Bitmap bitmap = BitmapUtils.fileToBitmap(strFileJpg);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap);
                                        }
                                    });
                                }
                                break;
                            default:
                                break;
                        }

                    }
                    Thread.sleep(30);
                }
            }
            catch (Exception ex)
            {
                ex.printStackTrace();
                L.i("结束：" + ex.toString());
            }
        }

        public void end()
        {
            taskFlag = false;
            try
            {
                queueTask.join();
            }
            catch (InterruptedException e)
            {
                e.printStackTrace();
            }
        }
    }

    /**
     * 更新左侧画面的数据
     *
     * @param index  表示那个画面
     * @param CPH    表示车牌
     * @param bitmap 表示图像数据
     */
    private void dealCPHInfoFromCamera(int index, String CPH, Bitmap bitmap)
    {
        parkingMonitoringView.setCPHText(index, CPH);
        parkingMonitoringView.setInPicture(index, bitmap);
    }

    @Override
    protected void onRestart()
    {
        super.onRestart();
    }


    // 把camera信息放到相应的列表中
    private List<GetNetCameraSetResp> netCameraList = new ArrayList<GetNetCameraSetResp>();

    private boolean requestGetNetCameraSet(String cameraIP)
    {
        GetNetCameraSetReq req = new GetNetCameraSetReq();
        req.setToken(Model.token);
        req.setJsonSearchParam(JsonSearchParam.getWhenGetCameraSet(cameraIP));
        GetNetCameraSetResp getNetCameraSetResp = GetServiceData.getInstance().GetNetCameraSet(req);
        if (getNetCameraSetResp != null)
        {
            netCameraList.add(getNetCameraSetResp); // 一个个放进去
            return true;
        }
        else
        {//防止出现错的情况，还是存放一个空的
            netCameraList.add(new GetNetCameraSetResp());
            return false;
        }
    }

    private boolean requestGetCheDaoSet()
    {
        GetCheDaoSetReq req = new GetCheDaoSetReq();
        req.setToken(Model.token);
        req.setJsonSearchParam(JsonSearchParam.getWhenGetCheDaoSet(String.valueOf(Model.stationID)));
        req.setOrderField(OrderField.getWhenGetCheDaoSet("desc", "asc", "asc"));
        getCheDaoSetResp = GetServiceData.getInstance().GetCheDaoSet(req);

        if (getCheDaoSetResp == null || getCheDaoSetResp.getData() == null)
        {
            return false;
        }

        Model.iChannelCount = getCheDaoSetResp.getData().size();
        return true;
    }

}



