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
        public ViewObj(UIViewBase view)
        {
            if (TitleText != null) return;
            TitleText = view.GetCommon<Text>("TitleText");
            BtnExit = view.GetCommon<Button>("BtnExit");
            Part_SelectBtnItem = view.GetCommon<GameObject>("Part_SelectBtnItem");
            RootSelectList = view.GetCommon<Transform>("RootSelectList");
            SelectDescText = view.GetCommon<Text>("SelectDescText");
            PartnerNameText = view.GetCommon<Text>("PartnerNameText");
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
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init();
    }

    void Init()
    {
        mViewObj.BtnExit.SetOnAduioClick(BtnEvt_Exit);

        Fresh();
    }

    void Fresh()
    {
        string sexStr = "??";
        if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex > (int)PartnerData.SelectStep.Sex)
            sexStr = string.Format("{0}:{1}", LangMgr.GetText("性别"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectSex.GetDescEx()));

        string hairStr = "??";
        if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex > (int)PartnerData.SelectStep.HairColor)
            hairStr = string.Format("{0}:{1}", LangMgr.GetText("发色"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectHairColor.GetDescEx()));
        
        string skinStr ="??";
        if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex > (int)PartnerData.SelectStep.SkinColor)
            skinStr = string.Format("{0}:{1}", LangMgr.GetText("皮肤"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectSkinColor.GetDescEx()));
        
        string memoryStr = string.Format("{0}:{1}", LangMgr.GetText("回忆"), "??");
        if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex > (int)PartnerData.SelectStep.HappyMemory)
            skinStr = string.Format("{0}:{1}", LangMgr.GetText("回忆"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectHappyMemory.GetDesc()));

        string characStr = string.Format("{0}:{1}", LangMgr.GetText("性格"), "??");
        if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex > (int)PartnerData.SelectStep.Charac)
            skinStr = string.Format("{0}:{1}", LangMgr.GetText("性格"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectCharacType.GetDescEx()));

        string nameStr = string.Format("{0}:{1}", LangMgr.GetText("名字"), "??");
        if ((int)PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex >= (int)PartnerData.SelectStep.Max)
            skinStr = string.Format("{0}:{1}", LangMgr.GetText("名字"), LangMgr.GetText(PlayerPrefsBridge.Instance.PartnerAcce.selectSkinColor.GetDescEx()));

        mViewObj.PartnerNameText.text = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}" ,sexStr , hairStr , skinStr , memoryStr , characStr , nameStr);

        string selectDesc = "";
        List<string> selectItemList = new List<string>();   //可选择项
        switch (PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex)
        {
            case PartnerData.SelectStep.Sex:
                {
                    selectDesc = string.Format("{0}\n", LangMgr.GetText("你回想起他(她)的性别是："));
                    for (int i = (int)PartnerData.Sex.None + 1; i < (int)PartnerData.Sex.Max; i++)
                    {
                        selectItemList.Add(((PartnerData.Sex) i).GetDescEx());
                    }
                    break;
                }
            case PartnerData.SelectStep.HairColor:
                {
                    selectDesc = string.Format("{0}\n", LangMgr.GetText("你回想起他(她)的发色是："));
                    for (int i = (int)PartnerData.HairColor.None + 1; i < (int)PartnerData.HairColor.Max; i++)
                    {
                        selectItemList.Add(((PartnerData.HairColor)i).GetDescEx());
                    }
                    break;
                }
            case PartnerData.SelectStep.SkinColor:
                {
                    selectDesc = string.Format("{0}\n", LangMgr.GetText("你回想起他(她)的肤色是："));
                    for (int i = (int)PartnerData.SkinColor.None + 1; i < (int)PartnerData.SkinColor.Max; i++)
                    {
                        selectItemList.Add(((PartnerData.SkinColor)i).GetDescEx());
                    }
                    break;
                }
            case PartnerData.SelectStep.Charac:
                {
                    selectDesc = string.Format("{0}\n", LangMgr.GetText("你回想起他(她)性格偏向："));
                    for (int i = (int)PartnerData.CharacType.None + 1; i < (int)PartnerData.CharacType.Max; i++)
                    {
                        selectItemList.Add(((PartnerData.CharacType)i).GetDescEx());
                    }
                    break;
                }
            case PartnerData.SelectStep.HappyMemory:
                {
                    selectDesc = string.Format("{0}\n", LangMgr.GetText("你回想起和他(她)有过的美好回忆："));
                    for (int i = (int)PartnerData.HappyMemory.None + 1; i < (int)PartnerData.HappyMemory.Max; i++)
                    {
                        selectItemList.Add(((PartnerData.HappyMemory)i).GetDescEx());
                    }
                    break;
                }
        }

        mSelectItemList = TAppUtility.Instance.AddViewInstantiate<SelectSmallObj>(mSelectItemList, mViewObj.Part_SelectBtnItem, mViewObj.RootSelectList, selectItemList.Count);
        for (int i = 0; i < mSelectItemList.Count; i++)
        {
            int tempIndex = i + 1;
            mSelectItemList[i].NameText.text = string.Format(selectItemList[i]);
            mSelectItemList[i].BgBtn.SetOnAduioClick(delegate() { BtnEvt_SelectItem(tempIndex); });
        }
    }

    void BtnEvt_SelectItem(int itemIndex)
    {
        TDebug.LogInEditorF("选择选项:{0}|{1}", PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex.ToString(), itemIndex);
        SetSelectStep(PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex, itemIndex);
    }

    //设置并保存某步骤信息
    void SetSelectStep(PartnerData.SelectStep selectStep, int selectType)
    {
        if (PlayerPrefsBridge.Instance.PartnerAcce.selectStepIndex == PartnerData.SelectStep.Max)
        {
            TDebug.LogError("selectStep == PartnerData.SelectStep.Max");
            return;
        }
        switch (selectStep)
        {
            case PartnerData.SelectStep.Sex:
            {
                PlayerPrefsBridge.Instance.PartnerAcce.selectSex = (PartnerData.Sex) selectType;
                break;
            }
            case PartnerData.SelectStep.HairColor:
            {
                PlayerPrefsBridge.Instance.PartnerAcce.selectHairColor = (PartnerData.HairColor)selectType;
                break;
            }
            case PartnerData.SelectStep.SkinColor:
            {
                PlayerPrefsBridge.Instance.PartnerAcce.selectSkinColor = (PartnerData.SkinColor)selectType;
                break;
            }
            case PartnerData.SelectStep.Charac:
            {
                PlayerPrefsBridge.Instance.PartnerAcce.selectCharacType = (PartnerData.CharacType)selectType;
                break;
            }
            case PartnerData.SelectStep.HappyMemory:
            {
                PlayerPrefsBridge.Instance.PartnerAcce.selectHappyMemory = (PartnerData.HappyMemory)selectType;
                break;
            }
        }
        PlayerPrefsBridge.Instance.savePartnerModule();
        Fresh();
    }


    void BtnEvt_Exit()
    {
        CloseWindow();
    }
}
