using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDropQualityRateFetcher
{
    DropQualityRate GetDropQualityRateCopy(int idx);
}
public class DropQualityRate : BaseObject
{
    private static IDropQualityRateFetcher mFetcher;
    public static IDropQualityRateFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public Eint[] qualityProb { get; private set; }

    public DropQualityRate()
    {
        
    }

    public DropQualityRate Clone()
    {
        return this.MemberwiseClone() as DropQualityRate;
    }



}
