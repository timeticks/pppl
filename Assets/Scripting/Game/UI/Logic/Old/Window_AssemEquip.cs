using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_AssemEquip : WindowBase,IScrollWindow
{
    public class ViewObj
    {
        public GameObject Part_SelectableItemBtnt;
        public Button ButtonType;
        public GameObject IconTypeMark;
        public Text TextBtnType;
        public GameObject PanelBtn;
        public Transform ItemRoot;
        public Button ButtonAll;
        public Button ButtonAtk;
        public Button ButtonDef0;
        public Button ButtonDef1;
        public Button ButtonAss0;
        public Button ButtonAss1;
        public Button ButtonAss2;
        public Button ButtonAss3;
        public Button ButtonAss4;
        public Text TextName;
        public Text TextEquipType;
        public Text TextDesc;
        public Text TextLvDemand;
        public Text TextBtnChange;
        public Text TextPrice;
        public Button BtnChange;
        public List<Button> BtnSellList;
        public List<Button> ButtonList;
        public Button BtnSell;
        public GameObject RightBg;

        public Button BtnBatchSell;
        public GameObject IconSellMark;
        public GameObject SellPanel;
        public Button BtnSell0;
        public Button BtnSell1;
        public Button BtnSell2;
        public Button BtnSell3;
        public Button BtnSell4;
        public Text TextFrameNum;
        public UIScroller Scroller;
        public Text TextNull;
        public Button BtnExit;

        public ViewObj(UIViewBase view)
        {
            Part_SelectableItemBtnt = view.GetCommon<GameObject>("Part_SelectableItemBtnt");
            ButtonType = view.GetCommon<Button>("ButtonType");
            IconTypeMark = view.GetCommon<GameObject>("IconTypeMark"); 
            TextBtnType = view.GetCommon<Text>("TextBtnType");
            PanelBtn = view.GetCommon<GameObject>("PanelBtn");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            ButtonAll = view.GetCommon<Button>("ButtonAll");
            ButtonAtk = view.GetCommon<Button>("ButtonAtk"); 
            ButtonDef0 = view.GetCommon<Button>("ButtonDef0");
            ButtonDef1 = view.GetCommon<Button>("ButtonDef1");
            ButtonAss0 = view.GetCommon<Button>("ButtonAss0");
            ButtonAss1 = view.GetCommon<Button>("ButtonAss1");
            ButtonAss2 = view.GetCommon<Button>("ButtonAss2");
            ButtonAss3 = view.GetCommon<Button>("ButtonAss3");
            ButtonAss4 = view.GetCommon<Button>("ButtonAss4");
            TextName = view.GetCommon<Text>("TextName");
            TextEquipType = view.GetCommon<Text>("TextEquipType");
            TextDesc = view.GetCommon<Text>("TextDesc");
            TextLvDemand = view.GetCommon<Text>("TextLvDemand");
            TextBtnChange = view.GetCommon<Text>("TextBtnChange");
            TextPrice = view.GetCommon<Text>("TextPrice");
            BtnChange = view.GetCommon<Button>("BtnChange");
            BtnSell = view.GetCommon<Button>("BtnSell");
            RightBg = view.GetCommon<GameObject>("RightBg");

            BtnBatchSell = view.GetCommon<Button>("BtnBatchSell");
            IconSellMark = view.GetCommon<GameObject>("IconSellMark");
            SellPanel = view.GetCommon<GameObject>("SellPanel");
            BtnSell0 = view.GetCommon<Button>("BtnSell0");
            BtnSell1 = view.GetCommon<Button>("BtnSell1");
            BtnSell2 = view.GetCommon<Button>("BtnSell2");
            BtnSell3 = view.GetCommon<Button>("BtnSell3");
            BtnSell4 = view.GetCommon<Button>("BtnSell4");
            TextFrameNum = view.GetCommon<Text>("TextFrameNum");
            Scroller = view.GetCommon<UIScroller>("Scroller");
            TextNull = view.GetCommon<Text>("TextNull");
            BtnExit = view.GetCommon<Button>("BtnExit");

            ButtonList = new List<Button>();
            BtnSellList = new List<Button>();
            ButtonList.Add(ButtonAtk);
            ButtonList.Add(ButtonDef0);
            ButtonList.Add(ButtonDef1);
            ButtonList.Add(ButtonAss0);
            ButtonList.Add(ButtonAss1);
            ButtonList.Add(ButtonAss2); ButtonList.Add(ButtonAss3); ButtonList.Add(ButtonAss4);
            BtnSellList.Add(BtnSell0);
            BtnSellList.Add(BtnSell1);
            BtnSellList.Add(BtnSell2);
            BtnSellList.Add(BtnSell3); BtnSellList.Add(BtnSell4);
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

            this.NameText.text = TUtility.GetTextByQuality(equip.name, equip.curQuality);
            this.IconItem.sprite = commonSprite.GetSprite(equip.icon);
            this.IconEquipMark.sprite = commonSprite.GetSprite(equip.icon + "1");
            this.IconItem.gameObject.SetActive(!equip.curIsEquip);
            this.IconEquipMark.gameObject.SetActive(equip.curIsEquip);

            this.InventoryPos = inventoryPos;
            this.Type = equip.type;
            this.SelectItem(false);
            this.gameobject.SetActive(true);
        }

    }
    private ViewObj mViewObj;
    private Dictionary<int, ItemObj> mEquipItemList = new Dictionary<int, ItemObj>();
    private Equip.EquipType? mCurTab; //使用法宝穿戴位置类型代表Tab类型，None代表所有法宝
    private List<Equip> mCurTypeEquipList =new List<Equip>();
    private Dictionary<Equip, int> curEquipList = new Dictionary<Equip, int>();
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Init();
    }

    void Init()
    {
        mViewObj.ButtonType.SetOnAduioClick(delegate() { BtEvt_ClickTypeTab(); });
        mViewObj.BtnBatchSell.SetOnAduioClick(delegate() { BtnEvt_ClickSellTab(); });
        mViewObj.ButtonAll.SetOnAduioClick(delegate() { OpenChildTab(Equip.EquipType.None); });
        mViewObj.BtnExit.SetOnClick(delegate() { CloseWindow();});
        //mViewObj.BtnChange.SetOnAduioClick(delegate() { UIRootMgr.Instance.OpenWindow<WindowBig_RoleInfo>(WinName.WindowBig_RoleInfo).OpenWindow(); });
        for (int i = 0; i < (int)Equip.EquipType.Max; i++)
        {
            int tempidx = i;
            mViewObj.ButtonList[i].SetOnAduioClick(delegate() { OpenChildTab((Equip.EquipType)tempidx); });
        }
        for (int i = 0; i < mViewObj.BtnSellList.Count; i++)
        {
            int quality = i;
            mViewObj.BtnSellList[i].SetOnAduioClick(delegate() { CallBackSellEquip(quality); });
        }

        mCurTab = null;
        OpenChildTab(Equip.EquipType.None);
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
    void OpenChildTab(Equip.EquipType toTab)
    {
        if (mCurTab != null && mCurTab == toTab) { return; }
        mCurTab = toTab;
        mViewObj.ResetEquipScroll();
        mViewObj.PanelBtn.SetActive(false);
        mViewObj.SellPanel.SetActive(false);
        mViewObj.IconSellMark.transform.rotation = Quaternion.Euler(Vector3.zero);
        mViewObj.IconTypeMark.transform.rotation = Quaternion.Euler(Vector3.zero);
        FreshEquipItem();
    }

    void CallBackSellEquip(int quality)
    {
        List<Equip> equipDataList = PlayerPrefsBridge.Instance.GetEquipAllListNoCopy();
        List<int> sellEquipList = new List<int>();
        for (int i = 0; i < equipDataList.Count; i++)
        {
            if (equipDataList[i].idx != 0 && equipDataList[i].curQuality == quality && equipDataList[i].curIsEquip == false
                && (mCurTab == Equip.EquipType.None||equipDataList[i].type == mCurTab)
                &&equipDataList[i].curLevel<PlayerPrefsBridge.Instance.PlayerData.Level)
            {
                sellEquipList.Add(i);
            }
        }
        if (sellEquipList.Count == 0)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(string.Format("当前无可出售的{0}{1}", TUtility.TryGetEquipQualityString(quality),mCurTab.GetDesc()), Color.white);
        }
        else
        {
            if (quality >= 2)
            {
                UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel("紫色以上品质为高级法宝，是否确认出售？", delegate() { BathSell(sellEquipList); });
            }
            else
            {
                BathSell(sellEquipList);
            }
        }
    }

    void BathSell(List<int> sellEquipList)
    {
        //UIRootMgr.Instance.IsLoading = true;
        //GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_SellEquip(sellEquipList));
        PlayerPrefsBridge.Instance.onSellEquip(sellEquipList);
        mViewObj.SellPanel.SetActive(false);
        mViewObj.IconSellMark.transform.rotation = Quaternion.Euler(Vector3.zero);

        FreshEquipItem(false);
    }

    void FreshEquipItem(bool reset =true)
    {      
        if (mCurTab == Equip.EquipType.None)
            mViewObj.TextBtnType.text = "全部法宝";
        else
            mViewObj.TextBtnType.text = mCurTab.Value.GetDesc();
        List<Equip> mEquipDataList = PlayerPrefsBridge.Instance.GetEquipAllListNoCopy();
        curEquipList.Clear();
        mCurTypeEquipList.Clear();
        for (int i = 0; i < mEquipDataList.Count; i++)
        {
            if (mEquipDataList[i] != null && mEquipDataList[i].idx != 0 && (mCurTab == Equip.EquipType.None || (int)mEquipDataList[i].type == (int)mCurTab))
            {
                mCurTypeEquipList.Add(mEquipDataList[i]);
                curEquipList.Add(mEquipDataList[i], i);
            }
        }
        if (mCurSelectItemIndex >= mCurTypeEquipList.Count) mCurSelectItemIndex = mCurTypeEquipList.Count-1;
        mCurTypeEquipList.Sort((x, y) =>
        {
            int xWeight = x.curIsEquip ? 20000 : 10000;
            xWeight += x.curQuality * 1000;
            xWeight += x.curLevel;

            int yWeight = y.curIsEquip ? 20000 : 10000;
            yWeight += y.curQuality * 1000;
            yWeight += y.curLevel;

            return yWeight.CompareTo(xWeight);
        });
        Reset();
        mViewObj.Scroller.Init(this, mCurTypeEquipList.Count);

        if (mCurTypeEquipList.Count > 0)
        {
            if (reset)
            {
                BtnEvt_ItemClick(curEquipList[mCurTypeEquipList[0]], 0);
                mViewObj.RightBg.SetActive(true);
                mViewObj.Scroller.Scrollbar.gameObject.SetActive(true);
            }
            else if(mCurSelectItemIndex>=0)
            {
                BtnEvt_ItemClick(curEquipList[mCurTypeEquipList[mCurSelectItemIndex]], mCurSelectItemIndex);
                mViewObj.RightBg.SetActive(true);
                mViewObj.Scroller.Scrollbar.gameObject.SetActive(true);
            }
            mViewObj.TextNull.gameObject.SetActive(false);
        }
        else if (mCurTypeEquipList.Count == 0)
        {
            mViewObj.RightBg.SetActive(false);
            mViewObj.Scroller.Scrollbar.gameObject.SetActive(false);
            mViewObj.TextNull.gameObject.SetActive(true);
            mViewObj.TextNull.text = LobbyDialogue.GetDescStr("desc_getway_equip"); 
        }
        FreshInventoryContent();
    }

    void FreshInventoryContent()
    {        
        int num=0;
        int MaxNum = 80;
        List<Equip> mEquipDataList = PlayerPrefsBridge.Instance.GetEquipAllListNoCopy();
        for (int i = 0; i < mEquipDataList.Count; i++)
        {
            if (mEquipDataList[i] != null && mEquipDataList[i].idx != 0)
                num++;
        }
        if (num > MaxNum)
            mViewObj.TextFrameNum.text = string.Format("储存空间: <color=#FF0000FF>{0}/{1}</color>", num, MaxNum);
        else
            mViewObj.TextFrameNum.text = string.Format("储存空间: {0}/{1}", num, MaxNum);
    }



    //-----------------打开下拉列表
    void BtEvt_ClickTypeTab()
    {
        if (mViewObj.PanelBtn.activeInHierarchy)
        {
            mViewObj.PanelBtn.SetActive(false);
            mViewObj.IconTypeMark.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            if (mViewObj.SellPanel.activeInHierarchy)
                BtnEvt_ClickSellTab();
            if (mCurTab == Equip.EquipType.None)
            {
                mViewObj.ButtonAll.gameObject.SetActive(false);
                for (int i = 0; i < mViewObj.ButtonList.Count; i++)
                {
                    mViewObj.ButtonList[i].gameObject.SetActive(true);
                }
            }
            else
            {
                mViewObj.ButtonAll.gameObject.SetActive(true);
                for (int i = 0; i < mViewObj.ButtonList.Count; i++)
                {
                    mViewObj.ButtonList[i].gameObject.SetActive(i != (int)mCurTab);
                }
            }         
            mViewObj.PanelBtn.SetActive(true);
            mViewObj.IconTypeMark.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
        }     
    }
    void BtnEvt_ClickSellTab()
    {
        if (mViewObj.SellPanel.activeInHierarchy)
        {
            mViewObj.SellPanel.SetActive(false);
            mViewObj.IconSellMark.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            if (mViewObj.PanelBtn.activeInHierarchy) BtEvt_ClickTypeTab();
            mViewObj.SellPanel.SetActive(true);
            mViewObj.IconSellMark.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }


    private int mCurSelectItemIndex = -1;
    public void BtnEvt_ItemClick(int inventoryPos,int index)
    {
        foreach (var temp in mEquipItemList)
        {
            if (null != temp.Value) temp.Value.SelectItem(temp.Value.Index == index);
        }     
        mCurSelectItemIndex = index;
        FreshEquipInof(inventoryPos);
    }

    void FreshEquipInof(int mInventoryPos)
    {
        Equip tempEquip = PlayerPrefsBridge.Instance.GetInventoryEquipByPos(mInventoryPos);
        mViewObj.TextName.text = TUtility.GetTextByQuality(tempEquip.name, tempEquip.curQuality);
        mViewObj.TextEquipType.text = tempEquip.type.GetDesc();
        mViewObj.TextLvDemand.text = string.Format(PlayerPrefsBridge.Instance.PlayerData.Level < tempEquip.curLevel ? "<color=#FF0000FF>{0}级携带</color>" : "{0}级携带", tempEquip.curLevel);
        
        //TODO ;法宝主属性值取第一个
        string descStr = tempEquip.desc;
        descStr += "\r\n\r\n主属性：";
        for (int i = 0; i < tempEquip.curMainAttrType.Length; i++)
        {
            descStr += "\r\n";
            descStr += string.Format("{0}: {1}", tempEquip.curMainAttrType[i].GetDesc(), TUtility.TryGetValOfAttr(tempEquip.curMainAttrType[i], tempEquip.curMainAttrVal[i]));
        }

        descStr += "\r\n\r\n附加属性：";
        for (int i = 0; i < tempEquip.curSubType.Length; i++)
        {
            string at = tempEquip.curSubType[i].GetDesc();
            string av = TUtility.TryGetValOfAttr(tempEquip.curSubType[i], tempEquip.curSubVal[i]);
            descStr += "\r\n";
            descStr += string.Format("{0}: {1}", at, av);
        }
        mViewObj.TextDesc.text = descStr;

        mViewObj.TextPrice.text = "价格:" + tempEquip.sell;

        if (tempEquip.curIsEquip)
        {
            mViewObj.BtnSell.gameObject.SetActive(false);
        }
        else
        {
            mViewObj.BtnSell.gameObject.SetActive(true);
            mViewObj.BtnSell.SetOnAduioClick(delegate() { BtnEvt_SellEquip(mInventoryPos); });
        }
        #region 装配
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
            mViewObj.BtnSell.gameObject.SetActive(false);

        }
        else
        {
            mViewObj.TextBtnChange.text = "装备";
            mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_BtnEquipClick(mInventoryPos, EquipPos); });
            mViewObj.BtnSell.gameObject.SetActive(true);
            mViewObj.BtnSell.SetOnClick(delegate() { BtnEvt_SellEquip(mInventoryPos); });
        }
        #endregion
    }


    public void BtnEvt_BtnUnloadClick(int partIndex)
    {
        //if (PlayerPrefsBridge.Instance.PlayerData.EquipList[partIndex] != (int)Equip.EquipType.None)
        //{
        //    //UIRootMgr.Instance.IsLoading = true;
        //    //GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipEquip((byte)partIndex, (sbyte)Equip.EquipType.None));
        //    PlayerPrefsBridge.Instance.onEquippedEquip(partIndex, -1);
        //    S2C_EquipEquip(partIndex);

        //}
        //else
        //    TDebug.LogError(string.Format("当前位置{0}不存在法宝", partIndex));
    }
    public void BtnEvt_BtnEquipClick(int inventoryPos, int equipPos)
    {
        // Debug.Log("装备仓库第" + inventoryPos + "位法宝to===" + equipPos);
        Equip equip = PlayerPrefsBridge.Instance.GetInventoryEquipByPos(inventoryPos);
        if ((equip == null || equip.curLevel > PlayerPrefsBridge.Instance.PlayerData.Level) && UIRootMgr.Instance.MessageBox.ShowStatus("等级不足，无法装备法宝"))
            return;
        if(PlayerPrefsBridge.Instance.onEquippedEquip(equipPos, inventoryPos))
            S2C_EquipEquip(equipPos);
        //GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EquipEquip((byte)equipPos, (sbyte)inventoryPos));
        //UIRootMgr.Instance.IsLoading = true;
    }

    public void BtnEvt_SellEquip(int inventoryPos)
    {
        Equip equip = PlayerPrefsBridge.Instance.GetInventoryEquipByPos(inventoryPos);
        List<int> sellList = new List<int>();
        sellList.Add(inventoryPos);
        if (equip.curQuality >= 4)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel("紫色以上品质为高级法宝，是否确认出售？", delegate() { BathSell(sellList); });
        }
        else
        {
            BathSell(sellList);
        }
    }
    public void S2C_EquipEquip(int equipPos )
    {
        //UIRootMgr.Instance.IsLoading = false;
        //NetPacket.S2C_EquipEquip msg = MessageBridge.Instance.S2C_EquipEquip(ios);

        //GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
        //SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();
        //int inventoryPos = PlayerPrefsBridge.Instance.PlayerData.EquipList[equipPos];
        //if (inventoryPos == (int)Equip.EquipType.None)//卸下
        //{
        //    mEquipItemList[mCurSelectItemIndex].IconItem.gameObject.SetActive(true);
        //    mEquipItemList[mCurSelectItemIndex].IconEquipMark.gameObject.SetActive(false);
        //    mViewObj.TextBtnChange.text = "装备";
        //    mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_BtnEquipClick(mEquipItemList[mCurSelectItemIndex].InventoryPos, equipPos); });
        //    mViewObj.BtnSell.gameObject.SetActive(true);
        //    mViewObj.BtnSell.SetOnClick(delegate() { BtnEvt_SellEquip(mEquipItemList[mCurSelectItemIndex].InventoryPos); });
        //}
        //else 
        //{
        //    mViewObj.TextBtnChange.text = "卸下";
        //    mViewObj.BtnSell.gameObject.SetActive(false);
        //    mEquipItemList[mCurSelectItemIndex].IconItem.gameObject.SetActive(false);
        //    mEquipItemList[mCurSelectItemIndex].IconEquipMark.gameObject.SetActive(true);
        //    mViewObj.BtnChange.SetOnClick(delegate() { BtnEvt_BtnUnloadClick(equipPos); });
        //    for (int i = 0; i < mEquipItemList.Count; i++)
        //    {
        //        if (mEquipItemList[i].Type == mEquipItemList[mCurSelectItemIndex].Type && i != mCurSelectItemIndex)
        //        {
        //            mEquipItemList[i].IconItem.gameObject.SetActive(true);
        //            mEquipItemList[i].IconEquipMark.gameObject.SetActive(false);
        //        }
        //    }
        //}
    }
    public void S2C_SellEquip(int sellNum)
    {
        UIRootMgr.Instance.IsLoading = false;
        //UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("获得 {0}灵石", msg.Gold),LobbyUIMsgType.Loot);
        FreshEquipItem(false);
    }
   
}
