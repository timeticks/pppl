using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

/// <summary>
/// 用于在ilr的外部代码中，某些情况需要调用Editor下的方法
/// </summary>
public class EditorUtils 
{
    public static string[] GetAssetPathsFromAssetBundleAndAssetName(string assetBundleName, string assetName)
    {
#if UNITY_EDITOR
        return AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
#endif
        return null;
    }

    public static UnityEngine.Object LoadAssetAtPath(string assetPath, System.Type type)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath(assetPath, type);
#endif
        return null;
    }

    public static UnityEngine.AsyncOperation LoadLevelAsyncInPlayMode(string path)
    {
#if UNITY_EDITOR
        return EditorApplication.LoadLevelAsyncInPlayMode(path);
#endif
        return null;
    }
}
