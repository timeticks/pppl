using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AppScene : MonoBehaviour
{
    public const float MIN_LOAD_TIME = 1.3f; //最小转场景时间

    public delegate void SceneAsyncCallBack(AsyncOperation async);
    public AsyncOperation SceneAsyncData; //当前的异步进度
    public SceneData SceneData;
    public bool IsLoading=false;
    private SceneType mCurSceneType;
    private float mCurAsyncTime = 0f;
    private int mLoadStep;


    public void Init()
    {
        SceneData = new SceneData();
    }

//    #region 同步加载场景
//    public void LoadSceneSync(SceneType aimType , string aimSceneName)
//    {
//        if (IsLoading) { TDebug.LogError("多次加载场景"+aimSceneName); return; }
//        UIRootMgr.Instance.TopBlackMask = true;
//        if (SceneMgrBase.InstanceBase != null && SceneMgrBase.InstanceBase.m_WorldCam != null) SceneMgrBase.InstanceBase.m_WorldCam.myCamera.cullingMask = 1 << LayerMask.NameToLayer("Nothing");
//        mCurSceneType = aimType;
//        IsLoading = true;
//        AppBridge.Instance.AppScene.SceneData.SetCurScene(SceneType.LoadingScene , 0);
//        //进行资源清除
//        if (AppBridge.Instance.CurScene != aimType)
//        {
//        }

//#if UNITY_EDITOR
//        if (SharedAsset.Instance.LoaderType == AssetLoaderType.TResources)
//        {
//            string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(BundleType.SceneBundle.BundleDictName(), aimSceneName);
//            if (levelPaths.Length == 0) SceneManager.LoadScene(aimSceneName);
//            else UnityEditor.EditorApplication.LoadLevelInPlayMode(levelPaths[0]);
//        }
//        else
//        {
//            SceneManager.LoadScene(aimSceneName);
//        }
//#else
//        SceneManager.LoadScene(aimSceneName);
//#endif
//        LoadSceneOver(aimType);
//    }
//    #endregion


    //IEnumerator LoadSceneByLoading(string aimScene , SceneType sceneType)//使用过渡场景异步加载场景
    //{
    //    LoadSceneAsync("LoadingScene");
    //    yield return SceneAsync;

    //    UIRootMgr.Instance.InitMainUI<LoadSceneMainUIMgr>(MainUIMgrType.LoadSceneMainUIMgr);
    //    LoadSceneAsync(aimScene);
    //    //m_SceneAsync = SceneManager.LoadSceneAsync(aimScene);
    //    System.Action<object> del = delegate(System.Object o)
    //    {
    //        LoadSceneOver((SceneType)o);
    //    };
    //    UIRootMgr.MainUI.m_Load.ShowLoadingAsync(SceneAsync, del, sceneType);
    //}



    void LateUpdate() //使用lateupdate，以免其他根据SceneAsync进行进度显示的物体，显示不完就被LoadSceneOver销毁了
    {
        if (SceneAsyncData != null && IsLoading)
        {
            mCurAsyncTime += Time.deltaTime;
            float pct = Mathf.Min(SceneAsyncData.progress, mCurAsyncTime / MIN_LOAD_TIME);
            Window_LoadBar.Instance.Fresh(pct, "加载游戏场景");
            if (mCurSceneType == SceneType.LobbyScene)
            {
                if (pct > 0.5f && mLoadStep == 0)
                {
                    mLoadStep = 1;
                    //进行预备加载
                    SharedAsset.Instance.LoadAssetSyncObj_ImmediateRelease(BundleType.MainBundle, MainUIMgrType.LobbySceneMainUIMgr.ToString());
                }
                if (SceneAsyncData.isDone && mCurAsyncTime >= MIN_LOAD_TIME)
                {
                    LoadSceneOver(mCurSceneType);
                }
            }
            else 
            {
                if (SceneAsyncData.isDone)
                {
                    LoadSceneOver(mCurSceneType);
                }
            }
        }
    }
    public void LoadSceneAsync(SceneType aimType, string aimSceneName)
    {
        if (IsLoading) { TDebug.LogErrorFormat("多次加载场景{0}",aimSceneName); return; }
        mLoadStep = 0;
        IsLoading = true;
        mCurSceneType = aimType;
        //if (SceneMgrBase.InstanceBase!=null && SceneMgrBase.InstanceBase.m_WorldCam != null) SceneMgrBase.InstanceBase.m_WorldCam.myCamera.cullingMask = 1 << LayerMask.NameToLayer("Nothing");
        //进行资源清除
        if (AppBridge.Instance.CurScene != aimType)
        {
        }
        LoadSceneAsync(aimSceneName);
    }


    void LoadSceneAsync(string aimSceneName)
    {
        mCurAsyncTime = 0;
        if (PlatformUtils.EnviormentTy == EnviormentType.Editor 
            && SharedAsset.Instance.LoaderType == AssetLoaderType.TResources)
        {
            string[] levelPaths = EditorUtils.GetAssetPathsFromAssetBundleAndAssetName(BundleType.SceneBundle.BundleDictName(), aimSceneName);
            if (levelPaths.Length == 0) SceneAsyncData = SceneManager.LoadSceneAsync(aimSceneName);
            else SceneAsyncData = EditorUtils.LoadLevelAsyncInPlayMode(levelPaths[0]);
        }
        else
        {
            SceneAsyncData = SceneManager.LoadSceneAsync(aimSceneName);
        }
    }



    /// <summary>
    /// 场景加载完成处理
    /// </summary>
    /// <param name="scene"></param>
    public void LoadSceneOver(SceneType curScene)
    {
        mLoadStep = 0;
        IsLoading = false;
        AppBridge.Instance.AppScene.SceneData.SetCurScene(curScene , AppBridge.Instance.AppScene.SceneData.m_ToSceneId);
        GameAssetsPool.Instance.ClearAll();
        switch (curScene)
        {
            case SceneType.StartScene:
                break;

            case SceneType.LobbyScene:
                Window_LoadBar.Instance.SetFalse();
                TDebug.Log("加载完成，进行资源填装");
                //UnityEngine.Object oceanObj = SharedAsset.EffectBundle.LoadAsset("AreaOcean");
                //GameObject ocean = Instantiate(oceanObj) as GameObject;
                UIRootMgr.Instance.InitMainUI<LobbySceneMainUIMgr>(MainUIMgrType.LobbySceneMainUIMgr);
                GameObject obj = new GameObject("SceneMgr");
                obj.CheckAddComponent<LobbySceneMgr>().m_SceneType = SceneType.LobbyScene;
                AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.EnterLobby));
                break;

            case SceneType.BattleScene:
                Window_LoadBar.Instance.SetFalse();
                UIRootMgr.Instance.InitMainUI<BattleSceneMainUIMgr>(MainUIMgrType.BattleSceneMainUIMgr);
                AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.EnterAnyBattle));
                break;

            case SceneType.LoadingScene:
                //初始化loading窗口，并开始异步加载场景
                break;
        }
    }

}



public enum SceneType//场景
{
    StartScene = 0,  //开始场景
    LobbyScene = 1,   //
    LoadingScene = 2,
    BattleScene = 3,
}





public class SceneData   //场景跳转信息
{

    public SceneType m_FromScene;  //源场景
    public SceneType m_ToScene;    //跳转目标场景
    public Eint m_ToSceneId;       //目标场景id

    public Dictionary<SceneType, int> m_LastSceneDict = new Dictionary<SceneType, int>();

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