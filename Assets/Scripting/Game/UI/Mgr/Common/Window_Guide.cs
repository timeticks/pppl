using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Window_Guide : WindowBase
{
    public const int GuideOrderLayer = 10; //引导的层级
    public static Window_Guide Instance { get; private set; }
    public static bool IsGuiding = false;

    public class ViewObj
    {
        public Button BtnMask;
        public Transform RootHighlit;
        public GameObject TransArrowHigh;
        public GameObject TransArrowLow;
        public Transform TextRoot;
        public Text TextGuide;
        public Transform RootArrow;
        public Transform LowRoot;
        public Transform HighRoot;
        public Transform Root;
        public Transform RootArrowImageLow;
        public Transform RootArrowImageHigh;
        public ViewObj(UIViewBase view)
        {
            if (BtnMask == null) BtnMask = view.GetCommon<Button>("BtnMask");
            if (RootHighlit == null) RootHighlit = view.GetCommon<Transform>("RootHighlit");
            if (TransArrowHigh == null) TransArrowHigh = view.GetCommon<GameObject>("TransArrowHigh");
            if (TransArrowLow == null) TransArrowLow = view.GetCommon<GameObject>("TransArrowLow");
            if (TextRoot == null) TextRoot = view.GetCommon<Transform>("TextRoot");
            if (TextGuide == null) TextGuide = view.GetCommon<Text>("TextGuide");
            if (RootArrow == null) RootArrow = view.GetCommon<Transform>("RootArrow");
            if (LowRoot == null) LowRoot = view.GetCommon<Transform>("LowRoot");
            if (HighRoot == null) HighRoot = view.GetCommon<Transform>("HighRoot");
            if (Root == null) Root = view.GetCommon<Transform>("Root");
            RootArrowImageLow = view.GetCommon<Transform>("RootArrowImageLow");
            RootArrowImageHigh = view.GetCommon<Transform>("RootArrowImageHigh");        }
    }
    private ViewObj mViewObj;


    private Transform mUiTrans;
    private Button mGuideBtn;
    private System.Action mCallback;
    private Button.ButtonClickedEvent mBtnClickEvent;
    private bool mLastUiRaycastTarget;
    private System.Action mOutFinishCallback;

    public int CurGuideStep;

    public void OpenWindow()
    {
        Instance = this;
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
    }

    //关闭引导
    public void CloseGuide(bool guideFinish)
    {
        TDebug.Log(string.Format("关闭引导:{0}|{1}", CurGuideStep, guideFinish));
        IsGuiding = false;
        //StopAllCoroutines();
        if (mGuideBtn != null)
        {
            mGuideBtn.onClick = mBtnClickEvent;
        }

        if(mUiTrans!=null)
        {
            if (mGuideBtn == null)
            {
                Destroy(mUiTrans.GetComponent<Button>());
                Graphic graphic = mUiTrans.GetComponent<Graphic>();
                if (graphic != null) graphic.raycastTarget = mLastUiRaycastTarget;
            }

            SetGuideCanvas(mUiTrans.gameObject, false, GuideStep.MaskStatus.Default);
        }
        mViewObj.BtnMask.SetOnClick(null);

        System.Action callback = mCallback;
        System.Action outFinishCallback = mOutFinishCallback;
        Button.ButtonClickedEvent btnClickEvent = mBtnClickEvent;
        mGuideBtn = null;
        mCallback = null;
        mUiTrans = null;
        mBtnClickEvent = null;
        mOutFinishCallback = null;
        CloseWindow();
        if (outFinishCallback != null) outFinishCallback();
        if (guideFinish)//只有引导完成，才执行
        {
            if (callback != null) callback();
            //if (btnClickEvent != null) btnClickEvent.Invoke();
        }

        GuideHandler(GuideAction.GuideClose, CurGuideStep);
    }

    /// <summary>
    /// 延迟一帧显示新手引导，以免有些UI未加载完成
    /// </summary>
    void YieldStartGuide(Transform uiTrans, Button guideBtn, float yieldTime , System.Action callback)
    {
        mViewObj.Root.gameObject.SetActive(false);
        IsGuiding = true;
        UIRootMgr.Instance.TopMasking = true;
        StopAllCoroutines();
        StartCoroutine(WaitStartGuideCor(uiTrans, guideBtn,yieldTime, callback));
    }
    public IEnumerator WaitStartGuideCor(Transform uiTrans, Button guideBtn, float yieldTime, System.Action callback)
    {
        if (yieldTime <= 0)
            yield return null;
        else
            yield return new WaitForSeconds(yieldTime);
        mViewObj.Root.gameObject.SetActive(true);
        UIRootMgr.Instance.TopMasking = false;
        StartGuide(uiTrans, guideBtn, callback);
    }

    private void StartGuide(Transform uiTrans , Button guideBtn , System.Action callback=null)
    {
        if (uiTrans == null) return;
        GuideStep curGuideStep = GuideStep.GuideStepFetcher.GetGuideStepByCopy(CurGuideStep);
        if (curGuideStep == null) return;
        TDebug.Log(string.Format("开启引导:{0}|{1}|{2}", curGuideStep.Step, curGuideStep.MyTextStatus, curGuideStep.MyMaskStatus));
        IsGuiding = true;
        SetEnable(true);
        mUiTrans = uiTrans;
        SetGuideCanvas(uiTrans.gameObject, true, curGuideStep.MyMaskStatus);
        mGuideBtn = guideBtn;
        mCallback = callback;
        if (mGuideBtn != null)
        {
            mBtnClickEvent = mGuideBtn.onClick; //存储按钮之前的点击信息
            if (curGuideStep.MyMaskStatus != GuideStep.MaskStatus.MaskButFinishByOut)
            {
                mGuideBtn.onClick = null;
                mGuideBtn.onClick = new Button.ButtonClickedEvent();
                mGuideBtn.SetOnClick(BtnEvt_ClickGuideBtn); //给按钮添加事件，使之被点击时完成引导
            }
        }
        else if(curGuideStep.MyMaskStatus != GuideStep.MaskStatus.ClickMaskCanFinish
            && curGuideStep.MyMaskStatus!= GuideStep.MaskStatus.MaskButFinishByOut
            && curGuideStep.MyMaskStatus != GuideStep.MaskStatus.NoMaskFinishByOut
            && curGuideStep.MyMaskStatus != GuideStep.MaskStatus.NoMask) //如果引导按钮为null，则给uiTrans添加按钮
        {
            Button tempBtn = uiTrans.gameObject.CheckAddComponent<Button>();
            tempBtn.transition = Selectable.Transition.None;
            Graphic graphic = uiTrans.GetComponent<Graphic>();
            if (graphic != null)
            {
                mLastUiRaycastTarget = graphic.raycastTarget;
                uiTrans.GetComponent<Graphic>().raycastTarget = true;
            }
            else
            {
                uiTrans.gameObject.CheckAddComponent<Image>().color = new Color(0, 0, 0, 1f/255f);
                TDebug.LogError("当GuideBtn传null时，需要自动给uiTrans添加Button，需保证uiTrans上有Graphic组件");
            }
            tempBtn.SetOnClick(BtnEvt_ClickGuideBtn);
        }

        switch (curGuideStep.MyMaskStatus) //根据遮罩类型，显示
        {
            case GuideStep.MaskStatus.Default:
                mViewObj.BtnMask.gameObject.SetActive(true);
                mViewObj.BtnMask.image.color = new Color(0.8f, 0.8f, 0.8f, 0.7f);
                break;
            case GuideStep.MaskStatus.TransparentMask:
                mViewObj.BtnMask.gameObject.SetActive(true);
                mViewObj.BtnMask.image.color = new Color(0.8f, 0.8f, 0.8f, 0.01f);
                break;
            case GuideStep.MaskStatus.ClickMaskCanFinish:
                mViewObj.BtnMask.gameObject.SetActive(true);
                mViewObj.BtnMask.image.color = new Color(0.8f, 0.8f, 0.8f, 0.7f);
                mViewObj.BtnMask.SetOnClick(BtnEvt_ClickGuideBtn);
                break;
            case GuideStep.MaskStatus.NoMask:
                mViewObj.BtnMask.gameObject.SetActive(false);
                break;
            case GuideStep.MaskStatus.MaskButFinishByOut:
                mViewObj.BtnMask.gameObject.SetActive(true);
                mViewObj.BtnMask.image.color = new Color(0.8f, 0.8f, 0.8f, 0.7f);
                break;
            case GuideStep.MaskStatus.NoMaskFinishByOut:
                mViewObj.BtnMask.gameObject.SetActive(false);
                break;
        }

        mViewObj.TextGuide.text = curGuideStep.Desc;

        SetArrowAndTextByPos(uiTrans, curGuideStep.MyTextStatus);
        
    }

    private void SetArrowAndTextByPos(Transform uiTrans, GuideStep.TextStatus textStatus)
    {
        if (textStatus == GuideStep.TextStatus.Close) //文本是否显示
        {
            mViewObj.TextRoot.gameObject.SetActive(false);
        }
        else
        {
            mViewObj.TextRoot.gameObject.SetActive(true);
        }

        //显示箭头提示，判断设置箭头的位置
        Vector3 uiPos = UIRootMgr.Instance.MyCanvas.transform.InverseTransformPoint(uiTrans.position);
        mViewObj.TransArrowHigh.gameObject.SetActive(false);
        mViewObj.TransArrowLow.gameObject.SetActive(false);
        
        //显示引导内容文本
        if ((textStatus ==GuideStep.TextStatus.ForceLow ||textStatus== GuideStep.TextStatus.ForceLowAndEdge)
            ||(textStatus ==GuideStep.TextStatus.Default && uiPos.y > UIRootMgr.Instance.MyCanvasScaler.referenceResolution.y / 2 - 400))//待引导的区域在顶部
        {
            mViewObj.TransArrowLow.transform.position = uiTrans.position;
            mViewObj.TransArrowLow.transform.localPosition -= Vector3.up * uiTrans.GetComponent<RectTransform>().sizeDelta.y / 2;
            mViewObj.TransArrowLow.gameObject.SetActive(true);
            mViewObj.TextRoot.SetParent(mViewObj.LowRoot.transform);
            mViewObj.TextRoot.localPosition = Vector3.zero;
            SetInScreen(mViewObj.TextRoot.GetComponent<RectTransform>(), textStatus);
            //mViewObj.RootArrowImageLow.position = new Vector3(uiTrans.position.x, mViewObj.RootArrowImageLow.position.y, mViewObj.RootArrowImageLow.position.z);

        }
        else
        {
            mViewObj.TransArrowHigh.transform.position = uiTrans.position;
            mViewObj.TransArrowHigh.transform.localPosition += Vector3.up * uiTrans.GetComponent<RectTransform>().sizeDelta.y / 2;
            mViewObj.TransArrowHigh.gameObject.SetActive(true);
            mViewObj.TextRoot.SetParent(mViewObj.HighRoot.transform);
            mViewObj.TextRoot.localPosition = Vector3.zero;
            SetInScreen(mViewObj.TextRoot.GetComponent<RectTransform>(), textStatus);
            //mViewObj.RootArrowImageHigh.position = new Vector3(uiTrans.position.x, mViewObj.RootArrowImageHigh.position.y, mViewObj.RootArrowImageHigh.position.z);

        }
    }

    //自动调整在屏幕中的位置
    private static void SetInScreen(RectTransform textTrans ,GuideStep.TextStatus textStatus)
    {
        Vector3 textPos = UIRootMgr.Instance.MyCanvas.transform.InverseTransformPoint(textTrans.position);
        Vector2 uiSize = textTrans.sizeDelta;
        //调整x轴
        if (Mathf.Abs(textPos.x) + textTrans.sizeDelta.x/2 > UIRootMgr.Instance.MyCanvasScaler.referenceResolution.x/2)
        {
            float x = Mathf.Abs(textPos.x) + textTrans.sizeDelta.x / 2 - UIRootMgr.Instance.MyCanvasScaler.referenceResolution.x / 2;
            textTrans.localPosition += new Vector3(textPos.x > 0 ? -x : x, 0, 0);
        }
        ////调整y轴
        //if (Mathf.Abs(textPos.y) + textTrans.sizeDelta.y / 2 > UIRootMgr.Instance.m_CanvasScaler.referenceResolution.y / 2)
        //{
        //    //float y = Mathf.Abs(uiPos.y) + textTrans.sizeDelta.y / 2 - UIRootMgr.Instance.m_CanvasScaler.referenceResolution.y / 2;
        //    float y = Mathf.Abs(textPos.y) - UIRootMgr.Instance.m_CanvasScaler.referenceResolution.y / 2;
        //    textTrans.localPosition += new Vector3(textPos.y > 0 ? -y : y, 0, 0);
        //}
    }

    public static void FinishGuideByOut(int guideStep , System.Action outFinishCallback)
    {
        if (!IsGuiding|| Window_Guide.Instance==null)
        {
            if (outFinishCallback != null) outFinishCallback();
            TDebug.Log(string.Format("当前未开启引导:{0}",  guideStep));
            return;
        }
        if (IsStepFinished(guideStep))
        {
            if (outFinishCallback != null) outFinishCallback();
            TDebug.Log(string.Format("当前不是此引导:{0}|{1}", Window_Guide.Instance.CurGuideStep, guideStep));
            return;
        }
        if (Window_Guide.Instance != null)
        {
            Window_Guide.Instance.mOutFinishCallback = outFinishCallback;
            Window_Guide.Instance.BtnEvt_ClickGuideBtn();
        }
        
    }
    void BtnEvt_ClickGuideBtn()
    {
        UIRootMgr.Instance.IsLoading = true;
        //RegisterNetCodeHandler(NetCode_S.SaveGuideStep, S2C_SaveGuideStep);
        //GameClient.Instance.SendMessage( MessageBridge.Instance.C2S_SaveGuideStep(CurGuideStep));
    }

    //发送给服务器，已完成某引导
    public void JustFinishGuideStep(int guideStep)
    {
        if (IsStepFinished(guideStep))
        {
            TDebug.LogError("强制完成引导失败,已经完成了" + guideStep);
            return;
        }
        UIRootMgr.Instance.IsLoading = true;
        //RegisterNetCodeHandler(NetCode_S.SaveGuideStep, S2C_SaveGuideStep);
        //GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_SaveGuideStep(guideStep));
    }
    public void S2C_SaveGuideStep(BinaryReader ios) //服务器保存好了引导打点
    {
        UIRootMgr.Instance.IsLoading = false;
        //NetPacket.S2C_SaveGuideStep msg = MessageBridge.Instance.S2C_SaveGuideStep(ios);
        //RegisterNetCodeHandler(NetCode_S.SaveGuideStep, null);
        if(IsGuiding)
            CloseGuide(true);
    }



    private void SetGuideCanvas(GameObject gameObj , bool isGuide,GuideStep.MaskStatus maskStatus)
    {
        if (isGuide)
        {
            if (maskStatus == GuideStep.MaskStatus.NoMask || maskStatus== GuideStep.MaskStatus.NoMaskFinishByOut) return;
            Canvas canvas = gameObj.CheckAddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = GuideOrderLayer+1;
            if (maskStatus!= GuideStep.MaskStatus.ClickMaskCanFinish)
            {
                GraphicRaycaster ray = gameObj.CheckAddComponent<GraphicRaycaster>();
            }
        }
        else
        {
            Destroy(gameObj.GetComponent<GraphicRaycaster>());
            Destroy(gameObj.GetComponent<Canvas>());
        }
    }

    
    /// <summary>
    /// 检查并打开新手引导。
    /// 如果guideBtn传空，则会尝试在uiTrans上添加Button组件，此时应保证uiTrans上有Image/Text组件
    /// </summary>
    public static bool CheckGuide(int guideStep, Transform uiTrans, Button guideBtn, System.Action callback , float yieldTime=0)
    {
        if (IsStepFinished(guideStep))
        {
            TDebug.Log(string.Format("没到或已经完成此引导:{0}", guideStep));
            return false;
        }
        if (IsGuiding)
        {
            TDebug.Log(string.Format("正在引导:{0}| {1}", Window_Guide.Instance.CurGuideStep, guideStep));
            return false;
        }
        TDebug.Log(string.Format("准备开始引导:{0}", guideStep));
        UIRootMgr.Instance.TopMasking = true;
        Window_Guide win = UIRootMgr.Instance.OpenWindow<Window_Guide>(WinName.Window_Guide, CloseUIEvent.None);
        win.CurGuideStep = guideStep;
        win.OpenWindow();
        win.YieldStartGuide(uiTrans, guideBtn, yieldTime,callback);
        return true;
    }

    public static bool IsGuideStepCanDoSequence(int step)
    {
        if (step>1 && !IsStepFinished(step - 1))//判断上一个引导是否完成
            return false;
        return !IsStepFinished(step);
    }

    public static bool IsStepFinished(int step)
    {
        long finish = ((long)1)<<step;
        if (step < 0) return false;
        return (PlayerPrefsBridge.Instance.PlayerData.FinishGuideStep & finish) == finish;
    }



    //之后将所有引导时进行的处理，集中放在这里
    public static void GuideHandler(GuideAction guideAction,object arg)
    {
        switch (guideAction)
        {
            case GuideAction.GuideClose://引导关闭时
            {
                int finishStep = (int)arg;
                if ((finishStep == GuideStepNum._3.ToInt()) || (finishStep == GuideStepNum._4.ToInt()))
                {
                    Window_DungeonMap win = UIRootMgr.Instance.GetOpenListWindow(WinName.Window_DungeonMap) as Window_DungeonMap;
                    if (win != null) win.FreshNpc();
                }
                break;
            }
            case GuideAction.DungeonMapSetMsg://秘境显示消息时
            {
                int msgId = (int) arg;
                if (Window_Guide.IsGuideStepCanDoSequence(GuideStepNum._3.ToInt()) && GuideStep.TryGetParam(GuideStepNum._3.ToInt(), 0) == msgId)//&& npcId == GuideStep.TryGetParam(2,0))
                {
                    //Window_Guide.CheckGuide(GuideStepNum._3.ToInt(), LobbySceneMainUIMgr.Instance.GetTextScroll(), null, delegate(){});
                }
                else if (Window_Guide.IsGuideStepCanDoSequence(GuideStepNum._5.ToInt()) && GuideStep.TryGetParam(GuideStepNum._5.ToInt(), 0) == msgId)
                {
                    Window_DungeonMap dungeonMap = UIRootMgr.Instance.GetOpenListWindow(WinName.Window_DungeonMap)as Window_DungeonMap;
                    if (dungeonMap == null)
                    {
                        TDebug.LogError("引导中查找Window_DungeonMap时为空");
                        return;
                    }
                    //Window_Guide.CheckGuide(GuideStepNum._5.ToInt(), dungeonMap.mNodeItemList[11].View.transform, null, delegate() { });
                }
                break;
            }
        }
    }
}

public enum GuideAction
{
    GuideClose,
    DungeonMapSetMsg,

}
