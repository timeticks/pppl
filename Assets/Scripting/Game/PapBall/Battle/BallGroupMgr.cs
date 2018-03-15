using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallGroupMgr : MonoBehaviour 
{
    public static BallGroupMgr Instance { get; private set; }
    public Transform CenterAnchor;

    internal List<BallBaseCtrl> BallList = new List<BallBaseCtrl>();
    internal List<BallBaseCtrl> BallDisableList = new List<BallBaseCtrl>();

    public class ViewObj
    {
        public GameObject Part_Ball;
        public Transform CenterAnchor;
        public ViewObj(UIViewBase view)
        {
            if (Part_Ball != null) return;
            Part_Ball = view.GetCommon<GameObject>("Part_Ball");
            CenterAnchor = view.GetCommon<Transform>("CenterAnchor");
        }
    }

    private ViewObj mViewObj;
    void Awake()
    {
        Instance = this;
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
    }

    void Start()
    {
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
            BallList[i].transform.rotation =  Quaternion.identity;
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
        ball.Init();
        return ball;
    }


    //添加球到转盘上
    public void AddBall(BallBaseCtrl ballCtrl,XyCoordRef pos)
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

        ballCtrl.MyData = new BallNodeData(pos, ballCtrl.MyData.Num);
        ballCtrl.MyData.BallCtrl = ballCtrl;
        MapData.SetNode(pos.m_X, pos.m_Y, ballCtrl.MyData);
        BallList.Add(ballCtrl);
    }
    //移除球
    public void DisableBall(BallBaseCtrl ballCtrl) 
    {
        if (ballCtrl.MyData != null && ballCtrl.MyData.Pos!=null)
        {
            MapData.SetNode(ballCtrl.MyData.Pos.m_X, ballCtrl.MyData.Pos.m_Y, null);
        }
        if (BallList.Contains(ballCtrl))
        {
            ballCtrl.MyData = null;
            BallList.Remove(ballCtrl);
            BallDisableList.Add(ballCtrl);
        }
        ballCtrl.gameObject.SetActive(false);
    }

    private IEnumerator mJumpAni;
    public void DestroyEqualNum(BallNodeData nodeData , bool needJumpAni)
    {
        BattleSceneMgr.instance.m_BallGroupMgr.ResetNodeSearch();
        List<BallNodeData> bList = BallGroupMgr.Instance.GetEqualNumNearList(nodeData);
        bList.Add(nodeData);
        if (bList.Count < 3) return;
        if (mJumpAni != null)
            StopCoroutine(mJumpAni);
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
            BallGroupMgr.Instance.DisableBall(nodeList[i].BallCtrl);
        }
        for (int i = 0; i < linklessList.Count; i++)
        {
            BallGroupMgr.Instance.DisableBall(linklessList[i].BallCtrl);
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
    public void StartRot(Vector3 ballPos ,  Vector3 ballDir)//中心球旋转
    {
        Vector3 fixedDir = ballPos - CenterAnchor.position;
        Vector2 forceDir = MathfUtility.GetForceDir(new Vector2(fixedDir.x, fixedDir.y), new Vector2(ballDir.x, ballDir.y));
        float rotAngle = Mathf.Sign(fixedDir.x * forceDir.y - fixedDir.y * forceDir.x) * forceDir.magnitude;
        if (mRotCor != null)
            StopCoroutine(mRotCor);
        mRotCor = RotCoroutine(rotAngle);
        StartCoroutine(mRotCor);
    }
    IEnumerator RotCoroutine(float rot)
    {
        float startSpdPct = 1f;
        float spd = rot * startSpdPct;
        float curRot = 0f;
        int signFlag = rot > 0 ? 1 : -1;
        while (Mathf.Abs(curRot) < Mathf.Abs(rot))
        {
            float rotNum = spd * Time.deltaTime;
            curRot += rotNum;

            spd = rot * (0.1f + 1 - curRot / rot) * startSpdPct;
            CenterAnchor.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, rotNum));
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


/// <summary>
/// 地图的第0排，第一个为半格。
/// 即0排的完整第0个横坐标为0.5
/// 1排第0个横坐标为0
/// </summary>
public class HexaMapData
{
    public static Vector2 StartPos;
    public static float Radius = 60f;
    public static float HeightRatio = Radius*1.732f;       //根号3

    public int Width, Height;

    public XyCoordRef CenterXy;
    public BallNodeData[][] Balls;

    public HexaMapData(int cols, int rows)
    {
        Width = cols;
        Height = rows;
        CenterXy = new XyCoordRef(Width / 2, Height / 2);
        StartPos = new Vector2(-Radius * (Width + ((CenterXy.m_Y % 2 == 0) ? 1 : 0)), -HeightRatio * CenterXy.m_Y);

        Balls = new BallNodeData[Width][];
        for (int i = 0; i < Width; i++)
        {
            Balls[i] = new BallNodeData[Height];
        }

    }
    public BallNodeData GetNode(int x, int y)
    {
        if ((x < 0 || x >= Width) || (y < 0 || y >= Height))
        {
            TDebug.LogErrorFormat("越界:x:{0}|y:{1}", x, y);
            return null;
        }
        return Balls[x][y];
    }
    public BallNodeData GetNode(XyCoordRef pos)
    {
        return GetNode(pos.m_X, pos.m_Y);
    }

    public void SetNode(int x , int y, BallNodeData data)
    {
        if ((x < 0 || x >= Width) || (y < 0 || y > Height))
        {
            TDebug.LogErrorFormat("越界:x:{0}|y:{1}", x, y);
            return;
        }
        Balls[x][y] = data;
    }

    public void ResetNodeSearch()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            for (int j = 0; j < Balls[i].Length; j++)
            {
                if(Balls[i][j]!=null)
                    Balls[i][j].IsSearched = false;
            }
        }
    }
    public void ResetNodeLinkCenter()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            for (int j = 0; j < Balls[i].Length; j++)
            {
                if (Balls[i][j] != null)
                    Balls[i][j].IsLinkCenter = false;
            }
        }
    }

    //是否坐标越界
    public bool IsLegal(int x, int y)
    {
        if ((x < 0 || x >= Width) || (y < 0 || y >= Height))
            return false;
        return true;
    }

    //通过xy坐标，得到世界位置
    public Vector2 GetNodeLocalPos(int x,int y)
    {
        float posX = ((y%2 == 0 ? 1f : 0) + x*2 + 1)*Radius;
        float posY = y * HeightRatio;
        return new Vector2(posX, posY) + StartPos;
    }

    //获取离此坐标最近的一个xy坐标
    public XyCoordRef GetNearestXy(Vector2 localPos , bool needNull)
    {
        float nearestDis = 100000f;
        XyCoordRef nearestXy = null;
        for (int i = 0; i < Balls.Length; i++)
        {
            for (int j = 0; j < Balls[i].Length; j++)
            {
                if (i == CenterXy.m_X && j == CenterXy.m_Y) continue;
                if (needNull && Balls[i][j]!=null)
                {
                    continue;
                }
                float dis = Vector2.Distance(GetNodeLocalPos(i, j), localPos);
                if (nearestDis > dis)
                {
                    nearestDis = dis;
                    nearestXy = new XyCoordRef(i, j);
                    TDebug.Log(nearestDis + "  " + nearestXy.ToString());
                }
            }
        }
        return nearestXy;
    }


    //public HexaNodeData GetNode(int x, int y)
    //{
    //    if ((x < 0 || x >= m_Width) || (y < 0 || y > m_Height))
    //        return null;
    //    int length = x + m_Width * y;
    //    if (length >= 0 && length < m_NodeList.Count)
    //        return m_NodeList[length];
    //    else
    //        return null;
    //}
    //public int GetNodeIndex(int x, int y)
    //{
    //    return x + m_Width * y;
    //}
}