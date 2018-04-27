using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System;
public class GameClient : MonoBehaviour 
{
    public static GameClient Instance { get; protected set; }
    public static bool IsShowWarn = true;   //客户端是否拦截异常
    public static AppModeType AppMode;

    private static LoadDataFromHTTP     mWeb;
    private static LoggerHelper         mLoggerHelper;
    private static AssetsLoader         mAssetsLoader;
    private static PlayerPrefsBridge    mPlayerPrefsBridge;
    private static SSocket              mSSocket;
    private static ServerStatusHandle         mStatusHandler;

    private static  Dictionary<short, ServPacketHander> mServHandlers = new Dictionary<short, ServPacketHander>();

    private int     mReConnectCount;
    private long    mSnapServerTime;


    protected virtual void Update()
    {
        if (mSSocket == null) return;

        if (mSSocket.Stage == SSocket.SStage.Disconnect)
        {
            Disconnect(true);
        }
        else if (mSSocket.Stage == SSocket.SStage.Connecting && mSSocket.IsTimeOut)
        {
            //连接超时
            if (mReConnectCount < 2)
            {
                TDebug.LogError("第" + mReConnectCount + "次连接服务器失败,ip:" + mSSocket.Ip + ":" + mSSocket.Port);
                mReConnectCount++;
                mSSocket.TryReConnect();
            }
            else
            {
                Disconnect(true);
                //TODO  mReConnectCount = 0;
                mSSocket.Stage = SSocket.SStage.ConnectFailed;
                mReConnectCount = 0;
                TDebug.LogError("连接服务器失败,尝试次数过多,ip:" + mSSocket.Ip + ":" + mSSocket.Port);
            }
        }
        else if (mSSocket.Stage == SSocket.SStage.Connected)
        {
            if (!IsConnected)// && BackgroundTime == 0
            {//检测断线了
                Connect();
                return;
            }

            if (mSSocket.InQueue.Count > 0)
            {
                BinaryReader ios = mSSocket.InQueue.Dequeue();
                OnMessageArrived(ios);
            }


            if (mSnapServerTime < TimeUtils.CurrentTimeMillis)
            {
                mSnapServerTime = TimeUtils.CurrentTimeMillis + 60000;//1分钟同步一次
                SendMessage(MessageBridge.Instance.C2S_SnapshotTime());
            }
        }

    }


    public void Init()
    {
        Instance            = gameObject.GetComponent<GameClient>();
        mWeb                = gameObject.CheckAddComponent<LoadDataFromHTTP>();
        mLoggerHelper       = gameObject.CheckAddComponent<LoggerHelper>();
        mPlayerPrefsBridge  = PlayerPrefsBridge.CreateInstance();
        mSSocket = new SSocket();
        mSSocket.OnConnectedDelegate= OnConnected;
        mSSocket.OnClosedDelegate   = OnDisconnect;

        mStatusHandler = new ServerStatusHandle();
        gameObject.CheckAddComponent<AndirDebug>();
        gameObject.CheckAddComponent<GameAssetsPool>();
        gameObject.CheckAddComponent<FingerGestureUtility>();
        gameObject.CheckAddComponent<GameData>();
        gameObject.CheckAddComponent<TAppUtility>();
        gameObject.CheckAddComponent<SharedAsset>();
        //gameObject.CheckAddComponent<AppLuaMgr>();
        gameObject.CheckAddComponent<TDebug>();

        

        mAssetsLoader = AssetsLoader.Instance;
        mAssetsLoader.Init();

        gameObject.CheckAddComponent<AppBridge>().Init();
        TDebug.InEditor = PlatformUtils.EnviormentTy == EnviormentType.Editor;
        ///初始化默认设置系统时间为服务器时间
        if(AppTimer.CurTimeStampMsSecond ==0)
            AppTimer.SetCurStamp((TUtility.DateTimeToStamp(DateTime.Now)));
    }

    public void LoginOutGame()//返回登录
    {
        TDebug.Log("登出游戏，返回登录");
        UIRootMgr.Instance.IsLoading = false;
        UIRootMgr.Instance.CloseAllOpenWin();
        GameClient.Instance.Disconnect(false);
        AppBridge.Instance.LoadSceneAsync(SceneType.StartScene);
    }

    public void InitUIRoot()
    {
        if (UIRootMgr.Instance == null) //加载出基础界面，需要AppSetting已经加载完毕
        {
            TDebug.Log("开始生成UIRoot");
            UnityEngine.Object uiRootObj = SharedAsset.Instance.LoadAssetSyncObj_ImmediateRelease(BundleType.StartBundle, "UIRoot");
            GameObject uiRoot = Instantiate(uiRootObj) as GameObject;
            uiRoot.CheckAddComponent<UIRootMgr>().AwakeInit();
        }
    }


    public void SetAppMode(bool isForce = false)
    {
        if (PlayerPrefs.GetInt("FullDebugMode", 0) == 1)
            GameClient.AppMode = AppModeType.FullDebug;

        if (GameClient.AppMode == AppModeType.Release)
        {
            if (isForce)
            {
                AndirDebug debug = gameObject.GetComponent<AndirDebug>();
                if (debug != null)
                {
                    TDebug.OutLogLevel = TDebug.LogLevelType.ERROR;
                    TDebug.LogLevel = TDebug.LogLevelType.ERROR;
                    Destroy(debug);
                }
            }
            else
            {
                TDebug.OutLogLevel = TDebug.LogLevelType.ERROR;
                TDebug.LogLevel = TDebug.LogLevelType.ERROR;
            }
        }
        else if (GameClient.AppMode == AppModeType.KeepOrigin)
        {
            TDebug.LogLevel = TDebug.LogLevelType.DEBUG;
            TDebug.OutLogLevel = TDebug.LogLevelType.DEBUG;
        }
        else if (GameClient.AppMode == AppModeType.FullDebug)
        {
            if (gameObject.GetComponent<AndirDebug>() == null)
            {
                AndirDebug andir = gameObject.CheckAddComponent<AndirDebug>();
                andir.LogLevel = TDebug.LogLevelType.DEBUG;
                TDebug.OutLogLevel = TDebug.LogLevelType.DEBUG;
                TDebug.LogLevel = TDebug.LogLevelType.DEBUG;
                andir.LogTrace = true;
                andir.Log = true; andir.mShowLogLevel = TDebug.LogLevelType.DEBUG;
#if !UNITY_EDITOR
            andir.mShowLogLevel = TDebug.LogLevelType.DEBUG;
#endif
            }
        }
        TDebug.Log("Mode" + GameClient.AppMode);
    }


#region 客户端网络操作
    public virtual void Connect(IPacket msg = null)
    {
        return;
        if (mSSocket == null)
        {
            TDebug.LogError("socket is null");
            return;
        }

        //IPAddress ipAddress = NetUtils.GetIpAddressByHost(ServerInfo.GetGateServHost());
        //if (ipAddress == null)
        //{
        //    TDebug.LogError("网关服务器地址错误:" + ServerInfo.GetGateServHost());
        //    mSSocket.Stage = SSocket.SStage.ConnectFailed;
        //    return;
        //}

        //IPEndPoint mIpEndpoint = new IPEndPoint(ipAddress, ServerInfo.GetGateServPort());
        //mSSocket.Connect(mIpEndpoint, msg);
    }


    /// <summary>
    /// 当连接断开回调
    /// </summary>
    /// <param name="idx"></param>
    public virtual void OnDisconnect(bool showTips)
    {
        if (showTips)
        {
            System.Action overDel = delegate()
            {
                LoginOutGame();
            };
            UIRootMgr.Instance.IsLoading = false;
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(overDel, "连接已断开", Color.red);
        }
    }

    /// <summary>
    /// 当客户端连接成功回调(废弃中)
    /// </summary>
    public void OnConnect()
    {
     
    }

    /// <summary>
    /// 连接操作完成
    /// </summary>
    /// <param name="succes">是否连接指定服务器成功</param>
    public virtual void OnConnected()
    {
        //检查资源更新后，     打开注册界面=>UIRootMgr.MainUI.m_Start.OpenLoginWindow();
        //AssetUpdate.Instance.Init();
        TDebug.Log("连接服务器成功,ip:" + mSSocket.Ip + ":" + mSSocket.Port);
    }

    public virtual void SendMessage(IPacket packet)
    {
        if (IsConnected && packet != null)
        {
            TDebug.LogWarning("发送客户端消息,协议===>>>" + (NetCode_C)packet.NetCode + "[NetCode:" + packet.NetCode + "]");
            mSSocket.Send(packet);
        }
        else
        {
            TDebug.LogError("连接已经断开!!!!!!!!!!!!!!!!!!!!!!!!发送失败");
            Connect(packet);
        }

    }

    public bool IsConnected
    {
        get
        {
            if (mSSocket != null) return mSSocket.IsConnected;
            else return false;
        }
    }

    /// <summary>
    /// 主动释放socket连接,并弹断开提示
    /// </summary>
    public virtual void Disconnect(bool showTips)
    {

        if (mSSocket != null)
        {
            mSSocket.Disconnect();
            mSSocket = null;
        }
        
        OnDisconnect(showTips);
    }

    public virtual void OnMessageArrived(BinaryReader ios)
    {

        short   head = ios.ReadInt16();
        IPacket packet = null;

        int status = ios.ReadInt32();

        if (status > 1)
        {
            if (mStatusHandler.Handle(status))
            {
                return;
            }
        }

        ServPacketHander hander = null;
        if (mServHandlers.TryGetValue(head, out hander))
        {
            if (hander != null)
            {
                TDebug.LogWarning("解析服务器消息,消息协议===>>" + ((NetCode_S)head) + "[NetCode:" + head + "],Status Code:" + status);
                hander(ios);
                return;
            }
        }

        TDebug.LogError("未注册的服务器消息回调" + ((NetCode_S)head) + "[NetCode:" + head + "]");
    }


    public void RegisterNetCodeHandler(NetCode_S servCmd, ServPacketHander handler)
    {
        if (null != handler)
            mServHandlers[(short)servCmd] = handler;
        else
            mServHandlers.Remove((short)servCmd);
    }


    void OnDestroy()
    {
        GameClient.Instance.Disconnect(false);
    }
    #endregion
}
