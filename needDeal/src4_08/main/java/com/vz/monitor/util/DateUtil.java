package com.vz.monitor.util;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;


public class DateUtil
{
    public static final String TYPE_DATE_TIME_1 = "yyyy-MM-dd HH:mm:ss";
    public static final String TYPE_DATE_TIME_2 = "yyyyMMdd_HHmmss";
    public static final String TYPE_DATE = "yyyy-MM-dd";
    public static final String TYPE_TIME = "HH:mm:ss";
    public static final String TYPE_TIME_1 = "HH-mm-ss";


    /**
     * 按照 yyyy-MM-dd 获得当前日期，例如 2017-04-05
     * @return
     */
    public static final String getNowDate()
    {
        return getNowDateAndTime(TYPE_DATE);
    }

    /**
     * 按照 HH:mm:ss 获取当前时间 例如：12:00:00
     * @return
     */
    public static final String getNowTime()
    {
        return getNowDateAndTime(TYPE_TIME);
    }

    /**
     * 提供指定的格式来获取style的日期时间
     * @param style
     * @return
     */
    public static final String getNowDateAndTime(String style)
    {
        Calendar calendar = Calendar.getInstance();
        Date date = calendar.getTime();
        SimpleDateFormat sdf = new SimpleDateFormat(style);
        String timeStr = sdf.format(date);
        return timeStr;
    }

    /**
     * 以当前时间为准，获取相对相隔当前时间 differ 后的时间，即偏移时间；
     * @param style
     * @param differ
     * @return
     */
    public static final String getNowDateAndTime(String style, int differ)
    {
        Calendar calendar = Calendar.getInstance();
        long time = calendar.getTime().getTime() + differ * 1000;
        Date date = new Date(time);
        SimpleDateFormat sdf = new SimpleDateFormat(style);
        String timeStr = sdf.format(date);
        return timeStr;
    }

    /**
     * 获取指定 calendar 对应的格式style的日期时间字符串
     * @param calendar
     * @param style
     * @return
     */
    public static final String getDate(Calendar calendar, String style)
    {
        Date date = calendar.getTime();
        SimpleDateFormat sdf = new SimpleDateFormat(style);
        String dateString = sdf.format(date);
        return dateString;
    }

    /**
     * 将String转换成相应的calendar日期对象
     * @param dateStr
     * @return
     */
    public static final Calendar string2Calendar(String dateStr)
    {
        GregorianCalendar calendar = null;
        try
        {
            SimpleDateFormat sdf = new SimpleDateFormat(TYPE_DATE_TIME_1);
            Date date = sdf.parse(dateStr);
            calendar = new GregorianCalendar();
            calendar.setTime(date);
        }
        catch (Exception e)
        {
            return null;
        }
        return calendar;
    }

    /**
     * 将str中含有- : 空格进行过滤，获取剩下的字符串
     * @param str
     * @return
     */
    public static final String string2String(String str)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < str.length(); i++)
        {
            Character temp = Character.valueOf(str.charAt(i));
            if (!"-".equals(temp.toString()) && !":".equals(temp.toString()) && !" ".equals(temp.toString()))
            {
                builder.append(temp);
            }
        }
        return builder.toString();
    }

    /**
     * 将毫秒数转换 yyyy-MM-dd HH:mm:ss 格式的字符
     * @param millisecond
     * @return
     */
    public static final String millisecond2String(long millisecond)
    {
        Date d = new Date(millisecond);
        String dStr = "";
        try
        {
            SimpleDateFormat sdf = new SimpleDateFormat(TYPE_DATE_TIME_1);
            dStr = sdf.format(d);
        }
        catch (Exception e)
        {
        }
        return dStr;
    }

    /**
     * 将秒数转换 mm:ss 12:35指定的格式
     * @param second
     * @return
     */
    public static final String convertTime(int second)
    {
        String text = "00:00";

        if (second >= 0)
        {
            int s = second % 60;
            int m = (second / 60) % 60;


            text = String.format("%02d:%02d", m, s);
        }

        return text;
    }
}
