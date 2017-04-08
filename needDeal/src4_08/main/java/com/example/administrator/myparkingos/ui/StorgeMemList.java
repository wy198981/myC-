package com.example.administrator.myparkingos.ui;

/**
 * Created by Administrator on 2017-03-28.
 */
public class StorgeMemList
{
    static private StorgeMemList mStoreMemList = new StorgeMemList();
    // 存储的内存数据列表数据
    private StorgeMemList()
    {

    }

    public static StorgeMemList getInstance()
    {
        return mStoreMemList;
    }



}
