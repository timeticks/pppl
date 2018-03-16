using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseAccessor : DescObject {

    public long ZazenStartTime;
    public int ZazenUseItemIdx;      //打坐所用丹药
    public long RetreatStartTime;
    public int RetreatUseItemIdx;  //闭关所用丹药
    public bool isSuffer;
    //====================打坐消息缓存-=======================

    public int ExpOffLine;
    public int PotentialOffLine;

    public ExerciseAccessor() { }
    public  ExerciseAccessor(ExerciseAccessor origin) 
    {
        ZazenStartTime = origin.ZazenStartTime;
        ZazenUseItemIdx = origin.ZazenUseItemIdx;
        RetreatStartTime = origin.RetreatStartTime;
        RetreatUseItemIdx = origin.RetreatUseItemIdx;
        ExpOffLine = origin.ExpOffLine;
        PotentialOffLine = origin.PotentialOffLine;
    }
    public void InitOffLineInfo(NetPacket.S2C_SnapshotOffLine msg)
    {
        this.ExpOffLine = msg.Exp;
        this.PotentialOffLine = msg.Potential;
    }
}
