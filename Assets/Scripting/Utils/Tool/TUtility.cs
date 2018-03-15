using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
/// <summary>
/// 功能类。。。
/// 所有的float -> int ，都是四舍五入取整
/// </summary>
public class TUtility
{
    /**
     * 一天内的毫秒数
     */
    public const  int ONE_DAY       = 86400000;
    /**
     * 一小时内的毫秒数
     */
    public const  int ONE_HOUR      = 3600000;
    /**
     * 一分钟内的毫秒数
     */
    public const  int ONE_MINUTE    = 60000;
    /**
     * 半分钟内的毫秒数
     */
    public const  int HALF_MINUTE    = 30000;
    /**
     * 一秒钟内的毫秒数
     */
    public const  int ONE_SECOND    = 1000;
    /**
     * 一周内的毫秒数
     */
    public const  int ONE_WEEK      = 604800000;

    public const int LevelUpConfigId = 1204000000;//加上level得到当前level在技能表中的ID

    /// <summary>
    /// 按钮不可点击时，按钮文字颜色
    /// </summary>
    public static Color Color_Btn_Disable = new Color(163 / 255f, 163 / 255f, 163 / 255f);

    public static string[] PlayerNames = new string[]
    {"丽璐·阿歌特","佐伯·杏太郎","耶纳斯·帕沙","胡里奥·埃涅科","希恩·杨","切萨雷·托尼","艾米利奥·费龙","安吉洛·普契尼","卡洛·西诺","伊恩·杜可夫","梨花·薛",
"朱利安·洛佩斯","令·谢","法蒂玛·哈涅","哈希姆·阿迪勒","略克·西萨","马丁·斯派尔","宗甚·来岛","瑜·文","罗伯特·斯托克","查尔斯·格万","保罗·莫雷斯","西蒙·利纳雷斯","恩佐·莱季奥尼","萨利多·阿加尔","伊本·尼亚迪","大卫·格雷文","长庆·神室","胡安·布兰克","杰克斯·布鲁姆","朱里安·弗美尔","尤里斯·胡格恩","布莱特·佩罗","约翰·拉穆吉奥","雅各伯·波图昂","贝拉索·阿戈雷","萨迦诺斯·比伊","威廉·克莱维","埃尔南·贝利奥","伊斯尔·亚塞尔","里查德·汤森德","汉斯·莱切尔","阿卡布·达迈","谢乌德·埃米","米瓦奥尔·根茨","夏洛特·米列","相铉·韩","克莱尔·波弗","宗九郎·泷田","比安卡·普契尼","弗朗西斯卡","玛丽","玛格丽特","汪达","朱丽叶","唐娜","玛蒂尔达","玛丽娜","法提希亚","娜丽","哈特拉","萨菲亚","贝娜齐尔","露西亚","泰塞斯","美华","丽姬","樱","维利萨","希尔维亚","伊莎贝尔","杰克·布鲁姆","里安·弗美尔","胡格恩","佩罗","拉穆吉奥","约翰","雅各伯","阿戈雷","波图昂","威廉·比伊","克莱维","贝利奥",
       };


    public static string GetPlayerNames(int playerId)
    {
        if (playerId >= PlayerNames.Length)
        {
            int repeatPlayerId = playerId%(PlayerNames.Length - 1);
            return string.Format("{0}{1}", PlayerNames[repeatPlayerId] , playerId.ToString());
        }
        playerId = Mathf.Clamp(playerId, 0, PlayerNames.Length - 1);
        return PlayerNames[playerId];
    }



    public static Color Switch16ToColor(string colorString)//16进制转颜色
    {
        if (colorString==null || colorString.Length < 6)
            return Color.black;
        try
        {
            Color returnCol = new Color();
            returnCol.r = (float) Convert.ToInt32(colorString.Substring(0, 2), 16)/256f;
            returnCol.g = (float) Convert.ToInt32(colorString.Substring(2, 2), 16)/256f;
            returnCol.b = (float) Convert.ToInt32(colorString.Substring(4, 2), 16)/256f;
            returnCol.a = 1;
            return returnCol;
        }
        catch
        {
            return Color.black;
        }
    }

    public static Texture2D CreatRgbTexture(float r, float g, float b)
    {
        Color fillColor = new Color(r, g, b);
        return CreatColorTexture(fillColor);
    }

    public static Texture2D CreatColorTexture(Color fillColor) //创建纯色图片
    {
        var texture = new Texture2D(32, 32);
        var pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = fillColor;
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }


    /// <summary>
    /// 获取所有子节点 包括隐藏
    /// </summary>
    public static List<Transform> GetAllChild(GameObject go)
    {
        List<Transform> childlist = new List<Transform>();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            childlist.Add(go.transform.GetChild(i));
            List<Transform> list = GetAllChild(go.transform.GetChild(i).gameObject);
            for (int j = 0; j < list.Count; j++)
            {
                childlist.Add(list[j]);
            }
        }
        return childlist;
    }

    /// <summary>
    /// 将数字转为可排序的名字1——0001
    /// </summary>
    public static string PadZeroLeft(int num , int padNum)
    {
        string numString = num.ToString();
        numString = numString.PadLeft(padNum, '0');
        return numString;
    }

    /// <summary>
    /// 根据数字，将sprite赋在imageList中。isFirstIs10是否数字的第一位都显示第11个sprite，如：-10
    /// </summary>
    /// <param name="numValue"></param>
    /// <param name="numImageList"></param>
    public static void SetNumToSpriteList(int numValue, List<Image> numImageList, List<Sprite> numSpriteList, bool isFirstIs10)
    {
        if (numSpriteList == null || numImageList == null)
        {
            TDebug.LogError("numSpriteList == null || numImageList == null");
            return;
        }
        for (int i = 0; i < numImageList.Count; i++)
        {
            numImageList[i].gameObject.SetActive(true);
        }
        int curIndex = 0;
        if (isFirstIs10 && numSpriteList.Count >= 11)//如显示伤害时，第一个都为减号
        {
            numImageList[0].sprite = numSpriteList[10]; numImageList[0].SetNativeSize();
            curIndex++;
        }
        if (numImageList.Count < curIndex + 2) numValue = Mathf.Min(numValue, 9);
        else if (numImageList.Count < curIndex + 3) numValue = Mathf.Min(numValue, 99);
        else if (numImageList.Count < curIndex + 4) numValue = Mathf.Min(numValue, 999);

        if (numValue >= 1000)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue / 1000, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 1000 / 100, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        else if (numValue >= 100)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue / 100, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        else if (numValue >= 10)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].gameObject.SetActive(false);
        }
        else
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        for (int i = curIndex + 1; i < numImageList.Count; i++)
        {
            numImageList[i].gameObject.SetActive(false);
        }
    }


    public static bool CheckIsNullAndDebug(object obj)//判空，并输出错误信息
    {
        if (obj == null)
        {
            TDebug.LogErrorFormat("{0}IsNull", obj.ToString());
            return true;
        }
        return false;
    }


    static System.Random rand = new System.Random();
    public static List<int> GetRandomList(List<int> randomList)//将有序列表，转为随机列表
    {
        int count = randomList.Count;
        for (int i = 0; i < count; i++)
        {
            int r1 = rand.Next(0, randomList.Count);
            int r2 = rand.Next(0, randomList.Count);
            int temp = randomList[r1];
            randomList[r1] = randomList[r2];
            randomList[r2] = temp;
        }
        return randomList;
    }

    //生成0-numAmount的列表，其中元素值随机
    public static List<int> GetRandomList(int numAmount)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < numAmount; i++)
        {
            list.Add(i);
        }
        return GetRandomList(list);
    }

    /// <summary>
    /// 根据V4的限制，得到随机的V2
    /// </summary>
    /// <param name="limit">x左，y上，z右，w下</param>
    /// <returns></returns>
    public static Vector2 GetRandomV2InRect(List<Vector4> limit)
    {
        int randNum = rand.Next(0, limit.Count);
        return new Vector2(rand.Next((int)limit[randNum].x, (int)limit[randNum].z), rand.Next((int)limit[randNum].w, (int)limit[randNum].y));
    }


    /// <summary>
    /// 在形如“111010;111012”的字符串中，获得数字列表
    /// </summary>
    public static List<int> SplietToIntList(string str, char splitChar = ';', bool ignoreZero = true)
    {
        List<int> l = new List<int>();
        string[] s = str.Split(splitChar);
        if (s != null && s.Length > 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int temp = 0;
                if (int.TryParse(s[i], out temp))
                {
                    if (ignoreZero && temp == 0) continue;
                    l.Add(temp);
                }
            }
        }
        return l;
    }
    public static int[] SplitToIntArray(string str, char splitChar = '|')
    {
        string[] s = str.Split(splitChar);
        int[] l = new int[s.Length];
        if (s != null && s.Length > 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int temp = 0;
                int.TryParse(s[i], out temp);
                l[i] = temp;
            }
        }
        return l;
    }
    /// <summary>
    /// 在形如“111010;111012”的字符串中，获得数字列表
    /// </summary>
    public static List<float> SplitToFloatList(string str, char splitChar = ';', bool ignoreZero = true)
    {
        List<float> l = new List<float>();
        string[] s = str.Split(splitChar);
        if (s != null && s.Length > 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                float temp = 0;
                if (float.TryParse(s[i], out temp))
                {
                    if (ignoreZero && temp == 0) continue;
                    l.Add(temp);
                }
            }
        }
        return l;
    }

    

    public static Vector2 GetDirByDisAndCenter(Vector2 curPos, Vector2 forward, float forwardDis, Vector2 centerPos, float radius)//用于绕某圆心运动
    {
        Vector2 v2 = Vector2.zero;
        Vector2 forwardPos = curPos + forward * forwardDis;
        v2 = forwardPos - centerPos;
        v2 = v2.normalized * radius + centerPos;
        return v2;
    }
    public static Vector3 GetDirByCenter(Vector3 curPos, float forwardDis, Vector3 centerPos, float radius)
    {
        Vector3 targetPos = Vector3.zero;
        Vector3 tanDir = Vector3.Cross(curPos - centerPos, Vector3.up);
        Vector2 tanDir2 = new Vector2(tanDir.x, tanDir.z);
        Vector2 v2 = GetDirByDisAndCenter(new Vector2(curPos.x, curPos.z), tanDir2.normalized, forwardDis, new Vector2(centerPos.x, centerPos.z), radius);
        targetPos = new Vector3(v2.x, curPos.y, v2.y);
        return targetPos;
    }

    public static void SetParent(Transform childTran, Transform parentTran, bool isRectTransformZero=false,bool isRotationZero=true)  //设置子物体后，重置位置信息
    {
        childTran.SetParent(parentTran);
        childTran.localPosition = Vector3.zero;
        childTran.localScale = Vector3.one;
        if (isRotationZero) childTran.localRotation = Quaternion.identity;
        if (isRectTransformZero)
        {
            childTran.GetComponent<RectTransform>().localPosition = Vector3.zero;
            childTran.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
    }
    public static void SetParent(Transform childTran, Transform parentTran, Vector3 localPosition, Vector2 sizeDetla) //设置子物体后，重置位置信息
    {
        childTran.SetParent(parentTran);
        childTran.localScale = Vector3.one;
        childTran.localRotation = Quaternion.identity;
        childTran.GetComponent<RectTransform>().localPosition = localPosition;
        childTran.GetComponent<RectTransform>().sizeDelta = sizeDetla;
    }


    #region 时间相关
    /// <summary>
    /// 秒数转化时:分:秒string返回
    /// </summary>
    public static string TimeSecondsToDayStr_LCD( int total)
    {
        // TDebug.LogWarning("----------------" + total);
        int sec = total % 60;
        int min = total / 60 % 60;
        int hour = total / 60 / 60;
        return string.Format("{0}:{1}:{2}", hour.ToString().PadLeft(2, '0'), min.ToString().PadLeft(2, '0'), sec.ToString().PadLeft(2, '0'));
    }
    /// <summary>
    /// 时间戳转为C#格式UTC时间
    /// </summary>
    public static DateTime stampToDateTime(long timeStamp)
    {
        DateTime dateTimeStart = TimeUtils.timeStampStart;
        try
        {
            return dateTimeStart.AddSeconds(timeStamp);
        }
        catch
        {
            return dateTimeStart;
        }
    }
    /// <summary>
    /// 时间戳转北京时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime ConvertToDateTime(long timeStamp)
    {
        long curTimeTick = timeStamp * 10000 + TimeUtils.timeStampStartTicks;
        DateTime utcTime = new DateTime(curTimeTick , DateTimeKind.Utc);
        TimeSpan offset = new TimeSpan(TimeUtils.TIME_UTC_OFFSET_TICKS); // utc时间+8小时 = 北京时间
        return utcTime.Add(offset);
    }
    /// <summary>
    /// 时间戳转手机本地时间
    /// </summary>
    public static DateTime ConvertToLocalDateTime(long timeStamp)
    {
        long curTimeTick = timeStamp * 10000 + TimeUtils.timeStampStartTicks;
        DateTime localTime = new DateTime(curTimeTick , DateTimeKind.Utc).ToLocalTime();
        return localTime;
    }

    //public static DateTime ConvertToDateTime(long timeStamp)
    //{
    //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
    //    long lTime = long.Parse(timeStamp.ToString() + "0000");
    //    TimeSpan toNow = new TimeSpan(lTime);
    //    return dtStart.Add(toNow);
    //} 

    /// <summary>
    /// C#格式时间(本地时间)转为时间戳
    /// </summary>
    public static long DateTimeToStamp(DateTime time)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return (long)(time - startTime).TotalMilliseconds;
    }

    ///// <summary>
    ///// 时间戳（毫秒）转为C#时间，返回豪秒
    ///// </summary>
    ///// <param name="timeStamp"></param>
    ///// <returns></returns>
    //public static long GetDiffToNow(long timeStamp)
    //{
    //    DateTime stampDataTime = ConvertToDateTime(timeStamp);
    //    TimeSpan diff = DateTime.Now.Subtract(stampDataTime);
    //    long r = Convert.ToInt64(diff.TotalMilliseconds);
    //    return r;
    //}

    ///// <summary>
    ///// 和当前时间的差值 字符串 X年/月/天/小时/分钟
    ///// </summary>
    //public static string GetStringDiffToNow(long timeStamp)
    //{
    //    long second = GetDiffToNow(timeStamp);
    //    return GetStringDiff(second);
    //}

    /// <summary>
    /// 获取startTime周的周一的零点时间戳
    /// </summary>
    public static long GetCurrWeekOfDay(long startTime , int day)
    {
        long startTimeTick = startTime * 10000 + TimeUtils.timeStampStartTicks;
        DateTime dt = new DateTime(startTimeTick,DateTimeKind.Utc);
        dt = dt.AddHours(8);// utc 时间转北京时间 +8小时
        DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
        DateTime endWeek = startWeek.AddDays(day-1);  //本周周n
        endWeek = DateTime.Parse(endWeek.ToString("yyyy-MM-dd") + " 00:00:00"); //零点
        
        long tempTime = (endWeek.ToUniversalTime().Ticks - TimeUtils.timeStampStartTicks) / (long)10000;
        if (tempTime > startTime && day <= 1) //如果但当前时间为星期天时，会进入下个星期。进行修正，即周一零点为每周的最小时间
        {
            tempTime -= ONE_WEEK;
        }
        return tempTime;
    }


    // 转换为XX天XX小时
    public static string GetDayHourTime(long second)
    {
        string str = "";
        long hourNum = second / 60 / 60 % 24;
        long dayNum = second / 60 / 60 / 24;
        if (dayNum > 0)
        {
            str += string.Format("{0}天", dayNum);
        }
        if (hourNum > 0)
        {
            if (dayNum <= 0) hourNum += 1; // 当时间不足一天时，小于一个小时按一个小时算
            str += string.Format("{0}小时", hourNum);
        }        
        return str;
    }

    /// <summary>
    /// 差值转换时间字符串 X年/月/天/小时/分钟
    /// </summary>
    public static string GetStringDiff(long second)
    {
        string str = "";
        int minNum = 60 * 1000;
        int hourNum = minNum * 60 * 1000;
        int dayNum = hourNum * 24 * 1000;
        int monthNum = dayNum * 30*1000;
        int yearNum = dayNum * 365*1000;

        if ((second / yearNum) > 0)
        {
            str = (second / yearNum) + "年";
        }
        else if ((second / monthNum) > 0)
        {
            str = (second / monthNum) + "月";
        }
        else if ((second / dayNum) > 0)
        {
            str = (second / dayNum) + "天";
        }
        else if ((second / hourNum) > 0)
        {
            str = (second / hourNum) + "小时";
        }
        else if ((second / minNum) > 0)
        {
            str = (second / minNum) + "分钟";
        }
        else
        {
            str = "1分钟";
        }
        return str;
    }
    /// <summary>
    /// 转换时间字符串 天/小时/分钟
    /// </summary>
    public static string GetStringTime(long second)
    {
        string str = "";
        long minNum = second / 60 % 60;
        long hourNum = second / 60 / 60 % 24;
        long dayNum = second / 60 / 60 / 24;
        long secNum = second % 60;
        if (dayNum > 0)
        {
            str += string.Format("{0}天", dayNum);
        }
        if (hourNum > 0)
        {
            str += string.Format("{0}小时", hourNum);
        }
        if (minNum > 0)
        {
            str += string.Format("{0}分", minNum);
        }
        str += string.Format("{0}秒", Math.Max(0, secNum));
        return str;
    }
    /// <summary>
    /// 只显示最大的时间单位
    /// </summary>
    public static string GetStringTime2(long second)
    {
        long minNum = second / 60 % 60;
        long hourNum = second / 60 / 60 % 24;
        long dayNum = second / 60 / 60 / 24;
        long secNum = second % 60;
        if (dayNum > 0)
        {
            return dayNum + "天";
        }
        if (hourNum > 0)
        {
            return hourNum + "小时";
        }
        if (minNum > 0)
        {
            return minNum + "分";
        }
        return Math.Max(0, secNum) + "秒";
    }
    public static void WatchPerformanceUseTime(Action del)
    {
        float startTime = Time.realtimeSinceStartup;
        if (del != null) { del(); }
        TDebug.Log((Time.realtimeSinceStartup - startTime).ToString());
    }

    #endregion

    /// <summary>
    /// 返回curValue是否包含二次幂的enumValue
    /// </summary>
    public static bool IsPowNumHave(int curValue, int enumValue)
    {
        if (enumValue == 0 || !Mathf.IsPowerOfTwo(enumValue))
        {
            return false;
        }
        for (int i = 0; i < 32; i++)
        {
            if (((enumValue >> i) & 1) == 1)
            {
                if (((curValue >> i) & 1) == 1)
                    return true;
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 返回curValue包含的所有二次幂值
    /// </summary>
    public static List<int> GetPowList(int curValue)
    {
        List<int> l = new List<int>();
        int i = 0;
        while (curValue != 0)
        {
            if (((curValue >> i) & 1) == 1)
            {
                curValue -= (1 << i);
                l.Add(1 << i);
            }
            i++;
        }
        return l;
    }

    public static bool IsNewUnlock(int lastLevel, int curLevel, int unlockLevel)
    {
        if (curLevel >= unlockLevel && lastLevel < unlockLevel)
        {
            return true;
        }
        return false;
    }


    public static BinaryReader StringToBinaryReader(string str)
    {
        int i = 0;
        byte[] writeArray = System.Text.Encoding.ASCII.GetBytes(str);
        BinaryWriter binWriter = new BinaryWriter(new MemoryStream());
        BinaryReader binReader = new BinaryReader(binWriter.BaseStream);
        try
        {
            for (i = 0; i < writeArray.Length; i++)
            {
                binWriter.Write(writeArray[i]);
            }
            // Set the stream position to the beginning of the stream.
            binReader.BaseStream.Position = 0;
            return binReader;
        }
        catch
        {
            return null;
        }
    }


    public static string TransStringToLine(string str)
    {
        if (str.Trim().Length <= 0)
        {
            return str;
        }
        string s = Regex.Replace(str, @"[^\u4E00-\u9FA5A-Za-z0-9~!！@#\$%\^&\*\（(\)）《》\-+=\|\\\}\]\{\[:：.。…；;,，\?？\""“”]+", "*", RegexOptions.IgnoreCase).Trim();
        return s;
    }


    public static string SaveScreen(Camera camera, string picName)
    {
        string ssname = picName;
        string sspath = FileBaseUtils.PersistentDataReadPath("", ssname);
        if (File.Exists(sspath))
        {
            File.Delete(sspath);
            TDebug.LogFormat( "{0} deleted first!",ssname);
        }
        Texture2D texture1 = CaptureCamera(camera, new Rect(0, 0, Screen.width * 0.7f, Screen.height * 0.7f));
        byte[] bytes = texture1.EncodeToPNG();
        File.WriteAllBytes(sspath, bytes);

        TDebug.LogFormat("Screenshot saved: {0}" ,sspath);
        return sspath;
    }

    public static Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        RenderTexture originRT = camera.targetTexture;      // 临时把camera中的targetTexture替换掉
        camera.targetTexture = rt;
        camera.RenderDontRestore();                         // 手动渲染
        camera.targetTexture = originRT;

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height);
        screenShot.ReadPixels(rect, 0, 0);                  // 读取的是 RenderTexture.active 中的像素
        screenShot.Apply();
        GameObject.Destroy(rt);
        RenderTexture.active = null;
        return screenShot;
    }



    #region 与游戏逻辑相关

    #endregion
}
 