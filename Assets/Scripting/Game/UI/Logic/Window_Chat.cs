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


    public void OpenWindow(List<int> showDialogList)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        mCurShowList = showDialogList;
        Init();
    }

    void Init()
    {
        mViewObj.MaskBtn.SetOnClick(BtnEvt_Continue);
        mViewObj.ContinueTipsText.text = LangMgr.GetText("点击任意处继续");
        mCurShowIndex = 0;
        ShowNext(mCurShowIndex);
    }

    void ShowNext(int showIndex)
    {
        mCurShowIndex = showIndex;
        Reset();
        if (mCurShowIndex < mCurShowList.Count)
        {
            TDebug.LogInEditorF("显示一句：{0}", mCurShowIndex);
            mViewObj.DescText.text = "";
            mCurShowText = SelectDialog.GetDesc(mCurShowList[showIndex]);
            mViewObj.DescText.DOText(mCurShowText, mCurShowText.Length / 8f).SetEase(Ease.Linear).OnComplete(delegate() { ShowComplete(); });
        }
        else
        {
            CloseWindow();
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
        mViewObj.DescText.DOText("", 0.5f).OnComplete(delegate() { ShowNext(mCurShowIndex); });
    }

    //点击继续
    void BtnEvt_Continue()
    {
        if (!mViewObj.ContinueTipsText.gameObject.activeSelf)
            return;
        mCurShowIndex++;
        Reset();
        mViewObj.DescText.DOText("", 0.5f).OnComplete(delegate() { ShowNext(mCurShowIndex); });
    }
}
