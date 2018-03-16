using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IPVEDialogueFetcher
{
    PVEDialogue GetPVEDialogueByCopy(int idx);
    List<PVEDialogue> GetPVEDialogueByTypeByCopy(PVELoggerType ty);
}
public class PVEDialogue :DescObject
{
    private static IPVEDialogueFetcher mFetcher;

    public static IPVEDialogueFetcher PVEDialogueFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    private PVELoggerType mType;
    private string          mDescribe = "";
    private string          mSequence = "";
    private ArrayList       mBookmarks = new ArrayList();


    public PVEDialogue() : base() { }

    public PVEDialogue(PVEDialogue origin)
        : base(origin)
    {
        this.mType             = origin.mType;
        this.mDescribe          = origin.mDescribe;
        this.mBookmarks         = origin.mBookmarks;
        this.mSequence         = origin.mSequence;
    }


    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        this.mType            = (PVELoggerType)ios.ReadByte();
        this.mDescribe          = NetUtils.ReadUTF(ios);
        this.mSequence          = NetUtils.ReadUTF(ios);
    }


    public PVELoggerType Type
    {
        get { return mType; }
    }

    public string Describe
    {
        get { return mDescribe; }
    }

    public string Sequence
    {
        get { return mSequence; }
    }

    public ArrayList Bookmarks
    {
        get { return mBookmarks; }
    }
}


public enum PVELoggerType:byte
{
    None,
    RoundStart,
    DoAttackSpell,
    DoAttackSubSpell,
    DoHealSpell,
    DoHealSubSpell,
    DoStateSpell,
    DoStateSubSpell,
    DoSignstSpell,
    DoSignstSubSpell,
    DoLifeRemoval,
    DoLifeRemovalSub,
    DoIdle,
    HotDoing,
    DotDoing,
    DmgCommon,
    DmgMiss,
    DmgCrit,
    DmgCritBlock,
    DmgBlock,
    DmgBlockDead,
    EndFail,
    EndDead,
}

public static class PVEDialogueExtension
{
    public static bool IsMainSpell(this PVELoggerType ty)
    {
        switch (ty)
        {
            case PVELoggerType.DoAttackSpell:
            case PVELoggerType.DoHealSpell:
            case PVELoggerType.DoStateSpell:
            case PVELoggerType.DoSignstSpell:
            case PVELoggerType.DoLifeRemoval:
            case PVELoggerType.DoIdle:
                return true;
        }
        return false;
    }

}