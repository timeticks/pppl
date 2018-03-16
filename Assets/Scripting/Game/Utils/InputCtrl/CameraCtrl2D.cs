using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class CameraCtrl2D : MonoBehaviour 
{
    public Transform followTarget;
    internal Camera myCamera;
    public float m_OffsetY;
    public float height = 5.0f;
    public float followDamping;
    public bool m_NeedLimit;
    public Vector4 m_PosLimit;      //x上，y下，z左，w右
    internal bool m_canEnlargeCam = true;
    private Transform mTrans;
    void Awake()
    {
        if (myCamera == null)
            myCamera = GetComponent<Camera>();
        mTrans = transform;
    }


    public void Init()
    {
        //CanEnlargeGestureToCam();
    }

    public void CanEnlargeGestureToCam()
    {
        if (AppBridge.Instance != null)//注册缩放事件
        {
            FingerGestureUtility.gestureEnlargeDelegate += new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
            FingerGestureUtility.scrollWheelDelegate += new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
        }
    }

    public void CanLongTouchToCam()//注册长按拖拽事件
    {
        //if (AppBridge.instance != null)
        //{
        //    FingerGestureUtility.longTouchDelegate += new FingerGestureUtility.TouchLongGesture_Del(LongTouch);
        //    FingerGestureUtility.dragDelegate += new FingerGestureUtility.DragGesture_Del(DragToEnlargeCamera);
        //    FingerGestureUtility.touchOverDelegate += new FingerGestureUtility.Touch_Del(EndDragCamera);
        //}
    }

    //void LateUpdate()
    //{
       
    //    FreshFollow();
    //}

    void FreshFollow()//跟随target移动
    {
        if (!followTarget)
            return;
        float wantedRotationAngle = followTarget.eulerAngles.y;
        float wantedHeight = followTarget.position.y + m_OffsetY + height;
        float currentHeight = mTrans.position.y;

        Vector3 wantedPos = new Vector3(followTarget.position.x, currentHeight, followTarget.position.z);
        Vector3 movePos = Vector3.Lerp(mTrans.position, wantedPos, followDamping * Time.deltaTime);
        mTrans.position = movePos;
        LimitCamPos();
    }

    void LimitCamPos()//摄像机区域限制
    {
        if (m_NeedLimit && !m_PosLimit.Equals(Vector4.zero))
        {
            mTrans.position = new Vector3(Mathf.Clamp(mTrans.position.x, m_PosLimit.z, m_PosLimit.w), mTrans.position.y, Mathf.Clamp(mTrans.position.z, m_PosLimit.y, m_PosLimit.x));
        }
    }

    public void EnlargeCamera(float enlargeOffset, float enlargeAmount)//缩放相机
    {
        if (!m_canEnlargeCam) return;
        if (myCamera != null)
        {
            if (myCamera.orthographic)
            {
                myCamera.orthographicSize -= 0.01f * enlargeOffset;
                myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, 3, 10);

                //InitCamLimit(new Vector4(0, -Tiled2Unity.TiledMap.Instance.NumTilesHigh, 0, Tiled2Unity.TiledMap.Instance.NumTilesWide));
            }
        }
    }

    /// <summary>
    /// 进行摄像机移动限制
    /// </summary>
    /// <param name="limitRect"></param>
    public void InitCamLimit(Vector4 limitRect)
    {
        if (myCamera.orthographic)
        {
            m_NeedLimit = true;
            m_PosLimit = GetCamMoveRange(myCamera, limitRect);
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
        if (!canDragCam) return;
        DragCamera(dragOffset);
    }
    public void DragCamera(Vector2 dragOffset)
    {
        height -= dragOffset.y * 0.3f;
        height = Mathf.Clamp(height, 10, 113);
        mTrans.rotation *= Quaternion.Euler(0, dragOffset.x * 0.15f, 0);
    }
    #endregion





    /// <summary>
    /// 返回正交垂直摄像机的移动区域，根据可显示的范围和正交size
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="limitRect">可显示的区域范围。x上，y下，z左，w右</param>
    /// <returns>正交垂直摄像机的移动区域。x上，y下，z左，w右</returns>
    public static Vector4 GetCamMoveRange(Camera cam, Vector4 limitRect)
    {
        Vector4 posLimit = new Vector4();
        float yOffset = cam.orthographicSize;
        float xOffset = cam.orthographicSize * Screen.width / Screen.height;

        posLimit.x = limitRect.x - yOffset;
        posLimit.y = limitRect.y + yOffset;
        posLimit.z = limitRect.z + xOffset;
        posLimit.w = limitRect.w - xOffset;

        if (posLimit.x < posLimit.y || posLimit.w <= posLimit.z)
        {
            posLimit = new Vector4((posLimit.x + posLimit.y) / 2, (posLimit.x + posLimit.y) / 2, (posLimit.w + posLimit.z) / 2, (posLimit.w + posLimit.z) / 2);
        }

        return posLimit;
    }


    void OnDestroy()
    {
        if (AppBridge.Instance == null) return;
        if (FingerGestureUtility.gestureEnlargeDelegate != null)
            FingerGestureUtility.gestureEnlargeDelegate -= new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
        if (FingerGestureUtility.scrollWheelDelegate != null)
            FingerGestureUtility.scrollWheelDelegate -= new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
        if (FingerGestureUtility.longTouchDelegate != null)
            FingerGestureUtility.longTouchDelegate -= new FingerGestureUtility.TouchLongGesture_Del(LongTouch);
        if (FingerGestureUtility.dragDelegate != null)
            FingerGestureUtility.dragDelegate -= new FingerGestureUtility.DragGesture_Del(DragToEnlargeCamera);
        if (FingerGestureUtility.touchOverDelegate != null)
            FingerGestureUtility.touchOverDelegate -= new FingerGestureUtility.Touch_Del(EndDragCamera);
    }
}
