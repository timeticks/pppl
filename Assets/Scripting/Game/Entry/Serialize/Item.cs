using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IItemFetcher
{
    Item GetItemCopy(int idx);
}
public class ItemBase : BaseObject
{
    public int num = 0;

    public ItemBase()
    {
    }

    public ItemBase(ItemBase origin) : base(origin)
    {
        num = origin.num;
    }

    public void CopyBy(ItemBase origin)
    {
        num = origin.num;
    }
}

public class Item : ItemBase
{
    private static IItemFetcher mFetcher;
    public static IItemFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }


    public enum ItemType
    {
        None = 0,
        Task,
        Stuff,
        Drug,
        Max
    }

    public ItemType type;       //类型
    public string desc ;
    public string icon;
    public int subType ;
    public int quality;
    public int level ;
    public int useLimitType ;   //使用条件
    public int useLimitValue ;
    public bool canSell ;
    public int sell ;
    public int maxStack ;
    public int ownNum;          //是否是唯一道具

    public Item()
    {
    }
    public Item Clone()
    {
        return this.MemberwiseClone() as Item;
    }

    public bool CanUse()
    {
        return false;
    }


}
