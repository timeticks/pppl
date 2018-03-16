using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PVEJob:MonoBehaviour
{
    public static PVEJob Instance
    {
        get
        {
            return mInstance;
        } 
    }
    private static PVEJob mInstance;

    public static Thread thread;
    public static AutoResetEvent resetEvent;
    public static bool ShowAnimation = false;   //是否播放动画
    private static bool isThread;

    public enum PVEStatus
    {
        None,
        Init,
        Doing,
        End,
        CanNext,
    }

    public PVEStatus curPVEStatus { get; private set; }
    public bool isSelfWin;//是否己方胜利


    public PVEHero challenger { get; private set; }
    public PVEHero defier { get; private set; }
    public int curRound { get; private set; }
    protected PVEAction action;

    void Awake()
    {
        mInstance = this;
    }

    public void Init(PVEHero chanlleger, PVEHero defier,bool isThread)
    {
        curPVEStatus = PVEStatus.Init;
        PVEJob.isThread = isThread;
        chanlleger.isSelf = true;
        if (false)
        {
            if (thread != null) thread.Abort(); 
            thread = null;
            resetEvent = new AutoResetEvent(false);
            thread = new Thread(new ThreadStart(()=>
            {
                try
                {
                    StartNewPVE(chanlleger, defier);
                }
                catch (Exception e)
                {
                    TDebug.LogError(e.Message);
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }
        else
        {
            StartNewPVE(chanlleger, defier);
        }
    }


    public static void SleepThread(PVEShowType showType ,params object[] arg)
    {
        //Panel_Battle.Instance.SetShowInfo(showType, arg);
        //resetEvent.WaitOne();
    }

    public static void ResumeThread()
    {
        //try
        //{
        //    if (thread.ThreadState == ThreadState.Running)
        //    {
        //        TDebug.LogError("没有被挂起");
        //    }
        //    resetEvent.Set();
        //}
        //catch (Exception e)
        //{
        //    TDebug.LogError(e.Message);
        //}
    }

    public static void AbortThread()
    {
        if (thread != null)
        {
            thread.Abort();
            thread = null;
        }
    }

    public void StartNewPVE(PVEHero chanlleger, PVEHero defier)
    {
        this.challenger = chanlleger;
        this.defier = defier;
        Panel_Battle.Instance.SetShowInfo(PVEShowType.PVEStart);
        BattleLog.Log("开始一场新的战斗，challengerName:{0}|level:{1},defier:{2}|level:{3}",
            challenger.getHeroName(), challenger.GetLevel(), defier.getHeroName(), defier.GetLevel());
        BattleLog.TextLog("{0}战斗开始   {0}VS{1}", challenger.getHeroName(), defier.getHeroName());
        curPVEStatus = PVEStatus.Doing;
        Begin(1);
    }

    public void Begin(int round)
    {
        BattleLog.Log("新的回合{0}=====[{1}]hp:{2} mp:{3}    [{4}]hp:{5} mp:{6}", round, challenger.getHeroName(), challenger.curHp,
             challenger.curMp,defier.getHeroName(), defier.curHp, defier.curMp);
        BattleLog.TextLog("第{0}回合", round );
        curRound = round;
        Panel_Battle.Instance.SetShowInfo(PVEShowType.NextRound, curRound);
        //判断先手 
        PVEHero first = challenger.GetAtt(AttrType.Speed) > defier.GetAtt(AttrType.Speed) ? challenger : defier;
        PVEHero last = challenger.GetAtt(AttrType.Speed) > defier.GetAtt(AttrType.Speed) ? defier : challenger;
        if (action == null) action = PVEMgr.Instance.gameObject.CheckAddComponent<PVEAction>();
        action.doing(first, last , curRound);
    }

    public bool isEnd()
    {
        if (!challenger.isLive() || !defier.isLive())
        {
            curPVEStatus = PVEStatus.End;
        }
        return curPVEStatus == PVEStatus.End;
    }

    public void end()
    {
        if (!challenger.isLive() || !defier.isLive() || curRound >=99)
        {
            if (!challenger.isLive() || !defier.isLive())
            {
                isSelfWin = challenger.isLive();
                curPVEStatus = PVEStatus.End;
                BattleLog.Log("某一方死亡，战斗结束，回合{0}。挑战方胜利:{1}，防御方胜利:{2}", curRound, !challenger.isLive(), !defier.isLive());
                if (isSelfWin)
                {
                    BattleLog.TextLog("战斗结束，你胜利了");
                }
                else
                {
                    BattleLog.TextLog("战斗结束，你失败了");
                }
            }
            else
            {
                isSelfWin = false;
                curPVEStatus = PVEStatus.End;
                BattleLog.TextLog("战斗超过最大回合数，战斗结束");
                BattleLog.Log("战斗超过最大回合数，战斗结束");
            }
            
            //进行掉落
            if (isSelfWin)
            {
                PVEMgr.Instance.AddDropMonster(defier);
            }
            Panel_Battle.Instance.SetShowInfo(PVEShowType.PVEEnd, isSelfWin);

            //进行下一场
            StopCoroutine("endCor");
            StartCoroutine("endCor");
            return;
        }
        
        curRound++;
        Begin(curRound);
    }

    IEnumerator endCor()
    {
        yield return new WaitForSeconds(PVEShowTime.timePVEEnd);
        curPVEStatus = PVEStatus.CanNext;
    }
}



public static class BattleLog
{
    public static void Log(string content, params object[] ss)
    {
        //TDebug.Log(string.Format(content, ss));
    }
    public static void TextLog(string content, params object[] ss)
    {
        Panel_Battle.Instance.AppendDesc(string.Format(content, ss));
        //TDebug.Log(string.Format(content, ss));
    }

    public static void WarnLog(string msg , string detail, params object[] ss)
    {
        TDebug.Log(msg + ": "+string.Format(detail, ss));
    }
}