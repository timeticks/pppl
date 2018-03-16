using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAttrTableFetcher
{
    AttrTable GetAttrTableCopy(AttrType attrTy);
}
public class AttrTable : BaseObject
{
    private static IAttrTableFetcher mFetcher;
    public static IAttrTableFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public string desc;
    public int weight;
    public string baseStr;  //基本字符串
    public string addStr;
    public int limitType;   //限制类型
    public int limitVal;    
    public int[] valProb;   //数值与品质的概率
    public int[] pctProb;   //百分比属性与品质的概率
    public Eint[] markLevel;//基准等级线
    public Eint[] markNum;
    public Eint[] markPct;

    public AttrTable()
    {
    }

    public override void CheckLegal()
    {
        base.CheckLegal();
        if (markLevel.Length ==0 || markLevel.Length != markNum.Length || markNum.Length != markPct.Length)
            TDebug.LogError(string.Format("AttrTable错误:{0}", idx));
        if (valProb.Length != pctProb.Length)
            TDebug.LogError(string.Format("AttrTable错误:{0}", idx));
    }

    public AttrTable Clone()
    {
        return this.MemberwiseClone() as AttrTable;
    }

    //得到属性类型
    public static AttrType GetAttrType(AttrType attrTy, bool isPct)
    {
        if (isPct) 
            return (AttrType) (attrTy.ToInt() + 100);
        return attrTy;
    }

    /// <summary>
    /// 根据等级，得到此类型在此等级的数值
    /// </summary>
    public static int GetAttrMarkValue(AttrType attrTy, int level)
    {
        AttrTable att = AttrTable.Fetcher.GetAttrTableCopy(attrTy);
        bool isPctAttr = attrTy.ToInt() > 100;

        int minIndex = 0;
        int maxIndex = 0;
        for (int i = 0; i < att.markLevel.Length-1; i++) //得到此等级在哪两个index之间
        {
            if (level > att.markLevel[i])
            {
                minIndex = i;
                maxIndex = i+1;
            }
        }
        //进行插值计算数值
        //TDebug.Log(string.Format("length:{0} | min:{1}| max:{2}", att.markLevel.Length, minIndex, maxIndex));
        float levelProgress = Mathf.InverseLerp(att.markLevel[minIndex], att.markLevel[maxIndex], level);
        float attrValue = 0;
        if (isPctAttr)
            attrValue =Mathf.LerpUnclamped(att.markPct[minIndex], att.markPct[maxIndex], levelProgress);
        else
            attrValue = Mathf.LerpUnclamped(att.markNum[minIndex], att.markNum[maxIndex], levelProgress);
        //进行一些随机
        attrValue = GameUtils.GetRandomByLimit(attrValue, 7000, 10500, -2, 3);
        return (int) attrValue;
    }
}

public enum AttrType
{
    [EnumDesc("无")]         None            =0,
    [EnumDesc("生命")]        Hp              =1,
    [EnumDesc("魔法")]        Mp              =2,
    [EnumDesc("物攻")]        PhyAtk          =3,
    [EnumDesc("法攻")]        MagAtk          =4,
    [EnumDesc("物防")]        PhyDef          =5,
    [EnumDesc("法防")]        MagDef          =6,
    [EnumDesc("速度")]        Speed           =7,
    [EnumDesc("暴击率")]    CritPct         =8,
    [EnumDesc("暴伤")]        CritDmg         =9,
    [EnumDesc("抗暴")]        DefCrit         =10,      //韧性，抗暴
    [EnumDesc("命中")]        Hit             =11,      //命中
    [EnumDesc("闪避")]        Dodge           =12,      //闪避
    [EnumDesc("幸运")]        Luk             =13,      //幸运
    [EnumDesc("额外伤害")]      ExtraDmg        =14,      //附加伤害，数值
    [EnumDesc("额外恢复")]      ExtraCure       =15,      //附加治疗
    [EnumDesc("物伤增加")]      PhyDmgInc       =16,      //物伤百分比增加
    [EnumDesc("法伤增加")]      MagDmgInc       =17,      //法伤百分比增加
    [EnumDesc("物理免疫")]      PhyDmgDec       =18,      //物免
    [EnumDesc("法术免疫")]      MagDmgDec       =19,      //法免
    [EnumDesc("全免疫")]       AllDmgDec       =20,      //全免
    [EnumDesc("回合流失生命")]    HpReduce        =21,        //每回合流失生命
    [EnumDesc("回合流失魔法")]    MpReduce        =22,        //每回合流失魔法
    [EnumDesc("回合恢复生命")]    HpRecover       =23,      //每回合恢复生命
    [EnumDesc("回合恢复魔法")]    MpRecover       =24,
    [EnumDesc("技能释放率")]     DoSkillPct      =25,      //技能释放率
    [EnumDesc("眩晕")]        DizzyPct        =26,      //眩晕概率
    [EnumDesc("附毒")]        PoisonPct       =27,      //附毒概率
    [EnumDesc("冰冻")]        FrozenPct       =28,      //冰冻概率
    [EnumDesc("灼烧")]        FirePct         =29,      //灼烧概率
    [EnumDesc("复活几率")]      RevivePct       =30,      //复活几率
    [EnumDesc("复活恢复")]      ReviveVal       =31,      //复活时生命值
    [EnumDesc("经验获取")]      UpExp           =32,        //提高经验获取
    [EnumDesc("金币获取")]      UpGold          =33,        //金币获取
    [EnumDesc("掉宝率")]       UpDrop          =34,        //掉宝率
    [EnumDesc("遇怪品质")]      UpEnemyQuality  =35,    //提高遇怪品质,对强大敌人吸引力
    [EnumDesc("强化成功概率")]    StrengthPct     =36,    //强化成功概率
    [EnumDesc("强化保护等级")]    StrengthLevel   =37,  //强化保护等级

    [EnumDesc("无")]         NPcNone=100,     //百分比分界线
    [EnumDesc("生命")]        PcHp = 101,
    [EnumDesc("魔法")]        PcMp,
    [EnumDesc("物攻")]        PcPhyAtk,
    [EnumDesc("法攻")]        PcMagAtk,
    [EnumDesc("物防")]        PcPhyDef,
    [EnumDesc("法防")]        PcMagDef,
    [EnumDesc("速度")]        PcSpeed,
    [EnumDesc("暴击率")]    CrPcCritPct,
    [EnumDesc("暴伤")]        PcCritDmg,
    [EnumDesc("抗暴")]        PcDefCrit,
    [EnumDesc("命中")]        PcHit,
    [EnumDesc("闪避")]        PcDodge,
    [EnumDesc("幸运")]        PcLuk,
    [EnumDesc("额外伤害")]    PcExtraDmg,
    [EnumDesc("额外恢复")]    PcExtraCure,
    [EnumDesc("物伤增加")]    PcPhyDmgInc,      //物伤增加
    [EnumDesc("法伤增加")]    PcMagDmgInc,      //法伤增加
    [EnumDesc("物理免疫")]    PcPhyDmgReduce,
    [EnumDesc("法术免疫")]    PcMagDmgReduce,
    [EnumDesc("全免疫")]      PcDmgReduce,
    [EnumDesc("回合流失生命")]PcHpReduce,
    [EnumDesc("回合流失魔法")]PcMpReduce,
    [EnumDesc("回合恢复生命")]PcHpRecover,
    [EnumDesc("回合恢复魔法")]PcMpRecover,
    [EnumDesc("技能释放率")]  PcDoSkillPct,
    [EnumDesc("眩晕")]        PcDizzyPct,
    [EnumDesc("附毒")]        PcPoisonPct,
    [EnumDesc("冰冻")]        PcFrozenPct,
    [EnumDesc("灼烧")]        PcFirePct,
    [EnumDesc("复活几率")]    PcRevivePct,
    [EnumDesc("复活恢复")]    PcReviveVal,
    [EnumDesc("经验获取")]    PcUpExp,
    [EnumDesc("金币获取")]    PcUpGold,
    [EnumDesc("掉宝率")]      PcUpDrop,
    [EnumDesc("遇怪品质")]    PcUpEnemyQuality,
    [EnumDesc("强化成功概率")]PcStrengthPct,
    [EnumDesc("强化保护等级")]PcStrengthLevel, 
    
}


public enum LimitType
{
    Level,
    HaveItem,

}
