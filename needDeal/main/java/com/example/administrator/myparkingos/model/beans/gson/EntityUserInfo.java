package com.example.administrator.myparkingos.model.beans.gson;

import java.util.List;

/**
 * Created by Administrator on 2017-03-20.
 */
public class EntityUserInfo
{


    /**
     * rcode : 200
     * msg : OK
     * data : [{"DeptName":"管理部","UserNO":"80052","UserName":"TTTT","DeptId":1,"Sex":"男","HomeAddress":"","Job":"操作员","WorkTime":"2017-02-13 00:00:00","BirthDate":"2017-02-13 00:00:00","IDCard":"","MaritalStatus":"未婚","Skill":"","TelNumber":"","MobNumber":"","NativePlace":"","CarPlaceNo":1,"ID":33}]
     */

    /**
     * DeptName : 管理部
     * UserNO : 80052
     * UserName : TTTT
     * DeptId : 1
     * Sex : 男
     * HomeAddress :
     * Job : 操作员
     * WorkTime : 2017-02-13 00:00:00
     * BirthDate : 2017-02-13 00:00:00
     * IDCard :
     * MaritalStatus : 未婚
     * Skill :
     * TelNumber :
     * MobNumber :
     * NativePlace :
     * CarPlaceNo : 1
     * ID : 33
     */

    private String DeptName;
    private String UserNO;
    private String UserName;
    private int DeptId;
    private String Sex;
    private String HomeAddress;
    private String Job;
    private String WorkTime;
    private String BirthDate;
    private String IDCard;
    private String MaritalStatus;
    private String Skill;
    private String TelNumber;
    private String MobNumber;
    private String NativePlace;
    private int CarPlaceNo;
    private int ID;

    public String getDeptName()
    {
        return DeptName;
    }

    public void setDeptName(String DeptName)
    {
        this.DeptName = DeptName;
    }

    public String getUserNO()
    {
        return UserNO;
    }

    public void setUserNO(String UserNO)
    {
        this.UserNO = UserNO;
    }

    public String getUserName()
    {
        return UserName;
    }

    public void setUserName(String UserName)
    {
        this.UserName = UserName;
    }

    public int getDeptId()
    {
        return DeptId;
    }

    public void setDeptId(int DeptId)
    {
        this.DeptId = DeptId;
    }

    public String getSex()
    {
        return Sex;
    }

    public void setSex(String Sex)
    {
        this.Sex = Sex;
    }

    public String getHomeAddress()
    {
        return HomeAddress;
    }

    public void setHomeAddress(String HomeAddress)
    {
        this.HomeAddress = HomeAddress;
    }

    public String getJob()
    {
        return Job;
    }

    public void setJob(String Job)
    {
        this.Job = Job;
    }

    public String getWorkTime()
    {
        return WorkTime;
    }

    public void setWorkTime(String WorkTime)
    {
        this.WorkTime = WorkTime;
    }

    public String getBirthDate()
    {
        return BirthDate;
    }

    public void setBirthDate(String BirthDate)
    {
        this.BirthDate = BirthDate;
    }

    public String getIDCard()
    {
        return IDCard;
    }

    public void setIDCard(String IDCard)
    {
        this.IDCard = IDCard;
    }

    public String getMaritalStatus()
    {
        return MaritalStatus;
    }

    public void setMaritalStatus(String MaritalStatus)
    {
        this.MaritalStatus = MaritalStatus;
    }

    public String getSkill()
    {
        return Skill;
    }

    public void setSkill(String Skill)
    {
        this.Skill = Skill;
    }

    public String getTelNumber()
    {
        return TelNumber;
    }

    public void setTelNumber(String TelNumber)
    {
        this.TelNumber = TelNumber;
    }

    public String getMobNumber()
    {
        return MobNumber;
    }

    public void setMobNumber(String MobNumber)
    {
        this.MobNumber = MobNumber;
    }

    public String getNativePlace()
    {
        return NativePlace;
    }

    public void setNativePlace(String NativePlace)
    {
        this.NativePlace = NativePlace;
    }

    public int getCarPlaceNo()
    {
        return CarPlaceNo;
    }

    public void setCarPlaceNo(int CarPlaceNo)
    {
        this.CarPlaceNo = CarPlaceNo;
    }

    public int getID()
    {
        return ID;
    }

    public void setID(int ID)
    {
        this.ID = ID;
    }

    @Override
    public String toString()
    {
        return "EntityUserInfo{" +
                "DeptName='" + DeptName + '\'' +
                ", UserNO='" + UserNO + '\'' +
                ", UserName='" + UserName + '\'' +
                ", DeptId=" + DeptId +
                ", Sex='" + Sex + '\'' +
                ", HomeAddress='" + HomeAddress + '\'' +
                ", Job='" + Job + '\'' +
                ", WorkTime='" + WorkTime + '\'' +
                ", BirthDate='" + BirthDate + '\'' +
                ", IDCard='" + IDCard + '\'' +
                ", MaritalStatus='" + MaritalStatus + '\'' +
                ", Skill='" + Skill + '\'' +
                ", TelNumber='" + TelNumber + '\'' +
                ", MobNumber='" + MobNumber + '\'' +
                ", NativePlace='" + NativePlace + '\'' +
                ", CarPlaceNo=" + CarPlaceNo +
                ", ID=" + ID +
                '}';
    }
}
