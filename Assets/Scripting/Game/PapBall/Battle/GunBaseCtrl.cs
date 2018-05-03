using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class GunBaseCtrl : MonoBehaviour {
    private Transform mTrans;
    private Vector3 mLastDir;
    private Window_BallBattle mParentWin;
    private bool IsMovingGun;

    public class ViewObj
    {
        public RectTransform GunRoot;
        public Transform WaitNextRoot;
        public Image GunImage;
        public ViewObj(UIViewBase view)
        {
            if (GunRoot != null) return;
            GunRoot = view.GetCommon<RectTransform>("GunRoot");
            WaitNextRoot = view.GetCommon<Transform>("WaitNextRoot");
            GunImage = view.GetCommon<Image>("GunImage");        }
    }    private ViewObj mViewObj;
    public List<BallBaseCtrl> WaitBallList = new List<BallBaseCtrl>();
    private BallBaseCtrl mCurWaitBall {
        get { return WaitBallList[0]; }
    }

    public void Init (Window_BallBattle parentWin , List<int> nextList)
	{
	    if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        mParentWin = parentWin;
        mTrans = transform;

        for (int i = 0; i < nextList.Count; i++)
        {
            if (nextList[i] < 0) nextList[i] = PlayerPrefsBridge.Instance.GetNextRandBall();
            CreateWaitBall(nextList[i], i);
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
                    Vector3 gunScreenPos = UIRootMgr.Instance.MyUICam.WorldToScreenPoint(mViewObj.GunRoot.position);
                    //mLastDir = hit.point - mTrans.position;
                    mLastDir = Input.mousePosition - gunScreenPos;
                    float angle = -Mathf.Atan(mLastDir.x / mLastDir.y) * Mathf.Rad2Deg;
                    mViewObj.GunRoot.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
        WaitBallList.RemoveAt(0);
        PlayerPrefsBridge.Instance.BallMapAcce.CurRound++;
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
            CreateWaitBall(ballIdx, WaitBallList.Count);
            for (int i = 0; i < WaitBallList.Count; i++)
            {
                SetBallPosByIndex(WaitBallList[i], i);
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

        WaitBallList.Insert(ballIndex,ballCtrl);
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
