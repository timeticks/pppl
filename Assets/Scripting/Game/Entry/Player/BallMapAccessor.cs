using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMapAccessor
{
    public Eint CurMapIdx=0;          //当前地图id，用于地图配置
    public Eint CurDiffiLevel=0;      //难度
    public Eint MapMaxSize=0;
    public Eint CurRound;             //当前回合数

    public Eint FireBallAmount=0;     //发射球总数
    public Eint DestoryBallAmount=0;  //销毁球的总数
    public Eint MutilBallAmount=0;    //随机加入球的总数

    public Eint Score=0;              //当前分数
    public List<int> NextBallList = new List<int>();           //已随机出来的后续发射球

    public double CenterAnchorRotate=0;               //中心点的旋转角度
    public Dictionary<string, int> BallDict = new Dictionary<string, int>();  //球的位置和信息

    public Eint MutilBallDown=0;                                    //当前多球加入倒计时
    public Eint LastMutilBallDown = 0;                              //上一次的多球倒计时
    public List<GoodsToDrop> goodsDropList = new List<GoodsToDrop>();   //此场的收获

    public Eint UseUniversalBallNum=0;  //使用万能球的个数
    public Eint UseNextBallNum=0;       //使用跳过球的个数
    public Eint UseFrozenNum = 0;       //使用冰冻的个数


    public int GetCurDifficultIndex()
    {
        BallMap map = BallMap.Fetcher.GetBallMapCopy(CurMapIdx);
        if (map == null) return 1;
        for (int i = 0; i < map.diffiUpScore.Length; i++)
        {
            if (Score >= map.diffiUpScore[i]) continue;
            return i;
        }
        return map.diffiUpScore.Length - 1;
    }

    private static System.Random mBallRand = new System.Random();
    public int GetNextRandBall()
    {
        BallMap map = BallMap.Fetcher.GetBallMapCopy(CurMapIdx, false);
        int max = Mathf.Min(map.diffiBallNum[GetCurDifficultIndex()], map.ballList.Length-1);
        return map.ballList[mBallRand.Next(0, max)];
    }

    //多球加入的数量
    public int GetMutilNum()
    {
        BallMap map = BallMap.Fetcher.GetBallMapCopy(CurMapIdx, false);
        return map.diffiMultiNum[GetCurDifficultIndex()];
    }

}
