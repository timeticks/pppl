using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ContentParentSizeFit : MonoBehaviour {

    public enum LayoutType
    {
        Vertical,
        Horizontal,
    }
    public LayoutType SortType = LayoutType.Vertical;
    public float OriginValue;
    public float Offset;
   /// <summary>
    ///  Must Attached GridLayoutGroup
   /// </summary>
    public RectTransform ListenerTrans;
    private RectTransform Trans;
    public void ResetSize()
    {
        if (Trans == null)
        {
            Trans = gameObject.GetComponent<RectTransform>();
        }

        if (ListenerTrans == null)
        {
            Debug.LogError("ListenerTrans is null");
            return;
        }
        if (!ListenerTrans.gameObject.activeInHierarchy)
        {
            if (SortType == LayoutType.Vertical)
            {
                Trans.sizeDelta = new Vector2(Trans.rect.width, OriginValue);
            }
            else
            {
                Trans.sizeDelta = new Vector2(OriginValue, Trans.rect.height);
            }
        }
        else
        {
            GridLayoutGroup listenerGrid = ListenerTrans.GetComponent<GridLayoutGroup>();
            Vector2 cellSize;
            Vector2 spacing;
            if (listenerGrid == null)
            {
                Debug.LogError(string.Format("the {0} Must Attached GridLayoutGroup", ListenerTrans));
                return;
            }
            else
            {
                cellSize = listenerGrid.cellSize;
                spacing = listenerGrid.spacing;
            }
            int childCount = 0;
            for (int i = 0; i < ListenerTrans.childCount; i++)
            {
                if (ListenerTrans.GetChild(i).gameObject.activeInHierarchy)
                    childCount++;
            }
            if (SortType == LayoutType.Vertical)
            {
                Trans.sizeDelta = new Vector2(Trans.rect.width, OriginValue + (cellSize.y + spacing.y) * childCount + Offset - spacing.y);
            }
            else
            {
                Trans.sizeDelta = new Vector2(OriginValue + (cellSize.x + spacing.x) * childCount + Offset - spacing.x, Trans.rect.height);
            }
        }      
    }


}
