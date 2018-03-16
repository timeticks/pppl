using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ITravelSmallEventFetcher
{
    TravelSmallEvent GetTravelSmallEventByCopy(int idx);
}
public class TravelSmallEvent : DescObject {

    private static ITravelSmallEventFetcher mFetcher;
    public static ITravelSmallEventFetcher TravelSmallEventFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private int    mLootID;//掉落包
    private string mBtnDesc;
    private string mDesc = "";
    private string mWinDesc = "";//成功描述
    private string mLoseDesc = "";//失败描述
    private EventType mEventType = EventType.None;
    private int[]   mEventTypeMisc ;
    public EventState eventState = EventState.None;
    public enum EventState
    {
        None,//未完成
        Finished,//完成
        Failed,//失败
    }
    public enum EventType
    {
        //1=等级判定
        //2=系统时间判定
        //3=货币判定
        //4=道具判定
        //5=属性判定
        //6=随机判定
        //7=生活技能判定
        //8=战斗判定
        //9=事件判定
        //10=宗门声望
        //11=宗门职位
        None,
        Level,
        Time,
        Money,
        Goods,
        Attribute,
        Rand,
        AuxSkill,
        PVE,
        Event,
        Max,
    }
    public TravelSmallEvent() : base() { }
    public TravelSmallEvent(TravelSmallEvent origin) : base(origin)
    {
        this.mLootID = origin.mLootID;
        this.mBtnDesc = origin.mBtnDesc;
        this.mDesc = origin.mDesc;
        this.mWinDesc = origin.mWinDesc;
        this.mLoseDesc = origin.mLoseDesc;
        this.mEventType = origin.mEventType;
        this.mEventTypeMisc = origin.mEventTypeMisc;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mEventType = (EventType)ios.ReadByte();
        
       
        this.mLootID = ios.ReadInt32(); 
        
        this.mBtnDesc = NetUtils.ReadUTF(ios);
        this.mDesc = NetUtils.ReadUTF(ios);
        this.mWinDesc = NetUtils.ReadUTF(ios);
        this.mLoseDesc = NetUtils.ReadUTF(ios);

        int length = ios.ReadByte();
        mEventTypeMisc = new int[length];
        for (int i = 0; i < length; i++)
        {
            mEventTypeMisc[i] = ios.ReadInt32();
        }

    }
    public int LootId
    {
        get { return mLootID; }
    }
    public string BtnDesc
    {
        get { return mBtnDesc; }
    }
    public string  Desc
    {
        get { return mDesc; }
    }
    public string WinDesc
    {
        get { return mWinDesc; }
    }
    public string LoseDesc
    {
        get { return mLoseDesc; }
    }
    public EventType eventType
    {
        get { return mEventType; }
    }
    public int[] EventTypeMisc
    {
        get { return mEventTypeMisc; }
    }
}
