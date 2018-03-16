using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    public const int MAX_FRAME_RATE = 30;
    protected static GameObject mAppRoot;

    public static LauncherState CurLauncherState {
        get { return mLauncherState; }
    }
    private static LauncherState mLauncherState;

    private bool mAppRootInited;
    private GameObject mInitRoot;
    private static  bool mInitAudio;
    private static bool mInitNotification;

    public enum LauncherState
    {
        Set,
        Logo,
        Init,
        Connect,
        Login,
        Playing,
        Disconnect,
    }



    void Awake()
    {
        mAppRootInited = false;
        mLauncherState = LauncherState.Logo;
    }

    void InitAudio()
    {
        mInitAudio = true;
        Object audio = SharedAsset.Instance.LoadAudioMgr();
        GameObject go = GameObject.Instantiate(audio) as GameObject;
        AudioMgr audioMgr = go.CheckAddComponent<AudioMgr>();
        audioMgr.InitAudioManager();
        audioMgr.IsMusic = PlayerPrefs.GetInt("IsMusic", 1) == 1;
        audioMgr.IsAudio = PlayerPrefs.GetInt("IsAudio", 1) == 1;
        audioMgr.PlayeBg(BgName.AudioBg_Main);
        DontDestroyOnLoad(go);
    }

    void InitAppRoot()
    {
        mAppRootInited = true;
        if (mAppRoot == null)
        {
            mAppRoot = GameObject.Find("AppRoot");
            if (mAppRoot == null)
            {
                mAppRoot = new GameObject("AppRoot");
            }
            GameClient clientApp = mAppRoot.CheckAddComponent<GameClient>();
            clientApp.Init();

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = MAX_FRAME_RATE;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            DontDestroyOnLoad(mAppRoot);
        }
        else
        {
            if (GameClient.Instance != null)
                GameClient.Instance.Init();
        }
        
    }

    void Start()
    {
        if (mInitRoot == null && UIRootMgr.Instance == null)
        {
            UnityEngine.Object initObj = Resources.Load("InitRoot");
            mInitRoot = Instantiate(initObj) as GameObject;
            Window_LoadBar load = mInitRoot.CheckAddComponent<Window_LoadBar>();
            load.ShowVersion();
        }
    }

    public static void AddGameLauncher(GameObject obj)
    {
        obj.CheckAddComponent<GameLauncher>();
    }

    void Update()
    {
        if (mLauncherState == LauncherState.Logo && !mAppRootInited)
        {
            InitAppRoot();
        }
        if (mLauncherState == LauncherState.Logo && AssetsLoader.Instance.LoaderStatus == AssetsLoader.ASSETS_LOADED_OK)
        {
            TDebug.Log("==初始资源完成，开始登入连接");
            OnStateChange(LauncherState.Connect);
        }
    }



    public void OnStateChange(LauncherState state)
    {
        mLauncherState = state;

        switch (mLauncherState)
        {
            case LauncherState.Init:
            {
                break;
            }
            case LauncherState.Connect:
            {
                // android：有通行证缓存则直接连通行证服务器，否则连接游戏服务器
                // IOS： 直接连接通行证服务器

                //#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
                //   GameClient.Instance.ConnectPassport(null); //连接通行证服务器 
                //#else

                //if (Panel_LoginNew.PassportToken != "" || Panel_LoginNew.PassportAccount != "") // 有新账号
                //{
                //    GameClient.Instance.ConnectPassport(null); //连接通行证服务器 
                //}
                //else
                //{
                //    if (PlayerPrefs.GetString("AccountID", "") != "") // 有老账号
                //    {
                //        GameClient.Instance.Connect(null);
                //    }
                //    else
                //    {
                //        GameClient.Instance.ConnectPassport(null); //连接通行证服务器 
                //    }
                //}
                //#endif
              //  GameClient.Instance.Connect(null);

                TDebug.Log("==初始资源完成，开始登入连接");
                if (Window_LoadBar.Instance == null || Window_LoadBar.Instance.IsDestroy)
                {
                    UIRootMgr.Instance.Window_LoadBar.SetInstance();
                }
                //初始完成
                Window_LoadBar.Instance.Fresh(Window_LoadBar.Instance.GetCurPct(), 1f, 1f, "初始完成, 进入游戏", delegate()
                {
                    OnStateChange(LauncherState.Login);
                });
                break;
            }
            case LauncherState.Login://回到登陆界面
            {             
                TDebug.Log("==准备进入登录界面");
                if (mInitRoot != null)
                {
                    Destroy(mInitRoot);
                    mInitRoot = null;
                    UIRootMgr.Instance.Window_LoadBar.SetInstance();
                    UIRootMgr.Instance.Window_LoadBar.SetFalse();
                }
                else
                {
                    UIRootMgr.Instance.Window_LoadBar.SetInstance();
                    Window_LoadBar.Instance.SetFalse();
                }
                TapDB.onStart("n571badtfg2mk9xt", AppSetting.Channel.ToString(), "v" + ServerInfo.Version, false);
                
                TDebug.Log("开始生成基础界面");
                UIRootMgr.Instance.InitMainUI<StartSceneMainUIMgr>(MainUIMgrType.StartSceneMainUIMgr);
                TDebug.Log("==生成基础界面完毕，开始初始音效");
                if (!mInitAudio)
                {
                    InitAudio();
                }
                TDebug.Log("==生成初始音效完毕，开始生成StartSceneMainUIMgr界面");
                if (!mInitNotification)   ///本地消息推送初始化
                {                
                    mInitNotification = true;
                }
                if (UIRootMgr.Instance.CurMgr != MainUIMgrType.StartSceneMainUIMgr)
                {
                    UIRootMgr.Instance.InitMainUI<StartSceneMainUIMgr>(MainUIMgrType.StartSceneMainUIMgr);
                }
                if (UIRootMgr.Instance != null) UIRootMgr.Instance.TopBlackMask = false;
                TDebug.Log("==生成StartSceneMainUIMgr界面完毕，开始创建Window_Login");

                OnStateChange(LauncherState.Playing);
                break;
            }
            case LauncherState.Playing:
            {
                break;
            }
            case LauncherState.Disconnect:
            {
                break;
            }
        }

    }


#region 网络消息回调


#endregion


}
