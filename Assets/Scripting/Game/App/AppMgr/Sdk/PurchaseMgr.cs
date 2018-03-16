using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseMgr : MonoBehaviour
{

    public static PurchaseMgr Instance;
    //internal PurcharseAndroid mPurcharseAndroid;
    //private PurcharseIOS mPurchaserIOS;

    void Awake()
    {
        Instance = this;
    }

    public void InitPurchaser(System.Action<int> initCallback)
    {
        //if (PlatformUtils.EnviormentTy == EnviormentType.Editor || PlatformUtils.EnviormentTy== EnviormentType.Standalone)
        //{
        //    if (initCallback != null) initCallback(1);
        //    return;
        //}
        //else if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        //{
        //    if (mPurcharseAndroid == null) mPurcharseAndroid = gameObject.CheckAddComponent<PurcharseAndroid>();
        //    mPurcharseAndroid.InitPurchaser(initCallback);
        //}
        //else if (PlatformUtils.EnviormentTy == EnviormentType.iOS)
        //{
        //    if (mPurchaserIOS == null) mPurchaserIOS = new PurcharseIOS();
        //    mPurchaserIOS.Init(Recharge.GetProduceIDList(0), initCallback);
        //}
    }

    private System.Action mBuySuccessCallback;
    public void BuyProduct(int rechargeId , string productId, System.Action successCallback=null)
    {
        //Recharge recharge = Recharge.RechargeFetcher.GetRechargeByCopy(rechargeId);
        //if (recharge == null)
        //{
        //    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("充值商品配置错误",Color.black);
        //    return;
        //}
        //if (recharge.Type == Recharge.RechargeType.Month)
        //{
        //    long remainVip = PlayerPrefsBridge.Instance.PlayerData.VipTime - AppTimer.CurTimeStampMsSecond;
        //    int remainDay = Mathf.Max(0, (int)(remainVip / TUtility.ONE_DAY ));
        //    if (remainDay > GameConstUtils.max_can_buy_vip)
        //    {
        //        UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LobbyDialogue.GetDescStr("desc_recharge_month_over"), Color.black);
        //        return;
        //    }
        //}
        //mBuySuccessCallback = successCallback;
        //if (PlatformUtils.EnviormentTy == EnviormentType.Editor || PlatformUtils.EnviormentTy == EnviormentType.Standalone)
        //{
        //    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("尚未开放充值", Color.red);
        //    return;
        //}
        //else if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        //{
        //    if (GameConstUtils.wechat_pay_open)
        //        UIRootMgr.Instance.OpenWindow<Window_AndroidPay>(WinName.Window_AndroidPay, CloseUIEvent.None).OpenWindow(rechargeId, productId);
        //    else
        //        PurchaseMgr.Instance.PurcharseAndroid(rechargeId, productId, PurchaserChannel.AndroidAliPay);
        //}
        //else if (PlatformUtils.EnviormentTy == EnviormentType.iOS)
        //{
        //    mPurchaserIOS.BuyProduct(rechargeId, productId);
        //}
    }

    public void PurcharseAndroid(int rechargeId, string productId, PurchaserChannel payChannel)
    {
        //mPurcharseAndroid.BuyProduct(rechargeId, productId, payChannel);     
    }


    //购买成功
    public void BuySuccess(string productId)
    {
        //List<Recharge> rechargeList = Recharge.RechargeFetcher.GetRechargesByCopy();
        //rechargeList.Add(Recharge.RechargeFetcher.GetMonthRechargeByCopy());
        //string productName = "";
        //for (int i = 0; i < rechargeList.Count; i++)
        //{
        //    if (rechargeList[i]!=null && rechargeList[i].ProductID == productId)
        //    {
        //        productName = rechargeList[i].Name;
        //        break;
        //    }
        //}
        
        //if (LobbySceneMainUIMgr.Instance != null)
        //{
        //    LobbySceneMainUIMgr.Instance.ShowAddWealth(GamePlayer.WealthType.Diamond, PlayerPrefsBridge.Instance.PlayerData.LastRechargeDiamond, LobbyDialogue.GetDescStr("desc_recharge_action" , productName));
        //}
        //if (UIRootMgr.Instance != null)
        //{
        //    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LobbyDialogue.GetDescStr("desc_recharge_success", productName, PlayerPrefsBridge.Instance.PlayerData.LastRechargeDiamond.ToString()), Color.black);
        //    Window_Charge charge = UIRootMgr.Instance.GetOpenListWindow<Window_Charge>(WinName.Window_Charge);
        //    if (charge != null)
        //        charge.Init();
        //}
        //if (mBuySuccessCallback != null) mBuySuccessCallback();
    }
}


public enum PurchaserChannel
{
    None,
    AppStore,        //苹果
    AndroidAliPay,   //支付宝
    AndroidWeChatPay,
}