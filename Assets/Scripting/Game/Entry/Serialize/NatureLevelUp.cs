using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface INatureLevelUpFetcher
{
    NatureLevelUp GetNatureLevelUpCopy(NatureType natureType, int level, bool isCopy);
    int GetNatureLevelUpMax(NatureType natureType);
}
public class NatureLevelUp : BaseObject 
{
    private static INatureLevelUpFetcher mFetcher;

    public static INatureLevelUpFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public string desc;
    public NatureType natureType;
    public int level;
    public int natureMisc;
    public int needNum;

    public NatureLevelUp Clone()
    {
        return this.MemberwiseClone() as NatureLevelUp;
    }



}

public enum NatureType
{
    None,
    ScoreLoot,
    MapEndLoot,
    UniversalBall,
    NextBall,
    Frozen,
    DelayMulti,
    Max,
}