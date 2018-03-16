using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//伤害发生的类型
public enum DmgOccurType : byte
{
    //Local
    LocalMin,
    LNormal,
    LBackAtk,

    //Server
    ServerMin,
    SNormal,
    SBackAtk,
}



public class DmgInfo
{
    public bool DefierIsSelf;
    public int SkillId;
    public DmgOccurType OccurType;   //是否是反击
    public DmgResultType ResultType;
    public int Num;
    public DmgInfo() { }
    public DmgInfo(int skillId, DmgOccurType occurTy)
    {
        SkillId = skillId;
        OccurType = occurTy;
    }
    public DmgInfo(DmgResultType resultTy, int num , bool defIsSelf)
    {
        DefierIsSelf = defIsSelf;
        //SkillId = skillId;
        //OccurType = occurTy;
        ResultType = resultTy;
        Num = num;
    }
    public DmgInfo(int skillId, DmgOccurType occurTy, DmgResultType resultTy, int num)
    {
        SkillId = skillId;
        OccurType = occurTy;
        ResultType = resultTy;
        Num = num;
    }

    public static DmgInfo GetByRecord(RecordActionStr record, bool getBackAtk)  //根据记录信息，得到伤害信息
    {
        if (record == null) return null;
        if (record.Type == PVELoggerType.DmgCritBlock || record.Type == PVELoggerType.DmgBlock
            || record.Type == PVELoggerType.DmgBlockDead || record.Type == PVELoggerType.DmgCommon
            || record.Type == PVELoggerType.DmgCrit || record.Type == PVELoggerType.DmgMiss
            || record.Type == PVELoggerType.DoLifeRemoval || record.Type == PVELoggerType.DoLifeRemovalSub)
        {
            DmgInfo dmg = new DmgInfo();
            dmg.OccurType = DmgOccurType.LNormal;
            if (getBackAtk)
            {
                dmg.Num = record.GetValueTryInt(BattleDescType.backDmg);
                if (dmg.Num == 0) return null;
            }
            else
            {
                dmg.Num = record.GetValueTryInt(BattleDescType.dmg);
            }
            switch (record.Type)
            {
                case PVELoggerType.DmgCommon:
                    dmg.ResultType = DmgResultType.HpDmgNormal; break;
                case PVELoggerType.DmgBlock:
                case PVELoggerType.DmgBlockDead:
                case PVELoggerType.DmgCritBlock:
                    dmg.ResultType = DmgResultType.HpDmgBlock; break;
                case PVELoggerType.DmgMiss:
                    dmg.ResultType = DmgResultType.HpDmgMiss; break;
                case PVELoggerType.DmgCrit:
                    dmg.ResultType = DmgResultType.HpDmgCrit; break;
            }
            if (getBackAtk) //如果是反击，则都是普通攻击
            {
                dmg.ResultType = DmgResultType.HpDmgNormal;
            }
            return dmg;
        }
        else if (record.Type == PVELoggerType.DoHealSpell || record.Type == PVELoggerType.DoHealSubSpell
            || record.Type == PVELoggerType.HotDoing) //治疗
        {
            if (getBackAtk) return null;
            DmgInfo dmg = new DmgInfo();
            dmg.OccurType = DmgOccurType.LNormal;
            dmg.ResultType = DmgResultType.HpCureNormal;
            dmg.Num = record.GetValueTryInt(BattleDescType.prom);
            if (dmg.Num == 0) dmg.Num = record.GetValueTryInt(BattleDescType.dmg);
            return dmg;
        }
        return null;
    }
}


public enum DmgResultType : byte   //伤害结果
{
    HpDmgMin,               //伤害耐久或生命值
    HpDmgNormal,
    HpDmgBlock,             //格挡或抵抗，减少伤害
    HpDmgMiss,              //闪避
    HpDmgCrit,
    HpDmgMax,
    //-------------------
    MpDmgNormal,            //减少法力
    //-------------------
    HpCureNormal,           //治疗
    //-------------------
    MpCureNormal,           //恢复法力

}