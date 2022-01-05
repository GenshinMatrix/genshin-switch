using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using YamlDotNet.Serialization;

namespace GenshinSwitch
{
    public class Config
    {
        [YamlIgnore]
        public static Config Instance { get; private set; } = ConfigCtrl.Init();

        [YamlMember(Alias = "install_path")]
        public string InstallPath { get; set; } = string.Empty;

        [YamlMember(Alias = "show_tooltip")]
        public bool ShowTooltip { get; set; } = true;

        [YamlMember(Alias = "accounts")]
        public List<Account> Accounts { get; set; } = new List<Account>();

        public Config()
        {
        }

        public void Init()
        {
            if (string.IsNullOrWhiteSpace(InstallPath))
            {
                InstallPath = SwitchCtrl.FindInstallPath();
            }
        }
    }

    public class Account
    {
        [YamlIgnore]
        public const string ProdKey = "MIHOYOSDK_ADL_PROD_CN_h3123967166";

        [YamlIgnore]
        public const string DataKey = "GENERAL_DATA_h2389025596";

        [YamlMember(Alias = "name")]
        public string Name { get; set; } = string.Empty;

        [Description(ProdKey)]
        [YamlMember(Alias = "prod")]
        public string Prod { get; set; } = string.Empty;

        [Description(DataKey)]
        [YamlMember(Alias = "data")]
        public string Data { get; set; } = string.Empty;

        public void ReadReg()
        {
            Prod = GetStringFromRegedit(ProdKey);
            //Data = GetStringFromRegedit(DataKey);
        }

        public void WriteReg()
        {
            if (!string.IsNullOrWhiteSpace(Prod))
            {
                SetStringToRegedit(ProdKey, Prod);
            }
            if (!string.IsNullOrWhiteSpace(Data))
            {
                SetStringToRegedit(DataKey, Data);
            }
        }

        private static string GetStringFromRegedit(string key)
        {
            object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\miHoYo\原神", key, "");
            return Encoding.UTF8.GetString((byte[])value);
        }

        private static void SetStringToRegedit(string key, string value)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\miHoYo\原神", key, Encoding.UTF8.GetBytes(value));
        }
    }
}
