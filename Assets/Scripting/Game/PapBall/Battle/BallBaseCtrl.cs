using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BallBaseCtrl : MonoBehaviour {

    public BallType MyBallType;

    internal float mMoveSpeed = 300;
    internal BallNodeData MyData;
    internal Transform MyTrans;
    internal Window_BallBattle ParentWin;
    internal Vector3 MoveDir;
    bool mAttached = false;

    public class ViewObj
    {
        public Image MyImage;
        public Rigidbody2D MyRigibody;
        public CircleCollider2D MyCollider;
        public Text TextNum;
        public ViewObj(UIViewBase view)
        {
            if (MyImage != null) return;
            MyImage = view.GetCommon<Image>("Part_BattleBall");
            MyRigibody = view.GetCommon<Rigidbody2D>("Part_BattleBall");
            MyCollider = view.GetCommon<CircleCollider2D>("Part_BattleBall");
            TextNum = view.GetCommon<Text>("TextNum");
        }
    }

    private ViewObj mViewObj;

    public void Init(Window_BallBattle parentWin)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        MyTrans = transform;
        ParentWin = parentWin;
        MyData = new BallNodeData(Random.Range(0, 3));
        mViewObj.TextNum.text = MyData.Num.ToString();
        SetRigibodyAndVelocity(true, false, Vector2.zero);
        mAttached = false;
        mViewObj.MyCollider.isTrigger = true;
        MyBallType = BallType.IdleBall;
        MyTrans.tag = "Untagged";
    }

    public void StartRun(Vector3 dir , BallType ballType)
    {
        SetRigibodyAndVelocity(false, false, dir.normalized * mMoveSpeed);
        mViewObj.MyCollider.isTrigger = false;
        MyBallType = ballType;
        gameObject.CheckAddComponent<BallStop>().Init(this);
    }

    public void ChangeVelocity(Vector3 velocity)
    {
        MoveDir = velocity.normalized;
        mViewObj.MyRigibody.velocity = velocity;
    }

    public bool IsInLegalPos()
    {
        if (Mathf.Abs(MyTrans.localPosition.y) > 2000 || Mathf.Abs(MyTrans.localPosition.x) > 1500)
            return false;
        return true;
    }

    public void Attach(Collider2D otherCol)
    {
        if (mAttached)
        {
            TDebug.LogErrorFormat("已经在转盘上");
            return;
        }
        BallBaseCtrl otherBallCtrl = otherCol.GetComponent<BallBaseCtrl>();
        if ((otherBallCtrl == null || otherBallCtrl.MyData == null || otherBallCtrl.MyData.IsDisable) && !otherCol.gameObject.CompareTag("CenterAnchor"))
            return;

        UIRootMgr.Instance.TopMasking = false;

        mViewObj.MyCollider.isTrigger = true;
        SetRigibodyAndVelocity(true, false, Vector2.zero);
        mAttached = true;
        Destroy(GetComponent<BallStop>());

        Vector3 localPosWithCenter = ParentWin.InverseTransformPointWithCenter(MyTrans); //球相对于中心球的位置
        XyCoordRef xyPos = ParentWin.MapData.GetNearestXy(new Vector2(localPosWithCenter.x, localPosWithCenter.y), true);

        Vector2 hitDir = otherCol.transform.localPosition - MyTrans.localPosition;

        //得到与col的相对位置
        //HexagonPosType posType = HexagonGridMgr.CurHexagon.GetHexagonPosType(Vector2.zero, offsetPos);
        TDebug.LogFormat("xy:{0}" , xyPos.ToString());
        ParentWin.AddBall(this, xyPos);
        if (MyBallType== BallType.RunByGunBall)
            ParentWin.DestroyEqualNum(MyData, hitDir);
        ParentWin.StartRot(MyTrans.position, MoveDir);

        MyBallType = BallType.IdleBall;
    }


    public void SetRigibodyAndVelocity(bool isKinematic , bool useGravity , Vector2? velocity)
    {
        mViewObj.MyRigibody.isKinematic = isKinematic;
        mViewObj.MyRigibody.gravityScale = useGravity ? 60f : 0f;
        if (velocity != null)
        {
            ChangeVelocity(velocity.Value);
        }
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


public enum BallType
{
    IdleBall,       
    RunByGunBall,   //炮台发射的球
    ForceAddBall,   //强制增加的球，此球不引发掉落
}

