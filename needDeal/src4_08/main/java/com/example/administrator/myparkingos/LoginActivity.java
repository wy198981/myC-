package com.example.administrator.myparkingos;

import android.annotation.TargetApi;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.example.administrator.mydistributedparkingos.R;
import com.example.administrator.myparkingos.constant.ConstantConfig;
import com.example.administrator.myparkingos.constant.ConstantSharedPrefs;
import com.example.administrator.myparkingos.constant.GlobalParams;
import com.example.administrator.myparkingos.constant.JsonSearchParam;
import com.example.administrator.myparkingos.model.GetServiceData;
import com.example.administrator.myparkingos.model.beans.Model;

import com.example.administrator.myparkingos.model.requestInfo.GetOperatorsReq;
import com.example.administrator.myparkingos.model.requestInfo.GetOperatorsWithoutLoginReq;
import com.example.administrator.myparkingos.model.requestInfo.GetRightsByGroupIDReq;
import com.example.administrator.myparkingos.model.requestInfo.GetServerTimeReq;
import com.example.administrator.myparkingos.model.requestInfo.GetStationSetWithoutLoginReq;
import com.example.administrator.myparkingos.model.requestInfo.GetSysSettingObjectReq;
import com.example.administrator.myparkingos.model.requestInfo.LoginUserReq;
import com.example.administrator.myparkingos.model.responseInfo.GetOperatorsResp;
import com.example.administrator.myparkingos.model.responseInfo.GetOperatorsWithoutLoginResp;
import com.example.administrator.myparkingos.model.responseInfo.GetRightsByGroupIDResp;
import com.example.administrator.myparkingos.model.responseInfo.GetServerTimeResp;
import com.example.administrator.myparkingos.model.responseInfo.GetStationSetWithoutLoginResp;
import com.example.administrator.myparkingos.model.responseInfo.GetSysSettingObjectResp;
import com.example.administrator.myparkingos.model.responseInfo.LoginUserResp;
import com.example.administrator.myparkingos.myUserControlLibrary.niceSpinner.NiceSpinner;

import com.example.administrator.myparkingos.ui.FormServerSetActivity;
import com.example.administrator.myparkingos.ui.onlineMonitorPage.ParkingMonitoringActivity;
import com.example.administrator.myparkingos.util.L;
import com.example.administrator.myparkingos.util.MD5Utils;
import com.example.administrator.myparkingos.util.RegexUtil;
import com.example.administrator.myparkingos.util.SPUtils;
import com.example.administrator.myparkingos.util.T;
import com.example.administrator.myparkingos.util.WeakHandler;

import java.util.ArrayList;
import java.util.List;

public class LoginActivity extends AppCompatActivity implements View.OnClickListener
{
    private final String TAG = "LoginActivity";

    private EditText et_password;
    private Button btnLogin;
    private Button btnParkSet;
    private Button btnServerParamSet;
    private int indexUserName = 0; // 初始化为0,
    private int indexStationId = 0;

    private String curStationID = "";

    /**
     * 从服务器请求数据的数据的返回列表
     */
    private String md5Password;

    private NiceSpinner spinnerUsername;
    private NiceSpinner spinnerstation;

    private GetOperatorsWithoutLoginResp getOperatorsWithoutLoginResp;
    private GetStationSetWithoutLoginResp getStationSetWithoutLoginResp;
    private LoginUserResp loginUserResp;
    private GetSysSettingObjectResp getSysSettingObjectResp;
    private GetServerTimeResp serverTime;
    private GetOperatorsResp getOperatorsResp;
    private GetRightsByGroupIDResp getRightsByGroupIDResp;

    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login1);
//        createConfigSettings();
//        ScreenUtils.setWindowPositionAndSize(this, 3, 2);

        getWindow().setTitleColor(Color.BLUE);
        initView();
        login_Loaded();
    }

    @Override
    protected void onRestart()
    {
        L.i("onRestart===================================");
        super.onRestart();
        L.i("indexUserName:" + indexUserName + ",indexStationId:" + indexStationId);
        login_Loaded();
    }

    /**
     * 创建配置文件，在程序执行时，即配置信息(可以将其放在Application进行初始化)
     */
    private void createConfigSettings()
    {
        SPUtils.put(ConstantSharedPrefs.FileAppSetting, getApplicationContext(), ConstantSharedPrefs.ServiceIP, ConstantConfig.ServerIP);
        SPUtils.put(ConstantSharedPrefs.FileAppSetting, getApplicationContext(), ConstantSharedPrefs.ServicePort, ConstantConfig.ServerPort);
    }

    private void initView()
    {
        et_password = (EditText) findViewById(R.id.et_password);
        btnLogin = (Button) findViewById(R.id.btnLogin);
        btnParkSet = (Button) findViewById(R.id.btnParkSet);
        btnServerParamSet = (Button) findViewById(R.id.btnServerParamSet);

        btnLogin.setOnClickListener(this);
        btnParkSet.setOnClickListener(this);
        btnServerParamSet.setOnClickListener(this);

        spinnerUsername = (NiceSpinner) findViewById(R.id.spinnerUsername);
        spinnerUsername.setSpinnerListener(new NiceSpinner.SpinnerListener()
        {
            @Override
            public void OnSpinnerItemClick(int pos)
            {
                indexUserName = pos; // 记录当前的位置
            }
        });

        spinnerstation = (NiceSpinner) findViewById(R.id.spinnerstation);
        spinnerstation.setSpinnerListener(new NiceSpinner.SpinnerListener()
        {
            @Override
            public void OnSpinnerItemClick(int pos)
            {
                indexStationId = pos;
            }
        });
    }

    @Override
    public void onClick(View v)
    {
        switch (v.getId())
        {
            case R.id.btnLogin:
                btnLogin_Click();
                break;
            case R.id.btnParkSet:
                btnParkSet_Click();
                break;
            case R.id.btnServerParamSet:
                btnSet_Click(); //服务器设置
                break;
        }
    }

    /**
     * 点击车场设置
     */
    private void btnParkSet_Click()
    {

    }

    /**
     * 点击服务器设置按钮
     */
    private void btnSet_Click()
    {
        Intent intent = new Intent(LoginActivity.this, FormServerSetActivity.class);
        startActivity(intent);
    }

    /**
     * 点击登录按钮
     */
    private void btnLogin_Click()
    {
        if (!checkUIInput()) return;
        final String password = et_password.getEditableText().toString();

        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                try
                {
                    md5Password = MD5Utils.MD5(password);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
                Model.sUserPwd = md5Password;
                requestLoginUser();
                handlerLoginRequest();
            }
        }).start();
    }

    /**
     * 检测ui的合法性数据
     *
     * @return
     */
    private boolean checkUIInput()
    {
        String username = spinnerUsername.getCurrentText();
        if (TextUtils.isEmpty(username))
        {
            Toast.makeText(LoginActivity.this, "请选择操作员", Toast.LENGTH_SHORT).show();
            return false;
        }

        String station = spinnerstation.getCurrentText();
        if (TextUtils.isEmpty(station))
        {
            Toast.makeText(LoginActivity.this, "请选择工作站", Toast.LENGTH_SHORT).show();
            return false;
        }

        L.i("username:" + username + ",station:" + station);
        return true;
    }

    private void login_Loaded()
    {
        if (SPUtils.checkPathExist(getApplicationContext(), ConstantSharedPrefs.FileAppSetting))
        {
            String serviceIP = (String) SPUtils.get(ConstantSharedPrefs.FileAppSetting, getApplicationContext()
                    , ConstantSharedPrefs.ServiceIP, "");
            String servicePort = (String) SPUtils.get(ConstantSharedPrefs.FileAppSetting, getApplicationContext()
                    , ConstantSharedPrefs.ServicePort, "");
            String tempStationID = (String) SPUtils.get(ConstantSharedPrefs.FileAppSetting, getApplicationContext()
                    , ConstantSharedPrefs.StationID, "");

            Model.serverIP = serviceIP;
            Model.serverPort = servicePort; // 重新读取服务器的配置文件
            GetServiceData.getInstance().setAddressAndPort(serviceIP, servicePort);

            boolean b = RegexUtil.checkDigit(tempStationID);
            curStationID = b ? tempStationID : "";// 已经读取了工作数据
            if (!RegexUtil.checkIpAddress(serviceIP) || !RegexUtil.checkDecimals(servicePort))
            {
                Log.i(TAG, "RegexUtil serviceIP:" + serviceIP + ",servicePort:" + servicePort);
                Toast.makeText(LoginActivity.this, "服务IP或端口格式不正确,请重新配置", Toast.LENGTH_SHORT).show();
            }
            else
            {
                new Thread(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        requestGetOperatorNOLogin();
                        mHandler.post(new Runnable()
                        {
                            @Override
                            public void run()
                            {
                                updateUserName();
                            }
                        });

                        requestGetStationNoLogin();
                        mHandler.post(new Runnable()
                        {
                            @Override
                            public void run()
                            {
                                updateStationName();
                            }
                        });
                    }
                }).start();

                mHandler.postDelayed(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        if (getOperatorsWithoutLoginResp == null || getStationSetWithoutLoginResp == null)
                        {
                            // 从服务器获取数据失败，进入服务器参数设置界面
                            Intent intent = new Intent(LoginActivity.this, FormServerSetActivity.class);
                            startActivity(intent);
                        }
                    }
                }, 3000);
            }
        }
        else
        {
            showDialog("配置文件丢失，请联系管理员");
        }
    }


    private void updateStationName()
    {
        if (getStationSetWithoutLoginResp != null
                || getStationSetWithoutLoginResp.getData() != null)
        {
            setSpinnerStationName(getStationSetWithoutLoginResp.getData());
        }
    }

    private void setSpinnerStationName(List<GetStationSetWithoutLoginResp.DataBean> data)
    {
        if (data != null || data.size() > 0)
        {
            List<String> stationList = new ArrayList<String>();
            for (int i = 0; i < data.size(); i++)
            {
                stationList.add(String.valueOf(data.get(i).getStationName()));// 把站点名放到列表中
            }
            int selectIndex = indexStationId;
            if (stationList.size() > 0)
            {
                if (curStationID != null && curStationID != "")
                {
                    for (int i = 0; i < data.size(); i++)
                    {
                        if (Short.parseShort(curStationID) == data.get(i).getStationId())
                        {
                            selectIndex = i;
                            break;
                        }

                    }
                }

            }
            spinnerstation.refreshData(stationList, selectIndex);
        }
    }

    /**
     * 更新ui界面的数据
     */
    private void updateUserName()
    {
        if (getOperatorsWithoutLoginResp != null
                && getOperatorsWithoutLoginResp.getData() != null)
        {
            setSpinnerUserName(getOperatorsWithoutLoginResp.getData());
        }
    }

    private void setSpinnerUserName(List<GetOperatorsWithoutLoginResp.DataBean> data)
    {
        if (data != null || data.size() > 0)
        {
            List<String> userList = new ArrayList<String>();
            for (int i = 0; i < data.size(); i++)
            {
                userList.add(data.get(i).getUserName());
            }
            spinnerUsername.refreshData(userList, indexUserName);
        }
    }

    private WeakHandler mHandler = new WeakHandler();


    /**
     * 判断是否有指定的权限
     *
     * @param fromName
     * @param itemName
     * @return
     */
    private boolean isHaveRightsByName(String fromName, String itemName)
    {
        boolean result = false;
        // 从right获取出相应的权限
        for (int i = 0; i < Model.lstRights.size(); i++)
        {
            GetRightsByGroupIDResp.DataBean tmpRights = Model.lstRights.get(i);
            if (tmpRights.getFormName().equals(fromName)
                    && tmpRights.getItemName().equals(itemName))
            {
                if (tmpRights.isCanOperate())
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    private int getStationIdFromList(int index)
    {
        if (getStationSetWithoutLoginResp != null
                && getStationSetWithoutLoginResp.getData() != null
                && getOperatorsWithoutLoginResp.getData().size() > 0
                && (index >= 0 && index < getOperatorsWithoutLoginResp.getData().size()))
        {
            return getStationSetWithoutLoginResp.getData().get(index).getStationId();
        }

        return 0;
    }

    private int getCarparkNOFromList(int index)
    {
        if (getStationSetWithoutLoginResp != null
                && getStationSetWithoutLoginResp.getData() != null
                && getOperatorsWithoutLoginResp.getData().size() > 0
                && (index >= 0 && index < getOperatorsWithoutLoginResp.getData().size()))
        {
            return getStationSetWithoutLoginResp.getData().get(index).getCarparkNO();
        }

        return 0;
    }

    private String getUserNoFromList(int index)
    {
        if (getOperatorsWithoutLoginResp != null
                && getOperatorsWithoutLoginResp.getData() != null
                && getOperatorsWithoutLoginResp.getData().size() > 0
                && (index >= 0 && index < getOperatorsWithoutLoginResp.getData().size()))
        {
            return getOperatorsWithoutLoginResp.getData().get(index).getUserNO();
        }

        return null;
    }


    @TargetApi(Build.VERSION_CODES.KITKAT)
    private void handlerLoginRequest()
    {
        Model.stationID = getStationIdFromList(indexStationId);
        Model.iParkingNo = getCarparkNOFromList(indexStationId);

        if (SPUtils.checkPathExist(getApplicationContext(), ConstantSharedPrefs.FileAppSetting)) // 配置文件存在
        {
            L.i("curStationID:" + curStationID + ", Model.stationID:" + Model.stationID);
            if (!TextUtils.isEmpty(curStationID) && RegexUtil.checkDigit(curStationID))
            {
                if (Model.stationID == Integer.parseInt(curStationID))//相等
                {
                    enterNextSetting();
                }
                else // 不相等
                {
                    // 界面判断是否需要切换工作站点
                    mHandler.post(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            showWorkStationSwitchDialog("是否切换工作站，请谨慎操作?", indexStationId);
                        }
                    });
                }
            }
            else// 初次登录时，站点不存在时
            {
                SPUtils.put(ConstantSharedPrefs.FileAppSetting, getApplicationContext(),
                        ConstantSharedPrefs.StationID, String.valueOf(Model.stationID));
                curStationID = String.valueOf(Model.stationID);
                enterNextSetting();
            }
        }
    }

    private void enterNextSetting()
    {
        new Thread(new Runnable()//        DataSourceToPubVar() 获取车场设置，主要是对Model的数据的填充
        {
            @Override
            public void run()
            {
                // 获取系统设置
                requestSysSettingObject();
            }
        }).start();

        // 设置本地日期格式，从服务器获取时间 http://192.168.2.158:9000/ParkAPI/getServerTime?token=806f13c43e7044c1a268bc6a09e00c81
        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                requestSysTime();
            }
        }).start();

        // 数据定义

        // 准备工作
        //判断用户卡号是否相等的，即同一个用户登录
        if (Model.sUserCard.equals(SPUtils.get(ConstantSharedPrefs.FileAppSetting, getApplicationContext(),
                ConstantSharedPrefs.UserCode, "0")))
        {
            // LoginDate 可以存放毫秒数
            String result = (String) SPUtils.get(ConstantSharedPrefs.FileAppSetting, getApplicationContext()
                    , ConstantSharedPrefs.LoginDate, "");
            if (!result.equals(""))
                Model.dLoginTime = Long.parseLong(result);
        }
        else
        {
            SPUtils.put(ConstantSharedPrefs.FileAppSetting, getApplicationContext(),
                    ConstantSharedPrefs.UserCode, Model.sUserCard);
            long l = System.currentTimeMillis();
            SPUtils.put(ConstantSharedPrefs.FileAppSetting, getApplicationContext(),
                    ConstantSharedPrefs.LoginDate, "0");
            Model.dLoginTime = l;
        }
        // 设置本地日期格式

        // 获取操作员信息
        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                requestGetOperators();

                if (getOperatorsResp != null && getOperatorsResp.getData() != null
                        && getOperatorsResp.getData().size() > 0)
                {
                    Model.sUserName = getOperatorsResp.getData().get(0).getUserName();
                    Model.sUserCard = getOperatorsResp.getData().get(0).getCardNO();
                    Model.sGroupNo = getOperatorsResp.getData().get(0).getUserLevel();

                    // 获取权限组
                    requestGetRightByID();

                    if (getRightsByGroupIDResp != null && getRightsByGroupIDResp.getData() != null)
                    {
                        Model.lstRights = getRightsByGroupIDResp.getData();
                    }

                    mHandler.post(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            if (Model.lstRights == null || Model.lstRights.size() <= 0)
                            {
                                Toast.makeText(LoginActivity.this, "无进入在线监控权限", Toast.LENGTH_SHORT).show();
                            }
                            else
                            {
                                // 获取是否有在线监控的权限
                                if (isHaveRightsByName("在线监控", "CmdView"))
                                {
                                    Intent intent = new Intent(LoginActivity.this, ParkingMonitoringActivity.class);
                                    startActivity(intent);
                                }
                                else
                                {
                                    Toast.makeText(LoginActivity.this, "无进入在线监控权限", Toast.LENGTH_SHORT).show();
                                }
                            }
                        }
                    });
                }
            }
        }).start();
    }


    private void showDialog(String str)
    {
        new AlertDialog.Builder(this).setTitle("提示信息")
                .setMessage(str).setPositiveButton("确认", new DialogInterface.OnClickListener()
        {
            @Override
            public void onClick(DialogInterface dialog, int which)
            {
                Intent intent = new Intent(LoginActivity.this, FormServerSetActivity.class);
                startActivity(intent);
            }
        }).show();
    }

    private void showWorkStationSwitchDialog(String str, final int index)
    {
        new AlertDialog.Builder(this).setTitle("提示信息")
                .setMessage(str).setPositiveButton("确认", new DialogInterface.OnClickListener()
        {
            @Override
            public void onClick(DialogInterface dialog, int which)// 对比StationId是否相等
            {
                int id = getStationIdFromList(index);

                //保存站点即可
                SPUtils.put(ConstantSharedPrefs.FileAppSetting, getApplicationContext()
                        , ConstantSharedPrefs.StationID, String.valueOf(id));
                curStationID = String.valueOf(id);

                enterNextSetting();
            }
        }).setNegativeButton("取消", new DialogInterface.OnClickListener()
        {
            @Override
            public void onClick(DialogInterface dialog, int which)
            {

            }
        }).show();
    }

    @Override
    protected void onDestroy()
    {
        super.onDestroy();

    }

    @Override
    protected void onPause()
    {
        super.onPause();
        mHandler.removeCallbacksAndMessages(null);
    }

    private void showMsgToScreen(final String msg)
    {
        mHandler.post(new Runnable()
        {
            @Override
            public void run()
            {
                T.showShort(LoginActivity.this, msg);
            }
        });
    }


    /**
     * 1 获取操作员信息
     */
    private void requestGetOperatorNOLogin()
    {
        GetOperatorsWithoutLoginReq req = new GetOperatorsWithoutLoginReq();
        getOperatorsWithoutLoginResp = GetServiceData.getInstance().GetOperatorsWithoutLogin(req);
    }

    /**
     * 2 获取站点消息
     */
    private void requestGetStationNoLogin()
    {
        GetStationSetWithoutLoginReq stationReq = new GetStationSetWithoutLoginReq();
        stationReq.setOrderField("StationId");
        getStationSetWithoutLoginResp = GetServiceData.getInstance().GetStationSetWithoutLogin(stationReq);
    }

    /**
     * 3 获取登录消息用户的token
     */
    private boolean requestLoginUser()
    {
        LoginUserReq loginUserReq = new LoginUserReq();
        loginUserReq.setPassword(md5Password);
        String tempUserNO = "";
        if (getOperatorsWithoutLoginResp != null
                && getOperatorsWithoutLoginResp.getData() != null
                && getOperatorsWithoutLoginResp.getData().size() != 0)
        {
            tempUserNO = getOperatorsWithoutLoginResp.getData().get(indexUserName).getUserNO();
        }
        loginUserReq.setUserNo(tempUserNO);
        loginUserResp = GetServiceData.getInstance().LoginUser(loginUserReq);

        if (loginUserResp != null && loginUserResp.getData() != null)
        {
            if (Integer.parseInt(loginUserResp.getRcode()) != 200)
            {
                showMsgToScreen(loginUserResp.getMsg());
                return false;
            }
            else
            {
                Model.token = loginUserResp.getData().getToken(); // 登录成功
                return true;
            }
        }
        else
        {
            showMsgToScreen("登录服务器失败");
            return false;
        }
    }


    /**
     * 4 获取系统配置信息
     */
    private void requestSysSettingObject()
    {
        GetSysSettingObjectReq getSysSettingObjectReq = new GetSysSettingObjectReq();
        getSysSettingObjectReq.setToken(Model.token);
        getSysSettingObjectReq.setStationID(getStationIdFromList(indexStationId));
        getSysSettingObjectResp = GetServiceData.getInstance().GetSysSettingObject(getSysSettingObjectReq);
        if (getSysSettingObjectReq != null)
        {
            GlobalParams.getSysSettingObjectResp = getSysSettingObjectResp;
        }
        L.i("getSysSettingObjectResp", getSysSettingObjectResp.toString());
    }

    /**
     * 5 获取系统时间
     */
    private void requestSysTime()
    {
        GetServerTimeReq getServerTimeReq = new GetServerTimeReq();
        getServerTimeReq.setToken(Model.token);
        serverTime = GetServiceData.getInstance().getServerTime(getServerTimeReq);
    }

    /**
     * 6 获取操作员信息
     */
    private void requestGetOperators()
    {
        GetOperatorsReq getOperatorsReq = new GetOperatorsReq();
        getOperatorsReq.setToken(Model.token);
        getOperatorsReq.setJsonSearchParam(JsonSearchParam.getWhenGetOperators(getUserNoFromList(indexUserName)));
        getOperatorsResp = GetServiceData.getInstance().GetOperators(getOperatorsReq);
    }


    /**
     * 7 获取权限消息
     */
    private void requestGetRightByID()
    {
        GetRightsByGroupIDReq getRightsByGroupIDReq = new GetRightsByGroupIDReq();
        getRightsByGroupIDReq.setToken(Model.token);
        getRightsByGroupIDReq.setGroupID(Model.sGroupNo);
        getRightsByGroupIDResp = GetServiceData.getInstance().GetRightsByGroupID(getRightsByGroupIDReq);
    }
}