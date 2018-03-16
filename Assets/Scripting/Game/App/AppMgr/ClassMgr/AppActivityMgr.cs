using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppActivityMgr  {

    static Dictionary<int, ActivityItem> m_ActivityDic = new Dictionary<int, ActivityItem>();

    public static bool GetActivity(int ActivityId ,out ActivityItem item)
    {
        if(m_ActivityDic.TryGetValue(ActivityId,out item))
        {
            return true;
        }
        return false;
    }

    public static void SaveOrUpdateActivity(ActivityItem acitem)
    {
      
    }

    public static bool Contains(int ActivityId)
    {
        return m_ActivityDic.ContainsKey(ActivityId);
    }

}

public class ActivityItem
{
    public int ActivityId;
    //开始的倒计时，-1为已经过去
    public int StartTimeDown;
    public int EndTimeDown;
    public string Content;  //组装的Json

    public ActivityItem() { }
    //public ActivityItem(Seraph.pb_ActivityItem item) {
    //    ActivityId = item.ActivityId;
    //    StartTimeDown = item.StartTimeDown;
    //    EndTimeDown = item.EndTimeDown;
    //    Content = item.Content;
    //    AppTimer.AddTimeDown(ActivityId + "S", StartTimeDown);
    //    AppTimer.AddTimeDown(ActivityId + "E", EndTimeDown);
    //}
}