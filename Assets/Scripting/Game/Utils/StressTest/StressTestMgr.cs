using SeverStressTest;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StressTestMgr : MonoBehaviour
{
    public int ThreadNum;
    public static int ConnectingNum;
    public static int ConnectedNum;
    public static int SendMessageNum;
    public static int MessageArrivedNum;
    private List<SeverThread> ThreadList = new List<SeverThread>();
    private List<Thread> TList = new List<Thread>();
    void Awake()
    {
        //ServerInfo.BundleVersion = 1;
        //ServerInfo.GateServHost = "ucweb.devilfactory.com";
        //ServerInfo.GateServPort = 1209;
    }

    void OnGUI()
    {
        if (GUILayout.Button("Init"))
        {
            for (int i = 0; i < ThreadNum; i++)
            {
                SeverThread task = new SeverThread(i);
                Thread th = new Thread(new ThreadStart(task.Run));
                ThreadList.Add(task);
                TList.Add(th);
                th.Start();
            }
        }
        if (GUILayout.Button("SendRegister"))
        {
            for (int i = 0; i < ThreadList.Count; i++)
            {
                ThreadList[i].MyStatus = SeverThread.SendStatus.Register;
            }
        }
        GUILayout.Label("ConnectingNum:" + ConnectingNum);
        GUILayout.Label("ConnectedNum:" + ConnectedNum);
        GUILayout.Label("SendMessageNum:" + SendMessageNum);
        GUILayout.Label("MessageArrivedNum:" + MessageArrivedNum);
    }

    void OnDestroy()
    {
        for (int i = 0; i < ThreadList.Count; i++)
        {
            ThreadList[i].Disconnect(false);
            TList[i].Abort();
        }
    }
}
