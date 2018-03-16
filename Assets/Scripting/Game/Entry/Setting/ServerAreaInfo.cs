using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerAreaInfo
{
    
    public enum EnterStatus
    {
        NoOpen,      //0  默认不可进入
        CanEnter,    //1  可以进入
        Full,        //2  爆满
        Maintenance, //3  维护中
    }

    public static List<ServerArea> ServerList
    {
        get { return ServerInfo.ServerList; }
    }

    //public static List<ServerArea> ServerList =new List<ServerArea>();

    public static EnterStatus GetEnterStatus(int idx)
    {
        for (int i = 0; i < ServerList.Count; i++)
        {
            if (ServerList[i].Idx == idx)
            {
                return ServerList[i].GetEnterStatus();
            }
        }
        return EnterStatus.NoOpen;
    }

    public static List<ServerArea> GetOldServerList( )
    {
        List<ServerArea> tempList = new List<ServerArea>();
        // 老账号登录只取idx 为1， 2的区
        for (int i = 0; i < ServerList.Count; i++)
        {
            if (ServerList[i].Idx == 1 || ServerList[i].Idx == 2)
            {
                tempList.Add(ServerList[i]);
            }
        }
        return tempList;
    }
}

public class ServerArea
{
    public string ServerId;
    public int Idx;
    public string Name;
    public string Desc;
    public int Status;
    public string GateServHost;
    public int GateServPort;
    public ServerAreaInfo.EnterStatus GetEnterStatus()
    {
        return (ServerAreaInfo.EnterStatus)Status;
    }
}
public class PassPortHostArea
{
    public int Idx;
    public int Port;
    public string Host;
}
