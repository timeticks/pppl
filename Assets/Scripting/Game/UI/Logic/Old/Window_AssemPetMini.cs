using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_AssemPetMini : WindowBase,IScrollWindow {

    public class ViewObj
    {
        public GameObject Part_SelectableItemBtnt;   
        public Transform ItemRoot;
        public GameObject RightBg;
        public Text TextName;
        public Text TextLevel;
        public Text TextDesc;
        public Text TextAttributeA;
        public Text TextAttributeB;
        public Text TextAttributeC;
        public Text TextAttributeD;
        public Text TextSkillName;
        public Text TextSkillDesc;
        public Text TextCostMa0;
        public Text TextCostMa1;
        public Text TextStateLimit;
        public Button BtnLearn;
        public Button BtnChange;
        public Text TextBtnLvUp;
        public Text TextBtnEquip;
        public ItemObj CurEquipPet;
        public List<Text> TextAttrList;
        public Button BtnMask;
        public UIScroller Scroller;
        public Text TextTitleCost;
        public ViewObj(UIViewBase view)
        {
            Part_SelectableItemBtnt = view.GetCommon<GameObject>("Part_SelectableItemBtnt");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            RightBg = view.GetCommon<GameObject>("RightBg");
            TextName = view.GetCommon<Text>("TextName");
            TextLevel = view.GetCommon<Text>("TextLevel");
            TextDesc = view.GetCommon<Text>("TextDesc");
            TextAttributeA = view.GetCommon<Text>("TextAttributeA");
            TextAttributeB = view.GetCommon<Text>("TextAttributeB");
            TextAttributeC = view.GetCommon<Text>("TextAttributeC");
            TextAttributeD = view.GetCommon<Text>("TextAttributeD");
            TextSkillName = view.GetCommon<Text>("TextSkillName");
            TextSkillDesc = view.GetCommon<Text>("TextSkillDesc");
            TextCostMa0 = view.GetCommon<Text>("TextCostMa0");
            TextCostMa1 = view.GetCommon<Text>("TextCostMa1");
            TextStateLimit = view.GetCommon<Text>("TextStateLimit");
            BtnLearn = view.GetCommon<Button>("BtnLearn");
            BtnChange = view.GetCommon<Button>("BtnChange");
            TextBtnLvUp = view.GetCommon<Text>("TextBtnLvUp");
            TextBtnEquip = view.GetCommon<Text>("TextBtnEquip");
            Scroller = view.GetCommon<UIScroller>("Scroller");
            TextTitleCost = view.GetCommon<Text>("TextTitleCost");
            CurEquipPet = new ItemObj();
            CurEquipPet.Init(view.GetCommon<UIViewBase>("CurEquipPet"));
            TextAttrList = new List<Text>();
            TextAttrList.Add(TextAttributeA);
            TextAttrList.Add(TextAttributeB);
            TextAttrList.Add(TextAttributeC);
            TextAttrList.Add(TextAttributeD);
        }
        public void ResetPetScroll()
        {
            Scroller.ScrollView.StopMovement();
            Vector3 tempPos = ItemRoot.transform.localPosition;
            ItemRoot.transform.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
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
            SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();

            this.gameobject.SetActive(true);
            this.NameText.text = pet.ColorName;
            this.SelectItem(false);
            this.isEquip = pet.IsEquiped;

            this.IconItem.sprite = commonSprite.GetSprite(TUtility.TryGetPetIcon(pet.Type));
            this.IconEquipMark.sprite = commonSprite.GetSprite(TUtility.TryGetPetIcon(pet.Type) + "1");

            this.IconItem.gameObject.SetActive(!pet.IsEquiped);
            this.IconEquipMark.gameObject.SetActive(pet.IsEquiped);

            this.Type = pet.Type;
            int tempPos = PlayerPrefsBridge.Instance.GetPetIndexOf(pet.idx);
            this.InventoryPos = tempPos;
        }

    }
    private Pet.PetTypeEnum curPetType;


    private ViewObj mViewObj;
    private Dictionary<int, ItemObj> mPetItemList = new Dictionary<int, ItemObj>();
    private List<Pet> mCurTypePetList = new List<Pet>();
    private int mInventoryPos;


    public void OpenWindowMini(Pet.PetTypeEnum type)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        mViewObj.BtnMask = mViewBase.GetCommon<Button>("BtnMask");
        OpenWin();
        curPetType = type;
        FreshPetItem();
        mViewObj.BtnMask.SetOnClick(delegate() { CloseWindow(CloseActionType.OpenHide); });
        RegisterNetCodeHandler(NetCode_S.PetLevelUp, S2C_PetLvUp);
        RegisterNetCodeHandler(NetCode_S.EquipPet, S2C_EquipPet);
    }
    public void BtnEvt_PetItemClick(int inventoryPos,int index)
    {
        mViewObj.CurEquipPet.SelectItem(index==-1);
        foreach (var item in mPetItemList)
        {
            if (null != item.Value) item.Value.SelectItem(item.Value.InventoryPos == inventoryPos);
        }
        FreshPetInfo(inventoryPos);
    }

    public void FreshScrollItem(int index)
    {
        if (index > mCurTypePetList.Count || mCurTypePetList[index] == null)
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
        item.InitItem(mCurTypePetList[index]);
        item.SelectItem(item.InventoryPos == mInventoryPos);

        int tempPos = item.InventoryPos;
        int tempIndex = item.Index;
        item.BgBtn.SetOnClick(delegate() { BtnEvt_PetItemClick(tempPos, tempIndex); });
        if (!mPetItemList.ContainsKey(index))
            mPetItemList.Add(index, item);
        else
            mPetItemList[index] = item;
    }
    public void Reset()
    {
        mPetItemList.Clear();
    }


    public void FreshPetItem(bool reset =true)
    {
        List<Pet> petDataAll = PlayerPrefsBridge.Instance.GetPetAllListCopy();     
        Pet curEquipPet = null;
        mCurTypePetList.Clear();
        mPetItemList.Clear();
        for (int i = 0; i < petDataAll.Count; i++)
        {
            if (petDataAll[i].Type == curPetType)
            {
                mCurTypePetList.Add(petDataAll[i]);
                if (petDataAll[i].IsEquiped)
                    curEquipPet = petDataAll[i];
            }         
        }
        if (curEquipPet != null)
        {
            mViewObj.CurEquipPet.InitItem(curEquipPet);
            int inventoryPos = PlayerPrefsBridge.Instance.GetPetIndexOf(curEquipPet.idx);
            mViewObj.CurEquipPet.BgBtn.SetOnClick(delegate() { BtnEvt_PetItemClick(inventoryPos, -1); });
            mCurTypePetList.Remove(curEquipPet);
        }
        else
        {
            mViewObj.CurEquipPet.gameobject.SetActive(false);
        }
        Reset();
        mViewObj.Scroller.Init(this, mCurTypePetList.Count);   
         
        if (reset)
        {
            if (mCurTypePetList.Count > 0)
            {
                mViewObj.RightBg.SetActive(true);
                int inventoryPos = PlayerPrefsBridge.Instance.GetPetIndexOf(mCurTypePetList[0].idx);
                BtnEvt_PetItemClick(inventoryPos,0);
            }
            else if (curEquipPet != null)
            {
                mViewObj.RightBg.SetActive(true);
                int inventoryPos = PlayerPrefsBridge.Instance.GetPetIndexOf(curEquipPet.idx);
                BtnEvt_PetItemClick(inventoryPos,- 1);
            }
            else
            {
                mViewObj.RightBg.SetActive(false);
            }

        }
       
    }
    public void FreshPetInfo(int inventoryPos )
    {
        mInventoryPos = inventoryPos;
        Pet curPet = PlayerPrefsBridge.Instance.GetPetByPosInventory(inventoryPos);
        mViewObj.TextName.text = curPet.ColorName;
        PetLevelUp lvUp = PetLevelUp.petLevelUpFetcher.GetPetLevelUpByCopy(curPet.CurLevel+curPet.Level);
        mViewObj.TextLevel.text = lvUp.name;
        mViewObj.TextDesc.text = curPet.Desc;
        //属性加成
        int[] curAdditionVal = new int[curPet.BaseVal.Length];
        for (int i = 0,length = curPet.ChangeVal.Length ; i < length; i++)
        {
            if (curPet.CurLevel >= i)
                curAdditionVal[i] = curPet.ChangeVal[i];
            else
                curAdditionVal[i] = curPet.BaseVal[i];
        }
        for (int i = 0; i < curPet.BaseType.Length; i++)
        {
            AttrType type = (AttrType)curPet.BaseType[i];
            mViewObj.TextAttrList[i].text = string.Format("{0}:{1}", TUtility.TryGetAttrTypeStr(type), TUtility.TryGetValOfAttr(type, curAdditionVal[i]));
        }
        //宠物技能
        Spell spell = Spell.Fetcher.GetSpellCopy(curPet.Skill);
        mViewObj.TextSkillName.text = spell.name;
        mViewObj.TextSkillDesc.text = spell.desc;
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
            mViewObj.TextBtnLvUp.text = "进化";
            if (curPet.NextState != 0)
            {
                mViewObj.TextStateLimit.text = string.Format("进化条件：{0}", HeroLevelUp.GetStateName(curPet.NextState));
                mViewObj.TextStateLimit.gameObject.SetActive(true);
                mViewObj.BtnLearn.gameObject.SetActive(true);
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
            mViewObj.TextBtnLvUp.text = "喂养";
            mViewObj.BtnLearn.gameObject.SetActive(true);
        }
        if (costGold > 0)
        {
            mViewObj.TextCostMa0.gameObject.SetActive(true);
            mViewObj.TextCostMa0.text = string.Format("灵石: {0}", costGold);
        }
        else
            mViewObj.TextCostMa0.gameObject.SetActive(false);
        if(costMa==0) 
        {
            mViewObj.TextTitleCost.gameObject.SetActive(false);
            mViewObj.TextCostMa1.gameObject.SetActive(false);
        }
        else
        {
            Item item = Item.Fetcher.GetItemCopy(costMa);
            mViewObj.TextCostMa1.gameObject.SetActive(true);
            mViewObj.TextCostMa1.text = string.Format("{0}: {1}/{2}", item.name,PlayerPrefsBridge.Instance.GetItemNum(item.idx),costMaNum);
            mViewObj.TextTitleCost.gameObject.SetActive(true);
        }
        // 满级判断
        int tempIndex= PlayerPrefsBridge.Instance.GetPetIndexOf(curPet.idx);
        int tempPos = (int)curPet.Type;
        if (curPet.NextState == 0&&curPet.CurLevel==curPet.LevelMax)
            mViewObj.BtnLearn.gameObject.SetActive(false);
        else
        {
            mViewObj.BtnLearn.SetOnClick(delegate() { BtnEvt_PetLvUp(tempIndex); });
            mViewObj.BtnLearn.gameObject.SetActive(true); 
        }
        if (curPet.IsEquiped)
        {
            mViewObj.TextBtnEquip.text = "休息";
            mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_PetUnLoad(tempPos); });
        }
        else
        {
            mViewObj.TextBtnEquip.text = "参战";
            mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_PetEquip(tempIndex, tempPos); });
        }  
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
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PetLevelUp(inventoryPos));
    }
    public void BtnEvt_PetEquip(int inventoryPos,int equipPos) 
    {
        Pet pet = PlayerPrefsBridge.Instance.GetPetByPosInventory(inventoryPos);
        if (pet.TakeLevel > PlayerPrefsBridge.Instance.PlayerData.Level&& UIRootMgr.Instance.MessageBox.ShowStatus("等级不足，伙伴无法参战"))
        {          
            return;
        }

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
        int pos = PlayerPrefsBridge.Instance.GetPetIndexOf(msg.NewPetIdx);
        FreshPetInfo(pos);

        Pet OldPet = Pet.PetFetcher.GetPetByCopy(msg.OldPetIdx);
        if(msg.OldPetIdx==msg.NewPetIdx)//升级
        {
            PetLevelUp petLv = PetLevelUp.petLevelUpFetcher.GetPetLevelUpByCopy(msg.Level);
            Item item = Item.Fetcher.GetItemCopy(petLv.Scinece);
            UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("{0}升级成功", OldPet.name));
            UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗{0}×{1}", item.name, petLv.Number));
            UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗灵石×{0}", petLv.CostGold));
        }
        else
        {
            Pet NewPet = Pet.PetFetcher.GetPetByCopy(msg.NewPetIdx);
            Item item = Item.Fetcher.GetItemCopy(OldPet.Science);
            UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("{0}成功进化为{1}！",OldPet.name,NewPet.name));
            UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("消耗{0}×{1}", item.name, OldPet.ScienceNum));
        }   
    }
    public void S2C_EquipPet(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_EquipPet msg = MessageBridge.Instance.S2C_EquipPet(ios);
        int pos = PlayerPrefsBridge.Instance.PlayerData.PetList[msg.EquipPos];
        if (pos != (int)Pet.PetTypeEnum.None)
        {
            CloseWindow(CloseActionType.OpenHide);
            return;
        }
        FreshPetItem();
        #region 装配
        //int inventoryPos = PlayerPrefsBridge.Instance.PlayerData.PetList[msg.EquipPos];

        //if (inventoryPos == (int)Pet.PetTypeEnum.None)//卸下
        //{
        //    mPetItemList[mCurSelectItemIndex].IconItem.gameObject.SetActive(true);
        //    mPetItemList[mCurSelectItemIndex].IconEquipMark.gameObject.SetActive(false);
        //    mPetItemList[mCurSelectItemIndex].isEquip = false;
        //    mViewObj.TextBtnEquip.text = "参战";
        //    mViewObj.BtnChange.SetOnClick(delegate() {BtnEvt_PetEquip(mPetItemList[mCurSelectItemIndex].InventoryPos, msg.EquipPos); });
        //}
        //else
        //{
        //    mViewObj.TextBtnEquip.text = "休息";
   
        //    mPetItemList[mCurSelectItemIndex].IconItem.gameObject.SetActive(false);
        //    mPetItemList[mCurSelectItemIndex].IconEquipMark.gameObject.SetActive(true);

        //    mPetItemList[mCurSelectItemIndex].isEquip = true;
        //    mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_PetUnLoad(msg.EquipPos); });
        //    for (int i = 0; i < mPetItemList.Count; i++)
        //    {

        //        if (mPetItemList[i].Type == mPetItemList[mCurSelectItemIndex].Type && i != mCurSelectItemIndex)
        //        {
        //            mPetItemList[i].IconItem.gameObject.SetActive(true);
        //            mPetItemList[i].IconEquipMark.gameObject.SetActive(false);
        //        }
        //    }
        //}
        #endregion
    }
}
