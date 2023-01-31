using GenshinSwitch.ViewModels.Contacts;
using System.ComponentModel.DataAnnotations.Schema;
using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

public partial class Contact
{
    [YamlIgnore]
    [NotMapped]
    public ContactViewModel ViewModel { get; set; }

    public Contact()
    {
        ViewModel = new(this);
    }
}
