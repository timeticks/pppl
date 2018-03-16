using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAccessor : DescObject
{
    public long NextRefreshTime;
    public Dictionary<int, int> EveryDayList = new Dictionary<int,int>();
    public Dictionary<int, int> HistoryList = new Dictionary<int, int>();

    public ShopAccessor()
    { }

    public ShopAccessor(NetPacket.S2C_SnapshotShopInfo msg)
    {
        this.NextRefreshTime = msg.NextFreshTime;
        this.EveryDayList = msg.EveryDayList ;
        this.HistoryList = msg.HistoryList;
    }
    public void AddShopInfo(NetPacket.S2C_SnapshotBuyShopInfo msg)
    {
        if (EveryDayList.ContainsKey(msg.CommodityId))
            EveryDayList[msg.CommodityId] = msg.DayNum;
        else if (msg.DayNum > 0)
            EveryDayList.Add(msg.CommodityId,msg.DayNum);
        if (HistoryList.ContainsKey(msg.CommodityId))
            HistoryList[msg.CommodityId] = msg.HisNum;
        else if (msg.HisNum > 0)
            HistoryList.Add(msg.CommodityId, msg.HisNum);
    }
}
