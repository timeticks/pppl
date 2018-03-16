using UnityEngine;
using System.Collections;
using System.IO;

using System;
using System.Collections.Generic;

public class LobbySceneMgr : SceneMgrBase {
    internal static Vector3? LastShipPos=null;
    public static LobbySceneMgr Instance { get; private set; }
    internal Transform m_MoveAim;
    internal List<int> m_FoundRemain = new List<int>();

    void Awake() 
    {
        Instance = this;
        _Init(this);
    }
	void Start () 
    {
        m_WorldCam.Init();
        if (AppBridge.Instance.AppScene.SceneData.m_ToScene == SceneType.LobbyScene)
        {
            StartCoroutine(WaitInit());
        }
    }

    IEnumerator WaitInit()
    {
        UIRootMgr.Instance.MyUICam.enabled = false;
        while (Time.timeScale == 0 || UIRootMgr.LobbyUI==null) yield return 0;
        UIRootMgr.LobbyUI.Init();
        UIRootMgr.Instance.MyUICam.enabled = true;
        //bool haveHangEvent = true;
        //yield return new WaitForSeconds(0.5f);
        //if (haveHangEvent)
        //    UIRootMgr.Instance.OpenWindow<Window_HangInfo>(WinName.Window_HangInfo).OpenWindow();
    }



    void Init()  //初始化玩家和AI
    {
        
    }


}
