using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DestroySelf : MonoBehaviour
{
    public List<Transform> m_EnableList;   //当自身激活时，同时激活的子物体
    public List<ActiveItem> m_ActiveList;  //根据延迟时间，进行激活的子物体
    public float DestoryTime = 0f;
    public string Describle;
    public bool IsDestory;

    internal int EffectId;
    private string mMyName;
    private System.Action ReleaseCallback;  //销毁后的释放回调
    private IEnumerator delayActiveCor;

    public void Init(string myName, System.Action releaseCallback)
    {
        mMyName = myName;
        ReleaseCallback = releaseCallback;
    }

    void OnEnable()
    {
        if (DestoryTime > 0)
        {
            Invoke("Destroy", DestoryTime);
        }
        for (int i = 0; i < m_EnableList.Count; i++)
        {
            m_EnableList[i].gameObject.SetActive(true);
        }
        bool needDelay = false;
        for (int i = 0; i < m_ActiveList.Count; i++)  //延迟激活
        {
            if (m_ActiveList[i].m_Delay.CompareTo(0) <= 0)
            {
                if (m_ActiveList[i].m_Trans != null) m_ActiveList[i].m_Trans.gameObject.SetActive(true);
            }
            else { needDelay = true; }
        }
        if (needDelay)
        {
            if (delayActiveCor != null)
                StopCoroutine(delayActiveCor);
            delayActiveCor = DelayActive();
            StartCoroutine(delayActiveCor);
        }
    }

    public void Destroy()
    {
        CancelInvoke("Destroy");
        if (!IsDestory)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

    public IEnumerator DelayActive()
    {
        List<ActiveItem> remainList = new List<ActiveItem>();
        for (int i = 0; i < m_ActiveList.Count; i++)
        {
            if (m_ActiveList[i].m_Delay > 0)
            {
                remainList.Add(m_ActiveList[i]);
            }
        }
        float curTime = 0f;
        while (m_ActiveList.Count > 0)
        {
            for (int i = 0; i < remainList.Count; i++)
            {
                if (curTime >= remainList[i].m_Delay)
                {
                    if (remainList[i].m_Trans != null) remainList[i].m_Trans.gameObject.SetActive(true);
                    remainList.RemoveAt(i);
                }
            }
            curTime += Time.deltaTime;
            yield return null;
        }
    }


    void OnDestroy()
    {
        if (ReleaseCallback != null) ReleaseCallback();
        //if (GameAssetsPool.Instance!=null)
        //GameAssetsPool.Instance.RemoveEffectRef(this);
    }
}

[System.Serializable]
public class ActiveItem
{
    public float m_Delay;
    public Transform m_Trans;
}
