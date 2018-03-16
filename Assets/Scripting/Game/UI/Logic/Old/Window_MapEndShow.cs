using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Window_MapEndShow : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_MapEndFinishMapEvent;
        public Text TitleText;
        public Text EndDescText;
        public Transform ItemRoot;
        public Button OkBtn;
        public Text OkText;
        public Text QuestTitleText;
        public Text ResultWinText;
        public Text ResultFailText;
        public ViewObj(UIViewBase view)
        {
            if (Part_MapEndFinishMapEvent == null) Part_MapEndFinishMapEvent = view.GetCommon<GameObject>("Part_MapEndFinishMapEvent");
            if (TitleText == null) TitleText = view.GetCommon<Text>("TitleText");
            if (EndDescText == null) EndDescText = view.GetCommon<Text>("EndDescText");
            if (ItemRoot == null) ItemRoot = view.GetCommon<Transform>("ItemRoot");
            if (OkBtn == null) OkBtn = view.GetCommon<Button>("OkBtn");
            if (OkText == null) OkText = view.GetCommon<Text>("OkText");
            if (QuestTitleText == null) QuestTitleText = view.GetCommon<Text>("QuestTitleText");
            if (ResultWinText == null) ResultWinText = view.GetCommon<Text>("ResultWinText");
            if (ResultFailText == null) ResultFailText = view.GetCommon<Text>("ResultFailText");        }
    }

    public class EventItemObj : SmallViewObj
    {
        public Text NameText;
        public Text StatusText;        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (StatusText == null) StatusText = view.GetCommon<Text>("StatusText");

        }
    }
    private ViewObj mViewObj;

    private List<EventItemObj> mEventItemList = new List<EventItemObj>();
    private int mMapId;
    private System.Action mCloseCallback;
    public void OpenWindow(int mapId, List<MapEvent> finishEventList,MapEndType endType, int endId , System.Action closeCallback)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        mMapId = mapId;
        mCloseCallback = closeCallback;
        Init(endId, endType, finishEventList);
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    private void Init(int endId, MapEndType endType, List<MapEvent> finishEventList)
    {
        MapData mapData = MapData.MapDataFetcher.GetMapDataByCopy(mMapId);
        if (mapData == null) return;
        bool isWin = endId > 0;
        mViewObj.ResultWinText.gameObject.SetActive(isWin);
        mViewObj.ResultFailText.gameObject.SetActive(!isWin);
        switch (endType)
        {
            case MapEndType.TriggerEnd:
                MapEvent end = MapEvent.MapEventFetcher.GetMapEventByCopy(endId);
                if (end != null)
                {
                    mViewObj.TitleText.text = end.name;
                    mViewObj.EndDescText.text = "\u3000\u3000" + end.EndDesc;
                }
                break;
            case MapEndType.PlayerExit:
                mViewObj.TitleText.text = LangMgr.GetText("退出");
                mViewObj.EndDescText.text = "\u3000\u3000" + LobbyDialogue.GetDescStr("map_exitEnd");
                break;
            case MapEndType.Dead:
                mViewObj.TitleText.text = LangMgr.GetText("死亡");
                Window_DungeonMap mapWin = UIRootMgr.Instance.GetOpenListWindow(WinName.Window_DungeonMap)as Window_DungeonMap;
                string npcName = "某人";
                if (mapWin != null)
                {
                    OldHero hero = OldHero.HeroFetcher.GetHeroByCopy(mapWin.LastPveNpc);
                    if (hero != null) npcName = hero.name;
                }
                mViewObj.EndDescText.text = LobbyDialogue.GetDescStr("map_deadEnd", npcName);
                break;
        }
        mViewObj.OkText.text = LangMgr.GetText("确定");
        mViewObj.OkBtn.SetOnClick(BtnEvt_Ok);
        FreshItem(endId, finishEventList);
    }


    //刷新事件项
    void FreshItem(int endId , List<MapEvent> finishEventList)
    {
        MapData map = MapData.MapDataFetcher.GetMapDataByCopy(mMapId);
        for (int i = 0; i < finishEventList.Count; i++)
        {
            if (finishEventList[i].idx == endId) //移除结局
            {
                finishEventList.RemoveAt(i);
                break;
            }
        }
        List<MapEvent> allEvendList = new List<MapEvent>();
        for (int i = 0; i < map.Quest.Length; i++)
        {
            MapEvent mapEvent = MapEvent.MapEventFetcher.GetMapEventByCopy(map.Quest[i]);
            if (mapEvent != null) allEvendList.Add(mapEvent);
        }
        mViewObj.QuestTitleText.text = string.Format("支线: {0}/{1}", finishEventList.Count, allEvendList.Count);

        mEventItemList = TAppUtility.Instance.AddViewInstantiate<EventItemObj>(mEventItemList, mViewObj.Part_MapEndFinishMapEvent, mViewObj.ItemRoot, allEvendList.Count);

        for (int i = 0; i < allEvendList.Count; i++)
        {
            MapEvent.MapEventStatus mapStatus = MapEvent.MapEventStatus.None;
            for (int j = 0; j < finishEventList.Count; j++)
            {
                if (allEvendList[i].idx == finishEventList[j].idx)
                { mapStatus = MapEvent.MapEventStatus.Finish; break; }
            }
            mEventItemList[i].NameText.text = allEvendList[i].name;
            mEventItemList[i].StatusText.text = mapStatus == MapEvent.MapEventStatus.None ? "未完成" : "完成";

        }
    }


    public void BtnEvt_Ok()
    {
        if (mCloseCallback != null) mCloseCallback();
        mCloseCallback = null;
        CloseWindow();
    }


}