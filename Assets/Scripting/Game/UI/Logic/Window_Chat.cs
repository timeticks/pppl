using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Window_Chat : WindowBase
{
    public class ViewObj
    {
        public Text ContinueTipsText;
        public Text DescText;
        public Button MaskBtn;
        public Transform ItemRoot;
        public GameObject Part_ChatBtnItem;
        public ViewObj(UIViewBase view)
        {
            if (ContinueTipsText != null) return;
            ContinueTipsText = view.GetCommon<Text>("ContinueTipsText");
            DescText = view.GetCommon<Text>("DescText");
            MaskBtn = view.GetCommon<Button>("MaskBtn");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            Part_ChatBtnItem = view.GetCommon<GameObject>("Part_ChatBtnItem");
        }
    }
    public class SelectSmallObj : SmallViewObj
    {
        public TextButton SelectBtn;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            SelectBtn = view.GetCommon<TextButton>("SelectBtn");
        }
    }
    private List<SelectSmallObj> mButtonItemList = new List<SelectSmallObj>();

    private ViewObj mViewObj;
    private List<int> mCurShowList;
    private int mCurShowIndex;
    private string mCurShowText;
    private System.Action<int> mResultDel;  //列表中最后一个对话的选项
    private bool mCanCloseWindow = false;

    public void OpenWindow(List<int> showDialogList , System.Action<int> resultDel)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        mCurShowList = showDialogList;
        mResultDel = resultDel;
        Init();
    }

    void Init()
    {
        mCanCloseWindow = false;
        mViewObj.MaskBtn.SetOnClick(BtnEvt_Continue);
        mViewObj.ContinueTipsText.text = LangMgr.GetText("点击任意处继续");
        mCurShowIndex = 0;
        ShowNext(0,mCurShowIndex);
    }

    void ShowNext(int selectIdex , int showIndex)
    {
        mCurShowIndex = showIndex;
        Reset();
        if (mCurShowIndex < mCurShowList.Count)
        {
            mViewObj.DescText.text = "";
            mCurShowText = SelectDialog.GetDesc(mCurShowList[showIndex]);
            mViewObj.DescText.DOText(mCurShowText, mCurShowText.Length / 8f).SetEase(Ease.Linear).OnComplete(delegate() { ShowComplete(); });
        }
        else
        {
            CloseWin(selectIdex);
        }
    }

    void Reset()
    {
        mViewObj.ContinueTipsText.gameObject.SetActive(false);
        mViewObj.ItemRoot.gameObject.SetActive(false);
    }

    void ShowComplete()
    {
        mViewObj.DescText.text = mCurShowText;

        string[] buttons = SelectDialog.GetButtons(mCurShowList[mCurShowIndex]);
        //如果有选项，显示按钮
        if (buttons.Length > 0)
        {
            mViewObj.ItemRoot.gameObject.SetActive(true);
            mButtonItemList = TAppUtility.Instance.AddViewInstantiate(mButtonItemList, mViewObj.Part_ChatBtnItem, mViewObj.ItemRoot,
                buttons.Length);
            for (int i = 0; i < buttons.Length; i++)    
            {
                mButtonItemList[i].SelectBtn.TextBtn.text = buttons[i];
                int selectIndex = i;
                mButtonItemList[i].SelectBtn.SetOnAduioClick(delegate() { BtnEvt_Select(selectIndex); });
            }
        }
        else
        {
            mViewObj.ContinueTipsText.gameObject.SetActive(true);
        }
    }

    void BtnEvt_Select(int selectIndex)
    {
        TDebug.LogInEditorF("选择：{0}", selectIndex);
        mCurShowIndex++;
        Reset();
        mViewObj.DescText.DOText("", 0.5f).OnComplete(delegate() { ShowNext(selectIndex,mCurShowIndex); });
    }

    //点击继续
    void BtnEvt_Continue()
    {
        if (!mViewObj.ContinueTipsText.gameObject.activeSelf)
            return;
        mCurShowIndex++;
        Reset();
        mViewObj.DescText.DOText("", 0.5f).OnComplete(delegate() { ShowNext(0,mCurShowIndex); });
    }

    void CloseWin(int btnSelectIndex)
    {
        mCanCloseWindow = true;
        if (mResultDel != null) mResultDel(btnSelectIndex);
        if (mCanCloseWindow)    //如果回调后，mCanCloseWindow被重置，则不关闭窗口
            CloseWindow();
    }
}
