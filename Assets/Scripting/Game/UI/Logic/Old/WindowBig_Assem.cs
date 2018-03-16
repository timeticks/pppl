using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WindowBig_Assem : WindowBase
{
    public class ViewObj
    {
        public GameObject Root;
        public Button ButtonExit;
        public Button ButtonItem;
        public Button ButtonSpell;
        public Button ButtonPet;
        public Button ButtonEquip;
        public Text TextBtnItem;
        public Text TextBtnSpell;
        public Text TextBtnEquip;
        public Text TextBtnPet;
        public List<Text> TextBtnList;
        public List<Button> BtnTab;
        public ViewObj(UIViewBase view)
        {
            if (Root == null) Root = view.GetCommon<GameObject>("Root");
            if (ButtonExit == null) ButtonExit = view.GetCommon<Button>("ButtonExit");
            if (ButtonItem == null) ButtonItem = view.GetCommon<Button>("ButtonItem");
            if (ButtonSpell == null) ButtonSpell = view.GetCommon<Button>("ButtonSpell");
            if (ButtonPet == null) ButtonPet = view.GetCommon<Button>("ButtonPet");
            if (ButtonEquip == null) ButtonEquip = view.GetCommon<Button>("ButtonEquip");
            if (TextBtnItem == null) TextBtnItem = view.GetCommon<Text>("TextBtnItem");
            if (TextBtnSpell == null) TextBtnSpell = view.GetCommon<Text>("TextBtnSpell");
            if (TextBtnEquip == null) TextBtnEquip = view.GetCommon<Text>("TextBtnEquip");
            if (TextBtnPet == null) TextBtnPet = view.GetCommon<Text>("TextBtnPet");
            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtnItem);
                TextBtnList.Add(TextBtnSpell);
                TextBtnList.Add(TextBtnEquip);
                TextBtnList.Add(TextBtnPet);            }
            if (BtnTab == null)
            {
                BtnTab = new List<Button>();
                BtnTab.Add(ButtonItem);
                BtnTab.Add(ButtonSpell);
                BtnTab.Add(ButtonEquip);
                BtnTab.Add(ButtonPet);            }        }
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
    public enum ChildTab : byte
    {
        Bag=0,
        Spell,
        Equip,
        Pet,
    }
    public ChildTab? CurTab;

    private bool mIsInMap;
    public void OpenWindow(ChildTab childTab = ChildTab.Bag , bool isInMap=false)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();

        //按钮监听设置
        mViewObj.ButtonExit.SetOnAduioClick(delegate() { BtnEvt_Exit(); }); 
        mViewObj.ButtonItem.SetOnAduioClick(delegate() { OpenChildWindow(ChildTab.Bag); });
        mViewObj.ButtonSpell.SetOnAduioClick(delegate() { OpenChildWindow(ChildTab.Spell); });
        mViewObj.ButtonPet.SetOnAduioClick(delegate() { OpenChildWindow(ChildTab.Pet); });
        mViewObj.ButtonEquip.SetOnAduioClick(delegate() { OpenChildWindow(ChildTab.Equip); });

        mIsInMap = isInMap;
        mViewObj.TextBtnItem.text = mIsInMap ? LangMgr.GetText("任务道具") : LangMgr.GetText("杂物");

        //打开默认分页子窗口
        CurTab = null;
        OpenChildWindow(childTab);
        FreshBadge();
    }

    void OpenChildWindow(ChildTab toTab)
    {
        if (CurTab != null && CurTab == toTab) { return; }
        mViewObj.SelectTabBtn(toTab);      
        switch (toTab)
        {
            case ChildTab.Bag:
                OpenChildTabWindow<Window_ItemInventory>(WinName.Window_ItemInventory, CloseUIEvent.None).OpenWindow();
                break;
            case ChildTab.Spell:
                OpenChildTabWindow<Window_AssemSpell>(WinName.Window_AssemSpell, CloseUIEvent.None).OpenWindow();
                break;
            case ChildTab.Pet:
                OpenChildTabWindow<Window_AssemPet>(WinName.Window_AssemPet, CloseUIEvent.None).OpenWindow();
                break;
            case ChildTab.Equip:
                OpenChildTabWindow<Window_AssemEquip>(WinName.Window_AssemEquip, CloseUIEvent.None).OpenWindow();
                break;
            default:
                break;
        }
        CurTab = toTab;
    }

    public override void FreshBadge()
    {
        base.FreshBadge();
        BadgeTips.SetBadgeViewFalse(mViewObj.ButtonItem.transform);

        //System.Action<object> finishEventDel = delegate(object o)
        //{
        //    BadgeTips.SetBadgeView(mViewObj.ButtonItem.transform);
        //};
        //AppEvtMgr.Instance.Register(new EvtItemData(EvtType.ChangePrefsKey, EvtListenerType.InventoryBadgeOpen.ToString()), EvtListenerType.InventoryBadgeOpen, finishEventDel);
    }


    public void BtnEvt_Exit()
    {
        CloseWindow();
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);       
    }
}
