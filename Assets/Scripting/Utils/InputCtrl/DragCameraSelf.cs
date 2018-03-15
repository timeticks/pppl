using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragCameraSelf : MonoBehaviour {
    public FingerGestureUtility m_Finger;
    public Camera myCamera;
    public float m_OffsetY;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float rotationDamping;
    public float heightDamping;
    public bool m_NeedLimit;
    public Vector4 m_PosLimit;      //x左，y右，z上，w下
    public bool m_canEnlargeCam = true;

    private Transform mTrans;
	void Start ()
    {
        mTrans = transform;
        if (myCamera == null)
            myCamera = GetComponent<Camera>();
        FingerGestureUtility.gestureEnlargeDelegate += new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
        FingerGestureUtility.scrollWheelDelegate += new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
        FingerGestureUtility.longTouchDelegate += new FingerGestureUtility.TouchLongGesture_Del(LongTouch);
        FingerGestureUtility.dragDelegate += new FingerGestureUtility.DragGesture_Del(DragToEnlargeCamera);
        FingerGestureUtility.touchOverDelegate += new FingerGestureUtility.Touch_Del(EndDragCamera);
	}


    public void EnlargeCamera(float enlargeOffset, float enlargeAmount)//缩放相机
    {
        if (!m_canEnlargeCam) return;
        if (myCamera != null)
        {
            if (myCamera.orthographic)
            {
                myCamera.orthographicSize -= 0.1f * enlargeOffset;
                if (myCamera.orthographicSize < 0.5) myCamera.orthographicSize = 3f;
            }
            else
            {
                mTrans.localPosition -= new Vector3(0, enlargeOffset*0.1f,0);
            }
        }
    }



    #region 战斗场景中，长按屏幕后，可进行摄像机拖拽

    private bool canDragCam = false;
    public void LongTouch(float amountTime)
    {
        if (!m_canEnlargeCam) return;
        if (Input.touchCount > 1) //如果
        {
            return;
        }
        else
        {
            if (Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
        }
        canDragCam = true;
    }

    void EndDragCamera()//长按结束，取消拖拽
    {
        canDragCam = false;
    }

    public void DragToEnlargeCamera(Vector2 dragOffset)
    {
        if (!m_canEnlargeCam) return;
        //if (!canDragCam) return;
        DragCamera(dragOffset);
    }
    public void DragCamera(Vector2 dragOffset)
    {
        height -= dragOffset.y * 0.3f;
        height = Mathf.Clamp(height, 10, 113);
        mTrans.localPosition += new Vector3(-dragOffset.x, 0, -dragOffset.y)*0.3f;
        //EnlargeCamera(dragOffset.y , 0);
        //TDebug.Log("DragToEnlargeCamera"+dragOffset);
    }
    #endregion
}
