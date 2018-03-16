using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 改自KSFramework项目
/// </summary>
public class AssetLoaderPool 
{
    //所有的资源Laoder池   <Loader类型，<url，loader>>
    private static readonly Dictionary<Type, Dictionary<string, TLoader>> loadersPool =
        new Dictionary<Type, Dictionary<string, TLoader>>()
        {
            {typeof (TLoader), new Dictionary<string, TLoader>()}
        };
    //public static Dictionary<string, TLoader> GetTypeDict(Type type)
    //{
    //    Dictionary<string, TLoader> typesDict;
    //    if (!loadersPool.TryGetValue(type, out typesDict))
    //    {
    //        typesDict = loadersPool[type];
    //    }
    //    else
    //    {
    //        typesDict = new Dictionary<string, TLoader>();
    //    }
    //    return typesDict;
    //}

    public static void AddLoader(string url, TLoader loader)
    {
        if (loader == null) return;
        if (loadersPool[typeof (TLoader)].ContainsKey(url))
        {
            TDebug.LogError(string.Format("同一url资源被重复添加:{0}", url));
        }
        loadersPool[typeof (TLoader)].Add(url , loader);
    }

    public static void RemoveLoader(string url, TLoader loader)
    {
        if (loadersPool[typeof (TLoader)].ContainsKey(url))
        {
            bool bRemove = loadersPool[typeof (TLoader)].Remove(url);
            if (!bRemove)
            {
                TDebug.LogWarning(string.Format("{0}:移除失败", url));
            }
            return;
        }
        TDebug.LogWarning(string.Format("{0}:没有此loader，无法移除", url));
    }

    public static bool TryGetLoader(string url , out TLoader loader)
    {
        if (loadersPool[typeof (TLoader)].ContainsKey(url))
        {
            loader = loadersPool[typeof (TLoader)][url];
            return true;
        }
        else
        {
            loader = null;
            return false;
        }
    }


    /// <summary>
    /// Loader延迟Dispose
    /// </summary>
    private const float LoaderDisposeTime = 2;

    /// <summary>
    /// 间隔多少秒做一次GC(在AutoNew时)
    /// </summary>
    public static float GcIntervalTime = 5;

    /// <summary>
    /// 上次做GC的时间
    /// </summary>
    private static float _lastGcTime = -1;

    private static float _lastUnloadTime = -1;

    /// <summary>
    /// 缓存起来要删掉的，供DoGarbageCollect函数用, 避免重复的new List
    /// </summary>
    private static readonly List<TLoader> DisposeLoaderList = new List<TLoader>();

    /// <summary>
    /// 待回收列表，等待LoaderDisposeTime后再进行回收
    /// </summary>
    public static readonly Dictionary<TLoader, float> UnUsesLoaders = new Dictionary<TLoader, float>();


    /// <summary>
    /// 是否进行垃圾收集
    /// </summary>
    public static void CheckGcCollect()
    {
        if (_lastGcTime.Equals(-1) || (Time.time - _lastGcTime) >= GcIntervalTime)
        {
            DoGarbageCollect();
            _lastGcTime = Time.time;
        }

        if (Time.realtimeSinceStartup - _lastUnloadTime >= 200)
        {
            _lastUnloadTime = Time.realtimeSinceStartup;
            Resources.UnloadUnusedAssets();
        }
    }

    public static void DoGarbageCollect()// 判断回收列表中资源的时间，大于等待时间的进行回收
    {
        foreach (var kv in UnUsesLoaders)
        {
            if ((Time.realtimeSinceStartup - kv.Value) >= LoaderDisposeTime)
            {
                DisposeLoaderList.Add(kv.Key);
            }
        }
        for (int i = 0; i < DisposeLoaderList.Count; i++)
        {
            TLoader loader = DisposeLoaderList[i];
            UnUsesLoaders.Remove(DisposeLoaderList[i]);

            DisposeLoaderList[i] = null;
            DisposeLoaderList.RemoveAt(i);
            i--;
            loader.Dispose();
        }

        if (DisposeLoaderList.Count > 0)
        {
            Debug.LogError("DisposeLoaderList.Count > 0");
        }
    }


    public static  void ClearAll()
    {
        if (loadersPool != null)
        {
            Dictionary<string, TLoader> tDict = loadersPool[typeof (TLoader)];
            List<TLoader> loaderList = new List<TLoader>();
            foreach (var temp in tDict)
            {
                loaderList.Add(temp.Value);
            }
            for (int i = 0; i < loaderList.Count; i++)
            {
                if (loaderList[i] != null) loaderList[i].Dispose();
            }
            tDict.Clear();
        }
        for (int i = 0; i < DisposeLoaderList.Count; i++)
        {
            DisposeLoaderList[i].Dispose();
        }
        DisposeLoaderList.Clear();
        foreach (var temp in UnUsesLoaders)
        {
            temp.Key.Dispose();
        }
        UnUsesLoaders.Clear();
    }

}
