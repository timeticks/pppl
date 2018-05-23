using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

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
        public RectTransform MyRectTrans;
        public Rigidbody2D MyRigibody;
        public CircleCollider2D MyCollider;
        public Text TextNum;
        public GameObject WarnRoot;
        public ParticleSystem TrailParticle;
        
        public ViewObj(UIViewBase view)
        {
            if (MyImage != null) return;
            MyImage = view.GetCommon<Image>("MyImage");
            MyRectTrans = view.GetCommon<RectTransform>("Part_BattleBall");
            MyRigibody = view.GetCommon<Rigidbody2D>("Part_BattleBall");
            MyCollider = view.GetCommon<CircleCollider2D>("Part_BattleBall");
            TextNum = view.GetCommon<Text>("TextNum");
            WarnRoot = view.GetCommon<GameObject>("WarnRoot");
            TrailParticle = view.GetCommon<ParticleSystem>("TrailParticle");
        }
    }

    private ViewObj mViewObj;

    public void Init(Window_BallBattle parentWin , float scaleRatio , int ballIdx)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        MyTrans = transform;
        ParentWin = parentWin;
        SetBallIdx(ballIdx);
        SetRigibodyAndVelocity(true, false, Vector2.zero);
        mAttached = false;
        mViewObj.MyCollider.isTrigger = true;
        MyBallType = BallType.IdleBall;
        MyTrans.tag = GameConstUtils.TAG_UNTAGGED;
        mViewObj.MyRectTrans.localScale = Vector3.one * scaleRatio;
        mViewObj.WarnRoot.gameObject.SetActive(false);
        SetTrailActive(false, false);
    }

    public void SetBallIdx(int ballIdx)
    {
        Ball ball = Ball.Fetcher.GetBallCopy(ballIdx, false);
        MyData = new BallNodeData(ballIdx);
        SetBallIcon(ball.icon.ToString());
    }

    public void SetBallIcon(string icon)
    {
        mViewObj.TextNum.text = icon;
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

    //是否显示即将到达边界的警告
    public void SetWarnActive(bool isWarn)
    {
        if (mViewObj.WarnRoot.activeSelf!=isWarn)
            mViewObj.WarnRoot.SetActive(isWarn);
    }

    public void SetTrailActive(bool objActive , bool particleEmitting)
    {
        objActive = false; //TODO:不要尾迹
        if (mViewObj.TrailParticle.gameObject.activeSelf != objActive)
            mViewObj.TrailParticle.gameObject.SetActive(objActive);
        if (!particleEmitting)
            mViewObj.TrailParticle.Stop();
    }

    public void Attach(Collider2D otherCol)
    {
        if (mAttached)
        {
            TDebug.LogErrorFormat("已经在转盘上");
            return;
        }
        BallBaseCtrl otherBallCtrl = otherCol.GetComponent<BallBaseCtrl>();
        if ((otherBallCtrl == null || otherBallCtrl.MyData == null || otherBallCtrl.MyData.IsDisable) && !otherCol.gameObject.CompareTag(GameConstUtils.TAG_CENTER_ANCHOR))
            return;

        UIRootMgr.Instance.TopMasking = false;

        mViewObj.MyCollider.isTrigger = true;
        Vector3 lastMoveDir = MoveDir;
        SetRigibodyAndVelocity(true, false, Vector2.zero);
        mAttached = true;
        SetTrailActive(true, false);
        Destroy(GetComponent<BallStop>());

        Vector3 localPosWithCenter = ParentWin.InverseTransformPointWithCenter(MyTrans); //球相对于中心球的位置
        XyCoordRef xyPos = ParentWin.MapData.GetNearestXy(new Vector2(localPosWithCenter.x, localPosWithCenter.y), true);

        Vector3 hitDir = otherCol.transform.position - MyTrans.position;
        float dot = Vector3.Dot(hitDir.normalized, lastMoveDir.normalized);
        Vector3 boundDir = Vector3.one;
        if (dot <= 0.01f) 
            boundDir = lastMoveDir;
        else
            boundDir = lastMoveDir - Vector3.Dot(hitDir.normalized, lastMoveDir.normalized) * hitDir.normalized * 2;   //反射方向
        //TDebug.LogInEditorF("方向 hitDir:{0}   lastMoveDir:{1}   boundDir:{2}   dot:{3}", hitDir, lastMoveDir * 10, boundDir, dot);

        //得到与col的相对位置
        //HexagonPosType posType = HexagonGridMgr.CurHexagon.GetHexagonPosType(Vector2.zero, offsetPos);
        //TDebug.LogFormat("xy:{0}" , xyPos.ToString());
        ParentWin.AddBall(this, xyPos);
        PlayerPrefsBridge.Instance.BallMapAcce.FireBallAmount++;

        bool isDestroyEqual = false;
        if (MyBallType == BallType.RunByGunBall)
        {
            isDestroyEqual = ParentWin.DestroyEqualNum(MyData, new Vector2(boundDir.x, boundDir.y));
            if (!isDestroyEqual)
                ParentWin.FreshMutilDown(true);
        }

        ParentWin.StartRot(MyTrans.position, lastMoveDir);
        MyBallType = BallType.IdleBall;
        ParentWin.GunCtrl.CreateWaitBall(-1, false);
        PlayerPrefsBridge.Instance.saveMapAccessor();
        ParentWin.ResetFrozen();
        if (isDestroyEqual)
        {
            //爆炸特效
            DestroySelf exploEffect = GameAssetsPool.Instance.GetEffect("FE_BallColExplosion");
            TUtility.SetParent(exploEffect.transform, MyTrans.parent);//如果会消除，则特效不跟随球体走
            exploEffect.transform.position = (otherCol.transform.position + MyTrans.position) / 2;
            exploEffect.gameObject.SetActive(true);

            //掉落光点
            DestroySelf lootItem = GameAssetsPool.Instance.GetEffect("FE_LootItem");
            lootItem.gameObject.SetActive(true);
            TUtility.SetParent(lootItem.transform, Window_BallBattle.Instance.transform);//如果会消除，则特效不跟随球体走
            lootItem.transform.DOMove(Window_BallBattle.Instance.mViewObj.ScoreText.transform.position, 0.7f)
                .OnComplete(
                    delegate() { lootItem.Destroy(); });
        }
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

    //碰到边界
    void OnTriggerEnter2D(Collider2D col)
    {
        if (MyData == null) return;
        if (!MyData.IsDisable && MyBallType == BallType.IdleBall && gameObject.CompareTag(GameConstUtils.TAG_BALL))
        {
            if (col.gameObject.name.Contains(GameConstUtils.NAME_BORDER)) //碰到边界
            {
                int mapIdx = PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx;
                PlayerPrefsBridge.Instance.BallMapAcce.CurMapIdx = 0;
                PlayerPrefsBridge.Instance.saveMapAccessor();
                UIRootMgr.Instance.OpenWindow<Window_MapEndShow>(WinName.Window_MapEndShow).OpenWindow(mapIdx);
                TDebug.LogError("失败了");
            }
        }
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }

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

