using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于超长数据的位运算
/// </summary>
public class BigNumer
{
    public int[] Value;

    public BigNumer(int[] val)
    {
        Value = val;
    }
    public bool IsTrue(int _index) //得到某一位是否是1
    {
        int count = _index/32;
        if (count >= 0 && count < Value.Length)
        {
            int mod = _index%32;
            int num = Value[count];
            return ((num & (1 << mod)) == num);
        }
        TDebug.LogError("超过长度" + _index);
        return false;
    }
}
