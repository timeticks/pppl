using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.InteropServices;
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

    public SceneType CurScene { get { return AppScene.SceneData.m_CurScene; } }        //当前在哪个场景
    public string SceneStr;

    public SceneData SceneData { get { return AppScene.SceneData; } }
    [HideInInspector]
    public AppScene AppScene;
    [HideInInspector]
    public AppEvtMgr EvtMgr;         //事件管理器

    private int mSetFrame;
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
    }

    public static void InitSetting()
    {
        mBackgroundRate = GameLauncher.MAX_FRAME_RATE;
#if UNITY_IPHONE  || UNITY_ANDROID
        mBackgroundRate = 1;
#endif

#if UNITY_ANDROID   //设置处理分辨率
        SetResolution(mAndroidMaxSolution);
#endif
    }


    void Start()
    {
        //MemoryModity.callback = new MemoryModitiedCallback(MemoryModify);
    }
    public static void SetResolution(int width)
    {
        if (Screen.currentResolution.width <= width) return;
        float ratio = (float)Screen.height / Screen.width;
        Screen.SetResolution(width, (int)(ratio * width), true);
        
    }

    public void MemoryModify()
    {
        TDebug.LogError("内存修改");
    }

    void Update()
    {
        AppEvtMgr.Instance.Update();
        //if (Application.runInBackground)
        //{
        //    if (mSetFrame == 0)
        //    {
        //        Application.targetFrameRate = mBackgroundRate;
        //        mSetFrame = 1;
        //    }
        //}
        //else
        //{
        //    if (mSetFrame == 1)
        //    {
        //        Application.targetFrameRate = GameLaucher.MAX_FRAME_RATE;
        //        mSetFrame = 0;
        //    }
        //}


        if (Time.frameCount % 10000 == 0)
        {
            System.GC.Collect();
        }
        
        if (Input.GetKey(KeyCode.Escape))
        {
            if (UIRootMgr.Instance != null) 
            {
                UIRootMgr.Instance.OpenWindow<Window_ExitGame>(WinName.Window_ExitGame, CloseUIEvent.None).OpenWindow();
            }
        }
    }



    public void DoAndirLog(string actionStr)
    {
        TDebug.Log(actionStr);
        if (actionStr == "exitGame")
        {
            if (UIRootMgr.Instance != null && UIRootMgr.Instance.GetCurMainUI() != null)
            {
                if (UIRootMgr.Instance.GetOpenListWindow(WinName.Window_ExitGame) == null)
                    UIRootMgr.Instance.OpenWindow<Window_ExitGame>(WinName.Window_ExitGame, CloseUIEvent.None)
                        .OpenWindow();
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
            else { TDebug.LogError("unable to refresh shader: " + shaderName + " in material " + m.name); }
        }
    }

    /// <summary>
    /// 加载到某场景，此接口用于除开始场景的所有需要转场景的地方
    /// 是否需要去loading场景进行异步加载="needAsync"
    /// </summary>
    public void LoadScene(SceneType aimScene , bool isAsync , int toSceneId = 0)
    {
        string aimSceneName = aimScene.ToString();
        AppBridge.Instance.AppScene.SceneData.SceneSwitch(AppBridge.Instance.AppScene.SceneData.m_CurScene, aimScene, toSceneId);
        AppBridge.Instance.AppScene.SceneData.AddLastScene(AppBridge.Instance.AppScene.SceneData.m_CurScene, AppBridge.Instance.AppScene.SceneData.m_CurSceneId);
        if (aimScene == SceneType.LobbyScene)
        {
            //Hashtable tab = GameData.Instance.GetData(DataName.MapArea, toSceneId.ToString());
            //aimSceneName = TUtility.TryGetValueStr(tab, "Scene", "");
            aimSceneName = "LobbyScene";
        }
        AppScene.LoadScene(aimScene, aimSceneName, isAsync); 
    }
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


    void OnApplicationFocus(bool isFocus)
    {
        if (isFocus)
        {
            SetResolution(mAndroidMaxSolution);
        }
        else
        {
            if (UIRootMgr.Instance!=null) UIRootMgr.Instance.CloseDisableWin(3);
            System.GC.Collect();
        }
#if !UNITY_EDITOR
        if (isFocus)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 1;
        }
#endif
    }

    #endregion
}


