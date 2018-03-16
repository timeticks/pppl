using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class XyCoordRef
{
    public int m_X;
    public int m_Y;
    public XyCoordRef(int _x = 0, int _y = 0) { m_X = _x; m_Y = _y; }
    public XyCoordRef(XyCoord xy)
    {
        m_X = xy.m_X;
        m_Y = xy.m_Y;
    }

    /// <summary>
    /// 00 01 02
    /// 11 12 13
    /// </summary>
    public static ActionFourDir GetDir(XyCoordRef fromXy, XyCoordRef toXy)
    {
        if (fromXy.m_X > toXy.m_X)
            return ActionFourDir.Left;
        else if (fromXy.m_X < toXy.m_X)
            return ActionFourDir.Right;
        else if (fromXy.m_Y < toXy.m_Y)  //y更大是向下
            return ActionFourDir.Down;
        else if (fromXy.m_Y > toXy.m_Y)
            return ActionFourDir.Up;
        return ActionFourDir.Right;
    }

    /// <summary>
    /// 是否可以朝此方向移动，左上角为(0,0)点
    /// </summary>
    public bool CanMoveByDir(ActionFourDir dir, XyCoordRef maxSize)
    {
        switch (dir)
        {
            case ActionFourDir.Down:
                return m_Y < maxSize.m_Y;
            case ActionFourDir.Left:
                return m_X > 0;
            case ActionFourDir.Right:
                return m_X < maxSize.m_X;
            case ActionFourDir.Up:
                return m_Y > 0;
        }
        return false;
    }

    public string ToString()
    {
        return string.Format("({0},{1})", m_X, m_Y);
    }
}