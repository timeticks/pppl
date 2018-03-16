using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppSetting
{
    public static double VERSION = 1.31;    //脚本版本

    public static string    AppHost;
    public static int       Platform;
    public static int       BundleVersion;
    public static int       GameDataVersion;
    public static double    LogicVersion;
    public static double    Version;    //安装包版本，也是url版本
    public static double    GameVersion;//3位整数，小数后面ios不用强制更新
    public static int       AppMode;
    public static string    Channel="";
}

