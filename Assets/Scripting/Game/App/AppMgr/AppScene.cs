using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AppScene : MonoBehaviour 
{
    public delegate void SceneAsyncCallBack(AsyncOperation async);
    public AsyncOperation SceneAsync; //当前的异步进度
    public SceneData SceneData;
    public bool IsLoading;
    public void Init()
    {
        SceneData = new SceneData();
    }

    public void LoadScene(SceneType aimType , string aimSceneName, bool isAsync)
    {
        if (IsLoading) { TDebug.LogError("多次加载场景"+aimSceneName); return; }
        UIRootMgr.Instance.MyUICam.cullingMask = 1 << LayerMask.NameToLayer("Nothing");
        if (SceneMgrBase.InstanceBase != null && SceneMgrBase.InstanceBase.m_WorldCam != null) SceneMgrBase.InstanceBase.m_WorldCam.myCamera.cullingMask = 1 << LayerMask.NameToLayer("Nothing");

        IsLoading = true;
        AppBridge.Instance.AppScene.SceneData.SetCurScene(SceneType.LoadingScene , 0);
        //进行资源清除
        if (AppBridge.Instance.CurScene != aimType)
        {
        }
        if (isAsync)
        {
            StartCoroutine(LoadSceneByLoading(aimSceneName, aimType));
        }
        else
        {
            LoadSceneAsync(aimSceneName);
            StartCoroutine(SyncLoadSceneCor(aimType));
            //SceneManager.LoadScene(aimSceneName);
        }
    }
    IEnumerator SyncLoadSceneCor(SceneType aimType)
    {
        yield return SceneAsync;
        LoadSceneOver((System.Object)aimType);
    }

    public void LoadSceneAsync(SceneType aimType, string aimSceneName)
    {
        UIRootMgr.Instance.MyUICam.cullingMask = 1 << LayerMask.NameToLayer("Nothing");
        if (SceneMgrBase.InstanceBase!=null && SceneMgrBase.InstanceBase.m_WorldCam != null) SceneMgrBase.InstanceBase.m_WorldCam.myCamera.cullingMask = 1 << LayerMask.NameToLayer("Nothing");
        //进行资源清除
        if (AppBridge.Instance.CurScene != aimType)
        {
        }
        LoadSceneAsync(aimSceneName);
        //m_SceneAsync = SceneManager.LoadSceneAsync(aimSceneName);
        StartCoroutine(WaitAsync(aimType, SceneAsync));
    }

    void LoadSceneAsync(string aimSceneName)
    {
#if UNITY_EDITOR
        if (SharedAsset.Instance.LoaderType == AssetLoaderType.TResources)
        {
            string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(BundleType.SceneBundle.BundleDictName(), aimSceneName);
            if (levelPaths.Length == 0) SceneAsync = SceneManager.LoadSceneAsync(aimSceneName);
            else SceneAsync = UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode(levelPaths[0]);
        }
        else
        {
            SceneAsync = SceneManager.LoadSceneAsync(aimSceneName);
        }
        return;
#else
        SceneAsync = SceneManager.LoadSceneAsync(aimSceneName);
#endif
    }

    IEnumerator WaitAsync(SceneType aimScene,AsyncOperation async)
    {
        while (async!=null && !async.isDone)
        { 
            yield return null; 
        }
        LoadSceneOver((System.Object)aimScene);
    }

    IEnumerator LoadSceneByLoading(string aimScene , SceneType sceneType)//使用过渡场景异步加载场景
    {
       //UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsync()
        //SceneManager.LoadScene("LoadingScene");
        LoadSceneAsync("LoadingScene");
        yield return SceneAsync;

        UIRootMgr.Instance.InitMainUI<LoadSceneMainUIMgr>(MainUIMgrType.LoadSceneMainUIMgr);
        LoadSceneAsync(aimScene);
        //m_SceneAsync = SceneManager.LoadSceneAsync(aimScene);
        System.Action<object> del = delegate(System.Object o)
        {
            LoadSceneOver(o);
        };
        UIRootMgr.MainUI.m_Load.ShowLoadingAsync(SceneAsync, del, sceneType);
    }


    /// <summary>
    /// 场景加载完成，
    /// </summary>
    /// <param name="scene"></param>
    public void LoadSceneOver(System.Object scene)
    {
        IsLoading = false;
        SceneType curScene = (SceneType)scene;
        AppBridge.Instance.AppScene.SceneData.SetCurScene(curScene , AppBridge.Instance.AppScene.SceneData.m_ToSceneId);
        GameAssetsPool.Instance.ClearAll();
        switch (curScene)
        {
            case SceneType.StartScene:
                break;

            case SceneType.LobbyScene:
                TDebug.Log("加载完成，进行资源填装");
                //UnityEngine.Object oceanObj = SharedAsset.EffectBundle.LoadAsset("AreaOcean");
                //GameObject ocean = Instantiate(oceanObj) as GameObject;
                UIRootMgr.Instance.InitMainUI<LobbySceneMainUIMgr>(MainUIMgrType.LobbySceneMainUIMgr);
                AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.EnterLobby));
                break;

            case SceneType.BattleScene:
                UIRootMgr.Instance.InitMainUI<BattleSceneMainUIMgr>(MainUIMgrType.BattleSceneMainUIMgr);
                AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.EnterAnyBattle));
                break;

            case SceneType.LoadingScene:
                //初始化loading窗口，并开始异步加载场景
                break;
        }
    }

}



public enum SceneType : byte//场景
{
    StartScene = 0,  //开始场景
    LobbyScene = 1,   //海域大地图
    LoadingScene = 2,
    BattleScene = 3,
}





public class SceneData   //场景跳转信息
{
    //当前场景
    public SceneType m_CurScene { get { return m_curScene; } }
    private SceneType m_curScene;
    public int m_CurSceneId { get { return m_curSceneId; } } //海域id、港口id、遗迹id、遗迹关卡id、对战地图id
    private Eint m_curSceneId = 0;
    public void SetCurScene(SceneType curScene, int sceneId)  //只能在转场景处调用
    {
        m_curScene = curScene;
        m_curSceneId = sceneId;
        AppBridge.Instance.SceneStr = curScene.ToString() + sceneId;
    }


    public SceneType m_FromScene;  //源场景
    public SceneType m_ToScene;    //跳转目标场景
    public Eint m_ToSceneId;       //目标场景id

    public Dictionary<SceneType, Eint> m_LastSceneDict = new Dictionary<SceneType, Eint>();

    public SceneData()
    {
        SetCurScene(SceneType.StartScene, 0);
    }

    /// <summary>
    /// 记录场景切换信息
    /// </summary>
    public void SceneSwitch(SceneType fromScene, SceneType toScene, int toSceneId)
    {
        m_FromScene = fromScene;
        m_ToScene = toScene;
        m_ToSceneId = toSceneId;
    }

    /// <summary>
    /// 记录上一个场景信息
    /// </summary>
    public void AddLastScene(SceneType scene, int id)
    {
        if (m_LastSceneDict.ContainsKey(scene))
        {
            m_LastSceneDict[scene] = id;
        }
        else
        {
            m_LastSceneDict.Add(scene, id);
        }
    }
}