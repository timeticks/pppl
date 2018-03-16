using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大窗口基类，主要用于在窗口中，有几个窗口切换的。
/// 如角色界面，其中又可以分页切换到角色装备窗口、背包窗口、角色技能窗口、宠物窗口等
/// </summary>
public class BigWindowBase : WindowBase
{
    protected List<WindowBase> m_openedChildWin;
    protected WindowBase m_curChildWin;

    /// <summary>
    /// 用于切换内置的窗口
    /// </summary>
    public virtual void SwitchChildWindow(int childWinIndex)
    { 
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }
}
