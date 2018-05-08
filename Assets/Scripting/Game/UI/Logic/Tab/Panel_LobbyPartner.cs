using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_LobbyPartner : MonoBehaviour
{

    public class ViewObj
    {
        public TextButton TBtnPartner;
        public Image PartnerImage;
        public Text PartnerDialogueText;
        public ViewObj(UIViewBase view)
        {
            if (TBtnPartner != null) return;
            TBtnPartner = view.GetCommon<TextButton>("TBtnPartner");
            PartnerImage = view.GetCommon<Image>("PartnerImage");
            PartnerDialogueText = view.GetCommon<Text>("PartnerDialogueText");
        }
    }
    private ViewObj mViewObj;
    public void Init()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());

        mViewObj.TBtnPartner.TextBtn.text = LangMgr.GetText("同伴");
        mViewObj.TBtnPartner.SetOnAduioClick(BtnEvt_EnterPartner);

        mViewObj.PartnerDialogueText.text = LangMgr.GetText("你的名字");
    }


    void BtnEvt_EnterPartner()
    {
        UIRootMgr.Instance.OpenWindow<Window_CreatePartner>(WinName.Window_CreatePartner).OpenWindow();
    }





}
