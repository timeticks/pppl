using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Window_DungeonMap : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_DungeonMapItem;
        public Transform NodeRoot;
        public Transform RolePoint;
        public Transform MapRoot;
        public Button BgBtn;
        public GameObject Part_MapNpcItem;
        public Transform NpcRoot;
        public Button ExitBtn;
        public Button PauseExitBtn;
        public Button ClearBtn;
        public Button InventoryBtn;
        public Button EventBtn;
        public Text RoleNameText;
        public Text RoleLevelText;
        public Text RoleLevelStateText;
        public Text MapNameText;
        public Image BgImage;
        public Image IconHead;

        public ViewObj(UIViewBase view)
        {
            Part_DungeonMapItem = view.GetCommon<GameObject>("Part_DungeonMapItem");
            Part_MapNpcItem = view.GetCommon<GameObject>("Part_MapNpcItem");
            NodeRoot = view.GetCommon<Transform>("NodeRoot");
            RolePoint = view.GetCommon<Transform>("RolePoint");
            MapRoot = view.GetCommon<Transform>("MapRoot");
            BgBtn = view.GetCommon<Button>("BgBtn");
            NpcRoot = view.GetCommon < Transform >("NpcRoot");
            PauseExitBtn = view.GetCommon<Button>("PauseExitBtn");
            ExitBtn = view.GetCommon<Button>("ExitBtn");
            ClearBtn = view.GetCommon<Button>("ClearBtn");
            if (InventoryBtn == null) InventoryBtn = view.GetCommon<Button>("InventoryBtn");
            if (EventBtn == null) EventBtn = view.GetCommon<Button>("EventBtn");
            if (RoleNameText == null) RoleNameText = view.GetCommon<Text>("RoleNameText");
            if (RoleLevelText == null) RoleLevelText = view.GetCommon<Text>("RoleLevelText");
            if (RoleLevelStateText == null) RoleLevelStateText = view.GetCommon<Text>("RoleLevelStateText");
            if (MapNameText == null) MapNameText = view.GetCommon<Text>("MapNameText");
            if (BgImage == null) BgImage = view.GetCommon<Image>("BgImage");
            if (IconHead == null) IconHead = view.GetCommon<Image>("IconHead");
        }
    }

    public class NpcItemObj:SmallViewObj
    {
        public Text NameText;
        public Button NpcBtn;

        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (NpcBtn == null) NpcBtn = view.GetCommon<Button>("NpcBtn");
        }
    }

    public class NodeItemObj:SmallViewObj
    {
        public Text NameText;
        public Image IconImage;
        public NodeItemObj(UIViewBase view)
        {
            Init(view);
            NameText = view.GetCommon<Text>("NameText");
            IconImage = view.GetCommon<Image>("IconImage");
        }
    }

    public DungeonMapData MyMapData;
    [HideInInspector] public int LastPveNpc;   //刚刚与之战斗的npc

    private XyCoordRef mRoleXy;        //角色位置
    private XyCoordRef mMapCenterXy;   //地图的中心位置
    private ViewObj mViewObj;
    private List<NodeItemObj> mNodeItemList = new List<NodeItemObj>();
    private List<NpcItemObj> mNpcItemList = new List<NpcItemObj>();
    private SpritePrefab MapSprite;
    private MindTreeNode mCurNpcNode;
    private Window_NpcInteract mInteractWindow;
    private int mMapIdx;

    public void OpenWindow(int mapId)
    {
        if (mapId == 0)
        {
            OpenWin();
            return;
        }
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        if (mMapIdx > 0 && mMapIdx != mapId)  //TODO:如果进入了另一个新秘境
        {
            
        }
        mMapIdx = mapId;
        UIRootMgr.Instance.Window_LoadBar.Fresh(Random.Range(0.1f, 0.4f), "正在进入秘境");
        StopAllCoroutines();
        StartCoroutine("StartAnimation");
        
        mViewObj.BgBtn.SetOnClick(delegate() { BtnEvt_MoveClick(); });

        MapData mapData = MapData.MapDataFetcher.GetMapDataByCopy(mapId);
        if (mapData == null) return;
        bool isNewerMap = mapData.Type == MapData.MapType.NewerMap;

        //if (!isNewerMap) //如果不是新手秘境
        {
            UIRootMgr.Instance.IsLoading = true;
            RegisterNetCodeHandler(NetCode_S.EnterDungeonMap, S2C_EnterDungeonMap);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EnterDungeonMap(mapId));
        }
        //else
        //{
        //    Init();
        //}
        mViewObj.InventoryBtn.gameObject.SetActive(!isNewerMap);
        mViewObj.EventBtn.gameObject.SetActive(!isNewerMap);
        mViewObj.ExitBtn.gameObject.SetActive(!isNewerMap);
        mViewObj.RoleNameText.text = PlayerPrefsBridge.Instance.PlayerData.name;
        mViewObj.RoleLevelStateText.text = isNewerMap ? "凡人" : HeroLevelUp.GetStateName(PlayerPrefsBridge.Instance.PlayerData.Level);
        mViewObj.MapNameText.text = mapData.name;
        mViewObj.RoleLevelText.text = "等级：" + PlayerPrefsBridge.Instance.PlayerData.Level;
        //mViewObj.IconHead.sprite = LobbySceneMainUIMgr.Instance.HeadIconAtlas.GetSprite(HeadIcon.GetHeadIcon(PlayerPrefsBridge.Instance.PlayerData.HeadIconIdx, PlayerPrefsBridge.Instance.PlayerData.Sex));
        if (mapData.Icon != "null" && mapData.Icon!="") mViewObj.BgImage.sprite = GetAsset<Sprite>(SharedAsset.Instance.LoadSpritePart<Sprite>(mapData.Icon));
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        while (UIRootMgr.Instance.IsLoading)  //等待遮罩关闭
            yield return null;
        UIRootMgr.Instance.Window_LoadBar.SetFalse();
    }

    public void S2C_EnterDungeonMap(BinaryReader ios)
    {
        NetPacket.S2C_EnterDungeonMap msg =MessageBridge.Instance.S2C_EnterDungeonMap(ios);
        DungeonMapAccessor map = PlayerPrefsBridge.Instance.GetDungeonMapCopy();
        UIRootMgr.Instance.IsLoading = false;
        OpenWin();
        Init(true);
    }

    void Init(bool isStart)
    {
        if (MapSprite == null)
        {
            GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("DungeonMapAtlas");
            MapSprite = go.GetComponent<SpritePrefab>();
        }


        if (MyMapData == null || isStart)
        {
            MyMapData = new DungeonMapData(mMapIdx);
            MindTreeMap treeData = MindTreeMap.MindTreeMapFetcher.GetMindTreeMapByCopy(MyMapData.MapId);
            MyMapData.TreeData = new MindTreeMapCtrl(treeData, MyMapData.Type);
            MindTreeMapCtrl.CheckLegal(MyMapData.TreeData);
        }
        

        //生成地图格子
        int nodeAmount = MyMapData.Width*MyMapData.Height;
        for (int i = mNodeItemList.Count; i < nodeAmount; i++)
        {
            GameObject g = Instantiate(mViewObj.Part_DungeonMapItem , mViewObj.NodeRoot) as GameObject;
            g.transform.localScale = Vector3.one;
            UIViewBase viewBase = g.GetComponent<UIViewBase>();
            NodeItemObj item = new NodeItemObj(viewBase);
            mNodeItemList.Add(item);
        }

        XyCoordRef xy = null;
        for (int i = 0; i < nodeAmount; i++)//刷新item显示
        {
            xy = MyMapData.GetNodeXyByIndex(i);
            Vector2 pos = MyMapData.GetPos(xy.m_X, xy.m_Y);
            mNodeItemList[i].View.transform.localPosition = new Vector3(pos.x, pos.y, 0);
            mNodeItemList[i].NameText.text = MyMapData.GetTerrainName(i);
            mNodeItemList[i].IconImage.sprite = MapSprite.GetSprite(MyMapData.GetTerrainType(i).ToString());
            mNodeItemList[i].IconImage.material = MapSprite.Mat;
            mNodeItemList[i].View.gameObject.SetActive(true);
        }
        for (int i = nodeAmount; i < mNodeItemList.Count; i++)
        {
            mNodeItemList[i].View.gameObject.SetActive(false);
        }

        XyCoordRef startPos = mRoleXy;
        if (isStart)
        {
            startPos = MyMapData.GetNodeXyByIndex(MyMapData.Entry);
            MyMapData.GetNodeXyByIndex(MyMapData.Entry);
            int savePos = 0;
            if (PlayerPrefsBridge.Instance.GetMapSaveCondition(DungeonMapAccessor.MapSaveType.RolePos, 0, out savePos))
            {
                XyCoordRef savePosXy = MyMapData.GetNodeXyByIndex(savePos);
                if (MyMapData.GetWalkable(savePosXy))
                {
                    startPos = MyMapData.GetNodeXyByIndex(savePos);
                }
            }
            mRoleXy = new XyCoordRef(startPos.m_X, startPos.m_Y);
            mMapCenterXy = new XyCoordRef(startPos.m_X, startPos.m_Y);
            mViewObj.PauseExitBtn.SetOnClick(delegate() { CloseWindow(); });
            mViewObj.ExitBtn.SetOnClick(delegate() { SendMapEnd(MapEndType.PlayerExit, null); });
            mViewObj.ClearBtn.SetOnClick(delegate() { AppendNewLineDecs("", true); });
            mViewObj.EventBtn.SetOnClick(OpenMapEvent);
            mViewObj.InventoryBtn.SetOnClick(OpenInventory);
        }
        RoleMoveTo(startPos, isStart);
        TDebug.Log("刷新完毕");

    }




    void OpenMapEvent()
    {
        UIRootMgr.Instance.OpenWindow<Window_DungeonMapEvent>(WinName.Window_DungeonMapEvent , CloseUIEvent.None).OpenWindow(mMapIdx);
    }

    void OpenInventory()
    {
        UIRootMgr.Instance.OpenWindowWithHide<WindowBig_RoleInfo>(WinName.WindowBig_RoleInfo, WinName.Window_DungeonMap).OpenWindow(WindowBig_RoleInfo.ChildTab.Assem, true);
    }
 
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        if (mInteractWindow != null) mInteractWindow.CloseWindow();
        base.CloseWindow(actionType);
        ///TODO: 秘境CloseWindow屏蔽
        //if (MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.SingleMap || MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.MutiMap)
        //{
        //    WindowBig_WorldChoose world = UIRootMgr.Instance.OpenWindow<WindowBig_WorldChoose>(WinName.WindowBig_WorldChoose, CloseUIEvent.CloseAll);
        //    if (world != null && MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.SingleMap) world.OpenWindow(WindowBig_WorldChoose.ChildTab.PlotDungeon);
        //    if (world != null && MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.MutiMap) world.OpenWindow(WindowBig_WorldChoose.ChildTab.WarDungeon);
        //}
    }

    public void FreshByRoleXy()
    {
        int maxLineItem = 5;     //一横排可显示的item数量
        int maxVerticalItem = 4; //一竖排可显示的item数量
        int remainLine = maxLineItem/2;
        int remainVertical = maxVerticalItem / 2;

        mMapCenterXy = new XyCoordRef(mRoleXy.m_X, mRoleXy.m_Y);
        if (mMapCenterXy.m_X - remainLine < 0)   //让地图不超出边缘
        {
            mMapCenterXy.m_X = remainLine;
        }
        else if (mMapCenterXy.m_X + remainLine >= MyMapData.Width)
        {
            mMapCenterXy.m_X = MyMapData.Width - remainLine - MyMapData.Width%2;
        }

        if (mMapCenterXy.m_Y - remainVertical < 0)
        {
            mMapCenterXy.m_Y = remainVertical;
        }
        else if (mMapCenterXy.m_Y + remainVertical >= MyMapData.Height)
        {
            if (MyMapData.Height%2==0)
                mMapCenterXy.m_Y = MyMapData.Height - remainVertical;
            else
                mMapCenterXy.m_Y = MyMapData.Height - remainVertical;
        }
        
        //设置角色和地图的位置
        Vector2 mapPos = MyMapData.GetPos(mMapCenterXy.m_X, mMapCenterXy.m_Y);
        mViewObj.MapRoot.transform.localPosition = new Vector3(-mapPos.x, -mapPos.y, 0);

        Vector2 rolePos = MyMapData.GetPos(mRoleXy.m_X, mRoleXy.m_Y);
        mViewObj.RolePoint.transform.localPosition = new Vector3(rolePos.x, rolePos.y, 0);

        //设置npc
        FreshNpc();
    }

    public void BtnEvt_MoveClick()
    {
        if (MyMapData == null || mRoleXy == null) return;
        if (Input.touchCount == 1)
        {
            Vector2 pos = Input.GetTouch(0).position;
        }

        //计算点击位置与角色显示位置的偏差，进行移动
        Vector3 rolePos = UIRootMgr.Instance.MyCanvas.transform.InverseTransformPoint(mViewObj.RolePoint.position);
        rolePos.x *= Screen.width / 1080f;
        rolePos.y *= Screen.height / 1920f;

        float xOffset = rolePos.x - (Input.mousePosition.x - Screen.width * 0.5f);
        float yOffset = rolePos.y - (Input.mousePosition.y - Screen.height*0.5f);

        XyCoordRef moveXy = new XyCoordRef();
        if (Mathf.Abs(xOffset) > 30f || Mathf.Abs(yOffset) > 30f)
        {
            if (Mathf.Abs(xOffset) > Mathf.Abs(yOffset))// 哪边偏差大，朝哪边移动
            {
                moveXy.m_X = xOffset > 0 ? -1 : 1;
            }
            else
            {
                moveXy.m_Y = yOffset > 0 ? 1 : -1;
            }
        }
        XyCoordRef toXy = new XyCoordRef();
        toXy.m_X = Mathf.Clamp(mRoleXy.m_X + moveXy.m_X, 0, MyMapData.Width);
        toXy.m_Y = Mathf.Clamp(mRoleXy.m_Y + moveXy.m_Y, 0, MyMapData.Height);
        RoleMoveTo(toXy);
        //Debug.Log(moveXy.m_X + "   " + moveXy.m_Y);
    }

    public bool RoleMoveTo(XyCoordRef toXy , bool isStart=false)
    {
        XyCoordRef lastXy = new XyCoordRef(mRoleXy.m_X, mRoleXy.m_Y);
        mRoleXy = toXy;
        if (!MyMapData.GetWalkable(mRoleXy))
        {
            UIRootMgr.Instance.Window_UpTips.InitTips("那里不能行走", Color.red);
            mRoleXy = lastXy;
            return false;
        }
        else
        {
            FreshByRoleXy();
            if (isStart || lastXy.m_X != toXy.m_X || lastXy.m_Y != toXy.m_Y)
            {
                if (!isStart)
                {
                    //PlayerPrefsBridge.Instance.SetMapSave(global::DungeonMapAccessor.MapSaveType.RolePos, 0,  MyMapData.GetNodeIndex(toXy.m_X, toXy.m_Y));
                    SaveUtils.SetIntInPlayer(PrefsSaveType.RolePos.ToString(), MyMapData.GetNodeIndex(toXy.m_X, toXy.m_Y));
                }
                int toIndex = MyMapData.GetNodeIndex(toXy);
                MindTreeNode enterNode = MindTreeMapCtrl.Instance.GetModEnter(toIndex);
                if (enterNode != null)
                {
                    TDebug.Log("移动，触发进入事件");
                    MindTreeMapCtrl.Instance.StartDoList(enterNode.ChildNode);
                }
            }
               
            return true;
        }
    }


    public void FreshNpc()
    {
        int xyIndex = MyMapData.GetNodeIndex(mRoleXy.m_X, mRoleXy.m_Y);
        Dictionary<int, NpcStatus> npcshowDict = MyMapData.TreeData.GetCanShowNpcs(xyIndex);
        List<int> npcList = new List<int>(npcshowDict.Keys);

        mNpcItemList = TAppUtility.Instance.AddViewInstantiate<NpcItemObj>(mNpcItemList, mViewObj.Part_MapNpcItem,
            mViewObj.NpcRoot, npcList.Count);
        for (int i = 0; i < mNpcItemList.Count; i++)
        {
            mNpcItemList[i].NpcBtn.SetOnClick(null);
        }
        for (int i = 0; i < npcList.Count; i++)//刷新item显示
        {
            NpcStatus status = npcshowDict[npcList[i]];
            mNpcItemList[i].View.gameObject.SetActive(true);
            OldHero npc = OldHero.HeroFetcher.GetHeroByCopy(npcList[i]);
            if(npc==null)continue;
            if (status == NpcStatus.Enable)
            {
                if (npc != null)
                    mNpcItemList[i].NameText.text = npc.name;
                int npcId = npcList[i];
                mNpcItemList[i].NpcBtn.SetOnClick(delegate() { BtnEvt_ClickNpc(npcId); });
            }
            else
            {
                mNpcItemList[i].NameText.text = LangMgr.GetText("尸体");
                string npcName = npc.name;
                mNpcItemList[i].NpcBtn.SetOnClick(delegate() { AppendNewLineDecs(string.Format("{0}的尸体", npcName)); });
            }
        }
        //FreshNpcByList(showNpcList);
    }


    public void AppendNewLineDecs(string str, bool needClear = false)
    {
    }

    public void BtnEvt_ClickNpc(int npcId)
    {
        int nodeIndex = MyMapData.GetNodeIndex(mRoleXy.m_X, mRoleXy.m_Y);
        MindTreeNode node = MyMapData.TreeData.GetNpcNode(nodeIndex, npcId);
        if (node != null)  //打开交互窗口
        {
            mCurNpcNode = node;
            List<MindTreeNode> tempList = mCurNpcNode.GetChild(MindTreeNodeBaseType.InteractMod);
            if (mInteractWindow == null)
            {
                mInteractWindow = UIRootMgr.Instance.OpenWindow<Window_NpcInteract>(WinName.Window_NpcInteract,
                    CloseUIEvent.None);
            }
            mInteractWindow.OpenWindow(node);
        }
        else
        {
            TDebug.LogError(string.Format("此npc不存在:[{0}]", npcId));
        }
    }


    public bool OpenBattle(MindTreeNode battleNode)
    {
        int npcId = battleNode.TryGetInt(0);
        LastPveNpc = npcId;
        BattleType battleType = (BattleType)battleNode.TryGetInt(1);

        System.Action<int> callBack = delegate(int result) //战斗回调
        {
            TDebug.Log("战斗结束回调");
            OpenWin();
            Init(false);//根据战斗类型和战斗结果，刷新地图
            if (battleType.IsSeriousBattle())
            {
                if (result == 0) //决斗，自身死亡
                {
                    SendMapEnd(MapEndType.Dead , null);
                    return;
                }
                else //敌人死亡
                {
                    System.Action saveOver = delegate()//等待保存完后，再继续后续节点
                    {
                        FreshNpc();
                        MindTreeMapCtrl.Instance.DoNextByResult(battleNode, result);
                    };
                    NetMapSaveItem saveItem = new NetMapSaveItem(DungeonMapAccessor.MapSaveType.Npc, npcId, (int)NpcStatus.Dead);
                    PlayerPrefsBridge.Instance.SetMapSave(new List<NetMapSaveItem>() { saveItem }, saveOver,MindTreeMapCtrl.Instance.MyMapType);
                }
            }
            else
            {
                string descStr = "";
                OldHero hero = OldHero.HeroFetcher.GetHeroByCopy(npcId);
                if (result == 0)
                {
                    descStr = LobbyDialogue.GetDescStr("fightFail", hero.name);
                }
                else
                {
                    descStr = LobbyDialogue.GetDescStr("fightWin", hero.name);
                }
                AppendNewLineDecs(descStr);
                MindTreeMapCtrl.Instance.DoNextByResult(battleNode, result);
            }
        };

        if (npcId > 0 && battleType > 0)
        {
            BattleMgr.Instance.EnterPVE(battleType, npcId,PVESceneType.DungeonMap , callBack);
            return true;
        }
        else
        {
            TDebug.LogError(string.Format("错误的setBattle:[{0}]", battleNode.CacheString));
            callBack(0);
            return false;
        }
    }


    public void SendMapEnd(MapEndType endType, MindTreeNode node)
    {
        if (MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.NewerMap)
        {
            TDebug.Log("通过新手秘境");
            ServPacketHander del = delegate(BinaryReader ios)  //服务器会主动推奖励信息
            {
                LobbySceneMainUIMgr.Instance.AppendTextNewLine(LobbyDialogue.GetDescStr("newer_finishMap"));
                GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MapEventReward, null);
                NetPacket.S2C_MapEventReward msg = MessageBridge.Instance.S2C_MapEventReward(ios);
                UIRootMgr.LobbyUI.ShowDropInfo(msg.GoodsList);
            };
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MapEventReward, del);
        }

        if (MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.SingleMap || MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.NewerMap)
        {
            int endId = node == null ? 0 : node.TryGetInt(0);
            UIRootMgr.Instance.IsLoading = true;
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MapEnd, S2C_MapEnd);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_MapEnd(endId , endType));
        }
        else
        {
            TDebug.LogError(string.Format("非秘境地图触发了:[{0}]", node == null ? "无节点信息" : node.CacheString));
        }
    }

    void S2C_MapEnd(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_MapEnd msg = MessageBridge.Instance.S2C_MapEnd(ios);
        Window_MapEndShow mapEndWin = UIRootMgr.Instance.OpenWindow<Window_MapEndShow>(WinName.Window_MapEndShow, CloseUIEvent.None);
        mapEndWin.OpenWindow(mMapIdx, msg.FinishEvents, msg.EndType, msg.EndId, delegate() { CloseWindow(); });
    }
   
    void MapEnd(MapEndType endType)
    {
        if (MindTreeMapCtrl.Instance.MyMapType == MapData.MapType.NewerMap)
        {
            PlayerPrefsBridge.Instance.GuideData.JustPassNewerMap = true;
        }
        
        if (endType == MapEndType.TriggerEnd)//触发了结局
        {
            //UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(CloseWindow, string.Format("秘境结束,触发了[{0}]结局", mapId),
            //    Color.white);
        }
        else if (endType == MapEndType.Dead) //死亡退出
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(delegate() { CloseWindow(); },
                LangMgr.GetText("决斗失败，退出秘境"),
                Color.red);
        }
        else
        {
            CloseWindow();
        }
    }


    void OnDestroy()
    {
    }
}