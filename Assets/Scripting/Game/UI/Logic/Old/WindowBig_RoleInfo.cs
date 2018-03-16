using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WindowBig_RoleInfo : WindowBase
{
    public enum ChildTab:byte
    {
        DetailInfo =0,
        Assem,
        Reputation,
        DungeonInventory,  //任务道具
    }
    internal ChildTab? m_CurTab;
    public class ViewObj
    {
        public Button ButtonExit;
        public Button ButtonDetialInfo;
        public Button ButtonAssem;
        public Button ButtonReputation;
        public Text TextBtnRoleInfo;
        public Text TextBtnAssem;
        public Text TextBtnRePutation;
        public List<Text> TextBtnList;
        public Button ButtonInventory;
        public Text TextBtnInventory;
        public List<Button> BtnTabList;
        public ViewObj(UIViewBase view)
        {
            if (ButtonExit == null) ButtonExit = view.GetCommon<Button>("ButtonExit");
            if (ButtonDetialInfo == null) ButtonDetialInfo = view.GetCommon<Button>("ButtonDetialInfo");
            if (ButtonAssem == null) ButtonAssem = view.GetCommon<Button>("ButtonAssem");
            if (ButtonReputation == null) ButtonReputation = view.GetCommon<Button>("ButtonReputation");
            if (TextBtnRoleInfo == null) TextBtnRoleInfo = view.GetCommon<Text>("TextBtnRoleInfo");
            if (TextBtnAssem == null) TextBtnAssem = view.GetCommon<Text>("TextBtnAssem");
            if (TextBtnRePutation == null) TextBtnRePutation = view.GetCommon<Text>("TextBtnRePutation");
            if (ButtonInventory == null) ButtonInventory = view.GetCommon<Button>("ButtonInventory");
            if (TextBtnInventory == null) TextBtnInventory = view.GetCommon<Text>("TextBtnInventory");            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtnRoleInfo);
                TextBtnList.Add(TextBtnAssem);
                TextBtnList.Add(TextBtnRePutation);
                TextBtnList.Add(TextBtnInventory);
            }
            if (BtnTabList == null)
            {
                BtnTabList = new List<Button>();
                BtnTabList.Add(ButtonDetialInfo);
                BtnTabList.Add(ButtonAssem);
                BtnTabList.Add(ButtonReputation);
                BtnTabList.Add(ButtonInventory);
            }
        }
        public void SelectTabBtn(ChildTab child)
        {
            if (TextBtnList == null)
                return;
            for (int i = 0,length=TextBtnList.Count; i < length; i++)
            {
                if ((int)child == i)
                {
                    TextBtnList[i].color = new Color(255f / 255, 242f / 255, 0f);
                    TextBtnList[i].GetComponent<Outline>().enabled = true;
                    BtnTabList[i].enabled = false;
                }
                else
                {
                    TextBtnList[i].color = Color.black;
                    TextBtnList[i].GetComponent<Outline>().enabled = false;
                    BtnTabList[i].enabled = true;
                }             
            }
        }
    }
    private ViewObj mViewObj;
    public void OpenWindow(ChildTab childTab = ChildTab.Assem ,bool ShowQuestItems = false)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        //按钮监听设置
        mViewObj.ButtonExit.SetOnClick(delegate() { BtnEvt_Exit(); });
        mViewObj.ButtonDetialInfo.SetOnClick(delegate() { OpenChildWindow(ChildTab.DetailInfo); });
        mViewObj.ButtonAssem.SetOnClick(delegate() { OpenChildWindow(ChildTab.Assem); });
        mViewObj.ButtonInventory.gameObject.SetActive(ShowQuestItems);
        if (ShowQuestItems)
            mViewObj.ButtonInventory.SetOnClick(delegate() { OpenChildWindow(ChildTab.DungeonInventory); });
        //打开默认分页子窗口
        m_CurTab = null;
        OpenChildWindow(childTab);
    }
    public void OpenChildWindow(ChildTab toTab)
    {
        if (m_CurTab != null && m_CurTab == toTab) { return; }
        mViewObj.SelectTabBtn(toTab);
        switch (toTab)
        {
            case ChildTab.DetailInfo:
                OpenChildTabWindow<Window_RoleDetailInfo>(WinName.Window_RoleDetailInfo, CloseUIEvent.None).OpenWindow();
                break;
            case ChildTab.Assem:
                OpenChildTabWindow<Window_AssemInfo>(WinName.Window_AssemInfo, CloseUIEvent.None).OpenWindow();
                break;
            case ChildTab.DungeonInventory:
                OpenChildTabWindow<Window_ItemInventory>(WinName.Window_ItemInventory, CloseUIEvent.None).OpenWindow();
                break;
            default:
                break;
        }
        m_CurTab = toTab;
    }

    //public void OpenChildWindow(ChildTab toTab)
    //{
    //    if (m_CurTab!=null && m_CurTab == toTab) { return; }
    //    CloseCurTabWindow();
    //    mViewObj.SelectTabBtn(toTab);
    //    switch (toTab)
    //    {
    //        case ChildTab.DetailInfo:

    //            UIRootMgr.Instance.OpenWindow<Window_RoleDetailInfo>(WinName.Window_RoleDetailInfo, CloseUIEvent.None).OpenWindow();
    //            break;
    //        case ChildTab.Assem:
    //            UIRootMgr.Instance.OpenWindow<Window_AssemInfo>(WinName.Window_AssemInfo, CloseUIEvent.None).OpenWindow();
    //            break;
    //        case ChildTab.DungeonInventory:
    //            if (mIsInDungeon)
    //                UIRootMgr.Instance.OpenWindow<Window_ItemInventory>(WinName.Window_ItemInventory, CloseUIEvent.None).OpenWindow(mIsInDungeon);
    //            break;
    //        default:
    //            break;
    //    }
    //    m_CurTab = toTab;
    //}



    public void BtnEvt_Exit()
    {
        //
        //打开Hide
        CloseWindow(CloseActionType.OpenHide);
    }

    void CloseCurTabWindow()
    {
        if (m_CurTab == ChildTab.DetailInfo) UIRootMgr.Instance.CloseWindow_InOpenList(WinName.Window_RoleDetailInfo);
        if (m_CurTab == ChildTab.Assem) UIRootMgr.Instance.CloseWindow_InOpenList(WinName.Window_AssemInfo);
        if (m_CurTab == ChildTab.DungeonInventory) UIRootMgr.Instance.CloseWindow_InOpenList(WinName.Window_ItemInventory);
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
     //   OpenHideWindow();
       // CloseCurTabWindow();
        //if (mIsInDungeon)
        //{
        //    UIRootMgr.Instance.OpenWindow<Window_DungeonMap>(WinName.Window_DungeonMap , CloseUIEvent.CloseAll).OpenWindow(0);
        //}
    }
}
