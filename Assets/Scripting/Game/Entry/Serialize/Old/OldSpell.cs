using System.IO;
using System.Collections.Generic;

//public interface ISkillFetcher
//{
//    Skill GetSpellByCopy(int idx);
//}

//public class Skill : DescObject
//{
//    private static ISkillFetcher mFetcher;

//    public static ISkillFetcher SpellFetcher
//    {
//        get { return mFetcher; }
//        set
//        {
//            if (mFetcher == null)
//                mFetcher = value;
//        }
//    }
//    public enum SpellTypeEnum : byte//技能类型0=普通攻击
//    {
//        None,
//        Normal,
//        Atk,
//        Def,
//        Mental,
//        Prom,
//        Passive,
//        Attribute
//    }

//    public enum DmgTypeEnum : byte//伤害类型0=无属性；1=阴；2=阳；3=冰；4=火；5=雷；6=毒
//    {
//        [EnumDesc("color_spell_none")]
//        None,
//        [EnumDesc("color_spell_yin")]
//        Yin,
//        [EnumDesc("color_spell_yang")]
//        Yang,
//        [EnumDesc("color_spell_ice")]
//        Ice,
//        [EnumDesc("color_spell_fire")]
//        Fire,
//        [EnumDesc("color_spell_thunder")]
//        Thunder,
//        [EnumDesc("color_spell_poison")]
//        Poison
//    }

//    public enum EffectTypeEnum : byte//技能子类型  1=物理攻击；2=法术攻击；3=神识攻击；4=状态技能；5=治疗技能；5=互斥技能
//    {
//        None,
//        PhyDamage,
//        MagDamage,
//        MindDamage,
//        StateDamage,
//        Heal,
//        Mutex,        //互斥技能
//        Signet,       //印记技能
//        LifeRemoval,  //生命移除
//    }

//    public enum EffectMiscType : byte
//    {
//        [EnumDesc("<color=#444444FF>无</color>")]
//        None,
//        [EnumDesc("<color=#009900FF>毒</color>")]
//        Poison,
//        [EnumDesc("<color=#3333FFFF>寒</color>")]
//        HighlyPoison,
//        [EnumDesc("<color=#000000FF>噬</color>")]
//        IceFrost,
//        [EnumDesc("<color=#FF0000FF>伤</color>")]
//        Bleed, //流血
//        [EnumDesc("<color=#FFFF00FF>灼</color>")]
//        Firing, //灼烧
//        [EnumDesc("<color=#0099FFFF>愈</color>")]
//        Rebirth, //再生
//        [EnumDesc("<color=#33CC00FF>恢</color>")]
//        Recover, //恢复
//        [EnumDesc("<color=#9900CCFF>雷</color>")]
//        ThunderSignet, //雷印记
//        [EnumDesc("<color=#FF3300FF>火</color>")]
//        FireSignet
//    }

//    private SpellTypeEnum mType;             //技能类型0=普通攻击；1=攻击技能；2=防御技能；3=被动技能；
//    private DmgTypeEnum mDmgType;          //伤害类型0=无属性；1=阴；2=阳；3=冰；4=火；5=雷；6=毒
//    private EffectTypeEnum mEffectType;       //技能子类型  0=物理攻击；1=法术攻击；2=神识攻击；3=状态技能；4=治疗技能；5=互斥技能
//    private EffectMiscType mEffectMisc;       //子类型参数
//    private byte mEffectRound;      //生效回合   -1代表永久
//    private byte mTargetSelect;     //目标选择
//    private byte mCasterStateCheck; //释放者状态检测
//    private byte mCasterParam;      //释放者参数
//    private byte mTargetStateCheck; //目标状态检测
//    private byte mTargetParam;      //目标参数
//    private AttrType mPromType;         //状态变化类型
//    private Eint mPromMisc = 0;         //参数类型，0数值，1万分比
//    private byte mDuration;         //作用回合数   -1代表永久

//    private Eint mCost = 0;             //消耗 
//    private short mEffectProp;        //触发概率


//    private int mSpecialTime;       //特效持续毫秒
//    private int[] mSubSpell;          //触发子技能id
//    private int mDmgMin;            //伤害最小万分比
//    private int mDmgMax;            //伤害最大万分比
//    private Eint mBasePoint = 0;         //技能固定数值
//    private Eint mPromValue = 0;         //状态变化值

//    private string mIcon;           //图标
//    private string mDesc;           //描述

//    private string mSpecialEffect;  //技能特效
//    private string mSpellDesc1;     //技能效果描述1
//    private string mSpellDesc2;     //技能效果描述2

//    public bool isEquip;
//    public enum PosType : sbyte
//    {
//        None = -1,
//        [EnumDesc("攻击功法")]
//        Atk1,
//        [EnumDesc("攻击功法")]
//        Atk2,
//        [EnumDesc("攻击功法")]
//        Atk3,
//        [EnumDesc("防御功法")]
//        Def1,
//        [EnumDesc("防御功法")]
//        Def2,
//        [EnumDesc("基础心法")]
//        Mental,
//        Max
//    }
//    public PosType equipPos;  // 可穿戴的位置
//    public bool mCertainHit;
//    public bool mCertainCrit;
//    public bool mCertainResi;
//    public bool mCertainBroken;

//    private int mLevel;     //初始等级
//    private int mLevelMax;   //最大强化次数
//    private int mNextState; //下一重升级等级
//    private int mNextSpell;
//    private int mBookSpell;
//    private AttrType[] mBaseAttType;
//    private int[] mBaseAttVal;//
//    private int[] mAttPlusVal;


//    public Eint curLevel = 0;   //当前强化次数
//    public Eint curDuration = 0;

//    public Skill()
//        : base()
//    {

//    }

//    public Skill(Skill origin)
//        : base(origin)
//    {
//        this.mType = origin.mType;
//        this.mDmgType = origin.mDmgType;
//        this.mEffectType = origin.mEffectType;
//        this.mEffectMisc = origin.mEffectMisc;
//        this.mEffectRound = origin.mEffectRound;
//        this.mTargetSelect = origin.mTargetSelect;
//        this.mCasterStateCheck = origin.mCasterStateCheck;
//        this.mCasterParam = origin.mCasterParam;
//        this.mTargetStateCheck = origin.mTargetStateCheck;
//        this.mTargetParam = origin.mTargetParam;
//        this.mPromType = origin.mPromType;
//        this.mPromMisc = origin.mPromMisc;
//        this.mPromValue = origin.mPromValue;
//        this.mDuration = origin.mDuration;
//        this.mCost = origin.mCost;
//        this.mEffectProp = origin.mEffectProp;
//        this.mSpecialTime = origin.mSpecialTime;
//        this.mSubSpell = origin.mSubSpell;
//        this.mDmgMin = origin.mDmgMin;
//        this.mDmgMax = origin.mDmgMax;
//        this.mBasePoint = origin.mBasePoint;
//        this.mIcon = origin.mIcon;
//        this.mDesc = origin.mDesc;
//        this.mSpecialEffect = origin.mSpecialEffect;
//        this.mSpellDesc1 = origin.mSpellDesc1;
//        this.mSpellDesc2 = origin.mSpellDesc2;
//        this.mLevel = origin.mLevel;
//        this.mLevelMax = origin.mLevelMax;
//        this.mNextState = origin.mNextState;
//        this.mNextSpell = origin.mNextSpell;
//        this.mBookSpell = origin.mBookSpell;

//        this.mCertainHit = origin.mCertainHit;
//        this.mCertainCrit = origin.mCertainCrit;
//        this.mCertainResi = origin.mCertainResi;
//        this.mCertainBroken = origin.mCertainBroken;

//        this.mBaseAttVal = origin.mBaseAttVal;
//        this.mBaseAttType = origin.mBaseAttType;
//        this.mAttPlusVal = origin.mAttPlusVal;

//        this.equipPos = origin.equipPos;
//        this.curLevel = origin.curLevel;
//        this.isEquip = origin.isEquip;
//    }


//    public override void Serialize(BinaryReader ios)
//    {
//        base.Serialize(ios);

//        this.mCertainBroken = ios.ReadBoolean();
//        this.mCertainCrit = ios.ReadBoolean();
//        this.mCertainHit = ios.ReadBoolean();
//        this.mCertainResi = ios.ReadBoolean();

//        this.mType = (SpellTypeEnum)ios.ReadByte();
//        this.mDmgType = (DmgTypeEnum)ios.ReadByte();
//        this.mEffectType = (EffectTypeEnum)ios.ReadByte();
//        this.mEffectMisc = (EffectMiscType)ios.ReadByte();
//        this.mEffectRound = ios.ReadByte();
//        this.mTargetSelect = ios.ReadByte();
//        this.mCasterStateCheck = ios.ReadByte();
//        this.mCasterParam = ios.ReadByte();
//        this.mTargetStateCheck = ios.ReadByte();
//        this.mTargetParam = ios.ReadByte();
//        this.mPromType = (AttrType)ios.ReadByte();
//        this.mPromMisc = ios.ReadByte();
//        this.mDuration = ios.ReadByte();

//        this.mCost = ios.ReadInt16();
//        this.mEffectProp = ios.ReadInt16();
//        this.mLevel = ios.ReadInt16();
//        this.mLevelMax = ios.ReadInt16();
//        this.mNextState = ios.ReadInt16();

//        this.mBookSpell = ios.ReadInt32();
//        this.mSpecialTime = ios.ReadInt32();
//        this.mDmgMin = ios.ReadInt32();
//        this.mDmgMax = ios.ReadInt32();
//        this.mBasePoint = ios.ReadInt32();
//        this.mPromValue = ios.ReadInt32();

//        this.mIcon = NetUtils.ReadUTF(ios);
//        this.mDesc = NetUtils.ReadUTF(ios);
//        this.mSpecialEffect = NetUtils.ReadUTF(ios);
//        this.mSpellDesc1 = NetUtils.ReadUTF(ios);
//        this.mSpellDesc2 = NetUtils.ReadUTF(ios);

//        byte length = ios.ReadByte();
//        this.mBaseAttType = new AttrType[length];
//        for (int i = 0; i < length; i++)
//        {
//            this.mBaseAttType[i] = (AttrType)ios.ReadByte();
//        }

//        length = ios.ReadByte();
//        this.mBaseAttVal = new int[length];
//        for (int i = 0; i < length; i++)
//        {
//            this.mBaseAttVal[i] = ios.ReadInt32();
//        }

//        length = ios.ReadByte();
//        this.mAttPlusVal = new int[length];
//        for (int i = 0; i < length; i++)
//        {
//            this.mAttPlusVal[i] = ios.ReadInt32();
//        }

//        length = ios.ReadByte();
//        this.mSubSpell = new int[length];
//        for (byte i = 0; i < length; i++)
//        {
//            this.mSubSpell[i] = ios.ReadInt32();
//        }
//    }

//    public string ColorName
//    {
//        get
//        {
//            LobbyDialogue dia = LobbyDialogue.LobbyDialogueFetcher.GetLobbyDialogueByCopy(mDmgType.GetDesc());
//            return string.Format("<color=#{0}>{1}</color>", dia.Describe, this.name);
//        }
//    }
//    public int GetOffsetLevel() //相对等级，不计算初始等级。范围在1-100级之间
//    {
//        return idx % 10 + 1 + curLevel;
//    }

//    public string icon
//    {
//        get { return mIcon; }
//    }

//    public string desc
//    {
//        get { return mDesc; }
//    }

//    public int cost
//    {
//        get { return mCost; }
//    }

//    public string SpecialEffect
//    {
//        get { return mSpecialEffect; }
//    }

//    public int SpecialTime
//    {
//        get { return mSpecialTime; }
//    }

//    public SpellTypeEnum skillType
//    {
//        get { return mType; }
//    }

//    public DmgTypeEnum dmgType
//    {
//        get { return mDmgType; }
//    }

//    public EffectTypeEnum EffectType
//    {
//        get { return mEffectType; }
//    }

//    public EffectMiscType EffectMisc
//    {
//        get { return mEffectMisc; }
//    }

//    public short EffectProp
//    {
//        get { return mEffectProp; }
//    }

//    public byte EffectRound
//    {
//        get { return mEffectRound; }
//    }

//    public enum ObjectiveTeam
//    {
//        None,
//        Self,
//        Enemy,
//    }

//    public ObjectiveTeam TargetSelect
//    {
//        get { return (ObjectiveTeam)mTargetSelect; }
//    }

//    public byte CasterStateCheck
//    {
//        get { return mCasterStateCheck; }
//    }

//    public byte CasterParam
//    {
//        get { return mCasterParam; }
//    }

//    public byte TargetStateCheck
//    {
//        get { return mTargetStateCheck; }
//    }

//    public byte TargetParam
//    {
//        get { return mTargetParam; }
//    }

//    public int[] SubSpell
//    {
//        get { return mSubSpell; }
//    }

//    public int DmgMin
//    {
//        get { return mDmgMin; }
//    }

//    public int DmgMax
//    {
//        get { return mDmgMax; }
//    }

//    public int BasePoint
//    {
//        get { return mBasePoint; }
//    }

//    public AttrType attrType
//    {
//        get { return mPromType; }
//    }

//    public int attrTypeMisc
//    {
//        get { return mPromMisc; }
//    }

//    public int attrVal
//    {
//        get { return mPromValue; }
//    }

//    public byte Duration
//    {
//        get { return mDuration; }
//    }

//    public string SpellDesc1
//    {
//        get { return mSpellDesc1; }
//    }

//    public string SpellDesc2
//    {
//        get { return mSpellDesc2; }
//    }

//    public int NextSpell
//    {
//        get { return mNextSpell; }
//    }

//    public int MyBookSpell
//    {
//        get { return mBookSpell; }
//    }

//    public int NextState
//    {
//        get { return mNextState; }
//    }

//    public int maxLevel
//    {
//        get { return mLevelMax; }
//    }
//    /// <summary>
//    /// 初始等级
//    /// </summary>
//    public int level
//    {
//        get { return mLevel; }
//    }

//    public int[] attrPlus
//    {
//        get { return mAttPlusVal; }
//    }

//    public int[] attrVal
//    {
//        get { return mBaseAttVal; }
//    }

//    public AttrType[] attrType
//    {
//        get { return mBaseAttType; }
//    }

//    public bool CertainBroken
//    {
//        get { return mCertainBroken; }
//    }

//    public bool CertainResi
//    {
//        get { return mCertainResi; }
//    }

//    public bool CertainCrit
//    {
//        get { return mCertainCrit; }
//    }

//    public bool CertainHit
//    {
//        get { return mCertainHit; }
//    }

//    #region Logic

//    /// <summary>
//    /// 根据当前Spell获取DIC<第几重技能id ，解锁的天赋功法 >
//    /// </summary>
//    /// <param name="spell"></param>
//    /// <returns></returns>
//    public static Dictionary<int, BookSpell> GetBookSpell(Skill spell)
//    {
//        Dictionary<int, BookSpell> spellList = new Dictionary<int, BookSpell>();
//        int num = 10;
//        int CommonId = spell.idx - spell.idx % 10;
//        for (int i = 0; i < num; i++)
//        {
//            Skill tempSpell = Skill.SkillFetcher.GetSpellByCopy(CommonId + i);
//            if (tempSpell != null && tempSpell.MyBookSpell != 0)
//            {
//                BookSpell bookSpell = BookSpell.BookSpellFetcher.GetBookSpellByCopy(tempSpell.MyBookSpell);
//                spellList.Add(tempSpell.idx, bookSpell);
//            }
//        }
//        return spellList;
//    }
//    /// <summary>
//    /// 根据当前技能获取已获得的天赋技能
//    /// </summary>
//    /// <param name="spell"></param>
//    /// <returns></returns>
//    public void GetBookSpellUnlock(Dictionary<int, int> addProm)
//    {
//        int num = idx % 10;
//        for (int i = 0; i <= num; i++)
//        {
//            if (i == num)
//            {
//                if (curLevel < maxLevel) continue; //如果是n重，则需要n重10层才能解锁
//            }
//            Skill tempSpell = Skill.SkillFetcher.GetSpellByCopy(idx - num + i);
//            if (tempSpell != null && tempSpell.MyBookSpell > 0)
//            {
//                if (addProm.ContainsKey(tempSpell.MyBookSpell))
//                    addProm[tempSpell.MyBookSpell] += 1;
//                else
//                    addProm.Add(tempSpell.MyBookSpell, 1);
//            }
//        }
//    }
//    /// <summary>
//    /// 获取该技能的最大重数
//    /// </summary>
//    /// <param name="spellId"></param>
//    /// <returns></returns>
//    //TODO :判断该功法最大重数，遍历该功法各位数从0-9的功法数
//    public static int GetSpellMaxState(int spellId)
//    {
//        int MaxState = 0;
//        int CommonId = spellId - spellId % 10;
//        int num = 10;
//        for (int i = 0; i < num; i++)
//        {
//            Skill tempSpell = Skill.SkillFetcher.GetSpellByCopy(CommonId + i);
//            if (tempSpell != null)
//            {
//                MaxState++;
//            }
//        }
//        return MaxState;
//    }
//    public string GetCurStateStr()
//    {
//        string lvString = "";
//        int curState = this.idx % 10;//功法id%10得到该功法当前重数
//        int maxState = GetSpellMaxState(this.idx);
//        if (curState == maxState && this.curLevel == this.maxLevel)
//            lvString = "大圆满";
//        else
//            lvString = string.Format("{0}重{1}层", this.idx % 10 + 1, this.curLevel + 1);//几重几层

//        return lvString == "" ? "未知境界" : lvString;
//    }
//    public static int GetSpellLevel(int spellId, int curLevel)
//    {
//        Skill spell = Skill.SkillFetcher.GetSpellByCopy(spellId);
//        if (spell != null)
//        {
//            return spell.level + curLevel;
//        }
//        return 0;
//    }

//    public static int GetSpellLevelOffset(int spellId, int curLevel)
//    {
//        int curState = spellId % 10;
//        return curState * 10 + curLevel + 1;
//    }

//    //得到技能的基本id
//    public static int GetSpellBaseIdx(int spellId)
//    {
//        return spellId - spellId % 10;
//    }

//    #endregion
//}
