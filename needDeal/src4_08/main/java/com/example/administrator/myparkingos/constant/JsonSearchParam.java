package com.example.administrator.myparkingos.constant;

import android.util.ArrayMap;

import com.example.administrator.myparkingos.model.JsonJoin;
import com.example.administrator.myparkingos.model.RequestByURL;
import com.example.administrator.myparkingos.model.beans.SelectModel;
import com.example.administrator.myparkingos.model.beans.gson.EntityOperatorDetail;
import com.example.administrator.myparkingos.util.L;
import com.google.gson.Gson;

import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Created by Administrator on 2017-03-29.
 */
public class JsonSearchParam
{
    /**
     * 输入车牌，构建 JsonSearchParam 字符串
     *
     * @param cph
     * @return
     */
    public static String getWhenGetCardIssue(String cph)
    {
        List<SelectModel> lstSM = new ArrayList<SelectModel>();
        SelectModel sm = new SelectModel();

        sm.getConditions().add(sm.new conditions("CPH", "like", "%" + cph + "%", "and"));
        lstSM.add(sm);

        sm = new SelectModel();
        sm.getConditions().add(sm.new conditions("CardState", "=", 0, "or"));
        sm.getConditions().add(sm.new conditions("CardState", "=", 2, "or"));
        lstSM.add(sm);

        Gson gson = new Gson(); // gson不用连续来重建
        String where = gson.toJson(lstSM);
        return URLEncoder.encode(where);
    }


    public static String getWhenGetOperators(String userNo)
    {
        Map<String, String> map = new LinkedHashMap<String, String>();
        map.put("UserNO", userNo);

        String toJson = JsonJoin.ToJson(map, false, false);
        return URLEncoder.encode(toJson);
    }

    public static String getWhenGetCheDaoSet(String stationID)
    {
        Map<String, String> map = new LinkedHashMap<String, String>();
        map.put("StationID", stationID);
        String toJson = JsonJoin.ToJson(map, false, false);
        return URLEncoder.encode(toJson);
    }

    public static String getWhenGetCameraSet(String cameraIp)
    {
        Map<String, String> map = new LinkedHashMap<String, String>();
        map.put("VideoIP", cameraIp);
        String toJson = JsonJoin.ToJson(map, false, false);
        return URLEncoder.encode(toJson);
    }

    public static String getWhenGetCarOutAndIn(String carParkNo)
    {
        Map<String, String> map = new LinkedHashMap<String, String>();
        map.put("CarparkNO", carParkNo);
        String toJson = JsonJoin.ToJson(map, false, true);
        return URLEncoder.encode(toJson);
    }

    public static String getWhenSelectComeCPH_Like(String cph, String parkingNO)
    {
        L.i("getWhenSelectComeCPH_Like: cph," + cph + "parkingNO," + parkingNO);
        List<SelectModel> lstSM = new ArrayList<SelectModel>();
        SelectModel sm = new SelectModel();

        sm.getConditions().add(sm.new conditions("CPH", "like", "%" + cph + "%", "and"));
        sm.getConditions().add(sm.new conditions("bigsmall", "=", 0, "and"));
        sm.getConditions().add(sm.new conditions("CarParkNo", "=", parkingNO, "and"));
        lstSM.add(sm);

        Gson gson = new Gson();
        String where = gson.toJson(lstSM);
        return URLEncoder.encode(where);
    }
}
