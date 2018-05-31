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
    public int unlockRecall;//解锁需要记忆碎片数
    public int diffiStart;  //起始难度
    public int diffiCoeff;  //难度系数
    public int ballRadius;
    public int[] pos;
    public Eint[] ballList;    //此地图的小球类型列表
    public Eint[] scoreLoot;
    public Eint[] endLoot;

    public Eint[] diffiUpScore;  //分数
    public Eint[] diffiBallNum;  //难度与球种类数量
    public Eint[] diffiMultiNum;  //难度与多球加入数量
    public Eint[] multiTimeDown;  //倒计时
    public int startBallNum;        //初始时的星球圈数

    public BallMap Clone()
    {
        return this.MemberwiseClone() as BallMap;
    }

    public bool CheckLegal()
    {
        if (diffiUpScore.Length != diffiBallNum.Length || diffiUpScore.Length != diffiMultiNum.Length)
        {
            Debug.LogError("错误");
        }
        return true;
    }

}
