using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Reflection;
using Object = UnityEngine.Object;


[System.Serializable]
public class DescObject :System.Object
{

    [SerializeField] private int    mIdx;
    [SerializeField] private string mName;

    public int idx
    {
        get { return mIdx; }
        set { mIdx = value; }
    }


    public string name
    {
        get { return mName; }
        set { mName = value; }
    }


    public DescObject()
    {
    }

    public DescObject(DescObject origin)
    {
        mIdx    = origin.idx;
        mName   = origin.name;
    }


    public virtual void Serialize(BinaryReader ios)
    {
        mIdx    = ios.ReadInt32();
        mName   = NetUtils.ReadUTF(ios);
    }

   
}