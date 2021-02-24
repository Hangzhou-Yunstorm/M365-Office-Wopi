using System;
using System.Windows.Forms;
using UnifyEditor_Setup.CloudAPI;

namespace UnifyEditor_Interface
{
    public partial class UnifyEditorInterface : Form
    {
        public UnifyEditorInterface()
        {
            InitializeComponent();
            this.IISNameBox.Text = "UnifyEditor";
            this.IISPortBox.Text = "808";
        }

        private void IISWopiButton_Click(object sender, EventArgs e)
        {
            var siteName = this.IISNameBox.Text;
            var sitePort = this.IISPortBox.Text;
            if (string.IsNullOrEmpty(siteName))
            {
                MessageBox.Show("请输入站点名称！", "输入参数", MessageBoxButtons.OK);
                return;
            }
            int iisPort = 8080;
            if (string.IsNullOrEmpty(siteName))
            {
                MessageBox.Show("请输入站点端口！", "输入参数", MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (!int.TryParse(sitePort, out iisPort) || iisPort < 1 || iisPort > 65535)
                {
                    MessageBox.Show("服务器端口号必须是介于1到65535之间的正整数", "输入合法参数", MessageBoxButtons.OK);
                    return;
                }
            }
            this.IISWopiButton.Enabled = false;
            this.IISWopiBox.AppendText("这不会花费太久，请稍等...\r\n");
            this.IISWopiBox.AppendText("创建UnifyEditor Interface开始...\r\n");
            var result3 = PowerShellHelper.RunCreateIISPS(siteName, iisPort);
            if (result3.STATE != "OK")
            {
                this.IISWopiBox.AppendText(result3.DATA + "\r\n");
            }
            this.IISWopiBox.AppendText("创建UnifyEditor Interface结束。\r\n");
            this.IISWopiButton.Enabled = true;

        }
    }
}
