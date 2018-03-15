using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraCtrl : MonoBehaviour 
{
    public static Vector4[] LimitList = new Vector4[] {new Vector4(210,-470,-121,-257) };
    public Transform followTarget;
    public Camera myCamera;
    public float m_OffsetY;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float rotationDamping;
    public float heightDamping;
    public bool m_NeedLimit;
    public Vector4 m_PosLimit;      //x左，y右，z上，w下
    internal bool m_canEnlargeCam=true;

    private Transform mTrans;
    void Start()
    {
        if (myCamera == null)
            myCamera = GetComponent<Camera>();
        m_canEnlargeCam = true;
        mTrans = transform;
    }
    public void CanEnlargeGestureToCam()
    {
        //if (AppBridge.instance != null)//注册缩放事件
        //{
        //    FingerGestureUtility.gestureEnlargeDelegate += new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
        //    FingerGestureUtility.scrollWheelDelegate += new FingerGestureUtility.EnlargeGesture_Del(EnlargeCamera);
        //}
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


	void Update () 
    {
	
	}

    void LateUpdate()
    {
        FreshFollow();
    }

    void FreshFollow()//跟随target移动
    {
        if (!followTarget)
            return;
        float wantedRotationAngle = followTarget.eulerAngles.y;
        float wantedHeight = followTarget.position.y + m_OffsetY + height;
        float currentRotationAngle = mTrans.eulerAngles.y;
        float currentHeight = mTrans.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        mTrans.position = followTarget.position;
        mTrans.position -= currentRotation * Vector3.forward * distance;
        mTrans.position = new Vector3(mTrans.position.x, currentHeight, mTrans.position.z);
        mTrans.LookAt(new Vector3(followTarget.position.x, followTarget.position.y + m_OffsetY, followTarget.position.z));
        LimitCamPos();
    }

    void LimitCamPos()//摄像机区域限制
    {
        if (m_NeedLimit && !m_PosLimit.Equals(Vector4.zero))
        {
            mTrans.position = new Vector3(Mathf.Clamp(mTrans.position.x, m_PosLimit.y, m_PosLimit.x), mTrans.position.y, Mathf.Clamp(mTrans.position.z, m_PosLimit.w, m_PosLimit.z));
        }
    }

    public void EnlargeCamera(float enlargeOffset , float enlargeAmount)//缩放相机
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
                float xyRatio = distance / height;
                distance -= xyRatio * enlargeOffset * 0.1f;
                distance = Mathf.Clamp(distance, xyRatio * 25, xyRatio * 115);
                height -=  enlargeOffset * 0.1f;
                height = Mathf.Clamp(height, 25, 115);
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
        if (!canDragCam) return;
        DragCamera(dragOffset);
    }
    public void DragCamera(Vector2 dragOffset)
    {
        height -= dragOffset.y * 0.3f;
        height = Mathf.Clamp(height, 10, 113);
        mTrans.rotation *= Quaternion.Euler(0, dragOffset.x * 0.15f, 0);
        //EnlargeCamera(dragOffset.y , 0);
        //TDebug.Log("DragToEnlargeCamera"+dragOffset);
    }
    #endregion



    void OnDestroy()
    {
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

    private IEnumerator shakeCameraCor;
    public void ShockCam()
    {
        if (m_isShaking)
        {
            transform.position = m_lastPos;
            transform.rotation = m_lastRot;
        }
        if (shakeCameraCor != null)
            StopCoroutine(shakeCameraCor);
        shakeCameraCor = ShakeCamera();
        StartCoroutine(shakeCameraCor);
    }
    Vector3 m_lastPos ;
    Quaternion m_lastRot;
    bool m_isShaking = false;
    float m_shakeStrength = 0.1f;
    float m_rate = 2f;
    float m_shakeTime = 0.4f;
    public IEnumerator ShakeCamera()
    {
        m_isShaking = true;
        float shake_intensity = m_shakeTime;
        Vector3 orgPosition = transform.position;
        m_lastPos = orgPosition;
        Quaternion originRotation = transform.rotation;
        m_lastRot = originRotation;
        while (shake_intensity >0)
        {
            transform.position = orgPosition + Random.insideUnitSphere * m_shakeStrength * m_rate;
            transform.rotation = new Quaternion(
            originRotation.x + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength,
            originRotation.y + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength,
            originRotation.z + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength,
            originRotation.w + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength);
            shake_intensity -= Time.deltaTime;
            yield return null;
        }
        transform.position = orgPosition;
        transform.rotation = originRotation;
        m_isShaking = false;
    }

    
    
}
