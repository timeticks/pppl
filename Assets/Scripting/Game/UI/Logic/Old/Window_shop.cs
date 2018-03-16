using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_Shop : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_ItemStoreGoodsBig;
        public Transform RootItem;
        public Button BtnPre;
        public Button BtnNext;
        public Text TextPageNum;
        public GameObject TabBtn;
        public Button BtnTab0;
        public Button BtnTab1;
        public Button BtnTab2;
        public Button BtnTab3;
        public Text TextBtnTab0;
        public Text TextBtnTab1;
        public Text TextBtnTab2;
        public Text TextBtnTab3;
        public Sprite Bg_kuang_04;
        public Sprite Bg_kuang_06;
        public List<Button> BtnTab;
        public List<Text> TextBtnTab;
        public ViewObj(UIViewBase view)
        {
            if (Part_ItemStoreGoodsBig == null) Part_ItemStoreGoodsBig = view.GetCommon<GameObject>("Part_ItemStoreGoodsBig");
            if (BtnPre == null) BtnPre = view.GetCommon<Button>("BtnPre");
            if (TextPageNum == null) TextPageNum = view.GetCommon<Text>("TextPageNum");
            if (BtnNext == null) BtnNext = view.GetCommon<Button>("BtnNext");
            if (RootItem == null) RootItem = view.GetCommon<Transform>("RootItem");
            if (BtnTab0 == null) BtnTab0 = view.GetCommon<Button>("BtnTab0");
            if (BtnTab1 == null) BtnTab1 = view.GetCommon<Button>("BtnTab1");
            if (BtnTab2 == null) BtnTab2 = view.GetCommon<Button>("BtnTab2");
            if (BtnTab3 == null) BtnTab3 = view.GetCommon<Button>("BtnTab3");
            if (TabBtn == null) TabBtn = view.GetCommon<GameObject>("TabBtn");
            if (TextBtnTab0 == null) TextBtnTab0 = view.GetCommon<Text>("TextBtnTab0");
            if (TextBtnTab1 == null) TextBtnTab1 = view.GetCommon<Text>("TextBtnTab1");
            if (TextBtnTab2 == null) TextBtnTab2 = view.GetCommon<Text>("TextBtnTab2");
            if (TextBtnTab3 == null) TextBtnTab3 = view.GetCommon<Text>("TextBtnTab3");
            if (Bg_kuang_04 == null) Bg_kuang_04 = view.GetCommon<Sprite>("Bg_kuang_04");
            if (Bg_kuang_06 == null) Bg_kuang_06 = view.GetCommon<Sprite>("Bg_kuang_06");
            if (BtnTab == null)
            {
                BtnTab = new List<Button>();
                BtnTab.Add(BtnTab0);
                BtnTab.Add(BtnTab1);
                BtnTab.Add(BtnTab2);
                BtnTab.Add(BtnTab3);
            }
            if (TextBtnTab == null)
            {
                TextBtnTab = new List<Text>();
                TextBtnTab.Add(TextBtnTab0);
                TextBtnTab.Add(TextBtnTab1);
                TextBtnTab.Add(TextBtnTab2);
                TextBtnTab.Add(TextBtnTab3);
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
                    BtnTab[i].enabled = i != index;
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
            TextLimit = view.GetCommon<Text>("TextLimit");        }
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
    private List<CommodityObj> mListCommodity = new List<CommodityObj>();

    private int mShopId;//当前大商店
    private int commodityNum = 5;// 一页能放的商品数
    private Shop mCurSubShop= null;//当前子商店
    private Shop.ShopType mCurShopType = Shop.ShopType.Mall;

    private List<Commodity> mCommodityList = new List<Commodity>();
    public void OpenWindow(int shopId,Shop.ShopType type)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Init(shopId, type);
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        mCurSubShop = null;
        mCurShopType = Shop.ShopType.Mall;
        base.CloseWindow(actionType);
    }

    void Init(int shopId, Shop.ShopType type)
    {
        Shop shop = Shop.ShopFetcher.GetShopByCopy(shopId);
        if (shop == null)
        {
            TDebug.LogError(string.Format("获取商店配置失败,ID:{0}", shopId));
            CloseWindow();
            return;
        }
        mShopId = shopId;
        mCurShopType = type;
        mViewObj.BtnPre.SetOnClick(delegate() { BtnEvt_TurnPage(-1); });
        mViewObj.BtnNext.SetOnClick(delegate() { BtnEvt_TurnPage(1); });
        if (type == Shop.ShopType.Mall)
            InitMall(shop);
        else if (type == Shop.ShopType.Mark)
            InitMarket(shop);
        RegisterNetCodeHandler(NetCode_S.BuyProduct, S2C_BuyProduct);
       
    }

    void InitMall(Shop shop)
    {
        mViewObj.TabBtn.SetActive(false);
        InitSubShop(shop.idx,0);
    }

    void InitMarket(Shop shop)
    {
        mViewObj.TabBtn.SetActive(true);

        ///子商店,判断开启条件
        List<int> subShopList = new List<int>();
        string msg;
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
                mViewObj.BtnTab[i].SetOnClick(delegate() { InitSubShop(tempId, tempIdx); });
            }
        }
        for (int i = subShopList.Count; i < mViewObj.BtnTab.Count; i++)
        {
            mViewObj.BtnTab[i].gameObject.SetActive(false);
        }
        if (subShopList.Count > 0) InitSubShop(subShopList[0], 0);
    }


    void InitSubShop(int subShop,int index)
    {
        if (mCurSubShop != null && mCurSubShop.idx == subShop)
            return;
        mCurSubShop = Shop.ShopFetcher.GetShopByCopy(subShop);
        if (mCurSubShop == null)
        {
            TDebug.LogError(string.Format("获取商店配置失败,ID:{0}", subShop));
            return;
        }
        mViewObj.SelectTabBtn(index);
        
        curPage = 1; 
        maxPage = Mathf.Max(1, (mCurSubShop.Commodity.Length + commodityNum-1)/ commodityNum);
        mViewObj.TextPageNum.text = string.Format("{0}/{1}",curPage,maxPage);
        mViewObj.BtnPre.enabled = false;
        mViewObj.BtnNext.enabled = curPage < maxPage;
        TDebug.Log(string.Format("初始化子商店，{0},length:{1},MaxPage:{2}", mCurSubShop.name, mCurSubShop.Commodity.Length, maxPage));
        FreshCommodity(1,true);
    }

    void FreshCommodity(int page,bool reset=false)
    {
        int[] commodity = mCurSubShop.Commodity;
        int itemNum =  Mathf.Min(commodityNum, commodity.Length - (page - 1) * commodityNum);
     //   TDebug.Log(string.Format("Page:{0},itemNum:{1}",page,itemNum));
        mListCommodity = TAppUtility.Instance.AddViewInstantiate<CommodityObj>(mListCommodity, mViewObj.Part_ItemStoreGoodsBig, mViewObj.RootItem, itemNum);

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
            //mCommodityList.Sort((x, y) =>
            //{
            //    if (x.IsUnLock || y.IsUnLock)
            //    {
            //        if (x.IsUnLock && y.IsUnLock)
            //        {
            //            return x.Order.CompareTo(y.Order);
            //        }
            //        return x.IsUnLock ? -1 : 1;
            //    }
            //    else
            //    {
            //        return x.Order.CompareTo(y.Order);
            //    }
            //});
        }
       
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("CommonAtlas");
        SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();
        for (int i = 0; i < itemNum; i++)
        {
            Commodity tempCom = mCommodityList[(page - 1) * commodityNum + i];
            if (tempCom == null)
            {
                TDebug.LogError(string.Format("获取商品失败,"));
                continue;
            }
            CommodityObj obj = mListCommodity[i];
            obj.InitItem(tempCom, commonSprite);

            int tempId = tempCom.idx;
            obj.BtnBuy.SetOnClick(delegate() { BtnEvt_Buy(tempId); });
        }
    }


    void BtnEvt_Buy(int goodsId)
    { 
        Commodity tempCom = Commodity.CommodityFetcher.GetCommodityByCopy(goodsId);
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
        if ((WealthType)tempCom.mSellId == WealthType.Diamond)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(string.Format("确认花费{0}仙玉购买？", tempCom.Number), delegate() { CallBack_BuyProduct(goodsId);});
        }
        else
            CallBack_BuyProduct(goodsId);

      
    }

    void CallBack_BuyProduct(int goodsId)
    {
        UIRootMgr.Instance.IsLoading = true;
        Shop shop = Shop.ShopFetcher.GetShopByCopy(mShopId);
        Commodity tempCom = Commodity.CommodityFetcher.GetCommodityByCopy(goodsId);
        TDebug.Log(string.Format("BtnEvt_Buy==goodsId:{0},{2}==StoreId:{1},{3}", goodsId, mShopId, tempCom.name, shop.name));
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_BuyProduct(goodsId, mShopId));
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
