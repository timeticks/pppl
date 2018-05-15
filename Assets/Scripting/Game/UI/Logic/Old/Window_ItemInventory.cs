using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Window_ItemInventory : WindowBase, IScrollWindow
{
    public class ViewObj
    {
        public Transform ItemRoot;
        public Text NameText;
        public Text DescText;
        public Button SellBtn;
        public Button UseBtn;
        public GameObject Part_SelectableItemBtn;
        public ScrollRect ScrollView;
        public Text TextFrameNum;
        public Image IconItem;
        public Text TextNum;
        public UIScroller Scroller;
        public GameObject RightBg;
        public Button MaskBtn;
        public ViewObj(UIViewBase view)
        {
            if (ItemRoot == null) ItemRoot = view.GetCommon<Transform>("ItemRoot");
            if (DescText == null) DescText = view.GetCommon<Text>("DescText");
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (SellBtn == null) SellBtn = view.GetCommon<Button>("SellBtn");
            if (Part_SelectableItemBtn == null) Part_SelectableItemBtn = view.GetCommon<GameObject>("Part_SelectableItemBtn");
            if (UseBtn == null) UseBtn = view.GetCommon<Button>("UseBtn");
            if (TextFrameNum == null) TextFrameNum = view.GetCommon<Text>("TextFrameNum");
            if (IconItem == null) IconItem = view.GetCommon<Image>("IconItem");
            if (TextNum == null) TextNum = view.GetCommon<Text>("TextNum");
            if (Scroller == null) Scroller = view.GetCommon<UIScroller>("Scroller");
            if (ScrollView == null) ScrollView = view.GetCommon<ScrollRect>("ScrollView");
            if (RightBg == null) RightBg = view.GetCommon<GameObject>("RightBg");
            if (MaskBtn == null) MaskBtn = view.GetCommon<Button>("MaskBtn");
        }
        public void ResetSiteScroll()
        {
            ScrollView.StopMovement();
            Vector3 tempPos = ItemRoot.localPosition;
            ItemRoot.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
            Scroller.OnValueChange(Vector2.zero);
        }

    }
    public class ItemObj : ItemIndexObj
    {
        public Text NameText;
        public Button BgBtn;
        public Image IconItem;
        public Sprite SP_45;
        public Sprite SP_42;
        public int ItemId;
        public static Transform BadgeItemTrans;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (BgBtn == null) BgBtn = view.GetCommon<Button>("BgBtn");
            if (IconItem == null) IconItem = view.GetCommon<Image>("IconItem");
            SP_45 = view.GetCommon<Sprite>("SP_45");
            SP_42 = view.GetCommon<Sprite>("SP_42");
        }
        public void SelectItem(bool select)
        {
            if (BgBtn == null || BgBtn.image == null) return;
            BgBtn.enabled = !select;
            BgBtn.image.overrideSprite = select ? SP_42 : SP_45;
            BgBtn.image.sprite = select ? SP_42 : SP_45;
        }
        public void InitItem(Item item)
        {
            //GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
            //SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();
            this.ItemId = item.idx;
            this.NameText.text = TUtility.GetTextByQuality(item.name, item.quality);
            //this.IconItem.sprite = commonSprite.GetSprite(item.icon);
        }
    }
    private Dictionary<int, ItemObj> mItemList = new Dictionary<int, ItemObj>();
    private ViewObj mViewObj;
    private int mCurSelectIndex;
    private int InventoryCapacity = 80;

    public void OpenWindow(Item.ItemType onlyShowType = Item.ItemType.None)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        OpenWin();
        Init();
        mViewObj.ResetSiteScroll();
        RegisterNetCodeHandler(NetCode_S.SellItem, S2C_ItemSell);
        RegisterNetCodeHandler(NetCode_S.UseItem, S2C_UseItem);
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }
    public void Init()
    {
        mCurSelectIndex = 0;
        mViewObj.SellBtn.SetOnClick(BtnEvt_Sell);
        mViewObj.UseBtn.SetOnClick(BtnEvt_Use);
        mViewObj.MaskBtn.SetOnClick(BtnEvt_Exit);
        FreshItem();
        if (mItemList.ContainsKey(0)) BtnEvt_ItemDetail(0);
    }

    List<Item> GetItemList()
    {
        return PlayerPrefsBridge.Instance.GetItemAllListCopy();
    }
    private List<Item> ItemList;

    public void FreshScrollItem(int index)
    {
        if (index > ItemList.Count || ItemList[index] == null)
        {
            TDebug.LogError(string.Format("{Item不存在；index:{0}}", index));
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
            item.Init(mViewObj.Scroller.GetNewObj(mViewObj.ItemRoot, mViewObj.Part_SelectableItemBtn));
        }
        item.Scroller = mViewObj.Scroller;
        item.Index = index;
        mViewObj.Scroller._itemList.Add(item);

        //刷新显示
        item.InitItem(ItemList[index]);
        item.SelectItem(item.Index == mCurSelectIndex);

        int tempIndex = index;
        item.BgBtn.SetOnClick(delegate() { BtnEvt_ItemDetail(tempIndex); });
        if (!mItemList.ContainsKey(index))
            mItemList.Add(index, item);
        else
            mItemList[index] = item;
    }


    public void Reset()
    {
        mItemList.Clear();
    }


    void FreshItem()
    {
        ItemList = GetItemList();
        ItemList = SortByMaxStack(ItemList);
        if (mCurSelectIndex >= ItemList.Count) mCurSelectIndex = ItemList.Count-1;
        Reset();
        mViewObj.Scroller.Init(this, ItemList.Count);

        TDebug.Log("初始化道具仓库：length:" + ItemList.Count);

        if (ItemList.Count <= 0)
        {
            mViewObj.NameText.text = "你的背包的空的";
            mViewObj.DescText.text = "";
            mViewObj.TextNum.text = "";
            mViewObj.SellBtn.gameObject.SetActive(false);
            mViewObj.Scroller.Scrollbar.gameObject.SetActive(false);
            mViewObj.RightBg.SetActive(false);
        }
        else
        {
            mViewObj.Scroller.Scrollbar.gameObject.SetActive(true);
            mViewObj.Scroller.Scrollbar.gameObject.SetActive(true);
        }
        int itemNum = ItemList.Count;
        if (itemNum > InventoryCapacity)
            mViewObj.TextFrameNum.text = string.Format("储存空间: <color=#FF0000FF>{0}/{1}</color>", itemNum, InventoryCapacity);
        else
            mViewObj.TextFrameNum.text = string.Format("储存空间: {0}/{1}", itemNum, InventoryCapacity);
    }
   

    /// <summary>
    /// 根据最大叠加数刷新列表
    /// </summary>
    List<Item> SortByMaxStack(List<Item> itemList)
    {
        //itemList.Sort((x, y) => { return x.idx.CompareTo(y.idx); });
        itemList.Sort((x, y) =>
        {
            if ((x.type == Item.ItemType.Stuff && y.type == Item.ItemType.Task) || (y.type == Item.ItemType.Stuff && x.type == Item.ItemType.Task))
            {
                return ((int)x.type).CompareTo((int)y.type);
            }
            if(y.type == x.type)
                return ((int)x.idx).CompareTo(y.idx);
            return ((int)y.type).CompareTo((int)x.type);
        }); //根据类型排序 ：3、1、2

        for (int i = 0, length = itemList.Count; i < length; i++)
        {
            if (itemList[i].num > itemList[i].maxStack)
            {
                int remainNum = itemList[i].num - itemList[i].maxStack;

                itemList[i].num = itemList[i].maxStack;
                int count = Mathf.CeilToInt(remainNum / (float)itemList[i].maxStack);
                Item newItem = Item.Fetcher.GetItemCopy(itemList[i].idx);
                for (int j = 0; j < count; j++)
                {
                    int num = remainNum > itemList[i].maxStack ? itemList[i].maxStack : remainNum;
                    newItem.num = num;
                    itemList.Insert(i + 1, newItem);
                }
            }
        }
        return itemList;
    }

    public void BtnEvt_ItemDetail(int tempIndex)
    {
        mCurSelectIndex = tempIndex;
        mCurSelectIndex = Mathf.Clamp(mCurSelectIndex, 0, ItemList.Count - 1);
        Item item = ItemList[mCurSelectIndex];

        if (item == null) return;
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
        SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();

        //经验丹红点
        //if (item.idx == GameConstUtils.InventoryBadgeItem && mItemList.Count > mCurSelectIndex)
        //{
        //    SaveUtils.DeleteKeyInPlayer(EvtListenerType.InventoryBadgeOpen.ToString());
        //    BadgeTips.SetBadgeViewFlase(mItemList[mCurSelectIndex].BgBtn.transform);
        //}

        //mViewObj.IconItem.sprite = commonSprite.GetSprite(item.icon);
        mViewObj.NameText.text = TUtility.GetTextByQuality(item.name, item.quality);
        mViewObj.TextNum.text = string.Format(" 数量:{0}", item.num);
        mViewObj.DescText.text = "\u3000\u3000" + item.desc;

        mViewObj.SellBtn.gameObject.SetActive(item.canSell);
        mViewObj.UseBtn.gameObject.SetActive(item.CanUse());
        foreach (var temp in mItemList)
        {
            if(null!=temp.Value)temp.Value.SelectItem(temp.Value.Index == mCurSelectIndex);
        }
        //for (int i = 0; i < mItemList.Count; i++) //显示选中
        //{
        //    if (mItemList[i] != null)
        //        mItemList[i].SelectItem(mItemList[i].Index == mCurSelectIndex);
        //}
    }

    public void BtnEvt_Sell() //点击出售，弹出数量选择
    {
        if (mCurSelectIndex < 0) return;
        List<Item> itemList = GetItemList();
        itemList = SortByMaxStack(itemList);
        Item item = itemList[mCurSelectIndex];
        if (item == null) return;
        if (!item.canSell)
        {
            UIRootMgr.Instance.Window_UpTips.InitTips("不能出售", Color.red);
            return;
        }
        UIRootMgr.Instance.OpenWindow<Window_SellNumChoose>(WinName.Window_SellNumChoose, CloseUIEvent.None).OpenWindow(item.idx, item.num, SellItem);
    }

    public void BtnEvt_Exit()
    {
        CloseWindow();
    }


    public void SellItem(int sellNum)
    {
        List<Item> itemList = GetItemList();
        itemList = SortByMaxStack(itemList);
        Item item = itemList[mCurSelectIndex];
        if (item == null || sellNum <= 0) return;
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_SellItem(item.idx, sellNum));

    }

    void S2C_ItemSell(BinaryReader ios)
    {
        NetPacket.S2C_SellItem msg = MessageBridge.Instance.S2C_SellItem(ios);
        Item item = Item.Fetcher.GetItemCopy(msg.ItemID);
        UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("你出售了 {0}×{1}", item.name, msg.Num));
        UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("获得 {0}×{1}", "灵石", msg.Gold));
        UIRootMgr.Instance.IsLoading = false;
        FreshItem();
        BtnEvt_ItemDetail(mCurSelectIndex);
    }

    private Item curUseItem = null;
    public void BtnEvt_Use() //点击使用
    {
        if (mCurSelectIndex < 0) return;
        List<Item> itemList = GetItemList();
        itemList = SortByMaxStack(itemList);
        Item item = itemList[mCurSelectIndex];
        if (item == null) return;
        curUseItem = new Item();
        curUseItem.name = item.name;
        curUseItem.num = 1;
        if (item.CanUse())
        {
            UIRootMgr.Instance.IsLoading = true;
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_UseItem(item.idx, 1));
        }
    }
    void S2C_UseItem(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_UseItem msg = MessageBridge.Instance.S2C_UseItem(ios);
        Item item = Item.Fetcher.GetItemCopy(msg.ItemIdx);
        UIRootMgr.LobbyUI.ShowDropInfo(msg.GoodsList, string.Format("你使用了{0}×{1}", item.name, 1));
        FreshItem();
        BtnEvt_ItemDetail(mCurSelectIndex);
        //if (item.EffectType == Item.ItemEffectType.AddProm)
        //{
        //    PlayerPrefsBridge.Instance.FreshPromAchieve();
        //}
    }
}
