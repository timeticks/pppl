using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GunBaseCtrl : MonoBehaviour {
    private Transform mTrans;
    private Vector3 mLastDir;
    private Window_BallBattle mParentWin;
    private bool IsMovingGun;

    public class ViewObj
    {
        public RectTransform Gun;
        public Transform WaitNextRoot;
        public ViewObj(UIViewBase view)
        {
            if (Gun != null) return;
            Gun = view.GetCommon<RectTransform>("Gun");
            WaitNextRoot = view.GetCommon<Transform>("WaitNextRoot");        }
    }    private ViewObj mViewObj;
    private List<BallBaseCtrl> mWaitBallList = new List<BallBaseCtrl>();
    private BallBaseCtrl mCurWaitBall {
        get { return mWaitBallList[0]; }
    }

    public void Init (Window_BallBattle parentWin)
	{
	    if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        mParentWin = parentWin;
        mTrans = transform;
        for (int i = 0; i < 3; i++)
        {
            CreateWaitBall(PlayerPrefsBridge.Instance.GetNextRandBall() , i);
        }
	    
        IsMovingGun = false;

	}

	void Update ()
    {
        if (IsMovingGun)//方向跟随摄像机
        {
            if (Input.GetMouseButtonUp(0))
            {
                GunFireBall();
            }
            else
            {
                //Ray ray = UIRootMgr.Instance.MyUICam.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("UI")))
                {
                    Vector3 gunScreenPos = UIRootMgr.Instance.MyUICam.WorldToScreenPoint(mTrans.position);
                    //mLastDir = hit.point - mTrans.position;
                    mLastDir = Input.mousePosition - gunScreenPos;
                    float angle = -Mathf.Atan(mLastDir.x / mLastDir.y) * Mathf.Rad2Deg;
                    mViewObj.Gun.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
            }
        }
	}

    public void BtnEvt_BgPointerDown(BaseEventData data)
    {
        IsMovingGun = true;
    }
    public void GunFireBall()
    {
        UIRootMgr.Instance.TopMasking = true;
        IsMovingGun = false;
        mCurWaitBall.StartRun(mLastDir , BallType.RunByGunBall);
        mWaitBallList.RemoveAt(0);
    }

    /// <summary>
    /// 是否是取消上次发射，将球还原
    /// </summary>
    public void CreateWaitBall(int ballIdx , bool isCancelFire)
    {
        if (isCancelFire)
            CreateWaitBall(ballIdx, 0);
        else
        {
            CreateWaitBall(ballIdx, mWaitBallList.Count);
            for (int i = 0; i < mWaitBallList.Count; i++)
            {
                SetBallPosByIndex(mWaitBallList[i], i);
            }
        }
    }

    //生成新的待发射球
    void CreateWaitBall(int ballIdx , int ballIndex)
    {
        BallBaseCtrl ballCtrl = mParentWin.GetNewBall(ballIdx);
        ballCtrl.MyTrans.SetParent(mViewObj.WaitNextRoot);
        ballCtrl.MyTrans.rotation = Quaternion.identity;

        SetBallPosByIndex(ballCtrl, ballIndex);

        mWaitBallList.Insert(ballIndex,ballCtrl);
    }

    //根据index，设置大小和位置
    void SetBallPosByIndex(BallBaseCtrl ball, int ballIndex)
    {
        if (ballIndex == 0)
        {
            ball.MyTrans.localPosition = Vector3.zero;
            ball.MyTrans.localScale = Vector3.one * mParentWin.MapData.BallScaleRatio();
        }
        else if (ballIndex == 1)
        {
            ball.MyTrans.localPosition = new Vector3(0, -60 * mParentWin.MapData.BallScaleRatio(), 0);
            ball.MyTrans.localScale = Vector3.one * mParentWin.MapData.BallScaleRatio() * 0.8f;
        }
        else if (ballIndex == 2)
        {
            ball.MyTrans.localPosition = new Vector3(0, -120 * mParentWin.MapData.BallScaleRatio(), 0);
            ball.MyTrans.localScale = Vector3.one * mParentWin.MapData.BallScaleRatio() * 0.8f;
        }
    }
}
