using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_LobbyPartner : MonoBehaviour
{

    public class ViewObj
    {
        public TextButton TBtnPartner;
        public Text PartnerDialogueText;
        public RawImage PartnerHead;
        public RawImage PartnerHair;
        public Image PartnerLock;
        public ViewObj(UIViewBase view)
        {
            if (TBtnPartner != null) return;
            TBtnPartner = view.GetCommon<TextButton>("TBtnPartner");
            PartnerDialogueText = view.GetCommon<Text>("PartnerDialogueText");
            PartnerHead = view.GetCommon<RawImage>("PartnerHead");
            PartnerHair = view.GetCommon<RawImage>("PartnerHair");
            PartnerLock = view.GetCommon<Image>("PartnerLock");
        }
    }
    private ViewObj mViewObj;
    public void Init()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());

        mViewObj.TBtnPartner.TextBtn.text = LangMgr.GetText("同伴");
        mViewObj.TBtnPartner.SetOnAduioClick(BtnEvt_EnterPartner);

        mViewObj.PartnerDialogueText.text = LangMgr.GetText("你的名字");

        bool isLcok = true;
        if (PlayerPrefsBridge.Instance.PartnerAcce.HavePartner())
        {
            isLcok = false;
            Window_CreatePartner.SetPartnerTex(mViewObj.PartnerHead, mViewObj.PartnerHair,
                PlayerPrefsBridge.Instance.PartnerAcce.curPartener.idx,
                PlayerPrefsBridge.Instance.PartnerAcce.curPartener.hairColor);

            mViewObj.PartnerDialogueText.text =
                PartnerDialogue.GetPartnerDialogueStr(PlayerPrefsBridge.Instance.PartnerAcce.curPartener,
                    TUtility.GetLocalDayHour(), 0);
            mViewObj.PartnerLock.gameObject.SetActive(false);
        }
        else
        {
            mViewObj.PartnerDialogueText.text = LangMgr.GetText(LobbyDialogue.GetDescStr("desc_main_partner"));
        }

        mViewObj.PartnerHead.gameObject.SetActive(!isLcok);
        mViewObj.PartnerHair.gameObject.SetActive(!isLcok);
        mViewObj.PartnerLock.gameObject.SetActive(isLcok);
    }


    void BtnEvt_EnterPartner()
    {
        if (!PlayerPrefsBridge.Instance.PartnerAcce.HavePartner())
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk(LangMgr.GetText("你还没有寻找到那个人"), Color.white);
            return;
        }
        UIRootMgr.Instance.OpenWindow<Window_CreatePartner>(WinName.Window_CreatePartner).OpenWindow();
    }




}
