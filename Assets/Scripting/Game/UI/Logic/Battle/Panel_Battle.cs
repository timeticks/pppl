using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Battle : MonoBehaviour
{
    public static Panel_Battle Instance;
    #region View

    public class ViewObj
    {
        public Transform RootMonster;
        public Image ImageMonster;
        public UIAnimationBaseCtrl AnimationAtk;
        public UIAnimationBaseCtrl AnimationHurt;
        public NumScrollTool MonsterHpScrol;
        public NumScrollTool MyHpScrollTool;
        public NumScrollTool MySkillScrollTool;
        public Text RoundText;
        public GameObject Part_DmgText;
        public Transform RootMonsterDmg;
        public Transform RootSelfDmg;
        public TextScroller BattleDescScroll;
        public Transform BattleTextRoot;
        public TextScroller DropDescScroll;

        public ViewObj(UIViewBase view)
        {
            if (RootMonster == null) RootMonster = view.GetCommon<Transform>("RootMonster");
            if (ImageMonster == null) ImageMonster = view.GetCommon<Image>("ImageMonster");
            if (AnimationAtk == null) AnimationAtk = view.GetCommon<UIAnimationBaseCtrl>("AnimationAtk");
            if (AnimationHurt == null) AnimationHurt = view.GetCommon<UIAnimationBaseCtrl>("AnimationHurt");
            if (MonsterHpScrol == null) MonsterHpScrol = view.GetCommon<NumScrollTool>("MonsterHpScrol");
            if (MyHpScrollTool == null) MyHpScrollTool = view.GetCommon<NumScrollTool>("MyHpScrollTool");
            if (MySkillScrollTool == null) MySkillScrollTool = view.GetCommon<NumScrollTool>("MySkillScrollTool");
            if (RoundText == null) RoundText = view.GetCommon<Text>("RoundText");
            if (Part_DmgText == null) Part_DmgText = view.GetCommon<GameObject>("Part_DmgText");
            if (RootMonsterDmg == null) RootMonsterDmg = view.GetCommon<Transform>("RootMonsterDmg");
            if (RootSelfDmg == null) RootSelfDmg = view.GetCommon<Transform>("RootSelfDmg");
            if (DropDescScroll == null) DropDescScroll = view.GetCommon<TextScroller>("DropDescScroll");
            if (BattleDescScroll == null) BattleDescScroll = view.GetCommon<TextScroller>("BattleDescScroll");
            if (BattleTextRoot == null) BattleTextRoot = view.GetCommon<Transform>("BattleTextRoot");

        }
    }


    private ViewObj mViewObj;
    #endregion

    private List<Text> selfDmgList = new List<Text>();
    private List<Text> monsterDmgList = new List<Text>();

    private PVEShowType curShowType = PVEShowType.None;
    private object[] curShowArg;
   
    public void Init(int mapId)
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        Instance = this;
    }

    void NewPVE()
    {
        TDebug.Log("NewPVE");
        if (PVEJob.Instance!=null && PVEJob.Instance.curPVEStatus != PVEJob.PVEStatus.End)
        {
            TDebug.LogError("有战斗正在进行");
            return;
        }
        //TextAsset t = Resources.Load("PVEBot") as TextAsset;
        //PVEBot bot = LitJson.JsonMapper.ToObject<PVEBot>(t.text);
        //PVEHero challenger = PVEBot.GetPVEHero(bot.challengerHeroIdx, bot.challengerLevel, bot.challengerNormalSpell, bot.challengerAttackSpells,
        //    bot.challengerEquipList);
        //PVEHero defier = PVEBot.GetPVEHero(bot.defierHeroIdx, bot.defierLevel, bot.defierNormalSpell, bot.defierAttackSpells,
        //    bot.defierEquipList);
        //PVEApp p = new PVEApp(challenger, defier, true);
    }

    void Fresh()
    {
        mViewObj.MyHpScrollTool.Fresh((float) PVEJob.Instance.challenger.curHp/PVEJob.Instance.challenger.GetMaxHp(),
            string.Format("{0}/{1}", PVEJob.Instance.challenger.curHp, PVEJob.Instance.challenger.GetMaxHp()), "我方");

        string monsterName = string.Format("{0}(lv:{1})" ,PVEJob.Instance.defier.getHeroName(), PVEJob.Instance.defier.GetLevel());
        mViewObj.MonsterHpScrol.Fresh((float)PVEJob.Instance.defier.curHp / PVEJob.Instance.defier.GetMaxHp(),
            string.Format("{0}/{1}", PVEJob.Instance.defier.curHp, PVEJob.Instance.defier.GetMaxHp()), monsterName);
    }
    public void GetDamage(DmgInfo dmg)
    {
        if (dmg == null) return;
        DmgResultType resultTy = resultTy = dmg.ResultType;
        Text curText = null;
        if (dmg.DefierIsSelf)
            curText = TAppUtility.Instance.GetEnableObj<Text>(selfDmgList, mViewObj.Part_DmgText, mViewObj.RootSelfDmg);
        else
            curText = TAppUtility.Instance.GetEnableObj<Text>(monsterDmgList, mViewObj.Part_DmgText, mViewObj.RootMonsterDmg);
        curText.gameObject.SetActive(true);
        int dmgAmount = dmg.Num;
        if (resultTy < DmgResultType.HpDmgMax) //如果是伤害
        {
            curText.transform.localPosition = new Vector3(0, 0, 0);
            curText.GetComponent<UIAnimationBaseCtrl>().DoSelfAnimation();
            if (dmg.ResultType == DmgResultType.HpDmgBlock)
            {
                curText.color = Color.blue;
                curText.gameObject.SetActive(false);
            }
            else if (dmg.ResultType == DmgResultType.HpDmgCrit)
            {
                curText.color = Color.red;
                curText.text = "-" + dmgAmount;
                //TDebug.Log(string.Format("{0}伤害:{1}", RoleUid, dmgAmount));
            }
            else
            {
                curText.color = Color.yellow;
                curText.text = "-" + dmgAmount;
            }
        }
        else if (resultTy == DmgResultType.MpDmgNormal)
        {
        }
        else if (resultTy == DmgResultType.HpCureNormal) //如果是治疗
        {
            curText.transform.localPosition = new Vector3(0, 0, 0);
            curText.color = Color.blue;
            curText.text = "+" + Mathf.Abs(dmgAmount);
            curText.GetComponent<UIAnimationBaseCtrl>().DoSelfAnimation();
            //TDebug.Log(string.Format("{0}恢复:{1}", RoleUid, dmgAmount));
        }
    }


    void Update()
    {
        //if (Time.frameCount%5==0)
        {
            while (curRemainList.Count > 0)
            {
                mViewObj.BattleDescScroll.AddNewTextLine(curRemainList[0]);
                mViewObj.BattleDescScroll.MoveBottom();
                curRemainList.RemoveAt(0);
            }
            while (curDropRemainList.Count > 0)
            {
                mViewObj.DropDescScroll.AddNewTextLine(curDropRemainList[0]);
                mViewObj.DropDescScroll.MoveBottom();
                curDropRemainList.RemoveAt(0);
            }
        }
    }

    private List<string> curRemainList= new List<string>();
    private List<string> curDropRemainList = new List<string>();
    public void AppendDesc(string str , bool isDrop=false)
    {
        if (isDrop)
            curDropRemainList.Add(str);
        else
            curRemainList.Add(str);
    }


    public void SetShowInfo(PVEShowType showType, params object[] arg)//设置显示
    {
        //mCurText += string.Format("{0}|{1}", showType, arg.Length > 0 ? arg[0] : 0);
        //TDebug.Log(string.Format("{0}|{1}", showType,arg.Length>0?arg[0]:0));
        curShowType = showType;
        curShowArg = arg;
        Show(curShowType, curShowArg);
    }
    void Show(PVEShowType showType, params object[] arg)
    {
        //if (PVEJob.thread.ThreadState == System.Threading.ThreadState.Running)
        //{
        //    TDebug.LogError("Running");
        //}
        switch (showType)
        {
            case PVEShowType.PVEStart:
            {
                mViewObj.BattleDescScroll.AddNewTextLine("新的战斗", true);
                PVEJob.ResumeThread();
                break;
            }
            case PVEShowType.NextRound:
            {
                StopAllCoroutines();
                StartCoroutine(NextRoundCor(arg));
                break;
            }
            case PVEShowType.CastSkill:
            {
                StopAllCoroutines();
                StartCoroutine(CastSkillCor(arg));
                break;
            }
            case PVEShowType.GetDmg:
            {
                StopAllCoroutines();
                StartCoroutine(GetDmgCor(arg));
                break;
            }
            case PVEShowType.PVEEnd:
            {
                StopAllCoroutines();
                StartCoroutine(PVEEndCor(arg));
                break;
            }
        }
    }

    IEnumerator NextRoundCor(object[] arg)
    {
        mViewObj.RoundText.text = string.Format("回合：{0}", (int)arg[0]);
        yield return new WaitForSeconds(1f);
        PVEJob.ResumeThread();
    }
    IEnumerator CastSkillCor(object[] arg)
    {
        int skillId = (int)arg[0];
        bool isSelf = (bool)arg[1];
        if (PVEJob.ShowAnimation)
        {
            Spell skill = Spell.Fetcher.GetSpellCopy(skillId);
            if (skill != null)
            {
                if (skill.targetType == Spell.TargetType.Enemy)
                {
                    if (!isSelf)//如果不是自己，播放怪物攻击
                    {
                        mViewObj.AnimationAtk.DoSelfAnimation();
                    }

                }
            }
        }
        
        yield return new WaitForSeconds(0.2f);
        PVEJob.ResumeThread();
    }
    IEnumerator GetDmgCor(object[] arg)
    {
        DmgInfo dmg = arg[0]as DmgInfo;
        float waitTime = (float)arg[1];
        if (PVEJob.ShowAnimation)
            GetDamage(dmg);
        Fresh();
        yield return new WaitForSeconds(0.2f);
        PVEJob.ResumeThread();
    }

    IEnumerator PVEEndCor(object[] arg)
    {
        yield return new WaitForSeconds(3f);
        PVEJob.ResumeThread();
    }


    void OnApplicationQuit()
    {
        TDebug.Log("OnApplicationQuit");
        if (PVEJob.Instance != null)
            PVEJob.AbortThread();
    }
    void OnDestroy()
    {
#if UNITY_EDITOR
        TDebug.Log("OnDestroy");
        if (PVEJob.Instance != null)
            PVEJob.AbortThread();
#endif
    }
}


public enum PVEShowType
{
    None,
    PVEStart,
    NextRound,
    CastSkill,
    GetDmg,
    PVEEnd,
}

public static class PVEShowTime
{
    public const float timePVEStart=1;
    public const float timeNextRound=1;
    public const float timeCastSkill=1;
    public const float timeGetDmg=0.5f;
    public const float timePVEEnd=1;

}