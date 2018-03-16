using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface IPrestigeTaskFetcher
{
    PrestigeTask GetPrestigeTaskByCopy(int idx);
    //List<PrestigeTask> GetPrestigeTaskList();
}
public class PrestigeTask : DescObject {

    private static IPrestigeTaskFetcher mFetcher;
    public static IPrestigeTaskFetcher PrestigeTaskFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }


    public string mDesc = "";


    public int mQuality;
    public int mTime;    //耗时，毫秒
    public int mMoney;
    public PrestigeLevel.PrestigeType mType = PrestigeLevel.PrestigeType.Self;
    public int mReward;
    public PrestigeTask():base()
    {
    }

    public PrestigeTask(PrestigeTask origin):base(origin) {
        this.mDesc = origin.mDesc;
        this.mQuality = origin.mQuality;
        this.mTime = origin.mTime;
        this.mMoney = origin.mMoney;
        this.mType = origin.mType;
        this.mReward = origin.mReward;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mType = (PrestigeLevel.PrestigeType)ios.ReadByte();
        this.mQuality = ios.ReadByte();

        this.mTime = ios.ReadInt32();
        this.mReward = ios.ReadInt32();
        this.mMoney = ios.ReadInt32();

        this.mDesc = NetUtils.ReadUTF(ios);
    }



    public string Desc
    {
        get { return mDesc; }
    }

    public int Quality
    {
        get { return mQuality; }
    }

    public int Time
    {
        get { return mTime; }
    }

    public int Money
    {
        get { return mMoney; }
    }

    public PrestigeLevel.PrestigeType Type
    {
        get { return mType; }
    }

    public int Reward
    {
        get { return mReward; }
    }


}
