using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterRateFetcher
{
    MonsterRate GetMonsterRateCopy(int idx , bool isCopy=true);
}
public class MonsterRate : BaseObject
{
    private static IMonsterRateFetcher mFetcher;

    public static IMonsterRateFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public double attrRatio ;
    public double hp ;
    public double mp ;
    public double phyAtk=0;
    public double magAtk ;
    public double phyDef ;
    public double magDef ;
    public double speed ;
    public double critPct ;
    public double critDmg ;   //爆伤
    public double defCrit;         //韧性，抗暴；暂时不要
    public double hit ;             //命中
    public double dodge ;           //闪避
    public double luk ;             //幸运

    public MonsterRate()
    {
    }

    public MonsterRate Clone()
    {
        return this.MemberwiseClone() as MonsterRate;
    }

}
