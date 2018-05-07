using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartnerDialogueFetcher
{
    PartnerDialogue GetPartnerDialogueCopy(PartnerData partner ,int dayHour, bool isCopy = true);
}
public class PartnerDialogue : BaseObject
{
    private static IPartnerDialogueFetcher mFetcher;

    public static IPartnerDialogueFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public string ch;
    public string eg;
    public int[] timeRange;
    public int[] intimacyRange;
    public int[] sexRange;
    public int[] characRange;
    public int[] memoryRange;

    public PartnerDialogue()
    {
    }

    public PartnerDialogue Clone()
    {
        return this.MemberwiseClone() as PartnerDialogue;
    }

    public static string GetGroupKey(int sexTy , int characTy , int intimacy)
    {
        return (sexTy*10000 + characTy*100 + intimacy).ToString();
    }

    public static string GetPartnerDialogueStr(PartnerData partner)
    {
        return "";
    }
}
