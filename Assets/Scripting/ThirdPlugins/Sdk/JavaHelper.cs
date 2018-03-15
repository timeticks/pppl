using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaHelper
{
//#if UNITY_ANDROID
    public static JavaHelper Instance = new JavaHelper();
//#endif
    bool isInit = false;

    private void Init()
    {
        //需要先获取游戏当前activity
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass jj = new AndroidJavaClass("com.placegame.nativeplugins.AssetsBundleHelpr");
        jj.CallStatic("init", jo);
    }

    public byte[] GetStreamAssets(string path)
    {
        if (path.Contains("!/assets/"))
        {
            path = path.Substring(path.IndexOf("!/assets/"));
            path = path.Replace("!/assets/", "");
        }
        if (!isInit)
        {
            isInit = true;
            Init();
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.placegame.nativeplugins.AssetsBundleHelpr");
        return jc.CallStatic<byte[]>("getBytes", path);
    }


    public void AndroidRestart()
    {
#if !UNITY_ANDROID
        TDebug.LogError("非安卓，不能调用安卓重启");
        return;
#endif
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.placegame.nativeplugins.Restart");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("getInstance", "");
            //AndroidJavaObject jo.Call("restart", 10);
            jo.Call("restartApplication");
        }
        catch (Exception e)
        {
            TDebug.LogErrorFormat("AndroidRestart错误：{0}" , e.Message);
        }
    }
	
}
