package com.example.administrator.myparkingos.constant;

import android.util.Log;

/**
 * Created by Administrator on 2017-04-08.
 */
public enum RCode
{
    /// <summary>
    /// 重复入场
    /// </summary>
    RepeatAdmission(-3),
    /// <summary>
    /// 月租车过期按临时车计费
    /// </summary>
    MthBeOverdueToTmpCharge(-2),
    /// <summary>
    /// 月租车满位按临时车计费
    /// </summary>
    MthFullToTmpCharge(-1),
    /// <summary>
    /// 成功
    /// </summary>
    OK(200),
    /// <summary>
    /// token 已经失效
    /// </summary>
    TokenInvalid(40000),
    /// <summary>
    /// 未知异常，请联系管理员查看异常日志
    /// </summary>
    UnknownError(40001),
    /// <summary>
    /// 输入参数缺失
    /// </summary>
    ParameterMissing(40002),
    /// <summary>
    /// token已过期
    /// </summary>
    TokenBeOverdue(40003),
    /// <summary>
    /// 用户重复登录
    /// </summary>
    UserRepeatLogin(40004),
    /// <summary>
    /// 工作站重复
    /// </summary>
    StationRepeat(40005),
    /// <summary>
    /// 无权限
    /// </summary>
    NoPermission(40006),
    /// <summary>
    /// 已过有效期
    /// </summary>
    BeOverdue(40007),
    /// <summary>
    /// 没有通过此车道的权限
    /// </summary>
    NoThisLanePermission(40008),

    /// <summary>
    /// 个人车位已满
    /// </summary>
    PersonalFull(40010),
    /// <summary>
    /// 禁止通行
    /// </summary>
    ProhibitCurrent(40011),
    /// <summary>
    /// 余额不足
    /// </summary>
    BalanceNotEnough(40012),
    /// <summary>
    /// 黑名单车辆
    /// </summary>
    BlackList(40013),
    /// <summary>
    /// 禁止开闸模式
    /// </summary>
    ProhibitCutOff(40014),
    /// <summary>
    /// 月租车车位已满
    /// </summary>
    MonthCarFull(40015),
    /// <summary>
    ///临时车车位已满
    /// </summary>
    TemporaryCarFull(40016),
    /// <summary>
    /// 储值车车位已满
    /// </summary>
    PrepaidCarFull(40017),
    /// <summary>
    /// 总车位已满位
    /// </summary>
    SummaryCarFull(40018),

    /// <summary>
    /// 全字母车牌不处理
    /// </summary>
    AllLetterPlateNoHandle(40020),
    /// <summary>
    /// 全字符相同车牌不处理
    /// </summary>
    AllCharacterSamePlateNoHandle(40021),

    /// <summary>
    /// 临时车禁止入小车场
    /// </summary>
    TemporaryCarNotInSmall(40026),
    /// <summary>
    /// 确认开闸模式
    /// </summary>
    ConfirmCutOff(40027),

    /// <summary>
    /// 月租车车位已满确认开闸
    /// </summary>
    MonthCarFullConfirmCutOff(40030),
    /// <summary>
    /// 临时车车位已满确认开闸
    /// </summary>
    TemporaryCarFulllConfirmCutOff(40031),
    /// <summary>
    /// 储值车车位已满确认开闸
    /// </summary>
    PrepaidCarFullConfirmCutOff(40032),
    /// <summary>
    /// 所有车车位已满确认开闸
    /// </summary>
    SummaryCarFullConfirmCutOff(40033),

    /// <summary>
    /// 未找到入场记录
    /// </summary>
    NotFoundApproachRecord(40040);

    private int value;

    private RCode(int inValue)
    {
        value = inValue;
    }

    private RCode()
    {

    }

    public int getValue()
    {
        return value;
    }

    public void setValue(int value)
    {
        this.value = value;
    }

    @Override
    public String toString()
    {
        final StringBuffer sb = new StringBuffer("RCode{");
        sb.append("value(").append(value);
        sb.append('}');
        return sb.toString();
    }

    public static RCode valueOf(int value) //    手写的从int到enum的转换函数
    {
        switch (value)
        {
            case -3:
                return RepeatAdmission;
            case -2:
                return MthBeOverdueToTmpCharge;
            case -1:
                return MthFullToTmpCharge;
            case 200:
                return OK;
            case 40000:
                return TokenInvalid;
            case 40001:
                return UnknownError;
            case 40002:
                return ParameterMissing;
            case 40003:
                return TokenBeOverdue;
            case (40004):
                return UserRepeatLogin;
            case (40005):
                return StationRepeat;
            case (40006):
                return NoPermission;
            case (40007):
                return BeOverdue;
            case (40008):
                return NoThisLanePermission;
            case (40010):
                return PersonalFull;
            case (40011):
                return ProhibitCurrent;
            case (40012):
                return BalanceNotEnough;
            case (40013):
                return BlackList;
            case (40014):
                return ProhibitCutOff;
            case (40015):
                return MonthCarFull;
            case (40016):
                return TemporaryCarFull;
            case (40017):
                return PrepaidCarFull;
            case (40018):
                return SummaryCarFull;
            case (40020):
                return AllLetterPlateNoHandle;
            case (40021):
                return AllCharacterSamePlateNoHandle;
            case (40026):
                return TemporaryCarNotInSmall;
            case (40027):
                return ConfirmCutOff;
            case (40030):
                return MonthCarFullConfirmCutOff;
            case (40031):
                return TemporaryCarFulllConfirmCutOff;
            case (40032):
                return PrepaidCarFullConfirmCutOff;
            case (40033):
                return SummaryCarFullConfirmCutOff;
            case 40040:
                return NotFoundApproachRecord;
            default:
            {
                Log.e("debug", value + "是错误的");
                break;
            }
        }
        return null;
    }

}
