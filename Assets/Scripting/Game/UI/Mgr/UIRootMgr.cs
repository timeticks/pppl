using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// 所有场景中，所有界面的管理器
/// 
/// </summary>
public class UIRootMgr : MainUIMgrContainer,IAssetUser
{
    public static UIRootMgr Instance { get; private set; }

    public static void SetInstanceNull()
    {
        Instance = null;
    }

    public class ViewObj
    {
        public Camera UICam;
        public Canvas MyCanvas;
        public CanvasScaler MyCanvasScaler;
        public CanvasGroup LobbyCanvas;
        public Transform BaseRoot;
        public Transform FloorWindowRoot;
        public Transform NormalWindowRoot;
        public Transform TopLobbyRoot;
        public Transform TopWindowRoot;
        public Transform TopBattleRoot;
        public Transform OverlayRoot;
        public Transform TopRoot;
        public Transform Window_LoadBar;
        public Transform Window_UpTips;
        public Transform Window_MessageBox;
        public Transform Window_Loading;
        public Transform TopMask;
        public Transform TopBlackMask;
        public GameObject Part_Badge;
        public GameObject Window_Reconnect;
        public Transform Window_Introduce;
        public Image ImageNightMask;
        public ViewObj(UIViewBase view)
        {
            UICam = view.GetCommon<Camera>("UICam");
            MyCanvas = view.GetCommon<Canvas>("Canvas");
            MyCanvasScaler = view.GetCommon<CanvasScaler>("Canvas");
            LobbyCanvas = view.GetCommon<CanvasGroup>("LobbyCanvas");
            BaseRoot = view.GetCommon<Transform>("BaseRoot");
            FloorWindowRoot = view.GetCommon<Transform>("FloorWindowRoot");
            NormalWindowRoot = view.GetCommon<Transform>("NormalWindowRoot");
            TopLobbyRoot = view.GetCommon<Transform>("TopLobbyRoot");
            TopWindowRoot = view.GetCommon<Transform>("TopWindowRoot");
            TopBattleRoot = view.GetCommon<Transform>("TopBattleRoot");
            OverlayRoot = view.GetCommon<Transform>("OverlayRoot");
            TopRoot = view.GetCommon<Transform>("TopRoot");
            Window_LoadBar = view.GetCommon<Transform>("Window_LoadBar");
            Window_UpTips = view.GetCommon<Transform>("Window_UpTips");
            Window_MessageBox = view.GetCommon<Transform>("Window_MessageBox");
            Window_Loading = view.GetCommon<Transform>("Window_Loading");
            TopMask = view.GetCommon<Transform>("TopMask");
            TopBlackMask = view.GetCommon<Transform>("TopBlackMask");
            Part_Badge = view.GetCommon<GameObject>("Part_Badge");
            Window_Reconnect = view.GetCommon<GameObject>("Window_Reconnect");
            ImageNightMask = view.GetCommon<Image>("ImageNightMask");
            Window_Introduce = view.GetCommon<Transform>("Window_Introduce");

        }
    }
    public ViewObj MyViewObj;


    public Camera MyUICam { get { return MyViewObj.UICam; } }
    public Canvas MyCanvas { get { return MyViewObj.MyCanvas; } }
    public CanvasScaler MyCanvasScaler { get { return MyViewObj.MyCanvasScaler; } }
    public CanvasGroup LobbyCanvas { get { return MyViewObj.LobbyCanvas; } }

    public class UIRootContainer
    {
        public static Transform BaseRoot { get { return UIRootMgr.Instance.MyViewObj.BaseRoot; } }        //界面底板 
        public static Transform FloorWindowRoot { get { return UIRootMgr.Instance.MyViewObj.FloorWindowRoot; } }  //血条窗口等  100---
        public static Transform NormalWindowRoot { get { return UIRootMgr.Instance.MyViewObj.NormalWindowRoot; } }       //弹出性窗口  500---
        public static Transform TopLobbyRoot { get { return UIRootMgr.Instance.MyViewObj.TopLobbyRoot; } }     //普通窗口上，高层窗口下 700--
        public static Transform TopWindowRoot { get { return UIRootMgr.Instance.MyViewObj.TopWindowRoot; } }    //高层窗口    900---
        public static Transform TopBattleRoot { get { return UIRootMgr.Instance.MyViewObj.TopBattleRoot; } }    //战斗覆盖性窗口  1000--
        public static Transform OverlayRoot { get { return UIRootMgr.Instance.MyViewObj.OverlayRoot; } }
        public static Transform TopRoot { get { return UIRootMgr.Instance.MyViewObj.TopRoot; } }          //最高层      2000
    }

    public class CommonWindowContainer
    {
        public static Transform Window_Loading{ get { return UIRootMgr.Instance.MyViewObj.Window_Loading; } }
        public static Transform TopMask { get { return UIRootMgr.Instance.MyViewObj.TopMask; } }
        public static Transform Window_MessageBox { get { return UIRootMgr.Instance.MyViewObj.Window_MessageBox; } }
        public static Transform Window_UpTips { get { return UIRootMgr.Instance.MyViewObj.Window_UpTips; } }
        public static Transform Window_LoadBarTrans { get { return UIRootMgr.Instance.MyViewObj.Window_LoadBar; } }
        public static Transform Window_Introduce { get { return UIRootMgr.Instance.MyViewObj.Window_Introduce; } }
    }

    public Window_MessageBox MessageBox { get; private set; }

    public Window_UpTips Window_UpTips  { get; private set; }
    public Window_LoadBar Window_LoadBar { get; private set; }
    public LoadingCircleCtrl Window_Loading { get; private set; }

    public bool IsLoading
    {
        get { return Window_Loading.IsActive; }
        set
        {
            //TDebug.Log(string.Format("SetIsLoading:{0}", value));
            Window_Loading.SetEnable(value);
        }
    }
    public bool IsReconnecting
    {
        get { return MyViewObj.Window_Reconnect.activeSelf; }
        set { MyViewObj.Window_Reconnect.gameObject.SetActive(value); }
    }
    public bool IsNight
    {
        get { return MyViewObj.ImageNightMask.gameObject.activeSelf; }
        set { MyViewObj.ImageNightMask.gameObject.SetActive(value); }
    }
    public bool TopBlackMask
    {
        get { return MyViewObj.TopBlackMask.gameObject.activeSelf; }
        set { MyViewObj.TopBlackMask.gameObject.SetActive(value); }
    }
    public bool TopMasking   //最高层遮罩，全屏且完全透明的遮罩
    {
        get { return MyViewObj.TopMask.gameObject.activeSelf; }
        set { mIsYieldTopMasking = false; MyViewObj.TopMask.gameObject.SetActive(value); }
    }

    private bool mIsYieldTopMasking = false;
    //public void YieldTopMasking(float time)
    //{
    //    if (mIsYieldTopMasking) return;
    //    TopMasking = true;
    //    mIsYieldTopMasking = true;
    //    Invoke("CloseTopMasking", time);
    //}
    //void CloseTopMasking()
    //{
    //    if (mIsYieldTopMasking)
    //    {
    //        TopMasking = false;
    //    }
    //}


    public bool m_IsFullScreen  //用于在打开全屏窗口后，可以禁用场景摄像机以减少消耗...UI游戏暂时不用
    {
        get;set;
        //get { return mOpenWinList.Exists((item) => { return item.m_DisplayType == WinDisplayType.FullScreen; }); }
        //set
        //{
        //    if (SceneMgrBase.InstanceBase != null && SceneMgrBase.InstanceBase.m_WorldCam != null && SceneMgrBase.InstanceBase.m_WorldCam.myCamera != null)
        //    {
        //        SceneMgrBase.InstanceBase.m_WorldCam.myCamera.enabled = (!value);
        //    }
        //}
    }
    private List<WindowBase> mOpenWinList = new List<WindowBase>();
    private List<WindowBase> mDisableWinList = new List<WindowBase>();
    private WindowBase mCurWin;
    private Dictionary<string, Object> mUIObjects = new Dictionary<string, Object>();
    private Dictionary<WinName, List<SetTopTrans>> mPutTopDict = new Dictionary<WinName, List<SetTopTrans>>(); //暂放进TopWindow中的Trans
    public WindowBase CurOpenWindow
    {
        get { return mCurWin; }
        set { mCurWin = value; }
    }

    public void AwakeInit()
    {
        mOpenWinList = new List<WindowBase>();
        mDisableWinList = new List<WindowBase>();
        mUIObjects = new Dictionary<string, Object>();
        mPutTopDict = new Dictionary<WinName, List<SetTopTrans>>(); 

        MyViewObj = new ViewObj(GetComponent<UIViewBase>());
        Init();
    }

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Window_LoadBar = CommonWindowContainer.Window_LoadBarTrans.gameObject.CheckAddComponent<Window_LoadBar>();

            base.m_MainUIRoot = UIRootContainer.BaseRoot;
            MessageBox = CommonWindowContainer.Window_MessageBox.gameObject.CheckAddComponent<Window_MessageBox>();
            MessageBox.Init();
            Window_UpTips = CommonWindowContainer.Window_UpTips.gameObject.CheckAddComponent<Window_UpTips>();
            Window_UpTips.Init();
            Window_Loading = CommonWindowContainer.Window_Loading.gameObject.CheckAddComponent<LoadingCircleCtrl>();
            Window_Loading.Init();
            bool isNight = PlayerPrefs.GetInt("IsNight", 0)==1;
            IsNight = isNight;
        }
    }

    

    #region 窗口列表管理

    //获取当前打开的窗口
    public WindowBase GetCurWindow(string winName)
    {
        if (CurOpenWindow == null)
        {
            TDebug.LogErrorFormat("没有打开的窗口！----{0}" , winName);
            return null;
        }
        else if (CurOpenWindow.name != winName)
        {
            TDebug.LogError("不是当前打开的窗口！---" + winName);
            return null;
        }
        return CurOpenWindow;
    }

    //在所有打开窗口中获取
    public T GetOpenListWindow<T>(WinName winName) where T : WindowBase
    {
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            if (mOpenWinList[i].WindowName == winName && mOpenWinList[i].gameObject.activeSelf)
            {
                return (T)mOpenWinList[i];
            }
        }
        TDebug.LogFormat("列表中没有打开的窗口！---{0}" , winName);
        return default(T);
    }

    public WindowBase GetOpenListWindow(WinName winName)
    {
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            if (mOpenWinList[i].WindowName == winName && mOpenWinList[i].gameObject.activeSelf)
            {
                return mOpenWinList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 是否任意一个窗口是打开状态
    /// </summary>
    public bool IsAnyWindowInOpen(params WinName[] winNames)
    {
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            for (int j = 0; j < winNames.Length; j++)
            {
                if (mOpenWinList[i].WindowName == winNames[j] && mOpenWinList[i].gameObject.activeSelf)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //当窗口打开，不带参数
    public T OpenWindow<T>(WinName winName , CloseUIEvent closeUIEvent = CloseUIEvent.CloseAll)where T :WindowBase
    {
        return OpenWindow<T>(winName.ToString(), closeUIEvent);
    }

    /// <summary>
    /// 隐藏窗口顺序：需要先被打开/渲染层级在下的在前
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="winName"></param>
    /// <param name="hideWin">需要先被打开/渲染层级在下的在前</param>
    public T OpenWindowWithHide<T>(WinName winName ,params WinName[] hideWin) where T : WindowBase
    {
        return OpenWindow<T>(winName.ToString(), CloseUIEvent.HideCurrent, hideWin);
    }


    private T OpenWindow<T>(string winName, CloseUIEvent closeUIEvent = CloseUIEvent.CloseAll, params WinName[] hideWin) where T : WindowBase
    {
        string _name = winName;
        WindowBase openWin = null;
        WindowBase[] hideWindow = null;
        if (CurOpenWindow != null && CurOpenWindow.name == _name)
        {
            if (!CurOpenWindow.gameObject.activeSelf) TDebug.LogErrorFormat("当前窗口被隐藏--{0}" , _name);
            openWin = GetCurWindow(winName);
        }
        else
        {
            if (closeUIEvent == CloseUIEvent.HideCurrent)
            {
                hideWindow = new WindowBase[hideWin.Length];
                for (int i = 0; i < hideWin.Length; i++)
                {
                    WindowBase window = GetOpenListWindow(hideWin[i]);
                    hideWindow[i] = window;               
                }
            }      
            else
                CloseWindow(closeUIEvent);
            bool openWinExist = false;
            bool disableWinExist = false;
            foreach (var tempWin in mOpenWinList)
            {
                if (tempWin.name == winName) { openWinExist = true; break; }
            }
            foreach (var tempWin in mDisableWinList)
            {
                if (tempWin.name == winName) { disableWinExist = true; break; }
            }
            if (openWinExist)
            {
                for (int i = 0; i < mOpenWinList.Count; i++)
                {
                    if (mOpenWinList[i].name == winName)
                    {
                        WindowBase disableWin = mOpenWinList[i];
                        openWin = disableWin;
                        openWin.transform.SetAsLastSibling();
                        break;
                    }
                }
            }
            else if (disableWinExist) //如果窗口之前打开过，没有被销毁
            {
                for (int i = 0; i < mDisableWinList.Count; i++)
                {
                    if (mDisableWinList[i].name == winName)
                    {
                        WindowBase disableWin = mDisableWinList[i];
                        mDisableWinList.RemoveAt(i);
                        mOpenWinList.Add(disableWin);
                        openWin = disableWin;
                        openWin.transform.SetAsLastSibling();
                        break;
                    }
                }              
            }
            else
            {
                TLoader loader = SharedAsset.Instance.LoadAssetSync(BundleType.WindowBundle.BundleDictName() + _name.ToLower(), _name);
                //if (AssetLoaderList == null) AssetLoaderList = new List<TLoader>();
                //AssetLoaderList.Add(loader);

                UnityEngine.Object Obj = (UnityEngine.Object)loader.ResultObject;
                GameObject go = Instantiate(Obj, UIRootContainer.NormalWindowRoot) as GameObject;
#if UNITY_EDITOR
                if (go.GetComponent<T>() != null)
                {
                    Debug.LogError("窗口Prefab上不能直接绑定逻辑脚本！");
                }
#endif
                openWin = go.CheckAddComponent<T>();
                openWin.InitView();
                openWin.SetEnable(true);
                //Vector3 orignal_pos = go.transform.localPosition;//保存窗口预物体的初始位置

                //如果是底层、高层窗口
                if (openWin.Depth == DepthType.FloorWindow) { TUtility.SetParent(go.transform, UIRootContainer.FloorWindowRoot); }
                else if (openWin.Depth == DepthType.TopWindow) { TUtility.SetParent(go.transform, UIRootContainer.TopWindowRoot); }
                else if (openWin.Depth == DepthType.TopBattleWindow) { TUtility.SetParent(go.transform, UIRootContainer.TopBattleRoot); }
                else if (openWin.Depth == DepthType.OverlayWindow) { TUtility.SetParent(go.transform, UIRootContainer.OverlayRoot); }
                if (openWin.CloseEvent == CloseWinType.Destroy) //只对关闭即销毁的窗口进行回收
                {
                    loader.Release();
                }

                go.name = _name;
                mOpenWinList.Add(openWin);

                //使用完后，直接释放
                SharedAsset.Instance.UnloadBundle(BundleType.WindowBundle.BundleDictName() + _name.ToLower(), false);
            }
        }
        openWin.SetHideWindow(hideWindow);
        m_IsFullScreen = false;
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            if (mOpenWinList[i] != null && mOpenWinList[i].m_DisplayType.Equals(WinDisplayType.FullScreen))
            {
                m_IsFullScreen = true;
            }
        }

        return openWin as T;
    }

    public List<TLoader> AssetLoaderList { get; set; }

    public void DisposeUsedAsset()
    {
        if (AssetLoaderList != null)
        {
            for (int i = 0; i < AssetLoaderList.Count; i++)
            {
                AssetLoaderList[i].Release();
            }
        }
    }

    //public Window_Lua OpenLuaWindow(string winName, CloseUIEvent closeUIEvent)
    //{
    //    string _name = winName;
    //    if (CurOpenWindow != null && CurOpenWindow.name == _name)
    //    {
    //        if (!CurOpenWindow.gameObject.activeSelf) TDebug.LogError("当前窗口被隐藏--" + _name);
    //        return (Window_Lua)GetCurWindow(winName);
    //    }
    //    CloseWindow(closeUIEvent);
    //    TLoader loader = SharedAsset.Instance.LoadAssetSync(BundleType.WindowBundle.BundleDictName() + _name.ToLower(), _name);
    //    loader.Release();

    //    GameObject go = Instantiate((Object)loader.ResultObject) as GameObject;
    //    Vector3 orignal_pos = go.transform.localPosition;
    //    go.name = _name;
    //    go.transform.SetParent(m_Root.m_WindowRoot);
    //    go.transform.localScale = Vector3.one;
    //    go.transform.localPosition = orignal_pos;
    //    WindowBase mWin = go.GetComponent<WindowBase>();
    //    return (Window_Lua)mWin;
    //}

    /// <summary>
    /// 关闭当前打开窗口或全部关闭
    /// </summary>
    public void CloseWindow(CloseUIEvent closeUIEvent)
    {
        switch (closeUIEvent)
        {
            case CloseUIEvent.None: break;
            case CloseUIEvent.CloseCurrent:
                if (CurOpenWindow != null)
                {
                    CurOpenWindow.CloseWindow();
                }
                break;
            case CloseUIEvent.CloseAll:
                for (int i = mOpenWinList.Count-1; i >=0; i--)
                {
                    mOpenWinList[i].CloseWindow();
                }
                break;
        }
    }

    public WindowBase HideWindow()
    {
        WindowBase window=null;
        if (CurOpenWindow != null)
        {
            window = CurOpenWindow;
            CurOpenWindow.CloseWindow();
        }
        return window;  
    }


    public void AddOpenWindowList(WindowBase win)
    {
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            if (mOpenWinList[i].WindowName == win.WindowName)
            {
                return;
            }
        }
        mOpenWinList.Add(win);
    }

    /// <summary>
    /// 在打开窗口列表中关闭指定窗口
    /// </summary>
    public void CloseWindow_InOpenList(WinName winName)
    {
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            if (mOpenWinList[i].WindowName == winName)
            {
                mOpenWinList[i].CloseWindow();
                return;
            }
        }
        TDebug.LogFormat("没有此窗口{0}" , winName);
    }
    /// <summary>
    /// 获取打开的窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="winName"></param>
    /// <returns></returns>
    public T GetOpenWindow<T>(WinName winName) where T:WindowBase
    {
        for (int i = 0,length= mOpenWinList.Count; i < length; i++)
        {
            if (mOpenWinList[i].WindowName == winName)
            {
                return (T)mOpenWinList[i];
            }
        }
        return null;
    }

    public void FreshBadgeInOpenWindow()
    {
        for (int i = 0, length = mOpenWinList.Count; i < length; i++)
        {
            if (mOpenWinList[i].IsActiveShow)
            {
                mOpenWinList[i].FreshBadge();
            }
        }
    }

    /// <summary>
    /// 在打开窗口列表中移除指定窗口
    /// </summary>
    public void RemoveWindowInOpen(WindowBase win)
    {
        mOpenWinList.Remove(win);
        m_IsFullScreen = false;
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            if (mOpenWinList[i] != null && mOpenWinList[i].m_DisplayType.Equals(WinDisplayType.FullScreen))
            {
                m_IsFullScreen = true;
            }
        }
        if (mOpenWinList.Count == 0)
        {
            //TDebug.Log("所有被窗口关闭");
            //BaseMainUIMgr baseui = GetCurMainUI();
            //if (baseui != null) baseui.ShowMainUI();
            if (showUICor != null) StopCoroutine(showUICor);
            showUICor = ShowUICor();
            StartCoroutine(showUICor);
        }
    }

    private IEnumerator showUICor;
    public IEnumerator ShowUICor()
    {
        yield return null;
        if (mOpenWinList.Count == 0)
        {
            BaseMainUIMgr baseui = GetCurMainUI();
            if (baseui != null) baseui.ShowMainUI();
        }
    }

    //将此窗口，放于list末尾
    public void FreshWindowInDisable(WindowBase win) 
    {
        if (win.CloseEvent == CloseWinType.Disable)
        {
            for (int i = 0; i < mDisableWinList.Count; i++)
            {
                if (mDisableWinList[i] == win)
                {
                    mDisableWinList.RemoveAt(i);
                }
            }
            mDisableWinList.Add(win);
        }
        int maxWindowNum = 12;
        CloseDisableWin(maxWindowNum);
    }

    //回收窗口
    public void CloseDisableWin(int remainMax)
    {
        if (mDisableWinList.Count > remainMax)  //TODO：简单的窗口回收
        {
            //TDebug.Log("缓存窗口数量太多，移除最早的");
            WindowBase destroyWin = mDisableWinList[0];
            if (destroyWin.WindowName == WinName.Window_Battle) //战斗窗口尽量不回收
            {
                mDisableWinList.RemoveAt(0);
                mDisableWinList.Add(destroyWin);
                destroyWin = mDisableWinList[0];
            }
            bool isOpen = false;
            for (int i = 0; i < mOpenWinList.Count; i++)
            {
                if (mOpenWinList[i].WindowName == destroyWin.WindowName)
                {
#if UNITY_EDITOR
                    TDebug.LogErrorFormat("正在使用的窗口，被添加到了废弃列表{0}", destroyWin.WindowName);
#endif
                    isOpen = true;
                }
            }
            if (!isOpen)
            {
                mDisableWinList.RemoveAt(0);
                destroyWin.BeforeDestroy();
                DestroyImmediate(destroyWin.gameObject);
                Resources.UnloadUnusedAssets();
            }
        }
    }

    /// <summary>
    /// 将窗口的某一部分暂放在TopWindow下，并且添加进关联表，在此窗口关闭时还原回去
    /// </summary>
    public void SetRootInTopLobby(WinName winName, Transform panelRoot, bool setToTopWindowRoot =false)
    {
        if (panelRoot == null) return;
        if (!mPutTopDict.ContainsKey(winName)) mPutTopDict.Add(winName, new List<SetTopTrans>());
       
        List<SetTopTrans> tempList = mPutTopDict[winName];
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].MyTrans == null)//将空的移除掉
            {
                tempList.RemoveAt(i);
                i--;
            }
            else if (tempList[i].MyTrans == panelRoot)
            {
                panelRoot.SetAsLastSibling();
                return;
            }
        }
        tempList.Add(new SetTopTrans(panelRoot, panelRoot.parent));
        if (setToTopWindowRoot)
        {
            panelRoot.SetParent(UIRootContainer.TopWindowRoot.transform);
            panelRoot.SetAsLastSibling();
            return;
        }
        panelRoot.SetParent(UIRootContainer.TopLobbyRoot.transform);
    }


    //将暂放在TopWindow下的Trans，还原到原窗口下
    public void TryResetTopTrans(WinName winName)
    {
        if (mPutTopDict.ContainsKey(winName))
        {
            List<SetTopTrans> tempList = mPutTopDict[winName];
            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i].MyTrans == null) continue;
                tempList[i].MyTrans.SetParent(tempList[i].OriginParent);
                tempList[i].MyTrans.gameObject.SetActive(false);
            }
            tempList.Clear();
        }
    }


    //是否打开了指定的窗口
    public bool IsWinOpen(WinName winName)
    {
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            if (mOpenWinList[i].WindowName == winName)
            {
                if (!mOpenWinList[i].gameObject.activeSelf) TDebug.LogErrorFormat("当前窗口被隐藏：{0}" , winName);
                return true;
            }
        }
        return false;
    }

    public void DebugOpenWinName()
    {
        if (mOpenWinList == null) return;
        for (int i = 0; i < mOpenWinList.Count; i++) { TDebug.Log(mOpenWinList[i].WindowName); }
    }

    public void CloseAllOpenWin() //关闭所有窗口
    {
        for (int i = 0; i < mOpenWinList.Count; i++)
        {
            mOpenWinList[i].CloseWindow(CloseActionType.None);
        }
    }
    #endregion

    public List<WindowBase> OpenWinList
    {
        get { return mOpenWinList; }
    }

    void OnDestroy()
    {
        Instance = null;
    }
}

public class SetTopTrans
{
    public Transform MyTrans;
    public Transform OriginParent;

    public SetTopTrans(Transform myTrans, Transform parentTrans)
    {
        MyTrans = myTrans;
        OriginParent = parentTrans;
    }
}