using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;


public enum TaskStateType : byte// 任务状态
{
    [EnumDesc("未激活")]
    Inactive,
    [EnumDesc("不可领取")] //不满足领取条件..自动检测其领取条件。当某任务被激活后，即为不可领取状态
    Unacceptable,
    [EnumDesc("可领取")]   //满足领取条件
    Acceptable,
    [EnumDesc("待完成")]
    Completing,
    [EnumDesc("已完成、待领奖")]
    Finished,
    [EnumDesc("已奖励")]
    Rewarded,
    [EnumDesc("已废弃")]
    Rejected,
}

 
public enum TaskType : byte// 任务类型
{
    [EnumDesc("新手任务")]
    Newer,
    [EnumDesc("日常任务")]
    Daily,
    [EnumDesc("主线任务")]
    MainLine,
    [EnumDesc("支线任务")]
    Branch,
}


public class TaskDataBase:TrueHandler // 多线跳转任务数据
{
    public Eint m_Id;   //根据id，分为两个区域：需要被特定事件激活的，进行检测激活的
    public string m_Name;
    public TaskType m_Type;
    public TaskStateType m_State;      //任务状态
    public Dictionary<TaskStateType, DemandData> m_DemandDict = new Dictionary<TaskStateType, DemandData>(); //条件--满足条件时跳转的状态

    public TaskDataBase(int id, System.Action<object> switchDeleg, TaskStateType state = TaskStateType.Unacceptable, int[] askParams = null)
    {
        Register(switchDeleg);
        m_Id = id;
        m_Name = "任务名字" + id;
        m_Type = TaskType.Branch;
        m_State = state;

        //1.根据当前状态，初始化条件
        InitDemand(state, id);

        //2.根据askParams，设置当前任务的条件参数
        
        //3.设置监听
    }

    //根据状态，重置其条件信息。某些状态有多种目标状态
    public void InitDemand(TaskStateType curState, int id)
    {
        m_DemandDict = null;
        m_DemandDict = new Dictionary<TaskStateType, DemandData>();
        switch (curState)
        {
            case TaskStateType.Inactive:
                m_DemandDict.Add(TaskStateType.Unacceptable, new DemandData(id, TaskStateType.Unacceptable , Handle));
                break;
            case TaskStateType.Unacceptable:
                break;
            case TaskStateType.Acceptable:
                break;
            case TaskStateType.Completing:
                m_DemandDict.Add(TaskStateType.Finished, new DemandData(id, TaskStateType.Finished, Handle));
                m_DemandDict.Add(TaskStateType.Rejected, new DemandData(id, TaskStateType.Rejected, Handle));
                break;
            case TaskStateType.Finished:
                m_DemandDict.Add(TaskStateType.Finished, new DemandData(id, TaskStateType.Finished, Handle));
                break;
            case TaskStateType.Rewarded:
                break;
            case TaskStateType.Rejected:
                break;
            default:
                break;
        }
    }


    //得到任务的某个阶段的条件
    public DemandData GetAskDataByState(TaskStateType stateType)
    {
        DemandData curAsk = null;
        int count = m_DemandDict.Count;
        if (m_DemandDict.ContainsKey(stateType))//如果有
        {
            return m_DemandDict[stateType];
        }
        switch (stateType)//如果没有，读配置表生成
        {
            case TaskStateType.Inactive:
                break;
            case TaskStateType.Unacceptable:
                break;
            case TaskStateType.Acceptable:
                break;
            case TaskStateType.Completing:
                break;
            case TaskStateType.Rewarded:
                break;
            case TaskStateType.Rejected:
                break;
            default:
                break;
        }

        return curAsk;
    }


    public override void Handle(object val)//某个条件为真时。任务进行跳转通知
    {
        try
        {
            DemandData demand = (DemandData)val;
            foreach (var item in m_DemandDict)
            {
                if (item.Value != null)
                {
                    if (item.Value == demand)
                    {
                        if (m_TrueDeleg!=null) m_TrueDeleg(m_Id);
                    }
                }
            }
        }
        catch (Exception)
        {
            TDebug.LogErrorFormat("{0}task's demand is error", m_Id.ToString());
            throw;
        }
    }

    public virtual void Active() { }
    public virtual void Receive() { }
    public virtual void Finish() { }
    public virtual void Over() { }
    public virtual void SetInactive() { }
}