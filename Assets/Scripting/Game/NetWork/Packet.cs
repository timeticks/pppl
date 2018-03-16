using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Packet : IPacket {

    private int mStatus;


    public abstract short   NetCode { get; }


    public abstract void ReadFrom(BinaryReader ios);

    public abstract void WriteTo(BinaryWriter ios);

    public int StatusCode
    {
        get { return mStatus; }
        set { mStatus = value; }
    }
}
