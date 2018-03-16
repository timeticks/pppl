using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum BattleType:byte   //战斗模式
{
    None,
    Enemy_Auto_Fight,     //敌方挑战自动切磋
    Enemy_Auto_Battle,    //敌方挑战自动决斗
    My_Auto_Fight,        //我方挑战自动切磋
    My_Auto_Battle,       //
    Enemy_Hand_Fight,   //普通战斗
    Enemy_Hand_Battle,
    My_Hand_Fight,
    My_Hand_Battle,
}

public enum PVESceneType : byte
{
    None,
    Retreat0,
    Retreat1,
    Travel,         //游历
    DungeonMap,     //秘境
    RankWithPlayer, //排行榜与玩家切磋
    SectWithNpc,    //宗门中与npc切磋   
    Tower,
}

/// <summary>
/// 战斗信息
/// </summary>
public class BattleData
{
    public BattleType BattleType;  //战斗类型
    public Eint StageId = -1;      //关卡id
    public Eint MapId = 1;         //战斗地图id

    public int EnemyRoleUid;       //敌方主攻方玩家
    public int MyRoleUid;          //我方主攻方玩家

    public string FailName;

    public static BattleData GetTest()
    {
        BattleData battleData = new BattleData();
        battleData.MyRoleUid = 0;
        battleData.EnemyRoleUid = 1;
        return battleData;
    }


}

public enum BattleResultType:byte   //战斗结果类型
{
    [EnumDesc("战斗胜利")]
    Win = 0,    //胜利
    [EnumDesc("战斗失败")]
    Fail,
    [EnumDesc("平局")]
    Equal,
    [EnumDesc("逃跑")]
    Escape,
    Max
}