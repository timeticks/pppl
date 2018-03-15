using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GridScaleFit : MonoBehaviour {

    public RectTransform TargetRect;
    public float OriginalWidth;
    public RectTransform m_Rect;

    void Start( )
    {
        m_Rect = GetComponent<RectTransform>( );
    }

    void Update( )
    {
        if( TargetRect && OriginalWidth != 0 && m_Rect)
            transform.localScale = Vector2.one * TargetRect.sizeDelta.x / OriginalWidth;
    }

}
