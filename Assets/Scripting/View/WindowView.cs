using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowView : UIViewBase
{
    public DepthType Depth = DepthType.NormalWindow;

    /// <summary>
    /// 是否将UIRootMgr中的当前窗口设为自身，如果是大窗口的子窗口则不勾选
    /// </summary>
    public bool SetToCurrentWin = true;

    /// <summary>
    /// 打开窗口时的动画
    /// </summary>
    public List<UIAnimationBaseCtrl> OpenAniList;

    /// <summary>
    /// 关闭窗口时的动画
    /// </summary>
    public List<UIAnimationBaseCtrl> CloseAniList;

    /// <summary>
    /// 如果是比较少多次打开的，则设为Destroy。如创建角色窗口
    /// </summary>
    public CloseWinType CloseEvent;

    /// <summary>
    /// 窗口的显示类型。是否全屏
    /// </summary>
    public WinDisplayType DisplayType;

}





public enum DepthType
{
    FloorWindow,
    NormalWindow,
    TopWindow,
    TopBattleWindow,
    OverlayWindow,
}





public enum CloseUIEvent
{
    None,         //不关闭之前的窗口
    CloseAll,     //关闭之前的所有窗口
    CloseCurrent,  //关闭当前的窗口
    HideCurrent   //隐藏当前窗口，不关闭
};
public enum CloseActionType
{
    None,
    OpenHide,//打开上一次直接隐藏的界面
    NotCloseChild, // 不关闭子窗口 

}

public enum CloseWinType
{
    Disable,
    Destroy
}
public enum WinDisplayType
{
    UnfullSreen,     //非全屏窗口
    FullScreen       //全屏窗口
}