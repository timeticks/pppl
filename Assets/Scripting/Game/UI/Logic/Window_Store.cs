﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Store : WindowBase
{
    public class ViewObj
    {
        public Text TitleText;
        public Button BtnExit;
        public Button MaskBtn;
        public GameObject Part_StoreItem;
        public Transform ItemRoot;
        public ViewObj(UIViewBase view)
        {
            if (TitleText != null) return;
            TitleText = view.GetCommon<Text>("TitleText");
            BtnExit = view.GetCommon<Button>("BtnExit");
            MaskBtn = view.GetCommon<Button>("MaskBtn");
            Part_StoreItem = view.GetCommon<GameObject>("Part_StoreItem");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
        }
    }
    private ViewObj mViewObj;
    public class Part_SmallItem : SmallViewObj
    {
        public Text NameText;
        public Text DescText;
        public TextButton TBtnBuy;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            NameText = view.GetCommon<Text>("NameText");
            DescText = view.GetCommon<Text>("DescText");
            TBtnBuy = view.GetCommon<TextButton>("TBtnBuy");
        }
    }
    public List<Part_SmallItem> mRechargeItemList = new List<Part_SmallItem>();
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init();
    }

    void Init()
    {
        mViewObj.BtnExit.SetOnAduioClick(BtnEvt_Exit);
        mViewObj.MaskBtn.SetOnAduioClick(BtnEvt_Exit);
        Fresh();
    }

    void Fresh()
    {
        List<Commodity> comList = Commodity.CommodityFetcher.GetCommodityListNoCopy(1);
        comList.Sort((x, y) => { return x.order.CompareTo(y.order); });
        TAppUtility.Instance.AddViewInstantiate<Part_SmallItem>(mRechargeItemList, mViewObj.Part_StoreItem,
            mViewObj.ItemRoot, comList.Count);
        for (int i = 0; i < comList.Count; i++)
        {
            if (comList[i].limit > 0)
                mRechargeItemList[i].NameText.text = LangMgr.GetText("{0} (剩余:{1})", comList[i].name,
                    PlayerPrefsBridge.Instance.PlayerData.GetCommodityRemain(comList[i].idx));
            else
                mRechargeItemList[i].NameText.text = comList[i].name;
            mRechargeItemList[i].DescText.text = comList[i].desc;
            mRechargeItemList[i].TBtnBuy.TextBtn.text = LangMgr.GetText("购买");
            int idx = comList[i].idx;
            mRechargeItemList[i].TBtnBuy.SetOnAduioClick(delegate() { BtnEvt_Buy(idx); });
        }
    }

    void BtnEvt_Buy(int commodityIdx)
    {
        System.Action<int> del = delegate(int num)
        {
            BuyCommodity(commodityIdx, num);
        };
        Commodity com = Commodity.CommodityFetcher.GetCommodityByCopy(commodityIdx, false);
        int maxNum = com.batchMaxNum;
        UIRootMgr.Instance.OpenWindow<Window_BuyNumChoose>(WinName.Window_BuyNumChoose, CloseUIEvent.None)
            .OpenWindow(commodityIdx, maxNum, del);
    }

    void BuyCommodity(int commodityIdx, int num)
    {
        Commodity commodity = Commodity.CommodityFetcher.GetCommodityByCopy(commodityIdx, false);
        if (commodity.limit >0 && PlayerPrefsBridge.Instance.PlayerData.GetCommodityRemain(commodityIdx)<num)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LangMgr.GetText("此商品已到限购数量，无法继续购买"), Color.green);
            return;
        }

        if (PlayerPrefsBridge.Instance.onConsume(commodity.sellType, commodity.sellId, commodity.number * num, "买商品") >= 0)
        {
            PlayerPrefsBridge.Instance.onLoot(commodity.lootId, 1);
            PlayerPrefsBridge.Instance.PlayerData.AddCommodity(commodityIdx, num);
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LangMgr.GetText("购买{0}X{1}成功", commodity.name, num), Color.green);
        }
        else
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LangMgr.GetText("没有足够星晶或道具，无法购买此商品"), Color.green);
        }
        Fresh();
    }

    void BtnEvt_Exit()
    {
        CloseWindow();
    }
}
