using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface IPrestigeLevelFetcher
{
    PrestigeLevel GetPrestigeLevelByCopy(int level , PrestigeLevel.PrestigeType ty);
    //List<PrestigeLevel> GetPrestigeLevelList();
}
public class PrestigeLevel : DescObject {

    private static IPrestigeLevelFetcher mFetcher;
    public static IPrestigeLevelFetcher PrestigeLevelFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    private int mLevel;

    private PrestigeType mType = PrestigeType.Self;

    private int mDemand;
    private int mState;
    private int mTaskNumber;

    private int[] mTask = new int[0];
    private int[] mWeight = new int[0];
    
    public int CurPrestige;
    
    public enum PrestigeType:byte
    {
        [EnumDesc("本宗门")]
	    Self,     //自身宗门
        [EnumDesc("天工宗")]
	    TianGong, //天工
        [EnumDesc("百草宗")]
	    BaiCao,   //百草
        [EnumDesc("天机宗")]
	    TianJi,   //天机
        [EnumDesc("未知")]
	    Max
    }
    public PrestigeLevel ():base()
    {
    }
    public PrestigeLevel(PrestigeLevel origin):base(origin)
    {
        this.mType = origin.mType;
        this.mLevel = origin.mLevel;
        this.mDemand = origin.mDemand;
        this.mState = origin.mState;
        this.mTask = origin.mTask;
        this.mWeight = origin.mWeight;
        this.mTaskNumber = origin.mTaskNumber;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mType = (PrestigeType)ios.ReadByte();

        this.mLevel = ios.ReadByte();
        this.mTaskNumber = ios.ReadByte();
        this.mState = ios.ReadInt16();
        this.mDemand = ios.ReadInt32();
        int length = ios.ReadByte();
        this.mTask = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mTask[i] = ios.ReadInt32();
        }
        length = ios.ReadByte();
        this.mWeight = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mWeight[i] = ios.ReadInt32();
        }

    }
    
    public static int getKey(int level ,PrestigeType ty)
    {
	    return ((int)ty)*100 + level;
    }

    //得到当前等级下的熟练度增量
    public int GetCurLevelPrestigeOffset()
    {
        if (Level == 1)
            return CurPrestige;
        PrestigeLevel temp =  PrestigeLevel.mFetcher.GetPrestigeLevelByCopy(mLevel - 1, Type);
        if (temp != null)
        {
            return CurPrestige - temp.Demand;
        }
        return 0;
    }
    //得到当前等级升下一级所需要的熟练度
    public int GetCurLevelPrestigeMax()
    {
        if (Level == 1)
            return Demand;
        PrestigeLevel temp = PrestigeLevel.mFetcher.GetPrestigeLevelByCopy(mLevel - 1, Type);
        if (temp != null)
        {
            return Demand - temp.Demand;
        }
        return 0;
    }


    public int Level
    {
        get { return mLevel; }
    }

    public PrestigeType Type
    {
        get { return mType; }
    }

    public int Demand
    {
        get { return mDemand; }
    }

    public int State
    {
        get { return mState; }
    }

    public int TaskNumber
    {
        get { return mTaskNumber; }
    }

    public int[] Task
    {
        get { return mTask; }
    }

    public int[] Weight
    {
        get { return mWeight; }
    }

}
