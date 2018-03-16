using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetBattleRound  //回合信息
{
    public int MyRoundOverHp;    //回合结束时，我方血量
    public int EnemyRoundOverHp;
    public List<NetRoleAction> ActionList;   //行动者信息。TODO:之后添加灵兽后，需记录灵兽行动

    public void Serialize(BinaryReader ios)
    {
        MyRoundOverHp = ios.ReadInt32();
        EnemyRoundOverHp = ios.ReadInt32();
        ActionList = new List<NetRoleAction>();
        byte length = ios.ReadByte();
        for (int i = 0; i < length; i++)
        {
            NetRoleAction actionData = new NetRoleAction();
            actionData.Serialize(ios);
            ActionList.Add(actionData);
        }
    }
}

public class NetRoleAction    //角色行动信息
{
    public byte ActionRoleId;        //行动者id
    public int SkillId;              //可以根据技能判断行动类型

    public void Serialize(BinaryReader ios)
    {
        ActionRoleId = ios.ReadByte();
        SkillId = ios.ReadInt32();
    }
}