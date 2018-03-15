using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Threading;
using System.Net;

public class ExceptionSend : MonoBehaviour 
{
    public static string SystemMsg;
    public static string PlayerMsg;
    public static string Platform = "UnInit";
    private static List<string> mSendedMailList = new List<string>();

    void Awake()
    {
#if UNITY_EDITOR
        Platform = "Editor";
#elif UNITY_ANDROID
        Platform = "Android";
#elif UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
        Platform = "iOS";
#else
        Platform = "Standalone";
#endif
        Application.logMessageReceived += HandleLog;

        //SystemMsg = SystemInfo.deviceModel + "|" + SystemInfo.operatingSystem + "|" + SystemInfo.systemMemorySize + "|" + SystemInfo.graphicsDeviceType + "|" + SystemInfo.deviceUniqueIdentifier;
        //string unsend = PlayerPrefs.GetString(CrashLogUnsendKey, "");
        //if (unsend.Length > 0)
        //{
        //    SendExceptionLog(unsend, SystemMsg + "|" + PlayerMsg, Platform);
        //}
    }

    public static void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            //SendExceptionLog(logString + "-" + stackTrace, SystemMsg +"|"+ PlayerMsg, Platform);
        }
    }


    #region 发送崩溃日志
    private static string CrashLogSendedKey = "CrashLogSended";
    private static string CrashLogUnsendKey = "CrashLogUnsend";

    //发送异常日志邮件
    public static void SendExceptionLog(string msg, string playerMsg, string platform)
    {
#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
        return;
#endif
        return;   //TODO:subset中要将.Net.Mail加进去
#if UNITY_EDITOR
        platform = "Editor";
#endif
        string allLog = PlayerPrefs.GetString(CrashLogSendedKey, "");
        //if (allLog.Contains(msg))
        //    return;
        //for (int i = 0; i < allLogList.Length; i++)
        //{
        //    if (allLogList[i].Equals(msg)) //如果有发送过
        //    {
        //        return;
        //    }
        //}
        Debug.Log("开始发送邮件");
        string unsend = PlayerPrefs.GetString(CrashLogUnsendKey, "");
//        if (!unsend.Equals(msg))
//        {
//            PlayerPrefs.SetString(CrashLogUnsendKey, unsend.Length == 0 ? msg : (unsend + "#" + msg));
//            PlayerPrefs.Save();
//            msg = msg + unsend;
//        }
//        Thread thread = new Thread(delegate()//开启子线程
//        {
//            try
//            {
//                MailMessage mailObj = new MailMessage();
//                mailObj.IsBodyHtml = true;
//                mailObj.From = new MailAddress("thegametest@sina.com");
//                mailObj.To.Add(new MailAddress("3385592977@qq.com"));
//                //mailObj.To.Add(new MailAddress("125560479@qq.com"));
//                mailObj.Subject = "PocketImmortal:" + platform + ":" + playerMsg ; //前缀为游戏名
//                mailObj.Body = msg;
//                mailObj.BodyEncoding = System.Text.Encoding.UTF8;
//                SmtpClient smtp = new SmtpClient("smtp.sina.cn", 25);
//                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
//                //下面的回车不能删除
//                smtp.Credentials = (ICredentialsByHost)new NetworkCredential((@";&ƹǵͫ#ƹǨͯ=ƨǒ͹%Ʋ//̤-Ƴǿ").ToEncString(133), ("ﱈ０ﺏﺕﰎｋﻞﺟﰊｏﻒ").ToEncString(-77));
//                smtp.Send(mailObj);
//                mSendedMailList.Add(msg);
//            }
//            catch (System.Exception e)
//            {
//                TDebug.LogErrorFormat("邮件失败:{0}" , e.Message);
//            }
//        });
//        //开启子线程
//        thread.IsBackground = true;
//        thread.Start();
    }

    void Update()
    {
        if (mSendedMailList.Count > 0)
        {
            for (int i = 0; i < mSendedMailList.Count; i++)
            {
                SaveSendSuccess(mSendedMailList[i]);
            }
            mSendedMailList.Clear();
        }
    }

    public void SaveSendSuccess(string sendMsg)
    {
        //TDebug.Log("发送邮件完毕");
        //string allLog = PlayerPrefs.GetString(CrashLogSendedKey, "");
        //PlayerPrefs.SetString(CrashLogSendedKey, allLog.Length == 0 ? sendMsg : (allLog + "#" + sendMsg));
        //PlayerPrefs.SetString(CrashLogUnsendKey, "");
        //PlayerPrefs.Save();
    }
    #endregion

}
