using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface IBookSpellFetcher
{
    BookSpell GetBookSpellByCopy(int idx);
}
public class BookSpell : DescObject {

    private static IBookSpellFetcher mFetcher;
    public static IBookSpellFetcher BookSpellFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private string mDesc;
    private AttrType[] mBaseAttType;
    private int[] mBaseAttVal;

    public BookSpell() : base() { }

    public BookSpell(BookSpell origin)
        : base(origin)
    {
        this.mDesc = origin.mDesc;
        this.mBaseAttType = origin.mBaseAttType;
        this.mBaseAttVal = origin.mBaseAttVal;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);

        int length = ios.ReadByte();
        this.mBaseAttType = new AttrType[length];
        for (int i = 0; i < length; i++)
        {
            this.mBaseAttType[i] = (AttrType)ios.ReadByte();
        }

        length = ios.ReadByte();
        this.mBaseAttVal = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mBaseAttVal[i] = ios.ReadInt16();
        }
        this.mDesc = NetUtils.ReadUTF(ios);
    }

    public AttrType[] BaseAttType
    {
        get { return mBaseAttType; }
    }
    public string Desc
    {
        get { return mDesc; }
    }
    public int[] BaseAttVal
    {
        get { return mBaseAttVal; }
    }
}
