using System.Collections;
using UnityEngine;

/// <summary>
/// 将错误信息输出到游戏屏幕上
/// </summary>
public class AndirDebug : MonoBehaviour
{
    internal static bool mIsPrefsDebug = true;  //打开debug存档
    public TDebug.LogLevelType LogLevel
    {
        get { return mLogLevel; }
        set
        {
            mLogLevel = value;
#if UNITY_ANDROID
        TDebug.LogLevel = TDebug.LogLevelType.ERROR;
#else
            TDebug.LogLevel = value;
#endif
        }
    }

    [SerializeField]
    private TDebug.LogLevelType mLogLevel;
    public bool Log;
    public bool LogTrace;

    internal GUIStyle mSt;
    private float mScreenRatio;
    private Vector2 mScroll;        //
    private bool mShowLog = true;   //是否显示log
    internal TDebug.LogLevelType mShowLogLevel = TDebug.LogLevelType.ERROR;

    public static void ChangePrefsDebug()
    {
        //m_IsPrefsDebug = PlayerPrefs.GetInt("isPrefsDebug", 0) == 1;
        PlayerPrefs.SetInt("isPrefsDebug", mIsPrefsDebug ? 0 : 1);
    }

    void Awake()
    {
        Texture2D tex = new Texture2D(2, 2);
        for (int i = 0; i < tex.height; i++)
        {
            for (int j = 0; j < tex.width; j++)
            {
                tex.SetPixel(i, j, Color.black);
            }
        }
        mSt = new GUIStyle();
        mScreenRatio = Screen.height / 1920f;
        mSt.fontSize = Mathf.FloorToInt(24);
        mSt.normal.background = tex;
        TDebug.LogLevel = LogLevel;

        //gameObject.CheckAddComponent<FPSCounter>().m_FontSize = 30;
    }

    internal void OnEnable()
    {
        if (mIsPrefsDebug)
            mLogCached = PlayerPrefs.GetString("logs", "");
        Application.logMessageReceived += HandleLog;
    }

    internal void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private string mLogCached = "";
    private string mActionStr = "";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logString">错误信息</param>
    /// <param name="stackTrace">跟踪堆栈</param>
    /// <param name="type">错误类型</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if ((mShowLogLevel == TDebug.LogLevelType.ERROR && type != LogType.Log && type != LogType.Warning)
            || (mShowLogLevel == TDebug.LogLevelType.WARNING && type != LogType.Log)
            || (mShowLogLevel == TDebug.LogLevelType.DEBUG))
        {

            if (type == LogType.Error || type == LogType.Exception)
            {
                if (LogTrace && !mLogCached.Contains(stackTrace))
                    mLogCached += string.Format("<color=#ff0000ff>{0}\r\n{1}\r\n</color>", logString, stackTrace);
                else
                    mLogCached += string.Format("<color=#ff0000ff>{0}</color>\r\n", logString);
            }
            else
            {
                if (type == LogType.Log)
                    mLogCached += logString + "\r\n";
                else
                    mLogCached += string.Format("<color=#CD7F65FF>{0}</color>\r\n", logString);
            }
            PlayerPrefs.SetString("logs", mLogCached);
        }
    }

    void Update()
    {
        //TDebug.LogLevel = mLogLevel;
    }

    void OnGUI()
    {
        if (!Log) return;
        if (mLogCached.Length > 10000) //设置字数上限
        {
            mLogCached = mLogCached.Substring(mLogCached.IndexOf("\r\n", 4000) + 1);
        }
        if (mShowLog)
        {
            if (mLogCached != "")
            {
                mScroll = GUILayout.BeginScrollView(mScroll, GUILayout.Width(Screen.width - 100 * mScreenRatio), GUILayout.MaxHeight(Screen.height * 0.3f));
                GUILayout.Label("\r\n" + mLogCached, mSt);
                GUILayout.EndScrollView();
            }
            if (GUI.Button(new Rect(Screen.width - 80 * mScreenRatio, 10 * mScreenRatio, 90 * mScreenRatio, 80 * mScreenRatio), string.Format("<size=20>{0}</size>", LogTrace.ToString())))
            {
                LogTrace = !LogTrace;
            }

            if (GUI.Button(new Rect(Screen.width - 80 * mScreenRatio, 100 * mScreenRatio, 90 * mScreenRatio, 80 * mScreenRatio),
                string.Format("<size=18>{0}</size>", mShowLogLevel.ToString())))
            {
                if (mShowLogLevel == TDebug.LogLevelType.DEBUG)
                    mShowLogLevel = TDebug.LogLevelType.WARNING;
                else if (mShowLogLevel == TDebug.LogLevelType.WARNING)
                    mShowLogLevel = TDebug.LogLevelType.ERROR;
                else
                    mShowLogLevel = TDebug.LogLevelType.DEBUG;
                TDebug.LogLevel = mShowLogLevel;
            }
            mActionStr = GUI.TextField(new Rect(Screen.width - 80 * mScreenRatio, 190 * mScreenRatio, 90 * mScreenRatio, 30 * mScreenRatio), mActionStr);
            if (GUI.Button(new Rect(Screen.width - 80 * mScreenRatio, 220 * mScreenRatio, 90 * mScreenRatio, 80 * mScreenRatio),
                string.Format("<size=20>{0}</size>", "执行")))
            {
                if (AppBridge.Instance != null) AppBridge.Instance.DoAndirLog(mActionStr);
            }

        }


        if (GUI.Button(new Rect(0, Screen.height - 33, 200, 30), "显示Log"))
        {
            mShowLog = !mShowLog;
        }
        if (!string.IsNullOrEmpty(mLogCached) && mLogCached != "" && mShowLog)
        {
            GUILayout.Label("", GUILayout.Height(5 * mScreenRatio));
            if (GUI.Button(new Rect(0, Screen.height - 70, 200, 30), "=======清空error======="))
            //if (GUILayout.Button("=======清空error=======", GUILayout.Height(80 * ScreenRatio)))
            {
                ClearShowLog();
            }
        }

        //if (GUI.Button(new Rect(0, Screen.height - 403, 200, 200), "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"))
        //{
        //}
    }

    public void ClearShowLog()
    {
        mLogCached = "";
        PlayerPrefs.SetString("logs", mLogCached);
    }
}