using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public class TaskMgr
{
    public TaskMgr m_TaskMgr;

    public TaskDataMgr m_TaskData;
    public TaskDemandMgr m_TaskAsk;

    public void Init(int[] taskIDs = null, int[] taskStates = null, List<int[]> askParam = null)
    {
        m_TaskAsk.Init(this);
        //初始化所有的任务
        m_TaskData = new TaskDataMgr(taskIDs, taskStates, askParam , this);
        
        //初始化所有需要监听领取条件的任务

        //得到所有（已激活）正在等待领取、已领取、等待领奖、已结束、已废弃的任务
        //并根据各任务的状态，设置信息和进行监听
        //.........


        //任务激活配置表。。记录了由事件激活的支线任务头（支线的后续任务，由前置任务进行线性激活）
        //遍历所有有激活条件的任务，排除已完成和废弃的，进行监听。

    }




}

