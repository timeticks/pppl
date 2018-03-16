using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBase : BaseObject
{
    public Eint curLevel=0;

    public HeroBase()
    {
    }

    public HeroBase(BaseObject origin) : base(origin)
    {
    }
}

public interface IHeroFetcher
{
    Hero GetHeroCopy(int idx);
}
public class Hero : HeroBase
{
    private static IHeroFetcher mFetcher;

    public static IHeroFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public enum HeroType
    {
        None,
        Player,
        Monster,
        Pet
    }

    public Eint level=0;
    public Eint quality = 0;
    public Eint attrRatio = 0;
    public Eint rateId = 0;
    public HeroType heroType;

    public Eint hp=0;
    public Eint mp=0;
    public Eint phyAtk=0;
    public Eint magAtk=0;
    public Eint phyDef=0;
    public Eint magDef=0;
    public Eint speed=0;
    public Eint critPct=200;
    public Eint critDmg=15000;   //爆伤
    public Eint defCrit=0;         //韧性，抗暴；暂时不要
    public Eint hit=0;             //命中
    public Eint dodge=0;           //闪避
    public Eint luk=0;             //幸运
    public Eint extraDmg=0;        //附加伤害
    public Eint extraCure=0;       //附加治疗
    public Eint phyDmgInc=0;       //物免
    public Eint magDmgInc=0;       //法免
    public Eint phyDmgDec=0;       //物免
    public Eint magDmgDec=0;       //法免
    public Eint dmgReduce=0;       //全免
    public Eint hpReduce=0;        //每回合流失生命
    public Eint mpReduce=0;        //每回合流失魔法
    public Eint hpRecover=0;       //每回合恢复生命
    public Eint mpRecover=0;
    public Eint doSkillPct=0;      //技能释放率
    public Eint dizzyPct=0;        //眩晕概率
    public Eint poisonPct=0;       //附毒概率
    public Eint frozenPct=0;       //冰冻概率
    public Eint firePct=0;         //灼烧概率
    public Eint revivePct=0;       //复活几率
    public Eint reviveVal=0;       //复活时生命值
    public Eint commonAtk=0;       //普通攻击技能，冷却为0
    public Eint[] skill=new Eint[0];
    public Eint[] drop = new Eint[0];
    public Eint dropExp = 0;

    public Hero()
    {
    }

    public Hero Clone()
    {
        Hero temp = this.MemberwiseClone() as Hero;
        temp.skill = (Eint[])this.skill.Clone();
        temp.drop = (Eint[])this.drop.Clone();
        return temp;
    }



    public void Properties(Equip[] equips, List<Spell> equipSpells, Dictionary<AttrType, int> addAttr,List<int> birthBuff , int heroLevel)
    {
        Dictionary<AttrType, int> promAdd = new Dictionary<AttrType, int>();
        if (addAttr != null)
        {
            foreach (var temp in addAttr)
            {
                promAdd.Add(temp.Key, temp.Value);
            }
        }
        if (heroLevel > 0)
        {
            if (heroType == HeroType.Monster && heroLevel >level)
            {
                MonsterLevelUp fromLevelUp = level > 0 ? MonsterLevelUp.Fetcher.GetMonsterLevelUpCopy(level) : null;
                MonsterLevelUp toLevelUp = MonsterLevelUp.Fetcher.GetMonsterLevelUpCopy(heroLevel);
                MonsterLevelUp.freshByMinus(toLevelUp, fromLevelUp);

                MonsterRate monsterRate = MonsterRate.Fetcher.GetMonsterRateCopy(rateId);
                MonsterLevelUp.freshByRate(toLevelUp, monsterRate);

                MonsterLevelUp.freshHeroByLevelUp(toLevelUp, this);
            }
            else if (heroType == HeroType.Player)
            {
                HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(heroLevel);
                if (levelUp != null)
                {
                    hp = levelUp.hp;
                    mp = levelUp.mp;
                    phyAtk = levelUp.attack;
                    phyDef = levelUp.defPhysical;
                    magDef = levelUp.defMagic;
                }
            }
        }
        //天赋或称号加成
        if (birthBuff != null)
        {
            Buff buff;
            for (int i = 0; i < birthBuff.Count; i++)
            {
                if (birthBuff[i] == 0) continue;
                buff = Buff.Fetcher.GetBuffCopy(birthBuff[i]);
                for (int j = 0; j < buff.effectType.Length; j++)//主属性 
                {
                    promAdd.AddOrPlus<AttrType, int>(buff.effectType[j], buff.effectNum[j]);
                }
            }
        }


        //  装备加成
        if(equips!=null)
        {
            for (int i = 0; i < equips.Length; i++) 
            {
                if(equips[i]!=null)
                {
                    for (int j = 0; j < equips[i].curMainAttrType.Length; j++)//主属性 
                    {
                        promAdd.AddOrPlus<AttrType, int>(equips[i].curMainAttrType[j], equips[i].curMainAttrVal[j]);
                    }
                    for (int j = 0; j < equips[i].curSubType.Length; j++) //副属性
                    {
                        promAdd.AddOrPlus<AttrType, int>(equips[i].curSubType[j], equips[i].curSubVal[j]);
                    }
                }
            }
        }

        Spell curSpell; 
        for (int i = 0; i < equipSpells.Count; i++)
        {
            curSpell = equipSpells[i];
            if (curSpell == null) continue;
            for (int j = 0; j < curSpell.attrType.Length; j++)//主属性 
            {
                int num = curSpell.GetAttrValByLevel(j);
                promAdd.AddOrPlus<AttrType, int>(curSpell.attrType[j], num);
            }
        }
        FreshByAttr(promAdd);

    }

    private void FreshByAttr(Dictionary<AttrType, int> attrDict)
    {
        foreach (var item in attrDict)
        {
            if ((int) item.Key < 100)   //算数值
            {
                AddByAttr(item.Key, item.Value);
            }
        }
        foreach (var item in attrDict)
        {
            if ((int)item.Key > 100)    //算百分比
            {
                AttrType key = (AttrType)((int)item.Key - 100);
                int val = GetAttr(key).CaculatePctValue(item.Value);
                AddByAttr(key, val);
            }
        }
    }

    private void AddByAttr(AttrType attrTy, int value)
    {
        switch (attrTy)
        {
            case AttrType.None: break;
            case AttrType.Hp: hp += value; break;
            case AttrType.Mp: mp += value; break;
            case AttrType.PhyAtk: phyAtk += value; break;
            case AttrType.MagAtk: magAtk += value; break;
            case AttrType.PhyDef: phyDef += value; break;
            case AttrType.MagDef: magDef += value; break;
            case AttrType.Speed: speed += value; break;
            case AttrType.CritPct: critPct += value; break;
            case AttrType.CritDmg: critDmg += value; break;
            case AttrType.DefCrit: defCrit += value; break;
            case AttrType.Hit: hit += value; break;
            case AttrType.Dodge: dodge += value; break;
            case AttrType.Luk: luk += value; break;
            case AttrType.ExtraDmg: extraDmg += value; break;
            case AttrType.ExtraCure: extraCure += value; break;
            case AttrType.PhyDmgInc: phyDmgInc += value; break;
            case AttrType.MagDmgInc: magDmgInc += value; break;
            case AttrType.PhyDmgDec: phyDmgDec += value; break;
            case AttrType.MagDmgDec: magDmgDec += value; break;
            case AttrType.AllDmgDec: dmgReduce += value; break;
            case AttrType.HpReduce: hpReduce += value; break;
            case AttrType.MpReduce: mpReduce += value; break;
            case AttrType.HpRecover: hpRecover += value; break;
            case AttrType.MpRecover: mpRecover += value; break;
            case AttrType.DoSkillPct: doSkillPct += value; break;
            case AttrType.DizzyPct: dizzyPct += value; break;
            case AttrType.PoisonPct: poisonPct += value; break;
            case AttrType.FrozenPct: frozenPct += value; break;
            case AttrType.FirePct: firePct += value; break;
            case AttrType.RevivePct: revivePct += value; break;
            case AttrType.ReviveVal: reviveVal += value; break;
            //case AttrType.UpExp:                      UpExp += value; break;
            //case AttrType.UpGold:                      UpGold += value; break;
            //case AttrType.UpDrop:                         UpDrop += value; break;
            //case AttrType.UpEnemyQuality:                 UpEnemyQuality += value; break;
            //case AttrType.StrengthPct:              StrengthPct += value; break;
            //case AttrType.StrengthLevel:            StrengthLevel += value; break;
            default:
                break;
        }
    }

    public int GetAttr(AttrType attrTy)
    {
        switch (attrTy)
        {
            case AttrType.None: break;
            case AttrType.Hp: return hp;
            case AttrType.Mp: return mp;
            case AttrType.PhyAtk: return phyAtk;
            case AttrType.MagAtk: return magAtk;
            case AttrType.PhyDef: return phyDef;
            case AttrType.MagDef: return magDef;
            case AttrType.Speed: return speed;
            case AttrType.CritPct: return critPct;
            case AttrType.CritDmg: return critDmg;
            case AttrType.DefCrit: return defCrit;
            case AttrType.Hit:  return hit ;
            case AttrType.Dodge:return dodge ;
            case AttrType.Luk:return luk ;
            case AttrType.ExtraDmg:return extraDmg ;
            case AttrType.ExtraCure:return extraCure ;
            case AttrType.PhyDmgInc: return phyDmgInc;
            case AttrType.MagDmgInc: return magDmgInc;
            case AttrType.PhyDmgDec:return phyDmgDec ;
            case AttrType.MagDmgDec:return magDmgDec ;
            case AttrType.AllDmgDec:return dmgReduce ;
            case AttrType.HpReduce:return hpReduce ;
            case AttrType.MpReduce:return mpReduce ;
            case AttrType.HpRecover:return hpRecover ;
            case AttrType.MpRecover:return mpRecover ;
            case AttrType.DoSkillPct:return doSkillPct ;
            case AttrType.DizzyPct:return dizzyPct ;
            case AttrType.PoisonPct:return poisonPct ;
            case AttrType.FrozenPct:return frozenPct ;
            case AttrType.FirePct:return firePct ;
            case AttrType.RevivePct:return revivePct ;
            case AttrType.ReviveVal:return reviveVal ;
            //case AttrType.UpExp:                      UpExp += value; break;
            //case AttrType.UpGold:                      UpGold += value; break;
            //case AttrType.UpDrop:                         UpDrop += value; break;
            //case AttrType.UpEnemyQuality:                 UpEnemyQuality += value; break;
            //case AttrType.StrengthPct:              StrengthPct += value; break;
            //case AttrType.StrengthLevel:            StrengthLevel += value; break;
            default:
                return 0;
        }
        return 0;
    }


    
}
