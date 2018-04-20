using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMapAccessor
{
    public Eint CurMapIdx;          //当前地图id，用于地图配置
    public Eint CurDiffiLevel;      //难度
    public Eint MapMaxSize;         

    public Eint FireBallAmount;     //发射球总数
    public Eint DestoryBallAmount;  //销毁球的总数
    public Eint MutilBallAmount;    //随机加入球的总数

    public Eint Score;              //当前分数
    public int CurBall;             //当前发射台上的球
    public List<int> NextBallList = new List<int>();           //已随机出来的后续发射球

    public Eint CenterAnchorRotate;               //中心点的旋转角度
    public Dictionary<int, int> BallDict = new Dictionary<int, int>();  //球的位置和信息

    public Eint MutilBallTimeDown;                                 //多球加入倒计时

    public int GetCurDifficult()
    {
        return 1;
    }
}
