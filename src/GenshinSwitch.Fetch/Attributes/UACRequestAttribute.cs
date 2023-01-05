namespace GenshinSwitch.Fetch.Attributes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class UACRequestAttribute : Attribute
{
    public bool ButMaybe { get; set; } = false;
}
