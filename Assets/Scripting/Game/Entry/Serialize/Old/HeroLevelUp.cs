using System.IO;

public interface ILevelUpFetcher
{
    HeroLevelUp GetLevelUpByCopy(int level);
    HeroLevelUp GetLevelUpByNoCopy(int level);
}

public sealed class HeroLevelUp : DescObject
{



    private static ILevelUpFetcher mLevelUpInter;


    public static ILevelUpFetcher LevelUpFetcher
    {
        get { return mLevelUpInter; }
        set
        {
            if (mLevelUpInter == null)
                mLevelUpInter = value;
        }
    }

    public int level;

    //境界值
    public int state;

    public int smallState;

    ///
     //升级所需经验总值
     //
    public long exp;
    ///
     //生命值
     //
    public int hp;
    ///
     //法力值
     //
    public int mp;
    ///
     //攻击力
     //
    public int attack;
    ///
     //物理防御
     //
    public int defPhysical;
    ///
     //法术防御
     //
    public int defMagic;
    ///
     //是否要闭关
     //
    public bool retreat;
    ///
     //闭关成功率
     //
    public int retreatTime;
    ///
     //闭关所需时间
     //
    public int successProp;
    ///
     //失败后损失经验比例
     //
    public float expPunish;
    ///
     //小劫难dotid
     //
    public int minSuffering;
    ///
     //小劫难处罚id
     //
    public int minPunish;
    ///
     //大劫难怪物id
     //
    public int maxSffering;
    ///
     //大劫难处罚id
     //
    public int maxPunish;
    ///
     //每分钟自动exp
     //
    public int autoExp;
    ///
     //每分钟自动钱
     //
    public int autoPotential;

    public string openFunc;

    
    public HeroLevelUp()
    {
    }

    public HeroLevelUp Clone()
    {
        return this.MemberwiseClone() as HeroLevelUp;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.retreat     =ios.ReadBoolean();
        this.state       = ios.ReadByte();

        this.smallState    = ios.ReadInt16();
        this.level         =ios.ReadInt16();
        this.successProp    = ios.ReadInt16();
        this.expPunish      = ios.ReadInt16();

        this.hp            = ios.ReadInt32();
        this.mp            = ios.ReadInt32();
        this.attack        = ios.ReadInt32();
        this.defPhysical   = ios.ReadInt32();
        this.defMagic      = ios.ReadInt32();     
        this.retreatTime   = ios.ReadInt32();
        this.minSuffering  = ios.ReadInt32();
        this.minPunish     = ios.ReadInt32();
        this.maxSffering   = ios.ReadInt32();
        this.maxPunish     = ios.ReadInt32();
        this.autoExp       = ios.ReadInt32();
        this.autoPotential  = ios.ReadInt32();
        this.exp           = ios.ReadInt64();
        this.openFunc      = NetUtils.ReadUTF(ios);
    }



    //是否能闭关
    public static bool CanRetreat(int level ,long exp)
    {
        HeroLevelUp levelUp = HeroLevelUp.mLevelUpInter.GetLevelUpByNoCopy(level);
        if (levelUp != null)
        {
            return (levelUp.retreat && exp >= levelUp.exp);
        }
        return false;
    }
    
    public static string GetStateName(int level)
    {
        HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByNoCopy(level);
        if (levelUp != null)
        {
            return levelUp.name;
        }
        return "";
    }
    public static long GetMaxExp(int level)//此等级的最大经验
    {
        HeroLevelUp lv = LevelUpFetcher.GetLevelUpByCopy(level);
        if (lv != null)
        {
            return lv.exp;
        }
        return 0;
    }

    public static long GetCurLevelExp(int level)//得到当前等级的经验
    {
        if (level <= 1)
        {
            HeroLevelUp lv = HeroLevelUp.mLevelUpInter.GetLevelUpByNoCopy(level);
            if (lv != null) return lv.exp;
            return 0;
        }
        HeroLevelUp lv1 = HeroLevelUp.mLevelUpInter.GetLevelUpByNoCopy(level);
        HeroLevelUp lastLv = HeroLevelUp.mLevelUpInter.GetLevelUpByNoCopy(level-1);
        if (lv1 == null || lastLv == null) return 0;
        return lv1.exp - lastLv.exp;
    }

    public static long GetLevelAmountExp(int level)//得到升到此等级的总经验
    {
        if (level <= 1) return 0;
        HeroLevelUp lv1 = HeroLevelUp.mLevelUpInter.GetLevelUpByNoCopy(level);
        if (lv1 != null) return lv1.exp;
        return 0;
    }

    public int CurStateRetreatProp //闭关成功概率
    {
        get
        {
            if (this.retreat)
            {
                return this.successProp;
            }
            else
            {
                int level = this.level / 10 * 10 + 10;
                HeroLevelUp lvUp = LevelUpFetcher.GetLevelUpByCopy(level);
                return lvUp.successProp;
            }
        }

    }
}
