using UnityEngine;
using System.Collections;

public class BattleSceneMainUIMgr : BaseMainUIMgr
{
    //public Window_BattleNaviConsole m_Window_RoleConsole
    //{
    //    get{
    //        if (AppBridge.Instance.m_CurScene == SceneType.BattleSceneNavi) { return m_Window_NaviConsole; }
    //        else return m_Window_ShipConsole;
    //    }
    //}
    private BattleSceneMainUIMgrView m_myView;
    
    void Awake()
    {
        m_myView = GetComponent<BattleSceneMainUIMgrView>();
    }
    public void StartBattle()
    {
        //m_myView.m_MainUI.gameObject.SetActive(true);
    }


    public override void _Init()
    {
        base.init();
    }


	void Start () {
	
	}

}
