using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
public class LobbySceneMainUIMgr : BaseMainUIMgr
{
    public static LobbySceneMainUIMgr Instance { get; private set; }

    public static bool SendEnterBottingOver;  //用于判断服务器是否将EnterBotting发送完成

    private UIViewBase mView;

    public class ViewObj
    {
        public GameObject LobbyTop;
        public CanvasGroup LobbyUI;
        public Button BtnRoleInfo;
        public Button BtnAssem;
        public Button BtnWorldChoose;
        public Button BtnCave;
        public GameObject RoleInfo;
        public Text NameText;
        public TextScroller TextScrollView;
        public Text TextGold;
        public Text TextDiamond;
        public Text TextExp;
        public Text TextPoten;
        public Text TextSect;
        public Text TextLevel;
        public Button BtnActivity;
        public Button BtnHonor;
        public Button BtnShop;
        public GameObject LimitAssem;
        public GameObject LimitCave;
        public GameObject LimitActivity;
        public GameObject LimitHonor;
        public GameObject LimitShop;
        public Text TextBtnAssem;
        public Text TextBtnCave;
        public Text TextBtnActivity;
        public Text TextBtnHonor;
        public Text TextBtnShop;
        public Text TextAge;
        public Text TextState;
        public GameObject DownRoot;
        public GameObject UpRoot;
        public Button IconHeadBtn;

        public Button BtnAddDiamond;
        public Button BtnAddGold;
        public Button BtnAddPoten;

        public Button BtnSect;
        public Button BtnVIP;

        public Button BtnTipsMask;
        public GameObject PotentialTips;
        public GameObject GoldTips;
        public GameObject DiamondTips;
        public Text TextPotentialDesc;
        public Text TextGoldDesc;
        public Text TextDiamondDesc;
        public Button BtnDiamond;
        public Button BtnGold;
        public Button BtnPotential;

        public Transform TopIntroduce;
        public Button BtnLevelLimit;

        public Button BtnChatSystem;
        public Button BtnChatWorld;
        public Button BtnChatSect;
        public Text TextBtnChatSystem;
        public Text TextBtnChatWorld;
        public Text TextBtnChatSect;
        public GameObject ChatInputPanel;
        public Text TextPlaceholder;
        public Button BtnSendChat;
        public Text TextBtnSendChat;
        public InputField ChatInput;
        public Button BtnModule;
        public Text Text_yue;
        public Text Text_ka;

        public TextButton BtnFesitvity;
        public GameObject SPContainer;
        public List<Button> BtnChatList;

        public List<Text> TextBtnChatList;

        public ViewObj(UIViewBase view)
        {
            if (LobbyTop != null)
                return;
            if (LobbyTop == null) LobbyTop = view.GetCommon<GameObject>("LobbyTop");
            if (BtnRoleInfo == null) BtnRoleInfo = view.GetCommon<Button>("BtnRoleInfo");
            if (BtnAssem == null) BtnAssem = view.GetCommon<Button>("BtnAssem");
            if (BtnWorldChoose == null) BtnWorldChoose = view.GetCommon<Button>("BtnWorldChoose");
            if (BtnCave == null) BtnCave = view.GetCommon<Button>("BtnCave");
            if (RoleInfo == null) RoleInfo = view.GetCommon<GameObject>("RoleInfo");
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (TextScrollView == null) TextScrollView = view.GetCommon<TextScroller>("TextScrollView");
            if (TextGold == null) TextGold = view.GetCommon<Text>("TextGold");
            if (TextDiamond == null) TextDiamond = view.GetCommon<Text>("TextDiamond");
            if (TextExp == null) TextExp = view.GetCommon<Text>("TextExp");
            if (TextPoten == null) TextPoten = view.GetCommon<Text>("TextPoten");
            if (TextSect == null) TextSect = view.GetCommon<Text>("TextSect");
            if (TextLevel == null) TextLevel = view.GetCommon<Text>("TextLevel");
            if (BtnActivity == null) BtnActivity = view.GetCommon<Button>("BtnActivity");
            if (BtnHonor == null) BtnHonor = view.GetCommon<Button>("BtnHonor");
            if (BtnShop == null) BtnShop = view.GetCommon<Button>("BtnShop");
            if (LimitAssem == null) LimitAssem = view.GetCommon<GameObject>("LimitAssem");
            if (LimitCave == null) LimitCave = view.GetCommon<GameObject>("LimitCave");
            if (LimitActivity == null) LimitActivity = view.GetCommon<GameObject>("LimitActivity");
            if (LimitHonor == null) LimitHonor = view.GetCommon<GameObject>("LimitHonor");
            if (LimitShop == null) LimitShop = view.GetCommon<GameObject>("LimitShop");
            if (TextAge == null) TextAge = view.GetCommon<Text>("TextAge");
            if (TextState == null) TextState = view.GetCommon<Text>("TextState");
            if (TextBtnAssem == null) TextBtnAssem = view.GetCommon<Text>("TextBtnAssem");
            if (TextBtnCave == null) TextBtnCave = view.GetCommon<Text>("TextBtnCave");
            if (TextBtnActivity == null) TextBtnActivity = view.GetCommon<Text>("TextBtnActivity");
            if (TextBtnHonor == null) TextBtnHonor = view.GetCommon<Text>("TextBtnHonor");
            if (TextBtnShop == null) TextBtnShop = view.GetCommon<Text>("TextBtnShop");
            if (LobbyUI == null) LobbyUI = view.GetCommon<CanvasGroup>("LobbyUI");
            if (DownRoot == null) DownRoot = view.GetCommon<GameObject>("DownRoot");
            if (UpRoot == null) UpRoot = view.GetCommon<GameObject>("UpRoot");
            if (IconHeadBtn == null) IconHeadBtn = view.GetCommon<Button>("IconHeadBtn");
            if (BtnAddDiamond == null) BtnAddDiamond = view.GetCommon<Button>("BtnAddDiamond");
            if (BtnAddGold == null) BtnAddGold = view.GetCommon<Button>("BtnAddGold");
            if (BtnAddPoten == null) BtnAddPoten = view.GetCommon<Button>("BtnAddPoten");
            BtnSect = view.GetCommon<Button>("BtnSect");
            BtnVIP = view.GetCommon<Button>("BtnVIP");
            Text_yue = view.GetCommon<Text>("Text_yue");
            Text_ka = view.GetCommon<Text>("Text_ka");
            BtnTipsMask = view.GetCommon<Button>("BtnTipsMask");
            PotentialTips = view.GetCommon<GameObject>("PotentialTips");
            GoldTips = view.GetCommon<GameObject>("GoldTips");
            DiamondTips = view.GetCommon<GameObject>("DiamondTips");
            TextPotentialDesc = view.GetCommon<Text>("TextPotentialDesc");
            TextGoldDesc = view.GetCommon<Text>("TextGoldDesc");
            TextDiamondDesc = view.GetCommon<Text>("TextDiamondDesc");
            BtnDiamond = view.GetCommon<Button>("BtnDiamond");
            BtnGold = view.GetCommon<Button>("BtnGold");
            BtnPotential = view.GetCommon<Button>("BtnPotential");
            TopIntroduce = view.GetCommon<Transform>("TopIntroduce");
            BtnLevelLimit = view.GetCommon<Button>("BtnLevelLimit");
           
            BtnChatSystem = view.GetCommon<Button>("BtnChatSystem");
            BtnChatWorld = view.GetCommon<Button>("BtnChatWorld");
            BtnChatSect = view.GetCommon<Button>("BtnChatSect");
            TextBtnChatSystem = view.GetCommon<Text>("TextBtnChatSystem");
            TextBtnChatWorld = view.GetCommon<Text>("TextBtnChatWorld");
            TextBtnChatSect = view.GetCommon<Text>("TextBtnChatSect");
            ChatInputPanel = view.GetCommon<GameObject>("ChatInputPanel");
            TextPlaceholder = view.GetCommon<Text>("TextPlaceholder");
            BtnSendChat = view.GetCommon<Button>("BtnSendChat");
            TextBtnSendChat = view.GetCommon<Text>("TextBtnSendChat");
            ChatInput = view.GetCommon<InputField>("ChatInput");
            BtnModule = view.GetCommon<Button>("BtnModule");
            SPContainer = view.GetCommon<GameObject>("SPContainer");
            BtnFesitvity = view.GetCommon<TextButton>("BtnFesitvity");

            BtnChatList = new List<Button>();
            BtnChatList.Add( BtnChatSystem);
            BtnChatList.Add( BtnChatWorld );
            BtnChatList.Add( BtnChatSect);
            TextBtnChatList = new List<Text>();
            TextBtnChatList.Add(TextBtnChatSystem);
            TextBtnChatList.Add(TextBtnChatWorld);
            TextBtnChatList.Add(TextBtnChatSect);

        }
    }
    private ViewObj mViewObj;


    public void CheckLobbyUI()
    {
        bool haveWindow = false;
        if (UIRootMgr.Instance == null || mViewObj == null || mViewObj.LobbyUI == null) return;
        for (int i = 0; i < UIRootMgr.Instance.OpenWinList.Count; i++)
        {
            if (UIRootMgr.Instance.OpenWinList[i].SetCurrentWin)
                haveWindow = true;
        }
        mViewObj.LobbyUI.alpha = haveWindow ? 0 : 1;
        mViewObj.LobbyUI.interactable = !haveWindow;
    }
}