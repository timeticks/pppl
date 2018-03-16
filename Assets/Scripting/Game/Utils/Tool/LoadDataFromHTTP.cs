using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class LoadDataFromHTTP : MonoBehaviour
{
    private static LoadDataFromHTTP mInstance = null;

    public static LoadDataFromHTTP Instance
    {
        get { return mInstance; }
    }

    public readonly byte DownloadThreadsCount = 3;

    private Dictionary<string, WWWRequest>  mProcessingRequest   = new Dictionary<string, WWWRequest>();
    private Dictionary<string, WWWRequest>  mSucceedRequest      = new Dictionary<string, WWWRequest>();
    private Dictionary<string, WWWRequest>  mFailedRequest       = new Dictionary<string, WWWRequest>();
    private List<WWWRequest>                mWaitingRequests     = new List<WWWRequest>();

    private List<string>               mNewFaileds          = new List<string>();
    private List<string>               mNewFinisheds        = new List<string>();
    private bool                       mIsRunning;

    public class WWWRequest
    {
        public const    int         HTTP_TIMEOUT    = 408;
        public const    int         HTTP_GATEERROR  = 502;
        public const    int         HTTP_RETRYTIME  = 503;

        public string               RequestString   = "";
        public string               Url             = "";
        public int                  TriedTimes      = 0;
        public int                  Priority        = 0;
        public int                  RetryTime       = 100;
        public WWW                  Www             = null;
        public bool                 IsDone;
        public Action<WWWRequest>   SuccessAction;
        public Action<WWWRequest>   FailedAction;
        public long                 Timeout         = 0L;
        public int                  ErrorCode;

        public WWWRequest(Action<WWWRequest> successFunc, Action<WWWRequest> failedFunc)
        {
            SuccessAction   = successFunc;
            FailedAction    = failedFunc;
        }

        public  virtual  void CreatWWW()
        {
            TriedTimes++;
            Timeout = TimeUtils.CurrentTimeMillis + 600 * 1000;
        }

        public virtual void Dispose()
        {
            if (Www != null)
            {
                Www.Dispose();
                Www = null;
                if (SuccessAction != null)
                    SuccessAction = null;
                if (FailedAction != null)
                    FailedAction = null;
              
            }
        }
    }

    public sealed class HttpRequest : WWWRequest
    {
        public WWWForm      Form    =   null;

        public HttpRequest(Action<WWWRequest> successFunc, Action<WWWRequest> failedFunc) :base(successFunc, failedFunc)
        {
          
        }
     
        public override void CreatWWW()
        {
            base.CreatWWW();
            
            if (Form != null) Www = new WWW(Url, Form);
            else Www = new WWW(Url);
        }
    }

    public sealed class BundleRequest : WWWRequest
    {
        //public BundleData       BundleData          = null;
        //public BundleBuildState BundleBuildState    = null;
        public string           BundleName    = null;


        public BundleRequest(Action<WWWRequest> successFunc, Action<WWWRequest> failedFunc,string bundleName) :base(successFunc,failedFunc)
        {
            BundleName = bundleName;
        }

        public override void CreatWWW()
        {
            base.CreatWWW();
            Www = new WWW(Url);
            //if (false && AssetsLoader.Instance.UseCache && BundleBuildState != null)
            //{
#if !(UNITY_4_2 || UNITY_4_1 || UNITY_4_0)
            //    if (true || AssetsLoader.Instance.UseCRC)
            //        Www = WWW.LoadFromCacheOrDownload(Url, BundleBuildState.Version, BundleBuildState.Crc);
            //    else
#endif
            //         Www = WWW.LoadFromCacheOrDownload(Url, BundleBuildState.Version);
            //  }
            //  else

        }
    }
        

    public sealed class AssetsRequest : WWWRequest
    {
        public string SerialPath;

        public AssetsRequest(Action<WWWRequest> successFunc, Action<WWWRequest> failedFunc):base(successFunc, failedFunc)
        {
            SuccessAction = successFunc;
            FailedAction = failedFunc;
        }

        public override void CreatWWW()
        {
            base.CreatWWW();
            Www = new WWW(Url);
           //if (false&& AssetsLoader.Instance.UseCache)
           //{
#if !(UNITY_4_2 || UNITY_4_1 || UNITY_4_0)
               // if (AssetsLoader.Instance.UseCRC)
                  //  Www = WWW.LoadFromCacheOrDownload(Url, BundleBuildState.Version, BundleBuildState.Crc);
               // else
#endif
                  //  Www = WWW.LoadFromCacheOrDownload(Url, BundleBuildState.Version);
           // }
           // else
               
        }
    }

    void Awake()
    {
        mProcessingRequest = new Dictionary<string, WWWRequest>();
        mSucceedRequest = new Dictionary<string, WWWRequest>();
        mFailedRequest = new Dictionary<string, WWWRequest>();
        mWaitingRequests = new List<WWWRequest>();
        mNewFaileds = new List<string>();
        mNewFinisheds = new List<string>();
        mInstance   = this;
        mIsRunning  = true;
    }


    void Update() 
    {
        if (!mIsRunning) return;

        if (mNewFaileds.Count>0) mNewFaileds.Clear();
        if (mNewFinisheds.Count > 0) mNewFinisheds.Clear();
        if (mProcessingRequest.Count > 0)
        {
            foreach (WWWRequest request in mProcessingRequest.Values)
            {
                if (request.Www.error != null)
                {
                    if (request.TriedTimes - 1 < request.RetryTime)
                    {
                        request.CreatWWW();
                    }
                    else
                    {
                        request.ErrorCode = WWWRequest.HTTP_RETRYTIME;
                        TDebug.LogErrorFormat("Download {0} failed for {1} times.Error: {2}" , request.Url, request.TriedTimes.ToString() , request.Www.error);
                        if (!mNewFaileds.Contains(request.Url))
                            mNewFaileds.Add(request.Url);
                    }
                }
                else if (request.Timeout < TimeUtils.CurrentTimeMillis)
                {
                    TDebug.LogErrorFormat("Download time out: {0}" , request.Url);
                    request.ErrorCode = WWWRequest.HTTP_TIMEOUT;
                    if (!mNewFaileds.Contains(request.Url))
                        mNewFaileds.Add(request.Url);

                }
                else if (request.Www.isDone)
                {
                    if (!mNewFaileds.Contains(request.Url))
                    {
                        request.IsDone = true;
                        mNewFinisheds.Add(request.Url);
                    }
                }
            }
        }

        if (mNewFinisheds.Count > 0)
        {
            foreach (string finishedUrl in mNewFinisheds)
            {
                WWWRequest www;
                if (mProcessingRequest.TryGetValue(finishedUrl, out www))
                {
                    mProcessingRequest.Remove(finishedUrl);
                    if (www.SuccessAction != null)
                    {
                        www.SuccessAction(www);
                    }
                    else if (!mSucceedRequest.ContainsKey(finishedUrl))
                    {
                        mSucceedRequest.Add(finishedUrl, www);
                    }
                }
            }
        }


        if (mNewFaileds.Count > 0)
        {
            foreach (string finishedUrl in mNewFaileds)
            {
                WWWRequest www;
                if (mProcessingRequest.TryGetValue(finishedUrl, out www))
                {
                    mProcessingRequest.Remove(finishedUrl);
                    if (www.FailedAction != null)
                        www.FailedAction(www);
                    else if (!mFailedRequest.ContainsKey(finishedUrl))
                        mFailedRequest.Add(finishedUrl, www);
                }
            }
        }

        int waitingIndex = 0;
        while (mProcessingRequest.Count < DownloadThreadsCount &&
               waitingIndex < mWaitingRequests.Count)
        {

            WWWRequest curRequest = mWaitingRequests[waitingIndex++];
            mWaitingRequests.Remove(curRequest);
            curRequest.CreatWWW();
            mProcessingRequest.Add(curRequest.Url, curRequest);
        }
    }

    /************************************************************************/
    /* HTTP构建请求的信息                                                                     
    /************************************************************************/
  
    #region HTTP协议相关
    public HttpRequest StartHttpRequest(string url, Action<WWWRequest> successAction, Action<WWWRequest> failedAction, WWWForm form = null)
    {
        HttpRequest request = new HttpRequest(successAction, failedAction);
        request.Url         = url;
        RequestHttp(request);
        return request;
    }

    void RequestHttp(WWWRequest request)
    {
        if (mSucceedRequest.ContainsKey(request.Url))
            return;

        request.CreatWWW();
        TDebug.LogWarningFormat("start http request:{0}" , request.Url);
        mProcessingRequest.Add(request.Url, request);
    }
    #endregion


    #region Bundle协议相关
    /// <summary>
    /// 开始下载一个Bundle
    /// </summary>
    /// <param name="url">路径</param>
    /// <param name="bundleData">bundle数据</param>
    /// <param name="buildState">打包属性</param>
    /// <returns></returns>
    public BundleRequest StartDLCBundle(string url, string bundleName, Action<WWWRequest> successAction, Action<WWWRequest> failedAction)
    {

        if (mSucceedRequest.ContainsKey(url) || IsInWaitingList(url)) return null;

        BundleRequest request = new BundleRequest(successAction, failedAction, bundleName);
        request.Url             = url;


        int insertPos   = mWaitingRequests.FindIndex(x => x.Priority < request.Priority);
        insertPos       = insertPos == -1 ? mWaitingRequests.Count : insertPos;
        mWaitingRequests.Insert(insertPos, request);
        return request;

    }
    
    public void AddRequestToWaitingList(WWWRequest request)
    {
        if (mSucceedRequest.ContainsKey(request.Url) || IsInWaitingList(request.Url))
            return;

        int insertPos   = mWaitingRequests.FindIndex(x => x.Priority < request.Priority);
        insertPos       = insertPos == -1 ? mWaitingRequests.Count : insertPos;
        mWaitingRequests.Insert(insertPos, request);
    }

    public void StopAllRequest()
    {
        mIsRunning = false;
        foreach (WWWRequest request in mProcessingRequest.Values)
        {
            request.Dispose();
        }
        mNewFinisheds.Clear();
        mProcessingRequest.Clear();
    }

    private bool IsInWaitingList(string url)
    {
        foreach (WWWRequest request in mWaitingRequests)
            if (request.Url == url)
                return true;

        return false;
    }

    #endregion


    public WWWRequest GetWWW(string url)
    {
        if (mSucceedRequest.ContainsKey(url))
        {
            WWWRequest request = mSucceedRequest[url];
            return request;
        }
        else
            return null;
    }

    public IEnumerator WaitDownload(string url)
    {
        while (!mSucceedRequest.ContainsKey(url))
        {
            yield return new WaitForEndOfFrame();
        }
        WWWRequest request = mSucceedRequest[url];
        mSucceedRequest.Remove(url);
        yield return request;
    }

   
}
