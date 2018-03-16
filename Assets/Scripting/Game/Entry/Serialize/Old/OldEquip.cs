using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//public interface IOldEquipFetcher
//{
//    Equip GetEquipByCopy(int idx);
//}
//public class Equip : DescObject {


//    private static IOldEquipFetcher mFetcher;

//    public static IOldEquipFetcher EquipFetcher
//    {
//        get { return mFetcher; }
//        set
//        {
//            if (mFetcher == null)
//                mFetcher = value;
//        }
//    }

//    public enum PosType : sbyte
//    {
//        [EnumDesc("无")]
//        None =-1,
//        [EnumDesc("武器")]
//        Atk,
//        [EnumDesc("衣服")]
//        Def1,
//        [EnumDesc("披风")]
//        Def2,
//        [EnumDesc("戒指")]
//        Passive1,
//        [EnumDesc("项链")]
//        Passive2,
//        [EnumDesc("法宝")]
//        Passive3,
//        Max
//    }
//    public Equip(): base()      
//    {
 
//    }
//    private string mIcon;
//    private string mDesc;
//    private short mLevel; //携带等级
//    private PosType mEquipPos;
//    private int mQuality;
//    private PromType mPromType;     //主属性类型
//    private int[] mPromValue;           
//    public PromType[] mSubType;  //副属性，根据品质，品质越高拥有越多属性
//    public int[] mSubValue;
//    private int mSell;

//    public bool IsEquip;
//    public Equip(Equip origin)
//        : base(origin)
//    {     
//        this.mIcon         = origin.mIcon;
//        this.mDesc          = origin.mDesc;
//        this.mLevel         = origin.mLevel;
//        this.mEquipPos = origin.mEquipPos;
//        this.mQuality      = origin.mQuality;
//        this.mPromType     = origin.mPromType;
//        this.mPromValue     = origin.mPromValue;
//        this.mSubType     = origin.mSubType;
//        this.mSubValue     = origin.mSubValue;
//        this.mSell          = origin.mSell;
//        this.IsEquip = origin.IsEquip;

//    }
//    public override void Serialize(BinaryReader ios)
//    {
//        base.Serialize(ios);

//        this.mEquipPos = (PosType)ios.ReadByte();
//        this.mQuality = ios.ReadByte();

//        this.mLevel = ios.ReadInt16();

//        this.mSell = ios.ReadInt32();              

//        this.mIcon = NetUtils.ReadUTF(ios);
//        this.mDesc = NetUtils.ReadUTF(ios);

//        this.mPromType = (PromType)ios.ReadByte();

//        int length = ios.ReadByte();
//        this.mPromValue = new int[length];
//        for (int i = 0; i < length; i++)
//        {
//            this.mPromValue[i] = ios.ReadInt32();
//        }

//        length = ios.ReadByte();
//        this.mSubType = new PromType[length];
//        for (int i = 0; i < length; i++)
//        {
//            this.mSubType[i] = (PromType)ios.ReadByte();
//        }
//        length = ios.ReadByte();
//        this.mSubValue = new int[length];
//        for (int i = 0; i < length; i++)
//        {
//            this.mSubValue[i] = ios.ReadInt32();
//        }
//    }



//    public string desc
//    {
//        get { return mDesc; }
//    }

//    public string icon
//    {
//        get { return mIcon; }
//    }

//    public short level
//    {
//        get { return mLevel; }
//    }

//    public PosType equipPos
//    {
//        get { return mEquipPos; }
//        set { mEquipPos = value; }
//    }

//    public int quality
//    {
//        get { return mQuality; }
//    }

//    public PromType mainAttrType
//    {
//        get { return mPromType; }
//    }

//    public int[] mainAttrVal
//    {
//        get { return mPromValue; }
//    }

//    public PromType[] subType
//    {
//        get { return mSubType; }
//    }

//    public int[] subVal
//    {
//        get { return mSubValue; }
//    }

//    public int sell
//    {
//        get { return mSell; }
//    }
//    //public EquipTypeEnum Type
//    //{
//    //    get { return mType; }
//    //}


//    public string GetNameWithQuality()
//    {
//        return TUtility.GetTextByQuality(name,mQuality);
//    }
//    public string GetLevelString()
//    {
//        HeroLevelUp level = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(mLevel);
//        return level.name;
//    }

//}
