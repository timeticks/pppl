using UnityEngine;
using System.Collections;

public class UIDepth : MonoBehaviour {
    public int m_Order;
    public bool m_IsUI = true;
    void Start()
    {
        if (m_IsUI)
        {
            Canvas canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = m_Order;
        }
        else
        {
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (Renderer render in renders)
            {
                render.sortingOrder = m_Order;
            }
        }
    }
}
