using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 时间计时器
/// </summary>
public static class AppTimer
{
    /// <summary>
    /// 当前时间，毫秒级
    /// </summary>
    public static long CurTimeStampMsSecond { get { return StartTimeStampSecond + (long)Time.realtimeSinceStartup*1000 - StampStartDetla; } }
    public static int CurTimeStampSecond { get { return (int)((StartTimeStampSecond + (long)Time.realtimeSinceStartup * 1000 - StampStartDetla)/1000); } }

    static long StartTimeStampSecond;
    static long StampStartDetla = 0;
    static Dictionary<TimeCountEnum, long> TimePassDict = new Dictionary<TimeCountEnum, long>();                   //经过的时间
    static Dictionary<TimeCountEnum, AppTimeDownItem> TimeDownDict = new Dictionary<TimeCountEnum, AppTimeDownItem>(); //倒计时
    public static long OriginTime = 1494345600000;//开服时间 2017/5/10 00:00:00
    public class AppTimeDownItem//倒计时项
    {
        public Efloat m_StartTime;
        public Efloat m_Val;            //倒计时值
        public AppTimeDownItem() { }
        public AppTimeDownItem(float start, float val) { m_StartTime = start; m_Val = val; }
    }


    public static void SetCurStamp(long stampSecond)
    {
        StartTimeStampSecond = stampSecond;
        StampStartDetla = (long)(Time.realtimeSinceStartup*1000);
    }

    /// <summary>
    /// 得到倒计时，最小为0
    /// </summary>
    public static bool GetTimeDown(TimeCountEnum keyId, out float timedown)
    {
        if (TimeDownDict.ContainsKey(keyId)) //最小为0
        {
            timedown = Mathf.Max(0, TimeDownDict[keyId].m_Val - CurTimeStampMsSecond + TimeDownDict[keyId].m_StartTime);
            return true;
        }
        timedown = 1;
        return false;//如不存在，永远不为0
    }

    /// <summary>
    /// 添加倒计时
    /// </summary>
    public static void AddTimeDown(TimeCountEnum keyId, float val)
    {
        if (TimeDownDict.ContainsKey(keyId))
        {
            TimeDownDict[keyId] = new AppTimeDownItem(CurTimeStampMsSecond, val);
        }
        else { TimeDownDict.Add(keyId, new AppTimeDownItem(CurTimeStampMsSecond, val)); }
    }

    /// <summary>
    /// 移除
    /// </summary>
    public static void RemoveTimeDown(TimeCountEnum keyId)
    {
        if (TimeDownDict.ContainsKey(keyId)) { TimeDownDict.Remove(keyId); }
    }

    /// <summary>
    /// 得到经过的时间(毫秒)
    /// </summary>
    public static long GetTimePass(TimeCountEnum key)
    {
        if (TimePassDict.ContainsKey(key))
        {
            ///TODO :强行纠正 时间差GetTimePass（）
            if (CurTimeStampMsSecond < TimePassDict[key])
                TimePassDict[key] -= (TimePassDict[key] - CurTimeStampMsSecond);
            long diff = CurTimeStampMsSecond - TimePassDict[key];
            return diff;
        }
        return 0;//如不存在，永远为0
    }

    /// <summary>
    /// 添加计时器(传入开始的时间点)
    /// </summary>
    public static void AddTimePass(TimeCountEnum key, long startTime)
    {
        if (TimePassDict.ContainsKey(key)) { TimePassDict[key] = startTime; }
        else { TimePassDict.Add(key, startTime); }
    }

    /// <summary>
    /// 移除
    /// </summary>
    public static void RemoveTimePass(TimeCountEnum key)
    {
        if (TimePassDict.ContainsKey(key)) { TimePassDict.Remove(key); }
    }


}

public enum TimeCountEnum
{
    None,
    Travel,
}