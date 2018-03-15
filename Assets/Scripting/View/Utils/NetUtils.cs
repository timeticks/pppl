using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetUtils {

    //private static int mReConnectCount = 0;
    //public static IPAddress GetIpAddressByHost(string host)
    //{
    //    if (mReConnectCount < 5)
    //    {
    //        try
    //        {
    //            IPAddress[] ipHostEntry = Dns.GetHostAddresses(host);
    //            for (int i = 0; i < ipHostEntry.Length; i++)
    //            {
    //                if (ipHostEntry[i].AddressFamily == AddressFamily.InterNetworkV6 ||
    //                    ipHostEntry[i].AddressFamily == AddressFamily.InterNetwork)
    //                {
    //                    return ipHostEntry[i];
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError(string.Format("第{0}次获取Ip失败,host={1},error:{2}", mReConnectCount, host, ex.Message));
    //        }
    //        mReConnectCount++;
    //        GetIpAddressByHost(host);
    //    }
    //    else
    //    {
    //        TDebug.LogError("获取DNS信息失败,并超过重试次数");
    //        if (GameClient.Instance != null)
    //            GameClient.Instance.Disconnect(true);
    //        mReConnectCount = 0;
    //    }
    //    return null;
    //}

    public static byte[] WriteUTF(string data, ref BinaryWriter buffer)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        buffer.Write(bytes.Length);
        buffer.Write(bytes);
        return bytes;
    }


    public static void PositionToVector3(BinaryReader stream, ref Vector3 v)
    {
        v.x = stream.ReadInt16();
        v.z = stream.ReadInt16();
    }


    public static string  ReadUTF(BinaryReader buffer)
    {
        int     Length  = buffer.ReadInt32();
        byte[]  data    = buffer.ReadBytes(Length);
        return Encoding.UTF8.GetString(data);
    }

    /// <summary>
    /// 解析byte数组，加密   jave的byte转为c#的byte，-1=>255   -127=>129
    /// </summary>
    private static byte[] EncryptKeys = new byte[256] { 102, 232, 30, 41, 209, 129, 22, 139, 137, 13, 9, 8, 225, 247, 166, 99, 58, 179, 178, 245, 211, 173, 222, 3, 246, 210, 63, 174, 140, 240, 50, 198, 35, 236, 10, 239, 73, 220, 138, 97, 190, 79, 67, 228, 40, 121, 6, 124, 116, 150, 114, 14, 205, 113, 104, 108, 156, 33, 167, 255, 25, 71, 147, 165, 196, 133, 176, 103, 1, 72, 74, 164, 28, 215, 43, 194, 157, 87, 161, 86, 70, 180, 162, 38, 206, 15, 98, 128, 144, 249, 183, 16, 213, 130, 154, 105, 29, 195, 101, 112, 216, 31, 51, 177, 100, 57, 49, 66, 155, 106, 60, 185, 123, 181, 149, 250, 186, 125, 92, 88, 117, 47, 110, 254, 229, 111, 81, 18, 78, 96, 243, 83, 217, 20, 172, 7, 159, 168, 191, 221, 54, 109, 163, 204, 188, 39, 134, 45, 69, 214, 234, 132, 218, 94, 248, 151, 127, 203, 189, 231, 118, 135, 85, 237, 230, 197, 64, 56, 208, 235, 2, 65, 115, 4, 233, 55, 212, 238, 148, 52, 253, 175, 46, 37, 27, 171, 184, 252, 193, 44, 152, 223, 23, 89, 34, 244, 26, 11, 199, 95, 158, 242, 119, 224, 19, 200, 21, 160, 145, 62, 120, 142, 143, 202, 107, 170, 76, 0, 141, 201, 153, 82, 227, 226, 192, 187, 146, 5, 59, 219, 75, 131, 32, 126, 77, 80, 90, 93, 68, 24, 182, 48, 12, 251, 122, 241, 53, 61, 42, 207, 17, 36, 84, 91, 169, 136 };
    public static string ReadUTFEncrypt(BinaryReader buffer)
    {
        int Length = buffer.ReadInt32();
        byte[] data = buffer.ReadBytes(Length);
        //data = EncryptBytes(data);
        return Encoding.UTF8.GetString(data);
    }

    /// <summary>
    /// 加密字节
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] EncryptBytes(byte[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = EncryptKeys[data[i]];
        }
        return data;
    }
    /// <summary>
    /// 加密字节流
    /// </summary>
    public static BinaryReader EncryptBinary(BinaryReader ios)
    {
        byte[] originBytes = ios.ReadBytes((int)(ios.BaseStream.Length - ios.BaseStream.Position));
        BinaryReader bin = new BinaryReader(new MemoryStream(EncryptBytes(originBytes)));
        return bin;
    }
}
