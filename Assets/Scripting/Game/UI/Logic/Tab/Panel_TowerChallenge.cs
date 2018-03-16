using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Panel_TowerChallenge : MonoBehaviour
{
    public class ViewObj
    {
        public Text TextTitleName;
        public RawImage TextureNpc;
        public Text TextNpcName;
        public Text TextNpcLevel;
        public Text TextNpcLimit;
        public Text TextNpcReward;
        public Button BtnChallenge;
        public Text TextBtnChallenge;
        public Text TextFailNum;
        public ViewObj(UIViewBase view)
        {

            if (TextTitleName == null) TextTitleName = view.GetCommon<Text>("TextTitleName");
            if (TextureNpc == null) TextureNpc = view.GetCommon<RawImage>("TextureNpc");
            if (TextNpcName == null) TextNpcName = view.GetCommon<Text>("TextNpcName");
            if (TextNpcLevel == null) TextNpcLevel = view.GetCommon<Text>("TextNpcLevel");
            if (TextNpcLimit == null) TextNpcLimit = view.GetCommon<Text>("TextNpcLimit");
            if (TextNpcReward == null) TextNpcReward = view.GetCommon<Text>("TextNpcReward");
            if (BtnChallenge == null) BtnChallenge = view.GetCommon<Button>("BtnChallenge");
            if (TextBtnChallenge == null) TextBtnChallenge = view.GetCommon<Text>("TextBtnChallenge");
            if (TextFailNum == null) TextFailNum = view.GetCommon<Text>("TextFailNum");        }
    }
    private ViewObj mViewObj;

    public class TaskItemObj : SmallViewObj
    {
        public Text TextName;
        public Button BtnStartTask;
        public Text TextBtnStartTask;
        public Text TextUseTime;
        public Text TextReward;        public override void Init(UIViewBase view)
        {
            base.Init(view);

            if (TextName == null) TextName = view.GetCommon<Text>("TextName");
            if (BtnStartTask == null) BtnStartTask = view.GetCommon<Button>("BtnStartTask");
            if (TextBtnStartTask == null) TextBtnStartTask = view.GetCommon<Text>("TextBtnStartTask");
            if (TextUseTime == null) TextUseTime = view.GetCommon<Text>("TextUseTime");
            if (TextReward == null) TextReward = view.GetCommon<Text>("TextReward");        }
    }
    private List<TaskItemObj> mMailItemList;

    private TaskItemObj mCurDoingTaskObj;
    private float mCurDownTime;   //完成倒计时
    private Window_Tower mParentWin;
    public void Init(Window_Tower win)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        gameObject.SetActive(true);
        mParentWin = win;
        Fresh();
    }
    public void CloseWindow()
    {
    }

    public void Fresh()
    {
        gameObject.SetActive(true);
        Tower tower = Tower.TowerFetcher.GetTowerByCopy(PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex+1);
        if (tower != null)
        {
            mViewObj.TextTitleName.text = tower.name;
            OldHero npc = OldHero.HeroFetcher.GetHeroByCopy(tower.Monster);
            HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(npc.Level);
            HeroLevelUp minLevelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(tower.Level);

            mViewObj.TextNpcName.text = string.Format("关卡守将\n{0}", npc.name);
            mViewObj.TextNpcLevel.text = string.Format("守将境界\n{0}", levelUp.name);
            mViewObj.TextNpcLimit.text = string.Format("挑战等级\n{0}", minLevelUp.name);
            mViewObj.TextNpcReward.text = string.Format("过关奖励\n{0}", Loot.GetGoodsListString(tower.ComReward , tower.SpeReward));
            mViewObj.TextureNpc.texture = mParentWin.GetAsset<Texture>(SharedAsset.Instance.LoadSpritePart<Texture>(npc.Icon));
        }
        mViewObj.BtnChallenge.SetOnClick(delegate() { BtnEvt_Challenge(); });
        mViewObj.TextFailNum.text = string.Format("可失败次数：{0}" , GameConstUtils.max_tower_fail_num - PlayerPrefsBridge.Instance.ActivityData.TowerFailNum);
    }


    void BtnEvt_Challenge()//开启爬塔
    {
        //异常拦截
        if (VipAddition.MAX_TOWER_FAIL_NUM.getValueByVip(PlayerPrefsBridge.Instance.PlayerData.IsVip()) - PlayerPrefsBridge.Instance.ActivityData.TowerFailNum <= 0
            && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.LOBBY_WARN_JIN_RI_CI_SHU_YONG_WAN))
        {
            return;
        }
         Tower tower = Tower.TowerFetcher.GetTowerByCopy(PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex+1);
         if (tower != null && tower.Level > PlayerPrefsBridge.Instance.PlayerData.Level
             && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_DENG_JI_BU_ZU))
        {
            return;
        }
        BattleMgr.Instance.EnterPVE(BattleType.My_Auto_Battle, 0, PVESceneType.Tower, BattleOver);
        UIRootMgr.Instance.IsLoading = true;
    }
    void BattleOver(int result)
    {
        Fresh();
    }
}
