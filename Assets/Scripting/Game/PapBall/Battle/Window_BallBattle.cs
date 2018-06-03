using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class Window_BallBattle : WindowBase {

    public static Window_BallBattle Instance { get; private set; }

    public class ViewObj
    {
        public RectTransform CenterAnchor;
        public Transform CenterBall;
        public Transform FreeRoot;
        public GunBaseCtrl GunPanel;
        public GameObject Part_Ball;
        public EventTrigger BgBtn;
        public Transform TopBorder;
        public Transform DownBorder;
        public Transform LeftBorder;
        public Transform RightBorder;
        public TextButton TBtnExit;
        public Text MultiAddText;
        public Text ScoreText;
        public Text CurAddText;
        public Text LootText;
        public TextButton TBtnUniversalBall;
        public TextButton TBtnNextBall;
        public TextButton TBtnFrozenBall;
        public Text UniversalBallNumText;
        public Text NextBallNumText;
        public Text FrozenBallNumText;
        public GameObject FrozenRoot;
        public TextButton TBtnDelay;
        public Text DelayNumText;
        public GameObject Part_LootTips;
        public ViewObj(UIViewBase view)
        {
            if (CenterAnchor != null) return;
            CenterAnchor = view.GetCommon<RectTransform>("CenterAnchor");
            CenterBall = view.GetCommon<RectTransform>("CenterBall");
            FreeRoot = view.GetCommon<Transform>("FreeRoot");
            GunPanel = view.GetCommon<GameObject>("GunPanel").CheckAddComponent<GunBaseCtrl>();
            Part_Ball = view.GetCommon<GameObject>("Part_BattleBall");
            BgBtn = view.GetCommon<EventTrigger>("BgBtn");
            TopBorder = view.GetCommon<Transform>("TopBorder");
            DownBorder = view.GetCommon<Transform>("DownBorder");
            LeftBorder = view.GetCommon<Transform>("LeftBorder");
            RightBorder = view.GetCommon<Transform>("RightBorder");
            TBtnExit = view.GetCommon<TextButton>("TBtnExit");
            MultiAddText = view.GetCommon<Text>("MultiAddText");
            ScoreText = view.GetCommon<Text>("ScoreText");
            CurAddText = view.GetCommon<Text>("CurAddText");
            LootText = view.GetCommon<Text>("LootText");
            TBtnUniversalBall = view.GetCommon<TextButton>("TBtnUniversalBall");
            TBtnNextBall = view.GetCommon<TextButton>("TBtnNextBall");            TBtnFrozenBall = view.GetCommon<TextButton>("TBtnFrozenBall");
            UniversalBallNumText = view.GetCommon<Text>("UniversalBallNumText");
            NextBallNumText = view.GetCommon<Text>("NextBallNumText");
            FrozenBallNumText = view.GetCommon<Text>("FrozenBallNumText");
            FrozenRoot = view.GetCommon<GameObject>("FrozenRoot");
            DelayNumText = view.GetCommon<Text>("DelayNumText");
            TBtnDelay = view.GetCommon<TextButton>("TBtnDelay");
            Part_LootTips = view.GetCommon<GameObject>("Part_LootTips");
        }
    }
    public ViewObj mViewObj;
    public GunBaseCtrl GunCtrl { get { return mViewObj.GunPanel; } }
    public int MusicEnergy;     //音乐播放能量
    internal List<BallBaseCtrl> BallList = new List<BallBaseCtrl>();
    internal List<BallBaseCtrl> BallDisableList = new List<BallBaseCtrl>();
    private List<Text> mLootTextList = new List<Text>();

    
    public void OpenWindow(int mapIdx)
    {
        if (mViewBase == null) mViewBase = gameObject.GetComponent<WindowView>();
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        Instance = this;
        MusicEnergy = 0;
        Init(mapIdx);

        mViewObj.TBtnExit.TextBtn.text = LangMgr.GetText("btn_exit");
        mViewObj.TBtnExit.SetOnAduioClick(BtnEvt_ExitBattle);
        mViewObj.TBtnUniversalBall.SetOnAduioClick(BtnEvt_UniversalBall);
        mViewObj.TBtnNextBall.SetOnAduioClick(BtnEvt_NextBall);
        mViewObj.TBtnFrozenBall.SetOnAduioClick(BtnEvt_Frozen);
        mViewObj.TBtnDelay.SetOnAduioClick(BtnEvt_Delay);

        Fresh();
        base.OpenWin();
    }

    void Init(int mapIdx)
    {
        InitMap(mapIdx);
        FreshMutilDown(false);
        AddAndFreshScore(0);

        mViewObj.CurAddText.gameObject.SetActive(false);
        mViewObj.LootText.text = "";
        mViewObj.CenterBall.localScale = Vector2.one * MapData.BallScaleRatio();
        //添加枪按下按起响应
        EventTrigger.Entry myTrigger = new EventTrigger.Entry();
        myTrigger.eventID = EventTriggerType.PointerDown;
        myTrigger.callback.RemoveAllListeners();
        myTrigger.callback.AddListener(mViewObj.GunPanel.BtnEvt_BgPointerDown);
        mViewObj.BgBtn.triggers.Add(myTrigger);


        mViewObj.FrozenRoot.SetActive(false);

    }

    private void Fresh()
    {
        mViewObj.TBtnUniversalBall.TextBtn.text = LangMgr.GetText("万能");
        FreshItemUse(mViewObj.UniversalBallNumText,
            PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.UniversalBall),
            PlayerPrefsBridge.Instance.GetItemNum(GameConstUtils.id_item_universal_ball),
            PlayerPrefsBridge.Instance.BallMapAcce.UseUniversalBallNum);

        mViewObj.TBtnNextBall.TextBtn.text = LangMgr.GetText("跳过");
        FreshItemUse(mViewObj.NextBallNumText, PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.NextBall),
            PlayerPrefsBridge.Instance.GetItemNum(GameConstUtils.id_item_next_ball),
            PlayerPrefsBridge.Instance.BallMapAcce.UseNextBallNum);

        mViewObj.TBtnFrozenBall.TextBtn.text = LangMgr.GetText("冻结");
        FreshItemUse(mViewObj.FrozenBallNumText, PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.Frozen),
            PlayerPrefsBridge.Instance.GetItemNum(GameConstUtils.id_item_frozen_ball),
            PlayerPrefsBridge.Instance.BallMapAcce.UseFrozenNum);

        mViewObj.TBtnDelay.TextBtn.text = LangMgr.GetText("延缓");
        FreshItemUse(mViewObj.DelayNumText, PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.DelayMulti),
            PlayerPrefsBridge.Instance.GetItemNum(GameConstUtils.id_item_delay_ball),
            PlayerPrefsBridge.Instance.BallMapAcce.UseDelayNum);
    }

    void FreshItemUse(Text text , int freeNum , int itemNum , int useNum)
    {
        int frozenNum = PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.Frozen);
        if (useNum < freeNum)
        {
            text.text = LangMgr.GetText("免费:{0}", freeNum - useNum);
        }
        else
        {
            text.text = itemNum.ToString();
        }
    }
    

    public HexaMapData MapData;
    //初始化地图
    public void InitMap(int mapIdx)
    {
        if (PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx <= 0) //无地图信息
        {
            TDebug.LogInEditorF("初始化地图");
            BallMap ballMap = BallMap.Fetcher.GetBallMapCopy(mapIdx);
            PlayerPrefsBridge.Instance.InitNewMap(mapIdx);
            MapData = new HexaMapData(PlayerPrefsBridge.Instance.BallMapAcce.MapMaxSize, PlayerPrefsBridge.Instance.BallMapAcce.MapMaxSize, ballMap.ballRadius);
            mViewObj.GunPanel.Init(this, new List<int>() {-1, -1, -1});

            //初始化中心点周围
            List<XyCoordRef> rangeList = HexaMathf.GetInRange(ballMap.startBallNum, MapData.CenterXy.m_X, MapData.CenterXy.m_Y);
            for (int i = 0; i < rangeList.Count; i++)
            {
                AddBall(GetNewBall(-1), rangeList[i]);
            }
            PlayerPrefsBridge.Instance.saveMapAccessor();
        }
        else
        {
            BallMap ballMap = BallMap.Fetcher.GetBallMapCopy(PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx);

            //有地图信息
            MapData = new HexaMapData(PlayerPrefsBridge.Instance.BallMapAcce.MapMaxSize, PlayerPrefsBridge.Instance.BallMapAcce.MapMaxSize, ballMap.ballRadius);
            foreach (var temp in PlayerPrefsBridge.Instance.BallMapAcce.BallDict)
            {
                int posIndex = int.Parse(temp.Key);
                XyCoordRef xy = new XyCoordRef(posIndex % MapData.Height, posIndex / MapData.Width);
                AddBall(GetNewBall(temp.Value), xy);
            }
            mViewObj.GunPanel.Init(this, PlayerPrefsBridge.Instance.BallMapAcce.NextBallList);

            mViewObj.CenterAnchor.transform.rotation = Quaternion.Euler(new Vector3(0,0,(float)PlayerPrefsBridge.Instance.BallMapAcce.CenterAnchorRotate));
        }
    }
    void LateUpdate()
    {
        for (int i = 0; i < BallList.Count; i++)
        {
            BallList[i].transform.rotation = Quaternion.identity;
            if (!BallList[i].IsInLegalPos())
                DisableBall(BallList[i]);
        }
    }
    void Update()
    {
        if (Time.frameCount % 3 == 0 && MapData!=null)
        {
            float warnDistance = HexaMapData.DefaultRadius * MapData.BallScaleRatio() * 2.8f;
            warnDistance *= transform.lossyScale.x;
            for (int i = 0; i < MapData.Width; i++)
            {
                for (int j = 0; j < MapData.Height; j++)   //检测小球超出范围的，进行销毁
                {
                    BallNodeData tempNode = MapData.GetNode(i,j);
                    if (tempNode != null && tempNode.BallCtrl != null)
                    {
                        if (!tempNode.BallCtrl.IsInLegalPos())
                            DisableBall(tempNode.BallCtrl);
                        else
                        {
                            //判断是否靠近边界
                            if (tempNode.BallCtrl.MyTrans.position.x + warnDistance >= mViewObj.RightBorder.position.x
                                || tempNode.BallCtrl.MyTrans.position.x - warnDistance <= mViewObj.LeftBorder.position.x
                                || tempNode.BallCtrl.MyTrans.position.y + warnDistance >= mViewObj.TopBorder.position.y
                                || tempNode.BallCtrl.MyTrans.position.y - warnDistance <= mViewObj.DownBorder.position.y)
                            {
                                tempNode.BallCtrl.SetWarnActive(true);
                            }
                            else { tempNode.BallCtrl.SetWarnActive(false); }
                        }
                    }
                }
            }
        }
    }


    //如果ballIdx传0，则随机出一个数
    public BallBaseCtrl GetNewBall(int ballIdx)
    {
        BallBaseCtrl ball = null;
        if (BallDisableList.Count > 0)
        {
            ball = BallDisableList[0];
            BallDisableList.RemoveAt(0);
        }
        else
        {
            GameObject g = Instantiate(mViewObj.Part_Ball) as GameObject;
            ball = g.CheckAddComponent<BallBaseCtrl>();
        }
        if (ballIdx < 0)
            ballIdx = PlayerPrefsBridge.Instance.BallMapAcce.GetNextRandBall();
        ball.Init(this ,MapData.BallScaleRatio(),ballIdx);
        return ball;
    }


    //添加球到转盘上
    public void AddBall(BallBaseCtrl ballCtrl, XyCoordRef pos)
    {
        if (MapData.GetNode(pos.m_X, pos.m_Y) != null)
        {
            TDebug.LogError("此位置不为空，不能添加");
            return;
        }
        ballCtrl.MyTrans.tag = "Ball";
        ballCtrl.MyTrans.SetParent(mViewObj.CenterAnchor);
        ballCtrl.MyTrans.localPosition = MapData.GetNodeLocalPos(pos.m_X, pos.m_Y);
        ballCtrl.MyTrans.localScale = Vector3.one*MapData.BallScaleRatio();
        ballCtrl.gameObject.SetActive(true);
        ballCtrl.MyData = new BallNodeData(pos, ballCtrl.MyData.BallIdx, MapData);
        //ballCtrl.SetBallIcon(pos.ToString());
        ballCtrl.MyData.BallCtrl = ballCtrl;
        MapData.SetNode(pos.m_X, pos.m_Y, ballCtrl.MyData);
        BallList.Add(ballCtrl);

        PlayerPrefsBridge.Instance.AddBall(MapData.GetNodeIndex(pos.m_X, pos.m_Y), ballCtrl.MyData.BallIdx);
    }

    //从地图信息中移除小球
    public void RemoveBallInMap(BallBaseCtrl ballCtrl)
    {
        if (ballCtrl.MyData != null && ballCtrl.MyData.Pos != null)
        {
            MapData.SetNode(ballCtrl.MyData.Pos.m_X, ballCtrl.MyData.Pos.m_Y, null);
            PlayerPrefsBridge.Instance.BallMapAcce.DestoryBallAmount++;
        }
    }

    //禁用球
    public void DisableBall(BallBaseCtrl ballCtrl)
    {
        if (BallList.Contains(ballCtrl))
        {
            ballCtrl.MyData = null;
            BallList.Remove(ballCtrl);
            BallDisableList.Add(ballCtrl);
            ballCtrl.gameObject.SetActive(false);
        }
    }

    //返回是否有消减得分
    public bool DestroyEqualNum(BallNodeData nodeData, Vector2 gunBallJumpDir)
    {
        ResetNodeSearch();
        List<BallNodeData> bList = GetEqualNumNearList(nodeData);   //如果有万能球，可能返回的列表里有不同的球
        bList.Add(nodeData);
        //筛选出相同球大于3的
        Dictionary<int, int> equalBallDict = new Dictionary<int, int>();
        for (int i = 0; i < bList.Count; i++) //设置false，便于后面寻找无附着球
        {
            if (equalBallDict.ContainsKey(bList[i].BallIdx))
                equalBallDict[bList[i].BallIdx]++;
            else
                equalBallDict[bList[i].BallIdx] = 1;
        }
        int universalBallNum = 0;
        equalBallDict.TryGetValue(GameConstUtils.id_universal_ball, out universalBallNum);
        foreach (var temp in equalBallDict) //将普通球加万能球数量，小于3的，移除
        {
            if (temp.Value + universalBallNum < 3 && temp.Key != GameConstUtils.id_universal_ball)
                bList.RemoveAll(x => { return x.BallIdx.Equals(temp.Key); });
        }

        if (bList.Count < 3) return false;


        for (int i = 0; i < bList.Count; i++) //设置false，便于后面寻找无附着球
        {
            bList[i].IsDisable = true;
        }
        List<BallNodeData> linklessList = GetLinklessBall();
        bList.AddRange(linklessList);

        int destroyNum = 0;
        float forceRatio = Mathf.Min(2f, Mathf.Pow(bList.Count - 1, 0.2f)); //力度大小，
        int listCount = bList.Count;
        bool isLoot = false;

        NatureLevelUp nature = PlayerPrefsBridge.Instance.PlayerData.GetNatureLevelUp(NatureType.ScoreLoot);   //掉落加成
        float addCoeffi = nature.natureMisc / 10000f;
		addCoeffi = 10f;
        for (int i = 0; i < bList.Count; i++)//设置刚体跳跃速度
        {
            bList[i].IsDisable = true;
            bList[i].BallCtrl.MyTrans.SetParent(mViewObj.FreeRoot);
            RemoveBallInMap(bList[i].BallCtrl);
            if (bList[i].BallCtrl != null)
            {
                destroyNum++;
                Vector2 hitDir = bList[i].BallCtrl.MyTrans.position - nodeData.BallCtrl.MyTrans.position;
                if (bList[i].BallCtrl == nodeData.BallCtrl) hitDir = gunBallJumpDir;
                hitDir.y = Mathf.Abs(hitDir.y);
                hitDir = hitDir.normalized * Random.Range(60f, 70f) * forceRatio;
                bList[i].BallCtrl.SetRigibodyAndVelocity(false, true, hitDir);

                //掉落
                BallMap ballMap = BallMap.Fetcher.GetBallMapCopy(PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx);
                int lootId = ballMap.scoreLoot[Mathf.Min(listCount, ballMap.scoreLoot.Length - 1)];
                if (lootId > 0)
                {
                    List<GoodsToDrop> goodsList = PlayerPrefsBridge.Instance.onLoot(lootId, addCoeffi);
                    if (goodsList.Count > 0)
                    {
                        isLoot = true;
                        string lootString = GoodsToDrop.getListString(goodsList);
                        TDebug.LogInEditorF("进行掉落[倍数{0}]:{1}\n{2}", addCoeffi, lootId, lootString);
                        goodsList.AddRange(PlayerPrefsBridge.Instance.BallMapAcce.goodsDropList);
                        PlayerPrefsBridge.Instance.BallMapAcce.goodsDropList = GoodsToDrop.combineList(goodsList);

                        Text text = GetLootText();
                        text.text = lootString;
                        text.transform.position = bList[i].BallCtrl.transform.position;
                        text.transform.DOLocalMove(text.transform.localPosition + new Vector3(0, 50, 0), 0.7f).OnComplete(delegate()
                        {
                            text.gameObject.SetActive(false);
                        });
                        text.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                TDebug.LogErrorFormat("此节点ballCtrl为空:{0}", bList[i].Pos.ToString());
            }
        }
        if (isLoot)
        {
            PlayerPrefsBridge.Instance.saveMapAccessor();
        }

        AddAndFreshScore(destroyNum);

        string lootAllString = GoodsToDrop.getListString(PlayerPrefsBridge.Instance.BallMapAcce.goodsDropList);
        mViewObj.LootText.text = lootAllString;
        return true;
    }





    public void AddAndFreshScore(int addScore)
    {
        //分数
        TDebug.LogInEditorF("得到分数：{0}", addScore);
        if (addScore != 0)
        {
            if (mAddScoreCor != null) StopCoroutine(mAddScoreCor);
            mAddScoreCor = AddScoreCor(addScore);
            StartCoroutine(mAddScoreCor);

            PlayerPrefsBridge.Instance.BallMapAcce.Score += addScore;
            PlayerPrefsBridge.Instance.saveMapAccessor();
        }
        mViewObj.ScoreText.text = LangMgr.GetText("desc_score", PlayerPrefsBridge.Instance.BallMapAcce.Score.ToString());
    }

    public void FreshMutilDown(bool isReduceDown)
    {
		int timeDown = PlayerPrefsBridge.Instance.BallMapAcce.MutilBallDown;
        if (isReduceDown)
        {
            PlayerPrefsBridge.Instance.BallMapAcce.MutilBallDown--;
            PlayerPrefsBridge.Instance.saveMapAccessor();
			timeDown = PlayerPrefsBridge.Instance.BallMapAcce.MutilBallDown;
			if (timeDown == 0)
			{
				MultiAddBall(PlayerPrefsBridge.Instance.BallMapAcce.GetMutilNum());
			}
        }
        mViewObj.MultiAddText.text = LangMgr.GetText("desc_multi_down", timeDown.ToString());
    }

    public BallBaseCtrl GetBallCtrlByData(XyCoordRef pos)
    {
        BallNodeData node = MapData.GetNode(pos);
        if (node != null) return node.BallCtrl;
        return null;
    }

    public void ResetNodeSearch()//重置搜索状态
    {
        MapData.ResetNodeSearch(true,true);
    }


    public void StartRot(Vector3 ballPos, Vector3 ballDir)//中心球旋转
    {
        if (mViewObj.FrozenRoot.activeSelf)     //如果正在冻结，不旋转
            return;
        Vector3 fixedDir = ballPos - mViewObj.CenterAnchor.position;
        Vector2 forceDir = MathfUtility.GetForceDir(new Vector2(fixedDir.x, fixedDir.y), new Vector2(ballDir.x, ballDir.y));
        float rotAngle = Mathf.Sign(fixedDir.x*forceDir.y - fixedDir.y*forceDir.x)*forceDir.magnitude*50;
        float rotTime = Mathf.Sqrt(rotAngle);
        float endAngle = mViewObj.CenterAnchor.transform.localRotation.eulerAngles.z + rotAngle;
        Vector3 endRot = new Vector3(0, 0, endAngle);
        //TDebug.LogInEditor(endRot + "    " + ballDir);
        mViewObj.CenterAnchor.transform.DOLocalRotate(endRot, 1f).SetEase(Ease.OutQuad);
        PlayerPrefsBridge.Instance.BallMapAcce.CenterAnchorRotate = endAngle;
    }

   

    //得到周围数字相同的
    public List<BallNodeData> GetEqualNumNearList(BallNodeData node)
    {
        node.IsAdded = true;
        List<BallNodeData> sameList = new List<BallNodeData>();

        for (int i = 0; i < node.NearList.Count; i++)
        {
            BallNodeData temp = MapData.GetNode(node.NearList[i].m_X, node.NearList[i].m_Y);
            if (temp != null && temp.SearchedByIdx!=node.BallIdx && !temp.IsAdded)
            {
                if (node.BallIdx == GameConstUtils.id_universal_ball || temp.BallIdx == GameConstUtils.id_universal_ball)
                {
                    //MapData.ResetNodeSearch(true, false);   //添加了万能球之后，将isSearch重置，用于之前遍历过不能加入，但通过万能球后能加入的球
                    temp.IsAdded = true;
                    sameList.Add(temp);
                }
                else if (temp.BallIdx.Equals(node.BallIdx))
                {
                    temp.IsAdded = true;
                    sameList.Add(temp);
                }
                temp.SearchedByIdx = node.BallIdx;
            }
        }
        for (int i = 0; i < sameList.Count; i++)
        {
            List<BallNodeData> nList = GetEqualNumNearList(sameList[i]);
            if (nList.Count != 0 && nList != null)
                sameList.AddRange(nList);
        }
        return sameList;
    }

    //得到未连接的球
    public List<BallNodeData> GetLinklessBall()
    {
        List<BallNodeData> linklessList = new List<BallNodeData>();

        MapData.ResetNodeLinkCenter();

        //从中心球出发，递归所有有连接的球，将这些球置为有连接
        List<XyCoordRef> nearList = HexaMathf.GetInRange(1, MapData.CenterXy.m_X, MapData.CenterXy.m_Y);
        SignLinklessBall(nearList);

        for (int i = 0; i < MapData.Width; i++)
        {
            for (int j = 0; j < MapData.Height; j++)
            {
                BallNodeData nodeData = MapData.GetNode(i, j);
                if (nodeData != null && nodeData.BallCtrl != null && !nodeData.IsLinkCenter && !nodeData.IsDisable)
                    linklessList.Add(nodeData);
            }
        }
        return linklessList;
    }

    //球相对于中心球的位置
    public Vector3 InverseTransformPointWithCenter(Transform ballTrans)
    {
        return mViewObj.CenterAnchor.InverseTransformPoint(ballTrans.position);
    }


    //将无连接的球进行标记
    void SignLinklessBall(List<XyCoordRef> nearList)
    {
        for (int i = 0; i < nearList.Count; i++)
        {
            BallNodeData tempNode = MapData.GetNode(nearList[i]);
            if (tempNode == null || tempNode.BallCtrl == null || tempNode.IsLinkCenter || tempNode.IsDisable)
                continue;
            tempNode.IsLinkCenter = true;
            SignLinklessBall(tempNode.NearList);
        }
    }

    //每隔一定时间，随机添加球
    void MultiAddBall(int ballNum)
    {
        StartCoroutine(MultiAddBallCor(mViewObj.CenterAnchor, ballNum));
        UIRootMgr.Instance.TopMasking = true;
    }

    void BtnEvt_ExitBattle()
    {
        UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(LangMgr.GetText("desc_exit_battle"), delegate() { BattleEnd(); });
    }
    #region 道具
    
    void BtnEvt_UniversalBall()//万能球
    {
        int consumeNum = PlayerPrefsBridge.Instance.BallMapAcce.UseUniversalBallNum + 1 - PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.UniversalBall);
        if (!PlayerPrefsBridge.Instance.checkItem(GameConstUtils.id_item_universal_ball, consumeNum, true))
        {
            return;
        }
        System.Action okDel = delegate() { UseUniversalBall(consumeNum); };
        UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(LangMgr.GetText("是否消耗{0}个道具，激活万能球？", consumeNum), okDel);
    }
    void UseUniversalBall(int consumeNum)
    {
        PlayerPrefsBridge.Instance.consumeItem(GameConstUtils.id_item_universal_ball, consumeNum, false, "");
        GunCtrl.DisableCurWaitBall();
        GunCtrl.CreateWaitBall(GameConstUtils.id_universal_ball , true);
        PlayerPrefsBridge.Instance.BallMapAcce.UseUniversalBallNum++;
        PlayerPrefsBridge.Instance.saveMapAccessor();
        PlayerPrefsBridge.Instance.saveItemModule();
        Fresh();
        UIRootMgr.Instance.Window_UpTips.InitTips(string.Format("使用{0}成功", Item.GetName(GameConstUtils.id_item_universal_ball)), Color.white);
    }

    void BtnEvt_NextBall()  //跳过当前发射的球
    {
        int consumeNum = PlayerPrefsBridge.Instance.BallMapAcce.UseNextBallNum + 1 - PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.NextBall);
        GunCtrl.CreateWaitBall(-1, false);
        if (!PlayerPrefsBridge.Instance.checkItem(GameConstUtils.id_item_next_ball , consumeNum , true))
        {
            return;
        }
        System.Action okDel = delegate() { UseNextBall(consumeNum); };
        if (consumeNum > 0)
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(LangMgr.GetText("是否消耗{0}个道具，跳过当前待发射的球？", consumeNum), okDel);
        else
            okDel();
    }
    void UseNextBall(int consumeNum)
    {
        PlayerPrefsBridge.Instance.consumeItem(GameConstUtils.id_item_next_ball, consumeNum, false, "");
        GunCtrl.DisableCurWaitBall();
        GunCtrl.CreateWaitBall(-1, false);
        PlayerPrefsBridge.Instance.BallMapAcce.UseNextBallNum++;
        PlayerPrefsBridge.Instance.saveMapAccessor();
        PlayerPrefsBridge.Instance.saveItemModule();
        Fresh();
        UIRootMgr.Instance.Window_UpTips.InitTips(string.Format("使用{0}成功", Item.GetName(GameConstUtils.id_item_next_ball)), Color.white);
    }

    public void BtnEvt_Frozen()
    {
        int consumeNum = PlayerPrefsBridge.Instance.BallMapAcce.UseFrozenNum + 1 - PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.Frozen);
        if (!PlayerPrefsBridge.Instance.checkItem(GameConstUtils.id_item_frozen_ball, consumeNum, true))
        {
            return;
        }
        System.Action okDel = delegate() { UseFrozen(consumeNum); };
        if (consumeNum>0)
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(LangMgr.GetText("是否消耗{0}个道具，跳过当前待发射的球？", consumeNum), okDel);
        else
            okDel();
    }

    void UseFrozen(int consumeNum)
    {
        mViewObj.FrozenRoot.SetActive(true);
        PlayerPrefsBridge.Instance.consumeItem(GameConstUtils.id_item_frozen_ball, consumeNum, false, "");
        PlayerPrefsBridge.Instance.BallMapAcce.UseFrozenNum++;
        PlayerPrefsBridge.Instance.saveMapAccessor();
        PlayerPrefsBridge.Instance.saveItemModule();
        UIRootMgr.Instance.Window_UpTips.InitTips(string.Format("使用{0}成功", Item.GetName(GameConstUtils.id_item_frozen_ball)), Color.white);
        Fresh();
    }

    public void ResetFrozen()
    {
        mViewObj.FrozenRoot.SetActive(false);
    }

    void BtnEvt_Delay()
    {
        int consumeNum = PlayerPrefsBridge.Instance.BallMapAcce.UseDelayNum + 1 - PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(NatureType.DelayMulti);
        if (!PlayerPrefsBridge.Instance.checkItem(GameConstUtils.id_item_delay_ball, consumeNum, true))
        {
            return;
        }
        System.Action okDel = delegate() { UseDelay(consumeNum); };
        if (consumeNum > 0)
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(LangMgr.GetText("是否消耗{0}个道具，延缓一回合多球加入时间？", consumeNum), okDel);
        else
            okDel();
    }
    void UseDelay(int consumeNum)
    {
        PlayerPrefsBridge.Instance.consumeItem(GameConstUtils.id_item_delay_ball, consumeNum, false, "");
        PlayerPrefsBridge.Instance.BallMapAcce.UseDelayNum++;
        PlayerPrefsBridge.Instance.BallMapAcce.MutilBallDown++;
        PlayerPrefsBridge.Instance.saveMapAccessor();
        PlayerPrefsBridge.Instance.saveItemModule();
        UIRootMgr.Instance.Window_UpTips.InitTips(string.Format("使用{0}成功",Item.GetName(GameConstUtils.id_item_delay_ball)), Color.white);
        Fresh();
        FreshMutilDown(false);
    }

    #endregion

    public void BattleEnd()
    {
        if (PlayerPrefsBridge.Instance.BallMapAcce.Score > PlayerPrefsBridge.Instance.PlayerData.MaxScore)
        {
            PlayerPrefsBridge.Instance.PlayerData.MaxScore = PlayerPrefsBridge.Instance.BallMapAcce.Score;
            PlayerPrefsBridge.Instance.PlayerData.MaxScoreTime = AppTimer.CurTimeStampSecond;
        }
        PlayerPrefsBridge.Instance.PlayerData.AllScore += PlayerPrefsBridge.Instance.BallMapAcce.Score;
        PlayerPrefsBridge.Instance.PlayerData.BattleNum++;
        PlayerPrefsBridge.Instance.savePlayerModule();

        int mapIdx = PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx;
        PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx = 0;
        PlayerPrefsBridge.Instance.saveMapAccessor();
        UIRootMgr.Instance.OpenWindow<Window_MapEndShow>(WinName.Window_MapEndShow).OpenWindow(mapIdx);
    }

    public void CloseWin()
    {
        CloseWindow();
    }


    //在地图周围产生多球个球，进行加入
    IEnumerator MultiAddBallCor(Transform centerAnchor, int ballNum)
    {
        int mapRadiusLength = 1000;
        for (int i = 0; i < ballNum; i++)
        {
            //创建一个球
            BallBaseCtrl ball = GetNewBall(-1);
            ball.transform.SetParent(mViewObj.FreeRoot);
            ball.transform.rotation = Quaternion.identity;
            ball.transform.localScale = Vector3.one*MapData.BallScaleRatio();
            ball.gameObject.SetActive(true);
            //随机出其位置，其速度朝向中心点
            float randAngle = Random.Range(-45f, 225f);
            Vector3 randPos = new Vector3(Mathf.Cos(randAngle) , Mathf.Sin(randAngle) , 0);
            Vector3 dir = -randPos;

            ball.transform.localPosition = randPos * mapRadiusLength;

            //进行发射
            ball.StartRun(dir , BallType.ForceAddBall);
            PlayerPrefsBridge.Instance.BallMapAcce.MutilBallAmount++;
			yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
        BallMap map = BallMap.Fetcher.GetBallMapCopy(PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx);
        PlayerPrefsBridge.Instance.BallMapAcce.MutilBallDown =map.multiTimeDown[0] 
            + (int) Mathf.Repeat(PlayerPrefsBridge.Instance.BallMapAcce.LastMutilBallDown - map.multiTimeDown[0] - 1,
                map.multiTimeDown[1] - map.multiTimeDown[0]);
        PlayerPrefsBridge.Instance.BallMapAcce.LastMutilBallDown = PlayerPrefsBridge.Instance.BallMapAcce.MutilBallDown;
        FreshMutilDown(false);
        PlayerPrefsBridge.Instance.saveMapAccessor();
		yield return new WaitForSeconds (0.3f);
        UIRootMgr.Instance.TopMasking = false;
    }

    private IEnumerator mAddScoreCor;
    public IEnumerator AddScoreCor(int addScoreAmount)
    {
        float scoreAddInverseTime = 0.06f;
        Text numText = mViewObj.CurAddText;
        float curRoundTime = float.MaxValue;
        float endScale = Mathf.Pow(addScoreAmount+1,0.3f);
        int curScore = 0;
        numText.gameObject.SetActive(true);
        Vector3 startScale = Vector3.one;
        numText.transform.localScale = startScale;
        numText.transform.localPosition = Vector3.zero;
        numText.color = new Color(numText.color.r, numText.color.g, numText.color.b, 1f);
        numText.DOKill();
        while (curScore < addScoreAmount)
        {
            curRoundTime += Time.deltaTime;
            if (curRoundTime > scoreAddInverseTime)
            {
                curRoundTime = 0f;
                curScore++;
                numText.text = string.Format("+{0}", curScore);
                numText.transform.localScale = startScale * Mathf.Lerp(1, endScale, curScore / (float)addScoreAmount);
            }
            yield return null;
        }
        Color endColor = numText.color;
        endColor.a = 0f;
        numText.DOColor(endColor , 1).OnComplete(delegate() { numText.gameObject.SetActive(false); });
        //Tweener tweener = DOTween.To(() => numText.color, x => numText.color = x, endColor, 1f).OnComplete(
        //   delegate() { numText.gameObject.SetActive(false); });
        //tweener.PlayForward();
    }

    //检查是否没有小球了
    public void CheckMapEmpty()
    {
        for (int i = 0; i < MapData.Width; i++)
        {
            for (int j = 0; j < MapData.Height; j++)
            {
                BallNodeData tempNode = MapData.GetNode(i, j);
                if (tempNode != null && tempNode.BallCtrl != null)
                {
                    return;
                }
            }
        }
        UIRootMgr.Instance.TopMasking = true;
        float forWait = 0f;   //仅是为了等待时间
        Tweener tweener = DOTween.To(() => forWait, x => { forWait = x; }, 1f, 0.5f).OnComplete(delegate()
        {
            ResetMapWhenEmpty();
        });
    }
    public void ResetMapWhenEmpty()
    {
        UIRootMgr.Instance.TopMasking = true;
        //初始化中心点周围
        BallMap ballMap = BallMap.Fetcher.GetBallMapCopy(PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx);
        List<XyCoordRef> rangeList = HexaMathf.GetInRange(ballMap.startBallNum, MapData.CenterXy.m_X, MapData.CenterXy.m_Y);
        for (int i = 0; i < rangeList.Count; i++)
        {
            if (MapData.GetNode(rangeList[i].m_X, rangeList[i].m_Y) == null)
                AddBall(GetNewBall(-1), rangeList[i]);
            else
                TDebug.LogErrorFormat("{0}不为空，不能添加", rangeList[i].ToString());
        }
        PlayerPrefsBridge.Instance.saveMapAccessor();
        mViewObj.CenterAnchor.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        mViewObj.CenterAnchor.DOScale(1f, 1.2f);
        mViewObj.CenterAnchor.DOLocalRotate(Vector3.zero, 0f);
        mViewObj.CenterAnchor.DOLocalRotate(new Vector3(0, 0, 360.1f), 1.5f, RotateMode.FastBeyond360).OnComplete(
            delegate() { UIRootMgr.Instance.TopMasking = false; }
            );
    }


    Text GetLootText()
    {
        for (int i = 0; i < mLootTextList.Count; i++)
        {
            if (!mLootTextList[i].gameObject.activeSelf)
                return mLootTextList[i];
        }
        GameObject go = Instantiate(mViewObj.Part_LootTips, mViewObj.FreeRoot);
        return go.GetComponent<Text>();
    }

}
