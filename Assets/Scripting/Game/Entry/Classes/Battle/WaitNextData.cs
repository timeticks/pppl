using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用于记录一系列行为是否完结，只有当所有行为完结后，才能进行下一步
/// </summary>
public class WaitNextData
{
    public int Count { get { return WaitNextList.Count; } }
    public List<string> WaitNextList = new List<string>();
    public void Add(string str)
    {
        WaitNextList.Add(str);
    }
    public void RemoveAll(string str)  //移除所有
    {
        for (int i = 0; i < WaitNextList.Count; i++)
        {
            if (WaitNextList[i].Equals(str)) { WaitNextList.RemoveAt(i); i = Mathf.Max(0, i - 1); }
        }
    }
    public void Remove(string keyStr) //移除某一个keyStr
    {
        for (int i = 0; i < WaitNextList.Count; i++)
        {
            if (WaitNextList[i].Equals(keyStr)) { WaitNextList.RemoveAt(i); return; }
        }
        // TDebug.Log("NextWaitData 错误" + str +"    "+ m_NextWaitList.Count);
    }
}