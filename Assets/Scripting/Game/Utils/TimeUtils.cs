using System;

public class TimeUtils
{
    public static long CurrentTimeMillis
    {
        get
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);
            return ((DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000);
        }
    }
}
