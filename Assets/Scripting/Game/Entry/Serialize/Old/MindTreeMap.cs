using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IMindTreeMapFetcher
{
    MindTreeMap GetMindTreeMapNoCopy(int idx);
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
    public string content;

     public MindTreeMap():base()
    {
        
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
    }
}
