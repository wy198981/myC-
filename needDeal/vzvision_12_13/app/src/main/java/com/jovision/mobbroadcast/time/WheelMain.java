
package com.jovision.mobbroadcast.time;

import java.util.Arrays;
import java.util.Calendar;
import java.util.List;

import android.view.View;

import com.example.vzvision.R;

public class WheelMain {

    private View view;
    private WheelView wv_year;
    private WheelView wv_month;
    private WheelView wv_day;
    private WheelView wv_hours;
    private WheelView wv_mins;
    private WheelView wv_secs;
    /**
     * 0: 楠炲瓨婀�弮銉︽閸掞拷
     * 1閿涳拷楠炲瓨婀�弮锟�     * 2閿涳拷楠烇拷
     * */
    private int iTime;

    private static int START_YEAR = 1990, END_YEAR = 2100;

	public View getView() {
        return view;
    }

    public void setView(View view) {
        this.view = view;
    }

    public static int getSTART_YEAR() {
        return START_YEAR;
    }

    public static void setSTART_YEAR(int sTART_YEAR) {
        START_YEAR = sTART_YEAR;
    }

    public static int getEND_YEAR() {
        return END_YEAR;
    }

    public static void setEND_YEAR(int eND_YEAR) {
        END_YEAR = eND_YEAR;
    }

    public WheelMain(View view) {
        super();
        this.view = view;
        iTime = 0;
        setView(view);
    }

    public WheelMain(View view, int iTime) {
        super();
        this.view = view;
        this.iTime = iTime;
        setView(view);
    }

    public void initDateTimePicker() {
        Calendar calendar = Calendar.getInstance();

        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);
        int hour = calendar.get(Calendar.HOUR_OF_DAY);
        int min = calendar.get(Calendar.MINUTE);
        int sec = calendar.get(Calendar.SECOND);

        initDateTimePicker(year, month, day, hour, min,sec);
    }

    public void initDateTimePicker(int year, int month, int day) {
        this.initDateTimePicker(year, month, day, 0, 0,0);
    }

    /**
     * @Description: TODO 瀵懓鍤弮銉︽埂閺冨爼妫块柅澶嬪閸ｏ拷
     */
    public void initDateTimePicker(int year, int month, int day, int h, int m,int sec) {
        // 濞ｈ濮炴径褍鐨張鍫熸箑娴犺棄鑻熺亸鍡楀従鏉烆剚宕叉稉绨俰st,閺傞�绌舵稊瀣倵閻ㄥ嫬鍨介弬锟�       
    	String[] months_big = {
                "1", "3", "5", "7", "8", "10", "12"
        };
        String[] months_little = {
                "4", "6", "9", "11"
        };

        final List<String> list_big = Arrays.asList(months_big);
        final List<String> list_little = Arrays.asList(months_little);

        wv_year = (WheelView) view.findViewById(R.id.year);
        wv_month = (WheelView) view.findViewById(R.id.month);
        wv_day = (WheelView) view.findViewById(R.id.day);
        wv_hours = (WheelView) view.findViewById(R.id.hour);
        wv_mins = (WheelView) view.findViewById(R.id.min);
        wv_secs = (WheelView) view.findViewById(R.id.sec);

        // 濞ｈ濮�楠烇拷閻╂垵鎯�
        OnWheelChangedListener wheelListener_year = new OnWheelChangedListener() {
            public void onChanged(WheelView wheel, int oldValue, int newValue) {
                int year_num = newValue + START_YEAR;
                // 閸掋倖鏌囨径褍鐨張鍫濆挤閺勵垰鎯侀梻鏉垮嬀,閻€劍娼电涵顔肩暰"閺冿拷閻ㄥ嫭鏆熼幑锟�            
                if (list_big.contains(String.valueOf(wv_month.getCurrentItem() + 1))) {
                    wv_day.setAdapter(new NumericWheelAdapter(1, 31));
                } else if (list_little.contains(String.valueOf(wv_month
                        .getCurrentItem() + 1))) {
                    wv_day.setAdapter(new NumericWheelAdapter(1, 30));
                } else {
                    if ((year_num % 4 == 0 && year_num % 100 != 0)
                            || year_num % 400 == 0)
                        wv_day.setAdapter(new NumericWheelAdapter(1, 29));
                    else
                        wv_day.setAdapter(new NumericWheelAdapter(1, 28));
                }
            }
        };

        // 濞ｈ濮�閺堬拷閻╂垵鎯�
        OnWheelChangedListener wheelListener_month = new OnWheelChangedListener() {
            public void onChanged(WheelView wheel, int oldValue, int newValue) {
                int month_num = newValue + 1;
                // 閸掋倖鏌囨径褍鐨張鍫濆挤閺勵垰鎯侀梻鏉垮嬀,閻€劍娼电涵顔肩暰"閺冿拷閻ㄥ嫭鏆熼幑锟�            
                if (list_big.contains(String.valueOf(month_num))) {
                    wv_day.setAdapter(new NumericWheelAdapter(1, 31));
                } else if (list_little.contains(String.valueOf(month_num))) {
                    wv_day.setAdapter(new NumericWheelAdapter(1, 30));
                } else {
                    if (((wv_year.getCurrentItem() + START_YEAR) % 4 == 0 && (wv_year
                            .getCurrentItem() + START_YEAR) % 100 != 0)
                            || (wv_year.getCurrentItem() + START_YEAR) % 400 == 0)
                        wv_day.setAdapter(new NumericWheelAdapter(1, 29));
                    else
                        wv_day.setAdapter(new NumericWheelAdapter(1, 28));
                }
            }
        };

        if (iTime == 2) {
            wv_day.setVisibility(View.GONE);
            wv_month.setVisibility(View.GONE);
            wv_hours.setVisibility(View.GONE);
            wv_mins.setVisibility(View.GONE);
            wv_secs.setVisibility(View.GONE);

            // 楠烇拷
            wv_year.setAdapter(new NumericWheelAdapter(START_YEAR, END_YEAR));// 鐠佸墽鐤�楠烇拷閻ㄥ嫭妯夌粈鐑樻殶閹癸拷
            wv_year.setCyclic(true);// 閸欘垰鎯婇悳顖涚泊閸旓拷
            // wv_year.setLabel("楠烇拷);// 濞ｈ濮為弬鍥х摟
            wv_year.setCurrentItem(year - START_YEAR);// 閸掓繂顫愰崠鏍ㄦ閺勫墽銇氶惃鍕殶閹癸拷
        } else if (iTime == 0) {
            // 楠烇拷
            wv_year.setAdapter(new NumericWheelAdapter(START_YEAR, END_YEAR));// 鐠佸墽鐤�楠烇拷閻ㄥ嫭妯夌粈鐑樻殶閹癸拷
            wv_year.setCyclic(true);// 閸欘垰鎯婇悳顖涚泊閸旓拷
            // wv_year.setLabel("楠烇拷);// 濞ｈ濮為弬鍥х摟
            wv_year.setCurrentItem(year - START_YEAR);// 閸掓繂顫愰崠鏍ㄦ閺勫墽銇氶惃鍕殶閹癸拷

            // 閺堬拷
            wv_month.setAdapter(new NumericWheelAdapter(1, 12));
            wv_month.setCyclic(true);
            // wv_month.setLabel("閺堬拷);
            wv_month.setCurrentItem(month);

            // 閺冿拷
            wv_day.setCyclic(true);
            // 閸掋倖鏌囨径褍鐨張鍫濆挤閺勵垰鎯侀梻鏉垮嬀,閻€劍娼电涵顔肩暰"閺冿拷閻ㄥ嫭鏆熼幑锟�            
            if (list_big.contains(String.valueOf(month + 1))) {
                wv_day.setAdapter(new NumericWheelAdapter(1, 31));
            } else if (list_little.contains(String.valueOf(month + 1))) {
                wv_day.setAdapter(new NumericWheelAdapter(1, 30));
            } else {
                // 闂傛澘鍕�
                if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                    wv_day.setAdapter(new NumericWheelAdapter(1, 29));
                else
                    wv_day.setAdapter(new NumericWheelAdapter(1, 28));
            }
            // wv_day.setLabel("閺冿拷);
            wv_day.setCurrentItem(day - 1);

            wv_hours.setAdapter(new NumericWheelAdapter(0, 23));
            wv_hours.setCyclic(true);// 閸欘垰鎯婇悳顖涚泊閸旓拷
            // wv_hours.setLabel("閺冿拷);// 濞ｈ濮為弬鍥х摟
            wv_hours.setCurrentItem(h);

            wv_mins.setAdapter(new NumericWheelAdapter(0, 59));
            wv_mins.setCyclic(true);// 閸欘垰鎯婇悳顖涚泊閸旓拷
            // wv_mins.setLabel("閸掞拷);// 濞ｈ濮為弬鍥х摟
            wv_mins.setCurrentItem(m);
            
            wv_secs.setAdapter(new NumericWheelAdapter(0, 59));
            wv_secs.setCyclic(true);// 閸欘垰鎯婇悳顖涚泊閸旓拷
            // wv_mins.setLabel("閸掞拷);// 濞ｈ濮為弬鍥х摟
            wv_secs.setCurrentItem(sec);

            wv_year.addChangingListener(wheelListener_year);
            wv_month.addChangingListener(wheelListener_month);
        } else if (iTime == 1) {
            wv_hours.setVisibility(View.GONE);
            wv_mins.setVisibility(View.GONE);
            wv_secs.setVisibility(View.GONE);

            // 楠烇拷
            wv_year.setAdapter(new NumericWheelAdapter(START_YEAR, END_YEAR));// 鐠佸墽鐤�楠烇拷閻ㄥ嫭妯夌粈鐑樻殶閹癸拷
            wv_year.setCyclic(true);// 閸欘垰鎯婇悳顖涚泊閸旓拷
            wv_year.setLabel("年");// 濞ｈ濮為弬鍥х摟
            wv_year.setCurrentItem(year - START_YEAR);// 閸掓繂顫愰崠鏍ㄦ閺勫墽銇氶惃鍕殶閹癸拷

            // 閺堬拷
            wv_month.setAdapter(new NumericWheelAdapter(1, 12));
            wv_month.setCyclic(true);
            wv_month.setLabel("月");
            wv_month.setCurrentItem(month);

            // 閺冿拷
            wv_day.setCyclic(true);
            // 閸掋倖鏌囨径褍鐨張鍫濆挤閺勵垰鎯侀梻鏉垮嬀,閻€劍娼电涵顔肩暰"閺冿拷閻ㄥ嫭鏆熼幑锟�        
            if (list_big.contains(String.valueOf(month + 1))) {
                wv_day.setAdapter(new NumericWheelAdapter(1, 31));
            } else if (list_little.contains(String.valueOf(month + 1))) {
                wv_day.setAdapter(new NumericWheelAdapter(1, 30));
            } else {
                // 闂傛澘鍕�
                if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                    wv_day.setAdapter(new NumericWheelAdapter(1, 29));
                else
                    wv_day.setAdapter(new NumericWheelAdapter(1, 28));
            }
            wv_day.setLabel("日");
            wv_day.setCurrentItem(day - 1);

            wv_year.addChangingListener(wheelListener_year);
            wv_month.addChangingListener(wheelListener_month);
        }

        // 閺嶈宓佺仦蹇撶鐎靛棗瀹抽弶銉﹀瘹鐎规岸锟介幏鈺佹珤鐎涙ぞ缍嬮惃鍕亣鐏忥拷娑撳秴鎮撶仦蹇撶閸欘垵鍏樻稉宥呮倱)
        int textSize = 14;
        // if (iTime == 0)
        // textSize = (screenheight / 100) * 2;
        // else if (iTime == 1)
        // textSize = (screenheight / 100) * 4;
        // else if (iTime == 2)
        // textSize = (screenheight / 100) * 6;

        wv_day.TEXT_SIZE = textSize;
        wv_month.TEXT_SIZE = textSize;
        wv_year.TEXT_SIZE = textSize;
        wv_hours.TEXT_SIZE = textSize;
        wv_mins.TEXT_SIZE = textSize;
        wv_secs.TEXT_SIZE = textSize;
    }

    public String getTime() {
        StringBuffer sb = new StringBuffer();
        if (iTime == 1) {
            sb.append((wv_year.getCurrentItem() + START_YEAR)).append("-")
                    .append((wv_month.getCurrentItem() + 1)).append("-")
                    .append((wv_day.getCurrentItem() + 1));
        } else if (iTime == 0) {
            sb.append((wv_year.getCurrentItem() + START_YEAR)).append("-")
                    .append(lpad(wv_month.getCurrentItem() + 1)).append("-")
                    .append(lpad(wv_day.getCurrentItem() + 1)).append(" ")
                    .append(lpad(wv_hours.getCurrentItem())).append(":")
                    .append(lpad(wv_mins.getCurrentItem())).append(":")
                    .append(lpad(wv_secs.getCurrentItem()));
        } else if (iTime == 2) {
            sb.append((wv_year.getCurrentItem() + START_YEAR));
        }
        return sb.toString();
    }

    /**
     * 鐞涖儵缍堟稉宥堝喕闂�灝瀹�
     * 
     * @param number 閺佹澘鐡�
     * @return
     */
    private String lpad(int number) {
        return lpad(2, number);
    }

    /**
     * 鐞涖儵缍堟稉宥堝喕闂�灝瀹�
     * 
     * @param length 闂�灝瀹�
     * @param number 閺佹澘鐡�
     * @return
     */
    private String lpad(int length, int number) {
        String f = "%0" + length + "d";
        return String.format(f, number);
    }
}
