using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


/// <summary>
/// 用于保存枚举的描述
/// </summary>
public class EnumDesc : Attribute
{
    public EnumDesc(string desc)
    {
        m_Desc = desc;
    }
    public string m_Desc { get; set; }
    public static string GetEnumDesc(Enum enu)
    {
        var ty = enu.GetType();
        FieldInfo fd = ty.GetField(enu.ToString()); //此枚举的属性
        if (fd == null)
            return string.Empty;
        EnumDesc[] atts = fd.GetCustomAttributes(typeof(EnumDesc), false) as EnumDesc[];
        string name = string.Empty;
        for (int i = 0; i < atts.Length; i++)
        {
            name = atts[i].m_Desc;
        }
        return name;
    }
}
