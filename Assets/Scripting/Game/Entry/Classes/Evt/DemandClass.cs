using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//跳转条件，由多个条件项组合而成。
public class DemandData : TrueHandler
{
    public List<DemandHandler> m_DemandList = new List<DemandHandler>();

    //根据目标状态和任务编号，读表进行初始化
    public DemandData(int taskId, TaskStateType aimState, System.Action<object> trueDeleg)
    {
        Register(trueDeleg);
        //读表
        switch (aimState)
        {
            case TaskStateType.Unacceptable: //激活条件
                break;
            case TaskStateType.Acceptable://可领取条件
                break;
            case TaskStateType.Completing://领取条件
                break;
            case TaskStateType.Rewarded://完成条件
                break;
            case TaskStateType.Rejected://废弃条件
                break;
            default:
                break;
        }
    }

    public override void Handle(object val)
    {
        bool allTrue = true;
        for (int i = 0; i < m_DemandList.Count; i++)
        {
            if (!m_DemandList[i].m_IsTrue)
            { allTrue = false; }
        }
        if (allTrue)
        {
            if (m_TrueDeleg!=null) m_TrueDeleg(null);
        }
    }
}


/// <summary>
/// 条件项
/// </summary>
public class DemandHandler : EvtHandler // 条件项
{
    public EvtItemData m_TypeData;      //类型信息
    public Eint m_AimValue;             //目标参数
    public Eint m_CurValue;             //当前值
    public System.Action m_TrueDeleg;       //完成监听
    public bool m_IsTrue;               //第一次为真后，调用完成监听
    public DemandHandler(EvtItemData typeData , int aimValue , System.Action trueDeleg , int curValue = 0)
    {
        m_AimValue = aimValue;
        m_CurValue = curValue;
        m_EvtType = typeData;
        m_TrueDeleg = trueDeleg;
        if (m_TypeData.EventType.CanReqNotice())//某些事件需要主动去请求
        {
            AppEvtMgr.Instance.RequestSendNotice(m_TypeData);
        }
    }

    public override void Handle(object val)
    {
        bool curTrue = FreshAndCheckTrue((int)val);

        if (!m_IsTrue && curTrue)//第一次完成
        {
            if (m_TrueDeleg != null) m_TrueDeleg();
            m_IsTrue = true;
        }
    }

    public bool FreshAndCheckTrue(int val)//根据参数刷新，并且检查要求是否满足
    {
        switch (m_TypeData.EventType.GetTrueType())
        {
            case DemandTrueType.Number:        
                m_CurValue = val;
                m_IsTrue = m_CurValue >= m_AimValue;
                break;
            case DemandTrueType.Delta:
                m_CurValue += val;
                m_IsTrue = m_CurValue >= m_AimValue;
                break;
            case DemandTrueType.Once:
                m_CurValue = 1;
                m_IsTrue = true;
                break;
            default:
                break;
        }
        return m_IsTrue;
    }
}