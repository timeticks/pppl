using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Window_MapEndShow : WindowBase
{
    public class ViewObj
    {
        public Text TitleText;
        public Text EndText;
        public TextButton TBtnOk;
        public ViewObj(UIViewBase view)
        {
            if (TitleText != null) return;
            TitleText = view.GetCommon<Text>("TitleText");
            EndText = view.GetCommon<Text>("EndText");
            TBtnOk = view.GetCommon<TextButton>("TBtnOk");
        }    }

    public class EventItemObj : SmallViewObj
    {
        public Text NameText;
        public Text StatusText;        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (StatusText == null) StatusText = view.GetCommon<Text>("StatusText");

        }
    }
    private ViewObj mViewObj;

    private List<EventItemObj> mEventItemList = new List<EventItemObj>();
    private System.Action mCloseCallback;
    private int mMapIdx;
    public void OpenWindow(int mapIdx)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        mMapIdx = mapIdx;
        Init();
    }

    void Init()
    {
        BallMap ballMap = BallMap.Fetcher.GetBallMapCopy(mMapIdx);
        if (ballMap == null) return;

        mViewObj.EndText.text = string.Format("分数：{0}\n获得物品：\n{1}",
            PlayerPrefsBridge.Instance.BallMapAcce.Score.ToString(),
            GoodsToDrop.getListString(PlayerPrefsBridge.Instance.BallMapAcce.goodsDropList)
            );

        mViewObj.TBtnOk.TextBtn.text = LangMgr.GetText("确定");
        mViewObj.TBtnOk.SetOnClick(BtnEvt_Ok);
    }



    public void BtnEvt_Ok()
    {
        if (Window_BallBattle.Instance != null) Window_BallBattle.Instance.CloseWin();
        CloseWindow();
    }



}