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


    private LootGroup mType;
    private LootType[] mLootsType;
    private int[] mLootsId;
    private int[] mLootsNum;
    private int[] mLootsProp;

    public Loot() : base() { }

    public Loot(Loot origin): base(origin)
    {
        this.mType = origin.mType;
        this.mLootsType = origin.mLootsType;
        this.mLootsId = origin.mLootsId;
        this.mLootsNum = origin.mLootsNum;
        this.mLootsProp = origin.mLootsProp;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);

        this.mType = (LootGroup)ios.ReadByte();

        int length = ios.ReadByte();
        mLootsType = new LootType[length];
        for (int i = 0; i < length; i++)
        {
            mLootsType[i] = (LootType)ios.ReadByte();
        }

        length = ios.ReadByte();
        mLootsId = new int[length];
        for (int i = 0; i < length; i++)
        {
            mLootsId[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        mLootsNum = new int[length];
        for (int i = 0; i < length; i++)
        {
            mLootsNum[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        mLootsProp = new int[length];
        for (int i = 0; i < length; i++)
        {
            mLootsProp[i] = ios.ReadInt32();
        }
    }
    public LootType[] LootsType
    {
        get { return mLootsType; }
    }
    public int[] LootsId
    {
        get { return mLootsId; }
    }
    public int[] LootsNum
    {
        get { return mLootsNum; }
    }
    public LootGroup Type
    {
        get { return mType; }
    }
    /// <summary>
    /// 获取单个或组合掉落
    /// </summary>
    /// <param name="lootId"></param>
    /// <returns></returns>
    public static Dictionary<int, LootType> GetLootsId(int lootId)
    {
        Dictionary<int, LootType> Loots = new Dictionary<int, LootType>(); 
        Loot loot = Loot.LootFetcher.GetLootByCopy(lootId);
        if (loot == null)
            return null;
        if (loot.Type == LootGroup.Simple)
        {
            for (int i = 0; i < loot.LootsId.Length; i++)
            {
                if (!Loots.ContainsKey(loot.LootsId[i]))
                    Loots.Add(loot.LootsId[i],loot.LootsType[i]);
            }
        }
        else if (loot.Type == LootGroup.Group)
        {
            for (int i = 0; i < loot.mLootsId.Length; i++)
            {
                Loot tempLoot = Loot.LootFetcher.GetLootByCopy(loot.mLootsId[i]);
                for (int j = 0; j < tempLoot.LootsId.Length; j++)
                {
                    if (!Loots.ContainsKey(tempLoot.LootsId[j]))
                        Loots.Add(tempLoot.LootsId[j],tempLoot.LootsType[j]);
                }
            }
        }
        return Loots;
    }
    public static Dictionary<int, LootType> GetLootsId(List<int> lootIds)
    {
        Dictionary<int, LootType> Loots = new Dictionary<int, LootType>();
        for (int i = 0,length = lootIds.Count; i < length; i++)
        {
            int tempId = lootIds[i];
            if (tempId == 0) continue;
            Loot loot = Loot.LootFetcher.GetLootByCopy(tempId);
            if (loot == null) continue;
            if (loot.Type == LootGroup.Simple)
            {
                for (int j = 0; j < loot.LootsId.Length; j++)
                {
                    if (!Loots.ContainsKey(loot.LootsId[j]))
                        Loots.Add(loot.LootsId[j], loot.LootsType[j]);
                }
            }
            else if (loot.Type == LootGroup.Group)
            {
                for (int j = 0; j < loot.mLootsId.Length; j++)
                {
                    Loot tempLoot = Loot.LootFetcher.GetLootByCopy(loot.mLootsId[j]);
                    for (int k = 0; k < tempLoot.LootsId.Length; k++)
                    {
                        if (!Loots.ContainsKey(tempLoot.LootsId[k]))
                            Loots.Add(tempLoot.LootsId[k], tempLoot.LootsType[k]);
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
                for (int j = 0; j < loot.LootsId.Length; j++)
                {
                    GoodsToDrop g = new GoodsToDrop(loot.LootsId[j], loot.LootsNum[j], loot.LootsType[j]);
                    if(i==lootIds.Length-1)
                        str.Append(string.Format("{0}", g.GetString()));
                    else
                        str.Append(string.Format("{0}\n", g.GetString()));
                }
            }
        }
        return str.ToString();
    }
}
public class GoodsToDrop
{
    public int GoodsIdx;
    public int Amount;
    public LootType MyType;
    public GoodsToDrop()
    {
    }
    public GoodsToDrop(int goodsIdx , int amount , LootType ty)
    {
        GoodsIdx = goodsIdx;
        Amount = amount;
        MyType = ty;
    }
    public void Serialize(BinaryReader ios)
    {
        MyType = (LootType)ios.ReadByte();
        GoodsIdx = ios.ReadInt32();
        Amount = ios.ReadInt32();
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
        return string.Format("{0}×{1}", TUtility.TryGetLootGoodsName(MyType, GoodsIdx), Amount);
    }


    public static GoodsToDrop CreateWealth(WealthType ty, int num)
    {
        GoodsToDrop goods = new GoodsToDrop();
        goods.Amount = num;
        goods.MyType = LootType.Money;
        goods.GoodsIdx = ty.ToInt();
        return goods;
    }
    public static GoodsToDrop CreatePrestige(PrestigeLevel.PrestigeType ty, int num)
    {
        GoodsToDrop goods = new GoodsToDrop();
        goods.Amount = num;
        goods.MyType = LootType.Prestige;
        goods.GoodsIdx = ty.ToInt();
        return goods;
    }
}
public enum LootType
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