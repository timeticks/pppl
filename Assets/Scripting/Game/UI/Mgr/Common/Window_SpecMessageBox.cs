using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class Window_SpecMessageBox : WindowBase
{
    public class ViewObj
    {
        public Text ConditionText;
        public Button BtnOk;
        public Button BtnCancel;

        public ViewObj(UIViewBase view)
        {
            ConditionText = view.GetCommon<Text>("ConditionText");
            BtnOk = view.GetCommon<Button>("BtnOk");
            BtnCancel = view.GetCommon<Button>("BtnCancel");
        }
    }
    private ViewObj mViewObj;
    private Action mOkCallBack;
    private Action mCancelCallBack;
    private float mBtnPosOffset = 180f;   //按钮居左与居中时的位置差
    private bool mOnlyOk;

    public Button Btn_Ok
    {
        get { return mViewObj.BtnOk; }
    }
    public Button Btn_Cancel
    {
        get { return mViewObj.BtnCancel; }
    }

    public void OpenWindow(int codeStatus)
    {
        if (mViewObj == null) { mViewObj = new ViewObj(mViewBase); }
        OpenWin();
        Init();
        ErrorStatus error = ErrorStatus.ErrorStatusFetcher.GetErrorStatusByCopy(codeStatus);
        string str = error == null ? "未知错误" : error.name;
        ShowInfo_OnlyOk(null, str, Color.red);
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
        Reset();
    }


    public void Init ()
	{
	    mViewObj = new ViewObj(GetComponent<UIViewBase>());
        mViewObj.BtnOk.SetOnClick(BtnEvt_Ok);
        mViewObj.BtnCancel.SetOnClick(BtnEvt_Cancel);
	}

    public void ShowInfo_HaveCancel(string msg, Action callBack, Action cancelBack)
    {
        Reset();
        mOkCallBack = callBack;
        mCancelCallBack = cancelBack;
        mViewObj.ConditionText.text = msg;
        gameObject.SetActive(true);
        mViewObj.BtnOk.gameObject.SetActive(true);
        mViewObj.BtnCancel.transform.localPosition = new Vector3(-mBtnPosOffset, mViewObj.BtnCancel.transform.localPosition.y, mViewObj.BtnCancel.transform.localPosition.z);
        mViewObj.BtnCancel.gameObject.SetActive(true);
        mViewObj.BtnOk.transform.localPosition = new Vector3(mBtnPosOffset, mViewObj.BtnCancel.transform.localPosition.y, mViewObj.BtnCancel.transform.localPosition.z);

    }

    public void ShowInfo_HaveCancel(string msg,Action callBack)
    {
        Reset();
        mOkCallBack = callBack;
        mViewObj.ConditionText.text = msg;
        gameObject.SetActive(true);
        mViewObj.BtnOk.gameObject.SetActive(true);
        mViewObj.BtnCancel.transform.localPosition = new Vector3(-mBtnPosOffset, mViewObj.BtnCancel.transform.localPosition.y, mViewObj.BtnCancel.transform.localPosition.z);
        mViewObj.BtnCancel.gameObject.SetActive(true);
        mViewObj.BtnOk.transform.localPosition = new Vector3(mBtnPosOffset, mViewObj.BtnCancel.transform.localPosition.y, mViewObj.BtnCancel.transform.localPosition.z);
    }

    public void ShowInfo_OnlyOk(string msg , Color textCol)
    {
        Reset();
        mViewObj.ConditionText.text = msg;
        gameObject.SetActive(true);
        mViewObj.BtnOk.gameObject.SetActive(true);
        mViewObj.BtnOk.transform.localPosition = new Vector3(0f, mViewObj.BtnCancel.transform.localPosition.y, mViewObj.BtnCancel.transform.localPosition.z);
        mViewObj.BtnCancel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 客户端拦截显示
    /// 返回是否进行拦截
    /// </summary>
    public bool ShowStatus(int codeStatus,Action callBack=null)
    {
        ErrorStatus error = ErrorStatus.ErrorStatusFetcher.GetErrorStatusByCopy(codeStatus);
        string errorContext = error == null ? "未知错误" : error.name;
        if (GameClient.IsShowWarn)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(callBack, errorContext, Color.red);
            return true;
        }
        else
        {
            TDebug.Log(string.Format("跳过客户端拦截:{0}", errorContext));
            return false;
        }
    }
    /// <summary>
    /// 客户端拦截显示
    /// 返回是否进行拦截
    /// </summary>
    public bool ShowStatus(string statusStr, Action callBack = null)
    {
        if (GameClient.IsShowWarn)
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(callBack, statusStr, Color.red);
            return true;
        }
        else
        {
            TDebug.Log(string.Format("跳过客户端拦截:{0}", statusStr));
            return false;
        }
    }



    public void ShowInfo_OnlyOk(Action callBack, string msg, Color textCol)
    {
        Reset();
        mOnlyOk = true;
        mOkCallBack = callBack;
        mViewObj.ConditionText.text = msg;
        gameObject.SetActive(true);
        mViewObj.BtnOk.gameObject.SetActive(true);
        mViewObj.BtnOk.transform.localPosition = new Vector3(0f, mViewObj.BtnCancel.transform.localPosition.y, mViewObj.BtnCancel.transform.localPosition.z);
        mViewObj.BtnCancel.gameObject.SetActive(false);
    }

    void BtnEvt_Ok()
    {
        System.Action okCallback = mOkCallBack;
        mOkCallBack = null;
        AudioMgr.PlayClickAudio();
        CloseWindow();
        if (okCallback != null) okCallback();
    }

    void BtnEvt_Cancel()
    {
        if (mOnlyOk) return;
        AudioMgr.PlayClickAudio();
        if (!mViewObj.BtnCancel.gameObject.activeSelf)//如果没有取消按钮，则调用确认
        {
            BtnEvt_Ok();
        }
        else
        {
            if (mCancelCallBack != null) mCancelCallBack();
            mCancelCallBack = null;
            CloseWindow();
        }
        //StopCoroutine("CountDown");
    }

    private void Reset()
    {
        mViewObj.BtnOk.gameObject.SetActive(false);
        mViewObj.BtnCancel.gameObject.SetActive(false);
        mOkCallBack = null;
        mCancelCallBack = null;
        mOnlyOk = false;
     }

}
