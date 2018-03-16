using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MD5Utils 
{
    public const string VersionMd5TxtName = "versionMd5.txt";

    public static void CheckAndCreateVersion(string versionSavePath)
    {
#if UNITY_EDITOR
        //versionSavePath = versionSavePath.Replace("\\", "/").Replace("file://", "");
        string resPath = versionSavePath;//;versionSavePath;
        string[] files = Directory.GetFiles(resPath, "*", SearchOption.AllDirectories);
        StringBuilder versions = new StringBuilder();
        for (int i = 0, len = files.Length; i < len; i++)
        {
            string filePath = files[i];
            if (files[i].LastIndexOf(".") == -1)
            {
                string relativePath = filePath.Replace(resPath, "");//.Replace("\\", "/");

                string md5 = MD5File(filePath);
                Debug.Log(filePath);
                versions.Append(string.Format("{0},{1},{2}\n", relativePath, md5, GetFileLength(filePath) / 1000));
            }
        }
        // 生成配置文件  
        FileStream stream = new FileStream(versionSavePath + VersionMd5TxtName, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
        stream.Dispose();

        AssetDatabase.Refresh();
#endif
    }

    public static long GetFileLength(string dirPath)
    {
        //判断给定的路径是否存在,如果不存在则退出
        if (!File.Exists(dirPath))
            return 0;
        FileInfo file = new FileInfo(dirPath);
        long size = file.Length;
        return size;
    }

    public static string MD5File(string file)//从某文件路径下，得到某文件的md5信息
    {
#if UNITY_EDITOR
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
#else
        return string.Empty;
#endif
    }

}
