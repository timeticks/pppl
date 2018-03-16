using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEquipFetcher
{
    Equip GetEquipCopy(int idx);
    List<Equip> GetEquipByLevelCopy(int level);
}
public class EquipBase : BaseObject
{
    public string curGuid;
    public Eint curLevel;
    public Eint curEnhance;     //当前强化次数
    public bool curIsEquip;
    public Eint curQuality;     
    public AttrType[] curSubType=new AttrType[0];
    public Eint[] curSubVal=new Eint[0];
    public AttrType[] curMainAttrType = new AttrType[0]; //主属性
    public Eint[] curMainAttrVal = new Eint[0];
    public EquipBase()
    {
    }

    public EquipBase(Equip origin)
        : base(origin)
    {
        this.curGuid = origin.curGuid;
        this.curLevel = origin.curLevel;
        this.curEnhance = origin.curEnhance;
        this.curIsEquip = origin.curIsEquip;
        this.curQuality = origin.curQuality;
        this.curSubType = origin.curSubType;
        this.curSubVal = origin.curSubVal;
        this.curMainAttrType = origin.curMainAttrType;
        this.curMainAttrVal = origin.curMainAttrVal;
    }

    public Equip GetEquip()
    {
        Equip equip = Equip.Fetcher.GetEquipCopy(idx);
        equip.curGuid = this.curGuid;
        equip.curLevel = this.curLevel;
        equip.curEnhance = this.curEnhance;
        equip.curIsEquip = this.curIsEquip;
        equip.curQuality = this.curQuality;
        equip.curSubType = this.curSubType;
        equip.curSubVal = this.curSubVal;
        equip.curMainAttrType = this.curMainAttrType;
        equip.curMainAttrVal = this.curMainAttrVal;
        return equip;
    }
    public void CopyBy(EquipBase origin)
    {
        this.curGuid = origin.curGuid;
        this.curLevel = origin.curLevel;
        this.curEnhance = origin.curEnhance;
        this.curIsEquip = origin.curIsEquip;
        this.curQuality = origin.curQuality;
        this.curSubType = origin.curSubType;
        this.curSubVal = origin.curSubVal;
        this.curMainAttrType = origin.curMainAttrType;
        this.curMainAttrVal = origin.curMainAttrVal;
    }
}

//配置类
public class Equip : EquipBase
{
    private static IEquipFetcher mFetcher;
    public static IEquipFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public enum EquipType
    {
        [EnumDesc("无")]
        None    = -1,
        [EnumDesc("武器")]
        Atk     =0,
        [EnumDesc("衣服")]
        Cloth   =1,
        [EnumDesc("帽子")]
        Cap     =2,
        [EnumDesc("鞋子")]
        Shoe    =3,
        [EnumDesc("戒指")]
        Ring    =4,
        [EnumDesc("项链")]
        Neck    =5,
        [EnumDesc("披风")]
        Cloak   =6,
        [EnumDesc("宝物")]
        Treasure=7,
        Max
    }

    public string desc;
    public string icon;
    public EquipType type;
    public int[] qualityRange;  //[最小值，最大值]
    public int originLevel;
    public bool canSuit;
    public AttrType[] mainAttrType; //主属性
    public Eint[] mainAttrVal;
    public int subAttrRatio;        //附加属性增幅
    public int dropProb;            //爆率几率
    public Eint sell=0;

    public Equip()
    {
    }

    public Equip Clone()
    {
        return this.MemberwiseClone() as Equip;
    }

    public override void CheckLegal()
    {
        if (qualityRange.Length != 2 ||
             mainAttrType.Length != mainAttrVal.Length)
            TDebug.LogError("equip配置错误" + idx);

    }
}
