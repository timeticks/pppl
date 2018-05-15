using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBallFetcher
{
    Ball GetBallCopy(int idx,bool isCopy);
}
public class Ball : BaseObject 
{
    private static IBallFetcher mFetcher;

    public static IBallFetcher Fetcher
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

    public string icon;
    public string desc;
    public string color;

    public Ball Clone()
    {
        return this.MemberwiseClone() as Ball;
    }

}
