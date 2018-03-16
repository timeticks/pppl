using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader_Res :IAssetLoader
{
    public Dictionary<string, Dictionary<string, string>> m_BundleDict = new Dictionary<string, Dictionary<string, string>>(); //bundleName，，文件无后缀名，Assets之下全路径
    private Dictionary<TLoader, System.Action<TLoader>> mAyncDict = new Dictionary<TLoader, System.Action<TLoader>>();

    /// <summary>
    /// 初始化，建立所有类似bundle加载的键值对。
    /// </summary>
    public void Init()
    {
    }


    public TLoader LoadAssetSync(string bundleName, string assetName)
    {
        return LoadAssetSync(bundleName, assetName, typeof (Object));
    }

    public TLoader LoadAssetSync(string bundleName, string assetName, System.Type ty)
    {
        if (PlatformUtils.EnviormentTy == EnviormentType.Editor)
        {
            assetName = assetName.ToLower();
            string url = bundleName + assetName;
            TLoader loader = null;

            bool isNewCreate;
            loader = TLoader.AutoNew(bundleName, url, out isNewCreate);
            if (isNewCreate)
            {
                string[] assetPaths = EditorUtils.GetAssetPathsFromAssetBundleAndAssetName(bundleName, assetName);
                if (assetPaths == null || assetPaths.Length == 0)
                {
                    Debug.LogError(string.Format("There is no asset with name {0} in {1}", assetName, bundleName));
                    return null;
                }
                Object target = EditorUtils.LoadAssetAtPath(assetPaths[0], ty);
                loader.OnFinish(target);
            }
            return loader;
        }
        return null;
    }

    public TLoader LoadObjAsync(string bundleName, string assetName, System.Type ty, System.Action<TLoader> deleg)
    {
        TLoader loader = LoadAssetSync(bundleName, assetName, ty);
        //mAyncDict.Add(loader, deleg);    //等待协程中下一帧完成
        return loader;
    }

    public IEnumerator LoadAsyncUpdate()
    {
        while (true)
        {
            //foreach (var item in mAyncDict)//TODO:删除
            //{
            //    item.Value(item.Key);
            //}
            //mAyncDict.Clear();
            //yield return null;
            yield return null;
        }
    }
}
