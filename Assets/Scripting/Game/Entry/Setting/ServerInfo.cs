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
    public static double    LogicVersion;
    public static int       GameLocalizaVersion;
    public static double    NoticeVersion;     //整数为公告版本；    小数点后：0.0代表正常运行（只要打开看过一次就不会弹出来），0.1代表正在维护（每次都会弹）
    public static string    PassportHost;
    public static int       PassportPort;
    public static int    RecommendServer; // 推荐服
    public static bool OnlyAccountLogin;

    public static bool IsCachedServer = false; // 本地是否缓存了服务器id

    public static List<PassPortHostArea> PassPortHostList = new List<PassPortHostArea>();
    public static List<ServerArea> ServerList = new List<ServerArea>();
    public static List<OpenPackageStore> StoreList = new List<OpenPackageStore>();
    public static bool IsUpdateForce;
    public static int PassportHostCount
    {
        get { return PassPortHostList.Count; }
    }
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
        if (ServerNoticeInfo.EndTime > 0 && ServerNoticeInfo.EndTime < TimeUtils.CurrentTimeMillis)
        {
            TDebug.Log(string.Format("公告超时:{0}", TimeUtils.CurrentTimeMillis - ServerNoticeInfo.EndTime));
            return false;
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
        if (curServerIdx > ServerList.Count || curServerIdx < 0)
        {
            curServerIdx = ServerInfo.RecommendServer;
            SetCurServer(curServerIdx);
        }
        for (int i = 0; i < ServerList.Count; i++)
        {
            if (ServerList[i].Idx == curServerIdx)
                return ServerList[i];
        }
        return ServerList[0];
    }

    public static void SetCurServer(int curServerIdx)
    {
        PlayerPrefs.SetInt("CurSeverAreaId", curServerIdx);
        PlayerPrefs.Save();
    }
    
    public static PassPortHostArea GetCurPassPortHost(int curId = -1)
    {
        int curPassPortHostIdx = PlayerPrefs.GetInt("CurPassPortHostId", -1);
        if (curPassPortHostIdx > PassPortHostList.Count || curPassPortHostIdx < 0)
        {
            curPassPortHostIdx = PassPortHostList[0].Idx;
            SetCurPassPortHost(curPassPortHostIdx);
        }
        if (curId == -1) 
        {
            for (int i = 0; i < PassPortHostList.Count; i++)
            {
                if (PassPortHostList[i].Idx == curPassPortHostIdx)
                    return PassPortHostList[i];
            }
        }
        else // 上一个连接失败，尝试新的地址
        {
            for (int i = 0; i < PassPortHostList.Count; i++)
            {
                if (PassPortHostList[i].Idx == curId)
                {
                    if (i + 1 < PassPortHostList.Count)
                        return PassPortHostList[i+1];
                }
            }
        }
       
        return PassPortHostList[0];
    }

    public static void SetCurPassPortHost(int curPassPortHostIdx)
    {
        PlayerPrefs.SetInt("CurPassPortHostId", curPassPortHostIdx);
        PlayerPrefs.Save();
    }


    public static string GetCurStoreUrl()
    {
        for (int i = 0; i < StoreList.Count; i++)
        {
            if(StoreList[i].Name == AppSetting.Channel)
            {
                return StoreList[i].Url;
            }
        }
        TDebug.LogErrorFormat("没有找到此渠道url：{0}" , AppSetting.Channel);
        return "";
    }










    public static int IsForceUpdate(double fromVer, double toVer)
    {
#if UNITY_EDITOR
        return 0;
#endif
        if (Mathf.Abs((float)(fromVer - toVer)) < 0.0001f) return 0;
        if (!ServerInfo.GameVersion.Equals(AppSetting.GameVersion))
        {
            if (PlatformUtils.PlatformTy == PlatformType.Android)
            {
                if ((int)(fromVer / 100) - (int)(toVer / 100) > 0) //只有百位数改变，android才会强制更新包
                {
                    return 2;
                }
                return 1;
            }
            else
            {
                if (Mathf.Abs((int)fromVer - (int)toVer) >= 1)
                    return 2;
                return 0;
            }
            //mIAssetsLoader.OnChangeState("客户端版本已有新版本(Ver:"+ servSetting.GameVersion+ "),请重新更新游戏版本,W_Code:105");
        }
        return 0;
    }
}



public class OpenPackageStore
{
    public string Name;
    public string Url;
}