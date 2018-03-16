using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface IShopFetcher
{
    Shop GetShopByCopy(int idx);
}
public class Shop : DescObject {

    private static IShopFetcher mFetcher;
    public static IShopFetcher ShopFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    

    private ShopType mType=ShopType.Mall;
    public enum ShopType
    {
        None,
    	Mall,      //商城
    	Mark,      //坊市
    	FameShop,  //声望商店
    	SubShop,   //子商店
    }
    
    
    private int   mTheId;                     //所属npcId
    private int[] mSubShop = new int[0];
    
    private ConditionType mConditionType = ConditionType.Level;   //开放条件
    private int[]   mConditionValue ;     //开放条件值

    private PrestigeLevel.PrestigeType mPreType = PrestigeLevel.PrestigeType.Max; // 商店归属宗门
    public enum ConditionType
    {
        None,
    	Level,  //角色等级
    	Fame,   //声望
    }
    
    public int[] mCommodity = new int[0];


    public Shop(): base(){  }

    public Shop(Shop origin): base(origin)
    {
        this.mType                  = origin.mType;
        this.mTheId                 = origin.mTheId;
        this.mSubShop               = origin.mSubShop;
        this.mConditionType         = origin.mConditionType;
        this.mConditionValue        = origin.mConditionValue;
        this.mCommodity             = origin.mCommodity;
        this.mPreType               = origin.mPreType;
    }

    public override void Serialize(BinaryReader ios)
    { 
        base.Serialize(ios);
        this.mType = (ShopType)ios.ReadByte();
        this.mConditionType = (ConditionType)ios.ReadByte();
        this.mPreType = (PrestigeLevel.PrestigeType)ios.ReadByte();

        this.mTheId = ios.ReadInt32();

        int length = ios.ReadByte();
        this.mConditionValue = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mConditionValue[i] = ios.ReadInt16();
        }
    
        length = ios.ReadByte();
        this.mSubShop = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mSubShop[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        this.mCommodity = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mCommodity[i] = ios.ReadInt32();
        }
    }

    public static bool GetShopOpen(int shopId,out string statusStr)
    {
        Shop shop = Shop.ShopFetcher.GetShopByCopy(shopId);
        GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
        if (shop == null)
        {
            statusStr = "";
            return false;
        }        
        switch (shop.MConditionType)
        {
            case ConditionType.Level:
                {
                    if (player.Level < shop.ConditionValue[0])
                    {
                        statusStr = "人物等级不足";
                        return false;
                    }
                    break;
                }

            case ConditionType.Fame:
                {
                    PrestigeLevel prestigeLevel = player.GetPrestige((PrestigeLevel.PrestigeType)shop.ConditionValue[0]);
                    if (prestigeLevel.Level < shop.ConditionValue[1])
                    {
                        statusStr = "声望等级不足";
                        return false;
                    }
                    break;
                }
            default:
                statusStr = "";
                return false;
        }
        statusStr = "";
        return true;     
    }

    public ShopType Type
    {
        get { return mType; }
    }

    public int TheId
    {
        get { return mTheId; }
    }

    public int[] SubShop
    {
        get { return mSubShop; }
    }

    public ConditionType MConditionType
    {
        get { return mConditionType; }
    }

    public int[] ConditionValue
    {
        get { return mConditionValue; }
    }

    public int[] Commodity
    {
        get { return mCommodity; }
    }
    public PrestigeLevel.PrestigeType PrestigeType
    {
        get { return mPreType; }
    }
}
