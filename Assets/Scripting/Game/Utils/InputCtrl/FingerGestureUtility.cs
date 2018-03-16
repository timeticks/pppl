using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class FingerGestureUtility : MonoBehaviour {

    public delegate void EnlargeGesture_Del(float enlargeValue , float enlargeAmount);
    public delegate void Touch_Del();                             //点击屏幕，并且没有在UI上
    public delegate void DragGesture_Del(Vector2 dragOffset);     //拖拽
    public delegate void TouchLongGesture_Del(float amountTime);  //长按

    public static EnlargeGesture_Del gestureEnlargeDelegate;      //两手指，缩放
    public static EnlargeGesture_Del scrollWheelDelegate;         //滚轮滑动，模拟缩放手势
    public static Touch_Del touchDelegate;                        //点击屏幕
    public static Touch_Del touchDelegateInUI;                    //点击屏幕，点在UI上
    public static TouchLongGesture_Del longTouchDelegate;         //长按屏幕
    public static DragGesture_Del dragDelegate;                   //拖拽
    public static TouchLongGesture_Del longTouch_IgnoreUIDelegate;
    public static DragGesture_Del drag_IgnoreUIDelegate;          //拖拽，不在UI上
    public static Touch_Del touchOverDelegate;                    //抬起长按

    //左右滑动移动速度
    float xSpeed = 250.0f;
    float ySpeed = 120.0f;

    //摄像头的位置
    float x = 0.0f;
    float y = 0.0f;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 m_oldPosition1=Vector2.zero;
    private Vector2 m_oldPosition2=Vector2.zero;

    private float m_enlargeAmount;    //这一次的缩放手势移动总量
    private float m_srollWheelAmount; //滑轮总量

    private float m_touchAmountTime;  //长按时间
    private Vector2 m_oldDragedPos=Vector2.zero;   //拖拽记录
    private bool m_CanLongTouch = false;

    private float m_touchAmountTime_IgnoreUI;  //长按时间
    private Vector2 m_oldDragedPos_IgnoreUI = Vector2.zero;   //拖拽记录
    private bool m_CanLongTouch_IgnoreUI = false;
    
    //初始化游戏信息设置
    void Awake () {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

    }



    void Update()
    {
        //判断触摸数量为单点触摸
        if (Input.touchCount == 1)
        {
            //触摸类型为移动触摸
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //根据触摸点计算X与Y位置
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
        }

        //判断触摸数量为多点触摸
        if (Input.touchCount > 1 && !IsTouchOnUI(0) && !IsTouchOnUI(1))
        {
            //前两只手指触摸类型都为移动触摸
            if ((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
            {
                //计算出当前两点触摸点的位置
                Vector2 tempPosition1 = Input.GetTouch(0).position;
                Vector2 tempPosition2 = Input.GetTouch(1).position;

                if (m_oldPosition1 != Vector2.zero && m_oldPosition2 != Vector2.zero)
                {
                    //函数返回>0为放大，返回<0为缩小
                    float enlargeNum = isEnlarge(m_oldPosition1, m_oldPosition2, tempPosition1, tempPosition2);
                    m_enlargeAmount += enlargeNum;
                    if (gestureEnlargeDelegate != null)
                    {
                        gestureEnlargeDelegate(enlargeNum, m_enlargeAmount);
                    }
                }
                m_oldPosition1 = tempPosition1;//备份上一次触摸点的位置，用于对比
                m_oldPosition2 = tempPosition2;
            }
        }
        else//重置信息
        {
            m_oldPosition1 = Vector2.zero;
            m_oldPosition2 = Vector2.zero;
            m_enlargeAmount = 0;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && scrollWheelDelegate != null)
        {
            m_srollWheelAmount += Input.GetAxis("Mouse ScrollWheel");
            scrollWheelDelegate(Input.GetAxis("Mouse ScrollWheel") * 1000, m_srollWheelAmount * 1000);
        }

        #region 长按与拖拽
        if (CheckTouchLong_NoOnUI() && m_CanLongTouch)
        {
            if (dragDelegate != null)
            {
                Vector2 tempPosition = GetCurMousePos();
                if (m_oldDragedPos != Vector2.zero)
                {
                    dragDelegate(tempPosition - m_oldDragedPos);
                }
                m_oldDragedPos = tempPosition;
            }
            m_touchAmountTime += Time.deltaTime;
            if (m_touchAmountTime > 0.15)
            {
                if (longTouchDelegate != null) { longTouchDelegate(m_touchAmountTime); }
            }
        }
        else
        {
            m_touchAmountTime = 0f;
            m_oldDragedPos = Vector2.zero;
        }

        if (CheckTouchLong_IgnoreUI() && m_CanLongTouch_IgnoreUI)//不考虑ui层
        {
            m_touchAmountTime_IgnoreUI += Time.deltaTime;
            if (m_touchAmountTime_IgnoreUI > 0.15)
            {
                if (longTouch_IgnoreUIDelegate != null)
                {
                    longTouch_IgnoreUIDelegate(m_touchAmountTime);
                }
            }
            if (drag_IgnoreUIDelegate != null)
            {
                Vector2 tempPosition = GetCurMousePos();
                if (m_oldDragedPos_IgnoreUI != Vector2.zero)
                {
                    drag_IgnoreUIDelegate(tempPosition - m_oldDragedPos_IgnoreUI);
                }
                m_oldDragedPos_IgnoreUI = tempPosition;
            }
        }
        else
        {
            m_touchAmountTime_IgnoreUI = 0f;
            m_oldDragedPos_IgnoreUI = Vector2.zero;
        }
        #endregion


        if (CheckMouseDown())
        {
            if (!IsClickOnUI())
            {
                m_CanLongTouch = true;   //开始按钮时，点击在UI上则不进行长按计时
                if (touchDelegate != null) { touchDelegate(); }
            }
            else//点击在UI上
            {
                if (touchDelegateInUI != null) { touchDelegateInUI(); }
            }
            m_CanLongTouch_IgnoreUI = true;
        }
        if (CheckMouseUp())
        {
            m_CanLongTouch = false;
            m_CanLongTouch_IgnoreUI = false;
            if (touchOverDelegate != null) touchOverDelegate();
        }
       
    }


    //函数返回真为放大，返回假为缩小
    public static float isEnlarge(Vector2 oP1 ,Vector2 oP2,Vector2 nP1,Vector2 nP2)
    {
    	//函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        float leng1 =Mathf.Sqrt((oP1.x-oP2.x)*(oP1.x-oP2.x)+(oP1.y-oP2.y)*(oP1.y-oP2.y));
        float leng2 =Mathf.Sqrt((nP1.x-nP2.x)*(nP1.x-nP2.x)+(nP1.y-nP2.y)*(nP1.y-nP2.y));
        if(leng1<leng2)
        {
            return leng2 - leng1; //放大手势
        }
        else
        {
            return leng2 - leng1;//缩小手势
        }
    }

    public static Vector2 GetCurMousePos()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#else
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        return Vector2.zero;
#endif
    }



    public static Vector2 GetCurMousePos(int touchIndex)
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#else
        if (Input.touchCount > 0)
        {
             return Input.touchCount > touchIndex?Input.GetTouch(touchIndex).position:Input.GetTouch(0).position;
        }
        return Vector2.zero;
#endif
    }


    public static bool CheckDrag_NoOnUI()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
            return true;
        return false;
#else
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            if (!IsClickOnUI())
            {
                return true;
            }
        }
        return false;
#endif
    }

    public static bool CheckTouchLong_NoOnUI()
    { 
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
            return true;
        return false;
#else
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            if (!IsClickOnUI())
            {
                return true;
            }
        }
        return false;
#endif
    }

    #region 长按，不考虑UI
    public static bool CheckTouchLong_IgnoreUI()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
            return true;
        return false;
#else
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            //if (!IsClickOnUI())
            {
                return true;
            }
        }
        return false;
#endif
    }
    #endregion

    public static bool CheckMouseDown()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            return true;
        return false;
#else
        if (Input.touchCount != 0 && (Input.GetTouch(0).phase == TouchPhase.Began || (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)))
        {
            return true;
        }
        return false;
#endif
    }

    public static bool CheckMouseDown_NoOnUI()
    {
        return CheckMouseDown() && !IsClickOnUI() && Input.touchCount<=1;
    }
    public static bool CheckMouseUp_NoOnUI()
    {
        return CheckMouseUp() && !IsClickOnUI() && Input.touchCount <= 1;
    }

    public static bool CheckMouseUp()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
            return true;
        return false;
#else
        if (Input.touchCount ==1 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
        {
            return true;
        }
        return false;
#endif
    }


    public static bool IsClickOnUI()//点击在UI上
    {
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        if (Input.touchCount > 0) 
        {
            return IsPointerOverGameObject(Input.GetTouch(0).position);
        }
        return false;
#endif

    }
    public static bool IsPointerOverGameObject(Vector2 screenPosition)
    {
        Canvas canvas = UIRootMgr.Instance.MyCanvas;
        //实例化点击事件  
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //将点击位置的屏幕坐标赋值给点击事件  
        eventDataCurrentPosition.position = screenPosition;
        //获取画布上的 GraphicRaycaster 组件  
        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();

        List<RaycastResult> results = new List<RaycastResult>();
        // GraphicRaycaster 发射射线  
        uiRaycaster.Raycast(eventDataCurrentPosition, results);

        return results.Count > 0;
    }  
    public static bool IsTouchOnUI(int touchIndex)
    {
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current.IsPointerOverGameObject(touchIndex);
#endif

    }

}
