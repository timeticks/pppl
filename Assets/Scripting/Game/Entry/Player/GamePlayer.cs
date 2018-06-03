using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;

public class GamePlayerBase
{
    public string Name;
    public Eint BirthTime = 0;          //修炼时间
    public Eint PlayerUid = 0;          //唯一id

    public Eint Level = 1;
    public Elong Exp = 0;       //当前经验
    public Eint Gold = 0;       //普通货币
    public Eint Diamond = 0;    //充值货币

    public Eint GuideStepIndex=0;     //游戏进度
    public Eint UnlockMapLevel=0;     //当前已解锁的地图

    public Dictionary<Eint, Eint> ProduceDict = new Dictionary<Eint, Eint>();

    public Eint PlayerIdx = 0;
    public Dictionary<Eint, Eint> NatureDict = new Dictionary<Eint, Eint>(); //能力NatureType

    //统计记录
    public Dictionary<Eint, Eint> BuyCommodityDict = new Dictionary<Eint, Eint>();  //已购买商品<商品id、已买数量>
    public Dictionary<Eint, Eint> BuyRechargeDict = new Dictionary<Eint, Eint>();   //已充值   <充值id、已买数量>
    public Eint BattleNum = 0;              //战斗次数
    public Eint MaxScore = 0;               //最大分数
    public Eint MaxScoreTime = 0;           //最大分数获得时间

    public Eint AllScore = 0;               //所有的分数
    public Dictionary<Eint, Eint> ChatStatis = new Dictionary<Eint, Eint>();    //聊天统计<时间/h，次数>
    public Elong PartnerBirthTime = 0;      //同伴解锁时间

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
        
    }

    public int GetNatureLevel(NatureType natureTy)
    {
        if (!NatureDict.ContainsKey((int)natureTy))
            NatureDict.Add((int)natureTy, 0);
        return NatureDict[(int)natureTy];
    }

    public NatureLevelUp GetNatureLevelUp(NatureType natureTy)
    {
        int natureLevel = PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(natureTy);
        NatureLevelUp nature =
            NatureLevelUp.Fetcher.GetNatureLevelUpCopy(natureTy, natureLevel, false);   //掉落加成
        return nature;
    }

    public int GetPromTypeNum(AttrType type)
    {
        Hero hero = PlayerPrefsBridge.Instance.GetHeroWithProperties();
        return hero.GetAttr(type);
    }

    public void AddRecharge(int rechargeIdx , bool isSave=true)
    {
        if (BuyRechargeDict.ContainsKey(rechargeIdx))
            BuyRechargeDict[rechargeIdx]++;
        else
            BuyRechargeDict.Add(rechargeIdx, 1);
        if(isSave) PlayerPrefsBridge.Instance.savePlayerModule();
    }
    public void AddCommodity(int commondityIdx, int num, bool isSave = true)
    {
        if (BuyCommodityDict.ContainsKey(commondityIdx))
            BuyCommodityDict[commondityIdx] += num;
        else
            BuyCommodityDict.Add(commondityIdx, num);
        if (isSave) PlayerPrefsBridge.Instance.savePlayerModule();
    }
    public int GetCommodityRemain(int commodityIdx)
    {
        Commodity com = Commodity.CommodityFetcher.GetCommodityByCopy(commodityIdx, false);
        if (com.limit > 0 && BuyCommodityDict.ContainsKey(commodityIdx))
            return com.limit - BuyCommodityDict[commodityIdx];
        return com.limit;
    }
    public int GetRechargeRemain(int rechargeIdx)
    {
        Recharge recharge = Recharge.Fetcher.GetRechargeCopy(rechargeIdx, false);
        if (recharge.limit > 0 && BuyRechargeDict.ContainsKey(rechargeIdx))
            return recharge.limit - BuyRechargeDict[rechargeIdx];
        return recharge.limit;
    }

    public bool IsVip()
    {
        return false;
    }


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
