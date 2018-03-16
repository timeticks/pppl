using UnityEngine;
using System.Collections;
using System;

public abstract class EvtHandler //事件处理
{
    public EvtItemData m_EvtType;
    public Action<object> m_Deleg;
    public virtual void Register()//注册监听
    {
        //m_Deleg = Handle;
        //AppEvtMgr.Instance.Register(m_EvtType, m_Deleg);
    }
    public virtual void Remove()//移除监听
    {
        //AppEvtMgr.Instance.Remove(m_EvtType, m_Deleg);
    }

    public abstract void Handle(object val);//处理监听
    
}

public abstract class TrueHandler : ITrueHandler  //为真处理...Handle中判断是否为真，为真时调用
{
    protected System.Action<object> m_TrueDeleg { get; private set; }
    public virtual void Register(System.Action<object> trueDeleg)
    {
        m_TrueDeleg += trueDeleg;
    }
    public void Clear()
    {
        m_TrueDeleg = null;
    }

    public abstract void Handle(object val);
    
}
public interface ITrueHandler   //为真处理接口
{
    void Register(System.Action<object> trueDeleg); 
    void Clear();
    void Handle(object val);
}