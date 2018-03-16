using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ITravelEventFetcher
{
    TravelEvent GetTravelEventByCopy(int idx);
}
public class TravelEvent : DescObject {

    private static ITravelEventFetcher mFetcher;
    public static ITravelEventFetcher TravelEventFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private string mDesc = "";
    private string mTitle = "";
    private int mMaxLimit;//事件最大限制
    private int[] mEventList = new int[0];//小事件列表
    private int mFinishedEvents =0; //已完成小事件数
    private int mAchieve;
    public int CurSmallEventIndex;//当前进行小事件下标

    public TravelEvent(): base(){  }

    public TravelEvent(TravelEvent origin): base(origin)
    {
        this.mDesc                  = origin.mDesc;
        this.mTitle                 = origin.mTitle;
        this.mEventList             = origin.mEventList;
        this.mAchieve               = origin.mAchieve;
    }

    public override void Serialize(BinaryReader ios)
    { 
        base.Serialize(ios);
        this.mAchieve = ios.ReadInt32();

        this.mTitle = NetUtils.ReadUTF(ios);
        this.mDesc = NetUtils.ReadUTF(ios);

        int length = ios.ReadByte();
        this.mEventList = new int [length];
        for (int i = 0; i < length; i++)
        {
            this.mEventList[i] = ios.ReadInt32();
        }
    }

    public string Title
    {
        get { return mTitle; }
    }
    public string Desc
    {
        get { return mDesc; }
    }
    public int[] EventList
    {
        get { return mEventList; }
    }
    public int Achieve
    {
        get { return mAchieve; }
    }
    public int FinishedEvents
    {
        get { return mFinishedEvents; }
    }
}
