using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IIntimacyLevelUpFetcher
{
    IntimacyLevelUp GetIntimacyLevelUpCopy(int level,bool isCopy);
    int GetIntimacyLevelUpMax();
}
public class IntimacyLevelUp : BaseObject 
{
    private static IIntimacyLevelUpFetcher mFetcher;

    public static IIntimacyLevelUpFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public int level;
    public int num; //当前升级所需
    public string desc;

    public IntimacyLevelUp Clone()
    {
        return this.MemberwiseClone() as IntimacyLevelUp;
    }
    public static int GetCurLevelExp(int level)//得到当前等级的经验
    {
        IntimacyLevelUp lv = IntimacyLevelUp.Fetcher.GetIntimacyLevelUpCopy(level, false);
        if (lv != null) return lv.num;
        return 0;
    }
}
