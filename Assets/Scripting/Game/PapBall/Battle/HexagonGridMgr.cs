using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 六边形
/// </summary>
public class HexagonGridMgr 
{
    public static readonly float HorizontalRatio = 1;//1.3660254f;
    public static readonly float VerticalRatio = 0.8660254f * 2;

    public static HexagonGridMgr CurHexagon = new HexagonGridMgr(61);

    public readonly float m_HexagonRadius;
    private  float m_horizontalEdge;
    private  float m_verticalEdge;
    private  List<Vector2> m_hexagonPoints;

    public HexagonGridMgr(float radius)
    {
        m_HexagonRadius = radius;
        m_horizontalEdge = HorizontalRatio * m_HexagonRadius;
        m_verticalEdge = m_HexagonRadius * VerticalRatio;

        m_hexagonPoints = new List<Vector2>(){new Vector2(-2*m_HexagonRadius,0) , new Vector2(-m_HexagonRadius,VerticalRatio*m_HexagonRadius) , 
            new Vector2(m_HexagonRadius,VerticalRatio*m_HexagonRadius) ,new Vector2(2*m_HexagonRadius,0) , 
            new Vector2(m_HexagonRadius,-VerticalRatio*m_HexagonRadius) , new Vector2(-m_HexagonRadius,-VerticalRatio*m_HexagonRadius)};
    }

    public Vector2 GetHexagonPos(Vector2 oldPos)//得到在六边形上的位置
    {
        m_horizontalEdge = HorizontalRatio * m_HexagonRadius;
        m_verticalEdge = m_HexagonRadius * VerticalRatio;
        int yNum = Mathf.RoundToInt(oldPos.y / m_verticalEdge);
        float yPos = yNum * m_verticalEdge;

        float xPos = 0;
        if (yNum % 2 == 0)
        {
            m_horizontalEdge = m_horizontalEdge * 2;
            xPos = Mathf.RoundToInt(oldPos.x / m_horizontalEdge) * m_horizontalEdge;
        }

        if (Mathf.Abs(yNum % 2) == 1)
        {
            float xNumFloat = oldPos.x / m_horizontalEdge;
            if (Mathf.Abs(xNumFloat) <= 1)
            {
                xPos = (oldPos.x > 0 ? 1 : -1) * m_horizontalEdge;
            }
            else
            {
                float xPosOffset = (oldPos.x>0 ? 1:-1) * m_horizontalEdge;
                float xOldPos = oldPos.x - xPosOffset;
                m_horizontalEdge = m_horizontalEdge * 2;
                xPos = Mathf.RoundToInt(xOldPos / m_horizontalEdge) * m_horizontalEdge;
                xPos +=xPosOffset;
            }
        }
        
        Vector2 newPos = new Vector2(xPos, yPos);
        return newPos;
    }

    public Vector2 GetPos(Vector2 offsetPos)//得到相对位置
    {
        offsetPos = MathfUtility.GetNearVector2(m_hexagonPoints.ToArray(), offsetPos);
        HexagonPosType posType = GetHexagonPosType(Vector2.zero, offsetPos);
        Debug.Log(posType);
        switch (posType)
        {
            case HexagonPosType.Left:
                return new Vector2(-m_HexagonRadius * 2, 0);
            case HexagonPosType.Right:
                return new Vector2(m_HexagonRadius * 2, 0);
            case HexagonPosType.LeftUp:
                return new Vector2(-m_HexagonRadius, m_verticalEdge);
            case HexagonPosType.LeftDown:
                return new Vector2(-m_HexagonRadius, -m_verticalEdge);
            case HexagonPosType.RightUp:
                return new Vector2(m_HexagonRadius, m_verticalEdge);
            case HexagonPosType.RightDown:
                return new Vector2(m_HexagonRadius, -m_verticalEdge);
        }
        return Vector2.zero;
    }


    public HexagonPosType GetHexagonPosType(Vector2 baseCell, Vector2 newCell)//newCell在baseCell的哪个位置方位
    {
        float offsetY = newCell.y - baseCell.y;
        float offsetX = newCell.x - baseCell.x;
        if (offsetY > m_HexagonRadius*0.4f)
        {
            return offsetX > 0 ? HexagonPosType.RightUp : HexagonPosType.LeftUp;
        }
        else if (offsetY < -m_HexagonRadius * 0.4f)
        {
            return offsetX > 0 ? HexagonPosType.RightDown : HexagonPosType.LeftDown;
        }
        else
        {
            return offsetX > 0 ? HexagonPosType.Right : HexagonPosType.Left;
        }
    }

    public bool CheckIsNear(Vector2 pos1 , Vector2 pos2)//检查是否是相邻
    { 
        float dis = Vector2.Distance(pos1 , pos2);
        if (Mathf.Abs(dis - m_HexagonRadius * 2) < 3)
        {
            return true;
        }
        return false;
    }

    public HexagonPosType GetInversePosType(HexagonPosType posType)//得到与posType相反的位置
    {
        switch (posType)
        {
            case HexagonPosType.Left:
                return HexagonPosType.Right;
            case HexagonPosType.Right:
                return HexagonPosType.Left;
            case HexagonPosType.LeftUp:
                return HexagonPosType.RightDown;
            case HexagonPosType.RightDown:
                return HexagonPosType.LeftUp;
            case HexagonPosType.LeftDown:
                return HexagonPosType.RightUp;
            case HexagonPosType.RightUp:
                return HexagonPosType.LeftDown;
        }
        return HexagonPosType.EndCount;
    }
}

public enum HexagonPosType
{
    //   0  1
    // 5      2
    //   4  3 
    LeftUp = 0,
    RightUp = 1,
    Right = 2,
    RightDown = 3,
    LeftDown = 4,
    Left = 5,
    EndCount = 6
}