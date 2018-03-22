using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BallBaseCtrl : MonoBehaviour {

    public Image m_BallImage;
    public Text m_BallText;
    public BallState m_BallState;

    internal float mMoveSpeed = 2800;
    internal BallNodeData MyData;
    internal Transform MyTrans;
    private Vector3 mMoveDir;
    private Window_BallBattle mParentWin;
    bool mAttached = false;

    void Awake()
    {
        mMoveSpeed = 2000f;
    }

    public void Init(Window_BallBattle parentWin)
    {
        MyTrans = transform;
        mParentWin = parentWin;
        MyData = new BallNodeData(Random.Range(0, 4));
        m_BallText.text = MyData.Num.ToString();
        mAttached = false;
        m_BallState = BallState.Idle;
        MyTrans.tag = "Untagged";
    }

    private IEnumerator mRunCor;
    public void StartRun(Vector3 dir)
    {
        mMoveDir = dir;
        m_BallState = BallState.Run;
        if (mRunCor != null)
            StopCoroutine(mRunCor);
        mRunCor = RunCoroutine(dir);
        StartCoroutine(mRunCor);
    }

    

    IEnumerator RunCoroutine(Vector3 dir)
    {
        dir =dir.normalized;
        while (m_BallState == BallState.Run)
        {
            if (Mathf.Abs(MyTrans.localPosition.y) > 3000 || Mathf.Abs(MyTrans.localPosition.x) > 2000) //超出边界消失
            {
                UIRootMgr.Instance.TopMasking = false;
                mParentWin.DisableBall(this);
            }
            MyTrans.localPosition += dir * Time.deltaTime * mMoveSpeed;
            yield return null;
        }
    }

    
    //当飞出的球碰到物体后
    void OnTriggerEnter2D(Collider2D col)
    {
        if (m_BallState == BallState.Idle) return;
        if (col.gameObject.CompareTag("Ball") || col.gameObject.CompareTag("CenterAnchor")) //附着
        {
            Attach(col);
        }
        else if (col.gameObject.name.Contains("Border")) //反弹
        {
            mMoveDir = new Vector3(-mMoveDir.x, mMoveDir.y, mMoveDir.z);
            StartRun(mMoveDir);
        }
        else if (col.gameObject.name.Contains("Down")) //底部
        {
            mMoveDir = new Vector3(mMoveDir.x, -mMoveDir.y, mMoveDir.z);
            StartRun(mMoveDir);
        }
    }

    int aaaa = 0;
    void Attach(Collider2D otherCol)
    {
        if (mAttached)
        {
            TDebug.LogErrorFormat("已经在转盘上：{0}", ++aaaa);
            return;
        }
        BallBaseCtrl otherBallCtrl = otherCol.GetComponent<BallBaseCtrl>();
        if ((otherBallCtrl == null || otherBallCtrl.MyData == null || otherBallCtrl.MyData.IsDisable) && !otherCol.gameObject.CompareTag("CenterAnchor"))
            return;
        UIRootMgr.Instance.TopMasking = false;
        if (mRunCor != null)
            StopCoroutine(mRunCor);

        mAttached = true;
        m_BallState = BallState.Idle;
        //Vector3 ballLocalPosInMyTrans = mTrans.worldToLocalMatrix.MultiplyPoint(otherCol.transform.position);
        //ballLocalPosInMyTrans = mTrans.InverseTransformPoint(otherCol.transform.position);
        //Debug.Log(ballLocalPosInMyTrans +" "+m_myTrans.worldToLocalMatrix.MultiplyPoint(m_myTrans.position) + m_myTrans.localPosition);

        Vector3 localPosWithCenter = mParentWin.InverseTransformPointWithCenter(MyTrans); //球相对于中心球的位置
        XyCoordRef xyPos = mParentWin.MapData.GetNearestXy(new Vector2(localPosWithCenter.x, localPosWithCenter.y), true);

        //得到与col的相对位置
        //HexagonPosType posType = HexagonGridMgr.CurHexagon.GetHexagonPosType(Vector2.zero, offsetPos);
        TDebug.LogFormat("xy:{0}" , xyPos.ToString());
        mParentWin.AddBall(this, xyPos);
        mParentWin.DestroyEqualNum(MyData, true);
        mParentWin.StartRot(MyTrans.position, mMoveDir);

    }

    void FreshDataAfterAttach(XyCoordRef xy) //附着后，刷新信息
    {
        //BallCellData colData = colBall.m_myData;
        
    }


    #region 相邻测试
    bool needDeleteNear = false;
    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Space))  //测试相邻
        {
            Debug.LogError("测试相邻");
            needDeleteNear = true;
        }
    }
    void DeleteNearTest()
    {
        if (needDeleteNear)
        {
            needDeleteNear = false;
            for (int i = 0; i < MyData.NearList.Count; i++)
            {
                //if (mMyData.NearList[i] != null)
                //    BattleSceneMgr.instance.m_BallGroupMgr.DisableBall(BallGroupMgr.Instance.);
            }
        }
    }
    #endregion



    //Vector2 GetFixedPos(Vector2 ball , Vector2 spdDir , Vector2 curPos)//得到修正后的坐标，矫正碰撞延迟
    //{
    //    //得到过curPos和spdDir的直线
    //    float lineK = MathfUtility.GetLineK(spdDir);
    //    float lineB = MathfUtility.GetLineB(curPos, spdDir);
    //    ball += curPos;
    //    //得到直线与ball的距离
    //    float dis = MathfUtility.GetLineToPointDis(ball, lineK, lineB);
    //    float ballDis = Vector2.Distance(ball, curPos);
    //    //算出要沿spdDir方向回退多少
    //    float backLength = Mathf.Sqrt(HexagonGridMgr.CurHexagon.m_HexagonRadius * HexagonGridMgr.CurHexagon.m_HexagonRadius*4 - dis * dis);
    //    float curLength = 0;

    //    if(Mathf.Abs( ballDis-dis)>1f)
    //        curLength = Mathf.Sqrt(ballDis * ballDis - dis * dis);
    //    Vector2 toBallDir = ball - curPos;
    //    int flag = Vector2.Dot(toBallDir, spdDir) >= 0 ? -1 : 1;
    //    backLength = backLength + flag * curLength;
    //    Debug.Log("backLength" + backLength + "   curLength" + curLength + "  flag" + flag + "   curPos" + curPos + "  dis" + dis + "  lineK" + lineK + " lineB" + lineB + "  ball" + ball + "   spdDir" + spdDir);

    //    //Debug.Log(dis+"    "+backLength);
    //    Vector2 fixedPos = curPos - spdDir.normalized * backLength;
    //    return fixedPos;
    //}




}


public enum BallState
{
    Idle,
    Run
}

