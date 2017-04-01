package com.example.administrator.myparkingos.util;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by Administrator on 2017-02-17.
 */
public class RegexUtil
{
    /**
     * 检查email的合法性
     * @param email
     * @return
     */
    public static boolean checkEmail(String email)
    {
        String regex = "\\w+@\\w+\\.[a-z]+(\\.[a-z]+)?";
        /**
         * \\w: [\:转义] [\w: 匹配包括下划线的任何单词字符。等价于“[A-Za-z0-9_]”。] [+:匹配前面的子表达式一次或多次]
         * @ 普通字符
         * \\. 表示取消转义即\。 \。单独表示除换行外单独一个字符，特殊含义；。简单字符
         * [a-z]+表示a,z的一个字符
         * \w+@\w+\.[a-z]+(\.[a-z]+)?：字符串
         * 例子：13265539954@163.com
         */
        return Pattern.matches(regex, email);
    }

    /**
     * 检测card是否符合某一个规范
     * @param idCard
     * @return
     */
    public static boolean checkIdCard(String idCard)
    {
        String regex = "[1-9]\\d{13,16}[a-zA-Z0-9]{1}";
        /**
         * [1-9]\d{13,16}[a-zA-Z0-9]{1}
         * 限制
         */
        return Pattern.matches(regex, idCard);
    }

    /**
     * 检测电话的合法性
     * @param mobile
     * @return
     */
    public static boolean checkMobile(String mobile)
    {
        String regex = "(\\+\\d+)?1[3458]\\d{9}$";
        /**
         * (\+\d+)?1[3458]\d{9}$
         * ()表示规则
         * 因为含有一些特殊字符，所以加上\来表示特殊字符
         */
        return Pattern.matches(regex, mobile);
    }

    /**
     * 检测电话号码
     * @param phone
     * @return
     */
    public static boolean checkPhone(String phone)
    {
        String regex = "(\\+\\d+)?(\\d{3,4}\\-?)?\\d{7,8}$";
        return Pattern.matches(regex, phone);
    }

    /**
     * 检测数字 \-?[1-9]\d+
     * @param digit
     * @return
     */
    public static boolean checkDigit(String digit)
    {
//        String regex = "\\-?[1-9]\\d+";
//        return Pattern.matches(regex, digit);

        Matcher matcher = Pattern.compile("^[0-9]+$").matcher(digit);
        return matcher.matches();
    }

    /**
     * 检测小数
     * @param decimals
     * @return
     */
    public static boolean checkDecimals(String decimals)
    {
        String regex = "\\-?[1-9]\\d+(\\.\\d+)?";
        return Pattern.matches(regex, decimals);
    }

    /**
     * 检测 匹配任何不可见字符，包括空格、制表符、换页符等等
     * @param blankSpace
     * @return
     */
    public static boolean checkBlankSpace(String blankSpace)
    {
        String regex = "\\s+";
        return Pattern.matches(regex, blankSpace);
    }

    /**
     * 检测中文
     * @param chinese
     * @return
     */
    public static boolean checkChinese(String chinese)
    {
        String regex = "^[\u4E00-\u9FA5]+$";
        return Pattern.matches(regex, chinese);
    }

    /**
     * 检测生日
     * @param birthday
     * @return
     */
    public static boolean checkBirthday(String birthday)
    {
        String regex = "[1-9]{4}([-./])\\d{1,2}\\1\\d{1,2}";
        return Pattern.matches(regex, birthday);
    }

    /**
     * 检测url的合法性
     * @param url
     * @return
     */
    public static boolean checkURL(String url)
    {
//        String regex = "(https?://(w{3}\\.)?)?\\w+\\.\\w+(\\.[a-zA-Z]+)*(:\\d{1,5})?(/\\w*)*(\\??(.+=.*)?(&.+=.*)?)?";
        String regex = "(http|https):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?";
        return Pattern.matches(regex, url);
    }

    /**
     * 检测域名的合法性
     * @param url
     * @return
     */
    public static String getDomain(String url)
    {
        Pattern p = Pattern.compile("(?<=http://|\\.)[^.]*?\\.(com|cn|net|org|biz|info|cc|tv)", Pattern.CASE_INSENSITIVE);
        // 获取完整的域名
        // Pattern p=Pattern.compile("[^//]*?\\.(com|cn|net|org|biz|info|cc|tv)", Pattern.CASE_INSENSITIVE);
        Matcher matcher = p.matcher(url);
        matcher.find();
        return matcher.group();
    }

    /**
     * 检测邮编
     * @param postcode
     * @return
     */
    public static boolean checkPostcode(String postcode)
    {
        String regex = "[1-9]\\d{5}";
        return Pattern.matches(regex, postcode);
    }

    /**
     * 检测ip地址
     * @param ipAddress
     * @return
     */
    public static boolean checkIpAddress(String ipAddress)
    {
        String regex = "[1-9](\\d{1,2})?\\.(0|([1-9](\\d{1,2})?))\\.(0|([1-9](\\d{1,2})?))\\.(0|([1-9](\\d{1,2})?))";
        return Pattern.matches(regex, ipAddress);
    }

    /**
     * 测试 str 是否只包含字母和数字
     *
     * @param str
     * @return
     */
    public static boolean IsLetterOrFigure(String str)
    {
        Matcher matcher = Pattern.compile("^[a-zA-Z0-9]+$").matcher(str);
        return matcher.matches();
//        return Pattern.matches(str, "^[a-zA-Z0-9]+$");

    }

    /**
     * 测试 str 是否只包含字母
     *
     * @param str
     * @return
     */
    public static boolean IsLetter(String str)
    {
        Matcher matcher = Pattern.compile("^[a-zA-Z]+$").matcher(str);
        return matcher.matches();
//        return Pattern.matches(str, "^[a-zA-Z]+$");
    }

    /**
     * 测试 str 是否只包含字母，且不含有[i][o]的字符
     *
     * @param str
     * @return
     */
    public static boolean IsLetterNotIO(String str)
    {
        Matcher matcher = Pattern.compile("[a-hj-np-zA-HJ-NP-Z0]+$").matcher(str);
        return matcher.matches();
//        return Pattern.matches(str, "[a-hj-np-zA-HJ-NP-Z0]+$");
    }

    /**
     * 测试 str 是否只包含字母或者数字，且不含有[i][o]的字符
     *
     * @param str
     * @return
     */
    public static boolean IsLetterOrFigureNotIO(String str)
    {
        Matcher matcher = Pattern.compile("^[a-hj-np-zA-HJ-NP-Z0-9]+$").matcher(str);
        return matcher.matches();
//        return Pattern.matches(str, "^[a-hj-np-zA-HJ-NP-Z0-9]+$");
    }

}
