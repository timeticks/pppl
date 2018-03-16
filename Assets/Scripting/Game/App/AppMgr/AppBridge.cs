using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.IO;
/// <summary>
/// 从游戏开始到结束，一直存在的物体。
/// 需注意事项：
/// 1.除开始场景，其余转场景必须经过AppBridge的LoadScene()方法
/// 
/// </summary>

public class AppBridge : MonoBehaviour
{
    public static AppBridge Instance { get; private set; }
    public static bool      STAND_ALONE = true;    //是否单机
    public static int       ORIGIN_SCREEN_HEIGHT;

    public SceneType CurScene { get { return AppScene.SceneData.m_CurScene; } }        //当前在哪个场景
    public string SceneStr;

    public SceneData SceneData { get { return AppScene.SceneData; } }
    [HideInInspector]
    public AppScene AppScene;
    [HideInInspector]
    public AppEvtMgr EvtMgr;         //事件管理器

    private static int mBackgroundRate = 1;
    private static int mAndroidMaxSolution=720;


    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            AppScene = gameObject.CheckAddComponent<AppScene>();
            AppScene.Init();
        }
        Application.targetFrameRate = GameLauncher.MAX_FRAME_RATE;
    }

    public static bool InitSetting()
    {
        mBackgroundRate = GameLauncher.MAX_FRAME_RATE;
        if (PlatformUtils.EnviormentTy != EnviormentType.Editor)
        {
            mBackgroundRate = 1;
        }
        TDebug.InEditor = (PlatformUtils.EnviormentTy == EnviormentType.Editor);

        //return SetResolution(mAndroidMaxSolution);
        return true;
    }


    void Start()
    {
        MemoryModity.callback = MemoryModify;
        //StartCoroutine("AuToTestBattle");
    }
    public static bool SetResolution()
    {
        if (ORIGIN_SCREEN_HEIGHT == 0) ORIGIN_SCREEN_HEIGHT = Screen.height;
        if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
            int width = Mathf.Max((int)(Screen.width * 0.8f), mAndroidMaxSolution);
            if (Screen.currentResolution.width <= width)
                return true;
            float ratio = (float)Screen.height / Screen.width;
            Screen.SetResolution(width, (int)(ratio * width), true);
            return false;
        }
        return true;
    }

    public void MemoryModify()
    {
        TDebug.Log("内--存--修--改!");
        if (UIRootMgr.Instance != null)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(delegate() { Application.Quit(); }, LobbyDialogue.GetDescStr("desc_cheat_1"), Color.red);
        }
        StartCoroutine(StartQuitGame());
    }


    public IEnumerator StartQuitGame()
    {
        yield return new WaitForSeconds(4f);
        Application.Quit();
    }

    void Update()
    {
        AppEvtMgr.Instance.Update();
        JobMgr.Instance.Update(Time.deltaTime);
        if (Time.frameCount % 3000 == 0)
        {
            System.GC.Collect();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (UIRootMgr.Instance != null && GameLauncher.CurLauncherState == GameLauncher.LauncherState.Playing && !AppScene.IsLoading)
            {
                if (UIRootMgr.Instance.GetOpenListWindow(WinName.Window_ExitGame) == null)
                    UIRootMgr.Instance.OpenWindow<Window_ExitGame>(WinName.Window_ExitGame, CloseUIEvent.None).OpenWindow();
                else
                    UIRootMgr.Instance.GetOpenListWindow(WinName.Window_ExitGame).CloseWindow();
            }
        }
    }


    /// <summary>
    /// 检查curList数量，如果不足，则新建生成
    /// 将多余的隐藏
    /// </summary>
    public List<T> AddInstantiate<T>(List<T> curList, GameObject prefabObj, Transform parentTrans, int needNum, bool isRotateZero = true) where T : MonoBehaviour
    {
       
        for (int i = curList.Count; i < needNum; i++)
        {
            GameObject g = Instantiate(prefabObj) as GameObject;
            TUtility.SetParent(g.transform, parentTrans, false, isRotateZero);
            T p = g.GetComponent<T>();
            curList.Add(p);
        }
        for (int i = needNum; i < curList.Count; i++)
        {
            if (curList[i] == null) curList[i].gameObject.SetActive(false);
        }
        return curList;
    }

    #region 加载场景、资源

    void FreshAllShader(AssetBundle asset)//重新加载所有shader
    {
        var materials = asset.LoadAllAssets(typeof(Material));
        foreach (Material m in materials)
        {
            var shaderName = m.shader.name;
            var newShader = Shader.Find(shaderName);
            if (newShader != null) { m.shader = newShader; }
            else { TDebug.LogErrorFormat("unable to refresh shader: {0} in material {1}" , shaderName ,m.name); }
        }
    }

    ///// <summary>
    ///// 加载到某场景，此接口用于除开始场景的所有需要转场景的地方
    ///// 是否需要去loading场景进行异步加载="needAsync"
    ///// </summary>
    //public void LoadSceneSync(SceneType aimScene ,int toSceneId = 0)
    //{
    //    string aimSceneName = aimScene.ToString();
    //    AppBridge.Instance.AppScene.SceneData.SceneSwitch(AppBridge.Instance.AppScene.SceneData.m_CurScene, aimScene, toSceneId);
    //    AppBridge.Instance.AppScene.SceneData.AddLastScene(AppBridge.Instance.AppScene.SceneData.m_CurScene, AppBridge.Instance.AppScene.SceneData.m_CurSceneId);
    //    if (aimScene == SceneType.LobbyScene)
    //    {
    //        //Hashtable tab = GameData.Instance.GetData(DataName.MapArea, toSceneId.ToString());
    //        //aimSceneName = TUtility.TryGetValueStr(tab, "Scene", "");
    //        aimSceneName = "LobbyScene";
    //    }
    //    AppScene.LoadSceneSync(aimScene, aimSceneName); 
    //}

    public void LoadSceneAsync(SceneType aimScene , int aimScenId = 0)//不经loading场景，直接进行异步加载场景。。用于开始场景
    {
        string aimSceneName = aimScene.ToString();
        AppBridge.Instance.AppScene.SceneData.SceneSwitch(AppBridge.Instance.AppScene.SceneData.m_CurScene, aimScene, aimScenId);
        AppBridge.Instance.AppScene.SceneData.AddLastScene(AppBridge.Instance.AppScene.SceneData.m_CurScene, AppBridge.Instance.AppScene.SceneData.m_CurSceneId);
        if (aimScene == SceneType.StartScene)
        {
            aimSceneName = "GameLaucher";
        }
        else if (aimScene == SceneType.LobbyScene)
        {
            //Hashtable tab = GameData.Instance.GetData(DataName.MapArea, aimScenId.ToString());
            //aimSceneName = TUtility.TryGetValueStr(tab, "Scene","");
            aimSceneName = "LobbyScene";
        }
        AppScene.LoadSceneAsync(aimScene, aimSceneName);
    }


    #endregion



    #region 游戏退出、失去焦点等

    void OnDestroy()
    {
        MemoryModity.callback = null;
    }
    void OnApplicationQuit() //强杀进程
    {
      //  LocalNotification.SendNotification(2, 1, "OnApplicationQuit", LocalNotificationMgr.Instance == null ? "OnApplicationQuitNull" : "OnApplicationQuit!", new Color32(0xff, 0x44, 0x44, 255));
        if (LocalNotificationMgr.Instance != null)
            LocalNotificationMgr.Instance.RegisterMsgNotify();
    }
    private long mLastPauseTime = 0;
    void OnApplicationPause(bool isPause)
    {
        //TDebug.Log("OnApplicationPause==" + isPause.ToString());
        //if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
            if (!isPause) TapDB.onResume();
            else TapDB.onStop();
        }

        if (GameClient.Instance!=null)
            GameClient.Instance.mLastUnityTime = 0;
        if (!isPause)
        {
            //如果大于5分钟，或连接已断开，则VerifyLogin
            Application.targetFrameRate = GameLauncher.MAX_FRAME_RATE;

            int outTime = GameConstUtils.num_connect_out_time;
            long curOut = TimeUtils.CurrentTimeMillis - mLastPauseTime;
           
            if (PlayerPrefsBridge.Instance != null && PlayerPrefsBridge.Instance.PlayerData != null)
                LocalNotificationMgr.CancelNotify(PlayerPrefsBridge.Instance.PlayerData.PlayerUid);
           
            TDebug.LogFormat("IsConnected:{0}  {1}  {2}" , GameClient.Instance.IsConnected , GameConstUtils.num_connect_out_time , curOut);
            if ((curOut > outTime || !GameClient.Instance.IsConnected) && GameLauncher.CurLauncherState == GameLauncher.LauncherState.Playing
                && PlayerPrefsBridge.Instance != null && PlayerPrefsBridge.Instance.PlayerData.PlayerUid > 0)
            {
                TDebug.Log(string.Format("后台时间 {0}，强制重连|{1}|{2}", curOut, TimeUtils.CurrentTimeMillis,mLastPauseTime));
                GameClient.Instance.Reconnect();
            }
            else
            {
                if (!GameClient.Instance.IsConnected && GameLauncher.CurLauncherState == GameLauncher.LauncherState.Playing)
                {
                    GameClient.Instance.LoginOutGame();
                    return;  
                }
            }
        }
        else
        {
            mLastPauseTime = TimeUtils.CurrentTimeMillis;
           // LocalNotification.SendNotification(1, 1, "Pause", LocalNotificationMgr.Instance == null ? "OnApplicationPauseNUll" : "OnApplicationPause!", new Color32(0xff, 0x44, 0x44, 255));
            if (LocalNotificationMgr.Instance != null)
                LocalNotificationMgr.Instance.RegisterMsgNotify();
            if (PlatformUtils.EnviormentTy != EnviormentType.Editor)
            {
                Application.targetFrameRate = 0;
            }
        }
    }



    void OnApplicationFocus(bool isFocus)
    {
        if (isFocus)
        {
            
            //UIRootMgr.Instance.IsLoading = true;
            //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.VerifyLogin, S2C_VerifyLogin);
            //GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_VerifyLogin());
        }
        else
        {
            //if (UIRootMgr.Instance!=null) UIRootMgr.Instance.CloseDisableWin(3);
            // SetResolution(mAndroidMaxSolution);//会导致登录窗口输入栏自适应出现问题
            //System.GC.Collect();
        }
    }

    #endregion








    public void DoAndirLog(string actionStr)
    {
        TDebug.Log(actionStr);
        if (actionStr == "exitGame")
        {
            if (UIRootMgr.Instance != null && UIRootMgr.Instance.GetCurMainUI() != null)
            {
                if (UIRootMgr.Instance.GetOpenListWindow(WinName.Window_ExitGame) == null)
                    UIRootMgr.Instance.OpenWindow<Window_ExitGame>(WinName.Window_ExitGame, CloseUIEvent.None).OpenWindow();
                else
                    UIRootMgr.Instance.GetOpenListWindow(WinName.Window_ExitGame).CloseWindow();
            }
        }
        else if (actionStr == "openBattle")
        {
            UIRootMgr.Instance.OpenWindow<Window_Battle>(WinName.Window_Battle, CloseUIEvent.None);
        }
        else if (actionStr == "setLobby")
        {
            TDebug.Log(UIRootMgr.Instance.LobbyCanvas.alpha);
            UIRootMgr.Instance.LobbyCanvas.alpha = (UIRootMgr.Instance.LobbyCanvas.alpha == 0) ? 1 : 0;
        }
        else if (actionStr == "setLobby2")
        {
            TDebug.Log(UIRootMgr.Instance.LobbyCanvas.alpha);
            UIRootMgr.Instance.LobbyCanvas.alpha = Mathf.Repeat(UIRootMgr.Instance.LobbyCanvas.alpha + 0.25f, 1);
        }
        else if (actionStr == "loginOut")
        {
            GameClient.Instance.LoginOutGame();
        }
        else if (actionStr == "dll")
        {
            TDebug.LogFormat("{0}/{1}",FileUtils.PersistentDataPurePath , AssetUpdate.AssemDllFileName);
            TDebug.Log(File.Exists(string.Format("{0}/{1}", FileUtils.PersistentDataPurePath ,AssetUpdate.AssemDllFileName)));
        }
        else if (actionStr == "notify")
        {
            gameObject.AddComponent<NotificationTest>();
        }
        else if (actionStr == "cleanCache")
        {
            PlayerPrefs.DeleteAll();
            Caching.CleanCache();
        }
        else if (actionStr == "addFps")
        {
            gameObject.CheckAddComponent<FPSCounter>();
        }
        else if (actionStr == "resolution")
        {
            SetResolution();
        }
        else if (actionStr == "logerror")
        {
            TDebug.LogError("error");
        }
        else if (actionStr.Contains("paymytest"))
        {
            PurcharseAndroid.AliPay(actionStr.Replace("paymytest", ""));
        }
        else if (actionStr.Contains("paymysand"))
        {
            PurcharseAndroid.SetSandbox();
        }
        else if (actionStr.Contains("wechatpay"))
        {
            PurcharseAndroid.WeChatPay(actionStr.Replace("wechatpay", ""));
        }
        else if (actionStr.Contains("wechatact"))
        {
            PurcharseAndroid.WeChatPayActivity(actionStr.Replace("wechatact", ""));
        }
        else if (actionStr.Contains("wechatwww"))
        {
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_CreatePayOrder(PurchaserChannel.AndroidWeChatPay, 1501010001));
        }
        else if (actionStr.Contains("wechatid"))
        {
            string idStr = actionStr.Replace("wechatid", "");
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_CreatePayOrder(PurchaserChannel.AndroidWeChatPay, int.Parse(idStr)));
        }
        else if (actionStr.Contains("onewayads"))
        {
            AdsMgr.Instance.OneWayAds(null);
        }
        else if (actionStr.Contains("wwwurl"))
        {
            string infourl = actionStr.Replace("wwwurl", "");
            StartCoroutine(DownAppInfoCor(infourl, false));
        }
        else if (actionStr.Contains("wwwtrueurl"))
        {
            string infourl = actionStr.Replace("wwwtrueurl", "");
            StartCoroutine(DownAppInfoCor(infourl, true));
        }
        else if (actionStr.Contains("showtime"))
        {
            TDebug.LogError(TUtility.ConvertToDateTime(AppTimer.CurTimeStampMsSecond).ToString());
        }
        else if (actionStr == "edll")
        {
            TDebug.Log(AssetUpdate.AndroidDllPath());
            TDebug.Log(File.Exists(AssetUpdate.AndroidDllPath()));
        }
        else if (actionStr.Contains("appinfo"))
        {
            string infourl = FileUtils.AppInfoURL;
            if (actionStr.Contains("appinfo2"))
            {
                infourl = actionStr.Replace("appinfo2", "");
            }
            Action<LoadDataFromHTTP.WWWRequest> successAction = delegate(LoadDataFromHTTP.WWWRequest www)
            {
                TDebug.Log(string.Format("==下载成功{0}", www.Www.text));
            };
            Action<LoadDataFromHTTP.WWWRequest> failAction = delegate(LoadDataFromHTTP.WWWRequest www)
            {
                TDebug.Log(string.Format("==下载失败{0}", www.Www.error));
            };
            LoadDataFromHTTP.Instance.StartHttpRequest(infourl, successAction, failAction);
        }
        else if (actionStr.Contains("autoOpen"))
        {
            int num = 0;
            int.TryParse(actionStr.Replace("autoOpen", ""), out num);
            StartCoroutine(AutoOpen(num));
        }
        else if (actionStr.Contains("setversion"))
        {
            string[] result = actionStr.Split('|');
            PlayerPrefs.SetString("forceVersion",result[1]); 
            
        }
        else if (actionStr.Contains("makeFullDebug"))
        {
            if (actionStr == "makeFullDebug1")
            {
                PlayerPrefs.SetInt("FullDebugMode", 1);
            }
            else
            {
                PlayerPrefs.DeleteKey("FullDebugMode");
                GameClient.AppMode = AppModeType.Release;
                GameClient.Instance.SetAppMode(true);
            }
        }
        else if (actionStr.Contains("reconnect"))
        {
            GameClient.Instance.Reconnect();
        }
        else if (actionStr.Contains("autoRank"))
        {
            int num = int.Parse(actionStr.Replace("autoRank", ""));
            StartCoroutine(AutoRank());
        }
        else if (actionStr.Contains("autoBattle"))
        {
            int enemyId = 1201116015;
            if (actionStr != "autoBattle")
                int.TryParse(actionStr.Replace("autoBattle", ""), out enemyId);
            StartCoroutine(AutoBattle(enemyId));
        }
        else if (actionStr == "stopCoroutine")
        {
            StopAllCoroutines();
        }
        else if (actionStr == "snapshotTime")
        {
            GameClient.SnapshotTimeDetla = GameClient.SnapshotTimeDetla == 60000 ? 2000 : 60000;
        }
        else if (actionStr == "disconnect")
        {
            GameClient.Instance.Disconnect(false);
        }
        else if (actionStr == "error")
        {
            int[] ii = new int[0];
            int a = ii[11];
        }
        else if (actionStr == "initpoly")
        {
            AdsMgr.Instance.Init();
        }
        else if (actionStr == "poly1")
        {
            Debug.Log("aaaa1");
            Polymer.PolyADSDK.showRewardDebugView();
        }
        else if (actionStr == "poly2")
        {
            Debug.Log("aaaa2");
            AdsMgr.Instance.PolyAds();
        }
        else if (actionStr == "poly3")
        {
            Debug.Log("aaaa3");
            Polymer.PolyADSDK.showRewardAd("Playing");
        }
        else if (actionStr == "logerror")
        {
            TDebug.LogError("falshcat_testError0");
            TDebug.LogError("falshcat_testError1");
        }
        else if (actionStr == "logexc")
        {
            throw new Exception("falshcat_testException");
        }
        else if (actionStr == "dataPath")
        {
            TDebug.LogFormat("Application.dataPath:{0}", Application.dataPath);
        }
        else if (actionStr.Contains("serverTest"))
        {
            //#if !UNITY_EDITOR
            //            if (PlatformUtils.EnviormentTy != EnviormentType.Standalone)
            //                return;
            //#endif
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.StoreInfo, S2C_StoreInfo);
            if (actionStr.Contains("serverTestE"))
                StartCoroutine(serverTestExecutors());
            else
                StartCoroutine(serverTest());
        }
    }
    IEnumerator serverTestExecutors()
    {
        for (int i = 1; i < 1000; i++)
        {
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.ServerTest, i));
            yield return null;
        }
    }
    IEnumerator serverTest()
    {
        for (int i = 1; i < 1000; i++)
        {
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_ResMapExit(i));
            yield return null;
        }
    }

    private int mLastNum;
    void S2C_StoreInfo(BinaryReader ios)
    {
        int curNum = ios.ReadInt32();
        Debug.Log(curNum);
        if (curNum - mLastNum != 1)
            Debug.LogError(string.Format("顺序错乱！！==={0}|{1}", mLastNum, curNum));
        mLastNum = curNum;
    }

    protected IEnumerator AutoOpen(int num)
    {
        yield break;
        //GameClient.AppMode = AppModeType.Release;
        //GameClient.Instance.SetAppMode(true);
        //for (int i = 0; i < num; i++)
        //{
        //    if (LobbySceneMainUIMgr.Instance == null) yield break;
        //    LobbySceneMainUIMgr.Instance.BtnEvt_OpenAssem();
        //    yield return new WaitForSeconds(2f);
        //    LobbySceneMainUIMgr.Instance.BtnEvt_OpenCave();
        //    //yield return new WaitForSeconds(2f);
        //    //LobbySceneMainUIMgr.Instance.BteEvt_OpenWorld();
        //    yield return new WaitForSeconds(2f);
        //    LobbySceneMainUIMgr.Instance.BtnEvt_OpenShop();
        //    yield return new WaitForSeconds(2f);
        //    LobbySceneMainUIMgr.Instance.BtnEvt_OpenSect();
        //    yield return new WaitForSeconds(2f);
        //    LobbySceneMainUIMgr.Instance.BtnEvt_OpenHonor();
        //    yield return new WaitForSeconds(2f);
        //    UIRootMgr.Instance.OpenWindow<Window_Ranking>(WinName.Window_Ranking)
        //        .OpenWindow(WindowBig_Honor.ChildTab.Ranking0, Window_Ranking.RankEnum.Level, true);
        //    yield return new WaitForSeconds(2f);
        //    BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Fight, 1201102002, PVESceneType.None, null);
        //    yield return new WaitForSeconds(40f);
        //    Window_Battle.Instance.CloseWin();
        //    yield return new WaitForSeconds(3f);
        //}
        //GameClient.AppMode = AppModeType.FullDebug;
        //GameClient.Instance.SetAppMode(true);
    }

    public IEnumerator DownAppInfoCor(string url , bool isDispose)
    {
        WWW www = new WWW(url);
        yield return www;
        TDebug.Log(www.text);
        if (isDispose)
        {
            www.Dispose();
        }
    }

    protected IEnumerator AuToTestBattle()
    {
        yield break;
        //while (UIRootMgr.Instance == null)
        //{
        //    yield return new WaitForSeconds(2f);
        //}
        //UIRootMgr.Instance.TopMasking = true;
        //GameClient.Instance.gameObject.GetComponent<AndirDebug>().mShowLogLevel = TDebug.LogLevelType.ERROR;
        //GameClient.Instance.gameObject.GetComponent<AndirDebug>().ClearShowLog();
        
        //while (UIRootMgr.Instance.GetOpenListWindow<Window_Login>(WinName.Window_Login) == null)
        //{
        //    yield return new WaitForSeconds(2f);
        //}
        //GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_Login("yyyyyyy2", "111111"));
        //yield return new WaitForSeconds(2f);
        //while (UIRootMgr.Instance.GetOpenListWindow<Window_ChooseRole>(WinName.Window_ChooseRole) == null)
        //{
        //    yield return new WaitForSeconds(2f); 
        //}
        //UIRootMgr.Instance.GetOpenListWindow<Window_ChooseRole>(WinName.Window_ChooseRole).ChooseRoleSave(10013);
        //yield return new WaitForSeconds(10f);

        //BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Fight, 1201113009, PVESceneType.None, null);

        //yield return new WaitForSeconds(100f);
        

        //GameClient.Instance.LoginOutGame();
        //yield return new WaitForSeconds(5f);
        //UIRootMgr.Instance.TopMasking = true;
        //GameClient.Instance.gameObject.GetComponent<AndirDebug>().mShowLogLevel = TDebug.LogLevelType.ERROR;
        //GameClient.Instance.gameObject.GetComponent<AndirDebug>().ClearShowLog();
        //while (UIRootMgr.Instance.GetOpenListWindow<Window_Login>(WinName.Window_Login) == null)
        //{
        //    yield return new WaitForSeconds(2f);
        //}
        //GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_Login("yyyyyyy2", "111111"));
        //yield return new WaitForSeconds(2f);
        //while (UIRootMgr.Instance.GetOpenListWindow<Window_ChooseRole>(WinName.Window_ChooseRole) == null)
        //{
        //    yield return new WaitForSeconds(2f);
        //}
        //UIRootMgr.Instance.GetOpenListWindow<Window_ChooseRole>(WinName.Window_ChooseRole).ChooseRoleSave(10013);
        //yield return new WaitForSeconds(10f);

        //while (true)
        //{
        //    BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Fight, 1201113009, PVESceneType.None, null);
        //    yield return new WaitForSeconds(100f);
        //    Window_Battle.Instance.CloseWin();
        //    yield return new WaitForSeconds(2f);
        //}
        
    }


    protected IEnumerator AutoRank()
    {
        GameClient.AppMode = AppModeType.FullDebug;
        GameClient.Instance.SetAppMode(true);

        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(60f);
            AdsMgr.Instance.PlayAds(null);
        }

        yield return new WaitForSeconds(5f);
        

        yield return new WaitForSeconds(5f);

        PurcharseAndroid.WeChatPay(
            "wechatpay{\"appid\":\"wx1d09fcabc690b5e6\",\"partnerid\":1900000109,\"prepayid\":\"WX1217752501201407033233368018\",\"noncestr\":\"5K8264ILTKCH16CQ2502SI8ZNMTM67VS\",\"timestamp\":1511416843,\"sign\":\"C380BEC2BFD727A4B6845133519F3AD6\",\"package\":\"Sign=WXPay\"}");
        yield return new WaitForSeconds(60f);

    }
    protected IEnumerator AutoBattle(int id)
    {
        while (true)
        {
            if (!Window_Battle.IsBattleShowing)
            {
                BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Fight, id, PVESceneType.None,0);
                if (!Window_Battle.Instance.mViewObj.EndRoot.gameObject.activeSelf)
                {
                    yield return new WaitForSeconds(2f);
                }
                Window_Battle.Instance.CloseWin();
                yield return new WaitForSeconds(3f);
            }
        }
    }


    //public void LogTestByName(string str)
    //{
    //    TDebug.Log(str);
    //}
    public static void LogTestByName(string str)
    {
        TDebug.LogFormat("LogTestByName:{0}", str);
    }
    public static void JavaCallBackFunction(string detail)
    {
        TDebug.LogFormat("JavaCallBackFunction:{0}", detail);
        string[] details = detail.Split('|');
        if (details.Length == 0)
        {
            TDebug.LogErrorFormat("Detail错误:{0}", detail);
            return;
        }
        string nameKey = details[0];
        switch (nameKey)
        {
            case "WeChatPay":
                {
                    break;
                }
            case "AliPay":
                {

                    break;
                }
        }
    }

    //public static void JavaCallBackFunction(string detail)
    //{
    //    AppBridge.Instance.JavaCallBackFunction(detail);
    //}
}


