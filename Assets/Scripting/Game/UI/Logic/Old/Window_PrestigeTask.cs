using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class Window_PrestigeTask : WindowBase
{
    public class ViewObj
    {
        public Text TextPrestigeFinish;
        public Transform RootItem;
        public GameObject Part_PrestigeTypeItem;
        public Panel_PrestigeTaskList Panel_TaskRoot;   //任务列表界面
        public ViewObj(UIViewBase view)
        {
            if (TextPrestigeFinish == null) TextPrestigeFinish = view.GetCommon<Text>("TextPrestigeFinish");
            if (RootItem == null) RootItem = view.GetCommon<Transform>("RootItem");
            if (Part_PrestigeTypeItem == null) Part_PrestigeTypeItem = view.GetCommon<GameObject>("Part_PrestigeTypeItem");
            if (Panel_TaskRoot == null) Panel_TaskRoot = view.GetCommon<GameObject>("Panel_TaskRoot").CheckAddComponent<Panel_PrestigeTaskList>();        }
    }
    public class TypeItemObj : SmallViewObj
    {
        public Text NameText;
        public Button OpenTaskBtn;
        public Text PrestigeLevelText;
        public Text PrestigeNumText;
        public Text PrestigeRewardText;
        public Text DoingText;
        public Text OpenTaskText;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
            if (OpenTaskBtn == null) OpenTaskBtn = view.GetCommon<Button>("OpenTaskBtn");
            if (PrestigeLevelText == null) PrestigeLevelText = view.GetCommon<Text>("PrestigeLevelText");
            if (PrestigeNumText == null) PrestigeNumText = view.GetCommon<Text>("PrestigeNumText");
            if (PrestigeRewardText == null) PrestigeRewardText = view.GetCommon<Text>("PrestigeRewardText");
            if (DoingText == null) DoingText = view.GetCommon<Text>("DoingText");
            if (OpenTaskText == null) OpenTaskText = view.GetCommon<Text>("OpenTaskText");        }
    }
    private List<TypeItemObj> mTypeItemList;

    private ViewObj mViewObj;
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Fresh();
        UIRootMgr.Instance.SetRootInTopLobby(WinName.Window_PrestigeTask, mViewObj.Panel_TaskRoot.transform);

    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    public void Fresh()
    {
        mViewObj.Panel_TaskRoot.gameObject.SetActive(false); //初始关闭任务列表

        if (mTypeItemList == null) mTypeItemList = new List<TypeItemObj>();
        mTypeItemList = TAppUtility.Instance.AddViewInstantiate<TypeItemObj>(mTypeItemList, mViewObj.Part_PrestigeTypeItem,
            mViewObj.RootItem, PlayerPrefsBridge.Instance.PlayerData.PrestigeList.Length);

        PrestigeTask curTask=null;
        if (PlayerPrefsBridge.Instance.ActivityData.CurTaskIdx > 0)
        {
            curTask = PrestigeTask.PrestigeTaskFetcher.GetPrestigeTaskByCopy(PlayerPrefsBridge.Instance.ActivityData.CurTaskIdx);
        }
        PrestigeLevel curLevel;
        for (int i = 0; i < PlayerPrefsBridge.Instance.PlayerData.PrestigeList.Length; i++)
        {
            //根据信息进行显示
            curLevel = PlayerPrefsBridge.Instance.PlayerData.PrestigeList[i];
            PrestigeLevel.PrestigeType ty = (PrestigeLevel.PrestigeType)i;
            mTypeItemList[i].NameText.text = string.Format("{0}任务", ty.GetDesc());
            mTypeItemList[i].PrestigeLevelText.text = string.Format("声望等级：{0}", curLevel.name);
            mTypeItemList[i].PrestigeNumText.text = string.Format("声望\u3000\u3000：{0}/{1}", curLevel.GetCurLevelPrestigeOffset(), curLevel.GetCurLevelPrestigeMax());
            mTypeItemList[i].PrestigeRewardText.text = string.Format("任务奖励：{0}声望\n\u3000\u3000\u3000\u3000\u3000灵石",ty.GetDesc());
            
            mTypeItemList[i].OpenTaskText.text = "查看任务";
            mTypeItemList[i].OpenTaskBtn.SetOnClick(delegate() { BtnEvt_OpenTaskDetail(ty); });

            //如果有正在进行的任务
            if (curTask != null && curTask.Type == ty) 
            {
                mTypeItemList[i].DoingText.text = "任务中";
                mTypeItemList[i].DoingText.gameObject.SetActive(true);
            }
            else
            {
                mTypeItemList[i].DoingText.gameObject.gameObject.SetActive(false);
            }
        }
        mViewObj.TextPrestigeFinish.text = string.Format("今日已完成数量：{0}/{1}", PlayerPrefsBridge.Instance.ActivityData.TaskFinishNum, VipAddition.MAX_PRESTIGE_TASK_NUM.getValueByVip(PlayerPrefsBridge.Instance.PlayerData.IsVip()));
    }

    void BtnEvt_OpenTaskDetail(PrestigeLevel.PrestigeType ty )//刷新任务
    {
        mViewObj.Panel_TaskRoot.Init(this, ty);
    }

    public void CloseTaskListPanel()  //关闭任务列表界面时刷新
    {
        Fresh();
    }
}
