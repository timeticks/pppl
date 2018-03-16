#region Copyright (c) 2015 KEngine / Kelly <http://github.com/mr-kelly>, All rights reserved.

// KEngine - Toolset and framework for Unity3D
// Github: https://github.com/mr-kelly/KEngine
// 
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using KEngine;
using UnityEngine;
 
/// <summary>
/// 主要功能：
/// 1、同一接口加载Simulate和RealFromBundle资源
/// 2、将Loader作为返回对象。里面存有加载过程信息和加载对象。同一返回实现同步加载和异步加载
/// 3、资源使用完成时，调用Release来计算引用值，小于0时触发回收
/// 4、资源回收时，先放在回收列表，几秒后再真正回收。以免回收后又立即使用的情况
/// 5、创建Gameobject，实时显示各资源与其引用数----暂不实现
/// </summary>

/// <summary>
/// 所有资源Loader继承这个
/// </summary>
public class TLoader : IAsyncObject, ITLoader
{
    private readonly List<Action<TLoader>> mFinishedCallbacks = new List<Action<TLoader>>();

    /// <summary>
    /// 最终加载结果的资源
    /// </summary>
    public object ResultObject { get; private set; }

    /// <summary>
    /// 是否已经完成，它的存在令Loader可以用于协程StartCoroutine
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// 类似WWW, IsFinished再判断是否有错误对吧
    /// </summary>
    public bool IsError { get; private set; }

    public string AsyncMessage { get; private set; }

    /// <summary>
    /// 是否可用
    /// </summary>
    public bool IsSuccess
    {
        get { return !IsError && ResultObject != null && !IsWaitDisposed; }
    }

    /// <summary>
    /// 是否处于Application退出状态
    /// </summary>
    private bool mIsQuitApplication = false;

    /// <summary>
    /// RefCount 为 0，进入预备状态
    /// </summary>
    protected bool IsWaitDisposed { get; private set; }

    [System.NonSerialized] public float InitTiming = -1;
    [System.NonSerialized] public float FinishTiming = -1;

    /// <summary>
    /// 用时
    /// </summary>
    public float FinishUsedTime
    {
        get
        {
            if (!IsCompleted) return -1;
            return FinishTiming - InitTiming;
        }
    }

    /// <summary>
    /// 引用计数
    /// </summary>
    public int RefCount {
        get { return mRefCount; }
        private set { mRefCount = value;
            FreshObjToShowRefCount(false);
        } 
    }

    private int mRefCount;

    public string Url { get; private set; } //bundle+assetName
    public string FromAssetBundleName;  //属于的bundle名

    /// <summary>
    /// 进度百分比~ 0-1浮点
    /// </summary>
    public virtual float Progress { get; protected set; }


    private string mDesc = "";
    /// <summary>
    /// 描述, 额外文字, 一般用于资源Debugger用
    /// </summary>
    public virtual string Desc
    {
        get { return mDesc; }
        set { mDesc = value; }
    }


    /// <summary>
    /// 复活
    /// </summary>
    protected virtual void Revive()
    {
        IsWaitDisposed = false; // 复活！
    }

    private TLoader()  //不能外部构造
    { }

    protected virtual void Init(string fromAssetbunleName ,string url)
    {
        InitTiming = Time.realtimeSinceStartup;
        ResultObject = null;
        IsWaitDisposed = false;
        IsError = false;
        IsCompleted = false;
        FromAssetBundleName = fromAssetbunleName;

        Url = url;
        Progress = 0;
    }

    public virtual void OnFinish(object resultObj)
    {
        ResultObject = resultObj;

        FinishTiming = Time.realtimeSinceStartup;
        Progress = 1;
        IsError = false;
        IsCompleted = true;
        if (IsWaitDisposed) //如果在完成时，却已经在待回收列表。则不执行完成回调
        {
        }
        else
        {
            DoCallback(IsSuccess, ResultObject);
        }
    }

    /// <summary>
    /// 如果完成，返回true
    /// </summary>
    public virtual bool IsFinish(AssetBundleRequest req)
    {
        if (req.isDone)
        {
            OnFinish(req.asset);
            return true;
        }
        else
        {
            Progress = req.progress;
        }
        return false;
    }

    /// <summary>
    /// 在IsFinisehd后悔执行的回调
    /// </summary>
    /// <param name="callback"></param>
    protected void AddFinishCallback(Action<TLoader> callback)
    {
        if (callback != null)
        {
            if (IsCompleted)
            {
                if (ResultObject == null)
                    Debug.LogWarning("Null ResultAsset {0}");
                callback(this);
            }
            else
                mFinishedCallbacks.Add(callback);
        }
    }

    protected void DoCallback(bool isOk, object resultObj)
    {
        foreach (var callback in mFinishedCallbacks)
        {
            callback(this);
        }
        mFinishedCallbacks.Clear();
    }

    /// <summary>
    /// 当被执行了ReleaseNow，这里控制成true； DoDipose的其它类，根据这个来判断自己是否也需要releaseNow
    /// </summary>
    protected internal bool IsBeenReleaseNow = false;

    /// <summary>
    /// 执行Release，并立刻触发残余清理
    /// </summary>
    /// <param name="gcNow">是否立刻触发垃圾回收，默认垃圾回收是隔几秒进行的</param>
    public virtual void Release(bool gcNow)
    {
        if (gcNow)
            IsBeenReleaseNow = true;
        Release();
        if (gcNow)
            AssetLoaderPool.DoGarbageCollect();
    }

    /// <summary>
    /// 释放资源，减少引用计数
    /// </summary>
    public virtual void Release()
    {
        if (IsWaitDisposed)
        {
            Debug.LogWarning("[{0}]Too many dipose! {1}, Count: {2}");
        }
        RefCount--;
        if (RefCount <= 0)
        {
            if (RefCount < 0 || AssetLoaderPool.UnUsesLoaders.ContainsKey(this))
            {
                Debug.LogError("引用计数错误");
                RefCount = Mathf.Max(0, RefCount);
            }
            // 加入队列，准备Dispose
            AssetLoaderPool.UnUsesLoaders[this] = Time.realtimeSinceStartup;
            IsWaitDisposed = true;
            OnReadyDisposed();
            //DoGarbageCollect();
        }
    }

    /// <summary>
    /// 引用为0时，进入准备Disposed状态时触发
    /// </summary>
    protected virtual void OnReadyDisposed()
    {
    }

    /// <summary>
    /// Dispose是有引用检查的， DoDispose一般用于继承重写
    /// 回收此资源
    /// </summary>
    public void Dispose()
    {
        var type = GetType();
        AssetLoaderPool.RemoveLoader(Url, this);
        if (IsCompleted)
            DoDispose();
    }

    protected virtual void DoDispose()
    {
        //Debug.Log("回收" + Url);
        if (ResultObject != null && ((ResultObject is GameObject || ResultObject is Component)))
        {
            ResultObject = null;
            //Resources.UnloadUnusedAssets(); //AssetLoaderPool中时间间隔3分钟回收
        }
        else
        {
            //TODO:这里在IL中会报错
            if (ResultObject != null) Resources.UnloadAsset((UnityEngine.Object)ResultObject);//Resources.UnloadAsset仅能释放非GameObject和Component的资源，比如Texture、Mesh等真正的资源。对于由Prefab加载出来的Object或Component，则不能通过该函数来进行释放。
            ResultObject = null;
            //Resources.UnloadUnusedAssets();
        }
        FreshObjToShowRefCount(true);
    }


    /// <summary>
    /// By Unity Reflection
    /// </summary>
    protected void OnApplicationQuit()
    {
        mIsQuitApplication = true;
    }

    /// <summary>
    /// 方法中会先检查LoaderPool中有无此资源
    /// </summary>
    public static TLoader AutoNew(string fromBundleName , string url , out bool isNewCreate , Action<TLoader> callback = null) //where T : AbstractLoader, new()
    {
        TLoader loader;
        if (string.IsNullOrEmpty(url))
        {
            TDebug.LogError(string.Format("[{0}:AutoNew  url为空",fromBundleName));
        }

        isNewCreate = false;
        if (!AssetLoaderPool.TryGetLoader(url, out loader)) //之前没有此资源
        {
            isNewCreate = true;
            loader = new TLoader();
            loader.Init(fromBundleName, url);
            AssetLoaderPool.AddLoader(url, loader);
        }
        else if (loader.RefCount < 0)
        {
            Debug.LogError("Error RefCount!");
        }
        loader.RefCount++;

        // RefCount++了，重新激活，在队列中准备清理的Loader
        if (AssetLoaderPool.UnUsesLoaders.ContainsKey(loader))
        {
            AssetLoaderPool.UnUsesLoaders.Remove(loader);
            loader.Revive();
        }
        loader.AddFinishCallback(callback);

        return loader;
    }


    public T ResultObjTo<T>()where T: UnityEngine.Object
    {
        if (ResultObject == null) return null;
        return ResultObject as T;
    }

#if UNITY_EDITOR
    private GameObject mShowRefCountObj;
#endif

    void FreshObjToShowRefCount(bool isDestroy) //用于在游戏场景中创建物体，以显示当前各资源的引用数
    {
#if UNITY_EDITOR
        if (isDestroy && mShowRefCountObj != null)
            GameObject.Destroy(mShowRefCountObj);
        if (mShowRefCountObj == null)
        {
            mShowRefCountObj = new GameObject("");
        }
        string refCountName = string.Format("{0}|{1}", Url, RefCount);
        mShowRefCountObj.name = refCountName;
#endif
    }


}
