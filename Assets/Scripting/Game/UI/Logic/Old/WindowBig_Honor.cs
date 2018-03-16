using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class WindowBig_Honor : WindowBase {

  public enum ChildTab
    {
        Ranking0 = 0,//地榜
        Ranking1 = 1,//天榜
        Achieve = 2,//成就
        Max = 3
    }

    public class ViewObj
    {
        public Button BtnExit;
        public Button BtnRanking0;
        public Button BtnRanking1;
        public Button BtnAchieve;
        public Text TextBtnRank0;
        public Text TextBtnRank1;
        public Text TextBtnAchieve;
        public List<Text> TextBtnList;
        public List<Button> BtnTab; 
        public ViewObj(UIViewBase view)
        {
            if (BtnExit == null) BtnExit = view.GetCommon<Button>("BtnExit");
            if (BtnRanking0 == null) BtnRanking0 = view.GetCommon<Button>("BtnRanking0");
            if (BtnRanking1 == null) BtnRanking1 = view.GetCommon<Button>("BtnRanking1");
            if (BtnAchieve == null) BtnAchieve = view.GetCommon<Button>("BtnAchieve");
            if (TextBtnRank0 == null) TextBtnRank0 = view.GetCommon<Text>("TextBtnRank0");
            if (TextBtnRank1 == null) TextBtnRank1 = view.GetCommon<Text>("TextBtnRank1");
            if (TextBtnAchieve == null) TextBtnAchieve = view.GetCommon<Text>("TextBtnAchieve");
            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtnRank0);
                TextBtnList.Add(TextBtnRank1);
                TextBtnList.Add(TextBtnAchieve);
            }
            if (BtnTab == null)
            {
                BtnTab = new List<Button>();
                BtnTab.Add(BtnRanking0);
                BtnTab.Add(BtnRanking1);
                BtnTab.Add(BtnAchieve);
            }
        }
        public void SelectTabBtn(ChildTab child)
        {
            if (TextBtnList == null)
                return;
            for (int i = 0, length = TextBtnList.Count; i < length; i++)
            {
                if ((int)child == i)
                {
                    TextBtnList[i].color = new Color(255f / 255, 242f / 255, 0f);
                    TextBtnList[i].GetComponent<Outline>().enabled = true;
                    BtnTab[i].enabled = false;
                }
                else
                {
                    TextBtnList[i].color = Color.black;
                    TextBtnList[i].GetComponent<Outline>().enabled = false;
                    BtnTab[i].enabled = true;
                }
            }
        }
    }
    private ViewObj mViewObj;
    internal ChildTab? m_CurTab=null; //当前页面
    public void OpenWindow(ChildTab child = ChildTab.Achieve,Window_Ranking.RankEnum rank = Window_Ranking.RankEnum.Level,bool refresh =true)
    {
        if (child == ChildTab.Achieve)
        {
            gameObject.SetActive(false);
            if (!AchievementAccessor.IsInitAccessor)
            {
                UIRootMgr.Instance.IsLoading = true;
                RegisterNetCodeHandler(NetCode_S.PullInfo, S2C_PullInfo);
                GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.Achievement, 0));
                return;
            }
        }
      OpenHonor(child,rank,refresh);
    }
    public void S2C_PullInfo(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_PullInfo msg = MessageBridge.Instance.S2C_PullInfo(ios);
        OpenHonor(ChildTab.Achieve, Window_Ranking.RankEnum.Level,true);
    }

    public void OpenHonor(ChildTab child, Window_Ranking.RankEnum rank, bool refresh)
    {
        gameObject.SetActive(true);
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        mViewObj.BtnExit.SetOnClick(delegate() { BtnEvt_Exit(); });
        mViewObj.BtnRanking0.SetOnClick(delegate() { OpenChildWindow(ChildTab.Ranking0, rank, refresh); });
        mViewObj.BtnRanking1.SetOnClick(delegate() { OpenChildWindow(ChildTab.Ranking1, rank, refresh); });
        mViewObj.BtnAchieve.SetOnClick(delegate() { OpenChildWindow(ChildTab.Achieve, rank, refresh); });
        m_CurTab = null;
        OpenChildWindow(child, rank, refresh);
    }

    public void OpenChildWindow(ChildTab toTab ,Window_Ranking.RankEnum rank,bool refresh)
    {
        if (m_CurTab != null && m_CurTab == toTab) { return; }
        m_CurTab = toTab;
        mViewObj.SelectTabBtn(toTab);
        LobbySceneMainUIMgr.Instance.SetDownRoot(toTab==ChildTab.Achieve);
        switch (toTab)
        {
            case ChildTab.Ranking0:
                OpenChildTabWindow<Window_Ranking>(WinName.Window_Ranking, CloseUIEvent.None).OpenWindow(ChildTab.Ranking0, rank, refresh);
                break;
            case ChildTab.Ranking1:
                OpenChildTabWindow<Window_Ranking>(WinName.Window_Ranking, CloseUIEvent.None).OpenWindow(ChildTab.Ranking1, rank, refresh);
                break;
            case ChildTab.Achieve:
                OpenChildTabWindow<Window_Achieve>(WinName.Window_Achieve, CloseUIEvent.None).OpenWindow();
                break;
            default:
                break;
        }
    }

    public void BtnEvt_Exit()
    {
        base.CloseWindow();
        LobbySceneMainUIMgr.Instance.SetDownRoot(true);
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.NotCloseChild)
    {
        base.CloseWindow(CloseActionType.NotCloseChild);
    }
   
}
