using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

#endif
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class OverrideUGUICreate {

    [MenuItem("GameObject/UI/Image")] 
    static void CreatImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform, false);
                Selection.objects = new Object[] { go };
            }
            else
            { Debug.LogError("no find canvas"); }
        }
    }
    [MenuItem("GameObject/UI/Raw Image")]
    static void CreatRawImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("RawImage", typeof(RawImage));
                go.GetComponent<RawImage>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform, false);
                Selection.objects = new Object[] { go };
            }
            else
            { Debug.LogError("no find canvas"); }
        }
    }
    [MenuItem("GameObject/UI/Text")]
    static void CreatText()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Text", typeof(Text));
                Font font = AssetDatabase.LoadAssetAtPath<Font>(@"Assets/Build/BaseUI/Font/HYChengXingJ.TTF");
                if (font == null) { Debug.LogError("找不到字体"); }
                go.GetComponent<Text>().font = font;
                go.GetComponent<Text>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform, false);
                Selection.objects = new Object[] { go };
            }
            else
            { Debug.LogError("no find canvas"); }
        }
    }

    [MenuItem("Tools/SetAlphaMaterial %q")]  //ctrl+q   
    static void SetAlphaMaterial()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)  //遍历所有选中的Gamobejct
        {
            if (Selection.gameObjects[i] == null) continue;
            RawImage rawImg=null;
            FillChangeImage fillImg=null;
            Image img = Selection.gameObjects[i].GetComponent<Image>();
            if (img == null) rawImg = Selection.gameObjects[i].GetComponent<RawImage>();
            if (img == null && rawImg == null) fillImg = Selection.gameObjects[i].GetComponent<FillChangeImage>();

            MaskableGraphic graphic = Selection.gameObjects[i].GetComponent<MaskableGraphic>();
            if (!graphic.material.name.Contains("Default"))
            {
                graphic.material = null;
                return;
            }
            if (img != null || rawImg!=null || fillImg!=null)
            {
                string spritePath ="";
                if (img != null) spritePath = AssetDatabase.GetAssetPath(img.sprite);
                if (rawImg != null) spritePath = AssetDatabase.GetAssetPath(rawImg.texture);
                if (fillImg != null) spritePath = AssetDatabase.GetAssetPath(fillImg.sprite);

                //DirectoryInfo dir = new DirectoryInfo();
                //string fileName = dir.Name.Remove(dir.Name.LastIndexOf("."));
                //foreach (var temp in dir.Parent.GetFiles("*.mat"))
                {
                    graphic.material = AssetDatabase.LoadAssetAtPath(MaterialTextureForETC1.GetMatPath(spritePath), typeof(Material)) as Material;
                }
            }
        }
    }

    [MenuItem("Tools/SetGiemosShowLine %w")]  //ctrl+q   
    static void SetGiemosShowLine()
    {
        DebugUILine.DrawWireCube = !DebugUILine.DrawWireCube;
    }

    static string GetRelativeAssetPath(string _fullPath) //得到相对路径
    {
        _fullPath = _fullPath.Replace("\\", "/");
        int idx = _fullPath.IndexOf("Assets");
        string assetRelativePath = _fullPath.Substring(idx);
        return assetRelativePath;
    }
}
