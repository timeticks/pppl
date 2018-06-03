using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Recharge : WindowBase
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
        List<Recharge> rechargeList = Recharge.Fetcher.GetRechargeCopyAll();
        rechargeList.Sort((x, y) => { return x.order.CompareTo(y.order); });
        TAppUtility.Instance.AddViewInstantiate<Part_SmallItem>(mRechargeItemList, mViewObj.Part_StoreItem,
            mViewObj.ItemRoot, rechargeList.Count);
        for (int i = 0; i < rechargeList.Count; i++)
        {
            if (rechargeList[i].limit > 0)
            {
                mRechargeItemList[i].NameText.text = LangMgr.GetText("{0} (剩余:{1})", rechargeList[i].name,
                    PlayerPrefsBridge.Instance.PlayerData.GetRechargeRemain(rechargeList[i].idx));
            }
            else
                mRechargeItemList[i].NameText.text = rechargeList[i].name;
            mRechargeItemList[i].DescText.text = rechargeList[i].desc;
            mRechargeItemList[i].TBtnBuy.TextBtn.text = LangMgr.GetText("{0} 元",rechargeList[i].price);
            int idx = rechargeList[i].idx;
            mRechargeItemList[i].TBtnBuy.SetOnAduioClick(delegate() { BtnEvt_Buy(idx); });
        }
    }

    void BtnEvt_Buy(int rechargeIdx)
    {
        Recharge recharge = Recharge.Fetcher.GetRechargeCopy(rechargeIdx, false);
        if (recharge.limit > 0 &&
            PlayerPrefsBridge.Instance.PlayerData.BuyRechargeDict.ContainsKey(rechargeIdx) &&
            PlayerPrefsBridge.Instance.PlayerData.BuyRechargeDict[rechargeIdx] >= recharge.limit)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LangMgr.GetText("此商品已到限购数量，无法继续购买"), Color.green);
            return;
        }
        BuySuccess(rechargeIdx);
    }

    public void BuySuccess(int rechargeIdx)
    {
        Recharge recharge = Recharge.Fetcher.GetRechargeCopy(rechargeIdx, true);
        PlayerPrefsBridge.Instance.addDiamond(recharge.diamond + recharge.diamondAdd, "充值");
        PlayerPrefsBridge.Instance.onLoot(recharge.loot, 1);
        PlayerPrefsBridge.Instance.PlayerData.AddRecharge(rechargeIdx);
        UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LangMgr.GetText("购买{0}成功", recharge.name), Color.green);
        Fresh();
    }


    void BtnEvt_Exit()
    {
        CloseWindow();
    }
}
