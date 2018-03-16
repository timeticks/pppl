using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Window_LoadBar : MonoBehaviour
{
    public static Window_LoadBar Instance;
    #region View

    public class AsyncData
    {
        public float Progress;
        public bool IsDone;
        public string Desc;     
    }

    public class ViewObj
    {
        //public Transform ProgressRoot;
        public Scrollbar ProgressScrollbar;
        public RawImage BgImage;
        public Text DescText;      
        public ViewObj(UIViewBase view)
        {
            //ProgressRoot = view.GetCommon<Transform>("ProgressRoot");
            ProgressScrollbar = view.GetCommon<Scrollbar>("ProgressScrollbar");
            BgImage = view.GetCommon<RawImage>("BgImage");
            DescText = view.GetCommon<Text>("DescText");          
        }
    }
    private UIViewBase mViewBase;
    private ViewObj mViewObj;
    #endregion

    private AsyncOperation mAsyncOp;
    private WWW mWWW;
    private LoadDataFromHTTP.HttpRequest mReqWWW;

    private AsyncData mAsyncData;
    private Action mFinishDeleg;
    public bool IsDestroy;
    void Awake()
    {
        if (Instance==null)
            Instance = this;
    }
    
    public void SetInstance()
    {
        Instance = this;
    }

    void Update()
    {
        if (mAsyncOp != null)
        {
            mAsyncData.IsDone = mAsyncOp.isDone;
            mAsyncData.Progress = mAsyncOp.progress;
        }
        else if (mWWW != null)
        {
            try
            {
                mAsyncData.IsDone = mWWW.isDone;
                mAsyncData.Progress = mWWW.progress;
            }
            catch
            {
                mAsyncData.IsDone =true;
                mAsyncData.Progress = 1;
            }
        }
        else if (mReqWWW!=null)
        {
            try
            {
                mAsyncData.IsDone = mReqWWW.IsDone || mReqWWW.Www == null;
                mAsyncData.Progress = mReqWWW.IsDone ? 1 : mReqWWW.Www.progress;
            }
            catch
            {
                mAsyncData.IsDone = true;
                mAsyncData.Progress = 1;
            }
        }

        if (mAsyncData != null)
        {
            if (mAsyncData.IsDone)
            {
                if (mFinishDeleg != null) mFinishDeleg();
                Reset();
                gameObject.SetActive(false);
            }
            else
            {
                Fresh(mAsyncData.Progress, mAsyncData.Desc,false);
            }
        }
    }

    public void Init(AsyncOperation async, string desc, Action finishDeleg)
    {
        if (IsDestroy) return;
        Reset();
        mAsyncOp = async;
        if (mAsyncData == null) mAsyncData = new AsyncData();
        mAsyncData.Desc = desc;
        mFinishDeleg = finishDeleg;
        gameObject.SetActive(true);
    }

    public void ShowVersion()
    {
        if (mViewBase == null) mViewBase = GetComponent<UIViewBase>();
        Text TextVersion = mViewBase.GetCommon<Text>("TextVersion");
        TextVersion.text = string.Format("游戏版本:v{0}", AppSetting.Version);
    }

    public void Init(AsyncData async, Action finishDeleg)
    {
        if (IsDestroy) return;
        Reset();
        if (mAsyncData == null) mAsyncData = new AsyncData();
        mAsyncData = async;
        mFinishDeleg = finishDeleg;
        gameObject.SetActive(true);
    }

    public void Init(WWW www, string desc , Action finishDeleg)
    {
        if (IsDestroy) return;
        Reset();
        mWWW = www;
        if (mAsyncData == null) mAsyncData = new AsyncData();
        mAsyncData.Desc = desc;
        mFinishDeleg = finishDeleg;
        gameObject.SetActive(true);
    }
    public void Init(LoadDataFromHTTP.HttpRequest reqWWW, string desc, Action finishDeleg)
    {
        if (IsDestroy) return;
        Reset();
        mReqWWW = reqWWW;
        if (mAsyncData == null) mAsyncData = new AsyncData();
        mAsyncData.Desc = desc;
        mFinishDeleg = finishDeleg;
        gameObject.SetActive(true);
    }

    private void Reset()
    {
        if (IsDestroy) return;

        StopAllCoroutines();
        mAsyncData = null;
        mWWW = null;
        mAsyncOp = null;
        mFinishDeleg = null;
        mReqWWW = null;
        if (mViewBase == null)
        {
            mViewBase = GetComponent<UIViewBase>();
            mViewObj = new ViewObj(mViewBase);
        }
    }

    public void SetFalse()
    {
        Reset();
        gameObject.SetActive(false);
    }

    public float GetCurPct()
    {
        if (mViewBase == null) mViewBase = GetComponent<UIViewBase>();
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        return mViewObj.ProgressScrollbar.size;
    }

    public void Fresh(float pctValue, string str,bool needReset=true)
    {
        if (IsDestroy) return;
        if (needReset)
        {
            Reset();
        } 
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        if (mViewBase == null) mViewBase = GetComponent<UIViewBase>();
        if (mViewObj==null)   mViewObj = new ViewObj(mViewBase);
        mViewObj.ProgressScrollbar.size = pctValue;
        mViewObj.DescText.text = string.Format("{0}   {1}%", str, (pctValue*100).ToString("f0"));
    }

    /// <summary>
    /// 不修改当前的进度，仅显示文字，并中断进度
    /// </summary>
    public void FreshWithoutPct(string str)
    {
        Fresh(mViewObj.ProgressScrollbar.size, str);
    }

    private IEnumerator freshCor;
    /// <summary>
    /// 开启一个协程，进行插值显示
    /// </summary>
    public void Fresh(float startValue, float endValue, float duration, string str,System.Action overCallback=null)
    {
        if (IsDestroy) return;
        if (duration <= 0) duration = 0.1f;
        Fresh(startValue, str);
        if (freshCor != null) StopCoroutine(freshCor);
        freshCor = FreshCor(startValue, endValue, duration, str, overCallback);
        StartCoroutine(freshCor);
    }

    public IEnumerator FreshCor(float startValue, float endValue, float duration, string str, System.Action overCallback)
    {
        float curTime = 0;
        while (curTime < duration)
        {
            yield return null;
            curTime += Time.deltaTime;
            float curValue = Mathf.Lerp(startValue, endValue, curTime / duration);
            Fresh(curValue, str, false);
        }
        if (overCallback != null) overCallback();
    }


    void OnDestroy()
    {
        IsDestroy = true;
    }
}
