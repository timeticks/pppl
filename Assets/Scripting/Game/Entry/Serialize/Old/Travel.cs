using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ITravelFetcher
{
    Travel GetTravelByCopy(int idx);
    List<Travel> GetTravelList();
}
public class Travel : DescObject {

    private static ITravelFetcher mFetcher;
    public static ITravelFetcher TravelFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }


    private int mLevel;
    private string mDesc = "";
    private string mIcon = "";
    private int mExp; //每分钟经验
    private int mGold;//每分钟金钱
    private int mPotential;//每分钟潜能
    private int mEventTime;//触发时长(间隔毫秒)
    private List<int> mEventList;//事件集合
    private int[] mRepeatList = new int[0];//是否可重复
    private string mDialog1 = "";
    private string mDialog2 = "";
    private string mDialog3 = "";
    private string mDialog4 = "";
    public Travel():base()
    {

    }

    public Travel(Travel origin):base(origin) {
        this.mLevel      = origin.mLevel;
        this.mDesc       = origin.mDesc;
        this.mExp        = origin.mExp;
        this.mGold       = origin.mGold;
        this.mPotential  = origin.mPotential;
        this.mEventTime  = origin.mEventTime;
        this.mEventList  = origin.mEventList;
        this.mRepeatList = origin.mRepeatList;
        this.mIcon       = origin.mIcon;
        this.mDialog1 = origin.mDialog1;
        this.mDialog2 = origin.mDialog2;
        this.mDialog3 = origin.mDialog3;
        this.mDialog4 = origin.mDialog4;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mLevel = ios.ReadInt16();

        this.mGold = ios.ReadInt32();
        this.mExp = ios.ReadInt32();
        this.mPotential = ios.ReadInt32();

        this.mDesc = NetUtils.ReadUTF(ios);
        this.mIcon = NetUtils.ReadUTF(ios);
        this.mDialog1 = NetUtils.ReadUTF(ios);
        this.mDialog2 = NetUtils.ReadUTF(ios);
        this.mDialog3 = NetUtils.ReadUTF(ios);
        this.mDialog4 = NetUtils.ReadUTF(ios);

        int length = ios.ReadByte();
        this.mEventList = new List<int>();
        for (int i = 0; i < length; i++)
        {
            this.mEventList.Add(ios.ReadInt32());
        }
        length = ios.ReadByte();
        this.mRepeatList = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mRepeatList[i] = ios.ReadInt32();
        }
    }
    public int Level
    {
       get{ return mLevel;}
    }
    public string Desc
    {
        get { return mDesc; }
    }
    public int Exp
    {
        get { return mExp; }
    }
    public int Gold
    {
        get { return mGold; }
    }
    public int Potential
    {
        get { return mPotential; }
    }
    public int EventTime
    {
        get { return mEventTime; }
    }
    public List<int> EventList
    {
        get { return mEventList; }
    }
    public int[] RepeatList
    {
        get { return mRepeatList; }
    }
    public string Icon
    {
        get { return mIcon; }
    }
    public string[] Dialog
    {
        get { return new string[] { mDialog1,mDialog2,mDialog3,mDialog4}; }
    }
}
