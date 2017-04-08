package com.example.administrator.myparkingos.constant;

import com.example.administrator.myparkingos.model.responseInfo.GetSysSettingObjectResp;

/**
 * Created by Administrator on 2017-04-06.
 */
public class GlobalParams
{
    /**
     * 将一些全局的参数放到当前进行统一管理
     */
    public static GetSysSettingObjectResp getSysSettingObjectResp;

    public static void setGetSysSettingObjectResp(GetSysSettingObjectResp resp)
    {
        getSysSettingObjectResp = resp;
    }

    public static GetSysSettingObjectResp.DataBean getSysSettingObjectResp()
    {
        if (getSysSettingObjectResp == null || getSysSettingObjectResp.getData() == null) return null;
        return getSysSettingObjectResp.getData();
    }
}
