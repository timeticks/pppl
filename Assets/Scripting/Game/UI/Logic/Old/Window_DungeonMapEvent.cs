using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Window_DungeonMapEvent : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_DungeonMapEventItem;
        public Text TextName;
        public Text QuestTitleText;
        public Text EndTitleText;
        public Transform QuestItemRoot;
        public Transform EndItemRoot;
        public Button MaskBtn;


        public ViewObj(UIViewBase view)
        {
            Part_DungeonMapEventItem = view.GetCommon<GameObject>("Part_DungeonMapEventItem");
            TextName = view.GetCommon<Text>("TextName");
            QuestTitleText = view.GetCommon<Text>("QuestTitleText");
            EndTitleText = view.GetCommon<Text>("EndTitleText");
            QuestItemRoot = view.GetCommon<Transform>("QuestItemRoot");
            EndItemRoot = view.GetCommon<Transform>("EndItemRoot");
            MaskBtn = view.GetCommon<Button>("MaskBtn");

        }
    }

    public class EventItemObj : SmallViewObj
    {
        public Text NameText;
        public Button GetBtn;
        public Text StatusText;
        public Text GetText;
        public Text DescText;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (GetBtn == null) GetBtn = view.GetCommon<Button>("GetBtn");
            if (StatusText == null) StatusText = view.GetCommon<Text>("StatusText");
            if (GetText == null) GetText = view.GetCommon<Text>("GetText");
            if (DescText == null) DescText = view.GetCommon<Text>("DescText");

        }
    }

    private List<EventItemObj> mQuestItemList = new List<EventItemObj>();
    private List<EventItemObj> mEndItemList = new List<EventItemObj>();
    private int mMapId;

    private ViewObj mViewObj;

    public void OpenWindow(int mapId)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        mMapId = mapId;
        FreshEvent();
    }


    private void FreshEvent()
    {
        MapData mapData = MapData.MapDataFetcher.GetMapDataByCopy(mMapId);
        if (mapData == null) return;

        mViewObj.TextName.text = mapData.name;

        List<MapEvent> questList = new List<MapEvent>();
        List<MapEvent> endList = new List<MapEvent>();
        DungeonMapAccessor mapAccessor = PlayerPrefsBridge.Instance.GetDungeonMapCopy();
        List<int> finishEvents = mapAccessor.GetFinishEvent();

        MapEvent mapEvent;
        int eventId = 0;
        for (int i = 0; i < mapData.Quest.Length; i++)
        {
            eventId = mapData.Quest[i];
            mapEvent = MapEvent.MapEventFetcher.GetMapEventByCopy(eventId);
            if (mapEvent == null) continue;
            if (finishEvents.Exists(x=>x.Equals(eventId)))
            {
                mapEvent.EventStatus = MapEvent.MapEventStatus.Finish;
            }
            questList.Add(mapEvent);
        }
        for (int i = 0; i < mapData.Ending.Length; i++)
        {
            eventId = mapData.Ending[i];
            mapEvent = MapEvent.MapEventFetcher.GetMapEventByCopy(eventId);
            if (mapEvent == null) continue;
            if (finishEvents.Exists(x => x.Equals(eventId)))
            {
                mapEvent.EventStatus = MapEvent.MapEventStatus.Finish;
            }
            endList.Add(mapEvent);
        }

        mViewObj.MaskBtn.SetOnClick(delegate() { CloseWindow(); });

        mViewObj.QuestTitleText.text = LangMgr.GetText("支线");
        FreshEventList(mViewObj.QuestItemRoot, questList, mQuestItemList);

        mViewObj.EndTitleText.text = LangMgr.GetText("结局");
        FreshEventList(mViewObj.EndItemRoot, endList, mEndItemList);

    }


    //刷新事件项
    void FreshEventList(Transform parent, List<MapEvent> eventList, List<EventItemObj> itemList)
    {
        itemList = TAppUtility.Instance.AddViewInstantiate<EventItemObj>(itemList, mViewObj.Part_DungeonMapEventItem, parent, eventList.Count);
        
        for (int i = 0; i < eventList.Count; i++)
        {
            itemList[i].NameText.text = eventList[i].name;
            itemList[i].DescText.text = eventList[i].Desc;
            itemList[i].StatusText.gameObject.SetActive(true);
            itemList[i].GetBtn.gameObject.SetActive(false);
            itemList[i].GetText.text = LangMgr.GetText("领取");
            if (eventList[i].EventStatus == MapEvent.MapEventStatus.None)
            {
                itemList[i].StatusText.text = LangMgr.GetText("未完成");
            }
            else
            {
                itemList[i].StatusText.text = LangMgr.GetText("已完成");
            }
        }
    }


}