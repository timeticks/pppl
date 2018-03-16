using UnityEngine;
using System.Collections;

public class NotificationTest : MonoBehaviour
{
    public static bool testFlag;

    public static bool OpenTest;

    public static int cancleWay = 0;

    float sleepUntil = 0;

    void OnGUI()
    {
        //Color is supported only in Android >= 5.0
        GUI.enabled = sleepUntil < Time.time;

        if (GUI.Button(new Rect(0, Screen.height - 295, 300, 30), "5 SECONDS RetreatFinish"))
        {
            LocalNotificationMgr.RegisterNotification(NotifyType.RetreatFinish, 5, PlayerPrefsBridge.Instance.PlayerData.PlayerUid);
        }
        if (GUI.Button(new Rect(0, Screen.height - 265, 300, 30), "5 SECONDS ZazenFinish"))
        {
            LocalNotificationMgr.RegisterNotification(NotifyType.ZazenFinish, 5, PlayerPrefsBridge.Instance.PlayerData.PlayerUid);
        }
        if (GUI.Button(new Rect(0, Screen.height - 235, 300, 30), "298 SECONDS RetreatFinish"))
        {
            LocalNotificationMgr.RegisterNotification(NotifyType.RetreatFinish, 298, PlayerPrefsBridge.Instance.PlayerData.PlayerUid);
        }
        if (GUI.Button(new Rect(0, Screen.height - 205, 300, 30), "3 SECONDS RetreatFinish"))
        {
            LocalNotification.SendNotification(10011053, 5, "闭关已完成", "已经做好了突破境界的准备，快来应劫吧！", new Color32(0xff, 0x44, 0x44, 255));
            PlayerPrefs.SetInt("10011053", 1);
        }
        if (GUI.Button(new Rect(0, Screen.height - 175, 300, 30), "5 SECONDS Long message text"))
        {
            //  NotificationMgr.SendNotification(1, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
            LocalNotification.SendNotification(1, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
        }
        if (GUI.Button(new Rect(0, Screen.height - 145, 300, 30), "5 SECONDS 100000000"))
        {
            //  NotificationMgr.SendNotification(100000000, 5, "闭关已完成", "已经做好了突破境界的准备，快来应劫吧！", new Color32(0xff, 0x44, 0x44, 255));
            LocalNotification.SendNotification(100000000, 5, "闭关已完成", "已经做好了突破境界的准备，快来应劫吧！", new Color32(0xff, 0x44, 0x44, 255));
        }
        if (GUI.Button(new Rect(0, Screen.height - 115, 300, 30), "10 dddddd"))
        {
            // NotificationMgr.SendNotification(2, 10, "ddd", "dddddd!", new Color32(0xff, 0x44, 0x44, 255));
            LocalNotification.SendNotification(2, 10, "ddd", "dddddd!", new Color32(0xff, 0x44, 0x44, 255));
        }
        if (GUI.Button(new Rect(0, Screen.height - 85, 300, 30), "Stop"))
        {
            LocalNotificationMgr.CancelNotify(PlayerPrefsBridge.Instance.PlayerData.PlayerUid);
        }
        //if (GUILayout.Button("5 SECONDS BIG ICON", GUILayout.Height(Screen.height * 0.2f)))
        //{
        //    LocalNotification.SendNotification(1, 5, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
        //    sleepUntil = Time.time + 5;
        //}

        //if (GUILayout.Button("EVERY 5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        //{
        //    LocalNotification.SendRepeatingNotification(1, 5, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
        //    sleepUntil = Time.time + 99999;
        //}

        //if (GUILayout.Button("10 SECONDS EXACT", GUILayout.Height(Screen.height * 0.2f)))
        //{
        //    LocalNotification.SendNotification(1, 10, "Title", "Long exact message text", new Color32(0xff, 0x44, 0x44, 255), executeMode: LocalNotification.NotificationExecuteMode.ExactAndAllowWhileIdle);
        //    sleepUntil = Time.time + 10;
        //}

        //GUI.enabled = true;

        //if (GUILayout.Button("STOP", GUILayout.Height(Screen.height * 0.2f)))
        //{
        //    LocalNotification.CancelNotification(1);
        //    sleepUntil = 0;
        //}
    }
}
