using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface ISpellObtainFetcher
{
    SpellObtain GetSpellObtainByCopy(int idx);
}
public class SpellObtain : DescObject
{
    private static ISpellObtainFetcher mFetcher;
    public static ISpellObtainFetcher SpellObtainFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private string mDesc = "";
    private int mSect;
    private int mNpcId;
    private int[] mSpell;         //第一层可以学的技能

    private int[] mLevel;         //人物等级需求
    private int[] mPreSpellId;    //前置功法
    
    public SpellObtain():base()
    {
    	
    }
    public SpellObtain(SpellObtain origin): base(origin)
    {   
        this.mDesc          = origin.mDesc;
        this.mSect = origin.mSect;
        this.mNpcId = origin.mNpcId;
        this.mSpell = origin.mSpell;
        this.mLevel = origin.mLevel;
        this.mPreSpellId = origin.mPreSpellId;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);

        mSect = ios.ReadByte();
        mNpcId = ios.ReadInt32();

        int length = ios.ReadByte();
        mSpell = new int[length];
        for (int i = 0; i < length; i++)
        {
            mSpell[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        mLevel = new int[length];
        for (int i = 0; i < length; i++)
        {
            mLevel[i] = ios.ReadInt16();
        }

        length = ios.ReadByte();
        mPreSpellId = new int[length];
        for (int i = 0; i < length; i++)
        {
            mPreSpellId[i] = ios.ReadInt32();
        }

        if (mPreSpellId.Length != mLevel.Length || mPreSpellId.Length != mSpell.Length)
        {
            TDebug.LogError("长度不等");
        }
    }




    public string Desc
    {
        get { return mDesc; }
    }

    public int Sect
    {
        get { return mSect; }
    }

    public int NpcId
    {
        get { return mNpcId; }
    }

    public int[] Level
    {
        get { return mLevel; }
    }

    public int[] PreSpellId
    {
        get { return mPreSpellId; }
    }

    public int[] Spell
    {
        get { return mSpell; }
    }

}
