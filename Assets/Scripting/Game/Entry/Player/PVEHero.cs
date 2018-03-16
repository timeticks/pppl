using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PVEHero
{
    public OldHero MyHero;
    public Hero hero;
    public Eint monsterPrefix=0;//怪物前缀

    public List<Spell> atkList = new List<Spell>();     //普通技能也添加在里面去，普通技能没有冷却
    public List<Spell> passiveList = new List<Spell>();
    public List<Buff> buffList = new List<Buff>();      //正在生效的buff  
    private Eint curLevel=1;   //可为null 

    public bool isSelf;
    public Eint curHp=0;
    public Eint curMp=0;

    public enum StatusType
    {
        None,
        StopAction,
    }


    public PVEHero(Hero hero, int level , List<Spell> skillList , int prefixId=0)
    {
        this.hero = hero;
        if (skillList == null) return;
        SetLevel(level);
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].skillType == Spell.SkillType.Damage || skillList[i].skillType == Spell.SkillType.Cure)
                atkList.Add(skillList[i]);
            else if (skillList[i].skillType == Spell.SkillType.Passive)
                passiveList.Add(skillList[i]);
        }

        curHp = hero.hp;
        curMp = hero.mp;
        monsterPrefix = prefixId;
        if (prefixId > 0)
        {
            MonsterPrefix prefix = MonsterPrefix.Fetcher.GetMonsterPrefixCopy(prefixId);
            hero.name = prefix.name + hero.name;
        }
    }

    public void ResetHpMp()
    {
        
    }

    #region 战斗即时加成

    public void RoundFresh()
    {
        //生命恢复，生命流失
        int hpRecover = GetAtt(AttrType.HpRecover);
        if (hpRecover > 0)
        {
            BattleLog.Log("{0}回合生命恢复{1}=====hp:{2} mp:{3}", getHeroName(), hpRecover, curHp, curMp);
            BattleLog.TextLog("{0}回合生命恢复{1}", getHeroName(), hpRecover); 
            addHp(hpRecover,0);
        }
        int mpRecover = GetAtt(AttrType.MpRecover);
        if (mpRecover > 0)
        {
            BattleLog.Log("{0}回合魔法恢复{1}=====hp:{2} mp:{3}", getHeroName(), mpRecover, curHp, curMp);
            BattleLog.TextLog("{0}回合魔法恢复{1}", getHeroName(), mpRecover); 
            addMp(mpRecover);
        }
        int hpReduce = GetAtt(AttrType.HpReduce);
        if (hpReduce > 0)
        {
            BattleLog.Log("{0}回合生命流失{1}=====hp:{2} mp:{3}", getHeroName(), hpReduce, curHp, curMp);
            BattleLog.TextLog("{0}回合生命流失{1}", getHeroName(), hpReduce); 
            consumeHp(hpReduce , DmgResultType.HpDmgNormal,0);
        }
        int mpReduce = GetAtt(AttrType.MpReduce);
        if (mpReduce > 0)
        {
            BattleLog.Log("{0}回合魔法流失{1}=====hp:{2} mp:{3}", getHeroName(), mpReduce, curHp, curMp);
            BattleLog.TextLog("{0}回合魔法流失{1}", getHeroName(), mpReduce); 
            consumeMp(mpReduce);
        }

        //技能冷却
        for (int i = 0; i < atkList.Count; i++)
        {
            atkList[i].curCool = Mathf.Max(0, atkList[i].curCool - 1);
        }
        for (int i = 0; i < passiveList.Count; i++)
        {
            passiveList[i].curCool = Mathf.Max(0, passiveList[i].curCool - 1);
        }

        //buff
        List<int> removeList = new List<int>();
        for (int i = buffList.Count-1; i >=0; i--)
        {
            if (buffList[i] != null)
            {
                buffList[i].curDuration--;
                if (buffList[i].curDuration <= 0)
                {
                    buffList.RemoveAt(i);
                }
            }
        }
    }

    public void TryAddBuff(int buffId)
    {
        Buff buff = Buff.Fetcher.GetBuffCopy(buffId);
        TryAddBuff(buff);
    }
    public void TryAddBuff(Buff buff)
    {
        if (buff == null || !isLive()) return;
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].idx == buff.idx)
            {
                if (buff.maxAdd > buff.curAdd)
                {
                    buff.curAdd++;
                    buff.curDuration = buff.duration;
                    BattleLog.Log("{0}触发了buff[{1}]，当前叠加数{2}，剩余回合数{3}", getHeroName(), buff.name, buff.curAdd, buff.curDuration);
                }
                return;
            }
        }
        buff = buff.Clone();
        buff.curDuration = buff.duration;
        buff.curAdd = 1;
        buffList.Add(buff);
        BattleLog.Log("{0}触发了buff[{1}]，当前叠加数{2}，剩余回合数{3}", getHeroName(), buff.name, buff.curAdd, buff.curDuration);
    }

    #endregion

    //得到加成后的属性数值
    public int GetAtt(AttrType promTy)
    {
        int originVal = hero.GetAttr(promTy);
        int addVal = 0;
        AttrType pctType = (AttrType) ((int) promTy + 100);
        for (int i = 0; i < buffList.Count; i++)
        {
            for (int j = 0; j < buffList[i].effectType.Length; j++)
            {
                if (buffList[i].effectType[j] == promTy)
                    addVal += buffList[i].effectNum[j];
                else if (buffList[i].effectType[j] == pctType)
                    addVal += originVal.CaculatePctValue(buffList[i].effectNum[j]);
            }
        }
        return originVal + addVal;
    }




    #region 属性


    public int GetLevel()
    {
        return curLevel;
        //return (curLevel == null) ? 1 : curLevel.Level;
    }

    public void SetLevel(int level)
    {
        level = Mathf.Max(level, 1);
        curLevel = level;
        //if (levelData == null || (levelData != null && levelData.Level != level))
        //{
        //    levelData = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(level);
        //}
    }


    public int GetMaxHp()
    {
        return GetAtt(AttrType.Hp);
    }
    public int GetMaxMp()
    {
        return GetAtt(AttrType.Mp);
    }
    public int GetPhyAtk()
    {
        return GetAtt(AttrType.PhyAtk);
    }
    public int GetMagAtk()
    {
        return GetAtt(AttrType.MagAtk);
    }
    public int GetPhyDef()
    {
        return GetAtt(AttrType.PhyDef);
    }
    public int GetMagDef()
    {
        return GetAtt(AttrType.MagDef);
    }
    public string getHeroName()
    {
        return hero.name;
    }

    public int consumeHp(int damage , DmgResultType resultTy,float waitTime)
    {
        curHp = Mathf.Clamp(curHp - damage, 0, GetMaxHp());
        DmgInfo dmg = new DmgInfo(resultTy, damage, isSelf);
        Panel_Battle.Instance.SetShowInfo(PVEShowType.GetDmg, dmg, waitTime);
        return curHp;
    }
    public int addHp(int cure, float waitTime)
    {
        curHp = Mathf.Clamp(curHp + cure, 0, GetMaxHp());
        DmgInfo dmg = new DmgInfo(DmgResultType.HpCureNormal, cure, isSelf);
        Panel_Battle.Instance.SetShowInfo(PVEShowType.GetDmg, dmg, waitTime);
        return curHp;
    }
    public int consumeMp(int damage)
    {
        curMp = Mathf.Clamp(curMp - damage, 0, GetMaxMp());
        return curMp;
    }
    public int addMp(int cure)
    {
        curMp = Mathf.Clamp(curMp + cure, 0, GetMaxMp());
        return curMp;
    }

    public bool isCanHurt()
    {
        return isLive();
    }

    public bool isLive()
    {
        if (curHp <= 0)
            return false;
        return true;
    }

    public bool CanAction()
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            for (int j = 0; j < buffList[i].effectMode.Length; j++)
            {
                if (buffList[i].effectMode[j] == Buff.BuffMode.Spec)
                    if ((Buff.SpecBuffType)(int)buffList[i].effectType[j] == Buff.SpecBuffType.Dizzy
                        || (Buff.SpecBuffType)(int)buffList[i].effectType[j] == Buff.SpecBuffType.Frozen)
                        return false;
            }
        }
        return true;
    }
    #endregion




    public bool attackBy(PVEHero spellCaster, Spell skill)
    {
        if (!isCanHurt())
        {
            BattleLog.TextLog("{0}处于特殊状态，未受到技能效果", getHeroName());
            BattleLog.Log("{0}死亡或处于特殊状态，无法受到技能效果", getHeroName());
            return false;
        }
        //只有伤害技能，才判断命中
        if (skill.skillType == Spell.SkillType.Damage)
        {
            if (!GameUtils.isTrue(GameUtils.hitProp(spellCaster.GetAtt(AttrType.Hit), GetAtt(AttrType.Dodge))))
            {
                BattleLog.Log("{0}的{1}未命中{2}", spellCaster.getHeroName(), skill.name, getHeroName());
                BattleLog.TextLog("{0}的{1}未命中{2}", spellCaster.getHeroName(), skill.name, getHeroName());
                consumeHp(0, DmgResultType.HpDmgMiss, 0.3f);
                CheckTriggerBuff(Spell.TriggerType.Dodge, 0);
                return false;
            }
        }
        //判定是否触发状态？

        //伤害技能
        DmgInfo dmgInfo;
        if (skill.skillType == Spell.SkillType.Damage)
        {
            float finalDamage = 0;
            for (int i = 0; i < skill.dmgType.Length; i++)
            {
                switch (skill.dmgType[i])
                {
                    case Spell.DmgType.PhyAtk:
                    {
                        //防御减免
                        float defCoeff = GameUtils.defReductionCoefficient(GetPhyDef());
                        //基础伤害
                        float basePhyDamage = GameUtils.baseBaseDamage(spellCaster.GetPhyAtk(), defCoeff, spellCaster.GetAtt(AttrType.PhyDmgInc), GetAtt(AttrType.PhyDmgDec)+GetAtt(AttrType.AllDmgDec));
                        //技能的百分比
                        float spellCoeff = GameUtils.spellDamageCoeff(skill.GetDmgValByLevel(i), skill.GetDmgValByLevel(i));//万分比
                        float baseDamage = basePhyDamage * spellCoeff;
                        finalDamage += baseDamage;
                        break;
                    }
                    case Spell.DmgType.MagAtk:
                    {
                        float defCoeff = GameUtils.defReductionCoefficient(GetMagDef());
                        float baseMagDamage = GameUtils.baseBaseDamage(spellCaster.GetMagAtk(), defCoeff, spellCaster.GetAtt(AttrType.MagDmgInc), GetAtt(AttrType.MagDmgDec) + GetAtt(AttrType.AllDmgDec));
                        float spellCoeff = GameUtils.spellDamageCoeff(skill.GetDmgValByLevel(i), skill.GetDmgValByLevel(i));//万分比
                        float baseDamage = baseMagDamage * spellCoeff;
                        finalDamage += baseDamage;
                        break;
                    }
                    case Spell.DmgType.PhyExtra:
                    {
                        float extraDmg = GameUtils.getExtraDamage(skill.GetDmgValByLevel(i), spellCaster.GetAtt(AttrType.PhyDmgInc), GetAtt(AttrType.PhyDmgDec) + GetAtt(AttrType.AllDmgDec));
                        finalDamage += extraDmg;
                        break;
                    }
                    case Spell.DmgType.MagExtra:
                    {
                        float extraDmg = GameUtils.getExtraDamage(skill.GetDmgValByLevel(i), spellCaster.GetAtt(AttrType.MagDmgInc), GetAtt(AttrType.MagDmgDec) + GetAtt(AttrType.AllDmgDec));
                        finalDamage += extraDmg;
                        break;
                    }
                    case Spell.DmgType.RealDmg:
                    {
                        finalDamage += skill.GetDmgValByLevel(i);
                        break;
                    }
                }
            }
            //计算暴击
            float critDmg = 1;
            int critProp = GameUtils.critProp(spellCaster.GetAtt(AttrType.CritPct), this.GetAtt(AttrType.DefCrit));
            if (GameUtils.isTrue(critProp))
            {
                critDmg = spellCaster.GetAtt(AttrType.CritDmg).ToFloat_10000();
                spellCaster.CheckTriggerBuff(Spell.TriggerType.Crit, (int)skill.idx);
            }
            finalDamage *= critDmg;
            //计算真实伤害
            finalDamage += spellCaster.GetAtt(AttrType.ExtraDmg);
            int finalNum = Mathf.Max((int) finalDamage , 1);
            
            consumeHp(finalNum, critDmg > 1 ? DmgResultType.HpDmgCrit : DmgResultType.HpDmgNormal, 0.5f);
            BattleLog.Log("{0}对{1}施展了{2}，造成{3}点伤害，使其剩余{4}点生命", spellCaster.getHeroName(), getHeroName(), skill.name,
                finalNum, curHp);
            BattleLog.TextLog("{0}对{1}施展了{2}，造成{3}点伤害，使其剩余{4}点生命", spellCaster.getHeroName(), getHeroName(), skill.name,
                finalNum, curHp);
            CheckTriggerBuff(Spell.TriggerType.GetDmg, (int)skill.idx);
        }
        else if (skill.skillType == Spell.SkillType.Cure)//治疗技能
        {
            float finalCure = 0;
            for (int i = 0; i < skill.dmgType.Length; i++)
            {
                switch (skill.dmgType[i])
                {
                    case Spell.DmgType.CureByCurHp:
                    {
                        float cure = curHp*skill.GetDmgValByLevel(i)/10000f;
                        finalCure += cure;
                        break;
                    }
                    case Spell.DmgType.CureByMagAtk:
                    {
                        float cure = GetAtt(AttrType.MagAtk) * skill.GetDmgValByLevel(i) / 10000f;
                        finalCure += cure;
                        break;
                    }
                    case Spell.DmgType.CureByMaxHp:
                    {
                        float cure = GetMaxHp() * skill.GetDmgValByLevel(i) / 10000f;
                        finalCure += cure;
                        break;
                    }
                }
            }
            finalCure += GetAtt(AttrType.ExtraCure);
            int finalNum = Mathf.Max((int)finalCure, 0); 
            
            addHp(finalNum, 0.4f);
            BattleLog.TextLog("{0}对{1}施展了{2}，造成{3}点治疗，使其剩余{4}点生命", spellCaster.getHeroName(), getHeroName(), skill.name,
                finalNum, curHp);
            BattleLog.Log("{0}对{1}施展了{2}，造成{3}点治疗，使其剩余{4}点生命", spellCaster.getHeroName(), getHeroName(), skill.name,
                finalNum, curHp);
            CheckTriggerBuff(Spell.TriggerType.GetCure,0);
        }
        else if (skill.skillType == Spell.SkillType.State)
        {
            
        }

        //此buff添加到技能对象上
        CheckTriggerBuff(Spell.TriggerType.CastThisSpell, (int)skill.idx);
        spellCaster.CheckTriggerBuff(Spell.TriggerType.CastAnySpell, (int)skill.idx);
        return true;
    }

    /// <summary>
    /// 传入TriggerType，查看是否有buff可以被激活
    /// </summary>
    public void CheckTriggerBuff(Spell.TriggerType sceneTy ,object arg)
    {
        //触发了一个buff后，直接return
        switch (sceneTy)
        {
            case Spell.TriggerType.CastThisSpell:
            {
                int castSpellId = 0;
                if (arg is Eint)
                    castSpellId = (Eint)arg;
                else
                    castSpellId = (int)arg;
                for (int i = 0; i < atkList.Count; i++)
                {
                    if (castSpellId != atkList[i].idx) continue;
                    for (int j = 0; j < atkList[i].triggerType.Length; j++)
                    {
                        if (atkList[i].triggerType[j] == Spell.TriggerType.CastThisSpell)
                        {
                            if (GameUtils.isTrue(atkList[i].triggerProp[j]))
                                TryAddBuff(atkList[i].triggerBuff[j]);
                        }
                    }
                }
                break;
            }
            case Spell.TriggerType.RoundStart:
            case Spell.TriggerType.GetDmg:
            case Spell.TriggerType.GetCure:
            case Spell.TriggerType.CastAnySpell:
            case Spell.TriggerType.Dodge:
            case Spell.TriggerType.Crit:
            case Spell.TriggerType.Dead:
            {
                for (int i = 0; i < passiveList.Count; i++)
                {
                    for (int j = 0; j < passiveList[i].triggerType.Length; j++)
                    {
                        if (passiveList[i].triggerType[j] == sceneTy)
                        {
                            if (GameUtils.isTrue(passiveList[i].triggerProp[j]))
                                TryAddBuff(passiveList[i].triggerBuff[j]);
                        }
                    }
                }
                break;
            }
            case Spell.TriggerType.HpLowerThan30:
            {
                break;
            }
        }
    }

    

}
