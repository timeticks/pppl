using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IRechargeFetcher
{
    List<Recharge> GetRechargeCopyAll();
    Recharge GetRechargeCopy(int idx, bool isCopy);
}
public class Recharge : BaseObject 
{
    private static IRechargeFetcher mFetcher;

    public static IRechargeFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public enum MapType
    {
        Normal,
    }

    public int channel;
    public int price;
    public string desc;
    public string productId;
    public int type;

    public int diamond;
    public int diamondAdd;
    public int diamondDouble;
    public int order;
    public int firstLoot;
    public int loot;
    public int limit;   //可购买数量

    public Recharge Clone()
    {
        return this.MemberwiseClone() as Recharge;
    }

}
