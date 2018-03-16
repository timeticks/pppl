using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_AssemPet : WindowBase,IScrollWindow {

 
    public class ViewObj
    {
        public GameObject Part_SelectableItemBtnt;
        public Button BtnPet0;
        public Button BtnPet1;
        public Button BtnPet2;
        public Text TextWearNum;
        public Transform ItemRoot;
        public GameObject RightBg;
        public Text TextName;
        public Text TextLevel;
        public Text TextDesc;
        public Text TextAttributeA;
        public Text TextAttributeB;
        public Text TextAttributeC;
        public Text TextAttributeD;
        public Text TextSkill;
        public Text TextCostMa0;
        public Text TextCostMa1;
        public Text TextStateLimit;
        public Button BtnLearn;
        public Button BtnEquip;
        public Text TextBtnLvUp;
        public Text TextBtnEquip;
        public Text TextBtnPet0;
        public Text TextBtnPet1;
        public Text TextBtnPet2;
        public UIScroller Scroller;
        public Text TextTitleCost;
        public Text TextNull;
        public Text TextEquipLevel;

        public UIAnimationBaseCtrl AddAnimation;
        public Text TextAdditionA;
        public Text TextAdditionB;
        public Text TextAdditionC;
        public Text TextAdditionD;

        public List<Text> ListTextAttrAdd;
        public List<Text> TextBtnList;
        public List<Text> TextAttrList;
        public List<Button> BtnPetTab;
        public ViewObj(UIViewBase view)
        {
            Part_SelectableItemBtnt = view.GetCommon<GameObject>("Part_SelectableItemBtnt");
            BtnPet0 = view.GetCommon<Button>("BtnPet0");
            BtnPet1 = view.GetCommon<Button>("BtnPet1");
            BtnPet2 = view.GetCommon<Button>("BtnPet2");
            TextWearNum = view.GetCommon<Text>("TextWearNum");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            RightBg = view.GetCommon<GameObject>("RightBg");
            TextName = view.GetCommon<Text>("TextName");
            TextLevel = view.GetCommon<Text>("TextLevel");
            TextDesc = view.GetCommon<Text>("TextDesc");
            TextAttributeA = view.GetCommon<Text>("TextAttributeA");
            TextAttributeB = view.GetCommon<Text>("TextAttributeB");
            TextAttributeC = view.GetCommon<Text>("TextAttributeC");
            TextAttributeD = view.GetCommon<Text>("TextAttributeD");
            TextSkill = view.GetCommon<Text>("TextSkill");
            TextCostMa0 = view.GetCommon<Text>("TextCostMa0");
            TextCostMa1 = view.GetCommon<Text>("TextCostMa1");
            TextStateLimit = view.GetCommon<Text>("TextStateLimit");
            BtnLearn = view.GetCommon<Button>("BtnLearn");
            BtnEquip = view.GetCommon<Button>("BtnEquip");
            TextBtnLvUp = view.GetCommon<Text>("TextBtnLvUp");
            TextBtnEquip = view.GetCommon<Text>("TextBtnEquip");
            Scroller = view.GetCommon<UIScroller>("Scroller");
            TextBtnPet0 = view.GetCommon<Text>("TextBtnPet0");
            TextBtnPet1 = view.GetCommon<Text>("TextBtnPet1");
            TextBtnPet2 = view.GetCommon<Text>("TextBtnPet2");
            TextTitleCost = view.GetCommon<Text>("TextTitleCost");
            TextNull = view.GetCommon<Text>("TextNull");
            TextEquipLevel = view.GetCommon<Text>("TextEquipLevel");
            TextAdditionA = view.GetCommon<Text>("TextAdditionA");
            TextAdditionB = view.GetCommon<Text>("TextAdditionB");
            TextAdditionC = view.GetCommon<Text>("TextAdditionC");
            TextAdditionD = view.GetCommon<Text>("TextAdditionD");
            AddAnimation = view.GetCommon<UIAnimationBaseCtrl>("AddAnimation");

            TextAttrList = new List<Text>();
            TextAttrList.Add(TextAttributeA);
            TextAttrList.Add(TextAttributeB);
            TextAttrList.Add(TextAttributeC);
            TextAttrList.Add(TextAttributeD);

            ListTextAttrAdd = new List<Text>();
            ListTextAttrAdd.Add(TextAdditionA);
            ListTextAttrAdd.Add(TextAdditionB);
            ListTextAttrAdd.Add(TextAdditionC);
            ListTextAttrAdd.Add(TextAdditionD);

            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtnPet0);
                TextBtnList.Add(TextBtnPet1);
                TextBtnList.Add(TextBtnPet2);
            }
            if (BtnPetTab == null)
            {
                BtnPetTab = new List<Button>();
                BtnPetTab.Add(BtnPet0);
                BtnPetTab.Add(BtnPet1);
                BtnPetTab.Add(BtnPet2);
            }
        }
        public void ResetPetScroll()
        {
            Scroller.ScrollView.StopMovement();
            Vector3 tempPos = ItemRoot.transform.localPosition;
            ItemRoot.transform.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
        }
        public void SelectTabBtn(ChildTab? child)
        {
            if (TextBtnList == null)
                return;
            for (int i = 0, length = TextBtnList.Count; i < length; i++)
            {
                if ((int)child == i)
                {
                    TextBtnList[i].color = new Color(255f / 255, 242f / 255, 0f);
                    TextBtnList[i].GetComponent<Outline>().enabled = true;
                    BtnPetTab[i].enabled = false;
                }
                else
                {
                    TextBtnList[i].color = Color.black;
                    TextBtnList[i].GetComponent<Outline>().enabled = false;
                    BtnPetTab[i].enabled = true;
                }
            }
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
        public Pet.PetTypeEnum Type;
        public override void Init (UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            NameText = view.GetCommon<Text>("NameText");
            BgBtn = view.GetCommon<Button>("BgBtn");
            IconEquipMark = view.GetCommon<Image>("IconEquipMark");
            gameobject = view.gameObject;
            if (IconItem == null) IconItem = view.GetCommon<Image>("IconItem");
            Bg_kuang_06 = view.GetCommon<Sprite>("Bg_kuang_06");
            Bg_kuang_04 = view.GetCommon<Sprite>("Bg_kuang_04");
        }
        public void SelectItem(bool select)
        {
            BgBtn.image.overrideSprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BgBtn.image.sprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BgBtn.enabled = !select;
        }
        public void InitItem(Pet pet)
        {
            GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
            SpritePrefab sprite = go.GetComponent<SpritePrefab>();

            this.gameobject.SetActive(true);
            this.NameText.text = pet.ColorName;
            this.SelectItem(false);
            this.isEquip = pet.IsEquiped;

            this.IconItem.sprite = sprite.GetSprite(TUtility.TryGetPetIcon(pet.Type));
            this.IconEquipMark.sprite = sprite.GetSprite(TUtility.TryGetPetIcon(pet.Type) + "1");
            this.IconItem.gameObject.SetActive(!pet.IsEquiped);
            this.IconEquipMark.gameObject.SetActive(pet.IsEquiped);
            this.Type = pet.Type;
            this.InventoryPos = PlayerPrefsBridge.Instance.GetPetIndexOf(pet.idx);
            
        }
    }
    public enum ChildTab
    {
        [EnumDesc("灵兽")]
        Animal=0,  //灵兽
        [EnumDesc("傀儡")]
        Puppet,  //傀儡
        [EnumDesc("阴魂")]
        Ghost,      //阴魂
        Max
    }


    public ChildTab? CurTab;
    private ViewObj mViewObj;
    private Dictionary<int,ItemObj> mPetItemList = new Dictionary<int,ItemObj>();
    private List<Pet> mCurTypePet = new List<Pet>();
    private int mInventoryPos;
    private List<int> mNewPetList = new List<int>();

    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        FreshBadge();
        Init();
        RegisterNetCodeHandler(NetCode_S.PetLevelUp, S2C_PetLvUp);
    }

    public override void FreshBadge()
    {
        base.FreshBadge();

        for (int i = 0; i < mViewObj.BtnPetTab.Count; i++)//显示宠物类型切页的红点
        {
            //BadgeTips.SetBadgeViewFalse(mViewObj.BtnPetTab[i].transform);
            //if (GuideMgr.Instance.GetUIStatus(GuidePointUI.Inventory_PetTab_PetType.ToString()+i) == BadgeStatus.ShowBadge)
            //{
            //    BadgeTips.SetBadgeView(mViewObj.BtnPetTab[i].transform);
            //}
        }
        foreach (var temp in mPetItemList) //得到要显示红点的所有宠物
        {
            if (temp.Value != null)
            {
                BadgeTips.SetBadgeViewFalse(temp.Value.BgBtn.transform);
            }
        }
    }

    private void Init()
    {
        CurTab = null;
        mViewObj.BtnPet0.SetOnAduioClick(delegate() { OpenChildTab(ChildTab.Animal); });
        mViewObj.BtnPet1.SetOnAduioClick(delegate() { OpenChildTab(ChildTab.Puppet); });
        mViewObj.BtnPet2.SetOnAduioClick(delegate() { OpenChildTab(ChildTab.Ghost); });      
        OpenChildTab(ChildTab.Animal);
    }

    public void OpenChildTab(ChildTab toTab)
    {
        if (CurTab != null && CurTab == toTab) { return; }
        //GuideMgr.Instance.SetUIStatus(GuidePointUI.Inventory_PetTab_PetType.ToString() + toTab.ToInt() , BadgeStatus.Normal , FreshBadge);
        CurTab = toTab;
        mViewObj.SelectTabBtn(toTab);
        mViewObj.ResetPetScroll();
        mViewObj.TextBtnEquip.text = string.Format("配置{0}",toTab.GetDesc());
        FreshPetItem();
    }

    public void BtnEvt_PetItemClick(int inventoryPos)
    {
        foreach (var item in mPetItemList)
        {
            if(item.Value!=null)item.Value.SelectItem(item.Value.InventoryPos == inventoryPos);
        }
        mInventoryPos = inventoryPos;
        FreshPetInfo(inventoryPos);
    }
    public void FreshScrollItem(int index)
    {
        if (index > mCurTypePet.Count || mCurTypePet[index] == null)
        {
            TDebug.LogError(string.Format("{ Pet不存在；index:{0}}", index));
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
        Pet pet = mCurTypePet[index];
        item.InitItem(pet);
        item.SelectItem(item.InventoryPos == mInventoryPos);

        int tempPos = item.InventoryPos;
        item.BgBtn.SetOnClick(delegate() { BtnEvt_PetItemClick(tempPos); });
        if (!mPetItemList.ContainsKey(index))
            mPetItemList.Add(index, item);
        else
            mPetItemList[index] = item;

        BadgeTips.CheckIdToBadge(item.BgBtn.transform, pet.idx, mNewPetList);
    }
    public void Reset()
    {
        mPetItemList.Clear();
    }
    public void FreshPetItem(bool reset =true)
    {
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
        SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();

        List<Pet> petDataAll = PlayerPrefsBridge.Instance.GetPetAllListCopy();
        mCurTypePet.Clear();
        mPetItemList.Clear();
        for (int i = 0; i < petDataAll.Count; i++)
        {
            if ((int)petDataAll[i].Type == (int)CurTab)
            {
                if(petDataAll[i].IsEquiped)
                    mCurTypePet.Insert(0,petDataAll[i]);
                else
                    mCurTypePet.Add(petDataAll[i]);
            }
        }
        mCurTypePet.Sort((x, y) =>
        {
            int xWeight = (x.IsEquiped ? 1000 : 0)+ x.Level + x.CurLevel;
            int yWeight = (y.IsEquiped ? 1000 : 0) + y.Level + y.CurLevel;
            return yWeight.CompareTo(xWeight);
        });


        Reset();
        mViewObj.Scroller.Init(this, mCurTypePet.Count);

        if (reset)
        {
            if (mCurTypePet.Count > 0)
            {
                mViewObj.RightBg.SetActive(true);
                mViewObj.Scroller.Scrollbar.gameObject.SetActive(true);
                mViewObj.TextNull.gameObject.SetActive(false);
                BtnEvt_PetItemClick(PlayerPrefsBridge.Instance.GetPetIndexOf(mCurTypePet[0].idx));

            }
            else
            {
                mViewObj.RightBg.SetActive(false);
                mViewObj.Scroller.Scrollbar.gameObject.SetActive(false);
                mViewObj.TextNull.gameObject.SetActive(true);
                if (CurTab == ChildTab.Animal) mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_animal");
                else if (CurTab == ChildTab.Puppet) mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_puppet");
                else if (CurTab == ChildTab.Ghost) mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_ghost"); 
            }
        }
    }

    public ItemObj GetScrollItemObj(int inventoryPos)
    {
        foreach (var temp in mPetItemList)
        {
            if (temp.Value == null) continue;
            if (temp.Value.InventoryPos == inventoryPos)
                return temp.Value;
        }
        return null;
    }
    public void FreshPetInfo(int inventoryPos)
    {
        Pet curPet = PlayerPrefsBridge.Instance.GetPetByPosInventory(inventoryPos);

        mViewObj.TextName.text = curPet.ColorName;
        PetLevelUp lvUp = PetLevelUp.petLevelUpFetcher.GetPetLevelUpByCopy(curPet.CurLevel + curPet.Level);
        mViewObj.TextLevel.text = lvUp.name;
        mViewObj.TextDesc.text = curPet.Desc;
        //属性加成
        int[] curAdditionVal = new int[curPet.BaseVal.Length];
        for (int i = 0, length = curPet.ChangeVal.Length; i < length; i++)
        {
            if (curPet.CurLevel >= i)
                curAdditionVal[i] = curPet.ChangeVal[i];
            else
                curAdditionVal[i] = curPet.BaseVal[i];
        }
        for (int i = 0; i < curPet.BaseType.Length; i++)
        {
            //PromType type = (PromType)curPet.BaseType[i];
            //mViewObj.TextAttrList[i].text = string.Format("{0}: {1}", TUtility.TryGetAttrTypeStr(type), TUtility.TryGetValOfAttr(type, curAdditionVal[i]));
        }
        //携带等级限制
        if (PlayerPrefsBridge.Instance.PlayerData.Level < curPet.TakeLevel)
        {
            mViewObj.BtnEquip.enabled = false;
            mViewObj.TextBtnEquip.color = new Color(163 / 255f, 163 / 255f, 163 / 255f);
            mViewObj.TextEquipLevel.text = string.Format("<color=#F81414FF>{0}级可携带</color>", curPet.TakeLevel);
        }
        else
        {
            mViewObj.BtnEquip.enabled = true;
            mViewObj.TextBtnEquip.color = Color.white;
            mViewObj.TextEquipLevel.text = string.Format("{0}级可携带", curPet.TakeLevel);
            mViewObj.BtnEquip.SetOnAduioClick(delegate() { UIRootMgr.Instance.OpenWindow<WindowBig_RoleInfo>(WinName.WindowBig_RoleInfo).OpenWindow(); });
        }

        //宠物技能
        string skillStr = "";
        //for (int i = 0; i < curPet.Skill.c; i++)
        //{
        //    Skill spell = Skill.SpellFetcher.GetSpellByCopy(curPet.Skill[i]);
        //    if (skillStr != "")
        //        skillStr = skillStr + "\r\n";
        //    skillStr += string.Format("{0}：{1}", spell.Name, spell.Desc);
        //}
        mViewObj.TextSkill.text = skillStr;


        //升级耗材
        int costMa = 0;
        int costMaNum = 0;
        int costGold = 0;
        mViewObj.TextStateLimit.gameObject.SetActive(false);
        if (curPet.CurLevel == curPet.LevelMax) //进化
        {
            costMa = curPet.Science;
            costMaNum = curPet.ScienceNum;
            costGold = curPet.CostGold;
            mViewObj.TextBtnLvUp.text = "进  化";
            if (curPet.NextState != 0)
            {
                mViewObj.TextStateLimit.text = string.Format(
                    curPet.NextState > PlayerPrefsBridge.Instance.PlayerData.Level
                            ? "<color=#FF0000FF>进化条件: {0}</color>"
                            : "进化条件: {0}",HeroLevelUp.GetStateName(curPet.NextState));
                mViewObj.TextStateLimit.gameObject.SetActive(true);
            }
            else
            {
                mViewObj.TextStateLimit.text = "已达到最大等级";
                mViewObj.TextStateLimit.gameObject.SetActive(true);
            }
        }
        else
        {
            costMa = lvUp.Scinece;
            costMaNum = lvUp.Number;
            costGold = lvUp.CostGold;
            mViewObj.TextBtnLvUp.text = "喂  养";
        }
        if (costGold > 0)
        {
            mViewObj.TextCostMa0.gameObject.SetActive(true);
            mViewObj.TextCostMa0.text = string.Format("灵石: {0}", costGold);
        }
        else
            mViewObj.TextCostMa0.gameObject.SetActive(false);
        if (costMa == 0)
        {
            mViewObj.TextCostMa1.gameObject.SetActive(false);
            mViewObj.TextTitleCost.gameObject.SetActive(false);
        }
        else
        {
            //Item item = Item.ItemFetcher.GetItemByCopy(costMa);
            //mViewObj.TextCostMa1.gameObject.SetActive(true);
            //mViewObj.TextCostMa1.text = string.Format("{0}: {1}/{2}", item.Name, PlayerPrefsBridge.Instance.GetItemNum(item.idx), costMaNum);
            //mViewObj.TextTitleCost.gameObject.SetActive(true);
        }
        int tempIndex = PlayerPrefsBridge.Instance.GetPetIndexOf(curPet.idx);
        int tempPos = (int)curPet.Type;
        if (curPet.NextState == 0 && curPet.CurLevel == curPet.LevelMax)
            mViewObj.BtnLearn.gameObject.SetActive(false);
        else
        {
            mViewObj.BtnLearn.SetOnAduioClick(delegate() { BtnEvt_PetLvUp(tempIndex); });
            mViewObj.BtnLearn.gameObject.SetActive(true);
        }
        //if (curPet.IsEquiped)
        //{
        //    mViewObj.TextBtnEquip.text = "休息";
        //    mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_PetUnLoad(tempPos); });
        //}
        //else
        //{
        //    mViewObj.TextBtnEquip.text = "参战";
        //    mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_PetEquip(tempIndex, tempPos); });
        //}
        FreshEquipNum();
        //UIRootMgr.Instance.OpenWindow<Window_PetDetial>(WinName.Window_PetDetial, CloseUIEvent.None).OpenWindow(itemIndex);
    }


    public void BtnEvt_PetLvUp(int inventoryPos)
    {
        Pet pet = PlayerPrefsBridge.Instance.GetPetByPosInventory(inventoryPos);
        PetLevelUp lvUp = PetLevelUp.petLevelUpFetcher.GetPetLevelUpByCopy(pet.CurLevel + pet.Level);
        int costGold = 0;
        int costMatId = 0;
        int costMatNum = 0;
        if (pet.CurLevel == pet.LevelMax) //进化
        {
            costMatId = pet.Science;
            costMatNum = pet.ScienceNum;
            costGold = pet.CostGold;
        }
        else
        {
            costMatId = lvUp.Scinece;
            costMatNum = lvUp.Number;
            costGold = lvUp.CostGold;
        }

        if (PlayerPrefsBridge.Instance.PlayerData.Gold < costGold && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_JIN_BI_BU_ZU))
            return;
        if (PlayerPrefsBridge.Instance.GetItemNum(costMatId) < costMatNum && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_DAO_JU_BU_ZU))
            return;
        tempPet = pet;
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PetLevelUp(inventoryPos));
    }
    public void BtnEvt_PetEquip(int inventoryPos,int equipPos) 
    {
        // 装备宠物等级判断注释
        //Pet pet = PlayerPrefsBridge.Instance.GetPetByPosInventory(inventoryPos);
        //if (pet.TakeLevel > PlayerPrefsBridge.Instance.PlayerData.Level)
        //{
        //    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("等级不足，伙伴无法参战", Color.white);
        //    return;
        //}

        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipPet((sbyte)inventoryPos, (byte)equipPos));
    }
    public void BtnEvt_PetUnLoad(int equipPos)
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipPet((sbyte)Pet.PetTypeEnum.None, (byte)equipPos));
    }

    public void S2C_PetLvUp(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_PetLevelUp msg = MessageBridge.Instance.S2C_PetLevelUp(ios);
        FreshPetItem(false);
        BtnEvt_PetItemClick(mInventoryPos);
      
        Pet NewPet = Pet.PetFetcher.GetPetByCopy(msg.NewPetIdx);
        newPet = PlayerPrefsBridge.Instance.GetPetByPosInventory(mInventoryPos);
        StartCoroutine("AdditionAni");
        //if(msg.OldPetIdx==msg.NewPetIdx)//升级
        //{
        //    PetLevelUp petLv = PetLevelUp.petLevelUpFetcher.GetPetLevelUpByCopy(msg.Level);
        //    Item item = Item.ItemFetcher.GetItemByCopy(petLv.Scinece);
        //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("{0}升级成功", NewPet.Name));
        //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗{0}×{1}", item.Name, petLv.Number));
        //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗灵石×{0}", petLv.CostGold));        
        //}
        //else
        //{
        //    Pet OldPet = Pet.PetFetcher.GetPetByCopy(msg.OldPetIdx);
        //    Item item = Item.ItemFetcher.GetItemByCopy(OldPet.Science);
        //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("{0}成功进化为{1}！",OldPet.Name,NewPet.Name));
        //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗{0}×{1}", item.Name, OldPet.ScienceNum));
        //    UIRootMgr.Instance.OpenWindow<Window_PetLevelUp>(WinName.Window_PetLevelUp, CloseUIEvent.None).OpenWindow(tempPet, newPet);
        //}   
    }
    private Pet tempPet;
    private Pet newPet;
    IEnumerator AdditionAni()
    {
        //属性加成
        int[] newCurAdditionVal = new int[newPet.BaseVal.Length];
        for (int i = 0, length = newPet.ChangeVal.Length; i < length; i++)
        {
            if (newPet.CurLevel >= i)
                newCurAdditionVal[i] = newPet.ChangeVal[i];
            else
                newCurAdditionVal[i] = newPet.BaseVal[i];
        }

        int[] oldCurAdditionVal = new int[tempPet.BaseVal.Length];
        for (int i = 0, length = tempPet.ChangeVal.Length; i < length; i++)
        {
            if (tempPet.CurLevel >= i)
                oldCurAdditionVal[i] = tempPet.ChangeVal[i];
            else
                oldCurAdditionVal[i] = tempPet.BaseVal[i];

            int offsetVal = newCurAdditionVal[i] - oldCurAdditionVal[i];
            if (offsetVal > 0)
            {
                //PromType type = (PromType)tempPet.BaseType[i];
                //RectTransform textPos = mViewObj.TextAttrList[i].rectTransform;
                //Vector3 textPost = mViewObj.ListTextAttrAdd[i].rectTransform.anchoredPosition;
                //mViewObj.ListTextAttrAdd[i].rectTransform.anchoredPosition = new Vector3(textPos.anchoredPosition.x + textPos.sizeDelta.x + 6, textPost.y, textPost.z);
                //mViewObj.ListTextAttrAdd[i].text = string.Format("+{0}", TUtility.TryGetValOfAttr(type, offsetVal));
                //mViewObj.ListTextAttrAdd[i].gameObject.SetActive(true);
            }
            else
            {
                mViewObj.ListTextAttrAdd[i].gameObject.SetActive(false);
            }
        }
        mViewObj.AddAnimation.DoSelfAnimation();
        yield return new WaitForSeconds(2f);
    }

    //public void S2C_EquipPet(BinaryReader ios)
    //{
    //    UIRootMgr.Instance.IsLoading = false;
    //    NetPacket.S2C_EquipPet msg = MessageBridge.Instance.S2C_EquipPet(ios);

    //    int inventoryPos = PlayerPrefsBridge.Instance.PlayerData.PetList[msg.EquipPos];
    //    if (inventoryPos == (int)Pet.PetTypeEnum.None)//卸下
    //    {
    //        mPetItemList[mCurSelectItemIndex].IconItem.gameObject.SetActive(true);
    //        mPetItemList[mCurSelectItemIndex].IconEquipMark.gameObject.SetActive(false);
    //        mPetItemList[mCurSelectItemIndex].isEquip = false;
    //        mViewObj.TextBtnEquip.text = "参战";
    //        mViewObj.BtnChange.SetOnClick(delegate() {BtnEvt_PetEquip(mPetItemList[mCurSelectItemIndex].InventoryPos, msg.EquipPos); });
    //    }
    //    else
    //    {
    //        mViewObj.TextBtnEquip.text = "休息";
   
    //        mPetItemList[mCurSelectItemIndex].IconItem.gameObject.SetActive(false);
    //        mPetItemList[mCurSelectItemIndex].IconEquipMark.gameObject.SetActive(true);

    //        mPetItemList[mCurSelectItemIndex].isEquip = true;
    //        mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_PetUnLoad(msg.EquipPos); });
    //        for (int i = 0; i < mPetItemList.Count; i++)
    //        {

    //            if (mPetItemList[i].Type == mPetItemList[mCurSelectItemIndex].Type && i != mCurSelectItemIndex)
    //            {
    //                mPetItemList[i].IconItem.gameObject.SetActive(true);
    //                mPetItemList[i].IconEquipMark.gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //    FreshEquipNum();
    //}

    void FreshEquipNum()
    {
        int wearedNum = 0;
        for (int i = 0; i < mPetItemList.Count; i++)
        {
            if (mPetItemList[i].isEquip && mPetItemList[i].gameobject.activeInHierarchy)
            {
                wearedNum++;
                break;
            }
        }
        mViewObj.TextWearNum.text = string.Format("参战数量: {0}/1", wearedNum);
    }
}
