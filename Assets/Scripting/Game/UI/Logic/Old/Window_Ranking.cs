using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_Ranking : WindowBase {

    public enum RankEnum
    {
        None=-1,
        Level,
        Sect,
        Gold,
        Achieve,
        Max,   
    }
    public class RankItemObj:SmallViewObj
    {
        public Image IconHead;
        public Text TextName;
        public Text TextContent0;
        public Text TextContent1;
        public Text TextRank;
        public Button BtnBg;

        public GameObject gameobject;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            IconHead = view.GetCommon<Image>("IconHead");
            TextName = view.GetCommon<Text>("TextName");
            TextContent0 = view.GetCommon<Text>("TextContent0");
            TextContent1 = view.GetCommon<Text>("TextContent1");
            TextRank = view.GetCommon<Text>("TextRank");
            BtnBg = view.GetCommon<Button>("BtnBg");        
            gameobject = view.gameObject;
        }
        public void InitItem(RankEnum type,NetRankBotting.RankItem item, int rank,SpritePrefab sprite)
        {        
            if(rank>1000)
                TextRank.text = "壹仟+";
            else
                TextRank.text = TUtility.GetRankNumStr(rank + 1);
            if (item == null) //初始化底部自身排行
            {
                item = new NetRankBotting.RankItem();
                GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
                item.playerName = player.Name;
                item.sectType = player.MySect;
                item.level = player.Level;
                item.playerIcon = player.HeadIconIdx;
                if (type == RankEnum.Gold)
                    item.num = player.Gold;
                else if (type == RankEnum.Achieve)
                    item.num = 0;
            }
            IconHead.sprite = sprite.GetSprite(HeadIcon.GetHeadIcon(item.playerIcon, item.heroIdx));
            TextName.text = item.playerName;
            Sect sect  = null;
            if (item.sectType != Sect.SectType.None)
                sect = Sect.SectFetcher.GetSectByCopy(item.sectType);
            HeroLevelUp lv = null;
            if(item.level>0)
                lv = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(item.level);
            switch (type)
            {
                case RankEnum.Level:
                    TextContent0.text = sect != null? sect.name:"无";
                    TextContent1.text = lv !=null? lv.name:"获取等级失败";
                    break;
                case RankEnum.Sect:
                    TextContent0.text = sect != null ? sect.name : "无";
                    TextContent1.text = lv !=null? lv.name:"获取等级失败";
                    break;
                case RankEnum.Gold:
                    TextContent0.text = lv != null ? lv.name : "获取等级失败";
                    TextContent1.text = string.Format("{0}", TUtility.GetMoneyString(item.num));
                    TDebug.Log("GetMoneyString===" + item.num + "==" + TUtility.GetMoneyString(item.num));
                    break;
                case RankEnum.Achieve:
                    TextContent0.text = lv != null ? lv.name : "获取等级失败";
                    TextContent1.text = item.num.ToString();
                    break;
            }
            gameobject.SetActive(true);
        }
       
    }
    public class ViewObj
    {
        public GameObject Part_ItemRanking;
        public Button BtnTab0;
        public Button BtnTab1;
        public Button BtnTab2;
        public Button BtnTab3;
        public Transform ItemRoot;
        public Text TextTitleName;
        public Text TextTitle0;
        public Text TextTitle1;
        public Text TextTitleRank;
        public RankItemObj MyRankItem;
        public GameObject PanelBtn;
        public Button BtnRoleInfo;
        public Button BtnPK;
        public Button BtnMask;
        public Text TimeText;
        public Text TextBtn0;
        public Text TextBtn1;
        public Text TextBtn2;
        public Text TextBtn3;
        public List<Text> TextBtnList;
        public List<Button> BtnRankTab;
        public ViewObj(UIViewBase view)
        {
            if (Part_ItemRanking == null) Part_ItemRanking = view.GetCommon<GameObject>("Part_ItemRanking");
            if (BtnTab0 == null) BtnTab0 = view.GetCommon<Button>("BtnTab0");
            if (BtnTab1 == null) BtnTab1 = view.GetCommon<Button>("BtnTab1");
            if (BtnTab2 == null) BtnTab2 = view.GetCommon<Button>("BtnTab2");
            if (BtnTab3 == null) BtnTab3 = view.GetCommon<Button>("BtnTab3");
            if (ItemRoot == null) ItemRoot = view.GetCommon<Transform>("ItemRoot");
            if (TextTitleName == null) TextTitleName = view.GetCommon<Text>("TextTitleName");
            if (TextTitle0 == null) TextTitle0 = view.GetCommon<Text>("TextTitle0");
            if (TextTitle1 == null) TextTitle1 = view.GetCommon<Text>("TextTitle1");
            if (TextTitleRank == null) TextTitleRank = view.GetCommon<Text>("TextTitleRank");          
            if (PanelBtn == null) PanelBtn = view.GetCommon<GameObject>("PanelBtn");
            if (BtnRoleInfo == null) BtnRoleInfo = view.GetCommon<Button>("BtnRoleInfo");
            if (BtnPK == null) BtnPK = view.GetCommon<Button>("BtnPK");
            if (BtnMask == null) BtnMask = view.GetCommon<Button>("BtnMask");
            if (TimeText == null) TimeText = view.GetCommon<Text>("TimeText");
            if (TextBtn0 == null) TextBtn0 = view.GetCommon<Text>("TextBtn0");
            if (TextBtn1 == null) TextBtn1 = view.GetCommon<Text>("TextBtn1");
            if (TextBtn2 == null) TextBtn2 = view.GetCommon<Text>("TextBtn2");
            if (TextBtn3 == null) TextBtn3 = view.GetCommon<Text>("TextBtn3");
            if (MyRankItem == null)
            {
                MyRankItem = new RankItemObj();
                MyRankItem.Init(view.GetCommon<UIViewBase>("MyRankItem"));
            }
            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtn0);
                TextBtnList.Add(TextBtn1);
                TextBtnList.Add(TextBtn2);
                TextBtnList.Add(TextBtn3);            }
            if (BtnRankTab == null)
            {
                BtnRankTab = new List<Button>();
                BtnRankTab.Add(BtnTab0);
                BtnRankTab.Add(BtnTab1);
                BtnRankTab.Add(BtnTab2);
                BtnRankTab.Add(BtnTab3);            }        }
        public void SelectTabBtn(RankEnum rank)
        {
            if (TextBtnList == null)
                return;
            for (int i = 0, length = TextBtnList.Count; i < length; i++)
            {
                if ((int)rank == i)
                {
                    TextBtnList[i].color = new Color(255f / 255, 242f / 255, 0f);
                    TextBtnList[i].GetComponent<Outline>().enabled = true;
                    BtnRankTab[i].enabled = false;
                }
                else
                {
                    TextBtnList[i].color = Color.black;
                    TextBtnList[i].GetComponent<Outline>().enabled = false;
                    BtnRankTab[i].enabled = true;
                }
            }
        }
    }
    private ViewObj mViewObj;
    private List<RankItemObj> mRankObjList;
    private int mListCount = 10;
    private WindowBig_Honor.ChildTab mCurRankIndex = 0; //地、天榜
    private NetRankBotting.RankItem[] mRankList;
    private RankEnum mCurRankEnum;
    public void OpenWindow(WindowBig_Honor.ChildTab index,RankEnum rank,bool refresh)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        if (!refresh)
            return;
        RegisterNetCodeHandler(NetCode_S.PullRank, S2C_PullRank);
        Init(index, rank);
    }

    void PullRankInfo(RankEnum rank ,int index)
    {
        UIRootMgr.Instance.IsLoading = true;
        TDebug.Log("C2S_PullRank==="+ReturnRankType(rank, index));
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullRank((byte)ReturnRankType(rank, index)));
    }

    void Init(WindowBig_Honor.ChildTab index, RankEnum rank)
    { 
        mCurRankIndex = index;
        mViewObj.BtnTab0.SetOnClick(delegate() { BtnEvt_PullRank(RankEnum.Level); });
        mViewObj.BtnTab1.SetOnClick(delegate() { BtnEvt_PullRank(RankEnum.Sect); });
        mViewObj.BtnTab2.SetOnClick(delegate() { BtnEvt_PullRank(RankEnum.Gold); });
        mViewObj.BtnTab3.SetOnClick(delegate() { BtnEvt_PullRank(RankEnum.Achieve); });
        mViewObj.BtnMask.SetOnClick(delegate() { mViewObj.PanelBtn.SetActive(false); });    
        if (mRankObjList == null) mRankObjList = new List<RankItemObj>();
        mRankObjList = TAppUtility.Instance.AddViewInstantiate<RankItemObj>(mRankObjList,mViewObj.Part_ItemRanking, mViewObj.ItemRoot, mListCount);
        for (int i = 0, length = mRankObjList.Count; i < length; i++)
        {
            mRankObjList[i].gameobject.SetActive(false);
        }
        mViewObj.MyRankItem.gameobject.SetActive(false);
        BtnEvt_PullRank(rank);
        mRankList = null;
        long curTime = AppTimer.CurTimeStampMsSecond;
        long originTime = AppTimer.OriginTime;
        int offsetTime = (int)((curTime - originTime) / 1000);
        int year = offsetTime / (24 * 60 * 60);
        int month = (offsetTime - (24 * 60 * 60 * year)) / (2 * 60 * 60);
        mViewObj.TimeText.text = string.Format("当前修真月：{0}月", month + 1);

    }
    public void BtnEvt_PullRank(RankEnum rank)
    {
        if (rank== RankEnum.Sect&& PlayerPrefsBridge.Instance.PlayerData.MySect == Sect.SectType.None&& UIRootMgr.Instance.MessageBox.ShowStatus("尚未加入宗门"))    
            return;
        mCurRankEnum = rank;
        PullRankInfo(rank,(int)mCurRankIndex);
        mViewObj.SelectTabBtn(rank);
    }

    public void BtnEvt_PullOtherRoleInfo(int index,int playerID)
    {
        mViewObj.PanelBtn.SetActive(false);
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullOtherRoleInfo, S2C_PullOtherRoleInfo);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullOtherRoleInfo(playerID));
       
    }

    public void BtnEvt_BattlePlayer(int playerId)//与玩家进行切磋
    {
        mViewObj.PanelBtn.SetActive(false);
        BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Battle, playerId, PVESceneType.RankWithPlayer);
    }
    public void BtnEvt_OpenPanelBtn(int index)
    {
        mViewObj.PanelBtn.SetActive(true);
        mViewObj.PanelBtn.transform.localPosition = new Vector3(-258,422-index*121.6f,0);
        int playerId = mRankList[index].playerUid;
        mViewObj.BtnRoleInfo.SetOnClick(delegate() { BtnEvt_PullOtherRoleInfo(index,playerId); });
        mViewObj.BtnPK.SetOnClick(delegate() { BtnEvt_BattlePlayer(playerId); });
    }

    void FreshRank(RankEnum rank ,NetRankBotting.RankItem[] rankList,int selfRank)
    {
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("HeadIconAtlas");
        SpritePrefab mHeadIconAtlas = go.GetComponent<SpritePrefab>();

        mRankList = rankList;
        switch (rank)
        {
            case RankEnum.Level:
                mViewObj.TextTitle0.text = "宗门";
                mViewObj.TextTitle1.text = "境界";
                break;
            case RankEnum.Sect:
                mViewObj.TextTitle0.text = "宗门";
                mViewObj.TextTitle1.text = "境界";
                break;
            case RankEnum.Gold:
                mViewObj.TextTitle0.text = "境界";
                mViewObj.TextTitle1.text = "灵石";
                break;
            case RankEnum.Achieve:
                mViewObj.TextTitle0.text = "境界";
                mViewObj.TextTitle1.text = "积分";
                break;
        }
        for (int i = 0, length = rankList.Length; i < length; i++)
        {
            if (i > mListCount - 1)
            {
                TDebug.LogError(string.Format("排行榜数据列表过长，length：{0}", length));
                return;
            }
            RankItemObj item = mRankObjList[i];
            item.InitItem(rank, rankList[i], i, mHeadIconAtlas);
            if (i == selfRank)
            {
                item.BtnBg.GetComponent<Image>().color = new Color(124/255f,242/255f,164/255f);
                item.BtnBg.enabled = false;            
            }
            else          
            {
                int index = i;
                item.BtnBg.enabled = true;  
                item.BtnBg.GetComponent<Image>().color = Color.white;              
                item.BtnBg.SetOnClick(delegate() { BtnEvt_OpenPanelBtn(index); });
            }
              
        }
        for (int i = rankList.Length, length = mRankObjList.Count; i < length; i++)
            mRankObjList[i].gameobject.SetActive(false);
        mViewObj.MyRankItem.InitItem(rank, null, selfRank, mHeadIconAtlas);
    }

    public void S2C_PullRank(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_PullRank msg = MessageBridge.Instance.S2C_PullRank(ios);
        FreshRank(RetrunRankEnum(msg.RankType), msg.RankList, msg.SelfRank);
    }
    public void S2C_PullOtherRoleInfo(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_PullOtherRoleInfo msg = MessageBridge.Instance.S2C_PullOtherRoleInfo(ios);
        //UIRootMgr.Instance.OpenWindowWithHide<Window_OtherRoleInfo>(WinName.Window_OtherRoleInfo,WinName.WindowBig_Honor,WinName.Window_Ranking).OpenWindow(msg.OtherRole);
     //   UIRootMgr.Instance.CloseWindow_InOpenList(WinName.WindowBig_Honor);
    }

    #region 枚举转换
    RankEnum RetrunRankEnum(RankType type)
    {
        switch (type)
        {
            case RankType.Achieve0:
                return RankEnum.Achieve;
            case RankType.Achieve1:
                return RankEnum.Achieve;
            case RankType.Gold0:
                return RankEnum.Gold;
            case RankType.Gold1:
                return RankEnum.Gold;
            case RankType.Level0:
                return RankEnum.Level;
            case RankType.Level1:
                return RankEnum.Level;
            case RankType.Sect0:
                return RankEnum.Sect;
            case RankType.Sect1:
                return RankEnum.Sect;
            default :
                return RankEnum.None;
        }
    }
    RankType ReturnRankType(RankEnum type, int index)
    {
        if (index == 0)
        {
            switch (type)
            {
                case RankEnum.Achieve:
                    return RankType.Achieve0;
                case RankEnum.Gold:
                    return RankType.Gold0;
                case RankEnum.Level:
                    return RankType.Level0;
                case RankEnum.Sect:
                    return RankType.Sect0;
                default:
                    return RankType.Max;
            }
        }
        else 
        {
            switch (type)
            {
                case RankEnum.Achieve:
                    return RankType.Achieve1;
                case RankEnum.Gold:
                    return RankType.Gold1;
                case RankEnum.Level:
                    return RankType.Level1;
                case RankEnum.Sect:
                    return RankType.Sect1;
                default:
                    return RankType.Max;
            }
        }
    }
    #endregion

}
