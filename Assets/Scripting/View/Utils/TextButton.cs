using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class TextButton : Button {
    public Text TextBtn;

    public enum  ColorText
    {
        None,
        Yellow,
        Gray,
        White,
    }

    public void SetEnabled(bool enabled, ColorText textColor = ColorText.None)
    {
        this.enabled = enabled;
        if(textColor == ColorText.None) return;
        switch (textColor)
        {
            case ColorText.Yellow:
                TextBtn.color = new Color(255 / 255f, 242 / 255f, 62 / 255f);    
                break;
            case ColorText.Gray:
                TextBtn.color = new Color(163 / 255f, 163 / 255f, 163 / 255f);
                break;
            case ColorText.White:
                TextBtn.color = Color.white;
                break;
        }
             
    }
}

#if UNITY_EDITOR

[CanEditMultipleObjects()]
[CustomEditor(typeof(TextButton), true)]
public class CustomButtonEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }
}
#endif