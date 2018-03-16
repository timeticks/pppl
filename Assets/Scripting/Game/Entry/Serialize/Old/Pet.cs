using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public interface IPetFetcher
{
    Pet GetPetByCopy(int idx);
}
public class Pet : DescObject
{
    private static IPetFetcher mFetcher;

    public static IPetFetcher PetFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public enum PetTypeEnum : sbyte//宠物类型
    {
        None =-1,
        [EnumDesc("color_pet_animal")]
        Animal,
        [EnumDesc("color_pet_puppet")]
        Puppet,
        [EnumDesc("color_pet_ghost")]
        Ghost,
        Max,
    }
  
    private PetTypeEnum mType;
    private string mIcon;
    private string mDesc;
    private int mLevel;
    private int mLevelMax;
    private int mSkill;
    private AttrType[] mBaseType;
    private int[] mBaseVal;
    private int[] mChangeVal;
    private int mNextState;
    private int mCostGold;
    private int mScience;
    private int mScienceNum;
    private int mResult;
    private int mTakeLevel;

    public int CurLevel;//当前强化次数
    public bool IsEquiped;

    public Pet() : base() { }
    public Pet(Pet origin)
        : base(origin)
    {
        this.mType = origin.mType;
        this.mIcon = origin.mIcon;
        this.mDesc = origin.mDesc;
        this.mLevel = origin.mLevel;
        this.mLevelMax = origin.mLevelMax;
        this.mSkill = origin.mSkill;
        this.mBaseType = origin.mBaseType;
        this.mBaseVal = origin.mBaseVal;
        this.mChangeVal = origin.mChangeVal;
        this.mNextState = origin.mNextState;
        this.mCostGold = origin.mCostGold;
        this.mScience = origin.mScience;
        this.mScienceNum = origin.mScienceNum;
        this.mResult = origin.mResult;
        this.CurLevel = origin.CurLevel;
        this.mTakeLevel = origin.mTakeLevel;
        this.IsEquiped = origin.IsEquiped;

    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);

        this.mType = (PetTypeEnum)ios.ReadByte();

        this.mLevel = ios.ReadInt16();
        this.mLevelMax = ios.ReadInt16();
        this.mNextState = ios.ReadInt16();
        this.mTakeLevel = ios.ReadInt16();

        this.mCostGold = ios.ReadInt32();
        this.mSkill = ios.ReadInt32();
        this.mScience = ios.ReadInt32();
        this.mScienceNum = ios.ReadInt32();
        this.mResult = ios.ReadInt32();



        this.mIcon = NetUtils.ReadUTF(ios);
        this.mDesc = NetUtils.ReadUTF(ios);

        int length = ios.ReadByte();
        mBaseType = new AttrType[length];
        for (int i = 0; i < length; i++)
        {
            mBaseType[i] = (AttrType)ios.ReadByte();
        }

        length = ios.ReadByte();
        mBaseVal = new int[length];
        for (int i = 0; i < length; i++)
        {
            mBaseVal[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        mChangeVal = new int[length];
        for (int i = 0; i < length; i++)
        {
            mChangeVal[i] = ios.ReadInt32();
        }

    }

    public string ColorName
    {
        get
        {
            LobbyDialogue dia = LobbyDialogue.LobbyDialogueFetcher.GetLobbyDialogueByCopy(mType.GetDesc());
            return string.Format("<color=#{0}>{1}</color>", dia.Describe, this.name);
        }
    }

    public string Desc
    {
        get { return mDesc; }
    }

    public int Level
    {
        get { return mLevel; }
    }
    public int LevelMax
    {
        get { return mLevelMax; }
    }

    public PetTypeEnum Type
    {
        get { return mType; }
    }
    public string Icon
    {
        get { return mIcon; }
    }
    public int Result
    {
        get { return mResult; }
    }
    public int[] BaseVal
    {
        get { return mBaseVal; }
    }
    public AttrType[] BaseType
    {
        get { return mBaseType; }
    }
    public int[] PlusVal
    {
        get { return mChangeVal; }
    }
    public int Skill
    {
        get { return mSkill; }
    }
    public int Science
    {
        get { return mScience; }
    }
    public int ScienceNum
    {
        get { return mScienceNum; }
    }
    public int[] ChangeVal
    {
        get { return mChangeVal; }
    }
    public int NextState
    {
        get { return mNextState; }
    }
    public int CostGold
    {
        get { return mCostGold; }
    }
    public int TakeLevel
    {
        get { return mTakeLevel; }
    }
}
