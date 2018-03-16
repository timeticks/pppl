using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class FindPrefabRef : MonoBehaviour {
    [MenuItem("Export/Find/Find Object Reference")]
    static public void FindScriptReference()  //查找某一物体，被哪些Prefab和Scene引用
    {
        ShowProgress(0, 0, 0);
        string curPathName = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        Debug.Log("Find: =============================" + curPathName);
        //TDebug.Log("Find: =============================" + curPathName);
        string[] allGuids = AssetDatabase.FindAssets("t:Prefab t:Scene", new string[] { "Assets" });  //找到所有prefab和scene的guid
        int i = 0;
        foreach (string guid in allGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string[] names = AssetDatabase.GetDependencies(new string[] { assetPath });  //依赖的
            foreach (string name in names)
            {
                if (name.Equals(curPathName) && assetPath != name)
                {
                    Debug.Log("Reference:" + assetPath); break;
                }
            }
            ShowProgress((float)i / (float)allGuids.Length, allGuids.Length, i);
            i++;
        }
        EditorUtility.ClearProgressBar();
    }

   
    /// <summary>
    /// 查找对象引用的类型
    /// </summary>
    [MenuItem("Export/Find/Find Object Dependencies")]
    public static void FindObjectDependencies()
    {
        ShowProgress(0,0,0);
        //Dictionary<string, BetterList<string>> dic = new Dictionary<string, BetterList<string>>();
        //BetterList<string> prefabList = new BetterList<string>();
        //BetterList<string> fbxList = new BetterList<string>();
        //BetterList<string> scriptList = new BetterList<string>();
        //BetterList<string> textureList = new BetterList<string>();
        //BetterList<string> matList = new BetterList<string>();
        //BetterList<string> shaderList = new BetterList<string>();
        //BetterList<string> fontList = new BetterList<string>();
        //BetterList<string> animList = new BetterList<string>();
        //BetterList<string> animTorList = new BetterList<string>();
        string curPathName = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        string[] names = AssetDatabase.GetDependencies(new string[] { curPathName });  //依赖的
        int i = 0;
        foreach (string name in names)
        {
            Debug.Log("Dependence:"+name);
            ShowProgress((float)i / (float)names.Length,names.Length,i);
            i++;
        }
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Export/Find/Find Sprite Reference In Image By Name")]
    static public void FindSpriteReference()  //查找某一Sprite，通过名字查找被哪些Prefab引用
    {
        ShowProgress(0, 0, 0);
        string curPathName = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        //TDebug.Log("Find: =============================" + curPathName);
        string[] allGuids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets" });  //找到所有prefab和scene的guid
        int i = 0;
        curPathName = curPathName.Substring(curPathName.LastIndexOf("/")+1);
        string spriteName = curPathName.Remove(curPathName.LastIndexOf("."));
        Debug.Log(string.Format("Find: ============================={0}  \n{1}", curPathName, spriteName));
        foreach (string guid in allGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof (Object));
            GameObject go = Instantiate(obj)as GameObject;
            ActiveAllChild(go.transform);
            Image[] graList = go.transform.GetComponentsInChildren<Image>();
            CheckSprite(obj.name, spriteName, graList);
            graList = go.transform.GetComponents<Image>();
            CheckSprite(obj.name, spriteName, graList);

            DestroyImmediate(go);
            ShowProgress((float)i / (float)allGuids.Length, allGuids.Length, i);
            i++;
        }
        EditorUtility.ClearProgressBar();
    }

    static void CheckSprite(string objName ,string spriteName , Image[] images)
    {
        for (int j = 0; j < images.Length; j++)
        {
            if (images[j].sprite != null && images[j].sprite.name == spriteName)
            {
                Debug.Log(string.Format("有此图片：{0}   |{1}", objName, images[j].transform.name));
            }
            else if (images[j].overrideSprite != null && images[j].overrideSprite.name == spriteName)
            {
                Debug.Log(string.Format("有此图片：{0}   |{1}", objName, images[j].transform.name));
            }
        }
    }
    static void ActiveAllChild(Transform parentTrans)
    {
        for (int i = 0; i < parentTrans.childCount; i++)
        {
            Transform childTrans = parentTrans.GetChild(i);
            childTrans.gameObject.SetActive(true);
            ActiveAllChild(childTrans);
        }
    }
    [MenuItem("Export/Find/Find All Titled  Sprite")]
    static public void FindTitledSpriteReference()  //查找某一Sprite，通过名字查找被哪些Prefab引用
    {
        ShowProgress(0, 0, 0);       
        string[] allGuids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets" });  //找到所有prefab和scene的guid
        int i = 0;    
        foreach (string guid in allGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
            GameObject go = Instantiate(obj) as GameObject;
            ActiveAllChild(go.transform);
            Image[] graList = go.transform.GetComponentsInChildren<Image>();
            CheckTiledSprite(obj.name, graList);
            graList = go.transform.GetComponents<Image>();
            CheckTiledSprite(obj.name, graList);

            DestroyImmediate(go);
            ShowProgress((float)i / (float)allGuids.Length, allGuids.Length, i);
            i++;
        }
        EditorUtility.ClearProgressBar();
    }

    static void CheckTiledSprite(string objName, Image[] images)
    {
        for (int j = 0; j < images.Length; j++)
        {
            if (images[j].sprite != null && images[j].type== Image.Type.Tiled)
            {
                Debug.Log(string.Format("有此图片：{0}   |{1}", objName, images[j].transform.name));
            }
            else if (images[j].overrideSprite != null && images[j].type == Image.Type.Tiled)
            {
                Debug.Log(string.Format("有此图片：{0}   |{1}", objName, images[j].transform.name));
            }
        }
    }

    [MenuItem("Export/Find/Find All Viewport")]
    static public void FindViewPort()  //查找某一Sprite，通过名字查找被哪些Prefab引用
    {
        ShowProgress(0, 0, 0);
        string[] allGuids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets" });  //找到所有prefab和scene的guid
        int i = 0;
        foreach (string guid in allGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
            GameObject go = Instantiate(obj) as GameObject;
            ActiveAllChild(go.transform);
            Transform[] graList = go.transform.GetComponentsInChildren<Transform>();
            CheckViewport(obj.name, graList);
            graList = go.transform.GetComponents<Transform>();
            CheckViewport(obj.name, graList);

            DestroyImmediate(go);
            ShowProgress((float)i / (float)allGuids.Length, allGuids.Length, i);
            i++;
        }
        EditorUtility.ClearProgressBar();
    }

    static void CheckViewport(string objName, Transform[] images)
    {
        for (int j = 0; j < images.Length; j++)
        {
            if (images[j].gameObject.name == "Viewport" || images[j].gameObject.name == "viewport")
            {
                if (images[j].gameObject.GetComponent<Image>() != null || images[j].gameObject.GetComponent<RawImage>() != null)
                    Debug.LogError(string.Format("Error：{0}   |{1}", objName, images[j].transform.name));
            }
            if (Mathf.Abs(images[j].localPosition.z)>0.001f)
            {
                Debug.LogError(string.Format("PosError：{0}   |{1}   |{2}", objName, images[j].transform.name,images[j].localPosition.z));
            }
            if (images[j].GetComponent<Text>() != null && images[j].GetComponent<Text>().font != null&&images[j].GetComponent<Text>().font.name == "Arial")
            {
                Debug.LogError(string.Format("Error：{0}   |{1} text is Arial", objName, images[j].transform.name));
            }
        }
    }

    [MenuItem("Export/Find/Find AllAssetInFolder")]
    public static List<string> GetAllAssetInFolder()  //找到某文件夹下的所有非文件夹资源
    {
        //路径  
        string fullPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        List<string> allAssetPath = new List<string>();
        Dictionary<string, bool> allAssetName = new Dictionary<string, bool>();

        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                ShowProgress((float)i / files.Length, files.Length, i);
                if (files[i].Name.EndsWith(".meta")) { continue; }
                allAssetPath.Add(files[i].FullName);

                if (!allAssetName.ContainsKey(files[i].Name)) { allAssetName.Add(files[i].Name, true); }
                else { Debug.LogError("有相同名字的文件" + files[i].Name); }

                //Debug.Log("Name:" + files[i].Name);
                //Debug.Log( "FullName:" + files[i].FullName );  
                //Debug.Log( "DirectoryName:" + files[i].DirectoryName );  
            }
        }
        EditorUtility.ClearProgressBar();
        return allAssetPath;
    }

    [MenuItem("Export/SetAssetLabel")]
    static void SetAssetLabel()
    {
        List<string> paths = GetAllAssetInFolder();
        for (int i = 0; i < paths.Count; i++) 
        {
            AssetImporter ai = AssetImporter.GetAtPath(paths[i]);
            ai.SetAssetBundleNameAndVariant("aaaa","");
            AssetDatabase.Refresh();
        }
    }


    [MenuItem("Export/Find/Set Win Name")]
    public static void SetWinName() //根据窗口预物体名字，设置WinName
    {
         List<string> paths = GetAllAssetInFolder();
        for (int i = 0; i < paths.Count; i++)
        {
            GameObject o = AssetDatabase.LoadAssetAtPath(paths[i], typeof(GameObject)) as GameObject;
            Debug.Log(paths[i]);
        }
        
    }




    public static void ShowProgress(float val, int total, int cur)
    {
        EditorUtility.DisplayProgressBar("Searching", string.Format("Finding ({0}/{1}), please wait...", cur, total), val);
    }
}
