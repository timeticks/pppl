using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
public class BattleMgr
{
    public static float PLAY_TIME_RATIO;     //播放速度
    public static float PLAY_TIME_SCALE {
        get { return Time.timeScale; }
        set { Time.timeScale = value; }
    }

    private static BattleMgr mInstance;
    public static BattleMgr Instance {
        get {
            if (mInstance == null) mInstance = new BattleMgr();
            return mInstance;
        }
    }

    public BattleData BattleData;
    public Window_BattleTowSide BattleWindow;
    private System.Action<int> mOverCallback;

    private List<BattleRecordStr> RecordList = new List<BattleRecordStr>();
    public BattleRecordStr CurRecord {
        get { if (RecordList.Count > 0) return RecordList[RecordList.Count-1]; return null; }
    }

    private BattleMgr()
    {
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.EnterPVE, S2C_EnterPVE);  //以便可以被服务器主动拉进战斗，如在闭关时
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.BattleLog, S2C_BattleLog);
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.BattleReport, S2C_BattleReport);
    }

    public void Init()
    { }

    public void EnterPVE(BattleType battleType , int enemyIdx ,PVESceneType sceneType, Action<int> callback =null)
    {
        TDebug.Log("EnterPVE进入战斗");
        BattleData = BattleData.GetTest();
        BattleData.BattleType = battleType;
        mOverCallback = callback;
        //AppEvtMgr.Instance.m_ActMgr.DoActByEvt(new EvtItemData(EvtType.EnterAnyArea), new DoActData(DoActType.OpenWin, WinName.Window_ExploreStage.ToString(), objs));

        TDebug.Log("发送SendEnterPVE");
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_EnterPVE(battleType, enemyIdx, sceneType));
    }

    public void SetPveEndCallback(Action<int> callback)  //注册战斗展示完成后的回调
    {
        mOverCallback = callback;
    }

    public void S2C_EnterPVE(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        BattleWindow = UIRootMgr.Instance.GetOpenListWindow(WinName.Window_Battle)as Window_BattleTowSide;
        if (BattleWindow==null) BattleWindow = UIRootMgr.Instance.OpenWindow<Window_BattleTowSide>(WinName.Window_Battle, CloseUIEvent.None);
        NetPacket.S2C_EnterPVE msg = MessageBridge.Instance.S2C_EnterPVE(ios);
        if (RecordList.Count > 0)
        {
            if (!RecordList[RecordList.Count - 1].Winnder.HasValue)
            {
                TDebug.LogError("上一次战斗未完成，就收到了下一场的EnterPVE");
            }
        }
        RecordList.Add(msg.BattleStr);
        if (!BattleWindow.IsPlaying)
        {
            BattleWindow.OpenWindow(mOverCallback);
            mOverCallback = null;
        }
    }
    public void S2C_BattleReport(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_BattleReport msg = MessageBridge.Instance.S2C_BattleReport(ios);
        TDebug.Log("胜利者"+msg.Winner);
        if (CurRecord == null)
        {
            TDebug.Log("没有战斗记录被初始化");
            return;
        }
        CurRecord.Winnder = msg.Winner;
    }
    void S2C_BattleLog(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_BattleLog msg = MessageBridge.Instance.S2C_BattleLog(ios);
        if (CurRecord == null)
        {
            TDebug.Log("没有战斗记录被初始化");
            return;
        }
        CurRecord.ActionList.AddRange(msg.Record.ActionList);
        TDebug.Log(msg.Record.ToStr());
        TDebug.Log(msg.Record.ToStr2());
        BattleWindow.StartBattleLog();
    }


    public void BattleEnd()
    {
        PLAY_TIME_SCALE = 1;
        if (RecordList.Count > 0)
        {
            BattleWindow = UIRootMgr.Instance.OpenWindow<Window_BattleTowSide>(WinName.Window_Battle, CloseUIEvent.None);
            if (!BattleWindow.IsPlaying)
            {
                BattleWindow.OpenWindow(null);
            }
        }
    }

    public void RemoveCurRecord()
    {
        if (RecordList.Count > 0) RecordList.RemoveAt(RecordList.Count - 1);
    }
}
//public enum BattleState
//{
//    Init,          //初始化
//    Ready,         //准备
//    Start,         //开始战斗
//    WaitCmd,       //等待选择指令
//    ShowRound,     //进行战斗显示
//    ShowAction,    //显示行动
//    ToNextAction,  //进行下一个行动
//    ToNextRound,   //进行下一回合
//    BattleOver,    //战斗结束
//    Max,
//}
