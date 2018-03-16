using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 长按显示详细信息
/// </summary>
public class Part_TouchToDetail : MonoBehaviour
{
    public class ViewObj
    {
        public Text AnchorText;
        public Text DetailText;

        public ViewObj(UIViewBase view)
        {
            AnchorText = view.GetCommon<Text>("AnchorText");
            DetailText = view.GetCommon<Text>("DetailText");        }
    }

    private ViewObj mViewObj;


    EventTrigger m_trigger;
    string m_showString;
    void Start()
    {
        mViewObj = new ViewObj(GetComponent<UIViewBase>());
        m_trigger = gameObject.CheckAddComponent<EventTrigger>();
        if (m_trigger != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData>(BtnEvt_PressDown);
            entry.callback.AddListener(callback);
            m_trigger.triggers.Add(entry);
            FingerGestureUtility.touchOverDelegate += BtnEvt_PressUp;
        }
    }

    public void BtnEvt_PressDown(BaseEventData baseEvent)
    {
        TDebug.Log("按压开始");
        ShowDetail(true);
    }

    public void SetShowText(string showStr)
    {
        m_showString = showStr;
    }

    public virtual void ShowDetail(bool isTrue)
    {
        mViewObj.DetailText.transform.gameObject.SetActive(isTrue);
        if (isTrue)
        {
            mViewObj.DetailText.text = m_showString;
            mViewObj.DetailText.text = m_showString;
        }
    }

    public void BtnEvt_PressUp()
    {
        TDebug.Log("按压结束");
        ShowDetail(false);
    }

    void OnDestroy()
    {
        FingerGestureUtility.touchOverDelegate -= BtnEvt_PressUp;
    }
}
