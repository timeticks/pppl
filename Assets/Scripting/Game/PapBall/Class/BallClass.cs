using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallNodeData
{
    public static System.Random rand = new System.Random();

    public XyCoordRef Pos;          //如果不是转盘上的球，此值为null
    public int BallIdx=-1;
    public int SearchedByIdx;       //上一次被寻找过的ballIdx
    public bool IsAdded = false;    //是否已经添加到相同列表中
    public bool IsDisable;          //是否禁用
    public bool IsLinkCenter;       //是否有与中心球连接
    public List<XyCoordRef> NearList;   //用于缓存邻近的坐标

    public BallBaseCtrl BallCtrl;

    public BallNodeData(int ballIdx)
    {
        BallIdx = ballIdx;
        Pos = null;
    }
    public BallNodeData(XyCoordRef pos , int ballIdx , HexaMapData mapData)
    {
        Pos = pos;
        NearList = HexaMathf.GetInRange(1, Pos.m_X, Pos.m_Y);
        for (int i = NearList.Count-1; i >= 0; i--)
        {
            if (!mapData.IsLegal(NearList[i].m_X, NearList[i].m_Y))
                NearList.RemoveAt(i);
        }
        BallIdx = ballIdx;
    }


}
