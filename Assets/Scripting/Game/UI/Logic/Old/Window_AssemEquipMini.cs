using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_AssemEquipMini : WindowBase,IScrollWindow
{
    public class ViewObj
    {
        public GameObject Part_SelectableItemBtnt;
        public Transform ItemRoot;      
        public Text TextName;
        public Text TextEquipType;
        public Text TextDes;
        public Text TextLvDemand;
        public Text TextMainProm;
        public Text TextSubProm0;
        public Text TextSubProm1;
        public Text TextSubProm2;
        public Text TextBtnChange;
        public Text TextPrice;
        public Button BtnChange;
        public List<Text> ListTextSub;
        public Button BtnSell;
        public GameObject RightBg;
        public ItemObj CurEquipEquip;
        public Button BtnMask;
        public UIScroller Scroller;
        public ViewObj(UIViewBase view)
        {
            Part_SelectableItemBtnt = view.GetCommon<GameObject>("Part_SelectableItemBtnt");    
            ItemRoot = view.GetCommon<Transform>("ItemRoot");       
            TextName = view.GetCommon<Text>("TextName");
            TextEquipType = view.GetCommon<Text>("TextEquipType");
            TextDes = view.GetCommon<Text>("TextDes");
            TextLvDemand = view.GetCommon<Text>("TextLvDemand");
            TextMainProm = view.GetCommon<Text>("TextMainProm");
            TextSubProm0 = view.GetCommon<Text>("TextSubProm0");
            TextSubProm1 = view.GetCommon<Text>("TextSubProm1");
            TextSubProm2 = view.GetCommon<Text>("TextSubProm2");
            TextBtnChange = view.GetCommon<Text>("TextBtnChange");
            TextPrice = view.GetCommon<Text>("TextPrice");
            BtnChange = view.GetCommon<Button>("BtnChange");
            BtnSell = view.GetCommon<Button>("BtnSell");
            RightBg = view.GetCommon<GameObject>("RightBg");
            BtnMask = view.GetCommon<Button>("BtnMask");
            CurEquipEquip = new ItemObj();
            CurEquipEquip.Init(view.GetCommon<UIViewBase>("CurEquipEquip"));
            Scroller = view.GetCommon<UIScroller>("Scroller");
            ListTextSub = new List<Text>();
            ListTextSub.Add(TextSubProm0);
            ListTextSub.Add(TextSubProm1);
            ListTextSub.Add(TextSubProm2);

        }

        public void ResetEquipScroll()
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
        public GameObject gameobject;
        public int InventoryPos;
        public Equip.EquipType Type;
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
        public void InitItem(Equip equip,int inventoryPos)
        {
            GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
            SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();

            HeroLevelUp lvUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(equip.originLevel);
            string equipLvDemand = lvUp.name.ToString() == "" ? "获取失败" : lvUp.name.ToString();
            this.NameText.text = TUtility.GetTextByQuality(equip.name, equip.curQuality);

            this.IconItem.sprite = commonSprite.GetSprite(equip.icon);
            this.IconEquipMark.sprite = commonSprite.GetSprite(equip.icon + "1");

            this.IconItem.gameObject.SetActive(!equip.curIsEquip);
            this.IconEquipMark.gameObject.SetActive(equip.curIsEquip);

            this.InventoryPos = inventoryPos;
            //this.Type = equip.equipPos;
            this.gameobject.SetActive(true);
            SelectItem(false);
        }

    }



    private ViewObj mViewObj;
    private Dictionary<int, ItemObj> mEquipItemList = new Dictionary<int, ItemObj>();
    private Equip.EquipType? mCurTab; //使用法宝穿戴位置类型代表Tab类型，None代表所有法宝
    private List<Equip> mCurTypeEquipList = new List<Equip>();
    private Dictionary<Equip, int> curEquipList = new Dictionary<Equip, int>();


    public void OpenWindowMini(Equip.EquipType posType)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        mViewObj.BtnSell.gameObject.SetActive(false);
        mViewObj.ResetEquipScroll();
        mCurTab = posType;
        FreshEquipItem();
        mViewObj.BtnMask.SetOnClick(delegate() { CloseWindow(CloseActionType.OpenHide); });
        RegisterNetCodeHandler(NetCode_S.EquipEquip, S2C_EquipEquip);
     //   RegisterNetCodeHandler(NetCode_S.SellEquip, S2C_SellEquip);
    }
  
    void BathSell(List<byte> sellEquipList)
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_SellEquip(sellEquipList));
    }
    public void FreshScrollItem(int index)
    {
        if (index > mCurTypeEquipList.Count || mCurTypeEquipList[index] == null)
        {
            TDebug.LogError(string.Format("{Equip不存在；index:{0}}", index));
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
        int tempPos = curEquipList[mCurTypeEquipList[index]];
        item.InitItem(mCurTypeEquipList[index], tempPos);
        item.SelectItem(item.Index == mCurSelectItemIndex);

        int tempIndex = index;
        item.BgBtn.SetOnClick(delegate() { BtnEvt_ItemClick(tempPos, tempIndex); });
        if (!mEquipItemList.ContainsKey(index))
            mEquipItemList.Add(index, item);
        else
            mEquipItemList[index] = item;
    }
    public void Reset()
    {
        mEquipItemList.Clear();
    }
    void FreshEquipItem()
    {
        List<Equip> mEquipDataList = PlayerPrefsBridge.Instance.GetEquipAllListNoCopy();
        curEquipList.Clear();
        mCurTypeEquipList.Clear(); 
        Equip curEquipEquip = null;
        int curEquipEquipIndex = -1;
        for (int i = 0; i < mEquipDataList.Count; i++)
        {
            if (mEquipDataList[i] != null && mEquipDataList[i].idx != 0 && (mCurTab == Equip.EquipType.None || (int)mEquipDataList[i].type == (int)mCurTab))
            {
                if (mEquipDataList[i].curIsEquip)
                {
                    curEquipEquipIndex = i;
                    curEquipEquip = mEquipDataList[i];
                }
                else
                    mCurTypeEquipList.Add(mEquipDataList[i]);
                curEquipList.Add(mEquipDataList[i], i);
            }
        }
        if (curEquipEquip != null)
        {
            int inventoryPos = curEquipList[curEquipEquip];
            mViewObj.CurEquipEquip.InitItem(curEquipEquip, inventoryPos);
            mViewObj.CurEquipEquip.BgBtn.SetOnClick(delegate() { BtnEvt_ItemClick(inventoryPos, -1); });
            curEquipList.Remove(curEquipEquip);
        }
        else
        {
            mViewObj.CurEquipEquip.gameobject.SetActive(false);
        }
       
        mCurTypeEquipList.Sort((x, y) =>{ return y.curQuality.CompareTo(x.curQuality);});

        Reset();
        mViewObj.Scroller.Init(this, mCurTypeEquipList.Count);

        if (mCurTypeEquipList.Count > 0)
        {
            BtnEvt_ItemClick(curEquipList[mCurTypeEquipList[0]], 0);
            mViewObj.RightBg.SetActive(true);
        }
        else if (curEquipEquip!=null)
        {
            BtnEvt_ItemClick(curEquipEquipIndex, -1);
            mViewObj.RightBg.SetActive(true);
        }
        else 
        {
            mViewObj.RightBg.SetActive(false);
        }
    }

    private int mCurSelectItemIndex = 0;
    public void BtnEvt_ItemClick(int mInventoryPos,int index)
    {
        mViewObj.CurEquipEquip.SelectItem(index == -1);
        foreach (var temp in mEquipItemList)
        {
            if (null != temp.Value) temp.Value.SelectItem(temp.Value.Index == index);
        }     
        mCurSelectItemIndex = index;
        FreshEquipInof(mInventoryPos);
    }

    void FreshEquipInof(int mInventoryPos)
    {
        Equip tempEquip = PlayerPrefsBridge.Instance.GetInventoryEquipByPos(mInventoryPos);
        mViewObj.TextName.text = TUtility.GetTextByQuality(tempEquip.name, tempEquip.curQuality);
        //mViewObj.TextEquipType.text = TUtility.TryGetEquipTypeString(tempEquip.EquipPos);
        mViewObj.TextDes.text = "\u3000\u3000" + tempEquip.desc;
        mViewObj.TextLvDemand.text = string.Format(PlayerPrefsBridge.Instance.PlayerData.Level < tempEquip.curLevel ? "<color=#FF0000FF>{0}级携带</color>" : "{0}级携带", tempEquip.curLevel);
        ///TODO ;法宝主属性值取第一个，待修改
        //mViewObj.TextMainProm.text = string.Format("{0}: {1}", TUtility.TryGetAttrTypeStr(tempEquip.mainAttrType), TUtility.TryGetValOfAttr(tempEquip.mainAttrType[0], tempEquip.mainAttrVal[0]));
        AttrType[] subProm = tempEquip.curSubType;
        for (int i = 0; i < mViewObj.ListTextSub.Count; i++)
        {
            if (i < subProm.Length)
            {
                string at = TUtility.TryGetAttrTypeStr(tempEquip.curSubType[i]);
                string av = TUtility.TryGetValOfAttr(tempEquip.curSubType[i], tempEquip.curSubVal[i]);
                mViewObj.ListTextSub[i].text = string.Format("{0}: {1}", at, av);
                mViewObj.ListTextSub[i].gameObject.SetActive(true);
            }
            else
                mViewObj.ListTextSub[i].gameObject.SetActive(false);
        }
        int EquipPos = (int)tempEquip.type;
        if (tempEquip.curLevel > PlayerPrefsBridge.Instance.PlayerData.Level)
        {
            mViewObj.BtnChange.enabled = false;
            mViewObj.BtnChange.image.color = Color.gray;
        }
        else
        {
            mViewObj.BtnChange.enabled = true;
            mViewObj.BtnChange.image.color = Color.white;
        }
        if (tempEquip.curIsEquip)
        {
            mViewObj.TextBtnChange.text = "卸下";
            mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_BtnUnloadClick(EquipPos); });
        }
        else
        {
            mViewObj.TextBtnChange.text = "装备";
            mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_BtnEquipClick(mInventoryPos, EquipPos); });
        }
    }


    public void BtnEvt_BtnUnloadClick(int partIndex)
    {
        if (PlayerPrefsBridge.Instance.PlayerData.EquipList[partIndex] != (int)Equip.EquipType.None)
        {
            UIRootMgr.Instance.IsLoading = true;
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipEquip((byte)partIndex, (sbyte)Equip.EquipType.None));
        }
        else
            TDebug.LogError(string.Format("当前位置{0}不存在法宝", partIndex));

    }
    public void BtnEvt_BtnEquipClick(int inventoryPos, int equipPos)
    {
        Equip equip = PlayerPrefsBridge.Instance.GetInventoryEquipByPos(inventoryPos);
        if (equip.curLevel > PlayerPrefsBridge.Instance.PlayerData.Level && UIRootMgr.Instance.MessageBox.ShowStatus("等级不足，无法装备该法宝"))
            return;

        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipEquip((byte)equipPos, (sbyte)inventoryPos));
        UIRootMgr.Instance.IsLoading = true;
    }

    public void BtnEvt_SellEquip(int inventoryPos)
    {
        Equip equip = PlayerPrefsBridge.Instance.GetInventoryEquipByPos(inventoryPos);
        List<byte> sellList = new List<byte>();
        sellList.Add((byte)inventoryPos);
        if (equip.curQuality >= 2)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel("紫色以上品质为高级法宝，是否确认出售？", delegate() { BathSell(sellList); });
        }
        else
        {
            BathSell(sellList);
        }
    }
    public void S2C_EquipEquip(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_EquipEquip msg = MessageBridge.Instance.S2C_EquipEquip(ios);
        int pos = PlayerPrefsBridge.Instance.PlayerData.EquipList[msg.EquipPos];
        if (pos != (int)Equip.EquipType.None)
        {
            CloseWindow(CloseActionType.OpenHide);
            return;
        }
        FreshEquipItem();           
    }
    #region
    //public void S2C_SellEquip(BinaryReader ios)
    //{
    //    UIRootMgr.Instance.IsLoading = false;
    //    NetPacket.S2C_SellEquip msg = MessageBridge.Instance.S2C_SellEquip(ios);
    //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("出售了 {0}件法宝", msg.SellNum));
    //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("获得 {0}灵石", msg.Gold));
    //    FreshEquipItem();
    //}
    #endregion
}
