using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public interface IPetLevelUpFetcher
{
    PetLevelUp GetPetLevelUpByCopy(int idx);
}
public class PetLevelUp : DescObject {

    private static IPetLevelUpFetcher mFetcher;

    public static IPetLevelUpFetcher petLevelUpFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private int mLevel;
    private int mCostGold;
    private int mScience;
    private int mNumber; // 消耗材料数量
    private int mState;

    public PetLevelUp()
        : base()
    {
 
    }

    public PetLevelUp(PetLevelUp origin)
        : base(origin)
    {
        this.mLevel = origin.mLevel;
        this.mCostGold = origin.mCostGold;
        this.mScience = origin.mScience;
        this.mNumber = origin.mNumber;
        this.mState = origin.mState;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mState = ios.ReadByte();

        this.mNumber = ios.ReadInt16();
        this.mLevel = ios.ReadInt16();

        this.mScience = ios.ReadInt32();
        this.mCostGold = ios.ReadInt32();
    }

    public int State
    {
        get { return mState; }
    }
    public int Number
    {
        get { return mNumber; }
    }
    public int Level
    {
        get { return mLevel; }
    }
    public int Scinece
    {
        get { return mScience; }
    }
    public int CostGold
    {
        get { return mCostGold; }
    }
}
