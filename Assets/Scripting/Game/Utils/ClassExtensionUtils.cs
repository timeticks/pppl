using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public static class ClassExtensionUtils
{
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

    public static bool IsTrueInLayer(this long num, int layer)
    {
        if (layer < 0) return false;
        long finish = ((long)1) << layer;
        return (num & finish) == finish;
    }
    /// <summary>
    /// 二进制中，第layer位，是否为1
    /// layer大于0 且 layer小于32
    /// </summary>
    public static bool IsTrueInLayer(this int num, int layer)
    {
        if (layer < 0) return false;
        int finish = 1 << layer;
        return (num & finish) == finish;
    }

    /// <summary>
    /// 将100ms转为0.1s
    /// </summary>
    public static float ToFloat_1000(this int num)
    {
        return (float)num / 1000f;
    }

    /// <summary>
    /// 万分比转换：0.01-100
    /// </summary>
    public static float ToInt_10000(this float num)
    {
        return (int)(num * 10000f);
    }
    /// <summary>
    /// 四舍五入
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static int RoundToInt(this float num)
    {
        if (num >=0)
        {
            return (int)(num + 0.5f);
        }
        else
        {
            return Mathf.FloorToInt(num+0.5f);
        }
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
    public static float ToFloat(this string str)
    {
        float i = 0;
        float.TryParse(str, out i);
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

    /// <summary>
    /// 是否为真，pct=万分比
    /// </summary>
    public static bool IsTrue(this System.Random rand , int pct)
    {
        return rand.Next(0, 10000) < pct;
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
    //public static string GetDesc(this Enum enu)
    //{
    //    return EnumDesc.GetEnumDesc(enu);
    //}

    public static bool GetBool(this int num)
    {
        return num != 0;
    }
    public static int GetInt(this bool val)
    {
        return val ? 1 : 0;
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
    /// <summary>
    /// 返回对象所有属性的字符串
    /// </summary>
    public static string ToFieldsString<T>(this T t)
    {
        if (t == null) return "";
        if (t is string || t.GetType().IsValueType || t.GetType().IsEnum) return t.ToString();

        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
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
                sb.Append(string.Format("[{0}:{1}], ", fields[i].Name, o.ToString()));
            }
        }
        return sb.ToString().TrimEnd(new char[] { ',' });
    }



    static int a(int x) //对数字进行混淆
    {
        int temp = 0;
        if (x >= 0)
        {
            temp = ((x - 0x7FFFFFFF) ^ -12423556) - 435678;
            return temp ^ 71681687;  //正数
        }
        else
        {
            temp = ((x - (int)(-2147483648)) ^ -21456476) + 432433;
            return temp ^ 12344234;  //负数
        }
    }
    static int aa(int x) //对数字进行逆混淆
    {
        if (x >= 0)
        {
            x = x ^ 71681687;
            x = (x + 435678) ^ -12423556;
            return x += 0x7FFFFFFF;
        }
        else
        {
            x = x ^ 12344234;
            x = (x - 432433) ^ -21456476;
            return x += (int)(-2147483648);
        }
    }
}

