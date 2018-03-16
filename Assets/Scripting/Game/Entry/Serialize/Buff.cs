using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffFetcher
{
    Buff GetBuffCopy(int idx);
}
public class Buff :BaseObject
{
    private static IBuffFetcher mFetcher;
    public static IBuffFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public enum BuffType
    {
        None,
        MonsterBuff,
        Spec,
        StateBuff,  //增减益buff
        DotBuff,    //持续伤害buff
    }
    public enum SpecBuffType
    {
        None,
        Dizzy,
        Frozen, 
        Bind,   //缠绕
        Poison,
        Fire,   //灼烧
    }

    public enum BuffMode
    {
        Attr,   //属性buff
        Spec,   //特殊状态buff
    }

    public string icon ;
    public BuffType type;
    public Eint bindSkill;
    public Eint duration; 
    public Eint maxAdd;
    public bool isUp;
    public Eint dropUp;
    public BuffMode[] effectMode;
    public AttrType[] effectType;   //如果是特殊状态buff，此类型是特殊buff类型
    public Eint[] effectNum;


    public Eint curAdd=0;
    public Eint curDuration=0;
    public Buff()
    {
    }
    public Buff Clone()
    {
        return this.MemberwiseClone() as Buff;
    }

    public void CheckLegal()
    {
        if (effectMode.Length == 0)  //如果没有填任何值，则全是0
        {
            effectMode = new BuffMode[effectNum.Length];
            for (int i = 0; i < effectMode.Length; i++)
            {
                effectMode[i] = BuffMode.Attr;
            }
        }
        if (effectMode.Length != effectNum.Length || effectMode.Length != effectType.Length)
        {
            TDebug.LogError("buff出错" + idx);
        }
    }
}
