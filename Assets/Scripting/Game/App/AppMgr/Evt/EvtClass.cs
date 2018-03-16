using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EvtGroupData
{
    public List<EvtItemData> m_EvtList;   
    public List<bool> m_MeetList;      //条件是否完成
    public EvtGroupData(List<EvtItemData> evtList)
    {
        m_EvtList = evtList;
        m_MeetList = new List<bool>();
        for (int i = 0; i < m_EvtList.Count; i++)
        {
            m_MeetList.Add(false);
        }
    }
    public bool IsMeet()  //是否所有事件满足
    {
        for (int i = 0; i < m_MeetList.Count; i++)
        {
            if (!m_MeetList[i]) { return m_MeetList[i]; }
        }
        return true;
    }
}


//事件类型数据，都是唯一的
public class EvtItemData  
{
    public EvtType EventType;
    public string  Val;
    //public bool    CanKeep;   //监听事件时，是否调用一次后移除监听
    public EvtItemData(EvtType evt, string val="")
    {
        EventType = evt;
        Val = val;
    }
    public string ToTypeKey()
    {
        return EventType.ToString();
    }
    public string ToValKey()//Key值中，如果可保持1
    {
        return string.Format("{0}{1}", EventType.ToString(), Val);
    }
    //private string ToKey(bool canKeep)
    //{
    //    return string.Format("{0}{1}{2}", EventType.ToString() , Val , (canKeep ? 1 : 0));
    //}
}

//事件类型
public enum EvtType
{
    LevelUp,             //升级  val为等级
    CurLevel,            //等级修改        
    LevelEqualMore,      //等级大于等于....有一些事件需要在条件生成时主动获取
    LevelEqualLess,      //等级小于等于

    CurGold,             //当前金币数改变                 val为当前金币数
    GetGold,             //得到金币         Id无意义      val为增加金币数
    UseGold,             //使用                           val为使用金币数
    CurDiamnod,          //使用钻石
    CurPay,              //充值
    
    CurItem,             //拥有某道具
    GetItem,             //得到某道具       (道具id)      val为数量
    UseItem,             //使用某道具                     val为数量
                         
    ChatNpc,             //和npc对话        (npc的id)
    Chat,                //完成某对话       (对话id)
                         
    FinishGuide,         //某新手引导完成   (引导id)
    FinishTravelEvent,          //某任务完成       (任务id)
                         
    //Kill,                //杀敌             (敌人id)
    //FirstPass,           //首次通关         (关卡id)
    //PassLevel,           //通关             (关卡id)
                         
    OpenWin,             //打开窗口  val为窗口名   objs为打开窗口的参数列表
    CloseWin,            
                         
    FirstGame,           //首次进入游戏（本地保存）
    EnterScene,          //进入某场景
    FirstEnterLobby,     //第一次进入大厅
    EnterLobby,          //进入大厅

    EnterAnyBattle,      //进入任何战斗

    ProductFinish,       //生产完成
    ZazenFinish,         //打坐完成
    RetreatFinish,       //闭关完成
    MapEventCanReward,   //有秘境事件可领奖
    TravelNewSite,      // 游历解锁新地点

    ChangePrefsKey,     //SaveUtils中修改某key值
    TravelNewEvent,     // 游历触发事件

    AchieveCanReward,   //有成就奖励可以领取

    PrestigeTaskFinish,

    Max = 65535,
}

//事件对应条件的为真类型
public enum DemandTrueType : byte  
{
    Number,   //条件值等于事件传递的值，>=目标值为真
    Delta,    //条件值加上事件传递的值，>=目标值为真
    Once      //条件值直接为1，>=1则为真
}
public static class EvtTypeExtension
{
    public static DemandTrueType GetTrueType(this EvtType ty) //为真类型
    {
        switch (ty)
        {
            case EvtType.LevelUp:
            case EvtType.GetGold:
            case EvtType.UseGold:
            case EvtType.GetItem:
            case EvtType.UseItem:
                return DemandTrueType.Delta;

            case EvtType.CurLevel:
            case EvtType.CurGold:
            case EvtType.CurDiamnod:
            case EvtType.CurPay:
            case EvtType.CurItem:
                return DemandTrueType.Number;

            case EvtType.OpenWin:
            case EvtType.CloseWin:
            case EvtType.ChatNpc:
            case EvtType.Chat:
            case EvtType.FinishGuide:
            case EvtType.FinishTravelEvent:
            case EvtType.FirstGame:
            case EvtType.EnterScene:
            //case EvtType.FirstEnterPort:
            //case EvtType.FirstEnterArea:
            case EvtType.EnterLobby:
                return DemandTrueType.Once;

            case EvtType.Max:
            default:
                return DemandTrueType.Once;
        }
    }

    public static bool CanReqNotice(this EvtType ty)//是否可以主动请求查看事件的当前状态
    {
        switch (ty)
        {
            case EvtType.CurLevel:
            case EvtType.LevelEqualMore:
            case EvtType.LevelEqualLess:
            case EvtType.CurGold:
            case EvtType.FinishGuide:
            case EvtType.FinishTravelEvent:
            case EvtType.CurDiamnod:
            case EvtType.CurPay:
            case EvtType.CurItem:
            case EvtType.FirstGame:
                return true;

            case EvtType.LevelUp:
            case EvtType.GetGold:
            case EvtType.UseGold:
            case EvtType.GetItem:
            case EvtType.UseItem:
            case EvtType.ChatNpc:
            case EvtType.Chat:
            case EvtType.OpenWin:
            case EvtType.CloseWin:
            case EvtType.EnterScene:
            case EvtType.EnterLobby:
                return false;

            case EvtType.Max:
                break;
            default:
                break;
        }
        return true;
    }
}