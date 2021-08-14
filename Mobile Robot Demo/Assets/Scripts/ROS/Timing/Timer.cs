using System;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;

public class Timer
{
    public static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public virtual TimeMsg Now()
    {
        Now(out uint sec, out uint nanosec);
        return new TimeMsg(sec, nanosec);
    }

    public virtual void Now(TimeMsg stamp)
    {
        uint sec; uint nanosec;
        Now(out sec, out nanosec);
        stamp.sec = sec; stamp.nanosec = nanosec;
    }

    private static void Now(out uint sec, out uint nanosec)
    {
        TimeSpan timeSpan = DateTime.Now.ToUniversalTime() - UNIX_EPOCH;
        double msecs = timeSpan.TotalMilliseconds;
        sec = (uint)(msecs / 1000);
        nanosec = (uint)((msecs / 1000 - sec) * 1e+9);
    }
}