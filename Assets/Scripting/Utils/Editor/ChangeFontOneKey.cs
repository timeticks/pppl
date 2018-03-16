using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

public class ChangeFontOneKey : MonoBehaviour {
    #if UNITY_EDITOR
    [MenuItem("Tools/Change Font")]
    public static void Change()
    {
        Font m_TargetFont = Resources.Load<Font>("HYChengXingJ");
        if (m_TargetFont == null) return;
        if (Selection.objects == null || Selection.objects.Length == 0) return;
        Object[] texts = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
        foreach (Object item in texts)
        {
            Text text = (Text)item;
            text.font = m_TargetFont;
            EditorUtility.SetDirty(item);            
        }
    }
#endif
}
