using System;
using System.Windows.Forms;
using UnifyEditor_Setup.CloudAPI;

namespace UnifyEditor_Setup
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
            this.WindowsSelect.SelectedIndex = 0;
        }

        /// <summary>
        ///  IIS安装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.IISBox.AppendText("这不会花费太久，请稍等...\r\n");
            this.IISBox.AppendText("Internet Information Services 安装开始...\r\n");
            this.IISButton.Enabled = false;

            PSExcuteResult result = null;
            if (this.WindowsSelect.SelectedIndex == 0)
            {
                // 2016 IIS Setup
                result = PowerShellHelper.Run2016IISPS();
            }
            else
            {
                // 2012 IIS Setup
                result = PowerShellHelper.Run2012R2IISPS();
            }

            if (result.STATE == "OK" && !result.DATA.Contains("Error"))
            {
                this.IISBox.AppendText(result.DATA);

                DialogResult dr = MessageBox.Show("安装IIS完成，需要重新启动来完成必要操作！", "重启计算机", MessageBoxButtons.OK);
                if (dr == DialogResult.OK)
                {
                    PowerShellHelper.RunPS2RestartPC();
                }
            }
            else
            {
                this.IISBox.AppendText(result.DATA);
                this.IISBox.AppendText("Internet Information Services 安装未完成。\r\n");
            }
            this.IISButton.Enabled = true;
        }

        /// <summary>
        /// OOS安装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OOSButton_Click(object sender, EventArgs e)
        {
            this.OOSBox.AppendText("这不会花费太久，请稍等...\r\n");
            this.OOSButton.Enabled = false;

            // OOS 安装
            this.OOSBox.AppendText("UnifyEditor Office 安装开始...\r\n");
            var result1 = PowerShellHelper.RunOOSPS();
            if (result1.STATE == "OK")
            {
                this.OOSBox.AppendText("UnifyEditor Office 安装结束。\r\n");
            }
            else
            {
                this.OOSBox.AppendText(result1.DATA + "\r\n");
                this.OOSBox.AppendText("UnifyEditor Office 安装未完成。\r\n");
            }
            this.OOSButton.Enabled = true;
        }

        /// <summary>
        /// 新建场
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FarmButton_Click(object sender, EventArgs e)
        {
            string internalBox = this.InternalBox.Text.Trim();
            if (string.IsNullOrEmpty(internalBox))
            {
                MessageBox.Show("请输入内网站点地址！", "输入参数", MessageBoxButtons.OK);
                return;
            }

            string lower = internalBox.ToLower();
            string externalBox = this.ExternalBox.Text.Trim();

            if (lower.StartsWith("https") && string.IsNullOrEmpty(externalBox))
            {
                MessageBox.Show("请输入外网站点地址！", "输入参数", MessageBoxButtons.OK);
                return;
            }

            this.FarmBox.AppendText("这不会花费太久，请稍等...\r\n");
            this.FarmButton.Enabled = false;

            // 创建场
            this.FarmBox.AppendText("UnifyEditor Office 创建场开始...\r\n");
            var result2 = PowerShellHelper.RunCreateFarmPS(internalBox, externalBox);

            if (result2.STATE == "OK")
            {
                this.FarmBox.AppendText("UnifyEditor Office 创建场结束。\r\n");
            }
            else
            {
                this.FarmBox.AppendText(result2.DATA + "\r\n");
                this.FarmBox.AppendText("UnifyEditor Office 创建场未完成。\r\n");
            }
            this.FarmButton.Enabled = true;
        }

        /// <summary>
        /// 添加到场
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddToButton_Click(object sender, EventArgs e)
        {
            string pcName = this.PCNameBox.Text;
            if (string.IsNullOrEmpty(pcName))
            {
                MessageBox.Show("请输入主服务器FQDN！", "输入参数", MessageBoxButtons.OK);
            }
            else
            {
                this.AddToTextBox.AppendText("这不会花费太久，请稍等...\r\n");
                this.AddToButton.Enabled = false;

                // 创建场
                this.AddToTextBox.AppendText("UnifyEditor Office 添加到场开始...\r\n");
                var result2 = PowerShellHelper.RunAddToFarmPS(pcName);

                if (result2.STATE == "OK")
                {
                    this.AddToTextBox.AppendText("UnifyEditor Office 添加到场结束。\r\n");
                }
                else
                {
                    this.AddToTextBox.AppendText(result2.DATA + "\r\n");
                    this.AddToTextBox.AppendText("UnifyEditor Office 添加到场未完成。\r\n");
                }
                this.AddToButton.Enabled = true;
            }
        }

    }
}
