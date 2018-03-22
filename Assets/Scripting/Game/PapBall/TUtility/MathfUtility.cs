using UnityEngine;
using System.Collections;

public class MathfUtility
{

    //public Vector2 testA;
    //public Vector2 testB;
    //void OnGUI()
    //{
    //    if(GUILayout.Button("得到阴影"))
    //    {
    //         Debug.Log( GetShadowDir(testA, testB));
    //    }
    //    if (GUILayout.Button("剩余的力的方向大小"))
    //    {
    //         Debug.Log(GetForceDir(testA, testB));
    //    }
    //}
    /// <summary>
    /// 求在addForceDir抵消了fixedDir方向上的所有力后，剩余的力
    /// </summary>
    public static Vector2 GetForceDir(Vector2 fixedDir, Vector2 addForceDir)
    {
        return addForceDir-GetShadowDir(fixedDir, addForceDir);
    }

    /// <summary>
    /// 求getShadowDir在baseDir上的投影
    /// </summary>
    public static Vector2 GetShadowDir(Vector2 baseDir, Vector2 getShadowDir)
    {
        float cos = Vector2.Dot(baseDir.normalized, getShadowDir.normalized);
        float shadowLength = cos * Mathf.Abs(getShadowDir.magnitude);
        return shadowLength * baseDir.normalized;
    }

    /// <summary>
    /// 得到valueNum更靠近values中的哪个值
    /// values为由小到大有序
    /// </summary>
    public static float GetNearValue(float[] values , float curValue)
    {
        int minIndex = 0;
        int maxIndex = values.Length-1;
        for (int i = 0; i < values.Length; i++)
        {
            minIndex = values[i] <= curValue ? i : minIndex;
        }
        if (minIndex.Equals(maxIndex))
            return values[maxIndex];
        maxIndex = minIndex + 1;

        float offsetMin = Mathf.Abs(curValue - values[minIndex]);
        float offsetMax = Mathf.Abs(curValue - values[maxIndex]);
        return offsetMin > offsetMax ? values[maxIndex] : values[minIndex];
    }

    /// <summary>
    /// 找到所有点中最近的点
    /// </summary>
    public static Vector2 GetNearVector2(Vector2[] values , Vector2 curValue)
    {
        int minIndex = 0;
        float minDis = float.MaxValue;
        for (int i = 0; i < values.Length; i++)
        {
            float curDis = Vector2.Distance(values[i], curValue);
            if (curDis < minDis)
            {
                minDis = curDis;
                minIndex = i;
            }
        }
        return values[minIndex];
    }

    /// <summary>
    /// 由一个点和方向得到直线公式y=kx+b中的b
    /// </summary>
    public static float GetLineB(Vector2 point , Vector2 dir)//得到直线公式中的b
    {
        //y = kx+b
        float k = GetLineK(dir);
        float b = point.y - k * point.x;
        return b;
    }
    public static float GetLineK(Vector2 dir)//得到直线公式中的b
    {
        return dir.x == 0 ? 1000000f : dir.y / dir.x;
    }

    /// <summary>
    /// 得到直线y=kx+b与point的距离
    /// </summary>
    public static float GetLineToPointDis(Vector2 point , float lineK , float lineB)
    {
        float dis = Mathf.Abs(lineK * point.x - point.y + lineB) / Mathf.Sqrt(lineK * lineK + 1);
        return dis;
    }
}
