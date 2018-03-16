using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface ISectFetcher
{
    Sect GetSectByCopy(Sect.SectType sectType);
    List<Sect> GetAllSect();
}
public class Sect : DescObject
{
    private static ISectFetcher mFetcher;
    public static ISectFetcher SectFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public enum SectType:byte
    {
        None,
        QingYun,
        TianShi,
        KunLun,
        WanDu,
        XueSha,
        YinYang,
        ZhongLi,  //中立宗门
        Max,
    }
   
    private SectType mType = SectType.None;
    private string mDesc = "";

    
    private int mLimitMap;
    private int mFreeMap;
    private string mIcon;
    private string mColor;
    public static string GetSectName(SectType sectTy)
    {
        if (sectTy == SectType.None) return "无";
        Sect sect = Sect.SectFetcher.GetSectByCopy(sectTy);
        if (sect != null)
        {
            return sect.name;
        }
        return "空";
    }

    public Sect():base()
    {
        
    }
    public Sect(Sect origin): base(origin)
    {
        this.mType = origin.mType;
        this.mDesc          = origin.mDesc;
        this.mLimitMap = origin.mLimitMap;
        this.mFreeMap = origin.mFreeMap;
        this.mIcon = origin.mIcon;
        this.mColor = origin.mColor;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        mType = (SectType)ios.ReadByte();
        mLimitMap = ios.ReadInt32();
        mFreeMap = ios.ReadInt32();

        this.mDesc = NetUtils.ReadUTF(ios);
        this.mIcon = NetUtils.ReadUTF(ios);
        this.mColor = NetUtils.ReadUTF(ios);
    }

    public SectType Type
    {
        get { return mType; }
    }

    public string Desc
    {
        get { return mDesc; }
    }

    public int LimitMap
    {
        get { return mLimitMap; }
    }

    public int FreeMap
    {
        get { return mFreeMap; }
    }
    public string Icon
    {
        get { return mIcon; }
    }
    public string Color
    {
        get { return mColor; }
    }
}
