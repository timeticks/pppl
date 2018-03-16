using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class LocalNotification
{
    public enum NotificationExecuteMode
    {
        Inexact = 0,
        Exact = 1,
        ExactAndAllowWhileIdle = 2
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static string mainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";
    private static  AndroidJavaClass pluginClass ;
#endif

    public static void InitLocalNotification()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pluginClass = new AndroidJavaClass("com.placegame.nativeplugins.UnityNotificationManager");
#endif
    }

    public static void SendNotification(int id, TimeSpan delay, string title, string message)
    {
        SendNotification(id, (int)delay.TotalSeconds, title, message, Color.white);
    }

    public static void SendNotification(int id, long delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", NotificationExecuteMode executeMode = NotificationExecuteMode.Exact)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (pluginClass != null)
        {
         //   Debug.LogError(string.Format("本地消息推送注册：id:{0},Time:{1},Title:{2},msg{3}", id, delay, title, message));
            pluginClass.CallStatic("SetNotification", id, delay * 1000L, title, message, message, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "app_icon", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, (int)executeMode, mainActivityClassName);
        }
#endif
    }

    public static void SendRepeatingNotification(int id, long delay, long timeout, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id, delay * 1000L, title, message, message, timeout * 1000, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "app_icon", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, mainActivityClassName);
        }
#endif
    }

    public static void CancelNotification(int id)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (pluginClass != null) {
            pluginClass.CallStatic("CancelNotification", id);
        }
#endif
    }

    //public static void CancelAllNotifications()
    //{
    //#if UNITY_ANDROID && !UNITY_EDITOR
    //    AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
    //    if (pluginClass != null)
    //        pluginClass.CallStatic("CancelAll");
    //#endif
    //}
}
