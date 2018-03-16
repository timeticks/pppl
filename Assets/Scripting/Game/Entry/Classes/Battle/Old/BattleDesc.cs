using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public sealed class BattleDesc
{
    //public static string RoundDesc(int roundNum)
    //{
    //    string roundStr = LangMgr.GetText(DataName.textBattle, "roundStart");
    //    return string.Format(roundStr, roundNum);
    //}

    ////public static string SkillDes(List<RecordSkill> skillList,int atkerUid, string atkerName, string defName , HeroDataMgr atkerData , HeroDataMgr defData)
    ////{
    ////    StringBuilder str = new StringBuilder();
    ////    //for (int i = 0; i < skillList.Count; i++)
    ////    //{
    ////    //    Spell spell = Spell.SpellFetcher.GetSpellByCopy(skillList[i].SkillId);
    ////    //    if(spell==null)continue;
    ////    //    string tempStr = "";
    ////        
    ////    //    tempStr = SkillDesc(spell, 
    ////    //        atkerName, 
    ////    //        defName, 
    ////    //        new DmgInfo(skillList[i]), 
    ////    //        false,
    ////    //        skillList[i]
    ////    //        );
    ////        
    ////    //    str.Append(tempStr);
    ////    //}
    ////    return str.ToString();
    ////}

    //public static string SkillDesc(Spell spell, string myName, string aimName, DmgInfo dmg , bool aimDead ,RecordSkill record,int addNum=0)
    //{
    //    StringBuilder skillDesc = new StringBuilder();
    //    //施展了技能
    //    bool isSubSpell = record.IsSubspell;
    //    string playStr = "";
        
    //    if (spell.EffectType == Spell.EffectTypeEnum.MagDamage || spell.EffectType == Spell.EffectTypeEnum.PhyDamage)
    //    {
    //        playStr = isSubSpell ? GetRandomText("doAttackSubSpell") : GetRandomText("doAttackSpell");
    //    }
    //    else if (spell.EffectType == Spell.EffectTypeEnum.Heal)
    //    {
    //        playStr = isSubSpell ? GetRandomText("doHealSubSpell") : GetRandomText("doHealSpell");
    //    }
    //    else //if (spell.EffectType == Spell.EffectTypeEnum.StateDamage)
    //    {
    //        playStr = isSubSpell ? GetRandomText("doStateSubSpell") : GetRandomText("doStateSpell");
    //    }
    //    skillDesc.Append(playStr);
    //    WrapColor(skillDesc);

    //    skillDesc.Replace("[self]", myName);
    //    skillDesc.Replace("[aim]", aimName);
    //    skillDesc.Replace("[prom]", record.EffectVal.ToString());
    //    skillDesc.Replace("[spell]", spell.Name);
    //    skillDesc.Replace("[duration]", spell.Duration.ToString());
    //    skillDesc.Replace("[addNum]", addNum.ToString());

    //    //如果是法术攻击或物理攻击，进行伤害显示
    //    if (spell.EffectType == Spell.EffectTypeEnum.PhyDamage || spell.EffectType == Spell.EffectTypeEnum.MagDamage)
    //    {
    //        skillDesc.Append(DmgDesc(myName, aimName, dmg, aimDead));
    //    }
    //    else if (spell.EffectType == Spell.EffectTypeEnum.Heal)
    //    {
            
    //    }
    //    if (string.IsNullOrEmpty(skillDesc.ToString()) || skillDesc.ToString()=="")
    //    {
    //        TDebug.LogError(string.Format("{0}技能描述为null", spell.Idx));
    //    }
        
    //    return skillDesc.ToString();
    //}


    //private static string DmgDesc(string myName, string aimName, DmgInfo dmg, bool aimDead)
    //{
    //    if (dmg == null)
    //    {
    //        TDebug.LogError(string.Format("{0}的伤害信息为null", myName)); 
    //        return "";
    //    }
    //    //dmg.ResultType = (DmgResultType)Random.Range((int)DmgResultType.HpDmgMin + 1, (int)DmgResultType.HpDmgMax);

    //    List<string> dmgDescIdList =null;
    //    string dmgText="";
    //    int backDmg = 0;
    //    switch (dmg.ResultType) //根据攻击结果，得到不同的textKey
    //    {
    //        case DmgResultType.HpDmgNormal:
    //            dmgText = GetRandomText("dmgCommon");
    //            break;
    //        case DmgResultType.HpDmgMiss:
    //            dmgText = GetRandomText("dmgMiss");
    //            break;
    //        case DmgResultType.HpDmgCrit:
    //            dmgText = GetRandomText("dmgCrit");
    //            break;
    //        case DmgResultType.HpDmgBlock:
    //            dmgText = GetRandomText(aimDead ? "dmgBlockDead" : "dmgBlock");
    //            //获取反击信息
    //            //RecordSkill backRecord = Window_Battle.Instance.RecordBattle.GetNextBackAtk();
    //            //if (backRecord != null) backDmg = backRecord.EffectVal;
    //            break;
    //        case DmgResultType.HpCureNormal:
    //        case DmgResultType.MpCureNormal:
    //            return "";
    //    }
    //    if (dmgText == "")
    //    {
    //        TDebug.LogError("没有此文本键值");
    //        return "";
    //    }
    //    StringBuilder dmgStr = new StringBuilder(dmgText);
    //    if (dmg.ResultType == DmgResultType.HpDmgCrit)
    //    {
    //        dmgStr.Replace("[dmg]", "[critDmg]");
    //    }
    //    dmgStr = WrapColor(dmgStr);

    //    dmgStr = dmgStr.Replace("[critDmg]", "[dmg]");
    //    dmgStr = dmgStr.Replace("[self]", myName);
    //    dmgStr = dmgStr.Replace("[aim]", aimName);
    //    dmgStr = dmgStr.Replace("[dmg]", dmg.Num.ToString());
    //    dmgStr = dmgStr.Replace("[backDmg]", backDmg.ToString());
    //    return dmgStr.ToString();
    //}

    //public static string BuffDesc(string targetName, string atkerName, BuffData buffData)
    //{
    //    StringBuilder returnStr;
    //    Spell spell = Spell.SpellFetcher.GetSpellByCopy(buffData.SkillId);
    //    if (spell == null)
    //    {
    //        return "";
    //    }
    //    string tempStr = "";
    //    if (buffData.MiscType == Spell.EffectMiscType.Rebirth || buffData.MiscType == Spell.EffectMiscType.Recover)
    //    {
    //        tempStr = GetRandomText("stateHeal");
    //    }
    //    else if (buffData.MiscType == Spell.EffectMiscType.FireSignet || buffData.MiscType == Spell.EffectMiscType.ThunderSignet)
    //    {
    //        tempStr = GetRandomText("stateSignetDesc");
    //    }
    //    else
    //    {
    //        tempStr = GetRandomText("stateDmg");
    //    }
    //    returnStr = new StringBuilder(tempStr);
    //    returnStr = WrapColor(returnStr);

    //    returnStr.Replace("[self]", atkerName);
    //    returnStr.Replace("[aim]", targetName);
    //    returnStr.Replace("[num]", buffData.MaxDuration.ToString());
    //    returnStr.Replace("[addNum]", buffData.AddNum.ToString());

    //    return returnStr.ToString();
    //}

    ////public static string DotHotDesc(RecordDot dot , int duration, string targetName)
    ////{
    ////    StringBuilder str;
    ////    string tempStr = "";
    ////    if (dot.DotType == Spell.EffectMiscType.Rebirth || dot.DotType == Spell.EffectMiscType.Recover)
    ////    {
    ////        tempStr = GetRandomText("hotDoing");
    ////    }
    ////    else if (dot.DotType == Spell.EffectMiscType.FireSignet || dot.DotType == Spell.EffectMiscType.ThunderSignet)
    ////    {
    ////        return "";
    ////    }
    ////    else
    ////    {
    ////        tempStr = GetRandomText("dotDoing");
    ////    }
    ////    str = new StringBuilder(tempStr);
    ////    WrapColor(str);

    ////    str.Replace("[self]", targetName);
    ////    str.Replace("[aim]", targetName);
    ////    str.Replace("[dmg]", dot.DotVal.ToString());
    ////    str.Replace("[prom]", dot.DotVal.ToString());
    ////    str.Replace("[spell]", dot.DotType.ToString());
    ////    str.Replace("[duration]", duration.ToString());

    ////    return str.ToString();
    ////}


    //public static string DeadDesc(string loserName, bool isSeriousBattle)
    //{
    //    string mainKey = isSeriousBattle ? "endDead" : "endFail";
    //    List<string> descIdxList = GameData.Instance.GetSameKey(DataName.textBattle, mainKey);

    //    string descIdx = descIdxList[Random.Range(0, descIdxList.Count)];
    //    StringBuilder text = new StringBuilder(LangMgr.GetText(DataName.textBattle, descIdx));
    //    text = WrapColor(text);

    //    text = text.Replace("[loser]", loserName);

    //    return text.ToString();
    //}



    ////将文本里某些名称加颜色
    //public static StringBuilder WrapColor(StringBuilder str)
    //{
    //    str = str.Replace("[self]", "<color=#228B22>[self]</color>");
    //    str = str.Replace("[aim]", "<color=#228B22>[aim]</color>");
    //    str = str.Replace("[dmg]", "<color=#FFFF00>[dmg]</color>");
    //    str = str.Replace("[backDmg]", "<color=#FFFF00>[backDmg]</color>");
    //    str = str.Replace("[critDmg]", "<color=#FF4500>[critDmg]</color>"); 
    //    str = str.Replace("[spell]", "<color=#00FFFF>[spell]</color>");
    //    str = str.Replace("[prom]", "<color=#7CFC00>[prom]</color>");

    //    str = str.Replace("[loser]", "<color=#228B22>[loser]</color>");
    //    str = str.Replace("[winner]", "<color=#228B22>[winner]</color>");
    //    return str;
    //}

    //public static string GetRandomText(string strIdx)
    //{
    //    List<string> descIdxList = GameData.Instance.GetSameKey(DataName.textBattle, strIdx);
    //    string descIdx = descIdxList[Random.Range(0, descIdxList.Count)];
    //    return LangMgr.GetText(DataName.textBattle, descIdx);
    //}



    //public static string SwitchText(string str)
    //{
    //    StringBuilder returnStr = new StringBuilder();
    //    Dictionary<string, string> dict = new Dictionary<string, string>();
        

    //    return returnStr.ToString();
    //}
}


public enum BattleDescType
{
    self,
    aim,
    loser,
    winner,
    dmg,
    backDmg, 
    spell,   //技能
    duration,//持续时间
    num,     //回合数
    prom,    //效果值
    max
}