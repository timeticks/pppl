using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;

public class GamePlayerBase
{
    public string name;
    public Eint birthTime = 0;          //修炼时间
    public Eint PlayerUid = 0;          //唯一id

    public Eint Level = 1;
    public Eint CaveLevel = 1;  // 洞府等级

    public Elong Exp = 0;       //当前经验
    public Eint Gold = 0;       //普通货币
    public Eint Diamond = 0;    //充值货币
    public Eint Potential = 0;
    public Eint memoryPiece;    //记忆碎片数量


    public Dictionary<Eint, Eint> ProduceDict = new Dictionary<Eint, Eint>();

    public Eint HeadIconIdx = 0;   //头像

    public int[] SpellList = new int[(int)Spell.PosType.Max];   //如果为0则此槽没有装备招式
    public int[] EquipList = new int[(int)Equip.EquipType.Max];   //存放法宝在包裹中的位置
    public int[] PetList = new int[(int)Pet.PetTypeEnum.Max];   //存放宠物在背包中的位置

    //old..............
    public Sect.SectType MySect = Sect.SectType.None;
    public bool IsFinishNewerMap;
    public long NextVipDailyDiamond;
    public long FinishGuideStep;

    public Dictionary<AttrType, int> AddProm = new Dictionary<AttrType, int>();
    public bool IsInRank;     //是否加入排行榜排名
    public bool IsSetName;    //是否已经取名
    public Elong VipTime = 0;      //vip的到期时间
    public Eint PlayerIdx = 0;


    public void CopyBy(GamePlayerBase origin)
    {
        this.name = origin.name;
        this.birthTime = origin.birthTime;
        this.PlayerUid = origin.PlayerUid;
        this.Level = origin.Level;
        this.CaveLevel = origin.CaveLevel;
        this.Exp = origin.Exp;
        this.Gold = origin.Gold;
        this.Diamond = origin.Diamond;
        this.Potential = origin.Potential;
        this.HeadIconIdx = origin.HeadIconIdx;
        this.SpellList = origin.SpellList;
        this.EquipList = origin.EquipList;
        this.PetList = origin.PetList;
        this.MySect = origin.MySect;
        this.IsFinishNewerMap = origin.IsFinishNewerMap;
        this.NextVipDailyDiamond = origin.NextVipDailyDiamond;
        this.AddProm = origin.AddProm;
        this.IsInRank = origin.IsInRank;
        this.IsSetName = origin.IsSetName;
        this.VipTime = origin.VipTime;
        this.PlayerIdx = origin.PlayerIdx;

    }
}


[HideInInspector]
public sealed class GamePlayer : GamePlayerBase
{
    public Hero Hero = new Hero();     //角色

    public AuxSkillLevel[] AuxSkillList = new AuxSkillLevel[(int)AuxSkillLevel.SkillType.Max];//生活技能
    public PropLevelUp[] PropLevelList = new PropLevelUp[(int)PropLevelUp.PropLevelType.Max];
    public PrestigeLevel[] PrestigeList = new PrestigeLevel[(int)PrestigeLevel.PrestigeType.Max];

    public GamePlayer() 
    {
        for (int i = 0; i < EquipList.Length; i++)
        {
            EquipList[i] = (int)Equip.EquipType.None;
        }
        for (int i = 0; i < SpellList.Length; i++)
        {
            SpellList[i] = (int)Spell.PosType.None;
        }
        for (int i = 0; i < PetList.Length; i++)
        {
            PetList[i] = (int)Pet.PetTypeEnum.None;
        }
    }

    /// <summary>
    /// 当前等级经验
    /// </summary>
    /// <returns></returns>
    public long CurLevelExp
    {
        get 
        {
            if(Level>1)
            {
                HeroLevelUp lvUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(Level-1);
                return Exp - lvUp.exp;
            }
            else
                return Exp;
        }
    }

    public int GetPromTypeNum(AttrType type)
    {
        Hero hero = PlayerPrefsBridge.Instance.GetHeroWithProperties();
        return hero.GetAttr(type);
    }

    public PrestigeLevel GetPrestige(PrestigeLevel.PrestigeType type)
    {
        if (PrestigeList.Length - 1 < (int)type)
        {
            TDebug.LogError(string.Format("玩家宗门声望数据异常，length：{0}", PrestigeList.Length));
            return null;
        }       
        else
            return PrestigeList[(int)type];
    }

    public bool IsVip()
    {
        if (VipTime >= AppTimer.CurTimeStampMsSecond)
        {
            return true;
        }
        return false;
    }


    #region 道具获取
    //public void GetItem(List<Seraph.pb_Item> pbItemList)
    //{
    //    for (int i = 0; i < pbItemList.Count; i++)
    //    {
    //        GetItem(pbItemList[i]);
    //    }
    //}
    //public void GetItem(Seraph.pb_Item pbItem)
    //{
    //    ItemType itemType = ItemBase.GetItemType(pbItem.ItemConfigId);
    //    switch (itemType)
    //    {
    //        case ItemType.Money: m_Money += pbItem.ItemNum;
    //            break;
    //        case ItemType.Gold:
    //            break;
    //        case ItemType.PlayerExp: m_Exp += pbItem.ItemNum;
    //            break;
    //        case ItemType.Vitality: m_CurVitality += pbItem.ItemNum;
    //            break;
    //        case ItemType.Goods:
    //            break;
    //        case ItemType.Prop:
                
    //            break;
    //    }
    //}

    #endregion

    #region 测试
    static System.Random rand = new System.Random();
    public static GamePlayer GetTest()
    {
        GamePlayer d = new GamePlayer();

        d.Diamond = rand.Next(10000, 100000);

        return d;
    }
    #endregion
}



public enum WealthType : byte
{
    [EnumDesc("无")]
    None,
    [EnumDesc("灵石")]
    Gold,      //普通货币
    [EnumDesc("仙玉")]
    Diamond,   //充值货币
    [EnumDesc("潜能")]
    Potentail, //潜能
    [EnumDesc("经验")]
    Exp,
    Max
}

