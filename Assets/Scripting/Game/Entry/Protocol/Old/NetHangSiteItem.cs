using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class NetHangSiteItem : DescObject {

    public string IconString;
    public string SiteName;
    public int IncomeExp;
    public int IncomePoten;
    public int IncomeStone;
    public byte IncomeMaterial;//
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        IconString = ios.ReadString();
        // SectId      = ios.ReadInt32();
        SiteName = ios.ReadString();
        IncomeExp = ios.ReadInt32();
        IncomePoten = ios.ReadInt32();
        IncomeStone = ios.ReadInt32();
        IncomeMaterial = ios.ReadByte();
    }
}
