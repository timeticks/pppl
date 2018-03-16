using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Asset加载接口
/// </summary>
public interface IAssetLoader
{
    void Init();


    /// <summary>
    /// 加载Object
    /// </summary>
    TLoader LoadAssetSync(string bundleName, string assetName);

    /// <summary>
    /// 加载Object资源，bundleName形如：ui/areaui。
    /// </summary>
    TLoader LoadAssetSync(string bundleName, string assetName, System.Type ty);

    TLoader LoadObjAsync(string bundleName, string assetName, System.Type ty, System.Action<TLoader> del);

    IEnumerator LoadAsyncUpdate();

}


public enum AssetLoaderType:byte
{
    TResources,
    Bundle
}
