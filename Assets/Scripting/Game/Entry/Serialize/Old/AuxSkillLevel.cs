using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO ;
public interface IAuxSkillFetcher
{
    AuxSkillLevel GetAuxSkilleByCopy(int level, AuxSkillLevel.SkillType type);
}
public class AuxSkillLevel :DescObject
{
    private static IAuxSkillFetcher mFetcher;
    public static IAuxSkillFetcher AuxSkillFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    private int mLevel;
    
    private int mProficiency;
    private int mState;     //等级需求  
    private int mItem; //升级材料
    private int[] mFormula; //赠送配方

    public int CurProficiency;
    public SkillType mType = SkillType.Forge;
    public enum SkillType
    {
    	Forge=0,//炼器
    	MakeDrug,//炼丹
        Mine, // 挖矿
        GatherHerb,// 采药
    	Max   	   
    }
    public AuxSkillLevel(): base() { }
    public AuxSkillLevel(AuxSkillLevel origin):base(origin)
    {
        mLevel = origin.mLevel;
        mProficiency = origin.mProficiency;
        CurProficiency = origin.CurProficiency;
        mType = origin.mType;
        mState = origin.mState;
        mItem = origin.mItem;
        mFormula = origin.mFormula;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mLevel = ios.ReadByte();
        mType = (SkillType)ios.ReadByte();

        this.mState= ios.ReadInt16();

        this.mProficiency = ios.ReadInt32();
        this.mItem = ios.ReadInt32();
        int length = ios.ReadByte();
        this.mFormula = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mFormula[i] = ios.ReadInt32();            
        }
    }

    public static int GetCachedKey(int level,SkillType type)
    {
        return (int)type * 100 + level;
    }


    public int Level
    {
        get { return mLevel; }
    }
    public int Proficiency
    {
        get { return mProficiency; }
    }
    public int State
    {
        get { return mState; }
    }
    public int Item
    {
        get { return mItem; }
    }
}
