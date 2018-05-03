using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBallMapFetcher
{
    BallMap GetBallMapCopy(int idx,bool isCopy=true);
    List<BallMap> GetAllBallMapCopy(BallMap.MapType ty ,bool isCopy = true);
}
public class BallMap : BaseObject 
{
    private static IBallMapFetcher mFetcher;

    public static IBallMapFetcher Fetcher
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
        None,
        Test,
        Normal,
        DIY,
    }

    public MapType mapType;
    public string icon;
    public string desc;
    public string music;
    public int level;
    public int diffiStart;  //起始难度
    public int diffiCoeff;  //难度系数
    public int ballRadius;
    public int[] pos;
    public Eint[] ballList;    //此地图的小球类型列表
    public Eint[] scoreLoot;
    public Eint[] endLoot;

    public BallMap Clone()
    {
        return this.MemberwiseClone() as BallMap;
    }

}
