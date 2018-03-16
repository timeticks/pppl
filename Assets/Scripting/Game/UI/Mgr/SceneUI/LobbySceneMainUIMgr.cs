using UnityEngine;
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
        public Text TextGoldNum;
        public Text TextDiamondNum;
        public NumScrollTool ScollToolLevel;
        public Button BtnSetting;
        public GameObject Panel_Battle;
        public TextButton TBtnAdvent;
        public TextButton TBtnRole;
        public TextButton TBtnInventory;
        public TextButton TBtnActivity;
        public TextButton TBtnMenu;
        public TextButton TBtnMapChoose;
        public Transform SubBtnListRoot;
        public Transform SubBtnListPanel;
        public TextButton TBtnSub0;
        public TextButton TBtnSub1;
        public TextButton TBtnSub2;
        public TextButton TBtnSub3;
        public TextButton TBtnSub4;
        public List<TextButton> TBtnSubList=new List<TextButton>();
        public Button SubBtnMaskBtn;
        public ViewObj(UIViewBase view)
        {
            if (LobbyTop != null) return;
            LobbyTop = view.GetCommon<GameObject>("LobbyTop");
            TextGoldNum = view.GetCommon<Text>("TextGoldNum");
            TextDiamondNum = view.GetCommon<Text>("TextDiamondNum");
            ScollToolLevel = view.GetCommon<NumScrollTool>("ScollToolLevel");
            BtnSetting = view.GetCommon<Button>("BtnSetting");
            Panel_Battle = view.GetCommon<GameObject>("Panel_Battle");
            TBtnAdvent = view.GetCommon<TextButton>("TBtnAdvent");
            TBtnRole = view.GetCommon<TextButton>("TBtnRole");
            TBtnInventory = view.GetCommon<TextButton>("TBtnInventory");
            TBtnActivity = view.GetCommon<TextButton>("TBtnActivity");
            TBtnMenu = view.GetCommon<TextButton>("TBtnMenu");
            //TBtnMapChoose = view.GetCommon<TextButton>("TBtnMapChoose");
            SubBtnListRoot = view.GetCommon<Transform>("SubBtnListRoot");
            SubBtnListPanel = view.GetCommon<Transform>("SubBtnListPanel");
            TBtnSub0 = view.GetCommon<TextButton>("TBtnSub0");
            TBtnSub1 = view.GetCommon<TextButton>("TBtnSub1");
            TBtnSub2 = view.GetCommon<TextButton>("TBtnSub2");
            TBtnSub3 = view.GetCommon<TextButton>("TBtnSub3");
            TBtnSub4 = view.GetCommon<TextButton>("TBtnSub4");
            TBtnSubList.Add(TBtnSub0); TBtnSubList.Add(TBtnSub1);
            TBtnSubList.Add(TBtnSub2); TBtnSubList.Add(TBtnSub3);
            TBtnSubList.Add(TBtnSub4);

            LobbyUI = view.GetCommon<CanvasGroup>("LobbyUI");
            SubBtnMaskBtn = view.GetCommon<Button>("SubBtnMaskBtn");
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
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.GetLoot, S2C_GetLoot);
    }

    public void Init()
    {
        mViewObj.Panel_Battle.CheckAddComponent<Panel_Battle>().Init(0);
        //BattleMgr.Instance.Init();
        //gameObject.CheckAddComponent<GuideMgr>().Init();

        ////将这些界面放在置顶窗口中，并且添加到关联删除列表
        //mViewObj.LobbyTop.transform.SetParent(UIRootMgr.Instance.m_Root.m_TopLobbyRoot.transform);
        //m_RefList.Add(mViewObj.LobbyTop.gameObject);

        mViewObj.TBtnAdvent.SetOnClick(delegate() { OpenSubBtnPanel(SubBtnType.Adventure); });
        mViewObj.TBtnRole.SetOnClick(delegate() { OpenSubBtnPanel(SubBtnType.Role); });
        mViewObj.TBtnInventory.SetOnClick(delegate() { OpenSubBtnPanel(SubBtnType.Inventory); });
        mViewObj.TBtnActivity.SetOnClick(delegate() { OpenSubBtnPanel(SubBtnType.Activity); });
        mViewObj.TBtnMenu.SetOnClick(delegate() { OpenSubBtnPanel(SubBtnType.Menu); });
        mViewObj.SubBtnMaskBtn.SetOnClick(delegate() { CloseSubBtnPanel(); });
        CloseSubBtnPanel();

        mViewObj.TBtnRole.SetOnClick(delegate() { BtnEvt_OpenRoleInfo(); });
        //mViewObj.TBtnMapChoose.SetOnClick(BtnEvt_OpenMap);
        //mViewObj.BtnWorldChoose.onClick.SetListenr(delegate() { BteEvt_OpenWorld(); });
        //mViewObj.BtnCave.onClick.SetListenr(delegate() { BtnEvt_OpenCave(); });
        //mViewObj.BtnHonor.SetOnClick(delegate() { BtnEvt_OpenHonor(); });
        //mViewObj.BtnActivity.SetOnClick(delegate() { BtnEvt_OpenActivity(); });
        //mViewObj.BtnShop.SetOnClick(delegate() { BtnEvt_OpenShop(); });
        //mViewObj.IconHeadBtn.SetOnClick(BtnEvt_OpenMail);

        ////设置
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_CaveBtn,         mViewObj.BtnCave.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_InventoryBtn,    mViewObj.BtnAssem.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_WorldBtn,        mViewObj.BtnWorldChoose.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_RoleInfoBtn,     mViewObj.BtnRoleInfo.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_RankBtn,         mViewObj.BtnHonor.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_ShopBtn,         mViewObj.BtnShop.transform);
        //GuideMgr.Instance.SetUI(GuidePointUI.Lobby_ActivityBtn,     mViewObj.BtnActivity.transform);

        //FreshLobbyInfo();

        //ShowMainUI();
        //TDebug.Log("//TODO: 先屏蔽新手秘境");
        //bool canSkip = false;

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
        mViewObj.SubBtnListRoot.gameObject.SetActive(true);
        mViewObj.SubBtnListPanel.gameObject.SetActive(true);
        for (int i = 0; i < mViewObj.TBtnSubList.Count; i++)
        {
            mViewObj.TBtnSubList[i].gameObject.SetActive(false);
        }
        switch (ty)
        {
            case SubBtnType.Adventure:
            {
                mViewObj.SubBtnListRoot.transform.localPosition =
                    new Vector3(mViewObj.TBtnAdvent.transform.localPosition.x, mViewObj.SubBtnListRoot.transform.localPosition.y, 0);

                for (int i = 0; i < 2; i++)
                {
                    mViewObj.TBtnSubList[i].gameObject.SetActive(true);
                }
                mViewObj.TBtnSubList[0].TextBtn.text = "地图";

                mViewObj.TBtnSubList[1].TextBtn.text = "任务";
                break;
            }
            case SubBtnType.Role:
            {
                mViewObj.SubBtnListRoot.transform.localPosition =
                    new Vector3(mViewObj.TBtnRole.transform.localPosition.x, mViewObj.SubBtnListRoot.transform.localPosition.y, 0);

                mViewObj.TBtnSubList[0].SetOnClick(BtnEvt_OpenRoleInfo);
                break;
            }
            case SubBtnType.Inventory:
            {
                mViewObj.SubBtnListRoot.transform.localPosition =
                    new Vector3(mViewObj.TBtnInventory.transform.localPosition.x, mViewObj.SubBtnListRoot.transform.localPosition.y, 0);

                for (int i = 0; i < 3; i++)
                {
                    mViewObj.TBtnSubList[i].gameObject.SetActive(true);
                }
                mViewObj.TBtnSubList[0].TextBtn.text = "装备";
                mViewObj.TBtnSubList[0].SetOnClick(delegate() { GetWindowAndCloseSubPanel<Window_AssemEquip>(WinName.Window_AssemEquip).OpenWindow();});
                
                mViewObj.TBtnSubList[1].TextBtn.text = "道具";
                mViewObj.TBtnSubList[2].TextBtn.text = "技能";
                break;
            }
            case SubBtnType.Activity:
            {
                mViewObj.SubBtnListRoot.transform.localPosition =
                       new Vector3(mViewObj.TBtnActivity.transform.localPosition.x, mViewObj.SubBtnListRoot.transform.localPosition.y, 0);

                break;
            }
            case SubBtnType.Menu:
            {
                mViewObj.SubBtnListRoot.transform.localPosition =
                       new Vector3(mViewObj.TBtnMenu.transform.localPosition.x, mViewObj.SubBtnListRoot.transform.localPosition.y, 0);

                break;
            }
        }
    }
    public void CloseSubBtnPanel()
    {
        mViewObj.SubBtnListPanel.gameObject.SetActive(false);
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
        //暂时不进行隐藏
        return;
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
        //新手背包
        //AppEvtMgr.Instance.Register(new EvtItemData(EvtType.FinishTravelEvent, GameConstUtils.inventory_badge_event.ToString()), EvtListenerType.InventoryBadgeOpen,
        //    delegate(object o)
        //    {
        //        TDebug.Log(" GameConstUtils.InventoryBadgeEvent"); SaveUtils.SetIntInPlayer(EvtListenerType.InventoryBadgeOpen.ToString(), 1);
        //    });

        //GuideMgr.Instance.Fresh(GuidePointUI.Lobby_CaveBtn, mViewObj.BtnCave.transform);
        //GuideMgr.Instance.Fresh(GuidePointUI.Lobby_InventoryBtn, mViewObj.BtnAssem.transform);
        //GuideMgr.Instance.Fresh(GuidePointUI.Lobby_WorldBtn, mViewObj.BtnWorldChoose.transform);
        //GuideMgr.Instance.Fresh(GuidePointUI.Lobby_RoleInfoBtn, mViewObj.BtnRoleInfo.transform);
        //GuideMgr.Instance.Fresh(GuidePointUI.Lobby_RankBtn, mViewObj.BtnHonor.transform);
        //GuideMgr.Instance.Fresh(GuidePointUI.Lobby_ShopBtn, mViewObj.BtnShop.transform);
        //GuideMgr.Instance.Fresh(GuidePointUI.Lobby_ActivityBtn, mViewObj.BtnActivity.transform);        
    }



    //显示掉落
    public void S2C_GetLoot(BinaryReader ios)
    {
        NetPacket.S2C_GetLoot msg = MessageBridge.Instance.S2C_GetLoot(ios);
        string lootStr = "";
        switch (msg.ActionType)
        {
            case Loot.LootActionType.NpcLoot:
                {
                    OldHero npc = OldHero.HeroFetcher.GetHeroByCopy(msg.NpcId);
                    if (npc != null) lootStr = string.Format(LangMgr.GetText("你击败了{0}"), npc.name);
                    break;
                }
            case Loot.LootActionType.Tower:
                {
                    Tower tower = Tower.TowerFetcher.GetTowerByCopy(msg.NpcId);
                    if (tower != null) lootStr = string.Format(LangMgr.GetText("你通过了{0}"), tower.name);
                    break;
                }
            default:
                TDebug.LogError("未处理的掉落类型");
                break;
        }
        if (LobbySceneMainUIMgr.Instance != null)
            LobbySceneMainUIMgr.Instance.ShowDropInfo(msg.GoodsList, lootStr);
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
        return;
        if (Time.frameCount % 1000 == 0 && UIRootMgr.Instance != null)
        {
            FreshLobbyInfo();
        }
        if (Time.frameCount%30 == 0)
        {
            //if (UIRootMgr.Instance != null)
            //{
            //    FreshTopRoleInfo();
            //}
            int tipsLevel = SaveUtils.GetIntInPlayer(EvtListenerType.CanRetreat.ToString());
            if (PlayerPrefsBridge.Instance.PlayerData.Level >= tipsLevel
                && HeroLevelUp.CanRetreat(PlayerPrefsBridge.Instance.PlayerData.Level, PlayerPrefsBridge.Instance.PlayerData.Exp)
                && PlayerPrefsBridge.Instance.GetCurRetreatStartTime() <= 0
                && Window_BattleTowSide.Instance == null
                )
            {
                if (PlayerPrefsBridge.Instance.PlayerData != null && PlayerPrefsBridge.Instance.PlayerData.Level >= tipsLevel)
                {
                    SaveUtils.SetIntInPlayer(EvtListenerType.CanRetreat.ToString(), PlayerPrefsBridge.Instance.PlayerData.Level + 1); //提示完后加1级
                    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LobbyDialogue.GetDescStr("tips_retreat"), Color.red); //闭关提示
                }
            }
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
        mViewObj.ScollToolLevel.Fresh(expPct, expStr, PlayerPrefsBridge.Instance.PlayerData.Level.ToString());
        //mViewObj.TextState.text = HeroLevelUp.GetStateName(PlayerPrefsBridge.Instance.PlayerData.Level);
        //mViewObj.TextLevel.text = string.Format("{0}级", PlayerPrefsBridge.Instance.PlayerData.Level);
        FreshTopRoleInfo();
    }

    public void FreshTopRoleInfo()
    {
        mViewObj.TextGoldNum.text = TUtility.GetMoneyString(PlayerPrefsBridge.Instance.PlayerData.Gold);
        mViewObj.TextDiamondNum.text = TUtility.GetMoneyString(PlayerPrefsBridge.Instance.PlayerData.Diamond);
        //mViewObj.TextPoten.text = TUtility.GetMoneyString(PlayerPrefsBridge.Instance.PlayerData.Potential);
    }

    public void ShowDropInfo(GoodsToDrop[] DropGoods , string dropReason="")
    {
        if (dropReason != "")
        {
            UIRootMgr.LobbyUI.AppendTextNewLine(dropReason);
        }

        if (DropGoods == null) return;
        for (int i = 0; i < DropGoods.Length; i++)
        {
            GoodsToDrop drop = DropGoods[i];
            if (drop == null) continue;
            string dropMsg = string.Format("获得 {0}×{1}", TUtility.TryGetLootGoodsName(drop.MyType, drop.GoodsIdx), drop.Amount);
            UIRootMgr.LobbyUI.AppendTextNewLine(dropMsg);
            AppendLobbyTips(dropMsg);
        }
    }
    public void BtnEvt_LuaWindow()
    {
        //UIRootMgr.Instance.OpenLuaWindow("LuaWindow_Test", CloseUIEvent.CloseCurrent).OpenWindow();
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
    public void BtnEvt_OpenAssem()
    {
        SaveUtils.SetIntInPlayer(ModuleType.module_inventory.ToString(), BadgeStatus.Normal.ToInt());
        UIRootMgr.Instance.OpenWindow<WindowBig_Assem>(WinName.WindowBig_Assem).OpenWindow();
    }
    public void BteEvt_OpenWorld()
    {
        //UIRootMgr.Instance.OpenWindow<WindowBig_WorldChoose>(WinName.WindowBig_WorldChoose).OpenWindow();
    }
    public void BtnEvt_OpenCave()
    {
        SaveUtils.SetIntInPlayer(ModuleType.module_cave.ToString(), BadgeStatus.Normal.ToInt());
        //UIRootMgr.Instance.OpenWindow<WindowBig_CaveChoose>(WinName.WindowBig_CaveChoose).OpenWindow();
    }
    public void BtnEvt_OpenActivity()
    {
        SaveUtils.SetIntInPlayer(ModuleType.module_activity.ToString(), BadgeStatus.Normal.ToInt());

        //if (PlayerPrefsBridge.Instance.PlayerData.IsSetName)
        //    UIRootMgr.Instance.OpenWindow<WindowBig_Activity>(WinName.WindowBig_Activity).OpenWindow();
        //else
        //    UIRootMgr.Instance.OpenWindow<Window_CreatName>(WinName.Window_CreatName).OpenWindow();
    }
    public void BtnEvt_OpenShop()
    {
        SaveUtils.SetIntInPlayer(ModuleType.module_shop.ToString(), BadgeStatus.Normal.ToInt());

        UIRootMgr.Instance.OpenWindow<WindowBig_Store>(WinName.WindowBig_Store).OpenWindow();
        //UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("暂未开放"), Color.red);
    }


    public void AppendTextNewLine(string str , bool isClear=false)
    {
        //if (isClear)
        //{
        //    mViewObj.DescScroll.AppendText("");
        //    mViewObj.DescScroll.MoveBottom();
        //    return;
        //}
            
        //mViewObj.DescScroll.AppendTextNewLine(str);
        //mViewObj.DescScroll.MoveBottom();
    }
    public void AppendTextNewLine(string str,int color, bool isClear = false)
    {
        //if (isClear)
        //    mViewObj.DescScroll.AppendText("");
        //mViewObj.DescScroll.AppendTextNewLine(TUtility.GetColorText(str, color));
        //mViewObj.DescScroll.MoveBottom();
    }

    public void AppendLobbyTips(string tips)
    {
        mShowTipList.Add(tips);
    }
    public void AppendAchieveTips(Achieve achieve)
    {
        mShowGetAchieveList.Add(achieve);
    }


    public void ShowOffLineMsg(byte type)
    {
        //TDebug.Log("服务器离线显示");
        //打坐
        ExerciseAccessor exercise = PlayerPrefsBridge.Instance.GetExercise();
        //if (exercise.OffLineInfo)
        //{
        //    long maxSitTime = 8 * 3600 * 1000;
        //    int itemAdditon = 0;
        //    if (exercise.OffLineUseItem > 0)
        //    {
        //        Item item = Item.ItemFetcher.GetItemByCopy(PlayerPrefsBridge.Instance.GetExercise().RetreatUseItemIdx);
        //        if (item != null && item.EffectType == Item.ItemEffectType.Zazen)
        //            itemAdditon = item.EffectMisc1[0];
        //    }
        //    Cave cave = Cave.CaveFetcher.GetCaveByCopy(PlayerPrefsBridge.Instance.PlayerData.CaveLevel);
        //    HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(PlayerPrefsBridge.Instance.PlayerData.Level);

        //    int perExp = TUtility.GetZazenIncome(levelUp.AutoExp, cave.BaseExp, cave.PercentExp, itemAdditon, PlayerPrefsBridge.Instance.PlayerData.Hero.GetLuk());
        //    int perPotential = TUtility.GetZazenIncome(levelUp.AutoPotential, cave.BasePotential, cave.PercentPotential, itemAdditon, PlayerPrefsBridge.Instance.PlayerData.Hero.GetLuk());

        //    long incomeExp = perExp * maxSitTime.ToInt_1000() / 60;
        //    long incomePoten = perPotential * maxSitTime.ToInt_1000() / 60;

        //    UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("你通过打坐获得了{0}经验{1}潜能", incomeExp, incomePoten), 3); 
        //    PlayerPrefsBridge.Instance.ClearExrciseOffLineInfo();
        //}
        if (exercise.PotentialOffLine > 0 || exercise.ExpOffLine > 0)
        {
            UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("你通过打坐获得了{0}经验{1}潜能", exercise.ExpOffLine, exercise.PotentialOffLine), 3); 
            PlayerPrefsBridge.Instance.ClearExrciseOffLineInfo();
        }

       
        //游历
        TravelAccessor travel = PlayerPrefsBridge.Instance.GetTravel();
        if (travel.ExpOffLine>0||travel.PotentialOffLine>0||travel.GoldOffLine>0)
        {
            UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("离线期间游历获得{0}经验、{1}潜能、{2}灵石", travel.ExpOffLine, travel.PotentialOffLine, travel.GoldOffLine), 3);
            PlayerPrefsBridge.Instance.ClearTravelOffLineInfo();
        }
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
