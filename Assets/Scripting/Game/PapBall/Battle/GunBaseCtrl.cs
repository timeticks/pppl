using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GunBaseCtrl : MonoBehaviour {
    private Transform mTrans;
    private Vector3 mLastDir;
    private BallBaseCtrl mCurWaitBall;
    private Window_BallBattle mParentWin;
    private bool IsMovingGun;
	public void Init (Window_BallBattle parentWin)
	{
	    mParentWin = parentWin;
        mTrans = transform;
	    InitWaitBall();
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
                Ray ray = UIRootMgr.Instance.MyUICam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("UI")))
                {
                    Vector3 gunScreenPos = UIRootMgr.Instance.MyUICam.WorldToScreenPoint(mTrans.position);
                    //mLastDir = hit.point - mTrans.position;
                    mLastDir = Input.mousePosition - gunScreenPos;
                    float angle = -Mathf.Atan(mLastDir.x / mLastDir.y) * Mathf.Rad2Deg;
                    mTrans.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
        IsMovingGun = false;
        if (mCurWaitBall != null)
        {
            mCurWaitBall.StartRun(mLastDir);
        }
        InitWaitBall();
    }
    void InitWaitBall()
    {
        BallBaseCtrl ballCtrl = mParentWin.GetNewBall();
        ballCtrl.transform.SetParent(mTrans.parent);
        ballCtrl.transform.position = mTrans.position;
        ballCtrl.transform.localScale = Vector3.one;
        ballCtrl.transform.localPosition += Vector3.up * 50;
        mCurWaitBall = ballCtrl;
    }

}
