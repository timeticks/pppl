using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class WindowBase : MonoBehaviour 
{

    private Dictionary<NetCode_S, ServPacketHander> mServPackHanderDic;

    protected WindowView mViewBase;

    public WinName WindowName
    {
        get
        {
            if (mWinName == WinName.None)
            {
                string objName = (mViewBase.gameObject.name == null) ? "" : mViewBase.gameObject.name;
                object nameObj = System.Enum.Parse(typeof(WinName), objName); ;
                mWinName = (WinName)nameObj;
            }
            return mWinName;
        }
    }
    private WinName mWinName = WinName.None;


    public DepthType Depth { get { return mViewBase.Depth; } }

    public List<UIAnimationBaseCtrl> m_OpenAniList { get { return mViewBase.OpenAniList; } }
    public CloseWinType CloseEvent { get { return mViewBase.CloseEvent; } }
    public WinDisplayType m_DisplayType { get { return mViewBase.DisplayType; } }
    public List<UIAnimationBaseCtrl> m_CloseAniList { get { return mViewBase.CloseAniList; } }
    protected List<TLoader> mAssetLoaderList = new List<TLoader>();
    private bool SetToCurrentWin { get { return mViewBase.SetToCurrentWin; } }

    private bool m_staticDepth = false;

    private List<WindowBase> m_ChildWindow = new List<WindowBase>();

    public WindowBase CurChildWindow
    {
        set
        {
            m_CurChildWindow = value;
        }
    }
    private WindowBase m_CurChildWindow;

    private WindowBase[] m_HideWindow;

    public bool SetCurrentWin
	{
		get{return SetToCurrentWin;}
	}

	private  bool m_isOpened = false;
	public bool IsOpened
	{
		get{ return m_isOpened; }
	}

    public void SetCloseType(CloseWinType closeEvent)
    {
        mViewBase.CloseEvent = closeEvent;
    }

    public void SetHideWindow(WindowBase[] window)
    {
        m_HideWindow = window;
        if (window == null) return;         
        foreach (WindowBase win in window)
        {
            win.CloseWindow();
        }
    }


    public void InitView() {
        if (mViewBase == null) mViewBase = gameObject.GetComponent<WindowView>();
        if (mCanvas == null) mCanvas = gameObject.GetComponent<Canvas>();
        if (mRaycaster == null) mRaycaster = gameObject.GetComponent<GraphicRaycaster>();
    }

    private Canvas mCanvas;
    private GraphicRaycaster mRaycaster;
    public bool IsActiveShow;
    public void SetEnable(bool isActive)
    {
        //if (!gameObject.activeSelf) gameObject.SetActive(true);
        //if (mCanvas != null)
        //{
        //    mCanvas.enabled = isActive;
        //    this.enabled = isActive;
        //    if (mRaycaster != null) mRaycaster.enabled = isActive;
        //}
        //else
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        if (mCanvas != null)
        {
            gameObject.transform.localPosition = isActive ? Vector3.zero : Vector3.up*3000;
            gameObject.layer = LayerMask.NameToLayer(isActive ? "UI" : "Default");
            this.enabled = isActive;
            if (mRaycaster != null) mRaycaster.enabled = isActive;
        }
        else
        {
            gameObject.SetActive(isActive);
        }
        IsActiveShow = isActive;
    }

    public static void SetUI(GameObject obj, bool isActive)
    {
        if (!obj.activeSelf) obj.SetActive(true);
        obj.transform.localPosition = isActive ? Vector3.zero : Vector3.left * 3000;
        obj.layer = LayerMask.NameToLayer(isActive ? "UI" : "Default");
    }


	protected void OpenWin()
	{
	    if (m_isOpened)
	    {
            SetEnable(true);
	        return;
	    }
        enabled = true;
        if (m_OpenAniList.Count > 0)
        {
            foreach (UIAnimationBaseCtrl item in m_OpenAniList)
            {
                if (item!=null) item.ResetByOriginData();
            }
        }
        SetEnable(true);
        if(m_OpenAniList.Count>0)
        {
            foreach (UIAnimationBaseCtrl item in m_OpenAniList)
            {
                if (item != null) item.DoSelfAnimation();  
            }
        }

        //AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.OpenWin, WindowName.ToString()));
        if (SetToCurrentWin && UIRootMgr.Instance != null)
		{
            if (UIRootMgr.Instance.CurOpenWindow != null && !m_staticDepth)
            {
                //SetPanelDepth(UIRootMgr.Instance.CurOpenWindow.GetPanelDepth() + 1);
                transform.localPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, 0 );//3DUI
            }
            if (UIRootMgr.Instance.CurOpenWindow != this && SetCurrentWin)
            {
                UIRootMgr.Instance.CurOpenWindow = this;
            }
           
        }
        if (Depth == DepthType.TopBattleWindow)
        {
            //UIRootMgr.Instance.LobbyCanvas.gameObject.layer = LayerMask.NameToLayer("Default");
            UIRootMgr.Instance.LobbyCanvas.alpha = 0;
        }
	    transform.SetAsLastSibling();
		DoOpenEvent();
	    UIRootMgr.Instance.AddOpenWindowList(this);
		
        if (LobbySceneMainUIMgr.Instance != null)
            LobbySceneMainUIMgr.Instance.CheckLobbyUI();
        m_isOpened = true;
        FreshInfo();
	}

    /// <summary>
    /// 用于当窗口被隐藏后，直接调用OpenWin打开时窗口时。重写此方法来刷新界面
    /// </summary>
    public virtual void FreshInfo()
    {

    }

    /// <summary>
    /// 禁止重写该方法时，含有打开其他窗口方法
    /// </summary>
    /// <param name="actionType"></param>
	public virtual void  CloseWindow(CloseActionType actionType = CloseActionType.None)
	{    
        if(m_isOpened)
		{
            if (Depth == DepthType.TopBattleWindow)
            {
                //UIRootMgr.Instance.LobbyCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
                UIRootMgr.Instance.LobbyCanvas.alpha = 1;
            }
			m_isOpened = false;
		    UIRootMgr.Instance.TryResetTopTrans(WindowName);
            if (UIRootMgr.Instance.CurOpenWindow != null && UIRootMgr.Instance.CurOpenWindow.Equals(this))
            {
                UIRootMgr.Instance.CurOpenWindow = null;            
            }
           
            UIRootMgr.Instance.RemoveWindowInOpen(this);
            UIRootMgr.Instance.FreshWindowInDisable(this);
            if (LobbySceneMainUIMgr.Instance != null )
                LobbySceneMainUIMgr.Instance.CheckLobbyUI();
            DoCloseEvent();
            if (actionType != CloseActionType.NotCloseChild)
            {
                for (int i = 0; i < m_ChildWindow.Count; i++)
                {
                    m_ChildWindow[i].CloseWindow();
                }
                m_ChildWindow.Clear();
            }  
            m_CurChildWindow = null;
            if (actionType == CloseActionType.OpenHide)
            {
                if (m_HideWindow != null)
                {
                    foreach (WindowBase win in m_HideWindow)
                    {
                        win.OpenWin();
                    }                
                    m_HideWindow = null;
                }
            }
		    //if (SetCurrentWin)
		    {
                if (Window_Guide.IsGuiding && WindowName != WinName.Window_ExitGame) //如果当前有引导且不是强制引导
                {
                    GuideStep guideStep = GuideStep.GuideStepFetcher.GetGuideStepNoCopy(Window_Guide.Instance.CurGuideStep);
                    if (guideStep != null && (guideStep.MyMaskStatus == GuideStep.MaskStatus.NoMask || guideStep.MyMaskStatus== GuideStep.MaskStatus.NoMaskFinishByOut))
                    {
                        Window_Guide.Instance.CloseGuide(false);
                    }
                }
		    }
		}
	}
	private void DoOpenEvent()
	{
	    SetEnable(true);
	}

	private void DoCloseEvent()
	{
	    if (this && gameObject)
	        SetEnable(false);
		m_isOpened = false;
		switch(CloseEvent)
		{
		case CloseWinType.Disable:
            enabled = false;
			break;
		case CloseWinType.Destroy:
		    BeforeDestroy();
			Destroy(gameObject);
			Resources.UnloadUnusedAssets();
			break;
		}
    }

    public virtual void FreshBadge() //刷新界面的红点，和红点信息
    { }
    public virtual void FreshBadgeInfo() //刷新红点信息
    { }

    /// <summary>
    /// 得到在窗口中的显示排序
    /// </summary>
    /// <returns></returns>
	public int GetWindowDepth()  
	{
		return transform.GetSiblingIndex();        
	}

    public void SetWindowTop( )
    {
        if( m_staticDepth )
            return;
        transform.SetAsLastSibling( );
    }
    public void RegisterNetCodeHandler(NetCode_S netcode, ServPacketHander hander)
    {
        if (mServPackHanderDic == null) mServPackHanderDic = new Dictionary<NetCode_S, ServPacketHander>();
        if (!mServPackHanderDic.ContainsKey(netcode))
        {
            mServPackHanderDic.Add(netcode, hander);
            GameClient.Instance.RegisterNetCodeHandler(netcode, hander);
        }
        else
        {
            mServPackHanderDic[netcode] = hander;
            GameClient.Instance.RegisterNetCodeHandler(netcode, hander);
        }
    }
    /// <summary>
    /// 打开切页型子窗口，完成子窗口之间的切换，跟随大窗口关闭
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="closeEvent"></param>
    /// <returns></returns>
    public T OpenChildTabWindow<T>(WinName name,CloseUIEvent closeEvent) where T : WindowBase
    {
        T window = UIRootMgr.Instance.OpenWindow<T>(name, closeEvent);
        if (!m_ChildWindow.Contains(window)) { m_ChildWindow.Add(window); }
        if (m_CurChildWindow != null) m_CurChildWindow.CloseWindow();
        m_CurChildWindow = window;
        return window;
    }
  
    public T GetAsset<T>(TLoader loader)where T:Object
    {
        if (loader == null) return null;
        bool isNew = true;
        for (int i = 0; i < mAssetLoaderList.Count; i++)
        {
            if (mAssetLoaderList[i] == null) continue;
            if (mAssetLoaderList[i].Url == loader.Url) //已经引用过了，因此取消一次引用数
            {
                isNew = false;
                loader.Release();
            }
        }
        if (isNew)
            mAssetLoaderList.Add(loader);
        return loader.ResultObjTo<T>();
    }

    public void BeforeDestroy()//当窗口被destory之前会调用这个
    {
        for (int i = 0; i < mAssetLoaderList.Count; i++)
        {
            if (mAssetLoaderList[i] == null) continue;
            mAssetLoaderList[i].Release();
        }
        if (mServPackHanderDic != null)
        {
            foreach (var item in mServPackHanderDic)
            {
                GameClient.Instance.RegisterNetCodeHandler(item.Key, null);
            }
            mServPackHanderDic = null; 
        }
    }

}


/// <summary>
/// UI扩展方法
/// </summary>
public static class UIExtend
{
    public static T CheckAddComponent<T>(this GameObject obj) where T : Behaviour
    {
        T t = obj.GetComponent<T>();
        if (t == null)
        {
            //string typeName = typeof(T).Name;
            //TDebug.Log(typeName);
            //if (DllMgr.TypeDict.ContainsKey(typeName))
            //{
            //    Component c = obj.AddComponent(DllMgr.GameAssembly.GetType(typeName));
            //    t = obj.GetComponent<T>();
            //}
            //else
            {
                t = obj.AddComponent<T>();
            }
        }
        return t;
    }
    public static void SetListener(this Button.ButtonClickedEvent evt, UnityEngine.Events.UnityAction del)
    {
        evt.RemoveAllListeners();
        evt.AddListener(del);
    }

    /// <summary>
    /// 慎用，否则可能一个按钮会有多次相同回调
    /// </summary>
    public static void AddListener(this Button.ButtonClickedEvent evt, UnityEngine.Events.UnityAction del)
    {
        evt.AddListener(del);
    }

    public static void SetOnClick(this Button btn, UnityEngine.Events.UnityAction del)
    {
        btn.onClick.RemoveAllListeners();
        if (del != null)
        {
          //  btn.onClick.AddListener(AudioMgr.PlayClickAudio);
            btn.onClick.AddListener(del);           
        }
    }

    public static void SetOnAduioClick(this Button btn, UnityEngine.Events.UnityAction del)
    {
        btn.onClick.RemoveAllListeners();
        if (del != null)
        {
            btn.onClick.AddListener(AudioMgr.PlayClickAudio);
            btn.onClick.AddListener(del);
        }
    }
}