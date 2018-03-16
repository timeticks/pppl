using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务管理器
/// </summary>
public class JobMgr
{
    public static JobMgr Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new JobMgr();
            return mInstance;
        }
    }
    private static JobMgr mInstance;

    private List<TimerJob> mTimerJobList = new List<TimerJob>();

    public void AddTimerJob(TimerJob job)
    {
        if (job == null) return;
        mTimerJobList.Add(job);
    }

    public void ClearAllJob()
    {
        mTimerJobList.Clear();
    }

    public void Update(float detlaTime)
    {
        int count = mTimerJobList.Count;
        for (int i = 0; i < count; ) //不默认进行i++
        {
            if (mTimerJobList[i].IsStopped)
            {
                mTimerJobList.RemoveAt(i);
                continue;
            }
            mTimerJobList[i].Update(detlaTime);
            i++;
        }
    }
}


/// <summary>
/// 时间任务，LoopMax=0也会执行一次
/// </summary>
public class TimerJob
{
    public float Delay { get; private set; }

    public int LoopMax { get; private set; }         //为-1则无限
    public float LoopInterval { get; private set; }  //循环间隔
    public bool IsStopped { get; private set; }

    private int mCurLoopNum;
    private float mCurInterval;
    private float mCurDelay;
    private System.Action<TimerJob> mRunDel;

    public TimerJob(float interval, int loopMax)
    {
        LoopInterval = interval;
        LoopMax = loopMax;
    }

    public TimerJob(float interval, int loopMax, float delay)
    {
        LoopInterval = interval;
        LoopMax = loopMax;
        Delay = delay;
    }
    public TimerJob(float interval, int loopMax, System.Action<TimerJob> runDel, float delay)
    {
        LoopInterval = interval;
        LoopMax = loopMax;
        Delay = delay;
        mRunDel = runDel;
    }

    public void Update(float detlaTime)
    {
        if (IsStopped) return;
        if (mCurDelay < Delay)
        {
            mCurDelay += detlaTime;
            return;
        }
        mCurInterval += detlaTime;
        if (mCurInterval > LoopInterval)
        {
            //执行
            if (mRunDel != null) mRunDel(this);
            Run();
            mCurLoopNum++;
            mCurInterval = 0;
            if (LoopMax != -1 && mCurLoopNum >= LoopMax)
            {
                IsStopped = true;
            }
        }
    }

    protected virtual void Run()
    {

    }

}


