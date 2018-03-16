using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseObject
{
    public Eint idx=0;
    public string name="";

    public BaseObject()
    {
    }

    public BaseObject(BaseObject origin)
    {
        idx = origin.idx;
        name = origin.name;
    }

    
    public virtual void CheckLegal()
    {
        //if (valProb.Length != pctProb.Length)
        //    TDebug.LogError(string.Format("AttrTable错误:{0}", idx));
    }
}


public interface IObjectClone
{
    BaseObject Clone();
}