using UnityEngine;
using System.Collections;
using System;

public partial class DoActMgr
{
    public void DoAct_OpenWin(DoActData actData)
    { 
        string[] strs = ((string)actData.m_Val).Split(',');
        if (strs.Length < 1) { TDebug.LogErrorFormat("DoActMgr出错, {0}  {1}",actData.m_Val.ToString(),actData.m_Type.ToString()); }
        string winName = strs[0];
        object winTyObj = Enum.Parse(typeof(WinName), winName);
        WinName winTy = (WinName) winTyObj;
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


