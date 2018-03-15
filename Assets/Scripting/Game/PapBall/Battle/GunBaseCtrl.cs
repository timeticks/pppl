using UnityEngine;
using System.Collections;

public class GunBaseCtrl : MonoBehaviour {
    public GameObject m_BallPrefab;

    private Transform mTrans;
    private Vector3 mLastDir;
    private BallBaseCtrl mCurWaitBall;
	void Awake () 
    {
        mTrans = transform;
	}

    void Start()
    {
        InitWaitBall();
    }
	
	void Update () 
    {
        if (Input.GetMouseButton(0))//方向跟随摄像机
        {
            Ray ray = BattleSceneUIMgr.instance.m_UICamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("BgPanel")))
            {
                mLastDir = hit.point - mTrans.position;
                float angle = -Mathf.Atan(mLastDir.x / mLastDir.y) * Mathf.Rad2Deg;
                mTrans.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
	}


    public void BtnEvt_FireGun()//发射
    {
        if (mCurWaitBall != null)
        {
            mCurWaitBall.StartRun(mLastDir);
        }
        InitWaitBall();
    }
    void InitWaitBall()
    {
        BallBaseCtrl ballCtrl = BattleSceneMgr.instance.m_BallGroupMgr.GetNewBall();
        ballCtrl.transform.SetParent(mTrans.parent);
        ballCtrl.transform.position = mTrans.position;
        ballCtrl.transform.localScale = Vector3.one;
        ballCtrl.transform.localPosition += Vector3.up * 50;
        mCurWaitBall = ballCtrl;
    }

}
