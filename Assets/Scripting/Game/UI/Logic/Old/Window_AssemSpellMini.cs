using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_AssemSpellMini : WindowBase,IScrollWindow
{
    public class ViewObj
    {   
        public GameObject Part_SelectableItemBtnt;
        public Transform ItemRoot;
        public Button BtnLearn;
        public Button BtnChange;
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
        public Button BtnMask;
        public ItemObj CurEquipSpell;
        public List<Text> TextBtnList;
        public Button BtnLearnFast;
        public UIScroller Scroller;
        public ViewObj(UIViewBase view)
        {
            Part_SelectableItemBtnt = view.GetCommon<GameObject>("Part_SelectableItemBtnt");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            BtnLearn = view.GetCommon<Button>("BtnLearn");
            BtnChange = view.GetCommon<Button>("BtnChange");
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
            RightBg = view.GetCommon<GameObject>("RightBg");
            BtnMask = view.GetCommon<Button>("BtnMask");
            BtnLearnFast = view.GetCommon<Button>("BtnLearnFast");
            Scroller = view.GetCommon<UIScroller>("Scroller");
            CurEquipSpell = new ItemObj();
            CurEquipSpell.Init(view.GetCommon<UIViewBase>("CurEquipSpell"));
            ListTextAttr = new List<Text>();
            ListTextAttr.Add(TextAttributeA);
            ListTextAttr.Add(TextAttributeB);
            ListTextAttr.Add(TextAttributeC);
            ListTextAttr.Add(TextAttributeD);
            ListTextBookSpell = new List<Text>();
            ListTextBookSpell.Add(TextUnlockLvA);
            ListTextBookSpell.Add(TextUnlockLvB);
            ListTextBookSpell.Add(TextUnlockLvC);
            ListTextBookSpell.Add(TextUnlockLvD);
        }
        public void ResetSpellScroll()
        {
            Scroller.ScrollView.StopMovement();
            Vector3 tempPos = ItemRoot.localPosition;
            ItemRoot.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
        }
      
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
            gameobject = view.gameObject;
        }
        public void SelectItem(bool select)
        {
            BgBtn.image.overrideSprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BgBtn.image.sprite = select ? Bg_kuang_06 : Bg_kuang_04;
        }

        public void InitItem(Spell spell)
        {
            GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
            SpritePrefab sprite = go.GetComponent<SpritePrefab>();

            //this.NameText.text = spell.ColorName;
            //this.SelectItem(false);
            //this.InventoryPos = PlayerPrefsBridge.Instance.GetSpellIndexOf(spell.idx);
            //this.IconItem.sprite = sprite.GetSprite(spell.icon);
            //this.IconEquipMark.sprite = sprite.GetSprite(spell.icon + "1");
            //this.IconItem.gameObject.SetActive(!spell.isEquip);
            //this.IconEquipMark.gameObject.SetActive(spell.isEquip);
            //this.isEquip = spell.isEquip;
            this.gameobject.SetActive(true);
        }


    }
    public enum ChildTab
    {
        None=1,
        Atk ,  //
        Def,
        Mental,//心法
        Max
    }   


    private ChildTab? mCurTab = ChildTab.Atk;
    private Dictionary<int,ItemObj> mSpellItemList = new Dictionary<int,ItemObj>();
    private List<Spell> mCurTypeSpellList = new List<Spell>();
    private ViewObj mViewObj;
    private int mInventoryPos; //当前选择的技能
    private Spell.PosType mSpellPos = Spell.PosType.None;// 选择打开的技能装备位
    private int mCurOpenSpellID;  //当前打开的技能位技能ID；


    public void OpenWindowMini(Spell.PosType posType, int spellId = 0)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        InitMini(posType,spellId);
        
    }
    void InitMini(Spell.PosType posType, int spellId = 0)
    {
        //mSpellPos = posType;
        //mCurOpenSpellID = spellId;
        //mCurTab = (ChildTab)((int)TUtility.GetSpellType(posType));
        //FreshSpellItem(spellId);
        //mViewObj.ResetSpellScroll();

        //mViewObj.BtnTurnLeft.SetOnClick(delegate() { BtnEvt_TurnPage(false); });
        //mViewObj.BtnTurnRight.SetOnClick(delegate() { BtnEvt_TurnPage(true); });
        //mViewObj.BtnMask.SetOnClick(delegate() { CloseWindow(CloseActionType.OpenHide); });
        //RegisterNetCodeHandler(NetCode_S.EquipSpell, S2C_EquipSpell);
        //RegisterNetCodeHandler(NetCode_S.StudySpell, S2C_StudySpell);
        TDebug.Log("RegisterNetCodeHandler==NetCode_S.StudySpell");
    }
    public void FreshScrollItem(int index)
    {
        if (index > mCurTypeSpellList.Count || mCurTypeSpellList[index] == null)
        {
            TDebug.LogError(string.Format("{Spell不存在；index:{0}}", index));
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
        item.InitItem(mCurTypeSpellList[index]);
        item.SelectItem(item.InventoryPos == mInventoryPos);

        int tempPos = item.InventoryPos;
        int tempIndex = item.Index;
        item.BgBtn.SetOnClick(delegate() { BtnEvt_SpellItemClick(tempPos, tempIndex); });
        if (!mSpellItemList.ContainsKey(index))
            mSpellItemList.Add(index, item);
        else
            mSpellItemList[index] = item;
    }
    public void Reset()
    {
        mSpellItemList.Clear();
    }

    void FreshSpellItem(int spellIdx = 0)
    {
        List<Spell> spellDataList = PlayerPrefsBridge.Instance.GetSpellAllListCopy();
        mCurTypeSpellList.Clear();
        mSpellItemList.Clear();    
        Spell curEquipSpell =null;    
        for (int i = 0; i < spellDataList.Count; i++)
        {
            if ((int)spellDataList[i].skillType == (int)mCurTab)
            {
                if (spellDataList[i].curIsEquip)
                    mCurTypeSpellList.Insert(0, spellDataList[i]);
                else
                    mCurTypeSpellList.Add(spellDataList[i]);
            }
            if(spellIdx==spellDataList[i].idx)
                curEquipSpell = spellDataList[i];
        }
        if (curEquipSpell != null)
        {
            mViewObj.CurEquipSpell.InitItem(curEquipSpell);
            int inventoryPos = PlayerPrefsBridge.Instance.GetSpellIndexOf(curEquipSpell.idx);
            mViewObj.CurEquipSpell.BgBtn.SetOnClick(delegate() { BtnEvt_SpellItemClick(inventoryPos, -1); });
            mCurTypeSpellList.Remove(curEquipSpell);
        }
        else
            mViewObj.CurEquipSpell.gameobject.SetActive(false);

        Reset();
        mViewObj.Scroller.Init(this, mCurTypeSpellList.Count);   
    
        int firstUnEquipedSpellIndex = -1;
        for (int i = 0; i < mCurTypeSpellList.Count; i++)
        {
            if (mCurTypeSpellList[i].curIsEquip == false && firstUnEquipedSpellIndex == -1) firstUnEquipedSpellIndex = i;
        }
        if (mCurTypeSpellList.Count > 0)
        {
            if (firstUnEquipedSpellIndex != -1)
                BtnEvt_SpellItemClick(PlayerPrefsBridge.Instance.GetSpellIndexOf(mCurTypeSpellList[firstUnEquipedSpellIndex].idx), firstUnEquipedSpellIndex);
            else if(curEquipSpell!=null)
                BtnEvt_SpellItemClick(PlayerPrefsBridge.Instance.GetSpellIndexOf(curEquipSpell.idx), -1);               
            else
                BtnEvt_SpellItemClick(PlayerPrefsBridge.Instance.GetSpellIndexOf(mCurTypeSpellList[0].idx), 0);               
            mViewObj.RightBg.SetActive(true);
        }
        else if (curEquipSpell != null)
            BtnEvt_SpellItemClick(PlayerPrefsBridge.Instance.GetSpellIndexOf(curEquipSpell.idx), -1);    
        else
            mViewObj.RightBg.SetActive(false);
    }
    public void FreshSpellInfo(int spellPosInInventory)
    {
        mInventoryPos = spellPosInInventory;
        Spell curSpell = PlayerPrefsBridge.Instance.GetInventorySpellByPos(spellPosInInventory); //获取最新信息     
        //mViewObj.TextName.text = curSpell.ColorName;
        //mViewObj.TextLevel.text = string.Format("{0}级", Skill.GetSpellLevelOffset(curSpell.idx, curSpell.curLevel));

        //int maxState = Skill.GetSpellMaxState(curSpell.idx);
        //int minLevel = curSpell.level - curSpell.idx % 10 * (1 + curSpell.maxLevel);
        int maxLevel = curSpell.maxLevel;
        //HeroLevelUp lvUpMin = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(minLevel);
        //HeroLevelUp lvUpMax = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(maxLevel);

        //mViewObj.TextDesc.text = "\u3000\u3000" + curSpell.SpellDesc1 + string.Format("此功法可修行到{0}。", lvUpMax.name);
        mViewObj.TextEffect.text = curSpell.desc;
        //基础属性类型
        AttrType[] attrTypeList = curSpell.attrType;
        for (int i = 0; i < mViewObj.ListTextAttr.Count; i++)
        {
            if (i < attrTypeList.Length)
                mViewObj.ListTextAttr[i].text = string.Format("{0}: {1}", TUtility.TryGetAttrTypeStr(attrTypeList[i]), 1);
            else
                mViewObj.ListTextAttr[i].text = "";
        }
        //SpellLevelUp spellLvUp = SpellLevelUp.SpellLevelUpFetcher.GetSpellLevelUpByCopy(OldSpell.GetSpellLevel(curSpell.idx,curSpell.curLevel));
        //mViewObj.TextCostGold.text = string.Format("灵石：{0}", spellLvUp.CostGold);
        //mViewObj.TextCostPotential.text = string.Format("潜能：{0}", spellLvUp.CostPotential);
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
        //       // mViewObj.TextLevelDemand.text = string.Format("已达到最大等级");
        //        mViewObj.TextLevelDemand.gameObject.SetActive(false);
        //    }
        //}
        //else
        //    mViewObj.TextLevelDemand.gameObject.SetActive(false);
        //mViewObj.BtnLearn.SetOnClick(delegate() { BtnEvt_BtnLearnClick(spellPosInInventory); });
        //mViewObj.BtnLearnFast.SetOnClick(delegate() { BtnEvt_BtnLearnClick(spellPosInInventory,true); });
        /////天赋功法
        //Dictionary<int, BookSpell> bookSpellList = OldSpell.GetBookSpell(curSpell);
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
        //                mViewObj.ListTextBookSpell[index].text = string.Format("{0}级: {1}", OldSpell.GetSpellLevelOffset(mainSpellId, 9), bookSpellEffect);
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

        //if (curSpell.isEquip)
        //{
        //    if (OldSpell.GetSpellBaseIdx(curSpell.idx) != OldSpell.GetSpellBaseIdx(mCurOpenSpellID))
        //    {
        //        mViewObj.BtnChange.gameObject.SetActive(false);
        //        return;
        //    }
        //    mViewObj.BtnChange.gameObject.SetActive(true); 
        //    mViewObj.TextTitleChange.text = "卸下";
        //    mViewObj.BtnChange.gameObject.SetActive(true);
        //    mViewObj.BtnChange.enabled = true;
        //    mViewObj.BtnChange.GetComponent<Image>().color = Color.white;
        //    mViewObj.TextTitleChange.color = new Color(255f / 255, 255f / 255, 255f / 255);
        //    OldSpell.PosType posType = OldSpell.PosType.None;
        //    for (int i = 0; i < PlayerPrefsBridge.Instance.PlayerData.SpellList.Length; i++)
        //    {
        //        if (spellPosInInventory == PlayerPrefsBridge.Instance.PlayerData.SpellList[i])
        //            posType = (OldSpell.PosType)i;
        //    }
        //    if (posType == OldSpell.PosType.None)
        //    {
        //        TDebug.LogError(string.Format("该技能未尚未装备InventoryPos:{0}", spellPosInInventory));
        //        return;
        //    }
        //    mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_UnLoadSpell(posType); });
        //}
        //else
        //{
        //    mViewObj.TextTitleChange.text = "装备";
        //    mViewObj.BtnChange.gameObject.SetActive(true);
        //    int[] SpellList = PlayerPrefsBridge.Instance.PlayerData.SpellList;
        //    int canEquipSpellPos = (int)OldSpell.PosType.None;
        //    if (mSpellPos != OldSpell.PosType.None)
        //    {
        //        canEquipSpellPos = (int)mSpellPos;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < SpellList.Length; i++)
        //        {
        //            if (SpellList[i] == (int)OldSpell.PosType.None && ((OldSpell.PosType)i).SpellFrameType() == curSpell.skillType)
        //            {
        //                canEquipSpellPos = i;
        //                break;
        //            }
        //        }
        //    }        
        //    if (canEquipSpellPos != (int)OldSpell.PosType.None)
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
    }

   

    public void BtnEvt_TurnPage(bool right)
    {
        mViewObj.PageOne.SetActive(!right);
        mViewObj.BtnTurnRight.gameObject.SetActive(!right);
        mViewObj.PageTwo.SetActive(right);
        mViewObj.BtnTurnLeft.gameObject.SetActive(right);
    } 
    //点击
    private int mCurSelectItemIndex = 0;
    public void BtnEvt_SpellItemClick(int inventoryPos,int index)
    {
        mViewObj.CurEquipSpell.SelectItem(index == -1);
        foreach (var temp in mSpellItemList)
        {
            if (null != temp.Value) temp.Value.SelectItem(temp.Value.InventoryPos == inventoryPos);
        }    
        mCurSelectItemIndex = index;
        FreshSpellInfo(inventoryPos);
        BtnEvt_TurnPage(false);
       
    }

    bool CanLearnSpell(int spellPosInInventory)
    {
        ///功法修炼，客户端检测屏蔽
        Spell curSpell = PlayerPrefsBridge.Instance.GetInventorySpellByPos(spellPosInInventory);
        //SpellLevelUp spellLvUp = SpellLevelUp.SpellLevelUpFetcher.GetSpellLevelUpByCopy(Skill.GetSpellLevel(curSpell.idx, curSpell.curLevel));
        //int curState = curSpell.idx % 10+1;//功法id%10+1得到该功法当前重数
        //int maxState = Skill.GetSpellMaxState(curSpell.idx);
        //if (curState == maxState && UIRootMgr.Instance.MessageBox.ShowStatus("当前功法已大圆满，无法继续修炼"))
        //   return false;

        //if (PlayerPrefsBridge.Instance.PlayerData.Gold < spellLvUp.CostGold&& UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_JIN_BI_BU_ZU))           
        //    return false;

        //if (PlayerPrefsBridge.Instance.PlayerData.Potential < spellLvUp.CostPotential && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_QIAN_NENG_BU_ZU))          
        //    return false;

        //if (curSpell.NextState > PlayerPrefsBridge.Instance.PlayerData.Level && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_DENG_JI_BU_ZU))
        //    return false;

        return true;
    }


    public void BtnEvt_BtnLearnClick(int spellPosInInventory,bool fastStudy=false)
    {
        if (CanLearnSpell(spellPosInInventory))
        {
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


    public void S2C_EquipSpell(BinaryReader ios)
    {
        //UIRootMgr.Instance.IsLoading = false;
        //NetPacket.S2C_EquipSpell msg = MessageBridge.Instance.S2C_EquipSpell(ios);
        //int pos = PlayerPrefsBridge.Instance.PlayerData.SpellList[msg.EquipPos];
        //if (mCurSelectItemIndex == -1)
        //{
        //    FreshSpellItem();
        //}
        //else
        //{        
        //    if (pos == (int)Spell.PosType.None)//卸下技能
        //    {
        //        Spell curSpell = PlayerPrefsBridge.Instance.GetInventorySpellByPos(mInventoryPos);
        //        ItemObj item = mSpellItemList[mCurSelectItemIndex];
        //        item.IconItem.gameObject.SetActive(true);
        //        item.IconEquipMark.gameObject.SetActive(false);
        //        item.isEquip = false;
        //        mViewObj.TextTitleChange.text = "装备";
        //        int[] SpellList = PlayerPrefsBridge.Instance.PlayerData.SpellList;
        //        int canEquipSpellPos = (int)Spell.PosType.None;
        //        for (int i = 0; i < SpellList.Length; i++)
        //        {
        //            //if (SpellList[i] == (int)Skill.PosType.None && ((Skill.PosType)i).SpellFrameType() == curSpell.skillType)
        //            //{
        //            //    canEquipSpellPos = i;
        //            //    break;
        //            //}
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
        //        CloseWindow(CloseActionType.OpenHide);
        //    }
        //}     
    }
    public void S2C_StudySpell(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_StudySpell msg = MessageBridge.Instance.S2C_StudySpell(ios);
        int pos = PlayerPrefsBridge.Instance.GetSpellIndexOf(msg.NewSpellIdx);
        FreshSpellInfo(pos);

        Spell curSpell = PlayerPrefsBridge.Instance.GetInventorySpellByPos(pos); //获取最新信息     
        //UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("{0}等级提升至{1}级", curSpell.name, Skill.GetSpellLevelOffset(curSpell.idx, curSpell.curLevel)));
        UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗灵石 {0}", msg.CostGold));
        UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗潜能 {0}", msg.CostPotential));
        PlayerPrefsBridge.Instance.FreshPromAchieve();
    }

}
