using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class TravelAccessor : DescObject
{
    /// <summary>
    /// 游历挂机场景ID
    /// </summary>
    public int  TravelIdx;
    /// <summary>
    /// 游历挂机开始时间
    /// </summary>
    public long StartTime;
    /// <summary>
    /// 游历挂机下一次时间刷新时间
    /// </summary>
    public long NextEventTime;

    /// <summary>
    ///游历挂机小时间状态
    /// </summary>
    public int[] SubEventStatus = new int[0];
    /// <summary>
    /// 游历挂机大事件状态
    /// </summary>
    public int EventStatus;

   // public  Dictionary<int, TravelSmallEvent[]> TravelEvents = new Dictionary<int, TravelSmallEvent[]>(); //正在进行中的大事件情况

    public List<TravelEvent> TravelEventList;
    public Dictionary<int, List<TravelEventProgress>> TravelEventProgressList = new Dictionary<int,List<TravelEventProgress>>();

    /// <summary>
    /// travelEventId   TravelEvent
    /// </summary>
    public Dictionary<int, int> TravelEventDic = new Dictionary<int,int>(); 

    //===================游历离线消息===========
    public int ExpOffLine;
    public int PotentialOffLine;
    public int GoldOffLine;
    public TravelAccessor()
    {

    }


    public TravelAccessor(TravelAccessor origin) : base(origin)
    {
        TravelIdx = origin.TravelIdx;
        StartTime = origin.StartTime;
        NextEventTime = origin.NextEventTime;
        TravelEventList = origin.TravelEventList;
        ExpOffLine = origin.ExpOffLine;
        PotentialOffLine = origin.PotentialOffLine;
        GoldOffLine = origin.GoldOffLine;
    }

    public  void InitTravelAccessor(NetPacket.S2C_SnapshotTravelBotting msg)
    {
        this.TravelIdx      = msg.TravelIdx;
        this.StartTime      = msg.StartTime;
        this.TravelEventDic = msg.TravelEventProgress;
    }
    public void InitTravelAccessor(NetPacket.S2C_SnapshotOffLine msg)
    {
        this.ExpOffLine = msg.Exp;
        this.PotentialOffLine = msg.Potential;
        this.GoldOffLine = msg.Gold;
    }

}

public class TravelEventProgress
{
    public int TravelEventID;
    public int FinishSmallEventNum;
    public TravelEventProgress(TravelEventProgress origin)
    {
        this.TravelEventID = origin.TravelEventID;
        this.FinishSmallEventNum = origin.FinishSmallEventNum;
    }
    public TravelEventProgress() { }
}
