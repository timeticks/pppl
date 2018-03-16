using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public interface IPropLevelUpFetcher 
{
    PropLevelUp GetPropLevelUpByCopy(int level,PropLevelUp.PropLevelType type);
}
public class PropLevelUp : DescObject {

    private static IPropLevelUpFetcher mFetcher;

    public static IPropLevelUpFetcher propLevelUpFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private int mLevel;
    private int mGold;
    private int mWeight;
    private int mState;       //升级所需人物等级

    public PropLevelType mType = PropLevelType.None;
    public enum PropLevelType
    {
        None =-1,
        [EnumDesc("器炉")]
        Forge,
        [EnumDesc("丹炉")]
        Drug,
        [EnumDesc("矿锄")]
        Mine,
        [EnumDesc("药锄")]
        Herb,
        Max   	 
    }


    public PropLevelUp() : base() { }

    public PropLevelUp(PropLevelUp origin): base(origin)
    {
        this.mLevel = origin.mLevel;     
        this.mLevel = origin.mLevel;
        this.mGold = origin.mGold;   
        this.mWeight = origin.mWeight;    
        this.mState = origin.mState;     
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mLevel = ios.ReadByte();
        this.mType = (PropLevelType)ios.ReadByte();

        this.mState = ios.ReadInt16();
   
        this.mGold = ios.ReadInt32();
        this.mWeight = ios.ReadInt32();     
    }

    public static int GetCachedKey(int level,PropLevelType type)
    {
        return (int)type * 100 + level;
    }
    public int Level
    {
        get { return mLevel; }
    }
    public int State
    {
        get { return mState; }
    }
    public int Gold
    {
        get { return mGold; }
    }
}
