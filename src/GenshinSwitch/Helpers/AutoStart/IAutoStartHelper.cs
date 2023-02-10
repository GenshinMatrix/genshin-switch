namespace GenshinSwitch.Helpers;

internal interface IAutoStartHelper
{
    public void Enable();
    public bool IsEnabled();
    public void Disable();
    public void SetEnabled(bool enable);
}
