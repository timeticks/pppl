using UnityEngine;
using System.Collections;

public class PlayFrameByUVBase : MonoBehaviour
{
    public bool m_PlayEnable = false;
    public float m_PlayEnableDelay = 0f;
    public PlayUVFrameData m_Data;
    private System.Action m_overDel;    //结束回调，不一定是动作播放完成，可能是被覆盖
    private System.Action m_finishDel;  //此动作播放结束，被覆盖时不回调
    private System.Action m_offsetSetFreshDel;  //当偏移量生效时回调，用于同时刷新镜像scale
    private bool m_waitOffsetWait = false;
    public int m_Offset = 0;
    internal Texture m_CurTex;     //当前的贴图


    void OnEnable()
    {
        if (m_PlayEnable)
        {
            if (m_PlayEnableDelay <= 0f) EnablePlay();
            else Invoke("EnablePlay", m_PlayEnableDelay);
        }
    }
    void EnablePlay() { Play(m_Data); }

    Vector2 Convert(int index, int countX, int maxIndex, Vector2 scale)
    {
        //TDebug.Log(index);
        index = maxIndex - index;
        int y = (int)(index / (float)countX);
        //因为从0开始，+1补上0位，如果从1开始，则计算offset时需要-1
        int x = (int)(countX - (index % countX + 1));
        Vector2 offset = new Vector2(x * scale.x, y * scale.y);
        return offset;
    }

    void Play(PlayUVFrameData data)
    {
        m_Offset = 0;
        StopCoroutine("PlayAnim");
        m_Data = data;
        StartCoroutine("PlayAnim", data);
    }
    public void Play(PlayUVFrameData data , int dirOffset, System.Action overDel=null , System.Action finishDel = null)
    {
        StopCoroutine("PlayAnim");
        m_Offset = dirOffset;
        if (m_overDel != null) m_overDel();
        m_overDel = overDel;
        m_finishDel = finishDel;
        m_Data = data;
        StartCoroutine("PlayAnim", data);
    }

    // 当动画需要改变方向时，只改变offset使Index整体偏移，不是重新进行动画播放
    public void SetOffset(int offsetValue, System.Action over=null , System.Action setFresh=null)
    {
        m_waitOffsetWait = true;
        m_Offset = offsetValue;
        if (over != null) { m_overDel = over; }
        m_offsetSetFreshDel = setFresh;
    }
    public void AddDel(System.Action fini)
    {
        m_overDel += fini;
    }

    public virtual void SetScaleByDir(int actionDir, DirNumType dirNumType)
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


    public void Stop() { StopCoroutine("PlayAnim"); }
    public virtual void SetTexture(Texture tex){ }
    public virtual void SetTextureScale(Vector2 scale){}
    public virtual void SetTextureOffst(Vector2 num){}

    IEnumerator PlayAnim(PlayUVFrameData data)
    {
        m_offsetSetFreshDel = null;
        if (data.m_Tex != null)
        {
            if (m_CurTex != data.m_Tex)
            {
                m_CurTex = data.m_Tex;
                SetTexture(data.m_Tex);
            }
        }
        int index = data.m_StartIndex;
        int maxIndex = (int)(data.m_CountX * data.m_CountY) - 1;
        Vector2 scale = new Vector2(1f / (float)data.m_CountX, 1f / (float)data.m_CountY);
        SetTextureScale(scale);
        switch (data.m_PlayType)
        {
            case PlayFrameType.Repeat:
                while (true)
                {
                    SetTextureOffst(Convert(index + m_Offset, (int)data.m_CountX, maxIndex, scale));
                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    //当到达终点则回到起点
                    if (index == data.m_EndIndex)
                    {
                        index = data.m_StartIndex;
                        if (data.m_LoopWait > 0) yield return new WaitForSeconds(data.m_LoopWait);
                    }
                    else index++;
                    yield return new WaitForSeconds(data.m_Speed);
                }
            case PlayFrameType.PingPong:
                int direction = 1;
                while (true)
                {
                    SetTextureOffst(Convert(index + m_Offset, (int)data.m_CountX, maxIndex, scale));
                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    index += direction;
                    //逆向
                    if (index == data.m_StartIndex || index == data.m_EndIndex)
                    {
                        direction = -direction;
                        if (data.m_LoopWait > 0)
                            yield return new WaitForSeconds(data.m_LoopWait);
                    }
                    yield return new WaitForSeconds(data.m_Speed);
                }
            case PlayFrameType.PingPongOnce:
                bool inverse = false;
                while (true)
                {
                    SetTextureOffst(Convert(index + m_Offset, (int)data.m_CountX, maxIndex, scale));
                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    //当到达终点则回到起点
                    yield return new WaitForSeconds(data.m_Speed);
                    if (index == data.m_StartIndex || index == data.m_EndIndex)
                    {
                        if (!inverse && index == data.m_EndIndex) { inverse = true; }
                        else if (inverse && index == data.m_StartIndex) { break; }
                        else { index = index + ((inverse) ? -1 : 1); }
                    }
                    else { index = index + ((inverse) ? -1 : 1); }
                }
                if (m_overDel != null)
                {
                    m_overDel();
                    m_overDel = null;
                }
                if (m_finishDel != null) { m_finishDel(); m_finishDel = null; }
                switch (data.m_EndType)
                {
                    case PlayFrameEndType.Disable:
                        gameObject.SetActive(false);
                        break;
                    case PlayFrameEndType.Destroy:
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
                yield break;
            default:
                while (true)
                {
                    SetTextureOffst(Convert(index + m_Offset, (int)data.m_CountX, maxIndex, scale));
                    if (m_waitOffsetWait && m_offsetSetFreshDel != null) { m_offsetSetFreshDel(); m_offsetSetFreshDel = null; }
                    //当到达终点则回到起点
                    yield return new WaitForSeconds(data.m_Speed);
                    if (index == data.m_EndIndex) break;
                    else index++;
                }
                if (m_overDel != null)
                {
                    m_overDel();
                    m_overDel = null;
                }
                if (m_finishDel != null) { m_finishDel(); m_finishDel = null; }
                switch (data.m_EndType)
                {
                    case PlayFrameEndType.Disable:
                        gameObject.SetActive(false);
                        break;
                    case PlayFrameEndType.Destroy:
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
                yield break;
        }
    }
}




[System.Serializable]
public class PlayUVFrameData
{
    public PlayFrameType m_PlayType;
    public PlayFrameEndType m_EndType;
    public float m_LoopWait;
    public Texture m_Tex;
    public int m_CountX, m_CountY;
    public int m_StartIndex, m_EndIndex;
    public float m_Speed=0.15f;
    
    public PlayUVFrameData(int countX, int countY, int startIndex, int endIndex,PlayFrameType playType, Texture tex=null, float speed=0.1f , float loopWait=0f)
    {
        m_PlayType = playType;
        m_Tex = tex;
        m_CountX = countX;
        m_CountY = countY;
        m_StartIndex = startIndex;
        m_EndIndex = endIndex;
        m_Speed = speed;
        m_LoopWait = loopWait;
    }
}


public enum PlayFrameType
{
    None = 0,   //只播放一遍
    Repeat = 1,
    PingPong = 2,
    PingPongOnce = 3
}
public enum PlayFrameEndType
{
    None = 0,
    Disable = 1,
    Destroy = 2
}