using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_AchieveReward : WindowBase,IScrollWindow {

    public class ViewObj
    {
        public GameObject Part_ItemAchieveReward;
        public Button BtnMask;
        public UIScroller Scroller;
        public Transform RootItem;
        public Text TextPoint;

        public ViewObj(UIViewBase view)
        {
            if (Part_ItemAchieveReward == null) Part_ItemAchieveReward = view.GetCommon<GameObject>("Part_ItemAchieveReward");
            if (BtnMask == null) BtnMask = view.GetCommon<Button>("BtnMask");
            if (Scroller == null) Scroller = view.GetCommon<UIScroller>("Scroller");
            if (RootItem == null) RootItem = view.GetCommon<Transform>("RootItem");
            if (TextPoint == null) TextPoint = view.GetCommon<Text>("TextPoint");
        }
    }
    public class RewardItemObj : ItemIndexObj
    {
        public int RewardId;
        public Text TextPoint;
        public Text TextReward;
        public Button BtnGet;
        public Text TextBtnGet;
        public override void Init(UIViewBase view)
        {
            if (View == null) base.Init(view);
            if (TextPoint == null) TextPoint = view.GetCommon<Text>("TextPoint");
            if (TextReward == null) TextReward = view.GetCommon<Text>("TextReward");
            if (BtnGet == null) BtnGet = view.GetCommon<Button>("BtnGet");
            if (TextBtnGet == null) TextBtnGet = view.GetCommon<Text>("TextBtnGet");
        }
        public void InitItem(AchieveReward reward,bool isGot)
        {
            RewardId = reward.idx;
            TextReward.text = reward.name;
            TextPoint.text =string.Format("{0}积分",reward.Point);
            BtnGet.gameObject.SetActive(PlayerPrefsBridge.Instance.GetAchieveAccessor().AchievePoint >= reward.Point);
            TextBtnGet.text = isGot ? "已领取" : "领取";
            BtnGet.enabled = !isGot;
        }
    }

    private Dictionary<int, RewardItemObj> mRewardItemList = new Dictionary<int, RewardItemObj>();
    private List<AchieveReward> mAchRewardList = new List<AchieveReward>();
    private List<int> mGotAchReward;

    private ViewObj mViewObj;


    public void OpenWindow()
    {
        if (mViewObj == null) { mViewObj = new ViewObj(mViewBase); }
        OpenWin();
        Init();
        RegisterNetCodeHandler(NetCode_S.GetAchieveReward, S2C_GetAchieveReward);
    }

    public void FreshScrollItem(int index)
    {
        if (index > mAchRewardList.Count || mAchRewardList[index] == null)
        {
            TDebug.LogError(string.Format("{ Ach不存在；index:{0}}", index));
            return;
        }
        RewardItemObj item;
        if (mViewObj.Scroller._unUsedQueue.Count > 0)
        {
            item = (RewardItemObj)mViewObj.Scroller._unUsedQueue.Dequeue();
        }
        else
        {
            item = new RewardItemObj();
            item.Init(mViewObj.Scroller.GetNewObj(mViewObj.RootItem, mViewObj.Part_ItemAchieveReward));
        }
        item.Scroller = mViewObj.Scroller;
        item.Index = index;
        mViewObj.Scroller._itemList.Add(item);

        item.InitItem(mAchRewardList[index], mGotAchReward.Contains(mAchRewardList[index].idx));
        int tempId = mAchRewardList[index].idx;
        item.BtnGet.SetOnClick(delegate() { BtnEvt_GetAward(tempId); });

        if (!mRewardItemList.ContainsKey(index))
            mRewardItemList.Add(index, item);
        else
            mRewardItemList[index] = item;
    }
    public void Reset()
    {
        mRewardItemList.Clear();
    }

    void Init()
    {
        AchievementAccessor achieve = PlayerPrefsBridge.Instance.GetAchieveAccessor();
        mGotAchReward = achieve.GotAchieveAwardList;
        mAchRewardList = AchieveReward.AchieveRewardFetcher.GetAchieveRewardAllByCopy();   
        for (int i = 0,length = mAchRewardList.Count; i < length; i++)
        {
            if (mGotAchReward.Contains(mAchRewardList[i].idx))
                mAchRewardList[i].IsGot = true;
        }
        mAchRewardList.Sort((x, y) =>
        {
            if (x.IsGot || y.IsGot)
            {
                if (x.IsGot && y.IsGot)
                {
                    return x.Point.CompareTo(y.Point);
                }
                return x.IsGot ? 1 : -1;
            }
            else
            {
                return x.Point.CompareTo(y.Point);
            }
        });
        Reset();
        mViewObj.Scroller.Init(this, mAchRewardList.Count);
        mViewObj.BtnMask.SetOnClick(delegate() { CloseWindow(); });
        mViewObj.TextPoint.text = string.Format("成就积分：{0}", achieve.AchievePoint);
    }
    public void BtnEvt_GetAward(int id)
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_GetAchieveReward(id));
    }

    public void S2C_GetAchieveReward(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_GetAchieveReward msg = MessageBridge.Instance.S2C_GetAchieveReward(ios);
        UIRootMgr.LobbyUI.ShowDropInfo(msg.TranslateList);
        foreach (var item in mRewardItemList)
        {
            if (item.Value.RewardId == msg.rewardId)
            {
                item.Value.BtnGet.gameObject.SetActive(false);
            }
        }
    }
}
