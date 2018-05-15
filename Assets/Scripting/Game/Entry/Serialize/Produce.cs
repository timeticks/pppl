﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IProduceFetcher
{
    Produce GetProduceCopy(int idx);
}
public class Produce : BaseObject
{
    private static IProduceFetcher mFetcher;
    public static IProduceFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public LootItemType makeType;
    public int makeId;
    public Eint workerMake;

    public LootItemType[] needTypes;
    public int[] needIds;
    public int[] needNums;


}
