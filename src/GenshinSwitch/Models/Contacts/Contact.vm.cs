using GenshinSwitch.ViewModels.Contacts;
using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

public partial class Contact
{
    [YamlIgnore]
    public ContactViewModel ViewModel { get; set; }

    public Contact()
    {
        ViewModel = new(this);
    }
}
