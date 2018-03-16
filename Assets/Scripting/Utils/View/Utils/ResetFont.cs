using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
#endif
public class ResetFont : MonoBehaviour {

    public Font m_Font;

}



#if UNITY_EDITOR

[CanEditMultipleObjects()]
[CustomEditor(typeof(ResetFont), true)]
public class ResetFontEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        ResetFont targetIns = (ResetFont)target;
        if (GUILayout.Button("遍历设置字体"))
        {
            GameObject[] pAllObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));  //得到场景里所有物体
            List<GameObject> pReturn = new List<GameObject>();
            foreach (GameObject pObject in pAllObjects)
            {
                if (pObject.GetComponent<Text>()!=null)
                {
                    pObject.GetComponent<Text>().font = targetIns.m_Font;
                }
            }
        }
    }
}
#endif