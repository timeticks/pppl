using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public interface IAchieveRewardFetcher
{
    AchieveReward GetAchieveRewardByCopy(int point);
    List<AchieveReward> GetAchieveRewardAllByCopy();
}
public class AchieveReward : DescObject {

    private static IAchieveRewardFetcher mFetcher;
    public static IAchieveRewardFetcher AchieveRewardFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public int mPoint;
    public int mReward;

    public bool IsGot;
    public AchieveReward(): base(){ }
    public AchieveReward(AchieveReward origin)
        : base(origin)
    {
        this.mPoint = origin.mPoint;
        this.mReward = origin.mReward;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mPoint = ios.ReadInt16();
        this.mReward = ios.ReadInt32();
    }
    public int Point
    {
        get { return mPoint; }
    }
    public int Reward
    {
        get { return mReward; }
    }
}
