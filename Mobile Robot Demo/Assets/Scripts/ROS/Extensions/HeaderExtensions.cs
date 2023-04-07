using RosMessageTypes.Std;

/// <summary>
///     Extend header with a Update() function 
///     for easier frame update
/// </summary>
public static class HeaderExtensions
{
    private static Timer timer = new Timer();

    public static void Update(this HeaderMsg header)
    {
        header.seq++;
        header.stamp = timer.Now();
    }
}