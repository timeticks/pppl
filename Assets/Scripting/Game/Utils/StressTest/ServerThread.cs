using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace SeverStressTest
{

    public class SeverThread
    {
        public enum SendStatus
        {
            Init,
            Register,
        }

        public SendStatus MyStatus;
        private Dictionary<short, ServPacketHander> mServHandlers = new Dictionary<short, ServPacketHander>();
        
        private int mReConnectCount;
        private long mSnapServerTime;
        private SSocket mSSocket;
        private bool mConnneted;
        private int mIdx;
        private bool mIsLoading = false;
        private int mLastIndex;   //用于服务器传序号递增消息
        public SeverThread(int idx)
        {
            mIdx = idx;
            mSSocket = new SSocket();
            mSSocket.OnConnectedDelegate = OnConnected;
            mSSocket.OnClosedDelegate = OnDisconnect;
        }
        public void Run()
        {
            Thread.Sleep(mIdx * 10 % 100);
            if (mSSocket != null)
            {
                Connect(null);
            }
            while (!mConnneted)
            {
                Thread.Sleep(100);
            }
            while (true)
            {
                if (mConnneted && !mIsLoading)
                {
                    if (MyStatus == SendStatus.Register)
                    {
                        string acc = string.Format("stressb_{0}_{1}", StressTestMgr.SendMessageNum, TimeUtils.CurrentTimeMillis % 100000000);
                        lock (MessageBridge.Instance)
                        {
                            SendMessage(MessageBridge.Instance.C2S_Register(acc, "1", "R2", (PlatformType)AppSetting.Platform));
                        }
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    if (mSSocket.InQueue.Count > 0)
                    {
                        BinaryReader ios = mSSocket.InQueue.Dequeue();
                        OnMessageArrived(ios);
                    }
                    Thread.Sleep(100);
                }
                
            }
        }

        public void Update()
        {
            
        }

        public virtual void Connect(IPacket msg = null)
        {
            //if (mSSocket == null)
            //{
            //    Debug.LogError("socket is null");
            //    return;
            //}
            //StressTestMgr.ConnectingNum ++;
            //IPAddress ipAddress = NetUtils.GetIpAddressByHost(ServerInfo.GetGateServHost());
            //if (ipAddress == null)
            //{
            //    Debug.LogError("网关服务器地址错误:" + ServerInfo.GetGateServHost());
            //    mSSocket.Stage = SSocket.SStage.ConnectFailed;
            //    return;
            //}

            //IPEndPoint mIpEndpoint = new IPEndPoint(ipAddress, ServerInfo.GetGateServPort());
            //mSSocket.Connect(mIpEndpoint, msg);
        }


        /// <summary>
        /// 当连接断开回调
        /// </summary>
        /// <param name="idx"></param>
        public virtual void OnDisconnect(bool showTips)
        {

        }

        /// <summary>
        /// 当客户端连接成功回调(废弃中)
        /// </summary>
        public void OnConnect()
        {

        }

        /// <summary>
        /// 连接操作完成
        /// </summary>
        /// <param name="succes">是否连接指定服务器成功</param>
        public virtual void OnConnected()
        {
            //检查资源更新后，     打开注册界面=>UIRootMgr.MainUI.m_Start.OpenLoginWindow();
            //AssetUpdate.Instance.Init();
            StressTestMgr.ConnectedNum++;
            mConnneted = true;
            Debug.Log("连接服务器成功,ip:" + mSSocket.Ip + ":" + mSSocket.Port);
        }

        public void SendMessage(IPacket packet)
        {
            StressTestMgr.SendMessageNum ++;
            mIsLoading = true;
            if (IsConnected && packet != null)
            {
                Debug.LogWarning("发送客户端消息,协议===>>>" + (NetCode_C)packet.NetCode + "[NetCode:" + packet.NetCode + "]");
                mSSocket.Send(packet);
            }
            else
            {
                Debug.LogError("连接已经断开!!!!!!!!!!!!!!!!!!!!!!!!发送失败");
                Connect(packet);
            }

        }

        public bool IsConnected
        {
            get
            {
                if (mSSocket != null) return mSSocket.IsConnected;
                else return false;
            }
        }

        /// <summary>
        /// 主动释放socket连接,并弹断开提示
        /// </summary>
        public virtual void Disconnect(bool showTips)
        {

            if (mSSocket != null)
            {
                mSSocket.Disconnect();
                mSSocket = null;
            }

            OnDisconnect(showTips);
        }

        public virtual void OnMessageArrived(BinaryReader ios)
        {
            StressTestMgr.MessageArrivedNum++;
            mIsLoading = false;
            short head = ios.ReadInt16();
            IPacket packet = null;

            int mStatus = ios.ReadInt32();

            ServPacketHander hander = null;
            if (mServHandlers.TryGetValue(head, out hander))
            {
                if (hander != null)
                {
                    Debug.LogWarning("解析服务器消息,消息协议===>>" + ((NetCode_S)head) + "[NetCode:" + head + "],Status Code:" + mStatus);
                    hander(ios);
                    return;
                }
            }
            
            Debug.LogError("未注册的服务器消息回调" + ((NetCode_S)head) + "[NetCode:" + head + "]");
        }


        public void RegisterNetCodeHandler(NetCode_S servCmd, ServPacketHander handler)
        {
            if (null != handler)
                mServHandlers[(short)servCmd] = handler;
            else
                mServHandlers.Remove((short)servCmd);
        }
    }
}
