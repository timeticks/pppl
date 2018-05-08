using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartnerFetcher
{
    Partner GetPartnerCopy(int idx, bool isCopy = true);
    Partner GetPartnerRandomCopy(PartnerData.Sex sex, PartnerData.CharacType characTy);
}
public class Partner : BaseObject
{
    private static IPartnerFetcher mFetcher;

    public static IPartnerFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public string desc;
    public PartnerData.Sex sex = PartnerData.Sex.Male ;
    public PartnerData.CharacType characType;

    public Partner()
    {
    }

    public Partner Clone()
    {
        return this.MemberwiseClone() as Partner;
    }

}
