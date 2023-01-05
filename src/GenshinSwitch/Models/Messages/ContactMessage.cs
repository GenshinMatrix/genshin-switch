namespace GenshinSwitch.Models.Messages;

internal class ContactMessage
{
    internal enum ContactMessageType
    {
        Added,
        Removed,
        Edited,
    }

    public ContactMessageType Type { get; set; } = ContactMessageType.Added;
    public Contact Contact { get; set; } = null!;
}
