using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 任务触发和完成的要求
/// </summary>
public class TaskDemandMgr
{
    public TaskMgr TaskMgr;
    public Dictionary<EvtItemData, List<int>> ListenDict = new Dictionary<EvtItemData, List<int>>();

    public void Init(TaskMgr mgr)
    {
        TaskMgr = mgr;
    }

    // 进行对某任务进行监听
    public void StartListen(TaskDataBase task, TaskStateType stateType)
    {
        //List<DemandData> askDataList = task.GetAskDataList();
        //for (int i = 0; i < askDataList.Count; i++)
        //{
        //    if (askDataList[i] == null) continue;
        //    //foreach (var item in askDataList[i].m_AskDic)//某一任务完成条件与失败条件，不能有相同的条件项
        //    //{
        //    //    TaskAskTypeData typeData = item.Key;
        //    //    SetDemandData(item.Value); //设置其值
        //    //    //添加到监听列表
        //    //    if (m_ListenDic.ContainsKey(typeData)) { m_ListenDic[typeData].Add(task.m_Id); }
        //    //    else { m_ListenDic.Add(typeData, new List<int> { task.m_Id }); }
        //    //}
        //}
        
        //if (IsTaskAskTrue(task))//如果已经满足条件
        //{
        //}
        //else
        //{ 
        //}
    }

    //某任务的所有条件项是否满足要求
    public bool IsTaskAskTrue(TaskDataBase task) 
    {
        DemandData askData = null;
        bool isAllTrue = true;
        //foreach (var item in askData.m_AskDic)
        //{
        //    TaskAskTypeData typeData = item.Key;
        //    if (!IsDemandTrue(task.GetAskItemByState(typeData, task.m_State)))
        //    {
        //        isAllTrue = false;
        //    }
        //}
        return isAllTrue;
    }

    //设置条件项的值
    public void SetDemandData(DemandHandler askItem)
    {
        //TaskAskType askType = askItem.m_TypeData.m_Type;
        //int askId = 0;
        //TaskAskTypeData typeData = new TaskAskTypeData(askType, askId);
        //switch (askType)
        //{
        //    case TaskAskType.PlayerLevelLess:
        //    case TaskAskType.PlayerLevelMore:
        //        askItem.m_CurParam = AppData.Instance.m_MyPlayerData.m_Level;
        //        break;

        //    case TaskAskType.RoleMaxLevelMore:
        //        break;
        //    case TaskAskType.GoldNum:
        //        askItem.m_CurParam = AppData.Instance.m_MyPlayerData.m_Money;
        //        break;
        //}
    }

    //某要求项是否满足
    public bool IsDemandTrue(DemandHandler askItem) 
    {
        if (askItem == null) return false;
        return askItem.m_IsTrue;
    }

    // 当某个任务相关的信息被修改，进行刷新
    public void Fresh(EvtItemData typeData, int param , bool isAdd=false)
    {
        //if (!m_ListenDic.ContainsKey(askTypeData)) return;

        //List<int> listen = m_ListenDic[askTypeData];
        //for (int i = 0; i < listen.Count; i++)
        //{
        //    //listen.m_TaskList[i].SetAskData(askTypeData, param, isAdd);
        //}
    }

    //移除监听列表
    public void RemoveListen(TaskDataBase task)
    {
        
    }
}
