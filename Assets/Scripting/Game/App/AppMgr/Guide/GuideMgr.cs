using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 新手引导管理器
/// </summary>
public class GuideMgr : MonoBehaviour
{
    public static GuideMgr Instance;
    public bool IsNewer;
    public bool IsFirstInDevice;  //在此设备中第一次登录
    void Awake()
    {
        Instance = this; 
    }

    #region 功能解锁
    
    public void Init ()
	{
        IsFirstInDevice = SaveUtils.GetIntInPlayer("IsFirstInDevice", 0) == 0;
        IsNewer = PlayerPrefsBridge.Instance.PlayerData.Level == 1;
        if (IsFirstInDevice)//如果是玩家第一次进入此设备，则初始将所有功能刷新
	    {
            SaveUtils.SetIntInPlayer("IsFirstInDevice", 1);
            int level = PlayerPrefsBridge.Instance.PlayerData.Level;
            for (int i = 0, length = (int)ModuleType.Max; i < length; i++)
            {
                BadgeStatus ty = BadgeStatus.NoOpen;
                ModuleType moduleTy = (ModuleType)i;
                int unlockLevel = GameConstUtils.GetNewModuleUnlockLevel(moduleTy);
                if (level >= unlockLevel)
                {
                    ty = IsNewer ? BadgeStatus.ShowBadge : BadgeStatus.Normal;
                }
                else
                {
                    ty = BadgeStatus.NoOpen;
                }
                SaveUtils.SetIntInPlayer(moduleTy.ToString(), (int)ty);
            }
	    }
	    FreshModuleUnlock(null);
	}

    public void FreshModuleUnlock(object obj) //刷新某个功能被解锁
    {
        bool isLevelUp = obj != null;
        int level = PlayerPrefsBridge.Instance.PlayerData.Level;
        for (int i = 0, length = (int)ModuleType.Max; i < length; i++) //遍历等级，查看功能解锁
        {
            BadgeStatus ty = BadgeStatus.NoOpen;
            ModuleType moduleTy = (ModuleType)i;
            int unlockLevel = GameConstUtils.GetNewModuleUnlockLevel(moduleTy);
            bool isChanged = false;
            if(level >=unlockLevel && SaveUtils.GetIntInPlayer(moduleTy.ToString()) == 0)
            {
                if (isLevelUp) //如果是升级上去的，则设红点
                    SaveUtils.SetIntInPlayer(moduleTy.ToString(), (int)BadgeStatus.ShowBadge);
                else
                    SaveUtils.SetIntInPlayer(moduleTy.ToString(), (int)BadgeStatus.Normal);
                isChanged = true;
            }
            if (isChanged)
            {
                GuidePointUI pointUi = GetGuidePointByModule(moduleTy);
                if (PointPool.ContainsKey(pointUi))
                {
                    BadgeStatus status = Fresh(pointUi, PointPool[pointUi]);
                    TDebug.Log(string.Format("刷新解锁:{0}--{1}",pointUi, status));
                }
            }
        }
        AppEvtMgr.Instance.Register(new EvtItemData(EvtType.LevelUp), EvtListenerType.GuideModuleOpen, FreshModuleUnlock , false);
    }

    #endregion

    void Update()
    {

    }


    //public void HighLitObj(GameObject obj)//高亮某物体，并且设置点击时的回调
    //{
    //    obj.AddComponent<Canvas>().overrideSorting = true;
    //    obj.AddComponent<GraphicRaycaster>();
    //    AddTriggersListener(obj, EventTriggerType.PointerClick, HighLitClick); 
    //}
    //public void HighLitClick(UnityEngine.EventSystems.BaseEventData baseEvent)
    //{ 
    //    Debug.Log(baseEvent.selectedObject.name + " triggered an event!"); 
    //}

    /// <summary>
    /// 设置监听
    /// </summary>
    public static void AddTriggersListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger;
        if (obj.GetComponent<EventTrigger>() != null)
        {
            trigger = obj.GetComponent<EventTrigger>();
        }
        else
        {
            trigger = obj.AddComponent<EventTrigger>();
        }
        if (trigger.triggers.Count == 0)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }
        
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }


    private Dictionary<GuidePointUI, Transform> PointPool = new Dictionary<GuidePointUI, Transform>();//UI控件与其transform
    public void SetUI(GuidePointUI pointTy, Transform trans)
    {
        if (trans == null) return;
        if (PointPool.ContainsKey(pointTy) && PointPool[pointTy]!=null) return;

        if (PointPool.ContainsKey(pointTy)) PointPool[pointTy] = trans;
        else  PointPool.Add(pointTy, trans);
        BadgeStatus status = Fresh(pointTy, trans);
        //TDebug.Log(string.Format("SetUI刷新:{0}:{1}",pointTy, status));
    }

    public BadgeStatus Fresh(GuidePointUI pointTy, Transform pointTrans ,params object[] attach)
    {
        switch (pointTy)
        {
            case GuidePointUI.Lobby_CaveBtn://洞府按钮
            {
                BadgeStatus status = CheckAndSetModuleStatus(pointTrans, ModuleType.module_cave, false);
                if (status != BadgeStatus.Normal)
                {
                    return status;
                }
                //将所有红点隐藏，重新计算红点信息
                BadgeTips.SetBadgeViewFalse(pointTrans);

                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.ProductFinish), EvtListenerType.ProductFinish,
                    delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });  //生成制作红点

                System.Action<object> levelUpDel = delegate(object o)//闭关红点
                {
                    if (HeroLevelUp.CanRetreat(PlayerPrefsBridge.Instance.PlayerData.Level, PlayerPrefsBridge.Instance.PlayerData.Exp))
                    {  BadgeTips.SetBadgeView(pointTrans); }
                };
                levelUpDel(PlayerPrefsBridge.Instance.PlayerData.Level);//先查看一次
                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.LevelUp), EvtListenerType.CanRetreat, levelUpDel); //闭关

                break;
            }
            case GuidePointUI.Lobby_WorldBtn:
            {
                //if (CheckAndSetModuleStatus(pointTrans, ModuleType.module_travel) == BadgeStatus.NoOpen)
                //    return BadgeStatus.NoOpen;

                //将所有红点隐藏，重新计算红点信息
                BadgeTips.SetBadgeViewFalse(pointTrans);

                //秘境事件可领取红点
                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.MapEventCanReward), EvtListenerType.MapEventCanReward,
                    delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });

                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.TravelNewSite), EvtListenerType.TravelNewSite,
                   delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });

                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.TravelNewEvent), EvtListenerType.TravelNewEvent,
                  delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });
                break;
            }
            case GuidePointUI.Lobby_RankBtn:
            {
                BadgeStatus status = CheckAndSetModuleStatus(pointTrans, ModuleType.module_rank, false);
                if (status != BadgeStatus.Normal)
                {
                    return status;
                }
                break;
            }
            case GuidePointUI.Lobby_ShopBtn:
            {
                BadgeStatus status = CheckAndSetModuleStatus(pointTrans, ModuleType.module_shop, false);
                if (status != BadgeStatus.Normal)
                {
                    return status;
                }
                break;
            }
            case GuidePointUI.Lobby_ActivityBtn:
            {
                BadgeStatus status = CheckAndSetModuleStatus(pointTrans, ModuleType.module_activity, false);
                if (status != BadgeStatus.Normal)
                {
                    return status;
                }
                break;
            }
            case GuidePointUI.Lobby_InventoryBtn:
            {
                BadgeStatus status = CheckAndSetModuleStatus(pointTrans, ModuleType.module_inventory, false);
                if (status != BadgeStatus.Normal)
                {
                    return status;
                }
                break;
            }
            case GuidePointUI.Lobby_RoleInfoBtn:
            {
                //if (CheckAndSetModuleStatus(pointTrans, ModuleType.module_) == BadgeStatus.NoOpen)
                //    return BadgeStatus.NoOpen;
                break;
            }
        }

        return BadgeStatus.Normal;
    }

    //查看此功能模块是否能开启
    public BadgeStatus CheckAndSetModuleStatus(Transform moduleBtn, ModuleType ty , bool canSetDisable = true)
    {
        if (moduleBtn == null) return BadgeStatus.NoOpen;
        BadgeStatus status = (BadgeStatus)SaveUtils.GetIntInPlayer(ty.ToString());
        BadgeTips.FreshByStatus(moduleBtn, status, canSetDisable);
        if (LobbySceneMainUIMgr.Instance != null)
        {
            bool isLock = status == BadgeStatus.NoOpen;
            LobbySceneMainUIMgr.Instance.LockLobbyBtn(GetGuidePointByModule(ty), isLock);
        }
        return status;
    }


    public GuidePointUI GetGuidePointByModule(ModuleType moduleTy)
    {
        switch (moduleTy)
        {
            case ModuleType.module_cave:
            case ModuleType.module_make_drug:
            case ModuleType.module_make_equip:
                return GuidePointUI.Lobby_CaveBtn;
            case ModuleType.module_sect:
            case ModuleType.module_travel:
                return GuidePointUI.Lobby_WorldBtn;
            case ModuleType.module_rank:
                return GuidePointUI.Lobby_RankBtn;
            case ModuleType.module_pet_animal:
            case ModuleType.module_pet_puppet:
            case ModuleType.module_pet_ghost:
            case ModuleType.module_inventory:
                return GuidePointUI.Lobby_InventoryBtn;
            case ModuleType.module_tower:
            case ModuleType.module_activity:
                return GuidePointUI.Lobby_ActivityBtn;
            case ModuleType.module_shop:
                return GuidePointUI.Lobby_ShopBtn;
        }
        return GuidePointUI.Max;
    }
}

public enum GuidePointUI:short
{
    Lobby_WorldBtn,     //世界按钮
    Lobby_RoleInfoBtn,  //纳戒按钮
    Lobby_CaveBtn,      //洞府按钮
    Lobby_RankBtn,      //排行榜按钮
    Lobby_ShopBtn,      //坊市按钮
    Lobby_ActivityBtn,  //活动按钮
    Lobby_InventoryBtn, //纳戒按钮

    World_TravelBtn,
    World_PlotMapBtn,   //剧情秘境
    World_SectBtn,

    Travel_ItemBtn,     //游历地图按钮


    RoleInfo_InventoryBtn,
    Activity_Tower,
    Max,
}

public enum ModuleType:byte
{
    module_travel,
    module_inventory,
    module_cave,
    module_sect,
    module_dungeon_map,
    module_make_drug,
    module_rank,
    module_make_equip,
    module_pet_animal,
    module_pet_puppet,
    module_pet_ghost,
    module_tower,
    module_activity,
    module_shop,
    Max
}




public class BadgePointGroup
{
    public CancelType BadgeMode;  //0-点击后自动消失；1-事件解决后消失
    public List<BadgePointItem> PointList;

    public static BadgePointGroup GetGroup(BadgeChainType chainTy, int? paramObj = null)
    {
        BadgePointGroup temp = new BadgePointGroup();
        switch (chainTy)
        {
            case BadgeChainType.Produce:
            {
                temp.BadgeMode = CancelType.Finish;
                temp.PointList = new List<BadgePointItem>()
                {
                    new BadgePointItem(GuidePointUI.Lobby_WorldBtn),
                    new BadgePointItem(GuidePointUI.World_TravelBtn),
                    new BadgePointItem(GuidePointUI.Travel_ItemBtn, paramObj)
                };
                break;
            }
        }

        return temp;
    }
}

public enum BadgeChainType
{
    Produce,

}

public enum CancelType
{
    Click,
    Finish,
}

public class BadgePointItem
{
    public GuidePointUI PointUI;//控件
    public int? ParamId;     //附加信息

    public BadgePointItem(GuidePointUI pointUI, int? paramObj=null)
    {
        PointUI = pointUI;
        ParamId = paramObj;
    }
}