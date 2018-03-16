using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 战斗回合管理器
/// </summary>
public class BattleRoundMgr : MonoBehaviour 
{
    public WaitNextData NextWait = new WaitNextData();//是否可以进行下一步的标志，为0时可以进行下一步
    //public BattleRecordData RecrodData { get { return AppData.Instance.BattleData.RecordData; } } //回合信息

    RoleBattle_Base mCurRole;
    bool m_isOver;   //是否结束
    bool mIsWaitToNext = false;
    public void Init()
    {
        m_isOver = false;
    }


    void Update()
    { 
        if(mIsWaitToNext)
        {
            if (NextWait.Count == 0)
            { 
            }
        }
    }
    
    public void NextRound()
    {
        
    }

    public void NextAction()
    { 
    }


    /// 得到新的一轮的可行动对象
    List<RoleBattle_Base> GetNewRoundRoleList()
    {
        return null;
    }

    public bool CheckIsBattleOver()//检查是否战斗结束
    {
        return false;
    }


    bool IsTeamAllRoleDead(ref bool isMyWin)//某队伍所有角色死亡
    {
        //bool isEnemyAllDead = true;
        //bool isMyAllDead = true;
        
        return false;
    }




}



