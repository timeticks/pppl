using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVEAction:MonoBehaviour
{
    private PVEHero faster;  //先手
    private PVEHero slower;
    private int curRound;

    public void doing(PVEHero faster, PVEHero slower, int round)
    {
        this.faster = faster;
        this.slower = slower;
        this.curRound = round;
        StopCoroutine("doingCor");
        StartCoroutine("doingCor");
    }


    IEnumerator doingCor()
    {
        if (curRound == 1)
        {
            yield return new WaitForSeconds(PVEShowTime.timePVEStart);
        }
        else
        {
            yield return new WaitForSeconds(PVEShowTime.timeNextRound);
        }
        computeDot();//外部
        if (checkIsEnd(true)) yield break;


        computeBuff();//外部
        if (checkIsEnd(true)) yield break;


        Spell skill = GetRandomSkill(faster);
        castSkill(faster, skill);
        yield return new WaitForSeconds(PVEShowTime.timeCastSkill);
        if (checkIsEnd(true)) yield break;
        
        if (slower.isLive())
        {
            skill = GetRandomSkill(slower);
            castSkill(slower, skill);
            yield return new WaitForSeconds(PVEShowTime.timeCastSkill);
        }
        if (checkIsEnd(true)) yield break;

        PVEJob.Instance.end();
    }
    
    public void computeDot()//计算持续伤害buff
    {
        for (int i = 0; i < faster.buffList.Count; i++)
        {
            if (faster.buffList[i].type == Buff.BuffType.DotBuff)
            {
            }
        }
    }
    public void computeBuff()
    {
        faster.RoundFresh();
        if (checkIsEnd()) return;
        slower.RoundFresh();

        faster.CheckTriggerBuff(Spell.TriggerType.RoundStart, curRound);
        slower.CheckTriggerBuff(Spell.TriggerType.RoundStart, curRound);
    }


    //先判断死亡buff是否触发，再判断是否战斗结束
    public bool checkIsEnd(bool doEnd=false)
    {
        if (!faster.isLive())
        {
            faster.CheckTriggerBuff(Spell.TriggerType.Dead, 0);
        }
        if (!slower.isLive())
        {
            slower.CheckTriggerBuff(Spell.TriggerType.Dead, 0);
        }
        bool isEnd = PVEJob.Instance.isEnd();
        if (isEnd)
        {
            PVEJob.Instance.end();
        }
        return isEnd;
    }


    //施放技能
    void castSkill(PVEHero spellCaster, Spell skill)
    {
        if (!spellCaster.isLive())
        {
            BattleLog.Log("{0}已经死亡，无法发动技能", spellCaster.getHeroName());
            BattleLog.TextLog("{0}已经死亡，正在结算", spellCaster.getHeroName());
            return;
        }
        if (!spellCaster.CanAction())
        {
            BattleLog.Log("{0}被眩晕或冻结，无法发动技能", spellCaster.getHeroName());
            return;
        }
        skill.curCool = skill.cool;
        PVEHero target = getTarget(spellCaster, skill.targetType);
        Panel_Battle.Instance.SetShowInfo(PVEShowType.CastSkill, (int)skill.idx, spellCaster.isSelf);
        //BattleLog.Log("{0}对{1}施放技能{2}", spellCaster.getHeroName(), target.getHeroName(), skill.name);
        target.attackBy(spellCaster, skill);
    }

    
    //随机得到可以释放的技能
    Spell GetRandomSkill(PVEHero spellCaster)
    {
        List<Spell> coolSkills = new List<Spell>();
        for (int i = 0; i < spellCaster.atkList.Count; i++)
        {
            if (spellCaster.atkList[i].curCool <= 0)
            {
                coolSkills.Add(spellCaster.atkList[i]);
            }
        }
        Spell skill = coolSkills[GameUtils.GetRandom(0, coolSkills.Count)];
        return skill;
    }

    public PVEHero getTarget(PVEHero spellCaster, Spell.TargetType targetTy)
    {
        if (targetTy == Spell.TargetType.Self)
        {
            if (faster == spellCaster)
                return faster;
            else return slower;
        }
        else
        {
            if (faster != spellCaster)
                return faster;
            else return slower;
        }
    }
}
