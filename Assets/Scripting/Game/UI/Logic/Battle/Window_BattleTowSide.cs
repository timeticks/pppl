using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Window_BattleTowSide : WindowBase
{
    #region View

    public static Window_BattleTowSide Instance;
    public class ViewObj
    {
        public TextScrollView BattleDescScroll;
        public Transform MyPart_BattleRole;
        public Transform EnemyPart_BattleRole;
        public GameObject Part_BattleRoleBuff;
        public GameObject Part_DmgText;
        public Transform EndRoot;
        public Button EndBtn;
        public Button UpSpeedBtn;
        public GameObject Panel_BattleHand;
        public ViewObj(UIViewBase view)
        {
            BattleDescScroll = (TextScrollView)view.GetCommon<TextScrollView>("BattleDescScroll");
            MyPart_BattleRole = (Transform)view.GetCommon<Transform>("MyPart_BattleRole");
            EnemyPart_BattleRole = (Transform)view.GetCommon<Transform>("EnemyPart_BattleRole");
            Part_BattleRoleBuff = (GameObject)view.GetCommon<GameObject>("Part_BattleRoleBuff");
            Part_DmgText = (GameObject)view.GetCommon<GameObject>("Part_DmgText");
            EndRoot = (Transform)view.GetCommon<Transform>("EndRoot");
            EndBtn = (Button)view.GetCommon<Button>("EndBtn");
            UpSpeedBtn = (Button)view.GetCommon<Button>("UpSpeedBtn");
            if (Panel_BattleHand == null) Panel_BattleHand = view.GetCommon<GameObject>("Panel_BattleHand");        }
    }

    #endregion

    public Part_BattleRole MyRole;                                                                                 
    public Part_BattleRole EnemyRole;

    public bool IsPlaying;

    public BattleRecordStr CurRecord {
        get { return BattleMgr.Instance.CurRecord; }
    }

    public BattleData MyBattleData {
        get { return BattleMgr.Instance.BattleData; }
    }

    //头像
    public SpritePrefab HeadIconAtlas
    {
        get
        {
            if (mHeadIconAtlas == null)
            {
                GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("HeadIconAtlas");
                mHeadIconAtlas = go.GetComponent<SpritePrefab>();
            }
            return mHeadIconAtlas;
        }
    }
    private SpritePrefab mHeadIconAtlas;
    public BattleAction mAction;

    private ViewObj mViewObj;
    private BattleResultType mResultType;
    private System.Action<int> mOverCallback;//胜利返回1，失败返回0
    private Panel_BattleHand mPanel_Hand;
    public void OpenWindow(System.Action<int> overCallback)
    {
        Instance = this;
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        IsPlaying = true;
        mOverCallback = overCallback;
        mViewObj.Panel_BattleHand.gameObject.SetActive(false);
        Init();
    }


    private void Init()
    {
        Window_BattleTowSide.Instance.AppendDecs("战斗开始", true);

        BattleMgr.PLAY_TIME_RATIO = 1;
        mAction = null;
        InitBattle();
        mAction = new BattleAction();
        mAction.Start(MyRole, EnemyRole);
    }


    private void InitBattle()
    {
        List<Spell> skillList = new List<Spell>();
        PVEHero myRole = new PVEHero(PlayerPrefsBridge.Instance.GetHeroWithProperties(), PlayerPrefsBridge.Instance.PlayerData.Level, skillList);
        myRole.MyHero.name = CurRecord.ChallengerName;//PlayerPrefsBridge.Instance.PlayerData.Name;
        //myRole.GetMaxHp = CurRecord.ChallengerHp;
        //myRole.GetMaxMp = CurRecord.ChallengerMp;
        //myRole.ResetHpMp();
        //PVEHero enemyRole = new PVEHero(CurRecord.DefierIdx);
        //enemyRole.MyHero.name = CurRecord.DefierName;
        //enemyRole.GetMaxHp = CurRecord.DefierHp;
        //enemyRole.GetMaxMp = CurRecord.DefierMp;
        //enemyRole.ResetHpMp();

        //InitRole(myRole, enemyRole);
        mViewObj.EndRoot.gameObject.SetActive(false);
        mViewObj.EndBtn.SetOnClick(delegate() { CloseWindow(); });
        mViewObj.UpSpeedBtn.SetOnClick(UpSpeed);
    }

    private void InitRole(PVEHero myRole, PVEHero enemyRole)//初始化角色
    {
        MyRole = mViewObj.MyPart_BattleRole.gameObject.CheckAddComponent<Part_BattleRole>();
        MyRole.Init(this, myRole, CurRecord.ChallengerHeadIconIdx,CurRecord.ChallengerPets, 0);
        EnemyRole = mViewObj.EnemyPart_BattleRole.gameObject.CheckAddComponent<Part_BattleRole>();
        EnemyRole.Init(this, enemyRole, CurRecord.DefierHeadIconIdx,CurRecord.DefierPets, 1);
    }


    public void AppendDecs(string str, bool needClear = false)//战斗描述
    {
        //TDebug.Log(str);
        mViewObj.BattleDescScroll.AppendText(str.ToString(), needClear);
        mViewObj.BattleDescScroll.MoveBottom();
    }

    public void AppendNewLineDecs(string str, bool needClear = false)
    {
        StringBuilder s = new StringBuilder(str);
        if (!s.ToString().EndsWith("\r\n"))
        {
            s.Append("\r\n");
        }
        AppendDecs(s.ToString(), needClear);
    }



    public void StartBattleLog()
    {
        mViewObj.BattleDescScroll.gameObject.SetActive(true);
        if (mPanel_Hand != null)
        {
            mPanel_Hand.GetBattleLog();
        }
        if (mAction.IsWaitRecord)
        {
            mAction.Doing();
        }
    }

    public void WaitHand() //等待手动
    {
        //if (CurRecord.MyBattleType.IsHand())
        //{
        //    mViewObj.BattleDescScroll.gameObject.SetActive(false);
        //    mPanel_Hand = mViewObj.Panel_BattleHand.CheckAddComponent<Panel_BattleHand>();
        //    mPanel_Hand.Init(PlayerPrefsBridge.Instance.GetSpellBarInfo(), mAction.CurRound, this);
        //}
        //else { TDebug.LogError("不是手动战斗"); }
    }


    public Part_BattleRole GetBattleRole(int roleId)
    {
        if (roleId == MyRole.RoleUid) return MyRole;
        else return EnemyRole;
    }
    public Part_BattleRole GetBattleRole(string roleName)
    {
        if (roleName.Contains("无名少") || roleName.Contains("无名小辈") || roleName.Contains("你"))
        {
            TDebug.Log("这里先强制将特殊名字设为玩家自身");
            return MyRole;
        }
        if (roleName == MyRole.MyData.MyHero.name) return MyRole;
        else return EnemyRole;
    }
    public bool IsMyRole(int uid)
    {
        if (uid == MyRole.RoleUid) return true;
        else return false;
    }

    public string GetRoleName(Part_BattleRole role)
    {
        if (role.RoleUid == 0)
            return "你";
        else return role.MyData.MyHero.name;
    }

    //得到对应uid角色的己方或敌方角色
    //public Part_BattleRole GetRoleUid(int myUid, Skill.ObjectiveTeam getTeam)
    //{
    //    if (IsMyRole(myUid))
    //    {
    //        if (getTeam == Skill.ObjectiveTeam.Enemy) return EnemyRole;
    //        else return MyRole;
    //    }
    //    else
    //    {
    //        if (getTeam == Skill.ObjectiveTeam.Enemy) return MyRole;
    //        else return EnemyRole;
    //    }
    //}
    public Part_BattleRole GetTargetRole(RecordActionStr record, Part_BattleRole self)//得到目标
    {
        object targetName = record.GetValue(BattleDescType.aim);
        Part_BattleRole target = null;
        if (targetName != null)
        {
            if ("自己" == targetName.ToString()) target = self;
            else target = Window_BattleTowSide.Instance.GetBattleRole((string)targetName);
        }
        else
        {
            target = self;
        }
        return target;
    }


    public void ShowBattleEnd(BattleResultType resultTy)
    {
        mResultType = resultTy;
        if (!BattleMgr.Instance.CurRecord.Winnder.HasValue)
        {
            TDebug.LogError("没有收到战斗结果");
            BattleMgr.Instance.CurRecord.Winnder = false;
        }
        mResultType = (BattleMgr.Instance.CurRecord.Winnder.Value) ? BattleResultType.Win : BattleResultType.Fail;
        mViewObj.EndRoot.gameObject.SetActive(true);
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        IsPlaying = false;
        if (!BattleMgr.Instance.CurRecord.Winnder.HasValue)
        {
            TDebug.LogError("没有收到战斗结果");
            BattleMgr.Instance.CurRecord.Winnder = false;
        }
        mResultType = (BattleMgr.Instance.CurRecord.Winnder.Value) ? BattleResultType.Win : BattleResultType.Fail;
        BattleMgr.Instance.RemoveCurRecord();
        //if (UIRootMgr.LobbyUI!=null ) UIRootMgr.LobbyUI.Init();  //重回大厅
        base.CloseWindow(actionType);
        System.Action<int> callback = mOverCallback;
        mOverCallback = null;
        if (mResultType == BattleResultType.Win)
        {
            if (callback != null) callback(1);
        }
        else
        {
            if (callback != null) callback(0);
        }
        BattleMgr.Instance.BattleEnd();
        Instance = null;
    }
  

    public void UpSpeed()
    {
        if (BattleMgr.PLAY_TIME_SCALE == 1)
            BattleMgr.PLAY_TIME_SCALE = 6f;
        else
            BattleMgr.PLAY_TIME_SCALE = 1f;
        TDebug.Log(string.Format("{0}倍加速{1}", BattleMgr.PLAY_TIME_SCALE, gameObject.name));
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            UpSpeed();
        }
    }

}



