using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MD5Utility 
{
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
                
                string md5 = MD5Utility.MD5File(filePath);
                Debug.Log(filePath);
                versions.Append(string.Format("{0},{1},{2}\n", relativePath, md5, GetFileLength(filePath)/1000));
            }
        }
        // 生成配置文件  
        FileStream stream = new FileStream(versionSavePath + SharedAsset.VersionMd5TxtName, FileMode.Create);
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

    public static List<string> CompareVersion(string oldStr, string newStr , bool thisIsOld)//比较版本差别
    {
        return CompareVersion(ParseVersionFile(oldStr), ParseVersionFile(newStr));
    }
    public static List<CompareItem> CompareVersion(string oldStr, string newStr)//比较md5，返回比较项
    {
        List<CompareItem> itemList = new List<CompareItem>();
        Dictionary<string, string> oldDict = ParseVersionFile(oldStr);
        Dictionary<string, string> newDict = ParseVersionFile(newStr);
        Dictionary<string, int> newDictSize = ParseVersionSize(newStr);
        foreach (var item in newDict)
        {
            string oldVal="";
            if (oldDict.ContainsKey(item.Key))
            {
                oldVal = oldDict[item.Key];
            }
            CompareItem compareItem = new CompareItem(item.Key, oldVal, item.Value);
            newDictSize.TryGetValue(item.Key, out compareItem.FileSize);
            itemList.Add(compareItem);
        }
        return itemList;
    }

    //比较，得到md5不同的资源
    private static List<string> CompareVersion(Dictionary<string, string> oldDic, Dictionary<string, string> newDic)
    {
        List<string> diffAsset = new List<string>();
        foreach (var version in newDic)
        {
            string fileName = version.Key;
            string serverMd5 = version.Value;
            //新增的资源
            if (!oldDic.ContainsKey(fileName))
            {
                diffAsset.Add(fileName);
            }
            else
            {
                string localMd5;
                oldDic.TryGetValue(fileName, out localMd5);
                if (!serverMd5.Equals(localMd5))//md5不一样
                {
                    diffAsset.Add(fileName);
                }
            }
        }
        //本次有更新，同时更新本地的version.txt    
        return diffAsset;
    }

    public static Dictionary<string, string> ParseVersionFile(string md5Str)//根据字符串，得到所有资源的md5键值表
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        if (md5Str == null || md5Str.Length == 0)
        {
            return new Dictionary<string, string>();
        }
        string[] items = md5Str.Split(new char[] { '\n' });
        foreach (string item in items)
        {
            string[] info = item.Split(new char[] { ',' });
            if (info != null && info.Length >= 2)
            {
                dict.Add(info[0], info[1]);
            }
        }
        return dict;
    }

    public static Dictionary<string, int> ParseVersionSize(string md5Str)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        if (md5Str == null || md5Str.Length == 0)
        {
            return new Dictionary<string, int>();
        }
        string[] items = md5Str.Split(new char[] { '\n' });
        foreach (string item in items)
        {
            string[] info = item.Split(new char[] { ',' });
            if (info != null && info.Length >= 3)
            {
                dict.Add(info[0], info[2].ToInt());
            }
        }
        return dict;
    }

    public static string GetNewMd5Str(List<CompareItem> itemList) //将比较列表，转为md5字符串
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < itemList.Count; i++)
        {
            str.Append(string.Format("{0},{1},{2}\n", itemList[i].ItemName,
                    itemList[i].IsUpdateOver ? itemList[i].NewValue : itemList[i].OldValue, itemList[i].FileSize));
        }
        return str.ToString();
    }




    ////根据md5，刷新版本数
    //public static Dictionary<string, int> FreshVersionNum(string verNumStr,string newMd5 , List<string> diffList)
    //{
    //    Dictionary<string, int> numDic = new Dictionary<string, int>();
    //    Dictionary<string, string> md5Dic = MD5Utility.ParseVersionFile(newMd5);
    //    if (verNumStr == null || verNumStr.Length == 0 || verNumStr=="")//如果初次加载，则将所有资源的版本号设为0
    //    {
    //        foreach (var item in md5Dic)
    //        {
    //            numDic.Add(item.Key, 0);
    //            return numDic;
    //        }
    //    }
    //    string[] items = verNumStr.Split(new char[] { '\n' });
    //    foreach (string item in items)
    //    {
    //        string[] info = item.Split(new char[] { ',' });
    //        if (info != null && info.Length == 2) { numDic.Add(info[0], info[1].ToInt()); }
    //    }
    //    for (int i = 0; i < diffList.Count; i++)//将新加资源的版本号设为1，是需要去服务器下载的
    //    {
    //        if (!numDic.ContainsKey(diffList[i])) { numDic.Add(diffList[i], 1); }
    //        else { numDic[diffList[i]] = numDic[diffList[i]] + 1; }
    //    }
    //    return numDic;
    //}
}

public class CompareItem
{
    public string ItemName;
    public string OldValue;
    public string NewValue;
    public bool IsUpdateOver;     //已经更新到新版本
    public int FileSize;          //文件大小,kb
    //public bool IsInStreaming;  //没有persist版本，直接读取streaming版本
    public bool NeedUpdate
    {
        get { return OldValue != NewValue; }
    }
    public CompareItem(string name, string oldVal, string newVal)
    {
        ItemName = name;
        OldValue = oldVal;
        NewValue = newVal;
    }

    
}