using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInfo
{
    public static double    Version;
    public static double    GameVersion;
    //public static string    GateServHost;
    //public static int       GateServPort;
    public static int       BundleVersion;
    public static int       GameDataVersion;
    public static int       GameLocalizaVersion;
    public static double    NoticeVersion;     //整数为公告版本；    小数点后：0.0代表正常运行（只要打开看过一次就不会弹出来），0.1代表正在维护（每次都会弹）

    public static List<ServerArea> ServerList = new List<ServerArea>();

    public static bool NeedOpenNotice() //是否显示公告
    {
        if ((int)NoticeVersion == 0) return false;

        if (NoticeVersion%1 < 0.09f)
        {
            int showedVersion = PlayerPrefs.GetInt("ShowedNoticeVersion", 0);
            if (showedVersion == (int)NoticeVersion)
            {
                PlayerPrefs.SetInt("ShowedNoticeVersion", (int)NoticeVersion);
                return false;
            }
        }
        
        PlayerPrefs.SetInt("ShowedNoticeVersion", (int)NoticeVersion);
        return true;
    }

    public static int GetGateServPort()
    {
        //return GateServPort;
        return GetCurServer().GateServPort;
    }

    public static string GetGateServHost()
    {
        //return GateServHost;
        return GetCurServer().GateServHost;
    }

    public static ServerArea GetCurServer()
    {
        int curServerIdx = PlayerPrefs.GetInt("CurSeverAreaId", -1);
        if (curServerIdx >= ServerList.Count || curServerIdx < 0)
        {
            curServerIdx = ServerList[ServerList.Count - 1].Idx;
            SetCurServer(curServerIdx);
        }
        for (int i = 0; i < ServerList.Count; i++)
        {
            if (ServerList[i].Idx == curServerIdx)
                return ServerList[i];
        }
        return null;
    }

    public static void SetCurServer(int curServerIdx)
    {
        PlayerPrefs.SetInt("CurSeverAreaId", curServerIdx);
        PlayerPrefs.Save();
    }
}
