using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_VIP : WindowBase {

    public class ViewObj
    {
        public Text TextNextFreshTime;
        public Text TextVIPTime;
        public Button BtnBuyVip;
        public Button BtnGetAward;
        public Button BtnMask;

        public ViewObj(UIViewBase view)
        {
            if (TextNextFreshTime == null) TextNextFreshTime = view.GetCommon<Text>("TextNextFreshTime");
            if (TextVIPTime == null) TextVIPTime = view.GetCommon<Text>("TextVIPTime");
            if (BtnBuyVip == null) BtnBuyVip = view.GetCommon<Button>("BtnBuyVip");
            if (BtnGetAward == null) BtnGetAward = view.GetCommon<Button>("BtnGetAward");
            if (BtnMask == null) BtnMask = view.GetCommon<Button>("BtnMask");
        }
    }

    private ViewObj mViewObj;

    private long mNextFreshTime;
    private long mVIPTime;
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init();
    }

    void Init()
    {
        mNextFreshTime = PlayerPrefsBridge.Instance.PlayerData.NextVipDailyDiamond;
        mVIPTime = PlayerPrefsBridge.Instance.PlayerData.VipTime;
        mViewObj.BtnBuyVip.SetOnClick(delegate() { BtnEvt_BuyVIP(); });
        mViewObj.BtnGetAward.SetOnClick(delegate() { BtnEvt_GetAward(); });
        mViewObj.BtnMask.SetOnClick(delegate() { CloseWindow(); });

        RegisterNetCodeHandler(NetCode_S.BuyVIP,S2C_BuyVIP);
        RegisterNetCodeHandler(NetCode_S.GetVIPDailyAward,S2C_GetVIPAward);
    }

    void BtnEvt_BuyVIP()
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_BuyVIP());
    }

    void BtnEvt_GetAward()
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_GetVIPAward());
    }
    void Update()
    {
        FreshTime();
    }
    void FreshTime()
    {
        if (mNextFreshTime > 0)
        {
            long offestTime = mNextFreshTime - AppTimer.CurTimeStampMsSecond;
            if (offestTime < 0) offestTime = 0;
            mViewObj.TextNextFreshTime.text = TUtility.TimeSecondsToDayStr_LCD((int)(offestTime / 1000));
        }
        if (mVIPTime > 0)
        {
            long offestTime = mVIPTime - AppTimer.CurTimeStampMsSecond;
            if (offestTime < 0) offestTime = 0;
            mViewObj.TextVIPTime.text = TUtility.GetStringTime(offestTime / 1000);
        }
    }

    public void S2C_BuyVIP(BinaryReader ios)
    {
        NetPacket.S2C_BuyVIP msg = MessageBridge.Instance.S2C_BuyVIP(ios);
        mVIPTime = PlayerPrefsBridge.Instance.PlayerData.VipTime;
        UIRootMgr.Instance.IsLoading = false;      
    }

    public void S2C_GetVIPAward(BinaryReader ios)
    {
        NetPacket.S2C_GetVIPAward msg = MessageBridge.Instance.S2C_GetVIPAward(ios);
        mNextFreshTime = PlayerPrefsBridge.Instance.PlayerData.NextVipDailyDiamond;
        UIRootMgr.Instance.IsLoading = false;
    }
}
