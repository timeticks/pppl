using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingCircleCtrl : MonoBehaviour
{
    public class ViewObj
    {
        public RectTransform Window_Loading;
        public RectTransform Mask;
        public Empty4Raycast TransparentImage;
        public RectTransform RotateImage;

        public ViewObj(UIViewBase view)
        {
            Window_Loading = view.GetCommon<RectTransform>("Window_Loading");
            Mask = view.GetCommon<RectTransform>("Mask");
            TransparentImage = view.GetCommon<Empty4Raycast>("TransparentImage");
            RotateImage = view.GetCommon<RectTransform>("RotateImage");
        }
    }
    private ViewObj mViewObj;

    public class ViewValue
    {
        public float AllWaitTime;
        public float TransparentWait;
        public ViewValue(ValueViewBase view)
        {
            AllWaitTime = view.Getstring("AllWaitTime").ToFloat();
            TransparentWait = view.Getstring("TransparentWait").ToFloat();
        }
    }
    private ViewValue mViewValue;
    private float mCurTime = 0;
    [HideInInspector]
    public bool IsActive;

    public void Init()
    {
        mViewObj = new ViewObj(GetComponent<UIViewBase>());
        mViewValue = new ViewValue(GetComponent<ValueViewBase>());
        SetEnable(true);
        SetEnable(false);
    }

    public void SetEnable(bool isActive)
    {
        if (mViewObj == null) Init();
        WindowBase.SetUI(gameObject, isActive);
        IsActive = isActive;
        if (isActive)
        {
            mCurTime = 0f;
            mViewObj.TransparentImage.enabled = true;
            //mViewObj.Mask.gameObject.SetActive(false);
            mViewObj.Mask.localPosition = new Vector3(-3000, 0, 0);
        }
        else
        {
            mCurTime = int.MinValue;
        }
    }


    void Update()
    {
        if (mViewObj == null) Init();
        if (!IsActive) return;
        mCurTime += Time.deltaTime;
        if (mCurTime > mViewValue.TransparentWait)
        {
            if (mViewObj.TransparentImage.enabled)
            {
                mViewObj.TransparentImage.enabled = false;
                mViewObj.Mask.localPosition = new Vector3(0, 0, 0);
            }
        }
        if (mCurTime > mViewValue.AllWaitTime)
        {
            //if (UIRootMgr.Instance != null && LobbySceneMainUIMgr.Instance==null)
            //{
            //    UIRootMgr.Instance.m_CommonWin.m_MessageBox.ShowInfo_OnlyOk("服务器连接超时!", Color.red);
            //}
            System.Action overDel = delegate()
            {
                //GameClient.Instance.LoginOutGame();
            };
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(overDel, "服务器连接超时，请重新登录", Color.red);
            gameObject.SetActive(false);
        }
        mViewObj.RotateImage.Rotate(new Vector3(0, 0, -4.93f));
    }
}
