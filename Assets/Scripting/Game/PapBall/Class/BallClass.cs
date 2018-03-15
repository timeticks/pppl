using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallNodeData
{
    public static System.Random rand = new System.Random();

    public XyCoordRef Pos;          //如果不是转盘上的球，此值为null
    public int Num=-1;
    public bool IsSearched=false;   //是否寻找过
    public bool IsDisable;          //是否禁用
    public bool IsLinkCenter;       //是否有与中心球连接
    public List<XyCoordRef> NearList;   //用于缓存邻近的坐标

    public BallBaseCtrl BallCtrl;

    public BallNodeData(int num)
    {
        Num = num;
        Pos = null;
    }
    public BallNodeData(XyCoordRef pos , int num)
    {
        Pos = pos;
        NearList = HexaMathf.GetInRange(1, Pos.m_X, Pos.m_Y);
        for (int i = NearList.Count-1; i >= 0; i--)
        {
            if (!BallGroupMgr.Instance.MapData.IsLegal(NearList[i].m_X, NearList[i].m_Y))
                NearList.RemoveAt(i);
        }
        Num = num;
    }


}
