using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public static class ClassExtensionUtils
{
    /// <summary>
    /// 添加或者相加
    /// </summary>
    public static void AddOrPlus<TKey, TVal>(this Dictionary<TKey, int> dict, TKey k, int v) 
    {
        if (dict.ContainsKey(k))
        {
            dict[k] += v;
        }
        else
        {
            dict.Add(k, v);
        }
    }

    public static Vector2 ToVector2XY(this Vector3 temp)
    {
        return new Vector2(temp.x, temp.y);
    }

    /// <summary>
    /// 万分比转换，100-0.01
    /// </summary>
    public static float ToFloat_10000(this int num)
    {
        return (float)num / 10000f;
    }

    public static float ToFloat_100(this int num)
    {
        return (float)num / 100f;
    }

    public static float ToFloat_10000(this Eint num)
    {
        //if (null == num) return 0;//无法判空
        return (float)num / 10000f;
    }

    public static float ToFloat_100(this Eint num)
    {
        //if (null == num) return 0;//无法判空
        return (float)num / 100f;
    }

    /// <summary>
    /// 将100ms转为0.1s
    /// </summary>
    public static float ToFloat_1000(this int num)
    {
        return (float)num / 1000f;
    }

    /// <summary>
    /// 传入万分比，得到此万分比对应的数值
    /// </summary>
    /// <returns></returns>
    public static int CaculatePctValue(this int val, int pct)
    {
        return (int) (val*(pct/10000f));
    }

    public static int[] ToIntArray(this Eint[] num)
    {
        int[] temp = new int[num.Length];
        for (int i = 0; i < num.Length; i++)
        {
            temp[i] = num[i];
        }
        return temp;
    }

    /// <summary>
    /// 万分比转换：0.01-100
    /// </summary>
    public static float ToInt_10000(this float num)
    {
        return (int)(num * 10000f);
    }

    /// <summary>
    /// 四舍五入转为Int..I5电脑--100w次，30毫秒
    /// </summary>
    public static int ToIntRound(this float num)
    {
        return Mathf.RoundToInt(num);
    }
    /// <summary>
    /// 毫秒转秒
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static int ToInt_1000(this long num)
    {
        return (int)(num / 1000);
    }

    /// <summary>
    /// 将0.1转为100
    /// </summary>
    public static int ToInt_100(this float num)
    {
        return Mathf.RoundToInt(num * 100f);
    }
    /// <summary>
    /// 将0.111转为11.1%，根据format保留小数
    /// </summary>
    public static string ToString_100(this float num, string format = "f1")
    {
        return (num * 100f).ToString(format);
    }

    /// <summary>
    /// 将0.1转为100
    /// </summary>
    public static int ToInt_1000(this float num)
    {
        return Mathf.RoundToInt(num * 1000f);
    }

    public static int ToInt(this string str)
    {
        int i = 0;
        int.TryParse(str, out i);
        return i;
    }
    public static int ToInt(this Estring str)
    {
        int i = 0;
        int.TryParse(str, out i);
        return i;
    }

    public static int TryGetInt(this string str, int defaultValue)
    {
        int i = defaultValue;
        if (int.TryParse(str, out i))
        {
            return i;
        }
        return defaultValue;
    }
    public static float TryGetFloat(this string str, float defaultValue)
    {
        float i = defaultValue;
        if (float.TryParse(str, out i))
        {
            return i;
        }
        return defaultValue;
    }

    //重置转义符
    public static string RemoveN(this string key)
    {
        return key.Replace(@"\n", "\n").Replace(@"\f", "\f").Replace(@"\u3000", "\u3000");
    }

    /// <summary>
    /// 枚举转为int
    /// </summary>
    public static int ToInt(this Enum enu)
    {
        return Convert.ToInt32(enu);
    }

    /// <summary>
    /// 获取枚举的描述属性，[EnumDesc("未激活")]
    /// </summary>
    public static string GetDesc(this Enum enu)
    {
        return EnumDesc.GetEnumDesc(enu);
    }

    public static bool GetBool(this int num)
    {
        return num != 0;
    }
    public static int GetInt(this bool val)
    {
        return val ? 1 : 0;
    }
    public static float ToFloat(this string str)
    {
        float i = 0;
        float.TryParse(str, out i);
        return i;
    }

    public static Color Switch16ToColor(this string colorString)//16进制转颜色
    {
        if (colorString.Length != 6)
            return Color.black;
        Color returnCol = new Color();
        returnCol.r = (float)Convert.ToInt32(colorString.Substring(0, 2), 16) / 256f;
        returnCol.g = (float)Convert.ToInt32(colorString.Substring(2, 2), 16) / 256f;
        returnCol.b = (float)Convert.ToInt32(colorString.Substring(4, 2), 16) / 256f;
        returnCol.a = 1;
        return returnCol;
    }


    public static bool IsSeriousBattle(this BattleType battleTy)//是否是决斗
    {
        return battleTy.ToString().Contains("Battle");
    }
    public static bool IsHand(this BattleType battleTy)
    {
        return (battleTy == BattleType.Enemy_Hand_Battle || battleTy == BattleType.Enemy_Hand_Fight ||
                battleTy == BattleType.My_Hand_Battle || battleTy == BattleType.My_Hand_Fight);
    }

    /// <summary>
    /// 返回对象所有属性的字符串
    /// </summary>
    public static string ToFieldsString<T>(this T t)
    {
        if (t == null) return "";
        if (t is string || t.GetType().IsValueType || t.GetType().IsEnum) return t.ToString();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Type type = t.GetType();
        if (t is ICollection)
        {
            ICollection list = (ICollection)t;
            foreach (var item in list) { sb.Append("\r\n   " + ToFieldsString(item)); }
            return sb.ToString().TrimEnd(new char[] { ',' });
        }

        FieldInfo[] fields = type.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++)
        {
            object o = fields[i].GetValue(t);
            if (o is ICollection)
            {
                ICollection list = (ICollection)o;
                foreach (var item in list) { sb.Append("\r\n   " + ToFieldsString(item)); }
            }
            else
            {
                sb.Append("[" + fields[i].Name + ":" + o + "], ");
            }
        }
        return sb.ToString().TrimEnd(new char[] { ',' });
    }
}

