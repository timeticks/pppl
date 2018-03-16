using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_PrestigeStore : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_ItemStoreGoods;
        public Text TextStoreTitle;
        public Text TextPrestigeLevel;
        public Button BtnObtainPrestige;
        public Button BtnPre;
        public Text TextPageNum;
        public Button BtnNext;
        public Transform RootItem;
        public Button BtnTab1;
        public Button BtnTab2;
        public Button BtnTab0;
        public Text TextBtnTab1;
        public Text TextBtnTab2;
        public Text TextBtnTab0;
        public Button BtnMask;
        public Sprite Bg_kuang_04;
        public Sprite Bg_kuang_06;
        public List<Button> BtnTab;
        public List<Text> TextBtnTab;
        public ViewObj(UIViewBase view)
        {
            if (Part_ItemStoreGoods == null) Part_ItemStoreGoods = view.GetCommon<GameObject>("Part_ItemStoreGoods");
            if (TextStoreTitle == null) TextStoreTitle = view.GetCommon<Text>("TextStoreTitle");
            if (TextPrestigeLevel == null) TextPrestigeLevel = view.GetCommon<Text>("TextPrestigeLevel");
            if (BtnObtainPrestige == null) BtnObtainPrestige = view.GetCommon<Button>("BtnObtainPrestige");
            if (BtnPre == null) BtnPre = view.GetCommon<Button>("BtnPre");
            if (TextPageNum == null) TextPageNum = view.GetCommon<Text>("TextPageNum");
            if (BtnNext == null) BtnNext = view.GetCommon<Button>("BtnNext");
            if (RootItem == null) RootItem = view.GetCommon<Transform>("RootItem");
            if (BtnTab1 == null) BtnTab1 = view.GetCommon<Button>("BtnTab1");
            if (BtnTab2 == null) BtnTab2 = view.GetCommon<Button>("BtnTab2");
            if (BtnTab0 == null) BtnTab0 = view.GetCommon<Button>("BtnTab0");
            if (TextBtnTab1 == null) TextBtnTab1 = view.GetCommon<Text>("TextBtnTab1");
            if (TextBtnTab2 == null) TextBtnTab2 = view.GetCommon<Text>("TextBtnTab2");
            if (TextBtnTab0 == null) TextBtnTab0 = view.GetCommon<Text>("TextBtnTab0");
            if (BtnMask == null) BtnMask = view.GetCommon<Button>("BtnMask");
            if (Bg_kuang_04 == null) Bg_kuang_04 = view.GetCommon<Sprite>("Bg_kuang_04");
            if (Bg_kuang_06 == null) Bg_kuang_06 = view.GetCommon<Sprite>("Bg_kuang_06");
            if (BtnTab == null)
            {
                BtnTab = new List<Button>();
                BtnTab.Add(BtnTab0);
                BtnTab.Add(BtnTab1);
                BtnTab.Add(BtnTab2);
            }
            if (TextBtnTab == null)
            {
                TextBtnTab = new List<Text>();
                TextBtnTab.Add(TextBtnTab0);
                TextBtnTab.Add(TextBtnTab1);
                TextBtnTab.Add(TextBtnTab2);
            }
        }
        public void SelectTabBtn(int index)
        {
            for (int i = 0, length = BtnTab.Count; i < length; i++)
            {
                if (BtnTab[i].gameObject.activeInHierarchy)
                {
                    BtnTab[i].image.sprite = i == index ? Bg_kuang_06 : Bg_kuang_04;
                    BtnTab[i].image.overrideSprite = i == index ? Bg_kuang_06 : Bg_kuang_04;
                }
            }
        }
    }
    public class CommodityObj :SmallViewObj
    {
        public Button BtnBuy;
        public Image IconWealth;
        public Text TextCostNum;
        public Text TextName;
        public Text TextLimitNum;
        public Text TextDesc;
        public GameObject LimitMask;
        public Text TextLimit;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            BtnBuy = view.GetCommon<Button>("BtnBuy");
            IconWealth = view.GetCommon<Image>("IconWealth");
            TextCostNum = view.GetCommon<Text>("TextCostNum");
            TextName = view.GetCommon<Text>("TextName");
            TextLimitNum = view.GetCommon<Text>("TextLimitNum");
            TextDesc = view.GetCommon<Text>("TextDesc");
            LimitMask = view.GetCommon<GameObject>("LimitMask");
            TextLimit = view.GetCommon<Text>("TextLimit");
        }
        public void InitItem(Commodity com, SpritePrefab commonSprite)
        {
            this.TextName.text = com.name;
            // 开放限制
            string openCon = "";
            if (Commodity.GetCommodityOpen(com, out openCon))
            {
                this.TextName.color = new Color(255 / 255f, 205 / 255f, 137 / 255f);
                this.LimitMask.SetActive(false);
                this.TextDesc.gameObject.SetActive(true);
                this.BtnBuy.gameObject.SetActive(true);
            }
            else
            {
                this.TextName.color = new Color(143 / 255f, 141 / 255f, 139 / 255f);
                this.LimitMask.SetActive(true);
                this.TextLimit.text = openCon;
                this.TextLimitNum.gameObject.SetActive(false);
                this.TextDesc.gameObject.SetActive(false);
                this.BtnBuy.gameObject.SetActive(false);
                return;
            }
            /// 限购
            if (com.DayNumber > 0)//每日限购
            {
                int dayNum = PlayerPrefsBridge.Instance.GetShopInofDayNum(com.idx);
                this.TextLimitNum.gameObject.SetActive(true);
                this.TextLimitNum.text = string.Format("限购:{0}", Mathf.Max(0, com.DayNumber - dayNum));
            }
            else if (com.HisNumber > 0) //总限购
            {
                int hisNum = PlayerPrefsBridge.Instance.GetShopInofHisNum(com.idx);
                int remainNum = com.HisNumber - hisNum;
                if (com.HisNumber - hisNum <= 0)
                {
                    this.TextName.color = new Color(143 / 255f, 141 / 255f, 139 / 255f);
                    this.LimitMask.SetActive(true);
                    this.TextLimit.text = "已售罄";
                    this.TextLimitNum.gameObject.SetActive(false);
                    this.TextDesc.gameObject.SetActive(false);
                    this.BtnBuy.gameObject.SetActive(false);
                    return;
                }
                this.TextLimitNum.gameObject.SetActive(true);
                this.TextLimitNum.text = string.Format("剩余:{0}", Mathf.Max(0, remainNum));            
            }
            else
                this.TextLimitNum.gameObject.SetActive(false);
            this.TextDesc.text = com.Desc;
            this.TextCostNum.text = com.Number.ToString();
            WealthType wealth = (WealthType)com.SellId;
            this.IconWealth.overrideSprite = commonSprite.GetSprite(TUtility.GetWealthIcon((WealthType)com.mSellId));
           
        }
    }

    private ViewObj mViewObj;
    private List<CommodityObj> ListCommodity = new List<CommodityObj>();

    private int StoreId;//当前大商店
    private int commodityNum = 5;// 一页能放的商品数
    private Shop curSubShop= null;//当前子商店

    private List<Commodity> mCommodityList = new List<Commodity>();
    public void OpenWindow(int storeId)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Init(storeId);
        mViewObj.BtnMask.SetOnClick(delegate() { CloseWindow(CloseActionType.OpenHide); });
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        curPage = 1;
        curSubShop = null;
        base.CloseWindow(actionType);
    }
    void Init(int storeId)
    {
        Shop shop = Shop.ShopFetcher.GetShopByCopy(storeId);
        if (shop == null)
        {
            TDebug.LogError(string.Format("获取商店配置失败,ID:{0}", storeId));
            CloseWindow(CloseActionType.OpenHide);
            return;
        }
        StoreId = storeId;

        GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
        mViewObj.TextStoreTitle.text = shop.name;
        mViewObj.TextPrestigeLevel.text = string.Format("声望：{0}", player.GetPrestige(shop.PrestigeType).name);
        mViewObj.BtnPre.SetOnClick(delegate() { BtnEvt_TurnPage(-1); });
        mViewObj.BtnNext.SetOnClick(delegate() { BtnEvt_TurnPage(1); });
        mViewObj.BtnObtainPrestige.SetOnClick(delegate() { BtnEvt_Obtain(); });
        ///子商店,判断开启条件
        List<int> subShopList= new List<int>();
        string msg = "";
        for (int i = 0, length = shop.SubShop.Length; i < length; i++)
        {
            if (Shop.GetShopOpen(shop.SubShop[i],out msg))
                subShopList.Add(shop.SubShop[i]);
        }
        Shop subShop;
        for (int i = 0, length = subShopList.Count; i < length; i++)
        {
            if (i < mViewObj.BtnTab.Count)
            {
                subShop = Shop.ShopFetcher.GetShopByCopy(subShopList[i]);
                mViewObj.BtnTab[i].gameObject.SetActive(true);
                mViewObj.TextBtnTab[i].text = subShop.name;
                int tempId = subShop.idx;
                int tempIdx = i;
                mViewObj.BtnTab[i].SetOnClick(delegate() { BtnEvt_OpenSubShop(tempId, tempIdx); });
            }
        }
        for (int i = subShopList.Count; i < mViewObj.BtnTab.Count; i++)
        {
            mViewObj.BtnTab[i].gameObject.SetActive(false);
        }
        RegisterNetCodeHandler(NetCode_S.BuyProduct, S2C_BuyProduct);
        if (subShopList.Count > 0) BtnEvt_OpenSubShop(subShopList[0], 0);
    }

    void BtnEvt_Obtain()
    {
        //UIRootMgr.Instance.OpenWindow<WindowBig_Activity>(WinName.WindowBig_Activity,CloseUIEvent.CloseAll).OpenWindow(WindowBig_Activity.ChildTab.PrestigeTask);
    }


    void BtnEvt_OpenSubShop(int subShop,int index)
    {
        if (curSubShop != null && curSubShop.idx == subShop)
            return;
        curSubShop = Shop.ShopFetcher.GetShopByCopy(subShop);
        if (curSubShop == null)
        {
            TDebug.LogError(string.Format("获取商店配置失败,ID:{0}", subShop));
            return;
        }
        mViewObj.SelectTabBtn(index);

        curPage = 1;
        maxPage = Mathf.Max(1, (curSubShop.Commodity.Length + commodityNum - 1) / commodityNum);
        mViewObj.TextPageNum.text = string.Format("{0}/{1}", curPage, maxPage);
        mViewObj.BtnPre.enabled = false;
        mViewObj.BtnNext.enabled = curPage < maxPage;
        TDebug.Log(string.Format("初始化子商店，{0},length:{1},MaxPage:{2}", curSubShop.name, curSubShop.Commodity.Length, maxPage));
        FreshCommodity(1, true);
    }

    void FreshCommodity(int page,bool reset=false)
    {       
        int[] commodity = curSubShop.Commodity;
        int itemNum =  Mathf.Min(commodityNum, commodity.Length - (page - 1) * commodityNum);
        TDebug.Log(string.Format("Page:{0},itemNum:{1}", page, itemNum));
        ListCommodity = TAppUtility.Instance.AddViewInstantiate<CommodityObj>(ListCommodity, mViewObj.Part_ItemStoreGoods, mViewObj.RootItem, itemNum);

        if (reset)
        {
            mCommodityList.Clear();
            string conStr = "";
            for (int i = 0, length = commodity.Length; i < length; i++)
            {
                Commodity tempCom = Commodity.CommodityFetcher.GetCommodityByCopy(commodity[i]);
                tempCom.IsUnLock = Commodity.GetCommodityOpen(tempCom, out conStr);
                if (tempCom.HisNumber > 0)
                {
                    int hisNum = PlayerPrefsBridge.Instance.GetShopInofHisNum(tempCom.idx);
                    tempCom.IsSoldOut = (tempCom.HisNumber - hisNum) <= 0;
                }
                mCommodityList.Add(tempCom);
            }      
            mCommodityList.Sort((x, y) =>
            {
                if (x.IsSoldOut || y.IsSoldOut)
                {
                    if (x.IsSoldOut && y.IsSoldOut)
                    {
                        return x.Order.CompareTo(y.Order);
                    }         
                    return x.IsSoldOut ? 1 : -1;
                }
                else
                {
                    if (x.IsUnLock || y.IsUnLock)
                    {
                        if (x.IsUnLock && y.IsUnLock)
                        {
                            return x.Order.CompareTo(y.Order);
                        }
                        return x.IsUnLock ? -1 : 1;
                    }
                    return x.Order.CompareTo(y.Order);
                }
            }); 
        }

        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("CommonAtlas");
        SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();
        for (int i = 0; i < itemNum; i++)
        {
            Commodity tempCom = mCommodityList[(page - 1) * commodityNum + i];
            if (tempCom == null)
            {
                TDebug.LogError(string.Format("获取商品失败,ID:{0}", commodity[i]));
                continue;
            }
            CommodityObj obj = ListCommodity[i];
            obj.InitItem(tempCom, commonSprite);
        
            int tempId = tempCom.idx;
            obj.BtnBuy.SetOnClick(delegate() { BtnEvt_Buy(tempId); });

        }
    }


    void BtnEvt_Buy(int goodsId)
    {
        Commodity tempCom = Commodity.CommodityFetcher.GetCommodityByCopy(goodsId);
        if (tempCom.MySect != Sect.SectType.ZhongLi
            && tempCom.MySect != PlayerPrefsBridge.Instance.PlayerData.MySect 
            && UIRootMgr.Instance.MessageBox.ShowStatus(string.Format("该商品只能{0}弟子购买", Sect.GetSectName(tempCom.MySect))))          
            return;
        if ((WealthType)tempCom.mSellId == WealthType.Diamond)
        {
            if (PlayerPrefsBridge.Instance.PlayerData.Diamond < tempCom.Number && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_ZHUAN_SHI_BU_ZU))
                return;
        }
        else if ((WealthType)tempCom.mSellId == WealthType.Gold)
        {
            if (PlayerPrefsBridge.Instance.PlayerData.Gold < tempCom.Number && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_JIN_BI_BU_ZU))
                return;
        }
        UIRootMgr.Instance.IsLoading = true;               
        Shop shop = Shop.ShopFetcher.GetShopByCopy(StoreId);
        TDebug.Log(string.Format("BtnEvt_Buy==goodsId:{0},{2}==StoreId:{1},{3}", goodsId, StoreId, tempCom.name, shop.name));
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_BuyProduct(goodsId, StoreId));
    }

    void S2C_BuyProduct(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_BuyProduct msg = MessageBridge.Instance.S2C_BuyProduct(ios);
        UIRootMgr.LobbyUI.ShowDropInfo(msg.TranslateList);
        FreshCommodity(curPage);
    }

    #region 翻页
    int curPage=1;
    int maxPage=1;
    void BtnEvt_TurnPage(int num)
    {
        curPage += num;
        curPage = Mathf.Clamp(curPage, 1, maxPage);
        mViewObj.BtnPre.enabled = curPage > 1;
        mViewObj.BtnNext.enabled = curPage < maxPage;
        mViewObj.TextPageNum.text = string.Format("{0}/{1}", curPage, maxPage);
        FreshCommodity(curPage);
    }
    #endregion
   


}
