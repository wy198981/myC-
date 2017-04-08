package com.example.administrator.myparkingos.util;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Created by Administrator on 2017-03-28.
 */
public class ExeUtil
{
    // 创建固定线程数的 excutor，如果线程数不够，会出现直接拒绝；
    ExecutorService executorService= java.util.concurrent.Executors.newFixedThreadPool(Runtime.getRuntime().availableProcessors());

    /**
     * 投递任务
     * @param r
     */
    public void post(Runnable r)
    {
        executorService.submit(r);
    }

    /**
     * 关闭，还不完善
     */
    public void shutDown()
    {
        if (!executorService.isTerminated())
        {
            executorService.shutdown();
        }
    }
}
