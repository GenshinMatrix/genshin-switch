using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GenshinSwitch
{
    public partial class AddWindow : Window
    {
        public string AkaName
        {
            get => textBoxName.Text;
            set => textBoxName.Text = value;
        }

        public AddWindow()
        {
            InitializeComponent();

            buttonOK.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(AkaName))
                {
                    if (Config.Instance.Accounts.Any(t => t.Name == AkaName))
                    {
                        MessageBox.Show("输入的账号名已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("请输入账号名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            buttonCancel.Click += (s, e) =>
            {
                Close();
            };
        }
    }
}
