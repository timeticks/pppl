using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public interface ICaveFetcher
{
    Cave GetCaveByCopy(int level);
    int GetMaxCaveLevel();
}
public class Cave : DescObject {

    private static ICaveFetcher mFetcher;
    public static ICaveFetcher CaveFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private int mLevel;
    private string mDesc;
    private int mCostGold;
    private int mCostDiamond;
    private int mBasePotential;
    private int mBaseExp;
    private int mPercentPotential;
    private int mPercentExp;
    private int mRetreatProp;
    private int mLimit;
    public Cave(): base(){ }

    public Cave(Cave origin): base(origin)
    {
        this.mLevel                  = origin.mLevel;
        this.mDesc                   = origin.mDesc;
        this.mCostGold               = origin.mCostGold;
        this.mCostDiamond             = origin.mCostDiamond;
        this.mBasePotential           = origin.mBasePotential;
        this.mBaseExp                 = origin.mBaseExp;
        this.mPercentPotential         = origin.mPercentPotential;
        this.mPercentExp               = origin.mPercentExp;
        this.mRetreatProp              = origin.mRetreatProp;
        this.mLimit                    = origin.mLimit;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mLevel = ios.ReadByte();

        this.mBaseExp = ios.ReadInt16();
        this.mBasePotential = ios.ReadInt16();
        this.mPercentExp = ios.ReadInt16();
        this.mPercentPotential = ios.ReadInt16();
        this.mRetreatProp = ios.ReadInt16();
        this.mLimit = ios.ReadInt16();

        this.mCostGold = ios.ReadInt32();
        this.mCostDiamond = ios.ReadInt32();

        this.mDesc = NetUtils.ReadUTF(ios);
    
    }
    public static int MaxCaveLevel
    {
        get 
        {
            return mFetcher.GetMaxCaveLevel();
        }
    }


    public int Level
    {
        get { return mLevel; }
    }
    public string Desc
    {
        get { return mDesc; }
    }
    public int CostGold
    {
        get { return mCostGold; }
    }
    public int CostDiamond
    {
        get { return mCostDiamond; }
    }
    public int BasePotential
    {
        get { return mBasePotential; }
    }
    public int BaseExp
    {
        get { return mBaseExp; }
    }
    public int PercentPotential
    {
        get { return mPercentPotential; }
    }
    public int PercentExp
    {
        get { return mPercentExp; }
    }
    public int RetreatProp
    {
        get { return mRetreatProp; }
    }
    public int Limit
    {
        get { return mLimit; }
    }
}
