using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_CreatePartner : WindowBase {
    public class ViewObj
    {
        public Text TitleText;
        public Button BtnExit;
        public GameObject Part_SelectBtnItem;
        public Transform RootSelectList;
        public Text SelectDescText;
        public Text PartnerNameText;
        public Button BtnPartnerImage;
        public Button BtnAddIntimacy;
        public Button MaskBtn;
        public NumScrollTool IntimacyScrollTool;
        public RawImage PartnerHead;
        public RawImage PartnerHair;
        public ViewObj(UIViewBase view)
        {
            if (TitleText != null) return;
            TitleText = view.GetCommon<Text>("TitleText");
            BtnExit = view.GetCommon<Button>("BtnExit");
            Part_SelectBtnItem = view.GetCommon<GameObject>("Part_SelectBtnItem");
            RootSelectList = view.GetCommon<Transform>("RootSelectList");
            SelectDescText = view.GetCommon<Text>("SelectDescText");
            PartnerNameText = view.GetCommon<Text>("PartnerNameText");
            BtnPartnerImage = view.GetCommon<Button>("BtnPartnerImage");
            BtnAddIntimacy = view.GetCommon<Button>("BtnAddIntimacy");
            MaskBtn = view.GetCommon<Button>("MaskBtn");
            IntimacyScrollTool = view.GetCommon<NumScrollTool>("IntimacyScrollTool");
            PartnerHead = view.GetCommon<RawImage>("PartnerHead");
            PartnerHair = view.GetCommon<RawImage>("PartnerHair");
        }
    }
    public class SelectSmallObj : SmallViewObj
    {
        public Button BgBtn;
        public Text NameText;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            BgBtn = view.GetCommon<Button>("BgBtn");
            NameText = view.GetCommon<Text>("NameText");
        }
    }
    private ViewObj mViewObj;
    private List<SelectSmallObj> mSelectItemList = new List<SelectSmallObj>();
    private int mChatNum;   //每次打开窗口重置
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init();
    }

    void Init()
    {
        mChatNum = 0;
        mViewObj.BtnExit.SetOnAduioClick(BtnEvt_Exit);
        mViewObj.MaskBtn.SetOnAduioClick(BtnEvt_Exit);
        mViewObj.BtnPartnerImage.SetOnAduioClick(BtnEvt_PartnerDialogue);
        mViewObj.BtnAddIntimacy.SetOnAduioClick(BtnEvt_AddIntimacy);
        Fresh();
    }

    public void Fresh()
    {
        string sexStr = string.Format("{0}:{1}", LangMgr.GetText("性别"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectSex.GetDescEx()));

        string skinStr = string.Format("{0}:{1}", LangMgr.GetText("皮肤"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectSkinColor.GetDescEx()));

        string hairStr = string.Format("{0}:{1}", LangMgr.GetText("发色"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectHairColor.GetDescEx()));
        
        string characStr = string.Format("{0}:{1}", LangMgr.GetText("性格"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectCharacType.GetDescEx()));

        string hobbyStr = string.Format("{0}:{1}", LangMgr.GetText("爱好"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectHobbyType.GetDesc()));

        string nameStr = string.Format("{0}:{1}", LangMgr.GetText("名字"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.curPartener.partnerName));

        mViewObj.PartnerNameText.text = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", sexStr, skinStr ,hairStr, characStr, hobbyStr, nameStr);

        //string selectDesc = "";
        //mViewObj.SelectDescText.text = selectDesc;
        //mSelectItemList = TAppUtility.Instance.AddViewInstantiate<SelectSmallObj>(mSelectItemList, mViewObj.Part_SelectBtnItem, mViewObj.RootSelectList, selectItemList.Count);
        //for (int i = 0; i < selectItemList.Count; i++)
        //{
        //    int tempIndex = i + 1;
        //    mSelectItemList[i].NameText.text = string.Format(selectItemList[i]);
        //    mSelectItemList[i].BgBtn.SetOnAduioClick(delegate() { BtnEvt_SelectItem(tempIndex); });
        //}

        if (PlayerPrefsBridge.Instance.PartnerAcce.HavePartner())
        {
            IntimacyLevelUp intimacy = IntimacyLevelUp.Fetcher.GetIntimacyLevelUpCopy(PlayerPrefsBridge.Instance.PartnerAcce.curPartener.intimacyLevel, true);
            if (intimacy != null)
            {
                mViewObj.IntimacyScrollTool.m_NameText.text = intimacy.name;
                mViewObj.IntimacyScrollTool.m_NumText.text = string.Format("{0}/{1}",
                    PlayerPrefsBridge.Instance.PartnerAcce.curPartener.intimacyNum, intimacy.num);
            }
        }

        //头像
        if (PlayerPrefsBridge.Instance.PartnerAcce.HavePartner())
        {
            SetPartnerTex(mViewObj.PartnerHead, mViewObj.PartnerHair,
               PlayerPrefsBridge.Instance.PartnerAcce.curPartener.idx,
               PlayerPrefsBridge.Instance.PartnerAcce.curPartener.hairColor);
        }
    }

    void BtnEvt_SelectItem(int itemIndex)
    {
    }

    void BtnEvt_PartnerDialogue()
    {
        if (!PlayerPrefsBridge.Instance.PartnerAcce.HavePartner())
        {
            UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("你还没有找到他(她)哦，无法交流") , Color.red);
            return;
        }
        mChatNum++;
        mViewObj.SelectDescText.text = PartnerDialogue.GetPartnerDialogueStr(PlayerPrefsBridge.Instance.PartnerAcce.curPartener, TUtility.GetLocalDayHour(), mChatNum);
    }

    void BtnEvt_AddRecall()
    {
        UIRootMgr.Instance.OpenWindow<Window_ItemInventory>(WinName.Window_ItemInventory, CloseUIEvent.None)
            .OpenWindow(Item.ItemType.Recall);
    }

    void BtnEvt_AddIntimacy()
    {
        UIRootMgr.Instance.OpenWindow<Window_ItemInventory>(WinName.Window_ItemInventory, CloseUIEvent.None)
            .OpenWindow(Item.ItemType.Intimacy);
    }

    void BtnEvt_Exit()
    {
        CloseWindow();
    }



    public static void SetPartnerTex(RawImage head, RawImage hair, int partnerIdx, PartnerData.HairColor hairColor)
    {
        Partner partner = Partner.Fetcher.GetPartnerCopy(partnerIdx);
        TLoader tloader = SharedAsset.Instance.LoadSpritePart<Texture>(partner.icon);
        if (tloader != null)
            head.texture = tloader.ResultObjTo<Texture>();

        tloader = SharedAsset.Instance.LoadSpritePart<Texture>(string.Format("{0}_hair{1}", partner.icon, (int)hairColor));
        if (tloader != null)
            hair.texture = tloader.ResultObjTo<Texture>();
    }
}
