using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class WindowBig_Store : WindowBase
{
    public enum ChildTab:byte
    {
        Mall =0,
        Mark,
    }
    internal ChildTab? m_CurTab;
    public class ViewObj
    {
        public Button BtnExit;
        public Button BtnMall;
        public Button BtnMarket;
        public Button BtnCharge;
        public Button BtnAD;
        public Text TextBtnMall;
        public Text TextBtnMarket;
        public List<Text> TextBtnList;
        public List<Button> BtnTab;
        public ViewObj(UIViewBase view)
        {
            if (BtnExit == null) BtnExit = view.GetCommon<Button>("BtnExit");
            if (BtnMall == null) BtnMall = view.GetCommon<Button>("BtnMall");
            if (BtnMarket == null) BtnMarket = view.GetCommon<Button>("BtnMarket");
            if (BtnCharge == null) BtnCharge = view.GetCommon<Button>("BtnCharge");
            if (BtnAD == null) BtnAD = view.GetCommon<Button>("BtnAD");
            if (TextBtnMall == null) TextBtnMall = view.GetCommon<Text>("TextBtnMall");
            if (TextBtnMarket == null) TextBtnMarket = view.GetCommon<Text>("TextBtnMarket");
            if (TextBtnList == null)
            {
                TextBtnList = new List<Text>();
                TextBtnList.Add(TextBtnMall);
                TextBtnList.Add(TextBtnMarket);
            }
            if (BtnTab == null)
            {
                BtnTab = new List<Button>();
                BtnTab.Add(BtnMall);
                BtnTab.Add(BtnMarket);
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

    public void OpenWindow()
    {
        gameObject.SetActive(false);
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.ShopInfo, S2C_ShopInfo);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_ShopInfo(GameConstUtils.id_shopMall));   
           
    }
    void Init()
    {
        //按钮监听设置
        mViewObj.BtnAD.SetOnClick(delegate() { BtnEvt_OpenAD(); });
        mViewObj.BtnCharge.SetOnClick(delegate() { BtnEvt_Charge(); });
        mViewObj.BtnExit.SetOnClick(delegate() { BtnEvt_Exit(); });
        mViewObj.BtnMall.SetOnClick(delegate() { OpenChildWindow(ChildTab.Mall); });
        mViewObj.BtnMarket.SetOnClick(delegate() { OpenChildWindow(ChildTab.Mark); });

        //打开默认分页子窗口
        m_CurTab = null;
        string msg;
        mViewObj.BtnMall.gameObject.SetActive(Shop.GetShopOpen(GameConstUtils.id_shopMall, out msg));
        mViewObj.BtnMarket.gameObject.SetActive(Shop.GetShopOpen(GameConstUtils.id_shopMark, out msg));
        OpenChildWindow(ChildTab.Mall);
        //if (Shop.GetShopOpen(GameConstUtils.id_shopMall, out msg))
        //{
        //    OpenChildWindow(ChildTab.Mall);
        //}
        //else if (Shop.GetShopOpen(GameConstUtils.id_shopMark, out msg))
        //{
        //     OpenChildWindow(ChildTab.Mark);
        //}           
        //else
        //{
        //    CloseWindow();
        //    ErrorStatus error = ErrorStatus.ErrorStatusFetcher.GetErrorStatusByCopy(ServerStatusCode.GLOBAL_WARN_CODE_DENG_JI_YI_MAN);
        //    string errorStr = error == null ? "数据异常，请稍后重试" : error.Name;
        //    UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(errorStr, Color.black);
        //}
    }
    void S2C_ShopInfo(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.ShopInfo, null);
        NetPacket.S2C_ShopInfo msg = MessageBridge.Instance.S2C_ShopInfo(ios);
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init();
    }

    public void OpenChildWindow(ChildTab toTab)
    {
        if (m_CurTab!=null && m_CurTab == toTab) { return; }
     //   CloseCurTabWindow();
        mViewObj.SelectTabBtn(toTab);
        switch (toTab)
        {
            case ChildTab.Mall:
                UIRootMgr.Instance.OpenWindow<Window_Shop>(WinName.Window_Shop, CloseUIEvent.None).OpenWindow(GameConstUtils.id_shopMall,Shop.ShopType.Mall);
                break;
            case ChildTab.Mark:
                UIRootMgr.Instance.OpenWindow<Window_Shop>(WinName.Window_Shop, CloseUIEvent.None).OpenWindow(GameConstUtils.id_shopMark, Shop.ShopType.Mark);
                break;
            default:
                break;
        }
        m_CurTab = toTab;
    }


    public void BtnEvt_Charge()
    {
 
    }

    public void BtnEvt_OpenAD()
    {
 
    }



    public void BtnEvt_Exit()
    {
        CloseWindow();
    }

    void CloseCurTabWindow()
    {  
        UIRootMgr.Instance.CloseWindow_InOpenList(WinName.Window_Shop);
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
        CloseCurTabWindow();
    }
  

}
