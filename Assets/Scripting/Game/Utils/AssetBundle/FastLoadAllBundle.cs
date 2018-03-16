using UnityEngine;
using System.Collections;
using System.IO;

using System.Collections.Generic;

/// <summary>
/// 跳开开始场景，直接加载Bundle
/// 此脚本Start将优先执行
/// </summary>
public class FastLoadAllBundle : MonoBehaviour 
{
    static bool IsLoaded;
    public bool m_NeedLoad;
    public SceneType m_SceneType;
    private bool m_isLoading = false;
    public static FastLoadAllBundle a;

    void Awake()
    {
        a = this;
    }


	void Start ()
    {
        if (m_NeedLoad && AppBridge.Instance == null)
        {
            LoadAll();
        }
        IsLoaded = true;
	}

    void LoadAll()
    {
        //GameObject app = Instantiate(Resources.Load("AppBridge")) as GameObject;
        //AppBridge.Instance.AppScene.SceneData.SetCurScene(m_SceneType, 0);
        //m_isLoading = true;
        //Time.timeScale = 0;
        //System.Action del = delegate()
        //{
        //    LoadOver();
        //};
        //SharedAsset.Instance.LoadStartBundle(del);
    }

    void LoadOver()
    {
        System.Action del = delegate()
        {
            //AppData.Instance.MyPlayerData = GamePlayer.GetTest();

            Object uiRootObj = SharedAsset.Instance.LoadAssetSyncObj_ImmediateRelease(BundleType.StartBundle, "UIRoot");
            GameObject uiRoot = Instantiate(uiRootObj) as GameObject;
            MainUIMgrType type = MainUIMgrType.StartSceneMainUIMgr;
            switch (m_SceneType)
            {
                case SceneType.StartScene: 
                    type = MainUIMgrType.StartSceneMainUIMgr;
                    UIRootMgr.Instance.InitMainUI<StartSceneMainUIMgr>(type);
                    break;

                case SceneType.LobbyScene: 
                    type = MainUIMgrType.LobbySceneMainUIMgr; 
                    UIRootMgr.Instance.InitMainUI<LobbySceneMainUIMgr>(type);
                    break;

                case SceneType.BattleScene: 
                    type = MainUIMgrType.BattleSceneMainUIMgr;
                    //AppData.Instance.BattleData = BattleData.GetTest();
                    //AppData.Instance.BattleData.BattleType = BattleType.Normal;
                   
                    UIRootMgr.Instance.InitMainUI<BattleSceneMainUIMgr>(type);
                    break;
            }
            m_isLoading = false;
            Time.timeScale = 1;
            TDebug.Log("加载所有资源完成");
           // UIRootMgr.MainUI.m_Start.OpenLoginWindow();
        };
        GameData.Instance.LoadAllDataBase();
    }

    void OnGUI()
    {
        if (m_isLoading)
        {
            GUIStyle st = new GUIStyle();
            st.fontSize = 60;
            //st.normal.textColor = Color.white;
            GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "正在加载资源...", st);
        }
        
    }
}
