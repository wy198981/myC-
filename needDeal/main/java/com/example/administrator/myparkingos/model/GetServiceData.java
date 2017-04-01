package com.example.administrator.myparkingos.model;

import android.annotation.TargetApi;
import android.os.Build;
import android.util.ArrayMap;

import com.example.administrator.myparkingos.model.beans.Model;
import com.example.administrator.myparkingos.model.beans.gson.EntityNetCameraSet;
import com.example.administrator.myparkingos.model.beans.gson.EntityOperatorDetail;
import com.example.administrator.myparkingos.model.beans.gson.EntityRights;
import com.example.administrator.myparkingos.model.beans.gson.EntityRoadWaySet;
import com.example.administrator.myparkingos.model.beans.gson.EntitySetCarOut;
import com.example.administrator.myparkingos.model.beans.gson.EntityStationSet;
import com.example.administrator.myparkingos.model.beans.gson.EntitySysSetting;
import com.example.administrator.myparkingos.model.beans.gson.EntitySystemTime;
import com.example.administrator.myparkingos.model.beans.gson.EntityToken;
import com.example.administrator.myparkingos.model.requestInfo.CarPlateNumberLikeReq;
import com.example.administrator.myparkingos.model.requestInfo.GetCardIssueReq;
import com.example.administrator.myparkingos.model.requestInfo.GetCheDaoSetReq;
import com.example.administrator.myparkingos.model.requestInfo.GetNetCameraSetReq;
import com.example.administrator.myparkingos.model.requestInfo.GetOperatorsReq;
import com.example.administrator.myparkingos.model.requestInfo.GetOperatorsWithoutLoginReq;
import com.example.administrator.myparkingos.model.requestInfo.GetRightsByGroupIDReq;
import com.example.administrator.myparkingos.model.requestInfo.GetServerTimeReq;
import com.example.administrator.myparkingos.model.requestInfo.GetSysSettingObjectReq;
import com.example.administrator.myparkingos.model.requestInfo.LoginUserReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInConfirmReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarInWithoutCPHReq;
import com.example.administrator.myparkingos.model.requestInfo.SetCarOutReq;
import com.example.administrator.myparkingos.model.responseInfo.GetCardIssueResp;
import com.example.administrator.myparkingos.model.responseInfo.GetCheDaoSetResp;
import com.example.administrator.myparkingos.model.responseInfo.GetNetCameraSetResp;
import com.example.administrator.myparkingos.model.responseInfo.GetOperatorsResp;
import com.example.administrator.myparkingos.model.responseInfo.GetOperatorsWithoutLoginResp;
import com.example.administrator.myparkingos.model.requestInfo.GetStationSetWithoutLoginReq;
import com.example.administrator.myparkingos.model.responseInfo.GetRightsByGroupIDResp;
import com.example.administrator.myparkingos.model.responseInfo.GetServerTimeResp;
import com.example.administrator.myparkingos.model.responseInfo.GetStationSetWithoutLoginResp;
import com.example.administrator.myparkingos.model.responseInfo.GetSysSettingObjectResp;
import com.example.administrator.myparkingos.model.responseInfo.LoginUserResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInConfirmResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarInWithoutCPHResp;
import com.example.administrator.myparkingos.model.responseInfo.SetCarOutResp;
import com.example.administrator.myparkingos.util.HttpUtils;
import com.example.administrator.myparkingos.util.L;

import java.lang.reflect.Method;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Created by Administrator on 2017-03-27.
 */
public class GetServiceData
{
    /**
     * 获取服务器数据 使用静态内部类也可以实现单例模式
     */
    private static GetServiceData mGetServiceData;

    private static final String TAG = "RequestByURL";
    public static String address = "http://" + Model.serverIP + ":" + Model.serverPort + "/";

    private Request req;

    public void setAddressAndPort(String address, String port)
    {
        Model.serverIP = address;
        Model.serverPort = port;
    }

    private GetServiceData()
    {
        req = new Request();
    }

    static public GetServiceData getInstance()
    {
        if (mGetServiceData == null)
        {
            synchronized (GetServiceData.class)
            {
                if (mGetServiceData == null)
                {
                    mGetServiceData = new GetServiceData();
                }
            }
        }
        return mGetServiceData;
    }

    /**
     * 获取操作员信息
     */
    public GetOperatorsWithoutLoginResp GetOperatorsWithoutLogin(GetOperatorsWithoutLoginReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        CommonJsonList commonJsonList =
                getDataWithList("GetOperatorsWithoutLogin", GetOperatorsWithoutLoginResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }
        GetOperatorsWithoutLoginResp resp = new GetOperatorsWithoutLoginResp();
        resp.setRcode(commonJsonList.getRcode());
        resp.setMsg(commonJsonList.getMsg());
        resp.setData(commonJsonList.getData());
        L.i("GetOperatorsWithoutLogin" + resp.toString());
        return resp;
    }

    /**
     * 在没有登录情况下，获取工作站信息
     * <p>
     * 例如：请求：http://192.168.2.158:9000/ParkAPI/GetStationSetWithoutLogin?token=&OrderField=StationId
     *
     * @return
     */
    public GetStationSetWithoutLoginResp GetStationSetWithoutLogin(GetStationSetWithoutLoginReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        CommonJsonList commonJsonList =
                getDataWithList("GetStationSetWithoutLogin", GetStationSetWithoutLoginResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }
        GetStationSetWithoutLoginResp resp = new GetStationSetWithoutLoginResp();
        resp.setRcode(commonJsonList.getRcode());
        resp.setMsg(commonJsonList.getMsg());
        resp.setData(commonJsonList.getData());
        L.i("GetStationSetWithoutLogin" + resp.toString());
        return resp;
    }

    public LoginUserResp LoginUser(LoginUserReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        CommonJson commonJson =
                getData("LoginUser", LoginUserResp.DataBean.class, null, convertString);
        if (commonJson == null)
        {
            return null;
        }
        LoginUserResp resp = new LoginUserResp();
        resp.setRcode(commonJson.getRcode());
        resp.setMsg(commonJson.getMsg());
        resp.setData((LoginUserResp.DataBean) commonJson.getData());
        L.i("LoginUser" + resp.toString());
        return resp;
    }

    /**
     * 请求系统设置   http://192.168.2.158:9000/ParkAPI/GetSysSettingObject?token=af7ba4e7a0164b1582468612a18d9d57&StationID=2
     *
     * @return
     */
    @TargetApi(Build.VERSION_CODES.KITKAT)
    public GetSysSettingObjectResp GetSysSettingObject(GetSysSettingObjectReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        CommonJson commonJson =
                getData("GetSysSettingObject", GetSysSettingObjectResp.DataBean.class, null, convertString);
        if (commonJson == null)
        {
            return null;
        }
        GetSysSettingObjectResp resp = new GetSysSettingObjectResp();
        resp.setRcode(commonJson.getRcode());
        resp.setMsg(commonJson.getMsg());
        resp.setData((GetSysSettingObjectResp.DataBean) commonJson.getData());
        L.i("LoginUser" + resp.toString());
        return resp;
    }

    /**
     * 请求服务器时间 http://192.168.2.158:9000/ParkAPI/getServerTime?token=806f13c43e7044c1a268bc6a09e00c81
     */
    public GetServerTimeResp getServerTime(GetServerTimeReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        CommonJson commonJson =
                getData("getServerTime", GetServerTimeResp.DataBean.class, null, convertString);
        if (commonJson == null)
        {
            return null;
        }
        GetServerTimeResp resp = new GetServerTimeResp();
        resp.setRcode(commonJson.getRcode());
        resp.setMsg(commonJson.getMsg());
        resp.setData((GetServerTimeResp.DataBean) commonJson.getData());
        L.i("getServerTime" + resp.toString());
        return resp;
    }

    /**
     * 请求操作员的详细信息  //    http://192.168.2.158:9000/ParkAPI/GetOperators?token=3b773b3f20f24260bf4d379ff09c3839
     * &jsonSearchParam=%5b%7b%22Conditions%22%3a%5b%7b%22FieldName%22%3a%22UserNO%22%2c%22Operator%22%3a%22%3d%22%2c%22FieldValue%22%3a%22888888%22%2c%22Combinator%22%3a%22and%22%7d%5d%2c%22Combinator%22%3a%22and%22%7d%5d
     */
    public GetOperatorsResp GetOperators(GetOperatorsReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        CommonJsonList commonJsonList = getDataWithList("GetOperators", GetOperatorsResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }
        GetOperatorsResp resp = new GetOperatorsResp();
        resp.setRcode(commonJsonList.getRcode());
        resp.setMsg(commonJsonList.getMsg());
        resp.setData(commonJsonList.getData());
        L.i("GetOperators" + resp.toString());
        return resp;
    }

    /**
     * 请求权限组详细信息    http://192.168.2.158:9000/ParkAPI/GetRightsByGroupID?token=ca30622e4a2d421483e5e5da95ba6fd1&GroupID=1
     */
    @TargetApi(Build.VERSION_CODES.KITKAT)
    public GetRightsByGroupIDResp GetRightsByGroupID(GetRightsByGroupIDReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        CommonJsonList commonJsonList = getDataWithList("GetRightsByGroupID", GetRightsByGroupIDResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }
        GetRightsByGroupIDResp resp = new GetRightsByGroupIDResp();
        resp.setRcode(commonJsonList.getRcode());
        resp.setMsg(commonJsonList.getMsg());
        resp.setData(commonJsonList.getData());
        L.i("GetRightsByGroupID" + resp.toString());
        return resp;
    }


    /**
     * 车辆进场 http://192.168.2.158:9000/ParkAPI/SetCarIn
     */
    public SetCarInResp SetCarIn(SetCarInReq setCarInReq)
    {
        /**
         * 1，将setCarInReq 转换成 Map 集合；
         */
        Map value = getValue(setCarInReq);

        /**
         * 2, 将map集合转换成 key-value键值对的字符串，
         */
        String convertString = mapConvertString(value);

        /**
         * 3，请求网络获取数据
         */
        CommonJson setCarIn = getData("SetCarIn", SetCarInResp.DataBean.class, null, convertString);
        if (setCarIn == null)
        {
            return null; // 为空
        }

        SetCarInResp setCarInResp = new SetCarInResp();
        setCarInResp.setRcode(setCarIn.getRcode());
        setCarInResp.setMsg(setCarIn.getMsg());
        setCarInResp.setData((SetCarInResp.DataBean) setCarIn.getData());
        L.i("SetCarIn" + setCarInResp.toString());
        return setCarInResp;
    }

    /**
     * 无牌车进场  http://192.168.2.158:9000/ParkAPI/SetCarInWithoutCPH?token=b6018e51fe16417fb959f3a3383fccc7&CtrlNumber=3&StationId=2
     *
     * @param setCarInWithoutCPHReq
     * @return
     */
    public SetCarInWithoutCPHResp SetCarInWithoutCPH(SetCarInWithoutCPHReq setCarInWithoutCPHReq)
    {
        Map value = getValue(setCarInWithoutCPHReq);

        String convertString = mapConvertString(value);

        CommonJson commonJson = getData("SetCarInWithoutCPH", SetCarInWithoutCPHResp.DataBean.class, null, convertString);
        if (commonJson == null)
        {
            return null;
        }

        SetCarInWithoutCPHResp setCarInWithoutCPHResp = new SetCarInWithoutCPHResp();
        setCarInWithoutCPHResp.setRcode(commonJson.getRcode());
        setCarInWithoutCPHResp.setMsg(commonJson.getMsg());
        setCarInWithoutCPHResp.setData((SetCarInWithoutCPHResp.DataBean) commonJson.getData());
        L.i("SetCarInWithoutCPH" + setCarInWithoutCPHResp.toString());
        return setCarInWithoutCPHResp;
    }

    /**
     * 车辆进场确认  http://192.168.2.158:9000/ParkAPI/SetCarInWithoutCPH?CPH=京a43234&CtrlNumber=9&StationId=1&Token=eb118c26689046498b8a00c635c61da6
     *
     * @param setCarInConfirmReq
     * @return
     */
    public SetCarInConfirmResp SetCarInConfirmed(SetCarInConfirmReq setCarInConfirmReq)
    {
        Map value = getValue(setCarInConfirmReq);

        String convertString = mapConvertString(value);

        CommonJson commonJson = getData("SetCarInConfirmed", SetCarInConfirmResp.DataBean.class, null, convertString);
        if (commonJson == null)
        {
            return null;
        }

        SetCarInConfirmResp setCarInConfirmResp = new SetCarInConfirmResp();
        setCarInConfirmResp.setRcode(commonJson.getRcode());
        setCarInConfirmResp.setMsg(commonJson.getMsg());
        setCarInConfirmResp.setData((SetCarInConfirmResp.DataBean) commonJson.getData());
        L.i("SetCarInWithoutCPH" + setCarInConfirmResp.toString());
        return setCarInConfirmResp;
    }

    /**
     * 获取模糊对比车牌数据
     */
    public void GetCarInByCarPlateNumberLike(CarPlateNumberLikeReq carPlateNumberLikeReq)
    {
        Map value = getValue(carPlateNumberLikeReq);

        String convertString = mapConvertString(value);

        CommonJson commonJson = getData("GetCarInByCarPlateNumberLike", Integer.class, null, convertString);
        if (commonJson == null)
        {
            return;
        }
    }

    /**
     * 当车辆手动进场时，查询获取发卡行信息，提供相应的提示
     */
    public GetCardIssueResp SelectFxCPH_Like(GetCardIssueReq getCardIssueReq)
    {
        Map value = getValue(getCardIssueReq);

        String convertString = mapConvertString(value);

        // 解析数据
        CommonJsonList commonJsonList = getDataWithList("GetCardIssue", GetCardIssueResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }

        L.i("commonJsonList data" + commonJsonList.getData());
        GetCardIssueResp getCardIssueResp = new GetCardIssueResp();
        getCardIssueResp.setMsg(commonJsonList.getMsg());
        getCardIssueResp.setRcode(commonJsonList.getRcode());
        getCardIssueResp.setData(commonJsonList.getData());
        return getCardIssueResp;
    }

    /**
     * 车辆出场 http://192.168.2.158:9000/ParkAPI/SetCarOut?&StationId=1&CPH=粤b55555&token=520d2e8492d1405baa2e25314a39541e&CtrlNumber=10
     */
    public SetCarOutResp SetCarOut(SetCarOutReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        // 解析数据
        CommonJsonList commonJsonList = getDataWithList("SetCarOut", SetCarOutResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }

        L.i("commonJsonList data" + commonJsonList.getData());


        SetCarOutResp setCarOutResp = new SetCarOutResp();

        setCarOutResp.setMsg(commonJsonList.getMsg());
        setCarOutResp.setRcode(commonJsonList.getRcode());
        setCarOutResp.setData(commonJsonList.getData());
        return setCarOutResp;
    }

    /**
     * 获取车道信息
     */
    public GetCheDaoSetResp GetCheDaoSet(GetCheDaoSetReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        // 解析数据
        CommonJsonList commonJsonList = getDataWithList("GetCheDaoSet", GetCheDaoSetResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }

        L.i("commonJsonList data" + commonJsonList.getData());

        GetCheDaoSetResp getCheDaoSetResp = new GetCheDaoSetResp();

        getCheDaoSetResp.setMsg(commonJsonList.getMsg());
        getCheDaoSetResp.setRcode(commonJsonList.getRcode());
        getCheDaoSetResp.setData(commonJsonList.getData());
        return getCheDaoSetResp;

    }

    /**
     * 9, http://192.168.2.158:9000/ParkAPI/GetNetCameraSet?token=55b10f562a27406d921fccab85001ec9&jsonSearchParam=%5b%7b%22Conditions%22%3a%5b%7b%22FieldName%22%3a%22VideoIP%22%2c%22Operator%22%3a%22%3d%22%2c%22FieldValue%22%3a%22192.168.6.211%22%2c%22Combinator%22%3a%22and%22%7d%5d%2c%22Combinator%22%3a%22and%22%7d%5d
     * //通过camra IP来获取相机信息
     */
    public GetNetCameraSetResp GetNetCameraSet(GetNetCameraSetReq req)
    {
        Map value = getValue(req);

        String convertString = mapConvertString(value);

        // 解析数据
        CommonJsonList commonJsonList = getDataWithList("GetNetCameraSet", GetNetCameraSetResp.DataBean.class, null, convertString);
        if (commonJsonList == null)
        {
            return null;
        }

        L.i("commonJsonList data" + commonJsonList.getData());

        GetNetCameraSetResp resp = new GetNetCameraSetResp();

        resp.setMsg(commonJsonList.getMsg());
        resp.setRcode(commonJsonList.getRcode());
        resp.setData(commonJsonList.getData());
        return resp;
    }


    /**
     * 将实体类，转换成相应的map集合，但是需要注意的是，有些字段不是必须的；
     * int 默认时为0，0在字段中可以有意思的存在的;所以这里使用相应的封装类，封装类可以指定null,或者相应的基础类
     *
     * @param thisObj
     * @return
     */
    public static Map getValue(Object thisObj)
    {
        Map map = new LinkedHashMap<>();
        Class c;
        try
        {
            c = Class.forName(thisObj.getClass().getName()); // 获取object的class对象
            Method[] m = c.getMethods();
            for (int i = 0; i < m.length; i++)
            {
                String method = m[i].getName();// 遍历所有的方法，其getName后面即为相应的变量名字
                if (method.startsWith("get"))
                {
                    try
                    {
                        Object value = m[i].invoke(thisObj);
                        if (value != null && !"getClass".equals(method)) //value为空，则不存放
                        {
                            String key = method.substring(3); // 3即为get的长度
                            key = key.substring(0, 1).toUpperCase() + key.substring(1);// 大小写的变化
//                            map.put(method.substring(3).toLowerCase(), value);
                            map.put(key, value);
                        }
                    }
                    catch (Exception e)
                    {
                        System.out.println("error:" + method);
                    }
                }
            }
        }
        catch (Exception e)
        {
            // TODO: handle exception
        }
        return map;
    }

    /**
     * 将map集合的字符对转换成字符串
     *
     * @param srcMap
     * @return
     */
    @TargetApi(Build.VERSION_CODES.KITKAT)
    public String mapConvertString(Map<String, Object> srcMap)
    {
        StringBuffer stringBuffer = new StringBuffer();
        if (srcMap == null || srcMap.size() <= 0)
        {
            return null;
        }

        Set<String> strings = srcMap.keySet();

        int i = 0;
        for (String str : strings)
        {
            Object o = srcMap.get(str);
            String eachString;
            if (!(o instanceof String)) // 不是String,转换成String
            {
                eachString = String.valueOf(o);
            }
            else
            {
                eachString = (String) o;
            }
            stringBuffer.append(str).append("=").append(eachString);
            if (i != strings.size() - 1)
            {
                stringBuffer.append("&");
            }
            i++;
        }

//        L.i("stringBuffer.toString():" + stringBuffer.toString());
        return stringBuffer.toString();//
    }

    /**
     * 获取数据
     *
     * @param interfaceName
     * @param clazz
     * @param orderField
     * @param param
     * @return
     */
    public CommonJson getData(String interfaceName, Class clazz, String orderField, String param)
    {
        String data;
        CommonJson result = null;

        if (null == interfaceName || interfaceName.trim().length() <= 0)
        {
            return null;
        }

        /**
         * 表示url需要的参数序列
         */

        String URLParam = String.format("%1$s%2$s", // %3$s
                null == orderField ? "" : "&OrderField=" + orderField, // param2
                (null == param || param.trim().length() <= 0 ? "" : param)// param3
        );

        /**
         * 组合需要最后的 URL地址
         */
        String expectURL = String.format("%1$sParkAPI/%2$s%3$s%4$s"
                , address
                , interfaceName
                , (null == URLParam || "" == URLParam.trim() ? "" : "?")
                , URLParam);

        try
        {
            data = HttpUtils.doGet(expectURL);
            L.i("url:" + expectURL + "###, data:" + data);
            CommonJson commonJson = CommonJson.fromJson(data, clazz);// CommonJson cannot be cast to com.example.administrator.myparkingos.model.responseInfo.SetCarInResp

            return commonJson;
        }
        catch (Exception ex)
        {
            L.i(expectURL + " >>> 获取数据失败 " + ex);
        }

        return result;
    }


    /**
     * 获取带有列表的数据
     *
     * @param interfaceName
     * @param clazz
     * @param orderField
     * @param param
     * @return
     */
    public CommonJsonList getDataWithList(String interfaceName, Class clazz, String orderField, String param)
    {
        String data;
        CommonJsonList result = null;

        if (null == interfaceName || interfaceName.trim().length() <= 0)
        {
            return null;
        }

        /**
         * 表示url需要的参数序列
         */

        String URLParam = String.format("%1$s%2$s",
                (null == param || param.trim().length() <= 0 ? "" : param)// param2
                , null == orderField ? "" : "&OrderField=" + orderField// param3
        ); // 拼接字符

        /**
         * 组合需要最后的 URL地址
         */
        String expectURL = String.format("%1$sParkAPI/%2$s%3$s%4$s"
                , address
                , interfaceName
                , (null == URLParam || "" == URLParam.trim() ? "" : "?")
                , URLParam);

        try
        {
            data = HttpUtils.doGet(expectURL);
            L.i("url:" + expectURL + " ###, data:" + data);
            CommonJsonList commonJsonList = CommonJsonList.fromJson(data, clazz);
            return commonJsonList;
        }
        catch (Exception ex)
        {
            ex.printStackTrace();
            L.i(expectURL + ", 获取数据失败" + ex.getMessage());
        }

        return result;
    }


}
