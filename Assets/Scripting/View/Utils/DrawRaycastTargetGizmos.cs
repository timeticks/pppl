using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawRaycastTargetGizmos : MonoBehaviour
{

    static Vector3[] fourCorners = new Vector3[4];
    void OnDrawGizmos()  
    {  
        foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())  
        {  
            if (g.raycastTarget)  
            {  
                RectTransform rect = g.transform as RectTransform;  
                rect.GetWorldCorners(fourCorners);  
                Gizmos.color = Color.blue;  
                for (int i = 0; i < 4; i++)  
                {  
                    Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);  
                }  
            }  
        }     
    }  
}
