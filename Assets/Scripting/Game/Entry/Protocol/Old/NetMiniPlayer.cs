using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetMiniPlayer : DescObject
{
    public short    Level;
    public long     BirthSecond;  //年龄，用角色创建距今的秒数。客户端根据秒数转成年龄
    public int      SectId;       //宗门


    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        SectId      = ios.ReadByte();
        Level       = ios.ReadInt16();
        BirthSecond = ios.ReadInt64();
    }

}
