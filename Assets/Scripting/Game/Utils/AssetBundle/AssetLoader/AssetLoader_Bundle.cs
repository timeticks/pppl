using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 从bundle中加载资源
/// </summary>
public class AssetLoader_Bundle : IAssetLoader
{
    private Dictionary<TLoader, AssetBundleRequest> mAynsDict = new Dictionary<TLoader, AssetBundleRequest>();
    private List<TLoader> mFinishedList = new List<TLoader>();
    public void Init()
    {
    }


    public TLoader LoadAssetSync(string bundleName, string assetName)
    {
        return LoadAssetSync(bundleName, assetName, typeof (Object));
    }

    public TLoader LoadAssetSync(string bundleName, string assetName, System.Type ty)
    {
        assetName = assetName.ToLower();
        string url = bundleName + "/" + assetName;
        TLoader loader = null;

        bool isNewCreate;
        loader = TLoader.AutoNew(bundleName,url, out isNewCreate);
        if (isNewCreate)
        {
            if (!SharedAsset.Instance.BundleDict.ContainsKey(bundleName))
            {
                SharedAsset.Instance.LoadBundleSync(new List<string>() {bundleName});
            }
            try
            {
                //TDebug.Log("加载Asset：" + assetName);
                Object target = SharedAsset.Instance.BundleDict[bundleName].LoadAsset(assetName, ty);
                loader.OnFinish(target);
            }catch (System.Exception e)
            {
                TDebug.LogError("加载失败--" + assetName + "   " + e.Message);
            }
        }
        return loader;
    }

    public TLoader LoadObjAsync(string bundleName, string assetName, System.Type ty, System.Action<TLoader> loadFinish)
    {
        string url = bundleName + "/" + assetName;
        TLoader loader = null;
        bool isNewCreate;
        loader = TLoader.AutoNew(bundleName, url, out isNewCreate);
        if (isNewCreate)
        {
            if (!SharedAsset.Instance.BundleDict.ContainsKey(bundleName))
            {
                SharedAsset.Instance.LoadBundleSync(new List<string>() { bundleName });
            }
            try
            {
                AssetBundleRequest req = SharedAsset.Instance.BundleDict[bundleName].LoadAssetAsync(assetName, ty);
                mAynsDict.Add(loader, req);
            }
            catch (System.Exception e)
            {
                TDebug.LogError("加载失败--" + assetName + "   " + e.Message);
            }
        }

        return loader;
    }

    public void ClearAll()
    {
        
    }

    public IEnumerator LoadAsyncUpdate() //用于异步缓存加载
    {
        while (true)
        {
            if (mAynsDict.Count > 0)
            {
                foreach (var item in mAynsDict)
                {
                    if (item.Key.IsFinish(item.Value))
                    {
                        mFinishedList.Add(item.Key);
                    }
                }
                for (int i = 0; i < mFinishedList.Count; i++)
                {
                    mAynsDict.Remove(mFinishedList[i]);
                }
                if (mFinishedList.Count > 0) mFinishedList.Clear();
            }
            yield return null;
        }
    }
}