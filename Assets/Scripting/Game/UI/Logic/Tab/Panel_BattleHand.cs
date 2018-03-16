using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Panel_BattleHand : WindowBase 
{
    public class ViewObj
    {
        public GameObject Part_BattleSpellSelect;
        public Text TextSelected;
        public Text TextWaitSelect;
        public Transform RootSelectItem;
        public Transform RootWaitSelectItem;
        public Text WaitSelectTitleText;
        public Text SelectedTitleText;
        public Button BattleHandOkBtn;
        public Text TimeText;
        public Text RoundText;
        public Transform BgRoot;

        public ViewObj(UIViewBase view)
        {
            if (Part_BattleSpellSelect == null) Part_BattleSpellSelect = view.GetCommon<GameObject>("Part_BattleSpellSelect");
            if (TextSelected == null) TextSelected = view.GetCommon<Text>("TextSelected");
            if (TextWaitSelect == null) TextWaitSelect = view.GetCommon<Text>("TextWaitSelect");
            if (RootSelectItem == null) RootSelectItem = view.GetCommon<Transform>("RootSelectItem");
            if (RootWaitSelectItem == null) RootWaitSelectItem = view.GetCommon<Transform>("RootWaitSelectItem");
            if (WaitSelectTitleText == null) WaitSelectTitleText = view.GetCommon<Text>("WaitSelectTitleText");
            if (SelectedTitleText == null) SelectedTitleText = view.GetCommon<Text>("SelectedTitleText");
            if (BattleHandOkBtn == null) BattleHandOkBtn = view.GetCommon<Button>("BattleHandOkBtn");
            if (TimeText == null) TimeText = view.GetCommon<Text>("TimeText");
            if (RoundText == null) RoundText = view.GetCommon<Text>("RoundText");
            if (BgRoot == null) BgRoot = view.GetCommon<Transform>("BgRoot");
        }
    }

    public class SpellItemObj : SmallViewObj
    {
        public Button BtnBg;
        public Text TextName;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (BtnBg == null) BtnBg = view.GetCommon<Button>("BtnBg");
            if (TextName == null) TextName = view.GetCommon<Text>("TextName");           
        }
    }
    class SelectSpellItem
    {
        public Spell MySpell;
        public int OriginIndex;
        public SelectSpellItem() { }
    }
	
    private ViewObj mViewObj;
    private List<SpellItemObj> mItemListWait = new List<SpellItemObj>();
    private List<SpellItemObj> mItemListSelect = new List<SpellItemObj>();
    private Spell[] mWaitSeletSpellList;
    private List<SelectSpellItem> mSelectSpellList = new List<SelectSpellItem>();
    private Window_BattleTowSide mBattleWin;
    private float mCurSelectTime;   //当前选择的等待时间

    private float mCurBigRoundTime;  //当前大回合时间
    public void Init(Spell[] canSpellList, int roundNum, Window_BattleTowSide battleWin)
    {
        gameObject.SetActive(true);
        if(mViewObj == null) mViewObj = new ViewObj(gameObject.GetComponent<UIViewBase>());
        mViewObj.BgRoot.gameObject.SetActive(true);

        mBattleWin = battleWin;
        mCurSelectTime = 0;
        mViewObj.RoundText.text = string.Format("第{0}回合", roundNum);
        mViewObj.SelectedTitleText.text = LangMgr.GetText("已选技能");
        mViewObj.WaitSelectTitleText.text = LangMgr.GetText("可选技能");

        mSelectSpellList = new List<SelectSpellItem>();
        FreshSelectSpellItem(mSelectSpellList);

        //for (int i = 0; i < canSpellList.Length; i++)   //将空技能，赋为普通攻击或待机
        //{
        //    if (canSpellList[i] == null)
        //    {
        //        canSpellList[i] = Skill.SkillFetcher.GetSpellByCopy(GetNullNameByIndex(i));
        //    }
        //}
        mWaitSeletSpellList = canSpellList;
        if (mItemListWait == null) mItemListWait = new List<SpellItemObj>();
        mItemListWait = TAppUtility.Instance.AddViewInstantiate<SpellItemObj>(mItemListWait, mViewObj.Part_BattleSpellSelect, mViewObj.RootWaitSelectItem, mWaitSeletSpellList.Length);
        for (int i = 0, length = mItemListWait.Count; i < length; i++)
        {    
            mItemListWait[i].BtnBg.enabled = true;
            mItemListWait[i].BtnBg.image.color = Color.white;
            mItemListWait[i].TextName.text = mWaitSeletSpellList[i].name;
            int tempIndex = i;
            mItemListWait[i].BtnBg.SetOnClick(delegate() { BtnEvt_HandItemClick(tempIndex, true); });
        }
        mViewObj.BattleHandOkBtn.SetOnClick(BtnEvt_HandOk);

        mCurBigRoundTime = -1;
        BattleMgr.PLAY_TIME_SCALE = 1;
    }

    void Update()
    {
        //超过选择时间后，自动选择技能上传
        int maxWaitTime = GameConstUtils.max_battle_hand_wait;
        if (mCurSelectTime >= 0 && mCurSelectTime < maxWaitTime)
        {
            mCurSelectTime += Time.deltaTime;
            mViewObj.TimeText.text = string.Format("剩余{0}秒", maxWaitTime - (int) mCurSelectTime);
        }
        else if (mCurSelectTime > maxWaitTime)
        {
            for (int i = 0; i < mWaitSeletSpellList.Length; i++)
            {
                bool isAddInSelected = mSelectSpellList.Exists(x => { return x.OriginIndex == i; });               
                if (!isAddInSelected)
                {
                    BtnEvt_HandItemClick(i, true);
                }
            }
            BtnEvt_HandOk();
        }

        if (mCurSelectTime < 0) //正在播放动画时
        {
            //超过每大回合显示时间后，进行强制加速
            int maxBigRoundTime = GameConstUtils.max_battle_hand_show_time;
            if (mCurBigRoundTime >= 0)
            {
                mCurBigRoundTime += Time.deltaTime;
            }
            else if (mCurBigRoundTime > maxBigRoundTime) //加速
            {
                BattleMgr.PLAY_TIME_SCALE = 6f;
                mCurBigRoundTime = -1;
            }
        }
        
    }

    void FreshSelectSpellItem(List<SelectSpellItem> spellList) //刷新已选择的技能
    {
        if (mItemListSelect == null) mItemListSelect = new List<SpellItemObj>();
        mItemListSelect = TAppUtility.Instance.AddViewInstantiate<SpellItemObj>(mItemListSelect, mViewObj.Part_BattleSpellSelect, mViewObj.RootSelectItem, spellList.Count);
        for (int i = 0, length = spellList.Count; i < length; i++)
        {
            mItemListSelect[i].BtnBg.enabled = true;
            mItemListSelect[i].BtnBg.image.color = Color.white;
            mItemListSelect[i].TextName.text = spellList[i].MySpell.name;
            int tempIndex = i;
            mItemListSelect[i].BtnBg.SetOnClick(delegate() { BtnEvt_HandItemClick(tempIndex, false); });
        }
    }

    void BtnEvt_HandItemClick(int index, bool isEquip)
    {
        if (isEquip)
        {
            SelectSpellItem item = new SelectSpellItem();
            item.MySpell = mWaitSeletSpellList[index];
            item.OriginIndex = index;
            mSelectSpellList.Add(item);
            mItemListWait[index].BtnBg.enabled = false;
            mItemListWait[index].BtnBg.image.color = Color.gray;        
        }
        else
        {
            mItemListWait[mSelectSpellList[index].OriginIndex].BtnBg.enabled = true;
            mItemListWait[mSelectSpellList[index].OriginIndex].BtnBg.image.color = Color.white;
            mSelectSpellList.RemoveAt(index);
        }
        FreshSelectSpellItem(mSelectSpellList);
    }

    void BtnEvt_HandOk()
    {
        mCurSelectTime = -1;
        if (mSelectSpellList.Count < (int)Spell.PosType.Max)
        {
            UIRootMgr.Instance.Window_UpTips.InitTips("需要将技能选择完", Color.red);
            return;
        }
        TDebug.Log("选择完毕");
        UIRootMgr.Instance.IsLoading = true;
        int[] spellIds = new int[mSelectSpellList.Count];
        for (int i = 0; i < spellIds.Length; i++)
        {
            spellIds[i] = mSelectSpellList[i].MySpell.idx;
        }
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_BattleHand(spellIds));
    }

    public void GetBattleLog()
    {
        UIRootMgr.Instance.IsLoading = false;
        mViewObj.BgRoot.gameObject.SetActive(false);
        mCurBigRoundTime = 0;
    }

    

}
