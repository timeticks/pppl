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

    public Eint num;

    public static int GetGameConst(string key)
    {
        GameConst GameConst = GameConst.Fetcher.GetGameConstNoCopy(key);
        if (GameConst != null)
        {
            return GameConst.num;
        }
        return 0;
    }

    public GameConst():base()
    {
        
    }
    


}
