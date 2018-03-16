using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Panel_TowerRank : MonoBehaviour
{
    public class ViewObj
    {
        public GameObject Part_TowerItemRanking;
        public Transform ItemRoot;
        public Transform PanelBtn;
        public Button BtnMask;
        public Button BtnRoleInfo;
        public Text TextBtnRoleInfo;
        public ViewObj(UIViewBase view)
        {
            if (Part_TowerItemRanking == null) Part_TowerItemRanking = view.GetCommon<GameObject>("Part_TowerItemRanking");
            if (ItemRoot == null) ItemRoot = view.GetCommon<Transform>("ItemRoot");
            if (PanelBtn == null) PanelBtn = view.GetCommon<Transform>("PanelBtn");
            if (BtnMask == null) BtnMask = view.GetCommon<Button>("BtnMask");
            if (BtnRoleInfo == null) BtnRoleInfo = view.GetCommon<Button>("BtnRoleInfo");
            if (TextBtnRoleInfo == null) TextBtnRoleInfo = view.GetCommon<Text>("TextBtnRoleInfo");        }
    }
    private ViewObj mViewObj;

    public class RankItemObj : SmallViewObj
    {
        public Image IconHead;
        public Text TextName;
        public Text TextSect;
        public Text TextRank;
        public Button BtnBg;
        public Text TextLevel;
        public Text TextFloor;        public override void Init(UIViewBase view)
        {
            base.Init(view);

            if (IconHead == null) IconHead = view.GetCommon<Image>("IconHead");
            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (TextSect == null) TextSect = view.GetCommon<Text>("TextSect");
            if (TextRank == null) TextRank = view.GetCommon<Text>("TextRank");
            if (BtnBg == null) BtnBg = view.GetCommon<Button>("BtnBg");
            if (TextLevel == null) TextLevel = view.GetCommon<Text>("TextLevel");
            if (TextFloor == null) TextFloor = view.GetCommon<Text>("TextFloor");
            if (TextRank == null) TextRank = view.GetCommon<Text>("TextRank");        }
    }
    private List<RankItemObj> mRankObjList = new List<RankItemObj>();
    private NetRankBotting.RankItem[] mRankList;
    private int mCurRankIndex;

    private Window_Tower mParentWin;
    public void Init(Window_Tower win,bool refresh)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        mParentWin = win;
        gameObject.SetActive(true);
        if (!refresh)
            return;
        mViewObj.BtnMask.SetOnClick(delegate() { mViewObj.PanelBtn.gameObject.SetActive(false); });

        mParentWin.RegisterNetCodeHandler(NetCode_S.PullRank, S2C_PullRank);
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullRank((byte)RankType.Tower));
    }
    public void CloseWindow()
    {
    }

    


    public void Fresh()
    {
        if (mRankObjList == null) mRankObjList = new List<RankItemObj>();
    }

    void FreshRank(NetRankBotting.RankItem[] rankList, int selfRank)
    {
        mRankList = rankList;
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("HeadIconAtlas");
        SpritePrefab mHeadIconAtlas = go.GetComponent<SpritePrefab>();
        mRankObjList = TAppUtility.Instance.AddViewInstantiate<RankItemObj>(mRankObjList, mViewObj.Part_TowerItemRanking, mViewObj.ItemRoot, rankList.Length);
        if (mRankObjList.Count > rankList.Length)
        {
            Debug.Log("排行榜数据缺失，length："+ rankList.Length);
            return;
        }
        for (int i = 0; i < mRankObjList.Count; i++)
        {         
            mRankObjList[i].TextName.text = rankList[i].playerName;
            mRankObjList[i].TextSect.text = Sect.GetSectName(rankList[i].sectType);
            mRankObjList[i].TextLevel.text = HeroLevelUp.GetStateName(rankList[i].level);
            mRankObjList[i].TextFloor.text = rankList[i].num.ToString();
            mRankObjList[i].TextRank.text = TUtility.GetRankNumStr(i + 1);

            mRankObjList[i].IconHead.sprite = mHeadIconAtlas.GetSprite(HeadIcon.GetHeadIcon(rankList[i].playerIcon, rankList[i].heroIdx));//TODO:传玩家性别 
            if (rankList[i].playerUid == PlayerPrefsBridge.Instance.PlayerData.PlayerUid)
            {
                mRankObjList[i].BtnBg.GetComponent<Image>().color = new Color(124 / 255f, 242 / 255f, 164 / 255f);
                mRankObjList[i].BtnBg.enabled = false;
            }
            else
            {
                int index = i;
                mRankObjList[i].BtnBg.enabled = true;
                mRankObjList[i].BtnBg.GetComponent<Image>().color = Color.white;
                mRankObjList[i].BtnBg.SetOnClick(delegate() { BtnEvt_OpenPanelBtn(index); });
            }

        }
    }

    public void BtnEvt_OpenPanelBtn(int index)
    {
        mViewObj.PanelBtn.gameObject.SetActive(true);
        mViewObj.PanelBtn.transform.localPosition = new Vector3(-258, 422 - index * 121.6f, 0);
        int playerId = mRankList[index].playerUid;
        mViewObj.BtnRoleInfo.SetOnClick(delegate() { BtnEvt_PullOtherRoleInfo(index, playerId); });
    }
    public void S2C_PullRank(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_PullRank msg = MessageBridge.Instance.S2C_PullRank(ios);
        FreshRank( msg.RankList, msg.SelfRank);
    }

    public void BtnEvt_PullOtherRoleInfo(int index, int playerID)
    {
        mViewObj.PanelBtn.gameObject.SetActive(false);
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullOtherRoleInfo, S2C_PullOtherRoleInfo);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullOtherRoleInfo(playerID));
    }
    public void S2C_PullOtherRoleInfo(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_PullOtherRoleInfo msg = MessageBridge.Instance.S2C_PullOtherRoleInfo(ios);    
    }
}
