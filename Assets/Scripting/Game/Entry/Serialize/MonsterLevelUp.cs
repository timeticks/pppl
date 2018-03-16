using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterLevelUpFetcher
{
    MonsterLevelUp GetMonsterLevelUpCopy(int level, bool isCopy = true);
}
public class MonsterLevelUp : BaseObject
{
    private static IMonsterLevelUpFetcher mFetcher;

    public static IMonsterLevelUpFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public int  level;
    public Eint hp = 0;
    public Eint mp = 0;
    public Eint phyAtk = 0;
    public Eint magAtk = 0;
    public Eint phyDef = 0;
    public Eint magDef = 0;
    public Eint speed = 0;
    public Eint critPct = 0;
    public Eint critDmg = 0;         //爆伤
    public Eint defCrit = 0;         //韧性，抗暴；暂时不要
    public Eint hit = 0;             //命中
    public Eint dodge = 0;           //闪避
    public Eint luk = 0;             //幸运

    public MonsterLevelUp()
    {
    }

    public MonsterLevelUp Clone()
    {
        return this.MemberwiseClone() as MonsterLevelUp;
    }

    public static void freshByMinus(MonsterLevelUp levelUp, MonsterLevelUp minusLevelUp)
    {
        if (minusLevelUp == null) return;
        levelUp.hp -= minusLevelUp.hp;
        levelUp.mp -= minusLevelUp.mp;
        levelUp.phyAtk -= minusLevelUp.phyAtk;
        levelUp.magAtk -= minusLevelUp.magAtk;
        levelUp.phyDef -= minusLevelUp.phyDef;
        levelUp.magDef -= minusLevelUp.magDef;
        levelUp.speed -= minusLevelUp.speed;
        levelUp.critPct -= minusLevelUp.critPct;
        levelUp.critDmg -= minusLevelUp.critDmg;
        levelUp.defCrit -= minusLevelUp.defCrit;
        levelUp.hit -= minusLevelUp.hit;
        levelUp.dodge -= minusLevelUp.dodge;
        levelUp.luk -= minusLevelUp.luk;
    }

    public static void freshByRate(MonsterLevelUp levelUp, MonsterRate rate)
    {
        if (rate == null) return;
        levelUp.hp = (int)(levelUp.hp * ( rate.hp));
        levelUp.mp = (int)(levelUp.mp * ( rate.mp));
        levelUp.phyAtk = (int)(levelUp.phyAtk * ( rate.phyAtk));
        levelUp.magAtk = (int)(levelUp.magAtk * ( rate.magAtk));
        levelUp.phyDef = (int)(levelUp.phyDef * ( rate.phyDef));
        levelUp.magDef = (int)(levelUp.magDef * ( rate.magDef));
        levelUp.speed = (int)(levelUp.speed * ( rate.speed));
        levelUp.critPct = (int)(levelUp.critPct * ( rate.critPct));
        levelUp.critDmg = (int)(levelUp.hp * ( rate.critDmg));
        levelUp.defCrit = (int)(levelUp.hp * ( rate.defCrit));
        levelUp.hit = (int)(levelUp.hp * ( rate.hit));
        levelUp.dodge = (int)(levelUp.hp * ( rate.dodge));
        levelUp.luk = (int)(levelUp.hp * ( rate.luk));
    }

    public static void freshHeroByLevelUp(MonsterLevelUp levelUp, Hero hero)
    {
        hero.hp += levelUp.hp;
        hero.mp += levelUp.mp;
        hero.phyAtk += levelUp.phyAtk;
        hero.magAtk += levelUp.magAtk;
        hero.phyDef += levelUp.phyDef;
        hero.magDef += levelUp.magDef;
        hero.speed += levelUp.speed;
        hero.critPct += levelUp.critPct;
        hero.critDmg += levelUp.critDmg;
        hero.defCrit += levelUp.defCrit;
        hero.hit += levelUp.hit;
        hero.dodge += levelUp.dodge;
        hero.luk += levelUp.luk;
    }
}
