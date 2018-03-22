using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TMathUtility
{

    /// <summary>
    /// 获得2d仿3d的弹道曲线
    /// </summary>
    public static Vector3[] GetCuverPoints(Vector3 startPos, Vector3 target, float angle, float speed, float angleY, out float[] highs)
    {
        if (startPos.Equals(target) || angleY.Equals(0))
        {
            highs = new float[] { 0 };
            return new Vector3[] { target };
        }

        float offsetY = startPos.y;
        Vector3 offset = new Vector3(startPos.x, 0, startPos.z);
        startPos = Vector3.zero;
        target = new Vector3(target.x, 0, target.z) - offset;
        Vector3 axis = (target - startPos).normalized;
        float maxDis = Vector3.Distance(startPos, target);
        Matrix4x4 m = GeiMatri(angle, axis);

        Vector3 velocity = new Vector3(0, speed * Mathf.Sin(Mathf.Deg2Rad * angleY), 0);

        float timeAmount = Vector3.Distance(startPos, target) / speed;
        velocity.x = (target.x - startPos.x) / timeAmount;
        velocity.z = (target.z - startPos.z) / timeAmount;
        float gravity = velocity.y * 2 / timeAmount;

        List<Vector3> pointsTrans = new List<Vector3>();
        List<float> regionHigh = new List<float>();
        Vector3 pointpre = startPos;
        Vector3 pointTrans = TransposeByAxis(pointpre, m);
        float dispre = Vector3.Distance(pointTrans, target);
        float disnow = dispre;
        pointsTrans.Add(pointTrans);
        regionHigh.Add(startPos.y);

        while (dispre >= disnow)
        {
            pointpre += new Vector3(velocity.x, velocity.y, velocity.z) * Time.fixedDeltaTime;
            velocity.y -= gravity * Time.fixedDeltaTime;
            pointTrans = TransposeByAxis(pointpre, m);
            pointsTrans.Add(pointTrans);
            regionHigh.Add(pointpre.y);
            dispre = disnow;
            disnow = Vector3.Distance(pointTrans, target);
        }
        int curLast = pointsTrans.Count-1;
        while (true && curLast > 1)//如果倒数第二个点超过终点，删掉
        {
            float dis1 = Vector3.Distance(pointsTrans[curLast], target);
            float dis2 = Vector3.Distance(pointsTrans[curLast-1], target);
            if (dis2 >= dis1) { break; }
            else { pointsTrans.RemoveAt(curLast); curLast = pointsTrans.Count - 1; }
        }
        if (pointsTrans.Count>0)//将最后一个点设为终点
        {
            pointsTrans[pointsTrans.Count - 1] = target;
        }
        
        highs = regionHigh.ToArray();
        Vector3[] points = pointsTrans.ToArray();
        for (int i = 0; i < points.Length; i++)
        {
            points[i] += offset;
            points[i].y = offsetY;
        }

        return points;
    }

    // point绕axis轴旋转
    public static Vector3 TransposeByAxis(Vector3 point, Matrix4x4 m)
    {
        Vector3 b = m.MultiplyPoint(point);
        return b;
    }

    public static Matrix4x4 GeiMatri(float angle, Vector3 axis)
    {
        Vector3 a = axis.normalized;
        float s = Mathf.Sin(angle * Mathf.Deg2Rad);
        float c = Mathf.Cos(angle * Mathf.Deg2Rad);
        Matrix4x4 m = new Matrix4x4();

        m[0, 0] = a.x * a.x + (1 - a.x * a.x) * c;
        m[0, 1] = a.x * a.y * (1 - c) - a.z * s;
        m[0, 2] = a.x * a.z * (1 - c) + a.y * s;
        m[0, 3] = 0;

        m[1, 0] = a.x * a.y * (1 - c) + a.z * s;
        m[1, 1] = a.y * a.y + (1 - a.y * a.y) * c;
        m[1, 2] = a.y * a.z * (1 - c) - a.x * s;
        m[1, 3] = 0;

        m[2, 0] = a.x * a.z * (1 - c) - a.y * s;
        m[2, 1] = a.y * a.z * (1 - c) + a.x * s;
        m[2, 2] = a.z * a.z + (1 - a.z * a.z) * c;
        m[2, 3] = 0;

        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = 0;
        m[3, 3] = 1;

        return m;
    }

    /// <summary>
    /// 检测是否在攻击范围内
    /// </summary>
    public static bool CheckTarget(ActionSixDir actionDir, XyCoordRef ship, XyCoordRef target)
    {
        if (actionDir == ActionSixDir.Max) return true;

        Vector2 targetOffset = new Vector2(target.m_X - ship.m_X, target.m_Y - ship.m_Y);

        int absy = (int)Mathf.Abs(targetOffset.y);
        int Num = absy / 2;
        int remainder = absy % 2;

        if (actionDir.Equals(ActionSixDir.Left) || actionDir.Equals(ActionSixDir.Right))
        {
            if (target.m_Y % 2 == 0)
            {
                if (targetOffset.x >= -Num - remainder && targetOffset.x <= Num) return true;
            }
            else
            {
                if (targetOffset.x >= -Num && targetOffset.x <= Num + remainder) return true;
            }
        }
        else if (actionDir.Equals(ActionSixDir.RightBack) || actionDir.Equals(ActionSixDir.LeftFront))
        {
            if (target.m_Y % 2 == 0)
            {
                if (targetOffset.x >= Num && targetOffset.y >= 0) return true;
                else if (-targetOffset.x >= Num +remainder  && targetOffset.y <= 0) return true;
            }
            else
            {
                if (targetOffset.x >= Num + remainder && targetOffset.y >= 0) return true;
                else if (-targetOffset.x >= Num && targetOffset.y <= 0) return true;
            }
        }
        else
        {
            if (target.m_Y % 2 == 0)
            {
                if (targetOffset.x >= Num && targetOffset.y <= 0) return true;
                else if (-targetOffset.x >= Num + remainder && targetOffset.y >= 0) return true;
            }
            else
            {
                if (targetOffset.x >= Num + remainder && targetOffset.y <= 0) return true;
                else if (-targetOffset.x >= Num && targetOffset.y >= 0) return true;
            }
        }
        return false;
    }

}




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

    public string ToString()
    {
        return string.Format("({0},{1})", m_X, m_Y);
    }
}