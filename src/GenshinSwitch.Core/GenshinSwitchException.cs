namespace GenshinSwitch.Core;

public class GenshinSwitchException : Exception
{
    public GenshinSwitchException()
    {
    }

    public GenshinSwitchException(string? message) : base(message)
    {
    }

    public GenshinSwitchException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public GenshinSwitchException(Exception? innerException) : base(null!, innerException)
    {
    }
}
