using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQualityTableFetcher
{
    QualityTable GetQualityTableCopy(int quality, bool isCopy = true);
}
public class QualityTable : BaseObject
{
    private static IQualityTableFetcher mFetcher;

    public static IQualityTableFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public int  dropQuality;
    public Eint mainCoeff = 0;
    public Eint subCoeff = 0;
    public Eint[] gradeProb = new Eint[0];
    public Eint[] grade = new Eint[0];
    public Eint[] typeProb = new Eint[0];

    public QualityTable()
    {
    }

    public QualityTable Clone()
    {
        return this.MemberwiseClone() as QualityTable;
    }
}
