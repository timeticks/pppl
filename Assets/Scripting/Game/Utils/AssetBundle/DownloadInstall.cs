using UnityEngine;
using System.Collections;
using System.IO;


/// <summary>
/// 下载并覆盖安装
/// </summary>
public class DownloadInstall : MonoBehaviour
{
    internal string m_ApkPath = "";
    public Window_LoadBar.AsyncData m_DownProgress;  //下载进度

    public Window_LoadBar.AsyncData StartDownPackage()
    {
        //m_ApkPath = SharedAsset.GetPersistentHeadPath(true) + m_setData.m_Data.m_PackageName;
//#if !UNITY_EDITOR
//        m_DownProgress = new LoadProgressData();
//        StartCoroutine(DownloadCor());
//        return m_DownProgress;
//#endif
        return null;
    }


    void Install()
    {
        //获取Android的Java接口
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string isSuccess = jo.Call<string>("InstallApk", m_ApkPath);
        if (isSuccess != "")
        {
            Debug.LogError(isSuccess);
        }
    }

    public static NetworkType GetNetworkType()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string tyStr = jo.Call<string>("GetNetWorkType");
        if (tyStr == "WIFI")
        {
            return NetworkType.WIFI;
        }
        else if (tyStr == "GPRS")
        {
            return NetworkType.GPRS;
        }
        return NetworkType.NONE;
    }


}


public enum NetworkType:byte
{
    NONE,
    GPRS,
    WIFI
}