using UnityEngine;
using System.Collections;
using System;

public partial class DoActMgr
{
    public void DoAct_OpenWin(DoActData actData)
    { 
        string[] strs = ((string)actData.m_Val).Split(',');
        if (strs.Length < 1) { TDebug.LogError("DoActMgr出错, "+actData.m_Val+"    "+actData.m_Type.ToString()); }
        string winName = strs[0];
        WinName winTy = (WinName)Enum.Parse(typeof(WinName), winName);
        switch (winTy)
	    {
            case WinName.UIRoot:
            case WinName.Window_Login:
            case WinName.AreaUIRoot:
                break;
            case WinName.Window_Lua:
                break;
            case WinName.Window_Chat:
                break;
            default:
                break;
	    }
    }
}


