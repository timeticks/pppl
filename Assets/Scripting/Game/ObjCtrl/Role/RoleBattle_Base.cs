using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoleBattle_Base : MonoBehaviour   //角色控制器
{
    //[HideInInspector]
    //public RoleActionBase m_Action;
    [HideInInspector]
    public Transform MyRoleObj;
    public int RoleUid;
    public PVEHero MyData;
    public bool IsDead = false;

    protected void Init(PVEHero roleData, int roleUid)
    {
        MyData = roleData;
        RoleUid = roleUid;
        IsDead = false;
    }

    public virtual void DoAnimation(string actionStr, Vector3? targetPos = null, System.Action doOver = null)
    {
        RoleAnimationData actData = new RoleAnimationData(actionStr);
    }


    public virtual void GetDamage(int atkerUid , DmgInfo dmgNum)
    {
    }

    public virtual void Dead()
    {
    }

    public virtual void CheckBackAtk(string atkerUid, int skillId , bool isForceTrue = false)//反击
    {
    }
}

public class RoleAnimationData
{
    public RoleAnimationType ActType;
    public float  KeepAmount;
    public float  CurKeep;
    public object ValueObj;
    public RoleAnimationData(string str)//action(color,keep0.3,value200)
    {
        str = str.Substring(str.IndexOf('[')+1);
        str = str.Replace("]", "");
        string[] paramArray = str.Split(',');
        if (paramArray.Length < 1) return;
        ActType = (RoleAnimationType)Enum.Parse(typeof(RoleAnimationType), paramArray[0]);
        for (int i = 1; i < paramArray.Length; i++)
        {
            if (paramArray[i].Contains("value"))
            {
                try
                {
                    switch (ActType)
                    {
                        case RoleAnimationType.atk:
                        case RoleAnimationType.scale:
                            ValueObj = float.Parse(paramArray[i].Replace("value", ""));
                            break;
                        case RoleAnimationType.color:
                            ValueObj = TUtility.Switch16ToColor(paramArray[i].Replace("value", ""));
                            break;

                    }
                }
                catch (Exception) { TDebug.LogError("参数错误：  " + paramArray[i]); }
            }
            else if (paramArray[i].Contains("keep"))
            {
                try { KeepAmount = float.Parse(paramArray[i].Replace("keep", "")); }
                catch (Exception) { TDebug.LogError("参数错误：  " + paramArray[i]); }
            }
        }
    }}
public enum RoleAnimationType
{
    atk,   //ValueStr为移动总量
    color,     //ValueStr为颜色字符串  FF0000
    scale      //ValueStr为比例   2代表放大两倍
}