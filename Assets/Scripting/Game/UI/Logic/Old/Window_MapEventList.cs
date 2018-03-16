using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Window_MapEventList : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_MapListEventItem;
        public Text TextName;
        public Text QuestTitleText;
        public Text EndTitleText;
        public Transform QuestItemRoot;
        public Transform EndItemRoot;
        public Button MaskBtn;
        public Text DescText;
        public Text QuestNumText;
        public Text EndNumText;
        public ViewObj(UIViewBase view)
        {
            Part_MapListEventItem = view.GetCommon<GameObject>("Part_MapListEventItem");
            TextName = view.GetCommon<Text>("TextName");
            QuestTitleText = view.GetCommon<Text>("QuestTitleText");
            EndTitleText = view.GetCommon<Text>("EndTitleText");
            QuestItemRoot = view.GetCommon<Transform>("QuestItemRoot");
            EndItemRoot = view.GetCommon<Transform>("EndItemRoot");
            MaskBtn = view.GetCommon<Button>("MaskBtn");
            if (DescText == null) DescText = view.GetCommon<Text>("DescText");
            if (QuestNumText == null) QuestNumText = view.GetCommon<Text>("QuestNumText");
            if (EndNumText == null) EndNumText = view.GetCommon<Text>("EndNumText");
        }
    }

    public class EventItemObj : SmallViewObj
    {
        public Text NameText;
        public Button GetBtn;
        public Text StatusText;
        public Text GetText;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (GetBtn == null) GetBtn = view.GetCommon<Button>("GetBtn");
            if (StatusText == null) StatusText = view.GetCommon<Text>("StatusText");
            if (GetText == null) GetText = view.GetCommon<Text>("GetText");

        }
    }

    private List<EventItemObj> mQuestItemList = new List<EventItemObj>();
    private List<EventItemObj> mEndItemList = new List<EventItemObj>();
    private int mMapId;

    private ViewObj mViewObj;

    public void OpenWindow(int mapId)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        mMapId = mapId;
        OpenWin();
        FreshEvent();
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    private void FreshEvent()
    {
        MapData mapData = MapData.MapDataFetcher.GetMapDataByCopy(mMapId);
        if (mapData == null) return;

        mViewObj.TextName.text = mapData.name;
        mViewObj.DescText.text = "\u3000\u3000" + mapData.Desc;

        List<MapEvent> questList = new List<MapEvent>();
        List<MapEvent> endList = new List<MapEvent>();
        MapEventAccessor mapEventAccessor = PlayerPrefsBridge.Instance.GetMapEventAccessorCopy();
        Dictionary<int, MapEvent.MapEventStatus> finishEvents = mapEventAccessor.GetStatusByMapId(mMapId);

        MapEvent mapEvent;
        int eventId     = 0;
        int finishQuest = 0;   //已完成的支线任务
        int finishEnd   = 0;   //已完成的结局
        for (int i = 0; i < mapData.Quest.Length; i++)
        {
            eventId = mapData.Quest[i];
            mapEvent = MapEvent.MapEventFetcher.GetMapEventByCopy(eventId);
            if(mapEvent==null)continue;
            if (finishEvents.ContainsKey(eventId))
            {
                mapEvent.EventStatus = finishEvents[eventId];
                if(mapEvent.EventStatus== MapEvent.MapEventStatus.Finish)finishQuest++;
            }
            questList.Add(mapEvent);
        }
        for (int i = 0; i < mapData.Ending.Length; i++)
        {
            eventId = mapData.Ending[i];
            mapEvent = MapEvent.MapEventFetcher.GetMapEventByCopy(eventId);
            if (mapEvent == null) continue;
            if (finishEvents.ContainsKey(eventId))
            {
                mapEvent.EventStatus = finishEvents[eventId];
            }
            endList.Add(mapEvent);
            if (mapEvent.EventStatus == MapEvent.MapEventStatus.Finish) finishEnd++;
        }

        mViewObj.MaskBtn.SetOnClick(delegate() { CloseWindow(); });

        mViewObj.QuestTitleText.text = LangMgr.GetText("支线");
        mViewObj.QuestNumText.text = string.Format("{0}/{1}", finishQuest, mapData.Quest.Length);
        FreshEventList(mViewObj.QuestItemRoot, questList, mQuestItemList);

        mViewObj.EndTitleText.text = LangMgr.GetText("结局");
        mViewObj.EndNumText.text = string.Format("{0}/{1}", finishEnd, mapData.Ending.Length);
        FreshEventList(mViewObj.EndItemRoot, endList, mEndItemList);

    }


    //刷新事件项
    void FreshEventList(Transform parent, List<MapEvent> eventList, List<EventItemObj> itemList)
    {
        itemList = TAppUtility.Instance.AddViewInstantiate<EventItemObj>(itemList, mViewObj.Part_MapListEventItem, parent, eventList.Count);
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].GetBtn.SetOnClick(null);
        }

        for (int i = 0; i < eventList.Count; i++)
        {
            itemList[i].NameText.text = eventList[i].name;

            if (eventList[i].EventStatus == MapEvent.MapEventStatus.CanReaward)
            {
                itemList[i].GetBtn.gameObject.SetActive(true);
                int mapId = eventList[i].idx;
                itemList[i].GetBtn.SetOnClick(delegate() { BtnEvt_GetReward(mapId); });
                itemList[i].GetText.text = "领取";
                itemList[i].StatusText.gameObject.SetActive(false);
            }
            else
            {
                itemList[i].GetBtn.gameObject.SetActive(false);
                itemList[i].StatusText.gameObject.SetActive(true);
                if (eventList[i].EventStatus == MapEvent.MapEventStatus.None)
                {
                    itemList[i].StatusText.text = LangMgr.GetText("未完成");
                }
                else
                {
                    itemList[i].StatusText.text = LangMgr.GetText("已领取");
                }
            }
        }
    }


    public void BtnEvt_GetReward(int mapEventId) //领奖
    {
        //TDebug.Log("BtnEvt_GetReward:" + mapEventId);

        ServPacketHander del = delegate(BinaryReader ios)
        {
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MapEventReward, null);
            UIRootMgr.Instance.IsLoading = false;
            NetPacket.S2C_MapEventReward msg = MessageBridge.Instance.S2C_MapEventReward(ios);

            string getStr = "";
            MapEvent mapEvent = MapEvent.MapEventFetcher.GetMapEventByCopy(msg.MapEventId);
            if (mapEvent != null) getStr = string.Format("你领取了{0}的奖励", mapEvent.name);
            UIRootMgr.LobbyUI.ShowDropInfo(msg.GoodsList, getStr);
            FreshEvent();

            //刷新红点
            BadgeTips.FreshWindow(WinName.Window_ChoosePlotDungeon);
            BadgeTips.FreshWindow(WinName.WindowBig_WorldChoose);
        };
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MapEventReward, del);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_MapEventReward(mapEventId));
    }


}