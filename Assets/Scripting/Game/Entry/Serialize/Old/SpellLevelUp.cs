using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface ISpellLevelUpFetcher
{
    SpellLevelUp GetSpellLevelUpByCopy(int level);
}
public class SpellLevelUp : DescObject
{
    private static ISpellLevelUpFetcher mFetcher;
    public static ISpellLevelUpFetcher SpellLevelUpFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }


    private int mLevel;
    private int mCostPotential;
    private int mCostGold;

    public SpellLevelUp()
        : base()
    {

    }
    public SpellLevelUp(SpellLevelUp origin)
        : base(origin)
    {
        mLevel = origin.mLevel;
        mCostPotential = origin.mCostPotential;
        mCostGold = origin.mCostGold;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
       
        mLevel = ios.ReadInt16();
        mCostPotential = ios.ReadInt32();
        mCostGold = ios.ReadInt32();
    }
    public int Level
    {
        get { return mLevel; }
    }

    public int CostPotential
    {
        get { return mCostPotential; }
    }

    public int CostGold
    {
        get { return mCostGold; }
    }

}
