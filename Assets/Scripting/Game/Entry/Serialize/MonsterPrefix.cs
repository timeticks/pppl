using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterPrefixFetcher
{
    MonsterPrefix GetMonsterPrefixCopy(int idx, bool isCopy = true);
}
public class MonsterPrefix :BaseObject
{
    private static IMonsterPrefixFetcher mFetcher;
    public static IMonsterPrefixFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public Eint quality=0; 
    public Eint addSkill=0;
    public bool isUp;
    public Eint dropUp=0;
    public Eint buffId=0;
    public MonsterPrefix()
    {
    }
    public MonsterPrefix Clone()
    {
        return this.MemberwiseClone() as MonsterPrefix;
    }

    public void CheckLegal()
    {
        
    }

    public static int GetPrefixQuality(int prefixId)
    {
        if (prefixId == 0) return 0;
        MonsterPrefix p = MonsterPrefix.Fetcher.GetMonsterPrefixCopy(prefixId , false);
        return p == null ? 0 : (int)p.quality;
    }
}
