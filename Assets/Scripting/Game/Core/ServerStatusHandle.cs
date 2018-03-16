using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStatusHandle
{

    public bool Handle(int status)
    {
        if (status > 1)
        {
            TDebug.Log("服务器异常提示:" + status);
            System.Action callBack = delegate() { };
            if (IsError(status)) //如果是错误异常，则登出游戏
            {
                callBack = delegate()
                {
                    GameClient.Instance.LoginOutGame();
                };
            }

            UIRootMgr.Instance.IsLoading = false;
            ErrorStatus error = ErrorStatus.ErrorStatusFetcher.GetErrorStatusByCopy(status);
            if (error == null)
            {
                UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("未知错误", Color.red);
                TDebug.LogError(string.Format("未知异常,statusId:{0}", status));
                return true;
            }
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(callBack , error.name, Color.red);
            return true;
        }
        return false;
    }

    bool IsError(int status)
    {
        int ty = status % 100000000;
        ty = ty / 10000000;
        if (ty == 3) return false;
        return true;
    }
	
}
