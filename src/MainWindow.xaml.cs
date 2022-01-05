using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DialogResultF = System.Windows.Forms.DialogResult;
using MessageBoxF = System.Windows.Forms.MessageBox;

namespace GenshinSwitch
{
    public partial class MainWindow : Window
    {
        private bool _closeHandler = true;

        public MainWindow()
        {
            InitializeComponent();

            Config.Instance.Init();
            textBoxInstallPath.Text = Config.Instance.InstallPath;
            foreach (var account in Config.Instance.Accounts)
            {
                var item = new ListBoxItem() { Content = account.Name };
                listBoxAccount.Items.Add(item);
            }

            if (!Config.Instance.ShowTooltip)
            {
                imageMiHoYo.ToolTip = null;
                textBoxInstallPath.ToolTip = null;
                buttonInstallPath.ToolTip = null;
                buttonAdd.ToolTip = null;
                buttonRemove.ToolTip = null;
                buttonYaml.ToolTip = null;
                listBoxAccount.ToolTip = null;
                textBlockVersion.ToolTip = null;
            }

            imageMiHoYo.MouseLeftButtonDown += (s, e) =>
            {
                if(e.ClickCount == 3)
                {
                    Process.Start("https://webstatic.mihoyo.com/app/ys-map-cn");
                }
            };

            textBlockVersion.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    Process.Start("https://github.com/emako/genshin-switch");
                }
            };

            imageMiHoYo.MouseMove += (s, e) =>
            {
                try
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                        DragMove();
                }
                catch
                {
                }
            };

            buttonInstallPath.Click += (s, e) =>
            {
                using var dialog = new FolderBrowserDialog() { SelectedPath = textBoxInstallPath.Text, ShowNewFolderButton = true };

                if (dialog.ShowDialog() == DialogResultF.OK)
                {
                    textBoxInstallPath.Text = dialog.SelectedPath;
                }
            };

            textBoxInstallPath.TextChanged += (s, e) =>
            {
                Config.Instance.InstallPath = textBoxInstallPath.Text;
                ConfigCtrl.Save();
            };

            listBoxAccount.MouseDoubleClick += (s, e) =>
            {
                if (listBoxAccount.SelectedItem != null)
                {
                    int index = listBoxAccount.Items.IndexOf(listBoxAccount.SelectedItem);
                    Task.Run(() => SwitchCtrl.Switch(index));
                }
            };

            buttonAdd.Click += (s, e) =>
            {
                var dialog = new AddWindow()
                {
                    Owner = this,
                };
                if (dialog.ShowDialog() ?? false)
                {
                    var account = new Account();
                    account.Name = dialog.AkaName;
                    account.ReadReg();
                    Config.Instance.Accounts.Add(account);

                    var item = new ListBoxItem() { Content = dialog.AkaName };
                    listBoxAccount.Items.Add(item);

                    ConfigCtrl.Save();
                }
            };

            buttonRemove.Click += (s, e) =>
            {
                if (listBoxAccount.SelectedIndex >= 0)
                {
                    if (MessageBoxF.Show($"确定要删除{(listBoxAccount.SelectedItem as ListBoxItem)?.Content}？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResultF.Yes)
                    {
                        return;
                    }
                    Config.Instance.Accounts.RemoveAt(listBoxAccount.SelectedIndex);
                    listBoxAccount.Items.Remove(listBoxAccount.SelectedItem);
                    ConfigCtrl.Save();
                }
            };

            buttonYaml.Click += (s, e) =>
            {
                Process.Start(ConfigCtrl.ConfigUtil.FileName);
            };

            Closing += (s, e) =>
            {
                if (_closeHandler)
                    e.Cancel = true;
                Hide();
            };

            menuItemSlowStart.Click += (s, e) =>
            {
                if (listBoxAccount.SelectedItem != null)
                {
                    int index = listBoxAccount.Items.IndexOf(listBoxAccount.SelectedItem);
                    Task.Run(() => SwitchCtrl.Switch(index, 180000));
                }
            };
        }

        public void ForceClose()
        {
            _closeHandler = false;
            Close();
        }
    }
}
