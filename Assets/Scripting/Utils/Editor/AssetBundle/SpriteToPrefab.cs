#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

using UnityEditor;
using System.Linq;

/// <summary>
/// 将MultipleSprite中的子图片，转存在新建的Prefab上
/// </summary>
public class SpriteToPrefab : MonoBehaviour
{
    private static string LastPath = "Assets/Build/BuildSprite/";
    [MenuItem("Assets/Create/通道分离并Sprite转Prefab", false)]
    public static void SplitAndSpritePrefab()
    {
        SelectObjectToPrefabs2(true, false);
    }
    [MenuItem("Assets/Create/Sprite转Prefab，不分离", false)]
    public static void SpritePrefabNpSplit()
    {
        SelectObjectToPrefabs2(false, false);
    }
    [MenuItem("Assets/Create/Sprite转Prefab到当前文件夹，不分离", false)]
    public static void SpritePrefabInCurPathNoSplit()
    {
        SelectObjectToPrefabs2(false, true);
    }

    public static void SelectObjectToPrefabs2(bool splitAlpha , bool saveCurPath)
    {
        if (PlayerPrefs.HasKey("LastSpriteToPrefabPath"))
        {
            LastPath = PlayerPrefs.GetString("LastSpriteToPrefabPath");
        }
        string savePath = "";
        if (!saveCurPath)
        {
            savePath = EditorUtility.SaveFilePanel("Save Atlas", LastPath, "DontChangeTheName", "prefab");
            if (savePath == "") return;
            if (!savePath.Contains("DontChangeTheName"))
            {
                Debug.LogError("不要修改DontChangeTheName");
                return;
            }
        }
        
        PlayerPrefs.SetString("LastSpriteToPrefabPath", savePath);
        if(splitAlpha)
            MaterialTextureForETC1.SelectObjectToPrefabs();
        List<string> chooseObjPath = new List<string>();
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            string spritePath = AssetDatabase.GetAssetPath(Selection.objects[i]);
            chooseObjPath.Add(spritePath);
        }

        for (int i = 0; i < chooseObjPath.Count; i++)
        {
            string spritePath = chooseObjPath[i];
            string folderPath = spritePath.Remove(spritePath.LastIndexOf('/'));
            string spriteName = spritePath.Substring(spritePath.LastIndexOf('/') + 1).Replace(".png", "");
            string tempSavePath = "";
            if (saveCurPath)
            {
                tempSavePath = string.Format("{0}/{1}Prefab.prefab", folderPath, spriteName);
            }
            else
            {
                tempSavePath = savePath.Replace("DontChangeTheName", spriteName + "Prefab");
                tempSavePath = FileBaseUtils.AbsolutePathToAssetPath(tempSavePath);
            }
            CreateSpritePrefabByPath(spritePath, tempSavePath);
        }
        //释放进度条;
        EditorUtility.ClearProgressBar();
    }

    

    public static void CreateSpritePrefabByPath(string spritePath, string savePath)
    {
        string spriteName = spritePath.Substring(spritePath.LastIndexOf('/')).Replace(".png","");

        //所有子Sprite对象;
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(MaterialTextureForETC1.GetRGBTexPath(spritePath)).OfType<Sprite>().ToArray();
        //bool isSoloFolder = EditorUtility.DisplayDialog("创建选择?", "是否单个Sprite创建独立文件夹?", "独立", "共享");
        if (sprites.Length == 0)
        {
            Sprite selfSp = (Sprite)AssetDatabase.LoadAssetAtPath<Sprite>(MaterialTextureForETC1.GetRGBTexPath(spritePath));
            if (selfSp != null) sprites = new Sprite[1] { selfSp };
        }

        //创建对象;
        bool havePrefab = true;
        GameObject prefabGo = AssetDatabase.LoadAssetAtPath(savePath, typeof(GameObject)) as GameObject;
        GameObject go = null;
        if (prefabGo == null)
        {
            havePrefab = false;
            string atlasName = savePath.Replace(".prefab", "");
            atlasName = atlasName.Substring(savePath.LastIndexOfAny(new char[] {'/', '\\'}) + 1);
            go = new GameObject(atlasName);
        }
        else
        {
            go = PrefabUtility.InstantiatePrefab(prefabGo) as GameObject;
        }

        SpritePrefab spritePrefab = go.GetComponent<SpritePrefab>();
        if (spritePrefab == null)
        {
            spritePrefab = go.AddComponent<SpritePrefab>();
        }

        spritePrefab.SpriteList = new List<SpritePrefab.SpriteData>();
        if (sprites.Length == 0)
        {
            TDebug.LogError("请将图片设为sprite");
        }

        Material mat = AssetDatabase.LoadAssetAtPath<Material>(MaterialTextureForETC1.GetMatPath(spritePath));
        if (mat == null)
            Debug.LogError("分离通道的材质为空:" + MaterialTextureForETC1.GetMatPath(spritePath));
        for (int j = 0; j < sprites.Length; j++)
        {  
            //进度条;
            //EditorUtility.DisplayProgressBar(pgTitle, info, nowProgress);
            spritePrefab.SpriteList.Add(new SpritePrefab.SpriteData(sprites[j].name, sprites[j]));
            spritePrefab.Mat = mat;
        }
        if (!havePrefab)
        {
            Object prefab = PrefabUtility.CreateEmptyPrefab(savePath);
            PrefabUtility.ReplacePrefab(go, prefab);
        }
        PrefabUtility.ReplacePrefab(go, AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)), ReplacePrefabOptions.Default);
        DestroyImmediate(go);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //PrefabUtility.ResetToPrefabState(AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)));
        //AssetDatabase.SaveAssets();
        //go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        //NGUISettings.atlas = go.GetComponent<UIAtlas>();
        //Selection.activeGameObject = go;
    }

    /// <summary>
    /// 截取路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="leftIn">左起点</param>
    /// <param name="rightIn">右起点</param>
    /// <returns></returns>
    public static string Inset(string path, int leftIn, int rightIn)
    {
        return path.Substring(leftIn, path.Length - rightIn - leftIn);
    }

    /// <summary>
    /// 截取路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="inset"></param>
    /// <returns></returns>
    public static string InsetFromEnd(string path, int inset)
    {
        return path.Substring(0, path.Length - inset);
    }
}
#endif
