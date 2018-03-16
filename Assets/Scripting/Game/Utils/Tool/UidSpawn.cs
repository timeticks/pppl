using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Uid生成器
/// </summary>
public class UidSpawn
{
    public static int UID_AI = 500000000;
    public static int m_curId;
    public enum UidType : byte
    {
        User = 0,
        Max = 1
    }
    public static int GetUserUid()
    { return GetUid(); }
    public static int GetRoleUid()
    {
        return GetUid();
    }
    public static int GetUid()
    {
        return m_curId++;
    }
    public static bool IsAI(int uid)
    {
        return uid >= UID_AI;
    }

}
