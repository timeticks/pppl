//#define UNITY_ANDROID

using System;
using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;


#if UNITY_ANDROID
#endif

public delegate void AsyncLoadBundleCallback(GameObject go);

public class AssetsLoader : MonoBehaviour
{
    public const int ASSETS_WAIT_START          = 0;//等待开始
    public const int ASSETS_LOADED_OK           = 1;
    public const int ASSETS_LOADED_LOADING      = -1;
    public const int ASSETS_LOADED_NEW_CLEINT   = 100;//新的客户端版本
    public const int ASSETS_LOADED_UPDATE_RES   = 101;//资源需要更新
    public const int ASSETS_PARSE_ERROR         = 102;//客户端配置档解析出错
    public const int ASSETS_EXCETION_APPSETTING = 103;//客户端初始异常
    public const int ASSETS_LOADED_NET_FAILD    = 104;//请求HTTP失败 更新资源异常
    public const int ASSETS_ERROR_APPINFO       = 106;//初始Appinfo错误
    public const int ASSETS_EXCETION_LOCALIZA   = 105;//初始多语言包异常
    public const int ASSETS_EXCETION_GAMEDATA   = 108;//初始游戏数据异常
    public const int ASSETS_EXCETION_RES        = 107;//初始Res异常

    /////////////////////
    public interface IAssetsLoader
    {
        void OnChangeState(string error);
        void OnDownLoad(int curr, int max = 1);
        void OnLoad(int curr, int max = 1);
        void OnFinish();
        void ShowSystemConfirm(string text, Action confirm, Action cancel);
    }




    private static AssetsLoader     mInstance = null;

    /// <summary>
    /// 自定义路径
    /// </summary>
    private static string           ManualUrl           = "";
    private string                  mDownloadPathStr    = null;
    private int                     mLoaderStatus       = 0;
    private AssetUpdate             mAssetUpdate;
   
   
    /// <summary>
    /// 更新资源索引
    /// </summary>
    private List<string>                        mUpdateBundlesStates = new List<string>();

    /// <summary>
    /// 更新文件
    /// </summary>
    private byte[]                              mServResBytes;
    private long                                mHdSpace;

    private bool                                mGameBundlePass;
    private bool                                mGameDataPass;
    private bool                                mGameLocalizaPass;
    //private bool                                mServerAreaPass;
    private bool                                mNoticePass;

    private bool                                mIsAppSettingChanged;

    //本地加载资源
    public AssetBundle CreateFromFile(string bundleName)
    {
        try
        {
            //List<string> dependlist = GetDependList(bundleName);
            //if (dependlist == null) return null;
            //
            //LoadDependAsset(dependlist);
            //
            //byte[] data         = null;
            //string asstsPath =
#if UNITY_AN//DROID
            //string asstsPath = "Android/Game/" + FormatFileName(bundleName) + ".bytes";
            ////data = JavaHelper.Instance.GetStreamAssets(asstsPath);
            //
#elif !UNITY//_WEBPLAYER
            //asstsPath    = FormatAssetsPath(TUtils.FormatFileName(bundleName) + ".bytes");
            ////data = System.IO.File.ReadAllBytes(asstsPath);
#endif      //
            //
            //if (asstsPath != null)
            //{
            //    AssetBundle mainAsset = AssetBundle.CreateFromFile(asstsPath);
            //    return mainAsset;
            //}
            
        }
        catch (Exception ex)
        {
            Debug.LogError("下载Bundle出错!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + ex.Message);
        }

        return null;
    }


    void LoadDependAsset(List<string> dependlist)
    {
        string          asstsPath  = null;
        byte[]          data       = null;
        foreach (string dependAsset in dependlist)
        {
            if (dependAsset == "Level")//跳过level.地下城下会读取level根节点数据
                continue;

            //if (!mDependAssetRefs.ContainsKey(dependAsset))
            //{
            //    mDependAssetRefs.Add(dependAsset, null);//占坑 加锁
            //
#if UNITY_AN//DROID
            //    asstsPath = "Android/Game/"+FormatFileName(dependAsset) + ".bytes";
            //    data = JavaHelper.Instance.GetStreamAssets(asstsPath);
#elif !UNITY//_WEBPLAYER
            //
            //    asstsPath = FormatAssetsPath(TUtils.FormatFileName(dependAsset) + ".bytes");
            //   // data = System.IO.File.ReadAllBytes(asstsPath);
#endif      //
            //    //if (data == null || data.Length == 0)
            //    //{
            //    //    Debug.LogError("依赖Bundle丢失,加载失败");
            //    //    return;
            //    //}
            //    //
            //    //AssetBundle dependAssets = AssetBundle.CreateFromMemoryImmediate(data);
            //    //if (dependAssets != null)
            //    //    mDependAssetRefs[dependAsset] = dependAssets;
            //    //else
            //    //    mDependAssetRefs.Remove(dependAsset);
            //
            //    if (asstsPath != null)
            //    // if (data != null)
            //    {
            //        AssetBundle mainAsset = AssetBundle.CreateFromFile(asstsPath);
            //        
            //        // AssetBundle mainAsset = AssetBundle.CreateFromMemoryImmediate(data);
            //     //   return mainAsset;
            //    }
            //}
        }
    }

    public void Init()
    {
        mLoaderStatus = 0;
        mServResBytes       = null; 
        mHdSpace            = 0l; 
        mGameBundlePass     = false;
        mGameDataPass       = false;
        mGameLocalizaPass   = false;
        byte[] data         = null;
        
        Window_LoadBar.Instance.Fresh(UnityEngine.Random.Range(0.02f, 0.05f), UnityEngine.Random.Range(0.8f, 0.9f), 2f, "初始游戏信息");
        GameClient.Instance.InitUIRoot();
        
        //暂时屏蔽
        mLoaderStatus = ASSETS_LOADED_OK;
        return;
        
        try
        {
#if UNITY_ANDROID &&!UNITY_EDITOR
            TDebug.Log("Android平台编译的代码!!!!");
            string asstsPath = FileUtils.StreamingAssetsPathReadPath("/", "AppSetting.cfg");
            data=JavaHelper.Instance.GetStreamAssets(asstsPath);
# elif !UNITY_WEBPLAYER
            TDebug.Log("IOS PC 平台编译的代码!!!!LoaderStatus===>" + mLoaderStatus);
            string asstsPath = FileUtils.StreamingAssetsPathReadPath("/", "AppSetting.cfg");
            data = File.ReadAllBytes(asstsPath);
#endif
            string streamCfg = Encoding.UTF8.GetString(data);

            JsonMapper.ToObject<AppSetting>(streamCfg);
            FileUtils.Init();

            if (AppSetting.AppMode == 1) GameClient.AppMode = AppModeType.KeepOrigin;
            else if (AppSetting.AppMode == 112) GameClient.AppMode = AppModeType.FullDebug;
            else GameClient.AppMode = AppModeType.Release;
            GameClient.Instance.SetAppMode();

            if (PlayerPrefs.HasKey(AssetUpdate.AppSettingSave.ToString()) && PlayerPrefs.GetString(AssetUpdate.AppSettingSave.ToString()) != streamCfg)
            {
                mIsAppSettingChanged = true; //被覆盖安装时，应该舍弃之前下载到persistpath的资源
                PlayerPrefs.DeleteKey(AssetUpdate.BundleAssetUpdated); //清空之前的bundle缓存
                PlayerPrefs.DeleteKey(AssetUpdate.TryUpdateBundleVer);
                PlayerPrefs.DeleteKey(AssetUpdate.DownloadingKey);
                PlayerPrefs.DeleteKey(AssetUpdate.SaveMD5);
                PlayerPrefs.DeleteKey(AssetUpdate.BundleVersion + AppSetting.Version);
                PlayerPrefs.DeleteKey(AssetUpdate.GameVersion);
                TDebug.Log("==新的AppSetting==");
            }
            else
            {
                mIsAppSettingChanged = false;
            }
            TDebug.Log("==AppSetting状态==" + mIsAppSettingChanged +"==Flag"+ AssetUpdate.Flag);
            PlayerPrefs.SetString(AssetUpdate.AppSettingSave.ToString(), streamCfg);

            TDebug.Log(string.Format("加载AppInfo:{0}路径", FileUtils.AppInfoURL));
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Window_LoadBar.Instance.Fresh(0, "网络未连接,请检查网络后重试");
                if (UIRootMgr.Instance != null)
                    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(delegate() { Invoke("Init", 0.1f); }, "网络未连接,请检查网络后重试", Color.red);
                return;
            }
            LoadDataFromHTTP.Instance.StartHttpRequest(FileUtils.AppInfoURL, OnRequestAppInfoSucceed, OnRequestAppInfoFailed);
        }
        catch (Exception ex)
        {
            TDebug.LogError(string.Format("加载AppSetting错误:{0}",ex.Message));
            Window_LoadBar.Instance.Fresh(0 , "初始AppSetting错误");
            if (UIRootMgr.Instance!=null)
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("初始AppSetting错误，请与客服人员联系", Color.red);
        }
        if (data == null)
        {
            mLoaderStatus = ASSETS_EXCETION_APPSETTING;
            TDebug.LogError("Init Error");
        }
    }


    /// <summary>
    /// 向服务器请求平台信息,版本信息返回结果  
    /// </summary>
    /// <param name="www"></param>
    private void OnRequestAppInfoSucceed(LoadDataFromHTTP.WWWRequest www)
    {
        string text = www.Www.text;
        TDebug.Log(string.Format("==下载成功{0}", text));
        try
        {
            JsonMapper.ToObject<ServerInfo>(text);
            if (Mathf.Abs((float)(AppSetting.Version - ServerInfo.Version))>=0.0001f)
            {
                string errorStr = string.Format("版本过低(v{0})，请使用新版本进入游戏", AppSetting.Version.ToString("f2"));
                Window_LoadBar.Instance.Fresh(0, "版本过低");
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(Application.Quit , errorStr, Color.red);
                return;
            }
        }
        catch (Exception ex)
        {
            mLoaderStatus = ASSETS_ERROR_APPINFO;
            TDebug.LogError("初始ServerInfo错误:" + ex.Message);
            if (UIRootMgr.Instance != null)
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(string.Format("初始AppInfo错误({0}{1})，请与客服人员联系",FileUtils.Platform,AppSetting.Version), Color.red);
            Window_LoadBar.Instance.Fresh(0 , "初始AppInfo错误");
            return;
        }



        try
        {
            
            float saveGameVersion = PlayerPrefs.GetFloat(AssetUpdate.GameVersion, (float)AppSetting.GameVersion);
            int status = FileUtils.IsForceUpdate(saveGameVersion, (float)ServerInfo.GameVersion);
            TDebug.Log(string.Format("当前游戏版本:{0} ,最新版本:{1}    status:{2}", saveGameVersion, ServerInfo.GameVersion, status));
            if (mIsAppSettingChanged && status == 0 && AssetUpdate.IsOldDllExit())//如果当前dll为最新，且有旧dll缓存
            {
                Window_LoadBar.Instance.Fresh(0 ,"已为您移除旧版本无用数据,需重新进入游戏");
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(JavaHelper.Instance.AndroidRestart, "已为您移除旧版本无用数据,点击确认重新进入游戏", Color.red);
                return;
            }
            if (status != 0)
            {
                if (status == 1)
                {
                    TDebug.Log("开始进行dll更新");
                    Window_LoadBar.Instance.FreshWithoutPct("进行游戏更新");
                    if (mAssetUpdate == null)
                    {
                        GameObject g = new GameObject("AssetUpdate");
                        mAssetUpdate = g.CheckAddComponent<AssetUpdate>();
                    }
                    mAssetUpdate.UpdateDll(); 
                    return;
                }
                else if (status == 2)
                {
                    string errorStr = string.Format("版本过低(v{0})，请使用新版本进入游戏", ServerInfo.GameVersion.ToString("f2"));
                    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(errorStr, Color.red);
                    Window_LoadBar.Instance.Fresh(0, errorStr);
                    mLoaderStatus = ASSETS_LOADED_NEW_CLEINT;
                    return;
                }
                //mIAssetsLoader.OnChangeState("客户端版本已有新版本(Ver:"+ servSetting.GameVersion+ "),请重新更新游戏版本,W_Code:105");
                return;
            }
        }
        catch (Exception ex)
        {
            mLoaderStatus = ASSETS_ERROR_APPINFO;
            //mIAssetsLoader.OnChangeState("服务器返回数据错误,Code:104");
            TDebug.LogError("检测游戏版本错误:" + ex.Message);
            Window_LoadBar.Instance.Fresh(0, "检测游戏版本错误");
            if (UIRootMgr.Instance != null)
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("检测游戏版本错误" + ex.Message, Color.red);
            return;
        }
        try
        {
            //检查并初始多语言包
            //int appGameLocalizaVersion = PlayerPrefs.GetInt(ServerInfo.Instance.AppVersion + "_FixGameLocalizaVer", 0) + ServerInfo.Instance.GameLocalizaVersion;
            //Debug.LogWarning("start init game localiza.local version:" + appGameLocalizaVersion+
            //                ",remote serv version:"+ ServerInfo.Instance.AppServInfo.GameLocalizaVersion+ 
            //                ", localiza directory exists:"+ Directory.Exists(ServerInfo.PersistentDataReadPath("/Game/Localiza/", "")));
            ////Debug.LogError("数据配置,客户端配置版本:" + appGameDataVersion + "配置服务器:" + servSetting.GameDataVersion);
            //if (Directory.Exists(ServerInfo.PersistentDataReadPath(AppInfo.GameLocalizaPath, "")) && ServerInfo.Instance.AppServInfo.GameLocalizaVersion == appGameLocalizaVersion)
            //{
            //    InitLocalGameLocaliza();
            //}
            //else
            //{
            //    LoadDataFromHTTP.Instance.StartHttpRequest(ServerInfo.GameLocalizaURL, OnCheckGameLocalizaSucceed, OnRequestAppInfoFailed);
            //}
        }
        catch (Exception ex)
        {
            mLoaderStatus = ASSETS_EXCETION_LOCALIZA;
            //mIAssetsLoader.OnChangeState("服务器返回数据错误,Code:104");
            TDebug.LogError("初始Localiza错误:" + ex.Message);
        }

        try
        {
            //检查并初始游戏数据包
            int appGameDataVersion = PlayerPrefs.GetInt("GameDataVer:" + AppSetting.Version, 0);
            TDebug.Log("数据配置,客户端配置版本:" + appGameDataVersion + "配置服务器:" + ServerInfo.GameDataVersion);
            if (Directory.Exists(FileUtils.PersistentDataReadPath(FileUtils.GameDataPath,
                ServerInfo.GameDataVersion.ToString())))
            {
                InitLocalGameData();
            }
            else
            {
                LoadDataFromHTTP.HttpRequest req = LoadDataFromHTTP.Instance.StartHttpRequest(FileUtils.GameDataURL, OnCheckGameDataSucceed, OnRequestAppInfoFailed);
                if (req!=null && req.Www != null) Window_LoadBar.Instance.Init(req.Www, "初始配置信息", null);
            }
        }
        catch (Exception ex)
        {
            mLoaderStatus = ASSETS_EXCETION_GAMEDATA;
            //mIAssetsLoader.OnChangeState("服务器返回数据错误,Code:104");
            TDebug.LogError("初始GameData错误:" + ex.Message);
            if (UIRootMgr.Instance != null)
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("初始配置信息错误", Color.red);
            Window_LoadBar.Instance.Fresh(0, "初始配置信息错误");
        }


        try
        {
            //检查并初始游戏公告
            int appNoticeVersion = PlayerPrefs.GetInt("ServerNoticeVer:" + AppSetting.Version, 0);
            TDebug.Log("数据配置,客户端公告版本:" + appNoticeVersion + "公告服务器:" + ServerInfo.NoticeVersion);
            if (appNoticeVersion == (int)ServerInfo.NoticeVersion)
            {
                InitLocalNotice();
            }
            else
            {
                LoadDataFromHTTP.HttpRequest req = LoadDataFromHTTP.Instance.StartHttpRequest(FileUtils.NoticeURL, OnCheckNoticeSucceed, OnRequestAppInfoFailed);
                //if (req != null && req.Www != null) Window_LoadBar.Instance.Init(req.Www, "初始配置信息", null);
            }
        }
        catch (Exception ex)
        {
            mLoaderStatus = ASSETS_EXCETION_GAMEDATA;
            //mIAssetsLoader.OnChangeState("服务器返回数据错误,Code:104");
            TDebug.LogError("初始GameData错误:" + ex.Message);
            if (UIRootMgr.Instance != null)
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("初始公告配置信息错误", Color.red);
            Window_LoadBar.Instance.Fresh(0, "初始公告配置信息错误");
        }

        //TDebug.Log("初始区");
        //LoadDataFromHTTP.Instance.StartHttpRequest(FileUtils.ServerAreaURL, OnCheckServerAreaSucceed, OnRequestAppInfoFailed);

        //更新资源包
        try
        {
            TDebug.Log("当前网络环境:" + Application.internetReachability);
            if (SharedAsset.Instance.LoaderType == AssetLoaderType.TResources) //模拟模式，不进入bundle检测
            {
                GameBundlePass = true;
            }
            else
            {
                if (mIsAppSettingChanged)//新安装/覆盖安装后第一次进入
                {
                }

                int appBundleVersion = PlayerPrefs.GetInt(AssetUpdate.BundleVersion + AppSetting.Version, AppSetting.BundleVersion);

                //TODO:测试用句，强制每次检查更新
                //TDebug.Log("测试用句，强制每次检查更新");

                //待更新版本，如果待更新版本没有更新完，下一次若版本仍是待更新版本，不重置断点续传信息
                int tryUpdateBundle = PlayerPrefs.GetInt(AssetUpdate.TryUpdateBundleVer + AppSetting.Version, 0);
                if (tryUpdateBundle == 0 || tryUpdateBundle != ServerInfo.BundleVersion)
                {
                    PlayerPrefs.SetString(AssetUpdate.DownloadingKey, "");//将断点续传资源重置
                }
                TDebug.Log("资源配置,客户端资源版本:" + appBundleVersion + "资源服务器:" + ServerInfo.BundleVersion);

                //if(true)
                if (appBundleVersion == ServerInfo.BundleVersion)
                {
                    InitLocalBundle();
                }
                else
                {
                    mLoaderStatus = ASSETS_LOADED_UPDATE_RES;
                    Action<LoadDataFromHTTP.WWWRequest> successDeleg = delegate(LoadDataFromHTTP.WWWRequest bundleWWW)
                    {
                        string newMd5 = bundleWWW.Www.text;
                        //TDebug.Log("下载md5成功" + bundleWWW.Www.text); //下载md5成功，将需要更新目标资源号进行保存
                        TDebug.Log("下载md5成功");
                        PlayerPrefs.SetInt(AssetUpdate.TryUpdateBundleVer + AppSetting.Version, ServerInfo.BundleVersion);
                        if (mAssetUpdate == null)
                        {
                            GameObject g = new GameObject("AssetUpdate");
                            mAssetUpdate = g.CheckAddComponent<AssetUpdate>();
                        }
                        mAssetUpdate.UpdateBundle(newMd5, InitLocalBundle);
                    };

                    //下载本地，md5文件
                    LoadDataFromHTTP.HttpRequest req = LoadDataFromHTTP.Instance.StartHttpRequest(FileUtils.ResURL + "/" + SharedAsset.VersionMd5TxtName, successDeleg, OnRequestAppInfoFailed);
                    TDebug.Log("资源版本号不相同，进行资源md5下载");
                    Window_LoadBar.Instance.Fresh(0.05f, 0.9f, 1f, "有资源更新，开始更新游戏资源");
                }
            }

        }
        catch (Exception ex)
        {
            mLoaderStatus = ASSETS_EXCETION_RES;
            TDebug.LogError("初始Game Res错误:" + ex.Message);
            Window_LoadBar.Instance.Fresh(0, "初始游戏资源息错误");
            if (UIRootMgr.Instance != null)
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("初始游戏资源息错误", Color.red);
        }
        //try
        //{
        //
        //    //检查Res资源

        //    int appBundleVersion = PlayerPrefs.GetInt(ServerInfo.Instance.AppVersion+"_FixBundleVer" , 0) + ServerInfo.Instance.BundleVersion;
        //    //Debug.LogError("客户端资源版本:" + appBundleVersion + " 服务器:" + servSetting.BundleVersion);
        //
        //    Debug.LogWarning("start init game res Version:" + appBundleVersion +
        //                   ",remote serv version:" + ServerInfo.Instance.AppServInfo.BundleVersion);
        //
        //    if (ServerInfo.Instance.AppServInfo.BundleVersion == appBundleVersion)
        //    {
        //        GameBundlePass = true;
        //    }
        //    else
        //    {//本地加载配置
        //        LoadDataFromHTTP.Instance.StartHttpRequest(ServerInfo.ResURL, OnCheckBundleVersionSucceed, OnRequestAppInfoFailed);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    mLoaderStatus = ASSETS_EXCETION_RES;
        //    mIAssetsLoader.OnChangeState("服务器返回数据错误,Code:104");
        //    Debug.LogError("初始Game Res错误:" + ex.Message);
        //}

    }






    private void OnRequestAppInfoFailed(LoadDataFromHTTP.WWWRequest www)
    {
        TDebug.LogError("远程服务器请求失败. Code:" + www.ErrorCode + " URL:" + www.Url);
        Window_LoadBar.Instance.Fresh(0, string.Format("远程服务器请求失败({0}{1})", FileUtils.Platform, AppSetting.Version));
        if (UIRootMgr.Instance != null)
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(Application.Quit , "远程服务器请求失败，请检查网络并稍后重试", Color.red);
        mLoaderStatus = ASSETS_LOADED_NET_FAILD;
    }

    public static AssetsLoader Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new GameObject("AssetHelper").CheckAddComponent<AssetsLoader>();
			    DontDestroyOnLoad(mInstance.gameObject);
			}
			return mInstance;
		}
	}


    /// <summary>
    /// 服务器返回Bundle信息.并与本地Bundle进行对比
    /// </summary>
    /// <param name="www"></param>
    private void OnCheckGameDataSucceed(LoadDataFromHTTP.WWWRequest www)
    {
        Stream zipData = new MemoryStream(www.Www.bytes);
        string path = FileUtils.PersistentDataWritePath(FileUtils.GameDataPath, "");
        FileUtils.DeleteDirectory(path);
        FileUtils.UnGzipDirectoryFile(zipData, FileUtils.PersistentDataWritePath(FileUtils.GameDataPath, ServerInfo.GameDataVersion.ToString()));
        PlayerPrefs.SetInt("GameDataVer:" + AppSetting.Version, ServerInfo.GameDataVersion);

        Debug.Log("下载配置表完成.版本:" + ServerInfo.GameDataVersion + "\r\n" + path);
        InitLocalGameData();
    }

    private void OnCheckNoticeSucceed(LoadDataFromHTTP.WWWRequest www)
    {
        PlayerPrefs.SetInt("ServerNoticeVer:" + AppSetting.Version, (int)ServerInfo.NoticeVersion);
        PlayerPrefs.SetString("ServerNotice", System.Text.Encoding.UTF8.GetString(www.Www.bytes));

        //Debug.Log("下载公告完成.版本:" + ServerInfo.GameDataVersion + "\r\n" + path);
        InitLocalNotice();
    }

    //private void OnCheckServerAreaSucceed(LoadDataFromHTTP.WWWRequest www)
    //{
    //    string content = System.Text.Encoding.UTF8.GetString(www.Www.bytes);
    //    TDebug.Log(content);
    //    JsonMapper.ToObject<ServerAreaInfo>(content);
    //    //ServerAreaPass = true;
    //}

    private void InitLocalGameData()
    {
        TDebug.LogWarning("start init GameData");

        string url = FileUtils.PersistentDataReadPath(FileUtils.GameDataPath, ServerInfo.GameDataVersion+"");
        //string url = FileUtils.PersistentDataReadPath(FileUtils.GameDataPath, "");

        if (!Directory.Exists(url))
        {
            Window_LoadBar.Instance.Fresh(0 , "游戏数据丢失，请重新更新客户端");
            if (UIRootMgr.Instance != null)
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("游戏数据丢失，请重新更新客户端", Color.red);
            return;
        }

        string[] dataFiles = Directory.GetFiles(url);

        TDebug.LogWarning(string.Format("开始解析配置表，共{0}项", dataFiles.Length));
        //try
        {
            for (int i = 0; i < dataFiles.Length; i++)
            {
                FileInfo file = new FileInfo(dataFiles[i]);

                if (!file.Exists) continue;
                byte[] dataBytes = FileUtils.SyncReadStreamFile(file.FullName);

                if (dataBytes == null || dataBytes.Length < 2)
                    continue;
                MemoryStream _ms = new MemoryStream(dataBytes);
                BinaryReader _br = new BinaryReader(_ms);
                //ConfigHelper.Instance.Init(file.Name, _br);
            }
            GameDataPass = true;
        }
        //catch (Exception ex)
        //{
        //    TDebug.LogError("解析游戏配置错误"+ex.Message);
        //    Window_LoadBar.Instance.Fresh(0, string.Format("解析游戏配置错误({0}{1})", FileUtils.Platform, AppSetting.Version));
        //    if (UIRootMgr.Instance != null)
        //        UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("解析游戏配置错误，请检查游戏版本", Color.red);
        //}
        
    }

    private void InitLocalNotice()//初始化公告
    {
        string content = PlayerPrefs.GetString("ServerNotice", "");// System.Text.Encoding.UTF8.GetString(dataBytes);
        JsonMapper.ToObject<ServerNoticeInfo>(content);
        NoticePass = true;
    }

    private void InitLocalBundle()
    {
        if (mAssetUpdate != null && mAssetUpdate.gameObject != null)
        {
            if (mAssetUpdate.mDiffList != null)
            {
                for (int i = 0; i < mAssetUpdate.mDiffList.Count; i++)
                {
                    if (mAssetUpdate.mDiffList[i].NeedUpdate)  //如果有过bundle更新，则销毁UIRoot重新加载
                    {
                        TDebug.Log("销毁UIRoot，重新加载");
                        DestroyImmediate(UIRootMgr.Instance.gameObject);
                        UIRootMgr.SetInstanceNull();
                        SharedAsset.Instance.ClearAll();
                        GameClient.Instance.InitUIRoot();
                        break;
                    }
                }
            }
            Destroy(mAssetUpdate.gameObject);
        }
        PlayerPrefs.SetInt(AssetUpdate.BundleVersion + AppSetting.Version, ServerInfo.BundleVersion);
        Action callBack = delegate() { GameBundlePass = true; };
        SharedAsset.Instance.LoadStartBundle(callBack);
    }

    /// <summary>
    /// 解压streaming文件到persist
    /// </summary>
    public IEnumerator DepressBundle()
    {
        string streamingMd5 = AssetUpdate.GetStreamingMd5();
        Dictionary<string,string> dict = MD5Utility.ParseVersionFile(streamingMd5);
        foreach (var temp in dict)
        {

            yield return null;
        }
    }



    public int LoaderStatus
    {
        get { return mLoaderStatus; }
    }



    private bool GameDataPass 
    {
        set
        {
            mGameDataPass = value;
            if (mGameDataPass && mGameBundlePass && mNoticePass)
            {
                mLoaderStatus = ASSETS_LOADED_OK;
            }
        }
    }

    private bool GameBundlePass
    {
        set
        {
            mGameBundlePass = value;
            if (mGameDataPass && mGameBundlePass && mNoticePass)
            {
                mLoaderStatus = ASSETS_LOADED_OK;
            }
        }
    }

    //private bool GameLocalizaPass
    //{
    //    set
    //    {
    //        mGameLocalizaPass = value;
    //        if (mGameDataPass && mGameBundlePass && mNoticePass && mServerAreaPass)
    //        {
    //            mLoaderStatus = ASSETS_LOADED_OK;
    //        }
    //    }
    //}

    private bool NoticePass
    {
        set
        {
            mNoticePass = value;
            if (mGameDataPass && mGameBundlePass && mNoticePass)
            {
                mLoaderStatus = ASSETS_LOADED_OK;
            }
        }
    }


    protected void OnDestroy()
    {
        mUpdateBundlesStates.Clear();
        mLoaderStatus = 0;
    }


    ////////////////////////////////////////////////////////////////////////


}
 