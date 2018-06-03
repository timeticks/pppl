using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ICommodityFetcher
{
    Commodity GetCommodityByCopy(int idx,bool isCopy);
    List<Commodity> GetCommodityListNoCopy(int storeType);
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
    

    public string desc = "";
    public LootItemType sellType = LootItemType.None;//价格类型
    public int sellId;        //价格类型ID
    public int number;        //消耗数量
    public int order;           //顺序
    public int storeType;       //商店类型

    public int lootId ;    //商品LootID
    public int limit;  //历史购买数量
    public int batchMaxNum; //可批量购买数量

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

    public Commodity Clone()
    {
        return this.MemberwiseClone() as Commodity;
    }

    public bool CheckLegal()
    {
        return true;
    }


    public static bool GetCommodityOpen(Commodity com,out string openCon)
    {
        GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
        //switch (com.mConditionType)
        //{
        //    case ConditionType.Level:
        //        {
        //            if (player.Level < com.ConditionValue[0])
        //            {
        //                openCon = string.Format("人物等级达到:{0}级",com.ConditionValue[0]);
        //                return false;
        //            }
        //            break;
        //        }

        //    case ConditionType.Fame:
        //        {
        //            PrestigeLevel prestigeLevel = player.GetPrestige((PrestigeLevel.PrestigeType)com.ConditionValue[0]);
        //            if (prestigeLevel.Level < com.ConditionValue[1])
        //            {
        //                openCon = string.Format("声望等级达到:{0}阶", com.ConditionValue[1]);
        //                return false;
        //            }
        //            break;
        //        }
        //    default:
        //        openCon = "";
        //        return false;
        //}
        openCon = "";
        return true;     

    }
}
