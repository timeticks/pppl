using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Mail : DescObject
{


    public long LifeTime;
    public string Sender;  //发件人
    public string Context; //正文
    public bool IsOpened;
    public GoodsToDrop[] GoodsList = new GoodsToDrop[0];
    public int GoodsListLength = 0;

    public Mail(): base(){  }

    public Mail(Mail origin)
        : base(origin)
    {
        this.LifeTime = origin.LifeTime;
        this.Sender = origin.Sender;
        this.Context = origin.Context;
        this.IsOpened = origin.IsOpened;
        this.GoodsList = origin.GoodsList;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        IsOpened = ios.ReadBoolean();
        GoodsListLength = ios.ReadByte();
        LifeTime = ios.ReadInt64();
    }

    public void SerializeDetail(BinaryReader ios)
    {
        base.Serialize(ios);
        LifeTime = ios.ReadInt64();
        Context = NetUtils.ReadUTF(ios);
        GoodsList = GoodsToDrop.SerializeList(ios);
    }
}
