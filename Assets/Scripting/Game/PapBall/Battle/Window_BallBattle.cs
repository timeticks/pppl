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
        public Text ScoreText;        public ViewObj(UIViewBase view)
        {
            if (CenterAnchor != null) return;
            CenterAnchor = view.GetCommon<RectTransform>("CenterAnchor");
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
        }
    }
    private ViewObj mViewObj;
    public GunBaseCtrl GunCtrl { get { return mViewObj.GunPanel; } }

    internal List<BallBaseCtrl> BallList = new List<BallBaseCtrl>();
    internal List<BallBaseCtrl> BallDisableList = new List<BallBaseCtrl>();
    

    public void OpenWindow(int mapIdx)
    {
        if (mViewBase == null) mViewBase = gameObject.GetComponent<WindowView>();
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        Init(mapIdx);

        Instance = this;
        mViewObj.TBtnExit.TextBtn.text = LangMgr.GetText("btn_exit");
        mViewObj.TBtnExit.SetOnAduioClick(BtnEvt_ExitBattle);
        base.OpenWin();
    }

    void Init(int mapIdx)
    {
        InitMap(mapIdx);
        FreshMutilDown(false);
        AddAndFreshScore(0);

        mViewObj.CenterAnchor.sizeDelta = Vector2.one*(2*HexaMapData.DefaultRadius*MapData.BallScaleRatio());
        //添加枪按下按起响应
        EventTrigger.Entry myTrigger = new EventTrigger.Entry();
        myTrigger.eventID = EventTriggerType.PointerDown;
        myTrigger.callback.RemoveAllListeners();
        myTrigger.callback.AddListener(mViewObj.GunPanel.BtnEvt_BgPointerDown);
        mViewObj.BgBtn.triggers.Add(myTrigger);
    }

    

    public HexaMapData MapData;
    //初始化地图
    public void InitMap(int mapIdx)
    {
        if (PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx <= 0) //无地图信息
        {
            BallMap ballMap = BallMap.Fetcher.GetBallMapCopy(mapIdx);
            MapData = new HexaMapData(PlayerPrefsBridge.Instance.BallMapAcce.MapMaxSize, PlayerPrefsBridge.Instance.BallMapAcce.MapMaxSize, ballMap.ballRadius);
            PlayerPrefsBridge.Instance.InitNewMap(mapIdx);
            mViewObj.GunPanel.Init(this, new List<int>() {-1, -1, -1});

            //初始化中心点周围
            List<XyCoordRef> rangeList = HexaMathf.GetInRange(2, MapData.CenterXy.m_X, MapData.CenterXy.m_Y);
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
            ball.gameObject.SetActive(true);
        }
        else
        {
            GameObject g = Instantiate(mViewObj.Part_Ball) as GameObject;
            ball = g.GetComponent<BallBaseCtrl>();
        }
        if (ballIdx < 0)
            ballIdx = PlayerPrefsBridge.Instance.GetNextRandBall();
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
        ballCtrl.MyData = new BallNodeData(pos, ballCtrl.MyData.Num ,MapData);
        ballCtrl.MyData.BallCtrl = ballCtrl;
        MapData.SetNode(pos.m_X, pos.m_Y, ballCtrl.MyData);
        BallList.Add(ballCtrl);

        PlayerPrefsBridge.Instance.AddBall(MapData.GetNodeIndex(pos.m_X, pos.m_Y), ballCtrl.MyData.Num);
    }

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
        }
    }

    //返回是否有消减得分
    public bool DestroyEqualNum(BallNodeData nodeData, Vector2 gunBallJumpDir)
    {
        ResetNodeSearch();
        List<BallNodeData> bList = GetEqualNumNearList(nodeData);
        bList.Add(nodeData);
        if (bList.Count < 3) return false;

        for (int i = 0; i < bList.Count; i++) //设置false，便于后面寻找无附着球
        {
            bList[i].IsDisable = true;
        }
        List<BallNodeData> linklessList = GetLinklessBall();
        bList.AddRange(linklessList);

        int destroyNum = 0;
        float forceRatio = Mathf.Min(2.5f, Mathf.Pow(bList.Count - 2, 0.3f)); //力度大小，
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
                hitDir = hitDir.normalized * Random.Range(75f, 80f) * forceRatio;
                bList[i].BallCtrl.SetRigibodyAndVelocity(false, true, hitDir);
            }
            else
            {
                TDebug.LogErrorFormat("此节点ballCtrl为空:{0}", bList[i].Pos.ToString());
            }
        }
        AddAndFreshScore(destroyNum);
        return true;
    }

    public void AddAndFreshScore(int addScore)
    {
        //分数
        if (addScore != 0)
        {
            PlayerPrefsBridge.Instance.BallMapAcce.Score += addScore;
            PlayerPrefsBridge.Instance.saveMapAccessor();
        }
        mViewObj.ScoreText.text = LangMgr.GetText("desc_score", PlayerPrefsBridge.Instance.BallMapAcce.Score.ToString());
    }

    public void FreshMutilDown(bool isReduceDown)
    {
        if (isReduceDown)
        {
            PlayerPrefsBridge.Instance.BallMapAcce.MutilAddBallDown--;
            PlayerPrefsBridge.Instance.saveMapAccessor();
        }
        int timeDown = PlayerPrefsBridge.Instance.BallMapAcce.MutilAddBallDown;
        if (timeDown == 0)
        {
            MultiAddBall(5);
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
        MapData.ResetNodeSearch();
    }


    public void StartRot(Vector3 ballPos, Vector3 ballDir)//中心球旋转
    {
        Vector3 fixedDir = ballPos - mViewObj.CenterAnchor.position;
        Vector2 forceDir = MathfUtility.GetForceDir(new Vector2(fixedDir.x, fixedDir.y), new Vector2(ballDir.x, ballDir.y));
        float rotAngle = Mathf.Sign(fixedDir.x*forceDir.y - fixedDir.y*forceDir.x)*forceDir.magnitude*50;
        float rotTime = Mathf.Sqrt(rotAngle);
        float endAngle = mViewObj.CenterAnchor.transform.localRotation.eulerAngles.z + rotAngle;
        Vector3 endRot = new Vector3(0, 0, endAngle);
        TDebug.LogInEditor(endRot + "    " + ballDir);
        mViewObj.CenterAnchor.transform.DOLocalRotate(endRot, 1f).SetEase(Ease.OutQuad);
        PlayerPrefsBridge.Instance.BallMapAcce.CenterAnchorRotate = endAngle;
    }

   

    //得到周围数字相同的
    public List<BallNodeData> GetEqualNumNearList(BallNodeData node)
    {
        node.IsSearched = true;
        List<BallNodeData> sameList = new List<BallNodeData>();

        for (int i = 0; i < node.NearList.Count; i++)
        {
            BallNodeData temp = MapData.GetNode(node.NearList[i].m_X, node.NearList[i].m_Y);
            if (temp != null && !temp.IsSearched)
            {
                if (temp.Num.Equals(node.Num))
                    sameList.Add(temp);
                temp.IsSearched = true;
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
        UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(LangMgr.GetText("desc_exit_battle"), delegate() { CloseWin(); });
    }

    public void CloseWin()
    {
        PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx = 0;
        PlayerPrefsBridge.Instance.saveMapAccessor();
        CloseWindow();
    }

    void OnGUI()
    {
        if (GUILayout.Button("                 "))
        {
            MultiAddBall(5);
        } 
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
        PlayerPrefsBridge.Instance.BallMapAcce.MutilAddBallDown = 5;
        PlayerPrefsBridge.Instance.saveMapAccessor();
        UIRootMgr.Instance.TopMasking = false;
    }
}
