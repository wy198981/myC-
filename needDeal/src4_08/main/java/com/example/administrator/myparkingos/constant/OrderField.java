package com.example.administrator.myparkingos.constant;

import com.example.administrator.myparkingos.model.RequestByURL;

import java.net.URLEncoder;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

/**
 * Created by Administrator on 2017-03-29.
 */
public class OrderField
{
    public static String getWhenGetCardIssue(String cphOrder)
    {
        HashMap<String, String> hashMap = new HashMap<>();
        hashMap.put("CPH", cphOrder);

        String orderField = getOrderField(hashMap);
        return orderField;
    }

    public static String getWhenGetCheDaoSet(String onlineOrder, String ctrlNumberOrder, String inOutOrder)
    {
        HashMap<String, String> hashMap = new HashMap<>();
        hashMap.put("OnLine", onlineOrder);
        hashMap.put("CtrlNumber", ctrlNumberOrder);
        hashMap.put("InOut", inOutOrder);

        String orderField = getOrderField(hashMap);
        return orderField;
    }

    public static String getWhenGetCarOut(String order)
    {
        HashMap<String, String> hashMap = new HashMap<>();
        hashMap.put("InTime", order);

        String orderField = getOrderField(hashMap);
        return orderField;
    }

    public static String getWhenGetCarIn(String order)
    {
        HashMap<String, String> hashMap = new HashMap<>();
        hashMap.put("ID", order);

        String orderField = getOrderField(hashMap);
        return orderField;
    }

    public static String getSelectComeCPH_Like(String order)
    {
        HashMap<String, String> hashMap = new HashMap<>();
        hashMap.put("InTime", order);

        String orderField = getOrderField(hashMap);
        return orderField;
    }


    private static String getOrderField(Map<String, String> inMap)
    {
        if (inMap == null)
        {
            return null;
        }

        String order = "";
        int count = 0;
        Set<String> keySet = inMap.keySet();
        for (String tempStr : keySet)
        {
            count++;
            if (count == 1)
            {
                order = tempStr + " " + inMap.get(tempStr);
            }
            else
            {
                order += "," + tempStr + " " + inMap.get(tempStr);
            }
        }
//        return order;
        return URLEncoder.encode(order); //where条件都没有经过编码转换
    }
}
