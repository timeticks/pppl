using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIAnimationBaseCtrl : MonoBehaviour
{
    public GameObject m_AnimationObj;
    public bool m_PlayOnEnable;
    public bool m_PlayEndInactive;
    public bool m_InactiveDestory;
    public bool m_IgnoreTimeScale;      //忽视时间加快或变慢，但无法忽视timeScale=0
    public bool m_ResetStartWhenPlay;     //当开始播放时，先重置到动画初始值。窗口等打开动画一般需要勾选，以免因为协程导致动画跳乱。
     
    public List<AnimationData> m_AnimationData;

    public delegate void PlayOverCallBack();
    public PlayOverCallBack m_OverCallBack;
    public AnimationSaveData m_SaveStartData = null;

    void OnEnable()
    {
        if (m_PlayOnEnable)
        { DoSelfAnimation(); }
    }

    void OnDisable()
    {
        StopAnimation();
    }

    public void DoSelfAnimation()//播放自身的动画信息
    {
        StopAnimation();
        if (m_ResetStartWhenPlay)
            ResetByOriginData();
        m_AnimationObj.gameObject.SetActive(true);
        if (m_SaveStartData == null) m_SaveStartData = new AnimationSaveData();
        StartCoroutine(DoAniListCoroutine());
    }

    public void DoAnimation(AnimationData data)
    {
        StopAnimation();
        m_AnimationObj.gameObject.SetActive(true);
        if (m_SaveStartData == null) m_SaveStartData = new AnimationSaveData();
        m_curLeaveAniCount = 1;
        StartCoroutine(DoAnimtionCoroutine(data));
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
    }

    public void ResetByOriginData()
    {
        Transform trans = m_AnimationObj.transform;
        for (int i = 0; i < m_AnimationData.Count; i++)
        {
            switch (m_AnimationData[i].m_AniType)
            {
                case AnimationType.LocalPosition:
                    trans.localPosition = m_AnimationData[i].m_Start;
                    break;
                case AnimationType.LocalRotation:
                    trans.localRotation = Quaternion.Euler(m_AnimationData[i].m_Start);
                    break;
                case AnimationType.LocalScale:
                    trans.localScale = m_AnimationData[i].m_Start;
                    break;
                case AnimationType.Color:
                    Graphic img = m_AnimationObj.GetComponent<Graphic>();
                    if (img != null) { img.color = m_AnimationData[i].m_StartColor; }
                    break;
                case AnimationType.WidthAndHeight:
                    RectTransform rectT = m_AnimationObj.GetComponent<RectTransform>();
                    if (rectT != null) { rectT.sizeDelta = new Vector2(m_AnimationData[i].m_Start.x, m_AnimationData[i].m_Start.y); }
                    break;
                case AnimationType.WorldPosition:
                    trans.position = m_AnimationData[i].m_Start;
                    break;
                case AnimationType.ShaderAniParam:
                    Material mat = null;
                    Graphic img2 = m_AnimationObj.GetComponent<Graphic>();
                    if (img2 != null) { mat = img2.material; }
                    mat.SetFloat(m_AnimationData[i].m_ShaderParam, m_AnimationData[i].m_Start.x);
                    break;
            }
        }
    }

    public void ResetBySaveData()//重置到最初状态
    {
        if (m_SaveStartData == null) return;
        Transform trans = m_AnimationObj.transform;
        for (int i = 0; i < m_AnimationData.Count; i++)
        {
            switch (m_AnimationData[i].m_AniType)
            {
                case AnimationType.LocalPosition:
                    trans.localPosition = m_SaveStartData.m_LocalPos;
                    break;
                case AnimationType.LocalRotation:
                    trans.localRotation = Quaternion.Euler(m_SaveStartData.m_LocalRotate);
                    break;
                case AnimationType.LocalScale:
                    trans.localScale = m_SaveStartData.m_LocalScale;
                    break;
                case AnimationType.Color:
                    Graphic img = m_AnimationObj.GetComponent<Graphic>();
                    if (img != null) { img.color = m_SaveStartData.m_Color; }
                    break;
                case AnimationType.WidthAndHeight:
                    RectTransform rectT = m_AnimationObj.GetComponent<RectTransform>();
                    if (rectT != null) { rectT.sizeDelta = m_SaveStartData.m_WidthAndHeight; }
                    break;
                case AnimationType.WorldPosition:
                    trans.position = m_SaveStartData.m_WorldPos;
                    break;
                case AnimationType.ShaderAniParam:
                    Material mat = null;
                    Graphic img2 = m_AnimationObj.GetComponent<Graphic>();
                    if (img2 != null) { mat = img2.material; }
                    mat.SetFloat(m_AnimationData[i].m_ShaderParam, m_SaveStartData.m_ShaderParam);
                    break;
            }
        }
    }

    private int m_curLeaveAniCount = 0;
    public IEnumerator DoAniListCoroutine()
    {
        m_curLeaveAniCount = m_AnimationData.Count;
        for (int i = 0; i < m_AnimationData.Count; i++)
        {
            StartCoroutine(DoAnimtionCoroutine(m_AnimationData[i]));
        }
        while (m_curLeaveAniCount > 0)
        {
            yield return null;
        }
        PlayEnd();
    }

    void PlayEnd()
    {
        if (m_PlayEndInactive)
            m_AnimationObj.SetActive(false);
        if (m_InactiveDestory && !m_AnimationObj.activeSelf)
            Destroy(m_AnimationObj);
        if (m_OverCallBack != null)
        {
            m_OverCallBack();
            m_OverCallBack = null;
        }
    }

    public IEnumerator DoAnimtionCoroutine(AnimationData data)
    {
        Transform trans = m_AnimationObj.transform;
        if (data != null)
        {
            if (data.m_Delay > 0.0001f)
            {
                if (m_IgnoreTimeScale) { yield return new WaitForSeconds(data.m_Delay / Time.timeScale); }
                else yield return new WaitForSeconds(data.m_Delay);
            }
            float maxTime = Mathf.Max(0.001f ,data.m_Duration);
            float curTime = 0f;
            bool noPlayed = true;

            while (noPlayed || data.m_Loop)
            {
                noPlayed = false;
                curTime = 0f;
                switch (data.m_AniType)
                {
                    case AnimationType.LocalPosition:
                        {
                            if (!m_SaveStartData.m_LocalPos.HasValue()) m_SaveStartData.m_LocalPos = trans.localPosition;
                            Vector3 startValue = data.m_Start;
                            Vector3 endValue = data.m_End;
                            if (data.m_IsOffsetValue)
                            {
                                startValue = trans.localPosition + data.m_Start;
                                endValue = trans.localPosition + data.m_End;
                            }
                            trans.localPosition = startValue;
                            while (curTime < maxTime)
                            {
                                yield return null;
                                if (m_IgnoreTimeScale) { curTime += Time.deltaTime / Time.timeScale; ; }
                                else curTime += Time.deltaTime;

                                float posValue = data.m_AniCurve.Evaluate(curTime / maxTime);
                                trans.localPosition = Vector3.LerpUnclamped(startValue, endValue, posValue);
                            }
                            trans.localPosition = data.m_IsEndSetToStart ? startValue : endValue;
                            break;
                        }
                    case AnimationType.AnchoredPosition:
                        {
                            RectTransform recttrans = trans as RectTransform;
                            if (!m_SaveStartData.m_LocalPos.HasValue()) m_SaveStartData.m_LocalPos = recttrans.anchoredPosition;
                            Vector3 startValue = data.m_Start;
                            Vector3 endValue = data.m_End;
                            if (data.m_IsOffsetValue)
                            {
                                startValue = (Vector3)recttrans.anchoredPosition + data.m_Start;
                                endValue = (Vector3)recttrans.anchoredPosition + data.m_End;
                            }
                            recttrans.anchoredPosition = startValue;
                            while (curTime < maxTime)
                            {
                                yield return null;
                                if (m_IgnoreTimeScale) { curTime += Time.deltaTime / Time.timeScale; ; }
                                else curTime += Time.deltaTime;

                                float posValue = data.m_AniCurve.Evaluate(curTime / maxTime);
                                recttrans.anchoredPosition = Vector3.LerpUnclamped(startValue, endValue, posValue);
                            }
                            recttrans.anchoredPosition = data.m_IsEndSetToStart ? startValue : endValue;
                            break;
                        }
                    case AnimationType.LocalRotation:
                        {
                            if (!m_SaveStartData.m_LocalRotate.HasValue()) m_SaveStartData.m_LocalRotate = trans.localRotation.eulerAngles;
                            Vector3 startValue = data.m_Start;
                            Vector3 endValue = data.m_End;
                            if (data.m_IsOffsetValue)
                            {
                                startValue = trans.localRotation.eulerAngles + data.m_Start;
                                endValue = trans.localRotation.eulerAngles + data.m_End;
                            }
                            trans.localRotation = Quaternion.Euler(startValue);
                            while (curTime < maxTime)
                            {
                                yield return null;
                                if (m_IgnoreTimeScale) { curTime += Time.deltaTime / Time.timeScale; ; }
                                else curTime += Time.deltaTime;

                                float posValue = data.m_AniCurve.Evaluate(curTime / maxTime);
                                trans.localRotation = Quaternion.Euler(Vector3.LerpUnclamped(startValue, endValue, posValue));
                            }
                            trans.localRotation = data.m_IsEndSetToStart ? Quaternion.Euler(startValue) : Quaternion.Euler(endValue);

                            break;
                        }
                    case AnimationType.LocalScale:
                        {
                            if (!m_SaveStartData.m_LocalScale.HasValue()) m_SaveStartData.m_LocalScale = trans.localScale;
                            Vector3 startValue = data.m_Start;
                            Vector3 endValue = data.m_End;
                            if (data.m_IsOffsetValue)
                            {
                                startValue = trans.localScale + data.m_Start;
                                endValue = trans.localScale + data.m_End;
                            }
                            while (curTime < maxTime)
                            {
                                yield return null;
                                if (m_IgnoreTimeScale) { curTime += Time.deltaTime / Time.timeScale; ; }
                                else curTime += Time.deltaTime;

                                float posValue = data.m_AniCurve.Evaluate(curTime / maxTime);
                                trans.localScale = Vector3.LerpUnclamped(startValue, endValue, posValue);
                            }
                            trans.localScale = data.m_IsEndSetToStart ? startValue : endValue;
                            break;
                        }
                    case AnimationType.Color:
                    case AnimationType.Alpha:
                    {
                        Graphic img = m_AnimationObj.GetComponent<Graphic>();
                        if (img != null)
                        {
                            if (!m_SaveStartData.m_Color.HasValue()) m_SaveStartData.m_Color = img.color;
                            if (data.m_AniType == AnimationType.Alpha) //如果是仅alpha
                            {
                                data.m_StartColor = new Color(img.color.r, img.color.g, img.color.b, data.m_StartColor.a);
                                data.m_EndColor = new Color(img.color.r, img.color.g, img.color.b, data.m_EndColor.a);
                            }
                            img.color = data.m_StartColor;
                            while (curTime < maxTime)
                            {
                                yield return null;
                                if (m_IgnoreTimeScale)
                                {
                                    curTime += Time.deltaTime/Time.timeScale;
                                }
                                else curTime += Time.deltaTime;

                                float posValue = data.m_AniCurve.Evaluate(curTime/maxTime);
                                img.color = Vector4.LerpUnclamped(data.m_StartColor, data.m_EndColor, posValue);
                            }
                            img.color = data.m_IsEndSetToStart ? data.m_StartColor : data.m_EndColor;
                        }
                        else if (data.m_AniType == AnimationType.Alpha) //如果是透明度，可以用CanvasGroup进行动画
                        {
                            CanvasGroup canvasGroup = m_AnimationObj.GetComponent<CanvasGroup>();
                            if(canvasGroup!=null)
                            {
                                if (!m_SaveStartData.m_Color.HasValue()) m_SaveStartData.m_Color = Color.white;
                                while (curTime < maxTime)
                                {
                                    yield return null;
                                    if (m_IgnoreTimeScale)
                                    {
                                        curTime += Time.deltaTime / Time.timeScale;
                                    }
                                    else curTime += Time.deltaTime;

                                    float posValue = data.m_AniCurve.Evaluate(curTime / maxTime);
                                    canvasGroup.alpha = Mathf.LerpUnclamped(data.m_StartColor.a, data.m_EndColor.a, posValue);
                                }
                                canvasGroup.alpha = data.m_IsEndSetToStart ? data.m_StartColor.a : data.m_EndColor.a;
                            }
                        }
                        break;
                    }
                    case AnimationType.WorldPosition:
                    {
                        if (!m_SaveStartData.m_WorldPos.HasValue()) m_SaveStartData.m_WorldPos = trans.position;
                        Vector3 startValue = data.m_Start;
                        Vector3 endValue = data.m_End;
                        if (data.m_IsOffsetValue)
                        {
                            startValue = trans.position + data.m_Start;
                            endValue = trans.position + data.m_End;
                        }
                        trans.position = startValue;
                        while (curTime < maxTime)
                        {
                            yield return null;
                            if (m_IgnoreTimeScale)
                            {
                                curTime += Time.deltaTime/Time.timeScale;
                            }
                            else curTime += Time.deltaTime;

                            float posValue = data.m_AniCurve.Evaluate(curTime/maxTime);
                            trans.position = Vector3.LerpUnclamped(startValue, endValue, posValue);
                        }
                        trans.position = data.m_IsEndSetToStart ? startValue : endValue;
                        break;
                    }
                    case AnimationType.WidthAndHeight:
                    {
                        RectTransform rectT = m_AnimationObj.GetComponent<RectTransform>();
                        if (rectT != null)
                        {
                            if (!m_SaveStartData.m_WidthAndHeight.HasValue()) m_SaveStartData.m_WidthAndHeight = rectT.sizeDelta;
                            Vector2 startSize = new Vector2(data.m_Start.x, data.m_Start.y);
                            Vector2 endSize = new Vector2(data.m_End.x, data.m_End.y);
                            rectT.sizeDelta = startSize;
                            while (curTime < maxTime)
                            {
                                yield return null;
                                if (m_IgnoreTimeScale)
                                {
                                    curTime += Time.deltaTime/Time.timeScale;
                                }
                                else curTime += Time.deltaTime;

                                float posValue = data.m_AniCurve.Evaluate(curTime/maxTime);
                                rectT.sizeDelta = Vector2.LerpUnclamped(startSize, endSize, posValue);
                            }
                            rectT.sizeDelta = data.m_IsEndSetToStart ? startSize : endSize;
                        }
                        break;
                    }
                    case AnimationType.ShaderAniParam:
                        {
                            Material mat = null;
                            Graphic img = m_AnimationObj.GetComponent<Graphic>();
                            if (img != null)
                            {
                                mat = img.material;
                            }
                            if (mat != null && data.m_ShaderParam != string.Empty)
                            {
                                if (!m_SaveStartData.m_ShaderParam.HasValue()) m_SaveStartData.m_ShaderParam = mat.GetFloat(data.m_ShaderParam);
                                float startShaderFloat = data.m_Start.x;
                                float endShaderFloat = data.m_End.x;
                                mat.SetFloat(data.m_ShaderParam, startShaderFloat);
                                while (curTime < maxTime)
                                {
                                    yield return null;
                                    if (m_IgnoreTimeScale) { curTime += Time.deltaTime / Time.timeScale; ; }
                                    else curTime += Time.deltaTime;

                                    float posValue = data.m_AniCurve.Evaluate(curTime / maxTime);
                                    mat.SetFloat(data.m_ShaderParam, Mathf.LerpUnclamped(startShaderFloat, endShaderFloat, posValue));
                                    //TDebug.Log(mat.GetFloat(data.m_ShaderParam)+trans.parent.parent.name);
                                }
                                mat.SetFloat(data.m_ShaderParam, data.m_IsEndSetToStart ? startShaderFloat : endShaderFloat);
                            }
                            break;
                        }
                }
            }
        }
        //if (data.m_Loop)
        //{
        //    StartCoroutine("DoAnimtionCoroutine", data);
        //}
        //else
        //{
        m_curLeaveAniCount -= 1;
        if (m_curLeaveAniCount == 0) PlayEnd();
        //}
    }
}

public enum AnimationType
{
    LocalPosition = 0,
    LocalRotation = 1,
    LocalScale = 2,
    Color = 3,
    WidthAndHeight = 4,
    WorldPosition = 5,
    ShaderAniParam = 6,   //x轴
    AnchoredPosition,
    Alpha
}

public class AnimationSaveData
{
    public Vector3 m_LocalPos = ClassBaseUtils.NullVector3;
    public Vector3 m_LocalRotate = ClassBaseUtils.NullVector3;
    public Vector3 m_LocalScale = ClassBaseUtils.NullVector3;
    public Color m_Color = ClassBaseUtils.NullColor;
    public Vector2 m_WidthAndHeight = ClassBaseUtils.NullVector2;
    public Vector3 m_WorldPos = ClassBaseUtils.NullVector3;
    public float m_ShaderParam = ClassBaseUtils.NullFloat;
}
[System.Serializable]
public class AnimationData
{
    public AnimationType m_AniType;
    public bool m_IsEndSetToStart;        //是否在动画播放后，位置等信息设成Start时的状态
    public bool m_IsOffsetValue;          //是相对运动
    public AnimationCurve m_AniCurve; //动画曲线
    public float m_Delay;     //延迟
    public string m_ShaderParam;
    public bool m_Loop;      //循环
    public float m_Duration;  //持续时间
    public Vector3 m_Start;
    public Color m_StartColor = Color.white;
    public Vector3 m_End;
    public Color m_EndColor = Color.white;

    public AnimationData()
    {
        m_AniCurve = AnimationCurve.Linear(0, 0, 1, 1);
        m_StartColor = Color.white;
        m_EndColor = Color.white;
    }
}

