using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 第0排，第一个为半格。
/// 即0排的完整第0个横坐标为0.5
/// 1排第0个横坐标为0
/// </summary>
public class HexaMathf 
{

    public static Vector2 Xy2WorldPos(XyCoord xy)
    {
        return Xy2WorldPos(xy.m_X, xy.m_Y);
    }


    /// <summary>
    /// XY坐标转世界坐标
    /// </summary>
    public static Vector2 Xy2WorldPos(int x, int y)
    {
        if (y % 2 == 0) return new Vector2(HexaMapData.Radius * (x + 1) * 2 + HexaMapData.StartPos.x, (-y - 1) * HexaMapData.HeightRatio + HexaMapData.StartPos.y);
        else return new Vector2(HexaMapData.Radius * (x * 2 + 1) + HexaMapData.StartPos.x, (-y - 1) * HexaMapData.HeightRatio + HexaMapData.StartPos.y);
    }


    public static List<XyCoordRef> GetOneLineNode(XyCoordRef start, XyCoordRef end)//得到两节点间，与此直线相交的节点
    {
        List<XyCoordRef> xyList = new List<XyCoordRef>();
        Dictionary<XyCoord, bool> dic = new Dictionary<XyCoord, bool>();
        if (start.m_Y - end.m_Y == 0)
        {
            for (int i = Mathf.Min(start.m_X, end.m_X) + 1; i < Mathf.Max(start.m_X, end.m_X); i++)
            {
                xyList.Add(new XyCoordRef(i, start.m_Y));
            }
        }
        else 
        {  
            //得到两点的世界坐标
            Vector3 pos1 = Xy2WorldPos(start.m_X, start.m_Y);
            Vector3 pos2 = Xy2WorldPos(end.m_X, end.m_Y);
            int offsetAmount = Mathf.Abs(start.m_X - end.m_X) + Mathf.Abs(start.m_Y - end.m_Y);
            for (int i = 0; i < offsetAmount; i++)//将直线均分，得到与直线相交的点
            {
                Vector3 pos = pos1 + (pos2 - pos1) * ((float)(i + 1) / (offsetAmount + 1));
                XyCoord xy = WorldPos2Xy(pos);
                if (!dic.ContainsKey(xy))
                {
                    dic.Add(xy, true);
                    xyList.Add(new XyCoordRef(xy));
                }
            }

        }
        return xyList;
    }

    /// <summary>
    /// 世界坐标转xy
    /// </summary>
    public static XyCoord WorldPos2Xy(Vector2 worldPos)
    {
        worldPos -= HexaMapData.StartPos;
        int x = 0, y = 0,flag=-1;
        y = Mathf.RoundToInt((Mathf.Abs(worldPos.y) - 0.5f) / HexaMapData.HeightRatio);
        
        if (y % 2 == 0)
        {
            flag = 1;
            x = Mathf.RoundToInt(worldPos.x / (HexaMapData.Radius * 2)) - 1;
        }
        else
        {
            x = Mathf.RoundToInt((worldPos.x + 0.5f) / (HexaMapData.Radius * 2)) - 1;
        }
        
        //选最靠近的Node
        List<XyCoord> coordList = new List<XyCoord>() {
            new XyCoord(x,y),new XyCoord(x,y-1),new XyCoord(x,y+1),
            new XyCoord(x+flag,y-1),new XyCoord(x+flag,y+1)
        };
        XyCoord nearest = new XyCoord(-1, -1);
        float minDis = float.MaxValue;
        for (int i = 0; i < coordList.Count; i++)
        {
            //if (coordList[i].m_X < 0 || coordList[i].m_Y < 0) continue;
            float dis = Vector2.Distance(Xy2WorldPos(coordList[i].m_X, coordList[i].m_Y), worldPos + HexaMapData.StartPos);
            if (minDis > dis)
            {
                minDis = dis;
                nearest = coordList[i];
            }
        }
        return nearest;
    }


    /// <summary>
    /// 获取坐标为x,y的，半径radius内的所有坐标。。没有排除越界的
    /// </summary>
    /// <returns></returns>
    public static List<XyCoordRef> GetInRange(int radius, int x, int y)
    {
        List<XyCoordRef> pqnList;

        if (y % 2 == 0) pqnList = OuShuRowInitPos(radius, x, y);
        else pqnList = JiShuRowInitPos(radius, x, y);

        //生成竖直位置
        for (int i = 2; i <= radius; i += 2)
        {
            XyCoordRef pqn1 = new XyCoordRef(x, y + i);
            pqnList.Add(pqn1);
            XyCoordRef pqn2 = new XyCoordRef(x, y - i);
            pqnList.Add(pqn2);
        }
        return pqnList;
    }

    /// <summary>
    /// 在偶数行展开
    /// </summary>
    static List<XyCoordRef> OuShuRowInitPos(int radius, int x, int y)
    {
        List<XyCoordRef> pqnList = new List<XyCoordRef>();

        for (int row = 0; row <= radius; row++)
        {
            int index = 0;
            if (row != 0 && row % 2 == 1)
            {
                index = 1;
            }
            for (int col = 1; col <= radius - Mathf.FloorToInt(row / 2f); col++)
            {
                XyCoordRef pqn1 = new XyCoordRef(x + col, y + row);
                pqnList.Add(pqn1);

                if (row != 0)
                {
                    XyCoordRef pqn2 = new XyCoordRef(x + col, y - row);
                    pqnList.Add(pqn2);

                    XyCoordRef pqn3 = new XyCoordRef(x - col + index, y + row);
                    pqnList.Add(pqn3);
                }

                XyCoordRef pqn4 = new XyCoordRef(x - col + index, y - row);
                pqnList.Add(pqn4);
            }
        }
        return pqnList;
    }

    /// <summary>
    /// 在奇数行展开
    /// </summary>
    static List<XyCoordRef> JiShuRowInitPos(int radius, int x, int y)
    {
        List<XyCoordRef> pqnList = new List<XyCoordRef>();

        for (int row = 0; row <= radius; row++)
        {
            int index = 0;
            if (row != 0 && row % 2 == 1)
            {
                index = 1;
            }
            for (int col = 1; col <= radius - Mathf.FloorToInt(row / 2f); col++)
            {
                XyCoordRef pqn1 = new XyCoordRef(x + col - index, y + row);
                pqnList.Add(pqn1);

                if (row != 0)
                {
                    XyCoordRef pqn2 = new XyCoordRef(x + col - index, y - row);
                    pqnList.Add(pqn2);

                    XyCoordRef pqn3 = new XyCoordRef(x - col, y + row);
                    pqnList.Add(pqn3);
                }

                XyCoordRef pqn4 = new XyCoordRef(x - col, y - row);
                pqnList.Add(pqn4);
            }
        }
        return pqnList;
    }




    /// <summary>
    /// 得到六边形网格中，两个格子的无障碍距离
    /// </summary>
    /// <param name="startXY">起点</param>
    /// <param name="endXY">终点</param>
    /// <returns>距离，同一点则为0</returns>
    public static int GetNodeDis(XyCoordRef startXY, XyCoordRef endXY)
    {
        int radius = 0;

        int offsetY = endXY.m_Y - startXY.m_Y;
        int offsetX = endXY.m_X - startXY.m_X;
        if (offsetY == 0) return Mathf.Abs(offsetX);
        if (offsetX == 0) return Mathf.Abs(offsetY);

        int zeroRangeX = 0;
        if (startXY.m_Y % 2 == endXY.m_Y % 2) //同为奇/偶数排时
        {
            zeroRangeX = Mathf.Abs(offsetY) + 1;
            zeroRangeX = Mathf.FloorToInt(zeroRangeX / 2f);
        }
        else
        {
            int flag = startXY.m_Y % 2 == 0 ? 1 : -1;
            zeroRangeX = Mathf.Abs(offsetY) - 1;
            zeroRangeX = zeroRangeX == 0 ? 0 : zeroRangeX / 2;
            zeroRangeX = offsetX * flag > 0 ? zeroRangeX + 1 : zeroRangeX; //同向的+1
        }
        radius = Mathf.Abs(offsetY) + Mathf.Max(0, Mathf.Abs(offsetX) - zeroRangeX);
        return radius;
    }



    //得到距离内的所有格子
    public static List<XyCoordRef> GetNodeByDis(XyCoordRef startNode, int dis, int mapWidth, int mapHeight)
    {
        List<XyCoordRef> nodeList = new List<XyCoordRef>();

        for (int i = -dis; i <= dis; i++)
        {
            for (int j = -dis; j <= dis; j++)
            {
                XyCoordRef curCoord = new XyCoordRef(startNode.m_X + i, startNode.m_Y + j);
                if (curCoord.m_X < 0 || curCoord.m_X >= mapWidth) continue;
                if (curCoord.m_Y < 0 || curCoord.m_Y >= mapHeight) continue;
                if (i == 0 && j == 0) continue;

                if (HexaMathf.GetNodeDis(startNode, curCoord) <= dis)
                {
                    nodeList.Add(curCoord);
                }
            }
        }
        return nodeList;
    }

    public static int GetNearOffsetByY(int y)   //得到六边形中，根据奇偶，得到上下邻边除0之外的值
    {
        return y % 2 == 0 ? 1 : -1;
    }


}
