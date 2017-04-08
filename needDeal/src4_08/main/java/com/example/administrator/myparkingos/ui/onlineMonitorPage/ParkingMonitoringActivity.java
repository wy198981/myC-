package com.example.administrator.myparkingos.ui.onlineMonitorPage;

import android.graphics.Bitmap;
import android.graphics.Paint;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.text.TextPaint;
import android.text.TextUtils;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.constant.ColumnName;
import com.example.administrator.myparkingos.constant.GlobalParams;
import com.example.administrator.myparkingos.constant.JsonSearchParam;
import com.example.administrator.myparkingos.constant.OrderField;
import com.example.administrator.myparkingos.constant.RCode;
import com.example.administrator.myparkingos.constant.RCodeDeal;
import com.example.administrator.myparkingos.model.GetServiceData;
import com.example.administrator.myparkingos.model.ModelNode;
import com.example.administrator.myparkingos.model.MonitorRemoteRequest;
import com.example.administrator.myparkingos.model.RequestByURL;
import com.example.administrator.myparkingos.model.beans.BlackListOpt;
import com.example.administrator.myparkingos.model.beans.Model;
import com.example.administrator.myparkingos.model.beans.gson.EntityAddLog;
import com.example.administrator.myparkingos.model.beans.gson.EntityBlackList;
import com.example.administrator.myparkingos.model.beans.gson.EntityCarTypeInfo;
import com.example.administrator.myparkingos.model.beans.gson.EntityCardIssue;
import com.example.administrator.myparkingos.model.beans.gson.EntityMoney;
import com.example.administrator.myparkingos.model.beans.gson.EntityParkJHSet;
import com.example.administrator.myparkingos.model.beans.gson.EntityPersonnelInfo;
import com.example.administrator.myparkingos.model.beans.gson.EntityUserInfo;
import com.example.administrator.myparkingos.model.requestInfo.AddOptLog;
import com.example.administrator.myparkingos.model.requestInfo.AddOptLogReq;
import com.example.administrator.myparkingos.model.requestInfo.CaclChargeAmountReq;
import com.example.administrator.myparkingos.model.requestInfo.CancelChargeReq;
import com.example.administrator.myparkingos.model.requestInfo.GetCarInReq;
import com.example.administrator.myparkingos.model.requestInfo.GetCarOutReq;
import com.example.administrator.myparkingos.model.requestInfo.GetCardIssueReq;
import com.example.administrator.myparkingos.model.requestInfo.GetCheDaoSetReq;
import com.example.administrator.myparkingos.model.requestInfo.GetNetCameraSetReq;
import com.example.administrator.myparkingos.model.requestInfo.GetParkingInfoReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInConfirmReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInWithoutCPHReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarOutReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarOutWithoutEntryRecordReq;
import com.example.administrator.myparkingos.model.requestInfo.UpdateChargeAmountReq;
import com.example.administrator.myparkingos.model.requestInfo.UpdateChargeInfoReq;
import com.example.administrator.myparkingos.model.requestInfo.UpdateChargeWithCaptureImageReq;
import com.example.administrator.myparkingos.model.responseInfo.CancelChargeResp;
import com.example.administrator.myparkingos.model.responseInfo.GetCarInResp;
import com.example.administrator.myparkingos.model.responseInfo.GetCarOutResp;
import com.example.administrator.myparkingos.model.responseInfo.GetCardIssueResp;
import com.example.administrator.myparkingos.model.responseInfo.GetCheDaoSetResp;
import com.example.administrator.myparkingos.model.responseInfo.GetNetCameraSetResp;
import com.example.administrator.myparkingos.model.responseInfo.GetParkingInfoResp;
import com.example.administrator.myparkingos.model.responseInfo.GetRightsByGroupIDResp;
import com.example.administrator.myparkingos.model.responseInfo.GetSysSettingObjectResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInConfirmResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInWithoutCPHResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarOutResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarOutWithoutEntryRecordResp;
import com.example.administrator.myparkingos.model.responseInfo.UpdateChargeAmountResp;
import com.example.administrator.myparkingos.model.responseInfo.UpdateChargeInfoResp;
import com.example.administrator.myparkingos.model.responseInfo.UpdateChargeWithCaptureImageResp;
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
import com.google.gson.Gson;
import com.vz.tcpsdk;

import org.w3c.dom.Text;

import java.net.URLEncoder;
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

    // 请求数据
    private GetCheDaoSetResp getCheDaoSetResp;
    private GetCarOutResp getCarOutResp;
    private GetCarInResp getCarInResp;
    private GetParkingInfoResp getParkingInfoResp;
    private SetCarOutResp setCarOutResp;
    private ParkingChannelSelectView parkingChannelSelectView;

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
                mHandler.sendEmptyMessage(MSG_GetCarIn);
            }

            @Override
            public void chargeDetailToFragmentChange()
            {
                fragmentDetailManager.showFragment(1);
                mHandler.sendEmptyMessage(MSG_GetCarOut);
            }

            /**
             * 点击车入场按钮
             */
            @Override
            public void onClickCarInBtn()
            {
                // 当面对多个通道时，出现通道的选择
                if (inChannelNum >= 2)
                {
                    if (parkingChannelSelectView == null)
                    {
                        parkingChannelSelectView = new ParkingChannelSelectView(ParkingMonitoringActivity.this, CAR_CHANNEL_IN)
                        {
                            @Override
                            public void onSelectInOutName(String currentText)
                            {
                                if (checkCheDaoSetRespDataInvalid()) return; // 获取具体选中 13

                                int channelIndex = getCtrlIndexByInoutName(currentText);
                                if (channelIndex < 0)
                                    L.i("getCtrlIndexByInoutName" + currentText + "no find");
                                else
                                    popuCarInDialog(channelIndex, getCheDaoSetResp.getData().get(CheDaoIndex[channelIndex]).getCtrlNumber());
                            }

                            @Override
                            public void prepareLoadData()
                            {
                                if (checkCheDaoSetRespDataInvalid()) return;
                                List<String> data = getInOutListNameByType(CAR_CHANNEL_IN);
                                parkingChannelSelectView.setSpinnerData(data);
                            }
                        };
                    }
                    parkingChannelSelectView.show();
                }
                else if (inChannelNum == 1)
                {
                    int channelIndex = getChannelIndex(CAR_CHANNEL_IN);
                    if (channelIndex < 0)
                        L.i("getChannelIndex " + CAR_CHANNEL_IN + " no find");
                    else
                        popuCarInDialog(channelIndex, getCheDaoSetResp.getData().get(CheDaoIndex[channelIndex]).getCtrlNumber());
                }
                else
                {
                    T.showShort(ParkingMonitoringActivity.this, "没有车辆入场通道");
                }
            }

            /**
             * 弹出车辆进场的画面
             */
            private void popuCarInDialog(final int index, final int ctrlNumber)
            {
                if (CarInDialog == null)
                {
                    CarInDialog = new ParkingPlateNoInputView(ParkingMonitoringActivity.this, CAR_CHANNEL_IN)
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
                            SetCarInReq setCarInReq = initSetCarIn(CPH, ctrlNumber);
                            sendModeToQueue(ModelNode.E_CarInOutType.CAR_IN_TYPE_auto, CPH, setCarInReq, index);
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
                                    final GetCardIssueResp respList = requestGetCarIssueWhenCPH_Like(cph);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            ArrayList<String> strings = new ArrayList<>();
                                            if (respList.getData() != null && respList.getData().size() > 0)
                                            {
                                                for (GetCardIssueResp.DataBean o : respList.getData())
                                                {
                                                    strings.add(o.getCPH());
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
             * 点击出场按钮
             */
            @Override
            public void onClickCarOutBtn()
            {
                if (outChannelNum >= 2)
                {
                    if (parkingChannelSelectView == null)
                    {
                        parkingChannelSelectView = new ParkingChannelSelectView(ParkingMonitoringActivity.this, CAR_CHANNEL_OUT)
                        {
                            @Override
                            public void onSelectInOutName(String currentText)
                            {
                                if (checkCheDaoSetRespDataInvalid()) return; // 获取具体选中 13

                                int channelIndex = getCtrlIndexByInoutName(currentText);
                                if (channelIndex < 0)
                                    L.i("getCtrlIndexByInoutName" + currentText + "no find");
                                else
                                    popuSetCarOut(channelIndex, getCheDaoSetResp.getData().get(CheDaoIndex[channelIndex]).getCtrlNumber());
                            }

                            @Override
                            public void prepareLoadData()
                            {
                                if (checkCheDaoSetRespDataInvalid()) return;
                                List<String> data = getInOutListNameByType(CAR_CHANNEL_OUT);
                                parkingChannelSelectView.setSpinnerData(data);
                            }
                        };
                    }
                    parkingChannelSelectView.show();
                }
                else if (outChannelNum == 1)
                {
                    int channelIndex = getChannelIndex(CAR_CHANNEL_OUT);
                    if (channelIndex < 0)
                        L.i("getChannelIndex " + CAR_CHANNEL_OUT + " no find");
                    else
                        popuSetCarOut(channelIndex, getCheDaoSetResp.getData().get(CheDaoIndex[channelIndex]).getCtrlNumber());
                }
                else
                {
                    T.showShort(ParkingMonitoringActivity.this, "没有车辆出场通道");
                }
            }

            /**
             * 弹出车辆出场画面
             * @param index
             * @param ctrlNumber
             */
            private void popuSetCarOut(final int index, final int ctrlNumber)
            {
                if (CarOutDialog == null)
                {
                    CarOutDialog = new ParkingPlateNoInputView(ParkingMonitoringActivity.this, CAR_CHANNEL_OUT)
                    {
                        @Override
                        public void prepareLoadData()
                        {
                            super.prepareLoadData();
                            CarOutDialog.setProvince(Model.LocalProvince);
                        }

                        @Override
                        public void onCarOutBtnOk(final String CPH) // 车辆出场，直接发送出场消息
                        {
                            SetCarOutReq req = initSetCarOutReq(CPH, ctrlNumber);
                            sendModeToQueue(ModelNode.E_CarInOutType.CAR_OUT_TYPE_auto, CPH, req, index);
                            CarOutDialog.dismiss();
                        }

                        @Override
                        protected void onBtnCancel()
                        {
                            CarOutDialog.dismiss();
                        }

                        @Override
                        public void onClickOutTextInput(final String resultCPH, int length)
                        {
                            new Thread(new Runnable()
                            {
                                @Override
                                public void run()
                                {
                                    final GetCarInResp getCarInResp = requestSelectComeCPH_Like(resultCPH);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            ArrayList<String> strings = new ArrayList<>();
                                            if (getCarInResp != null && getCarInResp.getData() != null && getCarInResp.getData().size() > 0)
                                            {
                                                for (GetCarInResp.DataBean o : getCarInResp.getData())
                                                {
                                                    strings.add(o.getCPH());
                                                }
                                                CarOutDialog.showPopWindow();
                                                CarOutDialog.setCompleteCPHText(strings);
                                            }
                                        }
                                    });
                                }
                            }).start();

                        }
                    };
                }
                CarOutDialog.show();
            }

            /**
             * 点击无牌车入场按钮
             */
            @Override
            public void onClickNoPlateCarInBtn()
            {
                if (inChannelNum >= 2)
                {
                    if (parkingChannelSelectView == null)
                    {
                        parkingChannelSelectView = new ParkingChannelSelectView(ParkingMonitoringActivity.this, CAR_CHANNEL_IN)
                        {
                            @Override
                            public void onSelectInOutName(String currentText)
                            {
                                if (checkCheDaoSetRespDataInvalid()) return;

                                int channelIndex = getCtrlIndexByInoutName(currentText);
                                if (channelIndex < 0)
                                    L.i("getCtrlIndexByInoutName" + currentText + "no find");
                                else
                                    popuNoPlateSetCarIn(channelIndex, getCheDaoSetResp.getData().get(CheDaoIndex[channelIndex]).getCtrlNumber());
                            }

                            @Override
                            public void prepareLoadData()
                            {
                                if (checkCheDaoSetRespDataInvalid()) return;
                                List<String> data = getInOutListNameByType(CAR_CHANNEL_IN);
                                parkingChannelSelectView.setSpinnerData(data);
                            }
                        };
                    }
                    parkingChannelSelectView.show();
                }
                else if (inChannelNum == 1)
                {
                    int channelIndex = getChannelIndex(CAR_CHANNEL_IN);
                    if (channelIndex < 0)
                        L.i("getChannelIndex " + CAR_CHANNEL_IN + " no find");
                    else
                        popuNoPlateSetCarIn(channelIndex, getCheDaoSetResp.getData().get(CheDaoIndex[channelIndex]).getCtrlNumber());
                }
                else
                {
                    T.showShort(ParkingMonitoringActivity.this, "没有车辆入场通道");
                }
            }

            /**
             * 弹出无牌车入场界面
             */
            private void popuNoPlateSetCarIn(final int index, final int ctrlNumber)
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
                            carInWithoutCPHReq.setCtrlNumber(ctrlNumber);

                            String CPH = carInWithoutCPHReq.getCPH();
                            sendModeToQueue(ModelNode.E_CarInOutType.CAR_IN_TYPE_auto_noPlate, CPH, carInWithoutCPHReq, index);
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
                            if (checkCheDaoSetRespDataInvalid()) return;

                            List<String> data = getInOutListNameByType(CAR_CHANNEL_IN);
                            parkingInNoPlateView.setRoadNameData(data);//显示下拉列表

                            String fileName = SDCardUtils.getSDCardPath() + "picture1.jpg";
                            L.i("获取到的fileName:" + fileName);
                            parkingMonitoringView.saveImage(fileName, index);
                            parkingInNoPlateView.setImage(fileName); //显示图像
                            parkingInNoPlateView.cleanCarNo();
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

        tcpsdk.getInstance().setup();
        initView(savedInstanceState);

        initFields();
        initControl();

        startAliveTime = System.currentTimeMillis();
        mHandler.sendEmptyMessage(MSG_KeppAlive);

        exe = new ExeUtil();
        queueTask = new QueueTask(true);
        queueTask.start();
    }

    private int getCtrlIndexByInoutName(String currentText)
    {
        int resultValue = -1;
        for (int i = 0; i < CheDaoIndex.length; i++)
        {
            int tempI = CheDaoIndex[i]; // CheDaoIndex 重新整理的通道的下标
            if (getCheDaoSetResp.getData().get(tempI).getInOutName().equals(currentText))
            {
                resultValue = i;
                break;
            }
        }
        return resultValue;
    }

    @NonNull
    private List<String> getInOutListNameByType(int type)
    {
        if (checkCheDaoSetRespDataInvalid()) return null;
        List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();

        ArrayList<String> strings = new ArrayList<>();
        for (int i = 0; i < CheDaoIndex.length; i++)
        {
            int tempI = CheDaoIndex[i];
            if (data.get(tempI).getInOut() == type)
            {
                strings.add(data.get(tempI).getInOutName());
            }
        }
        return strings;
    }

    private SetCarOutReq initSetCarOutReq(String cph, int ctrlNumber)
    {
        SetCarOutReq req = new SetCarOutReq(); // 发送进场数据
        req.setCPH(cph);
        req.setToken(Model.token);
        req.setCtrlNumber(ctrlNumber);
        req.setStationId(Model.stationID);
        return req;
    }

    private void sendModeToQueue(ModelNode.E_CarInOutType type, String cph, Object data, int index)
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

    private int getChannelIndex(int type)
    {
        if (checkCheDaoSetRespDataInvalid()) return -1;
        List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();
        int resultValue = -1;
        for (int i = 0; i < CheDaoIndex.length; i++)
        {
            int tempI = CheDaoIndex[i]; // CheDaoIndex 重新整理的通道的下标
            if (data.get(tempI).getInOut() == type)
            {
                resultValue = i;
                break;
            }
        }
        return resultValue;
    }

    private void initView(@Nullable Bundle savedInstanceState)
    {
        // 初始化按钮颜色
        parkingMonitoringView.onClickInCarChargeInfo();
        parkingMonitoringView.onClickInCarInParkingDetail();

        // 初始化fragment
        initFragment(null); // 这个地方还是影响！！！
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
     * 预先检测字符串
     *
     * @return
     */
    private String prepareDetectString(String srcStr, String defaultStr)
    {
        if (TextUtils.isEmpty(srcStr))
        {
            return defaultStr;
        }
        return srcStr;
    }

    /**
     * 根据车辆进场，提示发送语音数据
     * 注意返回数据的车牌和原始的车牌号可能不同
     *
     * @param setCarInResp
     */
    private void dealSetCarInResponse(final SetCarInResp setCarInResp, final String srcCPH, final int index)
    {
        RCode rCode = RCode.valueOf(Integer.parseInt(setCarInResp.getRcode()));
        final SetCarInResp.DataBean data = setCarInResp.getData();
        switch (rCode)
        {
            case BlackList:
            {
                /**
                 * 1， 右上角显示警告信息
                 */
                updateCarHintToFragment(MSG_CarHintInfoAfterResume
                        , prepareDetectString(data.getCPH(), srcCPH) + ":" + prepareDetectString(data.getBlackReason(), ""));
                /**
                 * 2，发送语音
                 */
                break;
            }
            case NoThisLanePermission:
            {
                /**
                 * 1，右上角显示警告信息
                 */
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "在 " + data.getCtrlNumber() + " 号机上无权限！");

                /**
                 * 2, 右下角显示操作信息
                 */
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo(data.getCtrlNumber() + "号机上无权限!");
                    }
                });
                /**
                 * 发送语音
                 */
                break;
            }
            case BeOverdue:
            {
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setCPHText(index, prepareDetectString(data.getCPH(), srcCPH));
                        parkingMonitoringView.setOperatorHintInfo("已过期，请到管理处延期!");
                    }
                });
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "已过期，请到管理处延期!");
                /**
                 * 发送语音
                 */
                break;
            }
            case PersonalFull:
            {
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("此车禁止入场，入场车辆数已经超过车位个数");
                    }
                });
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "此车禁止入场，入场车辆数已经超过车位个数。");
                /**
                 * 发送语音
                 */
                break;
            }
            case ProhibitCurrent:
            {
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "此车已入场[" + data.getInOutName() + "]");
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("此车已入场[" + data.getInOutName() + "]");
                    }
                });
                /**
                 * 发送语音
                 */
                break;
            }
            case ProhibitCutOff:
            {
                /**
                 * 发送语音
                 */
                if (getCheDaoSetResp.getData().get(CheDaoIndex[index]).getInOut() == CAR_CHANNEL_IN)
                {

                }
                break;
            }
            case SummaryCarFull:
            {
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "车位已满!");
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("车位已满!");
                    }
                });

                /**
                 * 发送语音
                 */
                break;
            }
            case TemporaryCarFull:
            {
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "临时车位已满!");
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("临时车位已满!");
                    }
                });

                /**
                 * 发送语音
                 */
                break;
            }
            case MonthCarFull:
            {
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "月租车位已满!");
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("月租车位已满!");
                    }
                });

                /**
                 * 发送语音
                 */
                break;
            }
            case PrepaidCarFull:
            {
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "储值车位已满!");
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("储值车位已满!");
                    }
                });
                break;
            }
            case BalanceNotEnough:
            {
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "余额不足，请先充值!");
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("余额不足，请先充值!");
                    }
                });

                /**
                 * 发送语音
                 */
                break;
            }
            case AllCharacterSamePlateNoHandle:
            {
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("字符相同的车牌不处理!");
                    }
                });
                break;
            }
            case AllLetterPlateNoHandle:
            {
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("全字母车牌不处理!");
                    }
                });
                break;
            }
            case TemporaryCarNotInSmall:
            {
                updateCarHintToFragment(MSG_CarHintInfoAfterResume, "临时车禁止驶入小车场!");
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        parkingMonitoringView.setOperatorHintInfo("临时车禁止驶入小车场!");
                    }
                });
                /**
                 * 发送语音
                 */
                AddOptLogReq req = new AddOptLogReq();
                req.setToken(Model.token);
                req.setJsonModel(getAddOptLogText("在线监控:FillOutData", "临时车禁止驶入小车场" + data.getCardNO()));
                GetServiceData.getInstance().AddOptLog(req);
//                gsd.AddLog("在线监控:FillOutData", "临时车禁止驶入小车场" + approachResult.Model.CardNO);
                break;
            }
            case ConfirmCutOff:
            {
                // 判断是否放行，发送开闸指令，结束
                // 弹出界面后，然后是否放行
                mHandler.post(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        // 弹出界面
                        if (parkingTempCPHView == null)
                        {
                            parkingTempCPHView = new ParkingTempCPHView(ParkingMonitoringActivity.this)
                            {
                                @Override
                                public void onClickOk(final SetCarInConfirmReq setCarInConfirmReq, final String inOutName)
                                {
                                    new Thread(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            final SetCarInConfirmResp setCarInConfirmResp = requestSetCarInConfirm(setCarInConfirmReq, srcCPH, inOutName);
                                            if (setCarInConfirmResp == null) return;

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
                                                    dealCPHInfoFromCamera(channelIndex, setCarInConfirmResp.getData().getCPH(), bitmap, CAR_CHANNEL_IN);
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
                                    String inOutName = setCarInResp.getData().getInOutName();
                                    List<String> inOutListNameByType = getInOutListNameByType(CAR_CHANNEL_IN);

                                    int selectIndex = 0;
                                    for (int i = 0; i < inOutListNameByType.size(); i++)
                                    {
                                        if (inOutListNameByType.get(i).equals(inOutName))
                                        {
                                            selectIndex = i;
                                            break;
                                        }
                                    }
                                    parkingTempCPHView.setSpinnerRoadName(inOutListNameByType, selectIndex);
                                }
                            };
                        }
                        parkingTempCPHView.show();
                    }
                });
                break;
            }
            case MonthCarFullConfirmCutOff:
            {
                break;
            }
            case TemporaryCarFulllConfirmCutOff:
            {
                break;
            }
            case PrepaidCarFullConfirmCutOff:
            {
                break;
            }
            case SummaryCarFullConfirmCutOff:
            {
                break;
            }
            case MthBeOverdueToTmpCharge:
            {
                break;
            }
            case MthFullToTmpCharge:
            {
                break;
            }
            case RepeatAdmission:
            {
                break;
            }
            case OK:
            {
                break;
            }
            default:
                L.i("SetCarInResp 返回信息:" + setCarInResp.getMsg());
                break;
        }
    }

    private String getAddOptLogText(String menu, String content)
    {
        AddOptLog opt = new AddOptLog();
        opt.setOptNO(Model.sUserCard);
        opt.setUserName(Model.sUserName);
        opt.setOptMenu(menu);
        opt.setOptContent(content);
        opt.setOptTime(TimeConvertUtils.longToString(System.currentTimeMillis()));
        opt.setStationID(Model.stationID);

        Gson gson = new Gson();
        return URLEncoder.encode(gson.toJson(opt));
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


    private int[] CheDaoIndex; // 按照一定的规则将重新组织视频画面的显示
    private int inChannelNum = 0;// 表示当前入口车道的个数
    private int outChannelNum = 0;// 表示当前出口车道的个数

    /**
     * 重新组织下车道下标
     *
     * @param data
     * @param size
     */
    public void orderCheDaoIndex(List<GetCheDaoSetResp.DataBean> data, int size)
    {
        CheDaoIndex = new int[size]; // CheDaoIndex存放着视频画面对应的下标
        int tempIndex = 0;
        for (int i = 0; i < size; i++)
        {
            /**
             * 1,先找入场的通道下标放到数组中;
             */
            if (data.get(i).getInOut() == CAR_CHANNEL_IN)
            {
                CheDaoIndex[tempIndex] = i;
                tempIndex++;
                inChannelNum++;
            }
        }

        for (int i = 0; i < size; i++)
        {
            /**
             * 2,再找出口的通道下标放到数组中;
             */
            if (data.get(i).getInOut() == CAR_CHANNEL_OUT)
            {
                CheDaoIndex[tempIndex] = i;
                tempIndex++;
                outChannelNum++;
            }
        }

        L.i("orderCheDaoIndex" + "inChannelNum:" + inChannelNum + ",outChannelNum:" + outChannelNum);

        if (tempIndex != size)
        {
            L.e("reorderCheDaoIndex", "数据出现问题");
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
 *         怎么逻辑封装
 *              1,影响因子，总通道数，通道数 > 2,则不显示图片；
 *              2,优先显示入场视频；
 */
                L.i("getCheDaoSetResp:" + getCheDaoSetResp.getData().size());
                playVideoIn2Video();
//                if (getCheDaoSetResp.getData().size() <= 2)
//                {
//                    playVideoIn2Video();
//                }
//                else if (getCheDaoSetResp.getData().size() > 2)
//                {
//                    playVideoInGreaterThan2Video();
//                }
            }
        }).start();
    }

    /**
     * 在3路 ~4路的车辆车道
     */
    private void playVideoInGreaterThan2Video()
    {
        if (checkCheDaoSetRespDataInvalid()) return;
        final List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();
        int getDataSize = -1;
        if (data.size() > 4)
        {
            orderCheDaoIndex(data, 4);
            getDataSize = 4;
        }
        else
        {
            orderCheDaoIndex(data, data.size());
            getDataSize = data.size();
        }

        for (int i = 0; i < getDataSize; i++)
        {
            final GetCheDaoSetResp.DataBean dataBean = data.get(CheDaoIndex[i]);
            final int tempI = i;
            final int totalSize = getDataSize;
            mHandler.post(new Runnable()
            {
                @Override
                public void run()
                {
                    parkingMonitoringView.setChannelText(tempI, dataBean.getInOutName());
                }
            });
            if (requestGetNetCameraSet(dataBean.getCameraIP()))
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
                            L.i("videoType:" + videoType + "i:" + i);
                            parkingMonitoringView.playVideoByIndex(i, data1.get(0).getVideoIP(), mHandler);
                            break;
                        }
                    }
                }
            }
        }
    }

    /**
     * 在两路以下的车辆车道
     */
    private void playVideoIn2Video()
    {
        if (checkCheDaoSetRespDataInvalid()) return;
        final List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();
        int getDataSize = -1;
        if (data.size() > 2)
        {
            orderCheDaoIndex(data, 2);
            getDataSize = 2;
        }
        else
        {
            orderCheDaoIndex(data, data.size());
            getDataSize = data.size();
        }

        for (int i = 0; i < getDataSize; i++)
        {
            final GetCheDaoSetResp.DataBean dataBean = data.get(CheDaoIndex[i]);
            final int tempI = i;
            final int totalSize = getDataSize;
            mHandler.post(new Runnable()
            {
                @Override
                public void run()
                {
                    parkingMonitoringView.setChannelText(tempI, dataBean.getInOutName());
                    if (dataBean.getInOut() == 0)
                    {
                        parkingMonitoringView.setChannelText(tempI + 2, "入口图片显示");
                    }
                    else if (dataBean.getInOut() == 1)
                    {
                        parkingMonitoringView.setChannelText(tempI + 2, "出口图片显示");
                    }
                }
            });

            if (requestGetNetCameraSet(dataBean.getCameraIP()))
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
                            L.i("videoType:" + videoType + "i:" + i);
                            parkingMonitoringView.playVideoByIndex(i, data1.get(0).getVideoIP(), mHandler);
                            break;
                        }
                    }
                }
            }
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
         * 4, 统计获取车位信息，显示到界面即可
         */
        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                requestGetParkingInfo();
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
                requestGetCarIn();
                if (checkCarInRespValid())
                {
                    if (fragmentDetailManager.getCurrentIndex() == 0)
                    {
                        mHandler.sendEmptyMessage(MSG_GetCarIn);
                    }
                }

                requestGetCarOut();
                if (checkCarOutRespValid())
                {
                    if (fragmentDetailManager.getCurrentIndex() == 1)
                    {
                        mHandler.sendEmptyMessage(MSG_GetCarOut);
                    }
                }
            }
        }).start();
    }


    @Override
    protected void onSaveInstanceState(Bundle outState)
    {
        //“内存重启”时保存当前的fragment名字
        super.onSaveInstanceState(outState);

        outState.putInt(FragmentChargeManager.CURRENT_FRAGMENT, fragmentChargeManager.getCurrentIndex());
        outState.putInt(FragmentDetailManager.CURRENT_FRAGMENT, fragmentDetailManager.getCurrentIndex());
    }

    private void initFragment(Bundle savedInstanceState)
    {
        fragmentChargeManager = new FragmentChargeManager(getSupportFragmentManager());
        int anInt = -1;
        if (savedInstanceState != null)
        {
            anInt = savedInstanceState.getInt(FragmentChargeManager.CURRENT_FRAGMENT, 0);
        }
        fragmentChargeManager.init(anInt);

        fragmentDetailManager = new FragmentDetailManager(getSupportFragmentManager());
        int index = -1;
        if (savedInstanceState != null)
        {
            index = savedInstanceState.getInt(FragmentChargeManager.CURRENT_FRAGMENT, 0);
        }
        fragmentDetailManager.init(index);
    }


    public static final int MSG_GetCarIn = 0x01;
    public static final int MSG_GetCarOut = 0x02;
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

    public int CAR_CHANNEL_OUT = 1; // 表示车辆出口标记
    public int CAR_CHANNEL_IN = 0; // 表示车辆入口标记

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
                case MSG_GetCarIn: // 场内车辆
                {
                    updateCarInParkingData();
                    break;
                }
                case MSG_GetCarOut: // 车辆收费明细
                {
                    updateCarDetailData();
                    break;
                }
                case MSG_ParkingInfo: // 车场信息
                {
                    updateParkingInfo();
                    break;
                }
                case MSG_ChargeInfo:// 收费信息
                {
                    updateChargeInfo();
                    break;
                }
                case MSG_SETCarIn: // 车辆入场
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
                    if (checkSetCarOutInvalid()) return;
                    updateSetCarInByOut(setCarOutResp.getData());
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

                    if (checkCheDaoSetRespDataInvalid()) return;
                    List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();
                    for (int channel = 0; channel < data.size(); channel++)
                    {
                        if (data.get(channel).getCameraIP().equals(objIP))
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
                    if (checkCheDaoSetRespDataInvalid()) return;
                    List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();
                    for (int channel = 0; channel < data.size(); channel++)
                    {
                        if (data.get(channel).getCameraIP().equals(objIP))
                        {
                            L.i("MSG_STOIP_VIDEO_PLAY find index:" + channel + ", ip:" + objIP);
                            parkingMonitoringView.setTextEnableWhenStopVideo(channel);
                            break;
                        }
                    }
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

    private boolean checkSetCarOutInvalid()
    {
        if (setCarOutResp == null || setCarOutResp.getData() == null) return true;
        return false;
    }

    /**
     * 检测数据的是否无效
     *
     * @return
     */
    private boolean checkCheDaoSetRespDataInvalid()
    {
        if (getCheDaoSetResp == null || getCheDaoSetResp.getData() == null || getCheDaoSetResp.getData().size() == 0)
        {
            return true;
        }
        return false;
    }

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

    private void updateSetCarInByOut(SetCarOutResp.DataBean data)
    {
        ArrayList<String> arrayList = new ArrayList<>();
        arrayList.add(checkValue(data.getUserNO()));
        arrayList.add(checkValue(data.getUserName()));
        arrayList.add(checkValue(data.getCardNO()));
        arrayList.add(checkValue(data.getDeptName()));
        arrayList.add(checkValue(data.getCardType()));
        arrayList.add(checkValue("0.00"));
        arrayList.add(checkValue(data.getOutTime())); // 进场时间 ????
        arrayList.add(checkValue(data.getOutTime()));
        arrayList.add(checkValue("0.00"));//收费金额
        arrayList.add(checkValue("0.00")); //累计金额
        arrayList.add(checkValue(data.getBalance() + ""));
        fragmentChargeManager.setData(arrayList, null);
    }

    /**
     * 更新收费信息
     */
    private void updateChargeInfo()
    {

    }


    private boolean checkCarParkingInfoInvalid()
    {
        if (getParkingInfoResp == null || getParkingInfoResp.getData() == null)
        {
            return true;
        }
        return false;
    }

    /**
     * 更新车场数据
     */
    private void updateParkingInfo()
    {
        if (checkCarParkingInfoInvalid()) return;
        ArrayList<String> arrayList = new ArrayList<String>();

        GetParkingInfoResp.DataBean data = getParkingInfoResp.getData();
        arrayList.add(data.getMonthCarCountInPark() + "");
        arrayList.add(data.getFreeCarCountInPark() + "");
        arrayList.add(data.getTempCarCountInPark() + "");
        arrayList.add(data.getStrCarCountInPark() + "");
        arrayList.add(data.getManualOpenCarCount() + "");

        fragmentChargeManager.setData(null, arrayList);
    }

    private void updateCarDetailData()
    {
        if (!checkCarInRespValid()) return;
        List<GetCarOutResp.DataBean> data = getCarOutResp.getData();

//        if (entityCarOuts != null && entityCarOuts.size() >= 0)
        {
            L.i("entityCarOuts.size():" + data.size());
            ArrayList<HashMap<String, String>> items = new ArrayList<HashMap<String, String>>();
            for (int i = 0; i < data.size(); i++)
            {
                HashMap<String, String> item = new HashMap<String, String>();
                item.put(ColumnName.c1, checkStringIsNull(data.get(i).getCPH()));
                item.put(ColumnName.c2, checkStringIsNull(data.get(i).getChineseName()));
                item.put(ColumnName.c3, checkStringIsNull(data.get(i).getInTime()));
                item.put(ColumnName.c4, checkStringIsNull(data.get(i).getOutTime()));
                item.put(ColumnName.c5, String.valueOf(data.get(i).getSFJE()));
                item.put(ColumnName.c6, checkStringIsNull(data.get(i).getInGateName()));
                item.put(ColumnName.c7, checkStringIsNull(data.get(i).getOutGateName()));
                item.put(ColumnName.c8, checkStringIsNull(data.get(i).getUserNO()));
                item.put(ColumnName.c9, checkStringIsNull(data.get(i).getUserName()));
                item.put(ColumnName.c10, checkStringIsNull(data.get(i).getCardNO()));
                item.put(ColumnName.c11, checkStringIsNull(data.get(i).getBalance() + ""));//免费原因 FreeReason
                item.put(ColumnName.c12, String.valueOf(data.get(i).getYSJE()));
                item.put(ColumnName.c13, checkStringIsNull(data.get(i).getSFTime()));
                item.put(ColumnName.c14, checkStringIsNull(data.get(i).getInOperator())); // 收费人员 SFOperator
                item.put(ColumnName.c15, checkStringIsNull(data.get(i).getInOperatorCard()));//收费人员编号
                item.put(ColumnName.c16, checkStringIsNull(data.get(i).getInGateName()));//收费口名 SFGate

                item.put(ColumnName.c17, checkStringIsNull(""));//超时标志 OvertimeSymbol
                item.put(ColumnName.c18, checkStringIsNull(data.get(i).getOvertimeSFTime()));//超时收费时间 OvertimeSFTime
                item.put(ColumnName.c29, String.valueOf(data.get(i).getOvertimeSFJE()));//超时收费金额 OvertimeSFJE
                item.put(ColumnName.c20, String.valueOf(data.get(i).getCarparkNO()));//车场编号 CarparkNO
                item.put(ColumnName.c21, String.valueOf(data.get(i).getBigSmall()));//大小标识 BigSmall
                item.put(ColumnName.c22, checkStringIsNull(data.get(i).getFreeReason()));//免费原因 FreeReason
                item.put(ColumnName.c23, checkStringIsNull(data.get(i).getInUser()));//人场人员 InUser
                item.put(ColumnName.c24, checkStringIsNull(data.get(i).getOutUser()));//出场人员 OutUser
                item.put(ColumnName.c25, checkStringIsNull(data.get(i).getInPic()));//入场图片 InPic
                item.put(ColumnName.c26, checkStringIsNull(data.get(i).getOutPic()));//出场图片 OutPic
                item.put(ColumnName.c27, checkStringIsNull(data.get(i).getDeptName()));//部门名称 DeptName
                item.put(ColumnName.c28, "");//证件图片 ZJPic
                item.put(ColumnName.c29, checkStringIsNull(data.get(i).getInOperatorCard()));//入场操作编号 InOperatorCard
                item.put(ColumnName.c30, checkStringIsNull(data.get(i).getOutOperatorCard()));//出场操作编号 OutOperatorCard
                item.put(ColumnName.c31, checkStringIsNull(data.get(i).getInOperator()));//入场操作员 InOperator
                item.put(ColumnName.c32, checkStringIsNull(data.get(i).getOutOperator()));//出场操作员 OutOperator
                items.add(item);
            }
            fragmentDetailManager.setData(null, null, items, null);
        }
    }

    private void updateCarInParkingData()
    {
        if (!checkCarInRespValid()) return;
        List<GetCarInResp.DataBean> data = getCarInResp.getData();

//        if (entityCarIns != null && entityCarIns.size() >= 0)
        {
            int maxTextWidth[] = new int[16];
            for (int m = 0; m < 16; m++)
            {
                maxTextWidth[m] = -1;
            }

            ArrayList<HashMap<String, String>> items = new ArrayList<HashMap<String, String>>();
            for (int i = 0; i < data.size(); i++)
            {
                HashMap<String, String> item = new HashMap<String, String>();
                item.put(ColumnName.c1, checkStringIsNull(data.get(i).getCPH()));
                item.put(ColumnName.c2, checkStringIsNull(data.get(i).getChineseName()));
                item.put(ColumnName.c3, checkStringIsNull(data.get(i).getInTime()));
                item.put(ColumnName.c4, checkStringIsNull(data.get(i).getInGateName()));
                item.put(ColumnName.c5, checkStringIsNull(data.get(i).getUserNO()));
                item.put(ColumnName.c6, checkStringIsNull(data.get(i).getUserName()));
                item.put(ColumnName.c7, String.valueOf(data.get(i).getBalance()));
                item.put(ColumnName.c8, checkStringIsNull(data.get(i).getCardNO()));
                item.put(ColumnName.c9, String.valueOf(data.get(i).getCarparkNO()));
                item.put(ColumnName.c10, String.valueOf(data.get(i).getBigSmall()));
                item.put(ColumnName.c11, "");//免费原因 FreeReason
                item.put(ColumnName.c12, checkStringIsNull(data.get(i).getInPic()));
                item.put(ColumnName.c13, checkStringIsNull(data.get(i).getDeptName()));
                item.put(ColumnName.c14, ""); // ZJPic
                item.put(ColumnName.c15, checkStringIsNull(data.get(i).getInOperatorCard()));
                item.put(ColumnName.c16, checkStringIsNull(data.get(i).getInOperator()));
                items.add(item);

//                maxTextWidth[0] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[0]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[0]);
//                maxTextWidth[1] = ((entityCarIns.get(i).getCPH().length() > maxTextWidth[1]) ? entityCarIns.get(i).getCPH().length() : maxTextWidth[1]);
                maxTextWidth[2] = ((data.get(i).getInTime().length() > maxTextWidth[2]) ? data.get(i).getInTime().length() : maxTextWidth[2]);
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
//            L.i("maxTextWidth[2]:" + maxTextWidth[2] + ",maxTextWidth[11]:" + maxTextWidth[11] + ",textSize:" + textSize);
            maxTextWidth[2] = textSize;
            fragmentDetailManager.setData(items, maxTextWidth, null, null);
        }
    }

    @Override
    protected void onDestroy()
    {
        for (int i = 0; i < Model.iChannelCount; i++)
        {
            L.i("debug," + "   stopVideoByIndex-----------------------------------i:----" + i);
            parkingMonitoringView.stopVideoByIndex(i); // 这里的控件出现泄漏，如果取消视频播放呢?
        }

        mHandler.removeMessages(MSG_GetCarIn);
        mHandler.removeMessages(MSG_GetCarOut);
        mHandler.removeMessages(MSG_ParkingInfo);
        mHandler.removeMessages(MSG_SETCarIn);
        mHandler.removeMessages(MSG_SETCarOut);
        mHandler.removeMessages(MSG_KeppAlive);
        mHandler.removeMessages(MSG_UpdateBlackListData);

        mHandler.removeMessages(MSG_SET_CarChannel);
        mHandler.removeMessages(MSG_START_VIDEO_PLAY);
        mHandler.removeMessages(MSG_STOIP_VIDEO_PLAY);
        mHandler.removeMessages(MSG_TokenFailed);

        tcpsdk.getInstance().cleanup();
        mHandler.removeCallbacksAndMessages(null); // 清除所有的handler消息和runnable等;

        if (queueTask != null)
            queueTask.end();
        ConcurrentQueueHelper.getInstance().destory();
        super.onDestroy();
    }


    @Override
    public void onStart()
    {
        super.onStart();
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
                        switch (modelNode.type)
                        {
                            case CAR_IN_TYPE_auto: // 手动输入车牌
                                if (requestUrlUpdateUiWhenSetIn((SetCarInReq) modelNode.data, modelNode.getiDzIndex()) >= 0)
                                {
                                    String strFileJpg = modelNode.getStrFileJpg();
                                    final Bitmap bitmap = BitmapUtils.fileToBitmap(strFileJpg);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap, CAR_CHANNEL_IN);
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
                                            dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap, CAR_CHANNEL_IN);
                                        }
                                    });
                                }
                                break;
                            case CAR_OUT_TYPE_auto: // 手动接收的车辆出场
                                if (requestUrlUpdateUIWhenSetCarOut((SetCarOutReq) modelNode.data) >= 0)
                                {
                                    String strFileJpg = modelNode.getStrFileJpg();
                                    final Bitmap bitmap = BitmapUtils.fileToBitmap(strFileJpg);
                                    mHandler.post(new Runnable()
                                    {
                                        @Override
                                        public void run()
                                        {
                                            dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap, CAR_CHANNEL_OUT);
                                        }
                                    });
                                }
                                break;
                            case CAR_INOUT_TYPE_recognition: // 相机的主动识别
                                int index = modelNode.getiDzIndex();
                                if (checkCheDaoSetRespDataInvalid()) return;

                                List<GetCheDaoSetResp.DataBean> data = getCheDaoSetResp.getData();
                                if (data.get(CheDaoIndex[index]).getInOut() == CAR_CHANNEL_IN)
                                {
                                    L.i("入场:" + data.get(CheDaoIndex[index]).getInOutName());
                                    SetCarInReq req = new SetCarInReq(); // 发送进场数据
                                    req.setCPH(modelNode.getStrCPH());
                                    req.setToken(Model.token);
                                    req.setCtrlNumber(data.get(CheDaoIndex[modelNode.getiDzIndex()]).getCtrlNumber());
                                    req.setStationId(Model.stationID);


                                    if (requestUrlUpdateUiWhenSetIn(req) >= 0)
                                    {
                                        final Bitmap bitmap = modelNode.picture;
                                        mHandler.post(new Runnable()
                                        {
                                            @Override
                                            public void run()
                                            {
                                                dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap, CAR_CHANNEL_IN);
                                            }
                                        });
                                    }
                                }
                                else if (data.get(CheDaoIndex[index]).getInOut() == CAR_CHANNEL_OUT)
                                {
                                    L.i("出场:" + data.get(CheDaoIndex[index]).getInOutName());
                                    SetCarOutReq req = new SetCarOutReq();
                                    req.setCPH(modelNode.getStrCPH());
                                    req.setToken(Model.token);
                                    req.setCtrlNumber(data.get(CheDaoIndex[modelNode.getiDzIndex()]).getCtrlNumber());
                                    req.setStationId(Model.stationID);

                                    if (requestUrlUpdateUIWhenSetCarOut(req) >= 0)
                                    {
                                        final Bitmap bitmap = modelNode.picture;
                                        mHandler.post(new Runnable()
                                        {
                                            @Override
                                            public void run()
                                            {
                                                dealCPHInfoFromCamera(modelNode.getiDzIndex(), modelNode.getStrCPH(), bitmap, CAR_CHANNEL_OUT);
                                            }
                                        });
                                    }
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
     * 请求服务器数据，然后更新ui界面
     */
    private int requestUrlUpdateUIWhenSetCarOut(SetCarOutReq req)
    {
        setCarOutResp = GetServiceData.getInstance().SetCarOut(req);
        if (setCarOutResp == null)
        {
            return -1;
        }

        dealSetCarOutRcode(setCarOutResp);

        updateCarHintToFragment(MSG_CarHintInfoAfterResume, setCarOutResp.getMsg());

        mHandler.sendEmptyMessage(MSG_SETCarOut);
        return 0;
    }

    /**
     * 更新左侧画面的数据
     *
     * @param index  表示那个画面
     * @param CPH    表示车牌
     * @param bitmap 表示图像数据 只考虑两个图片
     */
    private void dealCPHInfoFromCamera(int index, String CPH, Bitmap bitmap, int type)
    {
        parkingMonitoringView.setCPHText(index, CPH);
        if (type == CAR_CHANNEL_IN)
            parkingMonitoringView.setInPicture(index, bitmap);
        else
            parkingMonitoringView.setOutPicture(index, bitmap);
    }

    @Override
    protected void onRestart()
    {
        super.onRestart();
    }

    @NonNull
    private SetCarInReq initSetCarIn(String CPH, int ctrlNumber)
    {
        SetCarInReq setCarInReq = new SetCarInReq(); // 发送进场数据
        setCarInReq.setCPH(CPH);
        setCarInReq.setToken(Model.token);
        setCarInReq.setCtrlNumber(ctrlNumber);
        setCarInReq.setStationId(Model.stationID);
        return setCarInReq;
    }

    // 把camera信息放到相应的列表中
    private List<GetNetCameraSetResp> netCameraList = new ArrayList<GetNetCameraSetResp>();

    private boolean checkCarInRespValid()
    {
        if (getCarInResp == null
                || getCarInResp.getData() == null
                || getCarInResp.getData().size() == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private boolean checkCarOutRespValid()
    {
        if (getCarOutResp == null
                || getCarOutResp.getData() == null
                || getCarOutResp.getData().size() == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /**
     * 处理入场的返回值
     *
     * @param resp
     */
    public void dealSetCarOutRcode(SetCarOutResp resp)
    {
        RCode rCode = RCode.valueOf(Integer.parseInt(resp.getRcode()));
        SetCarOutResp.DataBean respData = resp.getData();
        switch (rCode)
        {
            case BlackList:
            {
                break;
            }
            case NoThisLanePermission:
            {
                break;
            }
            case BeOverdue:
            {
                break;
            }
            case BalanceNotEnough:
            {
                break;
            }
            case NotFoundApproachRecord:
            {
                break;
            }
            case ProhibitCutOff:
            {
                break;
            }
            case OK:
            {
                break;
            }
            default:
            {
                break;
            }
        }
    }


    /**
     * 1, 获取车道信息
     *
     * @return
     */
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

    /**
     * 2，获取网络相机的信息
     *
     * @param cameraIP
     * @return
     */
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

    /**
     * 3, 获取在 车场内车辆明细信息
     */
    private void requestGetCarIn()
    {
        GetCarInReq req = new GetCarInReq();
        req.setToken(Model.token);
        req.setOrderField(OrderField.getWhenGetCarIn("desc"));
        req.setJsonSearchParam(JsonSearchParam.getWhenGetCarOutAndIn(String.valueOf(Model.iParkingNo)));
        getCarInResp = GetServiceData.getInstance().GetCarIn(req);
    }

    /**
     * 4，获取 车场内车辆收费明细信息
     */
    private void requestGetCarOut()
    {
        GetCarOutReq req = new GetCarOutReq();
        req.setToken(Model.token);
        req.setOrderField(OrderField.getWhenGetCarOut("desc"));
        req.setJsonSearchParam(JsonSearchParam.getWhenGetCarOutAndIn(String.valueOf(Model.iParkingNo)));
        getCarOutResp = GetServiceData.getInstance().GetCarOut(req);
    }

    /**
     * 5, 获取车场的车位信息
     */
    private void requestGetParkingInfo()
    {
        GetParkingInfoReq req = new GetParkingInfoReq();
        req.setToken(Model.token);
        req.setStationId(Model.stationID);
        req.setStartTime(TimeConvertUtils.longToString("yyyyMMddHHmmss", System.currentTimeMillis()));
        getParkingInfoResp = GetServiceData.getInstance().GetParkingInfo(req);
    }

    /**
     * 6，获取模糊对比车牌信息
     *
     * @param cph
     * @return
     */
    private GetCardIssueResp requestGetCarIssueWhenCPH_Like(String cph)
    {
        GetCardIssueReq getCardIssueReq = new GetCardIssueReq(); // 请求发卡行信息
        getCardIssueReq.setToken(Model.token);
        getCardIssueReq.setJsonSearchParam(JsonSearchParam.getWhenGetCardIssue(cph.substring(1)));  // 过滤掉第一个省份简称
        getCardIssueReq.setOrderField(OrderField.getWhenGetCardIssue("asc"));
        return GetServiceData.getInstance().SelectFxCPH_Like(getCardIssueReq);
    }

    /**
     * 7, 设置车辆入场操作
     *
     * @param setCarInReq
     * @return
     */
    private int requestUrlUpdateUiWhenSetIn(SetCarInReq setCarInReq, int index)
    {
        /**
         *  由车牌号，来请求服务器数据
         */
        final SetCarInResp setCarInResp = GetServiceData.getInstance().SetCarIn(setCarInReq);
        if (setCarInResp == null || setCarInResp.getData() == null)
        {
            return -1;
        }

        dealSetCarInResponse(setCarInResp, setCarInReq.getCPH(), index);

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
     * 8，设置车辆确认入场操作
     *
     * @param setCarInConfirmReq
     * @param srcCPH
     * @param roadName
     * @return
     */
    @Nullable
    private SetCarInConfirmResp requestSetCarInConfirm(SetCarInConfirmReq setCarInConfirmReq, String srcCPH, String roadName)
    {
        setCarInConfirmReq.setStationId(Model.stationID);
        setCarInConfirmReq.setToken(Model.token);
        setCarInConfirmReq.setCPH(srcCPH);

        // 通过入口车道名，来获取相应的机号

        int ctrlIndexByInoutName = getCtrlIndexByInoutName(roadName);
        setCarInConfirmReq.setCtrlNumber(CheDaoIndex[ctrlIndexByInoutName]);

        final SetCarInConfirmResp setCarInConfirmResp = GetServiceData.getInstance().SetCarInConfirmed(setCarInConfirmReq);
        if (setCarInConfirmResp == null)
            return null;
        L.i("确认车牌信息:" + setCarInConfirmReq);
        return setCarInConfirmResp;
    }

    /**
     * 9, 设置无牌车车辆进场设置
     *
     * @param carInWithoutCPHReq
     * @return
     */
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

    /**
     * 10, 车辆出场的模糊获取
     */
    private GetCarInResp requestSelectComeCPH_Like(String CPH)
    {
        GetCarInReq req = new GetCarInReq();
        req.setToken(Model.token);
        req.setJsonSearchParam(JsonSearchParam.getWhenSelectComeCPH_Like(CPH.substring(1), String.valueOf(Model.iParkingNo)));
        req.setOrderField(OrderField.getSelectComeCPH_Like("desc"));

        GetCarInResp getCarInResp = GetServiceData.getInstance().GetCarIn(req);
        if (getCarInResp == null || getCarInResp.getData() == null || getCarInResp.getData().size() == 0)
        {
            L.i("return null");
            return null;
        }
        else
        {
            L.i("getCarInResp.getData().size()" + getCarInResp.getData().size());
            return getCarInResp;
        }
    }

    /**
     * 11, 车辆出场
     *
     * @param CPH
     * @return
     */
    private SetCarOutResp requestSetCarOut(String CPH, int CtrlNumber, int StationId, int CPColor)
    {
        SetCarOutReq req = new SetCarOutReq();
        req.setToken(Model.token);
        req.setCtrlNumber(CtrlNumber);
        req.setCPH(CPH);
        req.setStationId(StationId);
        if (CPColor != -1)
        {
            req.setCPColor(CPColor);
        }

        SetCarOutResp setCarOutResp = GetServiceData.getInstance().SetCarOut(req);
        if (setCarOutResp == null)
        {
            return null;
        }
        else
        {
            return setCarOutResp;
        }
    }


    /**
     * 无入场记录的车辆出场
     */
    private SetCarOutWithoutEntryRecordResp requestSetCarOutWithoutEntryRecord(String cph, String ctrlNumber, String StationId, int cpColor)
    {
        SetCarOutWithoutEntryRecordReq req = new SetCarOutWithoutEntryRecordReq();
        req.setToken(Model.token);
        req.setCPH(cph);
        req.setCtrlNumber(Integer.parseInt(ctrlNumber));
        req.setStationId(Integer.parseInt(StationId));
        if (cpColor != -1)
        {
            req.setCPColor(cpColor);
        }

        SetCarOutWithoutEntryRecordResp resp = GetServiceData.getInstance().SetCarOutWithoutEntryRecord(req);
        if (resp == null)
        {
            return null;
        }
        else
        {
            return resp;
        }
    }

    public UpdateChargeAmountReq initUpdateChargeAmountReq()
    {
        UpdateChargeAmountReq req = new UpdateChargeAmountReq();
        return req;
    }

    /**
     * 更改收费金额
     */
    private UpdateChargeAmountResp requestUpdateChargeAmount(UpdateChargeAmountReq req)
    {
        UpdateChargeAmountResp resp = GetServiceData.getInstance().UpdateChargeAmount(req);
        if (resp == null)
        {
            return null;
        }
        else
        {
            return resp;
        }
    }

    /**
     * 更新收费信息接口
     */
    private UpdateChargeInfoResp UpdateChargeInfo()
    {
        UpdateChargeInfoReq req = new UpdateChargeInfoReq(); // 填充相应的字段数据
        UpdateChargeInfoResp resp = GetServiceData.getInstance().UpdateChargeInfo(req);
        if (resp == null)
        {
            return null;
        }
        else
        {
            return resp;
        }
    }

    /**
     * 证件抓拍
     */
    private UpdateChargeWithCaptureImageResp requestUpdateChargeWithCaptureImage()
    {
        UpdateChargeWithCaptureImageReq req = new UpdateChargeWithCaptureImageReq(); // 填充相应的字段数据
        UpdateChargeWithCaptureImageResp resp = GetServiceData.getInstance().UpdateChargeWithCaptureImage(req);
        if (resp == null)
        {
            return null;
        }
        else
        {
            return resp;
        }
    }

    /**
     * 取消收费接口
     */
    private CancelChargeResp requestCancelCharge()
    {
        CancelChargeReq chargeReq = new CancelChargeReq();
        CancelChargeResp cancelChargeResp = GetServiceData.getInstance().CancelCharge(chargeReq);
        if (cancelChargeResp == null)
        {
            return null;
        }
        else
        {
            return cancelChargeResp;
        }
    }


}



