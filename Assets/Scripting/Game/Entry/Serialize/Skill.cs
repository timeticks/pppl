using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ISkillFetcher
{
    Spell GetSpellCopy(int idx);
}
public class SkillBase : BaseObject 
{
    public Eint curLevel=0;
    public bool curIsEquip;  //这个之后删除掉

    public SkillBase()
    {
    }

    public SkillBase(BaseObject origin) : base(origin)
    {
    }

    public void CopyBy(SkillBase origin)
    {
        curLevel = origin.curLevel;
        //curIsEquip = origin.curIsEquip;
    }
}

public class Spell : SkillBase
{
    private static ISkillFetcher mFetcher;
    public static ISkillFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public enum SkillType
    {
        None,
        Damage,     //伤害技能
        Cure,       //治疗技能
        State,      //状态技能
        Passive,    //被动技能
        Max
    }
    public enum DmgType
    {
        None,
        PhyAtk,         //物理伤害
        MagAtk,         //魔法伤害
        PhyExtra,       //物攻数值
        MagExtra,       //魔攻数值
        RealDmg,        //真实伤害
        CureByMaxHp,    //根据最大生命值治疗技能
        CureByCurHp,    //根据当前生命值进行治疗
        CureByMagAtk,   //根据法伤进行治疗
    }
    public enum PosType
    {
        None = -1,
        Atk1,
        Atk2,
        Atk3,
        Def1,
        Def2,
        Passive,
        Max
    }

    public enum TargetType
    {
        None,
        Self,
        Enemy,
    }

    public enum TriggerType
    {
        CastThisSpell=0,    //施放此技能时，此状态的对象添加到skillTarget上
        RoundStart,         //回合开始时
        GetDmg,             //受到伤害时
        GetCure,            //受到治疗时
        CastAnySpell,       //施放任意技能时
        Dodge,              //闪避时
        Crit,               //暴击时
        HpLowerThan30,      //生命低于30%
        Dead,               //死亡时
    }

    public string desc;
    public string icon;
    public SkillType skillType;
    public DmgType[] dmgType;
    public Eint[] dmgVal;          //伤害值，根据dmgType判断是数值或万分比
    public Eint[] dmgPlus;         //每级增加
    public AttrType[] attrType ;//附加属性类型
    public Eint[] attrVal;       //附加属性值
    public Eint[] attrPlus;      //附加属性增量
    public Eint[] subSkill;      //子技能
    public Eint specBuff ;       //必定命中，必定暴击等，暂不用
    public Eint cool ;
    public Eint cost;
    public Eint maxLevel ;
    public int Level;
    public int NextState;
    public TargetType targetType;
    public Eint[] triggerProp;           //触发buff几率
    public TriggerType[] triggerType;   //触发类型
    public Eint[] triggerBuff ;

    public int curCool;         //当前的冷却时间

    public Spell()
    {
    }
    public Spell Clone()
    {
        if (triggerBuff.Length != triggerProp.Length)
            TDebug.LogError(string.Format("技能出错{0}", idx));
        if (attrVal.Length != attrPlus.Length || attrType.Length != attrPlus.Length)
            TDebug.LogError(string.Format("技能出错{0}", idx));
        if (dmgType.Length != dmgPlus.Length || dmgType.Length != dmgPlus.Length)
            TDebug.LogError(string.Format("技能出错{0}", idx));
        return this.MemberwiseClone()as Spell;
    }

    /// <summary>
    /// 根据等级，得到属性
    /// </summary>
    public int GetAttrValByLevel(int attrIndex)
    {
        if (attrIndex >= 0 && attrIndex < attrVal.Length)
        {
            return attrVal[attrIndex] + attrPlus[attrIndex]*curLevel;
        }
        return 0;
    }

    /// <summary>
    /// 根据等级，得到伤害值
    /// </summary>
    public int GetDmgValByLevel(int dmgIndex)
    {
        if (dmgIndex >= 0 && dmgIndex < dmgVal.Length)
        {
            return dmgVal[dmgIndex] + dmgPlus[dmgIndex] * curLevel;
        }
        return 0;
    }

    public bool canEquip(int pos)
    {
        return true;
    }
}
