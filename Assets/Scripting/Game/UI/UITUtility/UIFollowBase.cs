using UnityEngine;
using System.Collections;

/// <summary>
/// UI控件跟随Transform或Vector3
/// </summary>
public class UIFollowBase : MonoBehaviour {
    protected Transform m_myFollowTarget;
    protected Vector3 m_myFollowTargetPos;

    protected Camera m_worldCam;
    protected Camera m_uiCam;
    protected Transform m_myTrans;
    protected Vector2 m_worldOffset;         //与跟随物体的偏差
    protected Vector2 m_uiOffset;            //UI界面上面的偏差
    protected float m_offsetForPerspective;  //对于3D场景，是否要对靠近屏幕边缘的进行偏移调整

    protected bool m_IsVisable = true;       //是否可见
    protected bool m_isFollowByPos;          //是否只是跟随一个静态坐标
    protected bool m_isFalseWhenNoFollow=true;  //当没有跟随时，是否隐藏
    internal bool m_SetCanFollow 
    {
        get{return m_canFollow;} 
        set 
        {
            m_canFollow = value;
            if (m_canFollow && m_myFollowTarget!=null) Follow(m_myFollowTarget.position); 
        } 
    }
    private bool m_canFollow = true;
 	void Awake () 
    {
        //gameObject.GetComponent<RectTransform>();
	}


    public void Init(Camera worldCam, Camera uiCam, Transform myFollowTarget, Vector2 worldOffset , Vector2 uiOffset, float offsetForPerspective=0)
    {
        m_myTrans = transform;
        m_isFollowByPos = false;
        gameObject.SetActive(true); 
        m_worldCam = worldCam;
        m_uiCam = uiCam;
        m_myFollowTarget = myFollowTarget;
        m_worldOffset = worldOffset;
        m_uiOffset = uiOffset;
        m_offsetForPerspective = offsetForPerspective;
        enabled = true;
        Follow(m_myFollowTarget.position); 
    }
    protected void Init(Camera worldCam, Camera uiCam, Transform myFollowTarget)
    {
        m_myTrans = transform;
        m_isFollowByPos = false;
        gameObject.SetActive(true); 
        m_worldCam = worldCam;
        m_uiCam = uiCam;
        m_myFollowTarget = myFollowTarget;
        m_worldOffset = Vector2.zero;
        m_offsetForPerspective = 0;
        enabled = true;
        Follow(m_myFollowTarget.position);
    }

    /// <summary>
    /// 跟随一个静态坐标
    /// </summary>
    protected void Init(Camera worldCam, Camera uiCam, Vector3 myFollowTarget, Vector2 worldOffset, Vector2 uiOffset, float offsetForPerspective)
    {
        m_myTrans = transform;
        m_isFollowByPos = true;
        m_worldCam = worldCam;
        m_uiCam = uiCam;
        m_myFollowTargetPos = myFollowTarget;
        m_worldOffset = worldOffset;
        m_uiOffset = uiOffset;
        m_offsetForPerspective = offsetForPerspective;
        enabled = true;
        Follow(m_myFollowTargetPos); 
    }


 	void LateUpdate () 
    {
        if (!m_canFollow) return;
        if (m_worldCam == null || m_uiCam==null)
        {
            if (m_isFalseWhenNoFollow)
                gameObject.SetActive(false);
            return; 
        }
        if (m_isFollowByPos)
        {
            Follow(m_myFollowTargetPos);
        }
        else
        {
            if (m_myFollowTarget == null)
            {
                if (m_isFalseWhenNoFollow)
                    gameObject.SetActive(false);
                return; 
            }
            Follow(m_myFollowTarget.position);
        }
	}

    void OnBecameInvisible()
    {
        TDebug.Log("OnBecameInvisible");
        m_IsVisable = false;
    }
    void OnBecameVisible()
    {
        TDebug.Log("OnBecameInvisible");
        m_IsVisable = true;
    }

    void Follow(Vector3 targetPos)
    {
        if (!m_IsVisable || m_worldCam == null || m_uiCam==null) return;
        Vector3 newPos = m_worldCam.WorldToScreenPoint(new Vector3(targetPos.x + m_worldOffset.x, targetPos.y + m_worldOffset.y, targetPos.z));
        float screenOffset = newPos.x / Screen.width - 0.5f;
        m_myTrans.position = m_uiCam.ScreenToWorldPoint(newPos);
        m_myTrans.localPosition += new Vector3(m_offsetForPerspective * screenOffset + m_uiOffset.x, m_uiOffset.y, -m_myTrans.localPosition.z);
    }
}
