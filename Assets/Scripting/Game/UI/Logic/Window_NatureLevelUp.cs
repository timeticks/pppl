using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_NatureLevelUp : WindowBase
{
    public class ViewObj
    {
        public Text TitleText;
        public Button BtnExit;
        public Button BtnMask;
        public GameObject Part_NatureItem;
        public Transform ItemRoot;
        public ViewObj(UIViewBase view)
        {
            if (TitleText != null) return;
            TitleText = view.GetCommon<Text>("TitleText");
            BtnExit = view.GetCommon<Button>("BtnExit");
            BtnMask = view.GetCommon<Button>("BtnMask");
            Part_NatureItem = view.GetCommon<GameObject>("Part_NatureItem");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
        }
    }
    private ViewObj mViewObj;
    public class Part_NatureItem : SmallViewObj
    {
        public Text NameText;
        public Text DescText;
        public Text NumText;
        public TextButton TBtnLevelUp;
        public override void Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            NameText = view.GetCommon<Text>("NameText");
            DescText = view.GetCommon<Text>("DescText");
            NumText = view.GetCommon<Text>("NumText");
            TBtnLevelUp = view.GetCommon<TextButton>("TBtnLevelUp");
        }
    }
    public List<Part_NatureItem> mNatureItemList = new List<Part_NatureItem>();
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        base.OpenWin();
        Init();
    }

    void Init()
    {
        mViewObj.BtnExit.SetOnAduioClick(BtnEvt_Exit);
        mViewObj.BtnMask.SetOnAduioClick(BtnEvt_Exit);
        Fresh();
    }

    void Fresh()
    {
        int natureTyCount = (int) NatureType.Max - (int) NatureType.None-1;
        TAppUtility.Instance.AddViewInstantiate<Part_NatureItem>(mNatureItemList, mViewObj.Part_NatureItem,
            mViewObj.ItemRoot, natureTyCount);
        int itemNum = PlayerPrefsBridge.Instance.GetItemNum(GameConstUtils.id_item_nature);

        for (int i = 0; i < natureTyCount; i++)
        {
            NatureType natureTy = (NatureType) (i+1);
            int natureLevel = PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(natureTy);
            NatureLevelUp nature = NatureLevelUp.Fetcher.GetNatureLevelUpCopy(natureTy, natureLevel, true);
            int maxLevel = NatureLevelUp.Fetcher.GetNatureLevelUpMax(natureTy);

            mNatureItemList[i].NumText.text ="";
            string numText;
            if (natureLevel >= maxLevel)
            {
                mNatureItemList[i].DescText.text = LangMgr.GetText(nature.desc, nature.natureMisc.ToString_Pct(),LangMgr.GetText("无"));
                mNatureItemList[i].TBtnLevelUp.gameObject.SetActive(false);
                numText = LangMgr.GetText("已最大等级");
            }
            else
            {
                NatureLevelUp nextNature = NatureLevelUp.Fetcher.GetNatureLevelUpCopy(natureTy, natureLevel + 1, true);
                mNatureItemList[i].DescText.text = LangMgr.GetText(nature.desc, nature.natureMisc.ToString_Pct(), nextNature.natureMisc.ToString_Pct());
                mNatureItemList[i].TBtnLevelUp.gameObject.SetActive(true);
                mNatureItemList[i].TBtnLevelUp.SetOnAduioClick(delegate() { BtnEvt_LevelUp(natureTy); });
                numText = string.Format("{0}:{1}/{2}", LangMgr.GetText("宝石"), itemNum, nature.needNum.ToString());
            }
            mNatureItemList[i].NameText.text = string.Format("{0}  {1}", LangMgr.GetText(nature.name),numText);
        }
    }

    void BtnEvt_LevelUp(NatureType natureTy)
    {
        int natureLevel = PlayerPrefsBridge.Instance.PlayerData.GetNatureLevel(natureTy);
        NatureLevelUp nature = NatureLevelUp.Fetcher.GetNatureLevelUpCopy(natureTy, natureLevel, true);
        int maxLevel = NatureLevelUp.Fetcher.GetNatureLevelUpMax(natureTy);
        if (natureLevel >= maxLevel)
            return;
        int remain = PlayerPrefsBridge.Instance.consumeItem(GameConstUtils.id_item_nature, nature.needNum, true, "");
        if (remain < 0)
        {
            UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("物品不足"), Color.red);
            return;
        }
        PlayerPrefsBridge.Instance.natureLevelUp(natureTy, true);
        UIRootMgr.Instance.Window_UpTips.InitTips(LangMgr.GetText("升级成功"), Color.green);
        Fresh();
    }


    void BtnEvt_Exit()
    {
        CloseWindow();
    }
}
