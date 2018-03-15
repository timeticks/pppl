using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

// 帐户类型
public enum TGTUserType{
	TGTTypeAnonymous = 0, // 匿名用户
	TGTTypeRegistered = 1,// 注册用户
}

// 用户性别
public enum TGTUserSex{
	TGTSexMale = 0, // 男性
	TGTSexFemale = 1, // 女性
	TGTSexUnknown = 2, // 性别未知
}

public class TapDB
{
#if UNITY_IOS
    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeOnStart(string appId, string channel, string version);
	
    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeSetUser(string userId, int userType, int userSex, int userAge, string userName);
	
    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeSetLevel(int level);
	
    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeSetServer(string server);
	
    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeOnChargeRequest(string orderId, string product, Int32 amount, string currencyType, Int32 virtualCurrencyAmount, string payment);
	
    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeOnChargeSuccess(string orderId);
	
    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeOnChargeFail(string orderId, string reason);

    //[DllImport ("__Internal")]
    //public static extern void TapDB_nativeOnChargeOnlySuccess(string orderId, string product, Int32 amount, string currencyType, Int32 virtualCurrencyAmount, string payment);

#elif UNITY_ANDROID
	public static string JAVA_CLASS = "com.xindong.tyrantdb.TyrantdbGameTracker";
	private static string UNTIFY_CLASS = "com.unity3d.player.UnityPlayer";
	private static AndroidJavaClass agent = null;
	private static AndroidJavaClass unityClass = null;

	private static AndroidJavaClass getAgent() {
		if (agent == null) {
			agent = new AndroidJavaClass(JAVA_CLASS);
		}
		return agent;
	}

	private static AndroidJavaClass getUnityClass(){
		if (unityClass == null) {
			unityClass = new AndroidJavaClass(UNTIFY_CLASS);
		}
		return unityClass;
	}

	private static void TapDB_nativeInit(string appId, string channel, string gameVersion, bool requestPermission){

		AndroidJavaObject activity = getUnityClass().GetStatic<AndroidJavaObject>("currentActivity");
		getAgent().CallStatic("init", activity, appId, channel, gameVersion, requestPermission);
	}
	
	private static void TapDB_nativeOnResume(){
		AndroidJavaObject activity = getUnityClass().GetStatic<AndroidJavaObject>("currentActivity");
		getAgent().CallStatic("onResume", activity);
	}
	
	private static void TapDB_nativeOnStop(){
		AndroidJavaObject activity = getUnityClass().GetStatic<AndroidJavaObject>("currentActivity");
		getAgent().CallStatic("onStop", activity);
	}

#endif

    private static bool IsInited = false;
	/**
	 * 初始化，尽早调用
	 * appId: TapDB注册得到的appId
	 * channel: 分包渠道名称，可为空   ServerId+渠道名
	 * gameVersion: 游戏版本，可为空，为空时，自动获取游戏安装包的版本
	 * requestPermission: Android上是否由TapDB SDK来申请可选权限，具体内容参见对接文档
	 */
	public static void onStart(string appId, string channel, string gameVersion, bool requestPermission=false)
    {
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        return;
#endif
#if UNITY_IOS
        //TapDB_nativeOnStart(appId, channel, gameVersion);
#elif UNITY_ANDROID
		TapDB_nativeInit(appId, channel, gameVersion, requestPermission);
		TapDB_nativeOnResume();
#endif
	    IsInited = true;
	}

	public static void onResume(){
        if (!IsInited) return;

#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        return;
#endif
#if UNITY_ANDROID
		TapDB_nativeOnResume();
#endif
	}

	public static void onStop(){
        if (!IsInited) return;

#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        return;
#endif
#if UNITY_ANDROID
		TapDB_nativeOnStop();
#endif
	}

	/**
	 * 记录一个用户（注意是平台用户，不是游戏角色！！！！），需要保证唯一性
	 * userId: 用户的ID（注意是平台用户ID，不是游戏角色ID！！！！），如果是匿名用户，由游戏生成，需要保证不同平台用户的唯一性--【PassportId】
	 * userType: 用户类型
	 * userSex: 用户性别
	 * userAge: 用户年龄，年龄未知传递0
	 */
	public static void setUser(string userId, TGTUserType userType, TGTUserSex userSex, int userAge, string userName)
    {
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        return;
#endif

#if UNITY_IOS
        //TapDB_nativeSetUser(userId, (int)userType, (int)userSex, userAge, userName);
#elif UNITY_ANDROID
		AndroidJavaClass enumClassUserType = new AndroidJavaClass("com.xindong.tyrantdb.TyrantdbGameTracker$TGTUserType");
		AndroidJavaObject userType_obj = enumClassUserType.CallStatic<AndroidJavaObject>("valueOf", userType.ToString());

		AndroidJavaClass enumClassUserSex = new AndroidJavaClass("com.xindong.tyrantdb.TyrantdbGameTracker$TGTUserSex");
		AndroidJavaObject userSex_obj = enumClassUserSex.CallStatic<AndroidJavaObject>("valueOf", userSex.ToString());

		getAgent().CallStatic("setUser", userId, userType_obj, userSex_obj, userAge, userName);
#endif	
	}

	/**
	 * 设置用户等级，初次设置时或升级时调用
	 * level: 等级
	 */
	public static void setLevel(int level){
#if UNITY_IOS
        //TapDB_nativeSetLevel(level);
#elif UNITY_ANDROID
		getAgent().CallStatic("setLevel", level);
#endif	
	}

	/**
	 * 设置用户服务器，初次设置或更改服务器的时候调用
	 * server: 服务器
	 */
	public static void setServer(string server){
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        return;
#endif
#if UNITY_IOS
        //TapDB_nativeSetServer(server);
#elif UNITY_ANDROID
		getAgent().CallStatic("setServer", server);
#endif	
	}

	/**
	 * 发起充值请求时调用
	 * orderId: 订单ID，不能为空
	 * product: 产品名称，可为空
	 * amount: 充值金额（分）
	 * currencyType: 货币类型，可为空，参考：人民币 CNY，美元 USD；欧元 EUR
	 * virtualCurrencyAmount: 充值获得的虚拟币
	 * payment: 支付方式，可为空，如：支付宝
	 */
	public static void onChargeRequest(string orderId, string product, Int32 amount, string currencyType, Int32 virtualCurrencyAmount, string payment){
#if UNITY_IOS
        //TapDB_nativeOnChargeRequest(orderId, product, amount, currencyType, virtualCurrencyAmount, payment);
#elif UNITY_ANDROID
		getAgent().CallStatic("onChargeRequest", orderId, product, (long)amount, currencyType, (long)virtualCurrencyAmount, payment);
#endif	
	}

	/**
	 * 充值成功时调用
	 * orderId: 订单ID，不能为空，与上一个接口的orderId对应
	 */
	public static void onChargeSuccess(string orderId){
#if UNITY_IOS
        //TapDB_nativeOnChargeSuccess(orderId);
#elif UNITY_ANDROID
		getAgent().CallStatic("onChargeSuccess", orderId);
#endif	
	}

	/**
	 * 充值失败时调用
	 * orderId: 订单ID，不能为空，与上一个接口的orderId对应
	 * reason: 失败原因，可为空
	 */
	public static void onChargeFail(string orderId, string reason){
#if UNITY_IOS
        //TapDB_nativeOnChargeFail(orderId, reason);
#elif UNITY_ANDROID
		getAgent().CallStatic("onChargeFail", orderId, reason);
#endif	
	}

	/**
	 * 当客户端无法跟踪充值请求发起，只能跟踪到充值成功的事件时，调用该接口记录充值信息
	 * orderId: 订单ID，可为空
	 * product: 产品名称，可为空
	 * amount: 充值金额（单位分，即无论什么币种，都需要乘以100）
	 * currencyType: 货币类型，可为空，参考：人民币 CNY，美元 USD；欧元 EUR
	 * virtualCurrencyAmount: 充值获得的虚拟币
	 * payment: 支付方式，可为空，如：支付宝
	 */
	public static void onChargeOnlySuccess(string orderId, string product, Int32 amount, string currencyType, Int32 virtualCurrencyAmount, string payment){
#if UNITY_IOS
        //TapDB_nativeOnChargeOnlySuccess(orderId, product, amount, currencyType, virtualCurrencyAmount, payment);
#elif UNITY_ANDROID
		getAgent().CallStatic("onChargeOnlySuccess", orderId, product, (long)amount, currencyType, (long)virtualCurrencyAmount, payment);
#endif	
	}

}


