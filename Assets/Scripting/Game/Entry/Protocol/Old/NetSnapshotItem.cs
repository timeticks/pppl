using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetSnapshotItem : DescObject
{
    public byte    Pos;
    public int     Num;


    public void ReadFrom(BinaryReader ios)
    {
        Pos = ios.ReadByte();
        idx = ios.ReadInt32();
        Num = ios.ReadInt32();
    }
}
