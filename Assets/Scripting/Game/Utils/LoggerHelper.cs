using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LoggerHelper : MonoBehaviour {
    private static List<string> mLines      = new List<string>();
    private static List<string> mWriteTxt   = new List<string>();
    private static LoggerHelper mInstance;
    private static LogLevel     mLogLevel;
    private string mOutpath;

    public  bool _IsOutFile     = true;
    public  bool _IsPrintErr    = true;

    public static LoggerHelper Instance
    {
        get { return mInstance; }
    }


    public enum LogLevel
    {
        DEBUG,
        WARNING,
        ERROR,
        Release,
    }

    void Awake()
    {
        mInstance = this;
    }

    
    void Start()
    {

        mOutpath = Application.persistentDataPath + "/applog.txt";
        if (System.IO.File.Exists(mOutpath))
        {
            File.Delete(mOutpath);
        }
    }

    public static void Log(string debug)
    {
        if (mLogLevel <= LogLevel.DEBUG)
        {
            UnityEngine.Debug.Log(debug);
        }
    }

    public static void LogWarning(string warning)
    {

        if (mLogLevel <= LogLevel.WARNING)
        {
            UnityEngine.Debug.LogWarning(warning);
        }
    }

    public static void LogError(string error)
    {
        if (mLogLevel <= LogLevel.ERROR)
        {
            UnityEngine.Debug.LogError(error);
        }
    }

    public static void Break()
    {
        UnityEngine.Debug.Break();
    }

    //void Update()
    //{
    //    if (mWriteTxt.Count > 0 && _IsOutFile)
    //    {
    //        string[] temp = mWriteTxt.ToArray();
    //        foreach (string t in temp)
    //        {
    //            using (StreamWriter writer = new StreamWriter(mOutpath, true, Encoding.UTF8))
    //            {
    //                writer.WriteLine(t);
    //            }
    //            mWriteTxt.Remove(t);
    //        }
    //    }
    //}

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        mWriteTxt.Add(logString);
      
        if (type == LogType.Error || type == LogType.Exception)
          {
            OutLog(logString);
            OutLog(stackTrace);
        }
    }


    static public void OutLog(params object[] objs)
    {
        string text = "";
      
        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }
    
        if (Application.isPlaying)
        {
            if (mLines.Count > 20)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);

        }
    }

    public static LogLevel SetLogLevel
    {
        get { return mLogLevel; }
        set { mLogLevel = value; }
    }

    //void OnGUI()
    //{
    //    if (_IsPrintErr && (Application.platform == RuntimePlatform.Android || Application.platform == //RuntimePlatform.IPhonePlayer))
    //    {
    //        GUI.color = Color.red;
    //        for (int i = 0, imax = mLines.Count; i < imax; ++i)
    //        {
    //            GUILayout.Label(mLines[i]);
    //        }
    //    }
    //}
}
