using RosMessageTypes.Std;

public static class HeaderExtensions
{
    private static Timer timer = new Timer();

    public static void Update(this HeaderMsg header)
    {
        header.seq++;
        header.stamp = timer.Now();
    }
}