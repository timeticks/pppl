using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_Charge : WindowBase {

    public class ViewObj
    {
        public GameObject Part_ItemCharge;
        public Button BtnMore;
        public Button BtnBuy;
        public Button BtnPagePre;
        public Button BtnPageNext;
        public Text TextPage;
        public Transform Grid;
        public GameObject MonthCharge;
        public ViewObj(UIViewBase view)
        {
            if (Part_ItemCharge == null) Part_ItemCharge = view.GetCommon<GameObject>("Part_ItemCharge");
            if (BtnMore == null) BtnMore = view.GetCommon<Button>("BtnMore");
            if (BtnBuy == null) BtnBuy = view.GetCommon<Button>("BtnBuy");
            if (BtnPagePre == null) BtnPagePre = view.GetCommon<Button>("BtnPagePre");
            if (BtnPageNext == null) BtnPageNext = view.GetCommon<Button>("BtnPageNext");
            if (TextPage == null) TextPage = view.GetCommon<Text>("TextPage");
            if (Grid == null) Grid = view.GetCommon<Transform>("Grid");
            if (MonthCharge == null) MonthCharge = view.GetCommon<GameObject>("MonthCharge");
        }
    }

    public class ChargeItemObj:SmallViewObj
    {
        public Image Icon;
        public GameObject Mark;
        public Text TextMark;
        public Button BtnBuy;

        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            if (Icon == null) Icon = view.GetCommon<Image>("Icon");
            if (Mark == null) Mark = view.GetCommon<GameObject>("Mark");
            if (TextMark == null) TextMark = view.GetCommon<Text>("TextMark");
            if (BtnBuy == null) BtnBuy = view.GetCommon<Button>("BtnBuy");
        }

        public void InitItem()
        {
           //Icon.overrideSprite = ;

        }

    }

    private ViewObj mViewObj;

    private List<ChargeItemObj> mChargeItemList = new List<ChargeItemObj>();

    private List<int> mChargeDataList = new List<int>();

    private long mNextFreshTime;
    private long mVIPTime;
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
    }

    void Init()
    {
        mViewObj.BtnBuy.SetOnClick(delegate() { BtnEvt_BuyVIP(); });
        mViewObj.BtnPagePre.SetOnClick(delegate() { BtnEvt_TurnPage(-1); });
        mViewObj.BtnPageNext.SetOnClick(delegate() { BtnEvt_TurnPage(1); });

        RegisterNetCodeHandler(NetCode_S.BuyVIP, S2C_BuyVIP);
    }

    void FreshChargeItem(int page)
    {
        int itemNum = 4;

        mChargeItemList = TAppUtility.Instance.AddViewInstantiate<ChargeItemObj>(mChargeItemList, mViewObj.Part_ItemCharge, mViewObj.Grid, itemNum);

        //GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("CommonAtlas");
        //SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();
        //for (int i = 0; i < itemNum; i++)
        //{
        //    Commodity tempCom = mCommodityList[(page - 1) * commodityNum + i];
        //    if (tempCom == null)
        //    {
        //        TDebug.LogError(string.Format("获取商品失败,ID:{0}", commodity[i]));
        //        continue;
        //    }
        //    CommodityObj obj = ListCommodity[i];
        //    obj.InitItem(tempCom, commonSprite);

        //    int tempId = tempCom.Idx;
        //    obj.BtnBuy.SetOnClick(delegate() { BtnEvt_Buy(tempId); });

        //}

    }


    void BtnEvt_BuyVIP()
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_BuyVIP());
    }
    public void S2C_BuyVIP(BinaryReader ios)
    {
        NetPacket.S2C_BuyVIP msg = MessageBridge.Instance.S2C_BuyVIP(ios);
        UIRootMgr.Instance.IsLoading = false;
    }

    #region 翻页
    int curPage = 1;
    int maxPage = 1;
    void BtnEvt_TurnPage(int num)
    {
        curPage += num;
        curPage = Mathf.Clamp(curPage, 1, maxPage);
        mViewObj.BtnPagePre.enabled = curPage > 1;
        mViewObj.BtnPageNext.enabled = curPage < maxPage;
        mViewObj.TextPage.text = string.Format("{0}/{1}", curPage, maxPage);
       // FreshCommodity(curPage);
    }
    #endregion
}
