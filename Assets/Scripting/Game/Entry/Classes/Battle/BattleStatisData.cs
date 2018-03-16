using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 战斗统计信息
/// </summary>
public class BattleStatisData
{
    public Eint m_MyAll = 0;
    public Eint m_EnemyAll = 0;
    public Eint m_MyDead = 0;
    public Eint m_EnemyDead = 0;
    public Eint m_UseRound = 0;

    public static BattleStatisType GetTyStr(string typeStr)
    {
        return (BattleStatisType)System.Enum.Parse(typeof(BattleStatisType), typeStr);
    }

    public int GetStatisItem(string typeStr)
    {
        return GetStatisItem(GetTyStr(typeStr));
    }
    public int GetStatisItem(BattleStatisType statisType)//得到统计信息
    {
        switch (statisType)
        {
            case BattleStatisType.EnemyInLive:
                return m_EnemyAll - m_EnemyDead;

            case BattleStatisType.Round:
                return m_UseRound;

            case BattleStatisType.MyDead:
                return m_MyDead;

            case BattleStatisType.MyInLive:
                return m_MyAll - m_MyDead;
        }
        return 0;
    }
}
