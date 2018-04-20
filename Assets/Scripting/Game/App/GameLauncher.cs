using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    public const int MAX_FRAME_RATE = 30;
    protected static GameObject mAppRoot;

    public static LaucherState CurLaucherState
    {
        get { return mLaucherState; }
    }
    private static LaucherState mLaucherState;

    private bool mAppRootInited;
    private GameObject mInitRoot;
    private static bool mInitAudio;
    private static bool mInitNotification;

    public enum LaucherState
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
        mLaucherState = LaucherState.Logo;
    }
    public static void AddGameLauncher(GameObject obj)
    {
        obj.CheckAddComponent<GameLauncher>();
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
            mAppRoot = new GameObject("AppRoot");
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

    void Update()
    {
        //if (mLaucherState == LaucherState.Set)
        //{
        //    //AppBridge.InitSetting();
        //    mLaucherState = LaucherState.Logo;
        //    return;
        //}
        if (mLaucherState == LaucherState.Logo && !mAppRootInited)
        {
            InitAppRoot();
        }
        //if (mLaucherState == LaucherState.Logo && !mInitAudio)
        //{
        //    InitAudio();
        //}
        if (mLaucherState == LaucherState.Logo && AssetsLoader.Instance.LoaderStatus == AssetsLoader.ASSETS_LOADED_OK)
        {
            TDebug.Log("==初始资源完成，开始登入连接");
            //AdsMgr.Instance.Init();
            OnStateChange(LaucherState.Connect);
        }
    }

    public void OnStateChange(LaucherState state)
    {
        mLaucherState = state;

        switch (mLaucherState)
        {
            case LaucherState.Init:
                {
                    break;
                }
            case LaucherState.Connect:
                {
                    TDebug.Log("==初始资源完成，开始登入连接");
                    GameClient.Instance.Connect(null);
                    if (Window_LoadBar.Instance == null || Window_LoadBar.Instance.IsDestroy)
                    {
                        UIRootMgr.Instance.Window_LoadBar.SetInstance();
                    }
                    //初始完成
                    Window_LoadBar.Instance.Fresh(Window_LoadBar.Instance.GetCurPct(), 1f, 1f, "初始完成, 进入游戏", delegate()
                    {
                        OnStateChange(LaucherState.Login);
                    });
                    break;
                }
            case LaucherState.Login://回到登陆界面
            {
                GameData.Instance.LoadAllDataBase();
                PlayerPrefsBridge.Instance.LoadPlayer();
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
                    if (UIRootMgr.Instance.CurMgr != MainUIMgrType.LobbySceneMainUIMgr)
                    {
                        UIRootMgr.Instance.InitMainUI<LobbySceneMainUIMgr>(MainUIMgrType.LobbySceneMainUIMgr);
                    }
                    if (UIRootMgr.Instance != null) UIRootMgr.Instance.TopBlackMask = false;
                    TDebug.Log("==生成StartSceneMainUIMgr界面完毕，开始创建Window_Login");
                    UIRootMgr.Instance.OpenWindow<Window_BallBattle>(WinName.Window_BallBattle).OpenWindow(0);
                    OnStateChange(LaucherState.Playing);
                    break;
                }
            case LaucherState.Playing:
                {
                    break;
                }
            case LaucherState.Disconnect:
                {
                    break;
                }
        }

    }


    #region 网络消息回调


    /// <summary>
    /// 注册消息回调
    /// </summary>
    /// <param name="packet"> 服务器消息包 </param>
    void S2C_Register(BinaryReader ios)
    {
        NetPacket.S2C_Register msg = MessageBridge.Instance.S2C_Register(ios);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_Login("Cat1", "123"));
    }


    #endregion


}
