using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassBaseUtils 
{
    public static readonly Vector3 NullVector2 = new Vector2(float.NaN, 0);
    public static readonly Vector3 NullVector3 = new Vector3(float.NaN, 0, 0);
    public static readonly float NullFloat = float.NaN;
    public static readonly int NullInt = int.MinValue;
    public static readonly Color NullColor = new Color(float.NaN, 0f, 0f);

    //是否有值
    public static bool HasValue(this int num)
    {
        return (num != NullInt);
    }
    //是否有值
    public static bool HasValue(this float num)
    {
        return !float.IsNaN(num);
    }
    public static bool HasValue(this Vector2 num)
    {
        return !float.IsNaN(num.x);
    }
    public static bool HasValue(this Vector3 num)
    {
        return !float.IsNaN(num.x);
    }
    public static bool HasValue(this Color num)
    {
        return !float.IsNaN(num.r);
    }
}
