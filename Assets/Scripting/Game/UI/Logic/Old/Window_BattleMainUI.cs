using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class Window_BattleMainUI : MonoBehaviour 
{
    public List<Sprite> m_SpdNumList;
    public Button m_Button_AutoBattle;
    public Image m_Image_UpTimeSpd;     //加速
    
	void Start () 
    {
	
	}

    internal int m_TimeScale=1;
    public void BtnEvt_UpTime()
    {
        float maxTimeScale = 2.5f;
#if UNITY_EDITOR
        if (m_TimeScale == 3) {
            Time.timeScale = 9;
            m_TimeScale = 4;
            m_Image_UpTimeSpd.sprite = m_SpdNumList[m_TimeScale - 1];
            return;
        }
#endif
        if (m_TimeScale < 3) { m_TimeScale++; }
        else m_TimeScale = 1;
        Time.timeScale = Mathf.Clamp(m_TimeScale * 0.8f, 1, maxTimeScale);
        m_Image_UpTimeSpd.sprite = m_SpdNumList[m_TimeScale - 1];
        m_Image_UpTimeSpd.SetNativeSize();
        //m_Text_TimeUp.text = "加速X" + Time.timeScale.ToIntRound();
    }

    public bool m_IsAutoBattling = false;
    public void BtnEvt_AutoBattle()//自动战斗
    {
        if (m_IsAutoBattling)
        {
            //List<RoleBattle_Base> friendList = BattleSceneMgr.Instance.FindRoleList(AppData.Instance.BattleData.m_MyTeamId, false);//找到自身所有角色
            //for (int i = 0; i < friendList.Count; i++)
            //{
            //    //friendList[i].SetAiCtrl(false);
            //}
        }
        else
        {
            //List<RoleBattle_Base> friendList = BattleSceneMgr.Instance.FindRoleList(AppData.Instance.BattleData.m_MyTeamId, false);
            //for (int i = 0; i < friendList.Count; i++)
            //{
            //    //friendList[i].SetAiCtrl(true);
            //}
        }
        m_IsAutoBattling = !m_IsAutoBattling;
        m_Button_AutoBattle.image.color = m_IsAutoBattling ? m_Button_AutoBattle.colors.disabledColor : m_Button_AutoBattle.colors.normalColor;
    }


    public void BtnEvt_ExitBattle()
    {
        AppBridge.Instance.LoadScene(SceneType.LobbyScene, true);
    }
}
