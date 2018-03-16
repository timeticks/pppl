using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public interface IStockFetcher
{
    Stock GetStockByCopy(int idx);
}
public class Stock : DescObject
{
    private static IStockFetcher mFetcher;
    public static IStockFetcher StockFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    private string mIcon = "";
    private string mDesc = "";
    public int mOwnNum = 0;
    public Stock(Stock origin)
        : base(origin)
    {

    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
    }

}
