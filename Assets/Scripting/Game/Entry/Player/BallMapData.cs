using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMapData
{
    public int FireBallAmount;      //发射球总数
    public int Score;               //当前分数
    public int CurBall;             //当前发射台上的球

    public int MutilBallTimeDown;   //多球加入倒计时

    public double CenterAnchorRotate;               //中心点的旋转角度
    public Dictionary<int, int> BallDict = new Dictionary<int, int>();  //球的位置

}
