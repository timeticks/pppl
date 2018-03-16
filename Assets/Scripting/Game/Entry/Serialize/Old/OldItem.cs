using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//public interface IItemFetcher
//{
//    Item GetItemByCopy(int idx);
//    List<Item> GetItemsAll();
//}

//public class Item : DescObject
//{
//    private static IOldItemFetcher mFetcher;
//    public static IOldItemFetcher ItemFetcher
//    {
//        get { return mFetcher; }
//        set
//        {
//            if (mFetcher == null)
//                mFetcher = value;
//        }
//    }

//    private ItemType mType = ItemType.None;
//    private byte mQuality;          //物品品质
//    private short mLevel;            //物品初始等级
//    private short mMaxStack;         //最大叠放数量  最大999
//    private short mMinUseLevel;      //使用等级下限
//    private short mMaxUseLevel;      //使用等级上限
//    private short mOwnNum;           //最大拥有数量
//    private bool mCanSell;          //是否可出售
//    private int mSellPrice;        //卖店获得代币值
//    private ItemEffectType mEffectType;       //物品使用效果
//    private int[] mEffectMisc;      //效果参数
//    private string mIcon;            //图标
//    private string mDesc;            //描述

//    public int curNum;

//    public enum ItemEffectType
//    {
//        None,
//        Retreat, //闭关
//        Zazen,   //打坐
//        Loot,    //获取掉落
//        AddProm, // 属性增加
//    }

//    public bool CanUse()
//    {
//        return mEffectType == ItemEffectType.Loot || mEffectType == ItemEffectType.AddProm;
//    }

//    public Item()
//        : base()
//    {

//    }

//    public enum ItemType
//    {
//        None = 0,
//        Task,
//        Stuff,
//        Drug,
//        Max
//    }


//    public Item(Item origin)
//        : base(origin)
//    {
//        this.mIcon = origin.mIcon;
//        this.mDesc = origin.mDesc;
//        this.mType = origin.mType;
//        this.mQuality = origin.mQuality;
//        this.mCanSell = origin.mCanSell;
//        this.mLevel = origin.mLevel;
//        this.mMaxStack = origin.mMaxStack;
//        this.mMinUseLevel = origin.mMinUseLevel;
//        this.mMaxUseLevel = origin.mMaxUseLevel;
//        this.mOwnNum = origin.mOwnNum;
//        this.mSellPrice = origin.mSellPrice;
//        this.mEffectType = origin.mEffectType;
//        this.mEffectMisc = origin.mEffectMisc;
//        this.curNum = origin.curNum;
//    }

//    public override void Serialize(BinaryReader ios)
//    {
//        base.Serialize(ios);

//        this.mQuality = ios.ReadByte();
//        this.mType = (ItemType)ios.ReadByte();
//        this.mEffectType = (ItemEffectType)ios.ReadByte();

//        this.mCanSell = ios.ReadBoolean();
//        this.mLevel = ios.ReadInt16();
//        this.mMaxStack = ios.ReadInt16();
//        this.mMinUseLevel = ios.ReadInt16();
//        this.mMaxUseLevel = ios.ReadInt16();
//        this.mOwnNum = ios.ReadInt16();

//        this.mSellPrice = ios.ReadInt32();

//        this.mIcon = NetUtils.ReadUTF(ios);
//        this.mDesc = NetUtils.ReadUTF(ios);

//        int length = ios.ReadByte();
//        this.mEffectMisc = new int[length];
//        for (byte i = 0; i < length; i++)
//        {
//            this.mEffectMisc[i] = ios.ReadInt32();
//        }
//    }

//    /// <summary>
//    /// 根据使用效果获取背包中的物品
//    /// </summary>
//    /// <param name="type"></param>
//    /// <returns></returns>
//    public static List<Item> GetEffectItemsOwn(ItemEffectType type)
//    {
//        List<Item> tempList = new List<Item>();
//        List<Item> ownItemList = PlayerPrefsBridge.Instance.GetItemAllListCopy();
//        for (int i = 0; i < ownItemList.Count; i++)
//        {
//            if (ownItemList[i].EffectType == type)
//                tempList.Add(ownItemList[i]);
//        }
//        if (tempList.Count == 0)
//            return null;
//        else
//            return tempList;
//    }
//    public static List<Item> GetEffectItemsAll(ItemEffectType type)
//    {
//        List<Item> tempList = new List<Item>();
//        List<Item> ownItemList = ItemFetcher.GetItemsAll();
//        for (int i = 0; i < ownItemList.Count; i++)
//        {
//            if (ownItemList[i].EffectType == type)
//                tempList.Add(ownItemList[i]);
//        }
//        if (tempList.Count == 0)
//            return null;
//        else
//            return tempList;
//    }

//    public string icon
//    {
//        get { return mIcon; }
//    }

//    public string desc
//    {
//        get { return mDesc; }
//    }

//    public ItemType type
//    {
//        get { return mType; }
//    }

//    public byte quality
//    {
//        get { return mQuality; }
//    }

//    public short level        //配置表中的初始等级
//    {
//        get { return mLevel; }
//        set { mLevel = value; }
//    }

//    public short maxStack
//    {
//        get { return mMaxStack; }
//    }

//    public short MinUseLevel
//    {
//        get { return mMinUseLevel; }
//    }

//    public short MaxUseLevel
//    {
//        get { return mMaxUseLevel; }
//    }

//    public short OwnNum
//    {
//        get { return mOwnNum; }
//    }

//    public bool canSell
//    {
//        get { return mCanSell; }
//    }

//    public int sell
//    {
//        get { return mSellPrice; }
//    }

//    public ItemEffectType EffectType
//    {
//        get { return mEffectType; }
//    }

//    public int[] EffectMisc1
//    {
//        get { return mEffectMisc; }
//    }

//}
