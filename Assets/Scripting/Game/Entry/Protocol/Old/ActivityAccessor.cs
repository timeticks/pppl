using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityAccessor : DescObject
{
    public enum ActivityType
    {
        None,
        PrestigeTask,   // 声望任务
        Tower,          // 爬塔
        DailyDungeon,   // 每日秘境
    }

    public Dictionary<PrestigeLevel.PrestigeType ,List<int>> TaskMap = new Dictionary<PrestigeLevel.PrestigeType,List<int>>();

    public int TaskFreeFreshNum;
    public int TaskFinishNum;
    public int CurTaskIdx;
    public int CurTaskPos;
    public long TaskStartTime;

    public int TowerFloorIndex;        //当前已闯层数，初始为0；若要取当前可闯关卡需要+1
    public int TowerFailNum;         //失败次数
    public long TowerNextRefreshTime;

    public ActivityAccessor() { }

    public ActivityAccessor(ActivityAccessor origin)
    {
        this.TaskMap = origin.TaskMap;
        this.TaskFinishNum = origin.TaskFinishNum;
        this.CurTaskIdx = origin.CurTaskIdx;
        this.CurTaskPos = origin.CurTaskPos;
        this.TaskFreeFreshNum = origin.TaskFreeFreshNum;
        this.TaskStartTime = origin.TaskStartTime;
        this.TowerFloorIndex = origin.TowerFloorIndex;
        this.TowerFailNum = origin.TowerFailNum;
        this.TowerNextRefreshTime = origin.TowerNextRefreshTime;
    }

    public int GetRemainFreeFresh()//剩余可刷新的次数
    {
        return VipAddition.MAX_PRESTIGE_TASK_FREE_FRESH.getValueByVip(PlayerPrefsBridge.Instance.PlayerData.IsVip()) - TaskFreeFreshNum;
    }

    public int GetRemainFinish() //剩余可完成的任务数量
    {
        return VipAddition.MAX_PRESTIGE_TASK_NUM.getValueByVip(PlayerPrefsBridge.Instance.PlayerData.IsVip()) - TaskFinishNum;
    }
}
