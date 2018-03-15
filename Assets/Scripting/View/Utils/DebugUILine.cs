using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DebugUILine : MonoBehaviour {
	static Vector3[] fourCorners = new Vector3[4];
    public bool DrawLine = false;
    public static bool DrawWireCube = false;
#if UNITY_EDITOR
    void OnDrawGizmos()
	{
        if (DrawWireCube)
        {
            Transform[] select = Selection.transforms;
            if (select.Length > 0)
            {
                foreach (Transform item in select)
                {
                    foreach (var g in item.GetComponentsInChildren<RectTransform>())
                    {
                        ShowWireCube(g);
                    }
                }
            }
            else
            {
                DrawWireCube = false;
            }
           
        }
        if (!DrawLine)
            return;
        Transform[] selects = Selection.transforms;
        if (selects.Length > 0)
        {
            foreach (Transform item in selects)
            {
                foreach (var g in item.GetComponentsInChildren<MaskableGraphic>())
                {
                    ShowRect(g);
                }
            }
        }
        else
        {
            foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
            {
                ShowRect(g);
            }
        }
    }

    void ShowRect(MaskableGraphic g)
    {
        if (g.raycastTarget)
        {
            RectTransform rectTransform = g.transform as RectTransform;
            rectTransform.GetWorldCorners(fourCorners);

            Gizmos.color = new Color(1, 0, 0, 0.4f);
            Vector3 centerPos = (fourCorners[0] + fourCorners[1] + fourCorners[2] + fourCorners[3]) * 0.25f;
            float sizeX = Mathf.Max(fourCorners[0].x, fourCorners[1].x, fourCorners[2].x, fourCorners[3].x) - Mathf.Min(fourCorners[0].x, fourCorners[1].x, fourCorners[2].x, fourCorners[3].x);
            float sizeY = Mathf.Max(fourCorners[0].y, fourCorners[1].y, fourCorners[2].y, fourCorners[3].y) - Mathf.Min(fourCorners[0].y, fourCorners[1].y, fourCorners[2].y, fourCorners[3].y);
            Vector3 size = new Vector3(sizeX, sizeY, 0);
            Gizmos.DrawCube(centerPos, size);
        }
    }
    private Color blueColor = new Color(0,0,1,0.4f);
    void ShowWireCube(RectTransform trans)
    {
        if (trans.GetComponent<Image>() != null || trans.GetComponent<RawImage>() != null)
            Gizmos.color = blueColor;
        else
            return;     
        trans.GetWorldCorners(fourCorners);           
        Vector3 centerPos = (fourCorners[0] + fourCorners[1] + fourCorners[2] + fourCorners[3]) * 0.25f;
        float sizeX = Mathf.Max(fourCorners[0].x, fourCorners[1].x, fourCorners[2].x, fourCorners[3].x) - Mathf.Min(fourCorners[0].x, fourCorners[1].x, fourCorners[2].x, fourCorners[3].x);
        float sizeY = Mathf.Max(fourCorners[0].y, fourCorners[1].y, fourCorners[2].y, fourCorners[3].y) - Mathf.Min(fourCorners[0].y, fourCorners[1].y, fourCorners[2].y, fourCorners[3].y);
        Vector3 size = new Vector3(sizeX, sizeY, 0);
        Gizmos.DrawWireCube(centerPos, size);
    }
#endif
}
