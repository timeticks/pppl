using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public interface ILootFetcher
{
    Loot GetLootByCopy(int idx);
}
public class Loot :DescObject {

    private static ILootFetcher mFetcher;
    public static ILootFetcher LootFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public enum LootActionType //掉落的原因，以便客户端显示对应
    {
        None,
        Recipe,
        UseItem,
        NpcLoot,
        Travel,
        Tower,
    }

    
    public enum LootGroup
    {
        None,
        Simple, //普通掉落
        Group,  //组合掉落
    }


    public LootItemType[] lootItemType;
    public int[] lootMisc;
    public int[] lootNum;
    public int[] lootProp;
    public LootGroup groupType;

    public Loot() : base() { }

    public Loot Clone()
    {
        return this.MemberwiseClone() as Loot;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
    }

    /// <summary>
    /// 获取单个或组合掉落
    /// </summary>
    /// <param name="lootId"></param>
    /// <returns></returns>
    public static Dictionary<int, LootItemType> GetLootsId(int lootId)
    {
        Dictionary<int, LootItemType> Loots = new Dictionary<int, LootItemType>(); 
        Loot loot = Loot.LootFetcher.GetLootByCopy(lootId);
        if (loot == null)
            return null;
        if (loot.groupType == LootGroup.Simple)
        {
            for (int i = 0; i < loot.lootMisc.Length; i++)
            {
                if (!Loots.ContainsKey(loot.lootMisc[i]))
                    Loots.Add(loot.lootMisc[i],loot.lootItemType[i]);
            }
        }
        else if (loot.groupType == LootGroup.Group)
        {
            for (int i = 0; i < loot.lootMisc.Length; i++)
            {
                Loot tempLoot = Loot.LootFetcher.GetLootByCopy(loot.lootMisc[i]);
                for (int j = 0; j < tempLoot.lootMisc.Length; j++)
                {
                    if (!Loots.ContainsKey(tempLoot.lootMisc[j]))
                        Loots.Add(tempLoot.lootMisc[j],tempLoot.lootItemType[j]);
                }
            }
        }
        return Loots;
    }
    public static Dictionary<int, LootItemType> GetLootsId(List<int> lootIds)
    {
        Dictionary<int, LootItemType> Loots = new Dictionary<int, LootItemType>();
        for (int i = 0,length = lootIds.Count; i < length; i++)
        {
            int tempId = lootIds[i];
            if (tempId == 0) continue;
            Loot loot = Loot.LootFetcher.GetLootByCopy(tempId);
            if (loot == null) continue;
            if (loot.groupType == LootGroup.Simple)
            {
                for (int j = 0; j < loot.lootMisc.Length; j++)
                {
                    if (!Loots.ContainsKey(loot.lootMisc[j]))
                        Loots.Add(loot.lootMisc[j], loot.lootItemType[j]);
                }
            }
            else if (loot.groupType == LootGroup.Group)
            {
                for (int j = 0; j < loot.lootMisc.Length; j++)
                {
                    Loot tempLoot = Loot.LootFetcher.GetLootByCopy(loot.lootMisc[j]);
                    for (int k = 0; k < tempLoot.lootMisc.Length; k++)
                    {
                        if (!Loots.ContainsKey(tempLoot.lootMisc[k]))
                            Loots.Add(tempLoot.lootMisc[k], tempLoot.lootItemType[k]);
                    }
                }
            }
        }      
        return Loots;
    }


    public static string GetGoodsListString(params int[] lootIds)
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < lootIds.Length; i++)
        {
            if (lootIds[i] > 0)
            {
                Loot loot = Loot.LootFetcher.GetLootByCopy(lootIds[i]);
                for (int j = 0; j < loot.lootMisc.Length; j++)
                {
                    GoodsToDrop g = new GoodsToDrop(loot.lootMisc[j], loot.lootNum[j], loot.lootItemType[j]);
                    if(i==lootIds.Length-1)
                        str.Append(string.Format("{0}", g.GetString()));
                    else
                        str.Append(string.Format("{0}\n", g.GetString()));
                }
            }
        }
        return str.ToString();
    }

    #region 计算Loot掉落

    List<GoodsToDrop> simpleDropLoots(float addCoeffi)
    {
        List<GoodsToDrop> goodsToDropList = new List<GoodsToDrop>();
        GoodsToDrop goodsToDrop = null;
        for (int i = 0, length = lootMisc.Length; i < length; i++)
        {
            if (!GameUtils.isTrue((int)(lootProp[i] * addCoeffi)))
                continue;
            goodsToDrop = new GoodsToDrop();
            goodsToDrop.goodsIdx = lootMisc[i];
            goodsToDrop.amount = lootNum[i];
            goodsToDrop.lootItemType = lootItemType[i];
            goodsToDropList.Add(goodsToDrop);
        }
        return goodsToDropList;
    }

    List<GoodsToDrop> bigDropLoots(float addCoeffi)
    {
        int randomPos = GameUtils.GetRandomIndex(lootProp);
        Loot lootDrop = Loot.LootFetcher.GetLootByCopy(lootMisc[randomPos]);
        if (lootDrop != null)
            return lootDrop.simpleDropLoots(addCoeffi);
        return new List<GoodsToDrop>();
    }

    public List<GoodsToDrop> onDropLoots(float addCoeffi) //直接根据loot的类型，将最终掉落的物品赋给goodsToDropList
    {
        List<GoodsToDrop> goodsToDropList = new List<GoodsToDrop>();
        if(groupType == LootGroup.Simple)
        {
            return simpleDropLoots(addCoeffi);
        }
        else if (groupType == LootGroup.Group)
        {
            return bigDropLoots(addCoeffi);
        }
        return new List<GoodsToDrop>();
    }

    #endregion
}
public class GoodsToDrop
{
    public int goodsIdx;
    public int amount;
    public LootItemType lootItemType = LootItemType.None;
    public GoodsToDrop()
    {
    }
    public GoodsToDrop(int goodsId , int amountNum , LootItemType ty)
    {
        goodsIdx = goodsId;
        amount = amountNum;
        lootItemType = ty;
    }
    public void Serialize(BinaryReader ios)
    {
        lootItemType = (LootItemType)ios.ReadByte();
        goodsIdx = ios.ReadInt32();
        amount = ios.ReadInt32();
    }
    public long getKey()
    {
        return ((long)goodsIdx) * 100 + (int)lootItemType;
    }
    public static GoodsToDrop[] SerializeList(BinaryReader ios)
    {
        int length = ios.ReadByte();
        GoodsToDrop[] goodsList = new GoodsToDrop[length];
        GoodsToDrop goods;
        for (int i = 0; i < length; i++)
        {
            goods = new GoodsToDrop();
            goods.Serialize(ios);
            goodsList[i] = goods;
        }
        return goodsList;
    }

    public string GetString()
    {
        return string.Format("{0}×{1}", TUtility.TryGetLootGoodsName(lootItemType, goodsIdx), amount);
    }


    public static GoodsToDrop CreateWealth(WealthType ty, int num)
    {
        GoodsToDrop goods = new GoodsToDrop();
        goods.amount = num;
        goods.lootItemType = LootItemType.Money;
        goods.goodsIdx = ty.ToInt();
        return goods;
    }
    public static GoodsToDrop CreatePrestige(PrestigeLevel.PrestigeType ty, int num)
    {
        GoodsToDrop goods = new GoodsToDrop();
        goods.amount = num;
        goods.lootItemType = LootItemType.Prestige;
        goods.goodsIdx = ty.ToInt();
        return goods;
    }

    public static string getListString(List<GoodsToDrop> goodsList)
    {
        StringBuilder lootStr = new StringBuilder();
        for (int i = 0; i < goodsList.Count; i++)
        {
            lootStr.Append(string.Format("{0}\n", goodsList[i].GetString()));
        }
        return lootStr.ToString();
    }

    /// <summary>
    /// 将GoodsToDrop中，能合并的进行数量合并
    /// </summary>
    /// <param name="goodsToDropList"></param>
    /// <returns></returns>
    public static List<GoodsToDrop> combineList(List<GoodsToDrop> goodsToDropList)
    {
        Dictionary<long, GoodsToDrop> goodsMap = new Dictionary<long, GoodsToDrop>();
        List<GoodsToDrop> goodsToDrops = new List<GoodsToDrop>();
        GoodsToDrop tempGoods;
        for (int i = 0; i < goodsToDropList.Count; i++)
        {
            tempGoods = goodsToDropList[i];
            if (tempGoods == null) continue;
            if (tempGoods.lootItemType == LootItemType.Equip)	//如果是装备，不将其数量合并在一起
                goodsToDrops.Add(tempGoods);
            else
            {
                long goodsKey = tempGoods.getKey();
                if (goodsMap.ContainsKey(goodsKey))
                    tempGoods.amount += goodsMap[goodsKey].amount;
                else
                    goodsMap.Add(goodsKey, tempGoods);
            }
        }
        foreach (var temp in goodsMap.Values)
        {
            goodsToDrops.Add(temp);
        }
        return goodsToDrops;
    }


}
public enum LootItemType
{
    None,
    Money,
    Item,
    Pet,
    Spell,
    Equip,
    Recipe,//配方
    Prestige,
    Stuff,  //原材料
    Max
}