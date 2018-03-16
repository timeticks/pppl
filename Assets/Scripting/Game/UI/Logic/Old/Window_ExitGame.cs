using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Window_ExitGame : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_MapEndFinishMapEvent;
        public Text TextTitle;
        public Button BtnExitGame;
        public Text TextBtnExitGame;
        public Button MaskBtn;
        public Button BtnLoginOut;
        public Text TextBtnLoginOut;
        public ViewObj(UIViewBase view)
        {
            if (Part_MapEndFinishMapEvent == null) Part_MapEndFinishMapEvent = view.GetCommon<GameObject>("Part_MapEndFinishMapEvent");
            if (TextTitle == null) TextTitle = view.GetCommon<Text>("TextTitle");
            if (BtnExitGame == null) BtnExitGame = view.GetCommon<Button>("BtnExitGame");
            if (TextBtnExitGame == null) TextBtnExitGame = view.GetCommon<Text>("TextBtnExitGame");
            if (MaskBtn == null) MaskBtn = view.GetCommon<Button>("MaskBtn");
            if (BtnLoginOut == null) BtnLoginOut = view.GetCommon<Button>("BtnLoginOut");
            if (TextBtnLoginOut == null) TextBtnLoginOut = view.GetCommon<Text>("TextBtnLoginOut");        }
    }

    private ViewObj mViewObj;

    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Init();
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    private void Init()
    {
        mViewObj.TextBtnExitGame.text = LangMgr.GetText("退出游戏");
        mViewObj.BtnExitGame.SetOnClick(BtnEvt_ExitGame);
        mViewObj.TextBtnLoginOut.text = LangMgr.GetText("重新登录");
        mViewObj.BtnLoginOut.SetOnClick(BtnEvt_LoginOut);

        mViewObj.MaskBtn.SetOnClick(delegate() { CloseWindow(); });
    }

    public void BtnEvt_ExitGame()
    {
        Application.Quit();
    }
    public void BtnEvt_LoginOut()
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.LoginOutGame();
    }

}