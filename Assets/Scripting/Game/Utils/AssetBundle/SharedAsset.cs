using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using System;
#endif
public class SharedAsset : MonoBehaviour
{
    //public static List<string> StartList = new List<string> { "ui/baseui", "ui/startui", "model/gamedata", "model/scene" };
    public static List<string> StartList = new List<string> { "ui/baseui", "ui/startui", "model/scene" };
    public const string VersionMd5TxtName     = "versionMd5.txt";


#if UNITY_EDITOR
    public static bool SimulateAssetBundleInEditor
    {
        get { return EditorPrefs.GetBool("SimulateAssetBundleInEditor", true); }
        set { EditorPrefs.SetBool("SimulateAssetBundleInEditor", value); }
    }
#endif
    public static SharedAsset Instance { get; private set; }

    public Dictionary<string, int> VersionDict = new Dictionary<string, int>();      //版本信息.根据DiffList更改
    public IAssetLoader AssetLoader;
    public AssetLoaderType LoaderType = AssetLoaderType.Bundle;
    public Dictionary<string, AssetBundle> WaitBundleDict = new Dictionary<string, AssetBundle>(); 
    public Dictionary<string, AssetBundle> BundleDict = new Dictionary<string, AssetBundle>();
    [HideInInspector] public AssetBundleManifest BundleManifest;
    void Awake()
    {
        Instance = this;

#if UNITY_EDITOR
        LoaderType = SimulateAssetBundleInEditor ? AssetLoaderType.TResources : AssetLoaderType.Bundle;
        if (LoaderType == AssetLoaderType.TResources)
        {
            AssetLoader = new AssetLoader_Res();
        }
        else
        {
            AssetLoader = new AssetLoader_Bundle();
        }
#else
        LoaderType = AssetLoaderType.Bundle;
        AssetLoader = new AssetLoader_Bundle();
#endif
        AssetLoader.Init();
        //StartCoroutine(AssetLoader.LoadAsyncUpdate());
    }

    void Update()
    {
        AssetLoaderPool.CheckGcCollect();
    }

    #region 加载Asset
    //[Obsolete]
    //public TLoader LoadAssetAsync(string bundleName, string assetName, System.Action<TLoader> del)
    //{
    //    return null;
    //}

    /// <summary>
    /// 返回的TLoader，必须在此物体使用完成或OnDestroy时Release
    /// </summary>
    public TLoader LoadAssetSync(string bundleName, string assetName)
    {
        TLoader loader = AssetLoader.LoadAssetSync(bundleName, assetName);
        if (!loader.IsCompleted) TDebug.LogError("加载错误" + assetName);
        return loader;
    }

    /// <summary>
    /// 获取某物体，对应TLoader立即释放。图片音效等资源不建议使用
    /// </summary>
    public Object LoadAssetSyncObj_ImmediateRelease(BundleType ty, string assetName)
    {
        TLoader loader = AssetLoader.LoadAssetSync(ty.BundleDictName(), assetName);
        if (!loader.IsCompleted) TDebug.LogError("加载错误" + assetName);
        Object t = loader.ResultObject as Object;
        loader.Release();
        return t;
    }

    /// <summary>
    /// 返回的TLoader，必须在此物体使用完成或OnDestroy时Release
    /// </summary>
    public TLoader LoadAssetSyncByType<T>(BundleType ty, string assetName)
    {
        TLoader loader = AssetLoader.LoadAssetSync(ty.BundleDictName(), assetName, typeof(T));
        if (!loader.IsCompleted || loader.ResultObject == null) TDebug.LogError("加载错误" + assetName);
        return loader;
    }


    /// <summary>
    /// 图集暂时使用后不释放
    /// 图集预物体名后面会+Prefab，以免与图片同名
    /// </summary>
    public Object LoadSpritePrefabObj(string assetName) 
    {
        TLoader loader = AssetLoader.LoadAssetSync(string.Format("{0}{1}", BundleType.SpritePrefabBundle.BundleDictName(), assetName.ToLower()), assetName+"Prefab");
        if (!loader.IsCompleted) TDebug.LogError("加载错误" + assetName);
        Object t = loader.ResultObject as Object;
        //loader.Release();
        return t;
    }

    public Object LoadAudioMgr()
    {
        TLoader loader = AssetLoader.LoadAssetSync(BundleType.AudioBundle.BundleDictName(), "AudioMgr");
        if (!loader.IsCompleted) TDebug.LogError("AudioMgr加载错误");
        Object t = loader.ResultObject as Object;
        loader.Release();
        //SharedAsset.Instance.UnloadBundle(BundleType.AudioBundle.BundleDictName() ,false);
        return t;
    }

    public AudioClip LoadAudioClip(string clipName)
    {
        clipName = clipName.ToLower();
        TLoader loader = AssetLoader.LoadAssetSync(string.Format("{0}{1}", BundleType.AudioClipBundle.BundleDictName(), clipName),clipName,typeof(AudioClip));
        if (!loader.IsCompleted) TDebug.LogError(string.Format("AudioClip:{0}加载错误", clipName));
        AudioClip t = loader.ResultObject as AudioClip;
        loader.Release();
        return t;
    }


    /// <summary>
    /// 返回的TLoader，必须在此物体OnDestroy时Release
    /// </summary>
    public TLoader LoadSpritePart<T>(string assetName)where T:Object
    {
        TLoader loader = AssetLoader.LoadAssetSync(string.Format("{0}{1}", BundleType.SpritePartBundle.BundleDictName(), assetName.ToLower()), assetName , typeof(T));
        if (!loader.IsCompleted || loader.ResultObject==null) TDebug.LogError("加载错误" + assetName);
        return loader;
    }

    //[Obsolete("使用GameAssetPool生成特效资源，特效资源需要绑定DestroySelf")]
    //public TLoader LoadEffect(string assetName)
    //{
    //    return null;
    //}

    #endregion


    public TLoader LoadInitRoot()
    {
        TLoader loader = LoadAssetSync(BundleType.InitRoot.BundleDictName(), "initRoot");
        return loader;
    }

    #region 加载Bundle
    public void LoadStartBundle(System.Action finishBack)
    {
        if (LoaderType == AssetLoaderType.TResources) { TDebug.Log("Simulation模式不进行版本检测"); if (finishBack != null) finishBack(); return; }
        
        Caching.maximumAvailableDiskSpace = 500 * 1024 * 1024;
        LoadBundleSync(StartList);
        if (finishBack != null) finishBack();
    }




    // 根据bundleName列表，进行加载
    public void LoadBundleSync(List<string> bundleNameList)
    {
        if (bundleNameList == null) return;
        if (BundleManifest == null) //加载manifest文件
        {
            string path = ""; 
            if (AssetUpdate.IsUpdated("Res"))
            {
                path=FileUtils.PersistentDataReadPath(FileUtils.GameResPath, "Res");
            }
            else
            {
                path=FileUtils.StreamingAssetsPathReadPath(FileUtils.GameResPath, "Res");
            }
            try
            {
                AssetBundle mainAb = AssetBundle.LoadFromFile(path);
                if (mainAb == null)
                {
                    TDebug.LogError("资源因为被删除,已经丢失" + path);
                    return;
                }
                AssetBundleManifest manifest = (AssetBundleManifest) mainAb.LoadAsset("AssetBundleManifest");
                BundleManifest = manifest;
                mainAb.Unload(false);
            }
            catch
            {
            }
        }
        for (int i = 0; i < bundleNameList.Count; i++)
        {

            //已经加载过，跳过
            if (BundleDict.ContainsKey(bundleNameList[i])) continue;
            if (WaitBundleDict.ContainsKey(bundleNameList[i])) continue;//以便循环加载依赖
            if (!WaitBundleDict.ContainsKey(bundleNameList[i])) WaitBundleDict.Add(bundleNameList[i] , null);
            

            //先判断依赖关系
            string[] depens = BundleManifest.GetAllDependencies(bundleNameList[i]);
            if (depens.Length > 0)
            {
                LoadBundleSync(new List<string>(depens));
            }
            string path = "";
            bool isUpdated = AssetUpdate.IsUpdated(bundleNameList[i]);
            //TDebug.Log("加载bundle：" + bundleNameList[i] + "   IsUpdated：" + isUpdated);
            if (isUpdated)//如果没有过更新，直接从streaming中下载
            {
                path=FileUtils.PersistentDataReadPath(FileUtils.GameResPath, bundleNameList[i]);
            }
            else
            {
                path=FileUtils.StreamingAssetsPathReadPath(FileUtils.GameResPath, bundleNameList[i]);
            }
            try
            {
                AssetBundle ab = AssetBundle.LoadFromFile(path);
                if (ab == null)
                {
                    TDebug.LogError("资源因为被删除,已经丢失" + path); return;
                }
                BundleDict.Add(bundleNameList[i], ab);
            }
            catch
            {
                TDebug.LogError(string.Format("加载bundle错误 {0}", bundleNameList[i]));
            }
        }
    }

    public void UnloadBundle(string bundleName, bool unloadForce = false) //bundle，需要用LoadBundle加载完成后使用
    {
        if (LoaderType == AssetLoaderType.TResources) return;
        if (BundleDict.ContainsKey(bundleName))
        {
            AssetBundle bundle = BundleDict[bundleName];
            BundleDict.Remove(bundleName);
            if (WaitBundleDict.ContainsKey(bundleName)) WaitBundleDict.Remove(bundleName);
            bundle.Unload(unloadForce);
        }
    }

    #endregion

    //清除所有加载的bundle，用于更新bundle之后
    public void ClearAll()
    {
        AssetLoaderPool.ClearAll();

        foreach (var temp in WaitBundleDict)
        {
            if (temp.Value!=null) temp.Value.Unload(true);
        }
        WaitBundleDict.Clear();
        foreach (var temp in BundleDict)
        {
            if (temp.Value != null) temp.Value.Unload(true);
        }
        BundleDict.Clear();
        BundleManifest = null;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
}

public enum BundleType
{
    StartBundle,
    SceneBundle,
    SpriteBundle,
    WindowBundle,
    AudioBundle,
    LuaScriptBundle,
    MainBundle,
    BaseBundle,
    GameDataBundle,
    EffectBundle,
    SpritePartBundle,
    SpritePrefabBundle,
    PartBundle,
    IllegalText,
    InitRoot,
    AudioClipBundle,
    LegalText
}


    
public static class BundleTypeExt
{
    public static string BundleDictName(this BundleType ty)
    {
        switch (ty)
        {
            case BundleType.StartBundle: return "ui/startui";
            case BundleType.SceneBundle: return "model/scene";
            case BundleType.GameDataBundle: return "model/gamedata";
            case BundleType.SpriteBundle: return "ui/sprite";
            case BundleType.AudioBundle: return "model/audio";
            case BundleType.LuaScriptBundle: return "ui/lua";
            case BundleType.MainBundle: return "ui/mainui";
            case BundleType.BaseBundle: return "ui/baseui";
            case BundleType.WindowBundle: return "ui/window/";    //   添加'/'代表底下放的是多个bundle
            case BundleType.EffectBundle: return "model/effect";
            case BundleType.SpritePartBundle: return "ui/spritepart/";
            case BundleType.SpritePrefabBundle: return "ui/spriteprefab/";
            case BundleType.PartBundle:return "ui/part/";
            case BundleType.InitRoot:return "ui/initroot";
            case BundleType.IllegalText: return "model/illegaltext";
            case BundleType.AudioClipBundle: return "model/audios/";
            case BundleType.LegalText: return "model/legaltext";
        } return "";
    }
}
