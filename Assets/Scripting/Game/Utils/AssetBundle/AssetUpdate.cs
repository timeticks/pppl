using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

public class AssetUpdate :MonoBehaviour
{
    public const string AppSettingSave = "AppSetttingSave";
    public const string BundleVersion = "BundleVersion";
    public const string DownloadingKey = "DownloadingAsset";  //需要断点续传的资源
    public const string BundleAssetUpdated = "BundleAssetUpdated";   //已经更新在persist的资源...如果安装包覆盖更新后，会被清空
    public const string TryUpdateBundleVer = "TryUpdateBundleVer:";
    public const string SaveMD5 = "SaveMD5";                //当前已更新资源的md5
    public const string GameVersion = "GameVersionKey";
    public const string AssemDllFileName = "Assembly-CSharp.dll";

    static Dictionary<string, bool> UpdatedDict = null;

    public static Dictionary<string, bool> InitUpdatedDict()
    {
        UpdatedDict = new Dictionary<string, bool>();
        string str = PlayerPrefs.GetString(BundleAssetUpdated, "");
        if (TDebug.LogLevel == TDebug.LogLevelType.DEBUG)
            TDebug.LogFormat("已更新资源:{0}" , str);

        bool needFreshSet = false;
        string[] s = str.Split('|');
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == "") continue;
            if (!UpdatedDict.ContainsKey(s[i]))
            {
                UpdatedDict.Add(s[i], true);
            }
            else
            {
                needFreshSet = true;
            }
        }
        if (needFreshSet) //将重复的项去掉
        {
            FreshBundleAssetUpdated(UpdatedDict);
        }
        return UpdatedDict;
    }
    public static bool IsUpdated(string bundleName)  //此资源是否已经更新过，若更新过，则在persist路径中
    {
        if (UpdatedDict == null)
        {
            InitUpdatedDict();
        }
        if (UpdatedDict.ContainsKey(bundleName))
        {
            string path = FileUtils.PersistentDataReadPath(FileUtils.GameResPath, bundleName);
            bool isExists = File.Exists(path);
            if (!isExists)
            {
                //这里还原了此资源的版本号，以便下次进入游戏重新下载资源
                ResetMissedBundle(new List<string>() {bundleName});
                TDebug.LogError(string.Format("资源已下载，但路径中没有此资源:{0}:[{1}]",bundleName, path));
            }
            //TDebug.Log("已经下载"+bundleName);
            return isExists;
        }
        return false;
    }
    static void FreshBundleAssetUpdated(Dictionary<string, bool> updateDict)//根据UpdatedDict刷新
    {
        StringBuilder tempStr = new StringBuilder();
        foreach (var temp in UpdatedDict)
        {
            if (tempStr.Length > 0) tempStr.Append("|" + temp.Key);
            else tempStr.Append(temp.Key);
        }
        PlayerPrefs.SetString(BundleAssetUpdated, tempStr.ToString());
    }

    /// <summary>
    /// 当bundle更新过，但加载失败时，则会重置此bundle信息，下次玩家进入后会重新下载
    /// </summary>
    /// <param name="bundleName"></param>
    public static void ResetMissedBundle(List<string> bundleName)
    {
        string curMd5 = PlayerPrefs.GetString(SaveMD5, "");
        List<CompareItem> diffList = MD5Compare.CompareVersion(GetStreamingMd5(), curMd5);
        for (int i = 0; i < bundleName.Count; i++)
        {
            string curBundleName = bundleName[i].Replace("/", "\\");
            foreach (var item in diffList)
            {
                if (item != null)
                    item.IsUpdateOver = (item.ItemName != curBundleName);
            }
        }
        string newMd5 = MD5Compare.GetNewMd5Str(diffList);
        PlayerPrefs.SetString(SaveMD5, newMd5);
        if (ServerInfo.BundleVersion >= 1)
            PlayerPrefs.SetInt(AssetUpdate.BundleVersion + AppSetting.Version, ServerInfo.BundleVersion - 1);
        PlayerPrefs.Save();
    }

    public List<CompareItem> mDiffList;  //md5
    private bool mUpdateFinish;           //是否更新完成
    private int mNeedUpdateAmount;        //需要更新的总数
    private int mCurUpdateIndex;          //当前更新到的序号
    //private Window_LoadBar.AsyncData mAsyncData;
    private ThreadDownload mThreadDowner;
    private int mResAmountSize;


    /// <summary>
    /// 传入新的md5，将新旧md5进行对比后更新
    /// </summary>
    public void UpdateBundle(string newMd5 , System.Action overDeleg)
    {
        mUpdateFinish = false;
        bool haveNeedUpdate;
        mDiffList = CompareVersion(newMd5, out haveNeedUpdate);

        if (mThreadDowner == null)
        {
            GameObject g = new GameObject("ThreadDownloader");
            mThreadDowner = g.CheckAddComponent<ThreadDownload>();
        }

        mNeedUpdateAmount = 0;
        mCurUpdateIndex = 0;
        List<CompareItem> copyList = new List<CompareItem>();
        for (int i = 0; i < mDiffList.Count; i++)
        {
            if (mDiffList[i].NeedUpdate)
            {
                copyList.Add(mDiffList[i]);
            }
        }
        mNeedUpdateAmount = copyList.Count;
        mResAmountSize = 0;
        if (haveNeedUpdate)//进行更新
        {
            for (int i = 0; i < copyList.Count; i++)
            {
                mResAmountSize += copyList[i].FileSize;
            }
            if (mResAmountSize > 20000 && Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//20000k
            {
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(
                    delegate() { StartUpdate(copyList, overDeleg); }
                    , string.Format("您未连接WIFI,更新将消耗约{0}m流量,是否继续更新？", (mResAmountSize / 1000f).ToString("f2")),
                    Color.black);
            }
            else //使用wifi时
            { 
                StartUpdate(copyList, overDeleg);
            }
        }
        else
        {
            mUpdateFinish = true;
            overDeleg();
        }
    }
    void StartUpdate(List<CompareItem> itemList, System.Action callBack)
    {
        DoUpdate(itemList, FileUtils.ResURL, FileUtils.PersistentDataWritePath(FileUtils.GameResPath, ""), callBack);
    }


    /// <summary>
    /// 开启子线程，用回调递归，顺序下载bundle
    /// </summary>
    /// <param name="callBack">回调</param>
    void DoUpdate(List<CompareItem> itemList, string urlHead , string savePath , System.Action callBack)
    {
        Window_LoadBar.Instance.Fresh(mCurUpdateIndex / (float)mNeedUpdateAmount, string.Format("更新资源(共{0}k)", (mResAmountSize).ToString()));
        CompareItem updateItem = null;
        if (itemList.Count > 0) //获得第一个需要下载的资源
        {
            updateItem = itemList[0];
            itemList.RemoveAt(0);
        }

        if (updateItem != null)//如果仍有要更新的文件，递归循环
        {
            System.Action downOver = delegate()
            {
                TDebug.Log(string.Format("下载成功:{0}", updateItem.ItemName));
                updateItem.IsUpdateOver = true;
                PlayerPrefs.SetString(DownloadingKey, "");   //下载成功后，此资源不再断点续传
                PlayerPrefs.SetString(BundleAssetUpdated, string.Format("{0}|{1}", PlayerPrefs.GetString(BundleAssetUpdated, ""), updateItem.ItemName.Replace("\\","/")));
                UpdatedDict = null;
                mCurUpdateIndex++; DoUpdate(itemList, urlHead, savePath, callBack); 
            };
            bool needResume = (updateItem.ItemName == PlayerPrefs.GetString(DownloadingKey, ""));  //是否断点续传
            PlayerPrefs.SetString(DownloadingKey, updateItem.ItemName);
            try
            {
                //开始下载
                mThreadDowner.StartDownLoad(urlHead, savePath, updateItem.ItemName, needResume, downOver);
            }
            catch (System.Exception e)
            {
                TDebug.LogError("下载失败" + e.Message);
            }
        }
        else   //下载完成
        {
            if (callBack != null)
            {
                TDebug.Log("所有bundle更新完成");
                //mAsyncData.IsDone = true;
                mUpdateFinish = true;
                SaveVerion(true);
                callBack();
                callBack = null;
            } 
        }
    }


    /// <summary>
    /// 根据m_DiffList，在AssetbundleManager中进行更新
    /// </summary>
    static List<CompareItem> CompareVersion(string newMd5 , out bool haveUpdate)
    {
        List<CompareItem> diffList = new List<CompareItem>();
        string oldMd5 = PlayerPrefs.GetString(SaveMD5, "");//找到旧md5
        if (oldMd5.Equals(""))
        {
            oldMd5 = GetStreamingMd5();
            //TDebug.Log("初次进入，无版本信息" + oldMd5);
        }

        diffList = MD5Compare.CompareVersion(oldMd5, newMd5); //得到需更新的列表
        
        StringBuilder waitUpdate = new StringBuilder();
        for (int i = 0; i < diffList.Count; i++)
        {
            if (!diffList[i].NeedUpdate) continue;
            waitUpdate.Append(diffList[i].ItemName + "\r\n");
        }
        if (waitUpdate.ToString() != "")
        {
            TDebug.LogFormat("待更新资源{0}" , waitUpdate.ToString());
            haveUpdate = true;
        }
        else
        {
            TDebug.Log("md5比对后无待更新资源");
            haveUpdate = false;
        }
        return diffList;
    }

    //原始md5
    public static string GetStreamingMd5()
    {
        byte[] data = null;
        if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
            string asstsPath = FileUtils.StreamingAssetsPathReadPath(FileUtils.GameResPath, MD5Utils.VersionMd5TxtName);
            data = JavaHelper.Instance.GetStreamAssets(asstsPath);
        }
        else
        {
            string asstsPath = FileUtils.StreamingAssetsPathReadPath(FileUtils.GameResPath, MD5Utils.VersionMd5TxtName);
            data = File.ReadAllBytes(asstsPath);
        }
        return System.Text.Encoding.UTF8.GetString(data);
    }


    void SaveVerion(bool isStop)//更新版本信息====之后，需要加载完成一个就更新一个
    {
        TDebug.Log("更新储存的版本信息");
        if (mDiffList != null)
        {
            string newMd5 = MD5Compare.GetNewMd5Str(mDiffList);
            PlayerPrefs.SetString(SaveMD5, newMd5);
            PlayerPrefs.Save();
        }
        if (isStop && mThreadDowner != null && mThreadDowner.gameObject != null)
        {
            Destroy(mThreadDowner.gameObject);
        }
    }

    void OnApplicationQuit()  //如果在退出时没有更新完，保存已更新的
    {
        if (!mUpdateFinish)
        {
            SaveVerion(false);
        }
    }
    void OnApplicationPause(bool isPause)  //如果在退出时没有更新完，保存已更新的
    {
        if (!mUpdateFinish && isPause)
        {
            SaveVerion(false);
        }
    }
   










    #region 下载dll

    public static  string Flag = "1001";
    public void UpdateDll()
    {
        TDebug.LogFormat("=====更新游戏Flag：{0}" , Flag);
        if (mThreadDowner == null)
        {
            GameObject g = new GameObject("ThreadDownloader");
            mThreadDowner = g.CheckAddComponent<ThreadDownload>();
        }
        mNeedUpdateAmount = 1;
       
        //CompareItem dllItem = new CompareItem(AssemDll, "1", "2");
        //dllItem.FileSize = 800;
        StartCoroutine(LoadDll());
    }
    public IEnumerator LoadDll()
    {
        string path = AndroidDllPath();
        TDebug.LogFormat(string.Format("进行dll下载:{0}/{1}     |{2}", FileUtils.ResURL , AssemDllFileName, path));

        WWW www = new WWW(FileUtils.ResURL + "/" + AssemDllFileName);
        Window_LoadBar.Instance.Init(www, "正在更新" , null);//显示进度条界面
        yield return www;
        if ((www.error != null) || www.bytes == null || www.bytes.Length == 0)
        {
            TDebug.LogErrorFormat("下载错误: {0}",www.error);
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(Application.Quit, string.Format("下载新版本资源错误({0})",AppSetting.Version), Color.red);
            yield break;
        }
        byte[] bytes = www.bytes;
        MemoryStream zipStream = new MemoryStream(bytes);
        FileUtils.UnGzipDirectoryFile(zipStream, path);
        TDebug.LogFormat("是否存在：{0}", File.Exists(string.Format("{0}/{1}", path , AssemDllFileName)));
        //FileUtils.SaveBytes(path, AssemDllFileName, bytes);
        PlayerPrefs.SetFloat(AssetUpdate.GameVersion, (float)ServerInfo.GameVersion);
        PlayerPrefs.Save();

        TDebug.Log(bytes.Length + "更新完毕，即将重启游戏" + (float) ServerInfo.GameVersion);
        Window_LoadBar.Instance.Fresh(1f, "更新完毕，即将为您重启游戏");
        UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("更新完毕，即将为您重启游戏", Color.red);
        yield return new WaitForSeconds(1f);
        JavaHelper.Instance.AndroidRestart();
        www.Dispose();
    }

    public static string AndroidDllPath()
    {
        string datapath = Application.dataPath;
        int start = datapath.IndexOf("com.");
        int end = datapath.IndexOf("-");
        string packagename = datapath.Substring(start, end - start);
        return "/data/data/" + packagename + "/files";
    }

    

    public static bool IsOldDllExit() //当有旧版本dll存在，移除旧版本dll
    {
        if (PlatformUtils.EnviormentTy != EnviormentType.Android)
        {
            return false;
        }
        FileInfo file = new FileInfo(AndroidDllPath()+"/" + AssemDllFileName);
        if (file.Exists)
        {
            file.Delete();
            return true;
        }
        return false;
    }
    #endregion



    #region 更新链接

    public static void OpenAppStore()
    {
        try
        {
            string url="";
            if (PlatformUtils.EnviormentTy == EnviormentType.iOS)
            {
                url = ServerInfo.GetCurStoreUrl();
                //Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appID);
            }
            else
            {
                url = ServerInfo.GetCurStoreUrl();
                //Application.OpenURL("market://details?id=" + appID);
            }

            if (url.Length<=1)
            {
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("跳转应用商店失败，劳驾道友自行到应用商店更新游戏" , Color.red);
                return;
            }
            Application.OpenURL(url);
        }
        catch(Exception ex)
        {
            TDebug.LogErrorFormat("跳转商店失败：{0}" , ex.Message);
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("跳转应用商店失败，劳驾道友自行到应用商店更新游戏", Color.red);
        }
    }


    #endregion

}

