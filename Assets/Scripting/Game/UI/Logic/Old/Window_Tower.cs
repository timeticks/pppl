using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_Tower : WindowBase
{

    public enum TabPanleType:sbyte
    {
        None=-1,
        Challenge,  //挑战面板
        Reward,     //奖励面板
        Rank,       //排行面板
        Max,   
    }
    public class ViewObj
    {
        public GameObject Part_ItemRanking;
        public Button BtnTab0;
        public Button BtnTab1;
        public Button BtnTab2;
        public Button BtnExit;
        public Text TextBtnTab0;
        public Text TextBtnTab1;
        public Text TextBtnTab2;
        public Panel_TowerRank Panel_TowerRank;
        public Panel_TowerChallenge Panel_TowerChallenge;
        public Panel_RewardLook Panel_RewardLook;

        public List<Text> TextBtnList;
        public ViewObj(UIViewBase view)
        {

            if (Part_ItemRanking == null) Part_ItemRanking = view.GetCommon<GameObject>("Part_ItemRanking");
            if (BtnTab0 == null) BtnTab0 = view.GetCommon<Button>("BtnTab0");
            if (BtnTab1 == null) BtnTab1 = view.GetCommon<Button>("BtnTab1");
            if (BtnTab2 == null) BtnTab2 = view.GetCommon<Button>("BtnTab2");
            if (BtnExit == null) BtnExit = view.GetCommon<Button>("BtnExit");
            if (TextBtnTab0 == null) TextBtnTab0 = view.GetCommon<Text>("TextBtnTab0");
            if (TextBtnTab1 == null) TextBtnTab1 = view.GetCommon<Text>("TextBtnTab1");
            if (TextBtnTab2 == null) TextBtnTab2 = view.GetCommon<Text>("TextBtnTab2");
            if (Panel_TowerRank == null) Panel_TowerRank = view.GetCommon<GameObject>("Panel_TowerRank").CheckAddComponent<Panel_TowerRank>();
            if (Panel_TowerChallenge == null) Panel_TowerChallenge = view.GetCommon<GameObject>("Panel_TowerChallenge").CheckAddComponent<Panel_TowerChallenge>();
            if (Panel_RewardLook == null) Panel_RewardLook = view.GetCommon<GameObject>("Panel_RewardLook").CheckAddComponent<Panel_RewardLook>();

            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtnTab0);
                TextBtnList.Add(TextBtnTab1);
                TextBtnList.Add(TextBtnTab2);
            }
        }
        public void SelectTabBtn(TabPanleType rank)
        {
            if (TextBtnList == null)
                return;
            for (int i = 0, length = TextBtnList.Count; i < length; i++)
            {
                if ((int)rank == i)
                {
                    TextBtnList[i].color = new Color(255f / 255, 242f / 255, 0f);
                    TextBtnList[i].GetComponent<Outline>().enabled = true;
                }
                else
                {
                    TextBtnList[i].color = Color.black;
                    TextBtnList[i].GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
    private ViewObj mViewObj;
    private TabPanleType mCurTab;
    public void OpenWindow(TabPanleType childTab= TabPanleType.Challenge,bool refresh = true)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init(childTab, refresh);
    }


    void SwitchTab(TabPanleType panelTy,bool refresh)
    {
        mViewObj.Panel_RewardLook.gameObject.SetActive(false);
        mViewObj.Panel_TowerChallenge.gameObject.SetActive(false);
        mViewObj.Panel_TowerRank.gameObject.SetActive(false);
        mViewObj.SelectTabBtn(panelTy);
        switch (panelTy)        //切换面板
        {
            case TabPanleType.Challenge:
            {
                mViewObj.Panel_TowerChallenge.Init(this);
                break;
            }
            case TabPanleType.Reward:
            {
                mViewObj.Panel_RewardLook.Init(this);
                break;
            }
            case TabPanleType.Rank:
            {
                mViewObj.Panel_TowerRank.Init(this, refresh);
                break;
            }
        }
        mCurTab = panelTy;
    }

    void Init(TabPanleType panelTy, bool refresh)
    {
        //mViewObj.TextBtnTab0.text = "挑战关卡";
        //mViewObj.TextBtnTab1.text = "奖励预览";
        //mViewObj.TextBtnTab2.text = "高手榜";

        mViewObj.BtnExit.SetOnClick(delegate() { BtnEvt_Exit(); });

        mViewObj.BtnTab0.SetOnClick(delegate() { BtnEvt_OpenTab(TabPanleType.Challenge); });
        mViewObj.BtnTab1.SetOnClick(delegate() { BtnEvt_OpenTab(TabPanleType.Reward); });
        mViewObj.BtnTab2.SetOnClick(delegate() { BtnEvt_OpenTab(TabPanleType.Rank); });
        BtnEvt_OpenTab(panelTy,refresh);
    }
    void BtnEvt_OpenTab(TabPanleType rank, bool refresh=true)
    {
        SwitchTab(rank, refresh);
    }

    public void BtnEvt_Exit()
    {
        //UIRootMgr.Instance.OpenWindow<WindowBig_Activity>(WinName.WindowBig_Activity, CloseUIEvent.None).OpenWindow();
        base.CloseWindow();
    }   
}
