using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 图片残影
/// </summary>
public class ImageGhost : MonoBehaviour 
{
    public Image m_MyImage;
    public float m_CreateTime=0.1f;  //生成新图片的距离
    public float m_DisableTime=0.4f; //消失的时间
    public bool m_IsSpawn;
    
    internal Transform m_MyTrans;
    float m_curOffset = 0;
    List<GhostElem> m_ghostDataList = new List<GhostElem>();
    List<Image> m_ghostImageList = new List<Image>();
    class GhostElem
    {
        public float m_CurValue;
        public Vector3 m_Pos;
        public GhostElem(){ }
        public GhostElem(float curValue, Vector3 pos) { m_CurValue = curValue; m_Pos = pos; }
    }
	void Start ()
    {
        if (m_MyImage == null)
        {
            m_MyImage = GetComponent<Image>();
        }
        m_MyTrans = transform;
	}
	
	void Update ()
    {
        
        FreshGhostElemTime();
        FreshGhostElemPos();
        FreshGhostImage();
	}

    void FreshGhostElemTime()
    {
        for (int i = 0; i < m_ghostDataList.Count; i++)
        {
            m_ghostDataList[i].m_CurValue -= Time.deltaTime / m_DisableTime;
            if (m_ghostDataList[i].m_CurValue <= 0)
            {
                m_ghostDataList.Remove(m_ghostDataList[i]);
                i--;
            }
        }
    }
    void FreshGhostElemPos()
    {
        m_curOffset += Time.deltaTime;
        if (m_curOffset > m_CreateTime)
        {
            m_curOffset -= m_CreateTime;
            m_ghostDataList.Add(new GhostElem(1f, m_MyTrans.localPosition));
        }
    }

    void FreshGhostImage()
    {
        AddInstantiate<Image>(m_ghostImageList, m_MyImage.gameObject, m_MyTrans.parent, m_ghostDataList.Count);
        for (int i = 0; i < m_ghostImageList.Count; i++)
        {
            if (i > m_ghostDataList.Count - 1)
            {
                m_ghostImageList[i].gameObject.SetActive(false);
                continue;
            }
            Color col = m_ghostImageList[i].color;
            col.a = m_ghostDataList[i].m_CurValue;
            m_ghostImageList[i].color = col;
            m_ghostImageList[i].transform.localPosition = m_ghostDataList[i].m_Pos;
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < m_ghostImageList.Count; i++)
        {
            Destroy(m_ghostImageList[i]);
        }
    }

    public List<T> AddInstantiate<T>(List<T> curList, GameObject prefabObj, Transform parentTrans, int needNum) where T : MonoBehaviour
    {
        for (int i = curList.Count; i < needNum; i++)
        {
            GameObject g = Instantiate(prefabObj) as GameObject;
            SetParent(g.transform, parentTrans, false);
            T p = g.GetComponent<T>();
            curList.Add(p);
            Destroy(p.GetComponent<ImageGhost>());
        }
        return null;
    }

    public static void SetParent(Transform childTran, Transform parentTran, bool isRectTransformZero = false, bool isRotationZero = true)  //设置子物体后，重置位置信息
    {
        childTran.SetParent(parentTran);
        childTran.localPosition = Vector3.zero;
        childTran.localScale = Vector3.one;
        if (isRotationZero) childTran.localRotation = Quaternion.identity;
        if (isRectTransformZero)
        {
            childTran.GetComponent<RectTransform>().localPosition = Vector3.zero;
            childTran.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
    }
}
