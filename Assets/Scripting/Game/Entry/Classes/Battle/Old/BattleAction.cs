using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction 
{
    public Spell attackerSpell;
    public Spell defenderSpell;

    private Part_BattleRole   challenger  = null;
    private Part_BattleRole   defier      = null;

    public int CurRound;
    public bool IsOver;
    public bool IsWaitRecord;

    public BattleAction()
    {
    }


    public void Start(Part_BattleRole my, Part_BattleRole enemy)
    {
        CurRound = 0;
        challenger = my;
        defier = enemy;
        Doing();
    }

    public void Doing()
    {
        IsWaitRecord = false;
        if (Window_BattleTowSide.Instance.CurRecord.ActionList.Count == 0)
        {
            //Window_Battle.Instance.MyBattleData.BattleType = BattleType.My_Hand_Fight;
            if (CurRound +1>= GameConstUtils.max_battle_round)
            {
                TDebug.LogError("到达最大回合数");
            }
            TDebug.Log(string.Format("记录的行动列表为0，当前回合{0}", CurRound));
            IsWaitRecord = true;
            if (Window_BattleTowSide.Instance.CurRecord.MyBattleType.IsHand())
            {
                Window_BattleTowSide.Instance.WaitHand();
            }
            return;
        }
        RecordActionStr record = Window_BattleTowSide.Instance.CurRecord.ActionList[0];
        if (record.Type == PVELoggerType.RoundStart)
        {
            RemoveLastAction();
            int round = record.GetValueTryInt(BattleDescType.num);
            CurRound = round;
           // int round = roundObj == null ? 0 : (int.Parse(roundObj.ToString()));
            BattleMgr.Instance.BattleWindow.AppendDecs(record.ToStr());
            //challenger.MyData.RoundFresh();
            //challenger.FreshBuffItem();
            //defier.MyData.RoundFresh();
            //defier.FreshBuffItem();
        }
        NextShow();
    }

    void FreshHp()
    {
        if (mLastHpAction == null) return;
        RecordActionStr record = mLastHpAction;
        if (record.ChallengerHp.HasValue)
        {
            if (record.ChallengerHp.Value != challenger.MyData.MyHero.CurHp)
                TDebug.LogError(string.Format("血量不一致{0}:{1}|{2}", record.ChallengerHp.Value, challenger.MyData.MyHero.CurHp,record.ToStr()));
            challenger.MyData.MyHero.CurHp = record.ChallengerHp.Value;
        }
        if (record.ChallengerMp.HasValue)
        {
            challenger.MyData.MyHero.CurMp = record.ChallengerMp.Value;
        }
        if (record.DefierHp.HasValue)
        {
            if (record.DefierHp.Value != defier.MyData.MyHero.CurHp)
                TDebug.LogError(string.Format("血量不一致{0}:{1}|{2}", record.DefierHp.Value, defier.MyData.MyHero.CurHp, record.ToStr()));
            defier.MyData.MyHero.CurHp = record.DefierHp.Value;
        }
        if (record.DefierMp.HasValue)
        {
            defier.MyData.MyHero.CurMp = record.DefierMp.Value;
        }
        mLastHpAction = null;
    }

    private RecordActionStr mLastHpAction;
    public void RemoveLastAction()
    {
        if (Window_BattleTowSide.Instance.CurRecord.ActionList.Count < 1)
        {
            TDebug.LogError("回合数为0");
            return;
        }
        if (Window_BattleTowSide.Instance.CurRecord.ActionList[0].ChallengerHp.HasValue)
            mLastHpAction = Window_BattleTowSide.Instance.CurRecord.ActionList[0];
        //if (mLastHpAction.ChallengerHp.HasValue)
        //    TDebug.Log("HasValue");
        Window_BattleTowSide.Instance.CurRecord.ActionList.RemoveAt(0);
    }


    private void NextShow()
    {
        //强制刷新血量
        if (mLastHpAction != null) FreshHp();
        if (Window_BattleTowSide.Instance.CurRecord.ActionList.Count > 0)
        {
            RecordActionStr record = Window_BattleTowSide.Instance.CurRecord.ActionList[0];

            if (record.Type == PVELoggerType.RoundStart)
            {
                Doing();
            }
            else if (record.Type == PVELoggerType.DotDoing || record.Type == PVELoggerType.HotDoing)
            {
                RemoveLastAction();
                object selfName = record.GetValue(BattleDescType.self);
                if (selfName == null) selfName = record.GetValue(BattleDescType.aim);//如果self=null，则释放者和受击者相同
                Part_BattleRole hero = Window_BattleTowSide.Instance.GetBattleRole((string)selfName);
                if (hero != null)
                {
                    hero.OnStateBy(record, NextShow);
                }
            }
            else if (record.Type == PVELoggerType.DoAttackSpell
                     || record.Type == PVELoggerType.DoHealSpell
                     || record.Type == PVELoggerType.DoStateSpell
                     || record.Type == PVELoggerType.DoAttackSubSpell
                     || record.Type == PVELoggerType.DoHealSubSpell
                     || record.Type == PVELoggerType.DoSignstSpell
                     || record.Type == PVELoggerType.DoSignstSubSpell
                     || record.Type == PVELoggerType.DoStateSubSpell
                     || record.Type == PVELoggerType.DoLifeRemoval
                    || record.Type == PVELoggerType.DoLifeRemovalSub)
            {
                RemoveLastAction();
                object selfName = record.GetValue(BattleDescType.self);
                if (selfName == null) selfName = record.GetValue(BattleDescType.aim); //如果self没有值，则self=aim
                Part_BattleRole hero = Window_BattleTowSide.Instance.GetBattleRole((string)selfName);
                if (hero != null)
                {
                    hero.DoAttack(record, NextShow);
                }
                else
                {
                    TDebug.LogError("没有找到此角色{0}" + selfName);
                }
            }
            else if (record.Type == PVELoggerType.DoIdle)
            {
                RemoveLastAction();
                Window_BattleTowSide.Instance.AppendDecs(record.ToStr());
                NextShow();
            }
            else if (record.Type == PVELoggerType.EndDead || record.Type == PVELoggerType.EndFail)
            {
                End();
            }
            else
            {
                RemoveLastAction();
                TDebug.LogError("未处理的类型" + record.Type);
                Window_BattleTowSide.Instance.AppendDecs(record.ToStr());
                NextShow();
            }
        }
        else
        {
            Doing();
        }
    }


    public void End()
    {
        RecordActionStr record = Window_BattleTowSide.Instance.CurRecord.ActionList[0];
        FreshHp();
        BattleResultType resultTy = BattleResultType.Fail;

        if ( Window_BattleTowSide.Instance.CurRecord.Winnder.Value)
        {
            Window_BattleTowSide.Instance.AppendDecs(record.ToStr());
            Window_BattleTowSide.Instance.AppendNewLineDecs(string.Format("\n战斗胜利"));
            resultTy = BattleResultType.Win;
        }
        else
        {
            Window_BattleTowSide.Instance.AppendDecs(record.ToStr());
            Window_BattleTowSide.Instance.AppendNewLineDecs(string.Format("\n战斗失败"));
            resultTy = BattleResultType.Fail;
        }
        if (defier.MyData.curHp > 0 && challenger.MyData.curHp>0)
        {
            TDebug.Log(string.Format("结束时信息不匹配 [myHp:{0}]  [enemyHp:{1}]  回合{2}", challenger.MyData.curHp,
                defier.MyData.curHp , CurRound));
        }
        IsOver = true;

        Window_BattleTowSide.Instance.ShowBattleEnd(resultTy);
    }

}
