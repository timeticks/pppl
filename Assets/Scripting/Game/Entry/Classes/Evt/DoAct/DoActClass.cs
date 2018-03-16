using UnityEngine;
using System.Collections;
using System;
using System.Text;


public class DoActData //行为信息类
{
    public object[] m_Objs;   //执行行为的参数，如打开窗口需要传入某些值
    public Estring m_Val;     //行为值
    public DoActType m_Type;  //行为类型
    public DoActData(DoActType ty , string val = null)
    {
        m_Val = val;
        m_Type = ty;
    }
    public DoActData(DoActType ty,string val , object[] objs)
    {
        m_Val = val;
        m_Type = ty;
        m_Objs = objs;
    }

    public string ToKey()
    {
        StringBuilder keyStr =new StringBuilder( m_Type+ m_Val);
        if (m_Objs != null)
        {
            for (int i = 0; i < m_Objs.Length; i++)
            {
                keyStr.Append(m_Objs[i]);
            }
        }
        return keyStr.ToString();
    }
}

public enum DoActType:byte //根据配置表，进行的行为类型
{
    OpenWin,   //WinName , CloseType , 参数列表
    CloseWin,  

    ActiveTask,
    AcceptTask,
    FinishTask,
    RejectTask,

    ShowChat,

    Touch,    //引导玩家点击某处
    Drag,     //引导玩家拖拽


    Max
}




public interface IDoAct
{
    void DoAct(DoActData actData);
}