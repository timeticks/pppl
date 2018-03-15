using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFrameSprite : MonoBehaviour {
    public SpriteRenderer SpriteRen;
    public bool m_PlayEnable = false;
    public float m_PlayEnableDelay = 0f;
    public PlayFrameSpriteData m_Data;
    private System.Action m_overDel;    //结束回调，不一定是动作播放完成，可能是被覆盖
    private System.Action m_finishDel;  //此动作播放结束，被覆盖时不回调
    private System.Action m_offsetSetFreshDel;  //当偏移量生效时回调，用于同时刷新镜像scale
    private bool m_waitOffsetWait = false;
    internal DirNumType m_DirNumType = DirNumType.Two;
    internal int m_CurDirType;
    private IEnumerator playAnimCor;

    void OnEnable()
    {
        if(SpriteRen==null)SpriteRen = GetComponent<SpriteRenderer>();
        if (m_PlayEnable)
        {
            if (m_PlayEnableDelay <= 0f) EnablePlay();
            else Invoke("EnablePlay", m_PlayEnableDelay);
        }
    }
    void EnablePlay() { Play(m_Data); }

    void Play(PlayFrameSpriteData data)
    {
        if (playAnimCor != null) StopCoroutine(playAnimCor);
        m_Data = data;
        playAnimCor = PlayAnim(data);
        StartCoroutine(playAnimCor);
    }
    public void Play(PlayFrameSpriteData data, DirNumType numType, int dirValue, System.Action overDel = null, System.Action finishDel = null)
    {
        if (playAnimCor != null) StopCoroutine(playAnimCor);

        m_DirNumType = numType;
        m_CurDirType = dirValue;
        if (m_overDel != null) m_overDel();
        m_overDel = overDel;
        m_finishDel = finishDel;
        m_Data = data;
        playAnimCor = PlayAnim(data);
        StartCoroutine(playAnimCor);
    }

    // 当动画需要改变方向时，只改变offset使Index整体偏移，不是重新进行动画播放
    public void SetDir(DirNumType numType, int dirValue, System.Action over = null, System.Action setFresh = null)
    {
        m_DirNumType = numType;
        m_CurDirType = dirValue;
        m_waitOffsetWait = true;
        if (over != null) { m_overDel = over; }
        m_offsetSetFreshDel = setFresh;
    }
    public void AddDel(System.Action fini)
    {
        m_overDel += fini;
    }

    public virtual void SetScaleSize(float scaleSize) { }

    public void SetScaleByDir(int actionDir, DirNumType dirNumType)
    {
        switch (dirNumType)
        {
            case DirNumType.Two:
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * ((actionDir == (int)ActionTwoDir.Right) ? -1 : 1), transform.localScale.y, transform.localScale.z);
                break;
            case DirNumType.Four:
                break;
            case DirNumType.Six:
                int flag = (actionDir == (int)ActionSixDir.Right || actionDir == (int)ActionSixDir.RightBack || actionDir == (int)ActionSixDir.RightFront) ? -1 : 1;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * flag, transform.localScale.y, transform.localScale.z);
                break;
            case DirNumType.Eight:
                int flag2 = (actionDir == (int)ActionEightDir.Right || actionDir == (int)ActionEightDir.RightBack || actionDir == (int)ActionEightDir.RightFront) ? -1 : 1;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * flag2, transform.localScale.y, transform.localScale.z);
                break;
        }
    }

    public void Stop() { StopAllCoroutines(); }
    public void FreshTexture(Sprite sp) 
    {
        SpriteRen.sprite = sp;
    }
    public IEnumerator PlayAnim(PlayFrameSpriteData data)
    {
        m_offsetSetFreshDel = null;
        
        int startNum = 0;
        int curNum = 0;
        int maxNum = data.SpriteList.Count-1;
        Dictionary<int, ActionFrameItem> m_FrameDic = new Dictionary<int, ActionFrameItem>();

       
        switch (data.m_PlayType)
        {
            case PlayFrameType.Repeat:
                while (true)
                {
                    Vector2 posDetla;
                    if (m_FrameDic.ContainsKey(curNum)) { posDetla = m_FrameDic[curNum].m_PosDetla; }
                    if (m_DirNumType == DirNumType.Two) { FreshTexture(m_Data.SpriteList[curNum]); }

                    float waitNum = data.m_Speed;
                    if (m_FrameDic.ContainsKey(curNum) && !m_FrameDic[curNum].m_PlaySpeed.Equals(0)) { waitNum = m_FrameDic[curNum].m_PlaySpeed; }
                    yield return new WaitForSeconds(waitNum);

                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    if (curNum == maxNum)//当到达终点则回到起点
                    {
                        curNum = startNum;
                        if (data.m_LoopWait > 0) yield return new WaitForSeconds(data.m_LoopWait);
                    }
                    else curNum++;
                }
            case PlayFrameType.PingPong:
                int direction = 1;
                while (true)
                {
                    Vector2 posDetla;
                    if (m_FrameDic.ContainsKey(curNum)) { posDetla = m_FrameDic[curNum].m_PosDetla; }
                    if (m_DirNumType == DirNumType.Two) { FreshTexture(m_Data.SpriteList[curNum]); }

                    float waitNum = data.m_Speed;
                    if (m_FrameDic.ContainsKey(curNum) && !m_FrameDic[curNum].m_PlaySpeed.Equals(0)) { waitNum = m_FrameDic[curNum].m_PlaySpeed; }
                    yield return new WaitForSeconds(waitNum);

                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    curNum += direction;
                    if (curNum == startNum || curNum == maxNum)//逆向
                    {
                        direction = -direction;
                        if (data.m_LoopWait > 0) yield return new WaitForSeconds(data.m_LoopWait);
                    }
                }
            case PlayFrameType.PingPongOnce:
                bool inverse = false;
                while (true)
                {
                    Vector2 posDetla;
                    if (m_FrameDic.ContainsKey(curNum)) { posDetla = m_FrameDic[curNum].m_PosDetla; }
                    if (m_DirNumType == DirNumType.Two) { FreshTexture(m_Data.SpriteList[curNum]); }

                    float waitNum = data.m_Speed;
                    if (m_FrameDic.ContainsKey(curNum) && !m_FrameDic[curNum].m_PlaySpeed.Equals(0)) { waitNum = m_FrameDic[curNum].m_PlaySpeed; }
                    yield return new WaitForSeconds(waitNum);

                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    //当到达终点则回到起点
                    if (curNum == startNum || curNum == maxNum)
                    {
                        if (!inverse && curNum == maxNum) { inverse = true; }
                        else if (inverse && curNum == startNum) { break; }
                        else { curNum = curNum + ((inverse) ? -1 : 1); }
                    }
                    else { curNum = curNum + ((inverse) ? -1 : 1); }

                }
                if (m_overDel != null)
                {
                    m_overDel();
                    m_overDel = null;
                }
                if (m_finishDel != null) { m_finishDel(); m_finishDel = null; }
                switch (data.m_EndType)
                {
                    case PlayFrameEndType.Disable: gameObject.SetActive(false); break;
                    case PlayFrameEndType.Destroy: Destroy(gameObject); break;
                    default: break;
                }
                yield break;
            default:
                while (true)
                {
                    Vector2 posDetla;
                    if (m_FrameDic.ContainsKey(curNum)) { posDetla = m_FrameDic[curNum].m_PosDetla; }
                    if (m_DirNumType == DirNumType.Two) { FreshTexture(m_Data.SpriteList[curNum]); }

                    float waitNum = data.m_Speed;
                    if (m_FrameDic.ContainsKey(curNum) && !m_FrameDic[curNum].m_PlaySpeed.Equals(0)) { waitNum = m_FrameDic[curNum].m_PlaySpeed; }
                    yield return new WaitForSeconds(waitNum);

                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    //当到达终点则回到起点
                    if (curNum == maxNum) break;
                    else curNum++;
                }
                if (m_overDel != null)
                {
                    m_overDel();
                    m_overDel = null;
                }
                if (m_finishDel != null) { m_finishDel(); m_finishDel = null; }
                switch (data.m_EndType)
                {
                    case PlayFrameEndType.Disable: gameObject.SetActive(false); break;
                    case PlayFrameEndType.Destroy: Destroy(gameObject); break;
                    default: break;
                }
                yield break;
        }
    }
}


[System.Serializable]
public class PlayFrameSpriteData
{
    public PlayFrameType m_PlayType;
    public PlayFrameEndType m_EndType;
    public float m_LoopWait;
    public float m_Speed = 0.15f;
    public List<Sprite> SpriteList;
    public PlayFrameSpriteData(PlayFrameType playType, float speed, float loopWait , List<Sprite> sprites)
    {
        m_Speed = speed;
        m_PlayType = playType;
        m_LoopWait = loopWait;
        SpriteList = sprites;
    }
}


[System.Serializable]
public class ActionFrameItem
{
    public int m_Index;
    public float m_PlaySpeed;
    public Vector2 m_PosDetla;
}
