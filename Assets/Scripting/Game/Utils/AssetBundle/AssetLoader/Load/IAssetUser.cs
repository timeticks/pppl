using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetUser
{
    List<TLoader> AssetLoaderList { get;set;}
    void DisposeUsedAsset();
}
