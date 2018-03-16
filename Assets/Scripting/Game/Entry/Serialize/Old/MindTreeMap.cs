using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IMindTreeMapFetcher
{
    MindTreeMap GetMindTreeMapByCopy(int idx);
}
public class MindTreeMap : DescObject
{
    private static IMindTreeMapFetcher mFetcher;
    public static IMindTreeMapFetcher MindTreeMapFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    private string mContent;

    public string Content
    {
        get { return mContent; }
    }

     public MindTreeMap():base()
    {
        
    }
    public MindTreeMap(MindTreeMap origin): base(origin)
    {
        this.mContent          = origin.mContent;
       
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mContent = NetUtils.ReadUTF(ios);
    }
}
