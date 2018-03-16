using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Panel_RewardLook : MonoBehaviour, IScrollWindow
{
    public class ViewObj
    {
        public Text TextTitleName;
        public Text TextFloorTitle;
        public Text TextRewardTitle;
        public Transform RewardItemRoot;
        public GameObject Part_TowerRewardItem;
        public UIScroller Scroller;
        public ViewObj(UIViewBase view)
        {

            if (TextTitleName == null) TextTitleName = view.GetCommon<Text>("TextTitleName");
            if (TextFloorTitle == null) TextFloorTitle = view.GetCommon<Text>("TextFloorTitle");
            if (TextRewardTitle == null) TextRewardTitle = view.GetCommon<Text>("TextRewardTitle");
            if (RewardItemRoot == null) RewardItemRoot = view.GetCommon<Transform>("RewardItemRoot");
            if (Part_TowerRewardItem == null) Part_TowerRewardItem = view.GetCommon<GameObject>("Part_TowerRewardItem");
            if (Scroller == null) Scroller = view.GetCommon<UIScroller>("Scroller");
        }
    }
    private ViewObj mViewObj;
    private List<Tower> mTowerList;
    public class TaskItemObj : ItemIndexObj
    {
        public Text TextName;
        public Text TextReward;
        public Button BtnBg;
        public Transform GetMaskRoot;

        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (TextReward == null) TextReward = view.GetCommon<Text>("TextReward");
            if (BtnBg == null) BtnBg = view.GetCommon<Button>("BtnBg");
            if (GetMaskRoot == null) GetMaskRoot = view.GetCommon<Transform>("GetMaskRoot");
        }
    }
    private Dictionary<int, TaskItemObj> mRewardItemList = new Dictionary<int, TaskItemObj>();

    private Window_Tower mParentWin;
    public void Init(Window_Tower win)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        mParentWin = win;
        Fresh();
    }
    public void Reset()
    {
        mRewardItemList.Clear();
    }
    public void FreshScrollItem(int index)
    {
        if (index > mTowerList.Count || mTowerList[index] == null)
        {
            Debug.LogError(string.Format("{Tower不存在；index:{0}}", index));
            return;
        }
        TaskItemObj item;
        if (mViewObj.Scroller._unUsedQueue.Count > 0)
        {
            item = (TaskItemObj)mViewObj.Scroller._unUsedQueue.Dequeue();
        }
        else
        {
            item = new TaskItemObj();
            item.Init(mViewObj.Scroller.GetNewObj(mViewObj.RewardItemRoot, mViewObj.Part_TowerRewardItem));
        }
        item.Scroller = mViewObj.Scroller;
        item.Index = index;
        mViewObj.Scroller._itemList.Add(item);

        item.TextName.text = mTowerList[index].name;
        Loot loot = Loot.LootFetcher.GetLootByCopy(mTowerList[index].SpeReward);
        if (loot == null || loot.LootsId.Length < 1)
        {
            TDebug.LogError(string.Format("爬塔特殊奖励错误，id:{0}", mTowerList[index].idx));
            return;
        }
        GoodsToDrop goods = new GoodsToDrop(loot.LootsId[0], loot.LootsNum[0], loot.LootsType[0]);
        item.TextReward.text = goods.GetString();

        //是否已获得
        bool isGot = PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex >= mTowerList[index].Order;
        item.GetMaskRoot.gameObject.SetActive(isGot);

        if (!mRewardItemList.ContainsKey(index))
            mRewardItemList.Add(index, item);
        else
            mRewardItemList[index] = item;

    }


    public void CloseWindow()
    {
    }

    public int aaa;
    public int bbb;
    public void Fresh()
    {
        gameObject.SetActive(true);
        mTowerList = Tower.TowerFetcher.GetSpeRewardTowersNoCopy();

        //已获得的排在后面，order小的排在后面
        mTowerList.Sort((x, y) =>
        {
            if (x.Order > PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex || y.Order > PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex)
            {
                if (x.Order > PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex && y.Order > PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex)
                {
                    return x.Order.CompareTo(y.Order);
                }
                return (x.Order > PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex) ? -1 : 1;//返回-1则x排在前面
            }
            else { return x.Order.CompareTo(y.Order); }
        });

        Reset();
        mViewObj.Scroller.Init(this, mTowerList.Count);


        //if (mRewardItemList == null) mRewardItemList = new List<TaskItemObj>();
        //mRewardItemList = TAppUtility.Instance.AddViewInstantiate<TaskItemObj>(mRewardItemList, mViewObj.Part_TowerRewardItem,
        //    mViewObj.RewardItemRoot, towerList.Count);

        ////刷新奖励item
        //for (int i = 0; i < mTowerList.Count; i++)
        //{
        //    mRewardItemList[i].TextName.text = mTowerList[i].Name;
        //    Loot loot = Loot.LootFetcher.GetLootByCopy(mTowerList[i].SpeReward);
        //    if (loot == null || loot.LootsId.Length < 1)
        //    {
        //        TDebug.LogError(string.Format("爬塔特殊奖励错误，id:{0}", mTowerList[i].Idx));
        //        continue;
        //    }
        //    GoodsDrop goods = new GoodsDrop(loot.LootsId[0], loot.LootsNum[0], loot.LootsType[0]);
        //    mRewardItemList[i].TextReward.text = goods.GetString();

        //    //是否已获得
        //    bool isGot = PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex >= mTowerList[i].Order;
        //    mRewardItemList[i].GetMaskRoot.gameObject.SetActive(isGot);
        //}

        //mViewObj.TextTitleName.text = "仙魔录奖励";
        //mViewObj.TextFloorTitle.text = "奖励层数";
        //mViewObj.TextRewardTitle.text = "奖励内容";

    }
}
