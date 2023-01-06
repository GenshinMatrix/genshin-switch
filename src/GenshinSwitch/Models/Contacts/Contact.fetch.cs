using Xunkong.Hoyolab.Account;
using Xunkong.Hoyolab.SpiralAbyss;
using Xunkong.Hoyolab.TravelNotes;
using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1404:Code analysis suppression should have justification", Justification = "<Pending>")]
public partial class Contact
{
	[YamlIgnore] public GenshinRoleInfo? _Role { get; set; }
	[YamlIgnore] public SignInInfo? _SignInInfo { get; set; }
	[YamlIgnore]public TravelNotesSummary? _TravelNotesSummary { get; set; }
	[YamlIgnore] public string? _SpiralAbyss { get; set; }
	[YamlIgnore] public SpiralAbyssInfo? _SpiralAbyssInfo { get; set; }
}
