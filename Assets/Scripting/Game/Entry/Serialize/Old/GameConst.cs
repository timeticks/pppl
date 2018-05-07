using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public interface IGameConstFetcher
{
    GameConst GetGameConstNoCopy(string key);
}
public class GameConst : BaseObject
{
    private static IGameConstFetcher mFetcher;
    public static IGameConstFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public Estring num;

    public static int GetGameConst(string key)
    {
        GameConst gameConst = GameConst.Fetcher.GetGameConstNoCopy(key);
        if (gameConst != null)
        {
            return int.Parse((string)gameConst.num);
        }
        return 0;
    }
    public static int[] GetGameConstArray(string key)
    {
        GameConst gameConst = GameConst.Fetcher.GetGameConstNoCopy(key);
        if (gameConst != null)
        {
            return TUtility.SplitToIntArray(gameConst.num);
        }
        return new int[0];
    }

    public GameConst():base()
    {
        
    }
    


}
