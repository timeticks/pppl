using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LoadDllHelper:MonoBehaviour
{
    public static string gameLogicName = "l";

    private static System.Action<byte[] , byte[]> LoadDllCallBack;
    public void LoadDll(System.Action<byte[], byte[]> loadDllCallBack)
    {
        LoadDllCallBack = loadDllCallBack;
        StartCoroutine(CheckLogicVersion());
    }

    public IEnumerator CheckLogicVersion()
    {
        byte[] data = null;
        string asstsPath = FileBaseUtils.StreamingAssetsPathReadPath("/", "AppSetting.cfg");
        if (PlatformUtils.EnviormentTy == EnviormentType.Android)
        {
            TDebug.Log("Android平台编译的代码!!!!");
            data = JavaHelper.Instance.GetStreamAssets(asstsPath);
        }
        else
        {
            TDebug.LogFormat("IOS PC 平台编译的代码!!!!");
            data = File.ReadAllBytes(asstsPath);
        }
        string streamCfg = Encoding.UTF8.GetString(data);
        Dictionary<string, object> settingDict = LitJson.JsonMapper.ToObject<Dictionary<string, object>>(streamCfg);
        float localLogicVer = float.Parse(settingDict["LogicVersion"].ToString());

        string baseUrl = string.Format("http://{0}/{1}/{2}", settingDict["AppHost"], PlatformUtils.PlatformTy, settingDict["Version"]); ;
        string appInfoUrl  = string.Format("{0}/AppInfo.html", baseUrl);
        Window_PureLoadBar loadBar = GetLoadBar();

        WWW www = new WWW(appInfoUrl);
        while (!www.isDone && string.IsNullOrEmpty(www.error))
        {
            loadBar.Fresh(www.progress, "检查游戏信息");
            yield return null;
        }
        if (!string.IsNullOrEmpty(www.error))
        {
            loadBar.Fresh(0, string.Format("检查游戏信息失败:{0}", www.error));
            yield break;
        }
        string appInfoCfg = Encoding.UTF8.GetString(www.bytes);
        Hashtable appInfoDict = LitJson.JsonMapper.ToObject<Hashtable>(appInfoCfg);
        float newestLogicVer = float.Parse(appInfoDict["LogicVersion"].ToString());

        string persistDllPath = FileBaseUtils.PersistentReadPath(FileBaseUtils.GameLogicPath, string.Format("{0}/{1}", newestLogicVer, gameLogicName));
        bool isExsit = File.Exists(persistDllPath);
        Debug.Log(string.Format("local:{0}|newest:{1}|isExsit:{2}", localLogicVer, newestLogicVer, isExsit));
        if (newestLogicVer > localLogicVer && !isExsit)//需要更新或重新下载逻辑
        {
            FileBaseUtils.DeleteDirectory(FileBaseUtils.PersistentReadPath(FileBaseUtils.GameLogicPath,""));

            string dllUrl = string.Format("{0}/{1}", baseUrl, gameLogicName+".bytes");

            www = new WWW(dllUrl);
            float curTime = 0;
            while (!www.isDone && string.IsNullOrEmpty(www.error))
            {
                loadBar.Fresh(www.progress, string.Format("更新游戏信息 {0}%",(www.progress*100).ToString("f0")));
                yield return null;
                curTime += Time.deltaTime;
            }
            if (!string.IsNullOrEmpty(www.error))
            {
                loadBar.Fresh(0, string.Format("更新游戏失败，请检查网络并重进游戏:{0}", www.error));
                Debug.LogError(string.Format("更新游戏失败，请检查网络并重进游戏:{0}", www.error));
                yield break;
            }
            Stream zipData = new MemoryStream(www.bytes);
            www.Dispose();
            FileBaseUtils.UnGzipDirectoryFile(zipData, FileBaseUtils.PersistentReadPath(FileBaseUtils.GameLogicPath, newestLogicVer.ToString()));
            //FileBaseUtils.SaveBytes(path, www.bytes);
        }
        string dllPath = "";
        if (newestLogicVer > localLogicVer)
        {
            dllPath = persistDllPath;
        }
        else
        {
            dllPath = FileBaseUtils.StreamingAssetsPathReadPath("/", gameLogicName);
            if (!File.Exists(dllPath))   //如果不存在，直接加载assembly-csharp
            {
                LoadDllCallBack(null, null);
                Destroy(loadBar.gameObject);
                yield break;
            }
        }
        Debug.Log(dllPath);
        byte[] dataBytes = FileBaseUtils.SyncReadStreamFile(dllPath);
        LoadDllCallBack(dataBytes,null);
        PlayerPrefs.SetFloat("LogicVersion", newestLogicVer);
        Destroy(loadBar.gameObject);
    }

     Window_PureLoadBar GetLoadBar()
    {
        UnityEngine.Object initObj = Resources.Load("InitRoot");
        GameObject initRoot = Instantiate(initObj) as GameObject;
        initRoot.name = "FirstInitRoot";
        Window_PureLoadBar win = initRoot.AddComponent<Window_PureLoadBar>();
         win.Init();
        return win;
    }

}
