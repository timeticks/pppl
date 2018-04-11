using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStop : MonoBehaviour
{
    private BallBaseCtrl mBallCtrl;

    public void Init(BallBaseCtrl ballCtrl)
    {
        mBallCtrl = ballCtrl;
    }


    void Update()
    {
        if (!mBallCtrl.IsInLegalPos()) //超出边界消失
        {
            UIRootMgr.Instance.TopMasking = false;
            mBallCtrl.ParentWin.DisableBall(mBallCtrl);
            Destroy(this);
        }
    }

    


    //当飞出的球碰到物体后
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ball") || col.gameObject.CompareTag("CenterAnchor")) //附着
        {
            mBallCtrl.Attach(col);
        }
        if (mBallCtrl.MyBallType == BallType.RunByGunBall)
        {
            if (col.gameObject.name.Contains("Border")) //反弹
            {
                Vector3 dir = new Vector3(-mBallCtrl.MoveDir.x, mBallCtrl.MoveDir.y, mBallCtrl.MoveDir.z);
                mBallCtrl.StartRun(dir , mBallCtrl.MyBallType);
            }
            else if (col.gameObject.name.Contains("Down")) //底部
            {
                Vector3 dir = new Vector3(mBallCtrl.MoveDir.x, -mBallCtrl.MoveDir.y, mBallCtrl.MoveDir.z);
                mBallCtrl.StartRun(dir, mBallCtrl.MyBallType);
            }
        }
    }
}
