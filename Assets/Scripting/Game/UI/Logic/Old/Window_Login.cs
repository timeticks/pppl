using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using UnityEngine.UI;

using System.IO;

public class Window_Login :WindowBase
{
  //  private Window_LoginView mView;
    private string mAccount;
    private Estring mPassword ;
    private bool mIsNewer = false;     
    public class ViewObj
    {
        public GameObject LoginRoot;
        public InputField AccountInput;
        public InputField PasswordInput;
        public Button LoginBtn;
        public Button FastLoginBtn;
        public Button RegisterTurnBtn;
        public GameObject RegisterRoot;
        public InputField RegisAccountInput;
        public InputField RegisPasswordInput ;
        public Button RegisterExitBtn ;
        public Button RegisterBtn;
        public ViewObj(UIViewBase view)
        {
            LoginRoot = view.GetCommon<GameObject>("LoginRoot");
            AccountInput = view.GetCommon<InputField>("AccountInput");
            PasswordInput = view.GetCommon<InputField>("PasswordInput");
            LoginBtn = view.GetCommon<Button>("LoginBtn");
            FastLoginBtn = view.GetCommon<Button>("FastLoginBtn");
            RegisterTurnBtn = view.GetCommon<Button>("RegisterTurnBtn");
            RegisterRoot = view.GetCommon<GameObject>("RegisterRoot");
            RegisAccountInput = view.GetCommon<InputField>("RegisAccountInput");
            RegisPasswordInput = view.GetCommon<InputField>("RegisPasswordInput");
            RegisterExitBtn = view.GetCommon<Button>("RegisterExitBtn");
            RegisterBtn = view.GetCommon<Button>("RegisterBtn");
        }
    }
    private ViewObj mViewObj;
    private bool mIsLogin;

    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init();
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    private void Init()
    {
        mAccount    = PlayerPrefs.GetString("AccountID", "");
        mPassword   = PlayerPrefs.GetString("Password", "");
        mViewObj.AccountInput.text = mAccount; //mView.AccountInput.text     = mAccount;
        mViewObj.PasswordInput.text = mPassword;//mView.PasswordInput.text    = mPassword;
        RootTurnTo(true);

        //按钮监听
        mViewObj.RegisterTurnBtn.SetOnClick(delegate() { BtnEvt_Login(); });       //登录
        mViewObj.FastLoginBtn.SetOnClick(delegate() { BtnEvt_FastRegister(); });   //快速登录
        mViewObj.RegisterTurnBtn.SetOnClick(delegate() { RootTurnTo(false); });    //跳转到注册
        mViewObj.RegisterExitBtn.SetOnClick(delegate() { RootTurnTo(true); });     //返回到登录
        mViewObj.RegisterBtn.SetOnClick(delegate() { BtnEvt_Register(); });        //注册
        mViewObj.LoginBtn.SetOnClick(delegate() { BtnEvt_Login(); });              //注册

        
        RegisterNetCodeHandler(NetCode_S.Register, S2C_Register);
        RegisterNetCodeHandler(NetCode_S.Login,      S2C_Login);
        RegisterNetCodeHandler(NetCode_S.FastRegister,  S2C_FastRegister);
        
    }

    public void RootTurnTo(bool isLogin)// 跳转登录/注册界面
    {
        mViewObj.LoginRoot.gameObject.SetActive(isLogin); //mView.LoginRoot.gameObject.SetActive(isLogin);
        mViewObj.RegisterRoot.gameObject.SetActive(!isLogin); //mView.RegisterRoot.gameObject.SetActive(!isLogin);
    }

    void S2C_Register(BinaryReader ios)//BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;

        NetPacket.S2C_Register msg = MessageBridge.Instance.S2C_Register(ios);
        TDebug.Log("注册账号成功,账号:" + mAccount + "密码:" + mPassword);

        UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("注册成功"), Color.green);

        //将登录界面的input，改为注册的账号密码
        mViewObj.AccountInput.text = mAccount;// mView.AccountInput.text     = mAccount;
        mViewObj.PasswordInput.text = mPassword;//mView.PasswordInput.text    = mPassword;
        RootTurnTo(true);
    }


    void S2C_Login(BinaryReader ios)//BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_Login msg = MessageBridge.Instance.S2C_Login(ios);

    }

    void S2C_FastRegister(BinaryReader ios) //快速登录
    {

        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_FastRegister msg = MessageBridge.Instance.S2C_FastRegister(ios);
        mAccount    = msg.Username; 
        mPassword   = msg.Password; 
        PlayerPrefs.SetString("AccountID", mAccount);
        PlayerPrefs.SetString("Password", mPassword);

        //注册成功，跳转到登录界面，并输入注册的账号
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_Login(mAccount, mPassword));
    }


    public void BtnEvt_Register()  //注册
    {
        mAccount = mViewObj.RegisAccountInput.text;// mView.RegisterAccountInput.text;
        mPassword = mViewObj.RegisPasswordInput.text; //mView.RegisterPasswordInput.text;
        if (mAccount == "" || mPassword == "")
        {
            UIRootMgr.Instance.Window_UpTips.InitTips("账号或密码不能为空", Color.red);
            return;
        }

        PlayerPrefs.SetString("AccountID", mAccount);
        PlayerPrefs.SetString("Password", mPassword);
        
        TDebug.Log("开始注册");
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_Register(mAccount, mPassword, "R2", (PlatformType)AppSetting.Platform));
    }

    public void BtnEvt_FastRegister()  //快速注册
    {
        TDebug.Log("快速注册");
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_FastRegister("R2", (PlatformType)AppSetting.Platform));

    }

    void OnGUI()
    {
        //if (GameClient.AppMode != AppModeType.Release)
        //{
        //    if (GUILayout.Button("清理缓存"))
        //    {
        //        PlayerPrefs.DeleteAll();
        //        Caching.CleanCache();
        //    }
        //}
       
    }


    public void BtnEvt_Login()
    {
        UIRootMgr.Instance.IsLoading = true;
        mAccount = mViewObj.AccountInput.text; //mView.AccountInput.text;
        mPassword = mViewObj.PasswordInput.text;// mView.PasswordInput.text;
        
        PlayerPrefs.SetString("AccountID", mAccount);
        PlayerPrefs.SetString("Password", mPassword);
      
        TDebug.Log("登陆游戏|账号:"+mAccount+"密码:"+mPassword);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_Login(mAccount, mPassword));
    }

   
}
