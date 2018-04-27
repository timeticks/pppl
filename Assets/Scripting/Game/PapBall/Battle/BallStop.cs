﻿using System.Collections;
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
            TDebug.LogInEditor("超出边界，销毁");
            UIRootMgr.Instance.TopMasking = false;
            mBallCtrl.ParentWin.DisableBall(mBallCtrl);
            mBallCtrl.ParentWin.GunCtrl.CreateWaitBall(mBallCtrl.MyData.Num, true);
            Destroy(this);
        }
    }

    


    //当飞出的球碰到物体后
    void OnTriggerEnter2D(Collider2D col)
    {
        TDebug.Log(transform.parent.name + "  " + mBallCtrl.MyBallType.ToString());

        if (mBallCtrl.MyBallType == BallType.RunByGunBall)
        {
            if (col.gameObject.CompareTag("Ball") || col.gameObject.CompareTag("CenterAnchor")) //附着
            {
                mBallCtrl.Attach(col);
            }
            else if (col.gameObject.name.Equals(GameConstUtils.NAME_LEFT_BORDER) || col.gameObject.name.Equals(GameConstUtils.NAME_RIGHT_BORDER)) //反弹
            {
                Vector3 dir = new Vector3(-mBallCtrl.MoveDir.x, mBallCtrl.MoveDir.y, mBallCtrl.MoveDir.z);
                mBallCtrl.StartRun(dir, mBallCtrl.MyBallType);
            }
            else if (col.gameObject.name.Equals(GameConstUtils.NAME_TOP_BORDER)) //顶部
            {
                Vector3 dir = new Vector3(mBallCtrl.MoveDir.x, -mBallCtrl.MoveDir.y, mBallCtrl.MoveDir.z);
                mBallCtrl.StartRun(dir, mBallCtrl.MyBallType);
            }
            else if (col.gameObject.name.Equals(GameConstUtils.NAME_DOWN_BORDER))
            {
                
            }
        }
    }
}
