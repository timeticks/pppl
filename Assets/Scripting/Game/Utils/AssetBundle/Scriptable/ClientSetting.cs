using UnityEngine;
using System.Collections;

public class ClientSetting : ScriptableObject
{
    public static string fileName = "ClientSetting";

    public ClientSettingData m_Data;
    public ClientSetting() { }
    public ClientSetting(ClientSettingData data)
    {
        m_Data = data;
    }

}

[System.Serializable]
public class ClientSettingData
{
    //public ResType m_EditorResType;
    //public IpType m_EditorHttpIpType;
    //public IpType m_EditorNetIpType;

    public int m_PackageVersion = 1;   //安装包版本
    public string m_BuildPath;
    public string m_PackageName;
    //public IpType m_HttpIpType;
    //public IpType m_NetIpType;
    //public ResType m_ResType;
    public bool m_IsPure;              //true则不含资源包
}
public enum ResType : byte       //资源类型
{
    Local,
    Inner,
    Outer,
    Release
}
public enum IpType : byte      //网络类型
{
    Inner,
    Outer,
    Release
}