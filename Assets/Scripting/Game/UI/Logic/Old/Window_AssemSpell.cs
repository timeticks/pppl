using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_AssemSpell : WindowBase,IScrollWindow
{
    public class ViewObj
    {   // 不可直接粘贴复制，内包含mini窗口与大窗口2中View
        public Text TextWearNum;
        public Button BtnAtk;
        public Button BtnDef;
        public Button BtnPassive;
        public GameObject Part_SelectableItemBtnt;
        public Transform ItemRoot;
        public TextButton BtnLearn;
        public TextButton BtnChange;
        public Button BtnTurnRight;
        public Button BtnTurnLeft;
        public Text TextName;
        public Text TextLevel;
        public GameObject PageOne;
        public GameObject PageTwo;
        public Text TextCostGold;
        public Text TextCostPotential;
        public Text TextDesc;
        public Text TextEffect;
        public Text TextUnlockLvA;
        public Text TextUnlockLvB;
        public Text TextUnlockLvC;
        public Text TextUnlockLvD;
        public Text TextAttributeA;
        public Text TextAttributeB;
        public Text TextAttributeC;
        public Text TextAttributeD;
        public Text TextTitleChange;
        public Text TextLevelDemand;
        public List<Text> ListTextAttr;
        public List<Text> ListTextBookSpell;
        public GameObject RightBg;
        public Text TextBtnAtk;
        public Text TextBtnDef;
        public Text TextPassive;
        public List<Text> TextBtnList;
        public List<Button> BtnSpellTab;
        public TextButton BtnFastLearn;
        public UIScroller Scroller;
        public Button BtnProm;
        public Text TextBtnProm;
        public GameObject PanelPromSpell;
        public Text TextNull;
        public UIAnimationBaseCtrl AddAnimation;
        public Text TextAdditionA;
        public Text TextAdditionB;
        public Text TextAdditionC;
        public Text TextAdditionD;
       
        public List<Text> ListTextAttrAdd;
        public ViewObj(UIViewBase view)
        {
            TextWearNum = view.GetCommon<Text>("TextWearNum");
            BtnAtk = view.GetCommon<Button>("BtnAtk");
            BtnDef = view.GetCommon<Button>("BtnDef");
            BtnPassive = view.GetCommon<Button>("BtnPassive");
            Part_SelectableItemBtnt = view.GetCommon<GameObject>("Part_SelectableItemBtnt");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            BtnLearn = view.GetCommon<TextButton>("BtnLearn");
            BtnChange = view.GetCommon<TextButton>("BtnChange");
            BtnTurnRight = view.GetCommon<Button>("BtnTurnRight");
            BtnTurnLeft = view.GetCommon<Button>("BtnTurnLeft");
            TextName = view.GetCommon<Text>("TextName");
            TextLevel = view.GetCommon<Text>("TextLevel");
            PageOne = view.GetCommon<GameObject>("PageOne");
            PageTwo = view.GetCommon<GameObject>("PageTwo");
            TextCostGold = view.GetCommon<Text>("TextCostGold");
            TextCostPotential = view.GetCommon<Text>("TextCostPotential");
            TextDesc = view.GetCommon<Text>("TextDesc");
            TextEffect = view.GetCommon<Text>("TextEffect");
            TextUnlockLvA = view.GetCommon<Text>("TextUnlockLvA");
            TextUnlockLvB = view.GetCommon<Text>("TextUnlockLvB");
            TextUnlockLvC = view.GetCommon<Text>("TextUnlockLvC");
            TextUnlockLvD = view.GetCommon<Text>("TextUnlockLvD");
            TextAttributeA = view.GetCommon<Text>("TextAttributeA");
            TextAttributeB = view.GetCommon<Text>("TextAttributeB");
            TextAttributeC = view.GetCommon<Text>("TextAttributeC");
            TextAttributeD = view.GetCommon<Text>("TextAttributeD");
            TextTitleChange = view.GetCommon<Text>("TextTitleChange");
            TextLevelDemand = view.GetCommon<Text>("TextLevelDemand");
            BtnFastLearn = view.GetCommon<TextButton>("BtnFastLearn");
            RightBg = view.GetCommon<GameObject>("RightBg");
            Scroller = view.GetCommon<UIScroller>("Scroller");
            BtnProm = view.GetCommon<Button>("BtnProm");
            TextBtnAtk = view.GetCommon<Text>("TextBtnAtk");
            TextBtnDef = view.GetCommon<Text>("TextBtnDef");
            TextPassive = view.GetCommon<Text>("TextPassive");
            TextBtnProm = view.GetCommon<Text>("TextBtnProm");
            PanelPromSpell = view.GetCommon<GameObject>("PanelPromSpell");
            TextNull = view.GetCommon<Text>("TextNull");

            AddAnimation = view.GetCommon<UIAnimationBaseCtrl>("AddAnimation");
 

            ListTextAttr = new List<Text>();
            ListTextAttr.Add(TextAttributeA);
            ListTextAttr.Add(TextAttributeB);
            ListTextAttr.Add(TextAttributeC);
            ListTextAttr.Add(TextAttributeD);

            ListTextAttrAdd = new List<Text>();
            ListTextAttrAdd.Add(TextAdditionA);
            ListTextAttrAdd.Add(TextAdditionB);
            ListTextAttrAdd.Add(TextAdditionC);
            ListTextAttrAdd.Add(TextAdditionD);

            ListTextBookSpell = new List<Text>();
            ListTextBookSpell.Add(TextUnlockLvA);
            ListTextBookSpell.Add(TextUnlockLvB);
            ListTextBookSpell.Add(TextUnlockLvC);
            ListTextBookSpell.Add(TextUnlockLvD);
            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtnAtk);
                TextBtnList.Add(TextBtnDef);
                TextBtnList.Add(TextPassive);
                TextBtnList.Add(TextBtnProm);
            }
            if (BtnSpellTab == null)
            {
                BtnSpellTab = new List<Button>();
                BtnSpellTab.Add(BtnAtk);
                BtnSpellTab.Add(BtnDef);
                BtnSpellTab.Add(BtnPassive);
                BtnSpellTab.Add(BtnProm);
            }
            
        }
        public void ResetSpellScroll()
        {
            Scroller.ScrollView.StopMovement();
            Vector3 tempPos = ItemRoot.localPosition;
            ItemRoot.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
        }
        public void SelectTabBtn(ChildTab? child)
        {
            if (TextBtnList == null)
                return;
            for (int i = 0, length = TextBtnList.Count; i < length; i++)
            {
                if ((int)child-2 == i)
                {
                    TextBtnList[i].color = new Color(255f / 255, 242f / 255, 0f);
                    TextBtnList[i].GetComponent<Outline>().enabled = true;
                    BtnSpellTab[i].enabled = false;
                }
                else
                {
                    TextBtnList[i].color = Color.black;
                    TextBtnList[i].GetComponent<Outline>().enabled = false;
                    BtnSpellTab[i].enabled = true;
                }
            }
        }
    }

    public class PromSpellViewObj
    {
        public Text TextName;
        public Text TextLevel;
        public Text TextUnlockLv0;
        public Text TextUnlockLv1;
        public Text TextUnlockLv2;
        public Text TextUnlockLv3;
        public Text TextUnlockLv4;
        public Text TextUnlockLv5;
        public Text TextUnlockLv6;
        public Text TextUnlockLv7;
        public Text TextUnlockLv8;
        public Text TextUnlockLv9;
        public Text TextCostPotential;
        public Text TextCostGold;
        public Text TextLevelDemand;
        public Button BtnLearn;
        public TextButton BtnFastLearn;

        public List<Text> TextUnlcok;
        public PromSpellViewObj(UIViewBase view)
        {
            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (TextUnlockLv0 == null) TextUnlockLv0 = view.GetCommon<Text>("TextUnlockLv0");
            if (TextUnlockLv1 == null) TextUnlockLv1 = view.GetCommon<Text>("TextUnlockLv1");
            if (TextUnlockLv2 == null) TextUnlockLv2 = view.GetCommon<Text>("TextUnlockLv2");
            if (TextUnlockLv3 == null) TextUnlockLv3 = view.GetCommon<Text>("TextUnlockLv3");
            if (TextUnlockLv4 == null) TextUnlockLv4 = view.GetCommon<Text>("TextUnlockLv4");
            if (TextUnlockLv5 == null) TextUnlockLv5 = view.GetCommon<Text>("TextUnlockLv5");
            if (TextUnlockLv6 == null) TextUnlockLv6 = view.GetCommon<Text>("TextUnlockLv6");
            if (TextUnlockLv7 == null) TextUnlockLv7 = view.GetCommon<Text>("TextUnlockLv7");
            if (TextUnlockLv8 == null) TextUnlockLv8 = view.GetCommon<Text>("TextUnlockLv8");
            if (TextUnlockLv9 == null) TextUnlockLv9 = view.GetCommon<Text>("TextUnlockLv9");
            if (TextCostPotential == null) TextCostPotential = view.GetCommon<Text>("TextCostPotential");
            if (TextCostGold == null) TextCostGold = view.GetCommon<Text>("TextCostGold");
            if (TextLevelDemand == null) TextLevelDemand = view.GetCommon<Text>("TextLevelDemand");
            if (BtnLearn == null) BtnLearn = view.GetCommon<Button>("BtnLearn");
            if (BtnFastLearn == null) BtnFastLearn = view.GetCommon<TextButton>("BtnFastLearn");
            if (TextLevel == null) TextLevel = view.GetCommon<Text>("TextLevel");
            if (TextUnlcok == null)
            {
                TextUnlcok = new List<Text>();
                TextUnlcok.Add(TextUnlockLv0);
                TextUnlcok.Add(TextUnlockLv1);
                TextUnlcok.Add(TextUnlockLv2);
                TextUnlcok.Add(TextUnlockLv3);
                TextUnlcok.Add(TextUnlockLv4);
                TextUnlcok.Add(TextUnlockLv5);
                TextUnlcok.Add(TextUnlockLv6);
                TextUnlcok.Add(TextUnlockLv7);
                TextUnlcok.Add(TextUnlockLv8);
                TextUnlcok.Add(TextUnlockLv9);            }        }
    }

    public class ItemObj : ItemIndexObj
    {
        public Text NameText;
        public Button BgBtn;
        public Image IconEquipMark;
        public bool isEquip;
        public GameObject gameobject;
        public int InventoryPos;
        public Image IconItem;
        public Sprite Bg_kuang_06;
        public Sprite Bg_kuang_04;
        public GameObject MaxLevel;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            NameText = view.GetCommon<Text>("NameText");
            BgBtn = view.GetCommon<Button>("BgBtn");
            IconEquipMark = view.GetCommon<Image>("IconEquipMark");
            if (IconItem == null) IconItem = view.GetCommon<Image>("IconItem");
            Bg_kuang_06 = view.GetCommon<Sprite>("Bg_kuang_06");
            Bg_kuang_04 = view.GetCommon<Sprite>("Bg_kuang_04");
            MaxLevel = view.GetCommon<GameObject>("MaxLevel");
            gameobject = view.gameObject;
        }
        public void SelectItem(bool select)
        {
            BgBtn.image.overrideSprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BgBtn.image.sprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BgBtn.enabled = !select;
        }
        public void InitItem(Spell spell)
        {
            GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
            SpritePrefab sprite = go.GetComponent<SpritePrefab>();

            NameText.text = spell.name;
            SelectItem(false);
            InventoryPos = PlayerPrefsBridge.Instance.GetSpellIndexOf(spell.idx);
            IconItem.sprite = sprite.GetSprite(spell.icon);
            IconEquipMark.sprite = sprite.GetSprite(spell.icon + "1");
            IconItem.gameObject.SetActive(!spell.curIsEquip);
            IconEquipMark.gameObject.SetActive(spell.curIsEquip);
            isEquip = spell.curIsEquip;
            gameobject.SetActive(true);
            MaxLevel.SetActive(spell.curLevel == spell.maxLevel && spell.NextState <= 0);
        }

    }
    public enum ChildTab
    {
        None=1,
        Atk ,  //
        Def,
        Mental,//心法
        Prom,
        Max
    }   

    private ChildTab? mCurTab = ChildTab.Atk;
    private Dictionary<int,ItemObj> mSpellItemList = new Dictionary<int,ItemObj>();
    private List<Spell> mCurTypeSpellList = new List<Spell>();
    private ViewObj mViewObj;
    private PromSpellViewObj mPromSpellViewObj;
    private int mInventoryPos; //当前选择的技能
    private List<int> mNewSpellList = new List<int>();
     
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        if (mPromSpellViewObj == null) mPromSpellViewObj = new PromSpellViewObj(mViewObj.PanelPromSpell.GetComponent<UIViewBase>());
        OpenWin();
        return;
        FreshBadge();
        Init();
      //  RegisterNetCodeHandler(NetCode_S.EquipSpell, S2C_EquipSpell);
        RegisterNetCodeHandler(NetCode_S.StudySpell, S2C_StudySpell);  
    }
    public void OpenChildTab(ChildTab? toTab)
    {
        if (mCurTab.HasValue && mCurTab == toTab) { return; }
        //GuideMgr.Instance.SetUIStatus(
        //    GuidePointUI.Inventory_SpellTab_SpellType.ToString() + (toTab.Value).ToInt(),
        //    BadgeStatus.Normal, FreshBadge);
        mCurTab = toTab;
        mViewObj.SelectTabBtn(toTab);
        FreshSpellItem();
        mViewObj.ResetSpellScroll();
    }


    void Init()
    {
        mViewObj.BtnAtk.SetOnAduioClick(delegate() { OpenChildTab(ChildTab.Atk); });
        mViewObj.BtnDef.SetOnAduioClick(delegate() { OpenChildTab(ChildTab.Def); });
        mViewObj.BtnPassive.SetOnAduioClick(delegate() { OpenChildTab(ChildTab.Mental); });
        mViewObj.BtnProm.SetOnAduioClick(delegate() { OpenChildTab(ChildTab.Prom); });
        mViewObj.BtnTurnLeft.SetOnAduioClick(delegate() { BtnEvt_TurnPage(false); });
        mViewObj.BtnTurnRight.SetOnAduioClick(delegate() { BtnEvt_TurnPage(true); });
        mViewObj.BtnChange.SetOnAduioClick(delegate() { UIRootMgr.Instance.OpenWindow<WindowBig_RoleInfo>(WinName.WindowBig_RoleInfo).OpenWindow(); });
        mCurTab = null;
        OpenChildTab(ChildTab.Atk);
    }

    public override void FreshBadge()
    {
        base.FreshBadge();
        for (int i = 0; i < mViewObj.BtnSpellTab.Count; i++)//显示技能类型切页的红点
        {
            BadgeTips.SetBadgeViewFalse(mViewObj.BtnSpellTab[i].transform);
            //if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Inventory_SpellTab_SpellType.ToString() +(i+2).ToString()) == BadgeStatus.ShowBadge)
            //{
            //    BadgeTips.SetBadgeView(mViewObj.BtnSpellTab[i].transform);
            //}
        }
        foreach (var temp in mSpellItemList) //得到要显示红点的所有技能
        {
            if (temp.Value != null)
            {
                BadgeTips.SetBadgeViewFalse(temp.Value.BgBtn.transform);
            }
        }
        //FreshBadgeInfo();
    }

    //public override void FreshBadgeInfo()
    //{
    //    base.FreshBadgeInfo();
    //    mNewSpellList = GuideMgr.Instance.GetOnceBadge(OnceBadgeType.NewSpell);
    //}


    public void FreshScrollItem(int index)
    {
        if (index > mCurTypeSpellList.Count || index<0 || mCurTypeSpellList[index] == null)
        {
            TDebug.LogError(string.Format("Spell不存在；index:{0}", index));
            return;
        }
        ItemObj item;
        if (mViewObj.Scroller._unUsedQueue.Count > 0)
        {
            item = (ItemObj)mViewObj.Scroller._unUsedQueue.Dequeue();
        }
        else
        {
            item = new ItemObj();
            item.Init(mViewObj.Scroller.GetNewObj(mViewObj.ItemRoot, mViewObj.Part_SelectableItemBtnt));
        }
        item.Scroller = mViewObj.Scroller;
        item.Index = index;
        mViewObj.Scroller._itemList.Add(item);

        //刷新显示
        Spell tempSpell = mCurTypeSpellList[index];
        item.InitItem(tempSpell);
        item.SelectItem(item.InventoryPos == mInventoryPos);

        int tempPos = item.InventoryPos;
        item.BgBtn.SetOnClick(delegate() { BtnEvt_SpellItemClick(tempPos); });
        if (!mSpellItemList.ContainsKey(index))
            mSpellItemList.Add(index, item);
        else
            mSpellItemList[index] = item;

        BadgeTips.CheckIdToBadge(item.BgBtn.transform, tempSpell.idx, mNewSpellList);
    }
    public void Reset()
    {
        mSpellItemList.Clear();
    }

    void FreshSpellItem()
    {
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
        SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();

        int wearedNum = 0;
        int maxWearNum = 2;//((Spell.PosType)mCurTab).SpellFrameNum();
        List<Spell> mSpellDataList = PlayerPrefsBridge.Instance.GetSpellAllListCopy();
        mCurTypeSpellList.Clear();
        mSpellItemList.Clear();    
        for (int i = 0; i < mSpellDataList.Count; i++)
        {
            if ((int)mSpellDataList[i].skillType == (int)mCurTab)
            {
                if (mSpellDataList[i].curIsEquip)
                {
                    wearedNum++;
                    mCurTypeSpellList.Insert(0,mSpellDataList[i]);
                }
                else
                    mCurTypeSpellList.Add(mSpellDataList[i]);
            }
        }
        mCurTypeSpellList.Sort((x, y) =>
        {
            int xWeight = x.curIsEquip ? 20000 : 10000;
            xWeight += (x.Level + x.curLevel);

            int yWeight = y.curIsEquip ? 20000 : 10000;
            yWeight += (y.Level + y.curLevel);
            return yWeight.CompareTo(xWeight);
        });
        Reset();
        mViewObj.Scroller.Init(this, mCurTypeSpellList.Count);
    
        if (mCurTab == ChildTab.Mental)
            mViewObj.TextWearNum.text = string.Format("已配置心法: {0}/{1}", wearedNum, maxWearNum);
        else
            mViewObj.TextWearNum.text = string.Format("已配置功法: {0}/{1}", wearedNum, maxWearNum);
        if (mCurTypeSpellList.Count <= 0)
        {
            mViewObj.RightBg.SetActive(false);
            mViewObj.PanelPromSpell.SetActive(false);
            mViewObj.Scroller.Scrollbar.gameObject.SetActive(false);
            mViewObj.TextNull.gameObject.SetActive(true);
            if (mCurTab == ChildTab.Atk) mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_atkspell");
            else if (mCurTab == ChildTab.Def) mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_defspell");
            else if (mCurTab == ChildTab.Mental) mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_mentalspell");
            else if (mCurTab == ChildTab.Prom) mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_promspell"); 
        }
        else
        {
            mViewObj.TextNull.gameObject.SetActive(false);
            mViewObj.Scroller.Scrollbar.gameObject.SetActive(true);
            BtnEvt_SpellItemClick(PlayerPrefsBridge.Instance.GetSpellIndexOf(mCurTypeSpellList[0].idx));
        }
    }

    public ItemObj GetScrollItemObj(int inventoryPos)
    {
        foreach (var temp in mSpellItemList)
        {
            if (temp.Value == null) continue;
            if (temp.Value.InventoryPos == inventoryPos)
                return temp.Value;
        }
        return null;
    }
    public void FreshSpellInfo(int spellPosInInventory)
    {
        mInventoryPos = spellPosInInventory;
        Spell curSpell = PlayerPrefsBridge.Instance.GetInventorySpellByPos(spellPosInInventory); //获取最新信息  
        
        //if (curSpell.Type == Spell.SpellTypeEnum.Prom)
        //{
        //    InitPromSpell(curSpell, spellPosInInventory);
        //    return;
        //}
        mViewObj.RightBg.SetActive(true);
        mViewObj.PanelPromSpell.SetActive(false);
        mViewObj.TextName.text = curSpell.name;
        mViewObj.TextLevel.text = string.Format("{0}级",  curSpell.curLevel);

        int maxState = curSpell.maxLevel;
        int minLevel = curSpell.Level - curSpell.idx % 10 * (1 + curSpell.maxLevel);
        int maxLevel = minLevel + (1 + curSpell.maxLevel) * (maxState - 1) + curSpell.maxLevel;
        HeroLevelUp lvUpMin = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(minLevel);
        HeroLevelUp lvUpMax = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(maxLevel);

        mViewObj.TextDesc.text = "\u3000\u3000" + curSpell.desc + string.Format("此功法可修行到{0}。", lvUpMax.name);
      //  mViewObj.TextEval.text = string.Format("此功法可修行到{0}", lvUpMax.Name);
        mViewObj.TextEffect.text = curSpell.desc;
        AttrType[] attrTypeList = curSpell.attrType;//基础属性类型
        for (int i = 0; i < mViewObj.ListTextAttr.Count; i++)
        {
            if (i < attrTypeList.Length)
                mViewObj.ListTextAttr[i].text = string.Format("{0}: {1}", TUtility.TryGetAttrTypeStr(attrTypeList[i]), curSpell.attrVal[i]);
            else
                mViewObj.ListTextAttr[i].text = "";
        }
        //SpellLevelUp spellLvUp = SpellLevelUp.SpellLevelUpFetcher.GetSpellLevelUpByCopy(Spell.GetSpellLevel(curSpell.idx,curSpell.curLevel));
        //mViewObj.TextCostGold.text = string.Format("灵石: {0}", spellLvUp.CostGold);
        //mViewObj.TextCostPotential.text = string.Format("潜能: {0}", spellLvUp.CostPotential);
        //if (curSpell.curLevel == curSpell.maxLevel)
        //{
        //    if (curSpell.NextState > 0)
        //    {
        //        HeroLevelUp lvUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(curSpell.NextState);
        //        mViewObj.TextLevelDemand.text =
        //            string.Format(
        //                curSpell.NextState > PlayerPrefsBridge.Instance.PlayerData.Level
        //                    ? "<color=#FF0000FF>升级条件: {0}</color>"
        //                    : "升级条件: {0}", lvUp.name);
        //        mViewObj.TextLevelDemand.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        mViewObj.TextLevel.text = "大圆满";
        //        mViewObj.TextLevelDemand.gameObject.SetActive(false);
        //    }
        //}
        //else
        //    mViewObj.TextLevelDemand.gameObject.SetActive(false);

        if (curSpell.curLevel == curSpell.maxLevel && curSpell.NextState <= 0)
        {
            mViewObj.BtnLearn.SetEnabled(false, TextButton.ColorText.Gray);
            mViewObj.BtnFastLearn.SetEnabled(false, TextButton.ColorText.Gray);
        }
        else
        {
            mViewObj.BtnLearn.SetEnabled(true, TextButton.ColorText.White);
            mViewObj.BtnFastLearn.SetEnabled(true, TextButton.ColorText.White);
        }
        mViewObj.BtnLearn.SetOnAduioClick(delegate() { BtnEvt_BtnLearnClick(spellPosInInventory); });
        mViewObj.BtnFastLearn.SetOnAduioClick(delegate() { BtnEvt_BtnLearnClick(spellPosInInventory, true); });
        //Dictionary<int, BookSpell> bookSpellList = Spell.GetBookSpell(curSpell);
        //int index = 0;
        //foreach (var spell in bookSpellList)
        //{
        //    if (index < mViewObj.ListTextBookSpell.Count)
        //    {
        //        int mainSpellId = spell.Key;
        //        BookSpell bookSpell = spell.Value;
        //        ///TODO : 天赋功法 类型和数值分别取取BaseAttType和BaseAttVal的第一个
        //        if (bookSpell.BaseAttType.Length > 0)
        //        {
        //            string bookSpellEffect = TUtility.TryGetAttrTypeStr(bookSpell.BaseAttType[0]) + TUtility.TryGetValOfAttr(bookSpell.BaseAttType[0], bookSpell.BaseAttVal[0]);
        //            if (mainSpellId % 10 + 1 == maxState)
        //                mViewObj.ListTextBookSpell[index].text = string.Format("圆满: {0}", bookSpellEffect);
        //            else
        //                mViewObj.ListTextBookSpell[index].text = string.Format("{0}级: {1}", Spell.GetSpellLevelOffset(mainSpellId, 9), bookSpellEffect);
        //            if (mainSpellId == curSpell.idx && curSpell.curLevel == curSpell.maxLevel)
        //                mViewObj.ListTextBookSpell[index].color = Color.black;
        //            else if (mainSpellId < curSpell.idx)
        //                mViewObj.ListTextBookSpell[index].color = Color.black;
        //            else
        //                mViewObj.ListTextBookSpell[index].color = Color.red;
        //        }
        //        else
        //        {
        //            mViewObj.ListTextBookSpell[index].text = "";
        //            mViewObj.ListTextBookSpell[index].text = "";
        //            TDebug.LogError(string.Format("{0}功法属性为空", bookSpell.idx));
        //        }
        //    }
        //    index++;
        //}
        //for (int i = bookSpellList.Count; i < mViewObj.ListTextBookSpell.Count; i++)
        //{
        //    mViewObj.ListTextBookSpell[i].text = "";
        //}
        #region 装配
        //if (curSpell.curIsEquip)
        //{
        //    mViewObj.TextTitleChange.text = "卸下";
        //    mViewObj.BtnChange.gameObject.SetActive(true);
        //    mViewObj.BtnChange.enabled = true;
        //    mViewObj.BtnChange.GetComponent<Image>().color = Color.white;
        //    mViewObj.TextTitleChange.color = new Color(255f / 255, 255f / 255, 255f / 255);
        //    Spell.PosType posType = Spell.PosType.None;
        //    for (int i = 0; i < PlayerPrefsBridge.Instance.PlayerData.SpellList.Length; i++)
        //    {
        //        if (spellPosInInventory == PlayerPrefsBridge.Instance.PlayerData.SpellList[i])
        //            posType = (Spell.PosType)i;
        //    }
        //    if (posType == Spell.PosType.None)
        //    {
        //        Debug.LogError(string.Format("该技能未尚未装备InventoryPos:{0}", spellPosInInventory));
        //        return;
        //    }
        //    mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_UnLoadSpell(posType); });
        //}
        //else
        //{
        //    mViewObj.TextTitleChange.text = "装备";
        //    int[] SpellList = PlayerPrefsBridge.Instance.PlayerData.SpellList;
        //    int canEquipSpellPos = (int)Spell.PosType.None;      
        //    for (int i = 0; i < SpellList.Length; i++)
        //    {
        //        if (SpellList[i] == (int)Spell.PosType.None && ((Spell.PosType)i).SpellFrameType() == curSpell.Type)
        //        {
        //            canEquipSpellPos = i;
        //            break;
        //        }
        //    }        
        //    if (canEquipSpellPos != (int)Spell.PosType.None)
        //    {
        //        mViewObj.BtnChange.enabled = true;
        //        mViewObj.BtnChange.GetComponent<Image>().color = new Color(255f/255, 255f/255, 255f/255, 255f/255);
        //        mViewObj.TextTitleChange.color = new Color(255f / 255, 255f / 255, 255f / 255);
        //        mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_EquipSpell(canEquipSpellPos); });
        //    }
        //    else
        //    {
        //        mViewObj.BtnChange.enabled = false;
        //        mViewObj.BtnChange.GetComponent<Image>().color = new Color(255f / 255, 255f / 255, 255f / 255, 165f / 255);
        //        mViewObj.TextTitleChange.color = new Color(156f/255,156f/255,156f/255);
        //    }
        //}
        #endregion
    }

   

    public void BtnEvt_TurnPage(bool right)
    {
        mViewObj.PageOne.SetActive(!right);
        mViewObj.BtnTurnRight.gameObject.SetActive(!right);
        mViewObj.PageTwo.SetActive(right);
        mViewObj.BtnTurnLeft.gameObject.SetActive(right);
    } 
    public void BtnEvt_SpellItemClick(int inventoryPos)
    {
        foreach (var temp in mSpellItemList)
        {
            if (null != temp.Value) temp.Value.SelectItem(temp.Value.InventoryPos == inventoryPos);
        }    
        mInventoryPos = inventoryPos;
        FreshSpellInfo(inventoryPos);
        BtnEvt_TurnPage(false);
       
    }
    bool CanLearnSpell(int spellPosInInventory,bool fastStudy)
    {
        ///功法修炼，客户端检测屏蔽
        //Spell curSpell = PlayerPrefsBridge.Instance.GetSpellByPosInInventory(spellPosInInventory);
        //SpellLevelUp spellLvUp = SpellLevelUp.SpellLevelUpFetcher.GetSpellLevelUpByCopy(Spell.GetSpellLevel(curSpell.idx, curSpell.curLevel));

        //int curState = curSpell.idx % 10+1;//功法id%10得到该功法当前重数
        //int maxState = Spell.GetSpellMaxState(curSpell.idx);
        //if (curState == maxState && curSpell.maxLevel ==curSpell.curLevel && UIRootMgr.Instance.MessageBox.ShowStatus("当前功法已大圆满，无法继续修炼"))
        //    return false;
        //int developTimes = (spellLvUp.Level / 10 + 1) * 10 - spellLvUp.Level;
        //int costGold = curSpell.Type == Spell.SpellTypeEnum.Prom ? spellLvUp.GetSpellDevelopCost(WealthType.Gold, developTimes) : spellLvUp.CostGold;
        //if (PlayerPrefsBridge.Instance.PlayerData.Gold < costGold && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_JIN_BI_BU_ZU))
        //    return false;
        //int costPotential = curSpell.Type == Spell.SpellTypeEnum.Prom ? spellLvUp.GetSpellDevelopCost(WealthType.Potentail, developTimes) : spellLvUp.CostPotential;
        //if (PlayerPrefsBridge.Instance.PlayerData.Potential < costPotential && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_QIAN_NENG_BU_ZU))
        //    return false;

        //if (curSpell.curLevel == curSpell.maxLevel && curSpell.NextState > PlayerPrefsBridge.Instance.PlayerData.Level && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_DENG_JI_BU_ZU))
        //    return false;

        return true;
    }
    // 快速修炼，最大修炼10次
    public void BtnEvt_BtnLearnClick(int spellPosInInventory,bool fastStudy=false)
    {
        if ( CanLearnSpell(spellPosInInventory,fastStudy))
        {
            Spell spell = PlayerPrefsBridge.Instance.GetInventorySpellByPos(spellPosInInventory);
            //if (spell.Type != Spell.SpellTypeEnum.Prom)
            //{
            //    tempSpell = spell;
            //}
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_StudySpell((sbyte)spellPosInInventory, fastStudy));
            UIRootMgr.Instance.IsLoading = true;
        }    
    }   
    public void BtnEvt_EquipSpell(int canWearPos)
    {
        UIRootMgr.Instance.IsLoading = true;      
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipSpell((sbyte)canWearPos, (sbyte)mInventoryPos));   
    }
    public void BtnEvt_UnLoadSpell(Spell.PosType posType)
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipSpell((sbyte)posType, (sbyte)Spell.PosType.None)); 
    }


    //public void S2C_EquipSpell(BinaryReader ios)
    //{
    //    UIRootMgr.Instance.IsLoading = false;
    //    NetPacket.S2C_EquipSpell msg = MessageBridge.Instance.S2C_EquipSpell(ios);
    //    int pos = PlayerPrefsBridge.Instance.PlayerData.SpellList[msg.EquipPos];    
    //    if (pos == (int)Spell.PosType.None)//卸下技能
    //    {
    //        Spell curSpell = PlayerPrefsBridge.Instance.GetSpellByPosInInventory(mInventoryPos);
    //        mSpellItemList[mCurSelectItemIndex].iconItem.gameObject.SetActive(true);
    //        mSpellItemList[mCurSelectItemIndex].iconEquipMark.gameObject.SetActive(false);
    //        mSpellItemList[mCurSelectItemIndex].curIsEquip = false;
    //        mViewObj.TextTitleChange.text = "装备";
    //        int[] SpellList = PlayerPrefsBridge.Instance.PlayerData.SpellList;
    //        int canEquipSpellPos = (int)Spell.PosType.None;
    //        for (int i = 0; i < SpellList.Length; i++)
    //        {
    //            if (SpellList[i] == (int)Spell.PosType.None && ((Spell.PosType)i).SpellFrameType() == curSpell.Type)
    //            {
    //                canEquipSpellPos = i;
    //                break;
    //            }
    //        }
    //        if (canEquipSpellPos != (int)Spell.PosType.None)
    //        {
    //            mViewObj.BtnChange.enabled = true;
    //            mViewObj.BtnChange.GetComponent<Image>().color = Color.white;
    //            mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_EquipSpell(canEquipSpellPos); });
    //        }
    //        else
    //        {
    //            mViewObj.BtnChange.enabled = false;
    //            mViewObj.BtnChange.GetComponent<Image>().color = Color.gray;
    //        }
    //    }
    //    else
    //    {         
    //        mSpellItemList[mCurSelectItemIndex].iconItem.gameObject.SetActive(false);
    //        mSpellItemList[mCurSelectItemIndex].iconEquipMark.gameObject.SetActive(true);
    //        mSpellItemList[mCurSelectItemIndex].curIsEquip = true;
    //        FreshSpellInfo(pos);
    //    }
    //    int wearedNum = 0;
    //    for (int i = 0; i < mSpellItemList.Count; i++)
    //    {
    //        if (mSpellItemList[i].curIsEquip && mSpellItemList[i].gameobject.activeInHierarchy)
    //            wearedNum++;
    //    }
    //    int maxWearNum = ((Spell.SpellTypeEnum)mCurTab).SpellFrameNum();
    //    if (mCurTab == ChildTab.Mental)
    //        mViewObj.TextWearNum.text = string.Format("已配置心法：{0}/{1}", wearedNum, maxWearNum);
    //    else
    //        mViewObj.TextWearNum.text = string.Format("已配置功法：{0}/{1}", wearedNum, maxWearNum);
    //}
    public void S2C_StudySpell(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_StudySpell msg = MessageBridge.Instance.S2C_StudySpell(ios);
        int pos = PlayerPrefsBridge.Instance.GetSpellIndexOf(msg.NewSpellIdx);
      
        Spell curSpell = PlayerPrefsBridge.Instance.GetInventorySpellByPos(pos); //获取最新信息   
        //if (curSpell.Type != Spell.SpellTypeEnum.Prom)
        //{ 
        //    newSpell = curSpell;
        //    StartCoroutine("AdditionAni");
        //}
        //FreshSpellInfo(pos);
        //if (curSpell.curLevel == curSpell.maxLevel && curSpell.NextState <= 0)
        //{
        //    foreach (var item in mSpellItemList)
        //    {
        //        if (item.Value.InventoryPos == pos)
        //        {
        //            item.Value.MaxLevel.SetActive(true);
        //        }
        //    }
        //}
        //for (int i = 0; i < mCurTypeSpellList.Count; i++)
        //{
        //    if (Spell.GetSpellBaseIdx(mCurTypeSpellList[i].idx) == Spell.GetSpellBaseIdx(msg.NewSpellIdx))
        //    {
        //        mCurTypeSpellList[i] = curSpell;
        //    }
        //}  
        //AudioMgr.Instance.PlayeAudio(AudioName.Audio_LevelUp);
        //UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("{0}等级提升至{1}级", curSpell.name, Spell.GetSpellLevelOffset(curSpell.idx,curSpell.curLevel)));
        UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗灵石 {0}", msg.CostGold));
        UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗潜能 {0}", msg.CostPotential));
        PlayerPrefsBridge.Instance.FreshPromAchieve();
    }

    private Spell tempSpell;
    private Spell newSpell;
    IEnumerator AdditionAni()
    {
        AttrType[] attrTypeList = newSpell.attrType;//基础属性类型
        for (int i = 0; i < mViewObj.ListTextAttr.Count; i++)
        {
            if (i < attrTypeList.Length)
            {
                RectTransform textPos = mViewObj.ListTextAttr[i].rectTransform;
                Vector3 textPost = mViewObj.ListTextAttrAdd[i].rectTransform.anchoredPosition;
                mViewObj.ListTextAttrAdd[i].rectTransform.anchoredPosition = new Vector3(textPos.anchoredPosition.x + textPos.sizeDelta.x + 6, textPost.y, textPost.z);
                mViewObj.ListTextAttrAdd[i].text = string.Format("+{0}", newSpell.attrVal[i]);          
            }
        }
        mViewObj.AddAnimation.DoSelfAnimation();
        yield return new WaitForSeconds(2f);
    }
}
