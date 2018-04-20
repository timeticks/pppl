using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AppEvtMgr //游戏的事件管理器
{
    public static readonly AppEvtMgr Instance = new AppEvtMgr();
    public DoActMgr ActMgr;         //处理行为管理器
    //public Dictionary<EvtType, object> m_TypeHandleDic;
    //public Dictionary<string, Action<object>> m_HandleDic = new Dictionary<string, Action<object>>();

    //事件池   <事件key值 ， 事件信息>
    public Dictionary<string, EvtItemData> EvtPool = new Dictionary<string, EvtItemData>();

    //<事件key值，<监听类型，监听回调>>
    public Dictionary<string, Dictionary<EvtListenerType, System.Action<object>>> HandlePool = new Dictionary<string, Dictionary<EvtListenerType, System.Action<object>>>();
    public AppEvtMgr()
    {
        ActMgr = new DoActMgr();
    }

    /// <summary>
    /// 某事件发生，发送事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="val">事件参数</param>
    public void SendNotice(EvtItemData eventType , object val=null)
    {
        //string keepEvtKey = eventType.ToKey(true);
        //SendNotice(keepEvtKey, val, true);

        SendNotice(eventType.ToValKey(), val, false);  //发送带参evt
        SendNotice(eventType.ToTypeKey(), val, false); //发送不带参evt
    }

    private List<System.Action<object>> mDelList = new List<Action<object>>();
    private void SendNotice(string keyStr, object val , bool canKeep)
    {
        if (HandlePool.ContainsKey(keyStr))
        {
            //TDebug.Log("发送事件:" + keyStr, "");
            mDelList.Clear();
            foreach (var temp in HandlePool[keyStr])
            {
                mDelList.Add(temp.Value);
            }
            if (!canKeep)
            {
                EvtPool.Remove(keyStr);
                HandlePool.Remove(keyStr);
            }
            for (int i = 0; i < mDelList.Count; i++)
            {
                if (mDelList[i] != null) mDelList[i](val);
            }
        }
    }


    /// <summary>
    /// 注册监听，相同EvtListenerType会被覆盖
    /// </summary>
    public void Register(EvtItemData eventType, EvtListenerType listenerTy, Action<object> hand , bool needRequest = true)//
    {
        string evtKey = eventType.ToValKey();

        if (!EvtPool.ContainsKey(evtKey))
        {
            EvtPool.Add(evtKey, eventType);
        }

        if (!HandlePool.ContainsKey(evtKey))
        {
            HandlePool.Add(evtKey, new Dictionary<EvtListenerType, Action<object>>() {});
        }
        if (!HandlePool[evtKey].ContainsKey(listenerTy))
            HandlePool[evtKey].Add(listenerTy, hand);
        else
        {
            HandlePool[evtKey][listenerTy] = null;
            HandlePool[evtKey][listenerTy] = hand;
        }
        if (needRequest)
            RequestSendNotice(eventType);
    }

    public void RequestSendNotice(EvtItemData eventType) //主动请求查看某事件是否发生
    {
        if (eventType == null || !eventType.EventType.CanReqNotice())
        {
            return;
        }
        //这里根据类型，从玩家信息等地方，获取事件的当前值，并调用SendNotice分发
        switch (eventType.EventType)
        {
            case EvtType.MapEventCanReward:
            {
                if (PlayerPrefsBridge.Instance.IsMapEventCanReward())
                {
                    MapEventAccessor mapEventAc = PlayerPrefsBridge.Instance.GetMapEventAccessorCopy();
                    List<int> canRewardList = mapEventAc.GetCanRewardEvents();
                    SendNotice(new EvtItemData(EvtType.MapEventCanReward), canRewardList);
                }
                break;
            }
            case EvtType.ProductFinish:
            {
                if (PlayerPrefsBridge.Instance.IsProduceFinish())
                {
                    OldProduceAccessor produceAc = PlayerPrefsBridge.Instance.GetProduceAccessor();
                    List<int> finishList = produceAc.GetFinishRecipeList();
                    SendNotice(new EvtItemData(EvtType.ProductFinish), finishList);
                }
                break;
            }
            case EvtType.CurLevel:
            {
                if (eventType.Val == "")
                    SendNotice(new EvtItemData(EvtType.CurLevel, PlayerPrefsBridge.Instance.PlayerData.Level.ToString()), PlayerPrefsBridge.Instance.PlayerData.Level);
                else
                {
                    int levelAim = 0;
                    int.TryParse(eventType.Val, out levelAim);
                    if( PlayerPrefsBridge.Instance.PlayerData.Level>=levelAim)
                        SendNotice(new EvtItemData(EvtType.CurLevel, levelAim.ToString()), PlayerPrefsBridge.Instance.PlayerData.Level);
                }
                break;
            }
            case EvtType.ChangePrefsKey:
            {
                if (eventType.Val != null && SaveUtils.HasKeyInPlayer(eventType.Val))
                {
                    SendNotice(new EvtItemData(EvtType.ChangePrefsKey, eventType.Val), "");
                }
                break;
            }
            case EvtType.TravelNewSite:
            {
                if (SaveUtils.GetIntInPlayer(EvtListenerType.TravelNewSite.ToString(), 0) > 0)
                {
                    AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.TravelNewSite));
                }
                break;
            }
        }
    }


    public void RemoveByListenerType(EvtItemData eventType, EvtListenerType listenerTy)//移除监听
    {
        string evtKey = eventType.ToValKey();
        if (HandlePool.ContainsKey(evtKey))
        {
            if (HandlePool[evtKey] != null && HandlePool[evtKey].ContainsKey(listenerTy))
            {
                HandlePool[evtKey][listenerTy] = null;
                if (HandlePool[evtKey].Count == 0)
                {
                    HandlePool.Remove(evtKey);
                    EvtPool.Remove(evtKey);
                }
            }
        }
    }

    private List<string> mKeyList = new List<string>();
    public void Fresh()  //主动刷新事件是否满足
    {
        mKeyList = new List<string>(HandlePool.Keys);
        for (int i = 0; i < mKeyList.Count; i++)
        {
            if (!EvtPool.ContainsKey(mKeyList[i]))
            {
                TDebug.LogError(string.Format("事件池与监听池信息不一致:{0}", mKeyList[i]));
                continue;
            }
            EvtItemData evt = EvtPool[mKeyList[i]];
            RequestSendNotice(evt);
        }
    }

    public void Update()
    {
        if (Time.frameCount%20 == 0)
        {
            Fresh();
        }
    }
}

public enum EvtListenerType
{
    ShowOfflineTime,
    MapEventCanReward,   //秘境任务可领取
    ProductFinish,
    GuideModuleOpen,     //模块开放
    CanRetreat,
    TravelNewSite,
    InventoryBadgeOpen,  //背包红点
    TravelNewEvent,
}