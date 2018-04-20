using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Panel_PrestigeTaskList : MonoBehaviour {
    public class ViewObj
    {
        public Text TextPrestigeFinish;
        public Text TextPrestigeLevel;
        public Text TextPrestigeNum;
        public Transform RootItem;
        public GameObject Part_PrestigeTaskItem;
        public Button BtnFreshTask;
        public Text TextBtnFreshTask;
        public Button BtnExit;
        public Text TextBtnExit;

        public ViewObj(UIViewBase view)
        {
            if (TextPrestigeFinish == null) TextPrestigeFinish = view.GetCommon<Text>("TextPrestigeFinish");
            if (TextPrestigeLevel == null) TextPrestigeLevel = view.GetCommon<Text>("TextPrestigeLevel");
            if (TextPrestigeNum == null) TextPrestigeNum = view.GetCommon<Text>("TextPrestigeNum");
            if (RootItem == null) RootItem = view.GetCommon<Transform>("RootItem");
            if (Part_PrestigeTaskItem == null) Part_PrestigeTaskItem = view.GetCommon<GameObject>("Part_PrestigeTaskItem");
            if (BtnFreshTask == null) BtnFreshTask = view.GetCommon<Button>("BtnFreshTask");
            if (TextBtnFreshTask == null) TextBtnFreshTask = view.GetCommon<Text>("TextBtnFreshTask");
            if (BtnExit == null) BtnExit = view.GetCommon<Button>("BtnExit");
            if (TextBtnExit == null) TextBtnExit = view.GetCommon<Text>("TextBtnExit");
        }
    }
    private ViewObj mViewObj;

    public class TaskItemObj : SmallViewObj
    {
        public Text TextName;
        public Button BtnStartTask;
        public Text TextBtnStartTask;
        public Text TextUseTime;
        public Text TextReward;
        public override void Init(UIViewBase view)
        {
            base.Init(view);

            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (BtnStartTask == null) BtnStartTask = view.GetCommon<Button>("BtnStartTask");
            if (TextBtnStartTask == null) TextBtnStartTask = view.GetCommon<Text>("TextBtnStartTask");
            if (TextUseTime == null) TextUseTime = view.GetCommon<Text>("TextUseTime");
            if (TextReward == null) TextReward = view.GetCommon<Text>("TextReward");
        }
    }
    private List<TaskItemObj> mMailItemList;

    private TaskItemObj mCurDoingTaskObj;
    private float mCurDownTime;   //完成倒计时
    private Window_PrestigeTask mParentWin;
    private PrestigeLevel.PrestigeType mCurType;
    public void Init(Window_PrestigeTask win ,PrestigeLevel.PrestigeType ty)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        mParentWin = win;
        mCurType = ty;
        if (!PlayerPrefsBridge.Instance.ActivityData.TaskMap.ContainsKey(ty)) //判断服务器是否已传此类型的声望任务过来
        {
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.FreshPrestigeTask, S2C_FreshPrestigeTask);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_FreshPrestigeTask(true, false, ty));
            UIRootMgr.Instance.IsLoading = true;
        }
        else
        {
            Fresh();
        }
    }
    public void CloseWindow()
    {
        mParentWin.CloseTaskListPanel();
    }

    


    public void Fresh()
    {
        gameObject.SetActive(true);
        List<int> taskIdList = PlayerPrefsBridge.Instance.ActivityData.TaskMap[mCurType];
        PrestigeTask[] taskList = new PrestigeTask[taskIdList.Count];
        for (int i = 0; i < taskIdList.Count; i++)
        {
            PrestigeTask t = PrestigeTask.PrestigeTaskFetcher.GetPrestigeTaskByCopy(taskIdList[i]);
            if (t != null) taskList[i] = t;
        }
        if (mMailItemList == null) mMailItemList = new List<TaskItemObj>();
        mMailItemList = TAppUtility.Instance.AddViewInstantiate<TaskItemObj>(mMailItemList, mViewObj.Part_PrestigeTaskItem,
            mViewObj.RootItem, taskList.Length);
        mCurDoingTaskObj = null;

        PrestigeLevel curLevel = PlayerPrefsBridge.Instance.PlayerData.PrestigeList[mCurType.ToInt()];
        //刷新任务item
        for (int i = 0; i < taskList.Length; i++)
        {
            mMailItemList[i].TextName.text = string.Format("<color=#{0}ff>{1}</color>", TUtility.GetColorStringByQuality(taskList[i].Quality), taskList[i].name);
            mMailItemList[i].TextReward.text = string.Format("任务奖励：{0}声望\n\u3000\u3000\u3000\u3000\u3000{1}灵石", taskList[i].Reward,GameUtils.getPrestigeMoneyReward(taskList[i].Money , PlayerPrefsBridge.Instance.PlayerData.Level));
            if (mCurDoingTaskObj==null && PlayerPrefsBridge.Instance.ActivityData.CurTaskIdx == taskList[i].idx
                && (PlayerPrefsBridge.Instance.ActivityData.CurTaskPos < 0 || PlayerPrefsBridge.Instance.ActivityData.CurTaskPos==i))//如果是-1则只要idx相同就可以
            {
                //如果时间到了    TODO:这里应该时间到了后主动刷新界面
                mCurDoingTaskObj = mMailItemList[i];
                long finishDown = PlayerPrefsBridge.Instance.ActivityData.TaskStartTime + taskList[i].Time - TimeUtils.CurrentTimeMillis;
                if (finishDown<0)
                {
                    mMailItemList[i].TextUseTime.text = string.Format("已经完成");
                    mMailItemList[i].TextBtnStartTask.text = "领取奖励";
                    mMailItemList[i].BtnStartTask.SetOnClick(delegate() { BtnEvt_FinishTask(false); });
                }
                else
                {
                    mCurDownTime = finishDown / (float)1000; //记录完成倒计时
                    mMailItemList[i].TextBtnStartTask.text = "加速";
                    mMailItemList[i].TextUseTime.text = string.Format("正在进行：{0}", TUtility.TimeSecondsToDayStr_LCD((int)mCurDownTime));
                    mMailItemList[i].BtnStartTask.SetOnClick(delegate() { BtnEvt_FinishTask(true); });
                }
            }
            else
            {
                int taskId = taskList[i].idx;
                int tempIndex = i;
                mMailItemList[i].TextBtnStartTask.text = "开始任务";
                mMailItemList[i].TextUseTime.text = string.Format("任务耗时：{0}", TUtility.TimeSecondsToDayStr_LCD(taskList[i].Time/1000));
                mMailItemList[i].BtnStartTask.SetOnClick(delegate() { BtnEvt_StartTask(taskId, tempIndex); });
            }
        }

        mViewObj.TextPrestigeFinish.text = string.Format("今日已完成数量：{0}/{1}", PlayerPrefsBridge.Instance.ActivityData.TaskFinishNum, VipAddition.MAX_PRESTIGE_TASK_NUM.getValueByVip(PlayerPrefsBridge.Instance.PlayerData.IsVip()));
        int remianFreeFresh = PlayerPrefsBridge.Instance.ActivityData.GetRemainFreeFresh();
        if (remianFreeFresh > 0)
        {
            mViewObj.TextBtnFreshTask.text = string.Format("免费刷新({0})", remianFreeFresh);
            mViewObj.BtnFreshTask.SetOnClick(delegate() { BtnEvt_FreshPrestigeTask(mCurType, false); });
        }
        else
        {
            mViewObj.TextBtnFreshTask.text = string.Format("仙玉刷新");
            mViewObj.BtnFreshTask.SetOnClick(delegate() { BtnEvt_FreshPrestigeTask(mCurType, true); });
        }
        mViewObj.TextPrestigeLevel.text = string.Format("声望等级：{0}", curLevel.name);
        mViewObj.TextPrestigeNum.text =  string.Format("声望：{0}/{1}",curLevel.GetCurLevelPrestigeOffset() , curLevel.GetCurLevelPrestigeMax());
        mViewObj.BtnExit.SetOnClick(CloseWindow);
    }

    void Update()
    {
        mCurDownTime -= Time.deltaTime;
        if (Time.frameCount%GameLauncher.MAX_FRAME_RATE == 0)
        {
            FreshTime();
        }
    }

    void FreshTime()
    {
        if (mCurDownTime > 0)
        {
            if (mCurDownTime < 0) //如果倒计时到了，重新刷新界面
            {
                Fresh();
            }
            else
            {
                if (mCurDoingTaskObj != null)
                {
                    mCurDoingTaskObj.TextUseTime.text = string.Format("正在进行：{0}", TUtility.TimeSecondsToDayStr_LCD((int)mCurDownTime));
                }
            }
        }
    }


    void BtnEvt_FreshPrestigeTask(PrestigeLevel.PrestigeType ty, bool useDiamond)//刷新任务
    {
        //异常拦截
        if (PlayerPrefsBridge.Instance.ActivityData.CurTaskIdx > 0)
        {
            PrestigeTask task = PrestigeTask.PrestigeTaskFetcher.GetPrestigeTaskByCopy(PlayerPrefsBridge.Instance.ActivityData.CurTaskIdx);
            if (task != null && task.Type == ty && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.LOBBY_WARN_YOU_ZHENG_ZAI_JIN_XING))
            {
                return;
            }
        }
        if (PlayerPrefsBridge.Instance.PlayerData.MySect == Sect.SectType.None && ty == PrestigeLevel.PrestigeType.Self
            && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.LOBBY_WARN_MEI_YOU_ZONG_MEN))
        {
            return;
        }
        if (useDiamond && PlayerPrefsBridge.Instance.PlayerData.Diamond < GameConstUtils.UseDiamondNum
            && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_ZHUAN_SHI_BU_ZU))
        {
           return;
        }
        if (!useDiamond && PlayerPrefsBridge.Instance.ActivityData.GetRemainFreeFresh()>0
            && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_MIAN_FEI_CI_SHU_YONG_WAN))
        {
            return;
        }
        System.Action okDel = delegate()
        {
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.FreshPrestigeTask, S2C_FreshPrestigeTask);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_FreshPrestigeTask(false, useDiamond, ty));
            UIRootMgr.Instance.IsLoading = true;
        };
        if (useDiamond == true)//若使用钻石，强制提示
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel("确定花费1仙玉刷新吗?", okDel);
        }
        else
        {
            okDel();
        }
        
    }
    void S2C_FreshPrestigeTask(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        Fresh();
    }

    void BtnEvt_StartTask(int taskIdx , int taskPos)//开始任务
    {
        if (PlayerPrefsBridge.Instance.ActivityData.CurTaskIdx > 0
            && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.LOBBY_WARN_YOU_ZHENG_ZAI_JIN_XING))
        {
            return;
        }
        if (PlayerPrefsBridge.Instance.ActivityData.TaskFinishNum >= GameConstUtils.max_prestige_task_num
            &&UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.LOBBY_WARN_JIN_RI_CI_SHU_YONG_WAN))
        {
            return;
        }
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.StartPrestigeTask, S2C_StartPrestigeTask);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_StartPrestigeTask(mCurType, taskIdx, taskPos));
        UIRootMgr.Instance.IsLoading = true;
    }
    void S2C_StartPrestigeTask(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.StartPrestigeTask, null);
        NetPacket.S2C_StartPrestigeTask msg = MessageBridge.Instance.S2C_StartPrestigeTask(ios);
        Fresh();
    }


    void BtnEvt_FinishTask(bool useDiamond)//完成任务
    {
        if (useDiamond && PlayerPrefsBridge.Instance.PlayerData.Diamond < GameConstUtils.UseDiamondNum
            &&UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_ZHUAN_SHI_BU_ZU))
        {
            return;
        }
        System.Action okDel = delegate()
        {
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.FinishPrestigeTask, S2C_FinishPrestigeTask);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_FinishPrestigeTask(useDiamond));
            UIRootMgr.Instance.IsLoading = true;
        };
        if (useDiamond == true)//若使用钻石，强制提示
        {
            UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel("确定花费1仙玉加速吗?", okDel);
        }
        else
        {
            okDel();
        }
    }
    void S2C_FinishPrestigeTask(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_FinishPrestigeTask msg = MessageBridge.Instance.S2C_FinishPrestigeTask(ios);
        PrestigeTask task = PrestigeTask.PrestigeTaskFetcher.GetPrestigeTaskByCopy(msg.FinishTask);
        if (task != null)
        {
            GoodsToDrop[] goodsList = new GoodsToDrop[2];
            goodsList[0] = GoodsToDrop.CreateWealth(WealthType.Gold, GameUtils.getPrestigeMoneyReward(task.Money , PlayerPrefsBridge.Instance.PlayerData.Level));
            goodsList[1] = GoodsToDrop.CreatePrestige(task.Type, task.Reward);
            LobbySceneMainUIMgr.Instance.ShowDropInfo(goodsList, "你完成了声望任务");
        }
        Fresh();
    }
}
