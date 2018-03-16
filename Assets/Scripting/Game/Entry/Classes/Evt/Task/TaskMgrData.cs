using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TaskDataMgr
{
    public TaskMgr m_TaskMgr;
    public Dictionary<string, List<int>> m_TaskAskDict = new Dictionary<string, List<int>>();
    public Dictionary<int, TaskStateType> m_TaskDict = new Dictionary<int, TaskStateType>();
    public Dictionary<TaskStateType, Dictionary<int, TaskDataBase>> m_TaskDataDict = new Dictionary<TaskStateType, Dictionary<int, TaskDataBase>>()
    {
        {TaskStateType.Inactive, new Dictionary<int, TaskDataBase>()},
        {TaskStateType.Unacceptable, new Dictionary<int, TaskDataBase>()},  //未激活
        {TaskStateType.Acceptable, new Dictionary<int, TaskDataBase>()},  //已激活
        {TaskStateType.Completing, new Dictionary<int, TaskDataBase>()}, 
        {TaskStateType.Rewarded, new Dictionary<int, TaskDataBase>()}, 
        {TaskStateType.Rejected, new Dictionary<int, TaskDataBase>()}
    };

    //移除出列表
    public void RemoveTaskInDic(int id , TaskStateType state) 
    {
        if (m_TaskDataDict[state].ContainsKey(id))
        {
            m_TaskDataDict[state].Remove(id);
            //移除监听
        }
        else { TDebug.LogError("m_TaskDataDic[state]不存在此任务" + state+"   "+id); }
        if (m_TaskDict.ContainsKey(id))
        {
            m_TaskDict.Remove(id);
        }
        else { TDebug.LogError("m_TaskDic不存在此任务" + state + "   " + id); }
    }

    //添加进列表
    public void AddTaskInDic(TaskDataBase task, TaskStateType state)
    {
        if (!m_TaskDataDict[state].ContainsKey(task.m_Id))
        {
            m_TaskDataDict[state].Add(task.m_Id, task);
        }
        else { TDebug.LogError("m_TaskDataDic[state]已存在此任务" + state + "   " + task.m_Id); }
        if (!m_TaskDict.ContainsKey(task.m_Id))
        {
            m_TaskDict.Add(task.m_Id, state);
        }
        else { TDebug.LogError("m_TaskDic已存在此任务" + state + "   " + task.m_Id); }
    }

    //根据有状态的任务列表，进行初始化信息
    public TaskDataMgr(int[] taskIDs , int[] taskStates, List<int[]> askParam  , TaskMgr taskMgr)
    {
        /////////////////从任务初始化开始，一步步进行~！！！！
        m_TaskMgr = taskMgr;
        if (taskIDs != null)
        {
            if (!(taskIDs.Length == taskStates.Length && taskStates.Length == askParam.Count))//如果长度不匹配
            {
                TDebug.LogError("任务参数错误" + taskIDs.Length + "   " + taskStates.Length + "  " + askParam.Count);
            }
            for (int i = 0; i < taskIDs.Length; i++)
            {
                TaskDataBase task = new TaskDataBase(taskIDs[i], null, (TaskStateType)taskStates[i], askParam[i]);
                task.m_State = (TaskStateType)taskStates[i];
            }
        }
    }

    //根据id，获取任务信息
    public TaskDataBase GetTask(int taskId)
    {
        if (m_TaskDict.ContainsKey(taskId))
        {
            TaskStateType state = m_TaskDict[taskId];
            return m_TaskDataDict[state][taskId];
        }
        else { TDebug.LogError("m_TaskDic不存在此任务" + "   " + taskId); }
        return null;
    }

    //切换任务状态、重置监听
    public void TrySwitchState(TaskStateType toState, int taskId)
    {
        TaskStateType fromState = m_TaskDict[taskId];
        if (fromState == toState) return;

        TaskDataBase task = GetTask(taskId);
        if (task == null) return;
        RemoveTaskInDic(taskId, fromState);//移除原状态列表
        //移除原有监听

        //修改任务项的状态
        //添加新的监听

        //添加到目标状态列表
        AddTaskInDic(task, toState);
    }

}