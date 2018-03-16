﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SaveUtils
{
    
    private static int mUid {
        get
        {
            return 0;
        }
    }

    private static int[] UidCode = new int[4] { 7821, 4209, 4312, 2093 };

    private static char UidMod{
        get{
            if (mUid % 21946 == 0) return (char)21947;
            return (char)(mUid % 21946);
        }
    }


    /// <summary>
    /// 获取加密的字符串，少调用，10000次产生约2.4mGC，21ms延迟
    /// </summary>
    public static string GetEncryptInPlayer(string key, string defaultString = "")
    {
        key = TUtils.MDEncode(GetKey(key));
        return PlayerPrefs.GetString(key, defaultString);
    }
    /// <summary>
    /// 设置加密的字符串
    /// </summary>
    public static void SetEncryptInPlayer(string key, string setValue)
    {
        key = GetEncryptKey(key);
        PlayerPrefs.SetString(key, setValue.ToEncString(UidMod));
    }

    static string GetEncryptKey(string key)
    {
        key = GetKey(key);
        return key.ToEncString(UidMod);
    }



    
    public static string GetStringInPlayer(string key , string defaultString="")
    {
        return PlayerPrefs.GetString(GetKey(key), defaultString);
    }
    public static void SetStringInPlayer(string key, string setString)
    {
        string playerKey = GetKey(key);
        PlayerPrefs.SetString(playerKey, setString);
    }

    public static int GetIntInPlayer(string key, int defaultValue =0)
    {
        return PlayerPrefs.GetInt(GetKey(key), defaultValue);
    }
    public static void SetIntInPlayer(string key, int setValue)
    {
        string playerKey = GetKey(key);
        PlayerPrefs.SetInt(playerKey, setValue);
    }
    public static void DeleteKeyInPlayer(string key)
    {
        PlayerPrefs.DeleteKey(GetKey(key));
    }
     
    public static bool HasKeyInPlayer(string key)
    {
        return PlayerPrefs.HasKey(GetKey(key));
    }

    public static float GetFloatInPlayer(string key, float defaultValue = 0)
    {
        return PlayerPrefs.GetFloat(GetKey(key), defaultValue);
    }
    public static void SetFloatInPlayer(string key, float setValue)
    {
        string playerKey = GetKey(key);
        PlayerPrefs.SetFloat(playerKey, setValue);
    }

    private static string GetKey(string key)
    {
        return string.Format("{0}:{1}" , mUid , key);
    }
}


public enum PrefsSaveType
{
    RolePos,
}