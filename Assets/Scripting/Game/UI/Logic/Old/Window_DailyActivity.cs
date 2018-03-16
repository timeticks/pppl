using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class Window_DailyActivity : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_DailyActivityItem;
        public Transform ItemRoot;
        public ViewObj(UIViewBase view)
        {
            if (Part_DailyActivityItem == null) Part_DailyActivityItem = view.GetCommon<GameObject>("Part_DailyActivityItem");
            if (ItemRoot == null) ItemRoot = view.GetCommon<Transform>("ItemRoot");
        }
    }
    public class TypeItemObj : SmallViewObj
    {
        public Image Icon;
        public Text TextName;
        public Button EnterBtn;
        public Text EnterBtnText;
        public GameObject LimitMask;
        public Text TextOpenDemand;
        public Material GreyImageMat;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (Icon == null) Icon = view.GetCommon<Image>("Icon");
            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (EnterBtn == null) EnterBtn = view.GetCommon<Button>("EnterBtn");
            if (EnterBtnText == null) EnterBtnText = view.GetCommon<Text>("EnterBtnText");
            if (LimitMask == null) LimitMask = view.GetCommon<GameObject>("LimitMask");
            if (TextOpenDemand == null) TextOpenDemand = view.GetCommon<Text>("TextOpenDemand");
            if (GreyImageMat == null) GreyImageMat = view.GetCommon<Material>("GreyImageMat");        }
    }
    private List<TypeItemObj> mTypeItemList;

    private ViewObj mViewObj;
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Fresh();
    }
 
    public void Fresh()
    {
        int dailyActivityNum = 2;
        if (mTypeItemList == null) mTypeItemList = new List<TypeItemObj>();
        mTypeItemList = TAppUtility.Instance.AddViewInstantiate<TypeItemObj>(mTypeItemList, mViewObj.Part_DailyActivityItem,
            mViewObj.ItemRoot, dailyActivityNum);
        InitItem(mTypeItemList[0], ActivityAccessor.ActivityType.Tower);
        InitItem(mTypeItemList[1], ActivityAccessor.ActivityType.DailyDungeon);
    }

    void InitItem(TypeItemObj itemObj, ActivityAccessor.ActivityType ty)
    {
        ActivityAccessor.ActivityType tempTy = ty;
        switch (ty)
        {
            case ActivityAccessor.ActivityType.DailyDungeon:
            {
                itemObj.TextName.text = "每日秘境";
                itemObj.LimitMask.gameObject.SetActive(true);
                itemObj.TextOpenDemand.text = HeroLevelUp.GetStateName(360);
                itemObj.EnterBtn.gameObject.SetActive(false);
                break;
            }
            case ActivityAccessor.ActivityType.Tower:
            {
                itemObj.TextName.text = "仙魔录";
                if (PlayerPrefsBridge.Instance.PlayerData.Level < GameConstUtils.module_tower)//查看模块开放等级
                {
                    itemObj.LimitMask.gameObject.SetActive(true);
                    itemObj.TextOpenDemand.text = HeroLevelUp.GetStateName(GameConstUtils.module_tower);
                    itemObj.EnterBtn.gameObject.SetActive(false);
                }
                else
                {
                    itemObj.LimitMask.gameObject.SetActive(false);
                    itemObj.EnterBtn.gameObject.SetActive(true);
                }
                itemObj.EnterBtn.SetOnClick(delegate() { BtnEvt_OpenWindowByActivity(tempTy); });
                break;
            }
        }
    }

    void BtnEvt_OpenWindowByActivity(ActivityAccessor.ActivityType ty)//刷新任务
    {
        if (ty == ActivityAccessor.ActivityType.DailyDungeon)
        {
            
        }
        else if (ty == ActivityAccessor.ActivityType.Tower)
        {
            UIRootMgr.Instance.CloseWindow_InOpenList(WinName.WindowBig_Activity);
            UIRootMgr.Instance.OpenWindow<Window_Tower>(WinName.Window_Tower, CloseUIEvent.None).OpenWindow();
        }
    }

    public void CloseTaskListPanel()  //关闭任务列表界面时刷新
    {
        Fresh();
    }
}
