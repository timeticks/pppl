using System.Collections.Generic;
using System.IO;

public interface IOldHeroFetcher
{
    OldHero GetHeroByCopy(int idx);
}

public class OldHero : DescObject
{

    private static IOldHeroFetcher mHeroInter;


    public static IOldHeroFetcher HeroFetcher
    {
        get { return mHeroInter; }
        set
        {
            if (mHeroInter == null)
                mHeroInter = value;
        }
    }

    ///
    //宗门
    //
    protected int mSect;
    ///
    //种族
    //
    protected RaceType mRace;

    public enum RaceType
    {
        None,
        Human,
        Monster,
        Thing
    }

    ///
    //等级
    //
    protected Eint mLevel = 0;
    ///
    //普通技能
    //
    protected int mCommonSpell;
    //
    //攻击技能
    //
    protected int[] mAtkSpell;
    //
    //防御技能
    //
    protected int[] mDefSpell;

    protected int[] mPassiveSpell;
    //力量
    //
    protected Eint mStr = 0;
    ///
    //法力
    //
    protected Eint mMana = 0;
    ///
     //神识
     //
    protected Eint mMind = 0;
    ///
    //体魄
    //
    protected Eint mCon = 0;
    ///
    //魂力
    //
    protected Eint mVit = 0;
    ///
     //机缘
     //
    protected Eint mLuk = 0;
    ///
     //生命值
     //
    protected Eint mHp = 0;
    ///
    //法力值
    //
    protected Eint mMp = 0;
    ///
    //攻击力
    //
    protected Eint mAttack = 0;
    ///
    //物理防御
    //
    protected Eint mDefPhysical = 0;
    ///
    //法术防御
    //
    protected Eint mDefMagic = 0;
    ///
    //命中
    //
    protected Eint mHit = 0;
    ///
    //躲闪
    //
    protected Eint mDodge = 0;
    ///
    //暴击
    //
    protected Eint mCrit = 0;
    ///
    //抗暴
    //
    protected Eint mResilience = 0;
    ///
    //暴击倍数
    //
    protected Eint mCritRate = 0;
    ///
    //韧性
    //
    protected Eint mTough = 0;
    ///
    //招架
    //
    protected Eint mBlock = 0;
    ///
    //破招
    //
    protected Eint mBroken = 0;
    ///
    //伤害增加率
    //
    protected Eint mDmgInc = 0;
    ///
    //伤害减少率
    //
    protected Eint mDmgDec = 0;
    ///
    //阴属性增加率
    //
    protected Eint mYinDmgInc = 0;
    ///
    //阴属性减少率
    //
    protected Eint mYinDmgDec = 0;
    ///
    //阳属性增加率
    //
    protected Eint mYangDmgInc = 0;
    ///
    //阳属性减少率
    //
    protected Eint mYangDmgDec = 0;
    ///
    //冰属性增加率
    //
    protected Eint mIceDmgInc = 0;
    ///
    //冰属性减少率
    //
    protected Eint mIceDmgDec = 0;
    ///
    //火属性增加率
    //
    protected Eint mFireDmgInc = 0;
    ///
    //火属性减少率
    //
    protected Eint mFireDmgDec = 0;
    ///
    //雷属性增加率
    //
    protected Eint mThunderDmgInc = 0;
    ///
    //雷属性减少率
    //
    protected Eint mThunderDmgDec = 0;
    ///
    //毒属性增加率
    //
    protected Eint mPoisonDmgInc = 0;
    ///
    //毒属性减少率
    //
    protected Eint mPoisonDmgDec = 0;
    ///
    //称号
    //
    protected string mTitle;
    ///
    //描述
    //
    protected string mDesc;
    ///
    //头像
    //
    protected string mIcon;
    ///
    //性别
    //
    protected Sex mSex;

    public enum Sex
    {
        None,
        Male,
        Female,
    }


    public OldHero() : base()
    {
        
    }

    public OldHero(OldHero origin) : base(origin)
    {
        this.mSect          = origin.mSect;
        this.mRace          = origin.mRace;
        this.mLevel         = origin.mLevel;
        this.mCommonSpell    = origin.mCommonSpell;
        this.mAtkSpell      = origin.mAtkSpell;
        this.mDefSpell      = origin.mDefSpell;
        this.mPassiveSpell  = origin.mPassiveSpell;
        this.mStr           = origin.mStr;
        this.mMana          = origin.mMana;
        this.mMind          = origin.mMind;
        this.mCon           = origin.mCon;
        this.mVit           = origin.mVit;
        this.mLuk           = origin.mLuk;
        this.mHp            = origin.mHp;
        this.mMp            = origin.mMp;
        this.mAttack        = origin.mAttack;
        this.mDefPhysical   = origin.mDefPhysical;
        this.mDefMagic      = origin.mDefMagic;
        this.mHit           = origin.mHit;
        this.mDodge         = origin.mDodge;
        this.mCrit          = origin.mCrit;
        this.mResilience    = origin.mResilience;
        this.mCritRate      = origin.mCritRate;
        this.mTough         = origin.mTough;
        this.mBlock         = origin.mBlock;
        this.mBroken        = origin.mBroken;
        this.mDmgInc        = origin.mDmgInc;
        this.mDmgDec        = origin.mDmgDec;
        this.mYinDmgInc     = origin.mYinDmgInc;
        this.mYinDmgDec     = origin.mYinDmgDec;
        this.mYangDmgInc    = origin.mYangDmgInc;
        this.mYangDmgDec    = origin.mYangDmgDec;
        this.mIceDmgInc     = origin.mIceDmgInc;
        this.mIceDmgDec     = origin.mIceDmgDec;
        this.mFireDmgInc    = origin.mFireDmgInc;
        this.mFireDmgDec    = origin.mFireDmgDec;
        this.mThunderDmgInc = origin.mThunderDmgInc;
        this.mThunderDmgDec = origin.mThunderDmgDec;
        this.mPoisonDmgInc  = origin.mPoisonDmgInc;
        this.mPoisonDmgDec  = origin.mPoisonDmgDec;
        this.mTitle         = origin.mTitle;
        this.mDesc          = origin.mDesc;
        this.mIcon          = origin.mIcon;
        this.mSex           = origin.mSex;
    }



    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mSex           = (Sex)ios.ReadByte();
        this.mSect          = ios.ReadByte();
        this.mRace          = (RaceType)ios.ReadByte();
        this.mLevel         = ios.ReadInt16();

        this.mCommonSpell   = ios.ReadInt32();
        this.mStr           = ios.ReadInt32();    
        this.mMana          = ios.ReadInt32();
        this.mMind          = ios.ReadInt32();
        this.mCon           = ios.ReadInt32();
        this.mVit           = ios.ReadInt32();
        this.mLuk           = ios.ReadInt32();
        this.mHp            = ios.ReadInt32();
        this.mMp            = ios.ReadInt32();
        this.mAttack        = ios.ReadInt32();
        this.mDefPhysical   = ios.ReadInt32();
        this.mDefMagic      = ios.ReadInt32();
        this.mHit           = ios.ReadInt32();
        this.mDodge         = ios.ReadInt32();
        this.mCrit          = ios.ReadInt32();
        this.mResilience    = ios.ReadInt32();
                             
        this.mTough         = ios.ReadInt32();
        this.mBlock         = ios.ReadInt32();
        this.mBroken        = ios.ReadInt32();
        this.mDmgInc        = ios.ReadInt32();
        this.mDmgDec        = ios.ReadInt32();
        this.mYinDmgInc     = ios.ReadInt32();
        this.mYinDmgDec     = ios.ReadInt32();
        this.mYangDmgInc    = ios.ReadInt32();
        this.mYangDmgDec    = ios.ReadInt32();
        this.mIceDmgInc     = ios.ReadInt32();
        this.mIceDmgDec     = ios.ReadInt32();
        this.mFireDmgInc    = ios.ReadInt32();
        this.mFireDmgDec    = ios.ReadInt32();
        this.mThunderDmgInc = ios.ReadInt32();
        this.mThunderDmgDec = ios.ReadInt32();
        this.mPoisonDmgInc  = ios.ReadInt32();
        this.mPoisonDmgDec  = ios.ReadInt32();

        //TODO:改为万分比整数，ReadInt32
        this.mCritRate      = (int)ios.ReadSingle();
            
        this.mTitle     = NetUtils.ReadUTF(ios);
        this.mDesc      = NetUtils.ReadUTF(ios);
        this.mIcon      = NetUtils.ReadUTF(ios);
        byte lengthAtk = ios.ReadByte();
        mAtkSpell = new int[lengthAtk];
        for (byte i = 0; i < lengthAtk; i++)
        {
            mAtkSpell[i] = ios.ReadInt32();
        }
        byte lengthDef= ios.ReadByte();
        mDefSpell = new int[lengthDef];
        for (byte i = 0; i < lengthDef; i++)
        {
            mDefSpell[i] = ios.ReadInt32();
        }

        int length = ios.ReadByte();
        mPassiveSpell = new int[length];
        for (byte i = 0; i < length; i++)
        {
            mPassiveSpell[i] = ios.ReadInt32();
        }
    }

    public int CurHp;
    public int CurMp;

    public int[] AtkSpell
    {
        get { return mAtkSpell; }
    }
    public int[] DefSpell
    {
        get { return mDefSpell; }
    }
    public int Attack
    {
        get { return mAttack; }
    }

    public int Block
    {
        get { return mBlock; }
    }

    public int Broken
    {
        get { return mBroken; }
    }

    public int CommonSpell
    {
        get { return mCommonSpell; }
    }

    public int Con
    {
        get { return mCon; }
    }

    public int Crit
    {
        get { return mCrit; }
    }

    public int CritRate
    {
        get { return mCritRate; }
    }

    public int DefMagic
    {
        get { return mDefMagic; }
    }

    public int DefPhysical
    {
        get { return mDefPhysical; }
    }

    public string Desc
    {
        get { return mDesc; }
    }

    public int DmgDec
    {
        get { return mDmgDec; }
    }

    public int DmgInc
    {
        get { return mDmgInc; }
    }

    public int Dodge
    {
        get { return mDodge; }
    }

    public int FireDmgDec
    {
        get { return mFireDmgDec; }
    }

    public int FireDmgInc
    {
        get { return mFireDmgInc; }
    }

    public int Hit
    {
        get { return mHit; }
    }

    public int Hp
    {
        get { return mHp; }
    }

    public int IceDmgDec
    {
        get { return mIceDmgDec; }
    }

    public int IceDmgInc
    {
        get { return mIceDmgInc; }
    }

    public string Icon
    {
        get { return mIcon; }
    }

    public int Level
    {
        get { return mLevel; }
    }

    public int Luk
    {
        get { return mLuk; }
    }

    public int Mana
    {
        get { return mMana; }
    }

    public int Mind
    {
        get { return mMind; }
    }

    public int Mp
    {
        get { return mMp; }
    }

    public int PoisonDmgDec
    {
        get { return mPoisonDmgDec; }
    }

    public int PoisonDmgInc
    {
        get { return mPoisonDmgInc; }
    }

    public RaceType Race
    {
        get { return mRace; }
    }

    public int Resilience
    {
        get { return mResilience; }
    }

    public int Sect
    {
        get { return mSect; }
    }

    public Sex MySex
    {
        get { return mSex; }
    }

    public int Str
    {
        get { return mStr; }
    }

    public int ThunderDmgDec
    {
        get { return mThunderDmgDec; }
    }

    public int ThunderDmgInc
    {
        get { return mThunderDmgInc; }
    }

    public string Title
    {
        get { return mTitle; }
    }

    public int Tough
    {
        get { return mTough; }
    }

    public int Vit
    {
        get { return mVit; }
    }

    public int YangDmgDec
    {
        get { return mYangDmgDec; }
    }

    public int YangDmgInc
    {
        get { return mYangDmgInc; }
    }

    public int YinDmgDec
    {
        get { return mYinDmgDec; }
    }

    public int YinDmgInc
    {
        get { return mYinDmgInc; }
    }
    //public void Properties(OldEquip[] equips ,List<OldSpell> equipSpells,  List<OldSpell> allSpells ,Pet[] pets,Dictionary<PromType,int> addProm, int level)
    //{
    //    Dictionary<PromType,int> promAdd   = new Dictionary<PromType,int>();
    //    foreach (var temp in addProm)
    //    {
    //        promAdd.Add(temp.Key, temp.Value);
    //    }
    //    if (level > 0)
    //    {
    //        HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(level);
    //        if (levelUp != null)
    //        {
    //            mHp = levelUp.Hp;
    //            mMp = levelUp.Mp;
    //            mAttack = levelUp.Attack;
    //            mDefPhysical = levelUp.DefPhysical;
    //            mDefMagic = levelUp.DefMagic;
    //        }
    //    }
    //    //  装备加成
    //    if(equips!=null)
    //    {
    //        for (int i = 0; i < equips.Length; i++) 
    //        {
    //            if(equips[i]!=null)
    //            {
    //                //主属性   品质越高，主属性所对应数值越高
    //                int mainValue =  equips[i].PromValue[0];
    //                if (!promAdd.ContainsKey(equips[i].MainPromType))
    //                {
    //                    promAdd.Add(equips[i].MainPromType, 0);                    
    //                }
    //                promAdd[equips[i].MainPromType]+=mainValue;				
    //                //副属性，品质越高，副属性越多
    //                for (int j = 0;j<equips[i].Quality && j < equips[i].SubType.Length&&j < equips[i].SubValue.Length; j++)
    //                {
    //                    if(!promAdd.ContainsKey(equips[i].SubType[j]))
    //                    {
    //                        promAdd.Add(equips[i].SubType[j], 0);
    //                    }
    //                    promAdd[equips[i].SubType[j]] += equips[i].SubValue[j];
    //                }
					
    //            }
    //        }
    //    }

    //    //灵兽加成
    //    Pet pet;
    //    for (int i = 0; i < pets.Length; i++)
    //    {
    //        pet = pets[i];
    //        if (pet == null) continue;
    //        for (int j = 0; j < pet.BaseType.Length && j < pet.BaseVal.Length && j < pet.ChangeVal.Length; j++)
    //        {
    //            int attValue = pet.BaseVal[j];
    //            if (pet.Level - 1 >= j)
    //                attValue = pet.ChangeVal[j];
    //            if (promAdd.ContainsKey(pet.BaseType[j]))
    //            {
    //                attValue += promAdd[pet.BaseType[j]];
    //                promAdd.Remove(pet.BaseType[j]);
    //            }
    //            promAdd.Add(pet.BaseType[j], attValue);
    //        }
    //        if (pet.Skill > 0)
    //        {
    //            OldSpell spell = OldSpell.SpellFetcher.GetSpellByCopy(pet.Skill);
    //            if (spell != null) allSpells.Add(spell);
    //        }
    //    }

    //    // 技能加成
    //    Dictionary<int, int> bookSpellMap = new Dictionary<int, int>();
    //    for (int i = 0; i < allSpells.Count; i++)//天赋技能加成
    //    {
    //        if (allSpells[i] == null) continue;
    //        allSpells[i].GetBookSpellUnlock(bookSpellMap);
    //    }

    //    BookSpell bookSpell;
    //    foreach (var temp in bookSpellMap)
    //    {
    //        bookSpell = BookSpell.BookSpellFetcher.GetBookSpellByCopy(temp.Key);
    //        if (bookSpell == null) continue;
    //        for (int i = 0; i < bookSpell.BaseAttVal.Length; i++)
    //        {
    //            int attValue = bookSpell.BaseAttVal[i]*temp.Value;
    //            if (promAdd.ContainsKey(bookSpell.BaseAttType[i]))
    //            {
    //                promAdd[bookSpell.BaseAttType[i]] += attValue;
    //            }
    //            else
    //                promAdd.Add(bookSpell.BaseAttType[i], attValue);
    //        }
    //    }
        

    //    OldSpell curSpell; 
    //    for (int i = 0; i < equipSpells.Count; i++)
    //    {
    //        curSpell = equipSpells[i];
    //        if (curSpell == null) continue;
    //        for (int j = 0; j < curSpell.BaseAttType.Length && j < curSpell.BaseAttVal.Length; j++)
    //        {
    //            PromType type = (PromType)curSpell.BaseAttType[j];
    //            int attVal = curSpell.BaseAttVal[j];
    //            if (j < curSpell.AttPlusVal.Length)
    //            {
    //                attVal =attVal + curSpell.CurLevel * curSpell.AttPlusVal[j];
    //            }

    //            if (!promAdd.ContainsKey(type))
    //                promAdd.Add(type, attVal);
    //            else
    //                promAdd[type] += attVal;
    //        }
    //    }
        
        
    //    // 结算
    //    foreach (var item in promAdd)
    //    {
    //        PromType key = item.Key;
    //        int value = item.Value;
    //        switch (key)
    //        {
    //            case PromType.None: break;
    //            case PromType.Str: mStr += value; break;
    //            case PromType.Mana: mMana += value; break;
    //            case PromType.Mind: mMind += value; break;
    //            case PromType.Con: mCon += value; break;
    //            case PromType.Vit: mVit += value; break;
    //            case PromType.Luk: mLuk += value; break;
    //            case PromType.Hp: mHp += value; break;
    //            case PromType.Mp: mMp += value; break;
    //            case PromType.Attack: mAttack += value; break;
    //            case PromType.DefPhysical: mDefPhysical += value; break;
    //            case PromType.DefMagic: mDefMagic += value; break;
    //            case PromType.Hit: mHit += value; break;
    //            case PromType.Dodge: mDodge += value; break;
    //            case PromType.Crit: mCrit += value; break;
    //            case PromType.Resilience: mResilience += value; break;
    //            case PromType.CritRate: mCritRate += value; break;
    //            case PromType.Tough: mTough += value; break;
    //            case PromType.Block: mBlock += value; break;
    //            case PromType.Broken: mBroken += value; break;
    //            case PromType.DmgInc: mDmgInc += value; break;
    //            case PromType.DmgDec: mDmgDec += value; break;
    //            case PromType.YinDmgInc: mYinDmgInc += value; break;
    //            case PromType.YinDmgDec: mYinDmgDec += value; break;
    //            case PromType.YangDmgInc: mYangDmgInc += value; break;
    //            case PromType.YangDmgDec: mYangDmgDec += value; break;
    //            case PromType.IceDmgDec: mIceDmgDec += value; break;
    //            case PromType.IceDmgInc: mIceDmgInc += value; break;
    //            case PromType.FireDmgInc: mFireDmgInc += value; break;
    //            case PromType.FireDmgDec: mFireDmgDec += value; break;
    //            case PromType.ThunderDmgInc: mThunderDmgInc += value; break;
    //            case PromType.ThunderDmgDec: mThunderDmgDec += value; break;
    //            case PromType.PoisonDmgInc: mPoisonDmgInc += value; break;
    //            case PromType.PoisonDmgDec: mPoisonDmgDec += value; break;
    //            default:
    //                break;
    //        }
    //    }

    //}


    public int GetProm(AttrType type)
    {
        switch (type)
        {

            case AttrType.Luk: return Luk;
            case AttrType.Hp: return Hp;
            case AttrType.Mp: return Mp;
            case AttrType.Hit: return Hit;
            case AttrType.Dodge: return Dodge;
        }
        return 0;
    }
}
