﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
public class LobbySceneMainUIMgr : BaseMainUIMgr
{
    public static LobbySceneMainUIMgr Instance { get; private set; }
    private UIViewBase mView;

    public class ViewObj
    {
        public GameObject LobbyTop;
        public CanvasGroup LobbyUI; 
        public Text TextGold;
        public Text TextDiamond;
        public ViewObj(UIViewBase view)
        {
            if (LobbyTop != null) return;
            LobbyTop = view.GetCommon<GameObject>("LobbyTop");
            LobbyUI = view.GetCommon<CanvasGroup>("LobbyUI");
            TextGold = view.GetCommon<Text>("TextGold");
            TextDiamond = view.GetCommon<Text>("TextDiamond");
        }
    }

    private ViewObj mViewObj;
    private List<string> mShowTipList = new List<string>();
    private List<Achieve> mShowGetAchieveList = new List<Achieve>();
    public float mShowTipsDetla = 0.8f;
    private float mCurShowTipsTime = 0;
    public SpritePrefab HeadIconAtlas
    {
        get
        {
            if (mHeadIconAtlas == null)
            {
                GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("HeadIconAtlas");
                mHeadIconAtlas = go.GetComponent<SpritePrefab>();
            }
            return mHeadIconAtlas;
        }
    }
    private SpritePrefab mHeadIconAtlas;

    public override void _Init()
    {
        base.init();
    }

    void Awake()
    {
        Instance = this;
        mView = GetComponent<UIViewBase>();
        if (mViewObj == null) mViewObj = new ViewObj(mView);
    }

    void Start()
    {
        UIRootMgr.LobbyUI.Init();
        UIRootMgr.Instance.MyUICam.enabled = true;
    }

    public void Init()
    {
        //BattleMgr.Instance.Init();
        //gameObject.CheckAddComponent<GuideMgr>().Init();

        ////将这些界面放在置顶窗口中，并且添加到关联删除列表
        //mViewObj.LobbyTop.transform.SetParent(UIRootMgr.Instance.m_Root.m_TopLobbyRoot.transform);
        //m_RefList.Add(mViewObj.LobbyTop.gameObject);

        CloseSubBtnPanel();

        ////设置
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_CaveBtn,         mViewObj.BtnCave.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_InventoryBtn,    mViewObj.BtnAssem.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_WorldBtn,        mViewObj.BtnWorldChoose.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_RoleInfoBtn,     mViewObj.BtnRoleInfo.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_RankBtn,         mViewObj.BtnHonor.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_ShopBtn,         mViewObj.BtnShop.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_ActivityBtn,     mViewObj.BtnActivity.transform);

        PlayerPrefsBridge.Instance.LoadPlayer();
        if (PVEMgr.Instance == null)
        {
            gameObject.CheckAddComponent<PVEMgr>().Init(30001);
        }
        //else PVEMgr.Instance.SwitchMap(30001);
    }

    public enum SubBtnType
    {
        Adventure,
        Role,
        Inventory,
        Activity,
        Menu,

    }



    public void OpenSubBtnPanel(SubBtnType ty)
    {
    }
    public void CloseSubBtnPanel()
    {
    }

    public T GetWindowAndCloseSubPanel<T>(WinName winName) where T : WindowBase
    {
        CloseSubBtnPanel();
        return (T)UIRootMgr.Instance.OpenWindow<T>(winName, CloseUIEvent.None);
    }



    //设置按钮是否解锁
    public void LockLobbyBtn(GuidePointUI pointUi, bool isLock)
    {
        switch (pointUi)
        {
            case GuidePointUI.Lobby_ActivityBtn:
            {
                //mViewObj.BtnActivity.enabled = !isLock;
                //mViewObj.TextBtnActivity.SetActive(!isLock);
                //mViewObj.LimitActivity.SetActive(isLock);
                //mViewObj.BtnActivity.image.overrideSprite = isLock ? mViewObj.BtnActivity.spriteState.disabledSprite : null;
                break;
            }
        }
        
       
    }


    public void CheckLobbyUI()
    {
        bool haveWindow = false;
        if (UIRootMgr.Instance == null || mViewObj == null || mViewObj.LobbyUI == null) return;
        for (int i = 0; i < UIRootMgr.Instance.OpenWinList.Count; i++)
        {
            if (UIRootMgr.Instance.OpenWinList[i].SetCurrentWin)
                haveWindow = true;
        }
        mViewObj.LobbyUI.alpha = haveWindow ? 0 : 1;
        mViewObj.LobbyUI.interactable = !haveWindow;
    }

    public override void ShowMainUI()//重新显示主界面 
    {
        FreshLobbyInfo();    
    }


    public void SetLobbyUI(bool open)
    {
        //mViewObj.LobbyUI.SetActive(open);
    }
    public void SetDownRoot(bool open)
    {
        //mViewObj.DownRoot.SetActive(open);
    }
    void Update()
    {
        if (Time.frameCount % 1000 == 0 && UIRootMgr.Instance != null)
        {
            FreshLobbyInfo();
        }
        //显示获得物品的tips,解锁的成就，战斗中不显示
        mCurShowTipsTime += Time.deltaTime;
        if (Window_BattleTowSide.Instance == null)
        {
            if (mShowTipList.Count > 0 && mCurShowTipsTime > mShowTipsDetla)
            { 
                mCurShowTipsTime = 0;
                UIRootMgr.Instance.Window_UpTips.InitTips(mShowTipList[0], Color.green);
                mShowTipList.RemoveAt(0);
            }
            if (mShowGetAchieveList.Count > 0)
            {
                UIRootMgr.Instance.Window_UpTips.AddAchieveTips(mShowGetAchieveList[0]);
                mShowGetAchieveList.RemoveAt(0);
            }
        }
    }

    public void FreshLobbyInfo()
    {
        if (PlayerPrefsBridge.Instance.PlayerData == null || mViewObj == null) return;
        //mViewObj.IconHeadBtn.image.sprite = HeadIconAtlas.GetSprite(HeadIcon.GetHeadIcon(PlayerPrefsBridge.Instance.PlayerData.HeadIconIdx, PlayerPrefsBridge.Instance.PlayerData.Sex));

        //mViewObj.NameText.text = PlayerPrefsBridge.Instance.PlayerData.name;
        long curTime = AppTimer.CurTimeStampMsSecond;
        long originTime = AppTimer.OriginTime;
        int offsetTime = (int)((curTime - originTime) / 1000);
        int year = offsetTime / (24 * 60 * 60);
        int month =(offsetTime-(24*60*60*year))/(2*60*60);
        long birthTime = PlayerPrefsBridge.Instance.PlayerData.birthTime;
        long LoginTime = (curTime - birthTime) / 1000 / 24 / 60 / 60;
        string expStr = string.Format("{0}/{1}",
           PlayerPrefsBridge.Instance.PlayerData.Exp - HeroLevelUp.GetLevelAmountExp(PlayerPrefsBridge.Instance.PlayerData.Level-1),
           HeroLevelUp.GetCurLevelExp(PlayerPrefsBridge.Instance.PlayerData.Level));
        float expPct = PlayerPrefsBridge.Instance.PlayerData.Exp -
                       HeroLevelUp.GetLevelAmountExp(PlayerPrefsBridge.Instance.PlayerData.Level - 1)/
                       (float) HeroLevelUp.GetCurLevelExp(PlayerPrefsBridge.Instance.PlayerData.Level);
        FreshTopRoleInfo();
    }

    public void FreshTopRoleInfo()
    {
        mViewObj.TextGold.text = TUtility.GetMoneyString(PlayerPrefsBridge.Instance.PlayerData.Gold);
        mViewObj.TextDiamond.text = TUtility.GetMoneyString(PlayerPrefsBridge.Instance.PlayerData.Diamond);
    }

    public void ShowDropInfo(GoodsToDrop[] DropGoods , string dropReason="")
    {
    }

    public void AppendTextNewLine(string str)
    {
    }

    public void BtnEvt_OpenRoleInfo()
    {
     //   TDebug.LogError("角色信息暂时频闭");
        WindowBig_RoleInfo win = UIRootMgr.Instance.GetOpenListWindow<WindowBig_RoleInfo>(WinName.WindowBig_RoleInfo);
        if (win == null)
            UIRootMgr.Instance.OpenWindow<WindowBig_RoleInfo>(WinName.WindowBig_RoleInfo).OpenWindow();
        else
            win.BtnEvt_Exit();
    }

    public void BtnEvt_ItemInventory()
    {
        WindowBig_Assem win = UIRootMgr.Instance.GetOpenListWindow<WindowBig_Assem>(WinName.WindowBig_Assem);
        if (win == null)
            UIRootMgr.Instance.OpenWindow<WindowBig_Assem>(WinName.WindowBig_Assem).OpenWindow();
        else
            win.BtnEvt_Exit();
        //UIRootMgr.Instance.OpenWindow<Window_ItemInventory>(WinName.Window_ItemInventory, CloseUIEvent.None).OpenWindow();
    }


    public void BtnEvt_OpenMail()
    {
        UIRootMgr.Instance.OpenWindow<Window_Mail>(WinName.Window_Mail,CloseUIEvent.None).OpenWindow();
    }
    public void BtnEvt_OpenShop()
    {
        SaveUtils.SetIntInPlayer(ModuleType.module_shop.ToString(), BadgeStatus.Normal.ToInt());

        UIRootMgr.Instance.OpenWindow<WindowBig_Store>(WinName.WindowBig_Store).OpenWindow();
        //UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("暂未开放"), Color.red);
    }

    public void AppendLobbyTips(string tips)
    {
        mShowTipList.Add(tips);
    }

    void OnGUI()
    {
        //if (GUILayout.Button("openLua"))
        //{
        //    luaWin = UIRootMgr.Instance.OpenWindow<Window_Lua>("LuaWindow_Test", CloseUIEvent.None);
        //    luaWin.OpenWindow();
        //}
        //if (GUILayout.Button("      进入战斗"))
        //{
        //    BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Fight, 1201101008,PVESceneType.None , null);
        //}
        //if (GUILayout.Button("      进入战斗"))
        //{
        //    BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Fight, 1201230002, null);
        //}
        //if (GUILayout.Button("      进入战斗"))
        //{
        //    BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Fight, 1201230003, null);
        //}
        //if (GUILayout.Button("      进入手操战斗"))
        //{
        //    BattleMgr.Instance.EnterPVE(BattleType.My_Hand_Fight, 1201221005, PVESceneType.None, null);
        //}
        //if (GUILayout.Button("      返回登录"))
        //{
        //    LoginOutGame();
        //}
        //if (GUILayout.Button("      进入手操战斗"))
        //{
        //    BattleMgr.Instance.EnterPVE(BattleType.My_Hand_Battle, 1201112011, PVESceneType.DungeonMap, null);
        //}
        //if (GUILayout.Button("      刷新角色"))
        //{
        //    float startT = Time.realtimeSinceStartup;
        //    Hero hero = PlayerPrefsBridge.Instance.GetHeroWithProperties();
        //    TDebug.Log(Time.realtimeSinceStartup - startT);
        //}
        //if (GUILayout.Button(" 1111111111111111"))
        //{
        //    UIRootMgr.Instance.IsLoading = true;
        //    GameClient.Instance.RegisterNetCodeHandler(NetCode_S.ShopInfo, S2C_ShopInfo);
        //    GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_ShopInfo(1308000090));
        //}
        //if (GUILayout.Button(" 1111111111111111"))
        //{
        //    UIRootMgr.Instance.OpenWindow<Window_VIP>(WinName.Window_VIP).OpenWindow();
        //}
      
    }

}
