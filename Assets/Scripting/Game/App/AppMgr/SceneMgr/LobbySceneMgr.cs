using UnityEngine;
using System.Collections;
using System.IO;

using System;
using System.Collections.Generic;

public class LobbySceneMgr : SceneMgrBase {
    internal static Vector3 LastShipPos = Vector3.one*10000;
    public static LobbySceneMgr Instance { get; private set; }
    //internal Transform m_MoveAim;
    internal List<int> m_FoundRemain = new List<int>();

    void Awake() 
    {
        Instance = this;
        _Init(this);
    }
	void Start () 
    {
        //m_WorldCam.Init();
        if (AppBridge.Instance.AppScene.SceneData.m_ToScene == SceneType.LobbyScene)
        {
            //UIRootMgr.Instance.TopBlackMask = true;
        }
        UIRootMgr.LobbyUI.Init();
        //UIRootMgr.Instance.TopBlackMask = false;
    }

    void WaitInit()
    {
    }


    void Init()  //初始化玩家和AI
    {
        
    }


}
