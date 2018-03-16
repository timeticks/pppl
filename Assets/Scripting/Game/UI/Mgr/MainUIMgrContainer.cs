using UnityEngine;
using System.Collections;

/// <summary>
/// 主界面容器
/// </summary>
public class MainUIMgrContainer:MonoBehaviour
{
    public static AllMainUIMgr MainUI = new AllMainUIMgr();
    public static LobbySceneMainUIMgr LobbyUI { get { return MainUI.m_Lobby; } }
    public static BattleSceneMainUIMgr BattleUI { get { return MainUI.m_Battle; } }
    public static StartSceneMainUIMgr StartUI { get { return MainUI.m_Start; } }


    [HideInInspector]public MainUIMgrType? CurMgr;
    protected Transform m_MainUIRoot;

    /// <summary>
    /// 初始化主界面管理器
    /// </summary>
    /// <typeparam name="T">StartSceneMainUIMgr/AreaSceneMainUIMgr/LoadSceneMainUIMgr/BattleSceneMainUIMgr/</typeparam>
    /// <param name="mgrType"></param>
    public void InitMainUI<T>(MainUIMgrType mgrType)where T:BaseMainUIMgr
    {
        if (CurMgr.HasValue)
        {
            if (CurMgr.Value == mgrType)
                return;
            BaseMainUIMgr mainUi = MainUI.GetMgrByType(CurMgr.Value);

            for (int i = 0; i < mainUi.m_RefList.Count; i++)
            {
                Destroy(mainUi.m_RefList[i]);
            }
            GameObject.Destroy(MainUI.GetMgrObjByType(CurMgr.Value));
        }

        CurMgr = mgrType;
        Object obj = null;
        switch (mgrType)
        {
            case MainUIMgrType.StartSceneMainUIMgr:
                obj = SharedAsset.Instance.LoadAssetSyncObj_ImmediateRelease(BundleType.StartBundle, mgrType.ToString());
                break;
            default:
                obj = SharedAsset.Instance.LoadAssetSyncObj_ImmediateRelease(BundleType.MainBundle, mgrType.ToString());
                break;
        }

        GameObject g = Instantiate(obj) as GameObject;
        TUtility.SetParent(g.transform, m_MainUIRoot, false);
        T t = g.CheckAddComponent<T>();
        t._Init();
        MainUI.SetMgr(t, mgrType);
    }

    public BaseMainUIMgr GetCurMainUI()
    {
        return MainUI.GetMgrByType(CurMgr.Value);;
    }


    public void DestroyMainUI()
    {
        if (CurMgr.HasValue)
        {
            GameObject.Destroy(MainUI.GetMgrObjByType(CurMgr.Value));
        }
    }

}
public class AllMainUIMgr
{
    public StartSceneMainUIMgr m_Start   = null;
    public LobbySceneMainUIMgr m_Lobby     = null;
    public LoadSceneMainUIMgr m_Load     = null;
    public BattleSceneMainUIMgr m_Battle = null;

    public BaseMainUIMgr GetMgrByType(MainUIMgrType mgrType)
    {
        switch (mgrType)
        {
            case MainUIMgrType.StartSceneMainUIMgr:
                return m_Start;
            case MainUIMgrType.LobbySceneMainUIMgr:
                return m_Lobby;
            case MainUIMgrType.LoadSceneMainUIMgr:
                return m_Load;
            default:
                return m_Battle;
        }
    }
    public GameObject GetMgrObjByType(MainUIMgrType mgrType)//得到管理器物体
    {
        return GetMgrByType(mgrType).gameObject;
    }
    public void SetMgr(Object t, MainUIMgrType mgrType)//为管理器赋值
    {
        switch (mgrType)
        {
            case MainUIMgrType.StartSceneMainUIMgr:m_Start = (StartSceneMainUIMgr)t;
                break;
            case MainUIMgrType.LobbySceneMainUIMgr: m_Lobby = (LobbySceneMainUIMgr)t;
                break;
            case MainUIMgrType.LoadSceneMainUIMgr: m_Load = (LoadSceneMainUIMgr)t;
                break;
            default: m_Battle = (BattleSceneMainUIMgr)t;
                break;
        }
    }
}
public enum MainUIMgrType   //与Mgr资源名一一对应
{
    StartSceneMainUIMgr  =0,
    LobbySceneMainUIMgr   =1,
    LoadSceneMainUIMgr   =2,
    BattleSceneMainUIMgr =3
}