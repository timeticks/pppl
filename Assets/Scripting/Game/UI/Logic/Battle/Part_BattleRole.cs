using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Part_BattleRole :MonoBehaviour{

    #region View

    public class ViewObj
    {
        public Text NameText;
        public NumScrollTool HpScrollTool;
        public NumScrollTool MpScrollTool;
        public RawImage RoleIcon;
        public Image RoleHeadIcon;
        public Transform BuffRoot;
        public Transform RoleRoot;
        public GameObject Part_DmgText;
        public GameObject Part_BattleRoleBuff;
        public List<Transform> PetList;
        public Transform SkillTextRoot;
        public GameObject Part_BattleSkillText;

        public ViewObj(UIViewBase view)
        {

            NameText = view.GetCommon<Text>("NameText");
            HpScrollTool = view.GetCommon<NumScrollTool>("HpScrollTool");
            MpScrollTool = view.GetCommon<NumScrollTool>("MpScrollTool");
            RoleIcon = view.GetCommon<RawImage>("RoleIcon");
            RoleHeadIcon = view.GetCommon<Image>("RoleHeadIcon");
            BuffRoot = view.GetCommon<Transform>("BuffRoot");
            RoleRoot = view.GetCommon<Transform>("RoleRoot");
            Part_DmgText = view.GetCommon<GameObject>("Part_DmgText");
            Part_BattleRoleBuff = view.GetCommon<GameObject>("Part_BattleRoleBuff");
            PetList = new List<Transform>();
            for (int i = 0; i < (int)Pet.PetTypeEnum.Max; i++)
            {
                PetList.Add(view.GetCommon<Transform>("Pet" + i));
            }
            if (SkillTextRoot == null) SkillTextRoot = view.GetCommon<Transform>("SkillTextRoot");
            if (Part_BattleSkillText == null) Part_BattleSkillText = view.GetCommon<GameObject>("Part_BattleSkillText");

        }
    }
    public class BuffItemObj : SmallViewObj
    {
        public Image Bg;
        public Text DurationText;
        public Text NameText;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (Bg == null) Bg = view.GetCommon<Image>("Bg");
            if (DurationText == null) DurationText = view.GetCommon<Text>("DurationText");
            if (NameText == null) NameText = view.GetCommon<Text>("NameText");
        }
    }

    public class PetItemObj
    {
        public UIViewBase MyView;
        public Text PetText;
        public Text PetLevelText;

        public PetItemObj(UIViewBase view)
        {
            MyView = view;
            if (PetText == null) PetText = view.GetCommon<Text>("PetText");
            if (PetLevelText == null) PetLevelText = view.GetCommon<Text>("PetLevelText");
        }
    }


    #endregion

    [HideInInspector]
    public Transform MyRoleObj;
    public int RoleUid;
    [HideInInspector]
    public PVEHero MyData;

    private ViewObj mView;
    private Window_BattleTowSide mBattleWindow;
    private System.Action mActionOverDel;
    private float mCurStateTime;                       //处于当前状态的时间
    private List<Text> mDmgTextList=new List<Text>();  //伤害数字
    private RoleAnimationData mCurAnimationAction;        //动画行为
    private List<BuffItemObj> mBuffItemList = new List<BuffItemObj>();
    private List<PetItemObj> mPetItemList = new List<PetItemObj>();
    private List<Text> mSkillItemList = new List<Text>();
    private List<Pet> mPetList = new List<Pet>();

    public void Init(Window_BattleTowSide win , PVEHero roleData ,int headIcon,List<Pet> petList , int roleUid)
    {
        mBattleWindow = win;
        mView = new ViewObj(GetComponent<UIViewBase>());

        MyData = roleData;
        RoleUid = roleUid;
        mPetList = petList;
        //mSkillCtrl = gameObject.CheckAddComponent<SkillStepCtrl>();
        //mSkillCtrl.Init(this);
        mView.RoleHeadIcon.sprite = win.HeadIconAtlas.GetSprite(HeadIcon.GetHeadIcon(headIcon , MyData.MyHero.MySex));
        if (headIcon > 0)
        {
            mView.RoleIcon.texture = win.GetAsset<Texture>(SharedAsset.Instance.LoadSpritePart<Texture>(HeadIcon.GetTexIcon(headIcon, MyData.MyHero.MySex)));
        }
        else
        {
            mView.RoleIcon.texture = win.GetAsset<Texture>(SharedAsset.Instance.LoadSpritePart<Texture>(roleData.MyHero.Icon));
        }
        
        FreshRoleView(MyData);
        FreshPet();
        FreshBuffItem();
        MyRoleObj = mView.RoleIcon.transform;
    }


    public void FreshRoleView(PVEHero roleData)
    {
        if (roleData == null)
        {
            mView.NameText.text = LangMgr.GetText("名字");
            mView.HpScrollTool.Fresh(1,
                string.Format("{0}/{1}", 100, 100),
                LangMgr.GetText("生命"));
            mView.MpScrollTool.Fresh(1,
                string.Format("{0}/{1}", 100, 100),
                LangMgr.GetText("灵力"));
        }
        else
        {
            mView.NameText.text = roleData.MyHero.name;
            mView.HpScrollTool.Fresh(roleData.curHp / (float)roleData.GetMaxHp(),
                string.Format("{0}/{1}", roleData.curHp, roleData.GetMaxHp()),
                LangMgr.GetText("生命"));
            mView.MpScrollTool.Fresh(roleData.curMp / (float)roleData.GetMaxMp(),
                string.Format("{0}/{1}", roleData.curMp, roleData.GetMaxMp()),
                LangMgr.GetText("灵力"));
            //mView.PetText.CrossFadeColor()
        }
    }

    public void FreshPet()
    {
        if (mPetItemList.Count == 0)
        {
            for (int i = 0; i < mView.PetList.Count; i++)
            {
                UIViewBase vi = mView.PetList[i].GetComponent<UIViewBase>();
                if (vi == null) TDebug.LogError("Pet没有绑定UIViewBase");
                PetItemObj item = new PetItemObj(vi);
                mPetItemList.Add(item);
            }
        }
        for (int i = 0; i < mPetItemList.Count; i++)
        {
            mPetItemList[i].PetLevelText.text = "无"; 
        }
        for (int i = 0; i < mPetList.Count; i++)
        {
            int pos = (int) mPetList[i].Type;
            mPetItemList[pos].MyView.gameObject.SetActive(true);
            mPetItemList[pos].PetLevelText.text = string.Format("{0}阶", TUtility.GetRankNumStr(mPetList[i].CurLevel / 4 + 1)); 
        }
    }

    public void FreshBuffItem()//刷新buff图标
    {
        mBuffItemList = TAppUtility.Instance.AddViewInstantiate<BuffItemObj>(mBuffItemList,
            mView.Part_BattleRoleBuff, mView.BuffRoot, MyData.buffList.Count);
        for (int i = 0; i < MyData.buffList.Count; i++)
        {
            mBuffItemList[i].View.gameObject.name = MyData.buffList[i].idx.ToString();
            //mBuffItemList[i].NameText.text = MyData.BuffList[i].EffectMisc.GetDesc();
            //mBuffItemList[i].DurationText.text = MyData.BuffList[i].curDuration.ToString();
        }
    }


    //技能名tips
    public void ShowSkillNameTips(string content, string colorString)
    {
        Text curText = TAppUtility.Instance.GetEnableObj<Text>(mSkillItemList, mView.Part_BattleSkillText, mView.SkillTextRoot);
        curText.gameObject.SetActive(false);
        curText.color = TUtility.Switch16ToColor(colorString);
        curText.gameObject.SetActive(true);
        curText.text = content;
    }

    #region 动画动作
    public void DoAnimation(string actionStr, Vector3? targetPos = null, System.Action doOver = null)
    {
        RoleAnimationData actData = new RoleAnimationData(actionStr);
        if (actData != null)
        {
            if (mCurAnimationAction == null) { mCurAnimationAction = actData; }
            else { TDebug.LogError("动作重叠了" + actionStr); }
        }
    }
    #endregion

    public void GetDamage(int atkerUid, DmgInfo dmg)
    {
        if (dmg == null) return;
        if (dmg.ResultType == DmgResultType.HpCureNormal)
        {
            MyData.curHp = Mathf.Clamp(MyData.curHp + dmg.Num, 0, MyData.GetMaxHp());
        }
        else
        {
            MyData.curHp = Mathf.Clamp(MyData.curHp - dmg.Num, 0, MyData.GetMaxHp());
        }
        int dmgAmount = dmg.Num;
        //TDebug.Log(string.Format("{0}当前血量:{1}", MyData.BaseHero.Name, MyData.CurHp));
        DmgResultType resultTy = resultTy = dmg.ResultType;

        Text curText = TAppUtility.Instance.GetEnableObj<Text>(mDmgTextList, mView.Part_DmgText, mView.RoleRoot);
        curText.gameObject.SetActive(true);
        if (resultTy < DmgResultType.HpDmgMax) //如果是伤害
        {
            curText.transform.localPosition = new Vector3(0, 150, 0);
            curText.GetComponent<UIAnimationBaseCtrl>().DoSelfAnimation();
            if (dmgAmount == 0)
            {
                curText.color = Color.blue;
                curText.gameObject.SetActive(false);
                ShowSkillNameTips("闪避", "7FD67F");
            }
            else
            {
                if (dmg.ResultType == DmgResultType.HpDmgBlock) { ShowSkillNameTips("招架", "7FA4D6"); }
                curText.color = Color.red;
                curText.text = "-" + dmgAmount;
                //TDebug.Log(string.Format("{0}伤害:{1}", RoleUid, dmgAmount));
            }
        }
        else if (resultTy == DmgResultType.MpDmgNormal)
        {
        }
        else if (resultTy == DmgResultType.HpCureNormal) //如果是治疗
        {
            curText.transform.localPosition = new Vector3(0, 250, 0);
            curText.color = Color.blue;
            curText.text = "+" + Mathf.Abs(dmgAmount);
            curText.GetComponent<UIAnimationBaseCtrl>().DoSelfAnimation();
            //TDebug.Log(string.Format("{0}恢复:{1}", RoleUid, dmgAmount));
        }
        else if (resultTy == DmgResultType.MpCureNormal)
        {

        }
        FreshRoleView(MyData);
    }

    //计算buff
    public DmgInfo CaculateBuff(RecordActionStr record)
    {
        int spellId = record.GetValueTryInt(BattleDescType.spell);
        //Skill spell = Skill.SkillFetcher.GetSpellByCopy(spellId);
        //if (spell == null) return null;

        int promValue = record.GetValueTryInt(BattleDescType.prom);
        if (promValue == 0) promValue = record.GetValueTryInt(BattleDescType.dmg);

        DmgInfo dmgInfo = null;
        //if (spell.EffectMisc == Skill.EffectMiscType.Recover || spell.EffectMisc == Skill.EffectMiscType.Rebirth)
        //{
        //    dmgInfo = new DmgInfo(0, DmgResultType.HpCureNormal, (int)promValue);
        //}
        //else
        //{
        //    dmgInfo = new DmgInfo(0, DmgResultType.HpDmgNormal, (int)promValue);
        //}
        //GetDamage(RoleUid, dmgInfo);

        return dmgInfo;
    }

    //计算并进行伤害，out反击伤害
    public DmgInfo CaculateDamage(RecordActionStr record, Part_BattleRole target ,out DmgInfo backDmg)
    {
        DmgInfo dmg = DmgInfo.GetByRecord(record, false); //如果是治疗或生命偷取，则返回不为空
        RecordActionStr nextAction = null;
        backDmg = null;
        if (dmg == null)
        {
            nextAction = Window_BattleTowSide.Instance.CurRecord.GetNext();
            dmg = DmgInfo.GetByRecord(nextAction, false);
            backDmg = DmgInfo.GetByRecord(nextAction, true);
        }
        if (dmg != null)
        {
            if (nextAction != null)
            {
                BattleMgr.Instance.BattleWindow.AppendDecs(nextAction.ToStr());
                Window_BattleTowSide.Instance.mAction.RemoveLastAction();
            }
            target.GetDamage(RoleUid, dmg);
        }
        if (backDmg != null)
        {
            GetDamage(target.RoleUid, backDmg);
        }
        return dmg;
    }

    //行动显示
    public IEnumerator DoAttackAni(Spell spell, RecordActionStr record ,System.Action overDeleg)
    {
        //Part_BattleRole target = Window_Battle.Instance.GetRoleUid(RoleUid, spell.TargetSelect);
        ////Part_BattleRole target = Window_Battle.Instance.GetTargetRole(record, this);
        
        ////调用攻击动作
        //if (spell.EffectType == Skill.EffectTypeEnum.PhyDamage || spell.EffectType == Skill.EffectTypeEnum.MagDamage)
        //{
        //    mCurAnimationAction = new RoleAnimationData("[atk,keep0.3,value200]");
        //    float atkTime = 0.3f;
        //    yield return new WaitForSeconds(atkTime*BattleMgr.PLAY_TIME_RATIO);
        //}

        //进行伤害
        //DmgInfo backDmg = null;
        //DmgInfo dmg = CaculateDamage(record, target, out backDmg);
        //if (dmg != null)
        //{
        //    float hurtWaitTime = 0.2f;
        //    yield return new WaitForSeconds(hurtWaitTime*BattleMgr.PLAY_TIME_RATIO);
        //}

        float overWaitTime = 0.8f;
        yield return new WaitForSeconds(overWaitTime * BattleMgr.PLAY_TIME_RATIO);

        overDeleg();//结束
    }

    public IEnumerator WaitAni(float waitTime, System.Action overDeleg)
    {
        yield return new WaitForSeconds(waitTime);
        if (overDeleg!=null) overDeleg();
    }

    public void DoAttack(RecordActionStr record, System.Action overDeleg)
    {
        Window_BattleTowSide.Instance.AppendDecs(record.ToStr());

        int spellId = record.GetValueTryInt(BattleDescType.spell);
        //Skill spell = Skill.SkillFetcher.GetSpellByCopy(spellId);
        //if (spell != null)
        //{
        //    MyData.CurMp = Mathf.Clamp(MyData.CurMp - spell.cost, 0, MyData.BattleMp);//扣蓝
            
        //    bool isWaitAni = false;
        //    if (record.Type.IsMainSpell()) //显示主技能文字
        //    {
        //        if (spell.skillTypSkillll.PosType.Def || spell.skillType ==SkillosType.Attribute ||  spell.skillType == SkiSkillpe.Passive)
        //        {
        //            ShowSkillNameTips(spell.name, "CFA972");
        //        }
        //        else{ ShowSkillNameTips(spell.name, "CFA972");}
        //        isWaitAni = true;
        //    }

        //    if (spell.EffectType != Skill.EffectTypeEnum.StateDamage && spell.EffectType != Skill.EffectTypeEnum.Signet)
        //    {
        //        //非状态技能，显示攻击动作
        //        StartCoroutine(DoAttackAni(spell, record, overDeleg));
        //    }
        //    else    //如果是状态技能
        //    {
        //        Part_BattleRole target = Window_Battle.Instance.GetTargetRole(record , this);
                
        //        //刷新buff
        //        if (record.Type == PVELoggerType.DoStateSpell || record.Type == PVELoggerType.DoStateSubSpell
        //            ||record.Type == PVELoggerType.DoSignstSpell || record.Type == PVELoggerType.DoSignstSubSpell)
        //        {
        //            //TDebug.Log(target.RoleUid + "增加buff:" + spell.Idx + "   ||" + record.ToStr());
        //            int duration = record.GetValueTryInt(BattleDescType.duration);
        //            target.MyData.AddBuff(spell.idx, duration);
        //            target.FreshBuffItem();
        //        }
                 
        //        if (isWaitAni)
        //        {
        //            StartCoroutine(WaitAni(0.9f * BattleMgr.PLAY_TIME_RATIO, overDeleg));
        //        }
        //        else
        //        {
        //            overDeleg();
        //        }
        //    }
        //}
        //else
        //{
        //    TDebug.LogError(string.Format("没有此技能{0}", spellId));
        //    overDeleg();
        //}
    }



    
    public void OnStateBy(RecordActionStr record, System.Action overDeleg)
    {
        if (record==null)
        {
            overDeleg();
            return;
        }
        string buffDesc = record.ToStr();
        Window_BattleTowSide.Instance.AppendDecs(buffDesc);  //显示状态描述
        DmgInfo dmg = CaculateBuff(record);

        float stateWaitTime = 0.3f;
        StartCoroutine(WaitAni(stateWaitTime, overDeleg));
    }

}


public enum RoleActionState : byte
{
    DoSkillAction,     //显示技能动作
    ShowDmg,           //显示伤害
    ShowDesc,          //显示描述
    ShowBackAtk,       //显示反击
}
