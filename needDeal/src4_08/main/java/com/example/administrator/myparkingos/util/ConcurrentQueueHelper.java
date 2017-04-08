package com.example.administrator.myparkingos.util;

import com.example.administrator.myparkingos.model.ModelNode;

import java.util.concurrent.ConcurrentLinkedQueue;

/**
 * Created by Administrator on 2017-03-09.
 */
public class ConcurrentQueueHelper
{
    /**
     * ConcurrentQueueHelper 具有多线程执行安全性，且不会出现阻塞；
     */
    private static volatile ConcurrentQueueHelper concurrentQueueHelper = null;

    private ConcurrentLinkedQueue<ModelNode> container = new ConcurrentLinkedQueue<ModelNode>();

    private ConcurrentQueueHelper()
    {

    }

    public static ConcurrentQueueHelper getInstance()
    {
        if (concurrentQueueHelper == null)
        {
            synchronized (ConcurrentQueueHelper.class)
            {
                if (concurrentQueueHelper == null)
                {
                    concurrentQueueHelper = new ConcurrentQueueHelper();
                }
            }
        }
        return concurrentQueueHelper;
    }

    public void put(ModelNode node)
    {
        container.offer(node);
    }

    public ModelNode get()
    {
        return container.poll();
    }

    public void destory()
    {
        container.clear();
    }
}
