using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 新手引导管理器
/// </summary>
public class GuideMgr
{
    public static GuideMgr Instance {
        get
        {
            if (mInstance == null) mInstance = new GuideMgr();
            return mInstance; 
        }
    }
    private static GuideMgr mInstance;

    public bool IsNewer;
    public bool IsFirstInDevice;  //在此设备中第一次登录
    

    #region 功能解锁
    
    public void Init ()
	{
        IsFirstInDevice = SaveUtils.GetIntInPlayer("IsFirstInDevice", 0) == 0;
        IsNewer = PlayerPrefsBridge.Instance.PlayerData.Level == 1;
        if (IsFirstInDevice)//如果是玩家第一次进入此设备，则初始将所有功能刷新
	    {
            SaveUtils.SetIntInPlayer("IsFirstInDevice", 1);
            //int level = PlayerPrefsBridge.Instance.PlayerData.Level;
            //for (int i = 0, length = (int)ModuleType.Max; i < length; i++)
            //{
            //    BadgeStatus ty = BadgeStatus.NoOpen;
            //    ModuleType moduleTy = (ModuleType)i;
            //    int unlockLevel = GameConstUtils.GetNewModuleUnlockLevel(moduleTy);
            //    if (level >= unlockLevel)
            //    {
            //        ty = IsNewer ? BadgeStatus.ShowBadge : BadgeStatus.Normal;
            //    }
            //    else
            //    {
            //        ty = BadgeStatus.NoOpen;
            //    }
            //    SaveUtils.SetIntInPlayer(moduleTy.ToString(), (int)ty);
            //}
	    }
	    //FreshModuleUnlock(0);
	}

    //public void FreshModuleUnlock(int lastLevel) //刷新某个功能被解锁
    //{
    //    bool isLevelUp = false;// obj != null;TODO:a刷新某个功能被解锁
    //    int level = PlayerPrefsBridge.Instance.PlayerData.Level;
    //    for (int i = 0, length = (int)ModuleType.Max; i < length; i++) //遍历等级，查看功能解锁
    //    {
    //        BadgeStatus ty = BadgeStatus.NoOpen;
    //        ModuleType moduleTy = (ModuleType)i;
    //        int unlockLevel = GameConstUtils.GetNewModuleUnlockLevel(moduleTy);
    //        bool isChanged = false;
    //        if(level >=unlockLevel && SaveUtils.GetIntInPlayer(moduleTy.ToString()) == 0)
    //        {
    //            if (isLevelUp) //如果是升级上去的，则设红点
    //                SaveUtils.SetIntInPlayer(moduleTy.ToString(), (int)BadgeStatus.ShowBadge);
    //            else
    //                SaveUtils.SetIntInPlayer(moduleTy.ToString(), (int)BadgeStatus.Normal);
    //            isChanged = true;
    //        }
    //        //if (isChanged)
    //        //{
    //        //    GuidePointUI pointUi = GetGuidePointByModule(moduleTy);
    //        //    if (PointPool.ContainsKey(pointUi))
    //        //    {
    //        //        BadgeStatus status = Fresh(pointUi, PointPool[pointUi]);
    //        //        TDebug.Log(string.Format("刷新解锁:{0}--{1}",pointUi, status));
    //        //    }
    //        //}
    //    }
    //}

    public void LevelUp(int lastLevel)
    {
        int curLevel = PlayerPrefsBridge.Instance.PlayerData.Level;
        if (curLevel == lastLevel) return;
        //解锁新的挂机地点
        List<Travel> travelList = Travel.TravelFetcher.GetTravelList();
        for (int i = 0, length = travelList.Count; i < length; i++)
        {
            if (TUtility.IsNewUnlock(lastLevel, curLevel, travelList[i].Level)) 
            {
                AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.TravelNewSite));
                SaveUtils.SetIntInPlayer(EvtListenerType.TravelNewSite.ToString(), 1);
                AddOnceBadge(OnceBadgeType.NewTravel, travelList[i].Idx);
            }
        }

        //解锁新的秘境地点
        List<MapData> mapList = MapData.MapDataFetcher.GetMapDataListNoCopy(MapData.MapType.SingleMap);
        for (int i = 0, length = mapList.Count; i < length; i++)
        {
            if (TUtility.IsNewUnlock(lastLevel, curLevel, mapList[i].OpenLevel))
            {
                AddOnceBadge(OnceBadgeType.NewDungeonMap, mapList[i].Idx);
            }
        }

        //遍历模块，查看功能解锁
        for (int i = 0, length = (int) ModuleType.Max; i < length; i++) 
        {
            BadgeStatus ty = BadgeStatus.NoOpen;
            ModuleType moduleTy = (ModuleType)i;
            int unlockLevel = GameConstUtils.GetNewModuleUnlockLevel(moduleTy);
            if (TUtility.IsNewUnlock(lastLevel, curLevel, unlockLevel))
            {
                switch (moduleTy)
                {
                    case ModuleType.module_make_equip: AddOnceBadge(OnceBadgeType.NewAuxSkill, (int)AuxSkillLevel.SkillType.Forge);
                        break;
                    case ModuleType.module_make_drug: AddOnceBadge(OnceBadgeType.NewAuxSkill, (int)AuxSkillLevel.SkillType.MakeDrug);
                        break;
                    case ModuleType.module_make_mine: AddOnceBadge(OnceBadgeType.NewAuxSkill, (int)AuxSkillLevel.SkillType.Mine);
                        break;
                    case ModuleType.module_make_herb: AddOnceBadge(OnceBadgeType.NewAuxSkill, (int)AuxSkillLevel.SkillType.GatherHerb);
                        break;
                    //case ModuleType.module_dungeon_map: AddOnceBadge(OnceBadgeType.NewDungeonMap, 0); //取消，由秘境关卡显示红点
                    //    break;
                    case ModuleType.module_prestige_task: AddOnceBadge(OnceBadgeType.OpenPrestigeTask, 0); //开启声望任务
                        break;
                    case ModuleType.module_tower: AddOnceBadge(OnceBadgeType.OpenActivity_Tower, 0);
                        break;
                    case ModuleType.module_activity_pvp: AddOnceBadge(OnceBadgeType.OpenActivity_PVP, 0);
                        break;
                    case ModuleType.module_market: AddOnceBadge(OnceBadgeType.OpenMarket, 0);
                        break;
                }
            }
        }

        
    }

    #region 一次性红点
    
    //得到一次性红点是否有
    public List<int> GetOnceBadge(OnceBadgeType onceTy)
    {
        string str = SaveUtils.GetStringInPlayer(onceTy.ToString());
        if (str.Length <= 0)
            return new List<int>();
        List<int> idList = new List<int>();
        string[] s = str.Split('|');
        for (int i = 0; i < s.Length; i++)
        {
            int temp = s[i].ToInt();
            idList.Add(temp);
        }
#if UNITY_EDITOR
        TDebug.Log(string.Format("一次性红点:{0}|[{1}]", onceTy.ToString(), str));
#endif
        return idList;
        
    }

    //是否此红点有需要显示的东西
    public bool GetOnceBadgeHave(OnceBadgeType onceTy)
    {
        string str = SaveUtils.GetStringInPlayer(onceTy.ToString());
        TDebug.Log(string.Format("{0}|[{1}]", onceTy.ToString(), str));
        return str.Length > 0;
    }

    //添加一次性红点信息
    public void AddOnceBadge(OnceBadgeType onceTy, int id)
    {
        string str = SaveUtils.GetStringInPlayer(onceTy.ToString());
        if (str.Length > 0)
        {
            if (str.Contains(id.ToString()))
            {
                string[] tempIds = str.Split('|');
                string idStr = id.ToString();
                for (int i = 0; i < tempIds.Length; i++) //如果已经存入相同的，则不再添加
                {
                    if (tempIds[i].Equals(idStr))
                    {
                        return;
                    }
                }
            }
            str += "|" + id;
        }
        else { str = id.ToString(); }
        SaveUtils.SetStringInPlayer(onceTy.ToString(), str);
        TDebug.Log(string.Format("添加红点信息:{0}, [{1}]", onceTy.ToString(), str));

        //将对应的按钮设为红点模式
        switch (onceTy)
        {
            case OnceBadgeType.NewTravel:
                SetUIStatus(GuidePointUI.Lobby_WorldBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.World_TravelTab.ToString(), BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.NewDungeonMap:
                SetUIStatus(GuidePointUI.Lobby_WorldBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.World_DungeonMapTab.ToString(), BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.NewAuxSkill:  //此时的id为生活技能类型
                SetUIStatus(GuidePointUI.Lobby_CaveBtn, BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Cave_AuxSkillTab, BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Cave_AuxSkillTab_SkillType.ToString() + id, BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.NewRecipe:
                Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(id);
                if (recipe == null) return;
                SetUIStatus(GuidePointUI.Lobby_CaveBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Cave_AuxSkillTab, BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Cave_AuxSkillTab_SkillType.ToString() + (int)recipe.MySkillType, BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Cave_AuxSkillTab_SkillType_RecipeTab.ToString() + recipe.SkillLevel, BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.OpenActivity_PVP:
                SetUIStatus(GuidePointUI.Lobby_ActivityBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Activity_PVP.ToString(), BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.OpenActivity_Tower:
                SetUIStatus(GuidePointUI.Lobby_ActivityBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Activity_Tower.ToString(), BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.OpenPrestigeTask:
                SetUIStatus(GuidePointUI.Lobby_ActivityBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Activity_PrestigeTaskTab.ToString(), BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.OpenMarket:
                SetUIStatus(GuidePointUI.Lobby_ShopBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Shop_MarketTab.ToString(), BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.NewMail:
                SetUIStatus(GuidePointUI.Lobby_HeadBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Head_MailTab.ToString(), BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.NewPet:
                Pet pet = Pet.PetFetcher.GetPetByCopy(id);
                if (pet == null) return;
                SetUIStatus(GuidePointUI.Lobby_InventoryBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Inventory_PetTab.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Inventory_PetTab_PetType.ToString() + (int)pet.Type, BadgeStatus.ShowBadge);
                break;
            case OnceBadgeType.NewSpell:
                Spell spell = Spell.SpellFetcher.GetSpellByNoCopy(id);
                if (spell == null) return;
                SetUIStatus(GuidePointUI.Lobby_InventoryBtn.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Inventory_SpellTab.ToString(), BadgeStatus.ShowBadge);
                SetUIStatus(GuidePointUI.Inventory_SpellTab_SpellType.ToString() + (int)spell.Type, BadgeStatus.ShowBadge);
                break;
        }
    }
    //移除一次性红点信息，返回是否有并且成功移除
    public bool RemoveOnceBadge(OnceBadgeType onceTy, int id ,bool removeAll = false)
    {
        if (!SaveUtils.HasKeyInPlayer(onceTy.ToString()))
            return false;
        string temp = SaveUtils.GetStringInPlayer(onceTy.ToString());
        if (removeAll)
        {
            SaveUtils.SetStringInPlayer(onceTy.ToString(), "");
            return temp.Length>0;
        }
        bool isFind = false;
        if (temp.Contains(id.ToString())) //如果包含此id，才进行检测
        {
            string[] tempIds = temp.Split('|');
            string idStr = id.ToString();
            for (int i = 0; i < tempIds.Length; i++) //如果有，置空
            {
                if (tempIds[i].Equals(idStr))
                {
                    tempIds[i] = "";
                    isFind = true;
                }
            }
            StringBuilder tempBuilder = new StringBuilder();
            for (int i = 0; i < tempIds.Length; i++)
            {
                if (tempIds[i].Length == 0) continue; //空的不添加

                if (tempBuilder.Length == 0) tempBuilder.Append(tempIds[i]);
                else tempBuilder.Append("|" + tempIds[i]);
            }
            SaveUtils.SetStringInPlayer(onceTy.ToString(), tempBuilder.ToString());
        }
        return isFind;
    }
    #endregion


    #endregion

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
        EventTrigger trigger = obj.CheckAddComponent<EventTrigger>();
        
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


    public BadgeStatus Fresh(GuidePointUI pointTy, Transform pointTrans ,params object[] attach)
    {
        switch (pointTy)
        {
            case GuidePointUI.Lobby_CaveBtn://洞府按钮
            {
                if (CheckIsModuleLock(ModuleType.module_cave))
                {
                    return BadgeStatus.NoOpen;
                }
                
                //将所有红点隐藏，重新计算红点信息
                BadgeTips.SetBadgeViewFalse(pointTrans);
                do
                {
                    if (PlayerPrefsBridge.Instance.GetCurSitStartTime() == 0) //打坐红点
                    {
                        BadgeTips.SetBadgeView(pointTrans);continue;
                    }
                    if (PlayerPrefsBridge.Instance.GetCaveCanLevelUp()) //洞府升级红点
                    {
                        BadgeTips.SetBadgeView(pointTrans);continue;
                    } 
                    if (PlayerPrefsBridge.Instance.RetreatFinish()) //闭关完成
                    {
                        BadgeTips.SetBadgeView(pointTrans);
                    }
                } while (false);
                
                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.ProductFinish), EvtListenerType.ProductFinish,
                    delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });  //生成制作红点

                System.Action<object> levelUpDel = delegate(object o)//闭关红点
                {
                    if (PlayerPrefsBridge.Instance.PlayerData.CanRetreat())
                    {  BadgeTips.SetBadgeView(pointTrans); }
                };
                levelUpDel(PlayerPrefsBridge.Instance.PlayerData.Level);//先查看一次
                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.LevelUp), EvtListenerType.CanRetreat, levelUpDel); //闭关

                break;
            }
            case GuidePointUI.Lobby_WorldBtn:
            {
                //将所有红点隐藏，重新计算红点信息
                BadgeTips.SetBadgeViewFalse(pointTrans);

                if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Lobby_WorldBtn) == BadgeStatus.ShowBadge)
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                //秘境事件可领取红点
                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.MapEventCanReward), EvtListenerType.MapEventCanReward,
                    delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });
                if (PlayerPrefsBridge.Instance.PlayerData.SpellLearnTime > 0 && PlayerPrefsBridge.Instance.PlayerData.GetSpellLearnDownTime() < 0)
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.TravelNewSite), EvtListenerType.TravelNewSite,
                   delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });

                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.TravelNewEvent), EvtListenerType.TravelNewEvent,
                  delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });
                break;
            }
            case GuidePointUI.Lobby_RankBtn:
            {
                if (CheckIsModuleLock(ModuleType.module_rank))
                {
                    return BadgeStatus.NoOpen;
                }
                do
                {
                    BadgeTips.SetBadgeViewFalse(pointTrans);
                    if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Lobby_RankBtn) == BadgeStatus.ShowBadge)
                    {
                        BadgeTips.SetBadgeView(pointTrans);
                        continue;
                    }
                    if (PlayerPrefsBridge.Instance.CanGetAchieveReward())//成就奖励红点
                    {
                        BadgeTips.SetBadgeView(pointTrans);
                    }
                } while (false);
                
                break;
            }
            case GuidePointUI.Lobby_ShopBtn:
            {
                if (CheckIsModuleLock(ModuleType.module_shop))
                {
                    return BadgeStatus.NoOpen;
                }
                BadgeTips.SetBadgeViewFalse(pointTrans);
                if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Lobby_ShopBtn) == BadgeStatus.ShowBadge)
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                break;
            }
            case GuidePointUI.Lobby_ActivityBtn:
            {
                if (CheckIsModuleLock(ModuleType.module_activity))
                {
                    return BadgeStatus.NoOpen;
                }
                do
                {
                    BadgeTips.SetBadgeViewFalse(pointTrans);
                    if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Lobby_ActivityBtn) == BadgeStatus.ShowBadge)
                    {
                        BadgeTips.SetBadgeView(pointTrans); continue;
                    }
                    if (PlayerPrefsBridge.Instance.CanGetPVPReward())
                    {
                        BadgeTips.SetBadgeView(pointTrans); 
                    }
                } while (false);
                

                AppEvtMgr.Instance.Register(new EvtItemData(EvtType.PrestigeTaskFinish), EvtListenerType.FreshBadge,
                    delegate(object o) { BadgeTips.SetBadgeView(pointTrans); });  //生成制作红点
                break;
            }
            case GuidePointUI.Lobby_InventoryBtn:
            {
                if (CheckIsModuleLock(ModuleType.module_inventory))
                {
                    return BadgeStatus.NoOpen;
                }
                BadgeTips.SetBadgeViewFalse(pointTrans);
                if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Lobby_InventoryBtn) == BadgeStatus.ShowBadge)
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                break;
            }
            case GuidePointUI.Lobby_RoleInfoBtn:
            {
                //if (CheckAndSetModuleStatus(pointTrans, ModuleType.module_) == BadgeStatus.NoOpen)
                //    return BadgeStatus.NoOpen;
                BadgeTips.SetBadgeViewFalse(pointTrans);
                if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Lobby_RoleInfoBtn) == BadgeStatus.ShowBadge)
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                else if (PlayerPrefsBridge.Instance.FormatAccessor.IsRuleUnlockFinish())
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                break;
            }
            case GuidePointUI.Lobby_HeadBtn:
            {
                BadgeTips.SetBadgeViewFalse(pointTrans);
                if (PlayerPrefsBridge.Instance.HaveMailNoOpened())
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                break;
            }
            case GuidePointUI.Lobby_VipBtn:
            {
                BadgeTips.SetBadgeViewFalse(pointTrans);
                if (PlayerPrefsBridge.Instance.PlayerData.CanGetVipReward())
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                break;
            }
            case GuidePointUI.Lobby_SectBtn:
            {
                BadgeTips.SetBadgeViewFalse(pointTrans);
                ChildAccessor childAccessor = PlayerPrefsBridge.Instance.ChildAccessor;
                if (!childAccessor.IsTravelInfoInit && childAccessor.IsChildNewMsg)
                {
                    BadgeTips.SetBadgeView(pointTrans);
                }
                else if (childAccessor.IsChildLevelUpFinish || childAccessor.IsChildTravelFinish)
                {                 
                     BadgeTips.SetBadgeView(pointTrans);
                }
                break;
            }
        }

        return BadgeStatus.Normal;
    }

    //查看此功能模块是否能开启
    public bool CheckIsModuleLock(ModuleType ty)
    {
        int unlockLevel = GameConstUtils.GetNewModuleUnlockLevel(ty);
        bool isLock = false;
        if (unlockLevel > PlayerPrefsBridge.Instance.PlayerData.Level)
        {
            isLock = true;
        }
        LobbySceneMainUIMgr.Instance.LockLobbyBtn(ty, isLock);
        return isLock;
    }


    public BadgeStatus GetUIStatus(GuidePointUI uiTy)//获取ui的红点状态
    {
        return GetUIStatus(uiTy.ToString());
    }
    public BadgeStatus GetUIStatus(string uiTyStr)
    {
        BadgeStatus status = (BadgeStatus)SaveUtils.GetIntInPlayer(uiTyStr, (int)BadgeStatus.Normal);
        //TDebug.Log(string.Format("获取状态:{0}|{1}", uiTyStr, status.ToString()));
        return status;
    }

    public bool SetUIStatus(GuidePointUI uiTy, BadgeStatus status , System.Action successCallback=null)//设置ui的红点状态
    {
        return SetUIStatus(uiTy.ToString(), status, successCallback);
    }
    public bool SetUIStatus(string uiTyStr, BadgeStatus status, System.Action successCallback = null)
    {
        BadgeStatus curStatus = (BadgeStatus)SaveUtils.GetIntInPlayer(uiTyStr, (int)BadgeStatus.Normal);
        if (status == curStatus)
            return false;
        //TDebug.Log(string.Format("设置状态:{0}|from:{1}|to:{2}", uiTyStr,curStatus.ToString(), status.ToString()));
        SaveUtils.SetIntInPlayer(uiTyStr, (int)status);
        if (successCallback != null) successCallback();
        return true;
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
    Lobby_RoleInfoBtn,  //角色按钮
    Lobby_CaveBtn,      //洞府按钮
    Lobby_RankBtn,      //排行榜按钮
    Lobby_ShopBtn,      //坊市按钮
    Lobby_ActivityBtn,  //活动按钮
    Lobby_InventoryBtn, //纳戒按钮
    Lobby_HeadBtn,      //头像按钮
    Lobby_VipBtn,       //月卡按钮
    Lobby_SectBtn,      //宗门按钮

    World_TravelTab,
    World_DungeonMapTab,
    Cave_AuxSkillTab,
    Cave_AuxSkillTab_SkillType,
    Cave_AuxSkillTab_SkillType_RecipeTab,
  

    Inventory_PetTab,
    Inventory_PetTab_PetType,

    Inventory_SpellTab,
    Inventory_SpellTab_SpellType,

    Sect_ChildTab_ChildTravelTab,
    Activity_PrestigeTaskTab,
    Activity_Tower,
    Activity_PVP,
    Shop_MarketTab,        //黑市
    Head_MailTab,
    Max,
}

public enum ModuleType:byte
{
    module_travel,
    module_inventory,
    module_cave,
    module_sect,
    module_make_drug,
    module_make_equip,
    module_make_mine, //挖矿
    module_make_herb, //采药
    module_rank,

    module_shop,
    module_market,

    module_activity,
    module_prestige_task,
    module_tower,
    module_activity_pvp,

    Max
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



//需要一次性显示红点的类型
public enum OnceBadgeType
{
    NewTravel,
    NewDungeonMap,
    NewAuxSkill,
    NewRecipe,
    OpenActivity_PVP,
    OpenActivity_Tower,
    OpenPrestigeTask,
    OpenMarket,  //黑市
    NewMail,
    NewPet,
    NewSpell,

    //---------------常驻型
    CaveCanLevelUp,

}

