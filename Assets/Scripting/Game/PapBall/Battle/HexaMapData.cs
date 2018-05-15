using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 地图的第0排，第一个为半格。
/// 即0排的完整第0个横坐标为0.5
/// 1排第0个横坐标为0
/// </summary>
public class HexaMapData
{
    public static float DefaultRadius = 36f;

    public static Vector2 StartPos;
    public static float Radius = 48f;
    public static float HeightRatio = Radius*1.732f;       //根号3

    public int Width, Height;

    public XyCoordRef CenterXy;
    private BallNodeData[][] Balls;

    public HexaMapData(int cols, int rows , int radius)
    {
        Radius = radius;
        HeightRatio = radius * 1.732f;
        Width = cols;
        Height = rows;
        CenterXy = new XyCoordRef(Width / 2, Height / 2);
        StartPos = new Vector2(-Radius * (Width + ((CenterXy.m_Y % 2 == 0) ? 1 : 0)), -HeightRatio * CenterXy.m_Y);

        Balls = new BallNodeData[Width][];
        for (int i = 0; i < Width; i++)
        {
            Balls[i] = new BallNodeData[Height];
        }

    }

    public float BallScaleRatio()
    {
        return Radius/DefaultRadius;
    }

    public BallNodeData GetNode(int x, int y)
    {
        if ((x < 0 || x >= Width) || (y < 0 || y >= Height))
        {
            TDebug.LogErrorFormat("越界:x:{0}|y:{1}", x, y);
            return null;
        }
        return Balls[x][y];
    }
    public BallNodeData GetNode(XyCoordRef pos)
    {
        return GetNode(pos.m_X, pos.m_Y);
    }

    public void SetNode(int x , int y, BallNodeData data)
    {
        if ((x < 0 || x >= Width) || (y < 0 || y > Height))
        {
            TDebug.LogErrorFormat("越界:x:{0}|y:{1}", x, y);
            return;
        }
        Balls[x][y] = data;
    }

    public void ResetNodeSearch(bool resetSearch , bool resetAdd)
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            for (int j = 0; j < Balls[i].Length; j++)
            {
                if (Balls[i][j] != null)
                {
                    if (resetSearch) Balls[i][j].SearchedByIdx = 0;
                    if (resetAdd) Balls[i][j].IsAdded = false;
                }
            }
        }
    }
    public void ResetNodeLinkCenter()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            for (int j = 0; j < Balls[i].Length; j++)
            {
                if (Balls[i][j] != null)
                    Balls[i][j].IsLinkCenter = false;
            }
        }
    }

    //是否坐标越界
    public bool IsLegal(int x, int y)
    {
        if ((x < 0 || x >= Width) || (y < 0 || y >= Height))
            return false;
        return true;
    }

    //通过xy坐标，得到世界位置
    public Vector2 GetNodeLocalPos(int x,int y)
    {
        float posX = ((y%2 == 0 ? 1f : 0) + x*2 + 1)*Radius;
        float posY = y * HeightRatio;
        return new Vector2(posX, posY) + StartPos;
    }

    //获取离此坐标最近的一个xy坐标
    public XyCoordRef GetNearestXy(Vector2 localPos , bool needNull)
    {
        float nearestDis = 100000f;
        XyCoordRef nearestXy = null;
        for (int i = 0; i < Balls.Length; i++)
        {
            for (int j = 0; j < Balls[i].Length; j++)
            {
                if (i == CenterXy.m_X && j == CenterXy.m_Y) continue;
                if (needNull && Balls[i][j]!=null)
                {
                    continue;
                }
                float dis = Vector2.Distance(GetNodeLocalPos(i, j), localPos);
                if (nearestDis > dis)
                {
                    nearestDis = dis;
                    nearestXy = new XyCoordRef(i, j);
                    //TDebug.Log(string.Format("dis:{0}|xy:{1}", nearestDis, nearestXy.ToString()));
                }
            }
        }
        return nearestXy;
    }


    //public HexaNodeData GetNode(int x, int y)
    //{
    //    if ((x < 0 || x >= m_Width) || (y < 0 || y > m_Height))
    //        return null;
    //    int length = x + m_Width * y;
    //    if (length >= 0 && length < m_NodeList.Count)
    //        return m_NodeList[length];
    //    else
    //        return null;
    //}
    public int GetNodeIndex(int x, int y)
    {
        return x + Width * y;
    }
}