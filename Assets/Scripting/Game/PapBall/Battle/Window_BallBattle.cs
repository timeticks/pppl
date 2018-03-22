using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Window_BallBattle : WindowBase {

    public Window_BallBattle Instance { get; private set; }

    public class ViewObj
    {
        public Transform CenterAnchor;
        public Transform FreeRoot;
        public GunBaseCtrl Gun;
        public GameObject Part_Ball;
        public EventTrigger BgBtn;
        public ViewObj(UIViewBase view)
        {
            if (CenterAnchor != null) return;
            CenterAnchor = view.GetCommon<Transform>("CenterAnchor");
            FreeRoot = view.GetCommon<Transform>("FreeRoot");
            Gun = view.GetCommon<GameObject>("Gun").CheckAddComponent<GunBaseCtrl>();
            Part_Ball = view.GetCommon<GameObject>("Part_BattleBall");
            BgBtn = view.GetCommon<EventTrigger>("BgBtn");
        }
    }
    private ViewObj mViewObj;

    internal List<BallBaseCtrl> BallList = new List<BallBaseCtrl>();
    internal List<BallBaseCtrl> BallDisableList = new List<BallBaseCtrl>();

    void Start()    //TODO:正式删除
    {
        OpenWindow();
    }

    public void OpenWindow()
    {
        if (mViewBase == null) mViewBase = gameObject.GetComponent<WindowView>();
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        Init();

        Instance = this;
        //base.OpenWin();
    }

    void Init()
    {
        mViewObj.Gun.Init(this);

        //添加枪按下按起响应
        EventTrigger.Entry myTrigger = new EventTrigger.Entry();
        myTrigger.eventID = EventTriggerType.PointerDown;
        myTrigger.callback.RemoveAllListeners();
        myTrigger.callback.AddListener(mViewObj.Gun.BtnEvt_BgPointerDown);
        mViewObj.BgBtn.triggers.Add(myTrigger);
        InitMap();
    }

    

    public HexaMapData MapData;
    //初始化地图
    public void InitMap()
    {
        MapData = new HexaMapData(21, 21);

        //初始化中心点周围
        List<XyCoordRef> rangeList = HexaMathf.GetInRange(2, MapData.CenterXy.m_X, MapData.CenterXy.m_Y);
        for (int i = 0; i < rangeList.Count; i++)
        {
            AddBall(GetNewBall(), rangeList[i]);
        }
    }
    void LateUpdate()
    {
        for (int i = 0; i < BallList.Count; i++)
        {
            BallList[i].transform.rotation = Quaternion.identity;
        }
    }


    public BallBaseCtrl GetNewBall()
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
        ball.Init(this);
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
        ballCtrl.MyTrans.localScale = Vector3.one;

        ballCtrl.MyData = new BallNodeData(pos, ballCtrl.MyData.Num ,MapData);
        ballCtrl.MyData.BallCtrl = ballCtrl;
        MapData.SetNode(pos.m_X, pos.m_Y, ballCtrl.MyData);
        BallList.Add(ballCtrl);
    }

    public void RemoveBallInMap(BallBaseCtrl ballCtrl)
    {
        if (ballCtrl.MyData != null && ballCtrl.MyData.Pos != null)
        {
            MapData.SetNode(ballCtrl.MyData.Pos.m_X, ballCtrl.MyData.Pos.m_Y, null);
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
        ballCtrl.gameObject.SetActive(false);
    }

    private IEnumerator mJumpAni;
    public void DestroyEqualNum(BallNodeData nodeData, bool needJumpAni)
    {
        ResetNodeSearch();
        List<BallNodeData> bList = GetEqualNumNearList(nodeData);
        bList.Add(nodeData);
        if (bList.Count < 3) return;
        //if (mJumpAni != null)             //不停止上一个跳跃动画
        //    StopCoroutine(mJumpAni);
        for (int i = 0; i < bList.Count; i++)
        {
            bList[i].IsDisable = true;
        }
        List<BallNodeData> linklessList = GetLinklessBall();
        for (int i = 0; i < linklessList.Count; i++)
        {
            linklessList[i].IsDisable = true;
        }
        mJumpAni = DisableJumpAni(bList, linklessList, nodeData.BallCtrl.MyTrans.localPosition);
        StartCoroutine(mJumpAni);
    }

    IEnumerator DisableJumpAni(List<BallNodeData> nodeList, List<BallNodeData> linklessList, Vector3 boomCenter)
    {
        float curTime = 0f;
        for (int i = 0; i < nodeList.Count; i++)
        {
            nodeList[i].BallCtrl.MyTrans.SetParent(mViewObj.FreeRoot);
        }
        for (int i = 0; i < nodeList.Count; i++)
        {
            RemoveBallInMap(nodeList[i].BallCtrl);
        }
        for (int i = 0; i < linklessList.Count; i++)
        {
            RemoveBallInMap(linklessList[i].BallCtrl);
        }
        UIRootMgr.Instance.TopMasking = false;
        while (curTime < 1.5f)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                nodeList[i].BallCtrl.MyTrans.localPosition = nodeList[i].BallCtrl.MyTrans.localPosition + Vector3.down * 600 * Time.deltaTime;
            }
            for (int i = 0; i < linklessList.Count; i++)
            {
                linklessList[i].BallCtrl.MyTrans.localPosition = linklessList[i].BallCtrl.MyTrans.localPosition + Vector3.left * 300 * Time.deltaTime;
            }
            yield return null;
            curTime += Time.deltaTime;
        }
        for (int i = 0; i < nodeList.Count; i++)
        {
            DisableBall(nodeList[i].BallCtrl);
        }
        for (int i = 0; i < linklessList.Count; i++)
        {
            DisableBall(linklessList[i].BallCtrl);
        }
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


    private IEnumerator mRotCor;
    public void StartRot(Vector3 ballPos, Vector3 ballDir)//中心球旋转
    {
        Vector3 fixedDir = ballPos - mViewObj.CenterAnchor.position;
        Vector2 forceDir = MathfUtility.GetForceDir(new Vector2(fixedDir.x, fixedDir.y), new Vector2(ballDir.x, ballDir.y));
        float rotAngle = Mathf.Sign(fixedDir.x * forceDir.y - fixedDir.y * forceDir.x) * forceDir.magnitude;
        if (mRotCor != null)
            StopCoroutine(mRotCor);
        mRotCor = RotCoroutine(rotAngle);
        StartCoroutine(mRotCor);
    }
    IEnumerator RotCoroutine(float rot)
    {
        rot *= 0.03f;    //缩小旋转幅度
        float startSpdPct = 1f;
        float spd = rot * startSpdPct;
        float curRot = 0f;
        int signFlag = rot > 0 ? 1 : -1;
        while (Mathf.Abs(curRot) < Mathf.Abs(rot))
        {
            float rotNum = spd * Time.deltaTime;
            curRot += rotNum;

            spd = rot * (0.1f + 1 - curRot / rot) * startSpdPct;
            mViewObj.CenterAnchor.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, rotNum));
            yield return null;
        }
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

        for (int i = 0; i < MapData.Balls.Length; i++)
        {
            for (int j = 0; j < MapData.Balls[i].Length; j++)
            {
                if (MapData.Balls[i][j] != null && MapData.Balls[i][j].BallCtrl != null && !MapData.Balls[i][j].IsLinkCenter && !MapData.Balls[i][j].IsDisable)
                    linklessList.Add(MapData.Balls[i][j]);
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
    void AddRandomBall(int ballNum)
    {

    }
}
