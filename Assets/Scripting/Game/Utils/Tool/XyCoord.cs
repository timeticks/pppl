using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct XyCoord
{
    public int m_X;
    public int m_Y;
    public XyCoord(int _x = 0, int _y = 0) { m_X = _x; m_Y = _y; }
    public XyCoord(XyCoordRef xy)
    {
        m_X = xy.m_X;
        m_Y = xy.m_Y;
    }

    public static bool operator ==(XyCoord temp1, XyCoord temp2)
    {
        return temp1.m_X == temp2.m_X && temp1.m_Y == temp2.m_Y;
    }
    public static bool operator !=(XyCoord temp1, XyCoord temp2)
    {
        return temp1.m_X != temp2.m_X || temp1.m_Y != temp2.m_Y;
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}