using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class Window_ChoosePlotDungeon : WindowBase, IScrollWindow
{

    public class ViewObj
    {
        public GameObject Part_ItemDungeon;
        public Transform ItemRoot;
        public Material CircleRectMat;
        public Material GreyAndRectImageMat;
        public ScrollRect ScrollView;
        public UIScroller Scroller;
        public ViewObj(UIViewBase view)
        {
            Part_ItemDungeon = view.GetCommon<GameObject>("Part_ItemDungeon");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            CircleRectMat = view.GetCommon<Material>("CircleRectMat");
            GreyAndRectImageMat = view.GetCommon<Material>("GreyAndRectImageMat");
            Scroller = view.GetCommon<UIScroller>("Scroller");
            ScrollView = view.GetCommon<ScrollRect>("ScrollView");
        }
        public void ResetSiteScroll()
        {
            ScrollView.StopMovement();
            Vector3 tempPos = ItemRoot.localPosition;
            ItemRoot.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
            Scroller.OnValueChange(Vector2.zero);
        }
    }
    public class SectHangItemObj : ItemIndexObj
    {
        public Image Icon;
        public Text TextName;
        public Button InfoBtn;
        public Button EnterBtn;
        public Text InfoBtnText;
        public Text EnterBtnText;
        public GameObject LimitMask;
        public Text TextOpenDemand;
        public Button OpenEventBtn;
        public Text OpenEventText;
        public int MapId;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (Icon == null) Icon = view.GetCommon<Image>("Icon");
            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (InfoBtn == null) InfoBtn = view.GetCommon<Button>("InfoBtn");
            if (EnterBtn == null) EnterBtn = view.GetCommon<Button>("EnterBtn");
            if (InfoBtnText == null) InfoBtnText = view.GetCommon<Text>("InfoBtnText");
            if (EnterBtnText == null) EnterBtnText = view.GetCommon<Text>("EnterBtnText");
            if (LimitMask == null) LimitMask = view.GetCommon<GameObject>("LimitMask");
            if (TextOpenDemand == null) TextOpenDemand = view.GetCommon<Text>("TextOpenDemand");
            //if (OpenEventBtn == null) OpenEventBtn = view.GetCommon<Button>("OpenEventBtn");
            if (OpenEventText == null) OpenEventText = view.GetCommon<Text>("OpenEventText");
        }

    }
    private Dictionary<int, SectHangItemObj> mMapItemList = new Dictionary<int, SectHangItemObj>();
    private List<MapData> mMapDataList;
    private List<int> mCanRewardMapIdList = new List<int>();
    private ViewObj mViewObj;
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        mViewObj.CircleRectMat.SetFloat("RadiusScale", 1.173f);
        mViewObj.CircleRectMat.SetFloat("RadiusRatio", 3.25f);
        mViewObj.GreyAndRectImageMat.SetFloat("RadiusScale", 1.173f);
        mViewObj.GreyAndRectImageMat.SetFloat("RadiusRatio", 3.25f);
        OpenWin();
        FreshBadge(); //在初始化item之前赋值
        Init();
    }
    public void Reset()
    {
        mMapItemList.Clear();
    }
    public override void FreshBadge()
    {
        base.FreshBadge();
        foreach (var temp in mMapItemList)
        {
            if(temp.Value!=null) BadgeTips.SetBadgeViewFalse(temp.Value.InfoBtn.transform);
        }
        mCanRewardMapIdList = MapEvent.GetMapsByEvents(PlayerPrefsBridge.Instance.GetCanRewardList(), MapData.MapType.SingleMap);
    }

    public void FreshScrollItem(int index)
    {
        if (index > mMapDataList.Count || mMapDataList[index] == null)
        {
            TDebug.LogError(string.Format("{Item不存在；index:{0}}", index));
            return;
        }
        SectHangItemObj item;
        if (mViewObj.Scroller._unUsedQueue.Count > 0)
        {
            item = (SectHangItemObj)mViewObj.Scroller._unUsedQueue.Dequeue();
        }
        else
        {
            item = new SectHangItemObj();
            item.Init(mViewObj.Scroller.GetNewObj(mViewObj.ItemRoot, mViewObj.Part_ItemDungeon));
        }
        item.Scroller = mViewObj.Scroller;
        item.Index = index;
        mViewObj.Scroller._itemList.Add(item);

        MapData data = mMapDataList[index];
        FreshItem(item, data);

        if (!mMapItemList.ContainsKey(index))
            mMapItemList.Add(index, item);
        else
            mMapItemList[index] = item;
    }

    void FreshItem(SectHangItemObj item, MapData data)
    {
        item.MapId = data.idx;
        item.TextName.text = data.name;// + "秘境";
        item.Icon.sprite = GetAsset<Sprite>(SharedAsset.Instance.LoadSpritePart<Sprite>(data.Icon));
        int mapIdx = data.idx;
        bool canEnter = false;
        if (PlayerPrefsBridge.Instance.PlayerData.Level >= data.OpenLevel) //判断等级条件是否达到
        {
            canEnter = true;
        }
        BadgeTips.SetBadgeViewFalse(item.InfoBtn.transform);
        for (int i = 0; i < mCanRewardMapIdList.Count; i++) //显示红点
        {
            if (mCanRewardMapIdList[i] == item.MapId)
            {
                BadgeTips.SetBadgeView(item.InfoBtn.transform);
            }
        }
        item.OpenEventText.text = LangMgr.GetText("领取");
        if (canEnter)
        {
            item.LimitMask.gameObject.SetActive(false);
            item.EnterBtn.SetOnClick(delegate() { BtnEvt_Enter(mapIdx); });
            item.Icon.material = mViewObj.CircleRectMat;
            item.EnterBtn.gameObject.SetActive(true);
            item.InfoBtn.gameObject.SetActive(true);
            item.InfoBtn.SetOnClick(delegate() { BtnEvt_OpenMapEventList(mapIdx); });
        }
        else
        {
            item.LimitMask.gameObject.SetActive(true);
            HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(data.OpenLevel);  //显示等级限制
            item.TextOpenDemand.text = levelUp.name;
            item.Icon.material = mViewObj.GreyAndRectImageMat;
            item.EnterBtn.gameObject.SetActive(false);
            item.InfoBtn.gameObject.SetActive(false);
        }
    }

    void Init()
    {
        //mMapDataList = new List<MapData>();
        //List<MapData> mapList = MapData.MapDataFetcher.GetMapDataListNoCopy(MapData.MapType.SingleMap); //获取所有单人秘境
        //mapList.Sort((x, y) => { return x.Order.CompareTo(y.Order); });  //排序秘境
        //for (int i = 0, length = mapList.Count; i < length; i++)
        //{
        //    mMapDataList.Add(mapList[i]);
        //    if (PlayerPrefsBridge.Instance.PlayerData.Level < mapList[i].OpenLevel) break;
        //}

        //////////////////////////////////////////////
        //Reset();
        //mViewObj.Scroller.Init(this, mMapDataList.Count);
        //mViewObj.ResetSiteScroll();

    }
    void BtnEvt_Enter(int mapIdx)//进入秘境
    {
        UIRootMgr.Instance.OpenWindow<Window_DungeonMap>(WinName.Window_DungeonMap).OpenWindow(mapIdx);
    }

    //void BtnEvt_OpenIntroduce(int mapId)
    //{
    //    UIRootMgr.Instance.OpenWindow<Window_MapEventList>(WinName.Window_MapEventList, CloseUIEvent.None).OpenWindow(mapId);
    //}

    //void CloseIntroduce()
    //{
    //    mViewObj.IntroduceRoot.gameObject.SetActive(false);
    //}

    void BtnEvt_OpenMapEventList(int mapId)
    {
        bool isMapExist = PlayerPrefsBridge.Instance.GetMapEventAccessorCopy().IsMapExsit(mapId);
        if (isMapExist)
        {
            UIRootMgr.Instance.OpenWindow<Window_MapEventList>(WinName.Window_MapEventList, CloseUIEvent.None).OpenWindow(mapId);
        }
        else
        {
            int curMapId = mapId;
            ServPacketHander del = delegate(BinaryReader ios)
            {
                RegisterNetCodeHandler(NetCode_S.PullInfo, null);
                UIRootMgr.Instance.IsLoading = false;
                UIRootMgr.Instance.OpenWindow<Window_MapEventList>(WinName.Window_MapEventList, CloseUIEvent.None).OpenWindow(curMapId);
            };
            UIRootMgr.Instance.IsLoading = true;
            RegisterNetCodeHandler(NetCode_S.PullInfo, null);
            RegisterNetCodeHandler(NetCode_S.PullInfo, del);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.MapEvent, curMapId));
        }

    }
}
