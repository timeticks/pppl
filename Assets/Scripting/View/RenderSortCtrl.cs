using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// 图片渲染顺序控制
/// </summary>
public class RenderSortCtrl : MonoBehaviour
{
    [SerializeField]
    private SortOrderType m_SortOrderType;
    [SerializeField]
    private int m_SortOffset;

    internal int m_lastSort = -999;
    internal Renderer m_render;
    public SortOrderType setSortType { get { return m_SortOrderType; } set { m_SortOrderType = value; Fresh(); } }
    public int setSortOffset { get { return m_SortOffset; } set { m_SortOffset = value; Fresh(); } }
    public int getRealSortOrder { get { return (int)m_SortOrderType + m_SortOffset; } }
    void Start()
    {
        m_render = transform.GetComponent<Renderer>();
        Fresh();
    }


    public void Fresh()
    {
        m_lastSort = (int)m_SortOrderType + m_SortOffset;
        if (m_render != null)
        {
            m_render.sortingOrder = m_lastSort;
            //Debug.Log(m_render.sortingLayerID);
            //Debug.Log(m_render.sortingOrder);
            //Debug.Log(m_render.sortingLayerName);
        }
    }

    public static int GetSortByZ(float curZ, float min = 0, float max = 100)
    {
        //0-999
        return (int)(Mathf.InverseLerp(min, max, curZ) * 999);
    }

}




public enum SortOrderType
{
    Ground = 0,
    GroundMax = 9999,

    Role = 10000,
    RoleMax = 19999,

    Upper = 20000,
    UpperMax = 29999,
    Overlay = 30000,
}



#if UNITY_EDITOR
[CanEditMultipleObjects()]
[CustomEditor(typeof(RenderSortCtrl), true)]
public class RenderSortEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (Time.frameCount % 5 == 0)
        {
            RenderSortCtrl targetIns = (RenderSortCtrl)target;
            if (targetIns.m_render == null) targetIns.m_render = targetIns.GetComponent<Renderer>();
            if (targetIns.m_lastSort != targetIns.getRealSortOrder) targetIns.Fresh();
        }
    }
}

#endif