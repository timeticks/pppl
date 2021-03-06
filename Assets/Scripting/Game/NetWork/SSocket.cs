using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public delegate void ServPacketHander(BinaryReader ios);
public delegate void OnConnect();
public delegate void OnConnected();
public delegate void OnClosed(bool showTips);

public class SSocket
{
    public OnConnect    OnConnectDelegate;
    public OnConnected  OnConnectedDelegate;
    public OnClosed     OnClosedDelegate;

    private             Queue<BinaryReader> mInQueue        = new Queue<BinaryReader>();
    private readonly    ManualResetEvent    TimeoutObject   = new ManualResetEvent(false);
    
    private int         mTimeoutMSec        = 15*1000;
    protected           SStage mStage       = SStage.NotConnected;
    protected           Socket mSocket;
    private long        mConnectTimeout;
    
    private IPEndPoint mIpEndpoint;
    
    public enum SStage
    {
        NotConnected,
        Connecting,
        Disconnect,
        Connected,
        ConnectFailed,
    }
    
    public SStage Stage
    {
        get { return mStage; }
        set { mStage = value; }
    }
    
    
    protected static int BytesToInt(byte[] ByteInt_)
    {
        UInt32 num = (UInt32)ByteInt_[0] & 0xff;
        num |= ((UInt32)(ByteInt_[1] << 8) & 0xff00);
        num |= ((UInt32)(ByteInt_[2] << 16) & 0xff0000);
        num |= ((UInt32)(ByteInt_[3] << 24) & 0xff000000);
        return (int)num;
    }
    
    
    class NDataBuffer
    {
        public enum EDatType : byte
        {
            E_Header = 0,
            E_Binary = 1
        };
    
        public byte[] CacheData;
        public int ReadedSize;
        public EDatType Type;
    
        public NDataBuffer(EDatType type, int cSize)
        {
            Type = type;
            CacheData = new byte[cSize];
            ReadedSize = 0;
        }
    
        public NDataBuffer(EDatType type)
        {
            Type = type;
            ReadedSize = 0;
            if (type == EDatType.E_Header)
                CacheData = new byte[4];
        }
    
    };
    
    public void Disconnect()
    {
        mStage = SStage.NotConnected;
        if (null != mSocket)
        {
            try
            {
                if (mSocket.Connected)
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    mSocket.Close();
                }
                else
                {
                    TDebug.LogWarning("Socket已经关闭~~~~~~~~~~~~~~~~~~~~~~~~~~");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                TDebug.LogWarning("释放SOCKET对象~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                mSocket = null;
            }
        }
    
    }
    
    
    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="serverIp">服务器IP</param>
    /// <param name="port">服务器端口</param>
    /// <param name="sndCmd">连接成功后的消息包</param>
    public void Connect(IPEndPoint ipEndpoint, IPacket packet)
    {
        if(mSocket != null || mStage != SStage.NotConnected)
            Disconnect();
    
        if (null == mSocket && mStage == SStage.NotConnected) //避免多次连接
        {
            LoggerHelper.LogWarning("创建新的连接,服务器地址" + ipEndpoint.Address);
            try
            {  
                mIpEndpoint = ipEndpoint;
                mStage      = SStage.Connecting;
                mSocket     = new Socket(mIpEndpoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
    
                mSocket.BeginConnect(mIpEndpoint,
                    new AsyncCallback(OnConnected),
                    packet); //
    
                mConnectTimeout = TimeUtils.CurrentTimeMillis + mTimeoutMSec;
            }
            catch(Exception ex)
            {
                TDebug.LogError("连接服务器异常:" + ex.Message);
            }
    
        }
        else if (IsConnected && null != packet)
        {
            TDebug.LogError("老的socket!已经连接");
            Send(packet);
        }
    
    }
    
    
    /// <summary>
    /// 放弃连接/连接超时
    /// </summary>
    /// <param name="obj"></param>
    void ConnectTimeOut(object obj)
    {
        IAsyncResult result = (IAsyncResult) obj;
        if (result != null && !result.AsyncWaitHandle.WaitOne(5000, true)) //等待10秒
        {
            TDebug.LogError("连接服务器超时.....");
            try
            {
                if (mSocket != null)
                {
                    if (Monitor.TryEnter(mSocket, 3000))
                    {
                        try
                        {
                            mStage = SStage.NotConnected;
                        }
                        finally
                        {
                            Monitor.Exit(mSocket);
                        }
                    }
                }
    
    
            }
            catch (System.Exception)
            {
            }
        }
    }
    
    
    protected void OnConnected(IAsyncResult asyncConnect)
    {
        try
        {
           
            mSocket.EndConnect(asyncConnect);
            if (false == mSocket.Connected)
            {
                return;
            }
    
           
            mStage = SStage.Connected;
    
            IPacket msg = (IPacket) asyncConnect.AsyncState;
    
            if (OnConnectedDelegate != null) OnConnectedDelegate();
            if (null != msg) Send(msg);
            
            Debug.Log("连接网关服务器(" + Ip + ":" + Port + ")成功");
            
            NDataBuffer recvBuffer = new NDataBuffer(NDataBuffer.EDatType.E_Header);
            mSocket.BeginReceive(recvBuffer.CacheData, 0, recvBuffer.CacheData.Length, 0,
                new AsyncCallback(OnRecieveData), recvBuffer);
    
        }
        catch (Exception ex)
        {
            Debug.LogError("连接远程服务器(" + Ip+":"+Port + ")失败.Error:" + ex.Message);
    
        }
    
    }
    
    
    protected void OnRecieveData(IAsyncResult asyncReceive)
    {
    
        NDataBuffer recvBuffer = (NDataBuffer) asyncReceive.AsyncState;
        int readedSize = mSocket.EndReceive(asyncReceive);
    
        if (readedSize == 0)
        {
            TDebug.Log("读取消息异常......");
            // Disconnect();
            mStage = SStage.Disconnect;
            return;
        }
    
        recvBuffer.ReadedSize += readedSize;
    
        NDataBuffer pipedBuffer = recvBuffer;
    
        if (recvBuffer.ReadedSize == recvBuffer.CacheData.Length)
        {
    
            switch (recvBuffer.Type)
            {
                case NDataBuffer.EDatType.E_Header:
                {
                    int cSize = BytesToInt(recvBuffer.CacheData);
                    pipedBuffer = new NDataBuffer(NDataBuffer.EDatType.E_Binary, cSize);
                    break;
                }
                case NDataBuffer.EDatType.E_Binary:
                {
                    MemoryStream stream = new MemoryStream(recvBuffer.CacheData);
                    BinaryReader ios = new BinaryReader(stream);
                    mInQueue.Enqueue(ios);
                    pipedBuffer = new NDataBuffer(NDataBuffer.EDatType.E_Header);
                    break;
                }
            }
        }
    
        if (mSocket != null)
            mSocket.BeginReceive(pipedBuffer.CacheData,
                pipedBuffer.ReadedSize,
                pipedBuffer.CacheData.Length - pipedBuffer.ReadedSize,
                SocketFlags.None,
                new AsyncCallback(OnRecieveData),
                pipedBuffer);
    }
    
    
    public void Send(IPacket packet)
    {
        if (IsConnected)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(0);
            //writer.Write((short)2);
            //writer.Write((byte)0);
            writer.Write(packet.NetCode);
            packet.WriteTo(writer);
            int dSize = stream.GetBuffer().Length;
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(dSize - 4);
    
            SocketError state;
            mSocket.BeginSend(stream.GetBuffer(),
                0,
                stream.GetBuffer().Length,
                SocketFlags.None, out state, asyncResult =>
                {
                    SocketError _state;
                    int bytesSent = mSocket.EndSend(asyncResult,out _state);
    
                    if (bytesSent != 0)
                    {
                    }
                    else
                    {
                        Debug.LogError("Socket消息发送失败:连接被断开");
                        //Disconnect();
                        mStage = SStage.Disconnect;
                    }
    
                }, stream.GetBuffer());
         
        }
        else
        {
            Debug.LogError("发送数据失败,因为socket连接已经关闭");
            mStage = SStage.Disconnect;
            Debug.Log("发送数据失败,因为socket连接已经关闭");
            //Disconnect();
        }
    
    }
    
    
    
    protected void OnSendCompleted(IAsyncResult asyncSend)
    {
        SocketError state;
        int bytesSent = mSocket.EndSend(asyncSend,out state);
        LoggerHelper.Log("Socket sended data size = " + bytesSent);
        if (bytesSent == 0)
        {
            LoggerHelper.Log("Socket send size =0,connection closed");
            mStage = SStage.Disconnect;
        }
    }
    
    public  void OnDisconnect()
    {
    
    }
    
    
    public bool IsConnected
    {
        get
        {
            if (mSocket != null) return mSocket.Connected && mStage == SStage.Connected;
            else return false;
        }
    }
    
    public void TryReConnect()
    {
        Disconnect();
        Connect(mIpEndpoint, null);
    }
    
    public string Ip
    {
        get { return mIpEndpoint.Address.ToString(); }
    }
    
    
    public int Port
    {
        get { return mIpEndpoint.Port; }
    }
    
    public Queue<BinaryReader> InQueue
    {
        get { return mInQueue; }
    }
    
    public IPEndPoint IpEndPoint
    {
        get { return mIpEndpoint; }
    }

    public bool IsTimeOut
    {
        get { return mConnectTimeout < TimeUtils.CurrentTimeMillis; }
    }
}