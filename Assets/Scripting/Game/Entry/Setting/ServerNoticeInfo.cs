using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerNoticeInfo
{
    public static long EndTime;
    public static List<AppNoticeItem> NoticeList=new List<AppNoticeItem>();
}

public class AppNoticeItem
{
    public string Title;
    public string Context;
}