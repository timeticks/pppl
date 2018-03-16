using System;

public class TimeUtils
{
    /**
     * UTC时间与北京时间差
     */
    public const long TIME_UTC_OFFSET_TICKS = 288000000000;

    public static DateTime timeStampStart = new DateTime(1970, 1, 1, 0,0,0,DateTimeKind.Utc);
    public static long timeStampStartTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
    public static long localTimeStampStartTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).Ticks;

    public static long timeStampStartMillis
    {
        get { return timeStampStartTicks/10000; }
    }
    /// <summary>
    /// 当前本地时间戳（utc）
    /// </summary>
    public static long CurrentTimeMillis
    {
        get
        {
            return ((DateTime.UtcNow.Ticks - timeStampStartTicks) / (long)10000);
        }
    }

    /// <summary>
    /// 设备本地时间与北京时间的时间差，毫秒
    /// </summary>
    public static long LocalWithBeiJingOffset
    {
        get
        {
            long localTicks = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime().Ticks;
            return ((localTicks - timeStampStartTicks - TIME_UTC_OFFSET_TICKS) / 10000);
        }
    }
}
