using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ICommodityFetcher
{
    Commodity GetCommodityByCopy(int idx);
}
public class Commodity : DescObject {

    private static ICommodityFetcher mFetcher;
    public static ICommodityFetcher CommodityFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    

    public string mDesc = "";

   
    public SellType mSellType = SellType.Wealth;//价格类型
    public int mSellId;       //价格类型ID
    public int mNumber;       //消耗数量
    public int mOrder;        //顺序

    public int mComId ;     //商品LootID
    public int mDayNumber;  //每日购买数量
    public int mHisNumber;  //历史购买数量

    public ConditionType mConditionType = ConditionType.Level;   //开放条件
    public int[] mConditionValue;     //开放条件值

    private Sect.SectType mSect = Sect.SectType.None;

    public bool IsUnLock = false;

    public bool IsSoldOut = false; // 总限购剩余数量为零
    public enum ConditionType
    {
        None,
        Level,  //角色等级
        Fame,   //声望
    }

    public enum SellType
    {
    	Wealth,
    	Item,
    }

    public Commodity(): base(){  }

    public Commodity(Commodity origin): base(origin)
    {
        this.mDesc = origin.mDesc;
        this.mSellType = origin.mSellType;
        this.mSellId = origin.mSellId;
        this.mNumber = origin.mNumber;
        this.mComId = origin.mComId;
        this.mDayNumber = origin.mDayNumber;
        this.mHisNumber = origin.mHisNumber;
        this.mConditionType = origin.mConditionType;
        this.mConditionValue = origin.mConditionValue;
        this.mSect = origin.mSect;
        this.mOrder = origin.mOrder;
    }

    public override void Serialize(BinaryReader ios)
    { 
        base.Serialize(ios);
        this.mSellType = (SellType)ios.ReadByte();
        this.mConditionType = (ConditionType)ios.ReadByte();
        this.mSect = (Sect.SectType)ios.ReadByte();

        this.mOrder = ios.ReadInt16();

        this.mSellId    = ios.ReadInt32();
        this.mNumber    = ios.ReadInt32();
        this.mComId     = ios.ReadInt32();
        this.mDayNumber = ios.ReadInt32();
        this.mHisNumber = ios.ReadInt32();

        this.mDesc = NetUtils.ReadUTF(ios);
        int length = ios.ReadByte();
        this.mConditionValue = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mConditionValue[i] = ios.ReadInt16();
        }

    }

    public static bool GetCommodityOpen(Commodity com,out string openCon)
    {
        GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
        switch (com.MConditionType)
        {
            case ConditionType.Level:
                {
                    if (player.Level < com.ConditionValue[0])
                    {
                        openCon = string.Format("人物等级达到:{0}级",com.ConditionValue[0]);
                        return false;
                    }
                    break;
                }

            case ConditionType.Fame:
                {
                    PrestigeLevel prestigeLevel = player.GetPrestige((PrestigeLevel.PrestigeType)com.ConditionValue[0]);
                    if (prestigeLevel.Level < com.ConditionValue[1])
                    {
                        openCon = string.Format("声望等级达到:{0}阶", com.ConditionValue[1]);
                        return false;
                    }
                    break;
                }
            default:
                openCon = "";
                return false;
        }
        openCon = "";
        return true;     

    }

    public Sect.SectType MySect
    {
        get { return mSect; }
    }

    public string Desc
    {
        get { return mDesc; }
    }

    public SellType MySellType
    {
        get { return mSellType; }
    }

    public int SellId
    {
        get { return mSellId; }
    }

    public int Number
    {
        get { return mNumber; }
    }

    public int ComId
    {
        get { return mComId; }
    }

    public int DayNumber
    {
        get { return mDayNumber; }
    }

    public int HisNumber
    {
        get { return mHisNumber; }
    }
    public ConditionType MConditionType
    {
        get { return mConditionType; }
    }
    public int[] ConditionValue
    {
        get { return mConditionValue; }
    }
    public int Order
    {
        get { return mOrder; }
    }
}
