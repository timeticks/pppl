using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface ITowerFetcher
{
    Tower GetTowerByCopy(int idx);
    List<Tower> GetSpeRewardTowersNoCopy();
}
public class Tower : DescObject
{
    private static ITowerFetcher mFetcher;
    public static ITowerFetcher TowerFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public int mOrder;
    public int mLevel;
    public int mMonster;          //怪物
    public int mComReward;        //普通奖励
    public int mSpeReward;       //特殊奖励
    
    public Tower():base()
    {
    	
    }
    public Tower(Tower origin): base(origin)
    {
        this.mOrder = origin.mOrder;
        this.mLevel = origin.mLevel;
        this.mMonster = origin.mMonster;
        this.mComReward = origin.mComReward;
        this.mSpeReward = origin.mSpeReward;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);

        mOrder = ios.ReadInt16();
        mLevel = ios.ReadInt16();
        mMonster = ios.ReadInt32();
        mComReward = ios.ReadInt32();
        mSpeReward = ios.ReadInt32();
    }

    public int Order
    {
        get { return mOrder; }
    }

    public int Level
    {
        get { return mLevel; }
    }

    public int Monster
    {
        get { return mMonster; }
    }

    public int ComReward
    {
        get { return mComReward; }
    }

    public int SpeReward
    {
        get { return mSpeReward; }
    }


}
